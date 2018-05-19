using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Eventos.Lib.ControlEvents;
using System.Threading;

namespace Gradual.MDS.Core
{
    public class ChannelUDMF
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool startup;
        public bool isRunning;
        public bool isPaused;
        public bool shouldMonitor = false;
        public ChannelUMDFState channelState;
        public ChannelUMDFConfig channelConfig;

        private SecurityListProcessor secListProc;
        private MarketRecoveryProcessor mktRecvProc;
        private MarketIncrementalProcessor mktIncrProc;
        private MonitorConfig monitorConfig;
        private Thread _me = null;
        private int lastIncMsgProcessed = 0;

        public ChannelUDMF(ChannelUMDFConfig config, MonitorConfig monitorConfig)
        {
            channelState = new ChannelUMDFState();
            channelConfig = config;
            this.monitorConfig = monitorConfig;
        }

        public void Start()
        {
            isRunning = true;
            isPaused = false;
            monitorConfig.channels[channelConfig.ChannelID].status = isRunning;
            monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Canal " + channelConfig.ChannelID + " ativado";
            monitorConfig.channels[channelConfig.ChannelID].AddDetails(
                channelConfig.ChannelID, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "Canal ativado");

            iniciaCanal();

            _me = new Thread(new ThreadStart(monitoraCanal));
            _me.Start();
        }

        public void ResumeIncremental()
        {
            mktIncrProc.ResumeIncremental();
            isPaused = false;
        }

        private void OnSecurityListCompleted(object sender, Eventos.Lib.ControlEvents.SecurityListCompletedEventArgs args)
        {
            mktRecvProc.Start();
            mktIncrProc.Start();
            Thread.Sleep(100);
            shouldMonitor = true;
        }

        private void OnRecoveryStarted(object sender, RecoveryStartedEventArgs args)
        {
            mktIncrProc.DiscardPackets(args.LastMsgSeqNumProcessed);
        }

        private void OnRecoveryCompleted(object sender, RecoveryCompletedEventArgs args)
        {
            mktIncrProc.StartPacketProcessing(args.LastMsgSeqNumProcessed);
        }

        public void Stop()
        {
            isRunning = false;
            isPaused = true;
            monitorConfig.channels[channelConfig.ChannelID].status = isRunning;
            monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Canal " + channelConfig.ChannelID + " desativado";
            monitorConfig.channels[channelConfig.ChannelID].configDetails = new Dictionary<string, MonitorConfigDetails>();

            finalizaCanal();
        }

        public void PauseIncremental()
        {
            mktIncrProc.PauseIncremental();
            isPaused = true;
        }

        public void DoD330()
        {
            int lastmsg = mktIncrProc.PauseIncremental();

            mktIncrProc.SolicitacaoReplay(lastmsg - 520, lastmsg - 500);

            isPaused = true;
        }

        public void RecoveryInterval(int seqNumIni, int seqNumFim)
        {
            int lastmsg = mktIncrProc.PauseIncremental();

            mktIncrProc.SolicitacaoReplay(seqNumIni, seqNumFim);

            isPaused = true;
        }


        private void iniciaCanal()
        {
            if (!channelConfig.IsNewsChannel)
            {
                secListProc = new SecurityListProcessor(channelState, channelConfig, monitorConfig);
                channelState.SecListProc = secListProc;
                secListProc.OnSecurityListCompleted += new SecurityListProcessor.OnSecurityListCompletedHandler(OnSecurityListCompleted);

                mktRecvProc = new MarketRecoveryProcessor(channelState, channelConfig, monitorConfig);
                mktRecvProc.OnRecoveryStarted += new MarketRecoveryProcessor.OnRecoveryStartedHandler(OnRecoveryStarted);
                mktRecvProc.OnRecoveryCompleted += new MarketRecoveryProcessor.OnRecoveryCompletedHandler(OnRecoveryCompleted);
                channelState.MktRecvProc = mktRecvProc;

                mktIncrProc = new MarketIncrementalProcessor(channelState, channelConfig, monitorConfig);
                channelState.MktIncrProc = mktIncrProc;

                secListProc.Start();
            }
            else
            {
                mktIncrProc = new MarketIncrementalProcessor(channelState, channelConfig, monitorConfig);
                channelState.MktIncrProc = mktIncrProc;

                mktIncrProc.Start();

                Thread.Sleep(200);

                mktIncrProc.StartPacketProcessing(0);
            }

        }

        private void finalizaCanal()
        {
            shouldMonitor = false;

            if (channelConfig.IsNewsChannel == false)
            {
                if ( secListProc != null )
                    secListProc.Stop();

                if (mktRecvProc != null)
                    mktRecvProc.Stop();
            }

            if ( mktIncrProc != null )
                mktIncrProc.Stop();

            if (channelConfig.IsNewsChannel == false)
            {
                if (secListProc != null)
                    secListProc.WaitStop();

                if (mktRecvProc != null)
                    mktRecvProc.WaitStop();
            }
            
            if (mktIncrProc != null) 
                mktIncrProc.WaitStop();
        }


        private void monitoraCanal()
        {
            logger.Info("Iniciando monitoracao do canal incremental [" + channelConfig.ChannelID + "]");
            long lastWatchDog = DateTime.UtcNow.Ticks;

            while (isRunning)
            {
                try
                {
                    if (mktIncrProc != null && !mktIncrProc.IsAlive() && shouldMonitor)
                    {
                        string subject = "REINICIANDO CANAL [" + channelConfig.ChannelID + "]";
                        string msg = subject + "\r\n" + "reiniciando as " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                        logger.Warn(subject);

                        if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
                        {
                            if (DateTime.Now.Hour >= ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_INICIO &&
                                DateTime.Now.Hour < ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_FIM)
                            {
                                MDSUtils.EnviarEmail(subject, msg);
                            }
                        }

                        finalizaCanal();
                        Thread.Sleep(10000);

                        iniciaCanal();
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - lastWatchDog);
                        if (ts.TotalMilliseconds > 30000)
                        {
                            logger.Info( "Canal [" + channelConfig.ChannelID + "] ativo");
                            lastWatchDog = DateTime.UtcNow.Ticks;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("monitoraCanal(): " + ex.Message, ex);
                }

                Thread.Sleep(250);
            }
        }

    }
}

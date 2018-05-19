using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using log4net;
using System.Configuration;
using Gradual.OMS.Library;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Gradual.Spider.ServicoSupervisor.Cotacao
{
    public class MDSNegocioEventArgs : EventArgs
    {
        public string Instrumento { get; set; }
        public DateTime DtNegocio { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public decimal VlrUltima { get; set; }
        public decimal VlrOscilacao { get; set; }
        public decimal VlrFechamento { get; set; }
    }

    public delegate void OnMDSNegocioHandler(object sender, MDSNegocioEventArgs args );



    public class CotacaoManager
    {
        private struct SinalStruct
        {
            public string Instrumento { get; set; }
            public string Mensagem { get; set; }
        }

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static CotacaoManager _me = null;
        private Thread _thProc = null;
        private bool _bKeepRunning;
        private ConcurrentQueue<SinalStruct> queueSinal = new ConcurrentQueue<SinalStruct>();
        private MDSPackageSocket mdsSocket = null;
        private long lastSonda = 0;

        public event OnMDSNegocioHandler OnNegocio;

        public static CotacaoManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new CotacaoManager();
                }
                return _me;
            }
        }

        public CotacaoManager()
        {
        }

        public void Start()
        {
            logger.Info("Iniciando processador da fila de sinais");

            _bKeepRunning = true;

            mdsSocket = new MDSPackageSocket();
            mdsSocket.IpAddr = ConfigurationManager.AppSettings["MDSHost"].ToString();
            mdsSocket.Port = ConfigurationManager.AppSettings["MDSPort"].ToString();

            _thProc = new Thread(new ThreadStart(procSinal));
            _thProc.Name = "procSinal";
            _thProc.Start();

        }

        public void Stop()
        {
            logger.Info("Finalizando processador da fila de sinais");
            _bKeepRunning = false;

            try
            {
                mdsSocket.CloseConnection();
            }
            catch (Exception ex)
            {
            }

            while (_thProc != null && _thProc.IsAlive)
            {
                Thread.Sleep(100);
            }

            logger.Info("Processador da fila de sinais finalizado");
        }

        public void EnqueueSinal(string instrumento, string mensagem)
        {
            try
            {

                SinalStruct sinal = new SinalStruct();
                sinal.Instrumento = instrumento;
                sinal.Mensagem = mensagem;

                queueSinal.Enqueue(sinal);
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueSinal: " + ex.Message, ex);
            }

        }


        private void procSinal()
        {
            long lastLogTicks = 0;
            long lastTrial = 0;

            while (_bKeepRunning)
            {
                try
                {
                    if (!mdsSocket.IsConectado() && _bKeepRunning)
                    {
                        TimeSpan tsmds = new TimeSpan(DateTime.Now.Ticks - lastTrial);
                        if ( tsmds.TotalMinutes > 1 )
                        {
                            logger.Info("Abrindo conexao com MDS [" + mdsSocket.IpAddr + ":" + mdsSocket.Port + "]");
                            mdsSocket.OpenConnection();
                            lastSonda = lastTrial= DateTime.Now.Ticks;
                            _sendMDSLoginMSG();
                            Thread.Sleep(10000);
                        }
                    }

                    SinalStruct sinal;

                    if (queueSinal.TryDequeue(out sinal))
                    {
                        MessageBroker(sinal.Instrumento, sinal.Mensagem);

                        if (shouldLog(lastLogTicks))
                        {
                            logger.Debug("Fila de sinais a processar: " + queueSinal.Count);
                            lastLogTicks = DateTime.Now.Ticks;
                        }
                        continue;
                    }

                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastSonda);
                    if ( ts.TotalSeconds > 60 )
                    {
                        logger.Error("Timeout de sonda, reconectando");
                        mdsSocket.CloseConnection();
                    }
                    
                    Thread.Sleep(100);
                }
                catch(Exception ex )
                {
                    logger.Error("procSinal:" + ex.Message, ex);
                }
            }
        }

        protected void _sendMDSLoginMSG()
        {
            string msg = "QLPP";

            try
            {

                //if (ConfigurationManager.AppSettings["EfetuarLogonMDS"] == null)
                //{
                //    logger.Warn("Chave 'EfetuarLogonMDS' nao declarada no appsettings. Nao efetua login");
                //    return;
                //}

                //if (!ConfigurationManager.AppSettings["EfetuarLogonMDS"].ToString().Equals("true"))
                //{
                //    logger.Warn("Nao efetua login no MDS, EfetuarLogonMDS=false.");
                //    return;
                //}

                msg += DateTime.Now.ToString("yyyyMMddHHmmssfff");
                msg += System.Environment.MachineName.PadRight(20);

                logger.Info("Efetuando login no MDS [" + msg + "]");

                if (mdsSocket != null && mdsSocket.IsConectado())
                {
                    mdsSocket.SendData(msg, true);
                }

                logger.Info("Mensagem de login enviada ao MDS");
            }
            catch (Exception ex)
            {
                logger.Info("_sendMDSLoginMSG():" + ex.Message, ex);
            }
        }


        public static bool shouldLog(long lastEventTicks)
        {
            if ((DateTime.Now.Ticks - lastEventTicks) > TimeSpan.TicksPerSecond)
                return true;

            return false;
        }

        private void MessageBroker(string Instrumento, string Mensagem)
        {
            try
            {
                if (ConfigurationManager.AppSettings["LogarMsgMDS"] != null &&
                    ConfigurationManager.AppSettings["LogarMsgMDS"].ToString().ToLowerInvariant().Equals("true"))
                {
                    logger.Debug("[" + Instrumento + "] [" + Mensagem + "]");
                }

                switch (Mensagem.ToString().Substring(0, 2))
                {

                    case ConstantesMDS.Negocio:
                        FireNegocioEvent(Instrumento, Mensagem);
                        break;
                    case ConstantesMDS.Sonda:
                        logger.Info("Recebeu Sonda: [" + Mensagem + "]");
                        ConfigurationManager.RefreshSection("appSettings");
                        lastSonda = DateTime.Now.Ticks;
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("MessageBroker(): " + ex.Message, ex);

                if (!String.IsNullOrEmpty(Instrumento))
                    logger.Error("Instrumento [" + Instrumento + "]");

                if (!String.IsNullOrEmpty(Mensagem))
                    logger.Error("Mensagem [" + Mensagem + "]");
            }
        }


        private void FireNegocioEvent(string instrumento, string mensagem)
        {
            try
            {

                NE_Negocio negocio = Utilities.MarshalFromStringBlock<NE_Negocio>(mensagem);

                MDSNegocioEventArgs args = new MDSNegocioEventArgs();

                args.Instrumento = instrumento;
                args.VlrOscilacao = negocio.Variacao.ByteArrayToDecimal(2);
                args.VlrUltima = negocio.UltimoPreco.ByteArrayToDecimal(3);
                args.DtAtualizacao = DateTime.ParseExact(negocio.DataHora.ByteArrayToString(), "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

                string dataNegocio = negocio.DateTimeBvsp.ByteArrayToString();
                if ( !dataNegocio.Substring(0,4).Equals("0000") )
                    args.DtNegocio = DateTime.ParseExact(negocio.DateTimeBvsp.ByteArrayToString(), "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

                if (OnNegocio != null && args.VlrUltima > Decimal.Zero)
                {
                    OnNegocio(this, args);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FireNegocioEvent: " + ex.Message, ex);
            }
        }


    }
}

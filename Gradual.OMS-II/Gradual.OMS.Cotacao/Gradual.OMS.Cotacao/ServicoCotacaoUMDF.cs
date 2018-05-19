using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using System.Configuration;
using System.Threading;

namespace Gradual.OMS.Cotacao
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoCotacaoUMDF : ServicoCotacao
    {
        private MDSPackageSocket[] umdfSockets;
        private UMDFConfig _umdfconfig = null;


        #region IServicoControlavel Members

        public override void IniciarServico()
        {
            logger.Info("Iniciando servico Cotacao com UMDF");

            //composicaoIndice = new ComposicaoIndice();

            _umdfconfig = Gradual.OMS.Library.GerenciadorConfig.ReceberConfig<UMDFConfig>();

            // Verifica se deve ser ativado o tratamento de delay da cotação
            MemoriaCotacaoDelay.GetInstance().DelayTickerOn = false;
            if (ConfigurationManager.AppSettings["DelayTicker"] != null &&
                ConfigurationManager.AppSettings["DelayTicker"].ToString().ToUpper().Equals("TRUE"))
            {
                MemoriaCotacaoDelay.GetInstance().DelayTickerOn = true;

                // Obtem o valor do Delay do sinal de cotação, em minutos
                MemoriaCotacaoDelay.GetInstance().DelayTickerAmount = 15 * 60 * 1000;
                if (ConfigurationManager.AppSettings["DelayTickerAmount"] != null)
                {
                    int delayAmount = Convert.ToInt32(ConfigurationManager.AppSettings["DelayTickerAmount"].ToString());
                    MemoriaCotacaoDelay.GetInstance().DelayTickerAmount = delayAmount * 60 * 1000;
                }

                MemoriaCotacaoDelay.GetInstance().StartProcessamento();
            }

            if (ConfigurationManager.AppSettings["FiltraIndiceCheio"] != null &&
                ConfigurationManager.AppSettings["FiltraIndiceCheio"].ToString().ToUpper().Equals("TRUE"))
            {
                _filtraIndiceCheio = true;
            }

            if (_umdfconfig == null)
            {
                logger.Fatal("Erro ao carregar configuracoes do UMDF");

                return;
            }

            umdfSockets = new MDSPackageSocket[_umdfconfig.Portas.Count];
            int i=0;
            foreach (string host in _umdfconfig.Portas)
            {
                string[] hostporta = host.Split(':');
                umdfSockets[i] = new MDSPackageSocket();
                umdfSockets[i].FiltraIndiceCheio = _filtraIndiceCheio;
                umdfSockets[i].IpAddr = hostporta[0];
                umdfSockets[i].Port = hostporta[1];
                //umdfSockets[i].setComposicaoIndice(composicaoIndice);
                umdfSockets[i].OpenConnection();

                _sendMDSLoginMSG(umdfSockets[i]);

                i++;
            }


            thrMonitorConexao = new Thread(new ThreadStart(MonitorConexaoMDS));
            thrMonitorConexao.Start();

            gTimer = new Timer(new TimerCallback(IniciarThread), null, 0, 5000);


            // Obtem o parametro de maxima diferenca de horario da ultima mensagem com a bolsa
            // para envio de alertas
            MaxDifHorarioBolsa = 75;
            if (ConfigurationManager.AppSettings["MaxDifHorarioBolsa"] != null)
            {
                MaxDifHorarioBolsa = Convert.ToDouble(ConfigurationManager.AppSettings["MaxDifHorarioBolsa"].ToString());
            }
            MaxDifHorarioBolsa *= 1000;

            // Obtem o timeout de mensagem com o MDS, em segundos
            TimeoutMDS = 300;
            if (ConfigurationManager.AppSettings["TimeoutMDS"] != null)
            {
                TimeoutMDS = Convert.ToDouble(ConfigurationManager.AppSettings["TimeoutMDS"].ToString());
            }
            TimeoutMDS *= 1000;

            logger.Info("Servico cotacao iniciado");

            this.Status = ServicoStatus.EmExecucao;
        }

        public override void PararServico()
        {
            logger.Info("Finalizando servico");

            bKeepRunning = false;

            MemoriaCotacaoDelay.GetInstance().StopProcessamento();

            if (thrMonitorConexao != null)
            {
                while (thrMonitorConexao.IsAlive)
                {
                    logger.Info("Aguardando finalizacao do monitor de conexao com MDS");
                    Thread.Sleep(250);
                }
            }

            logger.Info("Servico cotacao finalizado");

            this.Status = ServicoStatus.Parado;
        }

        public override ServicoStatus ReceberStatusServico()
        {
            return this.Status;
        }

        #endregion //IServicoControlavel Members

        #region Overrides
        protected override void MonitorConexaoMDS()
        {
            int i = 0;
            int iTrialInterval = 600;

            logger.Info("Iniciando thread de monitoracao de conexao com MDS");
            while (bKeepRunning)
            {
                // Reconecta a cada 5 min
                foreach (MDSPackageSocket umdfSocket in this.umdfSockets)
                {
                    if (!umdfSocket.IsConectado())
                    {
                        if (i > iTrialInterval)
                        {
                            _enviaAlertaDesconexao(umdfSocket.IpAddr, umdfSocket.Port);

                            logger.Info("Reabrindo conexao com MDS...");
                            umdfSocket.OpenConnection();

                            _sendMDSLoginMSG(umdfSocket);

                            i = 0;
                        }
                        else
                        {
                            i++;
                            // Configura intervalos de 1 minuto durante o dia ou 
                            // 5 minutos 
                            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 21)
                                iTrialInterval = 600;
                            else
                                iTrialInterval = 3000;

                        }
                    }
                    else
                    {
                        if (i > 600)
                        {
                            //logger.InfoFormat("Conexao com MDS [{0}:{1}] ativa {2}  LastPkt: " + umdfSocket.LastPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " [" + umdfSocket.LastMsg + "]", umdfSocket.IpAddr, umdfSocket.Port, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            //logger.Info("*** Ultimos pacotes: ");
                            //logger.Info("Neg =[" + umdfSocket.LastNegocioPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + umdfSocket.LastNegocioMsg + "]");
                            //logger.Info("Lof =[" + umdfSocket.LastLofPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + umdfSocket.LastLofMsg + "]");
                            //logger.Info("Dest=[" + umdfSocket.LastDestaquePacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + umdfSocket.LastDestaqueMsg + "]");
                            //logger.Info("Rank=[" + umdfSocket.LastRankingPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + umdfSocket.LastRankingMsg + "]");
                            //logger.Info("***");

                            // Verifica dessincronizacao de sinal 
                            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday &&
                                DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                            {
                                if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 20)
                                {
                                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - MemoriaCotacao.HorarioUltimaSonda.Ticks);
                                    if (ts.TotalMilliseconds > MaxDifHorarioBolsa)
                                    {
                                        _enviaAlertaAtraso(umdfSocket.IpAddr, umdfSocket.Port);
                                    }
                                }
                            }

                            // Verifica ultima comunicacao com MDS
                            TimeSpan tslastpkt = new TimeSpan(DateTime.Now.Ticks - umdfSocket.LastPacket.Ticks);
                            if (tslastpkt.TotalMilliseconds > TimeoutMDS)
                            {
                                logger.WarnFormat("Finalizando conexao com MDS [{0}:{1}] por timeout!!!", umdfSocket.IpAddr, umdfSocket.Port);
                                umdfSocket.CloseConnection();
                            }

                            i = 0;
                        }
                        else
                            i++;
                    }
                }

                Thread.Sleep(100);
            }

            logger.Info("Thread de monitoracao de conexao com MDS finalizacao");
        }

        #endregion //Overrides


    }
}

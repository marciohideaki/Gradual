using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.ServiceModel;
using log4net;
using Gradual.OMS.SpreadMonitor.Lib;
using Gradual.OMS.SpreadMonitor.Lib.Mensagens;
using System.Configuration;
using System.Threading;
using Gradual.OMS.Library;
using Gradual.OMS.SpreadMonitor.Lib.Dados;

namespace Gradual.OMS.SpreadMonitor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoSpreadMonitor : IServicoControlavel, IServicoSpreadMonitor
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private bool _bKeepRunning = false;
        private SocketPackage _sockServer;
        private MDSPackageSocket lSocket = null;
        private Thread thMonitorMds = null;
        protected double TimeoutMDS;

        #region IServicoControlavel
        public void IniciarServico()
        {
            _bKeepRunning = true;
            logger.Info("Iniciando servico SpreadMonitor");

            //Obtendo configuracoes 
            // Obtem o timeout de mensagem com o MDS, em segundos
            TimeoutMDS = 300;
            if (ConfigurationManager.AppSettings["TimeoutMDS"] != null)
            {
                TimeoutMDS = Convert.ToDouble(ConfigurationManager.AppSettings["TimeoutMDS"].ToString());
            }
            TimeoutMDS *= 1000;

            // Inicia o gerenciador das threads
            ThreadPoolManager.Instance.Start();

            // Carregar algos do DB
            PersistenciaDB db = new PersistenciaDB();
            List<AlgoStruct> algos = db.CarregarAlgoritmos();
            foreach (AlgoStruct algo in algos)
            {
                ThreadPoolManager.Instance.AddAlgoritmo(algo);
            }

            // Conectar ao MDS
            thMonitorMds = new Thread(new ThreadStart(this.MonitorConexaoMDS));
            thMonitorMds.Start();

            // Aguardar conexoes do streamer
            int streamerPort = 6565;
            if (ConfigurationManager.AppSettings["StreamerListenPort"] != null)
            {
                streamerPort = Convert.ToInt32(ConfigurationManager.AppSettings["StreamerListenPort"].ToString());
            }

            //
            _sockServer = new SocketPackage();
            _sockServer.StartListen(streamerPort);

            //_sockServer.
        }

        public void PararServico()
        {
            _bKeepRunning = false;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion //IServicoControlavel

        #region IServicoSpreadMonitor
        public MonitorarCotacoesResponse MonitorarCotacoes(MonitorarCotacoesRequest request)
        {
            MonitorarCotacoesResponse response = new MonitorarCotacoesResponse();

            return response;
        }

        #endregion // IServicoSpreadMonitor

        protected virtual void MonitorConexaoMDS()
        {
            int i = 0;
            int iTrialInterval = 600;

            logger.Info("Iniciando thread de monitoracao de conexao com MDS");
            while (_bKeepRunning)
            {
                try
                {
                    // Reconecta a cada 5 min
                    if (!lSocket.IsConectado())
                    {
                        if (i > iTrialInterval)
                        {
                            _enviaAlertaDesconexao(lSocket.IpAddr, lSocket.Port);

                            logger.Info("Reabrindo conexao com MDS...");
                            lSocket.OpenConnection();

                            _sendMDSLoginMSG(lSocket);

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
                            logger.Info("Conexao com MDS ativa " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [" + lSocket.LastMsg + "]");

                            // Verifica dessincronizacao de sinal 
                            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday &&
                                DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                            {
                                if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 18)
                                {
                                    //TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - MemoriaCotacao.HorarioUltimaSonda.Ticks);
                                    //if (ts.TotalMilliseconds > MaxDifHorarioBolsa)
                                    //{
                                    //    _enviaAlertaAtraso(lSocket.IpAddr, lSocket.Port);
                                    //}
                                }
                            }

                            // Verifica ultima comunicacao com MDS
                            TimeSpan tslastpkt = new TimeSpan(DateTime.Now.Ticks - lSocket.LastPacket.Ticks);
                            if (tslastpkt.TotalMilliseconds > TimeoutMDS)
                            {
                                logger.Warn("Finalizando conexao com MDS por timeout!!!");
                                lSocket.CloseConnection();
                            }

                            i = 0;
                        }
                        else
                            i++;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Fatal("MonitorCotacaoMDS(): " + ex.Message, ex);
                    Thread.Sleep(1000);
                }
            }

            logger.Info("Thread de monitoracao de conexao com MDS finalizacao");
        }

        protected void _sendMDSLoginMSG(MDSPackageSocket mdsSocket)
        {
            string msg = "QLPP";

            try
            {

                if (ConfigurationManager.AppSettings["EfetuarLogonMDS"] == null)
                {
                    logger.Warn("Chave 'EfetuarLogonMDS' nao declarada no appsettings. Nao efetua login");
                    return;
                }

                if (!ConfigurationManager.AppSettings["EfetuarLogonMDS"].ToString().Equals("true"))
                {
                    logger.Warn("Nao efetua login no MDS, EfetuarLogonMDS=false.");
                    return;
                }

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


        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        protected void _enviaAlertaAtraso(string server, string porta)
        {
            try
            {
                string body = "";

                string subject = System.Environment.MachineName + " Alerta: Atraso de sinal";
                body += System.Environment.MachineName + " Alerta: ";
                body += "\r\n";
                body += "Horario do server: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                body += "\r\n";
                body += "MDS: " + server + ":" + porta;
                body += "\r\n";
                //body += "Ultimo sinal recebido: " + MemoriaCotacao.HorarioUltimaSonda.ToString("yyyy/MM/dd HH:mm:ss");

                if (Utilities.EnviarEmail(subject, body))
                {
                    logger.InfoFormat("Email de alerta de atraso enviado com sucesso");
                }
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAlertaAtraso(): " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        protected void _enviaAlertaDesconexao(string server, string porta)
        {
            try
            {
                string body = "";

                string subject = System.Environment.MachineName + " Alerta: Desconectado do MDS";
                body += System.Environment.MachineName + " Alerta: ";
                body += "\r\n";
                body += "MDS: " + server + ":" + porta;
                body += "\r\n";
                body += "Desconectado do MDS: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                Utilities.EnviarEmail(subject, body);
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAlertaDesconexao(): " + ex.Message, ex);
            }

        }
    }
}

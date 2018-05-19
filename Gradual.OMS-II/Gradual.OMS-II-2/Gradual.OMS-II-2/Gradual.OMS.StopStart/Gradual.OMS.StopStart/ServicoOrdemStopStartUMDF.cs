using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Comunicacao.Automacao.Ordens;
using Gradual.OMS.Ordens.Comunicacao.DB;
using Gradual.OMS.Ordens.Comunicacao.Mensagens.Enviadas;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using System.Configuration;

namespace Gradual.OMS.StopStart
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoOrdemStopStartUMDF :  ServicoOrdemStopStart
    {
        private MDSPackageSocket[] umdfSockets;
        private UMDFConfig _umdfconfig = null;
        private string _authmessage = null;


        #region ||IServicoControlavel Members
        /// <summary>
        /// OnStart do Serviço 
        /// </summary>
        public override void IniciarServico()
        {
            bKeepRunning = true;

            _umdfconfig = Gradual.OMS.Library.GerenciadorConfig.ReceberConfig<UMDFConfig>();

            try
            {
                logger.Info("Iniciando ServicoOrdemStopStart");

                PersistenciaAutenticacaoMDS dados = new PersistenciaAutenticacaoMDS();

                dados.ExcluirMDSAuthentication(int.Parse(IdCliente), int.Parse(IdSistema));

                EnviarMensagemAutenticacao();



                //TimerCallback CallBack = ValidarStatusConexaoMDS;

                //WaitOrTimerCallback lCallBackExpiracaoOrdem = new WaitOrTimerCallback(VerificaOrdemParaExpiracao);

                //ThreadPool.RegisterWaitForSingleObject(_autoEventVerificacao, lCallBackExpiracaoOrdem, null, this.TemporizadorIntervaloVerificacao, false);

                //_autoEventVerificacao.Set();

                //if (_StackTimerMDS == null)
                //{
                //    _StackTimerMDS = new Timer(CallBack, _autoEvent,0, 30000);

                //    logger.Info("Ao inicializar o serviço, entrou no ticker do timer == null para chamar com o callback");
                //}

                //ArmaStopStartNaoArmado();

                logger.Info("Serviço inicializado com sucesso");

                _ServicoStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("O Serviço de StopStart não foi inicializado - ERRO: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;
        }

        /// <summary>
        /// Retorna o estado do serviço
        /// </summary>
        /// <returns></returns>
        public override ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region ||ValidaConexaoMDS
        protected override void ValidarStatusConexaoMDS()
        {
            foreach (MDSPackageSocket umdfsocket in umdfSockets)
            {
                try
                {
                    if ((umdfsocket == null) ||
                        (umdfsocket.IsConectado() == false) ||
                        Autenticado == false)
                    {
                        Autenticado = false;
                        if (umdfsocket != null && umdfsocket.IsConectado())
                        {
                            umdfsocket.CloseConnection();
                        }

                        lastPongReceived = DateTime.Now;

                        umdfsocket.OpenConnection();

                        logger.Info(string.Format("Tentativa de reconexao com MDS {0}:{1} às {2}", 
                            umdfsocket.IpAddr,
                            umdfsocket.Port,
                            DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")));

                        if (_authmessage != null)
                        {
                            logger.InfoFormat("Enviando mensagem de autenticacao para [{0}:{1}]", umdfsocket.IpAddr, umdfsocket.Port);
                            umdfsocket.SendData(_authmessage, true);
                        }


                    }
                    else
                    {
                        PG_Ping pg = new PG_Ping();

                        umdfsocket.SendData(pg.GetMessage(), true);

                        TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastPongReceived.Ticks);
                        if (ts.TotalSeconds > 60)
                        {
                            logger.Info("Timeout no hearbeat. Reconectando");
                            umdfsocket.CloseConnection();
                            Autenticado = false;
                        }
                    }

                    logger.Info(string.Format("SocketPrincipal conectado? {0} -  ValidarStatusConexaoMDS - Método de reconexao com MDS às {1}", umdfsocket != null ? umdfsocket.IsConectado().ToString() : "false", DateTime.Now.ToString()));
                }
                catch (Exception ex)
                {
                    logger.Error("ValidarStatusConexaoMDS(): " + ex.Message, ex);
                }
            }
        }
        #endregion 

        #region ||Métodos de Apoio e autenticações
        protected override void OnASAuthenticationResponse(object Response, ASEventArgs e)
        {
            try
            {
                _authmessage = e.Message;

                _authmessage = _authmessage.Replace("A2", "A3");

                logger.Info("Enviando token de autenticacao para o MDS [" + _authmessage + "]");

                umdfSockets = new MDSPackageSocket[_umdfconfig.Portas.Count];

                int i = 0;
                foreach (int porta in _umdfconfig.Portas)
                {
                    try
                    {
                        MDSPackageSocket umdfsocket = new MDSPackageSocket();

                        umdfSockets[i] = umdfsocket;

                        umdfsocket.OnMDSAuthenticationResponse += new MDSPackageSocket.MDSAuthenticationResponseEventHandler(mdssocket_OnMDSAuthenticationResponse);
                        umdfsocket.OnMDSSRespostaCancelamentoEvent += new MDSPackageSocket.MDSSRespostaCancelamentoEventHandler(mdssocket_OnMDSSRespostaCancelamentoEvent);
                        umdfsocket.OnMDSSRespostaSolicitacaoEvent += new MDSPackageSocket.MDSSRespostaSolicitacaoEventHandler(mdssocket_OnMDSSRespostaSolicitacaoEvent);
                        umdfsocket.OnMDSStopStartEvent += new MDSPackageSocket.MDSStopStartEventHandler(mdssocket_OnMDSStopStartEvent);
                        umdfsocket.OnMDSPing += new MDSPackageSocket.MDSPingEventHandler(mdssocket_OnMDSPing);

                        umdfsocket.IpAddr = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();
                        umdfsocket.Port = porta.ToString();
                        //umdfsocket.OpenConnection();

                        logger.InfoFormat("Adicionado conexao para [{0}:{1}]", umdfsocket.IpAddr, umdfsocket.Port);

                        //umdfsocket.SendData(_authmessage, true);

                        i++;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("OnASAuthenticationResponse(): " + ex.Message, ex);
                    }
                }

                FormatadorUMDF.SetMDSPackageSocketArray(umdfSockets);

                thrMonitor = new Thread(new ThreadStart(_monitorMDS));
                thrMonitor.Start();
            }
            catch (Exception ex)
            {
                logger.Error("OnASAuthenticationResponse(1): " + ex.Message, ex);
            }
        }

        #endregion //||Métodos de Apoio e autenticações
    }
}

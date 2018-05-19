using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.Ordens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using System.Threading;
using Gradual.Core.Ordens.Callback;
using Gradual.Core.OrdensMonitoracao.ADM.Lib;
using Gradual.Core.OrdensMonitoracao.ADM.Lib.Mensagens;
using System.ServiceModel;

namespace Gradual.Core.Ordens
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoOrdens : IServicoOrdens , IServicoControlavel, IServicoOrdensMonitoracaoADM
    {
        #region Declaracoes

        /// <summary>
        /// Objeto responsavel por logar as operações realizadas pelas solicitações de Ordens.
        /// </summary>
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        //IRoteadorOrdens ServicoRoteador;

        public ServicoOrdens()
        {
            //log4net.Config.XmlConfigurator.Configure();

             ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
        }

        #region Private Members

        private ServicoStatus _status = ServicoStatus.Parado;
        private bool _bKeepRunning = false;
        private IRoteadorOrdens ServicoRoteador = null;
        private Thread _thrMonitorRoteador = null;
        private NewOrderCallback OrderCallback = null;

        #endregion

        #region IServicoOrdens Members

        /// <summary>
        /// Metodo responsável por efetuar o controle de risco e tratamento da solicitação de envio de ordens.
        /// </summary>
        /// <param name="pParametroOrdemResquest">Informações da Ordem</param>
        /// <returns>EnviarOrdemResponse</returns>
        public EnviarOrdemResponse EnviarOrdem(EnviarOrdemRequest pParametroOrdemResquest)
        {           
            EnviarOrdemResponse _response = new EnviarOrdemResponse();

            try
            {
                logger.Info("***** INICIA O PROCESSAMENTO DE UMA NOVA OFERTA *****");
                _response = new ProcessarOrdem().
                    EnviarOrdem(pParametroOrdemResquest);

                logger.Info("***** FINALIZA O PROCESSAMENTO DA OFERTA ********");

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao invocar o método EnviarOrdem.", ex);
            }

            return _response;
        }

        /// <summary>
        /// Metodo responsavel por zerar a posicao do cliente para um determinado papel.
        /// </summary>
        /// <param name="pParametroOrdemResquest"></param>
        /// <returns></returns>
        public EnviarOrdemResponse ZerarPosicao(EnviarOrdemRequest pParametroOrdemResquest)
        {
            EnviarOrdemResponse _response = new EnviarOrdemResponse();
            

            try
            {
                logger.Info("***** INICIA O PROCESSAMENTO DE UMA NOVA OFERTA *****");
                _response = new ProcessarOrdem().
                    ZerarPosicao(pParametroOrdemResquest);

                logger.Info("***** FINALIZA O PROCESSAMENTO DA OFERTA ********");

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao invocar o método EnviarOrdem.", ex);
            }

            return _response;
        }

        /// <summary>
        ///  Metodo responsável por efetuar o controle de risco e tratamento da solicitação de cancelamento de oferta.
        /// </summary>
        /// <param name="pParametroCancelamentoRequest">Dados do cancelamento da oferta</param>
        /// <returns>EnviarCancelamentoOrdemResponse</returns>
        public ExecutarCancelamentoOrdemResponse CancelarOrdem(EnviarCancelamentoOrdemRequest pParametroCancelamentoRequest)
        {
            ExecutarCancelamentoOrdemResponse _response = new ExecutarCancelamentoOrdemResponse();

            logger.Info("TRACE DA SOLICITACAO DE CANCELAMENTO DE ORDENS:..........:");

            logger.Info("**************************** INICIO **********************************");
            logger.Info("CodigoCliente:......................................:" + DateTime.Now.ToString());
            logger.Info("OrderID:............................................:" + pParametroCancelamentoRequest.ClienteCancelamentoInfo.Account.ToString());            
            logger.Info("OrigClOrdID:........................................:" + pParametroCancelamentoRequest.ClienteCancelamentoInfo.OrigClOrdID.ToString());
            logger.Info("PortaControleOrdem:.................................:" + pParametroCancelamentoRequest.ClienteCancelamentoInfo.ChannelID.ToString());
            logger.Info("**************************** FIM ************************************");

            try{

                logger.Info("******* INICIA O PROCESSAMENTO DO CANCELAMENTO ********");

                _response = new ProcessarOrdem().
                    CancelarOrdem(pParametroCancelamentoRequest);

                logger.Info("******* FINALIZA O PROCESSAMENTO DO CANCELAMENTO ******");
            }
            catch (Exception ex){  
                logger.Error("Ocorreu um erro ao invocar o método CancelarOrdem.", ex);
            }

            return _response;

        }
      


        #region Monitoramento


        /// <summary>
        ///MONITOR DE CONEXOES DO ROTEADOR
        /// </summary>
        private void RunMonitor()
        {
            try
            {
                logger.Info("Iniciando thread de monitoracao do roteador de ordens");
                int _iMonitorConexoes = 0;

                if (ServicoRoteador == null)
                    ServicoRoteador = Ativador.Get<IRoteadorOrdens>();

                while (_bKeepRunning)
                {
                    // 4 * 250 = 1 segundo
                    if (_iMonitorConexoes == 30 * 4)
                    {
                        lock (ServicoRoteador)
                        {
                            try
                            {
                                if (ServicoRoteador == null)
                                    ServicoRoteador = Ativador.Get<IRoteadorOrdens>();
                                ServicoRoteador.Ping(new PingRequest());
                            }
                            catch (Exception ex)
                            {
                                Ativador.AbortChannel(ServicoRoteador);
                                ServicoRoteador = null;
                            }
                        }
                        _iMonitorConexoes = 0;
                    }
                    else
                    {
                        _iMonitorConexoes++;
                    }

                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("RunMonitor(): " + ex.Message, ex);
            }
            
            logger.Info("THREAD DE MONITORAMENTO DO ROTEADOR FINALIZADA");
        }

        #endregion


        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {

                _bKeepRunning = true;
                _thrMonitorRoteador = new Thread(new ThreadStart(RunMonitor));
                _thrMonitorRoteador.Start();

                _status = ServicoStatus.EmExecucao;
                logger.Info("SERVICO INICIALIZADO COM SUCESSO.");

                logger.Info("INICIALIZA CALLBACK");

                OrderCallback = new NewOrderCallback();
                OrderCallback.StartRouterCallBack();
                logger.Info("Servico NEW ORDER CALLBACK iniciado com sucesso.");


                logger.Info("NEW CALLBACK INICIALIZADO .");

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO INICIAR O SERVICO DE ORDENS.",ex);
            }
        }

        public void PararServico()
        {
            _bKeepRunning = false;

            try
            {

                if (_thrMonitorRoteador != null)
                {
                    while (_thrMonitorRoteador.IsAlive)
                    {
                        logger.Info("Aguardando finalizacao da thread de monitoracao do roteador");
                        Thread.Sleep(250);
                    }
                }
                _status = ServicoStatus.Parado;
                logger.Info("SERVICO PARADO COM SUCESSO.");
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO PARAR O SERVICO DE ORDENS.", ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion

        #region IServicoOrdensMonitoracaoADM
        public ServicoOrdensMonitoracaoResponse ObterStatusServicoOrdens(ServicoOrdensMonitoracaoRequest request)
        {
            ServicoOrdensMonitoracaoResponse response = new ServicoOrdensMonitoracaoResponse();

            // So por seguranca, nao devia precisar
            if (OrderCallback != null)
            {
                response.CanaisBolsaAtivos = OrderCallback.CanaisBolsaAtivos();
            }

            return response;
        }
        #endregion //IServicoOrdensMonitoracaoADM
    }
}

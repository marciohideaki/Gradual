using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using System.Threading;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.Spider.Acompanhamento4Socket.Cache;

namespace Gradual.Spider.Acompanhamento4Socket.Rede
{
    public class RoteadorCallback :IRoteadorOrdensCallback
    {

        #region log4net
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        private long _timestamp;
        bool _isRunning;
        Thread _thMonitorSign;
        private IAssinaturasRoteadorOrdensCallback _clientRoteador;
        static RoteadorCallback _me = null;
        #endregion

        public static RoteadorCallback GetInstance()
        {
            if (_me == null)
            {
                _me = new RoteadorCallback();
            }
            return _me;
        }

        public RoteadorCallback()
        {
            _timestamp = _getSecsFromTicks(DateTime.Now.Ticks);
            _isRunning = false;
            _thMonitorSign = null;
        }


        public void Start()
        {
            try
            {
                logger.Info("Iniciando callback do roteador...");
                _isRunning = true;
                _thMonitorSign = new Thread(new ThreadStart(this._monitorSignCallback));
                _thMonitorSign.Start();

                logger.Info("Callback iniciado");

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do RoteadorCallback: " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando callback do roteador...");

                logger.Info("Desalocando thread de monitoramento");
                _isRunning = false;
                if (null != _thMonitorSign)
                {
                    if (_thMonitorSign.IsAlive) _thMonitorSign.Join(500);
                    if (_thMonitorSign.IsAlive) _thMonitorSign.Abort();
                    _thMonitorSign = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do Callback: " + ex.Message, ex);
            }
        }

        public void _monitorSignCallback()
        {
            try
            {
                int i = 0;
                // Assinar em uma primeira tentativa
                _assinaCallbackRoteador();

                while (_isRunning)
                {
                    // Se ficou mais de 60 segundos sem receber status
                    // de conexao, reinicia o channel WCF ( 1 tentativa a cada minuto) 
                    if (this._lastTimeStampInterval() > 60)
                    {
                        if (i > 600)
                        {
                            _cancelRoteadorCallback();
                            _assinaCallbackRoteador();
                            i = 0;
                        }
                        else
                            i++;
                    }
                    Thread.Sleep(100);
                }

            }
            catch (Exception ex)
            {
                logger.Error("Problemas na reassinatura do callback: " + ex.Message, ex);
            }
        }

        #region IRoteadorOrdensCallback members
        public void OrdemAlterada(OrdemInfo pAlteracao)
        {
            logger.Info("OrdemAlterada():");

            _timestamp = _getSecsFromTicks(DateTime.Now.Ticks);

            // OnChegadaDeAcompanhamento(pAlteracao);
            // TODO [FF]: Efetuar chamada da funcao para processar rejeicao da requisicao da ordem
            if (pAlteracao.OrdStatus == OrdemStatusEnum.CANCELAMENTOREJEITADO)
                OrderCache4Socket.GetInstance().ProcessCancelRejection(pAlteracao);
        }

        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            logger.InfoFormat("Ex [{0}] Chan [{1}] Conn [{2}]", status.Bolsa, status.Operador, status.Conectado);

            _timestamp = _getSecsFromTicks(DateTime.Now.Ticks);
        }
        #endregion

        /// <summary>
        /// Converte DateTime.Ticks em segundos
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private long _getSecsFromTicks(long ticks)
        {
            // From fucking MSDN:
            //A single tick represents one hundred nanoseconds or one
            //ten-millionth of a second. There are 10,000 ticks in a millisecond. 
            return ticks / 10000 / 1000;
        }


        private long _lastTimeStampInterval()
        {
            return (_getSecsFromTicks(DateTime.Now.Ticks) - _timestamp);
        }

        /// <summary>
        /// Aborta a conexao com Roteador
        /// </summary>
        private void _cancelRoteadorCallback()
        {
            try
            {
                Ativador.AbortChannel(_clientRoteador);
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _cancelRoteadorCallback():" + ex.Message, ex);
            }
        }

        private void _assinaCallbackRoteador()
        {
            try
            {
                logger.Info("Chamando ativador para instanciar o cliente do roteador...");

                _clientRoteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

                if (_clientRoteador != null)
                {
                    logger.Info("Cliente do roteador instanciado, enviando request de assinatura...");

                    AssinarExecucaoOrdemResponse lResposta = _clientRoteador.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());                         // Faz a chamada pra abrir a conexão com o roteador; só serve pra enviar o contexto, e o roteador assinar a ponte duplex 

                    if (lResposta.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        logger.InfoFormat("Conexão com o roteador aberta, resposta do servidor: [{0}] [{1}]"
                                                   , lResposta.StatusResposta
                                                   , lResposta.DescricaoResposta);
                    }
                    else
                    {
                        logger.InfoFormat("Conexão com o roteador com erro, resposta do servidor: [{0}] [{1}]"
                                                   , lResposta.StatusResposta
                                                   , lResposta.DescricaoResposta);

                        // NULL para proxima tentativa
                        _clientRoteador = null;                                                                                   // Setando como null pra tentar novamente depois, ver conforme o protocolo o que fazer
                    }

                    // Assina os status de conexao a bolsa para manter o canal aberto.
                    AssinarStatusConexaoBolsaResponse resp = _clientRoteador.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _assinaCallbackRoteador():" + ex.Message, ex);
            }
        }

    }
}

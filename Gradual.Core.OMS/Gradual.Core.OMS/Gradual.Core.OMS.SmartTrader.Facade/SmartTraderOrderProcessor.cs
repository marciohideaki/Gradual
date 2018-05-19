using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.OMS.SmartTrader.Lib.Mensagens;
using log4net;
using Gradual.Core.SmartTrader.Persistencia;
using Gradual.Core.OMS.SmartTrader.Lib.Dados;
using Gradual.Core.Ordens.Lib;
using Gradual.OMS.ServicoRoteador;
using Gradual.Core.OrdensMonitoracao.ADM.Lib;
using Gradual.Core.OrdensMonitoracao.ADM.Lib.Mensagens;
using Gradual.Core.Ordens;
using System.Threading;
using Gradual.OMS.Library.Servicos;


namespace Gradual.Core.OMS.SmartTrader.Facade
{


    /// <summary>
    /// Class in charge of handle smarttrader's order request
    /// </summary>
    public class SmartTraderOrderProcessor
    {
             

        #region [Public declarations]

        /// <summary>
        /// Objeto responsavel por logar as operações realizadas pelas solicitações de Ordens.
        /// </summary>
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region [ WCF ORDER INTERFACE ]


        #endregion

        // Constructor
        public SmartTraderOrderProcessor()
        {
            // setting objects up
        }

        /// <summary>
        /// Method accountable for managing a new SmartTrader order
        /// </summary>
        /// <param name="request"> SmartTrader order parameters </param>
        /// <returns></returns>
        public EnviarOrdemSmartResponse EnviarOrdemSmartTrader(EnviarOrdemSmartRequest request)
        {
            EnviarOrdemSmartResponse response = new EnviarOrdemSmartResponse();

            try
            {
                /* TRACING */
                logger.Info("ACCOUNT............................................." + request.SmartOrder.Account.ToString());
                logger.Info("INSTRUMENT.........................................." + request.SmartOrder.Instrument.ToString());
                logger.Info("PRECOORDEM.........................................." + request.SmartOrder.OperacaoInicio.PrecoOrdem.ToString());
                logger.Info("PRECODISPARO........................................" + request.SmartOrder.OperacaoInicio.PrecoDisparo.ToString());
                logger.Info("OPERACAO LUCRO [PRECOORDEM] ........................" + request.SmartOrder.OperacaoLucro.PrecoOrdem.ToString());
                logger.Info("OPERACAO LUCRO [PRECODISPARO] ......................" + request.SmartOrder.OperacaoLucro.PrecoDisparo.ToString());
                logger.Info("OPERACAO LUCRO [TIPOORDEM] ........................." + request.SmartOrder.OperacaoLucro.PrecoOrdemTipo.ToString());
                logger.Info("OPERACAO LUCRO [VALOR] ............................." + request.SmartOrder.OperacaoLucro.Valor.ToString());
                logger.Info("OPERACAO PERDA [PRECOORDEM] ........................" + request.SmartOrder.OperacaoPerda.PrecoOrdem.ToString());
                logger.Info("OPERACAO PERDA [PRECODISPARO] ......................" + request.SmartOrder.OperacaoPerda.PrecoDisparo.ToString());
                logger.Info("OPERACAO LUCRO [TIPOORDEM] ........................." + request.SmartOrder.OperacaoPerda.PrecoOrdemTipo.ToString());
                logger.Info("OPERACAO LUCRO [VALOR] ............................." + request.SmartOrder.OperacaoPerda.Valor.ToString());

                //REGISTERING THE ORDER IN THE DB
                PersistenciaSmartTrader _PersistenciaSmartTrader = new PersistenciaSmartTrader();

                // RETURN THE SAME OBJECT ,HOWEVER THE PROPERTY "ID" IS FILLED IN
                response.OrdemSmart = _PersistenciaSmartTrader.InserirOrdem(request.SmartOrder);
                response.StatusResponse = "OK";

                ThreadPool.QueueUserWorkItem(new WaitCallback(SendOrder), request);

            }
            catch (Exception ex)
            {
                response.DescricaoErro = "OCORREU UM ERRO AO GRAVAR A SMARTORDER NO BANCO DE DADOS. DESCRICAO DO ERRO: " + ex.Message;
                response.StackTrace = ex.StackTrace;
                response.StatusResponse = "ERRO";
                logger.Info("OCORREU UM ERRO AO GRAVAR A SMARTORDER NO BANCO DE DADOS. DESCRICAO DO ERRO: " + ex.Message);
            }

            return response;

        }

        /// <summary>
        /// Method in charge of handle with the IServicoOrdens
        /// </summary>
        /// <param name="pEnviarOrdemSmartRequest"></param>
        private void SendOrder(object pEnviarOrdemSmartRequest)
        {
            //unboxing
            EnviarOrdemSmartRequest _EnviarOrdemSmartRequest = (EnviarOrdemSmartRequest)(pEnviarOrdemSmartRequest);

            EnviarOrdemRequest OrderRequest = new EnviarOrdemRequest();
            EnviarOrdemResponse OrderResponse = new EnviarOrdemResponse();

            try
            {
                IServicoOrdens ServicoOrdens = Ativador.Get<IServicoOrdens>();

                //FILLING SMARTTRADER ORDER 

                OrderRequest.ClienteOrdemInfo.Account = _EnviarOrdemSmartRequest.SmartOrder.Account;
                OrderRequest.ClienteOrdemInfo.Symbol = _EnviarOrdemSmartRequest.SmartOrder.Instrument;
                OrderRequest.ClienteOrdemInfo.ExpireDate = DateTime.Now;
                OrderRequest.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;

                if (_EnviarOrdemSmartRequest.SmartOrder.Side == Side.Compra){
                    OrderRequest.ClienteOrdemInfo.Side = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra;
                }
                else if (_EnviarOrdemSmartRequest.SmartOrder.Side == Side.Venda){
                    OrderRequest.ClienteOrdemInfo.Side = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda;
                }

                OrderRequest.ClienteOrdemInfo.Price = _EnviarOrdemSmartRequest.SmartOrder.OperacaoInicio.PrecoDisparo;
                OrderRequest.ClienteOrdemInfo.OrdType = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemTipoEnum.Limitada;
                OrderRequest.ClienteOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
                OrderRequest.ClienteOrdemInfo.OrderQty = _EnviarOrdemSmartRequest.SmartOrder.Qty;
                OrderRequest.ClienteOrdemInfo.ChannelID = 362;
                OrderRequest.ClienteOrdemInfo.CompIDOMS = "SMART";                
                OrderRequest.ClienteOrdemInfo.CumQty = 0;
                OrderRequest.ClienteOrdemInfo.MinQty = 0;
                OrderRequest.ClienteOrdemInfo.OrigClOrdID = "";
                OrderRequest.ClienteOrdemInfo.Memo5149 = "SMART";
                OrderRequest.ClienteOrdemInfo.StopStartID = _EnviarOrdemSmartRequest.SmartOrder.Id;

                OrderResponse = ServicoOrdens.EnviarOrdem(OrderRequest);

            }
            catch (Exception ex)
            {
                logger.Info("OCORREU UM ERRO AO ENVIAR A ORDEM PARA O SERVICO DE ORDENS. DESCRICAO: " + ex.Message);
            }


        }
    }
}

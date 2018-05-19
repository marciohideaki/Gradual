using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

using QuickFix;
using QuickFix44;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBMF
{
    public static class CanalNegociacaoBMFTradutor
    {
        /// <summary>
        /// Traduz mensagens proprietárias para mensagens fix
        /// </summary>
        /// <param name="canal">Utiliza o canal para ler configurações necessárias na tradução</param>
        /// <param name="mensagem">Mensagem proprietária a ser traduzida</param>
        /// <returns>Mensagem Fix gerada</returns>
        public static QuickFix.Message Traduzir(CanalNegociacaoBMF canal, MensagemRequestBase mensagem)
        {
            // Inicializa
            QuickFix.Message mensagemRetorno = null;
            Type tipoMensagem = mensagem.GetType();
            
            // Faz a tradução de acordo com o tipo
            if (tipoMensagem == typeof(ExecutarOrdemRequest))
            {
                // ------------------------------------------------------
                // NEWORDERSINGLE
                // ------------------------------------------------------
                ExecutarOrdemRequest mensagem2 = (ExecutarOrdemRequest)mensagem;
                mensagemRetorno = CanalNegociacaoBMFMessageFactory.CreateNewOrderSingle(
                    mensagem2.ClOrdID, 
                    CanalNegociacaoBMFTradutor.TraduzirSide(mensagem2.Side),
                    mensagem2.TransactTime, 
                    CanalNegociacaoBMFTradutor.TraduzirOrdType(mensagem2.OrdType),
                    CanalNegociacaoBMFTradutor.TraduzirSecurityID(canal, mensagem2.Symbol), 
                    canal.Config.SecurityIDSource, // 8 = símbolo da bolsa (identificador BM&FBOVESPA para instrumento
                    mensagem2.Symbol, 
                    mensagem2.Price, 
                    mensagem2.OrderQty,
                    canal.Config.Parties
                );

                // TODO: CanalNegociacaoBMFTradutor: Acessar cadastro do cliente para descobrir a conta que tem que mandar aqui
                NewOrderSingle.NoAllocs allocs1 = new NewOrderSingle.NoAllocs();
                allocs1.set(new AllocAccount("123"));
                allocs1.set(new AllocAcctIDSource(99));
                mensagemRetorno.addGroup(allocs1);
            } 
            else if (tipoMensagem == typeof(ListarInstrumentosRequest))
            {
                // ------------------------------------------------------
                // SECURITYLISTREQUEST
                // ------------------------------------------------------
                ListarInstrumentosRequest mensagem2 = (ListarInstrumentosRequest)mensagem;
                mensagemRetorno = CanalNegociacaoBMFMessageFactory.CreateSecurityListRequest(
                    mensagem2.CodigoMensagem, 
                    SubscriptionRequestTypeEnum.Snapshot,
                    null,
                    SecurityListRequestTypeEnum.TodosInstrumentos,
                    null, null, null, null);
            }
            else if (tipoMensagem == typeof(CancelarOrdemRequest))
            {
                // ------------------------------------------------------
                // ORDERCANCELREQUEST
                // ------------------------------------------------------
                CancelarOrdemRequest mensagem2 = (CancelarOrdemRequest)mensagem;
                mensagemRetorno = CanalNegociacaoBMFMessageFactory.CreateOrderCancelRequest(
                    mensagem2.OrigClOrdID,
                    mensagem2.OrderID,
                    mensagem2.Symbol,
                    CanalNegociacaoBMFTradutor.TraduzirSecurityID(canal, mensagem2.Symbol),
                    canal.Config.SecurityIDSource,
                    mensagem2.ClOrdID,
                    mensagem2.SecondaryOrderID,
                    CanalNegociacaoBMFTradutor.TraduzirSide(mensagem2.Side),
                    mensagem2.DataReferencia,
                    null,
                    canal.Config.Parties);
            }
            else if (tipoMensagem == typeof(SincronizarCanalRequest))
            {
                // ------------------------------------------------------
                // RESENDREQUEST
                // ------------------------------------------------------
                mensagemRetorno = CanalNegociacaoBMFMessageFactory.CreateResendRequest(1, 0);
            }
            else if (tipoMensagem == typeof(AlterarOrdemRequest))
            {
                // ------------------------------------------------------
                // ORDERCANCELREPLACEREQUEST
                // ------------------------------------------------------
                AlterarOrdemRequest mensagem2 = (AlterarOrdemRequest)mensagem;
                mensagemRetorno =
                    CanalNegociacaoBMFMessageFactory.CreateOrderCancelReplaceRequest(
                        mensagem2.OrigClOrdID, mensagem2.OrderID, mensagem2.SecurityID,
                        mensagem2.SecurityIDSource, mensagem2.Symbol, mensagem2.ClOrdID,
                        mensagem2.SecondaryOrderID, mensagem2.OrigSecondaryOrderID, CanalNegociacaoBMFTradutor.TraduzirSide(mensagem2.Side),
                        mensagem2.OrderQty.Value, mensagem2.MinQty.Value, mensagem2.MaxFloor.Value,
                        OrdType.LIMIT, mensagem2.Price.Value, mensagem2.TransactTime,
                        null, 0, ' ', canal.Config.Parties);
            }
            else if (tipoMensagem == typeof(ExecutarDiretoRequest))
            {
                // ------------------------------------------------------
                // NEWORDERCROSS
                // ------------------------------------------------------
                ExecutarDiretoRequest mensagem2 = (ExecutarDiretoRequest)mensagem;
                //mensagemRetorno = CanalNegociacaoBMFMessageFactory.CreateNewOrderCross();
            }
            else
            {
                throw (new Exception("Mensagem não suportada pelo canal"));
            }

            // Retorna
            return mensagemRetorno;
        }

        public static string TraduzirSecurityID(CanalNegociacaoBMF canal, string symbol)
        {
            // TODO: CanalNegociacaoTesteTradutor - TraduzirSecurityID: Traduzir canal para codigo bolsa
            // Faz a tradução código do instrumento
            return 
                ((ConsultarInstrumentosResponse)
                    canal.ServicoOrdens.ConsultarInstrumentos(
                            new ConsultarInstrumentosRequest() 
                            { 
                                CodigoBolsa = canal.Codigo, 
                                Symbol = symbol 
                            })).Instrumentos[0].SecurityID;
        }

        public static char TraduzirSide(OrdemDirecaoEnum direcao)
        {
            // 1 = compra / 2 = venda
            char retorno = ' ';
            switch (direcao)
            {
                case OrdemDirecaoEnum.Compra:
                    retorno = '1';
                    break;
                case OrdemDirecaoEnum.Venda:
                    retorno = '2';
                    break;
            }
            return retorno;
        }

        public static char TraduzirOrdType(OrdemTipoEnum ordemTipo)
        {
            char retorno = ' ';
            switch (ordemTipo)
            {
                case OrdemTipoEnum.Limitada:
                    retorno = '2';
                    break;
                case OrdemTipoEnum.StopLimitada:
                    retorno = '4';
                    break;
                case OrdemTipoEnum.MarketWithLeftOverLimit:
                    retorno = 'K';
                    break;
            }
            return retorno;
        }

    }
}

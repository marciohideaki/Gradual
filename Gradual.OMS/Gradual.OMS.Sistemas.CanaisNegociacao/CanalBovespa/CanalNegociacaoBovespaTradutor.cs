using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

using QuickFix;
using QuickFix42;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBovespa
{
    public static class CanalNegociacaoBovespaTradutor
    {
        /// <summary>
        /// Traduz mensagens proprietárias para mensagens fix
        /// </summary>
        /// <param name="canal">Utiliza o canal para ler configurações necessárias na tradução</param>
        /// <param name="mensagem">Mensagem proprietária a ser traduzida</param>
        /// <returns>Mensagem Fix gerada</returns>
        public static QuickFix.Message Traduzir(CanalNegociacaoBovespa canal, MensagemRequestBase mensagem)
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

                // Inicializa
                ExecutarOrdemRequest mensagem2 = (ExecutarOrdemRequest)mensagem;
                
                // Complementa informações da ordem
                mensagem2.HandInst = '1';
                mensagem2.ExecBroker = canal.Config.PartyID;

                // Cria a mensagem
                mensagemRetorno = CanalNegociacaoBovespaMessageFactory.CreateNewOrderSingle(
                    mensagem2.Account,
                    mensagem2.HandInst,
                    mensagem2.ExecBroker,
                    mensagem2.Symbol,
                    CanalNegociacaoBovespaTradutor.TraduzirSide(mensagem2.Side),
                    mensagem2.ClOrdID,
                    mensagem2.OrderQty,
                    mensagem2.MinQty,
                    mensagem2.MaxFloor,
                    CanalNegociacaoBovespaTradutor.TraduzirOrdType(mensagem2.OrdType),
                    mensagem2.Price,
                    mensagem2.StopPx,
                    CanalNegociacaoBovespaTradutor.TraduzirTimeInForce(mensagem2.TimeInForce),
                    mensagem2.TransactTime, mensagem2.ExpireDate);
            }
            else if (tipoMensagem == typeof(CancelarOrdemRequest))
            {
                // ------------------------------------------------------
                // ORDERCANCELREQUEST
                // ------------------------------------------------------
                
                // Inicializa
                CancelarOrdemRequest mensagem2 = (CancelarOrdemRequest)mensagem;
                
                // Complementa informações da ordem
                mensagem2.ExecBroker = canal.Config.PartyID;
                mensagem2.Account = mensagem2.CodigoCliente;

                // Cria mensagem
                mensagemRetorno = CanalNegociacaoBovespaMessageFactory.CreateOrderCancelRequest(
                    mensagem2.Account,
                    mensagem2.OrigClOrdID,
                    mensagem2.OrderID,
                    mensagem2.ClOrdID,
                    mensagem2.Symbol,
                    CanalNegociacaoBovespaTradutor.TraduzirSide(mensagem2.Side),
                    mensagem2.OrderQty,
                    mensagem2.ExecBroker,
                    mensagem2.TransactTime);
            }
            else if (tipoMensagem == typeof(AlterarOrdemRequest))
            {
                // ------------------------------------------------------
                // ORDERCANCELREPLACEREQUEST
                // ------------------------------------------------------
                
                // Inicializa
                AlterarOrdemRequest mensagem2 = (AlterarOrdemRequest)mensagem;
                
                // Complementa informações da ordem
                mensagem2.ExecBroker = canal.Config.PartyID;

                // Cria a mensagem
                mensagemRetorno =
                    CanalNegociacaoBovespaMessageFactory.CreateOrderCancelReplaceRequest(
                        mensagem2.OrigClOrdID,
                        mensagem2.OrderID,
                        mensagem2.ClOrdID,
                        mensagem2.ExecBroker,
                        CanalNegociacaoBovespaTradutor.TraduzirSide(mensagem2.Side),
                        mensagem2.Symbol,
                        mensagem2.OrderQty.Value,
                        mensagem2.MaxFloor,
                        CanalNegociacaoBovespaTradutor.TraduzirOrdType(mensagem2.OrdType), 
                        mensagem2.Price,
                        CanalNegociacaoBovespaTradutor.TraduzirTimeInForce(mensagem2.TimeInForce),
                        mensagem2.TransactTime);
            }
            else
            {
                throw (new Exception("Mensagem não suportada pelo canal"));
            }

            // Retorna
            return mensagemRetorno;
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
                case OrdemTipoEnum.Mercado:
                    retorno = '1';
                    break;
                case OrdemTipoEnum.Limitada:
                    retorno = '2';
                    break;
                case OrdemTipoEnum.StopLimitada:
                    retorno = '4';
                    break;
                case OrdemTipoEnum.OnClose:
                    retorno = 'A';
                    break;
                case OrdemTipoEnum.MarketWithLeftOverLimit:
                    retorno = 'K';
                    break;
            }
            return retorno;
        }

        public static char? TraduzirTimeInForce(OrdemValidadeEnum ordemValidade)
        {
            char? retorno = null;
            switch (ordemValidade)
            {
                case OrdemValidadeEnum.ValidaParaODia:
                    retorno = '0';
                    break;
                case OrdemValidadeEnum.ValidaAteSerCancelada:
                    retorno = '1';
                    break;
                case OrdemValidadeEnum.ValidaParaAberturaDoMercado:
                    retorno = '2';
                    break;
                case OrdemValidadeEnum.ExecutaIntegralParcialOuCancela:
                    retorno = '3';
                    break;
                case OrdemValidadeEnum.ValidoAteDeterminadaData:
                    retorno = '6';
                    break;
            }
            return retorno;
        }
    }
}

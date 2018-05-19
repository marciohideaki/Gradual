using System;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;

namespace Gradual.OMS.ServicoRoteador
{
    public static class TradutorFix
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DateTime TraduzirData(string valor)
        {
            return new DateTime(
                int.Parse(valor.Substring(0, 4)),
                int.Parse(valor.Substring(4, 2)),
                int.Parse(valor.Substring(6, 2)));
        }

        public static OrdemDirecaoEnum TraduzirOrdemDirecao(char valor)
        {
            // Decide
            OrdemDirecaoEnum retorno = OrdemDirecaoEnum.NaoInformado;
            switch (valor)
            {
                case QuickFix.Side.BUY:
                    retorno = OrdemDirecaoEnum.Compra;
                    break;
                case QuickFix.Side.SELL:
                    retorno = OrdemDirecaoEnum.Venda;
                    break;
                default:
                    logger.Error("TradutorFix.TraduzirOrdemDirecao() - Direção de ordem não implementado: " + valor.ToString());
                    retorno = OrdemDirecaoEnum.NaoInformado;
                    break;
            }

            // Retorna
            return retorno;
        }


        public static OrdemStatusEnum TraduzirOrdemStatus(char valor)
        {
            // Decide
            OrdemStatusEnum retorno = OrdemStatusEnum.NOVA;
            switch (valor)
            {
                case QuickFix.OrdStatus.PENDING_NEW:
                    retorno = OrdemStatusEnum.ENVIADAPARAABOLSA;
                    break;
                case QuickFix.OrdStatus.NEW:
                    retorno = OrdemStatusEnum.NOVA;
                    break;
                case QuickFix.OrdStatus.PARTIALLY_FILLED:
                    retorno = OrdemStatusEnum.PARCIALMENTEEXECUTADA;
                    break;
                case QuickFix.OrdStatus.FILLED:
                    retorno = OrdemStatusEnum.EXECUTADA;
                    break;
                case QuickFix.OrdStatus.CANCELED:
                    retorno = OrdemStatusEnum.CANCELADA;
                    break;
                case QuickFix.OrdStatus.REPLACED:
                    retorno = OrdemStatusEnum.SUBSTITUIDA;
                    break;
                case QuickFix.OrdStatus.PENDING_CANCEL:
                    retorno = OrdemStatusEnum.CANCELADA;
                    break;
                case QuickFix.OrdStatus.REJECTED:
                    retorno = OrdemStatusEnum.REJEITADA;
                    break;
                case QuickFix.OrdStatus.SUSPENDED:
                    retorno = OrdemStatusEnum.SUSPENSA;
                    break;
                case QuickFix.OrdStatus.EXPIRED:
                    retorno = OrdemStatusEnum.EXPIRADA;
                    break;
                default:
                    logger.Error("TradutorFix.TraduzirOrdemStatus() - Status de ordem não implementado: " + valor.ToString());
                    retorno = OrdemStatusEnum.REJEITADA;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemTipoEnum TraduzirOrdemTipo(char valor)
        {
            // Decide
            OrdemTipoEnum retorno = OrdemTipoEnum.Limitada;
            switch (valor)
            {
                case QuickFix.OrdType.LIMIT:
                    retorno = OrdemTipoEnum.Limitada;
                    break;
                case QuickFix.OrdType.STOPLIMIT:
                    retorno = OrdemTipoEnum.StopLimitada;
                    break;
                case QuickFix.OrdType.MARKET_WITH_LEFTOVER_AS_LIMIT:
                    retorno = OrdemTipoEnum.MarketWithLeftOverLimit;
                    break;
                case QuickFix.OrdType.ONCLOSE:
                    retorno = OrdemTipoEnum.OnClose;
                    break;
                default:
                    logger.Error("TraduzirOrdemTipo() - Tipo de ordem não implementado: " + valor.ToString());
                    retorno = OrdemTipoEnum.Limitada;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemValidadeEnum TraduzirOrdemValidade(char valor)
        {
            // Decide
            OrdemValidadeEnum retorno = OrdemValidadeEnum.NaoImplementado;
            switch (valor)
            {
                case QuickFix.TimeInForce.DAY:
                    retorno = OrdemValidadeEnum.ValidaParaODia;
                    break;
                case QuickFix.TimeInForce.IMMEDIATEORCANCEL:
                    retorno = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela;
                    break;
                case QuickFix.TimeInForce.FILL_OR_KILL:
                    retorno = OrdemValidadeEnum.ExecutaIntegralOuCancela;
                    break;
                case QuickFix.TimeInForce.GOOD_TILL_CANCEL:
                    retorno = OrdemValidadeEnum.ValidaAteSerCancelada;
                    break;
                case QuickFix.TimeInForce.GOOD_TILL_DATE:
                    retorno = OrdemValidadeEnum.ValidoAteDeterminadaData;
                    break;
                case QuickFix.TimeInForce.ATTHEOPENING:
                    retorno = OrdemValidadeEnum.ValidaParaAberturaDoMercado;
                    break;
                default:
                    logger.Error("TradutorFix.TraduzirOrdemValidade() - Validade de ordem não implementado: " + valor.ToString());
                    retorno = OrdemValidadeEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static QuickFix.TimeInForce deOrdemValidadeParaTimeInForce(OrdemValidadeEnum ordValidade)
        {
            QuickFix.TimeInForce retorno = null;

            switch (ordValidade)
            {
                case OrdemValidadeEnum.ValidaParaODia:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.DAY);
                    break;
                case OrdemValidadeEnum.ExecutaIntegralParcialOuCancela:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.IMMEDIATEORCANCEL);
                    break;
                case OrdemValidadeEnum.ExecutaIntegralOuCancela:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.FILL_OR_KILL);
                    break;
                case OrdemValidadeEnum.ValidaAteSerCancelada:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.GOOD_TILL_CANCEL);
                    break;
                case OrdemValidadeEnum.ValidoAteDeterminadaData:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.GOOD_TILL_DATE);
                    break;
                case OrdemValidadeEnum.ValidaParaAberturaDoMercado:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.ATTHEOPENING);
                    break;
                case OrdemValidadeEnum.FechamentoDoMercado:
                    retorno = new QuickFix.TimeInForce(QuickFix.TimeInForce.ATTHECLOSE);
                    break;
                case OrdemValidadeEnum.BoaParaLeilao:
                    retorno = new QuickFix.TimeInForce('A');
                    break;
                default:
                    logger.Error("TradutorFix.deOrdemValidadeParaTimeInForce() - Validade de ordem não implementado: " + ordValidade.ToString());
                    retorno = null;
                    break;
            }

            return retorno;
        }

        public static QuickFix.OrdType deOrdemTipoParaOrdType(OrdemTipoEnum ordTipo)
        {
            QuickFix.OrdType retorno = null;

            switch (ordTipo)
            {
                case OrdemTipoEnum.Limitada:
                    retorno = new QuickFix.OrdType(QuickFix.OrdType.LIMIT);
                    break;
                case OrdemTipoEnum.StopLimitada:
                    retorno = new QuickFix.OrdType(QuickFix.OrdType.STOPLIMIT);
                    break;
                case OrdemTipoEnum.MarketWithLeftOverLimit:
                    retorno = new QuickFix.OrdType(QuickFix.OrdType.MARKET_WITH_LEFTOVER_AS_LIMIT);
                    break;
                case OrdemTipoEnum.OnClose:
                    retorno = new QuickFix.OrdType(QuickFix.OrdType.ONCLOSE);
                    break;
                case OrdemTipoEnum.Mercado:
                    retorno = new QuickFix.OrdType(QuickFix.OrdType.MARKET);
                    break;
                case OrdemTipoEnum.StopLoss:
                    retorno = new QuickFix.OrdType(QuickFix.OrdType.STOP);
                    break;
                default:
                    logger.Error("TradutorFix.deOrdemTipoParaOrdType() - Tipo de ordem não implementado: " + ordTipo.ToString());
                    retorno = null;
                    break;
            }

            return retorno;
        }

        /// <summary>
        /// Imprime o conteudo dos campos do objeto OrdemInfo
        /// </summary>
        /// <param name="ordem">objeto OrdemInfo</param>
        public static void DumpOrdemInfo(OrdemInfo ordem)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("Account ...........: " + ordem.Account);
                logger.Debug("ID da Ordem (DB) ..: " + ordem.IdOrdem);
                logger.Debug("Numero da ordem ...: " + ordem.ClOrdID);
                logger.Debug("ExchangeNumber ....: " + ordem.ExchangeNumberID);
                logger.Debug("Bolsa .............: " + ordem.Exchange);
                logger.Debug("Operador...........: " + ordem.ChannelID);
                logger.Debug("OrigClOrdID .......: " + ordem.OrigClOrdID);
                logger.Debug("Symbol ............: " + ordem.Symbol);
                logger.Debug("Status ............: " + ordem.OrdStatus);
                logger.Debug("Side ..............: " + ordem.Side);
                logger.Debug("TimeInForce .......: " + ordem.TimeInForce);
                logger.Debug("TransactTime ......: " + ordem.TransactTime.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                logger.Debug("OrderType .........: " + ordem.OrdType.ToString());
                logger.Debug("Qtde  .............: " + ordem.OrderQty);
                logger.Debug("Qtde Pendente .....: " + ordem.OrderQtyRemmaining);
                logger.Debug("Preco .............: " + ordem.Price);
                logger.Debug("Preco Stop ........: " + ordem.StopPrice);
                logger.Debug("Register Time  ....: " + ordem.RegisterTime.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                logger.Debug("Qtde Aparente .....: " + ordem.MaxFloor);
                logger.Debug("Qtde minima .......: " + ordem.MinQty);
                logger.Debug("Expiration Date ...: " + (ordem.ExpireDate.HasValue? ordem.ExpireDate.Value.ToString("yyyy/MM/dd HH:mm:ss.fffffff") : "--"));

                if ( ordem.Acompanhamentos != null )
                {
                    int i = 0;
                    foreach( AcompanhamentoOrdemInfo acomp in ordem.Acompanhamentos )
                    {
                        logger.Debug("Acompanhamento : " + i);
                        
                        logger.Debug("(A) DataAtualizacao ......: " + acomp.DataAtualizacao.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                        logger.Debug("(A) CodigoRejeicao .......: " + acomp.CodigoRejeicao);
                        logger.Debug("(A) Descricao ............: " + acomp.Descricao);
                        logger.Debug("(A) Qtde Exc .............: " + acomp.QuantidadeExecutada);
                        logger.Debug("(A) Qtde Solicitada ......: " + acomp.QuantidadeSolicitada);
                        logger.Debug("(A) Status Ordem .........: " + acomp.StatusOrdem);
                        logger.Debug("(A) NumeroControleOrdem ..: " + acomp.NumeroControleOrdem);
                        logger.Debug("(A) CodigoResposta .......: " + acomp.CodigoResposta);
                        logger.Debug("(A) Data Envio Ordem   ...: " + acomp.DataOrdemEnvio.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));
                        logger.Debug("(A) Data Validade ........: " + acomp.DataValidade.ToString("yyyy/MM/dd HH:mm:ss.fffffff"));

                        i++;
                    }
                }
            } //if (logger.IsDebugEnabled)
        }

        public static EmolumentoTipoEnum TraduzirEmolumentoTipo(int tipo)
        {
            EmolumentoTipoEnum ret = EmolumentoTipoEnum.UNDEFINED;

            switch (tipo)
            {
                case 96: ret = EmolumentoTipoEnum.IOF_FEE; break;
                case 97: ret = EmolumentoTipoEnum.TRADING_FEE; break;
                case 98: ret = EmolumentoTipoEnum.SETTLEMENT_FEE; break;
                case 99: ret = EmolumentoTipoEnum.ONSHORE_BROKE_COMMISSION; break;
                default: ret = EmolumentoTipoEnum.UNDEFINED; break;
            }

            return ret;
        }
    }
}

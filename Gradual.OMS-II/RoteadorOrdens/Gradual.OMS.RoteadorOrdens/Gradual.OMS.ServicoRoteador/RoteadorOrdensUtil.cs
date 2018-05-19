using System;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using QuickFix;

namespace Gradual.OMS.ServicoRoteador
{
    public static class RoteadorOrdensUtil
    {

        /// <summary>
        /// Formata uma mensagem de erro de Execucao de Ordem
        /// </summary>
        /// <param name="msg">Mensagem a ser informada para o requerente</param>
        /// <param name="status">Status indicando sucesso ou erro</param>
        /// <returns></returns>
        public static DadosRetornoExecutarOrdem FormatarRespostaEO(string msg, StatusRoteamentoEnum status)
        {
            DadosRetornoExecutarOrdem retorno = new DadosRetornoExecutarOrdem();

            retorno.DataResposta = DateTime.Now;

            OcorrenciaRoteamentoOrdem ocorrencia = new OcorrenciaRoteamentoOrdem();
            ocorrencia.DataHoraOcorrencia = DateTime.Now;

            ocorrencia.Ocorrencia = msg;
            retorno.Ocorrencias.Add(ocorrencia);
            retorno.StatusResposta = status;

            return retorno;
        }

        /// <summary>
        /// Formata uma mensagem de erro de cancelamento de Ordem
        /// </summary>
        /// <param name="msg">Mensagem a ser informada para o requerente</param>
        /// <param name="status">Status indicando sucesso ou erro</param>
        /// <returns></returns>
        public static DadosRetornoExecutarCancelamentoOrdem FormatarRespostaCancelamento(string msg, StatusRoteamentoEnum status)
        {
            DadosRetornoExecutarCancelamentoOrdem retorno = new DadosRetornoExecutarCancelamentoOrdem();

            retorno.DataResposta = DateTime.Now;

            OcorrenciaRoteamentoOrdem ocorrencia = new OcorrenciaRoteamentoOrdem();
            ocorrencia.DataHoraOcorrencia = DateTime.Now;

            ocorrencia.Ocorrencia = msg;
            retorno.Ocorrencias.Add(ocorrencia);
            retorno.StatusResposta = status;

            return retorno;
        }

        /// <summary>
        /// Formata uma mensagem de erro de cancelamento de Ordem
        /// </summary>
        /// <param name="msg">Mensagem a ser informada para o requerente</param>
        /// <param name="status">Status indicando sucesso ou erro</param>
        /// <returns></returns>
        public static DadosRetornoExecutarModificacaoOrdem FormatarRespostaModificacao(string msg, StatusRoteamentoEnum status)
        {
            DadosRetornoExecutarModificacaoOrdem retorno = new DadosRetornoExecutarModificacaoOrdem();

            retorno.DataResposta = DateTime.Now;

            OcorrenciaRoteamentoOrdem ocorrencia = new OcorrenciaRoteamentoOrdem();
            ocorrencia.DataHoraOcorrencia = DateTime.Now;

            ocorrencia.Ocorrencia = msg;
            retorno.Ocorrencias.Add(ocorrencia);
            retorno.StatusResposta = status;

            return retorno;
        }

        /// <summary>
        /// Formata uma mensagem de erro de Execucao de OrdemCross
        /// </summary>
        /// <param name="msg">Mensagem a ser informada para o requerente</param>
        /// <param name="status">Status indicando sucesso ou erro</param>
        /// <returns></returns>
        public static DadosRetornoExecutarOrdemCross FormatarRespostaEOX(string msg, StatusRoteamentoEnum status)
        {
            DadosRetornoExecutarOrdemCross retorno = new DadosRetornoExecutarOrdemCross();

            retorno.DataResposta = DateTime.Now;

            OcorrenciaRoteamentoOrdem ocorrencia = new OcorrenciaRoteamentoOrdem();
            ocorrencia.DataHoraOcorrencia = DateTime.Now;

            ocorrencia.Ocorrencia = msg;
            retorno.Ocorrencias.Add(ocorrencia);
            retorno.StatusResposta = status;

            return retorno;
        }

        /// <summary>
        /// Cria um novo objeto OrdemInfo a partir do objeto fornecido
        /// </summary>
        /// <param name="ordem">OrdemInfo a </param>
        /// <returns></returns>
        public static OrdemInfo CloneOrder(OrdemInfo ordem)
        {
            OrdemInfo clone = new OrdemInfo();

            clone.Account = ordem.Account;
            clone.Acompanhamentos.AddRange(ordem.Acompanhamentos.GetRange(0, ordem.Acompanhamentos.Count));
            clone.ChannelID = ordem.ChannelID;
            clone.ClOrdID = ordem.ClOrdID;
            clone.CumQty = ordem.CumQty;
            clone.Exchange = ordem.Exchange;
            clone.ExchangeNumberID = ordem.ExchangeNumberID;
            clone.ExecBroker = ordem.ExecBroker;
            clone.ExpireDate = ordem.ExpireDate;
            clone.FixMsgSeqNum = ordem.FixMsgSeqNum;
            clone.IdOrdem = ordem.IdOrdem;
            clone.MaxFloor = ordem.MaxFloor;
            clone.MinQty = ordem.MinQty;
            clone.OrderQty = ordem.OrderQty;
            clone.OrderQtyRemmaining = ordem.OrderQtyRemmaining;
            clone.OrdStatus = ordem.OrdStatus;
            clone.OrdType = ordem.OrdType;
            clone.OrigClOrdID = ordem.OrigClOrdID;
            clone.Price = ordem.Price;
            clone.RegisterTime = ordem.RegisterTime;
            clone.SecurityExchangeID = ordem.SecurityExchangeID;
            clone.SecurityID = ordem.SecurityID;
            clone.SecurityIDSource = ordem.SecurityIDSource;
            clone.Side = ordem.Side;
            clone.StopPrice = ordem.StopPrice;
            clone.StopStartID = ordem.StopStartID;
            clone.Symbol = ordem.Symbol;
            clone.TimeInForce = ordem.TimeInForce;
            clone.TransactTime = ordem.TransactTime;
            clone.PositionEffect = ordem.PositionEffect;
            clone.ProtectionPrice = ordem.ProtectionPrice;
            clone.Currency = ordem.Currency;
            clone.ForeignFirm = ordem.ForeignFirm;
            clone.Memo5149  = ordem.Memo5149;
            clone.AcountType = ordem.AcountType;

            return clone;
        }


        /// <summary>
        /// ConsistirCancelamento - consistir CancelamentoInfo
        /// </summary>
        /// <param name="orderCancelInfo"></param>
        public static void ConsistirCancelamento(OrdemCancelamentoInfo orderCancelInfo)
        {
            // Basic order identification
            if ( orderCancelInfo.Account < 0)
                throw new ArgumentException("Account field must be provided");

            if (orderCancelInfo.OrigClOrdID == null || orderCancelInfo.OrigClOrdID.Length <= 0)
                throw new ArgumentException("OrigClOrdID field must be provided");

            if (orderCancelInfo.ClOrdID == null || orderCancelInfo.ClOrdID.Length <= 0)
                throw new ArgumentException("ClOrdID field must be provided");

            // Instrument Identification Block
            if (orderCancelInfo.Symbol == null || orderCancelInfo.Symbol.Length <= 0)
                throw new ArgumentException("Symbol must be provided");

            if (orderCancelInfo.SecurityID == null || orderCancelInfo.SecurityID.Length <= 0)
                throw new ArgumentException("SecurityID field must be provided");

            //ATP - 2012-10-29
            //Qtde de contratos/papeis a serem cancelados
            if ( orderCancelInfo.OrderQty <= 0)
                throw new ArgumentException("OrderQty must be > 0 ");

            // Cliente
            if (orderCancelInfo.ExecBroker == null || orderCancelInfo.ExecBroker.Length <= 0)
                throw new ArgumentException("ExecBroker must be provided");
        }


        /// <summary>
        /// ConsistirOrdem - efetuar consistencia do order info
        /// </summary>
        /// <param name="ordem"></param>
        /// <param name="alteracao">flag para indicar se é ou nao alteracao de ordem</param>
        public static void ConsistirOrdem(OrdemInfo ordem, bool alteracao=false)
        {
            // Basic order identification
            if (ordem.Account < 0)
                throw new ArgumentException("Account field must be provided");

            if (alteracao)
            {
                if (ordem.OrigClOrdID == null || ordem.OrigClOrdID.Length <= 0)
                    throw new ArgumentException("OrigClOrdID field must be provided");
            }

            if (ordem.ClOrdID == null || ordem.ClOrdID.Length <= 0)
                throw new ArgumentException("ClOrdID field must be provided");

            // Instrument Identification Block
            if (ordem.Symbol == null || ordem.Symbol.Length <= 0)
                throw new ArgumentException("Symbol must be provided");

            if (ordem.SecurityID == null || ordem.SecurityID.Length <= 0)
                throw new ArgumentException("SecurityID field must be provided");

            if (ordem.OrderQty <= 0)
                throw new ArgumentException("OrderQty must be > 0 ");

            // Cliente
            if (ordem.ExecBroker == null || ordem.ExecBroker.Length <= 0)
                throw new ArgumentException("ExecBroker must be provided");

            TimeInForce tif = TradutorFix.deOrdemValidadeParaTimeInForce(ordem.TimeInForce);
            if (tif == null)
            {
                throw new ArgumentException("TimeInForce is invalid");
            }

            if (ordem.TimeInForce == OrdemValidadeEnum.ValidoAteDeterminadaData)
            {
                if ( ordem.ExpireDate == null || ordem.ExpireDate <= DateTime.Now )
                    throw new ArgumentException("ExpireDate is invalid");
            }

            OrdType ordType = TradutorFix.deOrdemTipoParaOrdType(ordem.OrdType);
            if (ordType == null)
            {
                throw new ArgumentException("OrdType is invalid");
            }

            // Verifica envio do preco
            switch (ordem.OrdType)
            {
                case OrdemTipoEnum.Limitada:
                case OrdemTipoEnum.MarketWithLeftOverLimit:
                    if ( ordem.Price <= 0.0 && ordem.TimeInForce != OrdemValidadeEnum.BoaParaLeilao)
                        throw new ArgumentException("Price must be > 0");
                    break;
                case OrdemTipoEnum.StopLimitada:
                case OrdemTipoEnum.StopStart:
                    if ( ordem.Price <= 0.0 )
                        throw new ArgumentException("Price must be > 0");
                    if (ordem.StopPrice <= 0.0)
                        throw new ArgumentException("StopPrice must be > 0");
                    break;
                case OrdemTipoEnum.Mercado:
                case OrdemTipoEnum.OnClose:
                    break;
                default:
                    if ( ordem.Price <= 0.0 )
                        throw new ArgumentException("Price must be > 0");
                    break;
            }

            if (ordem.MaxFloor < 0)
                throw new ArgumentException("MaxFloor must be >= 0");

            if (ordem.MinQty < 0)
                throw new ArgumentException("MinQty must be >= 0");

            if (ordem.ExecBroker == null || ordem.ExecBroker.Length <= 0)
                throw new ArgumentException("ExecBroker must be provided");

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using QuickFix;
using QuickFix.FIX44;
using QuickFix.Fields;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;


namespace Gradual.Core.OMS.FixServerLowLatency.Regras
{
    public class OrdensConsistencia
    {

        public static void ConsistirNovaOrdem(NewOrderSingle nos, string exchange)
        {

            // Basic order identification
            if (string.IsNullOrEmpty(nos.Account.ToString()))
                throw new ArgumentException("Account field must be provided");

            if (string.IsNullOrEmpty(nos.ClOrdID.ToString()))
                throw new ArgumentException("ClOrdID field must be provided");

            // Instrument Identification Block
            if (string.IsNullOrEmpty(nos.Symbol.ToString()))
                throw new ArgumentException("Symbol must be provided");

            // "obrigatorio" somente para bmf (nao obrigatorio na mensageria, mas dah erro se enviar 
            // sem este campo para bmf)
            //if (exchange.Equals(ExchangePrefixes.BMF, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    if (string.IsNullOrEmpty(nos.SecurityID.ToString()))
            //        throw new ArgumentException("SecurityID field must be provided");
            //}

            if (string.IsNullOrEmpty(nos.OrderQty.ToString()))
                throw new ArgumentException("OrderQty must be > 0 ");

            // obrigatorio se for FIX.4.4
            if (nos.IsSetField(Tags.NoPartyIDs))
            {
                try
                {
                    Group partyIDs = nos.GetGroup(1, Tags.NoPartyIDs);
                    if (string.IsNullOrEmpty(partyIDs.GetField(Tags.PartyID)))
                        throw new ArgumentException("ExecBroker must be provided");
                }
                catch
                {
                    throw new ArgumentException("ExecBroker must be provided");
                }
            }
            // Somente validar se o campo estiver atribuido, nao obrigatorio na mensageria
            if (nos.IsSetField(Tags.TimeInForce))
            {
                TimeInForce tif = nos.TimeInForce;
                if (tif == null)
                {
                    throw new ArgumentException("TimeInForce is invalid");
                }

                if (tif.ToString().Equals(TimeInForce.GOOD_TILL_DATE.ToString()))
                {
                    if (!string.IsNullOrEmpty(nos.ExpireDate.ToString()))
                    {
                        DateTime dtExpireDate = DateTime.ParseExact(nos.ExpireDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                        if (dtExpireDate.Date < DateTime.Now.Date)
                            throw new ArgumentException("ExpireDate is invalid");
                    }
                    else
                        throw new ArgumentException("ExpireDate is invalid");
                }
            }

            OrdType ordType = nos.OrdType;
            if (ordType == null)
            {
                throw new ArgumentException("OrdType is invalid");
            }

            // Verifica envio do preco
            switch (ordType.ToString()[0])
            {
                case OrdType.LIMIT:// OrdemTipoEnum.Limitada:
                case OrdType.MARKET_WITH_LEFTOVER_AS_LIMIT:// OrdemTipoEnum.MarketWithLeftOverLimit:
                    if (nos.Price.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("Price must be > 0");
                    break;
                case OrdType.STOP_LIMIT:// OrdemTipoEnum.StopLimitada:
                //case OrdType.STO OrdemTipoEnum.StopStart:
                    if (nos.Price.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("Price must be > 0");
                    if (nos.StopPx.getValue()<=new Decimal(0.0))
                        throw new ArgumentException("StopPrice must be > 0");
                    break;
                case OrdType.MARKET:// OrdemTipoEnum.Mercado:
                case OrdType.ON_CLOSE://OrdemTipoEnum.OnClose:
                    break;
                default:
                    if (nos.Price.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("Price must be > 0");
                    break;
            }
            
            if (nos.IsSetMaxFloor())
                if (nos.MaxFloor.getValue() < new Decimal(0))
                    throw new ArgumentException("MaxFloor must be >= 0");

            if (nos.IsSetMinQty())
                if (nos.MinQty.getValue() < new Decimal(0))
                    throw new ArgumentException("MinQty must be >= 0");
        }

        public static void ConsistirAlteracaoOrdem(OrderCancelReplaceRequest occr, string exchange)
        {
            // Basic order identification
            if (string.IsNullOrEmpty(occr.Account.ToString()))
                throw new ArgumentException("Account field must be provided");

            if (string.IsNullOrEmpty(occr.OrigClOrdID.ToString()))
            //if (ordem.OrigClOrdID == null || ordem.OrigClOrdID.Length <= 0)
                throw new ArgumentException("OrigClOrdID field must be provided");
            
            if (string.IsNullOrEmpty(occr.ClOrdID.ToString()))
                throw new ArgumentException("ClOrdID field must be provided");

            // Instrument Identification Block
            if (string.IsNullOrEmpty(occr.Symbol.ToString()))
                throw new ArgumentException("Symbol must be provided");

            // "obrigatorio" somente para bmf (nao obrigatorio na mensageria, mas dah erro se enviar 
            // sem este campo para bmf)
            //if (exchange.Equals(ExchangePrefixes.BMF, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    if (string.IsNullOrEmpty(occr.SecurityID.ToString()))
            //        throw new ArgumentException("SecurityID field must be provided");
            //}

            if (string.IsNullOrEmpty(occr.OrderQty.ToString()))
                throw new ArgumentException("OrderQty must be > 0 ");

            if (occr.IsSetField(Tags.NoPartyIDs))
            {
                try
                {
                    Group partyIDs = occr.GetGroup(1, Tags.NoPartyIDs);
                    if (string.IsNullOrEmpty(partyIDs.GetField(Tags.PartyID)))
                        throw new ArgumentException("ExecBroker must be provided");
                }
                catch
                {
                    throw new ArgumentException("ExecBroker must be provided");
                }
            }
            // Valido somente se estiver atribuido, campo nao obrigatorio
            if (occr.IsSetField(Tags.TimeInForce))
            {
                TimeInForce tif = occr.TimeInForce;
                if (tif == null)
                {
                    throw new ArgumentException("TimeInForce is invalid");
                }

                if (tif.ToString().Equals(TimeInForce.GOOD_TILL_DATE.ToString()))
                {
                    if (!string.IsNullOrEmpty(occr.ExpireDate.ToString()))
                    {
                        DateTime dtExpireDate = DateTime.ParseExact(occr.ExpireDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                        if (dtExpireDate.Date < DateTime.Now.Date)
                            throw new ArgumentException("ExpireDate is invalid");
                    }
                    else
                        throw new ArgumentException("ExpireDate is invalid");
                }
            }

            OrdType ordType = occr.OrdType;
            if (ordType == null)
            {
                throw new ArgumentException("OrdType is invalid");
            }

            // Verifica envio do preco
            switch (ordType.ToString()[0])
            {
                case OrdType.LIMIT:// OrdemTipoEnum.Limitada:
                case OrdType.MARKET_WITH_LEFTOVER_AS_LIMIT:// OrdemTipoEnum.MarketWithLeftOverLimit:
                    if (occr.Price.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("Price must be > 0");
                    break;
                case OrdType.STOP_LIMIT:// OrdemTipoEnum.StopLimitada:
                    //case OrdemTipoEnum.StopStart:
                    if (occr.Price.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("Price must be > 0");
                    if (occr.StopPx.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("StopPrice must be > 0");
                    break;
                case OrdType.MARKET:// OrdemTipoEnum.Mercado:
                case OrdType.ON_CLOSE://OrdemTipoEnum.OnClose:
                    break;
                default:
                    if (occr.Price.getValue() <= new Decimal(0.0))
                        throw new ArgumentException("Price must be > 0");
                    break;
            }

            if (occr.IsSetMaxFloor())
                if (occr.MaxFloor.getValue() < new Decimal(0))
                    throw new ArgumentException("MaxFloor must be >= 0");

            if (occr.IsSetMinQty())
                if (occr.MinQty.getValue() < new Decimal(0))
                    throw new ArgumentException("MinQty must be >= 0");
        }

        public static void ConsistirCancelamentoOrdem(OrderCancelRequest ocr, string exchange)
        {
            // Basic order identification
            if (string.IsNullOrEmpty(ocr.Account.ToString())) 
                throw new ArgumentException("Account field must be provided");

            if (string.IsNullOrEmpty(ocr.OrigClOrdID.ToString())) 
                throw new ArgumentException("OrigClOrdID field must be provided");

            if (string.IsNullOrEmpty(ocr.ClOrdID.ToString())) 
                throw new ArgumentException("ClOrdID field must be provided");

            // Instrument Identification Block
            if (string.IsNullOrEmpty(ocr.Symbol.ToString()))
                throw new ArgumentException("Symbol must be provided");

            // "obrigatorio" somente para bmf (nao obrigatorio na mensageria, mas dah erro se enviar 
            // sem este campo para bmf)
            //if (exchange.Equals(ExchangePrefixes.BMF, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    if (string.IsNullOrEmpty(ocr.SecurityID.ToString()))
            //        throw new ArgumentException("SecurityID field must be provided");
            //}

            //ATP - 2012-10-29
            //Qtde de contratos/papeis a serem cancelados
            if (ocr.OrderQty.getValue() <= new Decimal(0))// orderCancelInfo.OrderQty <= 0)
                throw new ArgumentException("OrderQty must be > 0 ");
            
            if (ocr.IsSetField(Tags.NoPartyIDs))
            {
                try
                {
                    Group partyIDs = ocr.GetGroup(1, Tags.NoPartyIDs);
                    if (string.IsNullOrEmpty(partyIDs.GetField(Tags.PartyID)))
                        throw new ArgumentException("ExecBroker must be provided");
                }
                catch
                {
                    throw new ArgumentException("ExecBroker must be provided");
                }
            }
        }


        public static void ConsistirNovaOrdemCross(NewOrderCross noc, string exchange)
        {

            string strAccount1 = string.Empty;
            
            if (!noc.IsSetCrossID())
                throw new ArgumentException("CrossID field must be provided");
            
            if (!noc.IsSetCrossType())
                throw new ArgumentException("CrossType field must be provided");
            if (!noc.IsSetCrossPrioritization())
                throw new ArgumentException("CrossPriorization field must be provided");

            if (noc.IsSetNoSides() && noc.NoSides.getValue() == 2)
            {
                QuickFix.Group grpNoSides = noc.GetGroup(1, Tags.NoSides);
                OrdensConsistencia.ProcessGroupNOC(grpNoSides);
                grpNoSides = noc.GetGroup(2, Tags.NoSides);
                OrdensConsistencia.ProcessGroupNOC(grpNoSides);
            }

            // Instrument Identification Block
            if (string.IsNullOrEmpty(noc.Symbol.ToString()))
                throw new ArgumentException("Symbol field must be provided");

            if (exchange.Equals(ExchangePrefixes.BMF, StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(noc.SecurityID.ToString()))
                    throw new ArgumentException("SecurityID field must be provided");
            }
           
            OrdType ordType = noc.OrdType;
            if (ordType == null)
            {
                throw new ArgumentException("OrdType is invalid");
            }

            // Verifica envio do preco
            bool aux = false;
            switch (ordType.ToString()[0])
            {
                case OrdType.LIMIT:// OrdemTipoEnum.Limitada:
                    break;
                default:
                    aux = true;
                    break;
            }
            
            if (aux)
                throw new ArgumentException("OrderType must be 2 - Limit");

            if (noc.IsSetPrice())
                if (noc.Price.getValue() <= new Decimal(0))
                    throw new ArgumentException("Must must be >= 0");

        }


        private static void ProcessGroupNOC(Group grp)
        {
            if (!grp.IsSetField(Tags.Side))
                throw new ArgumentException("Side field must be provided");
            if (!grp.IsSetField(Tags.ClOrdID))
                throw new ArgumentException("ClOrdID field must be provided");
            try
            {
                Group partyIDs = grp.GetGroup(1, Tags.NoPartyIDs);
                if (string.IsNullOrEmpty(partyIDs.GetField(Tags.PartyID)))
                    throw new ArgumentException("ExecBroker must be provided");
            }
            catch
            {
                throw new ArgumentException("ExecBroker must be provided");
            }

            if (!grp.IsSetField(Tags.Account))
                throw new ArgumentException("Account field must be provided");

            if (string.IsNullOrEmpty(grp.GetField(Tags.OrderQty)))
                throw new ArgumentException("OrderQty must be > 0 ");
        }
    }
}

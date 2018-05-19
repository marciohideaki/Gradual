using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;
using QuickFix44;

using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBMF
{
    public enum SubscriptionRequestTypeEnum
    {
        Snapshot,
        SnapshotPlusUpdates,
        DisableSnapshotPlusUpdates
    }

    public enum SecurityListRequestTypeEnum
    {
        ConformeCFIOuTipoInstrumento,
        ConformeProduto,
        TodosInstrumentos,
        ConformeAtivo
    }

    public enum SecurityTypeEnum
    {
        FUT,
        SPOT,
        SOPT,
        FOPT,
        DTERM
    }

    public enum ProductEnum
    {
        Mercadoria,
        Moeda,
        Indice,
        Outros,
        Financiamento,
        CreditoDeCarbono,
        Indicador
    }

    public static class CanalNegociacaoBMFMessageFactory
    {
        public static NewOrderSingle CreateNewOrderSingle(
                                        string clOrdID,
                                        char side,
                                        DateTime transactTime,
                                        char ordType,
                                        string securityID,
                                        string securityIDSource,
                                        string symbol,
                                        double price,
                                        double orderQty,
                                        List<PartyInfo> parties)
        {
            NewOrderSingle msg1 = new NewOrderSingle();
            msg1.set(new ClOrdID(clOrdID));
            msg1.set(new Side(side));
            msg1.set(new TransactTime(transactTime));
            msg1.set(new OrdType(ordType));
            msg1.set(new SecurityID(securityID));
            msg1.set(new SecurityIDSource(securityIDSource));
            msg1.set(new Symbol(symbol));
            msg1.set(new QuickFix.Price(price));
            msg1.set(new OrderQty(orderQty));

            // Adiciona as partes
            foreach (PartyInfo partyInfo in parties)
            {
                NewOrderSingle.NoPartyIDs party = new NewOrderSingle.NoPartyIDs();
                party.set(new PartyID(partyInfo.PartyID));
                party.set(new PartyRole(partyInfo.PartyRole));
                party.set(new PartyIDSource(partyInfo.PartyIDSource[0]));
                msg1.addGroup(party);
            }

            // Retorna
            return msg1;
        }

        public static OrderCancelReplaceRequest CreateOrderCancelReplaceRequest(
                                                    string origClOrdID,
                                                    string orderID,
                                                    string securityID,
                                                    string securityIDSource,
                                                    string symbol,
                                                    string clOrdID,
                                                    string secondaryOrderID,
                                                    string origSecondaryOrderID,
                                                    char side,
                                                    double orderQty,
                                                    double? minQty,
                                                    double? maxFloor,
                                                    char ordType,
                                                    double? price,
                                                    DateTime transactTime,
                                                    string partyID,
                                                    int partyRole,
                                                    char partyIDSource,
                                                    List<PartyInfo> parties)
        {
            OrderCancelReplaceRequest msg1 = new OrderCancelReplaceRequest();
            msg1.set(new OrigClOrdID(origClOrdID));
            if (orderID != null) msg1.set(new OrderID(orderID));
            msg1.set(new SecurityID(securityID));
            msg1.set(new SecurityIDSource(securityIDSource));
            msg1.set(new Symbol(symbol));
            msg1.set(new ClOrdID(clOrdID));
            if (secondaryOrderID != null) msg1.setString(198, secondaryOrderID);
            if (origSecondaryOrderID != null) msg1.setString(10033, origSecondaryOrderID);
            msg1.set(new Side(side));
            msg1.set(new OrderQty(orderQty));
            if (minQty.HasValue) msg1.set(new MinQty((double)minQty));
            if (maxFloor.HasValue) msg1.set(new MaxFloor((double)maxFloor));
            msg1.set(new OrdType(ordType));
            if (price.HasValue) msg1.set(new Price((double)price));
            msg1.set(new TransactTime(transactTime));

            // Adiciona as partes
            foreach (PartyInfo partyInfo in parties)
            {
                NewOrderSingle.NoPartyIDs party = new NewOrderSingle.NoPartyIDs();
                party.set(new PartyID(partyInfo.PartyID));
                party.set(new PartyRole(partyInfo.PartyRole));
                party.set(new PartyIDSource(partyInfo.PartyIDSource[0]));
                msg1.addGroup(party);
            }

            // Retorna
            return msg1;
        }

        public static OrderCancelRequest CreateOrderCancelRequest(
                                            string origClOrdID,
                                            string orderID,
                                            string symbol,
                                            string securityID,
                                            string securityIDSource,
                                            string clOrdID,
                                            string secondaryOrderID,
                                            char side,
                                            DateTime transactTime,
                                            string orderRestrictions,
                                            List<PartyInfo> parties)
        {
            OrderCancelRequest msg1 = new OrderCancelRequest();
            msg1.set(new OrigClOrdID(origClOrdID));
            if (orderID != null) msg1.set(new OrderID(orderID));
            msg1.set(new Symbol(symbol));
            msg1.set(new SecurityID(securityID));
            msg1.set(new SecurityIDSource(securityIDSource));
            msg1.set(new ClOrdID(clOrdID));
            if (secondaryOrderID != null) msg1.setField(198, secondaryOrderID);
            msg1.set(new Side(side));
            msg1.set(new TransactTime(transactTime));
            if (orderRestrictions != null) msg1.setField(529, orderRestrictions);

            // Adiciona as partes
            foreach (PartyInfo partyInfo in parties)
            {
                NewOrderSingle.NoPartyIDs party = new NewOrderSingle.NoPartyIDs();
                party.set(new PartyID(partyInfo.PartyID));
                party.set(new PartyRole(partyInfo.PartyRole));
                party.set(new PartyIDSource(partyInfo.PartyIDSource[0]));
                msg1.addGroup(party);
            }

            // Retorna
            return msg1;
        }

        public static NewOrderCross CreateNewOrderCross(
                                        string crossID,
                                        int crossType,
                                        int crossPriorization,
                                        char ordType,
                                        string symbol,
                                        string securityID,
                                        string securityIDSource,
                                        double price,
                                        DateTime transactTime,
                                        OrdemDiretoPontaInfo ponta1,
                                        OrdemDiretoPontaInfo ponta2)
        {

            NewOrderCross msg1 = new NewOrderCross();
            msg1.set(new CrossID(crossID));
            msg1.set(new CrossType(crossType));
            msg1.set(new CrossPrioritization(crossPriorization));
            msg1.set(new OrdType(ordType));
            msg1.set(new SecurityID(securityID));
            msg1.set(new SecurityIDSource(securityIDSource));
            msg1.set(new Symbol(symbol));
            msg1.set(new Price(price));
            msg1.set(new TransactTime(transactTime));

            // SIDE 1
            NewOrderCross.NoSides side1 = new NewOrderCross.NoSides();
            side1.set(new Side(CanalNegociacaoBMFTradutor.TraduzirSide(ponta1.Side)));
            side1.set(new ClOrdID(ponta1.ClOrdID));
            side1.set(new OrderQty(ponta1.OrderQty));
            if (ponta1.PositionEffect.HasValue) side1.set(new PositionEffect(ponta1.PositionEffect.Value));

            // SIDE 1 - ALLOC
            if (ponta1.AllocAccount != null)
            {
                NewOrderCross.NoSides.NoAllocs alloc1 = new NewOrderCross.NoSides.NoAllocs();
                alloc1.set(new AllocAccount(ponta1.AllocAccount));
                alloc1.set(new AllocAcctIDSource(ponta1.AllocAcctIDSource.Value));
                side1.addGroup(alloc1);
            }

            // SIDE 1 - PARTIES
            foreach (PartyInfo partyInfo in ponta1.Parties)
            {
                NewOrderCross.NoSides.NoPartyIDs party = new NewOrderCross.NoSides.NoPartyIDs();
                party.set(new PartyID(partyInfo.PartyID));
                party.set(new PartyRole(partyInfo.PartyRole));
                party.set(new PartyIDSource(partyInfo.PartyIDSource[0]));
                side1.addGroup(party);
            }
            msg1.addGroup(side1);

            // SIDE 2
            NewOrderCross.NoSides side2 = new NewOrderCross.NoSides();
            side2.set(new Side(CanalNegociacaoBMFTradutor.TraduzirSide(ponta2.Side)));
            side2.set(new ClOrdID(ponta2.ClOrdID));
            side2.set(new OrderQty(ponta2.OrderQty));
            if (ponta2.PositionEffect.HasValue) side2.set(new PositionEffect(ponta2.PositionEffect.Value));

            // SIDE 2 - ALLOC
            if (ponta2.AllocAccount != null)
            {
                NewOrderCross.NoSides.NoAllocs alloc2 = new NewOrderCross.NoSides.NoAllocs();
                alloc2.set(new AllocAccount(ponta2.AllocAccount));
                alloc2.set(new AllocAcctIDSource(ponta2.AllocAcctIDSource.Value));
                side2.addGroup(alloc2);
            }

            // SIDE 2 - PARTIES
            foreach (PartyInfo partyInfo in ponta2.Parties)
            {
                NewOrderCross.NoSides.NoPartyIDs party = new NewOrderCross.NoSides.NoPartyIDs();
                party.set(new PartyID(partyInfo.PartyID));
                party.set(new PartyRole(partyInfo.PartyRole));
                party.set(new PartyIDSource(partyInfo.PartyIDSource[0]));
                side2.addGroup(party);
            }
            msg1.addGroup(side2);

            // Retorno
            return msg1;

        }

        public static OrderMassStatusRequest CreateOrderMassStatusRequest(
                                                string massStatusReqID,
                                                int massStatusReqType,
                                                string partyID,
                                                int partyRole,
                                                char partyIDSource)
        {
            OrderMassStatusRequest msg1 =
                new OrderMassStatusRequest(
                    new MassStatusReqID(massStatusReqID),
                    new MassStatusReqType(massStatusReqType));

            OrderMassStatusRequest.NoPartyIDs parties = new OrderMassStatusRequest.NoPartyIDs();
            parties.set(new PartyID(partyID));
            parties.set(new PartyRole(partyRole));
            parties.set(new PartyIDSource(partyIDSource));
            msg1.addGroup(parties);

            return msg1;
        }

        public static MarketDataRequest CreateMarketDataRequest(
                                            string mdReqID,
                                            char subscriptionRequestType,
                                            string securityID,
                                            string symbol,
                                            char mdEntryType)
        {
            MarketDataRequest msg1 =
                new MarketDataRequest(
                    new MDReqID(mdReqID),
                    new SubscriptionRequestType(subscriptionRequestType),
                    new MarketDepth(0));

            msg1.set(new MDUpdateType(1));

            MarketDataRequest.NoMDEntryTypes types1 =
                new MarketDataRequest.NoMDEntryTypes();
            types1.set(new MDEntryType(mdEntryType));
            msg1.addGroup(types1);

            MarketDataRequest.NoRelatedSym sym1 =
                new MarketDataRequest.NoRelatedSym();
            sym1.set(new SecurityID(securityID));
            sym1.set(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL));
            sym1.set(new Symbol(symbol));
            msg1.addGroup(sym1);

            return msg1;
        }

        public static ResendRequest CreateResendRequest(int beginSeqNo, int endSeqNo)
        {
            ResendRequest msg1 = new ResendRequest();
            msg1.set(new BeginSeqNo(beginSeqNo));
            msg1.set(new EndSeqNo(endSeqNo));
            return msg1;
        }

        public static SecurityListRequest CreateSecurityListRequest(
            string securityReqID, SubscriptionRequestTypeEnum subscriptionRequestType, DateTime? securityUpdatesSince, 
            SecurityListRequestTypeEnum securityListRequestType, SecurityTypeEnum? securityType, ProductEnum? product, 
            string cfiCode, string asset)
        {
            // Auxiliares
            char charValue = ' ';
            int intValue = 0;
            string stringValue = null;
            
            // Cria mensagem
            SecurityListRequest msg1 = new SecurityListRequest();

            // securityReqID
            msg1.set(new SecurityReqID(securityReqID));

            // subscriptionRequestType
            switch (subscriptionRequestType)
            {
                case SubscriptionRequestTypeEnum.Snapshot:
                    charValue = '0';
                    break;
                case SubscriptionRequestTypeEnum.SnapshotPlusUpdates:
                    charValue = '1';
                    break;
                case SubscriptionRequestTypeEnum.DisableSnapshotPlusUpdates:
                    charValue = '3';
                    break;
            }
            msg1.set(new SubscriptionRequestType(charValue)); 

            // securityUpdatesSince
            if (securityUpdatesSince.HasValue)
                msg1.setUtcTimeStamp(6935, securityUpdatesSince.Value);

            // securityListRequestType
            switch (securityListRequestType)
            {
                case SecurityListRequestTypeEnum.ConformeCFIOuTipoInstrumento:
                    intValue = 1;
                    break;
                case SecurityListRequestTypeEnum.ConformeProduto:
                    intValue = 2;
                    break;
                case SecurityListRequestTypeEnum.TodosInstrumentos:
                    intValue = 4;
                    break;
                case SecurityListRequestTypeEnum.ConformeAtivo:
                    intValue = 5;
                    break;
            }
            msg1.set(new SecurityListRequestType(intValue));

            // securityType
            if (securityType.HasValue)
            {
                switch (securityType.Value)
                {
                    case SecurityTypeEnum.DTERM:
                        stringValue = "DTERM";
                        break;
                    case SecurityTypeEnum.FOPT:
                        stringValue = "FOPT";
                        break;
                    case SecurityTypeEnum.FUT:
                        stringValue = "FUT";
                        break;
                    case SecurityTypeEnum.SOPT:
                        stringValue = "SOPT";
                        break;
                    case SecurityTypeEnum.SPOT:
                        stringValue = "SPOT";
                        break;
                }
                msg1.set(new SecurityType(stringValue));
            }

            // product 
            if (product.HasValue)
            {
                switch (product.Value)
                {
                    case ProductEnum.CreditoDeCarbono:
                        intValue = 14;
                        break;
                    case ProductEnum.Financiamento:
                        intValue = 13;
                        break;
                    case ProductEnum.Indicador:
                        intValue = 15;
                        break;
                    case ProductEnum.Indice:
                        intValue = 7;
                        break;
                    case ProductEnum.Mercadoria:
                        intValue = 2;
                        break;
                    case ProductEnum.Moeda:
                        intValue = 4;
                        break;
                    case ProductEnum.Outros:
                        intValue = 12;
                        break;
                }
                msg1.set(new Product(intValue));
            }

            // cfiCode 
            if (cfiCode != null)
                msg1.set(new CFICode(cfiCode));

            // asset
            if (asset != null)
                msg1.setString(6937, asset);

            // Retorna
            return msg1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;
using QuickFix42;

using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.CanalBovespa
{
    public static class CanalNegociacaoBovespaMessageFactory
    {
        public static NewOrderSingle CreateNewOrderSingle(
                                        string account,
                                        char handInst,
                                        string execBroker,
                                        string symbol,
                                        char side,
                                        string clOrdID,
                                        double orderQty,
                                        double? minQty,
                                        double? maxFloor,
                                        char? ordType,
                                        double? price,
                                        double? stopPx,
                                        char? timeInForce,
                                        DateTime transactTime,
                                        DateTime? expireDate)
        {
            NewOrderSingle msg1 = new NewOrderSingle();
            msg1.set(new Account(account));
            msg1.set(new HandlInst(handInst));
            if (execBroker != null) msg1.set(new ExecBroker(execBroker));
            msg1.set(new Symbol(symbol));
            msg1.set(new Side(side));
            msg1.set(new ClOrdID(clOrdID));
            msg1.set(new OrderQty(orderQty));
            if (minQty.HasValue) msg1.set(new MinQty(minQty.Value));
            if (maxFloor.HasValue) msg1.set(new MaxFloor(maxFloor.Value));
            if (ordType.HasValue) msg1.set(new OrdType(ordType.Value));
            if (price.HasValue) msg1.set(new Price(price.Value));
            if (stopPx.HasValue) msg1.set(new StopPx(stopPx.Value));
            if (timeInForce.HasValue) msg1.set(new TimeInForce(timeInForce.Value));
            msg1.set(new TransactTime(transactTime));
            if (expireDate.HasValue) msg1.set(new ExpireDate(expireDate.Value.ToString("yyyyMMdd")));

            return msg1;
        }

        public static OrderCancelReplaceRequest CreateOrderCancelReplaceRequest(
                                                    string origClOrdID,
                                                    string orderID,
                                                    string clOrdID,
                                                    string execBroker,
                                                    char side,
                                                    string symbol,
                                                    double orderQty,
                                                    double? maxFloor,
                                                    char ordType,
                                                    double? price,
                                                    char? timeInForce,
                                                    DateTime transactTime)
        {
            OrderCancelReplaceRequest msg1 = new OrderCancelReplaceRequest();
            msg1.set(new OrigClOrdID(origClOrdID));
            if (orderID != null) msg1.set(new OrderID(orderID));
            msg1.set(new ClOrdID(clOrdID));
            msg1.set(new ExecBroker(execBroker));
            msg1.set(new Side(side));
            msg1.set(new Symbol(symbol));
            msg1.set(new OrderQty(orderQty));
            if (maxFloor.HasValue) msg1.set(new MaxFloor(maxFloor.Value));
            msg1.set(new OrdType(ordType));
            if (price.HasValue) msg1.set(new Price((double)price));
            if (timeInForce.HasValue) msg1.set(new TimeInForce(timeInForce.Value));
            msg1.set(new TransactTime(transactTime));

            // Retorna
            return msg1;
        }

        public static OrderCancelRequest CreateOrderCancelRequest(
                                            string account,
                                            string origClOrdID,
                                            string orderID,
                                            string clOrdID,
                                            string symbol,
                                            char side,
                                            double orderQty,
                                            string execBroker,
                                            DateTime transactTime)
        {
            OrderCancelRequest msg1 = new OrderCancelRequest();
            msg1.set(new Account(account));
            msg1.set(new OrigClOrdID(origClOrdID));
            if (orderID != null) msg1.set(new OrderID(orderID));
            msg1.set(new ClOrdID(clOrdID));
            msg1.set(new Symbol(symbol));
            msg1.set(new Side(side));
            msg1.set(new OrderQty(orderQty));
            if (execBroker != null) msg1.set(new ExecBroker(execBroker));
            msg1.set(new TransactTime(transactTime));

            // Retorna
            return msg1;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.Site.Www.Transporte
{
    public class Transporte_Ordens
    {
        public string Account { get; set; }
        
        public string ChannelID { get; set; }
        
        public string ClOrdID { get; set; }
        
        public string CompIDBolsa { get; set; }
        
        public string CompIDOMS { get; set; }
        
        public string CumQty { get; set; }
        
        public string Currency { get; set; }
        
        public string Exchange { get; set; }
        
        public string ExchangeNumberID { get; set; }
        
        public string ExecBroker { get; set; }
        
        public string ExecutingTrader { get; set; }
        
        public string ExpireDate { get; set; }
        
        public string FixMsgSeqNum { get; set; }
        
        public string ForeignFirm { get; set; }
        
        public string IdOrdem { get; set; }
        
        public string InvestorID { get; set; }
        
        public string MaxFloor { get; set; }
        
        public string Memo5149 { get; set; }
        
        public string MinQty { get; set; }
        
        public string OrderQty { get; set; }
        
        public string OrderQtyRemmaining { get; set; }
        
        public string OrdStatus { get; set; }
        
        public string OrdType { get; set; }
        
        public string OrigClOrdID { get; set; }
        
        public string PositionEffect { get; set; }
        
        public string Price { get; set; }
        
        public string RegisterTime { get; set; }

        public string Side { get; set; }
        
        public string StopPrice { get; set; }
        
        public string StopStartID { get; set; }
        
        public string Symbol { get; set; }
        
        public string TimeInForce { get; set; }
        
        public string TransactTime { get; set; }

        public string CssOrdensTR { get; set; } 

        public List<Transporte_Ordens> TraduzirList(List<OrdemInfo> pLista)
        {
            var lRetorno = new List<Transporte_Ordens>();

            var lTrans = new Transporte_Ordens();

            pLista.ForEach(ordem => {
                lTrans = new Transporte_Ordens();

                lTrans.Account      = ordem.Account.ToString();
                lTrans.ChannelID    = ordem.ChannelID.ToString();
                lTrans.ClOrdID      = ordem.ClOrdID.ToString();
                lTrans.CumQty       = ordem.CumQty.ToString();
                lTrans.OrderQty     = ordem.OrderQty.ToString();
                lTrans.OrdStatus    = ordem.OrdStatus.ToString();
                lTrans.Price        = ordem.Price.ToString("N2");
                lTrans.StopPrice    = ordem.StopPrice.ToString("N2");
                lTrans.Symbol       = ordem.Symbol.ToString();
                lTrans.Side         = ordem.Side.ToString();
                lTrans.RegisterTime = ordem.RegisterTime.ToString("dd/MM/yyyy HH:mm:ss");
                lTrans.TransactTime = ordem.TransactTime.ToString("dd/MM/yyyy HH:mm:ss");
                lTrans.ExpireDate   = ordem.ExpireDate.HasValue ? ordem.ExpireDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                lTrans.CssOrdensTR  = RetornaCorTr(ordem.OrdStatus.ToString());

                lRetorno.Add(lTrans);
            });

            return lRetorno;
        }

        public string RetornaCorTr(string pOrderStatus)
        {
            string lRetorno = string.Empty;

            switch(pOrderStatus.ToUpper())
            {
                case "NOVA":
                    lRetorno = "tr_verdeclaro";
                    break;

                case "PARCIALMENTEEXECUTADA":
                    lRetorno = "tr_verdeclaro";
                    break;

                case "EXECUTADA":
                    lRetorno = "tr_verdeescuro";
                    break;

                case "CANCELADA":
                    lRetorno = "tr_vermelhoclaro";
                    break;

                case "SUBSTITUIDA":
                    lRetorno = "tr_verdeclaro";
                    break;

                case "REJEITADA":
                    lRetorno = "tr_vermelhoclaro";
                    break;

                case "EXPIRADA":
                    lRetorno = "tr_vermelhoclaro";
                    break;
                default:
                    lRetorno = "tr_cinzaclaro";
                    break;
            }

            return lRetorno;
        }
    }
}
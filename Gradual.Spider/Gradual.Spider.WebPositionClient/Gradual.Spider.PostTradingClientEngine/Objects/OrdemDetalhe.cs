using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.Spider.PostTradingClientEngine.Objects
{
    public class Detalhe
    {
        public System.String OrderDetailID { get; set; }
        public System.String TransactID { get; set; }
        public System.String OrderID { get; set; }
        public System.String OrderQty { get; set; }
        public System.String OrdQtyRemaining { get; set; }
        public System.String Price { get; set; }
        public System.String StopPx { get; set; }
        public System.String OrderStatusID { get; set; }
        public System.String Status { get; set; }
        public System.String TransactTime { get; set; }
        public System.String Description { get; set; }
        public System.String TradeQty { get; set; }
        public System.String CumQty { get; set; }
        public System.String Saldo { get; set; }
        public System.String FixMsgSeqNum { get; set; }
        public System.String CxlRejResponseTo { get; set; }
        public System.String CxlRejReason { get; set; }
        public System.String MsgFixDetail { get; set; }
        public System.String Contrabroker { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Lib
{
     [Serializable]
    public class ClientPortifolio
    {
        public int Account { set; get; }
        public string Instrument { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public decimal Volume { set; get; }
    }
}

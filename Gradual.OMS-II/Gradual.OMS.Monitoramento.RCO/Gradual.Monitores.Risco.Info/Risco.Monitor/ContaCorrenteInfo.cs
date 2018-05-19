using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    [Serializable]
    public class ContaCorrenteInfo
    {
        [DataMember]
        public decimal SaldoD0 { set; get; }

        /// <summary>
        /// Saldo do cliente D1
        /// </summary>    
        [DataMember]
        public decimal SaldoD1 { set; get; }

        /// <summary>
        /// Saldo do cliente em D2
        /// </summary>     
        [DataMember]
        public decimal SaldoD2 { set; get; }

        /// <summary>
        /// Saldo do cliente em D3
        /// </summary>        
        [DataMember]
        public decimal SaldoD3 { set; get; }

        /// <summary>
        /// Saldo em conta margem do cliente
        /// </summary>       
        [DataMember]
        public Nullable<decimal> SaldoContaMargem { set; get; }
    }
}

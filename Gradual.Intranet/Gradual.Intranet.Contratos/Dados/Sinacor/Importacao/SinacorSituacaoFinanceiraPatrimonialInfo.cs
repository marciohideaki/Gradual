using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorSituacaoFinanceiraPatrimonialInfo : ICodigoEntidade
    {
  
        public Nullable<int> CD_SFPGRUPO { get; set; }

        public Nullable<int> CD_SFPSUBGRUPO { get; set; }
   
        public Nullable<decimal> PC_LIMITE { get; set; }

        public string DS_BEN { get; set; }
     
        public Nullable<decimal> VL_BEN { get; set; }

        public Nullable<char> IN_ONUS { get; set; }
   
        public Nullable<decimal> VL_DEVEDOR { get; set; }
    
        public Nullable<DateTime> DT_VENCIMENTO { get; set; }
 
        public string SG_ESTADO { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

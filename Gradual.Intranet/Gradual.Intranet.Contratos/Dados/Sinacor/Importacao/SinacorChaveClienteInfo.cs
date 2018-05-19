using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;
using System.Collections;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorChaveClienteInfo : ICodigoEntidade
    {
       
        public Int64 CD_CPFCGC { get; set; }
     
        public DateTime DT_NASC_FUND { get; set; }
      
        public int CD_CON_DEP { get; set; }        

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

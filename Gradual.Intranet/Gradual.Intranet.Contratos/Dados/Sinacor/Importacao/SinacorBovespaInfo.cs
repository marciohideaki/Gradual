using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorBovespaInfo : ICodigoEntidade
    {
   
        public Nullable<int> CD_ASSESSOR { get; set; }
     
        public Nullable<char> IN_SITUAC { get; set; }
  
        public Nullable<char> IN_CONTA_INV { get; set; }
   
        public Nullable<int> CD_CLIENTE { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

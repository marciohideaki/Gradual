using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorBmfInfo : ICodigoEntidade
    {
       
        public Nullable<int> CODASS { get; set; }
 
        public Nullable<char> STATUS { get; set; }
 
        public Nullable<char> IN_CONTA_INV { get; set; }
   
        public Nullable<int> CODCLI { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

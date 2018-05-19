using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorEnderecoInfo : ICodigoEntidade
    {
    
        public string NM_BAIRRO { get; set; }

        public Nullable<int> CD_CEP { get; set; }
 
        public Nullable<int> CD_CEP_EXT { get; set; }
 
        public string NM_CIDADE { get; set; }
     
        public string NM_COMP_ENDE { get; set; }
     
        public Nullable<char> IN_ENDE_CORR { get; set; }
      
        public string NM_LOGRADOURO { get; set; }
   
        public string NR_PREDIO { get; set; }
     
        public string SG_PAIS { get; set; }
      
        public Nullable<char> IN_TIPO_ENDE { get; set; }
      
        public string SG_ESTADO { get; set; }

        public string DS_EMAIL { get; set; }

        public string DS_EMAIL_COMERCIAL { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

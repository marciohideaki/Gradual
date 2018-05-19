using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorTelefoneInfo : ICodigoEntidade
    {
      
        public Nullable<int> CD_DDD_TEL { get; set; }
      
        public Nullable<char> IN_ENDE_OFICIAL { get; set; }
  
        public Nullable<int> NR_RAMAL { get; set; }
    
        public Nullable<Int64> NR_TELEFONE { get; set; }
      
        public Nullable<char> IN_TIPO_ENDE { get; set; }
     
        public Nullable<int> CD_DDD_CELULAR1 { get; set; }

        public Nullable<Int64> NR_CELULAR1 { get; set; }
     
        public Nullable<int> CD_DDD_CELULAR2 { get; set; }
      
        public Nullable<Int64> NR_CELULAR2 { get; set; }

        public Nullable<int> CD_DDD_FAX { get; set; }

        public Nullable<Int64> NR_FAX { get; set; }

        public Nullable<int> CD_DDD_FAX2 { get; set; }

        public Nullable<Int64> NR_FAX2 { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

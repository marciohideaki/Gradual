using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorEmitenteInfo : ICodigoEntidade
    {

        public string NM_EMIT_ORDEM { get; set; }
  
        public string CD_DOC_IDENT_EMIT { get; set; }

        public Nullable<Int64> CD_CPFCGC_EMIT { get; set; }

        public Nullable<char> IN_PRINCIPAL { get; set; }
  
        public string CD_SISTEMA { get; set; }

        public string NM_E_MAIL { get; set; }

        public Nullable<DateTime> TM_STAMP { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

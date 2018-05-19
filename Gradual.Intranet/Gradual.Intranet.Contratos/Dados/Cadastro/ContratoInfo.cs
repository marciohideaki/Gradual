using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ContratoInfo : ICodigoEntidade
    {     
        public Nullable<int> IdContrato { get; set; }

        public string DsContrato { get; set; }

        public string DsPath { get; set; }

        public Boolean StObrigatorio { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class TipoTelefoneInfo : ICodigoEntidade
    {
        #region Propriedades

        public Nullable<int> IdTipoTelefone { get; set; }

        public string DsTelefone { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

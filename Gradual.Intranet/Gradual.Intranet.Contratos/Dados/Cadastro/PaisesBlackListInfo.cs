using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PaisesBlackListInfo : ICodigoEntidade
    {
        public Nullable<int> IdPaisBlackList { get; set; }
        public string CdPais { get; set; }
        public string DsNomePais { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PrimeiroAcessoValidaCpfInfo : ICodigoEntidade
    {

        public string DsCpfCnpj { get; set; }
        public int CdCodigo { get; set; }
        public int IdLogin { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

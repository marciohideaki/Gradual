using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PrimeiroAcessoValidaInfo : ICodigoEntidade
    {

        public string DsEmail { get; set; }
        public int CdCodigo { get; set; }
        public Boolean StPrimeiroAcesso { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

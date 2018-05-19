using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteAtivarInativarContasInfo : ICodigoEntidade
    {

        public Int32 CdCodigo { get; set; }

        public ClienteContaInfo Bovespa { get; set; }

        public ClienteContaInfo Bmf { get; set; }

        public ClienteContaInfo CC { get; set; }

        public ClienteContaInfo Custodia { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    
    }
}

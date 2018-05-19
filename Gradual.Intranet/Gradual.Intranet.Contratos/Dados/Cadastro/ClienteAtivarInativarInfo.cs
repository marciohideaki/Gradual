using System;
using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteAtivarInativarInfo : ICodigoEntidade
    {

        public int IdCliente { get; set; }

        public Boolean StClienteGeralAtivo { get; set; }

        public Boolean StLoginAtivo { get; set; }

        public Boolean StHbAtivo { get; set; }

        public DateTime DtUltimaAtualizacao { get; set; }

        public List<ClienteAtivarInativarContasInfo> Contas { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

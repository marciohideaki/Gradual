using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClientePassoContaInfo : ICodigoEntidade
    {
        #region ICodigoEntidade Members

        public int IdCliente { get; set; }

        public int StPasso { get; set; }

        public int IdClienteConta { get; set; }

        public int CdAssessor { get; set; }

        public eAtividade CdSistema { get; set; }

        public Boolean StPrincipal { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

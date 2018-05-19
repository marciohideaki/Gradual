#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
#endregion

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteAtivoInativoInfo :  ICodigoEntidade
    {

        #region Members

        public bool St_Ativo { get; set; }

        public int IdCliente { get; set; }

        public string DsNomeCliente { get; set; }

        public Nullable<DateTime> DtAtivacaoInativacao { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

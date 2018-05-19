using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteAlteracaoInfo : ICodigoEntidade
    {

        public Nullable<int> IdAlteracao { get; set; }
        public int IdCliente { get; set; }
        public char CdTipo { get; set; }
        public string DsInformacao { get; set; }
        public string DsDescricao { get; set; }
        public DateTime DtSolicitacao { get; set; }
        public Nullable<DateTime> DtRealizacao { get; set; }
        
        public Nullable<int> IdLoginRealizacao { get; set; }
        public Nullable<int> IdLoginSolicitante { get; set; }

        public string DsLoginRealizacao { get; set; }
        public string DsLoginSolicitante { get; set; }
        

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

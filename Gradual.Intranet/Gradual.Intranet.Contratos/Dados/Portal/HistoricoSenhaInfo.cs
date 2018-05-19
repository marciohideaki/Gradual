using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Portal
{
    public class HistoricoSenhaInfo : ICodigoEntidade
    {
        public int IdHistoricoSenha { get; set; }

        public int IdLogin { get; set; }

        public string CdSenha { get; set; }

        public DateTime DtAlteracao { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Contratos.Dados
{
    public class LoginInfo : ICodigoEntidade
    {
        public Nullable<int> IdLogin { get; set; }

        public string CdSenha { get; set; }

        public string CdAssinaturaEletronica { get; set; }

        public DateTime DtUltimaAlteracaosSenha { get; set; }

        public DateTime DtUltimaAlteracaoAssinaturaEletronica { get; set; }

        public int NrTentativasErradas { get; set; }

        public Nullable<int> IdFrase { get; set; }

        public string DsRespostaFrase { get; set; }

        public DateTime DtUltimaExpiracao { get; set; }

        public string DsEmail { get; set; }

        public eTipoAcesso TpAcesso { get; set; }

        public Nullable<int> CdAssessor { get; set; }

        public string DsNome { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

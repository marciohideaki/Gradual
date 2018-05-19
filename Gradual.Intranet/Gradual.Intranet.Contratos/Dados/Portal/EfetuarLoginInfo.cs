using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class EfetuarLoginInfo : ICodigoEntidade
    {
        public string DsEmail { get; set; }

        public int CdCodigo { get; set; }

        public string CdSenha { get; set; }

        public int IdCliente { get; set; }

        public int IdLogin { get; set; }

        public int NrTentativasErradas { get; set; }

        public string DsNome { get; set; }

        public string DsEmailRetorno { get; set; }

        public string DsCpfCnpj { get; set; }

        public int CdCodigoPrincipal { get; set; }

        public int CdAssessorPrincipal { get; set; }

        public int StPasso { get; set; }

        public DateTime DtNascimentoFundacao { get; set; }

        public DateTime DtPasso1 { get; set; }

        public string TpPessoa { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

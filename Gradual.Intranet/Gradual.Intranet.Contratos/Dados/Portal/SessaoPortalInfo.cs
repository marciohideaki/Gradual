using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SessaoPortalInfo : ICodigoEntidade
    {

        public int IdCliente { get; set; }
        public int IdLogin { get; set; }
        public string DsNome { get; set; }
        public string DsEmailRetorno { get; set; }
        public string DsCpfCnpj { get; set; }
        public int CdCodigoPrincipal { get; set; }
        public int CdAssessorPrincipal { get; set; }
        public int StPasso { get; set; }
        public DateTime DtNascimentoFundacao { get; set; }
        public string TpPessoa { get; set; }
        public DateTime DtPasso1 { get; set; }
        public DateTime DtUltimoLogin { get; set; }
        public string CdSenha { get; set; }
        public Int32 CodigoTipoOperacaoCliente { get; set; }
        public bool NovoHB { get; set; }
        public bool AlterarAssinatura { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

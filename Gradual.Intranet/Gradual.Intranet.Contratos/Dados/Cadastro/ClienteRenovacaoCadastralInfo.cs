using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteRenovacaoCadastralInfo : ClienteInfo, ICodigoEntidade
    {
        public DateTime? DtPesquisa { get; set; }

        public DateTime? DtPesquisaFim { get; set; }

        public string DsTelefone { get; set; }

        public DateTime DtRenovacao { get; set; }

        public string CodigoBovespa { get; set; }

        public string CdAssessor { get; set; }

        public string TipoPessoa { get; set; }

        public string Email { get; set; }

        public string DsDesejaAplicar { get; set; }
    }
}

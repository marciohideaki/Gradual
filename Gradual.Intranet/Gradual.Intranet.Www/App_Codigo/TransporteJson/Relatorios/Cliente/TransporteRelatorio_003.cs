using System;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Solicitação de alteração cadastral"
    /// </summary>
    public class TransporteRelatorio_003
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Bovespa { get; set; }

        public string CpfCnpj { get; set; }

        public string Assessor { get; set; }

        public string DataDaSolicitacao { get; set; }

        public string DataDaResolucao { get; set; }

        public string TipoDeSolicitacao { get; set; }

        public string Campo { get; set; }

        public string TpPessoa { get; set; }

        private string GetTipo(string pTipo)
        {
            if (pTipo == "I") return "Inclusão";
            else if (pTipo == "A") return "Inclusão";
            else if (pTipo == "E") return "Exclusão";
            else return pTipo;
        }

        public TransporteRelatorio_003() { }

        public TransporteRelatorio_003(ClienteSolicitacaoAlteracaoCadastralInfo pInfo)
        {
            this.Id                = pInfo.IdCliente;
            this.Nome              = pInfo.DsNomeCliente.ToStringFormatoNome();
            this.CpfCnpj           = pInfo.DsCpfCnpj.DBToInt64(true).ToCpfCnpjString();
            this.DataDaSolicitacao = pInfo.DtSolicitacao.ToString("dd/MM/yyyy");
            this.Assessor          = pInfo.CodigoAssessor.DBToString();
            this.TipoDeSolicitacao = GetTipo(pInfo.DsTipo);
            this.Campo             = pInfo.DsInformacao;
            this.TpPessoa          = pInfo.TipoPessoa.Equals("J") ? "Júridica" : "Física";
            this.Bovespa           = pInfo.CodigoBolsa.DBToString();
            this.DataDaResolucao   = DateTime.MinValue.Equals(pInfo.DtResolucao) ? "" : pInfo.DtResolucao.ToString("dd/MM/yyyy");
        }
    }
}

using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Clientes Cadastrados por Período"
    /// </summary>
    public class TransporteRelatorio_001
    {
        #region Members

        public int Id { get; set; }

        public string TipoDePessoa { get; set; }

        public string Nome { get; set; }

        public string CpfCnpj { get; set; }

        public string DataDeCadastro { get; set; }

        public string DataUltAtualizacao { get; set; }

        public string PassoAtual { get; set; }

        public string Bovespa { get; set; }

        public string BMF { get; set; }

        public string Assessor { get; set; }

        public string Exportado { get; set; }

        public string Telefones { get; set; }

        public string DsEmail { get; set; }

        /// <summary>
        /// Deseja operar em 
        /// </summary>
        public string DsDesejaOperarEm { get; set; }

        /// <summary>
        /// Codigo do tipo de operação
        /// </summary>
        public string CodigoTipoOperacao { get; set; }

        #endregion

        #region Construtor

        public TransporteRelatorio_001() { }

        public TransporteRelatorio_001(ClienteCadastradoPeriodoInfo pInfo) 
        {
            this.Id                 = pInfo.IdCliente.HasValue ? pInfo.IdCliente.Value : 0;
            this.TipoDePessoa       = "J".Equals(pInfo.TipoPessoa.Trim().ToUpper()) ? "Jurídica" : "Física";
            this.Nome               = pInfo.DsNomeCliente;
            this.CpfCnpj            = pInfo.DsCpfCnpj.DBToInt64(true).ToCpfCnpjString();
            this.DataDeCadastro     = pInfo.DtCadastro.ToString("dd/MM/yyyy");
            if (pInfo.DtUltimaAtualizacao.HasValue)
            { 
                this.DataUltAtualizacao = pInfo.DtUltimaAtualizacao.Value.ToString("dd/MM/yyyy HH:mm");
            }
            this.PassoAtual         = pInfo.PassoAtual;
            this.Bovespa            = (pInfo.CodigoBovespa.HasValue && !pInfo.CodigoBovespa.Equals(0)) ? pInfo.CodigoBovespa.DBToString() : "-";
            this.BMF                = (pInfo.CodigoBmf.HasValue && !pInfo.CodigoBmf.Equals(0))  ?  pInfo.CodigoBmf.Value.DBToString() : "-";
            this.Assessor           = pInfo.CodigoAssessor.DBToString();
            this.Exportado          = pInfo.BlnExportado ? "Sim" : "Não";
            this.Telefones          = string.Format("({0}) {1}", pInfo.DsDDD, pInfo.DsTelefone);
            this.DsEmail            = pInfo.DsEmail;
            this.DsDesejaOperarEm   = pInfo.DsDesejaOperarEm;
            this.CodigoTipoOperacao = pInfo.CodigoTipoOperacao;

        }

        #endregion
    }
}

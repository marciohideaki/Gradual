using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Clientes em período de renovação cadastral"
    /// </summary>
    public class TransporteRelatorio_004
    {
        #region Propriedades

        public int Id { get; set; }

        public string Nome { get; set; }

        public string CpfCnpj { get; set; }

        public string Telefone { get; set; }

        public string DataDeRenovacao { get; set; }

        public string CodigoBovespa { get; set; }

        public string Assessor { get; set; }

        public string TipoPessoa { get; set; }

        public string Email { get; set; }

        #endregion

        #region Constructors
        public TransporteRelatorio_004() { }

        public TransporteRelatorio_004(ClienteRenovacaoCadastralInfo pInfo)
        { 
            this.Nome            = pInfo.DsNome;
            this.CpfCnpj         = pInfo.DsCpfCnpj.ToCpfCnpjString();
            this.Telefone        = pInfo.DsTelefone.ToTelefoneString();
            this.Id              = pInfo.IdCliente.DBToInt32();
            this.DataDeRenovacao = pInfo.DtRenovacao.ToString("dd/MM/yyyy");
            this.CodigoBovespa   = pInfo.CodigoBovespa;
            this.Assessor        = pInfo.CdAssessor;
            this.TipoPessoa      = 'J'.Equals(pInfo.TpPessoa) ? "Jurídica" : "Física";
            this.Email           = pInfo.Email.ToString();
        }
        #endregion
    }
}

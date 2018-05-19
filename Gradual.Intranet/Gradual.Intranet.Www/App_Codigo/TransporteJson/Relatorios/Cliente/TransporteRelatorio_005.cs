using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Clientes exportados para o Sinacor"
    /// </summary>
    public class TransporteRelatorio_005
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public string CpfCnpj { get; set; }

        public string TipoDePessoa { get; set; }

        public string DataDeCadastro { get; set; }

        public string CodigoDeBolsa { get; set; }

        public string Assessor { get; set; }

        public string Telefones { get; set; }

        public string PrimeiraExportacao { get; set; }

        public string UltimaExportacao { get; set; }


        public TransporteRelatorio_005() { }

        public TransporteRelatorio_005(ClientesExportadosSinacorInfo pInfo)
        {
            this.Id = pInfo.IdCliente;
            this.Nome = pInfo.DsNomeCliente;
            this.CpfCnpj = pInfo.DsCpfCnpj.ToCpfCnpjString();
            this.TipoDePessoa = pInfo.TipoPessoa=="F"?"Física":"Jurídica";
            this.DataDeCadastro = pInfo.DtCadastro.ToString("dd/MM/yyyy");
            this.CodigoDeBolsa = pInfo.CodigoBovespa.DBToString();
            this.Assessor = pInfo.CodigoAssessor.DBToString();
            this.UltimaExportacao = pInfo.DtUltimaExportacao.ToString("dd/MM/yyyy");
            this.PrimeiraExportacao = pInfo.DtPrimeiraExportacao.ToString("dd/MM/yyyy");
            this.Telefones = string.IsNullOrWhiteSpace(pInfo.DsTelefone) ? "-" : string.Format("({0}) {1}", pInfo.DsDDD, pInfo.DsTelefone);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Fundos
{
    public class IntegracaoFundosInfo
    {
        public decimal CDI { get; set; }

        public DateTime DataInicio { get; set; }

        public Nullable<DateTime> DataFim { get; set; }

        public string NomeArquivoProspecto { get; set; }

        public string NomeArquivoRegulamento { get; set; }

        public string NomeArquivoTermo { get; set; }

        public string NomeArquivoLamina { get; set; }

        public string NomeArquivoDemonstracaoFin { get; set; }

        public string CpfCnpj { get; set; }

        public string IdCodigoAnbima { get; set; }

        public string CodigoFundoItau { get; set; }

        public int IdProduto { get; set; }

        public string NomeProduto { get; set; }

        public string Risco { get; set; }

        public string HorarioLimite { get; set; }

        public IntegracaoFundosCategoriaInfo Categoria { get; set; }

        public IntegracaoFundosMovimentoInfo DadosMovimentacao { get; set; }

        public IntegracaoFundosIndexadorInfo Indexador { get; set; }

        public IntegracaoFundosRentabilidadeInfo Rentabilidade { get; set; }

        public string StApareceSite { get; set; }

        public IntegracaoFundosInfo()
        {
            this.Indexador = new IntegracaoFundosIndexadorInfo();

            this.Categoria = new IntegracaoFundosCategoriaInfo();

            this.DadosMovimentacao = new IntegracaoFundosMovimentoInfo();

            this.NomeProduto = string.Empty;

            this.Risco = string.Empty;

            this.HorarioLimite = string.Empty;

            this.NomeArquivoProspecto = string.Empty;

            this.NomeArquivoRegulamento = string.Empty;

            this.NomeArquivoTermo = string.Empty;

            this.NomeArquivoLamina = string.Empty;

            this.NomeArquivoDemonstracaoFin = string.Empty;

            this.StApareceSite = string.Empty;
        }
    }
}

using System;

namespace Gradual.Intranet.Contratos.Mensagens
{
    public class CadastrarClienteRequest : BaseRequest
    {
        #region Propriedades

        public Nullable<int> IdCliente { get; set; }

        public int IdLogin { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

        public decimal CpfCnpj { get; set; }

        public DateTime DataPasso1 { get; set; }

        public Nullable<DateTime> DataPasso2 { get; set; }

        public Nullable<DateTime> DataPasso3 { get; set; }

        public Nullable<DateTime> DataPrimeiraExportacao { get; set; }

        public Nullable<DateTime> DataUltimaExportacao { get; set; }

        public string OrigemCadastro { get; set; }

        public char TipoPessoa { get; set; }

        public int TipoCliente { get; set; }

        public int Passo { get; set; }

        public Nullable<char> Sexo { get; set; }

        public Nullable<int> Nacionalidade { get; set; }

        public string PaisNascimento { get; set; }

        public string UfNascimento { get; set; }

        public string UfNascimentoEstrangeiro { get; set; }

        public Nullable<int> EstadoCivil { get; set; }

        public string Conjuge { get; set; }

        public string TipoDocumento { get; set; }

        public Nullable<DateTime> DataNascimento { get; set; }

        public string OrgaoEmissorDocumento { get; set; }

        public string UfEmissorDocumento { get; set; }

        public Nullable<int> Profissao { get; set; }

        public string Cargo { get; set; }

        public string Empresa { get; set; }

        public Nullable<char> PessoaVinculada { get; set; }

        public Nullable<char> PPE { get; set; }

        public Nullable<char> CarteiraPropria { get; set; }

        public Nullable<char> CVM387 { get; set; }

        public Nullable<char> Emancipado { get; set; }

        public Nullable<int> idAssessorInicial { get; set; }

        public Nullable<int> Escolaridade { get; set; }

        public char CadastroPortal { get; set; }

        #endregion
    }
}

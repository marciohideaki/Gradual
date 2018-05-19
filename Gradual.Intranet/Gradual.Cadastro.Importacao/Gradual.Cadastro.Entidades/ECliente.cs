using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Geral;

namespace Gradual.Cadastro.Entidades
{
    public class ECliente
    {
        public System.Nullable<int> ID_Cliente { get; set; }
        public System.Nullable<int> ID_Login { get; set; }
        public System.Nullable<int> ID_AssessorFilial { get; set; }
        public System.Nullable<int> LoginCadastrante { get; set; }
        public System.Nullable<System.DateTime> DataCadastroInicial { get; set; }
        public System.Nullable<char> Liberado { get; set; }
        public System.Nullable<System.DateTime> DataAprovacaoFinal { get; set; }
        public System.Nullable<System.DateTime> DataProximaAtualizacao { get; set; }
        public string CPF { get; set; }
        public System.Nullable<char> Sexo { get; set; }
        public System.Nullable<int> Conheceu { get; set; }
        public string ConheceuOutros { get; set; }
        public System.Nullable<int> Passo { get; set; }
        public System.Nullable<int> Nacionalidade { get; set; }
        public string UFNascimento { get; set; }
        public string Naturalidade { get; set; }
        public System.Nullable<int> EstadoCivil { get; set; }
        public string Conjugue { get; set; }
        public System.Nullable<System.DateTime> DataNascimento { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string TipoDocumento { get; set; }
        public string OrgaoEmissorDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public System.Nullable<System.DateTime> DataEmissaoDocumento { get; set; }
        public string EstadoEmissaoDocumento { get; set; }
        public string Empresa { get; set; }
        public string EmailComercial { get; set; }
        public System.Nullable<int> Profissao { get; set; }
        public System.Nullable<decimal> Salario { get; set; }
        public string SalarioString { get { return  Conversao.ToDecimalOracle(Salario); } }
        public string OutrosRendimentosDescricao { get; set; }
        public System.Nullable<decimal> OutrosRendimentosValor { get; set; }
        public string OutrosRendimentosValorString { get { return Conversao.ToDecimalOracle(this.OutrosRendimentosValor); } }
        public System.Nullable<char> Representante { get; set; }
        public System.Nullable<char> PessoaVinculada { get; set; }
        public System.Nullable<char> PPE { get; set; }
        public System.Nullable<char> CarteiraPropria { get; set; }
        public System.Nullable<char> CVM387 { get; set; }
        public string CodigoBovespa { get; set; }
        public System.Nullable<int> CodigoBMF { get; set; }
        public string Emancipado { get; set; }
        public string PaisNascimento { get; set; }
        public string UFNascimentoEstrangeiro { get; set; }
        public string Cargo { get; set; }
          public Nullable<int> Tipo { get; set; }
        public System.Nullable<char> AutorizaTerceiro { get; set; }
    }
}

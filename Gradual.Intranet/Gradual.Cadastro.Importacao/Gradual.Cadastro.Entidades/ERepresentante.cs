using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Cadastro.Entidades
{
    public class ERepresentante
    {     
        public System.Nullable<int> ID_Representante { get; set; }
        public int ID_Cliente { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public System.Nullable<DateTime> DataNascimento { get; set; }
        public Nullable<char> Sexo { get; set; }
        public System.Nullable<int> Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
        public string UFNascimento { get; set; }
        public System.Nullable<int> EstadoCivil { get; set; }
        public string Conjugue { get; set; }
        public System.Nullable<int> Profissao { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; } 
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string DDDTelefone { get; set; }
        public string Telefone { get; set; }
        public string DDDCelular { get; set; }
        public string Celular { get; set; }
        public string TipoDocumento { get; set; }
        public string OrgaoEmissorDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public Nullable<System.DateTime> DataEmissaoDocumento { get; set; }
        public string UFEmissaoDocumento { get; set; }
        public string PaisNascimento { get; set; }
        public string UFNascimentoEstrangeiro { get; set; }
        public System.Nullable<int> SituacaoLegal { get; set; }
    }
}

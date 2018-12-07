using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosClienteFinancialInfo
    {
        public int CodigoAnbima { get; set; }

        public int CodigoCliente { get; set; }

        public int CodigoAssessor { get; set; }

        public string NomeCliente { get; set; }

        public string StAtivo { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string DsCpfCnpj { get; set; }

        public string TipoPessoa { get; set; }

        public string CodigoCotistaItau { get; set; }

        //parametros usados para financial
        public string TipoCliente { get; set; }

        public int StResidentePais { get; set; }

        public int TipoCotistaCvm { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string CEP { get; set; }

        public string UF { get; set; }

        public string Pais { get; set; }

        public string EnderecoCom { get; set; }

        public string NumeroCom { get; set; }

        public string ComplementoCom { get; set; }

        public string BairroCom { get; set; }

        public string CidadeCom { get; set; }

        public string CEPCom { get; set; }

        public string UFCom { get; set; }

        public string PaisCom { get; set; }

        public string Fone { get; set; }

        public string EmailCom { get; set; }

        public string FoneCom { get; set; }

        public int EstadoCivil { get; set; }

        public string NumeroRG { get; set; }

        public string EmissorRG { get; set; }

        public DateTime DataEmissaoRG { get; set; }

        public string Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Profissao { get; set; }

        public string PaisNascimento { get; set; }
    }
}

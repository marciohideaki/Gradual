using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_013
    {
        public string CPFCNPJ { get; set; }

        public string NomeCliente { get; set; }

        public string NomeAssessor { get; set; }

        public string UF { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string Logradouro { get; set; }

        public string Complemento { get; set; }

        public string CEP { get; set; }

        public string Telefone { get; set; }

        public string Ramal { get; set; }

        public string Celular1 { get; set; }

        public string Celular2 { get; set; }

        public string Email { get; set; }

        public string DataCadastro { get; set; }

        public List<TransporteRelatorio_013> TraduzirLista(List<ClienteDistribuicaoRegionalInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_013>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(cdr =>
                {
                    lRetorno.Add(new TransporteRelatorio_013()
                    {
                        Bairro = cdr.NmBairro.ToStringFormatoNome(),
                        Celular1 = cdr.Celular1 > 0 ? string.Format("({0}) {1}", cdr.Celular1DDD, cdr.Celular1) : "-",
                        Celular2 = cdr.Celular2 > 0 ? string.Format("({0}) {1}", cdr.Celular2DDD, cdr.Celular2) : "-",
                        CEP = string.Format("{0}-{1}", cdr.CEP.PadRight(5, '0'), cdr.CEPExt.PadRight(3, '0')),
                        Cidade = cdr.NM_Cidade.ToStringFormatoNome(),
                        Complemento = cdr.DsCompEndereco,
                        CPFCNPJ = cdr.CPF.ToCpfCnpjString(),
                        DataCadastro = cdr.DT_Criacao.ToString("dd/MM/yyyy"),
                        Email = cdr.Email.ToLower(),
                        Logradouro = cdr.NmLogradouro.ToStringFormatoNome(),
                        NomeAssessor = cdr.NM_Assessor.ToStringFormatoNome(),
                        NomeCliente = cdr.NM_Cliente.ToStringFormatoNome(),
                        Ramal = cdr.TelefoneRamal > 0 ? cdr.TelefoneRamal.DBToString() : "-",
                        Telefone = cdr.Telefone > 0 ? cdr.Telefone.DBToString() : "-",
                        UF = cdr.SG_Estado.ToUpper(),
                    });
                });

            return lRetorno;
        }
    }
}
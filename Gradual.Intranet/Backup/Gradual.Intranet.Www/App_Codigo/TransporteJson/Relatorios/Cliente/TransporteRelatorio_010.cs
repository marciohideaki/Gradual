using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    public class TransporteRelatorio_010
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string TipoDePessoa { get; set; }
        public string DataDeCadastro { get; set; }
        public string Email { get; set; }
        public string Assessor { get; set; }
        public string Conta { get; set; }
        public string TipoConta { get; set; }

        public TransporteRelatorio_010() { }

        public TransporteRelatorio_010(ClienteInativoInfo pInfo)
        {
            this.Id = pInfo.IdCliente.DBToString();
            this.Nome = pInfo.DsNomeCliente.DBToString();
            this.CpfCnpj = pInfo.DsCpfCnpj.DBToString();
            this.TipoDePessoa = pInfo.TipoPessoa.DBToString();
            this.DataDeCadastro = pInfo.DtCadastro.ToString("dd/MM/yyyy");
            this.Email = pInfo.DsEmail.DBToString();
            this.Assessor = pInfo.IdAssessor.DBToString();
            this.Conta = pInfo.CdConta.DBToString();
            this.TipoConta = pInfo.DsConta.DBToString();
        }

    }
}
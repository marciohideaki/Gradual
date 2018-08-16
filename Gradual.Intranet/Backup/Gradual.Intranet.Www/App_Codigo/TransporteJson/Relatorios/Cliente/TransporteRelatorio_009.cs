using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Clientes sem email"
    /// </summary>
    public class TransporteRelatorio_009
    {


        public int Id { get; set; }

        public string Nome { get; set; }

        public string CpfCnpj { get; set; }

        public string TipoDePessoa { get; set; }

        public string DataDeCadastro { get; set; }

        public string Assessor { get; set; }

        public string Bovespa { get; set; }

        public string Email { get; set; }



        public TransporteRelatorio_009() { }

        public TransporteRelatorio_009(ClienteSemLoginInfo pInfo)
        {
            this.Id = pInfo.IdCliente;
            this.TipoDePessoa = "J".Equals(pInfo.TipoPessoa) ? "Jurídica" : "Física";
            this.Nome = pInfo.DsNomeCliente;
            this.CpfCnpj = pInfo.DsCpfCnpj.DBToInt64(true).ToCpfCnpjString();
            this.DataDeCadastro = pInfo.DtCadastro.ToString("dd/MM/yyyy");
            this.Assessor = pInfo.CodigoAssessor.DBToString();
            this.Bovespa = pInfo.CdBovespa;
            this.Email = pInfo.DsEmail;
        }


    }
}

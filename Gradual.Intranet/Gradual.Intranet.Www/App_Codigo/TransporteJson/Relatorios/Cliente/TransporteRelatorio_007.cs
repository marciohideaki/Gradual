#region Includes
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
#endregion 

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    /// <summary>
    /// Linha do Relatório Clientes > "Clientes suspeitos"
    /// </summary>
    public class TransporteRelatorio_007
    {
        #region Propriedades

        public string Assessor { get; set; }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string CpfCnpj { get; set; }

        public string TipoDePessoa { get; set; }

        public string DataDeCadastro { get; set; }

        public string Exportado { get; set; }

        public string Pais { get; set; }

        public string AtividadeIlicita { get; set; }

        public string Bovespa { get; set; }

        #endregion

        #region Construtor

        public TransporteRelatorio_007() { }

        public TransporteRelatorio_007(ClienteSuspeitoInfo pInfo) 
        {
            this.Id                 = pInfo.IdCliente;
            this.TipoDePessoa       = "F".Equals(pInfo.TipoPessoa) ? "Física" : "Jurídica";
            this.Nome               = pInfo.DsNomeCliente;
            this.CpfCnpj            = pInfo.DsCpfCnpj.DBToInt64(true).ToCpfCnpjString();
            this.DataDeCadastro     = pInfo.DtCadastro.ToString("dd/MM/yyyy");
            this.Exportado          = pInfo.blnExportado ? "Sim":"Não";
            this.Pais               = string.IsNullOrWhiteSpace(pInfo.NmPaisBlackList)    ? "-" : pInfo.NmPaisBlackList;
            this.AtividadeIlicita   = string.IsNullOrWhiteSpace(pInfo.DsAtividadeIlicita) ? "-" : pInfo.DsAtividadeIlicita;
            this.Assessor           = pInfo.CdAssessor;
            this.Bovespa            = string.IsNullOrWhiteSpace(pInfo.CdBovespa) ? "-" : pInfo.CdBovespa;
        }

        #endregion
    }
}

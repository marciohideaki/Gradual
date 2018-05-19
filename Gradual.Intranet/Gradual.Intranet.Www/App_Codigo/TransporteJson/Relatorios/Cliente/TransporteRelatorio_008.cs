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
    /// Linha do Relatório Clientes > "Emails disparados por período"
    /// </summary>
    public class TransporteRelatorio_008
    {
        #region | Atributos

        public string Nome { get; set; }

        public string Bovespa { get; set; }

        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public string DataDeEnvio { get; set; }

        public string DsEmailRemetente { get; set; }

        public string DsEmailDestinatario { get; set; }

        public string Assunto { get; set; }
        
        public string Perfil { get; set; }
        
        #endregion

        #region | Construtores
        
        public TransporteRelatorio_008() { }

        public TransporteRelatorio_008(EmailDisparadoPeriodoInfo pInfo)
        {
            this.Nome                = pInfo.DsNome.DBToString();
            this.Bovespa             = pInfo.CdCodigo;
            this.CpfCnpj             = pInfo.DsCpfCnpj.ToCpfCnpjString();
            this.Email               = pInfo.DsEmailDestinatario;
            this.DataDeEnvio         = pInfo.DtEnvio.ToString("dd/MM/yyyy HH:mm:ss");
            this.DsEmailRemetente    = pInfo.DsEmailRemetente;
            this.DsEmailDestinatario = pInfo.DsEmailDestinatario;
            this.Assunto             = pInfo.DsAssuntoEmail;
            this.Perfil              = pInfo.DsPerfil.DBToString();
        }
        #endregion
    }
}

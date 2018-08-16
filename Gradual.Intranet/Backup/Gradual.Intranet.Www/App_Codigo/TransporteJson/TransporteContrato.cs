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

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteContrato
    {
        #region Members
        public int Id { get; set; }

        public int ParentId { get; set; }

        public DateTime DataAssinatura { get; set; }

        public string TipoDeItem { get { return "Contratos"; } }

        public string Checked { get; set; }

        public string IdContrato { get; set; }

        public string DsContrato { get; set; }
        #endregion

        #region Constructors

        public TransporteContrato() { }

        public TransporteContrato(ClienteContratoInfo pInfo)
        {
            this.Id = pInfo.IdContrato;
            this.ParentId = pInfo.IdCliente.DBToInt32();
            this.DataAssinatura = pInfo.DtAssinatura;
            this.IdContrato = pInfo.IdContrato.ToString();
            //this.DsContrato = pInfo.d
        }
        #endregion

        #region Métodos Públicos
        public ClienteContratoInfo ToClienteContratoInfo()
        {
            return new ClienteContratoInfo()
            {
                IdCliente = ParentId,
                IdContrato = Id,
                DtAssinatura = DateTime.Now,
            };
        }
        #endregion
    }
}
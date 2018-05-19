using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STIntegracaoPortalHB
{
    public partial class Error : System.Web.UI.Page
    {
        public string lStackTrace = String.Empty;
        public string lMessage = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(!String.IsNullOrEmpty(Request.QueryString["Message"]))
                {
                    lMessage = Request.QueryString["Message"];
                    byte[] byteArray = Convert.FromBase64String(Request.QueryString["StackTrace"]);
                    lStackTrace = System.Text.Encoding.UTF8.GetString(byteArray);
                }
            }
        }
    }
}
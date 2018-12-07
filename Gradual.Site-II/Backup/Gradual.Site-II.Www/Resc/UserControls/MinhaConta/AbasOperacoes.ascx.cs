using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class AbasOperacoes : System.Web.UI.UserControl
    {
        #region Propriedades

        public string Modo { get; set; }

        public string AbaSelecionada { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.AbaSelecionada = "s";

            string lURL = Request.CurrentExecutionFilePath.ToLower();

            if (this.Modo == "menu")
            {
                if (lURL.Contains("acompanhamentoordens.aspx"))
                {
                    this.AbaSelecionada = "a";
                }
                else if (lURL.Contains("operacoes.aspx"))
                {
                    this.AbaSelecionada = "e";
                }
            }
        }
    }
}
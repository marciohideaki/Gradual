using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class AbasFundosInvestimentos : System.Web.UI.UserControl
    {
        #region Propriedades

        public string Modo { get; set; }

        public string AbaSelecionada { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.AbaSelecionada = "s";

            string lURL = Request.Url.OriginalString.ToLower();

            if (this.Modo == "menu")
            {
                if (lURL.Contains("recomendados.aspx"))
                {
                    this.AbaSelecionada = "r";
                }
                else if (lURL.Contains("rendafixa.aspx"))
                {
                    this.AbaSelecionada = "f";
                }
                else if (lURL.Contains("multimercados.aspx"))
                {
                    this.AbaSelecionada = "m";
                }
                else if (lURL.Contains("simular.aspx"))
                {
                    this.AbaSelecionada = "s";
                }
                else if (lURL.Contains("acoes.aspx"))
                {
                    this.AbaSelecionada = "a";
                }
                else if (lURL.Contains("referenciadodi.aspx"))
                {
                    this.AbaSelecionada = "d";
                }
                else if (lURL.Contains("cambial.aspx"))
                {
                    this.AbaSelecionada = "c";
                }
                else if (lURL.Contains("exterior.aspx"))
                {
                    this.AbaSelecionada = "e";
                }
                
                
            }
        }
    }
}
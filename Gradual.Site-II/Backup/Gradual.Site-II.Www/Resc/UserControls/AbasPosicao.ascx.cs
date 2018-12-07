using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class AbasPosicao : System.Web.UI.UserControl
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
                if (lURL.Contains("rendavariavel.aspx"))
                {
                    this.AbaSelecionada = "s";
                }
                else if (lURL.Contains("rendafixa.aspx"))
                {
                    this.AbaSelecionada = "e";
                }
                else if (lURL.Contains("fundos.aspx"))
                {
                    this.AbaSelecionada = "f";
                }
                
            }
            else
            {
                //if (lURL.Contains("custodia.aspx"))
                //{
                //    this.AbaSelecionada = "c";
                //}
                //else if (lURL.Contains("daytrade.aspx"))
                //{
                //    this.AbaSelecionada = "d";
                //}
                //else if (lURL.Contains("operacoes.aspx"))
                //{
                //    this.AbaSelecionada = "o";
                //}
                //else if (lURL.Contains("tesourodireto.aspx"))
                //{
                //    this.AbaSelecionada = "t";
                //}
                //else if (lURL.Contains("fundos.aspx"))
                //{
                //    this.AbaSelecionada = "f";
                //}
                //else if (lURL.Contains("saldofinanceiro.aspx"))
                //{
                //    this.AbaSelecionada = "s";
                //}
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        #region Propriedades
        

        public string SkinEmUso
        {
            get
            {
                if (Session["SkinEmUso"] == null)
                {
                    Session["SkinEmUso"] = ConfiguracoesValidadas.SkinPadrao;
                }

                return (string)Session["SkinEmUso"];
            }

            set
            {
                Session["SkinEmUso"] = value;
            }
        }

        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        public string RaizDoSite
        {
            get
            {
                return ConfiguracoesValidadas.RaizDoSite;
            }
        }

        public Usuario UsuarioLogado
        {
            get
            {
                if (Session["UsuarioLogado"] != null)
                {
                    return (Usuario)Session["UsuarioLogado"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Session["UsuarioLogado"] = value;
            }
        }
        public string TituloDaPagina
        {
            get
            {
                return lblTituloDaPagina.Text;
            }

            set
            {
                lblTituloDaPagina.Text = value;
            }
        }
        #endregion

        #region Métodos Private
        private void CarregarDados()
        {
            hidRaizDoSite.Value = ConfiguracoesValidadas.RaizDoSite;

            if (UsuarioLogado != null)
            {
                lblNomeDoUsuario.Text = string.Format("{0} - {1}", UsuarioLogado.CodBovespaTipoInt, UsuarioLogado.Nome);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        #endregion

        #region Métodos Public
        
        #endregion

        #region Events
        /// <summary>
        /// Evento de Load da página
        /// </summary>
        /// <param name="sender">Não está sendo usado</param>
        /// <param name="e">Não está sendo usado</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CarregarDados();
        }


        #endregion
    }
}
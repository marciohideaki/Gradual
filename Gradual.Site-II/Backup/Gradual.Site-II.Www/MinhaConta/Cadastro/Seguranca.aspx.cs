using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.MinhaConta.Cadastro
{
    public partial class Seguranca : PaginaBase
    {
        #region Métodos Private

        private string CarregarDados()
        {
            return string.Empty;
        }

        #endregion

        #region Event Handlers
        
        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Cadastro";
            this.PaginaMaster.Crumb3Text = "Segurança";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidarSessao();

            base.RegistrarRespostasAjax(new string[] {   CONST_FUNCAO_CASO_NAO_HAJA_ACTION 
                                                     }
                   , new ResponderAcaoAjaxDelegate[] {   CarregarDados
                                                     } );
        }

        #endregion

    }
}
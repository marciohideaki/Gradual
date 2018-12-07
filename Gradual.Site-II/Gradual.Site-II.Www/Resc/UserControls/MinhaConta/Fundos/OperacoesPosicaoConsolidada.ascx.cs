using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;

using Gradual.Site.DbLib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos
{
    public partial class OperacoesPosicaoConsolidada : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var lBase = (PaginaBase)this.Page;

                if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
                {
                    if (!lBase.JavascriptParaRodarOnLoad.Contains("Você ainda não possui código de conta Gradual"))
                    {
                        lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }

                    return;
                }
                this.CarregarDados();
            }
        }

        #region Métodos 
        private void CarregarDados()
        {
            try
            {
                var lBase = (PaginaBase)this.Page;
                var lBaseFundos = new PaginaFundos();

                var lListaFundos = lBaseFundos.PosicaoFundos();

                this.rptListaDePosicaoConsolidada.DataSource = lListaFundos;
                this.rptListaDePosicaoConsolidada.DataBind();

                this.trNenhumPosicaoConsolidada.Visible = lListaFundos.Count == 0;
            }
            catch (Exception ex)
            {
                ((PaginaBase)this.Page).ExibirMensagemJsOnLoad("E","Ocorreu um erro ao carregar a posição consolidada de fundos", false, ex.StackTrace);
            }

        }
        #endregion
    }
}
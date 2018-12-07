using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Site.Www.Transporte;


namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos
{
    public partial class OperacoesResgate : System.Web.UI.UserControl
    {
        #region Eventos
        
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
        #endregion

        #region Métodos
        private void CarregarDados()
        {
            try
            {
                var lBase = (PaginaBase)this.Page;
                var lBaseFundos = new PaginaFundos();

                var lListaFundos = lBaseFundos.PosicaoFundos();

                this.rptListaDePosicaoConsolidada.DataSource = lListaFundos;// new Transporte_PosicaoCotista().TraduzirLista(lPosicao);
                this.rptListaDePosicaoConsolidada.DataBind();

                this.trNenhumPosicaoConsolidada.Visible = lListaFundos.Count() == 0;
            }
            catch (Exception ex)
            {
                ((PaginaBase)this.Page).ExibirMensagemJsOnLoad("E", "Erro ao carregar posição de fundos de Cliente", false, ex.StackTrace);
            }
        }

        protected void rptListaDePosicaoConsolidada_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ((Button)(((Control)(e.Item)).Controls[1])).CommandArgument = ((Transporte_PosicaoCotista)(e.Item.DataItem)).IdFundo;
        }

        protected void btnResgatar_Click (object sender, EventArgs e)
        {
            try
            {
                var lBase = (PaginaBase)this.Page;

                string idFundo =  ((Button)(sender)).CommandArgument;

                Response.Redirect(lBase.HostERaizFormat("MinhaConta/Produtos/Fundos/resgate.aspx?idFundo=" + idFundo));
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        #endregion

    }
}
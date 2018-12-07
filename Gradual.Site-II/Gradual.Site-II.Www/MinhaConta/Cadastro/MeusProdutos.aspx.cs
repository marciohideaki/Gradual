using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.MinhaConta.Cadastro
{
    public partial class MeusProdutos : PaginaBase
    {
        #region Métodos Private

        private string CarregarDados()
        {
            BuscarComprasDoClienteRequest lRequest = new BuscarComprasDoClienteRequest();
            BuscarComprasDoClienteResponse lResponse;

            lRequest.CpfCnpj = SessaoClienteLogado.CpfCnpj;

            lResponse = base.ServicoPersistenciaSite.BuscarComprasDoCliente(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                rptListaDeCompras.DataSource = lResponse.ListaDeCompras;
                rptListaDeCompras.DataBind();
            }

            base.CarregarProdutosAdquiridosDoCliente(true);

            if (SessaoClienteLogado.ProdutosAdquiridos.Count > 0)
            {
                rptListaDeFerramentas.DataSource = SessaoClienteLogado.ProdutosAdquiridos;
                rptListaDeFerramentas.DataBind();

                //liNenhumItem_Ferramentas.Visible = false;

                //pnlComprasEfetuadas.Visible = true;
                //pnlBaixarAtualizador.Visible = true;
            }
            else
            {
                //liNenhumItem_Ferramentas.Visible = true;

                //pnlComprasEfetuadas.Visible = false;
                //pnlBaixarAtualizador.Visible = false;
            }

            return string.Empty;
        }

        #endregion

        #region Event Handlers
        
        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Cadastro";
            this.PaginaMaster.Crumb3Text = "Meus Produtos";
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
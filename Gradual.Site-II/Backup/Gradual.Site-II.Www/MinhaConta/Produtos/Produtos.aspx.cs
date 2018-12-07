using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.MinhaConta.Produtos
{
    public partial class Produtos : PaginaBase
    {

        #region Métodos Private

        private void CarregarDados()
        {
            BuscarComprasDoClienteRequest lRequest = new BuscarComprasDoClienteRequest();
            BuscarComprasDoClienteResponse lResponse;

            lRequest.CpfCnpj = SessaoClienteLogado.CpfCnpj;

            lResponse = base.ServicoPersistenciaSite.BuscarComprasDoCliente(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                rptListaDeCompras.DataSource = lResponse.ListaDeCompras;
                rptListaDeCompras.DataBind();

                trNenhumaCompra.Visible = (lResponse.ListaDeCompras.Count == 0);
            }

            base.CarregarProdutosAdquiridosDoCliente(true);

            if (SessaoClienteLogado.ProdutosAdquiridos.Count > 0)
            {
                rptListaDeFerramentas.DataSource = SessaoClienteLogado.ProdutosAdquiridos;
                rptListaDeFerramentas.DataBind();

                liNenhumProduto.Visible = false;

                //pnlComprasEfetuadas.Visible = true;
                //pnlBaixarAtualizador.Visible = true;
            }
            else
            {
                liNenhumProduto.Visible = true;

                //pnlComprasEfetuadas.Visible = false;
                //pnlBaixarAtualizador.Visible = false;
            }
        }

        #endregion
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (!this.IsPostBack)
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        return;
                    }

                    this.CarregarDados();
                }
            }
        }
    }
}
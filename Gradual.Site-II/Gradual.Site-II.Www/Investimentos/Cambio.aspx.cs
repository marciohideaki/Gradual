using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.Www.Resc.UserControls;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;

namespace Gradual.Site.Www.Investimentos
{
    public partial class Cambio : PaginaBase
    {
        #region Métodos Private

        private void CarregarDados()
        {
            //rptProdutosCambio.DataSource = new List<Gradual.Site.DbLib.Dados.MinhaConta.Comercial.ProdutoInfo
            
            BuscarDadosDosProdutosRequest lRequest = new BuscarDadosDosProdutosRequest();
            BuscarDadosDosProdutosResponse lResponse;

            lRequest.Plano = 2; //fixo: "Cambio"

            lResponse = base.ServicoPersistenciaSite.BuscarDadosDosProdutos(lRequest);

            rptProdutosCambio.DataSource = lResponse.DadosDosProdutos;
            rptProdutosCambio.ItemDataBound+=new RepeaterItemEventHandler(rptProdutosCambio_ItemDataBound);
            rptProdutosCambio.DataBind();
            
            rptProdutosTravelVisa.DataSource = lResponse.DadosDosProdutos;
            rptProdutosTravelVisa.ItemDataBound+=new RepeaterItemEventHandler(rptProdutosCambio_ItemDataBound);
            rptProdutosTravelVisa.DataBind();
            
            rptProdutosTravelMaster.DataSource = lResponse.DadosDosProdutos;
            rptProdutosTravelMaster.ItemDataBound+=new RepeaterItemEventHandler(rptProdutosCambio_ItemDataBound);
            rptProdutosTravelMaster.DataBind();

        }

        private void rptProdutosCambio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ItemCambio lItem = (ItemCambio)e.Item.FindControl("itCambio");

            TransporteProduto lTransporte = new TransporteProduto((Gradual.Site.DbLib.Dados.MinhaConta.Comercial.ProdutoInfo)e.Item.DataItem);

            lTransporte.Modo = lItem.Modo;

            lItem.DadosDoProduto = lTransporte;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CarregarDados();

            // Removido conforme solicitação do depto. de câmbio em 2017.07.19
            // CarregarLojas();

            //if (string.IsNullOrEmpty(Request["Popup"]))
            //{
            //    //this.PaginaMaster.CarrinhoVisivel = true;

            //    pnlConteudoNormal.Visible = true;
            //    pnlSejaCorrespondenteContainer.Visible = false;
            //}
            //else
            //{
            //    pnlConteudoNormal.Visible = false;
            //    pnlSejaCorrespondenteContainer.Visible = true;
            //}
        }

        // Removido conforme solicitação do depto. de câmbio em 2017.07.19
        /*
        private List<Gradual.Site.DbLib.Dados.LojaInfo> CarregarLojas()
        {
            var lRetorno = new List<Gradual.Site.DbLib.Dados.LojaInfo>();
            try
            {
                LojaRequest lRequest = new LojaRequest();
                LojaResponse lResponse;

                lResponse = base.ServicoPersistenciaSite.BuscarLojas(lRequest);

                if (lResponse.ListaLojas.Count > 0)
                {
                    lRetorno = lResponse.ListaLojas;
                    rptLojas.DataSource = lResponse.ListaLojas;
                    rptLojas.DataBind();
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Cambio.aspx > CarregarLojas() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                base.ExibirMensagemJsOnLoad("I", "");
            }

            return lRetorno;
        }
        */
    }


}
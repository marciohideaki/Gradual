using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Produtos
{
    public partial class StockMarket : System.Web.UI.UserControl
    {
        #region Propriedades

        public int IdDoProduto
        {
            get
            {
                return ConfiguracoesValidadas.IdDoProduto_StockMarket;
            }
        }

        #endregion

        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.CarregarDadosDoProduto();
            }
        }
        #endregion


        #region Métodos

        private void CarregarDadosDoProduto()
        {
            var lBase = (PaginaBase)this.Page;
            
            pnlDadosDeCompra_RealizarCompra.Visible =
            pnlDadosDeCompra_ProdutoJaAdquirido.Visible =
            pnlDadosDeCompra_AguardandoPagamento.Visible = false;
            //pnlDadosDeCompra_SemLogin.Visible = false;

            BuscarDadosDosProdutosRequest lRequest = new BuscarDadosDosProdutosRequest();
            BuscarDadosDosProdutosResponse lResponse;

            lRequest.IdProduto = ConfiguracoesValidadas.IdDoProduto_StockMarket;

            lResponse = lBase.ServicoPersistenciaSite.BuscarDadosDosProdutos(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.DadosDosProdutos.Count > 0)
                {
                    //lblPrecoDoProduto1.Text =
                    lblPrecoDoProduto2.Text = string.Format("Pagamento em até 12x através do PagSeguro ou R$ {0:n} à vista", lResponse.DadosDosProdutos[0].Preco);

                    if (lBase.SessaoClienteLogado != null)
                    {
                        lBase.CarregarProdutosAdquiridosDoCliente(false);

                        if (lBase.SessaoClienteLogado.TemProduto(ConfiguracoesValidadas.IdDoProduto_StockMarket))
                        {
                            int lStatus = lBase.SessaoClienteLogado.DadosDoProduto(ConfiguracoesValidadas.IdDoProduto_StockMarket).Status;

                            if (lStatus < 3)
                            {
                                pnlDadosDeCompra_AguardandoPagamento.Visible = true;
                            }
                            else
                            {
                                pnlDadosDeCompra_ProdutoJaAdquirido.Visible = true;
                            }
                        }
                        else
                        {
                            pnlDadosDeCompra_RealizarCompra.Visible = true;
                        }

                        if (ConfiguracoesValidadas.PermitirComprarDuasVezes)
                        {
                            pnlDadosDeCompra_RealizarCompra.Visible = true;
                        }
                    }
                }
                else
                {
                    PaginaBase.gLogger.ErrorFormat("Sem nenhum produto retornado em StockMarket.aspx > CarregarDadosDoProduto({0}): {1} > {2}"
                                        , ConfiguracoesValidadas.IdDoProduto_StockMarket
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }
            }
            else
            {
                PaginaBase.gLogger.ErrorFormat("Resposta com erro do serviço em StockMarket.aspx > CarregarDadosDoProduto(): {0} > {1}", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }

        }
        #endregion
    }


}
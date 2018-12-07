using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;
using System.Globalization;

namespace Gradual.Site.Www.Investimentos
{
    public partial class OfertaPublica : PaginaBase
    {
        #region Propriedades

        public bool VisualizandoUmaOferta
        {
            get
            {
                return !string.IsNullOrEmpty(Request["id"]);
            }
        }

        #endregion

        #region Métodos Private

        private void CarregarOfertaPublica()
        {
            List<TransporteISIN> lListaISIN = new List<TransporteISIN>();

            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            DateTime lDataFinal;

            string lCodigo = "0";

            lRequest.IdDaLista = ConfiguracoesValidadas.IdDaLista_ISIN;

            lResponse = base.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (base.SessaoClienteLogado != null) lCodigo = SessaoClienteLogado.CodigoPrincipal;


            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lListaISIN = lResponse.Itens.ParaListaTipada<TransporteISIN>();

                TransporteISIN.FiltrarLista(ref lListaISIN, Request["id"].DBToInt32());

                if (lListaISIN.Count > 0)
                {
                    foreach (TransporteISIN lItem in lListaISIN)
                    {
                        if (lItem.TipoOferta == "0")
                        {
                            btnPedidoReservaNormal.Attributes["data-CodigoPrincipal"] = lCodigo;
                            btnPedidoReservaNormal.Attributes["data-CodigoISIN"]      = lItem.CodigoISIN;

                            btnPedidoReservaNormal.Visible = true;
                        }

                        if (lItem.TipoOferta == "1")
                        {
                            btnPedidoPrioritarioON.Attributes["data-CodigoPrincipal"] = lCodigo;
                            btnPedidoPrioritarioON.Attributes["data-CodigoISIN"]      = lItem.CodigoISIN;

                            btnPedidoPrioritarioON.Visible = true;
                        }

                        if (lItem.TipoOferta == "2")
                        {
                            btnPedidoPrioritarioPN.Attributes["data-CodigoPrincipal"] = lCodigo;
                            btnPedidoPrioritarioPN.Attributes["data-CodigoISIN"]      = lItem.CodigoISIN;

                            btnPedidoPrioritarioPN.Visible = true;
                        }

                        if (lItem.TipoOferta == "3")
                        {
                            btnPedidoVarejoON.Attributes["data-CodigoPrincipal"] = lCodigo;
                            btnPedidoVarejoON.Attributes["data-CodigoISIN"]      = lItem.CodigoISIN;

                            btnPedidoVarejoON.Visible = true;
                        }

                        if (lItem.TipoOferta == "4")
                        {
                            btnPedidoVarejoPN.Attributes["data-CodigoPrincipal"] = lCodigo;
                            btnPedidoVarejoPN.Attributes["data-CodigoISIN"]      = lItem.CodigoISIN;

                            btnPedidoVarejoPN.Visible = true;
                        }

                        if (DateTime.TryParseExact(lItem.DataFinal, "dd/MM/yyyy", new CultureInfo("pt-BR"), System.Globalization.DateTimeStyles.NoCurrentDateDefault, out lDataFinal))
                        {
                            if (lDataFinal >= DateTime.Now)
                            {
                                if (base.SessaoClienteLogado == null)
                                {
                                    //independente do que tenha acontecido com os ISINs, sem usuário não mostra:
                                    pnlDadosDeCompra_RealizarCompra.Visible = false; 

                                    pnlDadosDeCompra_SemLogin.Visible = true;
                                }
                                else
                                {
                                    if (base.SessaoClienteLogado.Passo >= 4)
                                    {
                                        // usuário ok, ISINs ok, pode comprar!
                                        pnlDadosDeCompra_RealizarCompra.Visible = true;
                                    }
                                    else
                                    {
                                        //independente do que tenha acontecido com os ISINs, se usuário não tem passo suficiente, não mostra:
                                        pnlDadosDeCompra_RealizarCompra.Visible = false;

                                        pnlDadosDeCompra_SemPasso.Visible = true;
                                    }
                                }
                            }
                            else
                            {
                                pnlDadosDeCompra_RealizarCompra.Visible = false;

                                pnlDadosDeCompra_DataPassada.Visible = true;
                                lblDadosDeCompra_DataPassada.Text = lItem.DataFinal;
                            }
                        }
                    }
                }
                else
                {
                    pnlDadosDeCompra_SemISIN.Visible = true;
                }
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro do IServicoPersistenciaSite em OfertaPublica.aspx > CarregarOfertaPublica(pIdDaLista: [{0}]) - [{1}]\r\n{2}"
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            string lId = Request["id"];

            CarregarPagina(rdrConteudo, "investimentos/ofertapublica", "", lId);

            //hidIPO_URL.Value = ConfiguracoesValidadas.IPO_URL;
            hidIPO_URL.Value = "../minhaconta/Produtos/IPO/reservaIPO.aspx";

            if (!this.IsPostBack && this.VisualizandoUmaOferta)
            {
                // manda carregar de qualquer forma para verificar a validade...

                this.CarregarOfertaPublica();
            }
        }

        #endregion
    }
}
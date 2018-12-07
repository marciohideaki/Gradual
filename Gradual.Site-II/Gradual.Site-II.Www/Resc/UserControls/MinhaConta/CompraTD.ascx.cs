using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Compra;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using System.Globalization;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class CompraTD : UCTesouroDireto
    {
        #region Globais

        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";
        
        #endregion

        #region métodos Tesouro Direto
        private void CarregarTiposDeTituloEIndexadores()
        {
            List<CodigoNomeInfo> lTipos, lIndexadores;

            CarregarTiposEIndexadores(out lTipos, out lIndexadores);

            cboTipoDeTitulo.DataSource = lTipos;
            cboTipoDeTitulo.DataBind();

            cboTipoDeTitulo.Items.Insert(0, new ListItem("Selecione", ""));

            cboIndexadores.DataSource = lTipos;
            cboIndexadores.DataBind();

            cboIndexadores.Items.Insert(0, new ListItem("Selecione", ""));
        }

        private List<TituloMercadoInfo> ConsultarTitulosParaCompra()
        {
            List<TituloMercadoInfo> lRetorno = new List<TituloMercadoInfo>();
            
            try
            {
                CompraConsultaTituloMercadoRequest lRequestConsultaMercado = new CompraConsultaTituloMercadoRequest();
                CompraConsultaTituloMercadoResponse lResponseConsultaMercado;

                int lIndexador = (cboIndexadores.SelectedValue != "") ? int.Parse(cboIndexadores.SelectedValue) : 0;

                int lTipo = (cboTipoDeTitulo.SelectedValue != "") ? int.Parse(cboTipoDeTitulo.SelectedValue) : 0;

                DateTime lDate = DateTime.MinValue;

                txtConsultaVencimento.Text.EDataValida(out lDate);

                lRequestConsultaMercado.CodigoMercado = this.Mercado;
                lRequestConsultaMercado.CodigoTitulo = 0;
                lRequestConsultaMercado.Tipo = lTipo;
                lRequestConsultaMercado.DataEmissao = DateTime.MinValue;
                lRequestConsultaMercado.DataVencimento = lDate;
                lRequestConsultaMercado.TipoIndexador = lIndexador;
                lRequestConsultaMercado.SELIC = 0;
                lRequestConsultaMercado.ISIN = "";
                lRequestConsultaMercado.NotCesta = 0;

                gLogger.InfoFormat("Chamando ServicoTesouro.CompraConsultarTituloMercado(pCodigoMercado:[{0}], pTipo:[{1}], pDataVencimento:[{2}], pTipoIndexador:[{3}])"
                                    , lRequestConsultaMercado.CodigoMercado
                                    , lRequestConsultaMercado.Tipo
                                    , lRequestConsultaMercado.DataVencimento
                                    , lRequestConsultaMercado.TipoIndexador);

                lResponseConsultaMercado = ServicoTesouro.CompraConsultarTituloMercado(lRequestConsultaMercado);

                if (RespostaDoWebServiceSemErros(lResponseConsultaMercado))
                {
                    lRetorno = lResponseConsultaMercado.Titulos;
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoTesouro.CompraConsultarTituloMercado(pCodigoMercado:[{0}], pTipo:[{1}], pDataVencimento:[{2}], pTipoIndexador:[{3}]) > [{0}] [{1}]"
                                        , lRequestConsultaMercado.CodigoMercado
                                        , lRequestConsultaMercado.Tipo
                                        , lRequestConsultaMercado.DataVencimento
                                        , lRequestConsultaMercado.TipoIndexador
                                        , lResponseConsultaMercado.StatusResposta
                                        , lResponseConsultaMercado.DescricaoResposta);

                    this.btnConsultar.Enabled = false;
                }
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao consultar títulos disponiveis para compra:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > ConsultarTitulosParaCompra(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                btnConsultar.Enabled = false;
            }

            return lRetorno;
        }

        private void PreencherBoleta(TituloMercadoInfo pSelecionado)
        {
            LimparBoleta();

            lblBoletaTitulo.Text = pSelecionado.NomeTitulo.ToUpper();
            lblBoletaPrecoUnitario.Text = pSelecionado.ValorCompra.ToString("C2");
            lblBoletaDataVencimento.Text = pSelecionado.DataVencimento.ToString("dd/MM/yyyy");
        }

        private void LimparBoleta()
        {
            lblBoletaTitulo.Text =
            lblBoletaPrecoUnitario.Text =
            lblBoletaDataVencimento.Text =
            txtBoletaQuantidade.Text =
            txtBoletaValorCompra.Text =
            txtBoletaTaxaCBLC.Text =
            txtBoletaTaxaAgCustodia.Text =
            txtBoletaValorTotal.Text = "";

            btnCesta_Voltar.Visible = false;
        }

        private bool BoletaDeCompraEstaValida()
        {
            bool lRetorno = false;

            if (string.IsNullOrEmpty(txtBoletaQuantidade.Text.Trim()))
            {
                GlobalBase.ExibirMensagemJsOnLoad("I", "Entre com a quantidade.");
            }
            else
            {
                decimal lQuantidade;

                if (!decimal.TryParse(txtBoletaQuantidade.Text, System.Globalization.NumberStyles.Any, new CultureInfo("pt-BR"), out lQuantidade))
                {
                    GlobalBase.ExibirMensagemJsOnLoad("I", "Quantidade inválida.");
                }
                else
                {
                    decimal lResto = lQuantidade % 0.1M;

                    //if (txtBoletaQuantidade.Text[txtBoletaQuantidade.Text.Length - 1] != '0')
                    if (lResto != 0)
                    {
                        GlobalBase.ExibirMensagemJsOnLoad("I", "A quantidade deve ser múltipla de 0,1.");
                    }
                    else
                    {
                        lRetorno = true;
                    }
                }
            }

            return lRetorno;
        }

        private void CalcularTotal()
        {
            try
            {
                TituloMercadoInfo lTituloMercado = (TituloMercadoInfo)ViewState["Compra_SelectedItem"];

                txtBoletaValorCompra.Text = "";
                txtBoletaTaxaCBLC.Text = "";
                txtBoletaTaxaAgCustodia.Text = "";
                txtBoletaValorTotal.Text = "";

                if (BoletaDeCompraEstaValida())
                {
                    CompraCalculaTaxaWSRequest lRequestDeCompra = new CompraCalculaTaxaWSRequest();
                    CompraCalculaTaxaWSResponse lResponseDeCompra;

                    decimal lQuantidade = txtBoletaQuantidade.Text.DBToDecimal();

                    string Titulo = string.Format("<TITULOS><TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO><QUANTIDADE>{1}</QUANTIDADE></TITULO></TITULOS>", lTituloMercado.CodigoTitulo, lQuantidade.ToString("N2").Replace(",", "."));

                    lRequestDeCompra.CodigoMercado = this.Mercado;
                    lRequestDeCompra.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequestDeCompra.XMLTitulo = Titulo;

                    lResponseDeCompra = ServicoTesouro.CompraCalcularTaxaWs(lRequestDeCompra);

                    if (RespostaDoWebServiceSemErros(lResponseDeCompra))
                    {
                        decimal lTaxaCBLC = lResponseDeCompra.Objeto[0].ValorCBLC;
                        decimal lTaxaAgCustodia = lResponseDeCompra.Objeto[0].ValorAC;
                        decimal lValorUnitario = lTituloMercado.ValorCompra;
                        decimal lBoletaValorCompra = lValorUnitario * lQuantidade;

                        txtBoletaTaxaCBLC.Text = lTaxaCBLC.ToString("C2");
                        txtBoletaTaxaAgCustodia.Text = lTaxaAgCustodia.ToString("C2");
                        txtBoletaQuantidade.Text = lQuantidade.ToString("N2");
                        txtBoletaValorCompra.Text = lBoletaValorCompra.ToString("C2");
                        txtBoletaValorTotal.Text = (lBoletaValorCompra + lTaxaAgCustodia + lTaxaCBLC).ToString("C2");

                        btnIncluirTituloNaCesta.Visible = true;
                    }
                    else
                    {
                        btnIncluirTituloNaCesta.Visible = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao calcular total:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > CalcularTotal(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                this.btnIncluirTituloNaCesta.Visible = false;
            }
        }

        private void CalcularQuantidade()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtBoletaValorTotal.Text.Replace(" ", "")))
                {
                    GlobalBase.ExibirMensagemJsOnLoad("I", "Entre com o valor total.");

                    return;
                }

                string lTitulo;

                TituloMercadoInfo lSelecionado = (TituloMercadoInfo)ViewState["Compra_SelectedItem"];

                lTitulo = string.Format("<TITULOS><TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO><QUANTIDADE>1</QUANTIDADE></TITULO></TITULOS>", lSelecionado.CodigoTitulo);

                CompraCalculaTaxaWSRequest lRequest = new CompraCalculaTaxaWSRequest();
                CompraCalculaTaxaWSResponse lResponse;

                lRequest.CodigoMercado = this.Mercado;
                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.XMLTitulo = lTitulo;

                lResponse = this.ServicoTesouro.CompraCalcularTaxaWs(lRequest);

                if (!RespostaDoWebServiceSemErros(lResponse))
                    return;

                //CALCULOS

                decimal lTotalDesejado, lPrecoTitulo;

                double lQuantidadeEstimada;

                lTotalDesejado = txtBoletaValorTotal.Text.Replace("R$", "").Replace(" ", "").Replace(".", "").DBToDecimal();

                lPrecoTitulo = lSelecionado.ValorCompra;

                lQuantidadeEstimada = Math.Round((double)lTotalDesejado / (double)lPrecoTitulo, 1);

                txtBoletaQuantidade.Text = lQuantidadeEstimada.ToString("N2");

                this.CalcularTotal();
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao calcular quantidade:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > CalcularQuantidade: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void MostrarCesta()
        {
            try
            {
                bool lCestaExistenteAberta = false;

                if (ViewState["Cesta"] == null)
                {
                    ConsultasConsultaCestaRequest lRequestConsultarCesta = new ConsultasConsultaCestaRequest();
                    ConsultasConsultaCestaResponse lResponseConsultarCesta;

                    lRequestConsultarCesta.CodigoMercado = this.Mercado;
                    lRequestConsultarCesta.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequestConsultarCesta.CodigoCesta = 0;
                    lRequestConsultarCesta.DataCompra = DateTime.MinValue;
                    lRequestConsultarCesta.CodigoTitulo = 0;
                    lRequestConsultarCesta.Cliente = 0;

                    gLogger.InfoFormat("Chamando ServicoTesouro.ConsultarCesta(pMercado: [{0}], pCPF: [{1}])"
                                        , lRequestConsultarCesta.CodigoMercado
                                        , lRequestConsultarCesta.CPFNegociador);

                    lResponseConsultarCesta = ServicoTesouro.ConsultarCesta(lRequestConsultarCesta);

                    if (!RespostaDoWebServiceSemErros(lResponseConsultarCesta))
                        return;

                    if (lResponseConsultarCesta.Titulos.Count <= 0)
                    {
                        this.btnCesta_Desistir.Visible = false;
                        this.btnCesta_ConfirmarCompra.Visible = false;

                        GlobalBase.ExibirMensagemJsOnLoad("I", "Você não possui nenhuma cesta de compra aberta, inclua algum título para abrir uma nova cesta.");

                        return;
                    }

                    ViewState["Cesta"] = lResponseConsultarCesta.Titulos[lResponseConsultarCesta.Titulos.Count - 1].CodigoCesta;

                    lCestaExistenteAberta = lResponseConsultarCesta.Titulos[lResponseConsultarCesta.Titulos.Count - 1].Situacao == "4"; // Situação: 4 -> Pendente

                    this.btnCesta_ConfirmarCompra.Visible = true;
                    this.btnCesta_Desistir.Visible = true;
                }
                else
                {
                    lCestaExistenteAberta = true;
                }

                CompraConsultaCestaItensRequest lRequestConsultaDeItens = new CompraConsultaCestaItensRequest();
                CompraConsultaCestaItensResponse lResponseConsultaDeItens;

                lRequestConsultaDeItens.Mercado = this.Mercado;
                lRequestConsultaDeItens.CodigoCesta = this.CodigoCesta;
                lRequestConsultaDeItens.CodigoTitulo = 0;

                gLogger.InfoFormat("Chamando ServicoTesouro.ConsultarCesta(pMercado: [{0}], CodigoCesta: [{1}])"
                                    , lRequestConsultaDeItens.Mercado
                                    , lRequestConsultaDeItens.CodigoCesta);

                lResponseConsultaDeItens = ServicoTesouro.CompraConsultarCestaItens(lRequestConsultaDeItens);

                if (!RespostaDoWebServiceSemErros(lResponseConsultaDeItens))
                    return;

                if (lCestaExistenteAberta && lResponseConsultaDeItens.Objeto.Count > 0)
                {
                    litNenhumRegistroEncontradoCesta.Visible = false;

                    rptTesouroDiretoCompraCesta.DataSource = new Transporte_TesouroDireto_Compra().TraduzirLista(lResponseConsultaDeItens.Objeto);

                    rptTesouroDiretoCompraCesta.DataBind();

                    ViewState["ListaGridCestas"] = lResponseConsultaDeItens.Objeto;
                }
                else
                {
                    ViewState["ListaGridCestas"] = null;
                    ViewState["Cesta"] = null;

                    this.rptTesouroDiretoCompraCesta.DataBind();

                    this.litNenhumRegistroEncontradoCesta.Visible = true;
                }

                this.pnlTesouroDiretoCompraBoleta.Visible = false;
                this.pnlTesouroDiretoCompra.Visible = false;
                this.pnlTesouroDiretoBusca.Visible = false;
                this.pnlTesouroDiretoCompraCesta.Visible = true;

                this.btnCesta_Desistir.Visible = true;
                this.btnCesta_ConfirmarCompra.Visible = true;
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao mostrar cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > MostrarCesta(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }
        }

        private void CancelarCesta()
        {
            try
            {
                CompraExcluirCestaRequest lRequest = new CompraExcluirCestaRequest();
                CompraExcluirCestaResponse lResponse;

                lRequest.Mercado = this.Mercado;
                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoCesta = this.CodigoCesta;

                gLogger.InfoFormat("Chamando ServicoTesouro.CompraExcluirCesta(pMercado:[{0}], pCPF:[{1}], pCodigoCesta:[{2}]"
                                    , lRequest.Mercado
                                    , lRequest.CPFNegociador
                                    , lRequest.CodigoCesta);

                lResponse = this.ServicoTesouro.CompraExcluirCesta(lRequest);

                if (RespostaDoWebServiceSemErros(lResponse))
                {
                    gLogger.InfoFormat("Resposta OK do serviço para CompraExcluirCesta(pMercado:[{0}], pCPF:[{1}], pCodigoCesta:[{2}]"
                                        , lRequest.Mercado
                                        , lRequest.CPFNegociador
                                        , lRequest.CodigoCesta);

                    GlobalBase.ExibirMensagemJsOnLoad("I", "Compra cancelada com sucesso.");

                    ViewState["Cesta"] = null;

                    rptTesouroDiretoCompraCesta.DataBind();

                    litNenhumRegistroEncontradoCesta.Visible = true;

                    this.btnCesta_ConfirmarCompra.Visible = false;
                    this.btnCesta_Desistir.Visible = false;
                    //btnCesta_AdicionarMais.Visible   = false;
                    this.btnCesta_Voltar.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao finalizar cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > FinalizarCesta(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }
        }

        private void IncluirTituloNaCesta()
        {
            if (!BoletaDeCompraEstaValida())
                return;

            string lCesta = "";

            ConsultasConsultaCestaRequest lRequestConsultarCesta = new ConsultasConsultaCestaRequest();
            ConsultasConsultaCestaResponse lResponseConsultarCesta = new ConsultasConsultaCestaResponse();

            try
            {
                lRequestConsultarCesta.CodigoMercado = this.Mercado;
                lRequestConsultarCesta.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequestConsultarCesta.CodigoCesta = 0;
                lRequestConsultarCesta.DataCompra = DateTime.MinValue;
                lRequestConsultarCesta.CodigoTitulo = 0;
                lRequestConsultarCesta.Cliente = 0;

                lResponseConsultarCesta = this.ServicoTesouro.ConsultarCesta(lRequestConsultarCesta);

                if (!RespostaDoWebServiceSemErros(lResponseConsultarCesta))
                    return;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao consultar as cestas:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > IncluirTituloNaCesta: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                return;
            }

            try
            {
                if (lResponseConsultarCesta.Titulos.Count > 0 && lResponseConsultarCesta.Titulos[lResponseConsultarCesta.Titulos.Count - 1].Situacao == "4") // Situação: 4 -> Pendente
                {
                    lCesta = lResponseConsultarCesta.Titulos[lResponseConsultarCesta.Titulos.Count - 1].CodigoCesta;

                    if (lResponseConsultarCesta.Titulos.Where(c => c.CodigoTitulo.DBToString() == ViewState["TituloSelecionado"].DBToString() && c.CodigoCesta == lCesta).Count() > 0)
                    {
                        GlobalBase.ExibirMensagemJsOnLoad("I", "Você ja possui este título em sua cesta");

                        return;
                    }
                }
                else
                {
                    CompraInsereNovaCestaRequest lRequestInserirNovaCesta = new CompraInsereNovaCestaRequest();
                    CompraInsereNovaCestaResponse lResponseInserirNovaCesta;

                    lRequestInserirNovaCesta.Mercado = this.Mercado;
                    lRequestInserirNovaCesta.CPFNegociador = SessaoClienteLogado.CpfCnpj;

                    lResponseInserirNovaCesta = this.ServicoTesouro.CompraInserirNovaCesta(lRequestInserirNovaCesta);

                    if (!RespostaDoWebServiceSemErros(lResponseInserirNovaCesta))
                        return;

                    lCesta = lResponseInserirNovaCesta.Objeto.CodigoCesta;

                    ViewState["Cesta"] = lCesta;
                }

                CompraInsereItemNaCestaRequest lRequestInserirItem = new CompraInsereItemNaCestaRequest();
                CompraInsereItemNaCestaResponse lResponseInserirItem;

                lRequestInserirItem.Mercado = this.Mercado;
                lRequestInserirItem.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequestInserirItem.CodigoCesta = lCesta.DBToInt32();
                lRequestInserirItem.TituloCodigoTitulo = ViewState["TituloSelecionado"].DBToInt32();
                lRequestInserirItem.TituloQuantidadeCompra = txtBoletaQuantidade.Text.DBToDouble();

                lResponseInserirItem = this.ServicoTesouro.CompraInserirItemNaCesta(lRequestInserirItem);

                if (!RespostaDoWebServiceSemErros(lResponseInserirItem))
                    return;

                this.MostrarCesta();
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao incluir o item na cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > IncluirTituloNaCesta: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                btnIncluirTituloNaCesta.Visible = false;

                return;
            }

            btnIncluirTituloNaCesta.Visible = true;
        }

        private void VoltarParaTelaDeBusca()
        {
            /*
            LimparBoleta();

            pnlTesouroDiretoBusca.Visible = true;
            pnlTesouroDiretoCompra.Visible = true;
            pnlTesouroDiretoCompraBoleta.Visible = false;
            */

            this.pnlTesouroDiretoBusca.Visible = true;
            this.pnlTesouroDiretoCompra.Visible = true;
            this.pnlTesouroDiretoCompraBoleta.Visible = false;
            this.pnlTesouroDiretoCompraCesta.Visible = false;

            this.LimparBoleta();

            btnCesta_Voltar.Visible = false;
            btnCesta_ConfirmarCompra.Visible = true;
            btnCesta_Desistir.Visible = true;
            //btnCesta_AdicionarMais.Visible = true;
        }

        private void ConsultarTitulos()
        {
            try
            {
                List<TituloMercadoInfo> CompraListTitMercado = ConsultarTitulosParaCompra();

                rptTesouroDiretoCompraResultado.DataSource = new Transporte_TesouroDireto_Compra().TraduzirLista(CompraListTitMercado);
                rptTesouroDiretoCompraResultado.DataBind();

                ViewState["CompraListTitMercado"] = CompraListTitMercado;

                pnlTesouroDiretoCompra.Visible = true;
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao consultar título:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > ConsultarTitulos: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void ExcluirTituloDaCesta(int pCodigo)
        {
            try
            {
                List<TituloInfo> lTitulos = new List<TituloInfo>();

                lTitulos.Add(new TituloInfo() { CodigoTitulo = pCodigo });

                CompraExcluirItemDaCestaRequest lRequest = new CompraExcluirItemDaCestaRequest();
                CompraExcluirItemDaCestaResponse lResponse;

                lRequest.Mercado = this.Mercado;
                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoCesta = this.CodigoCesta;
                lRequest.Titulos = lTitulos;

                gLogger.InfoFormat("Chamando ServicoTesouro.CompraExcluirItemCesta(pMercado: [{0}], pCPF: [{1}], pCodigoCesta: [{2}])"
                                    , lRequest.Mercado
                                    , lRequest.CPFNegociador
                                    , lRequest.CodigoCesta);

                lResponse = this.ServicoTesouro.CompraExcluirItemCesta(lRequest);

                if (!RespostaDoWebServiceSemErros(lResponse))
                    return;

                MostrarCesta();

                GlobalBase.ExibirMensagemJsOnLoad("I", "Título removido com sucesso!");
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao excluir o item da cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > ExcluirTituloDaCesta: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                return;
            }
        }

        private void SelecionarParaCompra(int pChave)
        {
            try
            {
                TituloMercadoInfo lSelecionado = ((List<TituloMercadoInfo>)ViewState["CompraListTitMercado"]).Single(condicao => condicao.CodigoTitulo == pChave);

                ViewState["Compra_SelectedItem"] = lSelecionado;
                ViewState["TituloSelecionado"] = lSelecionado.CodigoTitulo;

                PreencherBoleta(lSelecionado);

                pnlTesouroDiretoBusca.Visible = false;
                pnlTesouroDiretoCompra.Visible = false;
                pnlTesouroDiretoCompraBoleta.Visible = true;
                pnlTesouroDiretoCompraCesta.Visible = false;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao selecionar títulos para compra:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > SelecionarParaCompra: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                btnConsultar.Enabled = false;
            }
        }

        private void ConfirmarCompra()
        {
            if (base.ValidarAssinaturaEletronica(txtAssinaturaEletronica.Value))
            {

                try
                {
                    CompraVerificaTituloNoMercadoRequest lRequest = new CompraVerificaTituloNoMercadoRequest();
                    CompraVerificaTituloNoMercadoResponse lResponse;

                    lRequest.CodigoMercado = this.Mercado;
                    lRequest.CodigoCesta = this.CodigoCesta;

                    lResponse = this.ServicoTesouro.CompraVerificarTituloNoMercado(lRequest);

                    if (!RespostaDoWebServiceSemErros(lResponse))
                        return;

                    if (lResponse.Objeto.Count > 0)
                    {
                        this.MostrarCesta();

                        GlobalBase.ExibirMensagemJsOnLoad("I", "Alguns títulos não estão mais disponíveis no mercado e foram removidos.<br />Verifique sua cesta e confirme a compra novamente caso deseje.");

                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao verificar titulos da cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                    gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > ConfirmarCompra: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                    return;
                }

                try
                {
                    List<CompraConsultaCestaItemInfo> lItens = ViewState["ListaGridCestas"] as List<CompraConsultaCestaItemInfo>;

                    List<TituloInfo> lTitulos = new List<TituloInfo>();

                    CompraFecharCestaRequest lRequest = new CompraFecharCestaRequest();
                    CompraFecharCestaResponse lResponse;

                    string lTitulosPorExtenso = "";

                    if (lItens == null || lItens.Count == 0)
                    {
                        GlobalBase.ExibirMensagemJsOnLoad("I", "Você não possui itens na cesta.");

                        return;
                    }

                    foreach (CompraConsultaCestaItemInfo lTitulo in lItens)
                    {
                        lTitulos.Add(new TituloInfo()
                        {
                            CodigoTitulo = lTitulo.CodigoTitulo,
                            Quantidade = lTitulo.QuantidadeCompra
                        });

                        lTitulosPorExtenso += string.Format("[Codigo {0}, Quantidade {1}], ", lTitulo.CodigoTitulo, lTitulo.QuantidadeCompra);
                    }

                    lTitulosPorExtenso = lTitulosPorExtenso.TrimEnd().TrimEnd(',');

                    lRequest.Mercado = this.Mercado;
                    lRequest.CodigoCesta = this.CodigoCesta;
                    lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequest.Titulos = lTitulos;

                    gLogger.InfoFormat("Chamando ServicoTesouro.CompraFecharCesta(pMercado: [{0}], pCodigoCesta: [{1}], pCPF: [{2}], pTitulos: [{3}])"
                                        , lRequest.Mercado
                                        , lRequest.CodigoCesta
                                        , lRequest.CPFNegociador
                                        , lTitulosPorExtenso);

                    lResponse = this.ServicoTesouro.CompraFecharCesta(lRequest);

                    if (RespostaDoWebServiceSemErros(lResponse))
                    {

                        string lProtocolo = this.CodigoCesta.ToString();

                        gLogger.InfoFormat("Resposta OK de ServicoTesouro.CompraFecharCesta(); Protocolo: [{0}]", lProtocolo);

                        MostrarCesta();

                        btnCesta_ConfirmarCompra.Visible = false;
                        btnCesta_Desistir.Visible = false;
                        //btnCesta_AdicionarMais.Visible   = false;
                        btnCesta_Voltar.Visible = true;

                        GlobalBase.ExibirMensagemJsOnLoad("I", string.Format("Compra concluída com sucesso, o número do protocolo (cesta) é: {0}", lProtocolo));
                    }
                }
                catch (Exception ex)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao fechar a cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                    gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > ConfirmarCompra: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                    return;
                }
            }
            else
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Assinatura eletrônica inválida.");
            }
        }
        #endregion

        #region Eventos
        protected new void Page_Init(object sender, EventArgs e)
        {
            this.IdentificadorDaPagina = "Compra";

            //this.PaginaMaster.Secao = new SecaoDoSite("MinhaConta", "MinhaConta");

            //this.PaginaMaster.SubSecao = new SecaoDoSite("Tesouro Direto", HostERaizFormat("MinhaConta/TesouroDireto/Compra.aspx"));
        }

        protected void pnlTesouroDiretoBusca_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                VerificarDisponibilidade();

                CarregarTiposDeTituloEIndexadores();
            }
        }

        protected void rptTesouroDiretoCompraResultado_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ((Button)(((Control)(e.Item)).Controls[1])).CommandArgument = ((Transporte_TesouroDireto_Compra.TesouroDireto_Compra)(e.Item.DataItem)).CodigoTitulo;
        }

        protected void rptTesouroDiretoCompraCesta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ((Button)(((Control)(e.Item)).Controls[1])).CommandArgument = ((Transporte_TesouroDireto_Compra.TesouroDireto_Cesta)(e.Item.DataItem)).CodigoTitulo;
        }

        protected void Compra_BtnVerCesta_Click(object sender, EventArgs e)
        {
            MostrarCesta();
        }

        protected void btnCalcularTotalDaBoleta_Click(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        protected void btnBoletaCalcularQuantidade_Click(object sender, EventArgs e)
        {
            CalcularQuantidade();
        }

        protected void btnIncluirTituloNaCesta_Click(object sender, EventArgs e)
        {
            IncluirTituloNaCesta();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            VoltarParaTelaDeBusca();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarTitulos();
        }

        protected void btnTesouroDiretoCompra_Click(object sender, EventArgs e)
        {
            SelecionarParaCompra(((Button)(sender)).CommandArgument.DBToInt32());
        }

        protected void btnTesouroDiretoCompraCestaExcluir_Click(object sender, EventArgs e)
        {
            ExcluirTituloDaCesta(((Button)(sender)).CommandArgument.DBToInt32());
        }

        protected void btnCesta_Desistir_Click(object sender, EventArgs e)
        {
            CancelarCesta();
        }

        protected void btnCesta_Voltar_Click(object sender, EventArgs e)
        {
            /*
            this.pnlTesouroDiretoBusca.Visible = true;
            this.pnlTesouroDiretoCompra.Visible = true;
            this.pnlTesouroDiretoCompraBoleta.Visible = false;
            this.pnlTesouroDiretoCompraCesta.Visible = false;

            this.LimparBoleta();

            btnCesta_Voltar.Visible = false;

            btnCesta_ConfirmarCompra.Visible = true;

            btnCesta_Desistir.Visible = true;

            btnCesta_AdicionarMais.Visible = true;*/

            VoltarParaTelaDeBusca();
        }

        protected void btnCesta_AdicionarMais_Click(object sender, EventArgs e)
        {
            /*
            this.pnlTesouroDiretoBusca.Visible = true;
            this.pnlTesouroDiretoCompra.Visible = true;
            this.pnlTesouroDiretoCompraBoleta.Visible = false;
            this.pnlTesouroDiretoCompraCesta.Visible = false;

            this.LimparBoleta();
            */

            VoltarParaTelaDeBusca();
        }

        protected void btnCesta_ConfirmarCompra_Click(object sender, EventArgs e)
        {
            ConfirmarCompra();
        }

        #endregion
    }
}
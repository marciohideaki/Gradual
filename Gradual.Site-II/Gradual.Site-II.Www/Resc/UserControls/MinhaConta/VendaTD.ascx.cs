using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Venda;
using Gradual.OMS.Library;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using System.Globalization;
using System.Data;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class VendaTD : UCTesouroDireto
    {
        #region Globais

        private const string ASCENDING = " ASC";

        private const string DESCENDING = " DESC";

        #endregion

        #region Propriedades

        private SortDirection VendaGridTitulosDisponiveis_SortDirection
        {
            get
            {
                if (ViewState["GridListaTitulosDisponiveis_SortDirection"] == null)
                    ViewState["GridListaTitulosDisponiveis_SortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["GridListaTitulosDisponiveis_SortDirection"];
            }

            set
            {
                ViewState["GridListaTitulosDisponiveis_SortDirection"] = value;
            }
        }

        #endregion

        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnRealizarBusca.Enabled = false;

                btnVerCesta.Visible = false;

                VerificarDisponibilidade();

                CarregarTiposDeTituloEIndexadores();

                btnRealizarBusca.Enabled = true;

                btnVerCesta.Visible = true;
            }
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.IdentificadorDaPagina = "Venda";

            //this.PaginaMaster.Secao = new SecaoDoSite("MinhaConta", "MinhaConta");

            //this.PaginaMaster.SubSecao = new SecaoDoSite("Tesouro Direto", HostERaizFormat("MinhaConta/TesouroDireto/Venda.aspx"));
        }

        protected void btnBoleta_CalcularQuantidade_Click(object sender, EventArgs e)
        {
            CalcularQuantidade();
        }

        protected void btnVerCesta_Click(object sender, EventArgs e)
        {
            MostrarCesta();
        }

        protected void grdCesta_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.grdCesta.EditIndex = e.NewEditIndex;
            this.grdCesta.DataSource = ViewState["ListaGridCestas"] as List<TituloMercadoInfo>;
            this.grdCesta.DataBind();
        }

        protected void grdCesta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.grdCesta.EditIndex = -1;
            this.grdCesta.DataSource = ViewState["ListaGridCestas"] as List<TituloMercadoInfo>;
            this.grdCesta.DataBind();
        }

        protected void grdCesta_RowUpdated1(object sender, GridViewUpdatedEventArgs e) { }

        protected void grdCesta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int lCodigo = (grdCesta.Rows[e.RowIndex].FindControl("hdObjRef") as HiddenField).Value.DBToInt32();

                List<TituloMercadoInfo> lTitulos = new List<TituloMercadoInfo>();

                lTitulos.Add(new TituloMercadoInfo() { CodigoTitulo = lCodigo });

                VendaExcluiItemCestaRequest lRequest = new VendaExcluiItemCestaRequest();
                VendaExcluiItemCestaResponse lServicoTesouro;

                lRequest.CodigoMercado = this.Mercado;
                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoCesta = ViewState["Cesta"].DBToInt32();
                lRequest.Titulos = lTitulos;

                lServicoTesouro = this.ServicoTesouro.VendaExcluirItemCesta(lRequest);

                if (!RespostaDoWebServiceSemErros(lServicoTesouro))
                    return;

                this.MostrarCesta();

                GlobalBase.ExibirMensagemJsOnLoad("I", "Título removido com sucesso!");
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao tentar excluir o item da cesta.");

                gLogger.Error("[Falha ao tentar excluir o item da cesta.]", ex);

                return;
            }
        }

        protected void grdCesta_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                String lNovoValor = (this.grdCesta.Rows[e.RowIndex].FindControl("txtQuantidade") as TextBox).Text;

                if (lNovoValor == "")
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", "Entre com o quantidade");

                    (this.grdCesta.Rows[e.RowIndex].FindControl("txtQuantidade") as TextBox).Focus();

                    return;
                }

                if (!System.Text.RegularExpressions.Regex.Match(lNovoValor.Replace(",", "."), @"^[-+]?\d*\.?\d*$").Success)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", "Campo quantidade inválido.");

                    (this.grdCesta.Rows[e.RowIndex].FindControl("txtQuantidade") as TextBox).Focus();

                    return;
                }

                if (lNovoValor.DBToDouble() == 0)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", "A quantidade deve ser maior que zero.");

                    (this.grdCesta.Rows[e.RowIndex].FindControl("txtQuantidade") as TextBox).Focus();

                    return;
                }

                List<TituloMercadoInfo> lCestas = ViewState["ListaGridCestas"] as List<TituloMercadoInfo>;

                int lReferencia = (this.grdCesta.Rows[e.RowIndex].FindControl("hdObjRef") as HiddenField).Value.DBToInt32();

                if (lCestas != null && lCestas.Count > 0)
                    lCestas.Single(c => c.CodigoTitulo == lReferencia).QuantidadeVenda = lNovoValor.DBToDouble();

                VendaAlteraItemDaCestaRequest lRequest = new VendaAlteraItemDaCestaRequest();
                VendaAlteraItemDaCestaResponse lResponse;

                lRequest.CodigoMercado = this.Mercado;
                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoCesta = ViewState["Cesta"].DBToInt32();
                lRequest.Titulos = lCestas;

                lResponse = this.ServicoTesouro.VendaAlterarItensCesta(lRequest);

                if (!RespostaDoWebServiceSemErros(lResponse))
                {
                    lCestas.Single(c => c.CodigoTitulo == lReferencia).QuantidadeVenda = lCestas.Single(c => c.CodigoTitulo == lReferencia).QuantidadeDisponivelVenda;

                    return;
                }

                this.grdCesta.EditIndex = -1;
                this.ViewState["ListaGridCestas"] = lCestas;
                this.MostrarCesta();
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao alterar o item da cesta.");

                gLogger.Error("[Falha ao alterar o item da cesta.]", ex);

                return;
            }
        }

        protected void btnBoleta_CalcularTotal_Click(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        protected void btnBoleta_IncluirTituloNaCesta_Click(object sender, EventArgs e)
        {
            IncluirTituloNaCesta();
        }

        protected void btnBoleta_Voltar_Click(object sender, EventArgs e)
        {
            LimparBoleta();

            this.Venda_Views.ActiveViewIndex = 0;
        }

        protected void btnCesta_ConfirmarVenda_Click(object sender, EventArgs e)
        {
            ConfirmarVenda();
        }

        protected void btnCesta_Desistir_Click(object sender, EventArgs e)
        {
            DesistirDaVenda();
        }

        protected void btnCesta_AdicionarMais_Click(object sender, EventArgs e)
        {
            this.Venda_Views.ActiveViewIndex = 0;
            this.LimparBoleta();
        }

        protected void btnCesta_Voltar_Click(object sender, EventArgs e)
        {
            this.LimparBoleta();

            this.Venda_Views.ActiveViewIndex = 0;
            this.btnCesta_Voltar.Visible = false;

            this.btnCesta_ConfirmarVenda.Visible = true;
            this.btnCesta_Desistir.Visible = true;
            this.btnCesta_AdicionarMais.Visible = true;
        }

        protected void grdTitulosDisponiveis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VendaGridTitulosDisponiveis.PageIndex = e.NewPageIndex;

            string lSortExpression = ViewState["GridListaTitulos_SortExpression"] != null ? (String)ViewState["GridListaTitulos_SortExpression"] : "";

            if (lSortExpression != "")
            {
                if (VendaGridTitulosDisponiveis_SortDirection == SortDirection.Ascending)
                {
                    SortConsulta_GridTitulosDisponiveis(lSortExpression, ASCENDING);
                }
                else
                {
                    SortConsulta_GridTitulosDisponiveis(lSortExpression, DESCENDING);
                }
            }
            else
            {
                List<TituloMercadoInfo> lNewsList = ViewState["VendaListTitMercado"] as List<TituloMercadoInfo>;

                VendaGridTitulosDisponiveis.DataSource = lNewsList;
                VendaGridTitulosDisponiveis.DataBind();
            }
        }

        protected void grdTitulosDisponiveis_Sorting(object sender, GridViewSortEventArgs e)
        {
            string lSortExpression = e.SortExpression;

            ViewState["GridListaTitulos_SortExpression"] = lSortExpression;

            if (VendaGridTitulosDisponiveis_SortDirection == SortDirection.Ascending)
            {
                VendaGridTitulosDisponiveis_SortDirection = SortDirection.Descending;
                SortConsulta_GridTitulosDisponiveis(lSortExpression, DESCENDING);
            }
            else
            {
                VendaGridTitulosDisponiveis_SortDirection = SortDirection.Ascending;
                SortConsulta_GridTitulosDisponiveis(lSortExpression, ASCENDING);
            }
        }

        private void SortConsulta_GridTitulosDisponiveis(string sortExpression, string direction)
        {
            List<TituloMercadoInfo> lNewsList = ViewState["VendaListTitMercado"] as List<TituloMercadoInfo>;

            DataTable lTable = new DataTable();

            lTable.Columns.Add("ObjectID");
            lTable.Columns.Add("Tipo_Titulo");
            lTable.Columns.Add("Nome_Titulo");
            lTable.Columns.Add("Data_Vencimento");
            lTable.Columns.Add("Tipo_Indexador_Nome");
            lTable.Columns.Add("Valor_Taxa_Venda");
            lTable.Columns.Add("Valor_Venda");
            lTable.Columns.Add("Quantidade_Saldo");

            lTable.Columns["ObjectID"].DataType = System.Type.GetType("System.Int32");
            lTable.Columns["Tipo_Titulo"].DataType = System.Type.GetType("System.String");
            lTable.Columns["Nome_Titulo"].DataType = System.Type.GetType("System.String");
            lTable.Columns["Data_Vencimento"].DataType = System.Type.GetType("System.String");
            lTable.Columns["Tipo_Indexador_Nome"].DataType = System.Type.GetType("System.String");
            lTable.Columns["Valor_Taxa_Venda"].DataType = System.Type.GetType("System.Double");
            lTable.Columns["Valor_Venda"].DataType = System.Type.GetType("System.Double");
            lTable.Columns["Quantidade_Saldo"].DataType = System.Type.GetType("System.String");

            try
            {
                DataRow lRow;

                foreach (TituloMercadoInfo lTitulo in lNewsList)
                {
                    lRow = lTable.NewRow();
                    lRow["ObjectID"] = lTitulo.CodigoCesta;
                    lRow["TipoTitulo"] = lTitulo.TipoIndexadorNome;
                    lRow["NomeTitulo"] = lTitulo.NomeTitulo;
                    lRow["DataVencimento"] = lTitulo.DataVencimento;
                    lRow["TipoIndexadorNome"] = lTitulo.TipoIndexadorNome;
                    lRow["ValorTaxaVenda"] = lTitulo.ValorTaxaVenda;
                    lRow["ValorVenda"] = lTitulo.ValorVenda;
                    lRow["QuantidadeSaldo"] = lTitulo.QuantidadeDisponivelVenda;

                    lTable.Rows.Add(lRow);
                }

                DataView lView = new DataView(lTable);

                lView.Sort = sortExpression + direction;

                VendaGridTitulosDisponiveis.DataSource = lView;
                VendaGridTitulosDisponiveis.DataBind();
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar classificar as colunas.");

                gLogger.Error("[Erro ao tentar classificar as colunas.]", ex);
            }
        }

        protected void VendaGridTitulosDisponiveis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int lKey = ((GridView)(sender)).SelectedDataKey.Value.DBToInt32();

                TituloMercadoInfo lSelecionado = ((List<TituloMercadoInfo>)ViewState["VendaListTitMercado"]).Single(condicao => condicao.CodigoTitulo == lKey);

                ViewState["Venda_SelectedItem"] = lSelecionado;
                ViewState["TituloSelecionado"] = lSelecionado.CodigoTitulo;

                PreencherBoleta(lSelecionado);

                Venda_Views.ActiveViewIndex = 1;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao tentar selecionar títulos para venda.");

                btnRealizarBusca.Enabled = false;

                gLogger.Error("[Falha ao tentar selecionar títulos para venda.]", ex);

                return;
            }
        }

        protected void btnRealizarBusca_Click(object sender, EventArgs e)
        {
            try
            {
                List<TituloMercadoInfo> lListaParaVenda = ConsultarTitulosParaVenda();

                if (lListaParaVenda.Count == 0)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("I", "Sem títulos para Venda");
                }

                VendaGridTitulosDisponiveis.DataSource = lListaParaVenda;
                VendaGridTitulosDisponiveis.DataBind();

                ViewState["VendaListTitMercado"] = lListaParaVenda;

                Venda_Views.ActiveViewIndex = 0;

                btnRealizarBusca.Enabled = true;
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar consultar título.");

                gLogger.Error("[Erro ao tentar consultar título.]", ex);
            }
        }
        #endregion
        #region Metodos Private

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

        private List<TituloMercadoInfo> ConsultarTitulosParaVenda()
        {
            List<TituloMercadoInfo> lRetorno = new List<TituloMercadoInfo>();

            try
            {
                VendaConsultaTituloDeVendaRequest lRequest = new VendaConsultaTituloDeVendaRequest();
                VendaConsultaTituloDeVendaResponse lResponse;

                int lIndexador = cboIndexadores.SelectedValue != "" ? int.Parse(cboIndexadores.SelectedValue) : 0;
                int lTipo = cboTipoDeTitulo.SelectedValue != "" ? int.Parse(cboTipoDeTitulo.SelectedValue) : 0;

                string lVencimento = "";

                DateTime lDate = DateTime.MinValue;

                txtBusca_Vencimento.Text.EDataValida(out lDate);

                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.DataEmissao = DateTime.MinValue;
                lRequest.DataVencimento = lVencimento.DBToDateTime();
                lRequest.SELIC = 0;
                lRequest.ISIN = "";
                lRequest.CodigoTitulo = 0;
                lRequest.CodigoMercado = Mercado;
                lRequest.NotCesta = 0;
                lRequest.TipoIndexador = lIndexador;

                gLogger.InfoFormat("Chamando ServicoTesouro.VendaConsultarTitulo(pCodigoMercado:[{0}], pTipo:[{1}], pDataVencimento:[{2}], pTipoIndexador:[{3}])"
                                    , lRequest.CodigoMercado
                                    , lRequest.TipoIndexador
                                    , lRequest.DataVencimento
                                    , lRequest.TipoIndexador);

                lResponse = this.ServicoTesouro.VendaConsultarTitulo(lRequest);

                if (RespostaDoWebServiceSemErros(lResponse))
                {
                    lRetorno = lResponse.Objeto;
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoTesouro.VendaConsultarTitulo(pCodigoMercado:[{0}], pTipo:[{1}], pDataVencimento:[{2}], pTipoIndexador:[{3}]) > [{0}] [{1}]"
                                        , lRequest.CodigoMercado
                                        , lRequest.TipoIndexador
                                        , lRequest.DataVencimento
                                        , lRequest.TipoIndexador
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);

                    btnRealizarBusca.Enabled = false;
                }
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao consultar títulos disponiveis para venda:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Venda.aspx > ConsultarTitulosParaVenda(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                btnRealizarBusca.Enabled = false;
            }

            return lRetorno;
        }

        private void PreencherBoleta(TituloMercadoInfo pTitulo)
        {
            this.LimparBoleta();
            lblBoleta_Titulo.Text = pTitulo.NomeTitulo;
            lblBoleta_PrecoUnitario.Text = Math.Round(pTitulo.ValorVenda, 2).ToString("c"); //TODO: Verificar esse campo
            lblBoleta_DataDeVencimento.Text = pTitulo.DataVencimento.ToString("dd/MM/yyyy");
        }

        private void LimparBoleta()
        {
            lblBoleta_Titulo.Text =
            lblBoleta_PrecoUnitario.Text =
            lblBoleta_DataDeVencimento.Text =
            txtBoleta_Quantidade.Text =
            txtBoleta_ValorDaVenda.Text =
            txtBoleta_TaxaCBLC.Text =
            txtBoleta_TaxaAgCustodia.Text =
            txtBoleta_ValorTotalDaVenda.Text = "";

            btnCesta_Voltar.Visible = false;
        }

        private bool BoletaDeVendaEstaValida()
        {
            if (string.IsNullOrEmpty(txtBoleta_Quantidade.Text.Trim()))
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Entre com a quantidade.");
                return false;
            }
            else
            {
                decimal lQuantidade;

                if (!decimal.TryParse(txtBoleta_Quantidade.Text, System.Globalization.NumberStyles.Any, new CultureInfo("pt-BR"), out lQuantidade))
                {
                    GlobalBase.ExibirMensagemJsOnLoad("I", "Quantidade inválida.");
                }
                else
                {
                    decimal lResto = lQuantidade % 0.1M;
                    
                    //if (txtBoleta_Quantidade.Text[txtBoleta_Quantidade.Text.Length - 1] != '0')
                    if (lResto != 0)
                    {
                        GlobalBase.ExibirMensagemJsOnLoad("I", "A quantidade deve ser múltipla de 0,1.");
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        private void CalcularTotal()
        {
            try
            {
                txtBoleta_ValorDaVenda.Text =
                txtBoleta_TaxaCBLC.Text =
                txtBoleta_TaxaAgCustodia.Text =
                txtBoleta_ValorTotalDaVenda.Text = "";

                if (!BoletaDeVendaEstaValida())
                    return;

                TituloMercadoInfo lItemSelecionado = (TituloMercadoInfo)ViewState["Venda_SelectedItem"];

                VendaConsultaValidadeDeTaxaProvisoriaRequest lRequest = new VendaConsultaValidadeDeTaxaProvisoriaRequest();
                VendaConsultaValidadeDeTaxaProvisoriaResponse lResponse;

                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoTitulo = lItemSelecionado.CodigoTitulo;
                lRequest.Quantidade = txtBoleta_Quantidade.Text.DBToDouble();

                lResponse = this.ServicoTesouro.VendaConsultarValidadeTaxaProvisoria(lRequest);

                if (!RespostaDoWebServiceSemErros(lResponse))
                {
                    btnBoleta_IncluirTituloNaCesta.Enabled = false;

                    return;
                }

                decimal lValorUnitario, lQuantidade, lTaxaCBLC, lTaxaAgCustodia, lResultado;

                txtBoleta_TaxaCBLC.Text = lResponse.Taxas[0].TaxaCBLC.ToString("C2");
                txtBoleta_TaxaAgCustodia.Text = lResponse.Taxas[0].TaxaCorretor.ToString("C2");

                lValorUnitario = lItemSelecionado.ValorVenda;

                lQuantidade = 0;
                lTaxaCBLC = 0;
                lTaxaAgCustodia = 0;

                lQuantidade = decimal.Parse(txtBoleta_Quantidade.Text);
                lTaxaCBLC = decimal.Parse(txtBoleta_TaxaCBLC.Text.Replace("R$", "").Replace(" ", ""));
                lTaxaAgCustodia = decimal.Parse(txtBoleta_TaxaAgCustodia.Text.Replace("R$", "").Replace(" ", ""));

                lResultado = lValorUnitario * lQuantidade;

                txtBoleta_ValorDaVenda.Text = String.Format("{0:c}", System.Math.Round(lResultado, 2));
                txtBoleta_ValorTotalDaVenda.Text = String.Format("{0:c}", System.Math.Round((lResultado + lTaxaAgCustodia + lTaxaCBLC), 2));

            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao tentar consultar taxas a serem cobradas.");

                btnBoleta_IncluirTituloNaCesta.Enabled = false;

                gLogger.Error("[Falha ao tentar consultar taxas a serem cobradas.]", ex);

                return;
            }

            btnBoleta_IncluirTituloNaCesta.Enabled = true;
        }

        private void CalcularQuantidade()
        {
            try
            {
                if (txtBoleta_ValorTotalDaVenda.Text.Replace(" ", "") == "")
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", "Entre com o valor total.");

                    return;
                }

                TituloMercadoInfo lSelecionado = (TituloMercadoInfo)ViewState["Venda_SelectedItem"];

                VendaConsultaValidadeDeTaxaProvisoriaRequest lRequest = new VendaConsultaValidadeDeTaxaProvisoriaRequest();
                VendaConsultaValidadeDeTaxaProvisoriaResponse lServicoTesouro;

                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoTitulo = lSelecionado.CodigoTitulo;
                lRequest.Quantidade = txtBoleta_Quantidade.Text.DBToInt32();

                lServicoTesouro = this.ServicoTesouro.VendaConsultarValidadeTaxaProvisoria(lRequest);

                if (!RespostaDoWebServiceSemErros(lServicoTesouro))
                    return;

                //CALCULOS

                string lValorAux = txtBoleta_ValorTotalDaVenda.Text.Replace("R$", "").Replace(" ", "").Replace(".", "");
                decimal lTotalDesejado = decimal.Parse(lValorAux.Replace(".", ","));
                decimal lValorDaCompra = lSelecionado.ValorVenda;
                decimal lQtdEstimada = lTotalDesejado / lValorDaCompra;
                String lValorEstimador = lQtdEstimada.ToString();
                String[] lCasas = lValorEstimador.Split(',');

                if (lCasas.Length > 1)
                {
                    int lCasaPosVirgula = int.Parse(lCasas[1].ToCharArray().GetValue(0).ToString());

                    if ((lCasaPosVirgula % 2) != 0)
                        lCasaPosVirgula--;

                    String lNovoValor = lCasas[0] + "," + lCasaPosVirgula;

                    lQtdEstimada = decimal.Parse(lNovoValor);
                }

                CalcularQuantidade(lQtdEstimada, lValorDaCompra, lServicoTesouro, lTotalDesejado);
            }
            catch (Exception)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar calcular a quantidade.");
            }
        }

        private void CalcularQuantidade(decimal pQuantidadeEstimada, decimal pValorCompra, VendaConsultaValidadeDeTaxaProvisoriaResponse pResponseCalc, decimal pTotalDesejado)
        {
            decimal lValorCBLC, lValorAC, lTaxasEstimadaCBLC, lTaxasEstimadaAC, lTaxasEstimadas, lTotalEstimado;

            lValorCBLC = pResponseCalc.Taxas[0].TaxaCBLC;
            lValorAC = pResponseCalc.Taxas[0].TaxaCorretor;

            lTaxasEstimadaCBLC = (lValorCBLC * pQuantidadeEstimada);
            lTaxasEstimadaAC = (lValorAC * pQuantidadeEstimada);

            lTaxasEstimadas = lTaxasEstimadaCBLC + lTaxasEstimadaAC;
            lTotalEstimado = (pQuantidadeEstimada * pValorCompra) + lTaxasEstimadas;

            if (lTotalEstimado > pTotalDesejado)
            {
                pQuantidadeEstimada = (pQuantidadeEstimada - 0.2M);

                CalcularQuantidade(pQuantidadeEstimada, pValorCompra, pResponseCalc, pTotalDesejado);
            }
            else
            {
                txtBoleta_ValorTotalDaVenda.Text = lTotalEstimado.ToString("c");
                txtBoleta_Quantidade.Text = pQuantidadeEstimada.ToString().Replace(".", ",");
                txtBoleta_TaxaCBLC.Text = lTaxasEstimadaCBLC.ToString("c");
                txtBoleta_TaxaAgCustodia.Text = lTaxasEstimadaAC.ToString("c");
                txtBoleta_ValorDaVenda.Text = (pQuantidadeEstimada * pValorCompra).ToString("c");
            }
        }

        private void MostrarCesta()
        {
            try
            {
                if (ViewState["Cesta"] == null)
                {
                    VendaConsultaCestaRequest lRequestConsultaCesta = new VendaConsultaCestaRequest();
                    VendaConsultaCestaResponse lResponseConsultaCesta;

                    lRequestConsultaCesta.CodigoMercado = this.Mercado;
                    lRequestConsultaCesta.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequestConsultaCesta.CodigoCesta = 0;
                    lRequestConsultaCesta.CodigoTitulo = 0;
                    lRequestConsultaCesta.DataRecompra = DateTime.MinValue;

                    lResponseConsultaCesta = this.ServicoTesouro.VendaConsultarCesta(lRequestConsultaCesta);

                    if (!RespostaDoWebServiceSemErros(lResponseConsultaCesta))
                        return;

                    if (lResponseConsultaCesta.Objeto.Count <= 0)
                    {
                        this.btnCesta_Desistir.Visible = false;
                        this.btnCesta_ConfirmarVenda.Visible = false;

                        GlobalBase.ExibirMensagemJsOnLoad("I", "Você não possui nenhuma cesta de vendas aberta, inclua algum título para abrir uma nova cesta.");

                        return;
                    }

                    ViewState["Cesta"] = lResponseConsultaCesta.Objeto[0].CodigoCesta;

                    this.btnCesta_ConfirmarVenda.Visible = true;
                    this.btnCesta_Desistir.Visible = true;
                }

                VendaConsultaCestaItensRequest lRequestConsultaItens = new VendaConsultaCestaItensRequest();
                VendaConsultaCestaItensResponse lResponseConsultaItens;

                lRequestConsultaItens.CodigoCesta = ViewState["Cesta"].DBToInt32();
                lRequestConsultaItens.CodigoTitulo = 0;

                lResponseConsultaItens = this.ServicoTesouro.VendaConsultarItensCesta(lRequestConsultaItens);

                if (!RespostaDoWebServiceSemErros(lResponseConsultaItens))
                    return;

                if (lResponseConsultaItens.Objeto.Count > 0)
                {
                    this.grdCesta.DataSource = lResponseConsultaItens.Objeto;
                    this.grdCesta.DataBind();

                    ViewState["ListaGridCestas"] = lResponseConsultaItens.Objeto;
                }
                else
                {
                    ViewState["ListaGridCestas"] = null;

                    this.grdCesta.DataSource = new List<TituloMercadoInfo>();
                    this.grdCesta.DataBind();
                }

                this.btnCesta_Desistir.Visible = true;
                this.btnCesta_ConfirmarVenda.Visible = true;

            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao tentar consultar os itens da cesta.");

                gLogger.Error("[Falha ao tentar consultar os itens da cesta.]", ex);

                return;
            }

            Venda_Views.ActiveViewIndex = 2;

        }

        private bool CancelarCesta(out MensagemResponseBase pMensagemDeRetorno)
        {
            pMensagemDeRetorno = new MensagemResponseBase();

            try
            {
                VendaExcluiItemCestaRequest lRequest = new VendaExcluiItemCestaRequest();
                VendaExcluiCestaResponse lResponse;

                lRequest.CodigoMercado = this.Mercado;
                lRequest.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequest.CodigoCesta = ViewState["Cesta"].DBToInt32();

                lResponse = this.ServicoTesouro.VendaExcluirCesta(lRequest);

                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoTesouro.VendaExcluirCesta(CodigoMercado: [{0}], CPFNegociador: [{1}], CodigoCesta: [{2}]) em TesouroDireto\\Venda.aspx > FinalizaCesta() > [{3}]\r\n{4}"
                                        , lRequest.CodigoMercado
                                        , lRequest.CPFNegociador
                                        , lRequest.CodigoCesta
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }

                ViewState["Cesta"] = null;
            }
            catch (System.Exception ex)
            {
                gLogger.ErrorFormat("Erro em TesouroDireto\\Venda.aspx > FinalizaCesta() > [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar finalizar a cesta.");

                return false;
            }
            return true;
        }

        private void IncluirTituloNaCesta()
        {
            if (!BoletaDeVendaEstaValida())
                return;

            string lCesta = "";

            VendaConsultaCestaResponse lResponseConsultaCesta = new VendaConsultaCestaResponse();
            VendaConsultaCestaRequest lRequestConsultaCesta = new VendaConsultaCestaRequest();

            try
            {
                lRequestConsultaCesta.CodigoMercado = this.Mercado;
                lRequestConsultaCesta.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequestConsultaCesta.CodigoCesta = 0;
                lRequestConsultaCesta.CodigoTitulo = 0;
                lRequestConsultaCesta.DataRecompra = DateTime.MinValue;

                lResponseConsultaCesta = this.ServicoTesouro.VendaConsultarCesta(lRequestConsultaCesta);

                if (!RespostaDoWebServiceSemErros(lResponseConsultaCesta))
                    return;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao verificar a existência da cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Venda.aspx > IncluirTituloNaCesta() > ServicoTesouro.ConsultarTipoTitulo(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                return;
            }

            try
            {
                if (lResponseConsultaCesta.Objeto.Count > 0)
                {
                    lCesta = lResponseConsultaCesta.Objeto[0].CodigoCesta;

                    if (lResponseConsultaCesta.Objeto.Where(c => c.CodigoTitulo.DBToInt32() == ViewState["TituloSelecionado"].DBToInt32()).Count() > 0)
                    {
                        GlobalBase.ExibirMensagemJsOnLoad("E", "Você ja possui esse título em sua cesta");
                        return;
                    }
                }
                else
                {
                    VendaInsereNovaCestaRequest lRequestNovaCesta = new VendaInsereNovaCestaRequest();
                    VendaInsereNovaCestaResponse lServicoTesouroInserirCesta;

                    lRequestNovaCesta.CodigoMercado = this.Mercado;
                    lRequestNovaCesta.CPFNegociador = SessaoClienteLogado.CpfCnpj;

                    lServicoTesouroInserirCesta = this.ServicoTesouro.VendaInserirCesta(lRequestNovaCesta);

                    if (!RespostaDoWebServiceSemErros(lServicoTesouroInserirCesta))
                        return;

                    lCesta = lServicoTesouroInserirCesta.Cesta;

                    ViewState["Cesta"] = lCesta;
                }

                VendaInscereItemNaCestaRequest lRequestInserirItem = new VendaInscereItemNaCestaRequest();
                VendaInsereItemNaCestaResponse lServicoTesouroInserirItemCesta;

                lRequestInserirItem.CodigoMercado = this.Mercado;
                lRequestInserirItem.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                lRequestInserirItem.CodigoCesta = lCesta.DBToInt32();
                lRequestInserirItem.TituloCodigoTitulo = ViewState["TituloSelecionado"].DBToInt32();
                lRequestInserirItem.TituloQuantidadeVenda = txtBoleta_Quantidade.Text.DBToDouble();

                lServicoTesouroInserirItemCesta = this.ServicoTesouro.VendaInserirItensCesta(lRequestInserirItem);

                if (!RespostaDoWebServiceSemErros(lServicoTesouroInserirItemCesta))
                    return;

                this.MostrarCesta();

                btnBoleta_IncluirTituloNaCesta.Enabled = true;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao inserir o item na cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Venda.aspx > IncluirTituloNaCesta() > ServicoTesouro.ConsultarTipoTitulo(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }
        }

        private void ConfirmarVenda()
        {
            if (base.ValidarAssinaturaEletronica(txtAssinaturaEletronica.Text))
            {
                try
                {
                    VendaVerificaTituloMercadoRequest lRequestVerificacao = new VendaVerificaTituloMercadoRequest();
                    VendaVerificaTituloMercadoResponse lResponseVerificacao;

                    lRequestVerificacao.CodigoMercado = this.Mercado;
                    lRequestVerificacao.CodigoCesta = ViewState["Cesta"].DBToInt32();

                    lResponseVerificacao = this.ServicoTesouro.VendaVerificarTituloMercado(lRequestVerificacao);

                    if (!RespostaDoWebServiceSemErros(lResponseVerificacao))
                        return;

                    if (lResponseVerificacao.Objeto.Count > 0)
                    {
                        MostrarCesta();

                        GlobalBase.ExibirMensagemJsOnLoad("I", "Alguns títulos não estão mais disponíveis no mercado e foram removidos.\r\nVerifique sua cesta e confirme a compra novamente caso desejar.");

                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao verificar titulos da cesta:\r\n{0}", ex.Message), false, ex.StackTrace);

                    gLogger.ErrorFormat("Erro em TesouroDireto/Venda.aspx > ConfirmarVenda: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                    return;
                }

                try
                {
                    List<TituloMercadoInfo> lListaDeTitulos = ViewState["ListaGridCestas"] as List<TituloMercadoInfo>;

                    VendaFechaCestaRequest lRequestVenda = new VendaFechaCestaRequest();
                    VendaFechaCestaResponse lRequesponseVenda;

                    string lTitulosPorExtenso = "";

                    if (lListaDeTitulos == null || lListaDeTitulos.Count == 0)
                    {
                        GlobalBase.ExibirMensagemJsOnLoad("I", "Você não possui itens na cesta.");

                        return;
                    }

                    List<TituloMercadoInfo> lTitulos = new List<TituloMercadoInfo>();

                    foreach (TituloMercadoInfo lTitulo in lListaDeTitulos)
                    {
                        lTitulos.Add(new TituloMercadoInfo() { CodigoTitulo = lTitulo.CodigoTitulo, QuantidadeVenda = lTitulo.QuantidadeVenda.DBToDecimal().DBToDouble() });

                        lTitulosPorExtenso += string.Format("[Codigo {0}, Quantidade {1}], ", lTitulo.CodigoTitulo, lTitulo.QuantidadeCompra);
                    }

                    lTitulosPorExtenso = lTitulosPorExtenso.TrimEnd().TrimEnd(',');

                    lRequestVenda.CodigoMercado = this.Mercado;
                    lRequestVenda.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequestVenda.CodigoCesta = ViewState["Cesta"].DBToInt32();
                    lRequestVenda.Titulos = lTitulos;

                    gLogger.InfoFormat("Chamando ServicoTesouro.VendaFecharCesta(pMercado: [{0}], pCodigoCesta: [{1}], pCPF: [{2}], pTitulos: [{3}])"
                                        , lRequestVenda.CodigoMercado
                                        , lRequestVenda.CodigoCesta
                                        , lRequestVenda.CPFNegociador
                                        , lTitulosPorExtenso);

                    lRequesponseVenda = this.ServicoTesouro.VendaFecharCesta(lRequestVenda);

                    if (RespostaDoWebServiceSemErros(lRequesponseVenda))
                    {
                        string lProtocolo = ViewState["Cesta"].ToString();

                        gLogger.InfoFormat("Resposta OK de ServicoTesouro.VendaFecharCesta(); Protocolo: [{0}]", lProtocolo);

                        this.MostrarCesta();

                        this.btnCesta_ConfirmarVenda.Visible = false;
                        this.btnCesta_Desistir.Visible = false;
                        this.btnCesta_AdicionarMais.Visible = false;
                        this.btnCesta_Voltar.Visible = true;

                        GlobalBase.ExibirMensagemJsOnLoad("I", string.Format("Venda concluída com sucesso, o número do protocolo (cesta) é: {0}", lProtocolo));
                    }
                }
                catch (Exception ex)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", string.Format("Erro ao fechar a cesta de venda:\r\n{0}", ex.Message), false, ex.StackTrace);

                    gLogger.ErrorFormat("Erro em TesouroDireto/Venda.aspx > ConfirmarVenda: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void DesistirDaVenda()
        {
            MensagemResponseBase lResponse;

            if (CancelarCesta(out lResponse))
            {
                if (!RespostaDoWebServiceSemErros(lResponse))
                    return;

                GlobalBase.ExibirMensagemJsOnLoad("I", "Venda cancelada com sucesso.");

                this.grdCesta.DataSource = new List<TituloMercadoInfo>();
                this.grdCesta.DataBind();

                this.btnCesta_ConfirmarVenda.Visible = false;
                this.btnCesta_Desistir.Visible = false;
                this.btnCesta_AdicionarMais.Visible = false;

                this.btnCesta_Voltar.Visible = true;
            }
        }

        #endregion
    }
}
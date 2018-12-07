using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.TesouroDireto.Lib;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Compra;
using Gradual.OMS.Library;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.CadastroInvestidor;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class ConsultaTD : UCTesouroDireto
    {
        #region Propriedades

        private IServicoTesouroDireto ServicoTesouro
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoTesouroDireto>();
            }
        }

        public int Mercado
        {
            get
            {
                return ((ConsultasConsultaMercadoResponse)ViewState["Mercado"]).CodigoMercado;
            }
        }

        public string CPFdoCliente
        {
            get
            {
                return base.SessaoClienteLogado.CpfCnpj;
            }
        }

        public DateTime DataVencimento
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(txtVencimento.Text, out lRetorno);

                return lRetorno;
            }
        }

        public int TipoTitulo
        {
            get
            {
                return cboTipoTitulo.SelectedValue != "-" ? cboTipoTitulo.SelectedValue.DBToInt32() : 0;
            }
        }

        public int Indexador
        {
            get
            {
                return cboIndexadores.SelectedValue != "-" ? cboIndexadores.SelectedValue.DBToInt32() : 0;
            }
        }

        private SortDirection GridListaTitulosDisponiveis_SortDirection
        {
            get
            {
                if (ViewState["GridListaTitulosDisponiveis_SortDirection"] == null)
                    ViewState["GridListaTitulosDisponiveis_SortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["GridListaTitulosDisponiveis_SortDirection"];
            }

            set { ViewState["GridListaTitulosDisponiveis_SortDirection"] = value; }
        }

        #endregion

        #region Métodos Private

        private void CarregarDados()
        {
            btnConsultar.Enabled = false;

            //Busca o Mercado
            try
            {
                ConsultasConsultaMercadoResponse lResponseConsultaMercado;

                lResponseConsultaMercado = this.ServicoTesouro.ConsultarMercado(new ConsultasConsultaMercadoRequest());

                if (!MostrarWSErro(lResponseConsultaMercado))
                    return;

                ViewState["Mercado"] = lResponseConsultaMercado;

                if (lResponseConsultaMercado.CodigoMercado == -1)
                {
                    GlobalBase.ExibirMensagemJsOnLoad("E", "Mercado Fechado.");
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao tentar consultar o mercado.");

                gLogger.Error("[Falha ao tentar consultar o mercado.]", ex);

                return;
            }

            //Segundo a documentação: 
            //  verifica se o Agente de custódia poderá 
            //  iniciar uma compra para um determinado investidor
            try
            {
                CompraVerificacaoDeCondicaoDeCompraRequest lRequestVerificacaoCondicao = new CompraVerificacaoDeCondicaoDeCompraRequest();
                CompraVerificacaoDeCondicaoDeCompraResponse lResponseVerificacaoCondicao;

                lResponseVerificacaoCondicao = this.ServicoTesouro.CompraVerificarCondicaoDeCompra(new CompraVerificacaoDeCondicaoDeCompraRequest()
                {
                    ConsultaCPFNegociador = this.CPFdoCliente,
                    ConsultaMercado = this.Mercado,
                });

                if (lResponseVerificacaoCondicao.DescricaoResposta != null && lResponseVerificacaoCondicao.DescricaoResposta.Contains("2147219991"))
                {
                    this.ExibirMensagemDeUsuarioNaoCadastrado();
                    return;
                }

                if (!MostrarWSErro(lResponseVerificacaoCondicao))
                    return;

                //Se o investidor não estiver habilitado. fazer a habilitação
                if (lResponseVerificacaoCondicao.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    base.ExibirMensagemJsOnLoad("I", "Você ainda não está habilitado para operar no Tesouro Direto. Por favor, entre em contato com a nossa Central de Atendimento.");
                    //if (!HabilitarInvestidor())
                    //{
                    //    base.ExibirMensagemJsOnLoad("I", "Não foi possível fazer a habilitação do usuário no sistema.<br />" + "Entre em contato com a Gradual Investimentos para solucionar o problema.");

                    //    return;
                    //}
                }
            }
            catch (System.Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", "Falha ao verificar situação do investidor.");

                gLogger.ErrorFormat("Erro em TesouroDireto/Consulta.aspx: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                return;
            }

            try
            {
                //--> Carrega todos os tipos de titulos disponiveis em Consulta_CBTipoTitulo

                ConsultasConsultaTipoTituloResponse lResponseConsultaTipoTitulo = this.ServicoTesouro.ConsultarTipoTitulo(new ConsultasConsultaTipoTituloRequest());

                if (!MostrarWSErro(lResponseConsultaTipoTitulo))
                    return;

                cboTipoTitulo.Items.Add(new ListItem() { Text = "Selecione", Value = "-" });

                if (null != lResponseConsultaTipoTitulo && null != lResponseConsultaTipoTitulo.Tipos && lResponseConsultaTipoTitulo.Tipos.Count > 0)
                {
                    lResponseConsultaTipoTitulo.Tipos.ForEach(lTipo =>
                    {
                        cboTipoTitulo.Items.Add(new ListItem()
                        {
                            Text = lTipo.Nome,
                            Value = lTipo.Codigo,
                        });
                    });
                }
            }
            catch (System.Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", "Falha ao tentar obter lista de tipo de títulos com o servidor.");

                gLogger.Error("[Falha ao tentar obter lista de tipo de títulos com o servidor.]", ex);

                return;
            }

            //Carrega os Tipo de indexadorees
            try
            {
                ConsultasConsultaTipoIndexadorResponse lResponseConsultaTipoIndexador = this.ServicoTesouro.ConsultarTipoIndexador(new ConsultasConsultaTipoIndexadorRequest());

                if (!MostrarWSErro(lResponseConsultaTipoIndexador))
                    return;

                cboIndexadores.Items.Add(new ListItem() { Text = "Selecione", Value = "-" });

                if (lResponseConsultaTipoIndexador != null && lResponseConsultaTipoIndexador.Indexadores != null && lResponseConsultaTipoIndexador.Indexadores.Count > 0)
                {
                    lResponseConsultaTipoIndexador.Indexadores.ForEach(lIndex =>
                    {
                        cboIndexadores.Items.Add(new ListItem()
                        {
                            Text = lIndex.Nome,
                            Value = lIndex.Codigo
                        });
                    });
                }
            }
            catch (System.Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", "Falha ao tentar obter lista de tipo de indexadores com o servidor.");

                gLogger.Error("[Falha ao tentar obter lista de tipo de indexadores com o servidor.]", ex);

                return;
            }

            btnConsultar.Enabled = true;
        }

        private Boolean MostrarWSErro(MensagemResponseBase pResposta)
        {
            if (pResposta.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                string lMensagemErro = pResposta.DescricaoResposta;

                //if (lMensagemErro.Contains("¬"))
                //    lMensagemErro = lMensagemErro.Split('¬')[1];

                if (pResposta.DescricaoResposta.Contains("-2147220910")) { lMensagemErro = "Dados informados inválidos."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217900")) { lMensagemErro = "Erro ao acessar o Banco de Dados."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220898")) { lMensagemErro = "Erro ao indentificar o cliente no sistema."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220187")) { lMensagemErro = "Mercado inválido."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220888")) { lMensagemErro = "Título inexistente no mercado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220865")) { lMensagemErro = "A(s) quantidade(s) deve(m) ser múltipla(s) de 0,2."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220912")) { lMensagemErro = "Erro ao validar os dados do cliente."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220902")) { lMensagemErro = "Valor inferior ao limite mínimo de compra."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220170")) { lMensagemErro = "Existem titulos que não fazem parte da cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220188")) { lMensagemErro = "Cesta Inválida."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220166")) { lMensagemErro = "Item inexistente na cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217873")) { lMensagemErro = "Você não pode excluir o(s) registro(s) porque o(s) mesmo(s) faz(em) parte de outra(s) tabela(s)"; }
                else if (pResposta.DescricaoResposta.Contains("-2147220173")) { lMensagemErro = "Já existe uma cesta para este negociador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220174")) { lMensagemErro = "A Cesta não pode ser alterada pois já foi fechada."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220175")) { lMensagemErro = "O CPF é diferente do CPF da cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220851")) { lMensagemErro = "Saldo Bloqueado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220852")) { lMensagemErro = "Título Bloqueado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220889")) { lMensagemErro = "Saldo Insuficiente para venda."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220890")) { lMensagemErro = "A Quantidade de venda é maior que a Quantidade Disponivel no Mercado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220891")) { lMensagemErro = "O Limite mensal de vendas foi ultrapassado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220896")) { lMensagemErro = "Mercado Fechado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220897")) { lMensagemErro = "Mercado Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220900")) { lMensagemErro = "A Quantidade a comprar e maior que a Quantidade Disponivel."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217833")) { lMensagemErro = "Um erro ocorreu. Contate o administrador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220899")) { lMensagemErro = "Investidor Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220901")) { lMensagemErro = "O Limite mensal foi ultrapassado."; }
                else { lMensagemErro = "Mercado Suspenso."; }

                base.ExibirMensagemJsOnLoad("I", lMensagemErro);

                return false;
            }

            return true;
        }

        private void ExibirMensagemDeUsuarioNaoCadastrado()
        {
            cboIndexadores.Enabled = false;
            cboTipoTitulo.Enabled = false;
            btnConsultar.Enabled = false;
            txtVencimento.Enabled = false;

            GlobalBase.ExibirMensagemJsOnLoad("I", "Caro Cliente,\r\né necesário possuir cadastro no Tesouro Direto para realizar suas operações.\r\nPara se cadastrar, entre com contato com a nossa Central de Relacionamento:\r\n- 4007-1873 (Regiões Metropolitanas)\r\n- 0800 655 1873 (Demais Regiões)");
        }

        private bool HabilitarInvestidor()
        {
            try
            {
                HabilitarInvestidorRequest lRequest = new HabilitarInvestidorRequest();
                HabilitarInvestidorResponse lResponse;

                lRequest.DataNascimento = base.SessaoClienteLogado.NascimentoFundacao.Value.ToString("MM/dd/yyyy").DBToDateTime();
                lRequest.CodigoInvestidor = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
                lRequest.DigitoInvestidor = base.ToCodigoClienteFormatado(base.SessaoClienteLogado.CodigoPrincipal).DBToInt32();
                lRequest.CPF = base.SessaoClienteLogado.CpfCnpj;
                lRequest.Email = base.SessaoClienteLogado.Email;

                lResponse = this.ServicoTesouro.HabilitarInvestidor(lRequest);

                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoTesouro.HabilitarInvestidor(DataNascimento: [{0}], CodigoInvestidor: [{1}], DigitoInvestidor: [{2}], CPF: [{3}], Email: [{4}]) em TesouroDireto\\Consulta.aspx > HabilitarInvestidor() > [{5}]\r\n{6}"
                                        , lRequest.DataNascimento
                                        , lRequest.CodigoInvestidor
                                        , lRequest.DigitoInvestidor
                                        , lRequest.CPF
                                        , lRequest.Email
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);

                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                gLogger.ErrorFormat("Erro em TesouroDireto\\Consulta.aspx > HabilitarInvestidor() > [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar habilitar investidor");

                return false;
            }
        }

        private void LimparDetalhes()
        {
            lblConsulta_DetalhesTitulo.Text = "";
            lblConsulta_DetalhesTipo.Text = "";
            lblConsulta_DetalhesDataEmissao.Text = "";
            lblConsulta_DetalhesVencimento.Text = "";
            lblConsulta_DetalhesIndexador.Text = "";
            lblConsulta_DetalhesTaxaAC.Text = "";
            lblConsulta_DetalhesTaxaCBLC.Text = "";
        }


        private void PreencherDetalhes(TituloMercadoInfo pSelecionado)
        {
            LimparDetalhes();

            lblConsulta_DetalhesTitulo.Text = pSelecionado.NomeTitulo;
            lblConsulta_DetalhesTipo.Text = pSelecionado.Tipo.Nome;
            lblConsulta_DetalhesDataEmissao.Text = pSelecionado.DataEmissao.ToString("dd/MM/yyyy");
            lblConsulta_DetalhesVencimento.Text = pSelecionado.DataVencimento.ToString("dd/MM/yyyy");
            lblConsulta_DetalhesIndexador.Text = pSelecionado.Indexador.Nome;
            lblConsulta_DetalhesISIN.Text = pSelecionado.ISIN;

            try
            {
                string lTitulo = string.Format("<TITULOS><TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO><QUANTIDADE>1.0</QUANTIDADE></TITULO></TITULOS>", pSelecionado.CodigoTitulo);

                CompraCalculaTaxaWSRequest lRequest = new CompraCalculaTaxaWSRequest();
                CompraCalculaTaxaWSResponse lResponse;

                lRequest.CodigoMercado = this.Mercado;
                lRequest.CPFNegociador = this.CPFdoCliente;
                lRequest.XMLTitulo = lTitulo;

                lResponse = this.ServicoTesouro.CompraCalcularTaxaWs(lRequest);

                if (!MostrarWSErro(lResponse))
                    return;

                lblConsulta_DetalhesTaxaAC.Text = lResponse.Objeto[0].ValorAC.ToString("C2");
                lblConsulta_DetalhesTaxaCBLC.Text = lResponse.Objeto[0].ValorCBLC.ToString("C2");
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar consultar o valor das taxas.");

                gLogger.Error("[Erro ao tentar consultar o valor das taxas.]", ex);

            }
        }

        private bool ValidarEntradaConsulta()
        {
            //É permitido passar a data em branco, portanto essa validação é feita para que o RegEx nao acuse erro

            if (txtVencimento.Text.Replace(" ", "") == "")
                return true;

            string lRegExData = @"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";

            if (!System.Text.RegularExpressions.Regex.Match(txtVencimento.Text, lRegExData).Success)
            {
                GlobalBase.ExibirMensagemJsOnLoad("I", "Formato da data inválido.");

                return false;
            }

            return true;
        }

        private List<TituloMercadoInfo> ConsultarConsTitMercado()
        {
            try
            {
                CompraConsultaTituloMercadoRequest lRequest = new CompraConsultaTituloMercadoRequest();
                CompraConsultaTituloMercadoResponse lResponse;

                lRequest.CodigoMercado = this.Mercado;
                lRequest.CodigoTitulo = 0;
                lRequest.Tipo = this.TipoTitulo;
                lRequest.DataEmissao = DateTime.MinValue;
                lRequest.DataVencimento = this.DataVencimento;
                lRequest.TipoIndexador = this.Indexador;
                lRequest.SELIC = 0;
                lRequest.ISIN = "";
                lRequest.NotCesta = 0;

                lResponse = this.ServicoTesouro.CompraConsultarTituloMercado(lRequest);

                if (!MostrarWSErro(lResponse))
                {
                    btnConsultar.Enabled = false;

                    return new List<TituloMercadoInfo>();
                }

                return lResponse.Titulos;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao consultar títulos disponiveis para compra..");

                gLogger.Error("[Falha ao consultar títulos disponiveis para compra.]", ex);

                btnConsultar.Enabled = false;

                return new List<TituloMercadoInfo>();
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.ValidarSessao(4);

            if (!Page.IsPostBack)
                CarregarDados();
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            //base.PaginaMaster.Secao = new SecaoDoSite("MinhaConta", "MinhaConta");

            //base.PaginaMaster.SubSecao = new SecaoDoSite("Tesouro Direto", HostERaizFormat("MinhaConta/TesouroDireto/Consulta.aspx"));
        }

        protected void Consulta_DetalhesBtnVoltar_Click(object sender, EventArgs e)
        {
            LimparDetalhes();

            pnlDescricaoTitulo.Visible = false;
            divResultado.Visible = true;
        }

        protected void btnMinhaConta_TesouroDireto_Consulta_Resultado_DescricaoTitulo_Click(object sender, EventArgs e)
        {
            try
            {
                int lChave = ((Button)(sender)).CommandArgument.DBToInt32();

                TituloMercadoInfo lSelecionado = ((List<TituloMercadoInfo>)ViewState["ConsultaTitListTitMercado"]).Single(condicao => condicao.CodigoTitulo == lChave);

                base.ViewState["Consulta_SelectedItem"] = lSelecionado;

                PreencherDetalhes(lSelecionado);

                pnlDescricaoTitulo.Visible = true;
                divResultado.Visible = false;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Falha ao selecionar títulos para vizualização de detalhes.");

                gLogger.Error("[Falha ao selecionar títulos para vizualização de detalhes.]", ex);

                return;
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarEntradaConsulta())
                    return;

                List<TituloMercadoInfo> CompraListTitMercado = this.ConsultarConsTitMercado();

                if (CompraListTitMercado.Count > 0)
                {
                    lblAvisoTitulos.Visible = true;
                    litNenhumRegistroEncontrado.Visible = false;
                }
                else
                {
                    lblAvisoTitulos.Visible = false;
                    litNenhumRegistroEncontrado.Visible = true;
                }

                rptMinhaConta_TesouroDireto_Consulta_Resultado.DataSource = new Transporte_TesouroDireto().TraduzirLista(CompraListTitMercado);
                rptMinhaConta_TesouroDireto_Consulta_Resultado.DataBind();

                divResultado.Visible = true;
                pnlDescricaoTitulo.Visible = false;

                ViewState["ConsultaTitListTitMercado"] = CompraListTitMercado;
            }
            catch (Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar consultar título.");

                gLogger.Error("[Erro ao tentar consultar título.]", ex);
            }
        }

        protected void rptMinhaConta_TesouroDireto_Consulta_Resultado_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //--> Atribuindo o valor do código título ao botão.
            ((Button)((System.Web.UI.Control)(e.Item)).Controls[1]).CommandArgument = ((Transporte_TesouroDireto)(e.Item.DataItem)).CodigoTitulo;
        }

        #endregion
    }
}
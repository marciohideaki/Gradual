using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.TesouroDireto.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using Gradual.OMS.TesouroDireto.Lib.Dados;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class ExtratoTD : UCTesouroDireto
    {
        #region Propriedades

        private IServicoTesouroDireto ServicoTesouro
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoTesouroDireto>();
            }
        }

        public String CPFdoCliente
        {
            get
            {
                return base.SessaoClienteLogado.CpfCnpj;
            }
        }

        #endregion

        #region Métodos Private

        private void DefinirAno()
        {
            Extrato_FiltroAno.Items.Clear();
            Extrato_FiltroAno.Items.Add(new ListItem("Selecione", "-"));

            string lAno;

            for (int i = 40; i > -20; i--)
            {
                lAno = DateTime.Now.AddYears(i).ToString("yyyy");

                this.Extrato_FiltroAno.Items.Add(new ListItem(lAno, lAno));
            }
        }

        private Boolean MostraWSErro(MensagemResponseBase pResposta)
        {
            if (pResposta.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                string lMensagemErro = pResposta.DescricaoResposta;

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

                GlobalBase.ExibirMensagemJsOnLoad("E", lMensagemErro);

                return false;
            }

            return true;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.ValidarSessao(4);

            if (!Page.IsPostBack)
                this.DefinirAno();
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            //this.PaginaMaster.Secao = new SecaoDoSite("MinhaConta", "MinhaConta");

            //this.PaginaMaster.SubSecao = new SecaoDoSite("Tesouro Direto", HostERaizFormat("MinhaConta/TesouroDireto/Extrato.aspx"));
        }

        protected void btnExtrato_DetalhesBtnVoltar_Click(object sender, EventArgs e)
        {
            this.pnlResultadoExtratoDetalhes.Visible = false;
            this.pnlResultadoExtrato.Visible = true;
        }

        protected void Extrato_BtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                // ExtratoGridListaExtrato

                ConsultasConsultaExtratoMensalRequest lRequest = new ConsultasConsultaExtratoMensalRequest();
                ConsultasConsultaExtratoMensalResponse lServicoTesouro;

                lRequest.CPFNegociador = this.CPFdoCliente;

                lServicoTesouro = this.ServicoTesouro.ConsultarExtratoMensal(lRequest);

                if (!MostraWSErro(lServicoTesouro))
                    return;

                List<TituloMercadoInfo> lParcial = lServicoTesouro.Titulos;

                if (Extrato_FiltroAno.SelectedValue != "-")
                    lParcial = lServicoTesouro.Titulos.Where(c => c.DataVencimento.Year.ToString() == this.Extrato_FiltroAno.SelectedValue).ToList();

                if (Extrato_FiltroMes.SelectedValue != "-")
                    lParcial = lServicoTesouro.Titulos.Where(c => c.DataVencimento.Month.ToString() == this.Extrato_FiltroMes.SelectedValue).ToList();

                base.ViewState["ListaExtrato"] = lParcial;

                if (lParcial != null && lParcial.Count > 0)
                {
                    litNenhumRegistroEncontrado.Visible = false;
                    rptResultadoExtrato.DataSource = new Transporte_TesouroDireto_Extrato().TraduzirLista(lParcial);
                    rptResultadoExtrato.DataBind();
                }
                else
                {
                    rptResultadoExtrato.DataBind();
                    litNenhumRegistroEncontrado.Visible = true;
                }

                pnlResultadoExtratoDetalhes.Visible = false;
                pnlResultadoExtrato.Visible = true;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar consultas extrato.");

                gLogger.Error("[Erro ao tentar consultas extrato.]", ex);
            }
        }

        protected void rptResultadoExtrato_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ((LinkButton)((Control)(e.Item)).Controls[1]).Text = ((Transporte_TesouroDireto_Extrato)(e.Item.DataItem)).TituloNome;
            ((LinkButton)((Control)(e.Item)).Controls[1]).CommandArgument = ((Transporte_TesouroDireto_Extrato)(e.Item.DataItem)).CodigoTitulo;
        }

        protected void lnkTituloNome_Click(object sender, EventArgs e)
        {
            try
            {
                int lChave = ((LinkButton)(sender)).CommandArgument.DBToInt32();

                ConsultasConsultaCestaRequest lRequest = new ConsultasConsultaCestaRequest();
                ConsultasConsultaCestaResponse lResponse;

                lRequest.CodigoMercado = 0;
                lRequest.CPFNegociador = this.CPFdoCliente;
                lRequest.Situacao = 0;
                lRequest.Tipo = 0;
                lRequest.CodigoCesta = 0;
                lRequest.DataCompra = DateTime.MinValue;
                lRequest.CodigoTitulo = lChave;
                lRequest.Cliente = 0;

                lResponse = this.ServicoTesouro.ConsultarCesta(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse != null && lResponse.Titulos != null && lResponse.Titulos.Count > 0)
                    {
                        this.rptResultadoExtratoDetalhes.DataSource = new Transporte_TesouroDireto_Extrato().TraduzirListaDetalhes(lResponse.Titulos);
                        this.rptResultadoExtratoDetalhes.DataBind();

                        this.pnlResultadoExtratoDetalhes.Visible = true;
                        this.pnlResultadoExtrato.Visible = false;
                    }
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoTesouro.ConsultarCesta(CodigoMercado: [{0}], CPFNegociador: [{1}], Situacao: [{2}], Tipo: [{3}], CodigoCesta: [{4}], DataCompra: [{5}], CodigoTitulo: [{6}], Cliente: [{7}]) em TesouroDireto\\Extrato.aspx > lnkTituloNome_Click() > [{8}]\r\n{9}"
                                        , lRequest.CodigoMercado
                                        , lRequest.CPFNegociador
                                        , lRequest.Situacao
                                        , lRequest.Tipo
                                        , lRequest.CodigoCesta
                                        , lRequest.DataCompra
                                        , lRequest.CodigoTitulo
                                        , lRequest.Cliente
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em TesouroDireto\\Extrato.aspx > lnkTituloNome_Click() > [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar exibir detalhes do extrato.");

                gLogger.Error("[Erro ao tentar exibir detalhes do extrato.]", ex);
            }
        }

        #endregion
    }
}
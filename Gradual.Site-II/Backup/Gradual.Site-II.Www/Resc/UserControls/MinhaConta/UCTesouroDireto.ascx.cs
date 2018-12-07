using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using Gradual.OMS.Library;
using Gradual.OMS.TesouroDireto.Lib;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Venda;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Compra;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.CadastroInvestidor;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class UCTesouroDireto : UserControlBase
    {
        #region Propriedades

        public PaginaBase GlobalBase 
        {
            get
            {
                return this.Page as PaginaBase;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        public string IdentificadorDaPagina { get; set; }

        public IServicoTesouroDireto ServicoTesouro
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoTesouroDireto>();
            }
        }

        public int CodigoCesta
        {
            get
            {
                if (null != ViewState["Cesta"])
                    return ViewState["Cesta"].DBToInt32();

                return 0;
            }
        }

        public int Mercado
        {
            get
            {
                if (ViewState["Mercado"] is ConsultasConsultaMercadoResponse)
                    return ((ConsultasConsultaMercadoResponse)ViewState["Mercado"]).CodigoMercado;

                return 0;
            }
        }

        public void CarregarTiposEIndexadores(out List<CodigoNomeInfo> pTitulos, out List<CodigoNomeInfo> pIndexadores)
        {
            pTitulos = new List<CodigoNomeInfo>();
            pIndexadores = new List<CodigoNomeInfo>();


            //Carrega todos os tipos de titulos disponiveis em cboTipoTitulo
            try
            {
                ConsultasConsultaTipoTituloResponse lResponseConsultaTipo;

                lResponseConsultaTipo = this.ServicoTesouro.ConsultarTipoTitulo(new ConsultasConsultaTipoTituloRequest());

                if (!RespostaDoWebServiceSemErros(lResponseConsultaTipo))
                    return;

                pTitulos = lResponseConsultaTipo.Tipos;

                //cboTipoTitulo.DataSource = lResponseConsultaTipo.Tipos;
                //cboTipoTitulo.DataBind();

                //cboTipoTitulo.Items.Insert(0, new ListItem() { Text = "Selecione", Value = "-" });
            }
            catch (System.Exception ex)
            {
                ExibirMensagemJsOnLoad("E", string.Format("Erro ao obter lista de tipo de títulos:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/{0}.aspx > CarregarTiposEIndexadores() > ServicoTesouro.ConsultarTipoTitulo(): [{1}]\r\n{2}"
                                    , this.IdentificadorDaPagina
                                    , ex.Message
                                    , ex.StackTrace);

                return;
            }

            //Carrega os Tipo de indexadores
            try
            {
                ConsultasConsultaTipoIndexadorResponse lResponseConsultaIndexador;

                lResponseConsultaIndexador = this.ServicoTesouro.ConsultarTipoIndexador(new ConsultasConsultaTipoIndexadorRequest());

                if (!RespostaDoWebServiceSemErros(lResponseConsultaIndexador))
                    return;

                pIndexadores = lResponseConsultaIndexador.Indexadores;

                //cboIndexadores.DataSource = lResponseConsultaIndexador.Indexadores;
                //cboIndexadores.DataBind();

                //this.cboIndexadores.Items.Insert(0, new ListItem() { Text = "Selecione", Value = "-" });
            }
            catch (System.Exception ex)
            {
                ExibirMensagemJsOnLoad("E", string.Format("Erro ao obter lista de tipo de indexadores:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/{0}.aspx > CarregarTiposEIndexadores() > ServicoTesouro.ConsultarTipoIndexador(): [{1}]\r\n{2}"
                                    , this.IdentificadorDaPagina
                                    , ex.Message
                                    , ex.StackTrace);

                return;
            }
        }

        public bool RespostaDoWebServiceSemErros(MensagemResponseBase pResposta)
        {
            if (pResposta.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                string lMensagemErro = pResposta.DescricaoResposta;

                if (lMensagemErro.Contains("¬"))
                    lMensagemErro = lMensagemErro.Split('¬')[1];

                if (pResposta.DescricaoResposta.Contains("-2147220910")) { lMensagemErro = "Dados informados inválidos."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217900")) { lMensagemErro = "Erro ao acessar o Banco de Dados."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220898")) { lMensagemErro = "Erro ao indentificar o cliente no sistema."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220187")) { lMensagemErro = "Mercado inválido."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220888")) { lMensagemErro = "Título inexistente no mercado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220912")) { lMensagemErro = "A(s) quantidade(s) deve(m) ser múltipla(s) de 0,2."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220865")) { lMensagemErro = "Erro ao validar os dados do cliente."; }
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
                else if (pResposta.DescricaoResposta.Contains("-2147220890")) { lMensagemErro = "A Quantidade de venda é maior que a Quantidade Disponivel."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220891")) { lMensagemErro = "O Limite mensal de vendas foi ultrapassado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220896")) { lMensagemErro = "Mercado Fechado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220897")) { lMensagemErro = "Mercado Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220900")) { lMensagemErro = "A Quantidade a comprar é maior que a Quantidade Disponivel."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217833")) { lMensagemErro = "Um erro ocorreu. Contate o administrador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220899")) { lMensagemErro = "Investidor Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220901")) { lMensagemErro = "O Limite mensal foi ultrapassado."; }
                else { lMensagemErro = "Mercado Suspenso."; }

                gLogger.ErrorFormat("Resposta com erro do Web Service (função MostrarErroDoWebService()) > Mensagem: [{0}] Descrição para o usuário: [{1}]", pResposta.DescricaoResposta, lMensagemErro);

                ExibirMensagemJsOnLoad("E", lMensagemErro);

                return false;
            }

            return true;
        }
        /*
        public bool HabilitarInvestidor()
        {
            bool lRetorno = false;

            try
            {
                HabilitarInvestidorRequest lRequestHabilitacao = new HabilitarInvestidorRequest();
                HabilitarInvestidorResponse lResponseHabilitacao;

                lRequestHabilitacao.DataNascimento = SessaoClienteLogado.NascimentoFundacao.DBToDateTime();
                lRequestHabilitacao.CodigoInvestidor = SessaoClienteLogado.CodigoPrincipal.DBToInt32();
                lRequestHabilitacao.CPF = SessaoClienteLogado.CpfCnpj;
                lRequestHabilitacao.DigitoInvestidor = base.ToCodigoClienteFormatado(base.SessaoClienteLogado.CodigoPrincipal).DBToInt32();
                lRequestHabilitacao.Email = SessaoClienteLogado.Email;
                lRequestHabilitacao.IdentificacaoOperacao = 1;
                gLogger.InfoFormat("Chamando do ServicoTesouro.HabilitarInvestidor(pDataNascimento:[{0}], pCodigoInvestidor:[{1}], pCPF:[{2}], pDigitoInvestidor:[{3}], pEmail:[{4}])"
                                    , lRequestHabilitacao.DataNascimento
                                    , lRequestHabilitacao.CodigoInvestidor
                                    , lRequestHabilitacao.CPF
                                    , lRequestHabilitacao.DigitoInvestidor
                                    , lRequestHabilitacao.Email);

                lResponseHabilitacao = this.ServicoTesouro.HabilitarInvestidor(lRequestHabilitacao);

                if (lResponseHabilitacao.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    gLogger.InfoFormat("Resposta OK de ServicoTesouro.HabilitarInvestidor()");

                    lRetorno = true;
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoTesouro.HabilitarInvestidor(pDataNascimento:[{0}], pCodigoInvestidor:[{1}], pCPF:[{2}], pDigitoInvestidor:[{3}], pEmail:[{4}]) > [{5}] [{6}]"
                                        , lRequestHabilitacao.DataNascimento
                                        , lRequestHabilitacao.CodigoInvestidor
                                        , lRequestHabilitacao.CPF
                                        , lRequestHabilitacao.DigitoInvestidor
                                        , lRequestHabilitacao.Email
                                        , lResponseHabilitacao.StatusResposta
                                        , lResponseHabilitacao.DescricaoResposta);
                }
            }
            catch (System.Exception ex)
            {
                ExibirMensagemJsOnLoad("E", string.Format("Erro ao habilitar investidor:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/{0}.aspx > HabilitarInvestidor() > ServicoTesouro.HabilitarInvestidor(): [{1}]\r\n{2}"
                                    , this.IdentificadorDaPagina
                                    , ex.Message
                                    , ex.StackTrace);
            }

            return lRetorno;
        }
        */
        public void VerificarDisponibilidade()
        {
            try
            {
                // Busca o Mercado

                ConsultasConsultaMercadoResponse lConsultarMercadoResponse = this.ServicoTesouro.ConsultarMercado(new ConsultasConsultaMercadoRequest());

                if (!RespostaDoWebServiceSemErros(lConsultarMercadoResponse))
                    return;

                ViewState["Mercado"] = lConsultarMercadoResponse;

                if (lConsultarMercadoResponse.CodigoMercado == -1)
                {
                    ExibirMensagemJsOnLoad("A", "Mercado Fechado.");

                    return;
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad("E", string.Format("Falha tentar consultar o mercado:\r\n{0}", ex.Message), false, ex.StackTrace);

                gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > VerificarDisponibilidade() > ServicoTesouro.ConsultarMercado(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);

                return;
            }

            if (this.IdentificadorDaPagina.ToLower() == "compra")
            {

                //Segundo a documentação: 
                //  verifica se o Agente de custódia poderá 
                //  iniciar uma compra para um determinado investidor
                try
                {
                    CompraVerificacaoDeCondicaoDeCompraRequest lRequestDeVerificacaoDeCondicaoDeCompra = new CompraVerificacaoDeCondicaoDeCompraRequest();
                    CompraVerificacaoDeCondicaoDeCompraResponse lResponseDeVerificacaoDeCondicaoDeCompra;

                    lRequestDeVerificacaoDeCondicaoDeCompra.ConsultaCPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequestDeVerificacaoDeCondicaoDeCompra.ConsultaMercado = this.Mercado;

                    lResponseDeVerificacaoDeCondicaoDeCompra = this.ServicoTesouro.CompraVerificarCondicaoDeCompra(lRequestDeVerificacaoDeCondicaoDeCompra);

                    //Se o investidor não estiver habilitado. fazer a habilitação
                    /*
                    if (lResponseDeVerificacaoDeCondicaoDeCompra.StatusResposta != MensagemResponseStatusEnum.OK
                    && (!HabilitarInvestidor()))
                    {
                        ExibirMensagemJsOnLoad("E", "Não foi possível fazer a habilitação do usuário no sistema.\r\nEntre em contato com a Gradual Investimentos para solucionar o problema.");
                    }
                     * */
                }
                catch (System.Exception ex)
                {
                    ExibirMensagemJsOnLoad("E", string.Format("Erro ao verificar situação do investidor:\r\n{0}", ex.Message), false, ex.StackTrace);

                    gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > VerificarDisponibilidade() > ServicoTesouro.CompraVerificarCondicaoDeCompra(pCPF: [{0}], pMercado: [{1}]): [{2}]\r\n{3}"
                                        , SessaoClienteLogado.CpfCnpj
                                        , this.Mercado
                                        , ex.Message
                                        , ex.StackTrace);
                }
            }
            else
            {
                try
                {
                    VendaVerificaCondicaoDeVendaRequest lRequestCondicao = new VendaVerificaCondicaoDeVendaRequest();
                    VendaVerificaCondicaoDeVendaResponse lResponseCondicao;

                    lRequestCondicao.CPFNegociador = SessaoClienteLogado.CpfCnpj;
                    lRequestCondicao.CodigoMercado = this.Mercado;

                    lResponseCondicao = ServicoTesouro.VendaVerificarCondicao(lRequestCondicao);

                    //Se o investidor não estiver habilitado. fazer a habilitação
                    /*
                    if (lResponseCondicao.StatusResposta != MensagemResponseStatusEnum.OK
                    && (!HabilitarInvestidor()))
                    {
                        ExibirMensagemJsOnLoad("E", "Não foi possível fazer a habilitação do usuário no sistema.\r\nEntre em contato com a Gradual Investimentos para solucionar o problema.");
                    }
                    */
                }
                catch (System.Exception ex)
                {
                    ExibirMensagemJsOnLoad("E", string.Format("Erro ao verificar situação do investidor:\r\n{0}", ex.Message), false, ex.StackTrace);

                    gLogger.ErrorFormat("Erro em TesouroDireto/Compra.aspx > VerificarDisponibilidade() > ServicoTesouro.CompraVerificarCondicaoDeCompra(pCPF: [{0}], pMercado: [{1}]): [{2}]\r\n{3}"
                                        , SessaoClienteLogado.CpfCnpj
                                        , this.Mercado
                                        , ex.Message
                                        , ex.StackTrace);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Produtos
{
    public partial class TravelCard : System.Web.UI.UserControl
    {
        #region Globais

        private string gQueryStringDeEnvio = "master=GRADUAL&CPF={0}&Nome={1}&RG={2}&CEP={3}&Endereco={4}&Num={5}&Comp={6}&Bairro={7}&Cidade={8}&Estado={9}&Telefone={10}&Celular={11}&Email={12}&Sexo={13}&Profissao={14}&Renda={15}&EstadoCivil={16}&Conjuge={17}&Filiacao={18}&DataNascimento={19}&LocalNascimento={20}";

        #endregion

        #region Propriedades

        public int IdDoProduto
        {
            get
            {
                return ConfiguracoesValidadas.IdDoProduto_GradualTravelCard;
            }
        }

        private string _CpfDoUsuario = "";

        public string CpfDoUsuario
        {
            get
            {
                return _CpfDoUsuario;
            }
        }

        private string _NomeDoUsuario = "";

        public string NomeDoUsuario
        {
            get
            {
                return _NomeDoUsuario;
            }
        }

        public string HostERaizHttps
        {
            get
            {
                return ((PaginaBase)this.Page).HostERaizHttps;
            }
        }

        #endregion

        #region Métodos Private

        private TransporteSaldoDeConta BuscarSaldoEmContaNoServico()
        {
            TransporteSaldoDeConta lRetorno = new TransporteSaldoDeConta();

            try
            {
                var lBase = (PaginaBase)this.Page;

                IServicoContaCorrente lServicoCC = lBase.InstanciarServicoDoAtivador<IServicoContaCorrente>();

                SaldoContaCorrenteRequest lRequest = new SaldoContaCorrenteRequest();

                SaldoContaCorrenteResponse<ContaCorrenteInfo> lRespostaCC;

                lRequest.IdCliente = lBase.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                lRespostaCC = lServicoCC.ObterSaldoContaCorrente(lRequest);

                if (lRespostaCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(lRespostaCC.Objeto);
                }
                else
                {
                    PaginaBase.gLogger.ErrorFormat("Resposta com erro do IServicoContaCorrente.ObterSaldoContaCorrente(IdCliente: [{0}]) em TravelCard.aspx > BuscarSaldoEmContaNoServico > [{1}] \r\n{2}"
                                        , lRequest.IdCliente
                                        , lRespostaCC.StatusResposta
                                        , lRespostaCC.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                PaginaBase.gLogger.ErrorFormat("Erro em TravelCard.aspx > BuscarSaldoEmContaNoServico > [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }

            return lRetorno;
        }

        private void CarregarDadosDeCadastro()
        {
            var lBase = (PaginaBase)this.Page;

            TransporteDadosCadastraisDoCliente lDados = new TransporteDadosCadastraisDoCliente(lBase.SessaoClienteLogado);

            TransporteEndereco lEndereco = new TransporteEndereco();
            TransporteTelefone lCelular, lResidencial;

            if (lDados.Enderecos != null && lDados.Enderecos.Count > 0)
            {
                lEndereco = lDados.Enderecos[0];

                foreach (TransporteEndereco lEnd in lDados.Enderecos)
                {
                    if (lEnd.Principal)
                    {
                        lEndereco = lEnd;

                        break;
                    }
                }
            }

            if (lDados.Telefones != null && lDados.Telefones.Count > 0)
            {
                lCelular = lDados.Telefones[0];
                lResidencial = lDados.Telefones[0];

                foreach (TransporteTelefone lTel in lDados.Telefones)
                {
                    if (lTel.IdTipo == 3)
                    {
                        lCelular = lTel;
                    }

                    if (lTel.IdTipo == 1)
                    {
                        lResidencial = lTel;
                    }
                }

                gQueryStringDeEnvio = string.Format(gQueryStringDeEnvio
                                                    , lDados.CPF
                                                    , lDados.NomeCompleto
                                                    , lDados.NumeroDoDocumento
                                                    , lEndereco.Cep
                                                    , lEndereco.Logradouro
                                                    , lEndereco.Numero
                                                    , lEndereco.Complemento
                                                    , lEndereco.Bairro
                                                    , lEndereco.Cidade
                                                    , lEndereco.UF
                                                    , lResidencial.ToString()
                                                    , lCelular.ToString()
                                                    , lDados.Email
                                                    , lDados.Sexo
                                                    , lDados.Profissao
                                                    , lDados.CodigoBovespa  //TODO: RENDA!!
                                                    , lDados.EstadoCivil
                                                    , lDados.NomeDoConjuge
                                                    , lDados.NomeDaMae
                                                    , lDados.DataDeNascimento
                                                    , lDados.Naturalidade);
            }
            
            gQueryStringDeEnvio = string.Format("{0}?{1}", ConfiguracoesValidadas.TravelCard_Url, gQueryStringDeEnvio);

            hidQueryString.Text = gQueryStringDeEnvio;

            _CpfDoUsuario = lDados.CPF;
            _NomeDoUsuario = lDados.NomeCompleto;
        }

        private void CarregarDadosDoProduto()
        {
            var lBase = (PaginaBase)this.Page;

            this.pnlDadosDeCompra_ProdutoJaAdquirido.Visible =
            this.pnlDadosDeCompra_RealizarCompra.Visible =
            this.pnlDadosDeCompra_SemSaldo.Visible =
            this.pnlDadosDeCompra_SemLogin.Visible = false;

            BuscarDadosDosProdutosRequest lRequest = new BuscarDadosDosProdutosRequest();
            BuscarDadosDosProdutosResponse lResponse;

            lRequest.IdProduto = ConfiguracoesValidadas.IdDoProduto_GradualTravelCard;

            lResponse = lBase.ServicoPersistenciaSite.BuscarDadosDosProdutos(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.DadosDosProdutos.Count > 0)
                {
                    if (lBase.SessaoClienteLogado == null)
                    {
                        pnlDadosDeCompra_SemLogin.Visible = true;
                    }
                    else
                    {
                        CarregarDadosDeCadastro();

                        lBase.CarregarProdutosAdquiridosDoCliente(false);

                        if (lBase.SessaoClienteLogado.TemProduto(ConfiguracoesValidadas.IdDoProduto_GradualTravelCard))
                        {
                            pnlDadosDeCompra_ProdutoJaAdquirido.Visible = true;
                        }
                        else
                        {
                            //verifica se tem saldo para comra:

                            TransporteSaldoDeConta lSaldo = BuscarSaldoEmContaNoServico();

                            if (lSaldo.SaldoProjetado > 0)
                            {
                                pnlDadosDeCompra_RealizarCompra.Visible = true;
                            }
                            else
                            {
                                pnlDadosDeCompra_SemSaldo.Visible = true;
                                lblSaldo.Text = lSaldo.SaldoProjetado.ToString("n");
                            }
                        }
                    }
                }
                else
                {
                    PaginaBase.gLogger.ErrorFormat("Sem nenhum produto retornado em TravelCard.aspx > CarregarDadosDoProduto({0}): {1} > {2}"
                                        , ConfiguracoesValidadas.IdDoProduto_GradualTraderInterface
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }
            }
            else
            {
                PaginaBase.gLogger.ErrorFormat("Resposta com erro do serviço em TravelCard.aspx > CarregarDadosDoProduto(): {0} > {1}", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }

        }

        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //foi pedido pra retirar a parte de compra, vamos só comentar e deixar invisível por enquanto...
                //this.CarregarDadosDoProduto();
                pnlDadosDeCompra_ProdutoJaAdquirido.Visible = 
                pnlDadosDeCompra_RealizarCompra.Visible =
                pnlDadosDeCompra_SemLogin.Visible =
                pnlDadosDeCompra_SemSaldo.Visible = false;
            }
        }
        #endregion
    }
}
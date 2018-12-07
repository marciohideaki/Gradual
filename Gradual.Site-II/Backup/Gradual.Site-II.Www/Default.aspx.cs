using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using log4net;

namespace Gradual.Site.Www
{
    public partial class Default : PaginaBase
    {
        #region Propriedades

        public string SkinEmUso
        {
            get
            {
                if (Session["SkinEmUso"] == null)
                {
                    Session["SkinEmUso"] = ConfiguracoesValidadas.SkinPadrao;
                }

                return (string)Session["SkinEmUso"];
            }

            set
            {
                Session["SkinEmUso"] = value;
            }
        }

        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        #endregion

        #region Métodos Private

        private void CarregarCotacaoIGB30()
        {
            /*
            wsIndiceGradual.WSIndiceGradualInterface lWsIndice = new wsIndiceGradual.WSIndiceGradualInterface();

            string lRetorno = lWsIndice.QueryIndiceGradualString("indicegradual", "indgra2013*", 1, 1);

            TransporteCotacaoWsIndiceGradual lTransporte = new TransporteCotacaoWsIndiceGradual(lRetorno);

            lblCotacaoIBOV30_Dados_Ultima.Text   = lTransporte.Cotacao.ToNumeroFormatado();
            lblCotacaoIBOV30_Dados_Variacao.Text = lTransporte.Variacao.ToNumeroFormatado();
             */
        }

        private void CarregarCotacoes()
        {
            TransporteMensagemDeNegocio lTransporteMensagem;
            TransporteLivroDeOferta lTransporteLivro;

            string lMensagem;

            try
            {
                IServicoCotacao lServico = InstanciarServicoDoAtivador<IServicoCotacao>();

                lMensagem = lServico.ReceberTickerCotacao("IBOV");

                lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);

                lblCotacoes_Dados_Ativo.Text = lTransporteMensagem.CodigoNegocio;
                /*
                lblCotacoes_Dados_Abertura.Text         = lTransporteMensagem.ValorAbertura.ToNumeroFormatado();
                lblCotacoes_Dados_Compra.Text           = lTransporteMensagem.MelhorPrecoCompra.ToNumeroFormatado();
                lblCotacoes_Dados_Fechamento.Text       = lTransporteMensagem.ValorFechamento.ToNumeroFormatado();
                lblCotacoes_Dados_Maxima.Text           = lTransporteMensagem.MaxDia.ToNumeroFormatado();
                lblCotacoes_Dados_Minima.Text           = lTransporteMensagem.MinDia.ToNumeroFormatado();
                lblCotacoes_Dados_NumeroDeNegocios.Text = lTransporteMensagem.NumNegocio.ToNumeroFormatado(true);
                lblCotacoes_Dados_QtdCompra.Text        = lTransporteMensagem.QuantidadeAcumuladaMelhorCompra.ToNumeroFormatado();
                lblCotacoes_Dados_QtdVenda.Text         = lTransporteMensagem.QuantidadeAcumuladaMelhorVenda.ToNumeroFormatado();
                lblCotacoes_Dados_Ultima.Text           = lTransporteMensagem.Preco.ToNumeroFormatado();
                lblCotacoes_Dados_Variacao.Text         = lTransporteMensagem.Variacao.ToNumeroFormatado();
                lblCotacoes_Dados_Venda.Text            = lTransporteMensagem.MelhorPrecoVenda.ToNumeroFormatado();
                lblCotacoes_Dados_Volume.Text           = lTransporteMensagem.VolumeAcumulado.ToNumeroFormatado(true);
                
                */

                if (string.IsNullOrEmpty(lTransporteMensagem.Variacao))
                    lTransporteMensagem.Variacao = "n/d";

                if (string.IsNullOrEmpty(lTransporteMensagem.ValorAbertura))
                    lTransporteMensagem.ValorAbertura = "n/d";

                lblConteudoTerciario_CotacaoIBOV_Variacao.Text = lTransporteMensagem.Variacao;
                lblConteudoTerciario_CotacaoIBOV_Ultima.Text = lTransporteMensagem.Preco;

                lMensagem = lServico.ReceberLivroOferta("IBOV");

                lTransporteLivro = new TransporteLivroDeOferta(lMensagem);

                //deixa só as 7 primeiras ofertas:

                for (int a = lTransporteLivro.OfertasDeCompra.Count - 1; a >= 7; a--)
                {
                    lTransporteLivro.OfertasDeCompra.RemoveAt(a);
                }

                for (int a = lTransporteLivro.OfertasDeVenda.Count - 1; a >= 7; a--)
                {
                    lTransporteLivro.OfertasDeVenda.RemoveAt(a);
                }

                while (lTransporteLivro.OfertasDeCompra.Count < 7)
                {
                    lTransporteLivro.OfertasDeCompra.Add(new TransporteMensagemDeLivroDeOferta() { NomeCorretora="&nbsp;", Preco="&nbsp;", QuantidadeAbreviada="&nbsp;" });
                }

                while (lTransporteLivro.OfertasDeVenda.Count < 7)
                {
                    lTransporteLivro.OfertasDeVenda.Add(new TransporteMensagemDeLivroDeOferta() { NomeCorretora="&nbsp;", Preco="&nbsp;", QuantidadeAbreviada="&nbsp;" });
                }

                rptOfertasDeCompra.DataSource = lTransporteLivro.OfertasDeCompra;
                rptOfertasDeCompra.DataBind();

                rptOfertasDeVenda.DataSource = lTransporteLivro.OfertasDeVenda;
                rptOfertasDeVenda.DataBind();

                //Pega as outras cotações pro conteúdo terciário:

                if (!string.IsNullOrEmpty(ConfiguracoesValidadas.CodigoAtualIbovFuturo))
                {
                    try
                    {
                        lMensagem = lServico.ReceberTickerCotacao(ConfiguracoesValidadas.CodigoAtualIbovFuturo);

                        lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);

                        if (string.IsNullOrEmpty(lTransporteMensagem.Variacao))
                            lTransporteMensagem.Variacao = "n/d";

                        if (string.IsNullOrEmpty(lTransporteMensagem.ValorAbertura))
                            lTransporteMensagem.ValorAbertura = "n/d";

                        lblConteudoTerciario_CotacaoDJI_Variacao.Text = lTransporteMensagem.Variacao;
                        lblConteudoTerciario_CotacaoDJI_Ultima.Text   = lTransporteMensagem.ValorAbertura;
                    }
                    catch (Exception exA)
                    {
                        gLogger.Error(string.Format("Erro em CarregarCotacoes() ao carregar cotação para [{0}]", ConfiguracoesValidadas.CodigoAtualIbovFuturo), exA);
                    }
                }

                if (!string.IsNullOrEmpty(ConfiguracoesValidadas.CodigoAtualDolarFuturo))
                {
                    try
                    {
                        lMensagem = lServico.ReceberTickerCotacao(ConfiguracoesValidadas.CodigoAtualDolarFuturo);

                        lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);

                        if (string.IsNullOrEmpty(lTransporteMensagem.Variacao))
                            lTransporteMensagem.Variacao = "n/d";

                        if (string.IsNullOrEmpty(lTransporteMensagem.ValorAbertura))
                            lTransporteMensagem.ValorAbertura = "n/d";

                        lblConteudoTerciario_CotacaoDolar_Variacao.Text = lTransporteMensagem.Variacao;
                        lblConteudoTerciario_CotacaoDolar_Ultima.Text = lTransporteMensagem.ValorAbertura;
                    }
                    catch (Exception exB)
                    {
                        gLogger.Error(string.Format("Erro em CarregarCotacoes() ao carregar cotação para [{0}]", ConfiguracoesValidadas.CodigoAtualDolarFuturo), exB);
                    }
                }

                if (!string.IsNullOrEmpty(ConfiguracoesValidadas.CodigoAtualDIFuturo))
                {
                    try
                    {
                        lMensagem = lServico.ReceberTickerCotacao(ConfiguracoesValidadas.CodigoAtualDIFuturo);

                        lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);

                        if (string.IsNullOrEmpty(lTransporteMensagem.Variacao))
                            lTransporteMensagem.Variacao = "n/d";

                        if (string.IsNullOrEmpty(lTransporteMensagem.ValorAbertura))
                            lTransporteMensagem.ValorAbertura = "n/d";

                        lblConteudoTerciario_CotacaoDI_Variacao.Text = lTransporteMensagem.Variacao;
                        lblConteudoTerciario_CotacaoDI_Ultima.Text = lTransporteMensagem.ValorAbertura;
                    }
                    catch (Exception exC)
                    {
                        gLogger.Error(string.Format("Erro em CarregarCotacoes() ao carregar cotação para [{0}]", ConfiguracoesValidadas.CodigoAtualDIFuturo), exC);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro em CarregarCotacoes()", ex);
            }
        }

        private void CarregarOfertasPublicas()
        {
            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            lRequest.IdDaLista = ConfiguracoesValidadas.IdDaLista_OfertasPublicasParaHome;

            lResponse = base.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Itens.Count > 0)
                {
                    rptOfertasPublicas.Visible = true;

                    List<TransporteConteudoInfo_OfertaPublica> lLista = lResponse.Itens.ParaListaTipada<TransporteConteudoInfo_OfertaPublica>();

                    while (lLista.Count > 4)    //pediram pra limitar só 4 itens...
                    {
                        lLista.RemoveAt(lLista.Count - 1);  
                    }

                    rptOfertasPublicas.DataSource = lLista;
                    rptOfertasPublicas.DataBind();

                    //liOfertaPublica_NenhumItem.Visible = false;
                }
                else
                {
                    rptOfertasPublicas.Visible = false;

                    //liOfertaPublica_NenhumItem.Visible = true;
                }
            }
            else
            {
                gLogger.ErrorFormat("Resposta do serviço com erro em CarregarOfertasPublicas() na Home: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
        }

        private void CarregarBanners()
        {
            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            lRequest.IdDaLista = ConfiguracoesValidadas.IdDaLista_BannersParaHome;

            lResponse = base.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                List<TransporteConteudoInfo_BannerDaHome> lLista = lResponse.Itens.ParaListaTipada<TransporteConteudoInfo_BannerDaHome>();

                rptBanners.DataSource = lLista;
                rptBanners.DataBind();
            }
            else
            {
                gLogger.ErrorFormat("Resposta do serviço com erro em CarregarBanners() na Home: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
        }

        private void CarregarDestaques()
        {
            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            lRequest.IdDaLista = ConfiguracoesValidadas.IdDaLista_DestaquesParaHome;

            lResponse = base.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                List<TransporteConteudoInfo_DestaqueDaHome> lLista = lResponse.Itens.ParaListaTipada<TransporteConteudoInfo_DestaqueDaHome>();

                while (lLista.Count > 1)    //só mantendo a primeira
                {
                    lLista.RemoveAt(lLista.Count - 1);
                }

                rptDestaques.DataSource = lLista;
                rptDestaques.DataBind();
            }
            else
            {
                gLogger.ErrorFormat("Resposta do serviço com erro em CarregarDestaques() na Home: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }

            
            lRequest.IdDaLista = ConfiguracoesValidadas.IdDaLista_VideoDaHome;

            lResponse = base.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Itens.Count > 0)
                {
                    string lHTML = lResponse.Itens[0].ConteudoHtml;

                    lHTML = lHTML.Replace("&lt;", "<").Replace("&gt;", ">");

                    litVideoHome.Mode = LiteralMode.PassThrough;

                    litVideoHome.Text = lHTML;
                }
                else
                {
                    litVideoHome.Text = "<span data-obs='(nenhum vídeo encontrado na lista)'></span>";
                }
            }
            else
            {
                gLogger.ErrorFormat("Resposta do serviço com erro em CarregarDestaques() na Home: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
        }

        private void ConfigurarInterface()
        {
            /*
            lnkDestaque_AbraSuaConta.Visible     = false;
            lnkDestaque_CompleteCadastro.Visible = false;
            lnkDestaque_MinhaConta.Visible       = false;

            lnkDestaque_AbraSuaConta.HRef = base.HostERaizFormat("MinhaConta/Cadastro/MeuCadastro.aspx");
            lnkDestaque_CompleteCadastro.HRef = base.HostERaizFormat("MinhaConta/Cadastro/MeuCadastro.aspx");
            lnkDestaque_MinhaConta.HRef = base.HostERaizFormat("MinhaConta/Default.aspx");
            */

            if (SessaoClienteLogado != null)
            {
                if (SessaoClienteLogado.Passo < 4)
                {
                    //lnkDestaque_CompleteCadastro.Visible = true;
                    //lnkDestaque_CompleteCadastro.Attributes["href"] = string.Format(HostERaizFormat("MinhaConta/Cadastro/CadastroPF.aspx#Passo{0}"), (SessaoClienteLogado.Passo.Value + 1));
                }
                else
                {
                    //lnkDestaque_MinhaConta.Visible = true;
                }
            }
            else
            {
                //lnkDestaque_AbraSuaConta.Visible = true;
            }

            try
            {
                hidServerData.Value = string.Format("[{0}] [{1}] [{2}] [{3}]"
                                                    , Request.ServerVariables["SERVER_NAME"]
                                                    , Request.ServerVariables["LOCAL_ADDR"]
                                                    , Request.ServerVariables["APP_POOL_ID"]
                                                    , Request.ServerVariables["INSTANCE_ID"]);
            }
            catch{}
        }

        private string ResponderRetornarXMLTVHome()
        {
            string lRetorno = "";

            try
            {
                HttpWebRequest lRequest;
                HttpWebResponse lResponse;

                string lChave = ConfiguracoesValidadas.TVGradual_ChaveDaAPI;

                string lURL = ConfiguracoesValidadas.TVGradual_UrlDaAPI;

                lURL += string.Format("?action=get_medias&api_key={0}&results_per_page=1&category=gradual-tv", lChave);

                lRequest = (HttpWebRequest)WebRequest.Create(lURL);

                lRequest.Timeout = 4000;

                lResponse = (HttpWebResponse)lRequest.GetResponse();

                XmlDocument lDetalheTV = new XmlDocument();

                lDetalheTV.Load(new StreamReader(lResponse.GetResponseStream()));

                List<TransporteVideoTV> lListaDeVideos = new List<TransporteVideoTV>();

                //lDetalheTV.SelectNodes("MonQi/media")[0]["media_id"].InnerText

                XmlNodeList lLista = lDetalheTV.SelectNodes("MonQi/media");

                foreach (XmlNode lNode in lLista)
                {
                    lListaDeVideos.Add(new TransporteVideoTV(lNode));
                }

                lRetorno = RetornarSucessoAjax(lListaDeVideos, "Sucesso em gerar URL");
            }
            catch (Exception ex)
            {
                gLogger.Error("[Erro ao carregar Videos TV Gradual]", ex);
            }

            return lRetorno;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[]{
                                                    "RetornarXMLTVHome"
                                                },
                    new ResponderAcaoAjaxDelegate[]{
                                                    ResponderRetornarXMLTVHome
                                                });

            Gradual.OMS.Library.Servicos.ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

            CarregarCotacaoIGB30();

            CarregarCotacoes();

            CarregarOfertasPublicas();

            CarregarBanners();

            CarregarDestaques();

            ConfigurarInterface();
        }

        #endregion

    }
}
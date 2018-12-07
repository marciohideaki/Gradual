using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.Cotacao.Lib;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using Gradual.Site.DbLib;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Site.DbLib.Dados;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Gradual.OMS.PlanoCliente.Lib;

namespace Gradual.Site.Www.Async
{
    public partial class Geral : PaginaBase
    {
        #region Métodos Private - Auxiliares

        private Gradual.Site.DbLib.Dados.MinhaConta.Comercial.ProdutoInfo BuscarDadosDoProduto(int pIdProduto)
        {
            string lMensagemDeErro = "Erro em Async/Geral.aspx > BuscarDadosDoProduto()";

            BuscarDadosDosProdutosRequest lRequest = new BuscarDadosDosProdutosRequest();
            BuscarDadosDosProdutosResponse lResponse;

            lRequest.IdProduto = pIdProduto;

            lResponse = base.ServicoPersistenciaSite.BuscarDadosDosProdutos(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.DadosDosProdutos.Count > 0)
                {
                    return lResponse.DadosDosProdutos[0];
                }
                else
                {
                    lMensagemDeErro = string.Format("Sem nenhum produto retornado em Async/Geral.aspx > BuscarDadosDoProduto({0}): {1} > {2}"
                                                    , ConfiguracoesValidadas.IdDoProduto_GradualTraderInterface
                                                    , lResponse.StatusResposta
                                                    , lResponse.DescricaoResposta);

                    gLogger.Error(lMensagemDeErro);
                }
            }
            else
            {
                lMensagemDeErro = string.Format("Resposta com erro do serviço em Async/Geral.aspx > BuscarDadosDoProduto(): {0} > {1}"
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta);

                //gLogger.ErrorFormat();

                gLogger.Error(lMensagemDeErro);
            }

            throw new Exception(lMensagemDeErro);
        }

        private MensagemResponseStatusEnum EnviarEmailCompraCarrinho(Intranet.Contratos.Dados.ClienteEnderecoInfo pEnderecoDeEntrega, string pTelefones)
        {
            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

            string lDadosDaCompra = "";

            string lTemplateDeItem = "<p>Produto: {0} - {4}{5}<br />Quantidade: {1}<br />Taxas: {2}<br />Total: {3}<br /><br /></p>\r\n";

            foreach (TransporteProduto lProd in SessaoClienteLogado.DadosDoCarrinho.Produtos)
            {
                lDadosDaCompra += string.Format(lTemplateDeItem, lProd.NomeProduto, lProd.QuantidadeProduto, lProd.Taxas.ToString("n2"), lProd.Total.ToString("n2"), lProd.Tipo, lProd.Modo);
            }

            lVariaveis.Add("###CODIGO###", SessaoClienteLogado.CodigoPrincipal);
            lVariaveis.Add("###NOME###", SessaoClienteLogado.Nome);
            lVariaveis.Add("###CPF###", SessaoClienteLogado.CpfCnpj);
            lVariaveis.Add("###TELS###", pTelefones);
            lVariaveis.Add("###CPA_DATA###", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            lVariaveis.Add("###COMPRAS###", lDadosDaCompra);

            lVariaveis.Add("###END_RUA###", string.Format("{0} {1} {2}", pEnderecoDeEntrega.DsLogradouro, pEnderecoDeEntrega.DsNumero, pEnderecoDeEntrega.DsComplemento));
            lVariaveis.Add("###END_BAIRRO###", pEnderecoDeEntrega.DsBairro);
            lVariaveis.Add("###END_CIDADE###", string.Format("{0}/{1}", pEnderecoDeEntrega.DsCidade, pEnderecoDeEntrega.CdUf));
            lVariaveis.Add("###END_CEP###", string.Format("{0}-{1}", pEnderecoDeEntrega.NrCep, pEnderecoDeEntrega.NrCepExt));

            string lEmailPara = ConfiguracoesValidadas.Email_CompraCambio;

            if (ConfiguracoesValidadas.EmailsCompraCambio_PorEstado.ContainsKey(pEnderecoDeEntrega.CdUf))
                lEmailPara = ConfiguracoesValidadas.EmailsCompraCambio_PorEstado[pEnderecoDeEntrega.CdUf];

            return base.EnviarEmail(lEmailPara
                                    , "Compra de câmbio realizada"
                                    , "EmailCompraCambio.html"
                                    , lVariaveis, eTipoEmailDisparo.Todos);
        }

        private MensagemResponseStatusEnum EnviarEmailCompraCarrinhoOffline(string pNome, string pEmail, List<TransporteProduto> pProdutos, Intranet.Contratos.Dados.ClienteEnderecoInfo pEnderecoDeEntrega, string pTelefones)
        {
            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

            string lDadosDaCompra = "";

            string lTemplateDeItem = "<p>Produto: {0} - {4}{5}<br />Quantidade: {1}<br />Taxas: R$ {2}<br />Total: R$ {3}<br /><br /></p>\r\n";

            foreach (TransporteProduto lProd in pProdutos)
            {
                lDadosDaCompra += string.Format(lTemplateDeItem, lProd.NomeProduto, lProd.QuantidadeProduto, lProd.Taxas.ToString("n2"), lProd.Total.ToString("n2"), lProd.Tipo, lProd.Modo);
            }

            lVariaveis.Add("###NOME###", pNome);
            lVariaveis.Add("###EMAIL###", pEmail);
            lVariaveis.Add("###TELS###", pTelefones);
            lVariaveis.Add("###CPA_DATA###", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            lVariaveis.Add("###COMPRAS###", lDadosDaCompra);

            lVariaveis.Add("###END_RUA###", string.Format("{0} {1} {2}", pEnderecoDeEntrega.DsLogradouro, pEnderecoDeEntrega.DsNumero, pEnderecoDeEntrega.DsComplemento));
            lVariaveis.Add("###END_BAIRRO###", pEnderecoDeEntrega.DsBairro);
            lVariaveis.Add("###END_CIDADE###", string.Format("{0}/{1}", pEnderecoDeEntrega.DsCidade, pEnderecoDeEntrega.CdUf));
            lVariaveis.Add("###END_CEP###", string.Format("{0}-{1}", pEnderecoDeEntrega.NrCep, pEnderecoDeEntrega.NrCepExt));

            gLogger.InfoFormat("COMPRA OFFLINE EFETUADA!\r\n  {0}\r\n  {1}\r\n  {2}\r\n "
                                , (pNome + " - " + pEmail + " - " + pTelefones)
                                , lDadosDaCompra
                                , JsonConvert.SerializeObject(pEnderecoDeEntrega));
            
            string lEmailPara = ConfiguracoesValidadas.Email_CompraCambio;

            if (ConfiguracoesValidadas.EmailsCompraCambio_PorEstado.ContainsKey(pEnderecoDeEntrega.CdUf))
                lEmailPara = ConfiguracoesValidadas.EmailsCompraCambio_PorEstado[pEnderecoDeEntrega.CdUf];

            return base.EnviarEmail(lEmailPara
                                    , "Compra de câmbio realizada"
                                    , "EmailCompraOffline.html"
                                    , lVariaveis, eTipoEmailDisparo.Todos);
        }

        private MensagemResponseStatusEnum EnviarEmailCompraProduto(int pIdProduto)
        {
            string lArquivo = "";

            string lNomeProd = "";

            if (pIdProduto == ConfiguracoesValidadas.IdDoProduto_GradualTraderInterface)
            {
                lArquivo = "NotificacaoCompra-GTI.html";
                lNomeProd = "GTI";
            }

            if (pIdProduto == ConfiguracoesValidadas.IdDoProduto_StockMarket)
            {
                lArquivo = "NotificacaoCompra-StockMarket.html";
                lNomeProd = "StockMarket";
            }

            if (!string.IsNullOrEmpty(lArquivo))
            {
                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

                lVariaveis.Add("###NOME###", SessaoClienteLogado.Nome);
                lVariaveis.Add("###CPF###", SessaoClienteLogado.CpfCnpj);
                lVariaveis.Add("###CODIGO###", SessaoClienteLogado.CodigoPrincipal);

                return base.EnviarEmail(ConfiguracoesValidadas.Email_Atendimento
                                        , ("Compra de " + lNomeProd)
                                        , lArquivo
                                        , lVariaveis, eTipoEmailDisparo.Todos);
            }
            else
            {
                throw new Exception(string.Format("Erro em Geral.aspx > EnviarEmailCompraProduto({0}) ID não esperado para envio de email de compra; ID GTI: [{1}] ID StockMarket: [{2}]"
                                    , pIdProduto
                                    , ConfiguracoesValidadas.IdDoProduto_GradualTraderInterface
                                    , ConfiguracoesValidadas.IdDoProduto_StockMarket));
            }
        }


        private void GravarCarrinhoDaSessaoNoBanco(Intranet.Contratos.Dados.ClienteEnderecoInfo pEnderecoDeEntrega, Intranet.Contratos.Dados.ClienteTelefoneInfo pCelularDeEntrega, Intranet.Contratos.Dados.ClienteTelefoneInfo pTelefoneDeEntrega)
        {
            VendaInfo lVenda = new VendaInfo();

            lVenda.CblcCliente = SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            lVenda.CpfCnpjCliente = SessaoClienteLogado.CpfCnpj;
            lVenda.Status = 1;

            lVenda.EnderecoDeEntrega = pEnderecoDeEntrega;

            foreach (TransporteProduto lProd in SessaoClienteLogado.DadosDoCarrinho.Produtos)
            {
                lVenda.Produtos.Add(lProd.ToVendaProdutoInfo());
            }

            lVenda.TelefoneDeEntrega = pTelefoneDeEntrega;
            lVenda.CelularDeEntrega = pCelularDeEntrega;

            IncluirDadosDeVendaNoBanco(ref lVenda);
        }

        private string ResponderBuscarCotacaoELivrosParaHome()
        {
            string lRetorno;

            try
            {
                TransporteMensagemDeNegocio lTransporteMensagem;
                TransporteLivroDeOferta lTransporteLivro;

                string lMensagem, lAtivo;

                IServicoCotacao lServico = InstanciarServicoDoAtivador<IServicoCotacao>();

                lAtivo = Request.Form["Ativo"];

                lMensagem = lServico.ReceberTickerCotacao(lAtivo);

                lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);

                if (!string.IsNullOrEmpty(lTransporteMensagem.CodigoNegocio))
                {
                    lMensagem = lServico.ReceberLivroOferta(lAtivo);

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
                        lTransporteLivro.OfertasDeCompra.Add(new TransporteMensagemDeLivroDeOferta() { NomeCorretora = "&nbsp;", Preco = "&nbsp;", QuantidadeAbreviada = "&nbsp;" });
                    }

                    while (lTransporteLivro.OfertasDeVenda.Count < 7)
                    {
                        lTransporteLivro.OfertasDeVenda.Add(new TransporteMensagemDeLivroDeOferta() { NomeCorretora = "&nbsp;", Preco = "&nbsp;", QuantidadeAbreviada = "&nbsp;" });
                    }

                    var lObjetoDeRetorno = new { DadosDeCotacao = lTransporteMensagem, DadosDeLivro = lTransporteLivro };

                    lRetorno = RetornarSucessoAjax(lObjetoDeRetorno, "ok");
                }
                else
                {
                    lRetorno = RetornarSucessoAjax(string.Format("Ativo [{0}] inválido ou sem dados.", lAtivo));
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderBuscarCotacaoELivrosParaHome", ex);
            }

            return lRetorno;
        }

        private string ResponderBuscarCotacaoRapida()
        {
            string lRetorno;

            string lMensagem, lAtivo;

            lAtivo = Request.Form["Ativo"];

            try
            {
                TransporteMensagemDeNegocio lTransporteMensagem;

                IServicoCotacao lServico = InstanciarServicoDoAtivador<IServicoCotacao>();

                lMensagem = lServico.ReceberTickerCotacao(lAtivo);

                lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);

                if (!string.IsNullOrEmpty(lTransporteMensagem.CodigoNegocio))
                {
                    lRetorno = RetornarSucessoAjax(lTransporteMensagem, "ok");
                }
                else
                {
                    lRetorno = RetornarSucessoAjax(string.Format("Ativo [{0}] inválido ou sem dados.", lAtivo));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Async.aspx > ResponderBuscarCotacaoRapida(pAtivo: [{0}]) [{1}]\r\n{2}"
                                    , lAtivo
                                    , ex.Message
                                    , ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro em ResponderBuscarCotacaoRapida", ex);
            }

            return lRetorno;
        }

        public string ResponderBuscarSessaoParaHB()
        {
            string lRetorno;

            if (SessaoClienteLogado != null)
            {
                if (SessaoClienteLogado.Passo >= 4)
                {
                    BuscarCodigoDeSessaoParaUsuarioLogado();

                    lRetorno = RetornarSucessoAjax(SessaoClienteLogado.CodigoDaSessao);
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("FALTA_CADASTRO");
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Sem usuário na sessão! Favor realizar login novamente...");
            }

            return lRetorno;
        }

        private string ResponderRetornarXMLTV()
        {
            string lRetorno = "";

            try
            {
                HttpWebRequest lRequest;

                HttpWebResponse lResponse;

                string lURL = ConfiguracoesValidadas.TVGradual_UrlDaAPI;

                string lCategoria = Request["Categoria"];

                string lTimeStamp, lSharedKey;

                DateTime lDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                DateTime lDateNow = DateTime.Now.ToUniversalTime();

                TimeSpan lSpan = lDateNow.Subtract(lDateTime);

                long lTicks = lSpan.Ticks;

                lTicks /= 10000000;

                lTimeStamp = lTicks.ToString();

                lSharedKey = GerarSHA1(ConfiguracoesValidadas.TVGradual_ChaveSecretaDaAPI + lTimeStamp);

                lURL += string.Format("?action=get_medias&api_key={0}&shared_key={1}&ts={2}&orderby=category&results_per_page=20&featured=1&category={3}"
                                        , ConfiguracoesValidadas.TVGradual_ChaveDaAPI
                                        , lSharedKey
                                        , lTimeStamp
                                        , lCategoria);

                lRequest = (HttpWebRequest)WebRequest.Create(lURL);

                lRequest.Timeout = 4000;

                lResponse = (HttpWebResponse)lRequest.GetResponse();

                XmlDocument lDetalheTV = new XmlDocument();

                lDetalheTV.Load(new StreamReader(lResponse.GetResponseStream()));

                List<TransporteVideoTV> lListaDeVideos = new List<TransporteVideoTV>();

                //lDetalheTV.SelectNodes("MonQi/media")[0]["media_id"].InnerText

                XmlNodeList lCategorias, lVideos;

                lCategorias = lDetalheTV.SelectNodes("MonQi/tab");

                string lNomeDaCategoria;

                foreach (XmlNode lNoCategoria in lCategorias)
                {
                    lNomeDaCategoria = lNoCategoria["name"].InnerText;

                    lVideos = lNoCategoria.SelectNodes("medias/media");

                    foreach (XmlNode lNoVideo in lVideos)
                    {
                        lListaDeVideos.Add(new TransporteVideoTV(lNoVideo, lNomeDaCategoria));
                    }
                }

                lRetorno = RetornarSucessoAjax(lListaDeVideos, "Sucesso em gerar URL");
            }
            catch (Exception ex)
            {
                gLogger.Error("[Erro ao carregar Videos TV Gradual]", ex);

                base.ExibirMensagemJsOnLoad("E", "Erro ao carregar Videos.");
            }

            return lRetorno;
        }

        private string ResponderSalvarFormularioGenerico()
        {
            string lRetorno = "";

            int lIdConteudo;

            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            try
            {
                lRequest = new ConteudoRequest();

                lRequest.Conteudo = new ConteudoInfo();

                lRequest.Conteudo.CodigoTipoConteudo = 0;
                lRequest.Conteudo.CodigoTipoConteudo = ConfiguracoesValidadas.IdDoTipo_ConteudoGenerico;

                lRequest.Conteudo.ValorPropriedade1 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                lRequest.Conteudo.ValorPropriedade2 = Request.Url.AbsolutePath;
                lRequest.Conteudo.ValorPropriedade3 = Request["Referencia"];
                lRequest.Conteudo.ValorPropriedade4 = Request["Tag"];

                lRequest.Conteudo.ConteudoJson = Request["ConteudoJson"];
                lRequest.Conteudo.DtCriacao = DateTime.Now;

                lResponse = base.ServicoPersistenciaSite.InserirConteudo(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

                    lRetorno = RetornarSucessoAjax(lResponse.Conteudo.CodigoConteudo, "Conteúdo foi salvo com sucesso.");

                    if (!string.IsNullOrEmpty(Request["EnviarEmail"]) && Request["EnviarEmail"].ToLower() == "sim")
                    {
                        string lDestinatario, lAssunto, lTipo, lCodigo, lNome, lEmail, lMensagem, lArquivo, lCelDDD, lCelNum, lTelDDD, lTelNum;

                        lDestinatario = Request["EnviarEmailPara"];
                        lAssunto      = Request["AssuntoEmail"];
                        lTipo         = Request["TipoEmail"];
                        lMensagem     = Request["MensagemEmail"];
                        lCodigo       = Request["CodigoCliente"];
                        lNome         = Request["NomeCliente"];
                        lEmail        = Request["EmailCliente"];
                        lArquivo      = Request["ArquivoDeTemplate"];
                        lCelDDD       = Request["CelDDD"];
                        lCelNum       = Request["CelNum"];
                        lTelDDD       = Request["TelDDD"];
                        lTelNum       = Request["TelNum"];

                        if (string.IsNullOrEmpty(lDestinatario))
                            lDestinatario = ConfiguracoesValidadas.Email_Atendimento;

                        if (string.IsNullOrEmpty(lAssunto))
                            lAssunto = string.Format("Email do site referente a {0}", Request["Referencia"]);

                        if (string.IsNullOrEmpty(lTipo))
                            lTipo = "(não especificado)";

                        if (string.IsNullOrEmpty(lMensagem))
                            lMensagem = "(não especificada)";

                        if (string.IsNullOrEmpty(lCodigo))
                            lCodigo = "(não especificado)";

                        if (string.IsNullOrEmpty(lNome))
                            lNome = "(não especificado)";
                        
                        if (string.IsNullOrEmpty(lEmail))
                            lEmail = "n/d";

                        if (string.IsNullOrEmpty(lCelDDD))
                            lCelDDD = "n/d";
                        
                        if (string.IsNullOrEmpty(lCelNum))
                            lCelNum = "n/d";
                        
                        if (string.IsNullOrEmpty(lTelDDD))
                            lTelDDD = "n/d";
                        
                        if (string.IsNullOrEmpty(lTelNum))
                            lTelNum = "n/d";

                        //variáveis padrão:

                        lVariaveis.Add("###TIPO###",        lTipo);
                        lVariaveis.Add("###REFERENCIA###",  lRequest.Conteudo.ValorPropriedade3);
                        lVariaveis.Add("###NOMECLIENTE###", lNome);
                        lVariaveis.Add("###CODIGO###",      lCodigo);
                        lVariaveis.Add("###MENSAGEM###",    lMensagem);
                        lVariaveis.Add("###CELDDD###",      lCelDDD);
                        lVariaveis.Add("###CELNUM###",      lCelNum);
                        lVariaveis.Add("###TELDDD###",      lTelDDD);
                        lVariaveis.Add("###TELNUM###",      lTelNum);
                        lVariaveis.Add("###JSON###",        lRequest.Conteudo.ConteudoJson);

                        if (string.IsNullOrEmpty(lArquivo))
                        {
                            lArquivo = "CadastroGenericoRealizado.html";
                        }
                        else
                        {
                            string lCaminho = Server.MapPath(Path.Combine("~/Resc/Emails/", lArquivo));

                            if (!File.Exists(lCaminho))
                            {
                                gLogger.ErrorFormat("Arquivo especificado para cadastro genérico [{0}] não encontrado, revertendo para [CadastroGenericoRealizado.html]", lCaminho);

                                lArquivo = "CadastroGenericoRealizado.html";
                            }
                            else
                            {
                                ///vamos abrir o arquivo e ver se tem mais variáveis que ele possa estar querendo

                                Regex lReg = new Regex("###.*?###");
                                string lValorSemHash;

                                foreach (Match lMatch in lReg.Matches(File.ReadAllText(lCaminho)))
                                {
                                    try
                                    {
                                        lValorSemHash = lMatch.Value.Replace("#", "");

                                        if(!lVariaveis.ContainsKey(lMatch.Value))
                                            lVariaveis.Add(lMatch.Value, Request[lValorSemHash]);
                                    }
                                    catch { }
                                }
                            }
                        }

                        try
                        {
                            base.EnviarEmail(lDestinatario, lAssunto, lArquivo, lVariaveis, eTipoEmailDisparo.Todos);
                        }
                        catch (Exception ex)
                        {
                            gLogger.ErrorFormat("Erro ao enviar email de mensagem do cliente para o atendimento: [{0}]\r\n[{1}]\r\n\r\n{2}"
                                                , ex.Message
                                                , JsonConvert.SerializeObject(lVariaveis)
                                                , ex.StackTrace);
                        }
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro no salvar Conteudo", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderSalvarConteudo()", ex);
            }

            return lRetorno;
        }

        private string ResponderBuscarCEP()
        {
            string lRetorno;

            try
            {
                CepLivreEndereco lEnd = CepLivreConnector.BuscarCEP(Request["CEP"]);

                lRetorno = RetornarSucessoAjax(lEnd, "ok");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Async/Geral.aspx: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarSucessoAjax("erro");
            }

            return lRetorno;
        }

        private string ResponderVisualizarConfiguracoes()
        {
            string lRetorno = "";

            lRetorno += "<code><table>";

            lRetorno += string.Format("<tr><td>AplicacaoEmModoDeTeste: </td><td>[{0}]</td></tr>",         ConfiguracoesValidadas.AplicacaoEmModoDeTeste);
            lRetorno += string.Format("<tr><td>CalculadoraIR_EmailIR: </td><td>[{0}]</td></tr>",          ConfiguracoesValidadas.CalculadoraIR_EmailIR);
            lRetorno += string.Format("<tr><td>CalculadoraIR_IDCorretora: </td><td>[{0}]</td></tr>",      ConfiguracoesValidadas.CalculadoraIR_IDCorretora);
            lRetorno += string.Format("<tr><td>CalculadoraIR_SiteMyCapital: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.CalculadoraIR_SiteMyCapital);
            lRetorno += string.Format("<tr><td>CepLivreChave: </td><td>[{0}]</td></tr>",                  ConfiguracoesValidadas.CepLivreChave);
            lRetorno += string.Format("<tr><td>Chat_URL: </td><td>[{0}]</td></tr>",                       ConfiguracoesValidadas.Chat_URL);
            lRetorno += string.Format("<tr><td>CodigoAtualDIFuturo: </td><td>[{0}]</td></tr>",            ConfiguracoesValidadas.CodigoAtualDIFuturo);
            lRetorno += string.Format("<tr><td>CodigoAtualDolarFuturo: </td><td>[{0}]</td></tr>",         ConfiguracoesValidadas.CodigoAtualDolarFuturo);
            lRetorno += string.Format("<tr><td>CodigoAtualIbovFuturo: </td><td>[{0}]</td></tr>",          ConfiguracoesValidadas.CodigoAtualIbovFuturo);
            lRetorno += string.Format("<tr><td>CodigoGestorItau: </td><td>[{0}]</td></tr>",               ConfiguracoesValidadas.CodigoGestorItau);
            
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarAnaliseEconomica: </td><td>[{0}]</td></tr>",         ConfiguracoesValidadas.CodigoPermissao_EditarAnaliseEconomica);
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarAnaliseFundamentalista: </td><td>[{0}]</td></tr>",   ConfiguracoesValidadas.CodigoPermissao_EditarAnaliseFundamentalista);
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarAnaliseGrafica: </td><td>[{0}]</td></tr>",           ConfiguracoesValidadas.CodigoPermissao_EditarAnaliseGrafica);
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarCarteirasRecomendadas: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.CodigoPermissao_EditarCarteirasRecomendadas);
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarCMS: </td><td>[{0}]</td></tr>",                      ConfiguracoesValidadas.CodigoPermissao_EditarCMS);
            
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarGradiusGestao: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.CodigoPermissao_EditarGradiusGestao);
            lRetorno += string.Format("<tr><td>CodigoPermissao_EditarNikkei: </td><td>[{0}]</td></tr>",           ConfiguracoesValidadas.CodigoPermissao_EditarNikkei);
            lRetorno += string.Format("<tr><td>Email_Atendimento: </td><td>[{0}]</td></tr>",                      ConfiguracoesValidadas.Email_Atendimento);
            lRetorno += string.Format("<tr><td>Email_CopiaDeEnvioDoCadastro: </td><td>[{0}]</td></tr>",           ConfiguracoesValidadas.Email_CopiaDeEnvioDoCadastro);
            lRetorno += string.Format("<tr><td>Email_Movimentacao: </td><td>[{0}]</td></tr>",                     ConfiguracoesValidadas.Email_Movimentacao);
            
            lRetorno += string.Format("<tr><td>Email_NotificacaoDeposito_Destinatarios: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.Email_NotificacaoDeposito_Destinatarios);
            lRetorno += string.Format("<tr><td>Email_NotificacaoDeposito_Remetente: </td><td>[{0}]</td></tr>",        ConfiguracoesValidadas.Email_NotificacaoDeposito_Remetente);
            
            lRetorno += string.Format("<tr><td>Email_Ouvidoria: </td><td>[{0}]</td></tr>",        ConfiguracoesValidadas.Email_Ouvidoria);
            lRetorno += string.Format("<tr><td>Email_RemetenteGradual: </td><td>[{0}]</td></tr>", ConfiguracoesValidadas.Email_RemetenteGradual);
            lRetorno += string.Format("<tr><td>Email_Tesouraria: </td><td>[{0}]</td></tr>",       ConfiguracoesValidadas.Email_Tesouraria);
            lRetorno += string.Format("<tr><td>HostDoSite: </td><td>[{0}]</td></tr>",             ConfiguracoesValidadas.HostDoSite);

            lRetorno += string.Format("<tr><td>IdContrato_TermoAlavancagemFinanceira: </td><td>[{0}]</td></tr>",      ConfiguracoesValidadas.IdContrato_TermoAlavancagemFinanceira);
            lRetorno += string.Format("<tr><td>IdContrato_TermoParaRealizacaoOrdemStop: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.IdContrato_TermoParaRealizacaoOrdemStop);
            lRetorno += string.Format("<tr><td>IdDaLista_BannersParaHome: </td><td>[{0}]</td></tr>",                  ConfiguracoesValidadas.IdDaLista_BannersParaHome);
            lRetorno += string.Format("<tr><td>IdDaLista_BannersProRodape: </td><td>[{0}]</td></tr>",                 ConfiguracoesValidadas.IdDaLista_BannersProRodape);
            lRetorno += string.Format("<tr><td>IdDaLista_CarteiraRecomendada: </td><td>[{0}]</td></tr>",              ConfiguracoesValidadas.IdDaLista_CarteiraRecomendada);
            lRetorno += string.Format("<tr><td>IdDaLista_ChatPublicado: </td><td>[{0}]</td></tr>",                    ConfiguracoesValidadas.IdDaLista_ChatPublicado);
            lRetorno += string.Format("<tr><td>IdDaLista_DestaquesParaHome: </td><td>[{0}]</td></tr>",                ConfiguracoesValidadas.IdDaLista_DestaquesParaHome);
            lRetorno += string.Format("<tr><td>IdDaLista_ISIN: </td><td>[{0}]</td></tr>",                             ConfiguracoesValidadas.IdDaLista_ISIN);
            lRetorno += string.Format("<tr><td>IdDaLista_OfertasPublicasParaHome: </td><td>[{0}]</td></tr>",          ConfiguracoesValidadas.IdDaLista_OfertasPublicasParaHome);
            lRetorno += string.Format("<tr><td>IdDaLista_VideoDaHome: </td><td>[{0}]</td></tr>",                      ConfiguracoesValidadas.IdDaLista_VideoDaHome);
            lRetorno += string.Format("<tr><td>IdDoProduto_CursoAnaliseGrafica: </td><td>[{0}]</td></tr>",            ConfiguracoesValidadas.IdDoProduto_CursoAnaliseGrafica);
            lRetorno += string.Format("<tr><td>IdDoProduto_CursoMarcioNoronha: </td><td>[{0}]</td></tr>",             ConfiguracoesValidadas.IdDoProduto_CursoMarcioNoronha);
            lRetorno += string.Format("<tr><td>IdDoProduto_GradualGraficos: </td><td>[{0}]</td></tr>",                ConfiguracoesValidadas.IdDoProduto_GradualGraficos);
            lRetorno += string.Format("<tr><td>IdDoProduto_GradualTraderInterface: </td><td>[{0}]</td></tr>",         ConfiguracoesValidadas.IdDoProduto_GradualTraderInterface);
            lRetorno += string.Format("<tr><td>IdDoProduto_GradualTravelCard: </td><td>[{0}]</td></tr>",              ConfiguracoesValidadas.IdDoProduto_GradualTravelCard);
            lRetorno += string.Format("<tr><td>IdDoProduto_StockMarket: </td><td>[{0}]</td></tr>",                    ConfiguracoesValidadas.IdDoProduto_StockMarket);
            
            lRetorno += string.Format("<tr><td>IdDoTipo_BannerLateral: </td><td>[{0}]</td></tr>",        ConfiguracoesValidadas.IdDoTipo_BannerLateral);
            lRetorno += string.Format("<tr><td>IdDoTipo_BannerLateralLink: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.IdDoTipo_BannerLateralLink);
            lRetorno += string.Format("<tr><td>IdDoTipo_ConteudoGenerico: </td><td>[{0}]</td></tr>",     ConfiguracoesValidadas.IdDoTipo_ConteudoGenerico);
            lRetorno += string.Format("<tr><td>IdDoTipo_MenuPrincipal: </td><td>[{0}]</td></tr>",        ConfiguracoesValidadas.IdDoTipo_MenuPrincipal);
            lRetorno += string.Format("<tr><td>IdPlanoCalculadoraIRAberto: </td><td>[{0}]</td></tr>",    ConfiguracoesValidadas.IdPlanoCalculadoraIRAberto);
            lRetorno += string.Format("<tr><td>IdPlanoCalculadoraIRFeachado: </td><td>[{0}]</td></tr>",  ConfiguracoesValidadas.IdPlanoCalculadoraIRFeachado);

            lRetorno += string.Format("<tr><td>IdPlanoPoupeGradual200: </td><td>[{0}]</td></tr>", ConfiguracoesValidadas.IdPlanoPoupeGradual200);
            lRetorno += string.Format("<tr><td>IdPlanoPoupeGradual500: </td><td>[{0}]</td></tr>", ConfiguracoesValidadas.IdPlanoPoupeGradual500);
            lRetorno += string.Format("<tr><td>IdPlanoPoupeGradual700: </td><td>[{0}]</td></tr>", ConfiguracoesValidadas.IdPlanoPoupeGradual700);

            lRetorno += string.Format("<tr><td>IPO_URL: </td><td>[{0}]</td></tr>",                        ConfiguracoesValidadas.IPO_URL);
            lRetorno += string.Format("<tr><td>Ordens_PortaDeControle: </td><td>[{0}]</td></tr>",         ConfiguracoesValidadas.Ordens_PortaDeControle);

            lRetorno += string.Format("<tr><td>RaizDoSite: </td><td>[{0}]</td></tr>",     ConfiguracoesValidadas.RaizDoSite);
            lRetorno += string.Format("<tr><td>SkinPadrao: </td><td>[{0}]</td></tr>",     ConfiguracoesValidadas.SkinPadrao);
            lRetorno += string.Format("<tr><td>VersaoDoSite: </td><td>[{0}]</td></tr>",   ConfiguracoesValidadas.VersaoDoSite);
            
            lRetorno += "</table></code>";

            return lRetorno;
        }

        private string ResponderTestarPerformance()
        {
            string lRetorno = "Teste de Performance...<br/><br/>";

            DateTime lData;
            TimeSpan lSpan;

            object lObj;

            lData = DateTime.Now;
            this.ListaDePaginas = null; //fará ele recarregar na próxima...
            lObj = this.ListaDePaginas; // recarregou
            lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

            lRetorno += string.Format("CarregarListaDePaginas() : [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);


            lData = DateTime.Now;
            this.BannersLateraisDisponiveis = null;
            lObj = this.BannersLateraisDisponiveis; // recarregou

            lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

            lRetorno += string.Format("CarregarBanners() : [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

            TestarOperacoesDePagina(ref lRetorno);

            TestarOperacoesDeConteudo(ref lRetorno);

            TestarOperacoesDeListaDeConteudo(ref lRetorno);

            return lRetorno;
        }

        private string ResponderAdicionarItemAoCarrinho()
        {
            string lRetorno = RetornarSucessoAjax("ok");

            if (SessaoClienteLogado == null)
                return RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);

            try
            {
                TransporteProduto lProduto = JsonConvert.DeserializeObject<TransporteProduto>(Request["DadosDoProduto"]);

                lProduto.PreId = DateTime.Now.ToString("HH_mm_ss_ff");

                SessaoClienteLogado.DadosDoCarrinho.Produtos.Add(lProduto);

                lRetorno = RetornarSucessoAjax(lProduto.PreId);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Async/Geral.aspx > ResponderAdicionarItemAoCarrinho() :: {0}\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao incluir item no carrinho", ex);
            }

            return lRetorno;
        }

        private string ResponderRemoverItemDoCarrinho()
        {
            string lRetorno = RetornarSucessoAjax("ok");

            if (SessaoClienteLogado == null)
                return RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);

            try
            {
                string lPreId = Request["PreId"];

                int lIdParaRemover = -1;

                for (int a = 0; a < SessaoClienteLogado.DadosDoCarrinho.Produtos.Count; a++)
                {
                    if (SessaoClienteLogado.DadosDoCarrinho.Produtos[a].PreId == lPreId)
                    {
                        lIdParaRemover = a;

                        break;
                    }
                }

                if (lIdParaRemover != -1)
                {
                    SessaoClienteLogado.DadosDoCarrinho.Produtos.RemoveAt(lIdParaRemover);

                    lRetorno = RetornarSucessoAjax(lPreId);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Async/Geral.aspx > ResponderRemoverItemDoCarrinho() :: {0}\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao excluir item do carrinho", ex);
            }

            return lRetorno;
        }

        private string ResponderFinalizarCarrinhoOffline()
        {
            string lRetorno;

            try
            {
                List<TransporteProduto> lProdutos = JsonConvert.DeserializeObject<List<TransporteProduto>>(Request["Produtos"]);

                TransporteCadastroEndereco lEndereco = JsonConvert.DeserializeObject<TransporteCadastroEndereco>(Request["Endereco"]);

                Intranet.Contratos.Dados.ClienteTelefoneInfo lTel = new Intranet.Contratos.Dados.ClienteTelefoneInfo();
                Intranet.Contratos.Dados.ClienteTelefoneInfo lCel = new Intranet.Contratos.Dados.ClienteTelefoneInfo();

                lTel.DsDdd = Request["tel_ddd"];
                lTel.DsNumero = Request["tel_num"];

                lCel.DsDdd = Request["cel_ddd"];
                lCel.DsNumero = Request["cel_num"];

                string lNomeCliente  = Request["NomeCliente"];
                string lEmailCliente = Request["EmailCliente"];

                string lTels = string.Format("({0}) {1} / ({2}) {3}", lTel.DsDdd, lTel.DsNumero, lCel.DsDdd, lCel.DsNumero);

                EnviarEmailCompraCarrinhoOffline(lNomeCliente, lEmailCliente, lProdutos, lEndereco.ToEnderecoInfo(), lTels);

                lRetorno = RetornarSucessoAjax("ok");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Async/Geral.aspx ResponderFinalizarCarrinhoOffline() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao finalizar compra", ex);
            }

            return lRetorno;
        }

        private string ResponderFinalizarCarrinho()
        {
            string lRetorno;

            try
            {
                if (SessaoClienteLogado.DadosDoCarrinho.Produtos.Count == 0)
                {
                    lRetorno = RetornarErroAjax("Sem produtos no carrinho");

                    return lRetorno;
                }

                string lEnd = Request["Endereco"];

                string lTels = "";

                Intranet.Contratos.Dados.ClienteEnderecoInfo lEndereco;

                Intranet.Contratos.Dados.ClienteTelefoneInfo lTel = new Intranet.Contratos.Dados.ClienteTelefoneInfo();

                Intranet.Contratos.Dados.ClienteTelefoneInfo lCel = new Intranet.Contratos.Dados.ClienteTelefoneInfo();

                if (lEnd.ToLower() == "conta")
                {
                    lEndereco = (Intranet.Contratos.Dados.ClienteEnderecoInfo)Session["Carrinho_EnderecoDaConta"];
                }
                else
                {
                    TransporteCadastroEndereco lTransp = JsonConvert.DeserializeObject<TransporteCadastroEndereco>(lEnd);

                    lEndereco = lTransp.ToEnderecoInfo();
                }

                try
                {
                    lTel.DsDdd = Request["tel_ddd"];
                    lTel.DsNumero = Request["tel_num"];

                    lCel.DsDdd = Request["cel_ddd"];
                    lCel.DsNumero = Request["cel_num"];

                    lTels = string.Format("({0}) {1} / ({2}) {3}", lTel.DsDdd, lTel.DsNumero, lCel.DsDdd, lCel.DsNumero);

                    GravarCarrinhoDaSessaoNoBanco(lEndereco, lCel, lTel);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "ERRO_INS_PRODUTO")
                    {
                        lRetorno = RetornarErroAjax(string.Format("Compra cadastrada no sistema, porém houve erro liberar o produto para uso; favor entrar em contato com o atendimento.", SessaoClienteLogado.Email));
                    }
                    else
                    {
                        throw ex;
                    }
                }

                if (EnviarEmailCompraCarrinho(lEndereco, lTels) == MensagemResponseStatusEnum.OK)
                {
                    SessaoClienteLogado.DadosDoCarrinho = new TransporteDadosCarrinho();

                    lRetorno = RetornarSucessoAjax("ok");
                }
                else
                {
                    gLogger.ErrorFormat("Erro ao enviar email para [{0}] em ResponderFinalizarCarrinho para compra [{1}] endereço [{2}]"
                                        , SessaoClienteLogado.Email
                                        , JsonConvert.SerializeObject(SessaoClienteLogado.DadosDoCarrinho)
                                        , lEndereco);

                    lRetorno = RetornarErroAjax(string.Format("Compra cadastrada no sistema, porém houve erro ao enviar email para [{0}]; favor entrar em contato com o atendimento.", SessaoClienteLogado.Email));
                }

                lRetorno = RetornarSucessoAjax("ok");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Async/Geral.aspx ResponderFinalizarCarrinho() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao finalizar compra", ex);
            }

            return lRetorno;
        }

        private string ResponderIniciarCompra()
        {
            string lRetorno;

            try
            {
                string lAssinatura = Request["Assinatura"];

                int lIdProduto = Convert.ToInt32(Request["IdProduto"]);

                if (base.ValidarAssinaturaEletronica(lAssinatura))
                {
                    VendaInfo lVenda = new VendaInfo();

                    lVenda.ReferenciaDaVenda = Guid.NewGuid().ToString();
                    lVenda.CblcCliente       = Convert.ToInt32(SessaoClienteLogado.CodigoPrincipal);
                    lVenda.CpfCnpjCliente    = SessaoClienteLogado.CpfCnpj;
                    lVenda.Status            = 1;   //aberta
                    lVenda.Data              = DateTime.Now;

                    lVenda.Produtos = new List<VendaProdutoInfo>();

                    VendaProdutoInfo lInfo = new VendaProdutoInfo();

                    lInfo.IdProduto = lIdProduto;
                    lInfo.Quantidade = 1;
                    lInfo.Preco = -1;   //a proc vai colocar o preço do produto mesmo

                    lVenda.Produtos.Add(lInfo);

                    try
                    {
                        IncluirDadosDeVendaNoBanco(ref lVenda);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "ERRO_INS_PRODUTO")
                        {
                            lRetorno = RetornarSucessoAjax("ERRO_HABILITACAO");
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                    if (EnviarEmailCompraProduto(lIdProduto) == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax("ok");
                    }
                    else
                    {
                        lRetorno = RetornarSucessoAjax("ERRO_EMAIL");
                    }
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("ERRO_ASSINATURA");
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao iniciar a compra", ex);
            }

            return lRetorno;
        }

        private void TestarOperacoesDePagina(ref string pStatusString)
        {
            DateTime lData = DateTime.Now;
            TimeSpan lSpan;

            IServicoPersistenciaSite lServico = InstanciarServicoDoAtivador<IServicoPersistenciaSite>();

            PaginaRequest lPagReq;
            PaginaResponse lPagResp;

            string lCodEstrutura;

            lPagReq = new PaginaRequest();

            lPagReq.Pagina = new PaginaInfo();
            lPagReq.Pagina.DescURL = string.Format("Teste/Teste_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm"));
            lPagReq.Pagina.TipoEstrutura = "1";

            lPagResp = lServico.InserirPagina(lPagReq);

            if (lPagResp.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lPagReq.Pagina.CodigoPagina = lPagResp.Pagina.CodigoPagina;

                lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                pStatusString += string.Format("TestarOperacoesDePagina - InserirPagina(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                lData = DateTime.Now;

                lPagResp = lServico.SelecionarPagina(lPagReq);

                if (lPagResp.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                    pStatusString += string.Format("TestarOperacoesDePagina - SelecionarPagina(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                    /*
                    lCodEstrutura = lPagResp.ListaPagina[0].ListaEstrutura[0].CodigoEstrutura.ToString();

                    WidgetRequest lWidReq = new WidgetRequest();
                    WidgetResponse lWidResp;

                    lWidReq.Widget = new WidgetInfo();
                    
                    lWidReq.Widget.CodigoEstrutura = Convert.ToInt32(lCodEstrutura);
                    lWidReq.Widget.OrdemPagina = 1;
                    lWidReq.Widget.WidgetJson = "{\"Texto\":\"Teste1\",\"NivelDeTitulo\":1,\"IdDaEstrutura\":" + lCodEstrutura + ",IdDaLista\":0,\"Ordenacao\":null,\"DirecaoDeOrdenacao\":null,\"OrdemNaPagina\":1,\"AtributoStyle\":\"\",\"AtributoClass\":\"\",\"Tipo\":\"Titulo\"}";

                    lData = DateTime.Now;

                    lWidResp = lServico.InserirWidget(lWidReq);

                    if (lWidResp.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                        pStatusString += string.Format("TestarOperacoesDePagina - InserirWidget(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                        lWidReq.Widget.CodigoWidget = lWidResp.Widget.CodigoWidget;
                        lWidReq.Widget.WidgetJson = "{\"Texto\":\"Teste2\",\"NivelDeTitulo\":1,\"IdDaEstrutura\":" + lCodEstrutura + ",\"IdDoWidget\":" + lWidReq.Widget.CodigoWidget.ToString() + ",\"IdDaLista\":0,\"Ordenacao\":null,\"DirecaoDeOrdenacao\":null,\"OrdemNaPagina\":1,\"AtributoStyle\":\"\",\"AtributoClass\":\"\",\"Tipo\":\"Titulo\"}";

                        lData = DateTime.Now;

                        lWidResp = lServico.InserirWidget(lWidReq);

                        if (lWidResp.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                            pStatusString += string.Format("TestarOperacoesDePagina - AtualizarWidget(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                            lData = DateTime.Now;

                            lWidResp = lServico.SelecionarWdiget(lWidReq);

                            if (lWidResp.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                                pStatusString += string.Format("TestarOperacoesDePagina - SelecionarWdiget(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);
                            }
                            else
                            {
                                pStatusString += string.Format("Erro em TestarOperacoesDePagina - SelecionarWdiget: [{0}]\r\n{1}<br/><br/>", lWidResp.StatusResposta, lWidResp.DescricaoResposta);
                            }
                        }
                        else
                        {
                            pStatusString += string.Format("Erro em TestarOperacoesDePagina - AtualizarWidget: [{0}]\r\n{1}<br/><br/>", lWidResp.StatusResposta, lWidResp.DescricaoResposta);
                        }

                        lData = DateTime.Now;

                        lWidResp = lServico.ApagarWidget(lWidReq);

                        if (lWidResp.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                            pStatusString += string.Format("TestarOperacoesDePagina - ApagarWidget(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);
                        }
                        else
                        {
                            pStatusString += string.Format("Erro em TestarOperacoesDePagina - ApagarWidget: [{0}]\r\n{1}<br/><br/>", lWidResp.StatusResposta, lWidResp.DescricaoResposta);
                        }
                    }
                    else
                    {
                        pStatusString += string.Format("Erro em TestarOperacoesDePagina - InserirWidget: [{0}]\r\n{1}<br/><br/>", lWidResp.StatusResposta, lWidResp.DescricaoResposta);
                    }
                    */

                    lPagResp = lServico.ExcluirPagina(lPagReq);
                    
                    if (lPagResp.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                        pStatusString += string.Format("TestarOperacoesDePagina - ExcluirPagina(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);
                    }
                    else
                    {
                        pStatusString += string.Format("Erro em TestarOperacoesDePagina - ExcluirPagina: [{0}]\r\n{1}<br/><br/>", lPagResp.StatusResposta, lPagResp.DescricaoResposta);
                    }
                }
                else
                {
                    pStatusString += string.Format("Erro em TestarOperacoesDePagina - SelecionarPagina: [{0}]\r\n{1}<br/><br/>", lPagResp.StatusResposta, lPagResp.DescricaoResposta);
                }
            }
            else
            {
                pStatusString += string.Format("Erro em TestarOperacoesDePagina - InserirPagina: [{0}]\r\n{1}<br/><br/>", lPagResp.StatusResposta, lPagResp.DescricaoResposta);
            }
        }

        private void TestarOperacoesDeConteudo(ref string pStatusString)
        {
            DateTime lData;
            TimeSpan lSpan;

            IServicoPersistenciaSite lServico = InstanciarServicoDoAtivador<IServicoPersistenciaSite>();

            ConteudoRequest lContReq = new ConteudoRequest();
            ConteudoResponse lContResp;

            lContReq.Conteudo = new ConteudoInfo();
            lContReq.Conteudo.CodigoTipoConteudo = ConfiguracoesValidadas.IdDoTipo_BannerLateral;
            lContReq.Conteudo.ValorPropriedade1 = "S";

            lData = DateTime.Now;

            lContResp = lServico.SelecionarConteudoPorPropriedade(lContReq);

            if (lContResp.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                pStatusString += string.Format("TestarOperacoesDeConteudo - SelecionarConteudoPorPropriedade(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                lContReq.Conteudo = new ConteudoInfo();

                lContReq.Conteudo.CodigoTipoConteudo = ConfiguracoesValidadas.IdDoTipo_BannerLateral;

                lData = DateTime.Now;

                lContResp = lServico.SelecionarConteudo(lContReq);

                lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                pStatusString += string.Format("TestarOperacoesDeConteudo - SelecionarConteudo(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                lContReq.Conteudo = lContResp.ListaConteudo[0];

                lContReq.Conteudo.CodigoConteudo = 0;   //para criar um novo, não atualizar

                lContReq.Conteudo.DtInicio = lContReq.Conteudo.DtFim = DateTime.Now;

                lData = DateTime.Now;

                lContResp = lServico.InserirConteudo(lContReq);
                
                if (lContResp.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                    pStatusString += string.Format("TestarOperacoesDeConteudo - InserirConteudo(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                    lContReq.Conteudo.CodigoConteudo = lContResp.Conteudo.CodigoConteudo;

                    lData = DateTime.Now;

                    lContResp = lServico.ApagarConteudo(lContReq);

                    if (lContResp.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                        pStatusString += string.Format("TestarOperacoesDeConteudo - ApagarConteudo(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);
                    }
                    else
                    {
                        pStatusString += string.Format("Erro em TestarOperacoesDeConteudo - ApagarConteudo: [{0}]\r\n{1}<br/><br/>", lContResp.StatusResposta, lContResp.DescricaoResposta);
                    }
                }
                else
                {
                    pStatusString += string.Format("Erro em TestarOperacoesDeConteudo - InserirConteudo: [{0}]\r\n{1}<br/><br/>", lContResp.StatusResposta, lContResp.DescricaoResposta);
                }
            }
            else
            {
                pStatusString += string.Format("Erro em TestarOperacoesDeConteudo - SelecionarConteudo: [{0}]\r\n{1}<br/><br/>", lContResp.StatusResposta, lContResp.DescricaoResposta);
            }
        }

        private void TestarOperacoesDeListaDeConteudo(ref string pStatusString)
        {
            DateTime lData;
            TimeSpan lSpan;

            IServicoPersistenciaSite lServico = InstanciarServicoDoAtivador<IServicoPersistenciaSite>();

            ListaConteudoRequest lReq = new ListaConteudoRequest();
            ListaConteudoResponse lResp;
            
            BuscarItensDaListaRequest lReqItens;
            BuscarItensDaListaResponse lRespItens;

            lReq.Conteudo = new ListaConteudoInfo();
            lReq.Conteudo.CodigoLista = ConfiguracoesValidadas.IdDaLista_BannersParaHome;

            lData = DateTime.Now;

            lResp = lServico.SelecionarListaConteudo(lReq);

            if (lResp.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                pStatusString += string.Format("TestarOperacoesDeListaDeConteudo - SelecionarListaConteudo(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                lReq.Conteudo = lResp.ListaConteudo[0];

                lReqItens = new BuscarItensDaListaRequest();
                lReqItens.IdDaLista = lReq.Conteudo.CodigoLista;

                lData = DateTime.Now;

                lRespItens = lServico.BuscarItensDaLista(lReqItens);

                if(lRespItens.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                    pStatusString += string.Format("TestarOperacoesDeListaDeConteudo - BuscarItensDaLista(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);
                }
                else
                {
                    pStatusString += string.Format("Erro em TestarOperacoesDeListaDeConteudo - BuscarItensDaLista: [{0}]\r\n{1}<br/><br/>", lResp.StatusResposta, lResp.DescricaoResposta);
                }

                lReq.Conteudo.CodigoLista = 0;  //zera pra incluir um novo

                lData = DateTime.Now;

                lResp = lServico.InserirListaConteudo(lReq);
                
                if (lResp.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                    pStatusString += string.Format("TestarOperacoesDeListaDeConteudo - InserirListaConteudo(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);

                    lData = DateTime.Now;

                    lReq.Conteudo.CodigoLista = lResp.Conteudo.CodigoLista;

                    lResp = lServico.ApagarListaConteudo(lReq);
                    
                    if (lResp.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lSpan = new TimeSpan(DateTime.Now.Ticks - lData.Ticks);

                        pStatusString += string.Format("TestarOperacoesDeListaDeConteudo - ApagarListaConteudo(): [{0:n0}]ms<br/><br/>", lSpan.TotalMilliseconds);
                    }
                    else
                    {
                        pStatusString += string.Format("Erro em TestarOperacoesDeListaDeConteudo - ApagarListaConteudo: [{0}]\r\n{1}<br/><br/>", lResp.StatusResposta, lResp.DescricaoResposta);
                    }

                }
                else
                {
                    pStatusString += string.Format("Erro em TestarOperacoesDeListaDeConteudo - InserirListaConteudo: [{0}]\r\n{1}<br/><br/>", lResp.StatusResposta, lResp.DescricaoResposta);
                }
            }
            else
            {
                pStatusString += string.Format("Erro em TestarOperacoesDeListaDeConteudo - SelecionarListaConteudo: [{0}]\r\n{1}<br/><br/>", lResp.StatusResposta, lResp.DescricaoResposta);
            }

        }

        private void IncluirDadosDeVendaNoBanco(ref VendaInfo pVenda)
        {
            InserirVendaRequest lRequest = new InserirVendaRequest();
            InserirVendaResponse lResponse;

            lRequest.Venda = pVenda;

            lResponse = base.ServicoPersistenciaSite.InserirVenda(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                pVenda.IdVenda = lResponse.IdDoRegistroIncluido;

                gLogger.InfoFormat("Dados da Venda incluídos com sucesso. ID: [{0}]", pVenda.IdVenda);

                InserirProdutosClienteRequest  lRequestProduto = new InserirProdutosClienteRequest();

                lRequestProduto.LstPlanoCliente = new List<PlanoClienteInfo>();

                /*
                
                // essa parte incluía o produto na tb_cliente_produto porém isso só deve ser feito depois que 
                // autorizarem o produto manualmente, pela tela alteração de vendas da intranet.
                // removendo daqui para o produto só entrar nessa tabela quando alterarem a venda para status 3 ou 4, 
                // na procedure sp_evnda_alteracao_ins

                foreach (VendaProdutoInfo lProduto in pVenda.Produtos)
                {
                    if (lProduto.IdProduto == ConfiguracoesValidadas.IdDoProduto_GradualTraderInterface
                     || lProduto.IdProduto == ConfiguracoesValidadas.IdDoProduto_StockMarket)
                    {
                        lRequestProduto.LstPlanoCliente.Add(new PlanoClienteInfo()
                        {
                            DsCpfCnpj = SessaoClienteLogado.CpfCnpj
                            , DtOperacao = DateTime.Now
                            , StSituacao = 'A'
                            , IdProdutoPlano = lProduto.IdProduto
                            , CdCblc = Convert.ToInt32(SessaoClienteLogado.CodigoPrincipal)
                        });
                    }
                }

                if(lRequestProduto.LstPlanoCliente.Count > 0)
                {
                    gLogger.InfoFormat("[{0}] Produtos para adicionar à conta do cliente, instanciando IServicoPlanoCliente", lRequestProduto.LstPlanoCliente.Count);

                    IServicoPlanoCliente lServicoPlano = InstanciarServicoDoAtivador<IServicoPlanoCliente>();

                    InserirProdutosClienteResponse lResponseProduto;

                    lResponseProduto = lServicoPlano.InserirPordutosCliente(lRequestProduto);

                    if (lResponseProduto.StatusResposta != MensagemResponseStatusEnum.OK)
                    {
                        gLogger.InfoFormat("Erro ao inserir produtos: [{0}] [{1}]", lResponseProduto.StatusResposta, lResponseProduto.DescricaoResposta);

                        throw new Exception("ERRO_INS_PRODUTO");
                    }
                    else
                    {
                        gLogger.Info("Produtos incluídos com sucesso!");
                    }
                }
                */
            }
            else
            {
                string lMensagem = string.Format("Erro ao inserir dados de venda no banco: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);

                gLogger.Error(lMensagem);

                throw new Exception(lMensagem);
            }
        }

        private void EnviarEmailDeVendaIniciadaParaCliente()
        {
            Dictionary<string, string> lParametrosDoEmail = new Dictionary<string, string>();

            lParametrosDoEmail.Add("###NOME###", base.SessaoClienteLogado.Nome);

            MensagemResponseStatusEnum lEnvio = base.EnviarEmail(base.SessaoClienteLogado.Email, "Compra realizada", "EmailCompraRealizada.html", lParametrosDoEmail, eTipoEmailDisparo.Todos);

            if (lEnvio == MensagemResponseStatusEnum.OK)
            {
                gLogger.InfoFormat("Email de compra efetuada enviado para [{0}] CBLC: [{1}] com sucesso.", SessaoClienteLogado.Nome, SessaoClienteLogado.CodigoPrincipal);
            }
            else
            {
                gLogger.ErrorFormat("Erro ao enviar email de compra efetuada para [{0}] CBLC: [{1}].", SessaoClienteLogado.Nome, SessaoClienteLogado.CodigoPrincipal);
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[]{
                                                        "BuscarCotacaoELivrosParaHome"
                                                      , "BuscarCotacaoRapida"
                                                      , "RetornarXMLTV"
                                                      , "BuscarSessaoParaHB"
                                                      , "SalvarFormularioGenerico"
                                                      , "BuscarCEP"
                                                      , "VisualizarConfiguracoes"
                                                      , "TestarPerformance"
                                                      , "AdicionarItemAoCarrinho"
                                                      , "RemoverItemDoCarrinho"
                                                      , "FinalizarCarrinho"
                                                      , "FinalizarCarrinhoOffline"
                                                      , "IniciarCompra"
                                                    },
                    new ResponderAcaoAjaxDelegate[]{
                                                        ResponderBuscarCotacaoELivrosParaHome
                                                      , ResponderBuscarCotacaoRapida
                                                      , ResponderRetornarXMLTV
                                                      , ResponderBuscarSessaoParaHB
                                                      , ResponderSalvarFormularioGenerico
                                                      , ResponderBuscarCEP
                                                      , ResponderVisualizarConfiguracoes
                                                      , ResponderTestarPerformance
                                                      , ResponderAdicionarItemAoCarrinho
                                                      , ResponderRemoverItemDoCarrinho
                                                      , ResponderFinalizarCarrinho
                                                      , ResponderFinalizarCarrinhoOffline
                                                      , ResponderIniciarCompra
                                                    });
        }

        #endregion
    }
}
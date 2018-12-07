<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="Gradual.Site.Www.Default" %>

<%@ Register src="Resc/UserControls/MapaDoSite.ascx"    tagname="MapaDoSite"    tagprefix="ucMapa" %>

<%@ Register src="Resc/UserControls/MenuPrincipal.ascx" tagname="MenuPrincipal" tagprefix="ucMenuPrincipal" %>
<%@ Register src="Resc/UserControls/PainelDeBusca.ascx" tagname="PainelDeBusca" tagprefix="ucPainelDeBusca" %>
<%@ Register src="Resc/UserControls/PainelDeLogin.ascx" tagname="PainelDeLogin" tagprefix="ucPainelDeLogin" %>
<%@ Register src="Resc/UserControls/Sauron.ascx"        tagname="Sauron"        tagprefix="ucSauron" %>

<html>

    <head>
        <meta name="keywords" content="investimentos, investir, home broker, wealth, management, trader, carteira, análise">

        <meta http-equiv="X-UA-Compatible" content="IE=edge" />

        <link href="https://fonts.googleapis.com/css?family=Roboto:400,400italic,700italic,700" rel="stylesheet" type="text/css">
        <link href="https://fonts.googleapis.com/css?family=Alice" rel="stylesheet" type="text/css">

        <title>Gradual Investimentos</title>

        <script src="Resc/Js/Lib/jquery-latest.min.js?v=<%=this.VersaoDoSite %>"></script>
        <script src="Resc/Js/Lib/jquery-ui-1.10.js?v=<%=this.VersaoDoSite %>"></script>
        <script src="Resc/Js/Lib/unslider.min.js?v=<%=this.VersaoDoSite %>"></script>
        <script src="Resc/Js/Lib/jquery.jplayer.min.js?v=<%=this.VersaoDoSite %>"></script>
        <script src="Resc/Js/Lib/jquery.keyboard.min.js?v=<%=this.VersaoDoSite %>"></script>

        <script src="Resc/Js/Lib/jquery.validationEngine.js?v=<%=this.VersaoDoSite %>"></script>
        <script src="Resc/Js/Lib/jquery.validationEngine-ptBR.js?v=<%=this.VersaoDoSite %>"></script>
        <script src="Resc/Js/Lib/jquery.maskedinput-1.2.2.js?v=<%=this.VersaoDoSite %>"></script>

        <script src="Resc/Js/Site/00-Base.js"></script>
        <script src="Resc/Js/Site/01-Principal.js"></script> 

        <link rel="Stylesheet" type="text/css" href="Resc/Skin/<%=this.SkinEmUso %>/01-Principal.css?v=<%=this.VersaoDoSite %>" />

        <link rel="Stylesheet" type="text/css" href="Resc/Skin/<%=this.SkinEmUso %>/02-ModuloCMS.css?v=<%=this.VersaoDoSite %>" />

        <link rel="Stylesheet" type="text/css" href="Resc/Skin/<%=this.SkinEmUso %>/42-PaginaInicial.css?v=<%=this.VersaoDoSite %>" />
        <link rel="Stylesheet" type="text/css" href="Resc/Skin/<%=this.SkinEmUso %>/48-keyboard-New.css?v=<%=this.VersaoDoSite %>" />

        <% if(Request.Browser.Browser.ToLower() == "internetexplorer") { %>
        <link rel="Stylesheet" media="screen" href="Resc/Skin/<%= this.SkinEmUso %>/99-IE.css?v=<%= this.VersaoDoSite %>&ms=<%= new Random().Next(1, 9999) %>" />
        <% } %>

    </head>

    <body onload="Page_Load()">

        <form id="form1" runat="server">

        <div style="display:none">

            <p class="DadosPrincipais">

                <span style="width:72px; text-align:left">
                    <a href="AnalisesEMercado/IGB30.aspx">IGB30</a>
                </span>

                <span style="width:78px">
                    <asp:literal id="lblCotacaoIBOV30_Dados_Ultima" runat="server"></asp:literal>
                </span>

                <span id="lblCotacaoIBOV30_Dados_Variacao" style="width:55px;">
                    <asp:literal id="lblCotacaoIBOV30_Dados_Variacao" runat="server"></asp:literal>
                </span>

            </p>

            <p class="DadosPrincipais">

                <span id="lblCotacoes_Dados_Ativo">
                    <asp:literal id="lblCotacoes_Dados_Ativo" runat="server"></asp:literal>
                </span>

                <span id="lblCotacoes_Dados_Ultima">
                    <asp:literal id="lblCotacoes_Dados_Ultima" runat="server"></asp:literal>
                </span>

                <span id="lblCotacoes_Dados_Variacao">
                    <asp:literal id="lblCotacoes_Dados_Variacao" runat="server"></asp:literal>
                </span>

            </p>

            <table>
                <asp:repeater id="rptOfertasDeCompra" runat="server">
                <itemtemplate>

                <tr>
                    <td><%#Eval("NomeCorretora") %></td>
                    <td><%#Eval("QuantidadeAbreviada") %></td>
                    <td><%#Eval("Preco") %></td>
                </tr>

                </itemtemplate>
                </asp:repeater>
            </table>

            <table>
                <asp:repeater id="rptOfertasDeVenda" runat="server">
                <itemtemplate>

                <tr>
                    <td><%#Eval("NomeCorretora") %></td>
                    <td><%#Eval("QuantidadeAbreviada") %></td>
                    <td><%#Eval("Preco") %></td>
                </tr>

                </itemtemplate>
                </asp:repeater>
            </table>

            <div id="pnlCotacoesFixas">

                <div>
                    <h1>IBOV</h1>
                    <h2 class="Zerado"> <asp:literal id="lblConteudoTerciario_CotacaoIBOV_Variacao" runat="server"></asp:literal> </h2>
                    <span><asp:literal id="lblConteudoTerciario_CotacaoIBOV_Ultima" runat="server"></asp:literal></span>
                </div>

                <div>
                    <h1>DJI</h1>
                    <h2  class="Zerado"><asp:literal id="lblConteudoTerciario_CotacaoDJI_Variacao" runat="server"></asp:literal></h2>
                    <span><asp:literal id="lblConteudoTerciario_CotacaoDJI_Ultima" runat="server"></asp:literal></span>
                </div>

                <div>
                    <h1>Dólar</h1>
                    <h2  class="Zerado"><asp:literal id="lblConteudoTerciario_CotacaoDolar_Variacao" runat="server"></asp:literal></h2>
                    <span><asp:literal id="lblConteudoTerciario_CotacaoDolar_Ultima" runat="server"></asp:literal></span>
                </div>

                <div style="background:none">
                    <h1>DI</h1>
                    <h2  class="Zerado"><asp:literal id="lblConteudoTerciario_CotacaoDI_Variacao" runat="server"></asp:literal></h2>
                    <span><asp:literal id="lblConteudoTerciario_CotacaoDI_Ultima" runat="server"></asp:literal></span>
                </div>

            </div>


        </div>

        
        <div id="header">
            <div class="inner">

            <ucPainelDeLogin:PainelDeLogin id="ucPainelDeLogin1" runat="server" AparecerVisivel="true" />

            </div>
        </div>

        <ucMenuPrincipal:MenuPrincipal id="ucMenuPrincipal1" runat="server" />

        <!-- banner -->

        <div id="banner-area" style="display:none;">
            <ul>
            
                <asp:repeater id="rptBanners" runat="server">
                <itemtemplate>
                <li>
                    <div class="Banner banner-preload" data-url="<%#Eval("Link") %>" onclick="return divBanner_Click(this)" style="<%# (Container.ItemIndex == 0) ? "display:block" : "" %>">
                        
                        <div class="<%#Eval("Classe") %>" data-url="<%#Eval("Imagem") %>" data-opcoes="<%#Eval("Opcoes") %>">
                            <a href="<%#Eval("Link") %>">
                                <%#Eval("TagRenderizada") %>
                            </a>
                        </div>
                    </div>
                </li>
                </itemtemplate>
                </asp:repeater>

            </ul>
        </div>
        
        <!-- fim banner -->
        
        <!-- conteudo -->



        <div id="conteudo" style="display:none;">

            <div class="inner">
            
                <ul class="corretora clear">
                    <li>
                        <h3>Ações, Opções e BM&F</h3>
                        <p> Na hora de investir nos produtos da BM&FBovespa, conte com quem oferece o melhor para você. </p>
                        <a href="Investimentos/AcoeseOpcoes" class="btn-saiba-mais">Saiba mais</a>
                    </li>
                    <li>
                        <h3>Fundos de Investimento</h3>
                        <p> Conheça a nossa plataforma com os melhores Fundos<br /> e invista com praticidade e segurança.</p>
                        <a href="Investimentos/FundosDeInvestimentos" class="btn-saiba-mais">Saiba mais</a>
                    </li>
                    <li>
                        <h3>Renda Fixa</h3>
                        <p>Aproveite para diversificar a sua carteira com investimento em títulos públicos e privados de renda fixa. </p>
                        <a href="Investimentos/RendaFixa" class="btn-saiba-mais">Saiba mais</a>
                    </li>
                    <li>
                        <h3>Câmbio</h3>
                        <p> Agora você conta com um serviço diferenciado e a melhor taxa do mercado para adquirir moedas estrangeiras. </p>
                        <a href="Investimentos/Cambio.aspx" class="btn-saiba-mais">Saiba mais</a>
                    </li>
                </ul>
            </div>

            <div class="borda-full clear" style="padding-bottom:40px">

                <div class="inner">
                    <div class="analises-noticias clear">
                        <div class="chat-ao-vivo clear">
                            <h2>Plantão e Chat Gradual</h2>

                            <asp:literal id="litVideoHome" runat="server"></asp:literal>
                        </div>

                        <h2>Análises <span>&amp;</span> Notícias</h2>

                        <p>
                            Fique por dentro de conteúdos especiais, que vão auxiliá-lo na tomada<br />
                            de decisões e a ficar sempre antenado nas melhores oportunidades.<br />
                            Nesta área, você acompanha os principais acontecimentos da economia,<br />
                            informações sobre o funcionamento do mercado e avisos importantes.
                        </p>
                        <p style="border-bottom:1px dotted #dcdad8; padding-bottom:28px; ">
                            <a href="AnaliseseNoticias/Default/NoticiaseAvisos" class="saiba-mais">Saiba Mais</a>
                        </p>

                        <!--ul>

                        <asp:repeater id="rptDestaques" runat="server">
                        <itemtemplate>

                            <li>
                                <h3><%#Eval("Titulo") %></h3>
                                <p><%#Eval("Conteudo") %></p>
                                <a class="saiba-mais" href="<%#Eval("Link") %>">Saiba Mais</a>
                            </li>

                        </itemtemplate>
                        </asp:repeater>

                        </ul-->

                        <h2>Ofertas Públicas</h2>
                        <ul class="lista-ofertas-publicas" style="margin-top:-7px">

                            <asp:repeater id="rptOfertasPublicas" runat="server">
                            <itemtemplate>

                                <li>
                                    <a href="Investimentos/OfertaPublica.aspx?id=<%#Eval("CodigoConteudo") %>"><%#Eval("Titulo") %></a>
                                    <!--p><a href="Investimentos/OfertaPublica.aspx?id=<%#Eval("CodigoConteudo") %>"><%#Eval("Titulo") %></a></p-->
                                </li>

                            </itemtemplate>
                            </asp:repeater>

                        </ul>

                        <p style="margin-top:0px; padding:5px 0 5px 10px; ">
                            <a href="Investimentos/OfertaPublica" class="saiba-mais">Veja Todas</a>
                        </p>

                    </div>

                    <!--div class="ofertas-publicas">

                    </div-->

                    <div class="small-col" style="">
                        <div class="destaques clear">
                            <h2>Gradual 360º</h2>
                            <ul>
                                <li>
                                    <div class="corretora-logo"><img src="Resc/Skin/<%=this.SkinEmUso %>/Img/corretora.png"></div>
                                    <div>
                                        <p>
                                            Garante a você um portfólio e <br />
                                            plataformas de negociação avançadas <br />
                                            para aplicações em Ações, Renda Fixa, <br />
                                            Derivativos e muito mais.
                                            <span style="font-size:6px;line-height: 12px">
                                            <br /><br />
                                            </span>
                                            <a class="saiba-mais" href="QuemSomos/Atuacao#Aba-GradualCorretora">Saiba Mais</a>
                                        </p>
                                    </div>
                                </li>
                                
                                <li class="noBorder">
                                    <div class="corretora-logo"><img src="Resc/Skin/<%=this.SkinEmUso %>/Img/wealth.png" style="margin-top: 10px"></div>
                                    <div>
                                        <p>
                                            Oferece um atendimento personalizado 
                                            para o seu perfil e objetivo, através de 
                                            uma equipe pronta para entender 
                                            suas necessidades e realizar o seu 
                                            planejamento financeiro pessoal e empresarial.
                                            <span style="font-size:6px;line-height: 12px">
                                            <br /><br />
                                            </span>
                                            <a class="saiba-mais" href="QuemSomos/Atuacao#Aba-WealthManagement">Saiba Mais</a>
                                        </p>
                                    </div>
                                </li>
                                    
                                <li class="noBorder">
                                    <div class="corretora-logo"><img src="Resc/Skin/<%=this.SkinEmUso %>/Img/financial.png" style="margin-top:10px;margin-left:8px"></div>
                                    <div>
                                        <p>
                                            Selecionamos os melhores parceiros para você!
                                            Conte com o auxílio especializado no planejamento corporativo 
                                            e desenvolvimento das estratégias de aumento de capital, 
                                            como administração de dívida e patrimônios.
                                            <span style="font-size:6px;line-height: 12px">
                                            <br /><br />
                                            </span>
                                            <a class="saiba-mais" href="QuemSomos/Atuacao#Aba-FinancialAdvisory">Saiba Mais</a>
                                        </p>
                                    </div>
                                </li>
                                    
                                <li class="noBorder">
                                    <div class="corretora-logo"><img src="Resc/Skin/<%=this.SkinEmUso %>/Img/asset.png" style="margin-top:20px"></div>
                                    <div>
                                        <p>
                                            Realize o planejamento dos seus investimentos com 
                                            profissionais altamente qualificados na gestão de 
                                            carteiras preparados para orientá-lo nas suas decisões.
                                            <span style="font-size:6px;line-height: 12px">
                                            <br /><br />
                                            </span>
                                            <a class="saiba-mais" href="QuemSomos/Atuacao#Aba-AssetManagement">Saiba Mais</a>
                                        </p>
                                    </div>
                                </li>
                            </ul>
                        </div>

                    </div>

                    
                </div>

                <div style="display:inline-block; text-align:center; width:100%; padding-top:24px; margin-bottom:-16px; border-top: 1px solid #dcdad8">
                
                    <div class="destaque-fundo">
                        <a href="AnaliseseNoticias/Default/IGB30">
                            <img src="Resc/Skin/<%=this.SkinEmUso %>/Img/destaques-igb30.jpg" />
                        </a>
                    </div>

                    <div class="destaque-fundo">
                        <a href="PlataformasdeNegociacao/PlataformaDeFundos">
                            <img src="Resc/Skin/<%=this.SkinEmUso %>/Img/bannerzinho-Fundos.jpg" />
                        </a>
                    </div>

                    <div class="destaque-fundo">
                        <a href="PlataformasdeNegociacao/HB">
                            <img src="Resc/Skin/<%=this.SkinEmUso %>/Img/destaques-homebroker.jpg" />
                        </a>
                    </div>

                    <div class="destaque-fundo">
                        <a href="PlataformasdeNegociacao/Profitchart">
                            <img src="Resc/Skin/<%=this.SkinEmUso %>/Img/destaques-profitchart.jpg" />
                        </a>
                    </div>

                </div>

            </div>


            <div id="gradual-texto">
               <h3>Na Gradual o melhor investimento é você.</h3>
               <p>Aqui você realiza a compra, venda e aluguel de ações, além de investir em Fundos de Investimento, Renda Fixa, Tesouro Direto, BM&F e muito mais. Tudo isso com o apoio de ferramentas da alta performance e recomendações exclusivas. Invista já!</p>
            

                <div class="row" style="text-align:center; padding:10px 0px 0px 0px; margin:0px 0px 0px 0px;">
                <p><strong>Atendimento:</strong> (11) 4007-1873 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Ouvidoria: </strong> 0800 655 1466 </p>
                </div>

                <div class="row" style="text-align:center; padding:10px 0px 0px 0px; margin:0px 0px 0px 0px;">

                <a style="" class="abra-sua-conta-rodape" href="MinhaConta/NovoCadastro.aspx">Abra sua conta</a>

                </div>
            </div>

        </div>

        <!-- fim conteudo -->

        <input type="hidden" id="hidRaiz" value="<%= this.HostERaiz %>" />
        <input type="hidden" id="Token" value="" />

        <script language='javascript'>

            function Page_Load_CodeBehind()
            {
                <asp:literal id="litJavascriptOnLoad" runat="server"></asp:literal>
            }

            $(document).ready(Page_Load);

            $(window).on('load',function()
            {
                $('#pnlModal').modal('show');
            });

        </script>

        <input type="hidden" id="hidServerData" runat="server" />

        <ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Home" />

        </form>
    </body>
</html>

/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />

var gFlagBuscandoCotacao = false;

var gIndiceDoBannerAtual, gIndiceDoFrameAtual;

var gBanners = [];

var gTimeOutDoProximoFrame;

var gDuracaoDaTransicao = 300;

function Page_Load()
{
    var lHash = document.location.hash;

    if (lHash != "")
        PaginaInicial_SelecionarCatalogo( $("a[href='" + lHash + "']") );

    try
    {
        PaginaInicial_Banners_IniciarBanners();
    }
    catch (erro) { }

    PaginaInicial_AtualizarValoresDeCotacaoPositivosNegativos();

    txtMapaDoSite_BuscaRapida = $("#txtMapaDoSite_BuscaRapida");

    gLIsDoMapa = $("#pnlMapaDoSite > ul > li");

    //CarregarTVHome(); //Carrega a TV na Home

    PaginaInicial_AtualizarPaginacaoDestaquesNoticias();

    if($.browser.msie)
        GradSite_PrepararParaIE();

    if(document.location.href.toLowerCase().indexOf("localhost") != -1)
    {
        window.setTimeout(function()
        {
        //alert($("img.keyboardInputInitiator").length);
        $("img.keyboardInputInitiator")
            .attr("src", "http://localhost/Gradual.Site.Www/Resc/Skin/Default/Img/keyboard.bmp")
            .css( { marginTop: 7, marginLeft: 3, float: "left" } );
        }, 200);
    }

    GradSite_CorrigirLinksAtendimentoOnline();

    $('.password').pstrength();

    var pnlAnaliseGrafica = $("#pnlBannerAnaliseGrafica");

    if (pnlAnaliseGrafica.length > 0)
    {
        var lData = new Date();

        var lDiaDaSemana = lData.getDay();
        var lHora = lData.getHours();

        var lTexto = "";

        if (lDiaDaSemana == 0 || lDiaDaSemana >= 6)
        {
            lDiaDaSemana = 1;
        }
        else
        {
            if (lHora > 11)
                lDiaDaSemana += 1;

            if (lDiaDaSemana >= 6)
                lDiaDaSemana = 1;
        }

        if (lDiaDaSemana == 1)
        {
            lTexto = "Segunda";
        }
        else if (lDiaDaSemana == 2)
        {
            lTexto = "Terça";
        }
        else if (lDiaDaSemana == 3)
        {
            lTexto = "Quarta";
        }
        else if (lDiaDaSemana == 4)
        {
            lTexto = "Quinta";
        }
        else if (lDiaDaSemana == 5)
        {
            lTexto = "Sexta";
        }

        pnlAnaliseGrafica
            .html("<span>" + lTexto + ", às 9:30</span>")
            .click(function() { document.location = "AnalisesEMercado/Chats.aspx"; } );
    }
    
    GradSite_IniciarAnalytics();

    Page_Load_CodeBehind();

}

function CarregarTVHome()
{
    var lRequest = { Acao: "RetornarXMLTVHome" };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Default.aspx"), lRequest, ModuloTV_RetornarXMLTVHome_CallBack);
}

function ModuloTV_RetornarXMLTVHome_CallBack(pResposta)
{
    var lXML = pResposta.ObjetoDeRetorno;

    var lblTitulo = $("#lblVideoEmDestaque_Titulo");

    var lblDescricao = $("#lblVideoEmDestaque_Descricao a");

    var ImgVideo = $("#imgVideoEmDestaque");

    lblTitulo.html(lXML[0].Titulo);

    lblDescricao
        .html(lXML[0].Descricao)
        .attr("href", "TV/Default.aspx#" + lXML[0].ID);

    ImgVideo.attr("src", lXML[0].UrlDaImagem);
}

function PaginaInicial_Banners_IniciarBanners()
{
    var lDivsBanners = $("#pnlBanners_Container div.Banner");

    var lDivBanner, lBanner;

    var lImagens, lImagem;

    var lNav = $("#pnlBanners_Container > nav").html("");

    var lContador = 1;

    lDivsBanners.each(function()
    {
        lDivBanner = $(this);

        lImagens = lDivBanner.find("img");

        lBanner = new Object();

        lBanner = { URL: lDivBanner.attr("data-URL"), Imagens: [] };

        lImagens.each(function()
        {
            lImagem = $(this);

            lBanner.Imagens.push( {
                                        Titulo:     lImagem.attr("title")
                                      , Duracao:    lImagem.attr("data-duracao")
                                      , Transicao:  lImagem.attr("data-transicao")
                                  });
        });

        gBanners.push( lBanner );

        lNav.append("<button onclick='return btnBanner_SelecionarBanner_Click(this)' class='" + ((lContador == 1) ? "Selecionado" : "") + "'>" + lContador + "</button>");

        lContador++;
    });

    gIndiceDoBannerAtual = 0;
    gIndiceDoFrameAtual = 0;

    $("#pnlBanners_Container div.Banner img").css( { display: "none" } );

    $("#pnlBanners_Container div.Banner").css( { display: "block" } );

    $("#pnlBanners_Container div.Banner:eq(0) img:eq(0)").css( { display: "block" } );
    
    gBanners.FrameAtual = function() { return gBanners[gIndiceDoBannerAtual].Imagens[gIndiceDoFrameAtual]; };
    
    gBanners.ImagemAtual = function() { return $("#pnlBanners_Container div.Banner:eq(" + gIndiceDoBannerAtual + ") img:eq(" + gIndiceDoFrameAtual + ")"); };

    gTimeOutDoProximoFrame = window.setTimeout(PaginaInicial_Banners_ProximoFrame, (gBanners[gIndiceDoBannerAtual].Imagens[gIndiceDoFrameAtual].Duracao * 1000));
}


function PaginaInicial_Banners_ProximoFrame()
{
    if((gIndiceDoFrameAtual + 1) >= gBanners[gIndiceDoBannerAtual].Imagens.length)
    {
        //tem que mudar de banner!
        if((gIndiceDoBannerAtual + 1) >= gBanners.length)
        {
            gIndiceDoBannerAtual = 0;
        }
        else
        {
            gIndiceDoBannerAtual++;
        }

        gIndiceDoFrameAtual = 0;

        $("#pnlBanners_Container nav button").removeClass("Selecionado");
        $("#pnlBanners_Container nav button:eq(" + gIndiceDoBannerAtual + ")").addClass("Selecionado");
    }
    else
    {
        //somente próximo frame mesmo
        gIndiceDoFrameAtual++;
    }

    var lImagem = gBanners.ImagemAtual();

    var lFrame = gBanners.FrameAtual();
    
    lImagem
        .addClass("Transitando")
        .css(       { opacity: 0, display: "block", zIndex: 300 } )

    if(lFrame.Transicao == "slideDireita")
    {
        lImagem
            .css(       { opacity: 1, marginLeft: 840 } )
            .animate(   { marginLeft: 0 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
    else if(lFrame.Transicao == "slideEsquerda")
    {
        lImagem
            .css(       { opacity: 1, marginLeft: -840 } )
            .animate(   { marginLeft: 0 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
    else if(lFrame.Transicao == "slideTopo")
    {
        lImagem
            .css(       { opacity: 1, marginTop: -240 } )
            .animate(   { marginTop: 0 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
    else if(lFrame.Transicao == "slideBaixo")
    {
        lImagem
            .css(       { opacity: 1, marginTop: 240 } )
            .animate(   { marginTop: 0 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
    else if(lFrame.Transicao == "inCentro")
    {
        lImagem
            .css(       { opacity: 0, width: 504, height: 144, marginLeft: 168, marginTop: 48} )
            .animate(   { opacity: 1, width: 840, height: 240, marginLeft:   0, marginTop: 0 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
    else if(lFrame.Transicao == "outCentro")
    {
        lImagem
            .css(       { opacity: 0, width: 924, height: 264, marginLeft: -42, marginTop: -12} )
            .animate(   { opacity: 1, width: 840, height: 240, marginLeft:   0, marginTop:   0 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
    else
    {
        //a padrão é "fade"

        lImagem
            .animate(   { opacity: 1 }
                        , gDuracaoDaTransicao
                        , PaginaInicial_Banners_FimDoFrame );
    }
}

function PaginaInicial_Banners_FimDoFrame()
{
    $("#pnlBanners_Container div.Banner img[class!='Transitando']").hide();

    $("#pnlBanners_Container div.Banner img.Transitando").removeClass("Transitando").css( { zIndex: 0 } );

    gTimeOutDoProximoFrame = window.setTimeout(PaginaInicial_Banners_ProximoFrame, (gBanners[gIndiceDoBannerAtual].Imagens[gIndiceDoFrameAtual].Duracao * 1000));
}


function PaginaInicial_SelecionarBanner(pIndice)
{
    window.clearTimeout(gTimeOutDoProximoFrame);

    gIndiceDoBannerAtual = pIndice;

    gIndiceDoFrameAtual = -1;

    PaginaInicial_Banners_ProximoFrame();
}

function PaginaInicial_SelecionarCatalogo(pSender)
{
    pSender = $(pSender);

    pSender.parent().find("a").removeClass("Selecionado");

    pSender.addClass("Selecionado");

    var lSecao = pSender.attr("href").substr(1);

    pSender.closest("section").find(">section").hide();

    $("#pnlCatalogo-" + lSecao).show();
}

function PaginaInicial_AtualizarValoresDeCotacaoPositivosNegativos()
{
    var lTD;
    var lTexto;

    $("#tblConteudoSecundario_Cotacoes tbody tr").each(function()
    {
        lTD = $(this).find("td");

        //alert(lTD.text());

        lTexto = lTD.text().trim();

        if(lTexto != "" && lTexto != "&mdash;")
        {
            if(lTexto.charAt(0) == "-")
            {
                lTD
                    .removeClass("ValorPositivo").addClass("ValorNegativo");
            }
            else
            {
                lTD
                    .removeClass("ValorNegativo");
                    //.addClass("ValorPositivo");
            }
        }
        else
        {
            lTD.html("&mdash;");
        }
    });

    lTD = $("#lblCotacoes_Dados_Variacao");

    lTexto = lTD.text().trim();

    if(lTexto != "" && lTexto != "&mdash;")
    {
        if(lTexto.charAt(0) == "-")
        {
            lTD.removeClass("ValorPositivo").addClass("ValorNegativo");
        }
        else
        {
            lTD.removeClass("ValorNegativo").addClass("ValorPositivo");
        }
    }

    
    lTD = $("#lblCotacaoIBOV30_Dados_Variacao");

    lTexto = lTD.text().trim();

    if(lTexto != "" && lTexto != "&mdash;")
    {
        if(lTexto.charAt(0) == "-")
        {
            lTD.removeClass("ValorPositivo").addClass("ValorNegativo");
        }
        else
        {
            lTD.removeClass("ValorNegativo").addClass("ValorPositivo");
        }
    }

    $("#pnlCotacoesFixas > div > h2").each(function()
    {
        lTD = $(this);

        lTD.removeClass("Subindo").removeClass("Descendo").removeClass("Zerado");

        lTexto = lTD.text().trim();

        if(lTexto == "+0,00" || lTexto == "n/d")
        {
            lTD.addClass("Zerado");
        }
        else if(lTexto.charAt(0) == "-")
        {
            lTD.addClass("Descendo");
        }
        else
        {
            lTD.addClass("Subindo");
        }
    });

    //exibe o botao de scroll das ofertas públicas se tiver mais de 5 itens:
    if($("#lstOfertasPublicas li").length > 6)
    {
        $("#btnScrollOfertasPublicas").show();
    }
    else
    {
        $("#btnScrollOfertasPublicas").hide();
    }
}

function PaginaInicial_OcultarExibirOfertasPublicas(pExibir)
{
    if(pExibir)
    {
        $("#lstOfertasPublicas").removeClass("Minimizado");

        $("#pnlDestaques").removeClass("Expandido")
    }
    else
    {
        $("#lstOfertasPublicas").addClass("Minimizado");

        $("#pnlDestaques").addClass("Expandido");
    }
}

function PaginaInicial_AtualizarPaginacaoDestaquesNoticias()
{
    var lQuandidadeNoticias = $("div.pnlDestaques_Container_Noticia").length;

    var lQuantidadePaginas = Math.ceil(lQuandidadeNoticias / 2);

    var lListaDePaginacao = $("ul.pnlDestaques_Paginas");

    if(lQuantidadePaginas > 1)
    {
        lListaDePaginacao.html("");

        for(var a = 1; a <= lQuantidadePaginas; a++)
        {
            lListaDePaginacao.append("<li>  <a href='#' onclick='return pnlDestaques_Paginas_Link_Click(this)'>" + a + "</a>  </li>");
        }

        lListaDePaginacao.find("li:eq(0)").addClass("Selecionado");

        lListaDePaginacao.show();
    }
    else
    {
        lListaDePaginacao.hide();
    }
}

function PaginaInicial_BuscarCotacaoELivros(pAtivo)
{
    gFlagBuscandoCotacao = true;

    PaginaInicial_ExibirOcultarPainelCarregandoCotacoes(true);

    window.setTimeout(function()
    {
        var lDados = { Acao: "BuscarCotacaoELivrosParaHome", Ativo: pAtivo };

        GradSite_CarregarJsonVerificandoErro("Async/Geral.aspx", lDados, PaginaInicial_BuscarCotacaoELivros_CallBack);
    }, 500);
}

function PaginaInicial_BuscarCotacaoELivros_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        var lDados = pResposta.ObjetoDeRetorno.DadosDeCotacao;

        var lTabela = $("#tblConteudoSecundario_Cotacoes");
        var lTR;
        var lTRVazia = "<tr>   <td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>   </tr>";

        $("#lblCotacoes_Dados_Ativo").html(     lDados.jCN ).effect("highlight");
        $("#lblCotacoes_Dados_Ultima").html(    lDados.jPC ).effect("highlight");
        $("#lblCotacoes_Dados_Variacao").html(  lDados.jVR ).effect("highlight");

        //lTabela.find("thead tr:eq(0) td:eq(0)").html( lDados.jCN );
        //lTabela.find("thead tr:eq(0) td:eq(1)").html( lDados.jPC + " <span class='Variacao'>" + lDados.jVR + "</span>");

        lTabela.find("tbody tr:eq(0) td").html( NumConv.StrToPretty( lDados.jMPC , 2) );
        lTabela.find("tbody tr:eq(1) td").html( NumConv.StrToPretty( lDados.jMPV , 2) );
        lTabela.find("tbody tr:eq(2) td").html( NumConv.StrToPretty( lDados.jXD  , 2) );
        lTabela.find("tbody tr:eq(3) td").html( NumConv.StrToPretty( lDados.jND  , 2) );
        lTabela.find("tbody tr:eq(4) td").html( NumConv.StrToPretty( lDados.jVA  , 2) );
        lTabela.find("tbody tr:eq(5) td").html( NumConv.StrToPretty( lDados.jVF  , 2) );
        lTabela.find("tbody tr:eq(6) td").html( NumConv.StrToPretty( lDados.jNN  , 0) );
        lTabela.find("tbody tr:eq(7) td").html( NumConv.StrToPretty( lDados.jLA  , 0) );
        //lTabela.find("tbody tr:eq(8) td").html( lDados.jVF );
        //lTabela.find("tbody tr:eq(9) td").html( lDados.jNN );

        PaginaInicial_AtualizarValoresDeCotacaoPositivosNegativos();

        lDados = pResposta.ObjetoDeRetorno.DadosDeLivro;

        lTabela = $("#tblConteudoSecundario_OfertasDeCompra tbody");

        lTabela.html("");

        for(var a = 0; a < 7; a++)
        {
            lTabela.append(lTRVazia);

            if(lDados.OfertasDeCompra.length > a)
            {
                lTR = lTabela.find("tr:eq(" + a + ")");

                lTR.find("td:eq(0)").html( lDados.OfertasDeCompra[a].NomeCorretora );
                lTR.find("td:eq(1)").html( lDados.OfertasDeCompra[a].Quantidade );
                lTR.find("td:eq(2)").html( lDados.OfertasDeCompra[a].Preco );
            }
        }

        lTabela = $("#tblConteudoSecundario_OfertasDeVenda tbody");

        lTabela.html("");

        for(var a = 0; a < 7; a++)
        {
            lTabela.append(lTRVazia);

            if(lDados.OfertasDeVenda.length > a)
            {
                lTR = lTabela.find("tr:eq(" + a + ")");

                lTR.find("td:eq(0)").html( lDados.OfertasDeVenda[a].NomeCorretora );
                lTR.find("td:eq(1)").html( lDados.OfertasDeVenda[a].Quantidade );
                lTR.find("td:eq(2)").html( lDados.OfertasDeVenda[a].Preco );
            }
        }
    }
    else
    {
        GradSite_ExibirMensagem("A", pResposta.Mensagem, true);
    }

    PaginaInicial_ExibirOcultarPainelCarregandoCotacoes(false);

    gFlagBuscandoCotacao = false;
}

function PaginaInicial_ExibirOcultarPainelCarregandoCotacoes(pExibir)
{
    if(pExibir)
    {
        $("#pnlConteudoSecundario-Cotacoes-Carregando")
            .css( { opacity: 0 } )
            .show()
            .animate( { opacity: 1 }, 300 );
    }
    else
    {
        $("#pnlConteudoSecundario-Cotacoes-Carregando")
            .animate( { opacity: 0 }, 300, function(){ $(this).hide(); }  );
    }
}








/*      Event Handlers          */

function divBanner_Click(pSender)
{
    document.location = gBanners[gIndiceDoBannerAtual].URL;
}

function btnBanner_SelecionarBanner_Click(pSender)
{
    pSender = $(pSender);

    if(!pSender.hasClass("Selecionado"))
    {
        pSender.parent().find(".Selecionado").removeClass("Selecionado");

        pSender.addClass("Selecionado");

        PaginaInicial_SelecionarBanner(pSender.index());
    }

    return false;
}

function lnkCatalogo_MouseMove(pSender)
{
    PaginaInicial_SelecionarCatalogo(pSender);
}

function lnkCatalogo_Click(pSender)
{
    PaginaInicial_SelecionarCatalogo(pSender);
}

function lnkAbaCotacaoRapida_Click(pSender, pIndice)
{
    pSender = $(pSender);

    if(!pSender.hasClass("Selecionado"))
    {
        pSender.parent().find("a").removeClass("Selecionado");

        pSender.addClass("Selecionado");

        pSender.closest("div").find("table").hide();

        pSender.closest("div").find("table:eq(" + pIndice + ")").show();
    }

    return false;
}

function lnkConteudoSecundario_Cotacoes_BuscarCotacao_Click(pSender)
{
    if(!gFlagBuscandoCotacao)
    {
        var lAtivo = $("#txtCotacaoAtivo").val().trim().toUpperCase();

        if(lAtivo.length < 3)
        {
            GradSite_ExibirMensagem("A", "Código de ativo inválido", true);
        }
        else
        {
            PaginaInicial_BuscarCotacaoELivros(lAtivo);
        }
    }

    return false;
}

function btnScrollOfertasPublicas_Click(pSender)
{
    pSender = $(pSender);

    var lUL = $("#lstOfertasPublicas ul");
    //SetaPraCima

    if(!pSender.hasClass("SetaPraCima"))
    {
        var lDiff = 327 - lUL.height();     //magic number: 327 é a altura do painel que está visível sem scroll

        lUL.animate( { marginTop: lDiff } );

        pSender.addClass("SetaPraCima");
    }
    else
    {
        lUL.animate( { marginTop: 0 } );

        pSender.removeClass("SetaPraCima");
    }

    return false;
}


function pnlDestaques_Paginas_Link_Click(pSender)
{
    var lLink = $(pSender);
    var   lLI = lLink.parent();

    if( !lLI.hasClass("Selecionado") )
    {
        var lPainelScroll = $("div.pnlDestaques_Container_Scroll");

        var lPagina = (lLink.text() - 1);

        var lMarginLeft = (lPagina * -372); //magic number: 372 é a largura de uma página

        lPainelScroll.animate( { marginLeft: lMarginLeft }, 300);

        lLI.parent().find("li.Selecionado").removeClass("Selecionado");

        lLI.addClass("Selecionado");
    }

    return false;
}


function txtCotacaoAtivo_OnKeyDown(pSender, pEvent)
{
    if(pEvent.keyCode == 13)
    {
        lnkConteudoSecundario_Cotacoes_BuscarCotacao_Click(null);

        return false;
    }
}


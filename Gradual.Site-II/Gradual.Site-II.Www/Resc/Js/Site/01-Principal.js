/// <reference path="00-Base.js" />

var gIntervaloKeyPressMapaDoSite;

var txtMapaDoSite_BuscaRapida;

var pnlCotacaoRapida, pnlDadosDeCotacaoRapida;

var gUltimoValorDoMapa = "";

var gLIsDoMapa;

var gTimeOutMenuIE;

var gTimeoutExcluirSociais;

var gTipoTeclado;

function Page_Load() {

    if (location.pathname.toLowerCase().indexOf('reservaipo') == -1) {
        PageLoad_Agencia();
    }

    Page_Load_CodeBehind();
}

function PaginaInterna_Load()
{
    //console.log("PaginaInterna_Load");

    $.datepicker.setDefaults( { regional: "pt-BR", duration: "" } );

    //ModuloCMS_Load();

    PageLoad_Agencia();
    
    //padronizando todos os pdf abrindo em outra janela:
    $("section.PaginaConteudo a[href$='.pdf']").attr("target", "_blank");
    
    var lHash = document.location.hash + "";
    var lSubHash = "";

    if(lHash != "")
    {
        if (lHash.toLowerCase().indexOf("#aba-") == 0 || lHash.toLowerCase().indexOf("#li_") == 0)
        {
            lHash = lHash.substr(1);
            
            if(lHash.toLowerCase().indexOf("-sub-") != -1)
            {
                lSubHash = lHash.substr(lHash.toLowerCase().indexOf("-sub-") + 5);

                lHash = lHash.substr(0, lHash.indexOf("-sub-"));
            }

            var lLink = $("#" + lHash);

            lLink.click();

            if(lSubHash != "")
            {
                lLink = $("#" + lSubHash);

                lLink.click();
            }
        }
    }


    $("ul[data-AbasSimples] li a").click(function()
    {
        var lLink = $(this);

        if(!lLink.parent().hasClass("ativo") && lLink.parent().attr("data-desabilitado") == null)
        {
            var lFechar = lLink.closest("ul").find("li.ativo a").attr("href");

            lLink.closest("ul").find("li.ativo").removeClass("ativo");

            lFechar = lFechar.substr(1);

            $("#ContentPlaceHolder1_pnl" + lFechar).hide();

            $("#ContentPlaceHolder1_pnl" + lLink.attr("href").substr(1)).show();

            lLink.parent().addClass("ativo");

            return false;
        }
    });

    $(".cadastro-formulario").bind("click", btnCadastroFormulario_Enviar_Click);

    // corrige os itens do menu principal conforme os atributos de um hidden chamado config-menu:

    var lConfigMenu = $("#config-menu");

    if(lConfigMenu.length > 0)
    {
        var lValor = lConfigMenu.attr("data-espacamento");

        if(lValor.indexOf("px") == -1)
        {
            lValor = lValor + "px";
        }

        $("#menu > ul.lista-menu > li").css( { paddingLeft: lValor, paddingRight: lValor  } );
    }
    
    //precisa verificar se tem algum formulário cadastrado por HTML do CMS
    //para validar só ali, não na página inteira:

    if($(".cadastro-formulario").length > 0)
    {
        GradSite_AtivarInputs($(".cadastro-formulario").closest("div.form-padrao"));
    }
    else
    {
        GradSite_AtivarInputs("section.PaginaConteudo");
    }

    //se tiver o user control de carrinho, ativa nele também:
    if($("#pnlCarrinhoOverlay").length > 0)
    {
        GradSite_AtivarInputs("#Carrinho1_pnlCarrinho .SubForm_Dados");
        GradSite_AtivarInputs("#Carrinho1_pnlCarrinho .SubForm_Telefones");
        GradSite_AtivarInputs("#Carrinho1_pnlCarrinho .SubForm_Endereco");
        
    }

    //coloca os títulos nas "bolinhas" de tipo de fundos:
    $(".bolinha-vermelho").attr("title", "Arrojado");
    $(".bolinha-amarelo").attr("title", "Moderado");
    $(".bolinha-verde").attr("title", "Conservador");

    //dentro do CMS, itens com o atributo data-ElementoPai serão removidos de seu lugar e colocados dentro do elemento indicado:

    var lElementosPraMover = $("[data-ElementoPai]");

    var lElemento, lPai;

    for(var a = 0; a < lElementosPraMover.length; a++)
    {
        lElemento = $(lElementosPraMover[a]);

        lPai = $("#" + lElemento.attr("data-ElementoPai"));

        if(lElemento[0].tagName.toLowerCase() == "li")
        {
            lElemento = lElemento.closest("ul");
        }

        if(lPai.length > 0 && lElemento.parent().attr("id") != lPai.attr("id"))
        {
            lElemento.attr("data-RemoverAoEditarHtml", "true");

            lPai.append(lElemento);
        }
    }

    Page_Load_CodeBehind();

    //dentro do CMS, itens com o atributo data-FiltradaPor irão preenchar uma combo com opções para filtragem:
    // esta função tem que vir após o page_lod_codebehind porque a inicialização do cms remove o eventhandler das combos.

    var lElementrosQueFiltram = $("[data-FiltradaPor]");

    var lComboFiltro = null;
    var lValorFiltro = "";

    for(var a = 0; a < lElementrosQueFiltram.length; a++)
    {
        if(lComboFiltro == null || lComboFiltro.attr("id") != lElementrosQueFiltram[a].getAttribute("data-FiltradaPor"))
        {
            lComboFiltro = $("#" + lElementrosQueFiltram[a].getAttribute("data-FiltradaPor"));
        }

        lValorFiltro = lElementrosQueFiltram[a].getAttribute(lElementrosQueFiltram[a].getAttribute("data-FiltrarPropriedade"));

        if(lComboFiltro.find("option[value='" + lValorFiltro + "']").length == 0)
        {
            lComboFiltro.append("<option value='" + lValorFiltro + "'>" + lValorFiltro + "</option>");

            lComboFiltro.attr("data-RemoverFilhosAoEditarHtml", "true");

            lComboFiltro.off("change").on("change", function()
            {
                //tem que dar o "off" primeiro pra não dar bind mais de uma vez.

                var lCombo = $(this);
                var lLI = $("[data-FiltradaPor='" + lCombo.attr("id") + "']:eq(0)");
                var lUL = lLI.closest("ul");

                lUL.find(">li").show();

                if(lCombo.val() != "")
                {
                    var lAtributo = lLI.attr("data-FiltrarPropriedade");

                    lUL.find("li[" + lAtributo + "!='" + lCombo.val() + "']").hide();
                }
            });
        }
    }

    $("[data-HabilitarNoLoad='true']").attr("disabled", null);

    //analytics para links de pdf:
    
    $("a[href$='pdf'").on("click", function() {
        
        var lArquivo = $(this).attr("href");

        lArquivo = lArquivo.substr(lArquivo.lastIndexOf("/") + 1);

        ga('send', 'event', 'link', 'click', lArquivo);
    });
    
}


function GradSite_SociaisCarregados()
{
    window.clearTimeout(gTimeoutExcluirSociais);
}


var gSlider;

var gTimeoutExcluirSociais;

function PageLoad_Agencia()
{
    //Transformando Checkbox & Radio
    //$('input[type="checkbox"], input[type="radio"]').iCheck();

     gTimeoutExcluirSociais = window.setTimeout(function(){ $("#redes-sociais").find("li").html("&nbsp;") }, 5000);  //cinco segundos pra carregar as redes sociais, se não apaga.

    //área de banners:
    gSlider = $("#banner-area").unslider({keys: false, dots: true, fluid: true, delay: 8000});

    $("#banner-area .banner-preload").removeClass("banner-preload");

    //verifica se tem vídeo nos banners pra iniciar os vídeos:

    var lBannerVideos = $("#banner-area").find(".video-content");

    if(lBannerVideos.length > 0)
    {
        lBannerVideos.jPlayer( {     swfPath: "resc/js/lib/"
                                  , supplied: "m4v,flv"
                                  , solution: "flash,html" 
                                  , ready: function() 
                                           {
                                              var lPlayer = $(this);
                                              var lURL = lPlayer.closest("div.video-banner").attr("data-url");
                                              
                                              var lOpcoes = lPlayer.closest("div.video-banner").attr("data-opcoes").split(",");

                                              var lMudo = false;

                                              try
                                              {
                                                  for(var a = 0; a < lOpcoes.length; a++)
                                                  {
                                                        //por enquanto só "iniciarmudo", podem haver outras opções aqui futuramente
                                                      var lOpcao = lOpcoes[a].split(":");

                                                      if(lOpcao[0].toLowerCase().trim() == "iniciarmudo")
                                                      {
                                                          lMudo = (lOpcao[1].toLowerCase().trim() == "sim");
                                                      }
                                                  }
                                              }
                                              catch(erro_config){}

                                              lPlayer.jPlayer("option", "muted", lMudo);

                                              lPlayer.jPlayer("setMedia", { flv: lURL });

                                              lPlayer.jPlayer("play");
                                           } 
                               });
    }

    if( $("#hidRaiz").val().indexOf("localhost") != -1)  { gSlider.stop(); }


    // precisa adicionar esse click handler no menu principal
    // para quando clicar em uma ancora que esteja dentro da mesma página atual, ele também clique pra
    //selecionar a aba, porque não solta evento "click" no browser quando vai pra um link de ancora...

    $("#menu").find("a[href*='#']").click( lnkMenuPrincipal_Click );

    $("#mapa-site").find("a[href*='#']").click( lnkMenuPrincipal_Click );

    $("ul.menu-tabs li a").click( lnkAba_Conteudo_Click );
    
    $("ul.abas-menu li a").click( lnkAba_Conteudo_Click );

    // verifica se tem abas que foram copiadas direto do html da produtora pra dentro de um "ConteudoHTML",
    // daí ela não tem "data-idConteudo" porque não foi feita no cms... 

    var lAbas = $(".abas-menu, .menu-tabs");
    var lItem;

    lAbas.each(function()
    {
        lItem = $(this).find(" > ul > li:eq(0)");   //verifica só pelo primeiro li

        if(lItem.attr("data-IdConteudo") === undefined)
        {
            //o li deste item não tem data-idconteudo, então foi "colado" via html

            var lDIVs = lItem.parent().next("div").find(" > div");

            lDIVs.hide();   //esconde todos os divs de conteudo relacionados às das abas 

            lDIVs.filter(":eq(0)").show(); //mostra o primeiro, padrão...

            var lLIs = lItem.parent().find(">li");

            var lSeed = Date.now();

            for(var a = 0; a < lDIVs.length; a++)
            {
                 lLIs[a].setAttribute("data-IdConteudo", (lSeed + a));
                lDIVs[a].setAttribute("data-IdConteudo", (lSeed + a));
            }

            lLIs.click( lnkAba_Conteudo_Click );    //não precisa ter o "a" dentro do "li" necessariamente
        }
    });

    //Para imagens subidas pelo upload com raiz '/' funcionarem no localhost:

    if($("#hidRaiz").val().toLowerCase().indexOf("localhost") != -1)
    {
        $("img[src^='/']").each(function()
        {
            $(this).attr("src", ($("#hidRaiz").val() + $(this).attr("src")));
        });
    }

    //Exportar & Minha Conta
    $('.exportar').hover(
        function () 
        {
            //$(this).find(">input, >ul").toggleClass('ativo').show();
            $(this).find("ul").toggleClass('ativo');
        },
        function () 
        {
            $(this).find("ul").delay(1000).toggleClass('ativo');
            //$(this).find('ul').stop(true, true).fadeOut(50);
            
        }
    );
    /*
    $(document).bind('click', function(e) 
    {
        if($(e.target).closest('.exportar').length === 0)
        {
            if( $('.menu-exportar li ul').is(':visible') )
            {
                $('.exportar button').removeClass('ativo');
                $('.menu-exportar li ul').removeClass('ativo');
            };
        }
    });
    */

    
    //Acordeon

    if( $(".acordeon, .acordeon-box").size() >= 1 )
    {
        var lAcordeons = $(".acordeon-opcao");
        var lItem;

        lAcordeons.each(function(){

            lItem = $(this);

            if(lItem.attr("onclick") === undefined || lItem.attr("onclick") == "")
            {
                if(lItem.parent().attr("onclick") === undefined || lItem.parent().attr("onclick") == "")
                {
                    lItem.bind("click", lnkAcordeon_Conteudo_Click); //alterando pra usar a nossa função, não dar conflito
                }
            }
        });
    }

    //Acordeon - ONE FOR ALL
    $('.one-for-all').click(function() {
        //var allAcord = $('.acordeon > ')
        
        if( !$('.acordeon-conteudo:visible').length == 0 ) {
            $('.acordeon-conteudo').slideUp('100');
            $('.acordeon li').not('#abas-menu li').removeClass('ativo');
            $(this).removeClass('ativo');
        } else {
            $('.acordeon-conteudo').slideDown('100');
            $('.acordeon li').not('#abas-menu li').addClass('ativo');
            $(this).addClass('ativo');
        }
        
        return false;
    });

    //Submenu
    /*
    $('#menu > ul > li, #ucMenuPrincipal1_pnlMinhaconta').hover(function() {
        $(this).find('ul').stop(true, true).fadeIn(300);
        $('.minha-conta ul').removeClass('ativo');
        $('.minha-conta button').removeClass('ativo');
    },
    function() {
        $(this).find('ul').stop(true, true).fadeOut(50);
    });*/

    
    $('#menu > ul > li').hover(function() {
        $(this).find('div').stop(true, true).fadeIn(300);
    },
    function() {
        $(this).find('div').stop(true, true).fadeOut(50);
    });

    /*
    $('#ucMenuPrincipal1_pnlMinhaconta > button').hover(function() {
    
        console.log("não fecha!");

        window.clearTimeout(gMenuTimeOut);

        $('#ucMenuPrincipal1_pnlMinhaconta > div').stop(true, true).fadeIn(300);

        //$('.minha-conta ul').removeClass('ativo');
        //$('.minha-conta button').removeClass('ativo');
    },
    function() {
        gMenuTimeOut = window.setTimeout(function()
        {
            console.log("vai fechar");
            $('#ucMenuPrincipal1_pnlMinhaconta > div').stop(true, true).fadeOut(50);
        }, 500);
    });

    $("#ucMenuPrincipal1_pnlMinhaconta").find(">div, li").hover(function()
    {
        console.log("não fecha!");

        $("#ucMenuPrincipal1_pnlMinhaconta").find(">div").show();
        window.clearTimeout(gMenuTimeOut);
    });
    */

    $('#menu > ul > li > a').click(function() {
        return false;
    });

    if($.datepicker)
    {
        //Date Picker
        $('.calendario').datepicker({
            dateFormat: "dd/mm/yy",
            monthNames: ['Janeiro','Fevereiro','Mar&ccedil;o','Abril','Maio','Junho','Julho','Agosto','Setembro','Outubro','Novembro','Dezembro'],
            monthNamesShort: ['Jan','Fev','Mar','Abr','Mai','Jun', 'Jul','Ago','Set','Out','Nov','Dez'],
            dayNames: [ "Domingo","Segunda","Terça","Quarta","Quinta","Sexta","Sábado"],
            dayNamesShort: ['Dom','Seg','Ter','Qua','Qui','Sex','Sab'],
            dayNamesMin: [ "Dom","Seg","Ter","Qua","Qui","Sex","Sab"]
        });
    }

    GradSite_CorrigirLinksParaSubAbas();

    GradSite_AtivarInputs("#header");

}

var gMenuTimeOut;

function GradSite_CorrigirLinksParaSubAbas()
{
    //procura por links de sub-aba para alterar o link para a função de clique que entenda Aba-Tal-sub-Aba-Outra:

    $("a[href*='-sub-']").click(function()
    {
        var lLink = $(this).attr("href");

        var lURL = document.location.pathname;

        var lLinkSemHash = lLink.substr(0, lLink.indexOf("#"));

        if(lURL != "/" && lURL.toLowerCase() != "/gradual.site-ii.www/" && lLinkSemHash.toLowerCase().indexOf(lURL.toLowerCase()) != -1)
        {
            //está na mesma página, precisamos clicar nas abas:

            lLink = lLink.substr(lLink.indexOf("#") + 1);
            
            var lAnchor1 = $("a[id='" + lLink.split("-sub-")[0] + "']");
            var lAnchor2 = $("a[id='" + lLink.split("-sub-")[1] + "']");

            lAnchor1.click();

            lAnchor2.click();

            lAnchor2.focus();

            return false;
        }

        //está indo pra outra página, deixar ir...
    });
}

function GradSite_HabilitarZoomParaImagens()
{
    var lImagens = $(".ContainerDeZoom img");

    lImagens.each(function()
                  {
                    GradSite_HabilitarZoomParaImagem($(this));
                  });
}

function GradSite_HabilitarZoomParaImagem(pImagem)
{
    if(!pImagem.parent().hasClass("ContainerDeZoom"))
    {
        var lLink = $("<a href='#' class='ContainerDeZoom'></a>");

        pImagem.before(lLink);
        
        lLink.append(pImagem);

        lLink.append("<span class='IconeDeZoom'></span>");
    }

    pImagem.parent().bind("click", imgComZoom_Click);
}

function GradSite_DesabilitarZoomParaImagem(pImagem)
{
    var lLink = pImagem.parent();
    
    if(lLink.hasClass("ContainerDeZoom"))
    {
        var lClone = pImagem.clone();

        lLink.after( lClone );

        lLink.remove();
    }
}

function GradSite_CorrigirLinksAtendimentoOnline()
{
    var lLinks = $("a[href*='http://chat.gradualinvestimentos.com.br']");
    var lLink;

    lLinks.each(function()
    {
        lLink = $(this);

        lLink.attr("href", "#").bind("click", function() { GradSite_AbrirAtendimentoOnline(); return false; });
    });
}

function GradSite_AbrirAtendimentoOnline()
{
    try
    {
        _gaq.push(["_trackEvent", "Acessar", "Chat"]);
    }
    catch(erro){}

    window.open("http://chat.gradualinvestimentos.com.br/default.aspx?chat=1&group=Origem&url=" + window.location, "chat","width=498,height=348,resizable=no,toolbar=0,location=0,directories=0,status=no,menubar=0,scrollbars=0")
}

function SiteGradual_FocarProximoInput(pSender)
{
    var lSender = $(pSender);

    if (lSender.attr("maxlength") == lSender.val().length)
    {
        if (lSender.next().length == 0)
        {
            var lNext = lSender.closest("p").next();

            if (lNext[0].tagName.toLowerCase() != "p")
                lNext = lNext.next();

            if (lNext.find("input"))
                lNext.find("input").focus();

            else if (lNext.find("select"))
                lNext.find("select").focus();
        }
        else if (lSender.next().is(":visible") && (lSender.next().is("input")|| lSender.next().is("select")))
        {
            lSender.next().focus();
        }
        else
        {
            lSender.next().next().focus();
        }
        
    }

    return false;
}


function GradSite_PrepararParaIE()
{
    // ai ai ai...

    for(var a = 1; a <= 4; a++)
        $("nav.LinksNavegacao a:eq(" + (a - 1) + ")").addClass("f" + a);

    $("ul.LinksConteudos > li")
        .on("mouseenter", function()
                          {
                               $(this).parent().find("> li.Hover").removeClass("Hover");

                               $(this).addClass("Hover");
                          })
        .on("mouseleave", function()
                          {
                               gTimeOutMenuIE = window.setTimeout(function() { $("ul.LinksConteudos > li.Hover").removeClass("Hover") }, 200);
                          });
                       
    $("ul.LinksConteudos > li > ul > li > a")
        .on("mouseenter", function()
                          {
                              window.clearTimeout(gTimeOutMenuIE);
                              
                              $(this).parent().parent().find("li.Hover").removeClass("Hover");

                              $(this).parent().addClass("Hover");
                          });

    $("#pnlConteudo > section").addClass("f");

    $("#pnlConteudo > p").addClass("f");

    $("#pnlConteudo > section > table, #pnlConteudo > table")
        .wrap("<div style='text-align:center' />");
        
    //$("#pnlConteudo > table").wrap("<div style='text-align:center;background:red' />");

    $("#pnlConteudo > p:nth-child(2)").addClass("f2");

    $("fieldset.FormADireita input[type='text']").addClass("TipoTexto");

    $("#pnlLogin p input[type='text']").addClass("TipoTexto");
    $("#pnlLogin p input[type='passowrd']").addClass("TipoSenha");
    $("#pnlLogin p input[type='submit']").addClass("TipoSubmit");

    $("div.PainelLoginInline p input[type='text']").addClass("TipoTexto");
    $("div.PainelLoginInline p input[type='password']").addClass("TipoSenha");

    $("fieldset.FormularioPadrao p        input[type='text']").addClass("TipoTexto");
    $("fieldset.FormularioPadrao p        input[type='checkbox']").addClass("TipoCheck");
    $("fieldset.FormularioPadrao p        input[type='password']").addClass("TipoSenha");
    $("fieldset.FormularioPadrao table td input[type='text']").addClass("TipoTexto");
    $("fieldset.FormularioPadrao table td input[type='checkbox']").addClass("TipoCheck");

    $("fieldset.FormularioPadrao p.RadiosSimNao label[for]").addClass("TemAtributoFor");

    $("fieldset.FormularioPadrao p.RadiosSimNao input[type='radio']").addClass("TipoRadio");

    /*
    if($.browser.version.indexOf("10") != 0)
    {
        $("label[for='txtCadastro_PFPasso1_Senha'], label[for='txtCadastro_PFPasso1_AssEletronica']").css( { width: 269 } );
    }
    */

    //arrumando os labels do cadastro:
    $("#ContentPlaceHolder1_pnlSenhas label").css( { width : "auto" } );
    
    $("#ContentPlaceHolder1_pnlSenhas label.ComAjuda").css( {width: 160 } );

    window.setTimeout(function()
    {
        $("img.keyboardInputInitiator")
            .css( { marginTop: 6, marginLeft: 4, float: "left" } );

    }, 500);
}


function GradSite_BuscarCotacaoRapida(pAtivo)
{
    if(!pnlDadosDeCotacaoRapida.is(":visible"))
    {
        pnlDadosDeCotacaoRapida
            .css( { width: 0, display: "block" } )
            .addClass("Carregando")
            .animate( { width: 30 }, 300, "easeOutExpo");

    }
    else
    {
        if(pnlDadosDeCotacaoRapida.hasClass("Retraido"))
        {
            pnlDadosDeCotacaoRapida
                .removeClass("Retraido")
                .addClass("Carregando")
                .animate( { width: 30 }, 300, "easeOutExpo");
        }
        else
        {
            pnlDadosDeCotacaoRapida
                .addClass("Carregando")
                .animate( { width: 30 }, 200, "easeOutExpo");
        }
    }

    window.setTimeout(function()
    {
        GradSite_CarregarJsonVerificandoErro(     GradSite_BuscarRaiz("/Async/Geral.aspx")
                                                , { Acao: "BuscarCotacaoRapida", Ativo: pAtivo }
                                                , GradSite_BuscarCotacaoRapida_CallBack);
    }, 1000);
}



function GradSite_BuscarCotacaoRapida_CallBack(pResposta)
{
    if(pResposta.ObjetoDeRetorno != null)
    {
        var lDados = pResposta.ObjetoDeRetorno;

        $("#lblCotacaoRapida_Ativo").html(      lDados.jCN );
        $("#lblCotacaoRapida_Maxima").html(     NumConv.StrToPretty( lDados.jXD , 2) );
        $("#lblCotacaoRapida_Minima").html(     NumConv.StrToPretty( lDados.jND , 2) );
        $("#lblCotacaoRapida_Abertura").html(   NumConv.StrToPretty( lDados.jVA , 2) );
        $("#lblCotacaoRapida_Fechamento").html( NumConv.StrToPretty( lDados.jVF , 2) );
        $("#lblCotacaoRapida_Ultima").html(     NumConv.StrToPretty( lDados.jPC , 2) );

        pnlDadosDeCotacaoRapida
            .removeClass("Carregando")
            .animate( { width: 590 }, 400, "easeOutExpo");
    }
    else
    {
        GradSite_ExibirMensagem("E", pResposta.Mensagem, false, null);
    }
}

function GradSite_RetrairCotacaoRapida()
{
    if(!pnlDadosDeCotacaoRapida.hasClass("Retraido"))
    {
        pnlDadosDeCotacaoRapida
            .addClass("Retraido")
            .animate( { width: 6 }, 300, "easeOutExpo");
    }
    else
    {
        pnlDadosDeCotacaoRapida
            .removeClass("Retraido")
            .animate( { width: 590 }, 300, "easeOutExpo");
    }
}


function GradSite_Formulario_ExibirMensagemDeValidacao(pIdDoControle, pMensagem, pRealizarScroll)
{
    var lCampo = $("#" + pIdDoControle);

    var lParent = lCampo.parent();

    lCampo.addClass("CampoComErro");

    lParent.find(".formError").remove();

    lParent.append("<div class='formError " + pIdDoControle + "formError' style='opacity: 1;'><div class='formErrorContent'>" + pMensagem + "<br></div></div>");

    if(pRealizarScroll)
        $("html:not(:animated),body:not(:animated)").animate({ scrollTop: $(".formError:not('.greenPopup'):first").offset().top}, 1100);
}

function GradSite_VerificarPositivoseNegativos(pItensSelecionados)
{
    var lItem, lValor;

    $(pItensSelecionados).each(function()
    {
        lItem = $(this);

        lValor = lItem.text().replace("R$ ", "");

        lItem.removeClass("ValorNegativo_Vermelho").removeClass("ValorPositivo_Azul");

        if(NumConv.StrToNum(lValor) < 0)
        {
            lItem.addClass("ValorNegativo_Vermelho");
        }

        if(NumConv.StrToNum(lValor) > 0)
        {
            lItem.addClass("ValorPositivo_Azul");
        }
    });
}

function GradSite_CarregarHtmlDaAba(pDiv)
{
    pDiv
    .show()
    .load( GradSite_BuscarRaiz("/Async/ModuloCMS.aspx?Acao=BuscarHtmlParaAba&IdConteudo=" + pDiv.attr("data-IdConteudo") )
                , function()
                  {
                      $(this).removeClass("FaltaCarregar") ;
                  }
             );
}


/*      Event Handlers     */


function btnGradSite_Login_EfetuarLogin(pSender, pEvent)
{
    pEvent.preventDefault();

    var lDados = {     
                      Acao  : "EfetuarLogin"
                    , Login : $("#txtGradSite_Login_Usuario").val()
                    , Senha : $("#txtGradSite_Login_Senha").val()
                 };

    if (!lDados.Login || lDados.Login == "")
    {
        //talvez esteja no inline (tela de login)
        lDados.Login = $("#txtGradSite_Login_Usuario_InLine").val();


        if ($("#txtGradSite_Login_Senha_InLine").length > 0)
        {
            lDados.Senha = $("#txtGradSite_Login_Senha_InLine").val();

        }
        else
        {
            lDados.Senha = $("#txtGradSite_Login_Senha").val();
        }
    }

    // Carrega a senha do teclado dinâmico
    if (arrayRef.length > 0) 
    {
        // Cria o objeto que irá receber as opções dos caracteres digitados
        var SenhaInfo = { Caractere1: [], Caractere2: [], Caractere3: [], Caractere4: [], Caractere5: [], Caractere6: [] };

        // Varre a matriz alimentando o objeto de transporte da senha
        for (i = 0; i < arrayRef.length; i++) 
        {
            // Cria o objeto que transportará as opções digitadas para o caractere
            var OpcaoCaractereInfo = { Opcao1: arrayRef[i][0], Opcao2: arrayRef[i][1] };
            
            // Valida a posição do caractere e associa a propriedade correspondente do objeto de transporte
            if (i == 0) // caractere2
            {
                SenhaInfo.Caractere1 = OpcaoCaractereInfo;
            }
            else if (i == 1) // caractere2
            {
                SenhaInfo.Caractere2 = OpcaoCaractereInfo;
            }
            else if (i == 2) // caractere3
            {
                SenhaInfo.Caractere3 = OpcaoCaractereInfo;
            }
            else if (i == 3) // caractere4
            {
                SenhaInfo.Caractere4 = OpcaoCaractereInfo;
            }
            else if (i == 4) // caractere5
            {
                SenhaInfo.Caractere5 = OpcaoCaractereInfo;
            }
            else if (i == 5) // caractere6
            {
                SenhaInfo.Caractere6 = OpcaoCaractereInfo;
            }
        }

        lDados.Senha = SenhaInfo;
    }

    if(lDados.Login == "" || lDados.Login === undefined)
    {
        GradSite_ExibirMensagem("I", "Favor preencher a conta");
        $("#lblNovoCadastro_MsgLogin").text("Favor preencher a conta");

        return false;
    }
    
    if(lDados.Senha == "" || lDados.Senha === undefined)
    {
        GradSite_ExibirMensagem("I", "Favor peencher a senha");
        $("#lblNovoCadastro_MsgLogin").text("Favor preencher a senha");
    }
    else
    {
        $("div.header-login").addClass("header-login-loading");
        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Login.aspx"), lDados, GradSite_Login_EfetuarLogin_CallBack);
    }

    return false;
}

function GradSite_Login_EfetuarLogin_CallBack(pResposta)
{
    arrayRef        = [];
    ref             = [];
    refNova         = [];
    refConfirmacao  = [];
    controlClick    = 0;
    $(field).val("");

    var lMsg = pResposta.Mensagem;

    var lRestaurar = true;

    $("#txtGradSite_Login_Senha").val("");

    if(lMsg.indexOf("URL:") == 0)
    {
        $("#ucMenuPrincipal1_lnkAbraSuaConta").hide();

        $("#ucMenuPrincipal1_pnlMinhaconta").show();

        lMsg = lMsg.substr(4);

        if(lMsg == "")
        {
            if(document.location.href.toLowerCase().indexOf("/login.aspx") != -1)
            {
                //fez login pela página de login, redireciona pro minha conta:

                document.location = GradSite_BuscarRaiz("/MinhaConta/Financeiro/SaldosELimites.aspx");
            }
            else
            {
                //refresh current page

                GradSite_RecarregarComAncora()
            }
        }
        else
        {
            document.location = GradSite_BuscarRaiz(lMsg);
        }

        lRestaurar = false;
    }
    else if(lMsg.indexOf("SENHA:") == 0)
    {
        lMsg = lMsg.substr(6);

        GradSite_ExibirMensagem("A", lMsg);
        $("#lblNovoCadastro_MsgLogin").text(lMsg);
    }
    else
    {
        GradSite_ExibirMensagem("A", lMsg);
        $("#lblNovoCadastro_MsgLogin").text(lMsg);
    }

    if(lRestaurar)
        $("div.header-login").removeClass("header-login-loading");
}

var gTimeoutLogout;

function lnkGradSite_MinhaConta_Logout_Click(pSender)
{
    $("div.header-login").addClass("header-login-loading");

    gTimeoutLogout = window.setTimeout(function() { document.location = GradSite_BuscarRaiz(""); }, 3500 );

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Login.aspx"), { Acao: "EfetuarLogout" }, GradSite_Login_EfetuarLogout_CallBack);

    return false;
}

function GradSite_Login_EfetuarLogout_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        window.clearTimeout(gTimeoutLogout);

        var lURL = document.location.href;

        if(lURL.indexOf("#") != -1)
            lURL = lURL.substr(0, lURL.indexOf("#"));

        document.location = lURL;
    }
    else
    {
        $("div.header-login").removeClass("header-login-loading");
        
        GradSite_ExibirMensagem("A", pResposta.Mensagem);
    }
}


function btnGradSite_Login_EsqueciSenha_Click(pSender)
{
    document.location = GradSite_BuscarRaiz("/MinhaConta/Cadastro/EsqueciSenha.aspx");

    return false;
}

function btnGradSite_Login_EsqueciAssinatura_Click(pSender) {
    document.location = GradSite_BuscarRaiz("/MinhaConta/Cadastro/EsqueciAssinatura.aspx");

    return false;
}


function imgComZoom_Click()
{
    alert( $(this).find("img").attr("src") );

    return false;
}

function lnkAtendimentoOnline_Click(pSender)
{
    GradSite_AbrirAtendimentoOnline();

    return false;
}


function lnkMapaDoSite_Click(pSender)
{
    var pnlMapaDoSite = $("#pnlMapaDoSite");

    pnlMapaDoSite.css( { left: ($(".LinksSociais").position().left + 26) } );

    if(!pnlMapaDoSite.is(":visible"))
    {
        gLIsDoMapa.css("style", "");

        pnlMapaDoSite
            .css( { height: 300 } )
            .show()
            .animate(     { height: 880 }
                        , { duration: 500, easing: "easeOutBounce"} );

        txtMapaDoSite_BuscaRapida.focus();
    }
    else
    {
        window.clearInterval(gIntervaloKeyPressMapaDoSite);

        pnlMapaDoSite
            .animate(     { height: 0 }
                        , 300
                        , "easeOutExpo"
                        , function() { $(this).hide(); } );
    }

    return false;
}


function lnkBusca_Click(pSender)
{
    pSender = $(pSender);

    pSender.parent().find(".Selecionado").removeClass("Selecionado");
    $("#pnlLogin").hide();

    var pnlBusca = $("#pnlBusca");

    pnlBusca.css( { left: (pSender.parent().position().left - 24) } );

    if(!pnlBusca.is(":visible"))
    {
        pnlBusca.show();
        pSender.addClass("Selecionado");

        $("#txtBusca_Termo").focus().select();
    }
    else
    {
        pnlBusca.hide();
        pSender.removeClass("Selecionado");
    }

    return false;
}

function lnkLogin_Click(pSender)
{
    pSender = $(pSender);

    pSender.parent().find(".Selecionado").removeClass("Selecionado");
    $("#pnlBusca").hide();

    var pnlLogin = $("#pnlLogin");

    pnlLogin.css( { left: (pSender.parent().position().left - 42) } );

    if(!pnlLogin.is(":visible"))
    {
        pnlLogin.show();
        pSender.addClass("Selecionado");

        pnlLogin.find("input[type='text']:eq(0)").focus().select();
    }
    else
    {
        pnlLogin.hide();
        pSender.removeClass("Selecionado").blur();
    }


    return false;
}

function txtMapaDoSite_BuscaRapida_Focus(pSender)
{
    window.clearInterval(gIntervaloKeyPressMapaDoSite);

    gIntervaloKeyPressMapaDoSite = window.setInterval(gIntervaloKeyPressMapaDoSite_Ellapsed, 100);
}

function txtMapaDoSite_BuscaRapida_KeyDown(pSender, pEvent)
{
    if (pEvent.keyCode == 13)
        return false;

    if(pEvent.keyCode == 27)
    {
        //"ESC"

        lnkMapaDoSite_Click(pSender);
    }
}

function gIntervaloKeyPressMapaDoSite_Ellapsed()
{
    if(txtMapaDoSite_BuscaRapida.val() == "")
    {
        gLIsDoMapa.attr("style", "");
    }
    else
    {
        if(txtMapaDoSite_BuscaRapida.val().toLowerCase() != gUltimoValorDoMapa)
        {
            gUltimoValorDoMapa = txtMapaDoSite_BuscaRapida.val().toLowerCase();

            var lTexto = "";

            for(var a = 0; a < gLIsDoMapa.length; a++)
            {
                lTexto = gLIsDoMapa[a].textContent.toLowerCase();

                if(lTexto.indexOf(gUltimoValorDoMapa) != -1)
                {
                    gLIsDoMapa[a].setAttribute("style", "text-decoration:underline;");
                }
                else
                {
                    gLIsDoMapa[a].setAttribute("style", "opacity:0.3");
                }
            }
        }
    }
}

function txtBusca_Termo_KeyDown(pSender, pEvent)
{
    if(pEvent.keyCode == 13)
    {
        btnBusca_Click(pSender);

        return false;
    }
}

function btnBusca_Click(pSender)
{
    var lTextBox = $(pSender).parent().find("input[type='text']");

    if(lTextBox.length == 0)
    {
        lTextBox = $(pSender).closest(".row").find("input[type='text']");
    }

    var lVal = lTextBox.val().trim();

    if(lVal != "")
    {
        document.location = GradSite_BuscarRaiz("/Busca.aspx?termo=" + lVal);
    }

    return false;
}

function btnCotacaoRapida_Buscar_Click(pSender)
{
    GradSite_BuscarCotacaoRapida( $("#txtCotacaoRapida_Ativo").val().toUpperCase() );

    return false;
}

function txtCotacaoRapida_Ativo_KeyDown(pEvent)
{
    if(pEvent.keyCode == 13)
    {
        btnCotacaoRapida_Buscar_Click(null);

        return false;
    }
}

function btnPedidoDeReserva_Click(pSender)
{
    pSender = $(pSender);

    var lCodigoUsuario = pSender.attr("data-CodigoPrincipal");
    var lCodigoISIN    = pSender.attr("data-CodigoISIN");

    var lURL = $("#ContentPlaceHolder1_hidIPO_URL").val();

    if(lURL != "")
    {
        if(lCodigoUsuario != "" && lCodigoISIN != "")
        {
            lURL = lURL + "?COD=" + lCodigoUsuario + "&ISIN=" + lCodigoISIN;

            window.open(lURL, "window", "width=540,height=540,scrollbars=yes");
        }
        else
        {
            GradSite_ExibirMensagem("E", "Erro de configuração de Usuário [" + lCodigoUsuario + "] / ISIN [" + lCodigoISIN + "]");
        }
    }
    else
    {
        GradSite_ExibirMensagem("E", "Erro de configuração da URL de IPO");
    }

    return false;
}

function btnPedidoDeReservaIPO_Click(pSender) {

    pSender = $(pSender);

    var lCodigoUsuario = pSender.attr("data-CodigoPrincipal");
    var lCodigoISIN    = pSender.attr("data-CodigoISIN");

    var lURL = $("#ContentPlaceHolder1_hidIPO_URL").val();

    if (lURL != "") 
    {
        if (lCodigoUsuario != "" && lCodigoISIN != "") 
        {
            lURL = lURL + "?COD=" + lCodigoUsuario + "&ISIN=" + lCodigoISIN;

            window.open(lURL, "window", "height = 850, width = 655,scrollbars=yes,top=1");
        }
        else 
        {
            GradSite_ExibirMensagem("E", "Erro de configuração de Usuário [" + lCodigoUsuario + "] / ISIN [" + lCodigoISIN + "]");
        }
    }
    else {
        GradSite_ExibirMensagem("E", "Erro de configuração da URL de IPO");
    }

    return false;
}

function pnlDadosDeCotacaoRapida_Click(pSender)
{
    GradSite_RetrairCotacaoRapida();

    return false;
}

function lnkPalestrasDetalhe_Click(pSender)
{
    pSender = $(pSender);

    pSender.closest("tr").next("tr").toggle();
}

function lnkCursosEPalestras_Inscreverse_Click(pSender, pIdDoEvento)
{
    pSender = $(pSender);

    pSender.closest("td").html("<span class='MensagemAguarde'>Favor Aguardar...</span>");

    var lDados = { Acao: "InscreverseNoEvento", Id: pIdDoEvento }

    GradSite_CarregarJsonVerificandoErro("Palestras.aspx", lDados, lnkCursosEPalestras_Inscreverse_CallBack);

    return false;
}

function lnkCursosEPalestras_Inscreverse_CallBack(pResposta)
{
    GradSite_ExibirMensagem("I", pResposta.Mensagem);

    $("span.MensagemAguarde").html("Inscrição realizada!");
}

function rdoConta_Selecionar_Click(pSender)
{
    pSender = $(pSender);

    var lConta = pSender.attr("for") + "";

    if(lConta == "" || lConta == "undefined")
        lConta = pSender.attr("id") + "";

    //pSender.closest("div").next("input[type='hidden']").val(lConta);
    pSender.closest(".pnlRadios").find("input[type='hidden']").val(lConta);
}

function txtPassarParaProximo_KeyUp(pEvent)
{
    try
    {
        if(pEvent.target.value.length == pEvent.target.getAttribute("maxlength"))
        {
            var lInput = $(pEvent.target)
            var lNext = lInput.next("input");
            
            if(lNext.length == 0)
                lNext = lInput.next().next("input");    //caso tenha um label na frente

            if(lNext.length == 0)
                lNext = lInput.parent().next("p").find("input, select")
                
            lNext.focus();
        }
    }
    catch(erro){}
}

function lnkAcordeon_Conteudo_Click(pSender, pEvent)
{
    if(pSender.target)
    {
        pSender = $(pSender.target).closest("li");
    }
    else
    {
        pSender = $(pSender).closest("li");
    }


    var acordeonOpcao = pSender.find(" > div.acordeon-opcao");
    var acordeonConteudo = pSender.find(" > div.acordeon-conteudo");

    if(acordeonConteudo.is(":visible"))
    {
        acordeonConteudo.slideUp("50");
        acordeonOpcao.removeClass("ativo");
        $(pSender).removeClass("ativo");

        if(pEvent)
        {
            pEvent.cancelBubble = true;
        }
    }
    else 
    {
        pSender.parent().find(" > li.ativo").removeClass("ativo");
        pSender.parent().find(" li > div.acordeon-conteudo").slideUp("20");

        acordeonConteudo.slideDown("100");
        pSender.addClass("ativo");

        if(pEvent)
        {
            pEvent.cancelBubble = true;
        }
    } 
}

function lnkSejaCorrespondente_Click(pSender)
{
    var windowObjectReference;

    windowObjectReference = window.open(GradSite_BuscarRaiz("/Investimentos/Cambio.aspx?popup=true"), "Correspondente", "menubar=yes,location=yes,resizable=no,scrollbars=no,status=no,width=1000,height=250,centerscreen=yes");

    return false;
}

/*  funções prontas para poder usar no CMS direto via [link](NomeDessaFuncaoAqui{parametros,se,precisar}|NomeDaCssOuStyle)   */

function lnkAbrirOChat_Click(pSender)
{
    AbrirOChat();

    return false;
}

function AbrirOChat()
{
    window.open("http://chat.gradualinvestimentos.com.br/default.aspx?chat=1&group=Origem&url=" + window.location
                ,"chat"
                ,"width=498,height=348,resizable=no,toolbar=0,location=0,directories=0,status=no,menubar=0,scrollbars=0"
               );

    return false;
}

function AbrirOHB()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");

    try
    {
        //_gaq.push(["_trackEvent", "Acessar", "HomeBroker"]);

         Sauron_EnviarScreenView("HomeBroker");
    }
    catch(erro){}

    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarSessaoParaHB" }
                                         , function (pResposta) {

                                             var lMensagem = pResposta.Mensagem;

                                             if (lMensagem == "SEM_ACESSO") {
                                                 GradSite_ExibirMensagem("I", "Sem acesso ao Home Broker, favor entrar em contato com a central de atendimento.");
                                             }
                                             else if (lMensagem == "FALTA_CADASTRO") {
                                                 GradSite_ExibirMensagem("I", "Para utilizar o Home Broker, favor completar seu cadastro.");
                                             }
                                             else if (lMensagem == "SOLTECH") {
                                                 var lToken = pResposta.ObjetoDeRetorno.Token;
                                                 var lAlterarAssinatura = pResposta.ObjetoDeRetorno.AlterarAssinatura;
                                                 $("#Token").val(lToken);
                                                 if (lAlterarAssinatura == true) {
                                                     GradSite_ExibirMensagem("I", "Para sua segurança, no primeiro acesso ao HomeBroker será necessário alterar a sua Assinatura Eletrônica!");
                                                     window.setInterval(function () {
                                                         var lPainel = $("#pnlMensagem");

                                                         if (lPainel.is(":hidden")) {
                                                             document.location = GradSite_BuscarRaiz("/MinhaConta/cadastro/Seguranca.aspx");
                                                         }
                                                     }, 1000);

                                                 }
                                                 else {
                                                     var lURL = "https://hb.gradualinvestimentos.com.br/hbnet2/IntegracaoGradual/IntegracaoPortalHB.aspx";
                                                     AbrirHB('POST', lURL, { 'Host': 'hb.gradualinvestimentos.com.br', 'TokenType': 'Integration2', 'Token': $("#Token").val() }, 'newwin');
                                                 }
                                             }
                                             else {
                                                 var lURL = "https://homebroker.gradualinvestimentos.com.br";

                                                 if ($("#hidRaiz").val().indexOf("localhost") != -1) {
                                                     lURL = "http://localhost/Gradual.HomeBroker-II.Www";
                                                 }

                                                 window.open(lURL + "/Default.aspx?guid=" + lMensagem
                                                 , "HomeBroker"
                                                 , "width=1280,height=1024,toolbar=no, location=no,directories=no,status=yes,menubar=no,scrollbars=no,copyhistory=yes, resizable=yes");
                                             }
                                         }
                                         );


    return false;
}



function lnkAba_Conteudo_Click(pEvent, pSender)
{

    if(pEvent.currentTarget)
    {
        pSender = $(pEvent.currentTarget);
    }
    else
    {
        pSender = $(pEvent);
    }

    var lLI = pSender;

    if(lLI[0].tagName.toLowerCase() == "a")
    {
        lLI = pSender.closest("li");
    }

    var lID = lLI.attr("data-IdConteudo");

    var lDiv = $("div[data-IdConteudo='" + lID + "']");

    var lTipo = lLI.attr("data-TipoLink");

    if(!lLI.hasClass("ativo") && lLI.attr("data-desabilitado") != "true" && !lLI.hasClass("inativo") )
    {
        if(lTipo == "Link")
        {
            var lLink = lLI.attr("data-URL");

            if(lLink === undefined || lLink == "")
            {
                lLink = lLI.find("a").attr("href");
            }
            else
            {
                GradSite_BuscarRaiz("/" + lLink);
            }

            document.location = lLink;
        }
        else
        {
            lLI.parent().find(".ativo").removeClass("ativo");

            lLI.addClass("ativo");
            
            if(lLI.closest("ul").next("div").find("> div[data-idconteudo]").length > 0)
            {
                //caso seja colado diretamente no html e tenha um container <div> pai de todas as abas
                lLI.closest("ul").next("div").find("> div[data-idconteudo]:visible").hide();
            }
            else
            {
                if(lLI.closest("ul").parent().find("> div[data-idconteudo]:visible").length > 0)
                {
                    //caso seja renderizado pelo cms e não tenha container para as abas
                    lLI.closest("ul").parent().find("> div[data-idconteudo]:visible").hide();

                    lLI.closest("ul").parent().find("> .abas-conteudo").find("> div[data-idconteudo]").hide()
                }
                else
                {
                    //caso onde o conteúdo [idconteudo] está completamente em outro lugar na página, como na 
                    //página de glossário.

                    if(lDiv.length > 0)
                    {
                        lDiv.parent().find(" > div[data-idconteudo]:visible").hide();

                        lDiv.show();
                    }
                }
            }

            if(lDiv.hasClass("FaltaCarregar"))
            {
                GradSite_CarregarHtmlDaAba(lDiv);
            }
            else
            {
                lDiv.show();
            }

            var lBotaoEdicao = lLI.parent().next("button");

            if(lBotaoEdicao.length > 0 && lBotaoEdicao.hasClass("btnVisitarPaginaConteudo"))
            {
                var lIndex = lLI.index() - 1;

                /*
                var lLeft = (lLI.width() + 24) / 2; // 24 is the padding left/right, adding just half to kinda centralize the button

                for(var a = lIndex; a >= 0; a--)
                {
                    lLeft += (lLI.parent().find(">li:eq(" + a + ")").width() + 24);
                }
                */

                lBotaoEdicao.css( { position: "absolute", left: (pEvent.clientX + 10), top: (pEvent.clientY + window.scrollY - 56)  } ); // 26 magic number porque o botão tem um -36px de margem
            }
        }
    }

    return false;
}

function lnkMenuPrincipal_Click(pSender, pEvent)
{
    pSender = $(pSender.target);

    var lLink = pSender.attr("href");

    var lLocation = document.location.href;

    var lLinkHash = lLink.substr(lLink.indexOf("#"));

    lLink = lLink.substr(0, lLink.indexOf("#"));

    if(lLocation.indexOf("#") != -1)
    {
        lLocation = lLocation.substr(0, lLocation.indexOf("#"));
    }

    if(lLocation.toLowerCase().indexOf(lLink.toLowerCase()) != -1)
    {
        //está na mesma página

        if(pSender.closest("ul").attr("id") != "mapa-site")
        {
            //só quando for o menu, não no rodape
            lLink = pSender.closest("div");

            if(lLink.attr("id") == "" || lLink.attr("id") === undefined )
            {
                lLink.hide(); //esconde o submenu pra ficar legal; id="menu" é o principal! 
            }
        }

        $(lLinkHash).click();

        return false;
    }
}


/* funções de cadastro genérico */

var gMensagemCadastroGenerico = null;

function btnCadastroFormulario_Enviar_Click(pSender)
{
    pSender.originalEvent.preventDefault();

    pSender = $(pSender.currentTarget);

    var lForm = pSender.closest(".form-padrao");

    var lItem, lLabel, lProp, lValue;

    var lObjeto = {};

    lForm.validationEngine(gValidationEngineDefaults);

    gMensagemCadastroGenerico = "Conteúdo salvo com sucesso.";

    if(lForm.validationEngine("validate"))
    {
        lForm.find("input, select, textarea").each(function()
        {
            lItem = $(this);

            if(lItem.attr("type") != "button" && lItem.attr("type") != "submit")
            {
                lValue = lItem.val();

                lLabel = lItem.closest("div").find("label");

                lProp = lItem.attr("id");

                if(lProp == "")
                {
                    lProp = lItem.attr("name");
                }

                if(lProp.indexOf("-") != -1)
                {
                    lProp = lProp.replace("-", "");
                }

                if(lProp != "")
                {
                    if(lProp.toLowerCase() == "mensagemdeenvio")
                    {
                        gMensagemCadastroGenerico = lValue;
                    }
                    else
                    {
                        eval(" lObjeto." + lProp + " = '" + lValue + "';");
                    }
                }
            }
        });

        lObjeto.Referencia = lForm.find(".referencia-formulario").val();

        if(lObjeto.Referencia === undefined)
        {
            lObjeto.Referencia = document.location.href;
        }

        if(lObjeto.Tag === undefined)
        {
            lObjeto.Tag = "";
        }

        lObjeto.ConteudoJson = $.toJSON( lObjeto );

        lObjeto.Acao = "SalvarFormularioGenerico";

        GradSite_CarregarJsonVerificandoErro( GradSite_BuscarRaiz("/Async/Geral.aspx"), lObjeto, CadastrarFormularioGenerico_CallBack );
    }
    else
    {
        lForm.find("input").blur();

        GradSite_ExibirMensagem("A", "Existem campos inválidos, favor verificar.");
    }

    return false;
}

function CadastrarFormularioGenerico_CallBack(pResposta)
{
    GradSite_ExibirMensagem("A", gMensagemCadastroGenerico);

    $(".validationEngineContainer").find("input[type!='submit'], select").val("");
}


function txtCEP_KeyUp(pSender, pEvent)
{
    try
    {
        if(pSender.value && pSender.value.length == 9 && (pSender.value.indexOf("_") == -1) && pSender.value != pSender.getAttribute("data-UltimaBuscaCEP"))
        {
            pSender.setAttribute("data-UltimaBuscaCEP", pSender.value);

            GradSite_Cadastro_VerificarCEP(pSender.getAttribute("data-CEPGroup"), pSender.value);
        }
    }
    catch(erro)
    {
        console.log(erro);
    }
}

function GradSite_Cadastro_VerificarCEP(pGrupo, pCEP)
{
    var lGrupo = $("[data-CEPGroup='" + pGrupo + "']");

    lGrupo.addClass("CarregandoCEP");

    //re-habilita a combo de estado just in case:
    $("select[data-cepgroup='" + pGrupo + "'][data-cepprop='uf_sigla']").attr("disabled", null);
    $("select[data-cepgroup='" + pGrupo + "'][data-cepprop='pais']").attr("disabled", null);

    var lDados = { Acao: "BuscarCEP", CEP: pCEP };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, GradSite_Cadastro_VerificarCEP_CallBack);
}

function GradSite_Cadastro_VerificarCEP_CallBack(pResposta)
{
    var lInputs = $(".CarregandoCEP");

    if(pResposta.Mensagem == "ok")
    {
        var lEndereco = pResposta.ObjetoDeRetorno;

        if(lEndereco.logradouro != "n/d")
        {
            var lCEPsRestritos = $("[data-CEPGroupStateRestriction='" + $(lInputs[0]).attr("data-CEPGroup") + "']");

            if(lCEPsRestritos.length > 0)
            {
                if(lCEPsRestritos.val().indexOf(lEndereco.uf_sigla) == -1)
                {
                    alert("Atenção! O endereço do CEP " + lEndereco.cep + " está no estado " + lEndereco.uf_sigla + ", que está na lista de estados restritos. Favor escolher outro.");
                    
                    lInputs.removeClass("CarregandoCEP");

                    return false;
                }
            }

            var lInput;

            lInputs.each(function()
            {
                lInput = $(this);

                var lProp = lInput.attr("data-CepProp");

                if(lProp == "logradouro")
                {
                    lInput.val(lEndereco.tp_logradouro + " " + lEndereco.logradouro);
                }
                else if(lProp == "pais")
                {
                    //a combo de país é fixa
                    lInput.val("BRA").attr("disabled", "disabled");    //não permite trocar estado para um incorreto
                }
                else
                {
                    eval("lInput.val(lEndereco." + lProp + ")");

                    if(lProp == "uf_sigla")
                    {
                        lInput.attr("disabled", "disabled");    //não permite trocar estado para um incorreto
                    }
                }
            });
        }
    }

    lInputs.removeClass("CarregandoCEP");
}

function pnlRelatorioDePerformance_Click(pSender)
{
    $(pSender).toggleClass("Fechado");
}

function cboPeriodo_Change(pSender)
{
    pSender = $(pSender);
    
    var lDataDe  = $("#ContentPlaceHolder1_txtDataInicial");
    var lDataAte = $("#ContentPlaceHolder1_txtDataFinal");

    if(pSender.val() != "")
    {
        lDataDe.attr("disabled", "disabled").attr("data-ValorAnterior", lDataDe.val()).val("");
        lDataAte.attr("disabled", "disabled").attr("data-ValorAnterior", lDataAte.val()).val("");
    }
    else
    {
        lDataDe.attr("disabled", null).val(lDataDe.attr("data-ValorAnterior"));
        lDataAte.attr("disabled", null).val(lDataAte.attr("data-ValorAnterior"));
    }
}

function Sauron_EnviarScreenView(pNomeDaTela)
{
    try
    {
        ga('send', 'screenview', {'screenName': pNomeDaTela});

    }catch(erro) { console.log("Erro do google analytics: " + erro); }
}

function AbrirHB(verb, url, data, target) 
{



    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", url);

        if (data) {
            for (var key in data) {
                var input = document.createElement("textarea");
                input.name = key;
                input.value = typeof data[key] === "object" ? JSON.stringify(data[key]) : data[key];
                form.appendChild(input);
            }
        }

    form.setAttribute("target", "HomeBroker");
    document.body.appendChild(form);
    window.open("test.html", "HomeBroker", "width=1280,height=1024,toolbar=no, location=no,directories=no,status=yes,menubar=no,scrollbars=no,copyhistory=yes, resizable=yes");

    form.submit();
}

var gVerificarTeclado_Caller;
var gVerificarTeclado_Param;

function VerificarTeclado(sender, param) 
{
    gVerificarTeclado_Caller = sender;
    gVerificarTeclado_Param = param;

    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    var lCodigoCliente = $("#txtGradSite_Login_Usuario").val()

    if (lCodigoCliente == "")
    {
        lCodigoCliente = $("#txtGradSite_Login_Usuario_InLine").val()
    }

    var lTeclado;

    //if ($(sender).attr("id").indexOf("Seguranca") <= -1 && $(sender).attr("id").indexOf("Retirada") <= -1 && $(sender).attr("id").indexOf("Resgate") <= -1 && $(sender).attr("id").indexOf("Aplicar") <= -1 && $(gVerificarTeclado_Caller).attr("id").indexOf("Cesta") <= -1)
    //{
    //    if (lCodigoCliente == "" || lCodigoCliente == undefined) 
    //    {
    //        GradSite_ExibirMensagem("I", "Favor preencher a conta");
    //        return false;
    //    }
    //}

    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: lCodigoCliente }
                                         , VerificarTeclado_CallBack);


}

function VerificarTeclado_CallBack(param) 
{
    //Força teclado dinamico
    param.ObjetoDeRetorno.Teclado = 1;

    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;
    
    // A chamada veio do painel de login
    if ($(gVerificarTeclado_Caller).attr("id").indexOf("Seguranca") <= -1 && $(gVerificarTeclado_Caller).attr("id").indexOf("Retirada") <= -1 && $(gVerificarTeclado_Caller).attr("id").indexOf("Resgate") <= -1 && $(gVerificarTeclado_Caller).attr("id").indexOf("Aplicar") <= -1 && $(gVerificarTeclado_Caller).attr("id").indexOf("Cesta") <= -1)
    {
        var lLogin = $('#txtGradSite_Login_Senha');
        var entryKeyboard = getEntryKeyBoard();
        var validationKeyboard = getValidationKeyBoard();

        switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
        {
            case 0: // O usuario utiliza o teclado virtual (QWERTY)
                {
                    // Acerta a visibilidade dos campos do teclado dinamico, mostra apenas o teclado virtual
                    entryKeyboard.hide();
                    validationKeyboard.hide();
                    break;
                }
            case 1: // O usuario ja foi migrado para o teclado dinamico
                {
                    // Remove o teclado virtual do campo de senha do login
                    var keyboard = $('#txtGradSite_Login_Senha').getkeyboard();
                    if (keyboard == undefined)
                    {
                        keyboard = $('#txtGradSite_Login_Senha_InLine').getkeyboard();
                    }
                    if (keyboard != null)
                    {
                        keyboard.destroy();
                    }
                    // Acerta a visibilidade dos campos do teclado dinamico, mostrando apenas o teclado dinamico
                    entryKeyboard.hide();
                    validationKeyboard.show();
                    lLogin.prop('readonly', true);
                    break;
                }
            case 2: // O usuario migrou apenas a senha para o teclado dinamico
                {
                    // Remove o teclado virtual (QWERTY) do campo de senha do login
                    var keyboard = $('#txtGradSite_Login_Senha').getkeyboard();
                    if (keyboard == undefined)
                    {
                        keyboard = $('#txtGradSite_Login_Senha_InLine').getkeyboard();
                    }
                    if (keyboard != null)
                    {
                        keyboard.destroy();
                    }

                    // Acerta a visibilidade dos campos do teclado dinamico, mostrando apenas o teclado dinamico
                    entryKeyboard.hide();
                    validationKeyboard.show();
                    lLogin.prop('readonly', true);
                    break;
                }
            case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
                {
                    entryKeyboard.hide();
                    validationKeyboard.hide();
                    break;
                }
        }
    }

    // A chamada veio da tela de seguranca
    if ($(gVerificarTeclado_Caller).attr("id").indexOf("Seguranca") >= 0 || $(gVerificarTeclado_Caller).attr("id").indexOf("Retirada") >= 0 || $(gVerificarTeclado_Caller).attr("id").indexOf("Resgate") >= 0 || $(gVerificarTeclado_Caller).attr("id").indexOf("Aplicar") >= 0 || $(gVerificarTeclado_Caller).attr("id").indexOf("Cesta") >= 0)
    {
        switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
        {
            case 0: // O usuario utiliza o teclado virtual (QWERTY)
                {
                    break;
                }
            case 1: // O usuario ja foi migrado para o teclado dinamico
                {
                    // Implementation not needed
                    break;
                }
            case 2: // O usuario migrou apenas a senha para o teclado dinamico
                {
                    // Implementation not needed
                    break;
                }
            case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
                {
                    // Implementation not needed
                    break;
                }
        }

        activeControl = $(gVerificarTeclado_Param.Controle);
        controlClick = 0;
        $('#KeyBoard_Header').text('Teclado Virtual (' + gVerificarTeclado_Param.Mensagem + ')').show();
        $('#KeyBoard_Description').text('Atenção: digite a sua ' + gVerificarTeclado_Param.Mensagem + ' no teclado abaixo.');

        // Exibe apenas o link 'esqueceu a senha'
        if (gVerificarTeclado_Param.Mensagem.indexOf('Senha') >= 0) 
        {
            $('#KeyBoard_ForgotPass').show();
            $('#KeyBoard_ForgotSign').hide();
        }
        
        // Exibe apenas o link 'esqueceu a assinatura'
        if (gVerificarTeclado_Param.Mensagem.indexOf('Assinatura') >= 0) 
        {
            $('#KeyBoard_ForgotPass').hide();
            $('#KeyBoard_ForgotSign').show();
        }

        // Pega o teclado dinamico de entrada de dados (sequencial de 0 a 9)
        var entryKeyboard = getEntryKeyBoard();
        // Pega o teclado dinamico de entrada de dados para validacao (5 teclas dinamicas)
        var validationKeyboard = getValidationKeyBoard();
        // Pega o recipiente da entrada do login
        var passwordContainer = getPasswordContainer();

        // Na tela de segurança deve ser exibido o teclado dinamico de entrada
        entryKeyboard.show();
        // Suprime o teclado de validação
        validationKeyboard.hide();
        // Suprime o recipiente do login
        passwordContainer.hide();

        $('#btn_confirm').hide();
    }

    if ($(gVerificarTeclado_Caller).attr("id").indexOf("Atual") <= 0)
    {
        $('#passwordPanel').modal('show');
    }
    else
    {
        if ($(gVerificarTeclado_Caller).attr("id").indexOf("SenhaAtual") >= 0 || $(gVerificarTeclado_Caller).attr("id").indexOf("AssinaturaAtual") >= 0)
        {
            
            if ($(gVerificarTeclado_Caller).attr("id").indexOf("SenhaAtual") >= 0 && lTeclado != 0 && lTeclado !=3)
            {
                $('#passwordPanel').modal('show');
            }

            if ($(gVerificarTeclado_Caller).attr("id").indexOf("AssinaturaAtual") >= 0 && lTeclado != 0 && lTeclado != 2)
            {
                $('#passwordPanel').modal('show');
            }

        }

    }
}

function Seguranca_VerificacaoTeclado()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: ""}
                                         , Seguranca_VerificarTeclado_CallBack);
}

function Seguranca_VerificarTeclado_CallBack(param)
{
    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;
    var lLogin = $('#txtGradSite_Login_Senha');
    var lSenhaAtual = $('#txtCadastro_PFPasso4_SenhaAtual');
    var lAssinaturaAtual = $('#txtCadastro_PFPasso4_AssinaturaAtual');

    switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
    {
        case 0: // O usuario utiliza o teclado virtual (QWERTY)
            {
                lSenhaAtual.prop('type', 'password');
                lAssinaturaAtual.prop('type', 'password');
                break;
            }
        case 1: // O usuario ja foi migrado para o teclado dinamico
            {
                lLogin.prop('readonly', true);
                lSenhaAtual.prop('readonly', true);
                lAssinaturaAtual.prop('readonly', true);

                lSenhaAtual.removeAttr('class');
                lSenhaAtual.addClass("teclado-dinamico");

                lAssinaturaAtual.removeAttr('class');
                lAssinaturaAtual.addClass("teclado-dinamico");
                break;
            }
        case 2: // O usuario migrou apenas a senha para o teclado dinamico
            {
                lLogin.prop('readonly', true);
                lSenhaAtual.prop('readonly', true);
                lSenhaAtual.removeAttr('class');
                lSenhaAtual.addClass("teclado-dinamico");
                lAssinaturaAtual.prop('type', 'password');
                break;
            }
        case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
            {
                lSenhaAtual.prop('type', 'password');
                lAssinaturaAtual.prop('readonly', true);
                lAssinaturaAtual.removeAttr('class');
                lAssinaturaAtual.addClass("teclado-dinamico");
                break;
            }
    }
}

function Aplicar_VerificacaoTeclado()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: "" }
                                         , Aplicar_VerificarTeclado_CallBack);
}

function Aplicar_VerificarTeclado_CallBack(param)
{
    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;

    var lAssinaturaDigital = $('#ContentPlaceHolder1_txtAssDigital');

    switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
    {
        case 0: // O usuario utiliza o teclado virtual (QWERTY)
            {
                break;
            }
        case 1: // O usuario ja foi migrado para o teclado dinamico
            {
                lAssinaturaDigital.prop('readonly', true);
                lAssinaturaDigital.removeAttr('class');
                break;
            }
        case 2: // O usuario migrou apenas a senha para o teclado dinamico
            {
                break;
            }
        case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
            {
                lAssinaturaAtual.prop('readonly', true);
                lAssinaturaAtual.removeAttr('class');
                break;
            }
    }
}

function Retiradas_VerificacaoTeclado()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: "" }
                                         , Retiradas_VerificarTeclado_CallBack);
}

function Retiradas_VerificarTeclado_CallBack(param)
{
    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;

    var lAssinaturaDigital = $('#ContentPlaceHolder1_txtAssDigital');

    switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
    {
        case 0: // O usuario utiliza o teclado virtual (QWERTY)
            {
                break;
            }
        case 1: // O usuario ja foi migrado para o teclado dinamico
            {
                lAssinaturaDigital.prop('readonly', true);
                lAssinaturaDigital.removeAttr('class');
                break;
            }
        case 2: // O usuario migrou apenas a senha para o teclado dinamico
            {
                break;
            }
        case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
            {
                lAssinaturaAtual.prop('readonly', true);
                lAssinaturaAtual.removeAttr('class');
                break;
            }
    }
}

function Resgate_VerificacaoTeclado()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: "" }
                                         , Resgate_VerificarTeclado_CallBack);
}

function Resgate_VerificarTeclado_CallBack(param)
{
    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;

    var lAssinaturaDigital = $('#ContentPlaceHolder1_txtAssDigital');

    switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
    {
        case 0: // O usuario utiliza o teclado virtual (QWERTY)
            {
                break;
            }
        case 1: // O usuario ja foi migrado para o teclado dinamico
            {
                lAssinaturaDigital.prop('readonly', true);
                lAssinaturaDigital.removeAttr('class');
                break;
            }
        case 2: // O usuario migrou apenas a senha para o teclado dinamico
            {
                break;
            }
        case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
            {
                lAssinaturaAtual.prop('readonly', true);
                lAssinaturaAtual.removeAttr('class');
                break;
            }
    }


}

function CompraTesouro_VerificacaoTeclado()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: "" }
                                         , Resgate_VerificarTeclado_CallBack);
}

function CompraTesouro_VerificarTeclado_CallBack(param)
{
    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;

    var lAssinaturaDigital = $('#ContentPlaceHolder1_CompraTD1_txtAssinaturaEletronica');

    switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
    {
        case 0: // O usuario utiliza o teclado virtual (QWERTY)
            {
                break;
            }
        case 1: // O usuario ja foi migrado para o teclado dinamico
            {
                lAssinaturaDigital.prop('readonly', true);
                lAssinaturaDigital.removeAttr('class');
                break;
            }
        case 2: // O usuario migrou apenas a senha para o teclado dinamico
            {
                break;
            }
        case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
            {
                lAssinaturaAtual.prop('readonly', true);
                lAssinaturaAtual.removeAttr('class');
                break;
            }
    }
}

function VendaTesouro_VerificacaoTeclado()
{
    var lURL = GradSite_BuscarRaiz("/Async/Geral.aspx");
    GradSite_CarregarJsonVerificandoErro(lURL
                                         , { Acao: "BuscarTipoTeclado", CodigoCliente: "" }
                                         , Resgate_VerificarTeclado_CallBack);
}

function VendaTesouro_VerificarTeclado_CallBack(param)
{
    lTeclado = param.ObjetoDeRetorno.Teclado;
    gTipoTeclado = lTeclado;

    var lAssinaturaDigital = $('#ContentPlaceHolder1_VendaTD1_txtAssinaturaEletronica');

    switch (lTeclado) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
    {
        case 0: // O usuario utiliza o teclado virtual (QWERTY)
            {
                break;
            }
        case 1: // O usuario ja foi migrado para o teclado dinamico
            {
                lAssinaturaDigital.prop('readonly', true);
                lAssinaturaDigital.removeAttr('class');
                break;
            }
        case 2: // O usuario migrou apenas a senha para o teclado dinamico
            {
                break;
            }
        case 3: // O usuario migrou apenas a assinatura para o teclado dinamico
            {
                lAssinaturaAtual.prop('readonly', true);
                lAssinaturaAtual.removeAttr('class');
                break;
            }
    }
}

function ValidarTamanhoCampo(sender, event)
{
    var size = $(sender).attr('maxlength');
    var KeyID = event.keyCode;

    if ($(sender).val().length >= size)
    {
        if (KeyID != "8" && KeyID != "46" && KeyID != "9")
        {
            event.preventDefault();
            GradSite_ExibirMensagem("I", "A quantidade máxima de caracteres permitidos é de " + size + " caracteres!");
        }
    }

}

function GradSiteLoginUsuario_KeyPress(sender, event)
{
    var keycode = (event.keyCode ? event.keyCode : event.which);
	if(keycode == '13')
    {
        showKeyboard($("#btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin"), event, '');
    }
}
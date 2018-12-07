/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />
/// <reference path="02-ModuloCMS-ConteudoDinamico.js" />
/// <reference path="02-ModuloCMS-Widgets.js" />

var   pnlTipoDeUsuario
    , pnlModuloCMS
        , pnlEstruturaContainer
            , lstEstruturaDaPagina
        , pnlConteudoDinamicoContainer
        , pnlPaginasContainer
            , pnlPaginasContainer_PainelEdicao
            , hidEdicaoPagina_IdDaPagina
            , txtEdicaoPagina_Titulo
            , txtEdicaoPagina_Url
            , cboEdicaoPagina_Modo
            , txtEdicaoWidget_Abas_NovaPagina_Titulo
            , txtEdicaoWidget_Abas_NovaPagina_Url 
            , cboEdicaoWidget_Abas_NovaPagina_Modo
        , pnlArquivosContainer
            , pnlArquivosContainer_lstImagens
            , pnlArquivosContainer_PreviewImagem
        , pnlInformacoesDaPaginaContainer
    , pnlEstruturaContainer_PainelEdicaoWidget
        , pnlEstruturaContainer_FormEdicaoWidget
            , lstEdicaoWidget_Abas_Itens
            , cboEdicaoWidget_Abas_NovoItem_Tipo
            , cboEdicaoWidget_Abas_AtributoClass
            , txtEdicaoWidget_Abas_AtributoStyle
            , txtEdicaoWidget_Abas_NovoItem_Texto
            , cboEdicaoWidget_Abas_NovoItem_Conteudo
            , lstEdicaoWidget_Acordeon_Itens
            , txtEdicaoWidget_Acordeon_NovoItem_Texto
            , cboEdicaoWidget_Acordeon_NovoItem_Conteudo
            , cboEdicaoWidget_Acordeon_AtributoClass
            , txtEdicaoWidget_Acordeon_AtributoStyle
            , txtEdicaoWidget_Acordeon_NovaPagina_Titulo
            , txtEdicaoWidget_Acordeon_NovaPagina_Url
            , cboEdicaoWidget_Acordeon_NovaPagina_Modo
            , cboEdicaoWidget_Titulo_Nivel
            , cboEdicaoWidget_Titulo_AtributoClass
            , txtEdicaoWidget_Titulo_AtributoStyle
            , txtEdicaoWidget_Titulo_Texto
            , txtEdicaoWidget_Texto_Texto
            , cboEdicaoWidget_Texto_AtributoClass
            , txtEdicaoWidget_Texto_AtributoStyle
            , txtEdicaoWidget_TextoHTML_ConteudoHTML
            , cboEdicaoWidget_TextoHTML_AtributoClass
            , txtEdicaoWidget_TextoHTML_AtributoStyle
            , txtEdicaoWidget_Imagem_AtributoSrc
            , txtEdicaoWidget_Imagem_AtributoAlt
            , txtEdicaoWidget_Imagem_LinkPara
            , rdoEdicaoWidget_Imagem_Tamanho_Automatico
            , rdoEdicaoWidget_Imagem_Tamanho_Fixo
            , txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura
            , txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura
            , chkEdicaoWidget_Imagem_HabilitarZoom
            , cboEdicaoWidget_Imagem_AtributoClass
            , txtEdicaoWidget_Imagem_AtributoStyle
            , cboEdicaoWidget_Lista_TipoDeLista
            , txtEdicaoWidget_Lista_ItensEstaticos
            , cboEdicaoWidget_Lista_TipoDeConteudo
            , cboEdicaoWidget_Lista_IdListaDinamica
            , cboEdicaoWidget_Lista_PropDisponiveis
            , cboEdicaoWidget_Lista_Ordenacao
            , txtEdicaoWidget_Lista_TemplateDoItem
            , txtEdicaoWidget_Lista_Atributos
            , cboEdicaoWidget_Lista_AtributoClass
            , txtEdicaoWidget_Lista_AtributoStyle
            , txtEdicaoWidget_Tabela_Cabecalho
            , cboEdicaoWidget_Tabela_TipoDeTabela
            , cboEdicaoWidget_Tabela_TipoDeConteudo
            , txtEdicaoWidget_Tabela_ItensEstaticos
            , cboEdicaoWidget_Tabela_IdListaDinamica
            , cboEdicaoWidget_Tabela_PropDisponiveis
            , cboEdicaoWidget_Tabela_Ordenacao
            , txtEdicaoWidget_Tabela_TemplateDaLinha
            , cboEdicaoWidget_Tabela_AtributoClass
            , txtEdicaoWidget_Tabela_AtributoStyle
            , cboEdicaoWidget_Repetidor_TipoDeConteudo
            , cboEdicaoWidget_Repetidor_IdListaDinamica
            , cboEdicaoWidget_Repetidor_PropDisponiveis
            , cboEdicaoWidget_Repetidor_Ordenacao
            , txtEdicaoWidget_Repetidor_TemplateDoItem
            , cboEdicaoWidget_Repetidor_AtributoClass
            , txtEdicaoWidget_Repetidor_AtributoStyle
            , cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo
            , cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica
            , cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis
            , cboEdicaoWidget_ListaDeDefinicao_Ordenacao
            , txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem
            , cboEdicaoWidget_ListaDeDefinicao_AtributoClass
            , txtEdicaoWidget_ListaDeDefinicao_AtributoStyle
            , txtEdicaoWidget_Embed_Codigo
            , txtEdicaoWidget_Embed_AtributoStyle
        , lstConteudoDinamico
            , cboCMS_ConteudoDinamico_SelecionarObjeto
        , pnlContainerCMS_Form_Itens
        , pnlContainerCMS_Form_Listas
            , txtEdicaoLista_Descricao
            , txtEdicaoLista_Regra
            , tblEdicaoLista_Regra_ResultadosDoTeste
        , pnlEdicaoDeHTML
            , txtEdicaoDeHTML
            , txtEdicaoDeHTML_InserirTabela_Cabecalho
            , txtEdicaoDeHTML_InserirTabela_Linhas
            , txtEdicaoDeHTML_InserirTabela_Colunas;

var gEstruturaDaPagina = null;

var gTimeOutApagarHoverEdicao = null;

var gTimeOutOcultarPainelDeImagens = null;

var CONST_WID_TIPO_ABAS      = "Abas";
var CONST_WID_TIPO_ACORDEON  = "Acordeon";
var CONST_WID_TIPO_IMAGEM    = "Imagem";
var CONST_WID_TIPO_LISTA     = "Lista";
var CONST_WID_TIPO_TEXTO     = "Texto";
var CONST_WID_TIPO_TEXTOHTML = "TextoHTML";
var CONST_WID_TIPO_TITULO    = "Titulo";
var CONST_WID_TIPO_TABELA    = "Tabela";
var CONST_WID_TIPO_REPETIDOR = "Repetidor";
var CONST_WID_TIPO_LISTA_DE_DEFINICAO = "ListaDeDefinicao";
var CONST_WID_TIPO_EMBED = "Embed";

var CONST_CLASS_HOVERELEMENTOEDICAO = "HoverElementoEdicao";

var gWidgetJsonAntesDeEditar, gWidgetSendoEditado, gElementoDoWidgetSendoEditado;

var gContDinamicoJsonAntesDeEditar, gContDinamicoSendoEditado;

var gDadosDaListaDinamicaSelecionada = null;

var gCampoImagemReferenciado = null;

var gCampoEdicaoHTMLReferenciado = null;

var gFlagEditandoConteudoDinamicoOuLista = null;

var gIDdoElementoAntesDeExcluir = null;


var gListaDeExtensoesDeImagem = [ "bmp", "gif", "jpg", "jpeg", "jpe", "png", "psd", "raw", "tga", "tif" ];

var gListaDeExtensoesDePDF = [ "pdf", "pdp" ];

var gListaDeExtensoesDeOffice = [ "doc", "docx", "ppt", "pptx", "xls", "xlsx" ];

var gListaDeExtensoesDeZip = [ "zip", "rar", "gzip", "7zip" ];


function ModuloCMS_Load()
{
    //console.log("ModuloCMS_Load() - Inicio");

    pnlTipoDeUsuario = $("#pnlTipoDeUsuario");

    pnlTipoDeUsuario.ExibeOculta =  function()
                                    {
                                        if(pnlTipoDeUsuario.is(":visible"))
                                        {
                                            //oculta

                                            pnlTipoDeUsuario.animate(   { width: 10, opacity: 0 }
                                                                      , {     duration: 140
                                                                            , complete: function() {pnlTipoDeUsuario.hide() }
                                                                        } 
                                                                    );
                                        }
                                        else
                                        {
                                            //exibe

                                            pnlTipoDeUsuario.css( { bottom: $("#pnlBotoesSecoes").height() - 34 } );    // magic number: 34 é a diferença entre a altura do pnlBotoes e o bottom que o painel precisa

                                            var lWidth = 190;

                                            if(pnlTipoDeUsuario.children().length == 1)
                                            {
                                                lWidth = 70;
                                            }

                                            pnlTipoDeUsuario
                                                .show()
                                                .animate(   { width: lWidth, opacity: 1 }
                                                          , { duration: 140 }
                                                        );
                                        }
                                    }

    pnlModuloCMS = $("#pnlModuloCMS");

    pnlModuloCMS.ExibeOculta =  function()
                                {
                                    if(!pnlModuloCMS.hasClass("Expandido"))
                                    {
                                        //exibe
                                        pnlModuloCMS.addClass("Expandido");
                                    }
                                    else
                                    {
                                        //oculta

                                        pnlTipoDeUsuario.hide();

                                        pnlModuloCMS.removeClass("Expandido");
                                    }
                                }

    pnlEstruturaContainer              = $("#pnlEstruturaContainer");
    pnlConteudoDinamicoContainer       = $("#pnlConteudoDinamicoContainer");
    pnlPaginasContainer                = $("#pnlPaginasContainer");
    pnlArquivosContainer               = $("#pnlArquivosContainer");
    pnlInformacoesDaPaginaContainer    = $("#pnlInformacoesDaPaginaContainer");
    pnlArquivosContainer_lstImagens    = $("#pnlArquivosContainer_lstImagens");
    pnlArquivosContainer_PreviewImagem = $("#pnlArquivosContainer_PreviewImagem");

    pnlPaginasContainer_PainelEdicao = $("#pnlPaginasContainer_PainelEdicao");

    lstEstruturaDaPagina = $("#lstEstruturaDaPagina");

    pnlEstruturaContainer_PainelEdicaoWidget = $("#pnlEstruturaContainer_PainelEdicaoWidget");
    pnlEstruturaContainer_FormEdicaoWidget   = $("#pnlEstruturaContainer_FormEdicaoWidget");

    lstEdicaoWidget_Abas_Itens               = $("#lstEdicaoWidget_Abas_Itens tbody");
    cboEdicaoWidget_Abas_NovoItem_Tipo       = $("#cboEdicaoWidget_Abas_NovoItem_Tipo");
    cboEdicaoWidget_Abas_AtributoClass       = $("#cboEdicaoWidget_Abas_AtributoClass");
    txtEdicaoWidget_Abas_AtributoStyle       = $("#txtEdicaoWidget_Abas_AtributoStyle");
    txtEdicaoWidget_Abas_NovoItem_Texto      = $("#txtEdicaoWidget_Abas_NovoItem_Texto");
    cboEdicaoWidget_Abas_NovoItem_Conteudo   = $("#cboEdicaoWidget_Abas_NovoItem_Conteudo");

    txtEdicaoWidget_Abas_NovaPagina_Titulo   = $("#txtEdicaoWidget_Abas_NovaPagina_Titulo");
    txtEdicaoWidget_Abas_NovaPagina_Url      = $("#txtEdicaoWidget_Abas_NovaPagina_Url");
    cboEdicaoWidget_Abas_NovaPagina_Modo     = $("#cboEdicaoWidget_Abas_NovaPagina_Modo");

    lstEdicaoWidget_Acordeon_Itens              = $("#lstEdicaoWidget_Acordeon_Itens tbody");
    txtEdicaoWidget_Acordeon_NovoItem_Texto     = $("#txtEdicaoWidget_Acordeon_NovoItem_Texto");
    cboEdicaoWidget_Acordeon_NovoItem_Conteudo  = $("#cboEdicaoWidget_Acordeon_NovoItem_Conteudo");
    cboEdicaoWidget_Acordeon_AtributoClass      = $("#cboEdicaoWidget_Acordeon_AtributoClass");
    txtEdicaoWidget_Acordeon_AtributoStyle      = $("#txtEdicaoWidget_Acordeon_AtributoStyle");
    txtEdicaoWidget_Acordeon_NovaPagina_Titulo  = $("#txtEdicaoWidget_Acordeon_NovaPagina_Titulo");
    txtEdicaoWidget_Acordeon_NovaPagina_Url     = $("#txtEdicaoWidget_Acordeon_NovaPagina_Url");
    cboEdicaoWidget_Acordeon_NovaPagina_Modo    = $("#cboEdicaoWidget_Acordeon_NovaPagina_Modo");

    cboEdicaoWidget_Titulo_Nivel             = $("#cboEdicaoWidget_Titulo_Nivel");
    cboEdicaoWidget_Titulo_AtributoClass     = $("#cboEdicaoWidget_Titulo_AtributoClass");
    txtEdicaoWidget_Titulo_AtributoStyle     = $("#txtEdicaoWidget_Titulo_AtributoStyle");
    txtEdicaoWidget_Titulo_Texto             = $("#txtEdicaoWidget_Titulo_Texto");

    txtEdicaoWidget_Texto_Texto              = $("#txtEdicaoWidget_Texto_Texto");
    cboEdicaoWidget_Texto_AtributoClass      = $("#cboEdicaoWidget_Texto_AtributoClass");
    txtEdicaoWidget_Texto_AtributoStyle      = $("#txtEdicaoWidget_Texto_AtributoStyle");

    txtEdicaoWidget_TextoHTML_ConteudoHTML   = $("#txtEdicaoWidget_TextoHTML_ConteudoHTML");
    cboEdicaoWidget_TextoHTML_AtributoClass  = $("#cboEdicaoWidget_TextoHTML_AtributoClass");
    txtEdicaoWidget_TextoHTML_AtributoStyle  = $("#txtEdicaoWidget_TextoHTML_AtributoStyle");

    txtEdicaoWidget_Imagem_AtributoSrc           = $("#txtEdicaoWidget_Imagem_AtributoSrc");
    txtEdicaoWidget_Imagem_AtributoAlt           = $("#txtEdicaoWidget_Imagem_AtributoAlt");
    txtEdicaoWidget_Imagem_LinkPara              = $("#txtEdicaoWidget_Imagem_LinkPara");
    rdoEdicaoWidget_Imagem_Tamanho_Automatico    = $("#rdoEdicaoWidget_Imagem_Tamanho_Automatico");
    rdoEdicaoWidget_Imagem_Tamanho_Fixo          = $("#rdoEdicaoWidget_Imagem_Tamanho_Fixo");
    txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura  = $("#txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura");
    txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura   = $("#txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura");
    chkEdicaoWidget_Imagem_HabilitarZoom         = $("#chkEdicaoWidget_Imagem_HabilitarZoom");
    cboEdicaoWidget_Imagem_AtributoClass         = $("#cboEdicaoWidget_Imagem_AtributoClass");
    txtEdicaoWidget_Imagem_AtributoStyle         = $("#txtEdicaoWidget_Imagem_AtributoStyle");

    cboEdicaoWidget_Lista_TipoDeLista            = $("#cboEdicaoWidget_Lista_TipoDeLista");
    txtEdicaoWidget_Lista_ItensEstaticos         = $("#txtEdicaoWidget_Lista_ItensEstaticos");
    cboEdicaoWidget_Lista_TipoDeConteudo         = $("#cboEdicaoWidget_Lista_TipoDeConteudo");
    cboEdicaoWidget_Lista_IdListaDinamica        = $("#cboEdicaoWidget_Lista_IdListaDinamica");
    cboEdicaoWidget_Lista_PropDisponiveis        = $("#cboEdicaoWidget_Lista_PropDisponiveis");
    cboEdicaoWidget_Lista_Ordenacao              = $("#cboEdicaoWidget_Lista_Ordenacao");
    txtEdicaoWidget_Lista_TemplateDoItem         = $("#txtEdicaoWidget_Lista_TemplateDoItem");
    txtEdicaoWidget_Lista_Atributos              = $("#txtEdicaoWidget_Lista_Atributos");
    cboEdicaoWidget_Lista_AtributoClass          = $("#cboEdicaoWidget_Lista_AtributoClass");
    txtEdicaoWidget_Lista_AtributoStyle          = $("#txtEdicaoWidget_Lista_AtributoStyle");

    txtEdicaoWidget_Tabela_Cabecalho             = $("#txtEdicaoWidget_Tabela_Cabecalho");
    cboEdicaoWidget_Tabela_TipoDeTabela          = $("#cboEdicaoWidget_Tabela_TipoDeTabela");
    txtEdicaoWidget_Tabela_ItensEstaticos        = $("#txtEdicaoWidget_Tabela_ItensEstaticos");
    cboEdicaoWidget_Tabela_TipoDeConteudo        = $("#cboEdicaoWidget_Tabela_TipoDeConteudo");
    cboEdicaoWidget_Tabela_IdListaDinamica       = $("#cboEdicaoWidget_Tabela_IdListaDinamica");
    cboEdicaoWidget_Tabela_PropDisponiveis       = $("#cboEdicaoWidget_Tabela_PropDisponiveis");
    cboEdicaoWidget_Tabela_Ordenacao             = $("#cboEdicaoWidget_Tabela_Ordenacao");
    txtEdicaoWidget_Tabela_TemplateDaLinha       = $("#txtEdicaoWidget_Tabela_TemplateDaLinha");
    cboEdicaoWidget_Tabela_AtributoClass         = $("#cboEdicaoWidget_Tabela_AtributoClass");
    txtEdicaoWidget_Tabela_AtributoStyle         = $("#txtEdicaoWidget_Tabela_AtributoStyle");

    cboEdicaoWidget_Repetidor_TipoDeConteudo     = $("#cboEdicaoWidget_Repetidor_TipoDeConteudo");
    cboEdicaoWidget_Repetidor_IdListaDinamica    = $("#cboEdicaoWidget_Repetidor_IdListaDinamica");
    cboEdicaoWidget_Repetidor_PropDisponiveis    = $("#cboEdicaoWidget_Repetidor_PropDisponiveis");
    cboEdicaoWidget_Repetidor_Ordenacao          = $("#cboEdicaoWidget_Repetidor_Ordenacao");
    txtEdicaoWidget_Repetidor_TemplateDoItem     = $("#txtEdicaoWidget_Repetidor_TemplateDoItem");
    cboEdicaoWidget_Repetidor_AtributoClass      = $("#cboEdicaoWidget_Repetidor_AtributoClass");
    txtEdicaoWidget_Repetidor_AtributoStyle      = $("#txtEdicaoWidget_Repetidor_AtributoStyle");

    cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo  = $("#cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo");
    cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica = $("#cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica");
    cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis = $("#cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis");
    cboEdicaoWidget_ListaDeDefinicao_Ordenacao       = $("#cboEdicaoWidget_ListaDeDefinicao_Ordenacao");
    txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem  = $("#txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem");
    cboEdicaoWidget_ListaDeDefinicao_AtributoClass   = $("#cboEdicaoWidget_ListaDeDefinicao_AtributoClass");
    txtEdicaoWidget_ListaDeDefinicao_AtributoStyle   = $("#txtEdicaoWidget_ListaDeDefinicao_AtributoStyle");

    txtEdicaoWidget_Embed_Codigo        = $("#txtEdicaoWidget_Embed_Codigo");
    txtEdicaoWidget_Embed_AtributoStyle = $("#txtEdicaoWidget_Embed_AtributoStyle");

    lstConteudoDinamico = $("#lstConteudoDinamico");
    cboCMS_ConteudoDinamico_SelecionarObjeto = $("#cboCMS_ConteudoDinamico_SelecionarObjeto");

    pnlContainerCMS_Form_Itens  = $("#pnlContainerCMS_Form_Itens");
    pnlContainerCMS_Form_Listas = $("#pnlContainerCMS_Form_Listas");

    txtEdicaoLista_Descricao               = $("#txtEdicaoLista_Descricao");
    txtEdicaoLista_Regra                   = $("#txtEdicaoLista_Regra");
    tblEdicaoLista_Regra_ResultadosDoTeste = $("#tblEdicaoLista_Regra_ResultadosDoTeste");

    pnlEdicaoDeHTML = $("#pnlEdicaoDeHTML");

    txtEdicaoDeHTML_InserirTabela_Cabecalho = $("#txtEdicaoDeHTML_InserirTabela_Cabecalho");
    txtEdicaoDeHTML_InserirTabela_Colunas = $("#txtEdicaoDeHTML_InserirTabela_Colunas");
    txtEdicaoDeHTML_InserirTabela_Linhas  = $("#txtEdicaoDeHTML_InserirTabela_Linhas");
    
    hidEdicaoPagina_IdDaPagina = $("#hidEdicaoPagina_IdDaPagina");
    txtEdicaoPagina_Titulo     = $("#txtEdicaoPagina_Titulo");
    txtEdicaoPagina_Url        = $("#txtEdicaoPagina_Url");
    cboEdicaoPagina_Modo       = $("#cboEdicaoPagina_Modo");
    

    $(".PickerDeArquivo")
        .bind("focus", iptPickerDeArquivo_Focus)
        .bind("blur", iptPickerDeArquivo_Blur);


    $("#txtGerenciadorDeArquivos_Upload")
    .fileupload({
                      dataType: "json"
                    , url:      GradSite_BuscarRaiz("/Async/ModuloCMS.aspx")
                    , done:     txtGerenciadorDeArquivos_Upload_CallBack
                });

    pnlModuloCMS.show();
    pnlEdicaoDeHTML.show();

    txtEdicaoDeHTML = $("#txtEdicaoDeHTML").htmlarea(); // Initialize jHtmlArea's with all default values

    ModuloCMS_AjustarAlturaDaEdicaoHTML();

    pnlModuloCMS.hide();
    pnlEdicaoDeHTML.hide();

    $("dl.ItensExpansiveis, dl.Acordeao")
        .find("dt")
            .bind("click", Widget_ListaDeDefinicao_DT_Click);

    $("dl.ItensExpansiveis, dl.Acordeao").find("dd").hide();
    

    try
    {
        ModuloCMS_CarregarDadosDaPaginaParaEdicao();

        try
        {
            ModuloCMS_CarregarArvoreDePaginas();

            var lIdPagina    = $("#hidIdDaPagina").val();
            var lIdEstrutura = $("#hidIdDaEstrutura").val();

            var lURL = document.location.pathname;

            if(lURL.toLowerCase().indexOf("/gradual.site-ii.www") == 0)
            {
                lURL = lURL.substr(20);
            }

            if($("#hidExibindoUmItem").val().toLowerCase() == "true")
            {
                alert("Esta página está exibindo somente um item (id=X na URL), CMS desabilitado.");
            }
            else
            {
                if(lIdPagina == "" || lIdPagina == "0")
                {
                    alert("Esta página está sem ID; favor cadastrar uma nova página para a URL \r\n\r\n" + lURL + "\r\n\r\nantes de utilizar o CMS");
                }
                else if(lIdEstrutura == "" || lIdEstrutura == "0")
                {
                    alert("Esta página está sem código de estrutura; favor cadastrar uma nova página para a URL \r\n\r\n" + lURL + "\r\n\r\nantes de utilizar o CMS");
                }
                else
                {
                    pnlModuloCMS.show();
                }
            }
        }
        catch(erro2)
        {
            alert("Erro de javascript ao iniciar CMS em ModuloCMS_CarregarArvoreDePaginas()\r\n" + erro2);
        }

        // coloca "title" nos elementos que têm IdConteudo pra facilitar a inspeção

        var lCont;

        $("[data-IdConteudo]").each(function()
        {
            lCont = $(this);

            if(lCont.attr("title") === undefined)
            {
                lCont.attr("title", "(" + lCont.attr("data-IdConteudo") + ") ");
            }
            else
            {
                lCont.attr("title", "(" + lCont.attr("data-IdConteudo") + ") " + lCont.attr("title"));
            }

        });
    }
    catch(erro)
    {
        alert("Erro de javascript ao iniciar CMS em ModuloCMS_CarregarDadosDaPaginaParaEdicao()\r\n" + erro);
    }
    
    //console.log("ModuloCMS_Load() - Fim");
}


function ModuloCMS_AjustarAlturaDaEdicaoHTML()
{
    //var lAltura = (document.documentElement.offsetHeight - 700);

    //if(lAltura > 650) lAltura = 650;

    var lAltura = pnlEdicaoDeHTML.css( {opacity:0} ).show().height();

    pnlEdicaoDeHTML.hide().css( {opacity:1} );

    lAltura = lAltura - 300;

    pnlEdicaoDeHTML.hide().find("iframe").css( { height: lAltura } );

    txtEdicaoDeHTML.css( { height: lAltura } );
}

function ModuloCMS_CarregarArvoreDePaginas()
{
    var lstPaginas = $("#lstPaginas");

    var lLIs = lstPaginas.find(">li[data-galho]");

    var lItem, lAdicionar, lFilhos;

    var lGalhos = $.parseJSON( $("#ModuloCMS_hidListaDePastas").val() );

    //inclui todos os diretórios:
    
    for(var a = 0; a < lGalhos.length; a++)
    {
        lstPaginas.append(
                            "<li class='GalhoPai' data-Galho='" + lGalhos[a].toLowerCase() + "'>" + 
                                "<label onclick='return liGalhoPai_Click(this)'>" + lGalhos[a] + "</label>" +
                                "<div> <button class='GalhoNovaPagina' onclick='return lnkGalhoPai_NovaPagina_Click(this)'></button> </div>" +
                                "<ul></ul>" +
                            "</li>"
                         );
    }

    // inclui os filhos dentro dos diretórios pai imediatos:

    for(var a = (lGalhos.length - 1); a >= 0; a--)
    {
        lFilhos = lstPaginas.find(">li[data-GalhoSub='" + lGalhos[a].toLowerCase() + "/']");

        if(lFilhos.length > 0)
        {
            lItem = lstPaginas.find("li.GalhoPai[data-Galho='" + lGalhos[a].toLowerCase() + "']");

            lItem.find(">ul").append(lFilhos);
        }
    }

    // roda todos os que são pai colocando dentro de seus pais, quando houver

    var lRever = true;
    var lGalhoPai;
    var lBreaker = 0;

    lstPaginas.find(">li.GalhoPai").attr("data-Processado", "false");

    try
    {
        while(lRever && lBreaker < 20000)
        {
            lRever = false;
            lBreaker++;

            lFilhos = lstPaginas.find(">li.GalhoPai[data-Processado]:last");

            lGalhoPai = lFilhos.attr("data-Galho");



            if(lGalhoPai.indexOf("/") != -1)
            {
                lGalhoPai = lGalhoPai.substr(0, lGalhoPai.lastIndexOf("/"));

                if(lFilhos.closest("li").attr("data-Galho") != lGalhoPai)
                {
                    lRever = true;

                    lstPaginas.find("li[data-Galho='" + lGalhoPai + "']").append(lFilhos);
                }
            }
            else
            {
                if(lFilhos.prev("li").length > 0)
                {
                    lFilhos.attr("data-Processado", null)

                    lRever = true;  //ainda não acabou, só terminou o primeiro galho pai com todos filhos e netos
                }
            }
        }
    }
    catch(erro_arvore)
    {
        console.log(erro_arvore);

        console.log( JSON.stringify( lFilhos ) );

        console.log( JSON.stringify( lGalhoPai ) );
    }

    //lstPaginas.find("li:not(.GalhoPai)").hide();
}

function liGalhoPai_Click(pSender)
{
    pSender = $(pSender).parent();

    var lGalhoSub = pSender.attr("data-GalhoSub");

    //var lFilhos = pSender.parent().find(">li[data-galhosub='" + lGalhoSub + "']:not(.GalhoPai)");

    if(!pSender.hasClass("GalhoAberto"))
    {
        //lFilhos.show();

        pSender.addClass("GalhoAberto");
    }
    else
    {
        //lFilhos.hide();

        pSender.removeClass("GalhoAberto");
    }

    return false;
}

function lnkGalhoPai_NovaPagina_Click(pSender)
{
    var lURL = $(pSender).closest("li").find(">label").text();

    if(lURL.charAt(lURL.length - 1) != "/")
    {
        lURL = lURL + "/";
    }

    $("#lblEdicaoPagina_Url").html(lURL);

    ModuloCMS_ConfigurarEdicaoDePaginaPeloDiretorio(true);

    btnCMS_Estrutura_AdicionarPagina_Click(null, true);

    var lLargura = (477 - $("#lblEdicaoPagina_Url").width());

    $("#txtEdicaoPagina_Url").attr("disabled", "disabled").css( { width: lLargura } );

    return false;
}

function txtEdicaoPagina_Titulo_KeyUp(pSender, pEvent)
{
    if(txtEdicaoPagina_Url.attr("disabled") != null)
    {
        txtEdicaoPagina_Url.val(ModuloCMS_ConverterTextoParaUrlAmigavel(txtEdicaoPagina_Titulo.val()));
    }
}

function ModuloCMS_BuscarDiretorioDaUrlAtual()
{
    // tendo um endereço como 'http://localhost/Gradual.Site-II.Www/Abas/Aprenda/Cursos.aspx' o diretório retorna '/Abas/Aprenda/'

    
    var lPaginaURL = document.location.pathname;

    if(lPaginaURL.toLowerCase().indexOf("/gradual.site-ii.www") == 0)
    {
        lPaginaURL = lPaginaURL.substr(21);
    }

    if(lPaginaURL.indexOf("/") == 0)        //precisa tirar a primeira barra caso não esteja no gradual.site-ii.wwww
        lPaginaURL = lPaginaURL.substr(1);

    if(lPaginaURL.toLowerCase().indexOf(".aspx") != -1)
    {
        lPaginaURL = lPaginaURL.substr(0, lPaginaURL.lastIndexOf(".aspx"));
    }

    if(!lPaginaURL.toLowerCase().indexOf("abas/") == 0)
        lPaginaURL = "Abas/" + lPaginaURL;

    if(lPaginaURL.lastIndexOf("/") != (lPaginaURL.length - 1))
    {
        lPaginaURL = lPaginaURL + "/";
    }

    return lPaginaURL;
}

function ModuloCMS_ConverterTextoParaUrlAmigavel(pString)
{
    return GradSite_ExtirparAcentuacao(pString.replace(/ /gi, ""))
}

function ModuloCMS_ProcessarTextoParaHTML(pTexto, pFlagConverterNewLinePraBR)
{
    //essa função converte o texto com as "tags" de bold, italico e link; relativa à função ProcessarTextoParaHTML dentro do WidgetBase

    
    //substitui os textos que estejam "codificados" pra incluir formatação simples:
    /*
    *italics*                               =>  italics
    **bold**                                =>  bold
    [texto](http://wwww.site.com)           =>  <a href="http://wwww.site.com">texto</a>
    [img](http://wwww.site.com/imagem.jpg)  =>  <img src="http://wwww.site.com/imagem.jpg" />
    [texto](AlgumaClasse)                   =>  <span class="AlgumaClasse">texto</span>
    */

    var lRegex, lMatch, lParte1, lParte2, lParte2_Split, lParte2_href, lParte2_class, lParte2_style, lParte2_onclick;

    //verifica se tem texto que seja link ou classe:

    lRegex = /\[(.*?)\]\((.*?)\)/gi;

    lMatch = lRegex.exec(pTexto);

    while(lMatch != null)
    {
        lParte1 = lMatch[1];
        lParte2 = lMatch[2];

        if(lParte1 == "img")
        {
            pTexto = pTexto.replace(lMatch[0], " -img- ");  //pra não dar loop enquanto não implementa
        }
        else
        {
            lParte2_href    = "";
            lParte2_target  = "";
            lParte2_class   = "";
            lParte2_style   = "";
            lParte2_onclick = "";

            lParte2_Split = lParte2.split("|");

            for(var a = 0; a < lParte2_Split.length; a++)
            {
                lParte2 = lParte2_Split[a];

                if (lParte2.indexOf("http:") != -1 || lParte2.indexOf("wwww") != -1 || lParte2.indexOf(".com") != -1 || lParte2.charAt(0) == "/" || lParte2.indexOf("..") == 0 || lParte2.indexOf(".aspx") != -1 || lParte2.indexOf(".htm") != -1)
                {
                    lParte2_href = lParte2;

                    if(lParte2_href.indexOf(":_blank") != -1)
                    {
                        lParte2_href   = lParte2_href.replace(":_blank", "");
                        lParte2_target = "_blank";
                    }
                }
                else if( lParte2.indexOf("js:") == 0 )
                {
                    var lFuncao = lParte2.substr(3).replace("{", "(").replace("}", ")");        //se precisar de parametros pra funcao, tem que usar {} ao inves de () porque o parêntese vai zuar o parse anterior

                    if(lFuncao.indexOf("(") == -1)  //os {} são opcionais... pode ficar só [texto do link]
                        lFuncao += "()";

                    lParte2_href    = "#";
                    lParte2_onclick = "return " + lFuncao;
                }
                else if((lParte2.indexOf(":") != -1) || (lParte2.indexOf(";") != -1))
                {
                    lParte2_style = lParte2;
                }
                else
                {
                    lParte2_class = lParte2;
                }
            }

            if (lParte2_href != "")
            {
                pTexto = pTexto.replace(lMatch[0], "<a href='" + lParte2_href + "'" +
                                                    ((lParte2_target  != "") ? "  target='" + lParte2_target   + "'" : "") +
                                                    ((lParte2_class   != "") ? "   class='" + lParte2_class    + "'" : "") +
                                                    ((lParte2_style   != "") ? "   style='" + lParte2_style    + "'" : "") +
                                                    ((lParte2_onclick != "") ? " onclick='" + lParte2_onclick  + "'" : "") +
                                                    ">" + lParte1 + "</a>");
            }
            else
            {
                pTexto = pTexto.replace(lMatch[0], "<span " + 
                                                    ((lParte2_class != "") ? " class='" + lParte2_class + "'" : "") +
                                                    ((lParte2_style != "") ? " style='" + lParte2_style + "'" : "") +
                                                    ">" + lParte1 + "</span>");
            }
        }

        lMatch = lRegex.exec(pTexto);
    }

    // verifica se tem bold:

    lRegex = /\*{2}(.*?)\*{2}/gi;
    
    lMatch = lRegex.exec(pTexto);

    while(lMatch != null)
    {
        pTexto = pTexto.replace(lMatch[0], "<span style='font-weight:bold'>" + lMatch[1] + "</span>");

        lMatch = lRegex.exec(pTexto);
    }

    // verifica se tem italico:

    lRegex = /\*(.*?)\*/gi;

    lMatch = lRegex.exec(pTexto);

    while(lMatch != null)
    {
        pTexto = pTexto.replace(lMatch[0], "<span style='font-style:italic'>" + lMatch[1] + "</span>");

        lMatch = lRegex.exec(pTexto);
    }

    if(pFlagConverterNewLinePraBR)
    {
        pTexto = pTexto.replace(/\n/gi, "<br />");
    }

    return pTexto;
}









/*    Event Handlers      */


function iptPickerDeArquivo_Focus()
{
    if(!pnlArquivosContainer.hasClass("DisplayComoPicker"))
    {
        gCampoImagemReferenciado = $(this);

        pnlArquivosContainer.addClass("DisplayComoPicker").show();
    }
}

function iptPickerDeArquivo_Blur()
{
    gTimeOutOcultarPainelDeImagens = window.setTimeout(function()
                                                       {
                                                           pnlArquivosContainer.removeClass("DisplayComoPicker").hide();
                                                       }, 400);

}

function cboGerenciadorDeArquivos_FiltroPorTipo_Focus(pSender)
{
    window.clearTimeout(gTimeOutOcultarPainelDeImagens);
}

function txtGerenciadorDeArquivos_Input_Focus(pSender)
{
    window.clearTimeout(gTimeOutOcultarPainelDeImagens);
}

function iptEdicaoDeHTML_Focus(pSender)
{
    pSender = $(pSender);

    gCampoEdicaoHTMLReferenciado = pSender.next("input[type='hidden']");

    txtEdicaoDeHTML.html(gCampoEdicaoHTMLReferenciado.val());

    txtEdicaoDeHTML.htmlarea("html", gCampoEdicaoHTMLReferenciado.val());

    var lBody = pnlEdicaoDeHTML.find("iframe").contents().find("body");

    lBody.html(gCampoEdicaoHTMLReferenciado.val());

    $("div.BarraDeFerramentas").css( { width: $("div.jHtmlArea").width() } );

    ModuloCMS_AjustarAlturaDaEdicaoHTML();

    pnlEdicaoDeHTML.show();

    pnlEdicaoDeHTML.find(".ToolBar ul li a.html").focus();

}


function btnEdicaoDeHTML_Salvar_Click(pSender)
{
    if(gWidgetSendoEditado != null && gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TEXTOHTML)
    {
        gElementoDoWidgetSendoEditado.html( txtEdicaoDeHTML.val() );
    }

    gCampoEdicaoHTMLReferenciado.val( txtEdicaoDeHTML.val() );

    gCampoEdicaoHTMLReferenciado.prev("input[type='text']").val( "<html>" );
    
    pnlArquivosContainer.removeClass("DisplayComoPickerEdicaoHTML").hide();

    pnlEdicaoDeHTML.hide();

    txtEdicaoDeHTML.htmlarea("html", "<br>");

    gCampoEdicaoHTMLReferenciado = null;


    return false;
}

function btnEdicaoDeHTML_Cancelar_Click(pSender)
{
    pnlEdicaoDeHTML.hide();

    txtEdicaoDeHTML.htmlarea("html", "<br>");

    pnlArquivosContainer.removeClass("DisplayComoPickerEdicaoHTML").hide();
    
    pnlEdicaoDeHTML.find("button.Selecionado").removeClass("Selecionado");

    gCampoEdicaoHTMLReferenciado = null;

    return false;
}

function lstImagens_LI_Click(pSender)
{
    if(gCampoImagemReferenciado != null)
    {
        pSender = $(pSender);

        var lDimensoes = { Largura: pSender.find("img").width(), Altura: pSender.find("img").height() };

        gCampoImagemReferenciado
            .val( pSender.find("input").val() )
            .attr("data-LarguraOriginal", lDimensoes.Largura)
            .attr("data-AlturaOriginal", lDimensoes.Altura);

        gCampoImagemReferenciado.closest("div.ContainerCMS_PainelComTabs_Form").find("input[data-ValorAnterior]").attr("data-ValorAnterior", null)

        pnlArquivosContainer.removeClass("DisplayComoPicker").hide();

        if(pnlEstruturaContainer.is(":visible"))
            Widget_Imagem_Elemento_Change(gCampoImagemReferenciado);

        //if(pnlConteudoDinamicoContainer.is(":visible")) 
        //    Widget_Imagem_Elemento_Change(gCampoImagemReferenciado);      aqui tem que atualizar os dados de edição do conteudo dinamico

        gCampoImagemReferenciado = null;
    }

    return false;
}

function lstImagens_LI_MouseOver(pSender)
{
    pSender = $(pSender);

    //var l

    pnlArquivosContainer_PreviewImagem
        .show()
        .find("img")
            .attr("src", pSender.find("img:eq(0)").attr("src"));
}

function btnImagens_Excluir_Click(pSender)
{
    if(confirm("Tem certeza que deseja excluir esse arquivo?\r\n\r\n Se algum objeto estiver utilizando a URL desse arquivo, ela ficará sem referência!"))
    {
        pSender = $(pSender);

        var lDados = {    Acao:          "ExcluirArquivo"
                        , NomeDiretorio: pSender.parent().attr("data-diretorio")
                        , NomeArquivo:   pSender.parent().attr("title")
                        , IdDaPagina:    gEstruturaDaPagina.IdDaPagina 
                     };

        GradSite_CarregarJsonVerificandoErro( GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, btnImagens_Excluir_Click_CallBack);
    }

    return false;
}

function btnImagens_Excluir_Click_CallBack(pResposta)
{
    pnlArquivosContainer_lstImagens.find("li[title='" +  pResposta.ObjetoDeRetorno + "']").remove();

    GradSite_ExibirMensagem("I", "Arquivo excluído com sucesso.");
}

function btnImagens_CopiarLink_Click(pSender)
{
    var lTexto = $(pSender).parent().find("input").val();

    window.prompt ("Atalho pra copiar o link: Ctrl+C, Enter", lTexto);

    return false;
}

function pnlArquivosContainer_PreviewImagem_Click(pSender)
{
    pnlArquivosContainer_PreviewImagem.hide();
}

function btnCMS_TipoDeUsuario_Click(pSender)
{
    pSender = $(pSender);

    if(pSender.hasClass("Tipo_Todos"))
    {
        alert("A página está configurada para ser o mesmo tipo para todos os usuários.");
    }
    else
    {
        pnlTipoDeUsuario.ExibeOculta();
    }

    return false;
}

function btnCMS_EstruturaDaPagina_Click(pSender)
{
    pSender = $(pSender);

    if(!pnlModuloCMS.hasClass("Expandido"))
    {
        pnlModuloCMS.ExibeOculta();
        pSender.addClass("Selecionado");

        pnlEstruturaContainer.show();
        pnlConteudoDinamicoContainer.hide();
        pnlPaginasContainer.hide();
        pnlArquivosContainer.hide();
        pnlInformacoesDaPaginaContainer.hide();
    }
    else
    {
        if(pnlEstruturaContainer.is(":visible"))
        {
            pnlModuloCMS.ExibeOculta();

            pSender.removeClass("Selecionado");
        }
        else
        {
            pSender.closest("ul").find("li button").removeClass("Selecionado");
            pSender.addClass("Selecionado");

            pnlEstruturaContainer.show();
            pnlConteudoDinamicoContainer.hide();
            pnlPaginasContainer.hide();
            pnlArquivosContainer.hide();
            pnlInformacoesDaPaginaContainer.hide();
        }
    }

    return false;
}

function btnCMS_Paginas_Click(pSender)
{
    pSender = $(pSender);

    if(!pnlModuloCMS.hasClass("Expandido"))
    {
        pnlModuloCMS.ExibeOculta();
        pSender.addClass("Selecionado");

        pnlEstruturaContainer.hide();
        pnlConteudoDinamicoContainer.hide();
        pnlPaginasContainer.show();
        pnlArquivosContainer.hide();
        pnlInformacoesDaPaginaContainer.hide();
    }
    else
    {
        if(pnlPaginasContainer.is(":visible"))
        {
            pnlModuloCMS.ExibeOculta();

            pSender.removeClass("Selecionado");
        }
        else
        {
            pSender.closest("ul").find("li button").removeClass("Selecionado");
            pSender.addClass("Selecionado");

            pnlEstruturaContainer.hide();
            pnlConteudoDinamicoContainer.hide();
            pnlPaginasContainer.show();
            pnlArquivosContainer.hide();
            pnlInformacoesDaPaginaContainer.hide();
        }
    }

    return false;
}

function btnCMS_ConteudoDinamico_Click(pSender)
{
    pSender = $(pSender);

    if(!pnlModuloCMS.hasClass("Expandido"))
    {
        pnlModuloCMS.ExibeOculta();
        pSender.addClass("Selecionado");

        pnlEstruturaContainer.hide();
        pnlConteudoDinamicoContainer.show();
        pnlPaginasContainer.hide();
        pnlArquivosContainer.hide();
        pnlInformacoesDaPaginaContainer.hide();
    }
    else
    {
        if(pnlConteudoDinamicoContainer.is(":visible"))
        {
            pnlModuloCMS.ExibeOculta();

            pSender.removeClass("Selecionado");
        }
        else
        {
            pSender.closest("ul").find("li button").removeClass("Selecionado");
            pSender.addClass("Selecionado");

            pnlEstruturaContainer.hide();
            pnlConteudoDinamicoContainer.show();
            pnlPaginasContainer.hide();
            pnlArquivosContainer.hide();
            pnlInformacoesDaPaginaContainer.hide();
        }
    }

    return false;
}

function btnCMS_Imagens_Click(pSender)
{
    pSender = $(pSender);

    if(!pnlModuloCMS.hasClass("Expandido"))
    {
        pnlModuloCMS.ExibeOculta();
        pSender.addClass("Selecionado");

        pnlEstruturaContainer.hide();
        pnlConteudoDinamicoContainer.hide();
        pnlPaginasContainer.hide();
        pnlInformacoesDaPaginaContainer.hide();
        pnlArquivosContainer.show();
    }
    else
    {
        if(pnlArquivosContainer.is(":visible"))
        {
            pnlModuloCMS.ExibeOculta();

            pSender.removeClass("Selecionado");
        }
        else
        {
            pSender.closest("ul").find("li button").removeClass("Selecionado");
            pSender.addClass("Selecionado");

            pnlEstruturaContainer.hide();
            pnlConteudoDinamicoContainer.hide();
            pnlPaginasContainer.hide();
            pnlInformacoesDaPaginaContainer.hide();
            pnlArquivosContainer.show();
        }
    }

    return false;
}

function btnCMS_InformacoesDaPagina_Click(pSender)
{
    pSender = $(pSender);

    if(!pnlModuloCMS.hasClass("Expandido"))
    {
        pnlModuloCMS.ExibeOculta();
        pSender.addClass("Selecionado");
        
        pnlEstruturaContainer.hide();
        pnlConteudoDinamicoContainer.hide();
        pnlPaginasContainer.hide();
        pnlArquivosContainer.hide();

        pnlInformacoesDaPaginaContainer.show();

        ModuloCMS_ExibirInformacoesDaPagina();
    }
    else
    {
        if(pnlInformacoesDaPaginaContainer.is(":visible"))
        {
            pnlModuloCMS.ExibeOculta();

            pSender.removeClass("Selecionado");
        }
        else
        {
            pSender.closest("ul").find("li button").removeClass("Selecionado");
            pSender.addClass("Selecionado");

            pnlEstruturaContainer.hide();
            pnlConteudoDinamicoContainer.hide();
            pnlPaginasContainer.hide();
            pnlArquivosContainer.hide();
            pnlInformacoesDaPaginaContainer.show();

            ModuloCMS_ExibirInformacoesDaPagina();
        }
    }


    return false;
}

function ModuloCMS_ExibirInformacoesDaPagina()
{
    $("#lblInfoPag_IdPagina").html( $("#hidIdDaPagina").val() );

    $("#lblInfoPag_Idestrutura").html( $("#hidIdDaEstrutura").val() );

    var lWidgets = $("[id^=wid]");

    var lValores = "";

    lWidgets.each(  function(){ lValores += this.getAttribute("id") + "<br/>"; } );

    $("#lblInfoPag_Widgets").html( lValores );
}

function ModuloCMS_RecarregarCache()
{
    var lURL = document.location.pathname;

    lURL += "?cache=0";

    document.location = lURL;

    return false;
}

function btnCMS_Estrutura_AdicionarWidget_Click(pSender)
{
    var lTipo = $("#cboCMS_Estrutura_AdicionarWidget").val();

    ModuloCMS_AdicionarNovoWidget(lTipo);

    return false;
}


function lstEstruturaDaPagina_LI_MouseOver(pSender)
{
    window.clearTimeout(gTimeOutApagarHoverEdicao);

    pSender = $(pSender);

    $("." + CONST_CLASS_HOVERELEMENTOEDICAO).removeClass(CONST_CLASS_HOVERELEMENTOEDICAO);

    $("#" + pSender.attr("data-IdElemento")).addClass(CONST_CLASS_HOVERELEMENTOEDICAO);

    gTimeOutApagarHoverEdicao = window.setTimeout(function() { $("." + CONST_CLASS_HOVERELEMENTOEDICAO).removeClass(CONST_CLASS_HOVERELEMENTOEDICAO) }, 1000);
}

function lstEstruturaDaPagina_LI_Click(pSender)
{
    $("." + CONST_CLASS_HOVERELEMENTOEDICAO).removeClass(CONST_CLASS_HOVERELEMENTOEDICAO);

    ModuloCMS_MontarFormDeEdicaoDoWidget($(pSender).parent());
}

function lstPaginas_LI_Click(pSender)
{
    ModuloCMS_MontarFormDeEdicaoDaPagina($(pSender).parent());
}

function lstPaginas_btnRemover_Click(pSender)
{
    ModuloCMS_ExcluirPagina($(pSender).closest("li"));

    return false;
}

function btnEstruturaDaPagina_Descer_Click(pSender)
{
    var lLI = $(pSender).closest("li");

    var lID = lLI.attr("data-IdDoWidget");

    var lFlagNovo = (lLI.attr("data-FlagNovoWidget").toLowerCase() == "true");

    if(lFlagNovo)
    {
        alert("Primeiro salve o Widget para poder mover");
    }
    else
    {
        ModuloCMS_MoverWidget(lID, -1);
    }

    return false;
}

function btnEstruturaDaPagina_Subir_Click(pSender)
{
    var lLI = $(pSender).closest("li");

    var lID = lLI.attr("data-IdDoWidget");

    var lFlagNovo = (lLI.attr("data-FlagNovoWidget").toLowerCase() == "true");

    if(lFlagNovo)
    {
        alert("Primeiro salve o Widget para poder mover");
    }
    else
    {
        ModuloCMS_MoverWidget(lID, 1);
    }

    return false;
}

function btnEstruturaDaPagina_Remover_Click(pSender)
{
    if(confirm("Tem certeza que deseja excluir esse widget?"))
    {
        var lLI = $(pSender).closest("li");

        var lID = lLI.attr("data-IdDoWidget");

        var lFlagNovo = (lLI.attr("data-FlagNovoWidget").toLowerCase() == "true");

        ModuloCMS_ExcluirWidget(lID, lFlagNovo);
    }

    return false;
}


function Widget_Titulo_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetTitulo();
}

function Widget_Texto_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetTexto();
}

function Widget_Imagem_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetImagem();
}

function Widget_Lista_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetLista();
}

function Widget_Tabela_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetTabela();
}

function Widget_Repetidor_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetRepetidor();
}

function Widget_ListaDeDefinicao_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetListaDeDefinicao();
}

function Widget_Embed_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetEmbed();
}

function Widget_Aba_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetAbas();
}

function Widget_Acordeon_Elemento_Change(pSender)
{
    ModuloCMS_AtualizarDadosDeEdicao_WidgetAcordeon();
}

function txtEdicaoWidget_KeyDown(pSender, pEvent)
{
    if(pEvent.keyCode == 13)
    {
        $(pSender).change();

        return false;
    }
}

function txtEdicaoConteudoDinamico_KeyDown(pSender, pEvent)
{
    if(pEvent.keyCode == 13)
    {
        return false;
    }
}

function txtEdicaoConteudoDinamicoNumerico_KeyDown(pSender, pEvent)
{

    var key = pEvent.keyCode;
    var val = pSender.value;
        
    //alert(key);
    //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
    if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
    {
        return true;
    }
    else 
    {
        if(navigator.userAgent.indexOf("MSIE") == -1)
        {
            pEvent.preventDefault();
        }

        return false;
    }
}

function btnEdicaoWidget_Abas_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Abas_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_Acordeon_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Acordeon_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_Titulo_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Titulo_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_Ajuda_Click(pSender)
{
    $(pSender).parent().next("div.ContainerCMS_PainelComTabs_PainelAjuda").toggle();

    return false;
}

function btnEdicaoWidget_Texto_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoPagina_Cancelar_Click(pSender)
{
    $("#lstPaginas").find(".Selecionado").removeClass("Selecionado");

    pnlPaginasContainer_PainelEdicao.hide();

    return false;
}

function btnEdicaoWidget_Texto_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_TextoHTML_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_TextoHTML_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_Imagem_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Imagem_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_Lista_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Lista_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}


function btnEdicaoWidget_Tabela_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Tabela_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}


function btnEdicaoWidget_Repetidor_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Repetidor_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_ListaDeDefinicao_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_ListaDeDefinicao_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}

function btnEdicaoWidget_Embed_Salvar_Click(pSender)
{
    ModuloCMS_SalvarWidgetSendoEditado();

    return false;
}

function btnEdicaoWidget_Embed_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeWidget();

    return false;
}


function btnEdicaoConteudoDinamico_Salvar_Click(pSender)
{
    if(gFlagEditandoConteudoDinamicoOuLista == "conteudo")
    {
        ModuloCMS_SalvarConteudoDinamico();
    }
    else if(gFlagEditandoConteudoDinamicoOuLista == "lista")
    {
        ModuloCMS_SalvarListaDinamica();
    }

    return false;
}

function btnEdicaoConteudoDinamico_Cancelar_Click(pSender)
{
    ModuloCMS_CancelarEdicaoDeConteudoDinamico();

    return false;
}



function btnGerenciadorDeArquivos_Filtrar_Click(pSender)
{
    var lFiltroTipo = $("#cboGerenciadorDeArquivos_FiltroPorTipo").val();

    var lFiltroDir = $("#cboGerenciadorDeArquivos_FiltroPorDiretorio").val();

    var lFiltro = $("#txtGerenciadorDeArquivos_Filtro").val().toUpperCase();

    $("#pnlArquivosContainer_lstImagens li").show();

    if(lFiltroTipo != "")
    {
        $("#pnlArquivosContainer_lstImagens li[data-Tipo!='" + lFiltroTipo + "']").hide();
    }

    if(lFiltroDir != "")
    {
        $("#pnlArquivosContainer_lstImagens li[data-Diretorio!='" + lFiltroDir + "']").hide();
    }

    if(lFiltro != "")
    {
        var lLI;

        $("#pnlArquivosContainer_lstImagens li").each(function()
        {
            lLI = $(this);

            if(lLI.find(">input").val().toUpperCase().indexOf(lFiltro) == -1)
            {
                lLI.hide();
            }
        });
    }

    return false;
}

function btnGerenciadorDeArquivos_LimparFiltro_Click(pSender)
{
    $("#pnlArquivosContainer_lstImagens li").show();

    return false;
}


function rdoEdicaoWidget_Imagem_Tamanho_Change(pSender)
{
    if(rdoEdicaoWidget_Imagem_Tamanho_Automatico.is(":checked"))
    {
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.attr("data-ValorAnterior", txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val());
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.attr("data-ValorAnterior",  txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val());
        chkEdicaoWidget_Imagem_HabilitarZoom.attr("data-ValorAnterior", chkEdicaoWidget_Imagem_HabilitarZoom.is(":checked"));

        txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val("").attr("disabled", "disabled");
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val("").attr("disabled",  "disabled");

        chkEdicaoWidget_Imagem_HabilitarZoom.attr("checked", false).attr("disabled",  "disabled");
    }
    else
    {
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.attr("disabled", null).val(txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.attr("data-ValorAnterior"));
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.attr("disabled",  null).val(txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.attr("data-ValorAnterior"));

        if(txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val() == "")
        {
            txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val(
                                                            txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura
                                                                .closest("div.ContainerCMS_PainelComTabs_Form")
                                                                    .find("input[data-LarguraOriginal]").attr("data-LarguraOriginal")
                                                            );
        }
        
        if(txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val() == "")
        {
            txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val(
                                                            txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura
                                                                .closest("div.ContainerCMS_PainelComTabs_Form")
                                                                    .find("input[data-AlturaOriginal]").attr("data-AlturaOriginal")
                                                            );
        }

        chkEdicaoWidget_Imagem_HabilitarZoom.attr("disabled", null);

        if(chkEdicaoWidget_Imagem_HabilitarZoom.attr("data-ValorAnterior") == "true")
            chkEdicaoWidget_Imagem_HabilitarZoom.attr("checked", true);
    }

    Widget_Imagem_Elemento_Change(pSender);
}



function btnCMS_ConteudoDinamico_CarregarItens_Click(pSender)
{
    $("#btnContainerCMS_ConteudoDinamico_Abas").html("Itens");

    gFlagEditandoConteudoDinamicoOuLista = "conteudo";

    pnlContainerCMS_Form_Itens.show();

    pnlContainerCMS_Form_Listas.hide();

    $(pSender).parent().find("button.IconeMais").attr("title", "Adicionar Novo Conteúdo").show();

    ModuloCMS_CarregarItensDoTipoSelecionado();

    return false;
}

function btnCMS_ConteudoDinamico_CarregarListas_Click(pSender)
{
    $("#btnContainerCMS_ConteudoDinamico_Abas").html("Listas");
    
    gFlagEditandoConteudoDinamicoOuLista = "lista";

    pnlContainerCMS_Form_Itens.hide();

    pnlContainerCMS_Form_Listas.show();

    $(pSender).parent().find("button.IconeMais").attr("title", "Adicionar Nova Lista").show();

    ModuloCMS_CarregarListasDoTipoSelecionado();

    return false;
}

function btnCMS_ConteudoDinamico_Adicionar_Click(pSender)
{
    if(gFlagEditandoConteudoDinamicoOuLista == null)
    {
        //quando não escolheu nenhum ainda, assume conteúdo como padrão:
        gFlagEditandoConteudoDinamicoOuLista = "conteudo";
    }

    if(gFlagEditandoConteudoDinamicoOuLista == "conteudo")
    {
        //monta o formulário

        ModuloCMS_CriarFormularioDeEdicao(cboCMS_ConteudoDinamico_SelecionarObjeto.find("option:selected").val(), null);
    }
    else if(gFlagEditandoConteudoDinamicoOuLista == "lista")
    {
        gContDinamicoSendoEditado = {   IdLista:        new Date().getTime()
                                      , IdTipoConteudo: cboCMS_ConteudoDinamico_SelecionarObjeto.find("option:selected").val()
                                      , FlagNovaLista:  true
                                      , Descricao:      ""
                                      , Regra:          "" };

        ModuloCMS_ConfigurarFormularioDeEdicaoParaListas();
    }

    return false;
}

function lstConteudoDinamico_LI_Click(pSender)
{
    pSender = $(pSender).closest("li");

    pSender.parent().find("li.Selecionado").removeClass("Selecionado");

    pSender.addClass("Selecionado");

    if(pSender.attr("data-IdConteudo"))
    {
        //é um item
        ModuloCMS_BuscarDadosParaEdicao(pSender.attr("data-IdTipoConteudo"), pSender.attr("data-IdConteudo"));
    }
    else
    {
        //é uma lista

        gContDinamicoSendoEditado = {   IdLista:        pSender.attr("data-IdLista")
                                      , IdTipoConteudo: pSender.attr("data-IdTipoConteudo")
                                      , FlagNovaLista:  false
                                      , Descricao:      pSender.find("label span:eq(1)").text()
                                      , Regra:          pSender.attr("data-Regra") };

        ModuloCMS_ConfigurarFormularioDeEdicaoParaListas();
    }

    return false;
}

function btnConteudoDinamico_Remover_Click(pSender)
{
    pSender = $(pSender).closest("li");

    if(pSender.attr("data-IdConteudo"))
    {
        //é um item
        ModuloCMS_ExcluirConteudoDinamico(pSender.attr("data-IdConteudo"));
    }
    else
    {
        ModuloCMS_ExcluirListaConteudo(pSender.attr("data-IdLista"));
    }

    return false;
}

function cboEdicaoWidget_Lista_TipoDeLista_Change(pSender)
{
    gDadosDaListaDinamicaSelecionada = null;

    pSender = $(pSender);

    if(pSender.val() == "E")
    {
        pSender.closest("div").find("p.FormEstatica").show();
        pSender.closest("div").find("p.FormDinamica").hide();
    }
    else
    {
        pSender.closest("div").find("p.FormEstatica").hide();
        pSender.closest("div").find("p.FormDinamica").show();
    }

    Widget_Lista_Elemento_Change(pSender);
}

function cboEdicaoWidget_Lista_TipoDeConteudo_Change(pSender)
{
    gDadosDaListaDinamicaSelecionada = null;

    if(cboEdicaoWidget_Lista_TipoDeConteudo.val() != "")
    {
        //pega as propriedades disponíveis desse tipo de objeto para exibir na combo:

        var lTipoJSON = cboEdicaoWidget_Lista_TipoDeConteudo.find("option:selected").attr("data-tipodeconteudojson");

        var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON(lTipoJSON);
        
        var lHTML = "";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value=''>$[" + lPropriedades[a] + "]</option>";
        }

        cboEdicaoWidget_Lista_PropDisponiveis.html(lHTML);

        lHTML = "<option value=''>(Nenhuma)</option>";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value='" + lPropriedades[a] + " A'>" + lPropriedades[a] + " Cres</option>";
            lHTML += "<option value='" + lPropriedades[a] + " D'>" + lPropriedades[a] + " Decr</option>";
        }

        cboEdicaoWidget_Lista_Ordenacao.html(lHTML);

        //faz o request para buscar as listas do tipo de conteúdo que foi escolhido

        var lRequest = { Acao: "CarregarListas", IdTipoConteudo: cboEdicaoWidget_Lista_TipoDeConteudo.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, cboEdicaoWidget_Lista_TipoDeConteudo_Change_CallBack);
    }
    else
    {
        cboEdicaoWidget_Lista_PropDisponiveis.html("");

        cboEdicaoWidget_Lista_Ordenacao.html("");
    }

    return false;
}

function cboEdicaoWidget_Lista_TipoDeConteudo_Change_CallBack(pResposta)
{
    var lLista = pResposta.ObjetoDeRetorno;

    var lHTML = "";
    
    if(lLista.length > 0)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            lHTML += "<option data-IdTipoConteudo='" + lLista[a].CodigoTipoConteudo + "' value='" + lLista[a].CodigoLista + "' data-Regra='" + lLista[a].Regra + "'>" +
                          lLista[a].DescricaoLista +
                     "</option>";
        }
    }
    else
    {
        lHTML += "<option value='0'>Nenhuma lista encontrada!</option>";
    }

    cboEdicaoWidget_Lista_IdListaDinamica.html(lHTML).show();
    
    cboEdicaoWidget_Lista_PropDisponiveis.show();

    cboEdicaoWidget_Lista_Ordenacao.show();

    if(gWidgetSendoEditado.Ordenacao && gWidgetSendoEditado.Ordenacao != null)
        cboEdicaoWidget_Lista_Ordenacao.val(gWidgetSendoEditado.Ordenacao);

    if(gWidgetSendoEditado != null && gWidgetSendoEditado.IdDaLista != null)
    {
        cboEdicaoWidget_Lista_IdListaDinamica.val(gWidgetSendoEditado.IdDaLista);
    }
}

function btnEdicaoWidget_Lista_Atualizar_Click(pSender)
{
    var lRequest = { Acao: "CarregarItensDaLista", IdDaLista: cboEdicaoWidget_Lista_IdListaDinamica.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, btnEdicaoWidget_Lista_Atualizar_Click_CallBack);
    

    return false;
}

function btnEdicaoWidget_Lista_Atualizar_Click_CallBack(pResposta)
{
    gDadosDaListaDinamicaSelecionada = pResposta.ObjetoDeRetorno;
    
    ModuloCMS_AtualizarDadosDeEdicao_WidgetLista();
}




function cboEdicaoWidget_Tabela_TipoDeTabela_Change(pSender)
{
    gDadosDaListaDinamicaSelecionada = null;

    pSender = $(pSender);

    if(pSender.val() == "E")
    {
        pSender.closest("div").find("p.FormEstatica").show();
        pSender.closest("div").find("p.FormDinamica").hide();
    }
    else
    {
        pSender.closest("div").find("p.FormEstatica").hide();
        pSender.closest("div").find("p.FormDinamica").show();
    }

    Widget_Tabela_Elemento_Change(pSender);
}


function cboEdicaoWidget_Tabela_TipoDeConteudo_Change(pSender)
{
    gDadosDaListaDinamicaSelecionada = null;

    if(cboEdicaoWidget_Tabela_TipoDeConteudo.val() != "" && cboEdicaoWidget_Tabela_TipoDeConteudo.val() != null)
    {
        //pega as propriedades disponíveis desse tipo de objeto para exibir na combo:

        var lTipoJSON = cboEdicaoWidget_Tabela_TipoDeConteudo.find("option:selected").attr("data-tipodeconteudojson");

        var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON(lTipoJSON);
        
        var lHTML = "";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value=''>$[" + lPropriedades[a] + "]</option>";
        }

        cboEdicaoWidget_Tabela_PropDisponiveis.html(lHTML);

        lHTML = "<option value=''>Nenhuma</option>";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value='" + lPropriedades[a] + " A'>" + lPropriedades[a] + " Cres</option>";
            lHTML += "<option value='" + lPropriedades[a] + " D'>" + lPropriedades[a] + " Decr</option>";
        }

        cboEdicaoWidget_Tabela_Ordenacao.html(lHTML);
        
        //faz o request para buscar as listas do tipo de conteúdo que foi escolhido

        var lRequest = { Acao: "CarregarListas", IdTipoConteudo: cboEdicaoWidget_Tabela_TipoDeConteudo.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, cboEdicaoWidget_Tabela_TipoDeConteudo_Change_CallBack);
    }
    else
    {
        cboEdicaoWidget_Tabela_PropDisponiveis.html("");
    }

    return false;
}

function cboEdicaoWidget_Tabela_TipoDeConteudo_Change_CallBack(pResposta)
{
    var lLista = pResposta.ObjetoDeRetorno;

    var lHTML = "";
    
    if(lLista.length > 0)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            lHTML += "<option data-IdTipoConteudo='" + lLista[a].CodigoTipoConteudo + "' value='" + lLista[a].CodigoLista + "' data-Regra='" + lLista[a].Regra + "'>" +
                          lLista[a].DescricaoLista +
                     "</option>";
        }
    }
    else
    {
        lHTML += "<option value='0'>Nenhuma lista encontrada!</option>";
    }

    cboEdicaoWidget_Tabela_IdListaDinamica.html(lHTML).show();

    cboEdicaoWidget_Tabela_PropDisponiveis.show();

    cboEdicaoWidget_Tabela_Ordenacao.show();

    if(gWidgetSendoEditado.Ordenacao && gWidgetSendoEditado.Ordenacao != null)
        cboEdicaoWidget_Tabela_Ordenacao.val(gWidgetSendoEditado.Ordenacao);

    if(gWidgetSendoEditado != null && gWidgetSendoEditado.IdDaLista != null)
    {
        cboEdicaoWidget_Tabela_IdListaDinamica.val(gWidgetSendoEditado.IdDaLista);
    }
}


function btnEdicaoWidget_Tabela_Atualizar_Click(pSender)
{
    var lRequest = { Acao: "CarregarItensDaLista", IdDaLista: cboEdicaoWidget_Tabela_IdListaDinamica.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, btnEdicaoWidget_Tabela_Atualizar_Click_CallBack);
    

    return false;
}

function btnEdicaoWidget_Tabela_Atualizar_Click_CallBack(pResposta)
{
    gDadosDaListaDinamicaSelecionada = pResposta.ObjetoDeRetorno;
    
    ModuloCMS_AtualizarDadosDeEdicao_WidgetTabela();
}




function cboEdicaoWidget_Repetidor_TipoDeConteudo_Change(pSender)
{
    gDadosDaListaDinamicaSelecionada = null;

    if(cboEdicaoWidget_Repetidor_TipoDeConteudo.val() != "")
    {
        //pega as propriedades disponíveis desse tipo de objeto para exibir na combo:

        var lTipoJSON = cboEdicaoWidget_Repetidor_TipoDeConteudo.find("option:selected").attr("data-tipodeconteudojson");

        var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON(lTipoJSON);
        
        var lHTML = "";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value=''>$[" + lPropriedades[a] + "]</option>";
        }

        cboEdicaoWidget_Repetidor_PropDisponiveis.html(lHTML);

        lHTML = "<option value=''>Nenhuma</option>";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value='" + lPropriedades[a] + " A'>" + lPropriedades[a] + " Cres</option>";
            lHTML += "<option value='" + lPropriedades[a] + " D'>" + lPropriedades[a] + " Decr</option>";
        }

        cboEdicaoWidget_Repetidor_Ordenacao.html(lHTML);
        
        //faz o request para buscar as listas do tipo de conteúdo que foi escolhido

        var lRequest = { Acao: "CarregarListas", IdTipoConteudo: cboEdicaoWidget_Repetidor_TipoDeConteudo.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, cboEdicaoWidget_Repetidor_TipoDeConteudo_Change_CallBack);
    }
    else
    {
        cboEdicaoWidget_Repetidor_PropDisponiveis.html("");
    }

    return false;
}

function cboEdicaoWidget_Repetidor_TipoDeConteudo_Change_CallBack(pResposta)
{
    var lLista = pResposta.ObjetoDeRetorno;

    var lHTML = "";
    
    if(lLista.length > 0)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            lHTML += "<option data-IdTipoConteudo='" + lLista[a].CodigoTipoConteudo + "' value='" + lLista[a].CodigoLista + "' data-Regra='" + lLista[a].Regra + "'>" +
                          lLista[a].DescricaoLista +
                     "</option>";
        }
    }
    else
    {
        lHTML += "<option value='0'>Nenhuma lista encontrada!</option>";
    }

    cboEdicaoWidget_Repetidor_IdListaDinamica.html(lHTML).show();

    cboEdicaoWidget_Repetidor_PropDisponiveis.show();

    cboEdicaoWidget_Repetidor_Ordenacao.show();

    if(gWidgetSendoEditado.Ordenacao && gWidgetSendoEditado.Ordenacao != null)
        cboEdicaoWidget_Repetidor_Ordenacao.val(gWidgetSendoEditado.Ordenacao);

    if(gWidgetSendoEditado != null && gWidgetSendoEditado.IdDaLista != null)
    {
        cboEdicaoWidget_Repetidor_IdListaDinamica.val(gWidgetSendoEditado.IdDaLista);
    }
}


function btnEdicaoWidget_Repetidor_Atualizar_Click(pSender)
{
    var lRequest = { Acao: "CarregarItensDaLista", IdDaLista: cboEdicaoWidget_Repetidor_IdListaDinamica.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, btnEdicaoWidget_Repetidor_Atualizar_Click_CallBack);

    return false;
}

function btnEdicaoWidget_Repetidor_Atualizar_Click_CallBack(pResposta)
{
    gDadosDaListaDinamicaSelecionada = pResposta.ObjetoDeRetorno;
    
    ModuloCMS_AtualizarDadosDeEdicao_WidgetRepetidor();
}








function cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo_Change(pSender)
{
    gDadosDaListaDinamicaSelecionada = null;

    if(cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo.val() != "")
    {
        //pega as propriedades disponíveis desse tipo de objeto para exibir na combo:

        var lTipoJSON = cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo.find("option:selected").attr("data-tipodeconteudojson");

        var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON(lTipoJSON);

        var lHTML = "";

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value=''>$[" + lPropriedades[a] + "]</option>";
        }

        cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis.html(lHTML);

        lHTML = "<option value=''>(Nenhuma)</option>";;

        for(var a = 0; a < lPropriedades.length; a++)
        {
            lHTML += "<option value='" + lPropriedades[a] + " A'>" + lPropriedades[a] + " Cres</option>";
            lHTML += "<option value='" + lPropriedades[a] + " D'>" + lPropriedades[a] + " Decr</option>";
        }

        cboEdicaoWidget_ListaDeDefinicao_Ordenacao.html(lHTML);
        
        //faz o request para buscar as listas do tipo de conteúdo que foi escolhido

        var lRequest = { Acao: "CarregarListas", IdTipoConteudo: cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo_Change_CallBack);
    }
    else
    {
        cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis.html("");
    }

    return false;
}

function cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo_Change_CallBack(pResposta)
{
    var lLista = pResposta.ObjetoDeRetorno;

    var lHTML = "";
    
    if(lLista.length > 0)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            lHTML += "<option data-IdTipoConteudo='" + lLista[a].CodigoTipoConteudo + "' value='" + lLista[a].CodigoLista + "' data-Regra='" + lLista[a].Regra + "'>" +
                          lLista[a].DescricaoLista +
                     "</option>";
        }
    }
    else
    {
        lHTML += "<option value='0'>Nenhuma lista encontrada!</option>";
    }

    cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.html(lHTML).show();

    cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis.show();

    cboEdicaoWidget_ListaDeDefinicao_Ordenacao.show();

    if(gWidgetSendoEditado.Ordenacao && gWidgetSendoEditado.Ordenacao != null)
        cboEdicaoWidget_ListaDeDefinicao_Ordenacao.val(gWidgetSendoEditado.Ordenacao);

    if(gWidgetSendoEditado != null && gWidgetSendoEditado.IdDaLista != null)
    {
        cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.val(gWidgetSendoEditado.IdDaLista);
    }
}


function btnEdicaoWidget_ListaDeDefinicao_Atualizar_Click(pSender)
{
    var lRequest = { Acao: "CarregarItensDaLista", IdDaLista: cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, btnEdicaoWidget_ListaDeDefinicao_Atualizar_Click_CallBack);
    

    return false;
}

function btnEdicaoWidget_ListaDeDefinicao_Atualizar_Click_CallBack(pResposta)
{
    gDadosDaListaDinamicaSelecionada = pResposta.ObjetoDeRetorno;
    
    ModuloCMS_AtualizarDadosDeEdicao_WidgetListaDeDefinicao();
}


function Widget_ListaDeDefinicao_DT_Click(pEvent)
{
    var lDT = $(pEvent.target);
    var lDL = lDT.closest("dl");
    
    if(lDL.hasClass("Acordeao"))
    {
        if(!lDT.next("dd").is(":visible"))
        {
            lDL.find("dd").hide();

            lDT.next("dd").show();
        }
        else
        {
            lDT.next("dd").hide();
        }
    }
    else if(lDL.hasClass("ItensExpansiveis"))
    {
        lDT.next("dd").toggle();
    }
}




function txtGerenciadorDeArquivos_Upload_CallBack(pResponse, pResposta)
{
    for(var a = 0; a < pResposta.result.ObjetoDeRetorno.length; a++)
    {
        //alert("Arquivo " + pResposta.result.ObjetoDeRetorno[a] + " ok!");
        var lPrimeiroItem = pnlArquivosContainer_lstImagens.find("li:eq(0)");

        var lNomeArquivo = pResposta.result.ObjetoDeRetorno[a];

        var lExtensao = "";

        var lCaminhoDoThumb = GradSite_BuscarRaiz();

        while(lNomeArquivo.indexOf("/") != -1)
        {
            lNomeArquivo = lNomeArquivo.substr(lNomeArquivo.indexOf("/") + 1);
        }

        lExtensao += lNomeArquivo.charAt(lNomeArquivo.length - 1);

        while(lExtensao.charAt(0) != ".")
        {
            lExtensao = lNomeArquivo.charAt(lNomeArquivo.length - (lExtensao.length + 1)) + lExtensao;
        }

        lExtensao = lExtensao.substr(1).toLowerCase();

        if($.inArray(lExtensao, gListaDeExtensoesDeImagem) != -1)
        {
            lCaminhoDoThumb = pResposta.result.ObjetoDeRetorno[a];      //imagem deixa a própria imagem
        }
        else if($.inArray(lExtensao, gListaDeExtensoesDePDF) != -1)
        {
            lCaminhoDoThumb += "/Resc/Skin/Default/Img/Imagem-TipoDeArquivo-PDF.jpg";
        }
        else if($.inArray(lExtensao, gListaDeExtensoesDeOffice) != -1)
        {
            if(lExtensao == "xls" || lExtensao == "xlsx")
            {
                lCaminhoDoThumb += "/Resc/Skin/Default/Img/Imagem-TipoDeArquivo-XLS.jpg";
            }
            else if(lExtensao == "ppt" || lExtensao == "pptx")
            {
                lCaminhoDoThumb += "/Resc/Skin/Default/Img/Imagem-TipoDeArquivo-PPT.jpg";
            }
            else
            {
                lCaminhoDoThumb += "/Resc/Skin/Default/Img/Imagem-TipoDeArquivo-DOC.jpg";
            }
        }
        else if($.inArray(lExtensao, gListaDeExtensoesDeZip) != -1)
        {
            lCaminhoDoThumb += "/Resc/Skin/Default/Img/Imagem-TipoDeArquivo-ZIP.jpg";
        }
        else
        {
            lCaminhoDoThumb += "/Resc/Skin/Default/Img/Imagem-TipoDeArquivo-Outro.jpg";
        }

        var lHTML = "<li title='" + lNomeArquivo + "' onClick='return lstImagens_LI_Click(this)'>" + 
                    "   <img src='" + lCaminhoDoThumb + "' alt='" + lNomeArquivo + "' />" + 
                    "   <button title='Excluir Imagem' class='BotaoIcone IconeLixeira'    onclick='return btnImagens_Excluir_Click(this)'><span>Excluir Imagem</span></button>" +
                    "   <button title='Copiar Link'    class='BotaoIcone IconeCopiarLink' onclick='return btnImagens_CopiarLink_Click(this)'><span>Copiar Link</span></button>" +
                    "   <input type='text' readonly value='" + pResposta.result.ObjetoDeRetorno[a] + "' />" + 
                    "</li>";

        if(lPrimeiroItem.length > 0)
        {
            lPrimeiroItem.before(lHTML);
        }
        else
        {
            pnlArquivosContainer_lstImagens.append(lHTML);
        }
    }
}


function btnGerenciadorDeArquivos_AlterarVisualizacao_Click(pSender, pTipo)
{
    window.clearTimeout(gTimeOutOcultarPainelDeImagens);

    if(pTipo == 0)
    {
        pnlArquivosContainer_lstImagens.removeClass("Detalhes");
    }
    else if(pTipo == 1)
    {
        pnlArquivosContainer_lstImagens.addClass("Detalhes");
    }

    return false;
}


function btnEdicaoDeHTML_PainelDeArquivos_Click(pSender)
{
    if(!pnlArquivosContainer.hasClass("DisplayComoPickerEdicaoHTML"))
    {
        pnlArquivosContainer.addClass("DisplayComoPickerEdicaoHTML").show();

        $(pSender).addClass("Selecionado");
    }
    else
    {
        pnlArquivosContainer.removeClass("DisplayComoPickerEdicaoHTML").hide();
        
        $(pSender).removeClass("Selecionado");
    }

    return false;
}

function FakeComboBotaoDeExpansao_Click(pSender)
{
    pSender = $(pSender);

    if(pSender.hasClass("Selecionado"))
    {
        pSender.removeClass("Selecionado");
        pSender.parent().find("ul.FakeComboParaCopiar").hide();
    }
    else
    {
        pSender.addClass("Selecionado");

        pSender
            .parent()
                .find("ul.FakeComboParaCopiar")
                .css( { left: pSender.position().left, top: pSender.position().top + 30 } )
                .show();
    }
}


function btnEdicaoDeHTML_InserirTabela_Click(pSender)
{
    pSender = $(pSender);

    var pnlEdicaoDeHTML_InserirTabela = $("#pnlEdicaoDeHTML_InserirTabela");

    if(pSender.hasClass("Selecionado"))
    {
        pSender.removeClass("Selecionado");
        pnlEdicaoDeHTML_InserirTabela.hide();
    }
    else
    {
        pSender.addClass("Selecionado");

        pnlEdicaoDeHTML_InserirTabela
            .css( { left: pSender.position().left, top: pSender.position().top + 30 } )
            .show();
    }
}


function txtEdicaoDeHTML_InserirTabela_Cabecalho_Blur()
{
    var lValor = txtEdicaoDeHTML_InserirTabela_Cabecalho.val();

    lValor = lValor.split("|");

    if(lValor.length > 0)
    {
        for(var a = 0; a < lValor.length; a++)
        {
            lValor[a] = lValor[a].trim();
        }

        if(txtEdicaoDeHTML_InserirTabela_Colunas.val() == "")
            txtEdicaoDeHTML_InserirTabela_Colunas.val(lValor.length);

        if(txtEdicaoDeHTML_InserirTabela_Linhas.val() == "")
            txtEdicaoDeHTML_InserirTabela_Linhas.val("10");
    }
    else
    {
        lValor = [];
    }

    return lValor;
}

function btnEdicaoDeHTML_InserirTabela_Cancelar_Click(pSender)
{
    $("#pnlEdicaoDeHTML_InserirTabela").hide().closest("div").prev("label").removeClass("Selecionado");

    return false;
}


function btnEdicaoDeHTML_InserirTabela_Ok_Click(pSender)
{
    var lCabecalho = txtEdicaoDeHTML_InserirTabela_Cabecalho_Blur();

    var lLinhas  = new Number(txtEdicaoDeHTML_InserirTabela_Linhas.val());
    var lColunas = new Number(txtEdicaoDeHTML_InserirTabela_Colunas.val());

    var lHTML = "<table>";

    if(isNaN(lLinhas))
    {
        alert("Número de linhas inválido!")

        txtEdicaoDeHTML_InserirTabela_Linhas.focus().select();

        return false;
    }

    if(isNaN(lColunas))
    {
        alert("Número de colunas inválido!")

        txtEdicaoDeHTML_InserirTabela_Colunas.focus().select();

        return false;
    }

    if(lCabecalho.length > 0)
    {
        lHTML += "<thead><tr>";

        for(var a = 0; a < lCabecalho.length; a++)
        {
            lHTML += "<td>" + lCabecalho[a] + "</td>";
        }

        if(lColunas > lCabecalho.length)
        {
            for(var a = lColunas; a > lCabecalho.length; a--)
            {
                lHTML += "<td>&nbsp;</td>";
            }
        }

        lHTML += "</tr></thead>";
    }

    lHTML += "<tbody>";
    
    for(var a = 0; a < lLinhas; a++)
    {
        lHTML += "<tr>";

        for(var b = 0; b < lColunas; b++)
        {
            lHTML += "<td>[" + (a+1) + "," + (b+1) + "]</td>";
        }
        
        lHTML += "</tr>";
    }

    lHTML += "</tbody>";
    lHTML += "</table>";

    //alert(lHTML);

    txtEdicaoDeHTML.htmlarea("insertHTML", lHTML);
    //txtEdicaoDeHTML.exec("insertHTML", false, lHTML);

    btnEdicaoDeHTML_InserirTabela_Cancelar_Click(pSender);

    return false;
}


function btnTipoDeUsuario_Click(pTipo)
{
    var lRequest = { Acao: "MudarTipoDeCliente", IdTipoDeCliente: pTipo, IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, btnTipoDeUsuario_Click_CallBack);

    return false;
}

function btnTipoDeUsuario_Click_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        document.location = document.location.href;
    }
}

function btnEdicaoWidget_Abas_NovoItem_Adicionar_Click(pSender)
{
    ModuloCMS_WidgetAba_AdicionarNova();

    return false;
}

function btnEdicaoWidget_Acordeon_NovoItem_Adicionar_Click(pSender)
{
    ModuloCMS_WidgetAcordeon_AdicionarNova();

    return false;
}

function CopiarEstrutura(pTipoUsuario)
{
    if(confirm("Tem certeza que deseja copiar a Estrutura?\r\n\r\n A estrutura será exatamente uma cópia da outra!"))
    {
        var lRequest = {   Acao: "CopiarEstrutura"
                        , IdTipoUsuarioPara: pTipoUsuario
                        , IdDaPagina: $("#hidIdDaPagina").val()
                        , IdDaEstrutura: $("#hidIdDaEstrutura").val()
                       };

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, CopiarEstrutura_CallBack);
    }

    return false;
}

function CopiarEstrutura_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        GradSite_ExibirMensagem("I", "Estrutura copiada com sucesso!");
    }
}



function btnEdicaoWidget_Lista_Verificar_Click(pSender)
{
    var lSelecionada = $("#lstConteudoDinamico li.Selecionado");

    var lDados = { IdDaLista: "", RegraDaLista: "" };

    /*
    if(lSelecionada.length > 0)
    {
        lDados.IdDaLista = lSelecionada.attr("data-IdLista");
    }
    else
    {
        lDados.RegraDaLista = $("#txtEdicaoLista_Regra").val();
        lDados.IdTipoConteudo = cboCMS_ConteudoDinamico_SelecionarObjeto.find("option:selected").val()
    }
    */

    lDados.RegraDaLista = $("#txtEdicaoLista_Regra").val();
    lDados.IdTipoConteudo = cboCMS_ConteudoDinamico_SelecionarObjeto.find("option:selected").val()

    ModuloCMS_CarregarDadosDaListaParaViz(lDados);

    return false;
}

function ModuloCMS_CarregarDadosDaListaParaViz(pDados)
{
    var lRequest = { Acao: "CarregarItensDaLista", IdDaLista: pDados.IdDaLista, RegraDaLista: pDados.RegraDaLista, IdTipoConteudo: pDados.IdTipoConteudo };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_CarregarDadosDaListaParaViz_CallBack);

    return false;
}

function ModuloCMS_CarregarDadosDaListaParaViz_CallBack(pResposta)
{
    //gDadosDaListaDinamicaSelecionada = pResposta.ObjetoDeRetorno;
    
    var lTabela = $("#tblEdicaoLista_Regra_ResultadosDoTeste");

    lTabela.closest("div.ContainerCMS_PainelComTabs_Form").find("p.DicaListas").hide();

    lTabela.find("tbody").remove();

    var lBody = "<tbody style='background-color:#fff'>";

    for(var a = 0; a < pResposta.ObjetoDeRetorno.length; a++)
    {
        lBody += "<tr><td style='white-space:nowrap'>" + $.toJSON(pResposta.ObjetoDeRetorno[a]) + "</td></tr>";
    }

    lBody += "</tbody>";

    lTabela.append(lBody);

    lTabela.find("thead > tr > td").html(pResposta.ObjetoDeRetorno.length + " itens encontrados");

    lTabela.parent().show();
}


function btnInfoPagina_Visualizar_Click(pSender)
{
    var lURL = document.location.pathname;

    document.location = lURL + "?versao=" + $("#cboInfoPag_VersoesDisponiveis").val();

    return false;
}

function btnInfoPagina_Publicar_Click(pSender)
{
    var lDados = { Acao: "PublicarVersao", Versao: $("#lblInfoPag_Versao").text().trim(), IdPagina: $("#hidIdDaPagina").val() };

    GradSite_CarregarJsonVerificandoErro( GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, ModuloCMS_PublicarPagina_CallBack );

    return false;
}

function ModuloCMS_PublicarPagina_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        GradSite_ExibirMensagem("I", "Versão publicada com sucesso");

        $("#btnInfoPagina_Publicar").remove();
    }
    else
    {
        alert(pResposta.Mensagem);
    }
}

function btnInfoPagina_CriarVersao_Click(pSender)
{
    var lDados = { Acao: "CriarVersao", Versao: $("#txtInfoPag_NovaVersao").val(), IdPagina: $("#hidIdDaPagina").val() };

    if(lDados.Versao == "")
    {
        alert("A versão não pode ser vazia!");

        return false;
    }

    if( $("#cboInfoPag_VersoesDisponiveis").find("option[value='" + lDados.Versao + "']").length > 0)
    {
        alert("Já existe uma versão " + lDados.Versao);

        return false;
    }
    else
    {
        GradSite_CarregarJsonVerificandoErro( GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, ModuloCMS_CriarVersao_CallBack );
    }

    return false;
}

function ModuloCMS_CriarVersao_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        GradSite_ExibirMensagem("I", "Versão criada com sucesso");

        $("#cboInfoPag_VersoesDisponiveis").append("<option value='" + pResposta.ObjetoDeRetorno + "'>" + pResposta.ObjetoDeRetorno + "</option>");
    }
    else
    {
        alert(pResposta.Mensagem);
    }
}
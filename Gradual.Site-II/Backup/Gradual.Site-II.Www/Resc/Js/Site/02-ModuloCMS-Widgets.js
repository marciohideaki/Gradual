/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />
/// <reference path="02-ModuloCMS-Base.js" />
/// <reference path="02-ModuloCMS-ConteudoDinamico.js" />


function ModuloCMS_CarregarDadosDaPaginaParaEdicao()
{
    var lObjetosDeWidget = $("[id^='wid']");

    var lIdDaPagina    = $("#hidIdDaPagina").val();
    var lIdDaEstrutura = $("#hidIdDaEstrutura").val();

    gEstruturaDaPagina = { 
                                IdDaPagina : lIdDaPagina
                            , IdDaEstrutura : lIdDaEstrutura
                            ,       Widgets : []
                            }

    if(lObjetosDeWidget.length > 0)
    {
        var lNovoWidget, lIdElemento, lIdDoWidget, lTipoWidget;

        for(var a = 0; a < lObjetosDeWidget.length; a++)
        {
            lIdElemento = lObjetosDeWidget[a].getAttribute("id"); 

            lIdDoWidget = lIdElemento;
            lTipoWidget = lIdElemento;

            lIdDoWidget = lIdDoWidget.substr(lIdDoWidget.indexOf("-") + 1);
            lIdDoWidget = lIdDoWidget.substr(lIdDoWidget.indexOf("-") + 1);

            lTipoWidget = lTipoWidget.substr(3, lTipoWidget.indexOf("-") - 3);

            lNovoWidget = ModuloCMS_ConfigurarWidgetAPartirDoConteudo(  {
                                                                                      Tipo: lTipoWidget
                                                                          ,     IdDoWidget: lIdDoWidget
                                                                          ,  IdDaEstrutura: lIdDaEstrutura
                                                                          ,     IdElemento: lIdElemento
                                                                          ,  OrdemNaPagina: (a + 1)
                                                                          , FlagNovoWidget: false
                                                                        }
                                                                      , $(lObjetosDeWidget[a])
                                                                     );

            gEstruturaDaPagina.Widgets.push(lNovoWidget);
        }

        try
        {
            ModuloCMS_MontarListaDeEstruturaDaPagina();
        }
        catch(erro)
        {
            alert("Erro de javascript em ModuloCMS_CarregarDadosDaPaginaParaEdicao() > ModuloCMS_MontarListaDeEstruturaDaPagina():\r\n" + erro);
        }
    }
}



function ModuloCMS_AdicionarNovoWidget(pTipo)
{
    var lExisteUmNovo = (lstEstruturaDaPagina.find("li[data-FlagNovoWidget='true']").length > 0);

    if(!lExisteUmNovo)
    {
        var lNovoWidget = {
                                          Tipo: pTipo
                            ,       IdDoWidget: new Date().getTime()
                            ,    IdDaEstrutura: $("#hidIdDaEstrutura").val()
                            ,       IdElemento: ""
                            ,    OrdemNaPagina: (lstEstruturaDaPagina.find("li").length + 1)
                            ,   FlagNovoWidget: true
                            ,    AtributoClass: ""
                            ,    AtributoStyle: ""
                          };

        lNovoWidget = ModuloCMS_ConfigurarNovoWidget(lNovoWidget);

        gEstruturaDaPagina.Widgets.push(lNovoWidget);

        ModuloCMS_MontarListaDeEstruturaDaPagina();

        lstEstruturaDaPagina.find("li:last label").click();

        try
        {
            lstEstruturaDaPagina.scrollTop(100000);
        }catch(erro){}

        var lDivVisivel = $("div.ContainerCMS_Painel[id]:visible");

        var lIdDoVisivel = lDivVisivel.attr("id");
    
        if(lIdDoVisivel == "pnlEstruturaContainer_PainelEdicaoWidget_Abas"  ||
           lIdDoVisivel == "pnlEstruturaContainer_PainelEdicaoWidget_Texto" ||
           lIdDoVisivel == "pnlEstruturaContainer_PainelEdicaoWidget_Lista" ||
           lIdDoVisivel == "pnlEstruturaContainer_PainelEdicaoWidget_Embed" )
        {
            lDivVisivel.find("textarea:eq(0)").focus();
        }
        else if(lIdDoVisivel == "pnlEstruturaContainer_PainelEdicaoWidget_TextoHTML")
        {
            //não dá foco para não aparecer a edição de HTML direto na cara do usuário
        }
        else
        {
            lDivVisivel.find("input[type='text']:eq(0)").focus();
        }
    }
    else
    {
        alert("Favor salvar ou excluir o novo widget antes de incluir mais um.");
    }
}

function ModuloCMS_ConfigurarNovoWidget(pWidgetBase)
{
    var lUltimoConteudo = $("#hidFimDosWidgets");

    pWidgetBase.IdElemento = "wid" + pWidgetBase.Tipo + "-" + pWidgetBase.IdDaEstrutura + "-" + pWidgetBase.IdDoWidget;

    if(pWidgetBase.Tipo == CONST_WID_TIPO_TITULO)
    {
        pWidgetBase.Nivel = "1";
        pWidgetBase.Texto = "";
        pWidgetBase.Resumo = "(novo)";

        lUltimoConteudo.before( "<h1 id='" + pWidgetBase.IdElemento + "'></h1>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_TEXTO)
    {
        pWidgetBase.Texto = "";
        pWidgetBase.Resumo = "(novo)";

        lUltimoConteudo.before( "<p id='" + pWidgetBase.IdElemento + "'></p>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_IMAGEM)
    {
        pWidgetBase.AtributoSrc = "";
        pWidgetBase.AtributoAlt = "";

        pWidgetBase.FlagHabilitarZoom = false;
        pWidgetBase.LinkPara = "";

        pWidgetBase.FlagTamanhoAutomatico = true;

        pWidgetBase.AtributoWidth = "";
        pWidgetBase.AtributoHeight = "";

        pWidgetBase.Resumo = "(imagem)";

        lUltimoConteudo.before( "<img id='" + pWidgetBase.IdElemento + "' />" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_LISTA)
    {
        pWidgetBase.IdDoConteudo = null;
        pWidgetBase.Texto = "";
        pWidgetBase.TemplateDoItem = "";
        pWidgetBase.FlagListaEstatica = true;

        pWidgetBase.Resumo = "(lista)";

        lUltimoConteudo.before( "<ul id='" + pWidgetBase.IdElemento + "'><code class='dataTemplateDoItem'></code><code class='dataAtributosDoItem'></code></ul>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_TABELA)
    {
        pWidgetBase.IdDaLista = "";
        pWidgetBase.Texto = "";
        pWidgetBase.Cabecalho = "";
        pWidgetBase.FlagTabelaEstatica = true;

        pWidgetBase.Resumo = "(tabela)";

        lUltimoConteudo.before( "<table id='" + pWidgetBase.IdElemento + "'><thead></thead><tbody></tbody></table>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_REPETIDOR)
    {
        pWidgetBase.IdDaLista = "";
        pWidgetBase.TemplateDoItem = "";

        pWidgetBase.Resumo = "(repetidor)";

        lUltimoConteudo.before( "<div id='" + pWidgetBase.IdElemento + "'><code class='TemplateDoItem'></code></div>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_LISTA_DE_DEFINICAO)
    {
        pWidgetBase.IdDaLista = "";
        pWidgetBase.TemplateDoItem = "";

        pWidgetBase.Resumo = "(lista)";

        lUltimoConteudo.before( "<dl id='" + pWidgetBase.IdElemento + "'></dl>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_EMBED)
    {
        pWidgetBase.Resumo = "(código)";
        pWidgetBase.Codigo = "";

        lUltimoConteudo.before( "<div id='" + pWidgetBase.IdElemento + "'></div>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_ABAS)
    {
        pWidgetBase.Texto = "";
        pWidgetBase.Resumo = "(novo)";

        lUltimoConteudo.before( "<div id='" + pWidgetBase.IdElemento + "' ><ul class='menu-tabs'></ul><button title='Visitar página da aba selecionada' onclick='return btnWidAbas_VisitarPaginaConteudo_Click(this)' class='btnVisitarPaginaConteudo'></button></div>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_ACORDEON)
    {
        pWidgetBase.Texto = "";
        pWidgetBase.Resumo = "(novo)";

        lUltimoConteudo.before( "<div id='" + pWidgetBase.IdElemento + "' ><ul class='acordeon'></ul></div>" );
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_TEXTOHTML)
    {
        pWidgetBase.Resumo = "(html)";
        pWidgetBase.Codigo = "";

        lUltimoConteudo.before( "<div id='" + pWidgetBase.IdElemento + "'></div>" );
    }
    else
    {
        
    }

    return pWidgetBase;
}


function ModuloCMS_ConfigurarWidgetAPartirDoConteudo(pWidgetBase, pConteudo)
{
    if(pWidgetBase.Tipo == CONST_WID_TIPO_TITULO)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.Nivel = pConteudo[0].tagName.toUpperCase().replace("H", "");

        pWidgetBase.Texto = pConteudo.find("input.TextoOriginal").val(); //pConteudo.html();

        pWidgetBase.Resumo = pConteudo.text();

        if(pWidgetBase.Resumo.length > 40)
            pWidgetBase.Resumo = pWidgetBase.Resumo.substr(0, 40) + " (...)";
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_TEXTO)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.Texto = pConteudo.find("input.TextoOriginal").val(); //pConteudo.html();

        pWidgetBase.Resumo = pConteudo.text();

        if(pWidgetBase.Resumo.length > 40)
            pWidgetBase.Resumo = pWidgetBase.Resumo.substr(0, 40) + " (...)";
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_TEXTOHTML)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        // se algum item foi incluido dentro de seu pai via atributo data-ElementoPai,
        // precisa remover ele temporariamente antes de incluir pegar o html,
        // porque ele não pertence ali na verdade.
        
        var lRemover       = pConteudo.find("[data-RemoverAoEditarHtml]");
        var lRemoverFilhos = pConteudo.find("[data-RemoverFilhosAoEditarHtml]");

        if(lRemover.length > 0 || lRemoverFilhos.length > 0)
        {
            var lHtmlAntesDeRemover = pConteudo.html();

            if(lRemover.length > 0)
                lRemover.remove();

            if(lRemoverFilhos.length > 0)
                lRemoverFilhos.find("option[value!='']").remove();

            pWidgetBase.ConteudoHTML = pConteudo.html(); //pConteudo.html();

            pConteudo.html(lHtmlAntesDeRemover);
        }
        else
        {
            pWidgetBase.ConteudoHTML = pConteudo.html(); //pConteudo.html();
        }

        pWidgetBase.Resumo = "(html)";
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_IMAGEM)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.AtributoSrc = pConteudo.attr("src");
        pWidgetBase.AtributoAlt = pConteudo.attr("alt");

        if(pConteudo.parent()[0].tagName.toUpperCase() == "A")
        {
            if(pConteudo.parent().hasClass("ContainerDeZoom"))
            {
                pWidgetBase.FlagHabilitarZoom = true;
                pWidgetBase.LinkPara = "";
            }
            else
            {
                pWidgetBase.FlagHabilitarZoom = false;
                pWidgetBase.LinkPara = pConteudo.parent().attr("href");
            }
        }

        if(pConteudo.attr("width"))
        {
            pWidgetBase.FlagTamanhoAutomatico = false;

            pWidgetBase.AtributoWidth = pConteudo.attr("width");
            pWidgetBase.AtributoHeight = pConteudo.attr("height");
        }
        else
        {
            pWidgetBase.FlagTamanhoAutomatico = true;
        }

        pWidgetBase.Resumo = "(imagem)";
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_LISTA)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.IdDaLista        = pConteudo.attr("data-IdDaLista");
        pWidgetBase.DescricaoDaLista = pConteudo.attr("data-DescricaoDaLista");
        pWidgetBase.Ordenacao        = pConteudo.attr("data-Ordenacao");

        pWidgetBase.TemplateDoItem   = pConteudo.find("code.dataTemplateDoItem").html();
        pWidgetBase.Atributos        = pConteudo.find("code.dataAtributosDoItem").html();

        var lTexto = "";

        if(pWidgetBase.IdDaLista == "0")
        {
            pWidgetBase.FlagListaEstatica = true;

            pConteudo.find("li").each(function()
            {
                lTexto = lTexto + $(this).find("code.dataTexto").html() + "\r\n";
            });
        }
        else
        {
            pWidgetBase.FlagListaEstatica = false;
        }

        pWidgetBase.Texto = lTexto;

        pWidgetBase.Resumo = "Lista " + pWidgetBase.DescricaoDaLista;
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_TABELA)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.Cabecalho = pConteudo.find("thead").attr("data-Cabecalho");

        var lTexto = "";

        if(pConteudo.attr("data-IdDaLista") == "0")
        {
            pWidgetBase.FlagTabelaEstatica = true;

            pConteudo.find("tbody tr").each(function()
            {
                lTexto = lTexto + $(this).attr("data-Texto") + "\r\n";
            });

            pWidgetBase.DescricaoDaLista = "Estática";
        }
        else
        {
            pWidgetBase.FlagTabelaEstatica = false;

            pWidgetBase.IdDaLista        = pConteudo.attr("data-IdDaLista");
            pWidgetBase.DescricaoDaLista = pConteudo.attr("data-DescricaoDaLista");
            pWidgetBase.Ordenacao        = pConteudo.attr("data-Ordenacao");

            lTexto = pConteudo.attr("data-TemplateDaLinha");
        }
        
        pWidgetBase.Texto = lTexto;

        pWidgetBase.Resumo = "Lista " + pWidgetBase.DescricaoDaLista;
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_REPETIDOR)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.IdDaLista        = pConteudo.attr("data-IdDaLista");
        pWidgetBase.DescricaoDaLista = pConteudo.attr("data-DescricaoDaLista");
        pWidgetBase.Ordenacao        = pConteudo.attr("data-Ordenacao");

        pWidgetBase.TemplateDoItem = pConteudo.find("code.TemplateDoItem").html();

        pWidgetBase.Resumo = "Lista " + pWidgetBase.DescricaoDaLista;
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_LISTA_DE_DEFINICAO)
    {
        pWidgetBase.AtributoClass = pConteudo.attr("class");
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.IdDaLista        = pConteudo.attr("data-IdDaLista");
        pWidgetBase.DescricaoDaLista = pConteudo.attr("data-DescricaoDaLista");
        pWidgetBase.Ordenacao        = pConteudo.attr("data-Ordenacao");

        pWidgetBase.TemplateDoItem = pConteudo.attr("data-TemplateDoItem");

        pWidgetBase.Resumo = "Lista " + pWidgetBase.DescricaoDaLista;
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_EMBED)
    {
        pWidgetBase.AtributoClass = "";
        pWidgetBase.AtributoStyle = pConteudo.attr("style");

        pWidgetBase.Codigo = pConteudo.html();

        pWidgetBase.Resumo = "(código)";
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_ABAS)
    {
        pWidgetBase.AtributoClass = pConteudo.find(" > ul").attr("class");
        pWidgetBase.AtributoStyle = pConteudo.find(" > ul").attr("style");

        try
        {
            pWidgetBase.ListaDeAbas = $.evalJSON(pConteudo.attr("data-ListaDeAbas"));
        }
        catch(erro)
        {
            alert("Erro ao interpretar JSON das abas:\r\n" + erro + "\r\n\r\n" + pConteudo.attr("data-ListaDeAbas"));
        }

        pWidgetBase.Resumo = "(abas)";

        if(pWidgetBase.Resumo.length > 40)
            pWidgetBase.Resumo = pWidgetBase.Resumo.substr(0, 40) + " (...)";
    }
    else if(pWidgetBase.Tipo == CONST_WID_TIPO_ACORDEON)
    {
        pWidgetBase.AtributoClass = pConteudo.find(" > ul").attr("class");
        pWidgetBase.AtributoStyle = pConteudo.find(" > ul").attr("style");

        try
        {
            pWidgetBase.ListaDeAbas = $.evalJSON(pConteudo.attr("data-ListaDeAbas"));
        }
        catch(erro)
        {
            alert("Erro ao interpretar JSON do cardeon:\r\n" + erro + "\r\n\r\n" + pConteudo.attr("data-ListaDeAbas"));
        }

        pWidgetBase.Resumo = "(acordeon)";

        if(pWidgetBase.Resumo.length > 40)
            pWidgetBase.Resumo = pWidgetBase.Resumo.substr(0, 40) + " (...)";
    }
    else
    {
        pWidgetBase.Resumo = "(falta fazer!)";
    }

    return pWidgetBase;
}





function ModuloCMS_MontarListaDeEstruturaDaPagina()
{
    var lWidget;

    var lHTML = "";

    var lApelido;

    for(var a = 0; a < gEstruturaDaPagina.Widgets.length; a++)
    {
        lWidget = gEstruturaDaPagina.Widgets[a];

        lApelido = lWidget.Tipo;

        if(lApelido == CONST_WID_TIPO_LISTA_DE_DEFINICAO)
        {
            lApelido = "L. Def.";
        }

        lHTML += "<li data-IdDoWidget='" + lWidget.IdDoWidget + "' data-IdElemento='" + lWidget.IdElemento + "' data-FlagNovoWidget=" + lWidget.FlagNovoWidget + " onMouseOver='lstEstruturaDaPagina_LI_MouseOver(this)'>" + 
                 "    <label onClick='lstEstruturaDaPagina_LI_Click(this)'><span>" + lApelido + "</span><span>" + lWidget.Resumo + "</span></label>" + 
                 "    <div>" + 
                 "        <button title='Descer'  onclick='return btnEstruturaDaPagina_Descer_Click(this)'>   <span>Descer</span>    </button>" + 
                 "        <button title='Subir'   onclick='return btnEstruturaDaPagina_Subir_Click(this)'>    <span>Subir</span>     </button>" + 
                 "        <button title='Remover' onclick='return btnEstruturaDaPagina_Remover_Click(this)'>  <span>Remover</span>   </button>" + 
                 "    </div>" + 
                 "</li>";
    }

    lstEstruturaDaPagina.html( lHTML );
}

function ModuloCMS_MontarFormDeEdicaoDoWidget(pItemSelecionado)
{
    ModuloCMS_VerificarModificacoesParaSalvar();

    if(!pItemSelecionado.hasClass("Selecionado"))
    {
        var lIdDoWidget, lIndiceDoWidget;

        pItemSelecionado.parent().find(".Selecionado").removeClass("Selecionado");

        pItemSelecionado.addClass("Selecionado");

        lIdDoWidget = pItemSelecionado.attr("data-IdDoWidget");

        gWidgetJsonAntesDeEditar = null;
        gWidgetSendoEditado = null;
        gElementoDoWidgetSendoEditado = null;

        for(var a = 0; a < gEstruturaDaPagina.Widgets.length; a++)
        {
            if(gEstruturaDaPagina.Widgets[a].IdDoWidget == lIdDoWidget)
            {
                gWidgetJsonAntesDeEditar = $.toJSON(gEstruturaDaPagina.Widgets[a]);

                gWidgetSendoEditado = gEstruturaDaPagina.Widgets[a];

                gElementoDoWidgetSendoEditado = $("#" + gWidgetSendoEditado.IdElemento);

                break;
            }
        }

        if(gWidgetSendoEditado != null)
        {
            if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TITULO)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetTitulo();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TEXTO)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetTexto();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TEXTOHTML)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetTextoHTML();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_IMAGEM)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetImagem();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_LISTA)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetLista();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TABELA)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetTabela();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_REPETIDOR)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetRepetidor();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_LISTA_DE_DEFINICAO)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetListaDeDefinicao();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_EMBED)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetEmbed();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_ABAS)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetAbas();
            }
            else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_ACORDEON)
            {
                ModuloCMS_MontarFormDeEdicao_WidgetAcordeon();
            }
        }
    }
}

function ModuloCMS_MontarFormDeEdicao_WidgetAbas()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Abas");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:
    
    if(!gWidgetSendoEditado.ListaDeAbas || gWidgetSendoEditado.ListaDeAbas === undefined)
    {
        gWidgetSendoEditado.ListaDeAbas = [];
    }

    lstEdicaoWidget_Abas_Itens.find(">tr:gt(0)").remove();

    for(var a = 0; a < gWidgetSendoEditado.ListaDeAbas.length; a++)
    {
        ModuloCMS_WidgetAba_AdicionarNovaNaLista(gWidgetSendoEditado.ListaDeAbas[a]);
    }
    
    if(gWidgetSendoEditado.AtributoClass == "")
         gWidgetSendoEditado.AtributoClass = "menu-tabs";

    txtEdicaoWidget_Abas_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);

    cboEdicaoWidget_Abas_AtributoClass.val(gWidgetSendoEditado.AtributoClass);
}

function ModuloCMS_MontarFormDeEdicao_WidgetAcordeon()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Acordeon");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:
    
    if(!gWidgetSendoEditado.ListaDeAbas || gWidgetSendoEditado.ListaDeAbas === undefined)
    {
        gWidgetSendoEditado.ListaDeAbas = [];
    }

    lstEdicaoWidget_Acordeon_Itens.find("tr:gt(0)").remove();

    for(var a = 0; a < gWidgetSendoEditado.ListaDeAbas.length; a++)
    {
        ModuloCMS_WidgetAcordeon_AdicionarNovaNaLista(gWidgetSendoEditado.ListaDeAbas[a]);
    }
    
    if(gWidgetSendoEditado.AtributoClass == "")
         gWidgetSendoEditado.AtributoClass = "acordeon";

    txtEdicaoWidget_Acordeon_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);

    cboEdicaoWidget_Acordeon_AtributoClass.val(gWidgetSendoEditado.AtributoClass);
}


function ModuloCMS_MontarFormDeEdicao_WidgetTitulo()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Titulo");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:
    cboEdicaoWidget_Titulo_Nivel.val(gWidgetSendoEditado.Nivel);

    txtEdicaoWidget_Titulo_Texto.val(gWidgetSendoEditado.Texto);

    txtEdicaoWidget_Titulo_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);

    cboEdicaoWidget_Titulo_AtributoClass.val(gWidgetSendoEditado.AtributoClass);
}


function ModuloCMS_MontarFormDeEdicao_WidgetTexto()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Texto");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //Foca na primeira aba:

    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("button").removeClass("Selecionado");
    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("button:eq(0)").addClass("Selecionado");

    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("div.ContainerCMS_PainelComTabs_Form").hide();
    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("div.ContainerCMS_PainelComTabs_Form:eq(0)").show();

    //preenche o form:
    txtEdicaoWidget_Texto_Texto.val(gWidgetSendoEditado.Texto);
    
    cboEdicaoWidget_Texto_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_Texto_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}


function ModuloCMS_MontarFormDeEdicao_WidgetTextoHTML()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_TextoHTML");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //Foca na primeira aba:

    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("button").removeClass("Selecionado");
    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("button:eq(0)").addClass("Selecionado");

    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("div.ContainerCMS_PainelComTabs_Form").hide();
    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("div.ContainerCMS_PainelComTabs_Form:eq(0)").show();

    if(gWidgetSendoEditado.ConteudoHTML === undefined)
    {
        gWidgetSendoEditado.ConteudoHTML = "";
    }

    //preenche o form:
    txtEdicaoWidget_TextoHTML_ConteudoHTML.val(gWidgetSendoEditado.ConteudoHTML);
    
    txtEdicaoDeHTML.htmlarea("html", gWidgetSendoEditado.ConteudoHTML);

    cboEdicaoWidget_TextoHTML_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_TextoHTML_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}

function ModuloCMS_MontarFormDeEdicao_WidgetImagem()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Imagem");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:

    txtEdicaoWidget_Imagem_AtributoSrc.val(gWidgetSendoEditado.AtributoSrc);

    txtEdicaoWidget_Imagem_AtributoAlt.val(gWidgetSendoEditado.AtributoAlt);

    txtEdicaoWidget_Imagem_LinkPara.val(gWidgetSendoEditado.LinkPara);

    if(gWidgetSendoEditado.FlagTamanhoAutomatico == true)
    {
        rdoEdicaoWidget_Imagem_Tamanho_Automatico.attr("checked", true);

        txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val( "" ).attr("disabled", "disabled");
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val(  "" ).attr("disabled", "disabled");

        chkEdicaoWidget_Imagem_HabilitarZoom.attr("checked", false).attr("disabled", "disabled");

        //já deixa o valor original do tamanho da imagem caso o usuário mude, aí já acerta o valor original como dados iniciais
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.attr("data-ValorAnterior", gElementoDoWidgetSendoEditado.width());
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.attr("data-ValorAnterior",  gElementoDoWidgetSendoEditado.height());

    }
    else
    {
        rdoEdicaoWidget_Imagem_Tamanho_Fixo.attr("checked", true);

        txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val( gWidgetSendoEditado.AtributoWidth  ).attr("disabled", null);
        txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val(  gWidgetSendoEditado.AtributoHeight ).attr("disabled", null);

        chkEdicaoWidget_Imagem_HabilitarZoom.attr("checked", gWidgetSendoEditado.FlagHabilitarZoom).attr("disabled", null);
    }

    cboEdicaoWidget_Imagem_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_Imagem_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}


function ModuloCMS_MontarFormDeEdicao_WidgetLista()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Lista");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:

    cboEdicaoWidget_Lista_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_Lista_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);

    txtEdicaoWidget_Lista_Atributos.val(gWidgetSendoEditado.Atributos);

    if(gWidgetSendoEditado.FlagListaEstatica)
    {
        lForm.find(".FormDinamica").hide();
        lForm.find(".FormEstatica").show();

        cboEdicaoWidget_Lista_TipoDeLista.val("E");

        txtEdicaoWidget_Lista_ItensEstaticos.val( gWidgetSendoEditado.Texto );
    }
    else
    {
        txtEdicaoWidget_Lista_TemplateDoItem.val( gWidgetSendoEditado.TemplateDoItem );

        cboEdicaoWidget_Lista_IdListaDinamica.html("").hide();
        //tem que zerar pra ela pegar tudo do callback se não caso algum outro item tenha sido editado antes desse, a combo está com os 
        //valores e ele vai "pegar" o primeiro valor na hora de atualizar os dados de edição

        cboEdicaoWidget_Lista_TipoDeLista.val("D");

        cboEdicaoWidget_Lista_TipoDeLista_Change(cboEdicaoWidget_Lista_TipoDeLista);

        cboEdicaoWidget_Lista_TipoDeConteudo.val( gElementoDoWidgetSendoEditado.attr("data-IdTipoConteudoDaLista") );

        cboEdicaoWidget_Lista_TipoDeConteudo_Change(cboEdicaoWidget_Lista_TipoDeConteudo);
    }

}


function ModuloCMS_MontarFormDeEdicao_WidgetTabela()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Tabela");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:
    
    txtEdicaoWidget_Tabela_Cabecalho.val( gWidgetSendoEditado.Cabecalho );

    if(gWidgetSendoEditado.FlagTabelaEstatica)
    {
        lForm.find(".FormDinamica").hide();
        lForm.find(".FormEstatica").show();

        cboEdicaoWidget_Tabela_TipoDeTabela.val("E");

        txtEdicaoWidget_Tabela_ItensEstaticos.val( gWidgetSendoEditado.Texto );

    }
    else
    {
        txtEdicaoWidget_Tabela_TemplateDaLinha.val( gWidgetSendoEditado.Texto );
        
        cboEdicaoWidget_Tabela_IdListaDinamica.html("").hide();
        //tem que zerar pra ela pegar tudo do callback se não caso algum outro item tenha sido editado antes desse, a combo está com os 
        //valores e ele vai "pegar" o primeiro valor na hora de atualizar os dados de edição

        cboEdicaoWidget_Tabela_TipoDeTabela.val("D");
        
        cboEdicaoWidget_Tabela_TipoDeTabela_Change(cboEdicaoWidget_Tabela_TipoDeTabela);

        cboEdicaoWidget_Tabela_TipoDeConteudo.val( gElementoDoWidgetSendoEditado.attr("data-IdTipoConteudoDaLista") );

        cboEdicaoWidget_Tabela_TipoDeConteudo_Change(cboEdicaoWidget_Tabela_TipoDeConteudo);
    }

    cboEdicaoWidget_Tabela_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_Tabela_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}


function ModuloCMS_MontarFormDeEdicao_WidgetRepetidor()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Repetidor");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:
    
    cboEdicaoWidget_Repetidor_IdListaDinamica.html("").hide();
    //tem que zerar pra ela pegar tudo do callback se não caso algum outro item tenha sido editado antes desse, a combo está com os 
    //valores e ele vai "pegar" o primeiro valor na hora de atualizar os dados de edição

    txtEdicaoWidget_Repetidor_TemplateDoItem.val( gWidgetSendoEditado.TemplateDoItem );

    cboEdicaoWidget_Repetidor_TipoDeConteudo.val( gElementoDoWidgetSendoEditado.attr("data-IdTipoConteudoDaLista") );

    cboEdicaoWidget_Repetidor_TipoDeConteudo_Change(cboEdicaoWidget_Repetidor_TipoDeConteudo);

    cboEdicaoWidget_Repetidor_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_Repetidor_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}



function ModuloCMS_MontarFormDeEdicao_WidgetListaDeDefinicao()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_ListaDeDefinicao");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //preenche o form:
    
    cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.html("").hide();
    //tem que zerar pra ela pegar tudo do callback se não caso algum outro item tenha sido editado antes desse, a combo está com os 
    //valores e ele vai "pegar" o primeiro valor na hora de atualizar os dados de edição

    txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem.val( gWidgetSendoEditado.TemplateDoItem );

    cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo.val( gElementoDoWidgetSendoEditado.attr("data-IdTipoConteudoDaLista") );

    cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo_Change(cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo);

    cboEdicaoWidget_ListaDeDefinicao_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_ListaDeDefinicao_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}


function ModuloCMS_MontarFormDeEdicao_WidgetEmbed()
{
    var lForm = $("#pnlEstruturaContainer_PainelEdicaoWidget_Embed");

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lForm.show();

    //Foca na primeira aba:

    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("button").removeClass("Selecionado");
    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("button:eq(0)").addClass("Selecionado");

    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("div.ContainerCMS_PainelComTabs_Form").hide();
    lForm.find("ContainerCMS_PainelComTabs_Tabs").find("div.ContainerCMS_PainelComTabs_Form:eq(0)").show();

    //preenche o form:
    txtEdicaoWidget_Embed_Codigo.val(gWidgetSendoEditado.Codigo);

    //cboEdicaoWidget_Embed_AtributoClass.val(gWidgetSendoEditado.AtributoClass);

    txtEdicaoWidget_Embed_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);
}









function ModuloCMS_AtualizarDadosDeEdicao_WidgetTitulo()
{
    gWidgetSendoEditado.Texto = txtEdicaoWidget_Titulo_Texto.val();
    gElementoDoWidgetSendoEditado.html( ModuloCMS_ProcessarTextoParaHTML(txtEdicaoWidget_Titulo_Texto.val()) );
    
    gWidgetSendoEditado.Resumo = gWidgetSendoEditado.Texto;

        if(gWidgetSendoEditado.Resumo.length > 40)
            gWidgetSendoEditado.Resumo = gWidgetSendoEditado.Resumo.substr(0, 40) + " (...)";

    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Titulo_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Titulo_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    var lNivelHeader = gElementoDoWidgetSendoEditado[0].tagName.toUpperCase().replace("H", "");

    if(lNivelHeader != cboEdicaoWidget_Titulo_Nivel.val())
    {
        var lIdOriginal = gWidgetSendoEditado.IdElemento;

        gElementoDoWidgetSendoEditado.attr("id", lIdOriginal + "-XXX");

        gWidgetSendoEditado.Nivel = cboEdicaoWidget_Titulo_Nivel.val();

        gElementoDoWidgetSendoEditado
            .after("<H" + gWidgetSendoEditado.Nivel + 
                    " id='" + lIdOriginal + "'" +
                    ((gWidgetSendoEditado.AtributoClass) ? (" class='" + gWidgetSendoEditado.AtributoClass + "'") : "") +
                    ((gWidgetSendoEditado.AtributoStyle) ? (" style='" + gWidgetSendoEditado.AtributoStyle + "'") : "") +
                    ">" + 
                    ModuloCMS_ProcessarTextoParaHTML(gWidgetSendoEditado.Texto) + 
                    "<input type='hidden' class='TextoOriginal' value='" + gWidgetSendoEditado.Texto + "'/>" +
                    "</H" + gWidgetSendoEditado.Nivel + ">");

        gElementoDoWidgetSendoEditado.remove();

        gElementoDoWidgetSendoEditado = $("#" + lIdOriginal);
    }
    
    //atualiza o "resumo":
    
    gWidgetSendoEditado.Resumo = gElementoDoWidgetSendoEditado.text();

    if(gWidgetSendoEditado.Resumo.length > 40)
        gWidgetSendoEditado.Resumo = gWidgetSendoEditado.Resumo.substr(0, 40) + " (...)";

    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html(  gWidgetSendoEditado.Resumo  );
}


function ModuloCMS_AtualizarDadosDeEdicao_WidgetTexto()
{
    gWidgetSendoEditado.Texto = txtEdicaoWidget_Texto_Texto.val();
    gElementoDoWidgetSendoEditado.html( ModuloCMS_ProcessarTextoParaHTML(txtEdicaoWidget_Texto_Texto.val(), true) );

    gWidgetSendoEditado.Resumo = gWidgetSendoEditado.Texto;

        if(gWidgetSendoEditado.Resumo.length > 40)
            gWidgetSendoEditado.Resumo = gWidgetSendoEditado.Resumo.substr(0, 40) + " (...)";

    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Texto_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Texto_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    //atualiza o "resumo":
    
    gWidgetSendoEditado.Resumo = gElementoDoWidgetSendoEditado.text();

    if(gWidgetSendoEditado.Resumo.length > 40)
        gWidgetSendoEditado.Resumo = gWidgetSendoEditado.Resumo.substr(0, 40) + " (...)";

    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html(  gWidgetSendoEditado.Resumo  );
}


function ModuloCMS_AtualizarDadosDeEdicao_WidgetTextoHTML()
{
    gWidgetSendoEditado.ConteudoHTML = $("#txtEdicaoWidget_TextoHTML_ConteudoHTML").val();
    //gElementoDoWidgetSendoEditado.html( gWidgetSendoEditado.ConteudoHTML );  //já vai no click do ok da edição de html

    gWidgetSendoEditado.Resumo = "(html)";

    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_TextoHTML_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_TextoHTML_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

}


function ModuloCMS_AtualizarDadosDeEdicao_WidgetImagem()
{
    //gWidgetSendoEditado.URL = txtEdicaoWidget_Texto_Texto.val();

    gWidgetSendoEditado.AtributoSrc = txtEdicaoWidget_Imagem_AtributoSrc.val();
    gElementoDoWidgetSendoEditado.attr("src", gWidgetSendoEditado.AtributoSrc);

    gWidgetSendoEditado.AtributoAlt = txtEdicaoWidget_Imagem_AtributoAlt.val();
    gElementoDoWidgetSendoEditado.attr("alt", gWidgetSendoEditado.AtributoAlt);
    
    gWidgetSendoEditado.Resumo = "(imagem)";

    //TODO: Observar que a classe do link não pode ter box-shadow junto com a da imagem, se não a mudança da classe da imagem "não pega" porque é o <a> que está com sombra.

    if(rdoEdicaoWidget_Imagem_Tamanho_Automatico.is(":checked"))
    {
        gWidgetSendoEditado.FlagTamanhoAutomatico = true;

        gWidgetSendoEditado.AtributoWidth = "";
        gElementoDoWidgetSendoEditado.attr("width", null);

        gWidgetSendoEditado.AtributoHeight = "";
        gElementoDoWidgetSendoEditado.attr("height", null);

        gWidgetSendoEditado.FlagHabilitarZoom = false;
        GradSite_DesabilitarZoomParaImagem(gElementoDoWidgetSendoEditado);

        gWidgetSendoEditado.LinkPara = txtEdicaoWidget_Imagem_LinkPara.val();

        if(gWidgetSendoEditado.LinkPara.indexOf("www") == 0)
        {
            gWidgetSendoEditado.LinkPara = "http://" + gWidgetSendoEditado.LinkPara;
            txtEdicaoWidget_Imagem_LinkPara.val(gWidgetSendoEditado.LinkPara);
        }

        if(gWidgetSendoEditado.LinkPara != "")
        {
            if(gElementoDoWidgetSendoEditado.parent().hasClass("LinkDeImagem"))
            {
                //se colocou o link e ja tem o <a> como parent, atualiza só o href:
                gElementoDoWidgetSendoEditado.parent().attr("href", gWidgetSendoEditado.LinkPara);
            }
            else
            {
                //cria o <a>
                var lLink = $("<a href='" + gWidgetSendoEditado.LinkPara + "' class='LinkDeImagem'></a>");

                gElementoDoWidgetSendoEditado.before(lLink);

                lLink.append(gElementoDoWidgetSendoEditado);
            }
        }
        else
        {
            //se tirou o link e ainda tem o <a> como parent, remove ele:

            if(gElementoDoWidgetSendoEditado.parent().hasClass("LinkDeImagem"))
            {
                var lClone = gElementoDoWidgetSendoEditado.clone();

                gElementoDoWidgetSendoEditado.parent().after( lClone );

                gElementoDoWidgetSendoEditado.parent().remove();
            }
        }
    }
    else
    {
        gWidgetSendoEditado.FlagTamanhoAutomatico = false;

        gWidgetSendoEditado.AtributoWidth = txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura.val();  //TODO: validacao
        gElementoDoWidgetSendoEditado.attr("width", gWidgetSendoEditado.AtributoWidth);

        gWidgetSendoEditado.AtributoHeight = txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura.val();
        gElementoDoWidgetSendoEditado.attr("height", gWidgetSendoEditado.AtributoHeight);

        gWidgetSendoEditado.FlagHabilitarZoom = chkEdicaoWidget_Imagem_HabilitarZoom.is(":checked");

        if(gWidgetSendoEditado.FlagHabilitarZoom)
        {
            gWidgetSendoEditado.LinkPara = "";
            txtEdicaoWidget_Imagem_LinkPara.val("");

            if(gElementoDoWidgetSendoEditado.parent().hasClass("LinkDeImagem"))
            {
                var lClone = gElementoDoWidgetSendoEditado.clone();

                gElementoDoWidgetSendoEditado.parent().after( lClone );

                gElementoDoWidgetSendoEditado.parent().remove();

                gElementoDoWidgetSendoEditado = $("#" + gWidgetSendoEditado.IdElemento);
            }

            GradSite_HabilitarZoomParaImagem(gElementoDoWidgetSendoEditado);
        }
        else
        {
            GradSite_DesabilitarZoomParaImagem(gElementoDoWidgetSendoEditado);
        }
    }

    //precisa disso pra quando o html mudou (com <a> e sem <a> em volta)
    gElementoDoWidgetSendoEditado = $("#" + gWidgetSendoEditado.IdElemento);

    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Imagem_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Imagem_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

}


function ModuloCMS_AtualizarDadosDeEdicao_WidgetLista()
{
    
    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Lista_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Lista_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    gWidgetSendoEditado.FlagListaEstatica = (cboEdicaoWidget_Lista_TipoDeLista.val() == "E");

    var lItens = 0;

    if(gWidgetSendoEditado.FlagListaEstatica)
    {
        var lHTML = "";
        
        var lTexto = txtEdicaoWidget_Lista_ItensEstaticos.val().split("\n");

        for(var a = 0; a < lTexto.length; a++)
        {
            if(lTexto[a] != "")
            {
                lHTML += "<li><code class='dataTexto'>" + lTexto[a] + "</code>" + ModuloCMS_ProcessarTextoParaHTML(lTexto[a], false) + "</li>";

                lItens++;
            }
        }
        
        gWidgetSendoEditado.IdDaLista = 0;
        gWidgetSendoEditado.DescricaoDaLista = "";
        gWidgetSendoEditado.TemplateDoItem = "";
        gWidgetSendoEditado.Atributos = "";
        gWidgetSendoEditado.Ordenacao = "";

        gWidgetSendoEditado.Texto = txtEdicaoWidget_Lista_ItensEstaticos.val();
        gElementoDoWidgetSendoEditado.html( lHTML );
        
        gWidgetSendoEditado.Resumo = "Lista Estática";
    }
    else
    {
        if(cboEdicaoWidget_Lista_IdListaDinamica.val() != "" && cboEdicaoWidget_Lista_IdListaDinamica.val() != null)
        {
            gWidgetSendoEditado.IdDaLista         = cboEdicaoWidget_Lista_IdListaDinamica.val();
            gWidgetSendoEditado.DescricaoDaLista  = cboEdicaoWidget_Lista_IdListaDinamica.find("option:selected").text();
            gWidgetSendoEditado.Ordenacao         = cboEdicaoWidget_Lista_Ordenacao.val();

            gWidgetSendoEditado.Resumo = "Lista " + gWidgetSendoEditado.DescricaoDaLista;
        }

        gWidgetSendoEditado.TemplateDoItem = txtEdicaoWidget_Lista_TemplateDoItem.val();
        gWidgetSendoEditado.Atributos      = txtEdicaoWidget_Lista_Atributos.val();

        gElementoDoWidgetSendoEditado.find("code.dataTemplateDoItem").html(  gWidgetSendoEditado.TemplateDoItem );
        gElementoDoWidgetSendoEditado.find("code.dataAtributosDoItem").html( gWidgetSendoEditado.Atributos );

        if(gDadosDaListaDinamicaSelecionada != null)
        {
            var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON( cboEdicaoWidget_Lista_TipoDeConteudo.find("option:selected").attr("data-TipoDeConteudoJSON") );

            var lHTML = "";

            var lTexto = gWidgetSendoEditado.TemplateDoItem;
            var lAtributos = gWidgetSendoEditado.Atributos;

            var lValorPropriedade = "";
            var lValorDoAtributo = "";

            var lEval;

            for(var a = 0; a < gDadosDaListaDinamicaSelecionada.length; a++)
            {
                for(var b = 0; b < lPropriedades.length; b++)
                {
                    if(lPropriedades[b].toLowerCase() == "conteudohtml")
                    {
                        lTexto = lTexto.replace("$[" + lPropriedades[b] + "]", "(conteúdo HTML)" );
                    }
                    else
                    {
                        lEval = "lValorPropriedade = gDadosDaListaDinamicaSelecionada[a]." + lPropriedades[b] + ";"

                        eval(lEval);

                        lTexto = lTexto.replace("$[" + lPropriedades[b] + "]", lValorPropriedade );
                    }
                }

                for(var b = 0; b < lPropriedades.length; b++)
                {
                    if(lPropriedades[b].toLowerCase() != "conteudohtml")
                    {
                        lEval = "lValorDoAtributo = gDadosDaListaDinamicaSelecionada[a]." + lPropriedades[b] + ";"

                        eval(lEval);

                        lAtributos = lAtributos.replace("$[" + lPropriedades[b] + "]", lValorDoAtributo );
                    }
                }

                if(lTexto != "")
                {
                    lHTML += "<li " + lAtributos + "><code class='dataTexto'>" + lTexto + "</code>" + ModuloCMS_ProcessarTextoParaHTML(lTexto, false) + "</li>";

                    lTexto = gWidgetSendoEditado.TemplateDoItem;

                    lItens++;
                }
            }

            gElementoDoWidgetSendoEditado.html( lHTML );
        }

        gWidgetSendoEditado.Texto = "";
    }
    

    //atualiza o "resumo":
    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html( gWidgetSendoEditado.Resumo );
}


function ModuloCMS_AtualizarDadosDeEdicao_WidgetTabela()
{
    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Tabela_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Tabela_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    gWidgetSendoEditado.FlagTabelaEstatica = (cboEdicaoWidget_Tabela_TipoDeTabela.val() == "E");
    
    gWidgetSendoEditado.Cabecalho =  txtEdicaoWidget_Tabela_Cabecalho.val();

    if(gWidgetSendoEditado.FlagTabelaEstatica)
    {
        var lHTML = "";

        var lCabecalho = gWidgetSendoEditado.Cabecalho.split("|");

        var lValores;

        var lValorCelula, lAtributoStyle, lAtributoClass, lContador;

        var lQuantidadeDeColunas;

        lHTML = "<tr>";

        lQuantidadeDeColunas = 0;

        for(var a = 0; a < lCabecalho.length; a++)
        {
            lValorCelula = lCabecalho[a].trim();

            lQuantidadeDeColunas ++;

            lContador = 1;  //começa no 1 porque colspan="1" não adianta, se tiver > tem que colocar colspan="2" e assim por diante
            lAtributoStyle = "";
            lAtributoClass = "";

            //se começar com > então é colspan, conta quantas colunas tem conforme a qtd do colspan
            while(lValorCelula.charAt(0) == ">")
            {
                lContador ++;
                lQuantidadeDeColunas ++;

                lValorCelula = lValorCelula.substr(1);
            }

            if(lValorCelula.indexOf("style:") != -1)
            {
                //tem que ser (style:___valores____) obrigatoriamente no final da string
                lAtributoStyle = lValorCelula.substr(lValorCelula.indexOf("style:") - 1, lValorCelula.length);

                lValorCelula = lValorCelula.replace(lAtributoStyle, "");

                lAtributoStyle = lAtributoStyle.substr(lAtributoStyle.indexOf("style:") + 6, lAtributoStyle.length - 8);
            }
            else if(lValorCelula.indexOf("class:") != -1)
            {
                //ou style ou class, não ambos...
                lAtributoClass = lValorCelula.substr(lValorCelula.indexOf("class:") - 1, lValorCelula.length);

                lValorCelula = lValorCelula.replace(lAtributoClass, "");

                lAtributoClass = lAtributoClass.substr(lAtributoClass.indexOf("class:") + 6, lAtributoClass.length - 8);
            }

            if(lValorCelula == "__") lValorCelula = "&nbsp;";       //dois underscores é uma célula em branco

            lHTML += "<td" + 
                     ((lContador > 1) ?        " colspan='" + lContador      + "'" : "") + 
                     ((lAtributoStyle != "") ? " style='"   + lAtributoStyle + "'" : "") + 
                     ((lAtributoClass != "") ? " class='"   + lAtributoClass + "'" : "") + 
                     ">" + 
                     ModuloCMS_ProcessarTextoParaHTML(lValorCelula) + 
                     "</td>";
        }

        lHTML += "</tr>";

        gElementoDoWidgetSendoEditado.find("thead").html(lHTML);

        var lTexto = txtEdicaoWidget_Tabela_ItensEstaticos.val().split("\n");
        
        lHTML = "";

        for(var a = 0; a < lTexto.length; a++)
        {
            if(lTexto[a] != "")
            {
                lValores = lTexto[a].split("|");

                lHTML += "<tr>";

                for(var b = 0; b < lQuantidadeDeColunas; b++)
                {
                    if(b < lValores.length)
                    {
                        lValorCelula = lValores[b].trim();

                        lContador = 1;  //começa no 1 porque colspan="1" não adianta, se tiver > tem que colocar colspan="2" e assim por diante
                        lAtributoStyle = "";
                        lAtributoClass = "";

                        //se começar com > então é colspan, conta quantas colunas tem conforme a qtd do colspan
                        while(lValorCelula.charAt(0) == ">")
                        {
                            lContador ++;
                            b++;
                            lValorCelula = lValorCelula.substr(1);
                        }

                        if(lValorCelula.indexOf("style:") != -1)
                        {
                            //tem que ser (style:___valores____) obrigatoriamente no final da string
                            lAtributoStyle = lValorCelula.substr(lValorCelula.indexOf("style:") - 1, lValorCelula.length);

                            lValorCelula = lValorCelula.replace(lAtributoStyle, "");

                            lAtributoStyle = lAtributoStyle.substr(lAtributoStyle.indexOf("style:") + 6, lAtributoStyle.length - 8);
                        }
                        else if(lValorCelula.indexOf("class:") != -1)
                        {
                            //ou style ou class, não ambos...
                            lAtributoClass = lValorCelula.substr(lValorCelula.indexOf("class:") - 1, lValorCelula.length);

                            lValorCelula = lValorCelula.replace(lAtributoClass, "");

                            lAtributoClass = lAtributoClass.substr(lAtributoClass.indexOf("class:") + 6, lAtributoClass.length - 8);
                        }

                        if(lValorCelula == "__") lValorCelula = "&nbsp;";       //dois underscores é uma célula em branco

                        lHTML += "<td" + 
                                 ((lContador > 1) ?        " colspan='" + lContador      + "'" : "") +
                                 ((lAtributoStyle != "") ? " style='"   + lAtributoStyle + "'" : "") +
                                 ((lAtributoClass != "") ? " class='"   + lAtributoClass + "'" : "") +
                                 ">" + 
                                 ModuloCMS_ProcessarTextoParaHTML(lValorCelula) + 
                                 "</td>";
                    }
                    else
                    {
                        lHTML += "<td>&nbsp;</td>";
                    }
                }

                lHTML += "</tr>";
            }
        }

        gWidgetSendoEditado.Texto = txtEdicaoWidget_Tabela_ItensEstaticos.val();
        gElementoDoWidgetSendoEditado.find("tbody").html( lHTML );
        
        gWidgetSendoEditado.DescricaoDaLista = "Estática";
        gWidgetSendoEditado.Resumo = "Lista Estática";
    }
    else
    {
        if(cboEdicaoWidget_Tabela_IdListaDinamica.val() != "" && cboEdicaoWidget_Tabela_IdListaDinamica.val() != null)
        {
            gWidgetSendoEditado.IdDaLista         = cboEdicaoWidget_Tabela_IdListaDinamica.val();
            gWidgetSendoEditado.DescricaoDaLista  = cboEdicaoWidget_Tabela_IdListaDinamica.find("option:selected").text();
            gWidgetSendoEditado.Ordenacao         = cboEdicaoWidget_Tabela_Ordenacao.val();

            gWidgetSendoEditado.Resumo = "Lista " + gWidgetSendoEditado.DescricaoDaLista;
        }

        gWidgetSendoEditado.Texto = txtEdicaoWidget_Tabela_TemplateDaLinha.val();

        if(gDadosDaListaDinamicaSelecionada != null)
        {
            //var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON( cboEdicaoWidget_Tabela_TipoDeConteudo.find("option:selected").attr("data-TipoDeConteudoJSON") );

            var lHTML = "";

            var lTexto;

            var lPropriedade;
            var lValorPropriedade = "";

            var lColunas, lTextoDaColuna;

            var lRegexPropriedades = /\$\[.*?\]/gi;

            lColunas = gWidgetSendoEditado.Cabecalho.split("|");

            lHTML += "<tr>";

            for(var a = 0; a < lColunas.length; a++)
            {
                lHTML += "<td>" + lColunas[a].trim() + "</td>";
            }

            lHTML += "</tr>";

            gElementoDoWidgetSendoEditado.find("thead").html(lHTML);

            lHTML = "";

            lTexto = gWidgetSendoEditado.Texto;

            lColunas = lTexto.split("|");

            for(var a = 0; a < gDadosDaListaDinamicaSelecionada.length; a++)
            {
                lHTML += "<tr>";

                for(var b = 0; b < lColunas.length; b++)
                {
                    lValorPropriedade = null;

                    lTextoDaColuna = lColunas[b].trim();

                    lPropriedade = lRegexPropriedades.exec( lTextoDaColuna );

                    while(lPropriedade != null)
                    {
                        lPropriedade = lPropriedade[0].substr(2, lPropriedade[0].length - 3);

                        if(lPropriedade != "")
                        {
                            eval("lValorPropriedade = gDadosDaListaDinamicaSelecionada[a]." + lPropriedade + ";");

                            if(lValorPropriedade != null)
                            {
                                lTextoDaColuna = lTextoDaColuna.replace("$[" + lPropriedade + "]", lValorPropriedade);
                            }
                        }

                        lPropriedade = lRegexPropriedades.exec( lTextoDaColuna );
                    }

                    lHTML += "<td>" + ModuloCMS_ProcessarTextoParaHTML(lTextoDaColuna) + "</td>";
                }

                lHTML += "</tr>";
            }
            
            gElementoDoWidgetSendoEditado.find("tbody").html(lHTML);
        }
    }
    
    //atualiza o "resumo":
    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html( gWidgetSendoEditado.Resumo );
}




function ModuloCMS_AtualizarDadosDeEdicao_WidgetRepetidor()
{
    
    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Repetidor_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Repetidor_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    gWidgetSendoEditado.TemplateDoItem = txtEdicaoWidget_Repetidor_TemplateDoItem.val();
    
    if(cboEdicaoWidget_Repetidor_IdListaDinamica.val() != "" && cboEdicaoWidget_Repetidor_IdListaDinamica.val() != null)
    {
        gWidgetSendoEditado.IdDaLista         = cboEdicaoWidget_Repetidor_IdListaDinamica.val();
        gWidgetSendoEditado.DescricaoDaLista  = cboEdicaoWidget_Repetidor_IdListaDinamica.find("option:selected").text();
        gWidgetSendoEditado.Ordenacao         = cboEdicaoWidget_Repetidor_Ordenacao.val();

        gWidgetSendoEditado.Resumo = "Lista " + gWidgetSendoEditado.DescricaoDaLista;
    }

    if(gDadosDaListaDinamicaSelecionada != null)
    {
        var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON( cboEdicaoWidget_Repetidor_TipoDeConteudo.find("option:selected").attr("data-TipoDeConteudoJSON") );

        var lHTML = "<code class='TemplateDoItem'>" + gWidgetSendoEditado.TemplateDoItem + "</code>";

        var lTemplate;
            
        var lValorPropriedade = "";

        for(var a = 0; a < gDadosDaListaDinamicaSelecionada.length; a++)
        {
            lTemplate = gWidgetSendoEditado.TemplateDoItem;

            for(var b = 0; b < lPropriedades.length; b++)
            {
                eval("lValorPropriedade = gDadosDaListaDinamicaSelecionada[a]." + lPropriedades[b] + ";");

                lTemplate = lTemplate.replace("$[" + lPropriedades[b] + "]", lValorPropriedade );
            }

            lHTML += lTemplate;
        }

        gElementoDoWidgetSendoEditado.html( lHTML );
    }

    //atualiza o "resumo":
    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html( gWidgetSendoEditado.Resumo );
}





function ModuloCMS_AtualizarDadosDeEdicao_WidgetListaDeDefinicao()
{
    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_ListaDeDefinicao_AtributoClass.val();
    gElementoDoWidgetSendoEditado.attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_ListaDeDefinicao_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    gWidgetSendoEditado.TemplateDoItem = txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem.val();
    
    if(cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.val() != "" && cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.val() != null)
    {
        gWidgetSendoEditado.IdDaLista         = cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.val();
        gWidgetSendoEditado.DescricaoDaLista  = cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica.find("option:selected").text();
        gWidgetSendoEditado.Ordenacao         = cboEdicaoWidget_ListaDeDefinicao_Ordenacao.val();

        gWidgetSendoEditado.Resumo = "Lista " + gWidgetSendoEditado.DescricaoDaLista;
    }

    if(gDadosDaListaDinamicaSelecionada != null)
    {
        var lPropriedades = GradSite_BuscarListaDePropriedadesAPartirDeJSON( cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo.find("option:selected").attr("data-TipoDeConteudoJSON") );

        var lHTML = "";

        var lTemplate, lTexto;

        var lValorPropriedade = "";

        for(var a = 0; a < gDadosDaListaDinamicaSelecionada.length; a++)
        {
            lTemplate = gWidgetSendoEditado.TemplateDoItem.split("|");
            
            for(var b = 0; b < lTemplate.length; b++)
            {
                lTexto = lTemplate[b];

                for(var c = 0; c < lPropriedades.length; c++)
                {
                    eval("lValorPropriedade = gDadosDaListaDinamicaSelecionada[a]." + lPropriedades[c] + ";");

                    lTemplate[b] = lTemplate[b].replace("$[" + lPropriedades[c] + "]", lValorPropriedade );
                }

                if(b == 0)
                {
                    lHTML += "<dt data-Texto='" + lTexto + "'>" + ModuloCMS_ProcessarTextoParaHTML(lTemplate[b], false) + "</dt>";
                }
                else
                {
                    lHTML += "<dd data-Texto='" + lTexto + "'>" + ModuloCMS_ProcessarTextoParaHTML(lTemplate[b], false) + "</dd>";
                }
            }

        }

        gElementoDoWidgetSendoEditado.html( lHTML );
    }

    if(gWidgetSendoEditado.AtributoClass == "ItensExpansiveis" || gWidgetSendoEditado.AtributoClass == "Acordeao")
    {
        gElementoDoWidgetSendoEditado
            .find("dt")
                .bind("click", Widget_ListaDeDefinicao_DT_Click);

        gElementoDoWidgetSendoEditado
            .find("dd")
                .hide();
    }

    //atualiza o "resumo":
    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html( gWidgetSendoEditado.Resumo );
}



function ModuloCMS_AtualizarDadosDeEdicao_WidgetEmbed()
{
    gWidgetSendoEditado.Codigo = txtEdicaoWidget_Embed_Codigo.val();

    gElementoDoWidgetSendoEditado.html( txtEdicaoWidget_Embed_Codigo.val() );

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Embed_AtributoStyle.val();

    gElementoDoWidgetSendoEditado.attr("style", gWidgetSendoEditado.AtributoStyle);

    //atualiza o "resumo":
    //lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] span:eq(1)").html(  gWidgetSendoEditado.Resumo  );
}

function ModuloCMS_AtualizarDadosDeEdicao_WidgetAbas()
{
    gWidgetSendoEditado.ListaDeAbas = [];

    var lTR, lLI, lDIV;

    var lIdConteudo, lTitulo, lTipo, lClasse, lURL;

    lstEdicaoWidget_Abas_Itens.find("tr[class!='NenhumItem']").each(function()
    {
        var lAtivo = false;

        lTR = $(this);

        lIdConteudo = lTR.attr("data-IdConteudo");
        lTitulo     = lTR.attr("data-Titulo");
        lURL        = lTR.attr("data-URL");
        lTipo       = lTR.attr("data-TipoLink");

        lClasse = "";

        gWidgetSendoEditado.ListaDeAbas.push( { Titulo: lTitulo, IdConteudo: lIdConteudo, TipoLink: lTipo, URL: lURL } );

        lLI = gElementoDoWidgetSendoEditado.find(" > ul li[data-IdConteudo='" + lIdConteudo + "']");

        lDIV = $("div[data-IdConteudo='" + lIdConteudo + "']");

        if(gElementoDoWidgetSendoEditado.find(" > ul > li.ativo").length == 0)
        {
            lClasse = "class='ativo'";

            lAtivo = true;
        }

        if(lLI.length == 0)
        {
            lLI = "<li data-IdConteudo='" + lIdConteudo + "'  data-Url='" + lURL + "' data-TipoLink='" + lTipo + "' " + lClasse + "> <a id='Aba-" + GradSite_ExtirparAcentuacaoEEspaco(lTitulo) + "'  href='#'>" + lTitulo + "</a> </li>";

            lClasse = (lTipo == "Embutida") ? "FaltaCarregar " : "";

            lDIV = "<div data-IdConteudo='" + lIdConteudo + "' class='" + lClasse + "' style='display:none'>&nbsp;</div>";

            gElementoDoWidgetSendoEditado.find(" > ul").append(lLI);

            gElementoDoWidgetSendoEditado.find("button.btnVisitarPaginaConteudo").after(lDIV);

            if(lAtivo)
            {
                GradSite_CarregarHtmlDaAba(gElementoDoWidgetSendoEditado.parent().find("div.FaltaCarregar"));
            }
        }
        else
        {
            lLI.attr("data-IdConteudo", lIdConteudo);

            lDIV.attr("data-IdConteudo", lIdConteudo);

            lLI.find("a").html(lTitulo);
        }
    });

    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Abas_AtributoClass.val();
    gElementoDoWidgetSendoEditado.find(" > ul").attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Abas_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.find(" > ul").attr("style", gWidgetSendoEditado.AtributoStyle);
}


function ModuloCMS_AtualizarDadosDeEdicao_WidgetAcordeon()
{
    gWidgetSendoEditado.ListaDeAbas = [];

    var lTR, lLI, lDIV;

    var lIdConteudo, lTitulo, lTipo, lClasse, lURL;

    lstEdicaoWidget_Acordeon_Itens.find("tr[class!='NenhumItem']").each(function()
    {
        var lAtivo = false;

        lTR = $(this);

        lIdConteudo = lTR.attr("data-IdConteudo");
        lTitulo     = lTR.attr("data-Titulo");
        lURL        = lTR.attr("data-URL");
        lTipo       = lTR.attr("data-TipoLink");

        lClasse = "";

        gWidgetSendoEditado.ListaDeAbas.push( { Titulo: lTitulo, IdConteudo: lIdConteudo, TipoLink: lTipo, URL: lURL } );

        lLI = gElementoDoWidgetSendoEditado.find(" > ul li[data-IdConteudo='" + lIdConteudo + "']");

        lDIV = $("div[data-IdConteudo='" + lIdConteudo + "']");

        if(gElementoDoWidgetSendoEditado.find(" > li.ativo").length == 0)
        {
            lClasse = "ativo";

            lAtivo = true;
        }

        if(lLI.length == 0)
        {
            lLI = "<li onclick='return lnkAcordeon_Conteudo_Click(this, event)' data-IdConteudo='" + lIdConteudo + "'  data-Url='" + lURL + "' data-TipoLink='" + lTipo + "' class='" + lClasse + "'> <button title='Visitar página' onclick='return btnWidAcordeon_VisitarPaginaConteudo_Click(this)' class='btnVisitarPaginaConteudo'></button> <div class='acordeon-opcao'>" + lTitulo + "</div> </li>";

            lClasse = (lTipo == "Embutida") ? "FaltaCarregar " : "";

            lDIV = "<div data-IdConteudo='" + lIdConteudo + "' class='acordeon-conteudo " + lClasse + "' style='display:none'>&nbsp;</div>";

            gElementoDoWidgetSendoEditado.find(" > ul").append(lLI);
             
            gElementoDoWidgetSendoEditado.find("> ul > li:last > div.acordeon-opcao").after(lDIV);

            if(lAtivo)
            {
                GradSite_CarregarHtmlDaAba(gElementoDoWidgetSendoEditado.parent().find("div.FaltaCarregar"));
            }
        }
        else
        {
            lLI.attr("data-IdConteudo", lIdConteudo);

            lDIV.attr("data-IdConteudo", lIdConteudo);

            lLI.find("> div.acordeon-opcao").html(lTitulo);
        }
    });

    gWidgetSendoEditado.AtributoClass = cboEdicaoWidget_Acordeon_AtributoClass.val();
    gElementoDoWidgetSendoEditado.find(" > ul").attr("class", gWidgetSendoEditado.AtributoClass);

    gWidgetSendoEditado.AtributoStyle = txtEdicaoWidget_Acordeon_AtributoStyle.val();
    gElementoDoWidgetSendoEditado.find(" > ul").attr("style", gWidgetSendoEditado.AtributoStyle);
}




function ModuloCMS_SalvarWidgetSendoEditado()
{
    if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TITULO)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetTitulo();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TEXTO)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetTexto();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TEXTOHTML)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetTextoHTML();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_IMAGEM)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetImagem();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_LISTA)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetLista();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_TABELA)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetTabela();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_REPETIDOR)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetRepetidor();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_LISTA_DE_DEFINICAO)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetListaDeDefinicao();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_EMBED)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetEmbed();
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_ABAS)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetAbas();

        gWidgetSendoEditado.ListaDeAbas = $.toJSON(gWidgetSendoEditado.ListaDeAbas);
    }
    else if(gWidgetSendoEditado.Tipo == CONST_WID_TIPO_ACORDEON)
    {
        ModuloCMS_AtualizarDadosDeEdicao_WidgetAcordeon();

        gWidgetSendoEditado.ListaDeAbas = $.toJSON(gWidgetSendoEditado.ListaDeAbas);
    }

    gWidgetSendoEditado.Acao = "IncluirAtualizarWidget";
    gWidgetSendoEditado.IdDaPagina = gEstruturaDaPagina.IdDaPagina

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), gWidgetSendoEditado, ModuloCMS_SalvarWidgetSendoEditado_CallBack);
}


function ModuloCMS_SalvarWidgetSendoEditado_CallBack(pResposta)
{
    var lResposta = pResposta.Mensagem;

    if(gWidgetSendoEditado.FlagNovoWidget == true)
    {
        var lID = pResposta.ObjetoDeRetorno;

        var lLI = lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "']");

        //troca o ID no list item:
        lLI.attr("data-IdDoWidget", lID);

        //tira a flag que é novo:
        lLI.attr("data-FlagNovoWidget", "false");

        //muda o ID do elemento: 
        var lIdElemento = gElementoDoWidgetSendoEditado.attr("id");

        if(lIdElemento.indexOf("-") != -1)
        {
            while( (lIdElemento.charAt(lIdElemento.length - 1) != "-") && lIdElemento.length > 0)
            {
                lIdElemento = lIdElemento.substr(0, lIdElemento.length - 1);
            }

            lIdElemento += lID;

            gElementoDoWidgetSendoEditado.attr("id", lIdElemento);

            gElementoDoWidgetSendoEditado = $("#" + lIdElemento);

            lLI.attr("data-IdElemento", lIdElemento);

            gWidgetSendoEditado.IdElemento = lIdElemento;
        }

        gWidgetSendoEditado.IdDoWidget = lID;

        lResposta = lResposta.substr(lResposta.indexOf("]") + 1);
    }

    gWidgetSendoEditado.FlagNovoWidget = false;

    gWidgetJsonAntesDeEditar = $.toJSON( gWidgetSendoEditado );

    lstEstruturaDaPagina.find("li[data-IdDoWidget='" + gWidgetSendoEditado.IdDoWidget + "'] label span:eq(1)").html( gWidgetSendoEditado.Resumo );

    GradSite_ExibirMensagem("I", pResposta.Mensagem);
}


function ModuloCMS_VerificarModificacoesParaSalvar()
{
    if(gWidgetJsonAntesDeEditar != null && gWidgetSendoEditado != null)
    {
        if($.toJSON(gWidgetSendoEditado) != gWidgetJsonAntesDeEditar)
        {
            if(confirm("ATENÇÃO!\r\n\r\nExistem modificações feitas nesse widget. Deseja SALVAR os dados antes de sair?"))
            {
                ModuloCMS_SalvarWidgetSendoEditado();
            }
        }
    }
}


function ModuloCMS_CancelarEdicaoDeWidget()
{
    ModuloCMS_VerificarModificacoesParaSalvar();

    //TODO: tem um bug aqui que se cancelar as alterações o conteúdo da página já foi modificado localmente; 
    //precisaria re-renderizar com os dados anteriores ao salvamento

    pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    var lLI = lstEstruturaDaPagina.find("li.Selecionado");

    if(lLI.attr("data-flagnovowidget") == "true")
    {
        lLI.remove();

        $("#" + gWidgetSendoEditado.IdElemento).remove();
    }
    else
    {
        //estava editando
        lLI.removeClass("Selecionado");
    }

    gWidgetJsonAntesDeEditar = null;
    gWidgetSendoEditado = null;

}


function ModuloCMS_ExcluirWidget(pID, pFlagNovo)
{
    if(pFlagNovo)
    {
        ModuloCMS_ExcluirWidget_CallBack( { Mensagem: pID } );
    }
    else
    {
        var lRequest = { Acao: "ExcluirWidget", IDdoWidget: pID, IdDaPagina: gEstruturaDaPagina.IdDaPagina, IdDaEstrutura: $("#hidIdDaEstrutura").val() };

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_ExcluirWidget_CallBack);
    }
}


function ModuloCMS_ExcluirWidget_CallBack(pResposta)
{
    var lID = pResposta.Mensagem;

    var lLI = lstEstruturaDaPagina.find("li[data-IdDoWidget='" + lID + "']");

    var lElemento = $("#" + lLI.attr("data-IdElemento"));

    var lIndice;

    if(gWidgetSendoEditado != null && gWidgetSendoEditado.IdDoWidget == lID)
    {
        //o que foi excluido é o que está sendo editado

        pnlEstruturaContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();
    }

    for(var a = 0; a < gEstruturaDaPagina.Widgets.length; a++)
    {
        if(gEstruturaDaPagina.Widgets[a].IdDoWidget == lID)
        {
            lIndice = a;

            break;
        }
    }

    //atualiza a ordemnapagina dos que sobraram:
    if(lIndice < (gEstruturaDaPagina.Widgets.length - 1))
    {
        for(var a = lIndice + 1; a < gEstruturaDaPagina.Widgets.length; a++)
        {
            gEstruturaDaPagina.Widgets[a].OrdemNaPagina--;
        }
    }

    gEstruturaDaPagina.Widgets.splice(lIndice, 1);

    if(lElemento.hasClass("menu-tabs"))
    {
        lElemento.next("button.btnVisitarPaginaConteudo").remove();

        var lAbas = $.evalJSON(lElemento.attr("data-listadeabas"));

        for(var a = 0; a < lAbas.length; a++)
        {
            $("div[data-IdConteudo='" + lAbas[a].IdConteudo + "']").remove();
        }
    }

    lElemento.remove();

    lLI.remove();
}


function ModuloCMS_MoverWidget(pID, pFlagDirecao_m1_1)
{
    var lLI = lstEstruturaDaPagina.find("li[data-IdDoWidget='" + pID + "']");

    var lElemento = $("#" + lLI.attr("data-IdElemento"));

    var lProximo;

    var lIndice;

    if(lLI.index() == 0 && pFlagDirecao_m1_1 == -1) return;

    if(lLI.index() == (lLI.parent().find("li").length - 1) && pFlagDirecao_m1_1 == 1) return;

    for(var a = 0; a < gEstruturaDaPagina.Widgets.length; a++)
    {
        if(gEstruturaDaPagina.Widgets[a].IdDoWidget == pID)
        {
            lIndice = a;

            break;
        }
    }

    gEstruturaDaPagina.Widgets[lIndice].OrdemNaPagina += pFlagDirecao_m1_1;

    //bug do widget de imagem: imagens com link ou zoom têm um container pai, precisa subir pro pai antes de mover

    if((lElemento.attr("id").indexOf("widImagem") != -1) && (lElemento.parent().hasClass("ContainerDeZoom") || lElemento.parent().hasClass("LinkDeImagem")))
    {
        lElemento = lElemento.parent();
    }

    if(pFlagDirecao_m1_1 == -1)
    {
        //"sobe" 1 (diminui o indice em 1)
        gEstruturaDaPagina.Widgets.splice((lIndice - 1), 0, gEstruturaDaPagina.Widgets[lIndice]);

        gEstruturaDaPagina.Widgets.splice((lIndice + 1), 1);

        lLI.after(lLI.parent().find("li:eq(" + (lIndice - 1) + ")"));

        lProximo = lElemento.prev();

        lElemento.after(lProximo);
    }
    else
    {
        //"desce" 1 (aumenta o indice em 1)

        var lItem = gEstruturaDaPagina.Widgets.splice((lIndice), 1);

        gEstruturaDaPagina.Widgets.splice((lIndice + 1), 0, lItem[0]);

        lLI.before(lLI.parent().find("li:eq(" + (lIndice + 1) + ")"));

        lProximo = lElemento.next();

        lElemento.before(lProximo);
    }

    var lRequest = { Acao: "MoverWidget", IdDaPagina: $("#hidIdDaPagina").val(), IdDaEstrutura: $("#hidIdDaEstrutura").val(), OrdemDeWidgets: "", IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    lstEstruturaDaPagina.children().each(function()
    {
        lRequest.OrdemDeWidgets += $(this).attr("data-IdDoWidget") + ",";
    });

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_MoverWidget_CallBack);
}

function ModuloCMS_MoverWidget_CallBack(pResposta)
{
    //nem precisa dar mensagem...
}



function btnCMS_Estrutura_AdicionarPagina_Click(pSender, pManter)
{
    $("#lstPaginas li.Selecionado").removeClass("Selecionado");

    $("#pnlEdicaoDePaginas_Dados_Manter").hide();

    $("#pnlEdicaoDePaginas_Dados_Versao").show();

    ModuloCMS_ZerarFormDeEdicaoDaPagina(false);

    if(!pManter)
    {
        ModuloCMS_ConfigurarEdicaoDePaginaPeloDiretorio(false);
    }

    pnlPaginasContainer_PainelEdicao.show();

    txtEdicaoPagina_Titulo.focus();

    return false;
}

function ModuloCMS_ConfigurarEdicaoDePaginaPeloDiretorio(pEditandoPeloDiretorio)
{
    if(pEditandoPeloDiretorio)
    {
        $("#pnlEdicaoDePaginas_Dados").addClass("PreConfigurado").show();

        var lLargura = (477 - $("#lblEdicaoPagina_Url").width());

        $("#txtEdicaoPagina_Url").attr("disabled", "disabled").css( { width: lLargura } );

    }
    else
    {
        $("#pnlEdicaoDePaginas_Dados").removeClass("PreConfigurado");

        $("#txtEdicaoPagina_Url").attr("disabled", null).css( { width: "75%" } );

    }
}

function ModuloCMS_ZerarFormDeEdicaoDaPagina(pEsconderTambem)
{
    hidEdicaoPagina_IdDaPagina.val("");
    txtEdicaoPagina_Titulo.val("");
    txtEdicaoPagina_Url.val("");
    cboEdicaoPagina_Modo.val("F");

    if(pEsconderTambem)
    {
        pnlPaginasContainer_PainelEdicao.hide();
    }
}

function ModuloCMS_MontarFormDeEdicaoDaPagina(pItemSelecionado)
{
    ModuloCMS_VerificarModificacoesParaSalvar();

    if(!pItemSelecionado.hasClass("Selecionado"))
    {
        var lIdDoWidget, lIndiceDoWidget;

        $("#lstPaginas").find(".Selecionado").removeClass("Selecionado");

        pItemSelecionado.addClass("Selecionado");

        ModuloCMS_ConfigurarEdicaoDePaginaPeloDiretorio(false);

        hidEdicaoPagina_IdDaPagina.val( pItemSelecionado.attr("data-IdPagina")       );
        txtEdicaoPagina_Titulo.val(     pItemSelecionado.attr("data-Titulo")         );
        txtEdicaoPagina_Url.val(        pItemSelecionado.attr("data-Url")            );
        cboEdicaoPagina_Modo.val(       pItemSelecionado.attr("data-TipoEstrutura")  );

        $("#pnlEdicaoDePaginas_Dados_Versao").hide();   //na edição da página a versão não importa, edita-se versões pela parte "info" do CMS

        pnlPaginasContainer_PainelEdicao.show();

        //preenche o form:
        //cboEdicaoWidget_Titulo_Nivel.val(gWidgetSendoEditado.Nivel);

        //txtEdicaoWidget_Titulo_Texto.val(gWidgetSendoEditado.Texto);

        //txtEdicaoWidget_Titulo_AtributoStyle.val(gWidgetSendoEditado.AtributoStyle);

        //cboEdicaoWidget_Titulo_AtributoClass.val(gWidgetSendoEditado.AtributoClass);
    }
}

function lstPaginas_btnVisitar_Click(pSender)
{
    var lURL = $(pSender).closest("li").attr("data-URL");

    if(lURL != "")
    {
        if(lURL.charAt(0) != "/")
        {
            lURL = "/" + lURL;
        }

        document.location = GradSite_BuscarRaiz(lURL);
    }
    else
    {
        alert("URL não encontrada!");
    }

    return false;
}

var gDadosNovaPagina;

function btnEdicaoPagina_Salvar_Click(pSender)
{
    gDadosNovaPagina = {
                      Acao:            "SalvarPagina"
                    , IdPagina:        hidEdicaoPagina_IdDaPagina.val()
                    , Titulo:          txtEdicaoPagina_Titulo.val()
                    , Url:             txtEdicaoPagina_Url.val()
                    , TipoDaEstrutura: cboEdicaoPagina_Modo.val()
                    , Versao:          $("#cboEdicaoPagina_Versao").val()
                    , Manter:          $("#cboEdicaoDePaginas_Dados_Manter").val()
                 };

    if(txtEdicaoPagina_Url.attr("disabled") != null)
    {
        gDadosNovaPagina.Url = $("#lblEdicaoPagina_Url").text() + gDadosNovaPagina.Url;
    }

    if(gDadosNovaPagina.Url.indexOf("/") == 0)
    {
        gDadosNovaPagina.Url = gDadosNovaPagina.Url.substr(1);

        txtEdicaoPagina_Url.val(gDadosNovaPagina.Url);
    }

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), gDadosNovaPagina, ModuloCMS_SalvarPagina_CallBack);

    return false;
}

function ModuloCMS_SalvarPagina_CallBack(pResposta)
{
    if(pResposta.Mensagem == "url_ja_existe")
    {
        GradSite_ExibirMensagem("E", "Já existe uma página com essa URL, favor escolher outra.");
    }
    else
    {
        if(hidEdicaoPagina_IdDaPagina.val() == "")
        {
            alert("Nova página criada, redirecionando...");
            //nova página, redireciona pra ela direto

            document.location = GradSite_BuscarRaiz("/" + gDadosNovaPagina.Url);
        }
        else
        {
            GradSite_ExibirMensagem("I", "Página salvada com sucesso!", false);
        }
    }
}

function btnEdicaoWidget_Abas_CancelarNovaPagina_Click(pSender)
{
    $("#pnlEstruturaContainer_PainelEdicaoWidget_Abas_NovaPagina").hide();

    cboEdicaoWidget_Abas_NovoItem_Conteudo.focus();

    return false;
}

var gDadosNovaPagina = null;

function cboEdicaoWidget_Abas_NovaPagina_Modo_Change(pSender)
{
    pSender = $(pSender);

    if(pSender.val() == "F" && hidEdicaoPagina_IdDaPagina.val() != "")
    {
        $("#pnlEdicaoDePaginas_Dados_Manter").show();
    }
    else
    {
        $("#pnlEdicaoDePaginas_Dados_Manter").hide();
    }
}

function btnEdicaoWidget_Abas_CriarNovaPagina_Click(pSender)
{
    pSender = $(pSender);

    var lReferencia = pSender.attr("data-referencia");

    gDadosNovaPagina = {
                      Acao:            "SalvarPagina"
                    , IdPagina:        ""
                    , Titulo:          txtEdicaoWidget_Abas_NovaPagina_Titulo.val()
                    , Url:             txtEdicaoWidget_Abas_NovaPagina_Url.val()
                    , TipoDaEstrutura: cboEdicaoWidget_Abas_NovaPagina_Modo.val()
                    , Versao:          $("#hidVersao").val()    //pega a mesma versão da "página pai"
                    , Referencia:      lReferencia
                 };

    if(lReferencia == "Acordeon")
    {
        gDadosNovaPagina.Titulo          = txtEdicaoWidget_Acordeon_NovaPagina_Titulo.val();
        gDadosNovaPagina.Url             = txtEdicaoWidget_Acordeon_NovaPagina_Url.val();
        gDadosNovaPagina.TipoDaEstrutura = cboEdicaoWidget_Acordeon_NovaPagina_Modo.val();
    }

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), gDadosNovaPagina, ModuloCMS_WidgetAbas_NovaPagina_CallBack);

    return false;
}

function ModuloCMS_WidgetAbas_NovaPagina_CallBack(pResposta)
{
    gDadosNovaPagina.IdPagina = pResposta.Mensagem;
    
    gDadosNovaPagina.Galho    = gDadosNovaPagina.Url.substr(0, gDadosNovaPagina.Url.lastIndexOf("/") + 1);
    gDadosNovaPagina.GalhoSub = gDadosNovaPagina.Galho.toLowerCase();

    var lOpcao = "<option value='" + gDadosNovaPagina.IdPagina + "' title='" + gDadosNovaPagina.Titulo + "' data-url='" + gDadosNovaPagina.Url + "'>" + gDadosNovaPagina.Url + "</option>";

    var lLiPagina = "<li data-tipoestrutura='" + gDadosNovaPagina.TipoDaEstrutura + "' data-url='" + gDadosNovaPagina.Url + "' data-galhosub='" + gDadosNovaPagina.GalhoSub + "' data-galho='" + gDadosNovaPagina.Galho + "' data-titulo='" + gDadosNovaPagina.Titulo + "' data-idpagina='" + gDadosNovaPagina.IdPagina + "' style=''>" +
                    "  <label onclick='lstPaginas_LI_Click(this)' title='" + gDadosNovaPagina.Titulo + "'>" + gDadosNovaPagina.Url + "</label>" +
                    "  <div>" +
                    "    <button onclick='return lstPaginas_btnVisitar_Click(this)' title='Visitar'>  <span>Visitar</span>  </button>" +
                    "    <button onclick='return lstPaginas_btnRemover_Click(this)' title='Remover'>  <span>Remover</span>  </button>" +
                    "  </div>" +
                    "</li>";

    var lGalhoPai = $("#lstPaginas li.GalhoPai[data-GalhoSub='" + gDadosNovaPagina.GalhoSub + "']");

    if(lGalhoPai.length == 0)
    {
        //não havia um pai antes desta:

        lGalhoPai = "<li data-galhosub='" + gDadosNovaPagina.GalhoSub + "/' class='GalhoPai'>" + 
                    "  <label onclick='return liGalhoPai_Click(this)'>" + gDadosNovaPagina.Galho + "</label>" + 
                    "  <div> <button onclick='return lnkGalhoPai_NovaPagina_Click(this)' class='GalhoNovaPagina'></button> </div>" +
                    "  <ul></ul>"  + 
                    "</li>";

        $("#lstPaginas").find("li:eq(0)").before(lGalhoPai);

        lGalhoPai = $("#lstPaginas").find("li:eq(0)");
    }

    lGalhoPai.find(">ul").append(lLiPagina);

    if(gDadosNovaPagina.Referencia == "Abas")
    {
        cboEdicaoWidget_Abas_NovoItem_Conteudo.find("option:eq(1)").before(lOpcao);

        alert("Nova página criada e incluída nas abas.");

        txtEdicaoWidget_Abas_NovaPagina_Url.val("");
        txtEdicaoWidget_Abas_NovaPagina_Titulo.val("");

        $("#pnlEstruturaContainer_PainelEdicaoWidget_Abas_NovaPagina").hide();

        cboEdicaoWidget_Abas_NovoItem_Conteudo.val(pResposta.Mensagem);

        ModuloCMS_WidgetAba_AdicionarNova();
    }
    else if(gDadosNovaPagina.Referencia == "Acordeon")
    {
        cboEdicaoWidget_Acordeon_NovoItem_Conteudo.find("option:eq(1)").before(lOpcao);

        alert("Nova página criada e incluída no acordeon.");

        txtEdicaoWidget_Acordeon_NovaPagina_Url.val("");
        txtEdicaoWidget_Acordeon_NovaPagina_Titulo.val("");

        $("#pnlEstruturaContainer_PainelEdicaoWidget_Acordeon_NovaPagina").hide();

        cboEdicaoWidget_Acordeon_NovoItem_Conteudo.val(pResposta.Mensagem);

        ModuloCMS_WidgetAcordeon_AdicionarNova();
    }


    //<option title="Aempresasobre" data-url="aempresasobre" value="140">aempresasobre</option>
}

function ModuloCMS_ExcluirPagina(pItemSelecionado)
{
    if(confirm("Tem certeza que deseja excluir esta página?"))
    {
        pItemSelecionado.attr("data-PreExclusao", "true");

        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), { Acao: "ExcluirPagina", IdPagina: pItemSelecionado.attr("data-IdPagina") }, ModuloCMS_ExcluirPagina_CallBack);
    }
}

function ModuloCMS_ExcluirPagina_CallBack(pResposta)
{
    //pItemSelecionado.attr("data-PreExclusao", "true");

    var lLI = $("#lstPaginas").find("li[data-PreExclusao]");

    var lLIPai = lLI.closest("li.GalhoPai");
    
    var lLIs = $("#lstPaginas").find("li[data-galhosub='" + lLI.attr("data-galhosub") + "']");

    if(lLIs.length == 1)
    {
        if(lLIPai.length > 0)
        {
            //se só tem um item e não é raiz , remove ambos:
            lLIPai.remove();
        }
        else
        {
            lLI.remove();
        }
    }
    else
    {
        lLI.remove();
    }

    ModuloCMS_ZerarFormDeEdicaoDaPagina(true);

    GradSite_ExibirMensagem("A", "Página excluída com sucesso!");
}

function ModuloCMS_WidgetAba_AdicionarNova()
{
    if(txtEdicaoWidget_Abas_NovoItem_Texto.val() == "")
    {
        alert("Favor especificar um título para a aba");

        txtEdicaoWidget_Abas_NovoItem_Texto.focus();

        return;
    }

    if(cboEdicaoWidget_Abas_NovoItem_Conteudo.val() == "-1")
    {
        $("#pnlEstruturaContainer_PainelEdicaoWidget_Abas_NovaPagina").show();

        var lPaginaTitulo;

        txtEdicaoWidget_Abas_NovaPagina_Titulo.val( txtEdicaoWidget_Abas_NovoItem_Texto.val() ).focus();

        var lPaginaURL = ModuloCMS_BuscarDiretorioDaUrlAtual();

        lPaginaURL = lPaginaURL + ModuloCMS_ConverterTextoParaUrlAmigavel(txtEdicaoWidget_Abas_NovoItem_Texto.val());

        txtEdicaoWidget_Abas_NovaPagina_Url.val(lPaginaURL);
    }
    else
    {
        var lNovaAba = {       Titulo: txtEdicaoWidget_Abas_NovoItem_Texto.val()
                         ,   TipoLink: cboEdicaoWidget_Abas_NovoItem_Tipo.val()
                         , IdConteudo: cboEdicaoWidget_Abas_NovoItem_Conteudo.val()
                         ,        URL: cboEdicaoWidget_Abas_NovoItem_Conteudo.find("option:selected").attr("data-url")
                         , NomePagina: cboEdicaoWidget_Abas_NovoItem_Conteudo.find("option:selected").text()
                       };

        var lTR = lstEdicaoWidget_Abas_Itens.find("tr[data-IdConteudo='" + lNovaAba.IdConteudo + "']");

        if(lTR.length > 0)
        {
            alert("A aba '" + lTR.attr("data-Titulo") + "' já referencia esse conteúdo, favor escolher outro");

            return;
        }

        ModuloCMS_WidgetAba_AdicionarNovaNaLista(lNovaAba);

        ModuloCMS_AtualizarDadosDeEdicao_WidgetAbas();
    }
}

function ModuloCMS_WidgetAba_AdicionarNovaNaLista(pDadosDaAba)
{
    if(!pDadosDaAba.TipoLink || pDadosDaAba.TipoLink === undefined || pDadosDaAba.TipoLink == "")
    {
        pDadosDaAba.TipoLink = "Embutida";
    }

    if(pDadosDaAba.NomePagina == "" || pDadosDaAba.NomePagina === undefined)
    {
        pDadosDaAba.NomePagina = cboEdicaoWidget_Abas_NovoItem_Conteudo.find("option[value='" + pDadosDaAba.IdConteudo + "']").attr("data-NomePagina");
    }

    if(pDadosDaAba.NomePagina == "")
    {
        pDadosDaAba.NomePagina = "(ref. à pág. [" + pDadosDaAba.IdConteudo + "] n/d)";
    }

    var lTR = "<tr data-IdConteudo='"     + pDadosDaAba.IdConteudo + "' data-Titulo='" + pDadosDaAba.Titulo + "' data-TipoLink='" + pDadosDaAba.TipoLink + "' data-URL='" + pDadosDaAba.URL + "'>" + 
                "<td style='width:35%'><span class='Trimmer' title='" + pDadosDaAba.Titulo     + "'>"  + pDadosDaAba.Titulo     + "</span></td>" +
                "<td style='width:15%'><span class='Trimmer' title='" + pDadosDaAba.TipoLink   + "'>"  + pDadosDaAba.TipoLink   + "</span></td>" +
                "<td style='width:34%'><span class='Trimmer' title='" + pDadosDaAba.NomePagina + "'>"  + pDadosDaAba.URL        + "</span></td>" +
                "<td style='width:68px'>" + 
                  "<button onclick='return btnWidgetAba_Subir_Click(this)'   class='SubirItem'></button> "   +
                  "<button onclick='return btnWidgetAba_Descer_Click(this)'  class='DescerItem'></button> "  +
                  "<button onclick='return btnWidgetAba_Remover_Click(this)' class='ExcluirItem'></button> " +
                "</td>" +
              "</tr>";

    lstEdicaoWidget_Abas_Itens.append(lTR);

    lstEdicaoWidget_Abas_Itens.find(":eq(0)").hide();
}

function btnWidgetAcordeon_Remover_Click(pSender)
{
    if(confirm("Tem certeza que deseja excluir este item?"))
    {
        var lTR = $(pSender).closest("tr");

        var lID = lTR.attr("data-IdConteudo");

        lTR.remove();

        var lLI = gElementoDoWidgetSendoEditado.find("li[data-IdConteudo='" + lID + "']");

        if(lLI.hasClass("ativo"))
        {
            var lUL = lLI.parent();

            lLI.remove();

            lUL.find("li:eq(0)").click();
        }
        else
        {
            lLI.remove();
        }

        ModuloCMS_AtualizarDadosDeEdicao_WidgetAcordeon();
    }

    return false;
}

function btnWidgetAba_Remover_Click(pSender)
{
    if(confirm("Tem certeza que deseja excluir esta aba?"))
    {
        var lTR = $(pSender).closest("tr");

        var lID = lTR.attr("data-IdConteudo");

        lTR.remove();

        var lLI = gElementoDoWidgetSendoEditado.find("li[data-IdConteudo='" + lID + "']");

        if(lLI.hasClass("ativo"))
        {
            var lUL = lLI.parent();

            lLI.remove();

            lUL.find("li:eq(0)").click();
        }
        else
        {
            lLI.remove();
        }

        $("div[data-IdConteudo='" + lID + "']").remove();

        ModuloCMS_AtualizarDadosDeEdicao_WidgetAbas();
    }

    return false;
}

function btnWidgetAba_Descer_Click(pSender)
{
    pSender = $(pSender);

    var lTR = pSender.closest("tr");

    if(lTR.index() == (lTR.parent().find(">tr").length - 1))
    {
        alert("Aba já está no final.");
    }
    else
    {
        var lProxTR = lTR.next("tr");

        var lIndex = lTR.index() - 1;   //tem que desconsiderar a TR de "nenhum item"

        var lLI = gElementoDoWidgetSendoEditado.find("li:eq(" + lIndex + ")");

        var lProxLI = lLI.next("li");

        lProxTR.after(lTR);
        lProxLI.after(lLI);
    }

    return false;
}

function btnWidgetAba_Subir_Click(pSender)
{
    pSender = $(pSender);

    var lTR = pSender.closest("tr");

    if(lTR.index() == 1)
    {
        alert("Aba já é a primeira.");
    }
    else
    {
        var lProxTR = lTR.prev("tr");

        var lIndex = lTR.index() - 1;   //tem que desconsiderar a TR de "nenhum item"

        var lLI = gElementoDoWidgetSendoEditado.find("li:eq(" + lIndex + ")");

        var lProxLI = lLI.prev("li");

        lProxTR.before(lTR);
        lProxLI.before(lLI);
    }

    return false;
}


function btnWidAbas_VisitarPaginaConteudo_Click(pSender)
{
    pSender = $(pSender);

    var lURL = pSender.prev("ul").find("li.Ativo").attr("data-URL");

    document.location = GradSite_BuscarRaiz("/" + lURL);

    return false;
}


function btnWidAcordeon_VisitarPaginaConteudo_Click(pSender)
{
    pSender = $(pSender);

    var lURL = pSender.closest("li").attr("data-URL");

    document.location = GradSite_BuscarRaiz("/" + lURL);

    return false;
}


function ModuloCMS_WidgetAcordeon_AdicionarNova()
{
    if(txtEdicaoWidget_Acordeon_NovoItem_Texto.val() == "")
    {
        alert("Favor especificar um título");

        txtEdicaoWidget_Acordeon_NovoItem_Texto.focus();

        return;
    }

    if(cboEdicaoWidget_Acordeon_NovoItem_Conteudo.val() == "-1")
    {
        $("#pnlEstruturaContainer_PainelEdicaoWidget_Acordeon_NovaPagina").show();

        var lPaginaTitulo;

        txtEdicaoWidget_Acordeon_NovaPagina_Titulo.val( txtEdicaoWidget_Acordeon_NovoItem_Texto.val() ).focus();

        var lPaginaURL = ModuloCMS_BuscarDiretorioDaUrlAtual();

        lPaginaURL = lPaginaURL + ModuloCMS_ConverterTextoParaUrlAmigavel(txtEdicaoWidget_Acordeon_NovoItem_Texto.val());

        txtEdicaoWidget_Acordeon_NovaPagina_Url.val(lPaginaURL);
    }
    else
    {
        var lNovaAba = {       Titulo: txtEdicaoWidget_Acordeon_NovoItem_Texto.val()
                         , IdConteudo: cboEdicaoWidget_Acordeon_NovoItem_Conteudo.val()
                         ,        URL: cboEdicaoWidget_Acordeon_NovoItem_Conteudo.find("option:selected").attr("data-url")
                         , NomePagina: cboEdicaoWidget_Acordeon_NovoItem_Conteudo.find("option:selected").text()
                       };

        var lTR = lstEdicaoWidget_Acordeon_Itens.find("tr[data-IdConteudo='" + lNovaAba.IdConteudo + "']");

        if(lTR.length > 0)
        {
            alert("A aba '" + lTR.attr("data-Titulo") + "' já referencia esse conteúdo, favor escolher outro");

            return;
        }

        ModuloCMS_WidgetAcordeon_AdicionarNovaNaLista(lNovaAba);

        ModuloCMS_AtualizarDadosDeEdicao_WidgetAcordeon();
    }
}

function ModuloCMS_WidgetAcordeon_AdicionarNovaNaLista(pDadosDaAba)
{
    if(!pDadosDaAba.TipoLink || pDadosDaAba.TipoLink === undefined || pDadosDaAba.TipoLink == "")
    {
        pDadosDaAba.TipoLink = "Embutida";
    }

    if(pDadosDaAba.NomePagina == "" || pDadosDaAba.NomePagina === undefined)
    {
        pDadosDaAba.NomePagina = cboEdicaoWidget_Acordeon_NovoItem_Conteudo.find("option[value='" + pDadosDaAba.IdConteudo + "']").text();
    }

    var lTR = "<tr data-IdConteudo='"     + pDadosDaAba.IdConteudo + "' data-Titulo='" + pDadosDaAba.Titulo + "' data-TipoLink='" + pDadosDaAba.TipoLink + "' data-URL='" + pDadosDaAba.URL + "'>" + 
                "<td style='width:42%'><span class='Trimmer' title='" + pDadosDaAba.Titulo     + "'>"  + pDadosDaAba.Titulo     + "</span></td>" +
                "<td style='width:42%'><span class='Trimmer' title='" + pDadosDaAba.NomePagina + "'>"  + pDadosDaAba.NomePagina + "</span></td>" +
                "<td style='width:68px'>" + 
                  "<button onclick='return btnWidgetAba_Subir_Click(this)'   class='SubirItem'></button> "   +
                  "<button onclick='return btnWidgetAba_Descer_Click(this)'  class='DescerItem'></button> "  +
                  "<button onclick='return btnWidgetAcordeon_Remover_Click(this)' class='ExcluirItem'></button> " +
                "</td>" +
              "</tr>";

    lstEdicaoWidget_Acordeon_Itens.append(lTR);

    lstEdicaoWidget_Acordeon_Itens.find(":eq(0)").hide();
}
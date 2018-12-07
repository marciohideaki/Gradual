/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />
/// <reference path="02-ModuloCMS-Base.js" />
/// <reference path="02-ModuloCMS-Widgets.js" />


function ModuloCMS_CarregarItensDoTipoSelecionado()
{
    var lRequest = { Acao: "CarregarConteudoDinamico", IdTipoConteudo: cboCMS_ConteudoDinamico_SelecionarObjeto.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_CarregarItensDoTipoSelecionado_CallBack);
}


function ModuloCMS_CarregarItensDoTipoSelecionado_CallBack(pResposta)
{
    var lLista = pResposta.ObjetoDeRetorno;

    var lHTML = "";

    var lLabel;

    if(lLista.length > 0)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            if(lLista[a].Titulo)
            {
                lLabel = lLista[a].Titulo;
            }
            else if(lLista[a].Nome)
            {
                lLabel = lLista[a].Nome;
            } 
            else if(lLista[a].Descricao)
            {
                lLabel = lLista[a].Descricao;
            }
            else if(lLista[a].UrlDaPagina)
            {
                lLabel = lLista[a].UrlDaPagina;
            }

            lHTML += "<li data-IdTipoConteudo='" + lLista[a].IdTipoConteudo + "' data-IdConteudo='" + lLista[a].CodigoConteudo + "'>" +
                     "    <label onClick='lstConteudoDinamico_LI_Click(this)'><span>" + lLista[a].CodigoConteudo + "</span><span>" + lLabel + "</span></label>" +
                     "    <div>" +
                     "        <button title='Remover' onclick='return btnConteudoDinamico_Remover_Click(this)'><span>Remover</span></button>" +
                     "    </div>" +
                     "</li>";
        }
    }
    else
    {
        lHTML += "<li class='NenhumItem'>Nenhum item encontrado.</li>";
    }

    lstConteudoDinamico.html(lHTML);

    lstConteudoDinamico.find("li.NenhumItem").effect("highlight");
}


function ModuloCMS_CarregarListasDoTipoSelecionado()
{
    var lRequest = { Acao: "CarregarListas", IdTipoConteudo: cboCMS_ConteudoDinamico_SelecionarObjeto.val(), IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_CarregarListasDoTipoSelecionado_CallBack);
}


function ModuloCMS_CarregarListasDoTipoSelecionado_CallBack(pResposta)
{
    var lLista = pResposta.ObjetoDeRetorno;

    var lHTML = "";
    
    if(lLista.length > 0)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            lHTML += "<li data-IdTipoConteudo='" + lLista[a].CodigoTipoConteudo + "' data-IdLista='" + lLista[a].CodigoLista + "' data-Regra='" + lLista[a].Regra + "'>" +
                     "    <label onClick='lstConteudoDinamico_LI_Click(this)'><span>" + lLista[a].CodigoLista + "</span><span>" + lLista[a].DescricaoLista + "</span></label>" +
                     "    <div>" +
                     "        <button title='Remover' onclick='return btnConteudoDinamico_Remover_Click(this)'><span>Remover</span></button>" +
                     "    </div>" +
                     "</li>";
        }
    }
    else
    {
        lHTML += "<li class='NenhumItem'>Nenhuma lista encontrada.</li>";
    }

    lstConteudoDinamico.html(lHTML);

    lstConteudoDinamico.find("li.NenhumItem").effect("highlight");
}


function ModuloCMS_BuscarDadosParaEdicao(pIdTipoConteudo, pCodigoConteudo)
{
    var lRequest = { Acao: "BuscarDadosDeConteudoDinamico", CodigoConteudo: pCodigoConteudo, IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_BuscarDadosParaEdicao_CallBack);
}


function ModuloCMS_BuscarDadosParaEdicao_CallBack(pResposta)
{
    gContDinamicoSendoEditado = $.evalJSON(pResposta.ObjetoDeRetorno.ConteudoJson.replace(/'/gi, "\""));

    gContDinamicoJsonAntesDeEditar = $.toJSON(gContDinamicoSendoEditado);   //só pra garantir que comparações serão feitas sempre no js

    ModuloCMS_CriarFormularioDeEdicao(pResposta.ObjetoDeRetorno.CodigoTipoConteudo, gContDinamicoSendoEditado);

    //pnlContainerCMS_Form_Listas.parent().find("div.ContainerCMS_PainelComTabs_BotoesSalvar").show();
}



function ModuloCMS_CriarFormularioDeEdicao(pIdTipoConteudo, pObjeto)
{
    var lOptionSelecionado = cboCMS_ConteudoDinamico_SelecionarObjeto.find("option[value='" + pIdTipoConteudo + "']");

    var lJsonConteudo = lOptionSelecionado.attr("data-TipoDeConteudoJson").replace(/'/gi, "\"");       //TODO: verificar se é melhor tirar isso porque já estão cadastrados ok no banco

    var lRegexPropriedades = /\"[^\":]*(?=\":)/gi;

    var lPropriedade = lRegexPropriedades.exec(lJsonConteudo);

    var lValor;

    var lObjetoConteudo = $.evalJSON(lJsonConteudo);

    var lNovoObjeto = {};

    pnlContainerCMS_Form_Itens.html("");

    $("#lblContainerCMS_Titulo").html(lOptionSelecionado.text());

    while(lPropriedade != null)
    {
        lPropriedade = lPropriedade[0].substr(1);  //tira a primeira aspas que nao deu pra fazer no backreference da regex =P

        lValor = eval("lObjetoConteudo." + lPropriedade);

        eval("lNovoObjeto." + lPropriedade + " = ''; ");        //vai deixando todas as propriedades vazias num objeto novo "dummie"

        if(lValor == "Texto")
        {
            ModuloCMS_CriarCampoDeEdicao_Texto(lPropriedade, pObjeto);
        }
        else if(lValor == "Numero")
        {
            ModuloCMS_CriarCampoDeEdicao_Numero(lPropriedade, pObjeto);
        }
        else if(lValor == "HTML")
        {
            ModuloCMS_CriarCampoDeEdicao_HTML(lPropriedade, pObjeto);
        }
        else if(lValor == "Data")
        {
            ModuloCMS_CriarCampoDeEdicao_Data(lPropriedade, pObjeto);
        }
        else if(lValor == "Imagem")
        {
            ModuloCMS_CriarCampoDeEdicao_Imagem(lPropriedade, pObjeto);
        }
        else if(lValor.indexOf("Opcoes") == 0)
        {
            ModuloCMS_CriarCampoDeEdicao_Opcoes(lPropriedade, lValor, pObjeto);
        }
        else if(lValor == "JSON")
        {
            ModuloCMS_CriarCampoDeEdicao_JSON(lPropriedade, pObjeto);
        }

        lPropriedade = lRegexPropriedades.exec(lJsonConteudo);
    }

    if(pObjeto == null)
    {
        //veio como null, então é uma inclusão de novo; pega o dummie e coloca ele como o objeto sendo editado
        lNovoObjeto.IdConteudo = new Date().getTime();
        lNovoObjeto.FlagNovoConteudo = true;

        gContDinamicoSendoEditado = lNovoObjeto;

        gContDinamicoJsonAntesDeEditar = $.toJSON(gContDinamicoSendoEditado);

        lstConteudoDinamico.find("li.Selecionado").removeClass("Selecionado");
    }

    pnlContainerCMS_Form_Itens.closest("div.ContainerCMS_Painel").show();
}

function ModuloCMS_CriarCampoDeEdicao_Texto(pPropriedade, pObjeto)
{
    var lIdCampo = "txtConteudoDinamico_" + pPropriedade;

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "'>" + pPropriedade + "</label>" + 
                "   <input  id='" + lIdCampo + "' type='text' data-Propriedade='" + pPropriedade + "' onkeydown='return txtEdicaoConteudoDinamico_KeyDown(this, event)' />" + 
                "</p>");

    if(pPropriedade.indexOf("URL") != -1 || pPropriedade.indexOf("LinkPara") != -1)
    {
        $("#" + lIdCampo)
            .addClass("PickerDeArquivo")
            .bind("focus", iptPickerDeArquivo_Focus)
            .bind("blur",  iptPickerDeArquivo_Blur);
    }
    
    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( eval("pObjeto." + pPropriedade) );
    }
}

function ModuloCMS_CriarCampoDeEdicao_Numero(pPropriedade, pObjeto)
{
    var lIdCampo = "txtConteudoDinamico_" + pPropriedade;

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "'>" + pPropriedade + "</label>" + 
                "   <input  id='" + lIdCampo + "' type='text' data-Propriedade='" + pPropriedade + "' onkeydown='return txtEdicaoConteudoDinamicoNumerico_KeyDown(this, event)' />" + 
                "</p>");

    if(pPropriedade.indexOf("URL") != -1 || pPropriedade.indexOf("LinkPara") != -1)
    {
        $("#" + lIdCampo)
            .addClass("PickerDeArquivo")
            .bind("focus", iptPickerDeArquivo_Focus)
            .bind("blur",  iptPickerDeArquivo_Blur);
    }
    
    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( eval("pObjeto." + pPropriedade) );
    }
}

function ModuloCMS_CriarCampoDeEdicao_HTML(pPropriedade, pObjeto)
{
    var lIdCampo = "txtConteudoDinamico_" + pPropriedade;

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "_Preview'>" + pPropriedade + "</label>" + 
                "   <input  id='" + lIdCampo + "_Preview' type='text' onfocus='iptEdicaoDeHTML_Focus(this)' onkeydown='return txtEdicaoConteudoDinamico_KeyDown(this, event)' value='<html>' />" + 
                "   <input  id='" + lIdCampo + "' type='hidden' data-Propriedade='" + pPropriedade + "' />" + 
                "</p>");

    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( "<aguarde, carregando HTML...>" );

        //está editando um objeto, então precisa ir lá pegar a propriedade que é em HTML através de uma chamada HTMl, não JSON:

        $.ajax({
                  url:      GradSite_BuscarRaiz("/Async/ModuloCMS.aspx")
                , type:     "post"
                , cache:    false
                , dataType: "html"
                , data:     { Acao: "BuscarConteudoHTML", CodigoConteudo: pObjeto.CodigoConteudo, IdDaPagina: gEstruturaDaPagina.IdDaPagina }
                , success:  function(pResposta) { ModuloCMS_CriarCampoDeEdicao_HTML_CallBack(lIdCampo, pResposta); }    //TODO: verificar se isso realmente funciona (lIdCampo) quando tiver mais de um campo HTML pro conteudo dinâmico...
                , error:    GradSite_TratarRespostaComErro
               });
    }
}

function ModuloCMS_CriarCampoDeEdicao_HTML_CallBack(pIdCampo, pResposta)
{
    $("#" + pIdCampo + "_Preview").val( "<HTML>" );

    $("#" + pIdCampo).val( pResposta );
}

function ModuloCMS_CriarCampoDeEdicao_Data(pPropriedade, pObjeto)
{
    var lIdCampo = "txtConteudoDinamico_" + pPropriedade;

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "'>" + pPropriedade + "</label>" + 
                "   <input  id='" + lIdCampo + "' type='text' data-Propriedade='" + pPropriedade + "' class='DatePicker' maxlength='10' onkeydown='return txtEdicaoConteudoDinamico_KeyDown(this, event)' />" + 
                "</p>");

    $("#" + lIdCampo)
        .datepicker( {
                         showAnim: ""
                       , beforeShow: function()     //precisa dessa parada pra ele se mostrar ACIMA do campo de texto (não achei opção pra isso, tive que fazer assim)
                                     {
                                          window.setTimeout(function() { $("#ui-datepicker-div").css({ top: "-=210" }).show(); }, 30);
                                     }
                     });

    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( eval("pObjeto." + pPropriedade) );
    }
    else
    {
        $("#" + lIdCampo).val( GradSite_DataDeHoje() );
    }
}

function ModuloCMS_CriarCampoDeEdicao_Imagem(pPropriedade, pObjeto)
{
    var lIdCampo = "txtConteudoDinamico_" + pPropriedade;

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "'>" + pPropriedade + "</label>" + 
                "   <input  id='" + lIdCampo + "' type='text' data-Propriedade='" + pPropriedade + "' class='PickerDeArquivo' onkeydown='return txtEdicaoConteudoDinamico_KeyDown(this, event)' />" + 
                "</p>");

    $("#" + lIdCampo)
        .bind("focus", iptPickerDeArquivo_Focus)
        .bind("blur",  iptPickerDeArquivo_Blur);

    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( eval("pObjeto." + pPropriedade) );
    }
}

function ModuloCMS_CriarCampoDeEdicao_Opcoes(pPropriedade, pOpcoes, pObjeto)
{
    var lIdCampo = "cboConteudoDinamico_" + pPropriedade;

    // Opcoes([M]-Masculino, [F]-Feminino)

    pOpcoes = pOpcoes.substr(pOpcoes.indexOf("(") + 1).trim();
    pOpcoes = pOpcoes.substr(0, pOpcoes.length - 1);

    var lOpcoes = pOpcoes.split(",");
    var lOpcoesValores;

    var lHTMLOpcoes = "";

    for(var a = 0; a < lOpcoes.length; a++)
    {
        lOpcoesValores = lOpcoes[a].split("-");

        if(lOpcoesValores.length == 2)
        {
            lHTMLOpcoes += "<option value='" + lOpcoesValores[0].trim().replace("[", "").replace("]", "") + "'>" + lOpcoesValores[1] + "</option>";
        }
    }

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "'>" + pPropriedade + "</label>" + 
                "   <select  id='" + lIdCampo + "' data-Propriedade='" + pPropriedade + "'>" + 
                        lHTMLOpcoes +
                "   </select>" + 
                "</p>");

    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( eval("pObjeto." + pPropriedade) );
    }
}

function ModuloCMS_CriarCampoDeEdicao_JSON(pPropriedade, pObjeto)
{
    var lIdCampo = "txtConteudoDinamico_" + pPropriedade;

    pnlContainerCMS_Form_Itens
        .append("<p>" + 
                "   <label for='" + lIdCampo + "'>" + pPropriedade + "</label>" + 
                "   <input  id='" + lIdCampo + "' type='text' data-Propriedade='" + pPropriedade + "' data-TipoPropriedade='JSON' onkeydown='return txtEdicaoConteudoDinamico_KeyDown(this, event)' />" + 
                "</p>");

    if(pObjeto != null)
    {
        $("#" + lIdCampo).val( eval(  "$.toJSON(pObjeto." + pPropriedade + ")"  ) );
    }
}




function ModuloCMS_SalvarConteudoDinamico() 
{
    var lInputs = pnlContainerCMS_Form_Itens.find("[data-Propriedade]");

    var lObjeto = { };

    var lInput, lValor;
    var lConteudoHTML = null;

    lInputs.each(function()
    {
        lInput = $(this);

        if(lInput.attr("data-Propriedade") != "ConteudoHTML")
        {
            if(lInput.attr("data-TipoPropriedade") == "JSON")
            {
                lValor = lInput.val();

                try
                {
                    eval("lObjeto." + lInput.attr("data-Propriedade") + " = $.evalJSON(lValor);");
                }
                catch(erro_json)
                {
                    alert("Atenção! A propriedade " + lInput.attr("data-Propriedade") + " está com um valor que não é JSON válido.\r\nExemplo de sintaxe: \r\n\r\n{ \"Opcao1\":\"Valor1\",\"Opcao2\":\"Valor2\" }\r\n\r\nSintaxe Encontrada:\r\n\r\n" + lValor);
                }
            }
            else
            {
                lValor = lInput.val();

                eval("lObjeto." + lInput.attr("data-Propriedade") + " = lValor;");
            }
        }
        else
        {
            lConteudoHTML = lInput.val();
        }
    });


    lObjeto.ConteudoJson = $.toJSON( lObjeto );

    lObjeto.CodigoConteudo = gContDinamicoSendoEditado.CodigoConteudo;

    lObjeto.CodigoTipoConteudo = $("#cboCMS_ConteudoDinamico_SelecionarObjeto").val();

    if(lConteudoHTML != null)
        lObjeto.ConteudoHTML = lConteudoHTML;

    gContDinamicoSendoEditado = lObjeto;

    lObjeto.Acao =  "SalvarConteudoDinamico";

    lObjeto.IdDaPagina = gEstruturaDaPagina.IdDaPagina

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lObjeto, ModuloCMS_SalvarConteudoDinamico_CallBack);

    
}

function ModuloCMS_SalvarConteudoDinamico_CallBack(pResposta)
{
    gContDinamicoSendoEditado.CodigoConteudo = pResposta.ObjetoDeRetorno;

    var lLIDoItem = lstConteudoDinamico.find("li[data-IdConteudo='" + gContDinamicoSendoEditado.CodigoConteudo + "']");

    if(lLIDoItem.length == 0)
    {
        lstConteudoDinamico.find("li.NenhumItem").remove();

        if(!gContDinamicoSendoEditado.Titulo || gContDinamicoSendoEditado.Titulo == "")
        {
            gContDinamicoSendoEditado.Titulo = gContDinamicoSendoEditado.UrlDaPagina;
        }

        if(gContDinamicoSendoEditado.Titulo == "")
        {
            gContDinamicoSendoEditado.Titulo = "(item)";
        }

        lstConteudoDinamico.append("<li data-IdConteudo='" + gContDinamicoSendoEditado.CodigoConteudo + "' data-IdTipoConteudo='" + gContDinamicoSendoEditado.CodigoTipoConteudo + "'>" + 
                                   "    <label onclick='lstConteudoDinamico_LI_Click(this)'>" +
                                   "        <span>" + gContDinamicoSendoEditado.CodigoConteudo + "</span>" +
                                   "        <span>" + gContDinamicoSendoEditado.Titulo + "</span>" +
                                   "    </label>" +
                                   "    <div>" +
                                   "        <button onclick='return btnConteudoDinamico_Remover_Click(this)' title='Remover'>" +
                                   "    </div>" +
                                   "</li>");
    }
    else
    {
        lLIDoItem.find("label span:eq(1)").html(gContDinamicoSendoEditado.Titulo);
    }

    GradSite_ExibirMensagem("I", pResposta.Mensagem);
}


function ModuloCMS_CancelarEdicaoDeConteudoDinamico()
{
    gContDinamicoJsonAntesDeEditar = null;
    gContDinamicoSendoEditado = null;

    pnlConteudoDinamicoContainer.find(" > div.ContainerCMS_Painel:gt(0)").hide();

    lstConteudoDinamico.find("li.Selecionado").removeClass("Selecionado");
}



function ModuloCMS_ConfigurarFormularioDeEdicaoParaListas()
{
    pnlContainerCMS_Form_Itens.hide();

    pnlContainerCMS_Form_Listas.show().closest("div.ContainerCMS_Painel").show();

    txtEdicaoLista_Descricao.val( gContDinamicoSendoEditado.Descricao );
    txtEdicaoLista_Regra.val( gContDinamicoSendoEditado.Regra );

    //TODO: Já roda um teste pra ver os resultados
}

function ModuloCMS_SalvarListaDinamica()
{
    var ldescricao      = $("#txtEdicaoLista_Descricao").val();

    var lRegra          = $("#txtEdicaoLista_Regra").val();

    var lIdLista         = gContDinamicoSendoEditado.IdLista;

    var lIdTipoConteudo = $("#cboCMS_ConteudoDinamico_SelecionarObjeto").val();

    var lDados = { Acao: "Salvarlista", Descricao: ldescricao, Regra: lRegra, CodigoLista: lIdLista, CodigoTipoConteudo: lIdTipoConteudo, IdDaPagina: gEstruturaDaPagina.IdDaPagina };

    gContDinamicoSendoEditado = lDados;

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, ModuloCMS_SalvarListaDinamica_CallBack);
    
}

function ModuloCMS_SalvarListaDinamica_CallBack(pResposta)
{
    gContDinamicoSendoEditado.IdLista = pResposta.ObjetoDeRetorno;
    
    var lLIDoItem = lstConteudoDinamico.find("li[data-IdLista='" + gContDinamicoSendoEditado.IdLista + "']");

    if(lLIDoItem.length == 0)
    {
        lstConteudoDinamico.find("li.NenhumItem").remove();

        lstConteudoDinamico.append("<li data-IdTipoConteudo='" + gContDinamicoSendoEditado.CodigoTipoConteudo + "' data-IdLista='" + gContDinamicoSendoEditado.IdLista + "' data-Regra='" + gContDinamicoSendoEditado.Regra + "' class='Selecionado'>" +
                                   "    <label onClick='lstConteudoDinamico_LI_Click(this)'><span>" + gContDinamicoSendoEditado.IdLista + "</span><span>" + gContDinamicoSendoEditado.Descricao + "</span></label>" +
                                   "    <div>" +
                                   "        <button title='Remover' onclick='return btnConteudoDinamico_Remover_Click(this)'><span>Remover</span></button>" +
                                   "    </div>" +
                                   "</li>");
    }
    else
    {
        lLIDoItem.find("label span:eq(1)").html(gContDinamicoSendoEditado.Descricao);
    }

    GradSite_ExibirMensagem("I", pResposta.Mensagem);
}


function ModuloCMS_ExcluirConteudoDinamico(pIdConteudo)
{
    var lRequest = { Acao: "ExcluirConteudoDinamico", CodigoConteudo: pIdConteudo, IdDaPagina: gEstruturaDaPagina.IdDaPagina };
    
    gIDdoElementoAntesDeExcluir = pIdConteudo;

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lRequest, ModuloCMS_ExcluirConteudoDinamico_CallBack);
}

function ModuloCMS_ExcluirConteudoDinamico_CallBack(pResposta)
{
    lstConteudoDinamico.find("li[data-IdConteudo='" + gIDdoElementoAntesDeExcluir + "']").remove();

    if(gContDinamicoSendoEditado.CodigoConteudo == pResposta.ObjetoDeRetorno)
    {
        ModuloCMS_CancelarEdicaoDeConteudoDinamico();
    }

    gIDdoElementoAntesDeExcluir = null;

    GradSite_ExibirMensagem("I", pResposta.Mensagem);
}

function ModuloCMS_ExcluirListaConteudo(pIdLista)
{
    var lDados = { Acao: "ExcluirLista", CodigoLista: pIdLista, IdDaPagina: gEstruturaDaPagina.IdDaPagina };
    
    gIDdoElementoAntesDeExcluir = pIdLista;

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, ModuloCMS_ExcluirListaConteudo_CallBack);
}

function ModuloCMS_ExcluirListaConteudo_CallBack(pResposta)
{
    lstConteudoDinamico.find("li[data-IdLista='" + gIDdoElementoAntesDeExcluir + "']").remove();
    
    if(gContDinamicoSendoEditado.IdLista == pResposta.ObjetoDeRetorno)
    {
        ModuloCMS_CancelarEdicaoDeConteudoDinamico();
    }

    gIDdoElementoAntesDeExcluir = null;

    GradSite_ExibirMensagem("I", pResposta.Mensagem);
}

function btnIncluirBannerLateral_Click(pSender)
{
    pSender = $(pSender);

    pSender.parent().find(".pnlListaDeBanners").toggle();



    return false;
}

function btnExcluirBanner_Click(pSender)
{
    pSender = $(pSender);

    var lDados = {
                      Acao: "ExcluirBannerLateral"
                    , IdBannerLink: pSender.attr("data-IdBannerLink")
                    , IdPagina: pSender.attr("data-IdDaPagina")
                 };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, ModuloCMS_ExcluirBannerLateral_CallBack);

    return false;
}


function ModuloCMS_ExcluirBannerLateral_CallBack(pResposta)
{
    if(pResposta.Mensagem == "OK")
    {
        var lPainel = $("button[data-IdBannerLink='" + pResposta.ObjetoDeRetorno + "']").parent()

        lPainel.remove();   //TODO: voltar o painel pra incluir um

        GradSite_ExibirMensagem("I", "Banner excluído com sucesso.");
    }
}

function btnSalvarBanner_Click(pSender)
{
    pSender = $(pSender);

    var lCombo = pSender.parent().find("select.cboBanner");

    var lPainel = pSender.closest(".pnlIncluirBannerLateral");

    var lDados =  {
                       CodigoTipoConteudo: 16
                     ,             Banner: lCombo.val()
                     ,            Posicao: lPainel.attr("data-Posicao")
                     ,       DataCadastro: GradSite_DataDeHoje()
                     ,        UrlDaPagina: lPainel.attr("data-UrlDaPagina")
                     ,           IdPagina: pSender.attr("data-IdDaPagina")
                  };

    var lJson = $.toJSON(lDados);

    lDados.ConteudoJson = lJson;
    lDados.Acao = "SalvarBannerLateralLink";

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/ModuloCMS.aspx"), lDados, ModuloCMS_SalvarBannerLateral_CallBack);

    return false;
}

function ModuloCMS_SalvarBannerLateral_CallBack(pResposta)
{
    GradSite_ExibirMensagem("A", "Banner salvo com sucesso, a página precisa ser recarregada.");

}
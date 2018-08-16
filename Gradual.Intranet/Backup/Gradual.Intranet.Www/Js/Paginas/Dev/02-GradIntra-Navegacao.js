/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

var gGradIntra_Navegacao_SistemaAtual    = null;
var gGradIntra_Navegacao_SubSistemaAtual = null;

var gGradIntra_Navegacao_SubSistemaAtualExibeBusca    = true;   //flag se deve exibir a busca    logo que trocar de SubSistema
var gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;   //flag se deve exibir o conteúdo logo que trocar de SubSistema

var gGradIntra_Navegacao_PainelDeBuscaAtual    = null;
var gGradIntra_Navegacao_PainelDeConteudoAtual = null;

/*

    Seção "1": Menus principal e secundário

*/

function GradIntra_Navegacao_IrParaSistema(pSistema, pNavegarAutomaticamenteParaSubSistema)
{
///<summary>Muda o sistema que o usuário está navegando, lidando com os menus e o display do conteúdo.</summary>
///<param name="pSistema" type="String">Sistema para o qual ir. Utilizar para essa string alguma das constantes CONST_SISTEMA_XXXX.</param>
///<param name="pNavegarAutomaticamenteParaSubSistema" type="Boolean">Flag se deve ir automaticamente para o primeiro sub-sistema dentro do sistema selecionado.</param>
///<returns>void</returns>

    if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
    {
        if (confirm(CONST_ALERT_CONFIRMA_CANCELAR_INCLUSAO))
            GradIntra_Navegacao_OcultarFormularioDeNovoItem(true);
        else
            return false;
    }

    if (pSistema != "" && pSistema != null && pSistema != gGradIntra_Navegacao_SistemaAtual)
    {
        var lLI, lLink;

        //verificar se não tem alguma operação sendo feita atualmente que impede navegação

        gGradIntra_Navegacao_SistemaAtual    = pSistema;
        gGradIntra_Navegacao_SubSistemaAtual = null;

        $("#pnlMenuPrincipal > li").removeClass(CONST_CLASS_ITEM_SELECIONADO);

        $("#pnlConteudo > div").hide();

        lLI = $("#pnlMenuPrincipal > li a[rel='" + gGradIntra_Navegacao_SistemaAtual + "']").parent();

        lLI.addClass(CONST_CLASS_ITEM_SELECIONADO);

        lLink = lLI.find("ul.SubMenu li." + CONST_CLASS_ITEM_SELECIONADO + " a");

        if (lLink.length == 0)
        {
            //nenhum subsistema selecionado ainda, vai pro primeiro
            
            lLink = lLI.find("ul.SubMenu li a[rel]:eq(0)");
        }
        
        VerificarMenuScroller();

        GradIntra_Navegacao_CarregarConteudosDoSistemaSelecionado();
        
        if (pNavegarAutomaticamenteParaSubSistema)
            GradIntra_Navegacao_IrParaSubSistema(pSistema, lLink.attr("rel"));
    }
}

function GradIntra_Navegacao_IrParaSubSistema(pSistema, pSubSistema)
{
///<summary>Muda o sub-sistema que o usuário está navegando, lidando com os menus e o display do conteúdo.</summary>
///<param name="pSistema" type="String">Sistema ao qual o sub-sistema pertente. Utilizar para essa string alguma das constantes CONST_SISTEMA_XXXX.</param>
///<param name="pSubSistema" type="String">Sub-Sistema desejado. Utilizar para essa string alguma das constantes CONST_SUBSISTEMA_XXXX.</param>
///<returns>void</returns>

    if (pSistema != gGradIntra_Navegacao_SistemaAtual)
        GradIntra_Navegacao_IrParaSistema(pSistema, false);

    if ((pSubSistema != "" && pSubSistema != null && pSubSistema != gGradIntra_Navegacao_SubSistemaAtual) || (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoDeRisco" || gGradIntra_Navegacao_SubSistemaAtual == "PLD" || gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoRiscoGeralDetalhamento" || gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoCustodia" || gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoPositionClient"))
    {
        // Verifica se está o usuário já estava incluindo um item, se sim precisa confirmar que ele quer cancelar a inclusão:


        if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
        {
            if (confirm(CONST_ALERT_CONFIRMA_CANCELAR_INCLUSAO))
                GradIntra_Navegacao_OcultarFormularioDeNovoItem(true);
            else
                return false;
        }

        // Esconde os painéis que estão sendo exibidos atualmente (de busca e de conteúdo)

        if (gGradIntra_Navegacao_PainelDeConteudoAtual != null)
            gGradIntra_Navegacao_PainelDeConteudoAtual.hide();

        if (gGradIntra_Navegacao_PainelDeBuscaAtual != null)
            gGradIntra_Navegacao_PainelDeBuscaAtual.hide();

        $("#pnlConteudo_Sistema_ObjetosDoSistema").hide();

        // Marca a flag se está incluindo ou não com base na palavra Novo_ antes do nome do subsistema

        gGradIntra_Cadastro_FlagIncluindoNovoItem = (pSubSistema.indexOf("Novo_") == 0);

        // Marca para exibir busca e conteúdo como padrão;
        //os sistemas que não exibem busca ou conteúdo quando troca o subsistema devem setar 
        //as flags apropriadas nas funções "AoSelecionarSistema"
        
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca    = true;
        gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

        // o sub-sistema de "Migração" dentro de "clientes" é uma excessão, só ele causa
        // o painel de busca ser atualizado. Então, se estivermos vindo de ou indo para ele,
        // temos que marcar a flag pra recarregar o painel de busca:
        if ((gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_MIGRACAO) || (pSubSistema == CONST_SUBSISTEMA_MIGRACAO))
        {
            if (gGradIntra_Navegacao_PainelDeBuscaAtual != null)
                gGradIntra_Navegacao_PainelDeBuscaAtual.removeClass(CONST_CLASS_CONTEUDOCARREGADO);
        }

        // Marca o subsistema atual com o que o cara clicou agora: (está vindo do parâmetro)

        gGradIntra_Navegacao_SubSistemaAtual = pSubSistema;

        // Marca o painel de busca atual como o do subsistema que o cara escolheu:

        gGradIntra_Navegacao_PainelDeBuscaAtual    = $("#pnlBusca_"    + gGradIntra_Navegacao_SistemaAtual + "_" + gGradIntra_Navegacao_SubSistemaAtual);

        // Se estamos incluindo, o painel de conteúdo é o pnlNovoItem independente do subsistema; 
        // se não, pegamos o painel pelo nome do sistema / subsistema

        if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
        {
            gGradIntra_Navegacao_PainelDeConteudoAtual = $("#pnlNovoItem");

            GradIntra_Navegacao_ReverterTipoDeObjetoAtual(null);
        }
        else
        {
            gGradIntra_Navegacao_PainelDeConteudoAtual = $("#pnlConteudo_" + gGradIntra_Navegacao_SistemaAtual + "_" + gGradIntra_Navegacao_SubSistemaAtual);
        }

        // muda o tracejado do sistema e do subsistema selecionados no menu acima:

        $("ul.Sistema_" + gGradIntra_Navegacao_SistemaAtual + " li." + CONST_CLASS_ITEM_SELECIONADO).removeClass(CONST_CLASS_ITEM_SELECIONADO);

        $("ul.Sistema_" + gGradIntra_Navegacao_SistemaAtual + " li a[rel='" + gGradIntra_Navegacao_SubSistemaAtual + "']")
            .parent()
            .addClass(CONST_CLASS_ITEM_SELECIONADO);

        // verifica qual sistema que é para invocar o "CallBack" fake para cada sistema poder se ajustar de acordo com a seleção:

        if (pSistema == CONST_SISTEMA_CLIENTES)
        {
            GradIntra_Clientes_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_COMPLIANCE) 
        {
            //gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            GradIntra_Compliance_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_RISCO) {

            if (gGradIntra_Navegacao_SubSistemaAtual == "PLD" || gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoRiscoGeralDetalhamento" || gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoCustodia") 
            {
                gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
            }
            
            GradIntra_Risco_AoSelecionarSistema();

            //setTimeout("GradIntra_Risco_AoSelecionarSistema()", 750);
        }
        else if (pSistema == CONST_SISTEMA_SEGURANCA) {
            GradIntra_Seguranca_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_MONITORAMENTO) {
            GradIntra_Monitoramento_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_SISTEMA) {
            GradIntra_Sistema_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_RELATORIOS) {
            GradIntra_Relatorios_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_DBM) {
            GradIntra_Relatorios_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_HOMEBROKER) {
            GradIntra_HomeBroker_AoSelecionarSistema();
        }
        else if (pSistema == CONST_SISTEMA_SOLICITACOES)
        {
            GradIntra_Solicitacoes_AoSelecionarSistema();
        }

        //verifica se a parte de cima é pra mostrar relatórios ou busca ou nada:
//        if (pSubSistema == CONST_SUBSISTEMA_ESTATISTICA_DAYTRADE) {
//            GradIntra_Navegacao_PaginaSubSistema_Compliance();
//        } else 
        if (pSubSistema == CONST_SUBSISTEMA_RELATORIOS) {
            GradIntra_Navegacao_ExibirFiltroDeRelatorio();
        }
        else if (pSubSistema == CONST_SUBSISTEMA_RELATORIOS_DBM) {
            GradIntra_Navegacao_ExibirFiltroDeRelatorio();
        }
        else if (pSubSistema == CONST_SUBSISTEMA_POUPE_DIRECT) {
            GradIntra_Solicitacoes_AoSelecionarSistema();
        }
        else if (pSubSistema == CONST_SUBSISTEMA_POUPE_OPERACAO) {
            GradIntra_Solicitacoes_AoSelecionarSistema();
        }
        else if (gGradIntra_Navegacao_SubSistemaAtualExibeBusca &&
        gGradIntra_Navegacao_SubSistemaAtual != "GruposDeRiscoRestricoes" &&
        gGradIntra_Navegacao_SubSistemaAtual != "GruposDeRiscoRestricoesSpider" &&
        gGradIntra_Navegacao_SubSistemaAtual != "GruposDeRisco" &&
        gGradIntra_Navegacao_SubSistemaAtual != "MonitoramentoDeRisco" &&
        gGradIntra_Navegacao_SubSistemaAtual != "MonitoramentoPositionClient")  
        {
            GradIntra_Navegacao_ExibirBusca();
        }

        //exibe o conteúdo atual:

        if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
        {
            GradIntra_Navegacao_ExibirFormularioDeNovoItem();
        }
        else
        {
            if (gGradIntra_Navegacao_SubSistemaAtualExibeConteudo)
            {            
                if (gGradIntra_Navegacao_PainelDeConteudoAtual.hasClass(CONST_CLASS_CONTEUDOCARREGADO))
                {
                    gGradIntra_Navegacao_PainelDeConteudoAtual.show();

                    if ($("#pnlConteudo_Clientes_Busca").find(".Expandida").length > 0)
                    {
                        GradIntra_Navegacao_ExpandirItemSelecionado($("#pnlConteudo_Clientes_Busca").find(".Expandida"));
                    }
                }
                else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_MIGRACAO)
                {
                    $("#pnlConteudo_Sistema_Migracao").hide();
                    $("#cboClientes_MigracaoEntreAssessores_AssessorDe").val('');
                }
                else
                {
                    var lPainelJaCarregado = gGradIntra_Navegacao_PainelDeConteudoAtual.find(".ConteudoCarregado");

                    if (lPainelJaCarregado.length > 0)
                    {
                        lPainelJaCarregado.closest(".Conteudo_Container").show();
                    }
//                    else
//                    {
//                        aqui tem que fazer o ajax
//                    }
                }
            }

            Preferencias_SalvarUltimoSubSistemaSelecionado(pSistema, pSubSistema);
        }
    }
}

/*

    Seção "2": Painéis de busca

*/

function GradIntra_Navegacao_ExibirBusca()
{
///<summary>Exibe o painel de busca de objeto, fazendo a chamada ajax para pegar o HTML se o conteúdo ainda não foi carregado.</summary>
///<returns>void</returns>

    if (gGradIntra_Navegacao_PainelDeBuscaAtual.hasClass(CONST_CLASS_CONTEUDOCARREGADO))
    {        
        GradIntra_Navegacao_ExibirBusca_CallBack();
    }
    else
    {
        var lUrlBusca = "Buscas/";

        if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_MONITORAMENTO
        || gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_COMPLIANCE
        || gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RISCO
        || (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SISTEMA)) 
        {   //sistemas com buscas diferentes por subsistema
            if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_MIGRACAO)
            {
                lUrlBusca += "Clientes_Assessores";
            }
            else
            {
                lUrlBusca += gGradIntra_Navegacao_SistemaAtual + "_" + gGradIntra_Navegacao_SubSistemaAtual;
            }
        }
        else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
        {
            if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_MIGRACAO)
            {
                lUrlBusca += "Clientes_Assessores";
            }
            else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_RESERVAIPO)
            {
                lUrlBusca += "Clientes_Reserva_IPO";
            }
            else
            {
                lUrlBusca += "Clientes";
            }
        }
        else if (gGradIntra_Navegacao_SistemaAtual == CONST_SUBSISTEMA_RELATORIOS)
        {
            lUrlBusca = "Clientes/Relatorios/Filtros";
        }
        else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SOLICITACOES) {

            if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_IPO) 
            {
                lUrlBusca += "Solicitacoes_IPO";
            } else {
                lUrlBusca += "Solicitacoes_VendasDeFerramentas";
            }
        }
        else
        {   //sistemas com busca única
            lUrlBusca += gGradIntra_Navegacao_SistemaAtual;
        }
    
        lUrlBusca += ".aspx";
    
        GradIntra_CarregarHtmlVerificandoErro( lUrlBusca
                                             , null
                                             , gGradIntra_Navegacao_PainelDeBuscaAtual.attr("id")
                                             , GradIntra_Navegacao_ExibirBusca_CallBack
                                             , { HabilitarMascaras   : true
                                               , HabilitarValidacoes : true
                                               , AtivarToolTips      : true
                                               , CustomInputs        : [ ".Busca_Formulario input[type='checkbox']"
                                                                       , ".Busca_Resultados table thead tr td input[type='checkbox']"]
                                               });
    }
}

function GradIntra_Navegacao_ExibirBusca_CallBack()
{
///<summary>[CallBack] Função de CallBack para GradIntra_Navegacao_ExibirBusca.</summary>
///<returns>void</returns>

    gGradIntra_Navegacao_PainelDeBuscaAtual
        .show()
        .find("input[type='text']:eq(0)")
            .focus();

    switch(gGradIntra_Navegacao_SistemaAtual)
    {
        case CONST_SISTEMA_CLIENTES:

            GradIntra_Busca_ConfigurarGridDeResultados_Clientes();
            break;

        case CONST_SISTEMA_SEGURANCA:

            GradIntra_Busca_ConfigurarGridDeResultados_Seguranca();
            break;

        case CONST_SISTEMA_RISCO:

            GradIntra_Busca_ConfigurarGridDeResultados_Risco();
            break;

        case CONST_SISTEMA_SOLICITACOES:

            if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_IPO) {
                //GradIntra_Busca_ConfigurarGridDeResultados_GerenciamentoIPO();
            } else {
                GradIntra_Busca_ConfigurarGridDeResultados_VendasDeFerramentas();
            }

            break;
        
    }
}

function GradIntra_Navegacao_ExibirFiltroDeRelatorio()
{
///<summary>Exibe o formulário de filtro de relatórios.</summary>
///<returns>void</returns>

    //gGradIntra_Navegacao_PainelDeBuscaAtual.removeClass(CONST_CLASS_CONTEUDOCARREGADO);
    
    var lUrlFiltro = gGradIntra_Navegacao_SistemaAtual + "/Relatorios/Filtros.aspx";
    
    if (gGradIntra_Navegacao_PainelDeBuscaAtual.hasClass(CONST_CLASS_CONTEUDOCARREGADO))
    {        
        GradIntra_Navegacao_ExibirBusca_CallBack();
    }
    else
    {
        GradIntra_CarregarHtmlVerificandoErro( lUrlFiltro
                                             , null
                                             , gGradIntra_Navegacao_PainelDeBuscaAtual.attr("id")
                                             , GradIntra_Navegacao_ExibirBusca_CallBack
                                             , { HabilitarMascaras   : true
                                               , HabilitarValidacoes : true
                                               , AtivarToolTips      : true
                                               , CustomInputs        : [ ".Busca_Formulario input[type='checkbox']"
                                                                       , ".Busca_Resultados table thead tr td input[type='checkbox']"]
                                               });
    }
}


function GradIntra_Navegacao_ExibirFiltroDeRelatorio_CallBack()
{
///<summary>[CallBack] Função de CallBack para GradIntra_Navegacao_ExibirFiltroDeRelatorio.</summary>
///<returns>void</returns>

    /*
    gGradIntra_Navegacao_PainelDeConteudoAtual
        .hide()
        .find(".ListaDeObjetos")
            .hide()
            .parent()
        .find(".MenuDoObjeto")
            .hide();

    gGradIntra_Navegacao_PainelDeBuscaAtual
        .show()
        .find("input[type='text']:eq(0)")
            .focus();
    */
}

function GradIntra_Navegacao_ExpandirTabelaDeBusca(pDivDeResultados, pIdDoGrid)
{
///<summary>Expande/Contrai a tabela de busca, para poder visualizar melhor os resultados.</summary>
///<param name="pDivDeResultados" type="Objeto_jQuery">Div de resultados que tem a tabela.</param>
///<param name="pIdDoGrid" type="Objeto_jQuery">ID do grid js que tem a tabela.</param>
///<returns>void</returns>

    //height: auto; position: absolute; z-index: 100; width: 780px; overflow: hidden;
    //width:99%

    if (pDivDeResultados.hasClass(CONST_CLASS_ITEM_EXPANDIDO))
    {
        pDivDeResultados
            .removeClass(CONST_CLASS_ITEM_EXPANDIDO)
            .attr("style", null);
            
        $("#" + pIdDoGrid).setGridHeight(96);                       // Essa é a função da API do grid que muda a altura dele
    }
    else
    {
        pDivDeResultados
            .addClass(CONST_CLASS_ITEM_EXPANDIDO)
            .css({
                    height:   "501px"
                 });

        $("#" + pIdDoGrid).setGridHeight(450);
    }
}

function GradIntra_Navegacao_ExpandirTabelaDeBuscaAbaixo(pDivDeResultados, pIdDoGrid) {
    ///<summary>Expande/Contrai a tabela de busca, para poder visualizar melhor os resultados.</summary>
    ///<param name="pDivDeResultados" type="Objeto_jQuery">Div de resultados que tem a tabela.</param>
    ///<param name="pIdDoGrid" type="Objeto_jQuery">ID do grid js que tem a tabela.</param>
    ///<returns>void</returns>

    //height: auto; position: absolute; z-index: 100; width: 780px; overflow: hidden;
    //width:99%

    if (pDivDeResultados.hasClass(CONST_CLASS_ITEM_EXPANDIDO)) {
        pDivDeResultados
            .removeClass(CONST_CLASS_ITEM_EXPANDIDO)
            .attr("style", "width: 1190px; margin-left: 0px; margin-top: 20px;");

        $("#" + pIdDoGrid).setGridHeight(96);                       // Essa é a função da API do grid que muda a altura dele
    }
    else {
        pDivDeResultados
            .addClass(CONST_CLASS_ITEM_EXPANDIDO)
            .css({
                height: "501px"
            });

        $("#" + pIdDoGrid).setGridHeight(450);
    }
}

/*

    Seção "2": Lista de objetos selecionados

*/

function GradIntra_Navegacao_AdicionarItemNaListaDeItensSelecionados(pItem, pSistema, pExpandirAposIncluir)
{
///<summary>Adiciona um item à lista de itens selecionados para edição.</summary>
///<param name="pItem" type="String_ou_Objeto_JSON">Objeto de cliente, pode ser em String serializada ou já instanciado como JSON, o efeito é o mesmo.</param>
///<param name="pSistema" type="String">Nome do sistema onde está a lista que pItem será adicionado. Se null, considera o sistema atual.</param>
///<param name="pExpandirAposIncluir" type="Boolean">Flag se deve expandir o item automaticamente após a inclusão na lista.</param>
///<returns>void</returns>

    var lItem, lItemJson;
    
    if (pSistema == null || pSistema == "")
        pSistema = gGradIntra_Navegacao_SistemaAtual;

    var lListaDeItens = gGradIntra_Navegacao_PainelDeConteudoAtual.find("ul.ListaDeObjetos");

    if (pItem.Id)
    {
        //cliente já está como objeto
        
        lItemJson = $.toJSON(pItem);
        lItem     = pItem;
    }
    else
    {
        //cliente está como string json (foi escolhido da lista de busca)
        //verificar se ele já existe na lista:
    
        var lInputsJson = lListaDeItens.find("input[Propriedade='json']");
        var lJson;

        for(var a = 0; a < lInputsJson.length; a++)
        {
            lJson = lInputsJson[a].value;

            if (lJson == pItem)
            {
                var lLi = $(lInputsJson[a]).parent();

                if (!lLi.hasClass(CONST_CLASS_ITEM_EXPANDIDO))
                    GradIntra_Navegacao_ExpandirItemSelecionado(lLi);

                return false;
            }
        }

        lItemJson = pItem;
        lItem     = $.evalJSON(pItem);

    }

    var lClasseTemplate = CONST_CLASS_TEMPLATE;
    
    //verifica se tem template para outros tipos de objeto

    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA)
    {
        lClasseTemplate = "Template_Seguranca_" + lItem.TipoDeObjeto;
    }
    else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
    {
        ///Para bindar as propriedades no discleamer lateral de detalhes de clientes 
        ///foi necessário "chumbar" as propriedades solicitadas no painel
        if (lItem.Assessor)           lItem.CodAssessor  = lItem.Assessor;
        if (lItem.CPF_CNPJ)           lItem.CPF          = lItem.CPF_CNPJ;
        if (lItem.DataCadastroString) lItem.DataCadastro = lItem.DataCadastroString;
    }

    var lNovoItem = lListaDeItens.find("li." + lClasseTemplate).clone();
    var lElemento, lPropriedade, lValor;

    lListaDeItens.children("li:eq(0)")
        .after(lNovoItem);

    lNovoItem.find("[Propriedade]").each(function()
    {
        lElemento    = $(this);
        lPropriedade = lElemento.attr("Propriedade");
        
        if (lPropriedade == "json")
        {
            lElemento.val(lItemJson);
        }
        else
        {
            lValor = eval("lItem." + lPropriedade);

            if (lPropriedade == "NomeCliente" && lValor.length > 26) //TODO: Magic number, número de caracteres que cabem
            {
                lElemento.attr("title", lValor);

                lValor = lValor.substr(0, 26) + " <small style='font-size:0.7em'>(...)</small>";
            }
            
            if (!lValor || lValor == "") lValor = "<small title='Não Disponível'>n/d</small>";

            if (lPropriedade == "CodBovespaComConta" && lValor != "" && lValor != "<small title='Não Disponível'>n/d</small>")
            {   //Informando se Conta está Ativa
                var TolTip;
                if (lItem.CodBovespaAtiva == "A")
                    TolTip = "Ativa";
                else
                    TolTip = "Inativa";
                lValor = "<span title = '"+TolTip+"' class = 'Conta_"+lItem.CodBovespaAtiva+"'>"+lValor+"</span>" ;
            }

            if (lPropriedade == "CodBMFComConta" && lValor != "" && lValor != "<small title='Não Disponível'>n/d</small>")
            {   //Informando se Conta está Ativa
                var TolTip;
                if (lItem.CodBMFAtiva == "A")
                    TolTip = "Ativa";
                else
                    TolTip = "Inativa";
                lValor = "<span title = '"+TolTip+"' class = 'Conta_"+lItem.CodBMFAtiva+"'>"+lValor+"</span>" ;
            }

            if ("Cise" == lPropriedade && eval("lItem." + lPropriedade) != "" && lValor.indexOf("Não Disponível") < 0)
                lElemento.show().parent().show().attr("title", lValor);
            else
                lElemento.html(lValor);
        }
    });

    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
    {   //--> se for cliente...
        
        lNovoItem.find("h3").attr("class", "Cliente_" + (lItem.TipoCliente == "PJ" ? "PJ" : lItem.Sexo));

        lNovoItem.find("ul.Detalhes li.Item_Doc label").html((lItem.TipoCliente == "PJ" ? "CNPJ:" : "CPF:"));
    }
    else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA)
    {   //--> se for objeto de segurança...

        lNovoItem.find("h3").attr("class", "Seguranca_" + lItem.TipoDeObjeto.charAt(0));
    }

    lNovoItem
        .removeClass(CONST_CLASS_TEMPLATE)
        .removeClass(lClasseTemplate)
        .addClass(CONST_CLASS_ITEM_ITEMDINAMICO)
        .bind("mouseenter", lstItensSelecionados_LI_MouseEnter)
        .bind("mouseleave", lstItensSelecionados_LI_MouseLeave);

    gGradIntra_Cadastro_ItemSendoEditado = lItem;

    if (pExpandirAposIncluir)
        lNovoItem.show();

    return lNovoItem;
}

function GradIntra_ExcluirItemDaListaDeItensSelecionados(pItem)
{
    ///<summary>void</returns>

    $('#tblBusca_Seguranca_Resultados').find("[title=\"" + gGradIntra_Cadastro_ItemSendoEditado.Id + "\"]").closest("tr").remove();

    $("#pnlConteudo_Seguranca_Busca").hide();

    $("#lstItemMenu_Seguranca").hide();

    pItem.remove();
}

/*

    Seção "3": Painel de conteúdo

*/

function GradIntra_Navegacao_CarregarConteudosDoSistemaSelecionado()
{
    var lHabilitarTudo = { CustomInputs        : []
                         , HabilitarMascaras   : true
                         , HabilitarValidacoes : true
                         , AtivarToolTips      : true}
                         
    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
    {
        if ( !$("#pnlConteudo_Clientes_Busca").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            GradIntra_CarregarHtmlVerificandoErro( "Clientes/Default.aspx"
                                                 , null
                                                 , "#pnlConteudo_Clientes_Busca"
                                                 , null
                                                 , lHabilitarTudo);
        }
    }
    
    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RISCO)
    {
        if (!$("#pnlConteudo_Risco_GruposDeRisco").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            GradIntra_CarregarHtmlVerificandoErro( "Risco/Default.aspx"
                                                 , null
                                                 , "#pnlConteudo_Risco_GruposDeRisco"
                                                 , null
                                                 , lHabilitarTudo);
        }

        if (!$("#pnlConteudo_Risco_GruposDeRiscoRestricoes").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) 
        {

            GradIntra_CarregarHtmlVerificandoErro("Risco/Restricoes.aspx"
                                                 , null
                                                 , "#pnlConteudo_Risco_GruposDeRiscoRestricoes"
                                                 , null
                                                 , lHabilitarTudo);
        }

        if (!$("#pnlConteudo_Risco_GruposDeRiscoRestricoesSpider").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) {

            GradIntra_CarregarHtmlVerificandoErro("Risco/RestricoesSpider.aspx"
                                                 , null
                                                 , "#pnlConteudo_Risco_GruposDeRiscoRestricoesSpider"
                                                 , null
                                                 , lHabilitarTudo);
        }
    }

    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_COMPLIANCE) 
    {
        if (!$("#pnlConteudo_Compliance_EstatisticaDayTrade").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) {
            GradIntra_CarregarHtmlVerificandoErro("Compliance/Default.aspx"
                                                 , null
                                                 , "#pnlConteudo_Compliance_EstatisticaDayTrade"
                                                 , null
                                                 , lHabilitarTudo);
        }
    }

    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA)
    {
        if ( !$("#pnlConteudo_Seguranca_Busca").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            GradIntra_CarregarHtmlVerificandoErro( "Seguranca/Default.aspx"
                                                 , null
                                                 , "#pnlConteudo_Seguranca_Busca"
                                                 , null
                                                 , lHabilitarTudo);
        }
    }

    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RELATORIOS)
    {
        if ( !$("#pnlClientes_FiltroRelatorio").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            GradIntra_CarregarHtmlVerificandoErro( "Clientes/Relatorios/Filtros.aspx"
                                                 , null
                                                 , "#pnlClientes_FiltroRelatorio"
                                                 , null
                                                 , lHabilitarTudo);
        }
    }
    
    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SISTEMA)
    {
         // se houver algum conteudo no default.aspx do sistema, tem que fazer chamada como os outros acima;
        //  como não há, basta adicionar a classe como se tivesse sido carregado:
        $("#pnlConteudo_Sistema_ObjetosDoSistema").addClass(CONST_CLASS_CONTEUDOCARREGADO);
        $("#pnlConteudo_Sistema_VariaveisDoSistema").addClass(CONST_CLASS_CONTEUDOCARREGADO);
        $("#pnlConteudo_Sistema_PessoasVinculadas").addClass(CONST_CLASS_CONTEUDOCARREGADO);
        $("#pnlConteudo_Sistema_PessoasExpostasPoliticamente").addClass(CONST_CLASS_CONTEUDOCARREGADO);
    }
    
    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SOLICITACOES)
    {
        if ( !$("#pnlConteudo_Solicitacoes_VendasDeFerramentas").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            GradIntra_CarregarHtmlVerificandoErro( "Solicitacoes/Default.aspx"
                                                 , null
                                                 , "#pnlConteudo_Solicitacoes_VendasDeFerramentas"
                                                 , null
                                                 , lHabilitarTudo);
        }
    }
    
}

function GradIntra_Navegacao_ExibirConteudo()
{
///<summary>Exibe o conteúdo relativo ao sistema atual.</summary>
///<returns>void</returns>

    if (gGradIntra_Navegacao_PainelDeConteudoAtual.hasClass(CONST_CLASS_CONTEUDOCARREGADO))
    {
        GradIntra_Navegacao_ExibirConteudo_CallBack();
    }
    else
    {
        GradIntra_CarregarHtmlVerificandoErro( gGradIntra_Navegacao_SistemaAtual + "/Default.aspx"
                                             , null
                                             , gGradIntra_Navegacao_PainelDeConteudoAtual.attr("id")
                                             , GradIntra_Navegacao_ExibirConteudo_CallBack
                                             , { CustomInputs        : []
                                               , HabilitarMascaras   : true
                                               , HabilitarValidacoes : true
                                               , AtivarToolTips      : true
                                               });
    }
}

function GradIntra_Navegacao_ExibirConteudo_CallBack()
{
///<summary>[CallBack] Função de CallBack para GradIntra_Navegacao_ExibirConteudo.</summary>
///<returns>void</returns>

    gGradIntra_Navegacao_PainelDeConteudoAtual.show();

    var lListaDeObjetos = gGradIntra_Navegacao_PainelDeConteudoAtual.find(".ListaDeObjetos");

    if (lListaDeObjetos.length > 0
    && (lListaDeObjetos.find("li." + CONST_CLASS_ITEM_ITEMDINAMICO).length > 0))
    {
        lListaDeObjetos
            .show()
            .next(".MenuDoObjeto")
                .show();
    }

    switch(gGradIntra_Navegacao_SubSistemaAtual)
    {
        case CONST_SUBSISTEMA_PESSOASVINCULADAS:

            GradIntra_Sistema_ExibirConteudo();

            break;

        case CONST_SUBSISTEMA_PESSOASEXPOSTASPOL:

            GradIntra_Sistema_ExibirConteudo();

            break;

        case CONST_SUBSISTEMA_ASSOCIAR_PERMISSOES:

            GradIntra_Seguranca_ExibirAssociacoes();

            break;

    }
}

/*

    Seção "3 A": Lista de objetos selecionados

*/

function GradIntra_Navegacao_ExpandirItemSelecionado(pLi)
{
///<summary>Expande os dados do item cujo pLi foi indicado.</summary>
///<param name="pLi" type="Objeto_jQuery">Item da lista que deve ser expandido.</param>
///<returns>void</returns>

    var lLista = gGradIntra_Navegacao_PainelDeConteudoAtual.find("ul.ListaDeObjetos");
    
    var lItemExpandido = lLista.find("li." + CONST_CLASS_ITEM_EXPANDIDO);
    
    if (lItemExpandido.hasClass("NovoItem"))
    {   //--> está incluindo um novo item
        GradIntra_ExibirMensagem("I", "Favor terminar de incluir o novo item, ou cancelar a operação", true);

        return;
    }
    
    // Remove a classe expandida
    lItemExpandido.removeClass("Expandida").css({opacity: 0.5});

    // Abre o que foi selecionado:
    
    pLi.addClass(CONST_CLASS_ITEM_EXPANDIDO);

    gGradIntra_Navegacao_PainelDeConteudoAtual
        .show()
        .find("div.pnlFormulario")
        .hide()
            .find("." + CONST_CLASS_CONTEUDOCARREGADO)
                .removeClass(CONST_CLASS_CONTEUDOCARREGADO);

    lLista.show();
    
    gGradIntra_Navegacao_PainelDeConteudoAtual.find("ul.MenuDoObjeto")
        .show()
            .find("li." + CONST_CLASS_ITEM_SELECIONADO)
                .removeClass(CONST_CLASS_ITEM_SELECIONADO);

    gGradIntra_Cadastro_ItemSendoEditado = $.evalJSON(pLi.find("input[Propriedade='json']").val());

    GradIntra_Navegacao_ReverterTipoDeObjetoAtual(gGradIntra_Cadastro_ItemSendoEditado);

    GradIntra_Navegacao_VerificarNecessidadeDeMenuPersonalizado();

    window.setTimeout(function() { pLi.css({opacity: 1}); }, 300);
}

function GradIntra_Navegacao_ExpandirDadosDoPrimeiroItemSelecionado()
{
    ///<summary>Expande o primeiro item selecionado que estiver na lista.</summary>
    ///<returns>void</returns>

    var lItem = gGradIntra_Navegacao_PainelDeConteudoAtual.find("ul.ListaDeObjetos li[class~='" + CONST_CLASS_ITEM_ITEMDINAMICO + "']:eq(0)");

    if (lItem.length > 0)
        GradIntra_Navegacao_ExpandirItemSelecionado( lItem  );
}

function GradIntra_Navegacao_ReverterTipoDeObjetoAtual(pObjeto)
{
    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
    {
        if (pObjeto == null)
        {   //--> quando está incluindo
            pObjeto = { TipoCliente: gGradIntra_Navegacao_SubSistemaAtual.replace("Novo_", "") };
        }

        GradIntra_Clientes_ReverterTipoDeClienteAtual(pObjeto.TipoCliente, true, pObjeto);
    }
    else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA)
    {
        if (pObjeto == null)
        {   //--> quando está incluindo
            pObjeto = { TipoDeObjeto: gGradIntra_Navegacao_SubSistemaAtual.replace("Novo_", "") };
        }

        GradIntra_Seguranca_ReverterTipoDeObjetoDeSegurancaAtual(pObjeto.TipoDeObjeto, true);
    }
    else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RISCO)
    {
        pObjeto.TipoDeObjeto = "Grupo";

        if (pObjeto == null)
        {
            //quando está incluindo
            pObjeto = { TipoDeObjeto: "" };
                
            if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_NOVO_PARAMETRODERISCO)
                pObjeto.TipoDeObjeto = "Parametro";
                
            else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_NOVO_PERMISSAODERISCO)
                pObjeto.TipoDeObjeto = "Permissao";
            
            else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_NOVO_GRUPODERISCO)
                pObjeto.TipoDeObjeto = "Grupo";
        }

        GradIntra_Risco_ReverterTipoDeObjetoDeRiscoAtual(pObjeto.TipoDeObjeto, true);
    }
}

/*

    Seção "3 C": Formulários "locais" (dados, ações, etc)

*/

function GradIntra_Navegacao_ExibirFormulario(pUrl, pCallBack, pDados)
{
    if (pUrl.indexOf(".") == -1)
    {
        console.log("GradIntra_Navegacao_ExibirFormulario(pURL)> pUrl inválida: [" + pUrl + "]");

        return false;
    }

    var lCaminho = pUrl.substr(0, pUrl.indexOf(".")).replace(/\//gi, "_");

    var lUrl = gGradIntra_Navegacao_SistemaAtual + "/" + pUrl;

    var lIdDoDivFormulario = "pnl" + gGradIntra_Navegacao_SistemaAtual + "_" + lCaminho;

    var lDivFormulario = $("#" + lIdDoDivFormulario);

    if (lDivFormulario.length == 0)
    {
        console.log("GradIntra_Navegacao_ExibirFormulario(pURL)> Sem pnlFormulario para [" + lIdDoDivFormulario + "]");

        return false;
    }

    if (lUrl.indexOf("DadosCompletos") != -1)
    {
        if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
        {   //--> diferenciar entre PF/PJ
            lUrl = lUrl.replace("DadosCompletos", "DadosCompletos_" + gGradIntra_Clientes_TipoDeClienteAtual);
        }
        
        else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA)
        {   //--> diferenciar entre Usuario, Grupo ou Perfil
            lUrl = lUrl.replace("DadosCompletos", "DadosCompletos_" + gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual);
        }

        else if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RISCO)
        {   //--> diferenciar entre Parametro, Permissão e Grupo
            lUrl = lUrl.replace("DadosCompletos", "DadosCompletos_" + gGradIntra_Risco_TipoDeObjetoDeRiscoAtual);
        }
    }

    var pnlFormulario = lDivFormulario.parent();

    if (lDivFormulario.hasClass(CONST_CLASS_CONTEUDOCARREGADO)
    && (gGradIntra_Navegacao_SistemaAtual != CONST_SISTEMA_RISCO))
    {
        if (!lDivFormulario.is(":visible"))
        {
            if (lDivFormulario.hasClass(CONST_CLASS_CAMPOSPREENCHIDOS))
            {
                pnlFormulario.children("div").hide();

                lDivFormulario.fadeIn(300);
            }
            else
            {   //os campos foram limpados porque clicou em um outro cliente, carregar só os dados

                pnlFormulario.children("div").hide();

                lDivFormulario.fadeIn(300);

                if (pCallBack)
                    pCallBack();
            }
        }
    }
    else
    {
        pnlFormulario
            .addClass(CONST_CLASS_CARREGANDOCONTEUDO)
            .children("div")
                .hide()
                .parent()
            .find(".Aguarde")
                .attr("style", null);

        window.setTimeout(function () 
        {
            if (!pDados || pDados == null) 
            {
                if (gGradIntra_Cadastro_ItemSendoEditado != null) 
                {
                    pDados = { Acao: "CarregarHtmlComDados", Id: gGradIntra_Cadastro_ItemSendoEditado.Id, CodBovespa: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa, CodBMF: gGradIntra_Cadastro_ItemSendoEditado.CodBMF, TipoDeObjeto: gGradIntra_Cadastro_ItemSendoEditado.TipoDeObjeto, DsCpfCnpj:gGradIntra_Cadastro_ItemSendoEditado.CPF  };
                }
                else
                {
                    pDados = { Acao: "CarregarHtml" };
                }
            }

            GradIntra_Navegacao_CarregarFormulario(lUrl, lIdDoDivFormulario, pDados, pCallBack);

        }, gGradIntra_Config_DelayDeRequest);
    }

    gGradIntra_Navegacao_PainelDeConteudoAtual
        .find("ul.MenuDoObjeto")
        .find("li")
            .removeClass(CONST_CLASS_ITEM_SELECIONADO)
            .find("a[href='" + pUrl + "']")
                .parent()
            .addClass(CONST_CLASS_ITEM_SELECIONADO);

    pnlFormulario.show();
}

function GradIntra_Navegacao_CarregarFormulario(pUrl, pDivParaCarregar, pDadosDoRequest, pCallBack)
{
///<summary>Exibe um formulário dentro do painel de formulários, verificando se é necessário fazer request ou não.</summary>
///<param name="pUrl"  type="String">URL para fazer a chamada ajax.</param>
///<param name="pDivParaCarregar" type="String_ou_Objeto_jQuery">Div que receberá o formulário.</param>
///<param name="pDadosDoRequest" type="Objeto_JSON">(opcional) Objeto de dados para a request ajax, é enviado completo para a chamada $.ajax(). Se não for passado, assume-se {Acao: 'CarregarHtml'}.</param>
///<param name="pCallBack" type="Função_Javascript">(opcional) Função de callback quando o formulário for exibido.</param>
///<returns>void</returns>
    
    var lDivId;
    
    if (pDivParaCarregar.attr)
    {
        lDivId = pDivParaCarregar.attr("id");
    }
    else
    {
        lDivId = pDivParaCarregar;
    }
    
    if (lDivId.charAt(0) != "#")
        lDivId = "#" + lDivId;

    GradIntra_CarregarHtmlVerificandoErro( pUrl
                                         , pDadosDoRequest
                                         , lDivId
                                         , function(pResposta) 
                                           { 
                                             GradIntra_Navegacao_CarregarFormulario_CallBack(lDivId, pCallBack); 
                                           } 
                                         , { HabilitarMascaras   : true
                                           , HabilitarValidacoes : true
                                           , AtivarToolTips      : true
                                           , AtivarAutoComplete  : true
                                           , CustomInputs        : [ lDivId + " input[type='checkbox']"
                                                                   , lDivId + " input[type='radio']"
                                                                   , "table.AvaliacaoDaExperiencia tbody"]
                                           }
                                      );
}

function GradIntra_CadastroPF_LoadPage()
{
    if (gGradIntra_Navegacao_SubSistemaAtual != CONST_SUBSISTEMA_NOVO_PF)
    {
        try
        {
            var lItem = JSON.parse(  $("#hidDadosCompletos_PF").val() );

            if(lItem && lItem.Assessor)
            {
                $("#cboClientes_DadosCompletos_Assessor")
                    .removeClass("validate[required]")
                    .val(lItem.Assessor)
                    .prop("disabled", true);
            }
        }
        catch(erro){}
    }

    GradIntra_Cadastro_InabilitarEstadoQuandoEstrangeiro
    (
          $("#cboClientes_DadosCompletos_Nacionalidade")
        , $("#cboClientes_DadosCompletos_EstadoDeNascimento")
    );

    GradIntra_Cadastro_InabilitarDadosComerciaisPF
    (
          $("#cboClientes_DadosCompletos_Profissao")
        , $('#txtClientes_DadosCompletos_CargoAtual')
        , $('#txtClientes_DadosCompletos_Empresa')
    );

    GradIntra_Cadastro_EstadoCivil
    (
          $("#cboClientes_DadosCompletos_EstadoCivil")
        , $("#txtClientes_DadosCompletos_Conjuge")
    )

    if ($("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").next().hasClass("checked"))
    {
        $("#divNaoOperaPorContaPropria").show();
    }

//    GradIntra_Cadastro_InabilitarDadosComerciaisPF
//    (
//          $('#cboClientes_DadosCompletos_Profissao')
//        , $('#txtClientes_DadosCompletos_CargoAtual')
//        , $('#txtClientes_DadosCompletos_Empresa')
//        , $('#txtClientes_DadosCompletos_RamoDeAtividade')
//        , $('#txtClientes_DadosCompletos_EmailComercial')
//    );
}

function GradIntra_CadastroPJ_LoadPage()
{
    if (gGradIntra_Navegacao_SubSistemaAtual != CONST_SUBSISTEMA_NOVO_PJ)
    {
        $("#cboClientes_DadosCompletos_Assessor")
            .removeClass("validate[required]")
            .val(gGradIntra_Cadastro_ItemSendoEditado.CodAssessor)
            .prop("disabled", true);
    }
}

function GradIntra_Navegacao_CarregarFormulario_CallBack(pDivId, pCallBack)
{
///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Clientes_CarregarFormulario: faz o 'fade' do 'Aguarde' e exibe o formulário que foi carregado.</summary>
///<param name="pDivId" type="String">Id do div onde o conteúdo foi carregado.</param>
///<param name="pCallBack" type="Função_Javascript">(opcional) Função de callback quando o formulário for exibido.</param>
///<returns>void</returns>

    var lDiv = $(pDivId);
    
    if (lDiv.parent().hasClass(CONST_CLASS_CARREGANDOCONTEUDO))
    {   // primeira vez que carregou
        
        var lDadosJson;
        
        lDadosJson = lDiv.find("input[type='hidden'].DadosJson").val();
        
        if (lDadosJson && lDadosJson != "")
        {
            GradIntra_ExibirObjetoNoFormulario($.evalJSON(lDadosJson), lDiv);
            
            lDiv.addClass(CONST_CLASS_CAMPOSPREENCHIDOS);

            //IFs para quando alguma função precisa ser rodada após o 'objeto sendo editado' ser carregado no form.
            // (equivalente a um Page_Load por exemplo).

            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES && gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_BUSCA)
            {
                if (gGradIntra_Cadastro_ItemSendoEditado.TipoCliente == "PF")
                {
                    GradIntra_CadastroPF_LoadPage();
                }
                else if (gGradIntra_Cadastro_ItemSendoEditado.TipoCliente == "PJ")
                {
                    GradIntra_CadastroPJ_LoadPage();
                }
            }

            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA
            && (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_BUSCA)
            && (gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual == CONST_SEGURANCA_TIPODEOBJETO_USUARIO))
            {
                cboSeguranca_DadosCompletos_Usuario_TipoAcesso_Change($("#cboSeguranca_DadosCompletos_Usuario_TipoAcesso"))
            }
        }

        lDadosJson = lDiv.find("input[type='hidden'].ListaJson").val();
        
        if (lDadosJson && lDadosJson != "")
        {
            //preenche a lista de filhos
            lDadosJson = $.evalJSON(lDadosJson);
            
            lDiv.find("table.Lista tbody tr.ItemDinamico").remove();

            $(lDadosJson).each(function()
            {
                GradIntra_ExibirObjetoNaLista(this, lDiv.find(".pnlFormulario_Campos"));
            });
        }
        
        lDiv.hide();
        
        var lDistancia = -50;

        if (lDiv.attr("id") == "pnlNovoItem_Formulario")
            lDistancia = -200;

        lDiv
            .parent()
                .find(".Aguarde")
                .show()
                .animate
                (
                      {marginTop: lDistancia, opacity:0}
                    , {
                          duration:      400
                        , specialEasing: { marginTop: "easeInQuad", opacity: "easeOutCubic" }
                        , complete: function()
                                    {
                                        $(this).hide();

                                        lDiv.fadeIn(300);

                                        $("." + CONST_CLASS_CARREGANDOCONTEUDO).removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

                                        if (pCallBack)
                                            pCallBack();
                                    }
                      }
                );
    }
    else
    {
        if (pCallBack)
            pCallBack();
    }
}


function GradIntra_Navegacao_ExibirFormularioDeNovoItem(pURL)
{
    var lURL;

    switch (gGradIntra_Navegacao_SubSistemaAtual) 
    {

        case CONST_SUBSISTEMA_NOVO_PF :                lURL = "Clientes/Formularios/Dados/DadosCompletos_PF.aspx";        break;

        case CONST_SUBSISTEMA_NOVO_PJ :                lURL = "Clientes/Formularios/Dados/DadosCompletos_PJ.aspx";        break;
        
        case CONST_SUBSISTEMA_NOVO_CB :                lURL = "Clientes/Formularios/Dados/DadosCompletos_CB.aspx";        break;

        case CONST_SUBSISTEMA_NOVO_USUARIO :           lURL = "Seguranca/Formularios/Dados/DadosCompletos_Usuario.aspx";  break;

        case CONST_SUBSISTEMA_NOVO_GRUPO :             lURL = "Seguranca/Formularios/Dados/DadosCompletos_Grupo.aspx";    break;

        case CONST_SUBSISTEMA_NOVO_PERFIL :            lURL = "Seguranca/Formularios/Dados/DadosCompletos_Perfil.aspx";   break;

        case CONST_SUBSISTEMA_NOVO_PERMISSAOSEGURANCA: lURL = "Seguranca/Formularios/Dados/DadosCompletos_PermissaoSeguranca.aspx";   break;

        case CONST_SUBSISTEMA_NOVO_GRUPODERISCO:       lURL = "Risco/Formularios/Dados/DadosCompletos_Grupo.aspx";        break;

        case CONST_SUBSISTEMA_NOVO_PARAMETRODERISCO:   lURL = "Risco/Formularios/Dados/DadosCompletos_Parametro.aspx";    break;

        case CONST_SUBSISTEMA_NOVO_PERMISSAODERISCO:   lURL = "Risco/Formularios/Dados/DadosCompletos_Permissao.aspx";    break;
    }
    
    if(pURL != "" && pURL !== undefined)
    {
        lURL = pURL;
    }

    $("#pnlConteudo input, #pnlConteudo a, #pnlConteudo select, #pnlConteudo button")
        .addClass(CONST_CLASS_BLOQUEADOPORINCLUSAO)
        .prop("disabled", true);
        
    $("#pnlNovoItem").show();

    $("#pnlNovoItem > div.pnlFormulario")
        .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
        .addClass(CONST_CLASS_CARREGANDOCONTEUDO);

    $("#pnlNovoItem_Formulario")
        .html("")
        .show();

    GradIntra_Navegacao_CarregarFormulario(lURL, "#pnlNovoItem_Formulario");
}

function GradIntra_Navegacao_OcultarFormularioDeNovoItem(pSelecionarProximo)
{
    gGradIntra_Cadastro_FlagIncluindoNovoItem = false;

    $("." + CONST_CLASS_BLOQUEADOPORINCLUSAO)
        .prop("disabled", false);

    $("#pnlNovoItem")
        .hide()
        .find(".Aguarde")
            .attr("style", null);

    if (pSelecionarProximo)
        $("ul.Sistema_" + gGradIntra_Navegacao_SistemaAtual + " li:first a").click();
}


function GradIntra_Navegacao_VerificarNecessidadeDeMenuPersonalizado()
{
    var lMenuRolavel =  gGradIntra_Navegacao_PainelDeConteudoAtual.find("ul.MenuRolavel");

    if (lMenuRolavel.length > 0)
    {
        //tem menu rolável, verifica se existem itens demais, que precise de rolagem:

        var lQuantidadeDeLiVisiveis = lMenuRolavel.find("li:visible");

        if (lQuantidadeDeLiVisiveis.length > 19)
        {
            //precisa rolar
            if (lMenuRolavel.css("marginTop") == "0px")
            {
                //ainda não rolou nada, mostra o botão de baixo
                lMenuRolavel.parent().closest("ul").find("li.Fundo").children("a").show();
            }
            else
            {
                //já rolou pra cima, rola o botão de cima
                lMenuRolavel.parent().closest("ul").find("li.Topo").children("a").show();
            }
        }
        else
        {
            //não precisa, esconde as setas
            
            lMenuRolavel.parent().closest("ul").find("li.TopoFundo").children("a").hide();
        }
    }
}

function chkGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue_Click(pSender)
{
    chkGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue(pSender);

    return false;
}

function txtGradIntra_Risco_ClienteGrupo_Cliente_KeyUp(pSender)
{
    GradIntra_Risco_ClienteGrupo_Cliente(pSender);

    return false;
}

function cmbGradIntra_Risco_ClienteGrupo_Assessor_Change(pSender)
{
    GradIntra_Risco_ClienteGrupo_Assessor(pSender);

    return false;
}

function btnGradIntra_Risco_ClienteGrupo_Gravar_Click(pSender)
{
    GradIntra_Risco_ClienteGrupo_Gravar(pSender);

    return false;
}

function btnGradIntra_Risco_ManutencaoCliente_Buscar_Click(pSender)
{
    GradIntra_Risco_ManutencaoCliente_Buscar_Click(pSender);

    return false;
}

function txtGradIntra_Risco_ManutencaoCliente_Cliente_KeyUp(pSender)
{
    GradIntra_Risco_ManutencaoCliente_Cliente_KeyUp(pSender);

    return false;
}

function cmbGradIntra_Risco_ManutencaoCliente_Assessor_Change(pSender)
{
    GradIntra_Risco_ManutencaoCliente_Assessor_Change(pSender);

    return false;
}

function btnCliente_limites_Bovespa_AtualizarDados_Click(pSender)
{
    Cliente_limites_Bovespa_AtualizarDados(pSender);

    return false;
}

function chkGradIntra_Clientes_Produtos_DadosPlanosCliente_Click(pSender)
{
    GradIntra_Clientes_Produtos_DadosPlanosCliente_Click(pSender);

    return false;
}

function btnGradIntra_Cliente_Produto_Detalhes_Click(pSender)
{
    GradIntra_Cliente_Produto_Detalhes_Click(pSender);

    return false;
}

function btnSolicitacao_AtualizarResgate_Click(pSender)
{
    Solicitacao_AtualizarResgate(pSender);

    return false;
}

function btnSolicitacao_AtualizarAplicacao_Click(pSender)
{
    Solicitacao_AtualizarAplicacao_Click(pSender);

    return false;
}

function btnBuscar_AplicacaoResgate_Click(pSender)
{
    Buscar_AplicacaoResgate_Click(pSender);

    return false;
}

function btnBuscarProduto_Click(pSender)
{
    BuscarProduto_Click(pSender);

    return false;
}

function btnPoupeCompra_Click(pSender)
{
    PoupeCompra_Click(pSender);

    return false;
}

function GradIntra_FiltrosDBM_PageLoad(pSender)
{
    GradIntra_RelatoriosDBM_DesabilitarFiltrosParaAssessores(pSender);

    return false;
}

function chkRisco_MonitoramentoRisco_AtualizarAutomaticamente_Click(pSender)
{
    GradIntra_Risco_MonitoramentoDeRisco_AtualizarAutomaticamente_Click(pSender);
}

function btnRisco_Monitoramento_Busca_Click(pSender)
{
    GradIntra_Risco_Monitoramento_Busca_Click(pSender);

    return false;
}

function btnRisco_Monitoramento_ExportarParaExcel_Click()
{
    GradIntra_Risco_Monitoramento_ExportarParaExcel();

    return false;
}

function btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(pSender)
{
    GradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra(pSender);

    return false;
}

function cboClientes_FiltroRelatorio_Custodia_Mercado_OnChange(pSender)
{
    Clientes_FiltroRelatorio_Custodia_Mercado_OnChange(pSender);

    return false;
}

var lPopUp;

var lPopUpIntradiario;

function btnRisco_Monitoramento_Intradiario_Busca_Click(pSender) 
{
    var lCodigoCliente  = $("#txtBuscar_Monitoramento_Intradiario_CodigoCliente").val();

    var lCodigoAssessor = $("#cboBM_Monitoramento_Intradiario_CodAssessor").val();

//    if (lCodigoCliente == "" && lCodigoAssessor == "") 
//    {
//        alert("É necessário selecionar um assessor ou inserir o código de um cliente");
//        return false;
//    }

    var lNETVOLUME = $("input[name='chkRisco_Monitoramento_Intradiario_NET']:checked").val();

    var lEXPxPosicao = $("input[name='chkRisco_Monitoramento_Intradiario_FiltroEXPxPosicao']:checked").val();

    var lNETxSFP = $("input[name='chkRisco_Monitoramento_Intradiario_NETxSFP']:checked").val();

    var lDe = $("#txtBusca_Monitoramento_Intraday_DataInicial").val();

    var lAte = $("#txtBusca_Monitoramento_Intraday_DataFinal").val();

    var lObjetoDeParametros = { Acao: "BuscarSuitabilityLavagem"
                                  , CodigoCliente: lCodigoCliente
                                  , CodAssessor  : lCodigoAssessor
                                  , NETVOLUME    : lNETVOLUME
                                  , EXPxPosicao  : lEXPxPosicao
                                  , NETxSFP: lNETxSFP
                                  , DataDe: lDe
                                  , DataAte: lAte 
                              };

   // var lConsulta = "?CodigoCliente=" + lCodigoCliente + "&CodAssessor=" + lCodigoAssessor + "&ProporcaoPrejuizo=" + lProporcaoPrejuizo + "&EXPxPosicao=" + lEXPxPosicao + "&Acao=BuscarItensParaSelecao";

    if (lPopUpIntradiario == null) 
    {
        lPopUpIntradiario = window.open('Risco/Formularios/Dados/MonitoramentoIntradiario.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
    }

    if (lPopUpIntradiario.GradIntra_Risco_Monitoramento_Intradiario_Busca == null) {
        setTimeout(btnRisco_Monitoramento_Intradiario_Busca_Click, 1000);

    }
    else {
        lPopUpIntradiario.GradIntra_Risco_Monitoramento_Intradiario_Busca(pSender, lObjetoDeParametros);
        lPopUpIntradiario = null;
    }

    return false;
}
var lPopUpSuitabilityLavagem = null;

function btnRisco_Suitability_Lavagem_Busca_Click(pSender) 
{
    var lCodigoCliente     = $("#txtBuscar_Suitability_Lavagem_CodigoCliente").val();

    var lCodigoAssessor    = $("#cboBM_Suitability_Lavagem_CodAssessor").val();

    var lPercentualVOLxSFP = $("input[name='chkRisco_Suitability_Lavagem_VOLxSFP']:checked").val();

    var lVolume            = $("input[name='chkRisco_Suitability_Lavagem_Volume']:checked").val();

    var lEnquadrado        = $("input[name='chkRisco_Suitability_Lavagem_Enquadrado']:checked").val();

    var lDe                = $("#txtBusca_Suitability_Lavagem_DataInicial").val();

    var lAte               = $("#txtBusca_Suitability_Lavagem_DataFinal").val();

    var lStartDate         = new Date(lDe.substring(6, 10) , parseInt(lDe.substring(3, 5)-1) , lDe.substring(0, 2));

    var lEndDate           = new Date(lAte.substring(6, 10), parseInt(lAte.substring(3, 5)-1) , lAte.substring(0, 2));

    var lDiaAtual          = new Date();

    var lHoje              = new Date(lDiaAtual.getFullYear(), lDiaAtual.getMonth(), lDiaAtual.getDate());

    if (lStartDate > lEndDate) 
    {
        alert("Data de filtro inválida");
        return false;
    }

    if (lEndDate >= lHoje || lStartDate >= lHoje) 
    {
        alert("Não é possível consultar dados do dia atual");
        return false;
    }

    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao"
                                  , CodigoCliente:      lCodigoCliente
                                  , CodAssessor:        lCodigoAssessor
                                  , PercentualVOLxSFP:  lPercentualVOLxSFP
                                  , Enquadrado:         lEnquadrado
                                  , Volume:             lVolume
                                  , DataDe:             lDe
                                  , DataAte:            lAte
    };

    if (lPopUpSuitabilityLavagem == null) 
    {
        lPopUpSuitabilityLavagem = window.open('Risco/Formularios/Dados/SuitabilityLavagem.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
    }

    if (lPopUpSuitabilityLavagem.GradIntra_Risco_Suitability_Lavagem_Buscar == null) 
    {
        setTimeout(btnRisco_Suitability_Lavagem_Busca_Click, 1000);
    }
    else 
    {
        //lPopUpSuitabilityLavagem.GradIntra_Compliance_Churning_Intraday_Buscar(pSender, lObjetoDeParametros);
        lPopUpSuitabilityLavagem.GradIntra_Risco_Suitability_Lavagem_Buscar(pSender, lObjetoDeParametros);
        lPopUpSuitabilityLavagem = null;
    }

    return false;
}

var lPopUpChurning = null;
function btnCompliance_Churning_Intraday_Busca_Click(pSender) 
{
    var lCodigoCliente = $("#txtBuscar_Churning_Intraday_CodigoCliente").val();

    var lCodigoAssessor = $("#cboBM_Churning_Intraday_CodAssessor").val();

    var lPercentualTR = $("input[name='chkRisco_Churning_Intraday_PercentalTR']:checked").val();

    var lPercentualCE = $("input[name='chkRisco_Churning_Intraday_PercentualCE']:checked").val();

    var lTotalCompras = $("input[name='chkRisco_Churning_Intraday_TotalCompras']:checked").val();

    var lTotalVendas = $("input[name='chkRisco_Churning_Intraday_TotalVendas']:checked").val();

    var lCarteiraMedia = $("input[name='chkRisco_Churning_Intraday_CarteiraMedia']:checked").val();

    var lDe = $("#txtBusca_Churning_Intraday_DataInicial").val();

    var lAte = $("#txtBusca_Churning_Intraday_DataFinal").val();

    var lPorta = $("#txtBuscar_Churning_Intraday_Porta").val();

    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao"
                                  , CodigoCliente: lCodigoCliente
                                  , CodAssessor  : lCodigoAssessor
                                  , PercentualTR : lPercentualTR
                                  , PercentualCE : lPercentualCE
                                  , DataDe       : lDe
                                  , DataAte      : lAte
                                  , TotalCompras : lTotalCompras
                                  , TotalVendas  : lTotalVendas
                                  , CarteiraMedia: lCarteiraMedia
                                  , Porta        : lPorta
    };

    if (lPopUpChurning == null) 
    {
        lPopUpChurning = window.open('Compliance/Formularios/Dados/Churning.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
    }

    if (lPopUpChurning.GradIntra_Compliance_Churning_Intraday_Buscar == null) 
    {
        setTimeout(btnCompliance_Churning_Intraday_Busca_Click, 1000);
    }
    else 
    {
        lPopUpChurning.GradIntra_Compliance_Churning_Intraday_Buscar(pSender, lObjetoDeParametros);
        lPopUpChurning = null;
    }

    return false;



}
function btnRisco_Monitoramento_LucroPrejuizo_Busca_Click(pSender) 
{
    var lCodigoCliente      = $("#txtDBM_FiltroRelatorio_CodigoCliente").val();

    var lCodigoAssessor     = $("#cboBM_FiltroRelatorio_CodAssessor").val();

    var lProporcaoPrejuizo  = $("input[name='chkRisco_Monitoramento_FiltroProporcaoPrejuizo']:checked").val();

    var lSemaforo           = $("input[name='chkRisco_Monitoramento_FiltroSemaforo']:checked").val();

    

    if (lSemaforo == undefined)
    {
        lSemaforo = '';
    }

    if (lProporcaoPrejuizo == undefined)
    {
        lProporcaoPrejuizo = '';
    }

    var lObjetoDeParametros    = { Acao              : "BuscarItensParaSelecao"
                                  , CodigoCliente    : lCodigoCliente
                                  , CodAssessor      : lCodigoAssessor
                                  , ProporcaoPrejuizo: lProporcaoPrejuizo
                                  , Semaforo         : lSemaforo
                                 };

    var lConsulta           = "?CodigoCliente=" + lCodigoCliente + "&CodAssessor=" + lCodigoAssessor + "&ProporcaoPrejuizo=" + lProporcaoPrejuizo + "&Semaforo=" + lSemaforo+"&Acao=BuscarItensParaSelecao";

    if (lPopUp == null) 
    {
        lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoRiscoGeral.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
    }

    if (lPopUp.GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca == null) 
    {
        setTimeout(btnRisco_Monitoramento_LucroPrejuizo_Busca_Click, 1000);
        
    }
    else 
    {
        lPopUp.GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca(pSender, lObjetoDeParametros);
        lPopUp = null;
    }
    
    return false;
}

function lnkGradIntra_Risco_Monitoramento_LucrosPrejuizos_OrdernarColuna_Click(pSender)
{
    GradIntra_Risco_Monitoramento_LucrosPrejuizos_OrdernarColuna(pSender);

    return false;
}

function lnkGradIntra_Risco_Monitoramento_LucrosPrejuizos_SelecionarTodos_Click(pSender)
{
    GradIntra_Risco_Monitoramento_LucrosPrejuizos_SelecionarTodos(pSender);

    return true;
}

function cmbGradIntra_Risco_MonitoramentoLucrosPrejuizos_FiltroOrigem_OnChange(pSender)
{
    GradIntra_Risco_MonitoramentoLucrosPrejuizos_FiltroOrigem(pSender);

    return true;
}

function btnGradIntra_Risco_Monitoramento_LucrosPrejuizos_ExportarParaExcel_Click()
{
    GradIntra_Risco_Monitoramento_LucrosPrejuizos_ExportarParaExcel();
    
    return false;
}

function chkRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente_Click(pSender)
{
    Risco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente(pSender);

    return true;
}

function chkRisco_Monitoramento_Intradiario_AtualizarAutomaticamente_Click(pSender) 
{
    Risco_Monitoramento_Intradiario_AtualizarAutomaticamente(pSender);

    return true;
}

function chkRisco_Compliance_AtualizarAutomaticamente_Click(pSender) 
{
    Compliance_Churning_AtualizarAutomaticamente(pSender);

    return true;
}

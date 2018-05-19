/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />

var gGradIntra_Risco_TipoDeObjetoDeRiscoAtual = CONST_RISCO_TIPODEOBJETO_GRUPO;
var gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira = false;
function GradIntra_Risco_AoSelecionarSistema() 
{
    //gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

//    if ($("#pnlConteudo_Risco_GruposDeRiscoRestricoes").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
//    {
//        if (gGradIntra_Navegacao_SubSistemaAtual == "GruposDeRiscoRestricoes") 
//        {
//            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

//            $("#pnlConteudo_Risco_GruposDeRiscoRestricoes").find(".ExibirEmNovo").show();

//            $("#lstItemMenu_Risco").hide();

//            $("#lstItemMenu_Risco_Restricoes").show()
//                .addClass("MenuDoObjetoDeslocado")
//                .find("li").show()
//                    .closest("div").show()
//                        .find(".pnlFormulario").css("left", "262px");
//            
//            //gGradIntra_Navegacao_PainelDeConteudoAtual.hide();

//        }
//    }

    if ($("#pnlConteudo_Risco_GruposDeRisco").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
    {
        $("#pnlRisco_Formularios_Dados_DadosCompletos")
            .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
            .hide();

        if (gGradIntra_Navegacao_SubSistemaAtual == "GruposDeRisco") 
        {
            
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            $("#pnlConteudo_Risco_GruposDeRisco").find(".ExibirEmNovo").show();

            $("#lstItemMenu_Risco_Restricoes").hide();

            $("#lstItemMenu_Risco").show()
                .addClass("MenuDoObjetoDeslocado")
                .find("li").show()
                    .closest("div").show()
                        .find(".pnlFormulario").css("left", "262px");

            //gGradIntra_Navegacao_PainelDeConteudoAtual.hide();
        }
        else 
        if (gGradIntra_Navegacao_SubSistemaAtual == "GruposDeRiscoRestricoes") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            $("#pnlConteudo_Risco_GruposDeRiscoRestricoes").find(".ExibirEmNovo").show();

            $("#lstItemMenu_Risco").hide();

            $("#lstItemMenu_Risco_Restricoes").show()
                .addClass("MenuDoObjetoDeslocado")
                .find("li").show()
                    .closest("div").show()
                        .find(".pnlFormulario").css("left", "262px");
            
            //gGradIntra_Navegacao_PainelDeConteudoAtual.hide();

        }
        if (gGradIntra_Navegacao_SubSistemaAtual == "GruposDeRiscoRestricoesSpider") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            $("#pnlConteudo_Risco_GruposDeRiscoRestricoesSpider").find(".ExibirEmNovo").show();

            $("#lstItemMenu_Risco").hide();

            $("#lstItemMenu_Risco_Restricoes_Spider").show()
                .addClass("MenuDoObjetoDeslocado")
                .find("li").show()
                    .closest("div").show()
                        .find(".pnlFormulario").css("left", "262px");
            
            //gGradIntra_Navegacao_PainelDeConteudoAtual.hide();

        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoRiscoGeralDetalhamento") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
            
            lPopUpDetalhamento = window.open('Risco/Formularios/Dados/MonitoramentoRiscoGeralDetalhamento.aspx', "", 'height=800, width=1200,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoDeRisco") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            var lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoRisco.aspx', '', 'height=1000, width=1000,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
            
            lPopUp = null;
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoPositionClient") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            var lPopUp = window.open('http://10.0.11.94:9803/', '', 'fullscreen=yes, scrollbars=auto');
            
            lPopUp = null;
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoRiscoGeral") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            //if ($("#pnlConteudo_Risco_GruposDeRisco").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) 
            //if ($("#pnlRisco_Formularios_Dados_DadosCompletos").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
            //{
            $(gGradIntra_Navegacao_PainelDeConteudoAtual).show();

            gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            //var lUrl = gGradIntra_Navegacao_SistemaAtual;

            $("#pnlRisco_Formularios_Dados_DadosCompletos")
                    .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
                    .hide();
            //}
            //            else 
            //            {
            //                window.setTimeout(GradIntra_Risco_AoSelecionarSistema, 750);
            //            }
            //            var lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoRiscoGeral.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
            //            lPopUp = null;
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoIntradiario") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            //if ($("#pnlConteudo_Risco_GruposDeRisco").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) 
            //if ($("#pnlRisco_Formularios_Dados_DadosCompletos").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
            //{
            $(gGradIntra_Navegacao_PainelDeConteudoAtual).show();

            gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            //var lUrl = gGradIntra_Navegacao_SistemaAtual;

            $("#pnlRisco_Formularios_Dados_DadosCompletos")
                    .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
                    .hide();
            //}
            //            else 
            //            {
            //                window.setTimeout(GradIntra_Risco_AoSelecionarSistema, 750);
            //            }
            //            var lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoRiscoGeral.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
            //            lPopUp = null;
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "SuitabilityLavagem") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            //if ($("#pnlConteudo_Risco_GruposDeRisco").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) 
            //if ($("#pnlRisco_Formularios_Dados_DadosCompletos").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
            //{
            $(gGradIntra_Navegacao_PainelDeConteudoAtual).show();

            gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

            //var lUrl = gGradIntra_Navegacao_SistemaAtual;

            $("#pnlRisco_Formularios_Dados_DadosCompletos")
                    .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
                    .hide();
            //}
            //            else 
            //            {
            //                window.setTimeout(GradIntra_Risco_AoSelecionarSistema, 750);
            //            }
            //            var lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoRiscoGeral.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');
            //            lPopUp = null;
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "PLD") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            var lPopUp = window.open('Risco/Formularios/Dados/PLD.aspx', '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

            lPopUp = null;
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "ItensDoGrupo") 
        {
            $(".GradIntra_ComboBox_Pesquisa").ComboBoxAutocomplete();
        }
        else if (gGradIntra_Navegacao_SubSistemaAtual == "MonitoramentoCustodia") 
        {
            gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

            var lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoCustodia.aspx', '', 'height=750, width=900, scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

            lPopUp = null;
        }
    }
    else
    {
        
        //gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

        window.setTimeout(GradIntra_Risco_AoSelecionarSistema, 750);
    }
}

function GradIntra_Risco_ReverterTipoDeObjetoDeRiscoAtual(pTipoDeObjeto, pRecarregarConteudo)
{
///<summary>Função geral centralizadora para tratar o objeto de segurança sendo editado como Usuario, Grupo ou Perfil.</summary>
///<param name="pTipoDeObjeto" type="String">Tipo de objeto: 'Usuario', 'Grupo' ou 'Perfil'.</param>
///<param name="pRecarregarConteudo" type="Boolean">Flag se deve marcar o formulário de dados básicos para recarregar o conteúdo.</param>
///<returns>void</returns>

    gGradIntra_Risco_TipoDeObjetoDeRiscoAtual = pTipoDeObjeto;

    if (pRecarregarConteudo)
        $("#pnlSeguranca_Formularios_Dados_DadosCompletos").removeClass(CONST_CLASS_CONTEUDOCARREGADO);    //mudou o tipo, tem que mudar o conteudo

    $("#lstItemMenu_Risco li.Tipo_Parametro, #lstItemMenu_Risco li.Tipo_Permissao, #lstItemMenu_Risco li.Tipo_Grupo").hide();

    $("#lstItemMenu_Risco li.Tipo_" + gGradIntra_Risco_TipoDeObjetoDeRiscoAtual).show();
}
 

function GradIntra_Risco_InstanciarParametrosDeBusca()
{
    var lData = new Object();
    
    lData.TermoDeBusca = $("#txtBusca_Risco_Termo").val();
    lData.BuscarPor    = $("#cboBusca_Risco_BuscarPor").val();
//    lData.Tipo         = $("#cboBusca_Clientes_Tipo").val();

//    lData.Status_Ativo   = $("#chkBusca_Clientes_Status_Ativo").prop("checked");
//    lData.Status_Inativo = $("#chkBusca_Clientes_Status_Inativo").prop("checked");

//    lData.Passo_Visitante        = $("#chkBusca_Clientes_Passo_Visitante").prop("checked");
//    lData.Passo_Cadastrado       = $("#chkBusca_Clientes_Passo_Cadastrado").prop("checked");
//    lData.Passo_ExportadoSinacor = $("#chkBusca_Clientes_Passo_ExportadoSinacor").prop("checked");

//    lData.Pendencia_ComPendenciaCadastral   = $("#chkBusca_Clientes_Pendencia_ComPendenciaCadastral").prop("checked");
//    lData.Pendencia_ComSolicitacaoAlteracao = $("#chkBusca_Clientes_Pendencia_ComSolicitacaoAlteracao").prop("checked");
//    
    
    return lData;
}

function GradIntra_Risco_CarregarParametrosDaRegra()
{
    var lDados = { Acao: "BuscarParametrosDaRegra", Id: $("#cboPerfilDeRisco_Regras_Regra").val() };

    $("#pnlFormulario_Campos_Regras p.Parametro").remove();

    if (lDados.Id != "")
    {
        GradIntra_CarregarJsonVerificandoErro( "Risco/Formularios/Dados/Regras.aspx"
                                             , lDados
                                             , GradIntra_Risco_CarregarParametrosDaRegra_CallBack);
    }
}

function GradIntra_Risco_CarregarParametrosDaRegra_CallBack(pResposta)
{
    var lRegra = pResposta.ObjetoDeRetorno;

    $(lRegra.Parametros).each(function()
    {
        //var lParam = this;

        var lParagrafo = "<p class='Parametro'>"
                       +    "<label for='txtRisco_Regra_Parametro_" + this.Id + "'>" + this.Nome + ":</label>"
                       +    "<input  id='txtRisco_Regra_Parametro_" + this.Id + "' type='text'  IdParametro='" + this.Id + "' class='" + this.Validacoes + "' />"
                       + "</p>";

        lParagrafo = $("#pnlFormulario_Campos_Regras p.BotoesSubmit").before(lParagrafo).prev("p");

        lParagrafo.validationEngine({showTriangle: false});

        HabilitarMascaraNumerica( lParagrafo.find("input.Mascara_Data"), "99/99/9999" );
    });

    $("#pnlFormulario_Campos_Regras").find("input.ProibirLetras").bind("keydown", function(event) { Validacao_SomenteNumeros_OnKeyDown(event) } );
}

function GradIntra_Risco_SalvarDadosCompletos()
{
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";

    var lDiv = $("#pnlNovoItem_Formulario");

    if (lAcao == "Atualizar") 
    {
        lDiv = $("#pnlRisco_Formularios_Dados_DadosCompletos");
    }
    
    var lUrl = "Risco/Formularios/Dados/DadosCompletos_Grupo.aspx";

    GradIntra_Risco_SalvarDadosCompletos_Formulario(lDiv, lUrl, lAcao);

    return false;
}

function GradIntra_Risco_SalvarDadosGrupoRestricaoSpider() 
{
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";

    var lDiv = $("#pnlNovoItem_Formulario");

    if (lAcao == "Atualizar") 
    {
        lDiv = $("#pnlRisco_Formularios_Dados_DadosGrupoRestricaoSpider");
    }

    var lUrl = "Risco/Formularios/Dados/DadosGrupoRestricaoSpider.aspx";

    GradIntra_Risco_SalvarDadosCompletos_Formulario(lDiv, lUrl, lAcao);

    return false;
}

function GradIntra_Risco_SalvarDadosGrupoRestricao() 
{
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";

    var lDiv = $("#pnlNovoItem_Formulario");

    if (lAcao == "Atualizar") 
    {
        lDiv = $("#pnlRisco_Formularios_Dados_DadosGrupoRestricao");
    }

    var lUrl = "Risco/Formularios/Dados/DadosGrupoRestricao.aspx";

    GradIntra_Risco_SalvarDadosCompletos_Formulario(lDiv, lUrl, lAcao);

    return false;
}

function GradIntra_Risco_SalvarDadosCompletos_Formulario(pDivDeFormulario, pUrl, pAcao)
{
///<summary>Salva um formulário qualquer da página.</summary>
///<param name="pDivDeFormulario" type="Objeto_jQuery">Div cujos inputs serão enviados para salvar.</param>
///<param name="pUrl" type="String">URL para onde a chamada ajax será feita.</param>
///<param name="pAcao" type="String">Ação que será enviada na chamada ajax.</param>///<returns>void</returns>

    if (pDivDeFormulario.validationEngine({returnIsValid:true}))
    {        
        var lDivId = pDivDeFormulario.attr("id");

        var lObjeto = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

        if (lObjeto.ParentId)
        {
            //é algum tipo de elemento filho
            lObjeto.ParentId = gGradIntra_Cadastro_ItemSendoEditado.Id;
        }

        var lDados = new Object();

        lDados.Acao = pAcao;

        lDados.ObjetoJson = $.toJSON(lObjeto);

        GradIntra_CarregarJsonVerificandoErro( pUrl
                                             , lDados
                                             , function(pResposta) { GradIntra_Risco_SalvarDadosCompletos_Formulario_CallBack(pResposta, lDivId); }
                                             , null
                                             , pDivDeFormulario);

        pDivDeFormulario.find("input, select").prop("disabled", true);
        
        gGradIntra_Cadastro_ItemSendoIncluido = lObjeto;
    }    
    else
    {
        //GradIntra_Cadastro_RemoverMultiplosRadiosDaTela();

        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar");
    }
}

function GradIntra_Risco_SalvarDadosCompletos_Formulario_CallBack(pResposta, lDivId)
{
    if (pResposta.TemErro)
    {
        GradIntra_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lTotalPorPagina = $(".ui-paging-info span:[class=\"TotalPorPagina\"]");
        var lTotalPorPaginaValor = lTotalPorPagina.html();
        lTotalPorPagina.html(parseInt(lTotalPorPaginaValor) + 1);

        var lTotalPorConsulta = $(".ui-paging-info span:[class=\"TotalPorConsulta\"]");
        var lTotalPorConsultaValor = lTotalPorConsulta.html();
        lTotalPorConsulta.html(parseInt(lTotalPorConsultaValor) + 1);
        
        var lDescricaoGrupo = $("#txtRisco_DadosCompletos_Grupo_Descricao");
        var lTabela = $("#tblRisco_ConfigurarGridDeResultados");
        var lTabelaMatriz = (lTabela.find("tr").length > 0) ? lTabela : $("#tblRisco_ConfigurarGridDeResultados_Matriz");
        var lTrClonada = $(lTabelaMatriz.find("tr")[0]).clone();
        var lTds = lTrClonada.find("td") ;

        $(lTds[0]).html(lDescricaoGrupo.val())
        $(lTds[1]).html(pResposta.ObjetoDeRetorno)
        $(lTds[2]).find("button").attr("id", "GrupoDeRisco_" + pResposta.ObjetoDeRetorno.IdCadastrado);

        lTabela.find("tbody").prepend(lTrClonada);

        GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);

        $("#GrupoDeRisco_" + pResposta.ObjetoDeRetorno.IdCadastrado).closest("tr").effect("highlight", {}, 2700);

        lDescricaoGrupo.prop("disabled", false).val("");
    }
}

function GradIntra_Risco_ItemIncluidoComSucesso(pItemIncluido)
{
    gGradIntra_Navegacao_SubSistemaAtual       = CONST_SUBSISTEMA_BUSCA;
    
    gGradIntra_Navegacao_PainelDeBuscaAtual    = $("#pnlBusca_Risco_Busca");

    gGradIntra_Navegacao_PainelDeConteudoAtual = $("#pnlConteudo_Risco_Busca");

    var lNovaLI = GradIntra_Navegacao_AdicionarItemNaListaDeItensSelecionados(pItemIncluido, CONST_SISTEMA_RISCO, true);
    
    //se já tinha algum selecionado, deseleciona:
    $("#lstItensSelecionados_Risco li").removeClass(CONST_CLASS_ITEM_EXPANDIDO);

    lNovaLI.addClass(CONST_CLASS_ITEM_EXPANDIDO);

    $("ul.Sistema_Risco li." + CONST_CLASS_ITEM_SELECIONADO).removeClass(CONST_CLASS_ITEM_SELECIONADO);

    $("ul.Sistema_Risco li a[rel='Busca']").parent().addClass(CONST_CLASS_ITEM_SELECIONADO);

    gGradIntra_Navegacao_PainelDeBuscaAtual
        .show();

    gGradIntra_Navegacao_PainelDeConteudoAtual
        .show()
        .find("ul.MenuDoObjeto")
            .show()
            .find("li.SubTitulo, li.Tipo_" + pItemIncluido.TipoDeObjeto)
                .show();

    GradIntra_Risco_ReverterTipoDeObjetoDeRiscoAtual(gGradIntra_Cadastro_ItemSendoEditado.TipoDeObjeto, false)
}

function GradIntra_Risco_SalvarRegra()
{
    
}

function GradIntra_Risco_AssociarPermissoes()
{
    var lArrPermissoes = $("#hidRisco_ListaPermissoesAssociadas").val().split(";");

    for(var i = 0; i < lArrPermissoes.length; i++)
    {
        $("#pnlFormulario_Campos_AssociarPermissoesParametros_Permissao")
            .find("input[ValorQuandoSelecionado='" + lArrPermissoes[i] + "']")
                .prop("checked", true)
                .parent()
                    .addClass("checked");
    }
}

function GradIntra_Risco_Relatorios_Load()
{
    var lIdAssessorLogado = $("#hddIdAssessorLogado").val();

    if (lIdAssessorLogado && "" != lIdAssessorLogado)
    {
        var lComboBusca_FiltroRelatorioRisco_Assessor = $("#cmbBusca_FiltroRelatorioRisco_Assessor");
        lComboBusca_FiltroRelatorioRisco_Assessor.prop("disabled", true);
        lComboBusca_FiltroRelatorioRisco_Assessor.val(lIdAssessorLogado);
    }
}

function Risco_ConfigurarGridDeResultados_GruposDeRiscoRestricoes() 
{
    $("#tblRisco_ConfigurarGridDeResultados").jqGrid(
    {
        url: "Risco/Formularios/Dados/DadosGrupoRestricao.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [{ label: "Nome do Grupo"     , name: "NomeDoGrupo"   , jsonmap: "NomeDoGrupo"    , index: "NomeDoGrupo"  , width: 40 , align: "left"     , sortable: false }
                  , { label: "Codigo do Grupo"  , name: "CodigoGrupo"   , jsonmap: "CodigoGrupo"    , index: "CodigoGrupo"  , width: 1  , align: "left"     , sortable: false, hidden: true }
                  , { label: "Excluir"          , name: "Excluir"       , jsonmap: "Check"          , index: "Check"        , width: 10 , align: "center"   , sortable: false }
                  ]
      , height: 441
      , pager: "#pnlRisco_ConfigurarGridDeResultados_Pager"
      , rowNum: 10
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   // flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: { root: "Itens"
                    , page: "PaginaAtual"
                    , total: "TotalDePaginas"
                    , records: "TotalDeItens"
                    , cell: ""               // vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id: "0"               // primeira propriedade do elemento de linha é o Id
                    , repeatitems: false
      }
      , afterInsertRow: Risco_GruposDeRiscoRestricoes_ItemDataBound
        //      , beforeSelectRow: GradIntra_Busca_OrdensSelectRow
    });

    //--> Chama o evento de page resize pra já ajustar:
    $("#tblRisco_ConfigurarGridDeResultados").setGridWidth(440);
    $("#tblRisco_ConfigurarGridDeResultados").setGridHeight(221);
}

function Risco_ConfigurarGridDeResultados_GruposDeRiscoRestricoesSpider() 
{
    $("#tblRisco_ConfigurarGridDeResultadosSpider").jqGrid(
    {
        url: "Risco/Formularios/Dados/DadosGrupoRestricaoSpider.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [{ label: "Nome do Grupo"     , name: "NomeDoGrupo"   , jsonmap: "NomeDoGrupo"    , index: "NomeDoGrupo"  , width: 40 , align: "left"     , sortable: false }
                  , { label: "Codigo do Grupo"  , name: "CodigoGrupo"   , jsonmap: "CodigoGrupo"    , index: "CodigoGrupo"  , width: 1  , align: "left"     , sortable: false, hidden: true }
                  , { label: "Excluir"          , name: "Excluir"       , jsonmap: "Check"          , index: "Check"        , width: 10 , align: "center"   , sortable: false }
                  ]
      , height: 441
      , pager: "#pnlRisco_ConfigurarGridDeResultadosSpider_Pager"
      , rowNum: 10
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   // flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: { root: "Itens"
                    , page: "PaginaAtual"
                    , total: "TotalDePaginas"
                    , records: "TotalDeItens"
                    , cell: ""               // vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id: "0"               // primeira propriedade do elemento de linha é o Id
                    , repeatitems: false
      }
      , afterInsertRow: Risco_GruposDeRiscoRestricoesSpider_ItemDataBound
        //      , beforeSelectRow: GradIntra_Busca_OrdensSelectRow
    });

    //--> Chama o evento de page resize pra já ajustar:
    $("#tblRisco_ConfigurarGridDeResultadosSpider").setGridWidth(440);
    $("#tblRisco_ConfigurarGridDeResultadosSpider").setGridHeight(221);
}

function Risco_ConfigurarGridDeResultados_GruposDeRisco()
{
    $("#tblRisco_ConfigurarGridDeResultados").jqGrid(
    {
        url: "Risco/Formularios/Dados/DadosCompletos_Grupo.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [ { label: "Nome do Grupo"    , name: "NomeDoGrupo"   , jsonmap: "NomeDoGrupo", index: "NomeDoGrupo"  , width: 40 , align: "left"     , sortable: false }
                  , { label: "Codigo do Grupo"  , name: "CodigoGrupo"   , jsonmap: "CodigoGrupo", index: "CodigoGrupo"  , width: 1  , align: "left"     , sortable: false, hidden: true }
                  , { label: "Excluir"          , name: "Excluir"       , jsonmap: "Check"      , index: "Check"        , width: 10 , align: "center"   , sortable: false }  
                  ]
      , height: 441
      , pager: "#pnlRisco_ConfigurarGridDeResultados_Pager"
      , rowNum: 10
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   // flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: { root        : "Itens"
                    , page        : "PaginaAtual"
                    , total       : "TotalDePaginas"
                    , records     : "TotalDeItens"
                    , cell        : ""               // vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id          : "0"               // primeira propriedade do elemento de linha é o Id
                    , repeatitems : false
                    }
      , afterInsertRow: Risco_GruposDeRisco_ItemDataBound
//      , beforeSelectRow: GradIntra_Busca_OrdensSelectRow
  });

    //--> Chama o evento de page resize pra já ajustar:
    $("#tblRisco_ConfigurarGridDeResultados").setGridWidth( 440 );
    $("#tblRisco_ConfigurarGridDeResultados").setGridHeight( 221 );
}

function Risco_BuscarItensGruposDeRisco()
{
    var lURL = "Risco/Formularios/Dados/DadosCompletos_Grupo.aspx";
    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao" };

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { Risco_BuscarItensGruposDeRisco_Callback(pResposta); }
                                         );
}

function Risco_BuscarItensGruposDeRiscoRestricoesSpider()
{
    var lURL = "Risco/Formularios/Dados/DadosGrupoRestricaoSpider.aspx";
    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao" };

    GradIntra_CarregarJsonVerificandoErro(lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { Risco_BuscarItensGruposDeRiscoRestricaoSpider_Callback(pResposta); }
                                         );
}
function Risco_BuscarItensGruposDeRiscoRestricaoSpider_Callback(pResposta) 
{
    Risco_ConfigurarGridDeResultados_GruposDeRiscoRestricoesSpider();
}

function Risco_BuscarItensGruposDeRiscoRestricoes() 
{
    var lURL = "Risco/Formularios/Dados/DadosGrupoRestricao.aspx";
    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao" };

    GradIntra_CarregarJsonVerificandoErro(lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { Risco_BuscarItensGruposDeRiscoRestricao_Callback(pResposta); }
                                         );
}
function Risco_BuscarItensGruposDeRiscoRestricao_Callback(pResposta) 
{
    Risco_ConfigurarGridDeResultados_GruposDeRiscoRestricoes();
}
function Risco_BuscarItensGruposDeRisco_Callback(pResposta)
{
    Risco_ConfigurarGridDeResultados_GruposDeRisco();
}

function Risco_GruposDeRiscoRestricoesSpider_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblRisco_ConfigurarGridDeResultadosSpider tr td[title=" + rowdata.CodigoGrupo + "]").parent("tr");

    lRow.children("td").eq(2).html("<button class=\"IconButton Excluir\" style=\"margin-left: 45%;\" id=\"GrupoDeRisco_" + rowdata.CodigoGrupo +
                                   "\" onclick=\"return Risco_ExcluirGrupoDeRiscoRestricoesSpider(this);\"></button>");

    //--> Excluir Grupo
}

function Risco_GruposDeRiscoRestricoes_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblRisco_ConfigurarGridDeResultados tr td[title=" + rowdata.CodigoGrupo + "]").parent("tr");

    lRow.children("td").eq(2).html("<button class=\"IconButton Excluir\" style=\"margin-left: 45%;\" id=\"GrupoDeRisco_" + rowdata.CodigoGrupo +
                                   "\" onclick=\"return Risco_ExcluirGrupoDeRiscoRestricoes(this);\"></button>");

    //--> Excluir Grupo
}

function Risco_GruposDeRisco_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblRisco_ConfigurarGridDeResultados tr td[title=" + rowdata.CodigoGrupo + "]").parent("tr");
    
    lRow.children("td").eq(2).html("<button class=\"IconButton Excluir\" style=\"margin-left: 45%;\" id=\"GrupoDeRisco_" + rowdata.CodigoGrupo +
                                   "\" onclick=\"return Risco_ExcluirGrupoDeRisco(this);\"></button>");

    //--> Excluir Grupo
}

function Risco_ExcluirGrupoDeRiscoRestricoesSpider(pSender, pIdGrupo, pExcluiComFilhos) 
{
    var lUrl = "Risco/Formularios/Dados/DadosGrupoRestricaoSpider.aspx";

    var lExcluiComFilhos = pExcluiComFilhos ? true : false;
    
    var lIdGrupo = pSender != null ? $(pSender).attr("id").replace("GrupoDeRisco_", "") : pIdGrupo;
    
    var lDados = { Acao: 'Excluir', Id: lIdGrupo, ExcluiComfilhos: lExcluiComFilhos };

    GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, GradIntra_Risco_ExcluirItemSpider_CallBack);

    return false;
}

function Risco_ExcluirGrupoDeRiscoRestricoes(pSender, pIdGrupo, pExcluiComFilhos) 
{
    var lUrl = "Risco/Formularios/Dados/DadosGrupoRestricao.aspx";

    var lExcluiComFilhos = pExcluiComFilhos ? true : false;
    
    var lIdGrupo = pSender != null ? $(pSender).attr("id").replace("GrupoDeRisco_", "") : pIdGrupo;
    
    var lDados = { Acao: 'Excluir', Id: lIdGrupo, ExcluiComfilhos: lExcluiComFilhos };

    GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, GradIntra_Risco_ExcluirItem_CallBack);

    return false;
}

function Risco_ExcluirGrupoDeRisco(pSender, pIdGrupo, pExcluiComFilhos)
{
    var lUrl             = "Risco/Formularios/Dados/DadosCompletos_Grupo.aspx";
    var lExcluiComFilhos = pExcluiComFilhos ? true : false;
    var lIdGrupo         = pSender != null ? $(pSender).attr("id").replace("GrupoDeRisco_", "") : pIdGrupo;
    var lDados           = { Acao: 'Excluir', Id: lIdGrupo, ExcluiComfilhos: lExcluiComFilhos };

    GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, GradIntra_Risco_ExcluirItem_CallBack);
    
    return false;
}
/*
function GradIntra_Risco_ExcluirItem(pExcluiComFilhos)
{
//    var lUrl = "Risco/Formularios/Dados/DadosCompletos_" + gGradIntra_Risco_TipoDeObjetoDeRiscoAtual + ".aspx";

    var lUrl = "Risco/Formularios/Dados/DadosCompletos_Grupo.aspx";
    var lDados = { Acao: 'Excluir', Id: gGradIntra_Cadastro_ItemSendoEditado.Id };

    GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, GradIntra_Risco_ExcluirItem_CallBack);
}
*/

function GradIntra_Risco_ExcluirItemSpider_CallBack(pResposta) 
{
    if ("ComFilhos" == pResposta.Mensagem)
    {
        if (confirm("Este Grupo possui 'Itens de Grupo' atrelados a ele, realmente deseja excluir?\n\nObs: Caso este Grupo seja excluído todos os itens relacionados a ele também serão excluídos."))
        {
            Risco_ExcluirGrupoDeRisco(null, pResposta.ObjetoDeRetorno, true);
        }
    }
    else
    {
        GradIntra_ExibirMensagem('A', pResposta.Mensagem);

        var lTrExcluida = $("#GrupoDeRisco_" + pResposta.ObjetoDeRetorno).closest("tr");

        lTrExcluida.next().find("button").parent().css("width", "100px"); 

        lTrExcluida.remove();
        
        var lTotalPorPagina = $(".ui-paging-info span:[class=\"TotalPorPagina\"]");
        var lTotalPorPaginaValor = lTotalPorPagina.html();
        lTotalPorPagina.html(parseInt(lTotalPorPaginaValor) - 1);

        var lTotalPorConsulta = $(".ui-paging-info span:[class=\"TotalPorConsulta\"]");
        var lTotalPorConsultaValor = lTotalPorConsulta.html();
        lTotalPorConsulta.html(parseInt(lTotalPorConsultaValor) - 1);
    }
}

function GradIntra_Risco_ExcluirItem_CallBack(pResposta) 
{
    if ("ComFilhos" == pResposta.Mensagem)
    {
        if (confirm("Este Grupo possui 'Itens de Grupo' atrelados a ele, realmente deseja excluir?\n\nObs: Caso este Grupo seja excluído todos os itens relacionados a ele também serão excluídos."))
        {
            Risco_ExcluirGrupoDeRisco(null, pResposta.ObjetoDeRetorno, true);
        }
    }
    else
    {
        GradIntra_ExibirMensagem('A', pResposta.Mensagem);

        var lTrExcluida = $("#GrupoDeRisco_" + pResposta.ObjetoDeRetorno).closest("tr");

        lTrExcluida.next().find("button").parent().css("width", "100px"); 

        lTrExcluida.remove();
        
        var lTotalPorPagina = $(".ui-paging-info span:[class=\"TotalPorPagina\"]");
        var lTotalPorPaginaValor = lTotalPorPagina.html();
        lTotalPorPagina.html(parseInt(lTotalPorPaginaValor) - 1);

        var lTotalPorConsulta = $(".ui-paging-info span:[class=\"TotalPorConsulta\"]");
        var lTotalPorConsultaValor = lTotalPorConsulta.html();
        lTotalPorConsulta.html(parseInt(lTotalPorConsultaValor) - 1);
    }
}

/*Início Grupo Alavancagem*/

function Risco_BuscarItens_GrupoAlavancagem_Load()
{
    var lURL = "Risco/Formularios/Dados/GrupoAlavancagem.aspx";
    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao" };

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { Risco_BuscarItens_GrupoAlavancagem_Load_Callback(pResposta); }
                                         );
}

function Risco_BuscarItens_GrupoAlavancagem_Load_Callback(pResposta)
{
    Risco_ConfigurarGridDeResultados_GrupoAlavancagem();
}

function Risco_ConfigurarGridDeResultados_GrupoAlavancagem()
{
    $("#tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem").jqGrid(
    {
        url: "Risco/Formularios/Dados/GrupoAlavancagem.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [ { label: "Nome do Grupo", name: "NomeDoGrupo", jsonmap: "NomeDoGrupo", index: "NomeDoGrupo", width: 40, align: "left", sortable: false }
                  , { label: "Codigo do Grupo", name: "CodigoGrupo", jsonmap: "CodigoGrupo", index: "CodigoGrupo", width: 1, align: "left", sortable: false, hidden: true }
                  , { label: "Excluir", name: "Excluir", jsonmap: "Check", index: "Check", width: 10, align: "center", sortable: false }  
                  ]
      , height: 441
      , pager: "#pnlRisco_ConfigurarGridDeResultados_GrupoAlavancagem_Pager"
      , rowNum: 10
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   // flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , afterInsertRow: Risco_ConfigurarGridDeResultados_GrupoAlavancagem_ItemDataBound
      , jsonReader: { root        : "Itens"
                    , page        : "PaginaAtual"
                    , total       : "TotalDePaginas"
                    , records     : "TotalDeItens"
                    , cell        : ""               // vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id          : "0"               // primeira propriedade do elemento de linha é o Id
                    , repeatitems : false
                    }
  });

    //--> Chama o evento de page resize pra já ajustar:
    $("#tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem").setGridWidth( 440 );
    $("#tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem").setGridHeight( 221 );
}

function Risco_ConfigurarGridDeResultados_GrupoAlavancagem_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem tr td[title=" + rowdata.CodigoGrupo + "]").parent("tr");
    
    lRow.children("td").eq(2).html("<button class=\"IconButton Excluir\" style=\"margin-left: 31px;\" id=\"GrupoAlavancagem_" + rowdata.CodigoGrupo
                                  +"\" onclick=\"return Risco_Excluir_GrupoAlavancagem(this);\" ></button>");
    //--> Excluir Grupo
}

function Risco_Excluir_GrupoAlavancagem(pSender, pIdGrupo, pExcluiComFilhos)
{
    var lUrl             = "Risco/Formularios/Dados/GrupoAlavancagem.aspx";
    var lExcluiComFilhos = pExcluiComFilhos  ? true : false;
    var lIdGrupo         = pSender != null   ? $(pSender).attr("id").replace("GrupoAlavancagem_", "") : pIdGrupo;
    var lDados           = { Acao            : 'Excluir'
                           , Id              : lIdGrupo
                           , ExcluiComfilhos : lExcluiComFilhos
                           };

    GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, Risco_Excluir_GrupoAlavancagem_CallBack);
    
    return false;
}

function Risco_Excluir_GrupoAlavancagem_CallBack(pResposta) 
{
    if ("ComFilhos" == pResposta.Mensagem)
    {
        if (confirm("Este Grupo possui 'Itens de Grupo' atrelados a ele, realmente deseja excluir?\n\nObs: Caso este Grupo seja excluído todos os itens relacionados a ele também serão excluídos."))
        {
            Risco_Excluir_GrupoAlavancagem(null, pResposta.ObjetoDeRetorno, true);
        }
    }
    else
    {
        GradIntra_ExibirMensagem('A', pResposta.Mensagem);

        var lTrExcluida = $("#GrupoAlavancagem_" + pResposta.ObjetoDeRetorno).closest("tr");

        lTrExcluida.next().find("button").parent().css("width", "100px"); 

        lTrExcluida.remove();
        
        var lTotalPorPagina = $(".ui-paging-info span:[class=\"TotalPorPagina\"]");
        var lTotalPorPaginaValor = lTotalPorPagina.html();
        lTotalPorPagina.html(parseInt(lTotalPorPaginaValor) - 1);

        var lTotalPorConsulta = $(".ui-paging-info span:[class=\"TotalPorConsulta\"]");
        var lTotalPorConsultaValor = lTotalPorConsulta.html();
        lTotalPorConsulta.html(parseInt(lTotalPorConsultaValor) - 1);
    }
}

function GradIntra_Risco_GrupoAlavancagem()
{
    var lAcao = "Atualizar";
    var lDiv = $("#pnlRisco_Formularios_Dados_GrupoAlavancagem");
    var lUrl = "Risco/Formularios/Dados/GrupoAlavancagem.aspx";

    GradIntra_Risco_SalvarDados_GrupoAlavancagem(lDiv, lUrl, lAcao);

    return false;
}

function GradIntra_Risco_SalvarDados_GrupoAlavancagem(pDivDeFormulario, pUrl, pAcao)
{ ///<summary>Salva um formulário qualquer da página.</summary>
  ///<param name="pDivDeFormulario" type="Objeto_jQuery">Div cujos inputs serão enviados para salvar.</param>
  ///<param name="pUrl" type="String">URL para onde a chamada ajax será feita.</param>
  ///<param name="pAcao" type="String">Ação que será enviada na chamada ajax.</param>
  ///<returns>void</returns>

    if (pDivDeFormulario.validationEngine({returnIsValid:true}))
    {        
        var lDivId = pDivDeFormulario.attr("id");

        var lObjeto = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

        if (lObjeto.ParentId)
        {
            //é algum tipo de elemento filho
            lObjeto.ParentId = gGradIntra_Cadastro_ItemSendoEditado.Id;
        }

        var lDados = new Object();

        lDados.Acao = pAcao;

        lDados.ObjetoJson = $.toJSON(lObjeto);

        GradIntra_CarregarJsonVerificandoErro( pUrl
                                             , lDados
                                             , function(pResposta) { GradIntra_Risco_SalvarDados_GrupoAlavancagem_CallBack(pResposta, lDivId); }
                                             , null
                                             , pDivDeFormulario);

        pDivDeFormulario.find("input, select").prop("disabled", true);
        
        gGradIntra_Cadastro_ItemSendoIncluido = lObjeto;
    }    
    else
    {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar");
    }
}

function GradIntra_Risco_SalvarDados_GrupoAlavancagem_CallBack(pResposta, lDivId)
{
    if (pResposta.TemErro)
    {
        GradIntra_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lTotalPorPagina = $(".ui-paging-info span:[class=\"TotalPorPagina\"]");
        var lTotalPorPaginaValor = lTotalPorPagina.html();
        lTotalPorPagina.html(parseInt(lTotalPorPaginaValor) + 1);

        var lTotalPorConsulta = $(".ui-paging-info span:[class=\"TotalPorConsulta\"]");
        var lTotalPorConsultaValor = lTotalPorConsulta.html();
        lTotalPorConsulta.html(parseInt(lTotalPorConsultaValor) + 1);
        
        var lDescricaoGrupo = $("#txtRisco_DadosCompletos_Grupo_GrupoAlavancagem");
        var lTabela = $("#tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem");
        var lTabelaMatriz = (lTabela.find("tr").length > 0) ? lTabela : $("#tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem_Matriz");
        var lTrClonada = $(lTabelaMatriz.find("tr")[0]).clone();
        var lTds = lTrClonada.find("td") ;

        $(lTds[0]).html(lDescricaoGrupo.val())
        $(lTds[1]).html(pResposta.ObjetoDeRetorno)
        $(lTds[2]).find("button").attr("id", "GrupoAlavancagem_" + pResposta.ObjetoDeRetorno.IdCadastrado);

        lTabela.find("tbody").prepend(lTrClonada);

        GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);

        $("#GrupoAlavancagem_" + pResposta.ObjetoDeRetorno.IdCadastrado).closest("tr").effect("highlight", {}, 2700);

        lDescricaoGrupo.prop("disabled", false).val("");
    }
}

/*Final  Grupo Alavancagem*/


function GradIntra_Risco_Restricoes_TravaExposicao_Load()
{
    var lAcao = "CarregarHtmlComDados";

    var lPerdaMaxima         = $("#txtPercentagem_Restricoes_PerdaMaxima").val();
    var lPercentualOscilacao = $("#txtPrejuizo_Restricoes_MaximoAtingido").val();

    var lURL = "Risco/Formularios/Dados/TravaExposicao.aspx";

    lObjetoDeParametros = 
        {
            Acao : lAcao,
            TravaExposicaoTeste: "Teste"
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_TravaExposicao_Load_Callback(pResposta); }
                                             );

    //return false;
}

function GradIntra_Risco_Restricoes_TravaExposicao_Load_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", "Insira os dados de  para definir os seus parâmetros");
    }
    else
    {
        
    }
}

function GradIntra_Risco_Restricoes_TravaExposicaoSpider_Salvar(pSender)
{
    var lAcao = "SalvarDadosTravaExposicao";

    var lPerdaMaxima         = $("#txtPercentagem_Restricoes_PerdaMaxima").val();
    var lPercentualOscilacao = $("#txtPrejuizo_Restricoes_MaximoAtingido").val();

    var lURL = "Risco/Formularios/Dados/TravaExposicaoSpider.aspx";

    lObjetoDeParametros = 
        {
            Acao                 : lAcao,
            VlPrejuizoMaximo     : lPerdaMaxima,
            VlPercentualOscilacao: lPercentualOscilacao
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_TravaExposicaoSpider_Callback(pResposta); }
                                             );
}

function GradIntra_Risco_Restricoes_TravaExposicaoSpider_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", "Insira os dados de  para definir os seus parâmetros");
    }
    else
    {
        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_Restricoes_TravaExposicao_Salvar(pSender)
{
    var lAcao = "SalvarDadosTravaExposicao";

    var lPerdaMaxima         = $("#txtPercentagem_Restricoes_PerdaMaxima").val();
    var lPercentualOscilacao = $("#txtPrejuizo_Restricoes_MaximoAtingido").val();

    var lURL = "Risco/Formularios/Dados/TravaExposicao.aspx";

    lObjetoDeParametros = 
        {
            Acao                 : lAcao,
            VlPrejuizoMaximo     : lPerdaMaxima,
            VlPercentualOscilacao: lPercentualOscilacao
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_TravaExposicao_Callback(pResposta); }
                                             );
}

function GradIntra_Risco_Restricoes_TravaExposicao_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", "Insira os dados de  para definir os seus parâmetros");
    }
    else
    {
        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_Restricoes_Regras_SelecionarGrupoSpider(pSender) 
{

    var lIdGrupo = $("#GradIntra_Risco_ItensDoGrupoRestricoesSpider_Grupos").val();
    var lAcao = "CarregarListaRegraGrupoItem";

    if ("" == lIdGrupo)
     {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoesSpider.aspx";

        var lObjetoDeParametros = null;

        //

        if ($("#DivtblGradIntra_Risco_ItemGrupoSpider").is(":visible")) 
        {
            lAcao = "CarregarListaRegraGrupoItem";
        } 
        else
        {
            lAcao = "CarregarListaRegraGrupoItemGlobal";
        }

        lObjetoDeParametros = 
        {
            Acao: lAcao,
            IdGrupo: $("#GradIntra_Risco_ItensDoGrupoRestricoesSpider_Grupos").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_Regras_SelecionarGrupoSpider_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_Restricoes_Regras_SelecionarGrupoSpider_Callback(pResposta) 
{
    if (pResposta.TemErro) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else 
    {
        var lTr;
        var lThis;
        var lTabela = $("#tblGradIntra_Risco_ItemRegraGrupoSpider tbody");
        lTabela.find("tr:visible").remove();
        lTabela = $("#tblGradIntra_Risco_ItemClienteSpider tbody");
        lTabela.find("tr:visible").remove();
        lTabela = $("#tblGradIntra_Risco_ItemGrupo_GlobalSpider tbody");
        lTabela.find("tr:visible").remove();

        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        for (var i = 0; i < lObjetoDeRetorno.Resultado.length; i++) 
        {
            lThis = $(lObjetoDeRetorno.Resultado[i]);

            var lTabela;
            var lTr;

            var lCliente      = lThis[0].CodigoCliente;   
            var lPapel        = lThis[0].Papel;
            var lDirecao      = lThis[0].Sentido;
            var lCodigoAcao   = lThis[0].CodigoAcao;
            var lNomeGrupo    = lThis[0].NomeGrupo;
            var lNomeAcao     = lThis[0].NomeAcao;
            var lCodigoGrupo  = lThis[0].CodigoGrupo;

            if ( lPapel != null )
            {
                lTabela = $("#tblGradIntra_Risco_ItemClienteSpider tbody");
                lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatrizSpider").clone();

                lTr.find(".tdGradIntra_Risco_ItemCliente_IdAcao")     .html(lCodigoAcao);
                lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente")    .html(lCliente);
                lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
                lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido")    .html(lDirecao);
                
                lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatrizSpider");
            }
            else if (lCodigoAcao == "1") 
            {
                lTabela = $("#tblGradIntra_Risco_ItemRegraGrupoSpider tbody");
                lTr =  $(".tdGradIntra_Risco_ItemRegraGrupo_TrMatrizSpider").clone();
                lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdAcao")       .html(lCodigoAcao);
                lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Cliente")      .html(lCliente);
                lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Sentido")      .html(lDirecao);
                
                lTr.removeClass("tdGradIntra_Risco_ItemRegraGrupo_TrMatrizSpider");
            }
            else if (lCodigoAcao == "2") 
            {
                lTabela = $("#tblGradIntra_Risco_ItemGrupo_GlobalSpider tbody");
                lTr = $(".tdGradIntra_Risco_ItemGrupo_Global_TrMatrizSpider").clone();

                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdGrupo")       .html(lCodigoGrupo);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao")        .html(lCodigoAcao);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeGrupo")     .html(lNomeGrupo);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido")       .html(lDirecao);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeAcao")      .html(lNomeAcao);
                
                lTr.removeClass("tdGradIntra_Risco_ItemGrupo_Global_TrMatrizSpider");
            }

            lTr.show();

            lTabela.prepend(lTr);

            lTr.effect("highlight", {}, 3700);
        }
    }

    GradIntra_ExibirMensagem("A", pResposta.Mensagem);

}

/*Início Itens do Grupo*/

function GradIntra_Risco_Restricoes_Regras_SelecionarGrupo(pSender) 
{

    var lIdGrupo = $("#GradIntra_Risco_ItensDoGrupoRestricoes_Grupos").val();
    var lAcao = "CarregarListaRegraGrupoItem";

    if ("" == lIdGrupo)
     {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";

        var lObjetoDeParametros = null;

        //

        if ($("#DivtblGradIntra_Risco_ItemGrupo").is(":visible")) 
        {
            lAcao = "CarregarListaRegraGrupoItem";
        } 
        else
        {
            lAcao = "CarregarListaRegraGrupoItemGlobal";
        }

        lObjetoDeParametros = 
        {
            Acao: lAcao,
            IdGrupo: $("#GradIntra_Risco_ItensDoGrupoRestricoes_Grupos").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_Regras_SelecionarGrupo_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_Restricoes_Regras_SelecionarGrupo_Callback(pResposta) 
{
    if (pResposta.TemErro) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else 
    {
        var lTr;
        var lThis;
        var lTabela = $("#tblGradIntra_Risco_ItemRegraGrupo tbody");
        lTabela.find("tr:visible").remove();
        lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
        lTabela.find("tr:visible").remove();
        lTabela = $("#tblGradIntra_Risco_ItemGrupo_Global tbody");
        lTabela.find("tr:visible").remove();

        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        for (var i = 0; i < lObjetoDeRetorno.Resultado.length; i++) 
        {
            lThis = $(lObjetoDeRetorno.Resultado[i]);

            var lTabela;
            var lTr;

            var lCliente      = lThis[0].CodigoCliente;   
            var lPapel        = lThis[0].Papel;
            var lDirecao      = lThis[0].Sentido;
            var lCodigoAcao   = lThis[0].CodigoAcao;
            var lNomeGrupo    = lThis[0].NomeGrupo;
            var lNomeAcao     = lThis[0].NomeAcao;
            var lCodigoGrupo  = lThis[0].CodigoGrupo;

            if ( lPapel != null )
            {
                lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
                lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatriz").clone();

                lTr.find(".tdGradIntra_Risco_ItemCliente_IdAcao")     .html(lCodigoAcao);
                lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente")    .html(lCliente);
                lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
                lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido")    .html(lDirecao);
                
                lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatriz");
            }
            else if (lCodigoAcao == "1") 
            {
                lTabela = $("#tblGradIntra_Risco_ItemRegraGrupo tbody");
                lTr =  $(".tdGradIntra_Risco_ItemRegraGrupo_TrMatriz").clone();
                lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdAcao")       .html(lCodigoAcao);
                lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Cliente")      .html(lCliente);
                lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Sentido")      .html(lDirecao);
                
                lTr.removeClass("tdGradIntra_Risco_ItemRegraGrupo_TrMatriz");
            }
            else if (lCodigoAcao == "2") 
            {
                lTabela = $("#tblGradIntra_Risco_ItemGrupo_Global tbody");
                lTr = $(".tdGradIntra_Risco_ItemGrupo_Global_TrMatriz").clone();

                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdGrupo")       .html(lCodigoGrupo);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao")        .html(lCodigoAcao);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeGrupo")     .html(lNomeGrupo);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido")       .html(lDirecao);
                lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeAcao")      .html(lNomeAcao);
                
                lTr.removeClass("tdGradIntra_Risco_ItemGrupo_Global_TrMatriz");
            }

            lTr.show();

            lTabela.prepend(lTr);

            lTr.effect("highlight", {}, 3700);
        }
    }

    GradIntra_ExibirMensagem("A", pResposta.Mensagem);

}

function GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItemSpider(pSender) 
{
    var lAcao = "CarregarListaClienteBloqueioInstrumento";

    var lURL = "Risco/Formularios/Dados/ManutencaoRestricoesSpider.aspx";
    var lObjetoDeParametros = {
        Acao: lAcao,
    };

    GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItemSpider_Callback(pResposta); }
                                             );
}

function GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItemSpider_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", "Erro listar bloqueios");
    }
    else
    {
        var lRetorno = pResposta.ObjetoDeRetorno.Resultado;

        var lTabela = $("#tblGradIntra_Risco_ItemClienteSpider tbody");

        lTabela.find("tr:visible").remove();

        for(i=0; i < lRetorno.length; i++)
        {
            lPapel      = lRetorno[i].CdAtivo;
            lCliente    = lRetorno[i].IdCliente;
            lDirecao    = lRetorno[i].Direcao;
            lCodigoAcao = "";
        
            lTabela = $("#tblGradIntra_Risco_ItemClienteSpider tbody");
            lTr     = $(".tdGradIntra_Risco_ItemCliente_TrMatrizSpider").clone();

            lTr.find(".tdGradIntra_Risco_ItemCliente_IdAcao")     .html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente")    .html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido")    .html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatrizSpider");

            lTr.show();

            lTabela.prepend(lTr);

            lTr.effect("highlight", {}, 3700);
        }
    }
}

function GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItem(pSender) 
{
    var lAcao = "CarregarListaClienteBloqueioInstrumento";

    var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";
    var lObjetoDeParametros = {
        Acao: lAcao,
    };

    GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItem_Callback(pResposta); }
                                             );
}

function GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItem_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", "Erro listar bloqueios");
    }
    else
    {
        var lRetorno = pResposta.ObjetoDeRetorno.Resultado;

        var lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");

        lTabela.find("tr:visible").remove();

        lTabela.find("tr").remove();

        for(i=0; i < lRetorno.length; i++)
        {
            lPapel      = lRetorno[i].CdAtivo;
            lCliente    = lRetorno[i].IdCliente;
            lDirecao    = lRetorno[i].Direcao;
            lCodigoAcao = "";
        
            lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
            lTr     = $(".tdGradIntra_Risco_ItemCliente_TrMatriz").clone();

            lTr.find(".tdGradIntra_Risco_ItemCliente_IdAcao")     .html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente")    .html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido")    .html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatriz");

            lTr.show();

            lTabela.prepend(lTr);

            lTr.effect("highlight", {}, 3700);
        }
    }
}


function GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItemSpider(pSender, pTipoRestricao) 
{

    var lIdGrupo    = $("#GradIntra_Risco_ItensDoGrupoRestricoesSpider_Grupos").val();
    var lCliente    = $("#txtCliente_Restricoes_RestricaoPorClienteSpider").val();
    var lPapel      = $("#txtCliente_Restricoes_RestricaoPorAtivoSpider").val();
    var lRestringir = $("#ckbGradIntraRisco_Restricao_Global_OMS_Spider").prop("checked");
    var lDirecao    = $(".RadiosRestricaoPorAtivosSpider").find("input[type='radio']:checked").val();
    var lAcao       = "SalvarRegraGrupoItem";

    /*
    Selecionar a ação
    "SalvarClienteBloqueioInstrumentoDirecao"
    "SalvarRegraGrupoItem"
    "SalvarRegraGrupoItemGlobal"
    */

    if(pTipoRestricao == "pnlCliente_Restricoes_Grupo" || pTipoRestricao == "")
    {
        lAcao = "SalvarRegraGrupoItem";
    }
    else if (pTipoRestricao == "pnlCliente_Restricoes_Grupo_Global")
    {
        lAcao = "SalvarRegraGrupoItemGlobal";
    }
    else if (pTipoRestricao  == "pnlCliente_Restricoes_Individual")
    {
        lAcao = "SalvarClienteBloqueioInstrumentoDirecao";
        lIdGrupo = "1";
    }

    if (lIdGrupo == "" ) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoesSpider.aspx";
        var lObjetoDeParametros = { 
            Acao         : lAcao,
            IdGrupo      : lIdGrupo,
            Ativo        : lPapel,
            CodigoCliente: lCliente,
            Direcao      : lDirecao
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItemSpider_Callback(pResposta, pTipoRestricao); }
                                             );
    }
}

function GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItemSpider_Callback(pResposta, pTipoRestricao) 
{
    if (pResposta.TemErro) 
    {
        GradIntra_ExibirMensagem("E", "Erro ao inserir item");
    }
    else
    {
        var lTabela;
        var lTr;

        var lRetorno = pResposta.ObjetoDeRetorno.RegraGrupoItem;

        if (lRetorno == null) 
        {
            lRetorno = pResposta.ObjetoDeRetorno.Objeto;
        }

        if (lRetorno == null)
        {
            lRetorno = pResposta.ObjetoDeRetorno.Resultado;
        }

        if (lRetorno.IdCliente == 0) { return ; }

        var lPapel      = "";
        var lCliente    = "";
        var lDirecao    = "";
        var lCodigoAcao = "";
        var lNomeGrupo  = "";
        var lNomeAcao   = "";
        var lIdGrupo    = "";

        if (pTipoRestricao == "pnlCliente_Restricoes_Grupo_Global")
        {
            lCodigoAcao = lRetorno.CodigoAcao;
            lNomeGrupo  = lRetorno.NomeGrupo;
            lNomeAcao   = lRetorno.NomeAcao;
            lCodigoAcao = lRetorno.CodigoGrupo
            lDirecao    = lRetorno.Sentido;
        }
        else if (pTipoRestricao == "pnlCliente_Restricoes_Individual") 
        {
            lPapel      = lRetorno.CdAtivo;
            lCliente    = lRetorno.IdCliente;
            lDirecao    = lRetorno.Direcao;
            lNomeGrupo  = lRetorno.NomeGrupo;
            lNomeAcao   = lRetorno.NomeAcao;
            //lIdGrupo    = lRetorno
            lCodigoAcao = "";
        }
        else 
        {
            lCliente = lRetorno.CodigoCliente; 
            lDirecao = lRetorno.Sentido;
            lCodigoAcao = lRetorno.CodigoAcao;
        }

        if (pTipoRestricao == "pnlCliente_Restricoes_Individual") 
        {
            lTabela = $("#tblGradIntra_Risco_ItemClienteSpider tbody");
            lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatrizSpider").clone();

            lTr.find(".tdGradIntra_Risco_ItemCliente_IdAcao").html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente").html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatrizSpider");
        }
        else
        if (pTipoRestricao == "pnlCliente_Restricoes_Grupo" || pTipoRestricao == "") 
        {
            lTabela = $("#tblGradIntra_Risco_ItemRegraGrupoSpider tbody");
            lTr =  $(".tdGradIntra_Risco_ItemRegraGrupo_TrMatrizSpider").clone();
            lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdAcao").html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Cliente").html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemRegraGrupo_TrMatrizSpider");
        }
        else if (pTipoRestricao == "pnlCliente_Restricoes_Grupo_Global")
        {
            lTabela = $("#tblGradIntra_Risco_ItemGrupo_GlobalSpider tbody");
            lTr = $(".tdGradIntra_Risco_ItemGrupo_Global_TrMatrizSpider").clone();

            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao").html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao")        .html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeGrupo")     .html(lNomeGrupo);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido")       .html(lDirecao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeAcao")      .html(lNomeAcao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemGrupo_Global_TrMatrizSpider");
        }

        lTr.show();

        lTabela.prepend(lTr);

        lTr.effect("highlight", {}, 3700);
    }

}

function GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItem(pSender, pTipoRestricao) 
{

    var lIdGrupo    = $("#GradIntra_Risco_ItensDoGrupoRestricoes_Grupos").val();
    var lCliente    = $("#txtCliente_Restricoes_RestricaoPorCliente").val();
    var lPapel      = $("#txtCliente_Restricoes_RestricaoPorAtivo").val();
    var lRestringir = $("#ckbGradIntraRisco_Restricao_Global_OMS").prop("checked");
    var lDirecao    = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();
    var lAcao       = "SalvarRegraGrupoItem";

    /*
    Selecionar a ação
    "SalvarClienteBloqueioInstrumentoDirecao"
    "SalvarRegraGrupoItem"
    "SalvarRegraGrupoItemGlobal"
    */

    if(pTipoRestricao == "pnlCliente_Restricoes_Grupo" || pTipoRestricao == "")
    {
        lAcao = "SalvarRegraGrupoItem";
    }
    else if (pTipoRestricao == "pnlCliente_Restricoes_Grupo_Global")
    {
        lAcao = "SalvarRegraGrupoItemGlobal";
    }
    else if (pTipoRestricao  == "pnlCliente_Restricoes_Individual")
    {
        lAcao = "SalvarClienteBloqueioInstrumentoDirecao";
        lIdGrupo = "1";
    }

    if (lIdGrupo == "" ) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";
        var lObjetoDeParametros = { 
            Acao         : lAcao,
            IdGrupo      : lIdGrupo,
            Ativo        : lPapel,
            CodigoCliente: lCliente,
            Direcao      : lDirecao
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItem_Callback(pResposta, pTipoRestricao); }
                                             );
    }
}

function GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItem_Callback(pResposta, pTipoRestricao) 
{
    if (pResposta.TemErro) 
    {
        GradIntra_ExibirMensagem("E", "Erro ao inserir item");
    }
    else
    {
        var lTabela;
        var lTr;

        var lRetorno = pResposta.ObjetoDeRetorno.RegraGrupoItem;

        if (lRetorno == null) 
        {
            lRetorno = pResposta.ObjetoDeRetorno.Objeto;
        }

        if (lRetorno == null)
        {
            lRetorno = pResposta.ObjetoDeRetorno.Resultado;
        }

        if (lRetorno.IdCliente == 0) { return ; }

        var lPapel      = "";
        var lCliente    = "";
        var lDirecao    = "";
        var lCodigoAcao = "";
        var lNomeGrupo  = "";
        var lNomeAcao   = "";
        var lIdGrupo    = "";

        if (pTipoRestricao == "pnlCliente_Restricoes_Grupo_Global")
        {
            lCodigoAcao = lRetorno.CodigoAcao;
            lNomeGrupo  = lRetorno.NomeGrupo;
            lNomeAcao   = lRetorno.NomeAcao;
            lCodigoAcao = lRetorno.CodigoGrupo
            lDirecao    = lRetorno.Sentido;
        }
        else if (pTipoRestricao == "pnlCliente_Restricoes_Individual") 
        {
            lPapel      = lRetorno.CdAtivo;
            lCliente    = lRetorno.IdCliente;
            lDirecao    = lRetorno.Direcao;
            lNomeGrupo  = lRetorno.NomeGrupo;
            lNomeAcao   = lRetorno.NomeAcao;
            //lIdGrupo    = lRetorno
            lCodigoAcao = "";
        }
        else 
        {
            lCliente = lRetorno.CodigoCliente; 
            lDirecao = lRetorno.Sentido;
            lCodigoAcao = lRetorno.CodigoAcao;
        }

        if (pTipoRestricao == "pnlCliente_Restricoes_Individual") 
        {
            lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
            lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatriz").clone();

            lTr.find(".tdGradIntra_Risco_ItemCliente_IdAcao").html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente").html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatriz");
        }
        else
        if (pTipoRestricao == "pnlCliente_Restricoes_Grupo" || pTipoRestricao == "") 
        {
            lTabela = $("#tblGradIntra_Risco_ItemRegraGrupo tbody");
            lTr =  $(".tdGradIntra_Risco_ItemRegraGrupo_TrMatriz").clone();
            lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdAcao").html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Cliente").html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemRegraGrupo_TrMatriz");
        }
        else if (pTipoRestricao == "pnlCliente_Restricoes_Grupo_Global")
        {
            lTabela = $("#tblGradIntra_Risco_ItemGrupo_Global tbody");
            lTr = $(".tdGradIntra_Risco_ItemGrupo_Global_TrMatriz").clone();

            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao").html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao")        .html(lCodigoAcao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeGrupo")     .html(lNomeGrupo);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido")       .html(lDirecao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_NomeAcao")      .html(lNomeAcao);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemGrupo_Global_TrMatriz");
        }

        lTr.show();

        lTabela.prepend(lTr);

        lTr.effect("highlight", {}, 3700);
    }

}



function GradIntra_Risco_Restricoes_ItensDoGrupo_SelecionarGrupo(pSender) 
{
    var lIdGrupo = $("#GradIntra_Risco_ItensDoGrupo_Grupos").val();

    if ("" == lIdGrupo) {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else {
        var lURL = "Risco/Formularios/Dados/AtivosGruposRestricoes.aspx";
        var lObjetoDeParametros = { Acao: "CarregarListaGrupoItem",
            IdGrupo: $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_Restricoes_ItensDoGrupo_SelecionarGrupo_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_SelecionarGrupo_Callback(pResposta) 
{
    if (pResposta.TemErro) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else 
    {
        var lTr;
        var lThis;
        var lTabela          = $("#tblGradIntra_Risco_ItemGrupo tbody");
        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        lTabela.find("tr:visible").remove();

        for (var i = 0; i < lObjetoDeRetorno.length; i++) 
        {
            lThis = $(lObjetoDeRetorno[i]);
            lTr = $(".tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz").clone();

            lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");
            lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html(lThis[0].Id);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lThis[0].Nome);
            lTr.show();

            lTabela.append(lTr);
        }

        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").show();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricaoSpider(pSender) 
{
    var lIdGrupo = $("#GradIntra_Risco_ItensDoGrupo_Grupos_Spider").val();

    if ("" == lIdGrupo) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else {
        var lURL = "Risco/Formularios/Dados/AtivosGruposRestricoesSpider.aspx";
        var lObjetoDeParametros = { Acao: "CarregarListaGrupoItem",
            IdGrupo: $("#GradIntra_Risco_ItensDoGrupo_Grupos_Spider").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricaoSpider_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricaoSpider_Callback(pResposta) 
{
    if (pResposta.TemErro) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else 
    {
        var lTr;
        var lThis;
        var lTabela = $("#tblGradIntra_Risco_ItemGrupo_Spider tbody");
        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        lTabela.find("tr:visible").remove();

        for (var i = 0; i < lObjetoDeRetorno.length; i++) 
        {
            lThis = $(lObjetoDeRetorno[i]);
            
            lTr   = $(".tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz_Spider").clone();

            lTr.removeClass("tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz_Spider");
            
            lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html(lThis[0].Id);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lThis[0].Nome);
            
            lTr.show();

            lTabela.append(lTr);
        }

        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").show();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricao(pSender) 
{
    var lIdGrupo = $("#GradIntra_Risco_ItensDoGrupo_Grupos").val();

    if ("" == lIdGrupo) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else {
        var lURL = "Risco/Formularios/Dados/AtivosGruposRestricoes.aspx";
        var lObjetoDeParametros = { Acao: "CarregarListaGrupoItem",
            IdGrupo: $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricao_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricao_Callback(pResposta) 
{
    if (pResposta.TemErro) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else 
    {
        var lTr;
        var lThis;
        var lTabela = $("#tblGradIntra_Risco_ItemGrupo tbody");
        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        lTabela.find("tr:visible").remove();

        for (var i = 0; i < lObjetoDeRetorno.length; i++) 
        {
            lThis = $(lObjetoDeRetorno[i]);
            
            lTr   = $(".tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz").clone();

            lTr.removeClass("tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz");
            
            lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html(lThis[0].Id);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lThis[0].Nome);
            
            lTr.show();

            lTabela.append(lTr);
        }

        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").show();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupo(pSender)
{
    var lIdGrupo = $("#GradIntra_Risco_ItensDoGrupo_Grupos").val();

    if ("" == lIdGrupo)
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else
    {
        var lURL = "Risco/Formularios/Dados/Itens.aspx";
        var lObjetoDeParametros = { Acao    : "CarregarListaGrupoItem",
                                    IdGrupo : $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
                                  };

        GradIntra_CarregarJsonVerificandoErro( lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItensDoGrupo_SelecionarGrupo_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupo_Callback(pResposta)
{
    if (pResposta.TemErro)
    {   
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else
    {
        var lTr;
        var lThis;
        var lTabela = $("#tblGradIntra_Risco_ItemGrupo tbody");
        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        lTabela.find("tr:visible").remove();
        
        for (var i = 0; i < lObjetoDeRetorno.length; i++)
        {
            lThis = $(lObjetoDeRetorno[i]);
            lTr = $(".tdGradIntra_Risco_ItemGrupo_TrMatriz").clone();

            lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");
            lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html(lThis[0].Id);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lThis[0].Nome);
            lTr.show();

            lTabela.append(lTr);
        }

        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").show();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_ItensDoGrupoRestricoes_Excluir(pSender)
{
    if (confirm("Realmente deseja excluir este item?"))
    {
        var lTr = $(pSender).closest("tr");

        if (lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco").hasClass("tdGradIntra_Risco_ItemGrupo_ItemNovo"))
        {
            lTr.remove();
        }
        else
        {   
            var lURL = "Risco/Formularios/Dados/AtivoesGruposRestricoes.aspx";
            var lIdItemGrupo = lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html()
            var lObjetoDeParametros = { Acao        : "ExcluirGrupoItem"
                                      , IdItemGrupo : lIdItemGrupo
                                      , IdGrupo     : $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
                                      };

            GradIntra_CarregarJsonVerificandoErro( lURL
                                                 , lObjetoDeParametros
                                                 , function (pResposta) { GradIntra_Risco_ItensDoGrupo_Excluir_Callback(pResposta); }
                                                 );
        }
    }

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoCliente_Spider_Excluir(pSender) 
{
    if (confirm("Realmente deseja excluir este item?")) 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoesSpider.aspx";

        var lTr = $(pSender).closest("tr");

        var lCliente = lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente").html();
        var lAtivo   = lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html();
        var lSentido = lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido").html();

        
        var lObjetoDeParametros = { Acao: "ExcluirClienteBloqueioInstrumento"
                                      , CodigoCliente: lCliente
                                      , Direcao      : lSentido
                                      , Ativo        : lAtivo
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                                 , lObjetoDeParametros
                                                 , null//function (pResposta) { GradIntra_Risco_ItensDoGrupo_Excluir_Callback(pResposta); }
                                                 );
        lTr.remove();

        GradIntra_ExibirMensagem("A", "Bloqueio excluído com sucesso!");
    }
}

function GradIntra_Risco_Restricoes_ItensDoCliente_Excluir(pSender) 
{
    if (confirm("Realmente deseja excluir este item?")) 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";

        var lTr = $(pSender).closest("tr");

        var lCliente = lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente").html();
        var lAtivo   = lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html();
        var lSentido = lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido").html();

        
        var lObjetoDeParametros = { Acao: "ExcluirClienteBloqueioInstrumento"
                                      , CodigoCliente: lCliente
                                      , Direcao      : lSentido
                                      , Ativo        : lAtivo
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                                 , lObjetoDeParametros
                                                 , null//function (pResposta) { GradIntra_Risco_ItensDoGrupo_Excluir_Callback(pResposta); }
                                                 );
        lTr.remove();

        GradIntra_ExibirMensagem("A", "Bloqueio excluído com sucesso!");
    }
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Spider_Excluir(pSender) 
{
    if (confirm("Realmente deseja excluir este item?")) 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoesSpider.aspx";

        var lTr = $(pSender).closest("tr");

        var lCliente   = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Cliente").html();
        var lDirecao   = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Sentido").html();
        var lAcao      = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdAcao").html();
        var lGrupo     = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdItem").html();
        
        var lObjetoDeParametros = { Acao: "ExcluirRegraGrupoItem"
                                      , CodigoCliente: lCliente
                                      , Direcao      : lDirecao
                                      , CodigoAcao   : lAcao
                                      , IdGrupo      : lGrupo
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                                 , lObjetoDeParametros
                                                 , null
                                                 );
        lTr.remove();

        GradIntra_ExibirMensagem("A", "Grupo item excluído com sucesso!");
    }
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Excluir(pSender) 
{
    if (confirm("Realmente deseja excluir este item?")) 
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";

        var lTr = $(pSender).closest("tr");

        var lCliente   = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Cliente").html();
        var lDirecao   = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_Sentido").html();
        var lAcao      = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdAcao").html();
        var lGrupo     = lTr.find(".tdGradIntra_Risco_ItemRegraGrupo_IdItem").html();
        
        var lObjetoDeParametros = { Acao: "ExcluirRegraGrupoItem"
                                      , CodigoCliente: lCliente
                                      , Direcao      : lDirecao
                                      , CodigoAcao   : lAcao
                                      , IdGrupo      : lGrupo
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                                 , lObjetoDeParametros
                                                 , null
                                                 );
        lTr.remove();

        GradIntra_ExibirMensagem("A", "Grupo item excluído com sucesso!");
    }
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Spider_Excluir(pSender) 
{
    if (confirm("Realmente deseja excluir este item?"))
    { 
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoesSpider.aspx";

        var lTr = $(pSender).closest("tr");

        var lDirecao   = lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido").html();
        var lAcao      = lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao").html();
        var lGrupo     = lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdGrupo").html();
        
        var lObjetoDeParametros = { Acao: "ExcluirRegraGrupoItemGlobal"
                                      , Direcao      : lDirecao
                                      , CodigoAcao   : lAcao
                                      , IdGrupo      : lGrupo
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                                 , lObjetoDeParametros
                                                 , null
                                                 );
        lTr.remove();

        GradIntra_ExibirMensagem("A", "Grupo item excluído com sucesso!");
    }
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Excluir(pSender) 
{
    if (confirm("Realmente deseja excluir este item?"))
    { 
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";

        var lTr = $(pSender).closest("tr");


        var lDirecao   = lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_Sentido").html();
        var lAcao      = lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdAcao").html();
        var lGrupo     = lTr.find(".tdGradIntra_Risco_ItemGrupo_Global_IdGrupo").html();
        
        var lObjetoDeParametros = { Acao: "ExcluirRegraGrupoItemGlobal"
                                      , Direcao      : lDirecao
                                      , CodigoAcao   : lAcao
                                      , IdGrupo      : lGrupo
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                                 , lObjetoDeParametros
                                                 , null
                                                 );
        lTr.remove();

        GradIntra_ExibirMensagem("A", "Grupo item excluído com sucesso!");
    }
}

function GradIntra_Risco_ItensDoGrupo_Excluir_Spider(pSender)
{
    if (confirm("Realmente deseja excluir este item?"))
    {
        var lTr = $(pSender).closest("tr");

        if (lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco").hasClass("tdGradIntra_Risco_ItemGrupo_ItemNovo"))
        {
            lTr.remove();
        }
        else
        {   
            var lURL = "Risco/Formularios/Dados/ItensSpider.aspx";
            var lIdItemGrupo = lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html()
            var lObjetoDeParametros = { Acao        : "ExcluirGrupoItem"
                                      , IdItemGrupo : lIdItemGrupo
                                      , IdGrupo     : $("#GradIntra_Risco_ItensDoGrupo_Grupos_Spider").val()
                                      };

            GradIntra_CarregarJsonVerificandoErro( lURL
                                                 , lObjetoDeParametros
                                                 , function (pResposta) { GradIntra_Risco_ItensDoGrupo_Excluir_Spider_Callback(pResposta); }
                                                 );
        }
    }

    return false;
}

function GradIntra_Risco_ItensDoGrupo_Excluir_Spider_Callback(pResposta)
{
    if (pResposta.TemErro)
    {   
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        $("#tblGradIntra_Risco_ItemGrupo_Spider tbody td:[class=tdGradIntra_Risco_ItemGrupo_IdItem]").each(function()
        {
            if ($(this).html() == pResposta.ObjetoDeRetorno)
                $(this).closest("tr").remove();
        })

        GradIntra_ExibirMensagem("A", "Grupo item excluído com sucesso!");
    }
}


function GradIntra_Risco_ItensDoGrupo_Excluir(pSender)
{
    if (confirm("Realmente deseja excluir este item?"))
    {
        var lTr = $(pSender).closest("tr");

        if (lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco").hasClass("tdGradIntra_Risco_ItemGrupo_ItemNovo"))
        {
            lTr.remove();
        }
        else
        {   
            var lURL = "Risco/Formularios/Dados/Itens.aspx";
            var lIdItemGrupo = lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html()
            var lObjetoDeParametros = { Acao        : "ExcluirGrupoItem"
                                      , IdItemGrupo : lIdItemGrupo
                                      , IdGrupo     : $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
                                      };

            GradIntra_CarregarJsonVerificandoErro( lURL
                                                 , lObjetoDeParametros
                                                 , function (pResposta) { GradIntra_Risco_ItensDoGrupo_Excluir_Callback(pResposta); }
                                                 );
        }
    }

    return false;
}

function GradIntra_Risco_ItensDoGrupo_Excluir_Callback(pResposta)
{
    if (pResposta.TemErro)
    {   
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        $("#tblGradIntra_Risco_ItemGrupo tbody td:[class=tdGradIntra_Risco_ItemGrupo_IdItem]").each(function()
        {
            if ($(this).html() == pResposta.ObjetoDeRetorno)
                $(this).closest("tr").remove();
        })

        GradIntra_ExibirMensagem("A", "Grupo item excluído com sucesso!");
    }
}

function GradIntra_Risco_ItemDeGrupo_AdicionarItem(pSender)
{
    if ($(pSender).closest("div").validationEngine({returnIsValid:true}))
    {
        var lTxtDescricaoItemGrupo = $("#GradIntra_Risco_ItemDeGrupo_Descricao");
        var lTabela = $("#tblGradIntra_Risco_ItemGrupo tbody");
        var lTr = $(".tdGradIntra_Risco_ItemGrupo_TrMatriz").clone();
    
        lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");
        lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco").addClass("tdGradIntra_Risco_ItemGrupo_ItemNovo");
        lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lTxtDescricaoItemGrupo.val());
        lTr.show();

        lTabela.prepend(lTr);

        lTr.effect("highlight", {}, 3700);

        lTxtDescricaoItemGrupo.val("");
    }

    return false;
}



function GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem_Spider(pSender)
{
    if ($(pSender).closest("div").validationEngine({ returnIsValid: true })) 
    {
//        var lTabela;
//        var lTr;

//        var lCliente    = $("#txtCliente_Restricoes_RestricaoPorCliente").val();
//        var lPapel      = $("#txtCliente_Restricoes_RestricaoPorAtivo").val();
//        var lRestringir = $("#ckbGradIntraRisco_Restricao_Global_OMS").val();
//        var lDirecao    = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();
//        
//        lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
//        lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatriz").clone();

//        //lTr.find("tdGradIntra_Risco_ItemCliente_IdItem")      .html();
//        //lTr.find("tdGradIntra_Risco_ItemCliente_ItemDoBanco") .html();
//        lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente")      .html(lCliente);
//        lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento")  .html(lPapel);
//        lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido")      .html(lDirecao);
//        lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatriz");

        var lTxtDescricaoItemGrupo = $("#GradIntra_Risco_ItemDeGrupo_Descricao_Spider");
        var lTabela                = $("#tblGradIntra_Risco_ItemGrupo_Spider tbody");
        var lTr                    = $(".tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz_Spider").clone();

        lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");

        lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco") .addClass("tdGradIntra_Risco_ItemGrupo_ItemNovo");
        lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem")      .html(lTxtDescricaoItemGrupo.val().toUpperCase());
        lTr.show();

        lTabela.prepend(lTr);

        lTr.effect("highlight", {}, 3700);

        lTxtDescricaoItemGrupo.val("");
    }

    return false;
}




function GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem(pSender)
{
    if ($(pSender).closest("div").validationEngine({ returnIsValid: true })) 
    {
//        var lTabela;
//        var lTr;

//        var lCliente    = $("#txtCliente_Restricoes_RestricaoPorCliente").val();
//        var lPapel      = $("#txtCliente_Restricoes_RestricaoPorAtivo").val();
//        var lRestringir = $("#ckbGradIntraRisco_Restricao_Global_OMS").val();
//        var lDirecao    = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();
//        
//        lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
//        lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatriz").clone();

//        //lTr.find("tdGradIntra_Risco_ItemCliente_IdItem")      .html();
//        //lTr.find("tdGradIntra_Risco_ItemCliente_ItemDoBanco") .html();
//        lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente")      .html(lCliente);
//        lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento")  .html(lPapel);
//        lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido")      .html(lDirecao);
//        lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatriz");

        var lTxtDescricaoItemGrupo = $("#GradIntra_Risco_ItemDeGrupo_Descricao");
        var lTabela                = $("#tblGradIntra_Risco_ItemGrupo tbody");
        var lTr                    = $(".tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz").clone();

        lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");

        lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco") .addClass("tdGradIntra_Risco_ItemGrupo_ItemNovo");
        lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem")      .html(lTxtDescricaoItemGrupo.val());
        lTr.show();

        lTabela.prepend(lTr);

        lTr.effect("highlight", {}, 3700);

        lTxtDescricaoItemGrupo.val("");
    }

    return false;
}

function GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem_Callback(pResposta) 
{
    if (pResposta.TemErro) 
    {
        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").hide();
        GradIntra_ExibirMensagem("E", "Existem campos inválidos, favor verificar");
    }
    else 
    {
        var lTr;
        var lThis;
        var lTabela = $("#tblGradIntra_Risco_ItemGrupo tbody");
        var lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

        lTabela.find("tr:visible").remove();

        for (var i = 0; i < lObjetoDeRetorno.length; i++) 
        {
            lThis = $(lObjetoDeRetorno[i]);
            lTr = $(".tdGradIntra_Risco_ItemGrupo_TrMatriz").clone();

            lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");
            lTr.find(".tdGradIntra_Risco_ItemGrupo_IdItem").html(lThis[0].Id);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lThis[0].Nome);
            lTr.show();

            lTabela.append(lTr);
        }

        $("#GradIntra_Risco_ItemDeGrupo_Fieldset").show();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_Restricoes_ItemDeGrupo_AdicionarItem(pSender) 
{
    if ($(pSender).closest("div").validationEngine({ returnIsValid: true })) 
    {
        var lTabela;
        var lTr;

        var lCliente    = $("#txtCliente_Restricoes_RestricaoPorCliente").val();
        var lPapel      = $("#txtCliente_Restricoes_RestricaoPorAtivo").val();
        var lRestringir = $("#ckbGradIntraRisco_Restricao_Global_OMS").val();
        var lDirecao    = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();

        if ($("#DivGradIntraRisco_Restricao_Global_OMS").is(":none"))
        {
            lTabela = $("#tblGradIntra_Risco_ItemCliente tbody");
            lTr = $(".tdGradIntra_Risco_ItemCliente_TrMatriz").clone();

            //lTr.find("tdGradIntra_Risco_ItemCliente_IdItem")      .html();
            //lTr.find("tdGradIntra_Risco_ItemCliente_ItemDoBanco") .html();
            lTr.find(".tdGradIntra_Risco_ItemCliente_Cliente").html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Instrumento").html(lPapel);
            lTr.find(".tdGradIntra_Risco_ItemCliente_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemCliente_TrMatriz");
        }
        else
        {
            lTabela = $("#tblGradIntra_Risco_ItemGrupo tbody");
            lTr = $(".tdGradIntra_Risco_ItemGrupo_TrMatriz").clone();

            lTr.find(".tdGradIntra_Risco_ItemGrupo_Cliente").html(lCliente);
            lTr.find(".tdGradIntra_Risco_ItemGrupo_Sentido").html(lDirecao);
            lTr.removeClass("tdGradIntra_Risco_ItemGrupo_TrMatriz");
        }

        
        lTr.find(".tdGradIntra_Risco_ItemGrupo_ItemDoBanco").addClass("tdGradIntra_Risco_ItemGrupo_ItemNovo");
        //lTr.find(".tdGradIntra_Risco_ItemGrupo_DsItem").html(lTxtDescricaoItemGrupo.val());
        lTr.show();

        lTabela.prepend(lTr);

        lTr.effect("highlight", {}, 3700);

        //lTxtDescricaoItemGrupo.val("");
    }

    return false;
}

function GradIntra_Risco_ItemDeGrupoRestricao_Gravar_Spider(pSender)
{
    var lItemGrupoDescricao = "";
    var lItensNovos = $(".tdGradIntra_Risco_ItemGrupo_ItemNovo");

    if (lItensNovos.length > 0) {
        $(".tdGradIntra_Risco_ItemGrupo_ItemNovo").each(function () {
            lItemGrupoDescricao += "|" + $(this).closest("tr").find(".tdGradIntra_Risco_ItemGrupo_DsItem").html();
        })

        var lURL = "Risco/Formularios/Dados/AtivosGruposRestricoesSpider.aspx";
        var lObjetoDeParametros = { Acao: "SalvarGrupoItem"
                                  , GrupoItensNovos: lItemGrupoDescricao
                                  , IdGrupo: $("#GradIntra_Risco_ItensDoGrupo_Grupos_Spider").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Spider_Callback(pResposta); }
                                             );
    }
    else {
        GradIntra_ExibirMensagem("E", "Nenhum item foi adicionado a este Grupo no momento.");
    }
}

function GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Spider_Callback(pResposta)
{
    if (pResposta.TemErro) 
    {
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else 
    {
        GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricaoSpider();

        setTimeout("GradIntra_ExibirMensagem(\"A\", \"" + pResposta.Mensagem + "\")", 750);
    }
}

function GradIntra_Risco_ItemDeGrupoRestricao_Gravar(pSender)
{
    var lItemGrupoDescricao = "";
    var lItensNovos = $(".tdGradIntra_Risco_ItemGrupo_ItemNovo");

    if (lItensNovos.length > 0) {
        $(".tdGradIntra_Risco_ItemGrupo_ItemNovo").each(function () {
            lItemGrupoDescricao += "|" + $(this).closest("tr").find(".tdGradIntra_Risco_ItemGrupo_DsItem").html();
        })

        var lURL = "Risco/Formularios/Dados/AtivosGruposRestricoes.aspx";
        var lObjetoDeParametros = { Acao: "SalvarGrupoItem"
                                  , GrupoItensNovos: lItemGrupoDescricao
                                  , IdGrupo: $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
        };

        GradIntra_CarregarJsonVerificandoErro(lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Callback(pResposta); }
                                             );
    }
    else {
        GradIntra_ExibirMensagem("E", "Nenhum item foi adicionado a este Grupo no momento.");
    }
}

function GradIntra_Risco_Restricoes_Salvar_Dados(pSender)
{
    var lItemGrupoDescricao = "";
    var lItensNovos = $(".tdGradIntra_Risco_ItemGrupo_ItemNovo");

    if (lItensNovos.length > 0)
    {
        $(".tdGradIntra_Risco_ItemGrupo_ItemNovo").each(function()
        { 
            lItemGrupoDescricao += "|" + $(this).closest("tr").find(".tdGradIntra_Risco_ItemGrupo_DsItem").html();
        })
         
        var lURL = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";

        var lObjetoDeParametros = { Acao            : "SalvarGrupoItem"
                                  , GrupoItensNovos : lItemGrupoDescricao
                                  , IdGrupo         : $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
                                  };

        GradIntra_CarregarJsonVerificandoErro( lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItemDeGrupo_Gravar_Callback(pResposta); }
                                             );
    }
    else
    {
        GradIntra_ExibirMensagem("E", "Nenhum item foi adicionado a este Grupo no momento.");
    }
}

function GradIntra_Risco_ItemDeGrupo_Gravar(pSender)
{
    var lItemGrupoDescricao = "";
    var lItensNovos = $(".tdGradIntra_Risco_ItemGrupo_ItemNovo");

    if (lItensNovos.length > 0)
    {
        $(".tdGradIntra_Risco_ItemGrupo_ItemNovo").each(function()
        { 
            lItemGrupoDescricao += "|" + $(this).closest("tr").find(".tdGradIntra_Risco_ItemGrupo_DsItem").html();
        })
         
        var lURL = "Risco/Formularios/Dados/Itens.aspx";
        var lObjetoDeParametros = { Acao            : "SalvarGrupoItem"
                                  , GrupoItensNovos : lItemGrupoDescricao
                                  , IdGrupo         : $("#GradIntra_Risco_ItensDoGrupo_Grupos").val()
                                  };

        GradIntra_CarregarJsonVerificandoErro( lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ItemDeGrupo_Gravar_Callback(pResposta); }
                                             );
    }
    else
    {
        GradIntra_ExibirMensagem("E", "Nenhum item foi adicionado a este Grupo no momento.");
    }
}

function GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Callback(pResposta)
{
    if (pResposta.TemErro) 
    {
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else 
    {
        GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricao();

        setTimeout("GradIntra_ExibirMensagem(\"A\", \"" + pResposta.Mensagem + "\")", 750);
    }
}

function GradIntra_Risco_ItemDeGrupo_Gravar_Callback(pResposta)
{
    if (pResposta.TemErro)
    {   
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        GradIntra_Risco_ItensDoGrupo_SelecionarGrupo();

        setTimeout("GradIntra_ExibirMensagem(\"A\", \"" + pResposta.Mensagem + "\")", 750);
    }
}

/*Final  Itens do Grupo*/

/*Início Parametro de Alavancagem*/

function GradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo(pSender)
{
    var lIdGrupo = $("#cmbGradIntra_Risco_ParametroDeAlavancagem_Grupos").val();

    if ("" == lIdGrupo)
    {
        $("#divGradIntra_Risco_ParametrosDeAlavancagem_Detalhes").hide();
        GradIntra_ExibirMensagem("E", "Selecione um grupo para definir os seus parâmetros");
    }
    else
    {
        var lURL = "Risco/Formularios/Dados/ParametroAlavancagem.aspx";
        var lObjetoDeParametros = { Acao    : "SelecionarParametrosDoGrupo"
                                  , IdGrupo : $("#cmbGradIntra_Risco_ParametroDeAlavancagem_Grupos").val()
                                  };

        GradIntra_CarregarJsonVerificandoErro( lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo_Callback(pResposta); }
                                             );
    }
}

function GradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        $("#divGradIntra_Risco_ParametrosDeAlavancagem_Detalhes").hide();

        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        GradIntra_Risco_ParametroDeAlavancagem_ConfigurarTela(pResposta.ObjetoDeRetorno);

        $("#divGradIntra_Risco_ParametrosDeAlavancagem_Detalhes").show();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradIntra_Risco_ParametroDeAlavancagem_ConfigurarTela(pObjetoDeRetorno)
{
    $("#txtGradIntraRisco_ParametroDeAlavancagem_CC").val(pObjetoDeRetorno.ContaCorrente);
    $("#txtGradIntraRisco_ParametroDeAlavancagem_Custodia").val(pObjetoDeRetorno.Custodia);
    
    $("#txtGradIntraRisco_ParametroDeAlavancagem_CompraAVista").val(pObjetoDeRetorno.CompraAVista);
    $("#txtGradIntraRisco_ParametroDeAlavancagem_VendaAVista").val(pObjetoDeRetorno.VendaAVista);
    $("#txtGradIntraRisco_ParametroDeAlavancagem_CompraOpcao").val(pObjetoDeRetorno.CompraOpcao);
    $("#txtGradIntraRisco_ParametroDeAlavancagem_VendaOpcao").val(pObjetoDeRetorno.VendaOpcao);

    if (pObjetoDeRetorno.UtilizarCarteira23)
        $("#ckbGradIntraRisco_ParametroDeAlavancagem_Carteira23").prop("checked", true).next().addClass("checked");
    else
        $("#ckbGradIntraRisco_ParametroDeAlavancagem_Carteira23").prop("checked", false).next().removeClass("checked");

    if (pObjetoDeRetorno.UtilizarCarteira27)
        $("#ckbGradIntraRisco_ParametroDeAlavancagem_Carteira27").prop("checked", true).next().addClass("checked");
    else
        $("#ckbGradIntraRisco_ParametroDeAlavancagem_Carteira27").prop("checked", false).next().removeClass("checked");
}

function GradIntra_Risco_ParametroDeAlavancagem_SalvarParametros(pSender)
{
    var lURL = "Risco/Formularios/Dados/ParametroAlavancagem.aspx";
    var lObjetoDeParametros = 
                            { Acao        : "SalvarParametrosDoGrupo"
                            , IdGrupo     : $("#cmbGradIntra_Risco_ParametroDeAlavancagem_Grupos").val()
                            , CC          : $("#txtGradIntraRisco_ParametroDeAlavancagem_CC").val()
                            , Custodia    : $("#txtGradIntraRisco_ParametroDeAlavancagem_Custodia").val()
                            , CompraAVista: $("#txtGradIntraRisco_ParametroDeAlavancagem_CompraAVista").val()
                            , VendaAVista : $("#txtGradIntraRisco_ParametroDeAlavancagem_VendaAVista").val()
                            , CompraOpcao : $("#txtGradIntraRisco_ParametroDeAlavancagem_CompraOpcao").val()
                            , VendaOpcao  : $("#txtGradIntraRisco_ParametroDeAlavancagem_VendaOpcao").val()
                            , Carteira23  : $("#ckbGradIntraRisco_ParametroDeAlavancagem_Carteira23").prop("checked")
                            , Carteira27  : $("#ckbGradIntraRisco_ParametroDeAlavancagem_Carteira27").prop("checked")
                            };

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Risco_ParametroDeAlavancagem_SalvarParametros_Callback(pResposta); }
                                         );
}

function GradIntra_Risco_ParametroDeAlavancagem_SalvarParametros_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("A", pResposta.Mensagem);   
    }
}

/*Final  Parametro de Alavancagem*/


//-----------------------------------------------------------------
// InícioManutenção Cliente
//-----------------------------------------------------------------

function GradIntra_Risco_ClienteGrupo_Cliente(pSender)
{
    if ($(pSender).val().length > 0)
    {
        $("#cmbGradIntra_Risco_ClienteGrupo_Assessor").prop("disabled", true).val("");
    }
    else
    {
        $("#cmbGradIntra_Risco_ClienteGrupo_Assessor").prop("disabled", false)
    }
}

function GradIntra_Risco_ClienteGrupo_Assessor(pSender)
{
    if ($(pSender).val() == "")
    {
        $("#txtGradIntra_Risco_ClienteGrupo_Cliente").prop("disabled", false);
    }
    else
    {
        $("#txtGradIntra_Risco_ClienteGrupo_Cliente").prop("disabled", true).val("");
    }
}

function GradIntra_Risco_ClienteGrupo_Gravar(pSender)
{
    var lCodigoCliente = $("#txtGradIntra_Risco_ClienteGrupo_Cliente").val();

    if (lCodigoCliente == "" ||lCodigoCliente.length < 3)
    {
        alert("É necessário inserir um código de cliente válido.");
        return;
    }

    var lObjetoDeParametros = { Acao       : "Gravar"
                              , CdCliente  : $("#txtGradIntra_Risco_ClienteGrupo_Cliente").val()
                              , CdAssessor : $("#cmbGradIntra_Risco_ClienteGrupo_Assessor").val()
                              , IdGrupo    : $("#cmbGradIntra_Risco_ClienteGrupo_Grupos").val()
                              };
                              
    GradIntra_CarregarJsonVerificandoErro( "Risco/Formularios/Dados/ClienteGrupo.aspx"
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Risco_ClienteGrupo_Gravar_Callback(pResposta); }
                                         );
}

function GradIntra_Risco_ClienteGrupo_Gravar_Callback(pResposta)
{
    if (pResposta.TemErro)
    {   
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("I", "Dados inseridos com sucesso.");
    }
}

//-----------------------------------------------------------------
// InícioManutenção Cliente
//-----------------------------------------------------------------

function GradIntra_Risco_ManutencaoCliente_Cliente_KeyUp(pSender)
{
    if ($(pSender).val().length > 0)
    {
        $("#cmbGradIntra_Risco_ManutencaoCliente_Assessor").prop("disabled", true).val("");
    }
    else
    {
        $("#cmbGradIntra_Risco_ManutencaoCliente_Assessor").prop("disabled", false)
    }
}

function GradIntra_Risco_ManutencaoCliente_Assessor_Change(pSender)
{
    if ($(pSender).val().length > 0)
    {
        $("#txtGradIntra_Risco_ManutencaoCliente_Cliente").prop("disabled", true).val("");
    }
    else
    {
        $("#txtGradIntra_Risco_ManutencaoCliente_Cliente").prop("disabled", false)
    }
}

function GradIntra_Risco_ManutencaoCliente_Buscar_Click(pSender)
{
    var lDados = { Acao       : "BuscarItensParaSelecao"
                 , CdCliente  : $("#txtGradIntra_Risco_ManutencaoCliente_Cliente").val()
                 , CdAssessor : $("#cmbGradIntra_Risco_ManutencaoCliente_Assessor").val()
                 , IdGrupo    : $("#cmbGradIntra_Risco_ManutencaoCliente_Grupos").val()
                 , DtInicial  : $("#txtGradIntra_Risco_ManutencaoCliente_DataDe").val()
                 , DtFinal    : $("#txtGradIntra_Risco_ManutencaoCliente_DataAte").val()
                 };
                              
    GradIntra_CarregarJsonVerificandoErro( "Risco/Formularios/Dados/ManutencaoCliente.aspx"
                                         , lDados
                                         , function (pResposta) { GradIntra_Risco_ManutencaoCliente_Buscar_Click_Callback(pResposta); }
                                         );
}

function GradIntra_Risco_ManutencaoCliente_Buscar_Click_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        $("#divGradIntra_Risco_ManutencaoCliente_Grid").hide();
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
        $("#tblGradIntra_Risco_ManutencaoCliente_Grupos.ui-jqgrid-btable")[0].addJSONData(pResposta.ObjetoDeRetorno);

        if (pResposta.ObjetoDeRetorno.TotalDeItens > 0)
        {
            $("#txtGradIntra_Risco_ManutencaoCliente_ItemNaoEncontrado").hide();
            $("#divGradIntra_Risco_ManutencaoCliente_Grid").show();
        }
        else
        {
            $("#divGradIntra_Risco_ManutencaoCliente_Grid").hide();
            $("#txtGradIntra_Risco_ManutencaoCliente_ItemNaoEncontrado").show();
        }
    }
}


function GradIntra_Risco_ManutencaoCliente_Grid_Load()
{
    GradIntra_Risco_ManutencaoCliente_Grid_Load();
}

function GradIntra_Risco_ManutencaoCliente_Grid_Load()
{       
    $("#tblGradIntra_Risco_ManutencaoCliente_Grupos").jqGrid(
    {
        url: "Risco/Formularios/Dados/ManutencaoCliente.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [ { label: "Cliente", name: "Cliente", jsonmap: "Cliente", index: "Cliente", width: 40, align: "left", sortable: false }
                  , { label: "Assessor", name: "Assessor", jsonmap: "Assessor", index: "Assessor", width: 40, align: "left", sortable: false }
                  , { label: "Grupo", name: "NomeDoGrupoGrupo", jsonmap: "NomeDoGrupoGrupo", index: "NomeDoGrupoGrupo", width: 40, align: "left", sortable: false }
                  , { label: "Data de Inclusao", name: "DataInclusao", jsonmap: "DataInclusao", index: "DataInclusao", width: 1, align: "left", sortable: false, hidden: true }
                  , { label: "Excluir", name: "Excluir", jsonmap: "Check", index: "Check", width: 10, align: "center", sortable: false }  
                  ]
      , height: 441
      , pager: "#pnltGradIntra_Risco_ManutencaoCliente_Grupos_Pager"
      , rowNum: 30
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   // flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: { root    : "Itens"
                    , page    : "PaginaAtual"
                    , total   : "TotalDePaginas"
                    , records : "TotalDeItens"
                    , cell    : ""               // vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id      : "0"             // primeira propriedade do elemento de linha é o Id
                    , repeatitems: false
                    }
      , afterInsertRow: GradIntra_Risco_ManutencaoCliente_Grid_ItemDataBound
  });

    //--> Chama o evento de page resize pra já ajustar:
    $("#tblGradIntra_Risco_ManutencaoCliente_Grupos").setGridWidth( 558 );
    $("#tblGradIntra_Risco_ManutencaoCliente_Grupos").setGridHeight( 665 );
}

function GradIntra_Risco_ManutencaoCliente_Grid_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblGradIntra_Risco_ManutencaoCliente_Grupos tr td[title=" + rowdata.Cliente + "]").parent("tr");
    
    lRow.children("td").eq(4).html("<button class=\"IconButton Excluir\" style=\"margin-left:16px;\" id=\"ManutencaoCliente" + rowdata.Cliente +
                                   "\" onclick=\"return GradIntra_Risco_ManutencaoCliente_Excluir(this);\" Value=\"" + rowdata.Cliente + "\" ></button>"
                                  );
}

function GradIntra_Risco_ManutencaoCliente_Excluir(pSender)
{
    if (confirm("Realmente deseja excluir este registro?"))
    {
        var lURL = "Risco/Formularios/Dados/ManutencaoCliente.aspx";
        var lObjetoDeParametros = { Acao      : "Excluir"
                                  , CdCliente :  $(pSender).attr("value")};

        GradIntra_CarregarJsonVerificandoErro( lURL
                                             , lObjetoDeParametros
                                             , function (pResposta) { GradIntra_Risco_ManutencaoCliente_ExcluirCallback(pResposta); }
                                             );
    }
    return false;
}

function GradIntra_Risco_ManutencaoCliente_ExcluirCallback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("E", pResposta.Mensagem);
    }
    else
    {
        $("#tblGradIntra_Risco_ManutencaoCliente_Grupos tr td[title=" + pResposta.ObjetoDeRetorno + "]").parent("tr").remove();

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);   
    }
}

//-----------------------------------------------------------------
//------------Fim-Manutenção Cliente-------------------------------
//-----------------------------------------------------------------

var gTimerMonitoramentoAtualizacaoAutomatica = new Number();
var gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = new Number();
var gTimerMonitoramentoDetalhesAtualizacaoAutomatica = new Number();

var gStatusPararContagem = false;
var gStatusPLDPararContagem = false;
var gStatusPararContagemDetalhamento = false;


function GradIntra_Risco_MonitoramentoDeRisco_AtualizarAutomaticamente()
{
    if (!gStatusPararContagem && $("#chkRisco_MonitoramentoRisco_AtualizarAutomaticamente").is(':checked'))
    {
        $("#lblRisco_Monitoramento_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerMonitoramentoAtualizacaoAutomatica - 1) >= 0)
        {
            gTimerMonitoramentoAtualizacaoAutomatica = gTimerMonitoramentoAtualizacaoAutomatica - 1;
            $("#lblRisco_Monitoramento_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerMonitoramentoAtualizacaoAutomatica + ' segundos.')
            setTimeout('GradIntra_Risco_MonitoramentoDeRisco_AtualizarAutomaticamente()', 1000);
        }
        else
        {
            GradIntra_Risco_Monitoramento_Buscar();
        }
    }

    return false;
}

function GradIntra_Risco_MonitoramentoDeRisco_Detalhes_AtualizarAutomaticamente() 
{
    if (!gStatusPararContagemDetalhamento && $("#chkRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente").is(':checked')) 
    {
        $("#lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerMonitoramentoDetalhesAtualizacaoAutomatica - 1) >= 0) 
        {
            gTimerMonitoramentoDetalhesAtualizacaoAutomatica = gTimerMonitoramentoDetalhesAtualizacaoAutomatica - 1;

            $("#lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerMonitoramentoDetalhesAtualizacaoAutomatica + ' segundos.')

            setTimeout('GradIntra_Risco_MonitoramentoDeRisco_Detalhes_AtualizarAutomaticamente()', 1000);
        }
        else 
        {
            GradIntra_Risco_Monitoramento_LucroPrejuizo_Detalhamento_Busca();
        }
    }

    return false;

}

function chkRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_Click(pSender) 
{
    if ($(pSender).is(":checked")) 
    {

        if (gCodigoBovespaDetalhes == null && gCodigoBMFDetalhes == null)
        {
            return;    
        }
        gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60;

        GradIntra_Risco_Monitoramento_LucroPrejuizo_Detalhamento_Busca();
    }
    else 
    {
        //window.setTimeout = null;
                
        gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 0; //--> Zerando o contador.
        
        $("#lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}

function chkRisco_Monitoramento_Intradiario_AtualizarAutomaticamente_Click(pSender) 
{
    if ($(pSender).is(":checked")) 
    {

//        if (gCodigoBovespaDetalhes == null && gCodigoBMFDetalhes == null)
//        {
//            return;    
//        }
        gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = 60;

        GradIntra_Risco_Monitoramento_Intradiario_Busca();
    }
    else 
    {
        //window.setTimeout = null;
                
        gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = 0; //--> Zerando o contador.
        
        $("#lblRisco_Monitoramento_Intradiario_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}

function GradIntra_Risco_MonitoramentoDeRisco_AtualizarAutomaticamente_Click(pSender)
{
    if ($(pSender).is(":checked"))
    {
        gTimerMonitoramentoAtualizacaoAutomatica = 60;
        GradIntra_Risco_Monitoramento_Buscar();
    }
    else
    {
        gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.
        $("#lblRisco_Monitoramento_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}



function GradIntra_Risco_Monitoramento_Busca_Click(pSender)
{
    GradIntra_Risco_Monitoramento_Buscar();
}

function GradIntra_Risco_MonitoramentoLucrosPrejuizos_Buscar(pSender)
{
    return false;
}
function btnRisco_PLD_Busca_Click() 
{

    gStatusPLDPararContagem = true;
    var lDados = { Acao: "BuscarItensParaSelecao"
                 , Instrumento: $("#txtRisco_PLD_Filtro_Instrumento").val()
                 //, CodigoCliente: $("#txtRisco_PLD_Filtro_CodCliente").val()
                 , Criticidade: $("#cboRisco_PLD_Filtro_Criticidade").val()
    };

    $("#div_Risco_PLD_Resultados").show();

    GradIntra_Risco_PLD_Grid(lDados);
    
    var lHorarioAtual = new Date();

    window.setTimeout(function () {
        var lUltimaAtualizacao = lHorarioAtual.getDate()
                             + "/" + (lHorarioAtual.getMonth() + 1)
                             + "/" + (lHorarioAtual.getFullYear())
                             + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                             + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                             + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

        gStatusPLDPararContagem = false;
        gTimerPLDAtualizacaoAutomatica = 30; //--> Setando o contador.

        $("#lblRisco_PLD_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                           .effect("highlight", {}, 7000)
                                                           .prev().effect("highlight", {}, 7000);


        //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
        GradIntra_Risco_PLD_AtualizarAutomaticamente();
    }, 700);

    return false;
}

function GradIntra_Risco_PLD_AtualizarAutomaticamente() 
{
    if (!gStatusPLDPararContagem && $("#chkRisco_PLD_AtualizarAutomaticamente").is(':checked')) 
    {
        $("#lblRisco_PLD_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerPLDAtualizacaoAutomatica - 1) >= 0) 
        {
            gTimerPLDAtualizacaoAutomatica = gTimerPLDAtualizacaoAutomatica - 1;
            $("#lblRisco_PLD_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerPLDAtualizacaoAutomatica + ' segundos.')
            setTimeout('GradIntra_Risco_PLD_AtualizarAutomaticamente()', 1000);
        }
        else 
        {
            btnRisco_PLD_Busca_Click();
        }
    }

    return false;

}

function GradIntra_Risco_MonitoramentoDeRisco_Intradiario_AtualizarAutomaticamente()
{
    if (!gStatusPararContagem && $("#chkRisco_Monitoramento_Intradiario_AtualizarAutomaticamente").is(':checked'))
    {
        $("#lblRisco_Monitoramento_Intradiario_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerMonitoramentoIntradiarioAtualizacaoAutomatica - 1) >= 0)
        {
            gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = gTimerMonitoramentoIntradiarioAtualizacaoAutomatica - 1;
            $("#lblRisco_Monitoramento_Intradiario_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerMonitoramentoIntradiarioAtualizacaoAutomatica + ' segundos.')
            setTimeout('GradIntra_Risco_MonitoramentoDeRisco_Intradiario_AtualizarAutomaticamente()', 1000);
        }
        else
        {
            GradIntra_Risco_Monitoramento_Intradiario_Busca();
        }
    }

    return false;
}

function GradIntra_Risco_MonitoramentoDeRisco_LucrosPrejuizos_AtualizarAutomaticamente()
{
    if (!gStatusPararContagem && $("#chkRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente").is(':checked'))
    {
        $("#lblRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerMonitoramentoAtualizacaoAutomatica - 1) >= 0)
        {
            gTimerMonitoramentoAtualizacaoAutomatica = gTimerMonitoramentoAtualizacaoAutomatica - 1;
            $("#lblRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerMonitoramentoAtualizacaoAutomatica + ' segundos.')
            setTimeout('GradIntra_Risco_MonitoramentoDeRisco_LucrosPrejuizos_AtualizarAutomaticamente()', 1000);
        }
        else
        {
            GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca();
        }
    }

    return false;
}

function GradIntra_Risco_Monitoramento_LucrosPrejuizos_ExibirLinhasDaPesquisa(pLista)
{
    var lTabela = $("#tblResultado_MonitoramentoRiscoLucrosPrejuizos");
    var lLinhaTemplate = lTabela.find(".LinhaMatrix");
    var lLinhaClone;
    
    lTabela.find("tbody tr.LinhaExibicao").remove();
    
    $(pLista).each(function()
    {
        lLinhaClone = lLinhaTemplate.clone();

        lLinhaClone.find("td#codigoParametro").html(this.Codigo)
        lLinhaClone.find("td#codigo")                       .html(this.Codigo)
        lLinhaClone.find("td#cliente")                      .html(this.Cliente)
        lLinhaClone.find("td#assessor")                     .html(this.Assessor)
        lLinhaClone.find("td#ContaCorrenteAbertura")        .html(this.ContaCorrenteAbertura)
        lLinhaClone.find("td#CustodiaAbertura")             .html(this.CustodiaAbertura)
        lLinhaClone.find("td#DtAtualizacao")                .html(this.DtAtualizacao)
        lLinhaClone.find("td#LucroPrejuizoBMF")             .html(this.LucroPrejuizoBMF)
        lLinhaClone.find("td#LucroPrejuizoBOVESPA")         .html(this.LucroPrejuizoBOVESPA)
        lLinhaClone.find("td#LucroPrejuizoTOTAL")           .html(this.LucroPrejuizoTOTAL)
        lLinhaClone.find("td#NetOperacoes")                 .html(this.NetOperacoes)
        lLinhaClone.find("td#PatrimonioLiquidoTempoReal")   .html(this.PatrimonioLiquidoTempoReal)
        lLinhaClone.find("td#PLAberturaBMF")                .html(this.PLAberturaBMF)
        lLinhaClone.find("td#PLAberturaBovespa")            .html(this.PLAberturaBovespa)
        lLinhaClone.find("td#SaldoBMF")                     .html(this.SaldoBMF)
        lLinhaClone.find("td#SaldoContaMargem")             .html(this.SaldoContaMargem)
        lLinhaClone.find("td#TotalContaCorrenteTempoReal")  .html(this.TotalContaCorrenteTempoReal)
        lLinhaClone.find("td#TotalGarantias")               .html(this.TotalGarantias)
        lLinhaClone.find("td#LimiteAVista")                 .html(this.LimiteAVista)
        lLinhaClone.find("td#LimiteDisponivel")             .html(this.LimiteDisponivel)
        lLinhaClone.find("td#LimiteOpcoes")                 .html(this.LimiteOpcoes)
        lLinhaClone.find("td#LimiteTotal")                  .html(this.LimiteTotal)

        lLinhaClone.removeClass("LinhaMatrix").addClass("LinhaExibicao");

        lLinhaClone.show();
        
        lTabela.find("tbody").append(lLinhaClone);
    });
}

function GradIntra_Risco_Monitoramento_Buscar()
{
    gStatusPararContagem = true;
    gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.

    var lURL = "MonitoramentoRisco.aspx";
    var lObjetoDeParametros = { Acao              : "BuscarItensParaSelecao"
                              , CdCliente         : $("#txtDBM_FiltroRelatorio_CodigoCliente").val()
                              , CdParametro       : $("#cboRisco_Monitoramento_FiltroParametro").val()
                              , CdGrupo           : $("#cboRisco_Monitoramento_FiltroGrupoAlavancagem").val()
                              , CdAssessor        : $("#txtDBM_FiltroRelatorio_CodAssessor").val()
                              , CkMaior75Menor100 : $("#chkRisco_Monitoramento_LimiteMaior75").is(":checked")
                              , CkMaior50Menos75  : $("#chkRisco_Monitoramento_LimiteMaior50Menor75").is(":checked")
                              , CkMenor50         : $("#chkRisco_Monitoramento_LimiteMenor50").is(":checked") };

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Risco_Monitoramento_Buscar_Callback(pResposta); });
}

function GradIntra_Risco_Monitoramento_Buscar_Callback(pResposta)
{
    if (!pResposta.TemErro)
    {
        window.setTimeout(function()
        {
            gStatusPararContagem = false;
            gTimerMonitoramentoAtualizacaoAutomatica = 60; //--> Setando o contador.
            $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(pResposta.ObjetoDeRetorno.DataHoraConsulta)
                                                           .effect("highlight", {}, 7000)
                                                           .prev().effect("highlight", {}, 7000);
            GradIntra_Risco_Monitoramento_ExibirLinhasDaPesquisa(pResposta.ObjetoDeRetorno.ListaExposicaoRisco);
            $("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
            GradIntra_Risco_MonitoramentoDeRisco_AtualizarAutomaticamente();
        }, 700);
    }
}

function GradIntra_Risco_Monitoramento_ExportarParaExcel()
{
    gStatusPararContagem = true;
    gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.

    var lUrl = "MonitoramentoRisco.aspx";

    lUrl += "?Acao=ExportarParaExcel";

    lUrl += "&CdCliente="         + $("#txtDBM_FiltroRelatorio_CodigoCliente").val()
         +  "&CdParametro="       + $("#cboRisco_Monitoramento_FiltroParametro").val()
         +  "&DsParametro="       + $("#cboRisco_Monitoramento_FiltroParametro option:selected").html()
         +  "&CdGrupo="           + $("#cboRisco_Monitoramento_FiltroGrupoAlavancagem").val()
         +  "&DsGrupo="           + $("#cboRisco_Monitoramento_FiltroGrupoAlavancagem option:selected").html()
         +  "&CdAssessor="        + $("#txtDBM_FiltroRelatorio_CodAssessor").val()
         +  "&CkMaior75Menor100=" + $("#chkRisco_Monitoramento_LimiteMaior75").is(":checked")
         +  "&CkMaior50Menos75="  + $("#chkRisco_Monitoramento_LimiteMaior50Menor75").is(":checked")
         +  "&CkMenor50="         + $("#chkRisco_Monitoramento_LimiteMenor50").is(":checked");
         
    window.open(lUrl);
}

function GradIntra_Risco_Monitoramento_ExibirLinhasDaPesquisa(pLista)
{
    var lTabela = $("#pnlResultado_MonitoramentoRisco");
    var lLinhaTemplate = lTabela.find(".LinhaMatrix");
    var lLinhaClone;

    lTabela.find("tbody tr.LinhaExibicao").remove();

    $("#lblRisco_Monitoramento_AssessorConsultado").html($("#txtDBM_FiltroRelatorio_CodAssessor").val() == "" ? "[Todos]" : $("#txtDBM_FiltroRelatorio_CodAssessor").val());
    $("#lblRisco_Monitoramento_ClienteConsultado").html($("#txtDBM_FiltroRelatorio_CodigoCliente").val() == "" ? "[Todos]" : $("#txtDBM_FiltroRelatorio_CodigoCliente").val());
    $("#lblRisco_Monitoramento_AlavancagemConsultado").html($("#cboRisco_Monitoramento_FiltroGrupoAlavancagem option:selected").html());
    $("#lblRisco_Monitoramento_ParametroConsultado").html($("#cboRisco_Monitoramento_FiltroParametro option:selected").html());

    $(pLista).each(function()
    {
        lLinhaClone = lLinhaTemplate.clone();
        
        lLinhaClone.find("td#status").addClass(this.Criticidade) //--> Adicionando a classe com o semáforo de agressão do limite: verde, amarelo ou vermelho.
        lLinhaClone.find("td#cliente").html(this.Cliente)
        lLinhaClone.find("td#assessor").html(this.Assessor)
        lLinhaClone.find("td#parametro").html(this.Parametro)
        lLinhaClone.find("td#grupo").html(this.Grupo)
        lLinhaClone.find("td#limite").html(this.ValorLimite)
        lLinhaClone.find("td#alocado").html(this.ValorAlocado)
        lLinhaClone.find("td#disponivel").html(this.ValorDisponivel)

        lLinhaClone.removeClass("LinhaMatrix").addClass("LinhaExibicao");

        lLinhaClone.show();
        
        lTabela.find("tbody").append(lLinhaClone);
    });
}


function GradIntra_Risco_Monitoramento_LucrosPrejuizos_OrdernarColuna(pFiltro)
{
    gStatusPararContagem = true;
    gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.
    
    var lURL = "MonitoramentoRiscoGeral.aspx";
    var lObjetoDeParametros = { Acao              : "FiltrarPorColuna"
                              , CodAssessor       : $("#txtDBM_FiltroRelatorio_CodAssessor").val()
                              , CodigoCliente     : $("#txtDBM_FiltroRelatorio_CodigoCliente").val()
                              , GrupoAlavancagem  : $("#cboRisco_Monitoramento_FiltroGrupoAlavancagem").val()
                              , Origem            : $("#cboRisco_Monitoramento_FiltroOrigem").val()
                              , Perda             : $("#cboRisco_Monitoramento_FiltroPerda").val()
                              , LucroPrejuizo     : $("#cboRisco_Monitoramento_FiltroLucroPrejuizo").val()
                              , OrdenarPor        : pFiltro
                              , Ordenacao         : $("#chkRisco_Monitoramento_FiltroOrdenacao").is(':checked')
    };

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca_Callback(pResposta); });
}


var gCodigoBovespaDetalhes;
var gCodigoBMFDetalhes;
var gNomeCliente;
var gCodigoAssessor;
var gNomeAssessor;
var lPopUpDetalhamento;
var gStatusBovespa;
var gStatusBmf;

function GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_Busca(pObjeto, pParametro) 
{
    var codigo          = pParametro.CodigoCliente;

    var codigoBmf       = pParametro.CodigoClienteBmf;

    var lAssessor       = gCodigoAssessor = pParametro.Assessor;

    var lNomeAssessor   = gNomeAssessor = pParametro.NomeAssessor;

    var lNomeCliente    = gNomeCliente = pParametro.NomeCliente;

    var lObjetoDeParametros = { Acao: "BuscarDetalhamento"
                              , CodigoCliente: codigo
                              , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bovespa(lObjetoDeParametros);

    var lObjetoDeParametrosBmf = { Acao: "BuscarDetalhamentoOperacoesBmf"
                              , CodigoCliente: codigo
                              , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bmf(lObjetoDeParametrosBmf);

    var lBuscaSemCarteira = gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira ? "BuscarCustodiaSemCarteira" : "BuscarCustodia";

    var lObjetoDeCustodiaVista = { Acao: lBuscaSemCarteira
                                 , CodigoCliente: codigo
                                 , Mercado: "VIS"
                                 , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Avista(lObjetoDeCustodiaVista);

    var lObjetoDeCustodiaOpcoes = { Acao: lBuscaSemCarteira
                                  , CodigoCliente: codigo
                                  , Mercado: "OPC"
                                  , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Opcoes(lObjetoDeCustodiaOpcoes);

//    var lObjetoDeCustodiaTermo = { Acao: "BuscarCustodia"
//                                 , CodigoCliente: codigo
//                                 , Mercado: "TER"
//                                 , CodigoClienteBmf: codigoBmf
//    };

//    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Termo(lObjetoDeCustodiaTermo);

    var lObjetoDeCustodiaBmf = { Acao: "BuscarCustodia"
                               , CodigoCliente: codigo
                               , Mercado: "FUT"
                               , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Bmf(lObjetoDeCustodiaBmf);

    var lObjetoDeCustodiaBmf = { Acao: "BuscarCustodiaPosicao"
                               , CodigoCliente: codigo
                               , Mercado: "FUT"
                               , CodigoClienteBmf: codigoBmf

    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Posicao_Bmf(lObjetoDeCustodiaBmf);

    var lObjetoDeCustodiaTedi = { Acao: "BuscarCustodia"
                               , CodigoCliente: codigo
                               , Mercado: "TEDI"
                               , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Tesouro(lObjetoDeCustodiaTedi);

//    var lObjetoDeCustodiaBTC = {
//                                 Acao            : "BuscarCustodiaOperacoesBTC"
//                               , CodigoCliente   : codigo
//                               , Mercado         : "BTC"
//                               , CodigoClienteBmf: codigoBmf
//                                };

//    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_BTC(lObjetoDeCustodiaBTC);

    var lObjetoDeCustodiaOperacoesTermo = {
                                 Acao: "BuscarCustodiaOperacoesTermo"
                               , CodigoCliente: codigo
                               , Mercado: "TER"
                               , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo(lObjetoDeCustodiaOperacoesTermo);

    var lObjetoDeCustodiaOperacoesBTC = {
                                 Acao: "BuscarCustodiaOperacoesBTC"
                               , CodigoCliente: codigo
                               , Mercado: "TER"
                               , CodigoClienteBmf: codigoBmf
    };


    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC(lObjetoDeCustodiaOperacoesBTC);

    var lObjetoDeCustodiaFundos = {
                                Acao: "BuscarCustodiaFundos"
                               , CodigoCliente: codigo
                               , Mercado: "FUN"
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos(lObjetoDeCustodiaFundos);

    var lObjetoDeCustodiaRendaFixa = {
                                Acao: "BuscarCustodiaRendaFixa"
                               , CodigoCliente: codigo
                               , Mercado: "FIX"
                               , CodigoClienteBmf: codigoBmf
                               };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_RendaFixa(lObjetoDeCustodiaRendaFixa);

    GradIntra_MonitoramentoRisco_BuscarSaldoLimite_ContaCorrente(codigo, codigoBmf, lAssessor,lNomeAssessor, lNomeCliente);

    //$("#div_MonitoramentoRiscoGeral_Detalhes").show();
}

function GradIntra_Risco_Monitoramento_Intradiario_AbrirDetalhamento_click(pSender) 
{
    var codigo       = $(pSender).parent("td").parent("tr").find("td").eq(2).html(); //--> Código Bovespa do cliente

    var codigoBmf    = $(pSender).parent("td").parent("tr").find("td").eq(13).html(); //--> Código bmf do cliente

    var lAssessor    = $(pSender).parent("td").parent("tr").find("td").eq(4).html(); //--> Assessor do cliente

    var lNomeCliente = $(pSender).parent("td").parent("tr").find("td").eq(3).html(); //--> Nome do cliente

    var lNomeAssessor = $(pSender).parent("td").parent("tr").find("td").eq(5).html(); //--> Nome do assessor

    if (lPopUpDetalhamento == null) 
    {
        lPopUpDetalhamento = window.open('MonitoramentoRiscoGeralDetalhamento.aspx', codigo + ' - ' + lNomeCliente, 'height=800, width=1200,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

        lPopUpDetalhamento.gCodigoBovespaDetalhes   = codigo;

        lPopUpDetalhamento.gCodigoBMFDetalhes       = codigoBmf;

        lPopUpDetalhamento.gCodigoAssessor          = lAssessor;

        lPopUpDetalhamento.gNomeCliente = lNomeCliente;

        lPopUpDetalhamento.gNomeAssessor = lNomeAssessor;
    }

    if (lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca == null) 
    {
        setTimeout(function () { GradIntra_Risco_Monitoramento_Intradiario_AbrirDetalhamento_click(pSender) }, 1000);
    }
    else 
    {
    
        var lObjetoDeParametros = { CodigoCliente: codigo, CodigoClienteBmf: codigoBmf, Assessor: lAssessor, NomeCliente: lNomeCliente, NomeAssessor: lNomeAssessor };

        lPopUpDetalhamento.gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60;

        lPopUpDetalhamento.GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid(this, lObjetoDeParametros);

        ////lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca(this, lObjetoDeParametros);
        //lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas(this, pIdJanela);

        lPopUpDetalhamento = null;
    }
    
    return false;
}

function GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_click(pSender) 
{
    var codigo       = $(pSender).parent("td").parent("tr").find("td").eq(4).html(); //--> Código Bovespa do cliente

    var codigoBmf    = $(pSender).parent("td").parent("tr").find("td").eq(5).html(); //--> Código bmf do cliente

    var lAssessor    = $(pSender).parent("td").parent("tr").find("td").eq(8).html(); //--> Assessor do cliente

    var lNomeCliente = $(pSender).parent("td").parent("tr").find("td").eq(7).html(); //--> Nome do cliente

    var lNomeAssessor = $(pSender).parent("td").parent("tr").find("td").eq(9).html(); //--> Nome do assessor

    if (lPopUpDetalhamento == null) 
    {
        lPopUpDetalhamento = window.open('MonitoramentoRiscoGeralDetalhamento.aspx', codigo + ' - ' + lNomeCliente, 'height=800, width=1200,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

        lPopUpDetalhamento.gCodigoBovespaDetalhes   = codigo;

        lPopUpDetalhamento.gCodigoBMFDetalhes       = codigoBmf;

        lPopUpDetalhamento.gCodigoAssessor          = lAssessor;

        lPopUpDetalhamento.gNomeCliente = lNomeCliente;

        lPopUpDetalhamento.gNomeAssessor = lNomeAssessor;
    }

    if (lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca == null) 
    {
        setTimeout(function () { GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_click(pSender) }, 1000);
    }
    else 
    {
    
        var lObjetoDeParametros = { CodigoCliente: codigo, CodigoClienteBmf: codigoBmf, Assessor: lAssessor, NomeCliente: lNomeCliente, NomeAssessor: lNomeAssessor };

        lPopUpDetalhamento.gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60;

        lPopUpDetalhamento.GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid(this, lObjetoDeParametros);

        ////lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca(this, lObjetoDeParametros);
        //lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas(this, pIdJanela);

        lPopUpDetalhamento = null;
    }
    
    return false;
}

function GradIntra_Risco_Monitoramento_LucroPrejuizo_BuscaDetalhes_Callback(pResposta) 
{

    $("#div_MonitoramentoRiscoGeral_Detalhes").find("#trDetalhe").remove();

    var lTabela = $("#tblResultado_MonitoramentoRiscoGeral_Operacoes");
    var lLinhaTemplate = lTabela.find(".LinhaMatrix");
    var lLinhaClone;

    lTabela.find("tbody tr.LinhaExibicao").remove();

    $(pResposta.ObjetoDeRetorno).each(function () {
        lLinhaClone = lLinhaTemplate.clone();

        lLinhaClone.find("td#Cliente")              .html(this.Cliente)
        lLinhaClone.find("td#Cotacao")              .html(this.Cotacao)
        lLinhaClone.find("td#FinanceiroAbertura")   .html(this.FinanceiroAbertura)
        lLinhaClone.find("td#FinanceiroComprado")   .html(this.FinanceiroComprado)
        lLinhaClone.find("td#FinanceiroVendido ")   .html(this.FinanceiroVendido)
        lLinhaClone.find("td#Instrumento")          .html(this.Instrumento)
        lLinhaClone.find("td#LucroPrejuizo")        .html(this.LucroPrejuizo)
        lLinhaClone.find("td#NetOperacao")          .html(this.NetOperacao)
        lLinhaClone.find("td#PrecoReversao")        .html(this.PrecoReversao)
        lLinhaClone.find("td#QtdeAber")             .html(this.QtdeAber)
        lLinhaClone.find("td#QtdeAtual")            .html(this.QtdeAtual)
        lLinhaClone.find("td#QtdeComprada")         .html(this.QtdeComprada)
        lLinhaClone.find("td#QtdeVendida")          .html(this.QtdeVendida)
        lLinhaClone.find("td#QtReversao")           .html(this.QtReversao)
        lLinhaClone.find("td#TipoMercado")          .html(this.TipoMercado)
        lLinhaClone.find("td#VLMercadoCompra")      .html(this.VLMercadoCompra)
        lLinhaClone.find("td#VLMercadoVenda")       .html(this.VLMercadoVenda)
        lLinhaClone.find("td#VLNegocioVenda")       .html(this.VLNegocioVenda)

        lLinhaClone.removeClass("LinhaMatrix").addClass("LinhaExibicao");

        lLinhaClone.show();

        lTabela.find("tbody").append(lLinhaClone);
    });
}

function GradIntra_MonitoramentoRisco_Detalhamento_Fechar(pSender) 
{
    $("#div_MonitoramentoRiscoGeral_Detalhes").find("#trDetalhe").remove();

    $("#div_MonitoramentoRiscoGeral_Detalhes").hide();

    gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 0; //--> Zerando o contador.

    $("#lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_ContagemRegressiva").hide();

    return false;
}

function GradIntra_Risco_Monitoramento_LucrosPrejuizos_SelecionarTodos(pParametro)
{
    if ($(pParametro).is(":checked"))
        $("#tblResultado_MonitoramentoRiscoLucrosPrejuizos tbody tr.LinhaExibicao :checkbox").prop("checked", true);
    else
        $("#tblResultado_MonitoramentoRiscoLucrosPrejuizos tbody tr.LinhaExibicao :checkbox").prop("checked", false);
}

function GradIntra_Risco_MonitoramentoLucrosPrejuizos_FiltroOrigem(pSender)
{
    if ($(pSender).val() == "")
    {
        $("#opcBM_FiltroRelatorio_CodAssessor_Todos").show();
        $("#cboBM_FiltroRelatorio_CodAssessor").val("");
    }
    else
    {
        $("#opcBM_FiltroRelatorio_CodAssessor_Todos").hide();
        if ($("#cboBM_FiltroRelatorio_CodAssessor").val() == "")
            $("#cboBM_FiltroRelatorio_CodAssessor").val("0");
    }
}

function GradIntra_Risco_Monitoramento_LucrosPrejuizos_ExportarParaExcel()
{
    var lUrl = "MonitoramentoLucrosPrejuizos.aspx?Acao=ExportarParaExcel";

    lUrl += "&CodAssessor="              + $("#cboBM_FiltroRelatorio_CodAssessor").val()
         +  "&CodigoCliente="            + $("#txtDBM_FiltroRelatorio_CodigoCliente").val()
         +  "&Origem="                   + $("#cboRisco_Monitoramento_FiltroOrigem").val()
         +  "&Perda="                    + $("#cboRisco_Monitoramento_FiltroPerda").val()
         +  "&LucroPrejuizo="            + $("#cboRisco_Monitoramento_FiltroLucroPrejuizo").val()
         +  "&ClienteSelecionado="       + ("" == $("#txtDBM_FiltroRelatorio_CodigoCliente").val()       ? "Todos" : $("#txtDBM_FiltroRelatorio_CodigoCliente").val())
         +  "&AssessorSelecionado="      + ("" == $("#cboBM_FiltroRelatorio_CodAssessor").val()          ? "Todos" : $("#cboBM_FiltroRelatorio_CodAssessor option:selected").html())
         +  "&PerdaSelecionada="         + ("" == $("#cboRisco_Monitoramento_FiltroPerda").val()         ? "Todos" : $("#cboRisco_Monitoramento_FiltroPerda option:selected").html())
         +  "&OrigemSelecionada="        + ("" == $("#cboRisco_Monitoramento_FiltroOrigem").val()        ? "Todos" : $("#cboRisco_Monitoramento_FiltroOrigem option:selected").html())
         +  "&LucroPrejuizoSelecionado=" + ("" == $("#cboRisco_Monitoramento_FiltroLucroPrejuizo").val() ? "Todos" : $("#cboRisco_Monitoramento_FiltroLucroPrejuizo option:selected").html());
         
    window.open(lUrl);
}

function Risco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente(pSender)
{
    if ($(pSender).is(":checked"))
    {
        gTimerMonitoramentoAtualizacaoAutomatica = 60;
        GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca();
    }
    else
    {
        gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.
        $("#lblRisco_Monitoramento_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}

function Risco_Monitoramento_Intradiario_AtualizarAutomaticamente(pSender)
{
    if ($(pSender).is(":checked"))
    {
        gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = 60;
        GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca();
    }
    else
    {
        gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = 0; //--> Zerando o contador.
        $("#lblRisco_Monitoramento_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}

function Risco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente_CallBack(pSender)
{

}

function GradIntra_MonitoramentoRisco_Detalhamento_Operacoes_Abrir(pSender) 
{
    var lURL                = "MonitoramentoRiscoGeral.aspx";

    var lCodInstrumento     = $(pSender).closest("tr").find("td#Instrumento").html();

    var codigo              = $(pSender).closest("tr").find("td#Cliente").html();

    var lObjetoDeParametros = { Acao         : "BuscarDetalhamentoOperacoes"
                              , Instrumento  : lCodInstrumento
                              , CodigoCliente: codigo
    };

    GradIntra_CarregarJsonVerificandoErro(lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_MonitoramentoRisco_Detalhamento_Operacoes_Abrir_Callback(pResposta, pSender); });

}

function GradIntra_MonitoramentoRisco_Detalhamento_Operacoes_Abrir_Callback(pResposta, pSender) 
{
    $(pSender).parent("td").closest("table").find("#trDetalhe").remove();

    var lDivTableDetalhe = "<tr id=\"trDetalhe\"><td colspan=\"19\" width=\"50%\">";
    
    /*<div id=\"detalhes\" style=\"display:inline;\">*/

    lDivTableDetalhe += "<table border=\"1\" style=\"width:50%\" > <tr style=\"font-size:  10px;\">";

    //lDivTableDetalhe += "<th>Cliente       </th>";
    lDivTableDetalhe += "<th>Instrumento    </th>";
    lDivTableDetalhe += "<th>Lucro Prejuizo </th>";
    lDivTableDetalhe += "<th>Preço Mercado  </th>";
    lDivTableDetalhe += "<th>Preço Negócio  </th>";
    lDivTableDetalhe += "<th>Quantidade     </th>";
    lDivTableDetalhe += "<th>Sentido        </th>";
    lDivTableDetalhe += "<th>Total Mercado  </th>";
    lDivTableDetalhe += "<th>Total Negócio  </th>";

    lDivTableDetalhe += "</tr>";

    $(pResposta.ObjetoDeRetorno).each(function () {

        lDivTableDetalhe += "<tr height=\"20px\">";

       // lDivTableDetalhe += "<td>" + this.Cliente + "</td>";
        lDivTableDetalhe += "<td align=\"center\">" + this.Instrumento   + "</td>";
        lDivTableDetalhe += "<td align=\"right\">"   + this.LucroPrejuiso + "</td>";
        lDivTableDetalhe += "<td align=\"right\">"   + this.PrecoMercado  + "</td>";
        lDivTableDetalhe += "<td align=\"right\">"   + this.PrecoNegocio  + "</td>";
        lDivTableDetalhe += "<td align=\"right\">"   + this.Quantidade    + "</td>";
        lDivTableDetalhe += "<td align=\"center\">" + this.Sentido       + "</td>";
        lDivTableDetalhe += "<td align=\"right\">"   + this.TotalMercado  + "</td>";
        lDivTableDetalhe += "<td align=\"right\">"   + this.TotalNegocio  + "</td>";

        lDivTableDetalhe += "</tr>";
    });
    
    lDivTableDetalhe +="</table>";

    lDivTableDetalhe += "</div></tr>";

    $(pSender).parent("td").closest("tr").after(lDivTableDetalhe);
}

function GradIntra_Risco_Monitoramento_Intradiario_Busca(pSender, pConsulta) 
{
    gStatusPararContagem = true;
    
    gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = 0; //--> Zerando o contador.

    $("#pnlResultado_MonitoramentoRisco").fadeOut();

    $("#div_Risco_MonitoramentoIntradiario_Resultados").show();

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoIntradiario(pConsulta);

    var lHorarioAtual = new Date();

        window.setTimeout(function () {
            var lUltimaAtualizacao = lHorarioAtual.getDate()
                                 + "/" + (lHorarioAtual.getMonth() + 1)
                                 + "/" + (lHorarioAtual.getFullYear())
                                 + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                                 + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                                 + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

            gStatusPararContagem = false;
        
            gTimerMonitoramentoIntradiarioAtualizacaoAutomatica = 60; //--> Setando o contador.
        
            $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                               .effect("highlight", {}, 7000)
                                                               .prev().effect("highlight", {}, 7000);


            //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
            GradIntra_Risco_MonitoramentoDeRisco_Intradiario_AtualizarAutomaticamente();
        }, 700);
}

function GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca(pSender, pConsulta) 
{
    gStatusPararContagem = true;
    
    gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.

    $("#pnlResultado_MonitoramentoRisco").fadeOut();

    $("#div_Risco_MonitoramentoRisco_Resultados").show();

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco(pConsulta);

    var lHorarioAtual = new Date();

        window.setTimeout(function () {
            var lUltimaAtualizacao = lHorarioAtual.getDate()
                                 + "/" + (lHorarioAtual.getMonth() + 1)
                                 + "/" + (lHorarioAtual.getFullYear())
                                 + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                                 + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                                 + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

            gStatusPararContagem = false;
        
            gTimerMonitoramentoAtualizacaoAutomatica = 60; //--> Setando o contador.
        
            $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                               .effect("highlight", {}, 7000)
                                                               .prev().effect("highlight", {}, 7000);


            //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
            GradIntra_Risco_MonitoramentoDeRisco_LucrosPrejuizos_AtualizarAutomaticamente();
        }, 700);
}

function GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca_Callback(pResposta) 
{
    if (!pResposta.TemErro) 
    {
        var lClienteSelecionado = ("" == $("#txtDBM_FiltroRelatorio_CodigoCliente").val()) ? "[Todos]" : $("#txtDBM_FiltroRelatorio_CodigoCliente").val();
        $("#lblRisco_Monitoramento_ClienteConsultado").html(lClienteSelecionado);

        var lAssessorSelecionado = ("" == $("#cboBM_FiltroRelatorio_CodAssessor").val()) ? "[Todos]" : $("#cboBM_FiltroRelatorio_CodAssessor option:selected").html();
        $("#lblRisco_Monitoramento_AssessorConsultado").html(lAssessorSelecionado);

        var lPerdaSelecionada = ("" == $("#cboRisco_Monitoramento_FiltroPerda").val()) ? "[Todos]" : $("#cboRisco_Monitoramento_FiltroPerda option:selected").html();
        $("#lblRisco_Monitoramento_PerdaConsultado").html(lPerdaSelecionada);

        var lOrigemSelecionada = ("" == $("#cboRisco_Monitoramento_FiltroOrigem").val()) ? "[Todos]" : $("#cboRisco_Monitoramento_FiltroOrigem option:selected").html();
        $("#lblRisco_Monitoramento_OrigemConsultado").html(lOrigemSelecionada);

        var lLucroPrejuizoSelecionado = ("" == $("#cboRisco_Monitoramento_FiltroLucroPrejuizo").val()) ? "[Todos]" : $("#cboRisco_Monitoramento_FiltroLucroPrejuizo option:selected").html();
        $("#lblRisco_Monitoramento_LucroPrejuizoConsultado").html(lLucroPrejuizoSelecionado);

        var lHorarioAtual = new Date();

        window.setTimeout(function () {
            var lUltimaAtualizacao = lHorarioAtual.getDate()
                             + "/" + (lHorarioAtual.getMonth() + 1)
                             + "/" + (lHorarioAtual.getFullYear())
                             + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                             + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                             + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

            gStatusPararContagem = false;
            gTimerMonitoramentoAtualizacaoAutomatica = 60; //--> Setando o contador.
            $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                           .effect("highlight", {}, 7000)
                                                           .prev().effect("highlight", {}, 7000);

            GradIntra_Risco_Monitoramento_LucrosPrejuizos_ExibirLinhasDaPesquisa(pResposta.ObjetoDeRetorno);

            $("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
            GradIntra_Risco_MonitoramentoDeRisco_LucrosPrejuizos_AtualizarAutomaticamente();
        }, 700);
    }
}

gFirstClickBuscarMonitoramentoIntradiario = true;

function GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoIntradiario(pParametros)
{
    $("#pnlResultado_MonitoramentoIntradiario").show();

    if (gFirstClickBuscarMonitoramentoIntradiario != true) 
    {
        $("#tblBusca_MonitoramentoIntradiario_Resultados")
            .setGridParam({ postData : pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoIntradiario = false;

    $("#tblBusca_MonitoramentoIntradiario_Resultados").jqGrid(
    {
        url: "MonitoramentoIntradiario.aspx?Acao=BuscarItensParaSelecao"
      , datatype : "json"
      , mtype    : "POST"
      , hoverrows: true
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"                        , name: "Id"                       , jsonmap: "Codigo"                     , index: "Id"                           , width: 29,  align: "center"}
                      , { label: "Cod.Cliente"               , name: "CodigoCliente"            , jsonmap: "CodigoCliente"              , index: "CodigoCliente"                , width: 54,  align: "center", sortable: true, sorttype:'int'}
                      , { label: "Nome Cliente"              , name: "NomeCliente"              , jsonmap: "NomeCliente"                , index: "NomeCliente"                  , width: 220, align: "left"  }
                      , { label: "Assessor"                  , name: "CodigoAssessor"           , jsonmap: "CodigoAssessor"             , index: "CodigoAssessor"               , width: 54,  align: "center"}
                      , { label: "NM. Assessor"              , name: "NomeAssessor"             , jsonmap: "NomeAssessor"               , index: "NomeAssessor"                 , width: 200, align: "left"  }
                      , { label: "Net"                       , name: "Net"                      , jsonmap: "Net"                        , index: "Net"                          , width: 85,  align: "right" }
                      , { label: "SFP"                       , name: "SFP"                      , jsonmap: "SFP"                        , index: "SFP"                          , width: 85,  align: "right" }
                      , { label: "Posição"                   , name: "Posicao"                  , jsonmap: "Posicao"                    , index: "Posicao"                      , width: 85,  align: "right" }
                      , { label: "LP"                        , name: "Exposicao"                , jsonmap: "Exposicao"                  , index: "Exposicao"                    , width: 85,  align: "right" }
                      , { label: "NET/SFP %"                 , name: "PercentualSFPxNET"        , jsonmap: "PercentualSFPxNET"          , index: "PercentualSFPxNET"            , width: 85,  align: "right" }
                      , { label: "LP/Finan %"                , name: "PercentualFINANxExposicao", jsonmap: "PercentualFINANxExposicao"  , index: "PercentualFINANxExposicao"    , width: 85,  align: "right" }
                      , { label: "Data"                      , name: "Data"                     , jsonmap: "Data"                       , index: "Data"                         , width: 85,  align: "right" }
                      , { label: "CodigoClienteBmf"          , name: "CodigoClienteBmf"         , jsonmap: "CodigoClienteBmf"           , index: "CodigoClienteBmf"             , width: 85,  align: "right" }
                    ]
      , height: '600'
      , width: 1240
      //, pager           : "#pnlBusca_MonitoramentoRisco_Resultados_Pager"
      , rowNum          : 0
      //, sortname        : 'Codigo'
      //, sortorder       : 'asc'
      , sortable        : false
      , viewrecords     : true
      , gridview        : false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid         : false
      , multiselect     : true
      , jsonReader: {
          root: "Itens"
                       , page       : "PaginaAtual"
                       , total      : "TotalDePaginas"
                       , records    : "TotalDeItens"
                       , cell       : ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id         : "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow  : GradIntra_MonitoramentoIntradiario_ItemDataBound
      //, onSelectRow     : GradIntra_MonitoramentoRisco_SelectRow
      //, beforeSelectRow : GradIntra_MonitoramentoRisco_beforeSelectRow
      //, loadComplete    : GradIntra_MonitoramentoRisco_LoadComplete
  }).jqGrid('hideCol', ['Id', 'CodigoClienteBmf']); 
}

function GradIntra_MonitoramentoIntradiario_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoIntradiario_Resultados tr[id=" + rowid + "]");

    lRow.css("height", 25);

    lRow.css("background-color", 'white');

    if (rowelem.Net.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.SFP.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.Posicao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.Exposicao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PercentualSFPxNET.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PercentualFINANxExposicao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }
//    if (rowelem.CustodiaAbertura.indexOf("-") > -1)
//    {
//        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
//    }

//    if (rowelem.LucroPrejuizoBMF.indexOf("-") > -1) 
//    {
//        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
//    }

//    if (rowelem.LucroPrejuizoBOVESPA.indexOf("-") > -1) 
//    {
//        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
//    }

//    if (rowelem.LucroPrejuizoTOTAL.indexOf("-") > -1) 
//    {
//        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
//    }

    //lRow.children("td").eq(0).remove(jQuery(this).find('#' + rowid + ' input[type=checkbox]'));
    lRow.children("td").eq(0)
        .html("<img alt=\"\" style=\"cursor: pointer\" onclick=\"return GradIntra_Risco_Monitoramento_Intradiario_AbrirDetalhamento_click(this)\" src=\"../../../../Skin/Default/Img/Ico_Busca.png\" />");
}

var gFirstClickBuscarMonitoramentoRisco = true;

function GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco(pParametros)
{
    $("#pnlResultado_MonitoramentoRisco").show();

    if (gFirstClickBuscarMonitoramentoRisco != true) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados")
            .setGridParam({ postData : pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRisco = false;

    $("#tblBusca_MonitoramentoRisco_Resultados").jqGrid(
    {
        url: "MonitoramentoRiscoGeral.aspx?Acao=BuscarItensParaSelecao"
      , datatype : "json"
      , mtype    : "POST"
      , hoverrows: true
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        //{ label: ""                          , name: ""                           , jsonmap: "Check"                      , index: "Check"                      , width: 29, align: "center"}
                        { label: ""                          , name: ""                           , jsonmap: "CheckLinhas"                , index: "Check"                      , width: 29, align: "center", formatter: "checkbox", editable: true, edittype: "checkbox", formatoptions: { disabled: false} }
                      , { label: "SemaforoHide"              , name: "SemaforoHide"               , jsonmap: "SemaforoHide"               , index: "SemaforoHide"               , width: 29, align: "center"}
                      , { label: "Semaforo"                  , name: "Semaforo"                   , jsonmap: "Semaforo"                   , index: "Semaforo"                   , width: 45, align: "center"}
                      , { label: "Id"                        , name: "Id"                         , jsonmap: "Codigo"                     , index: "Id"                         , width: 29, align: "center"}
                      , { label: "Cliente"                   , name: "Codigo"                     , jsonmap: "Codigo"                     , index: "Codigo"                     , width: 54, align: "center", sortable: true, sorttype:'int'}
                      , { label: "CodigoBmf"                 , name: "CodigoBmf"                  , jsonmap: "CodigoBmf"                  , index: "CodigoBmf"                  , width: 40, align: "center"}
                      , { label: "Nome Cliente"              , name: "NmCliente"                  , jsonmap: "NmCliente"                  , index: "NmCliente"                  , width: 220, align: "left"}
                      , { label: "Assessor"                  , name: "Assessor"                   , jsonmap: "Assessor"                   , index: "Assessor"                   , width: 54, align: "center" }
                      , { label: "NM. Assessor"              , name: "NomeAssessor"               , jsonmap: "NomeAssessor"               , index: "NomeAssessor"               , width: 200, align: "left" }
                      
                      , { label: "C.C. Abertura"             , name: "ContaCorrenteAbertura"      , jsonmap: "ContaCorrenteAbertura"      , index: "ContaCorrenteAbertura"      , width: 85, align: "right"}
                      , { label: "Custodia Abertura"         , name: "CustodiaAbertura"           , jsonmap: "CustodiaAbertura"           , index: "CustodiaAbertura"           , width: 85, align: "right"}
                      , { label: "Dt. Atualizacao"           , name: "DtAtualizacao"              , jsonmap: "DtAtualizacao"              , index: "DtAtualizacao"              , width: 85, align: "center"}
                      , { label: "Lucro Prejuízo BMF"        , name: "LucroPrejuizoBMF"           , jsonmap: "LucroPrejuizoBMF"           , index: "LucroPrejuizoBMF"           , width: 85, align: "right"}
                      , { label: "Lucro Prejuízo BOVESPA"    , name: "LucroPrejuizoBOVESPA"       , jsonmap: "LucroPrejuizoBOVESPA"       , index: "LucroPrejuizoBOVESPA"       , width: 100, align: "right"}
                      , { label: "Lucro Prejuízo TOTAL"      , name: "LucroPrejuizoTOTAL"         , jsonmap: "LucroPrejuizoTOTAL"         , index: "LucroPrejuizoTOTAL"         , width: 95, align: "right"}
                      , { label: "Net Operações"             , name: "NetOperacoes"               , jsonmap: "NetOperacoes"               , index: "NetOperacoes"               , width: 85, align: "right"}
                      , { label: "Patrimônio Liq. Tempo Real", name: "PatrimonioLiquidoTempoReal" , jsonmap: "PatrimonioLiquidoTempoReal" , index: "PatrimonioLiquidoTempoReal" , width: 120, align: "right"}
                      , { label: "PL Abertura BMF"           , name: "PLAberturaBMF"              , jsonmap: "PLAberturaBMF"              , index: "PLAberturaBMF"              , width: 85, align: "right"}
                      , { label: "PL Abertura Bovespa"       , name: "PLAberturaBovespa"          , jsonmap: "PLAberturaBovespa"          , index: "PLAberturaBovespa"          , width: 85, align: "right"}
                      , { label: "Saldo BMF"                 , name: "SaldoBMF"                   , jsonmap: "SaldoBMF"                   , index: "SaldoBMF"                   , width: 85, align: "right"}
                      , { label: "Saldo Conta Margem"        , name: "SaldoContaMargem"           , jsonmap: "SaldoContaMargem"           , index: "SaldoContaMargem"           , width: 85, align: "right"}
                      , { label: "Total C. C. Tempo Real"    , name: "TotalContaCorrenteTempoReal", jsonmap: "TotalContaCorrenteTempoReal", index: "TotalContaCorrenteTempoReal", width: 75, align: "right"}
                      , { label: "Total Garantias"           , name: "TotalGarantias"             , jsonmap: "TotalGarantias"             , index: "TotalGarantias"             , width: 75, align: "right"}
                      , { label: "Limite A Vista"            , name: "LimiteAVista"               , jsonmap: "LimiteAVista"               , index: "LimiteAVista"               , width: 75, align: "right"}
                      , { label: "Limite Disponível"         , name: "LimiteDisponivel"           , jsonmap: "LimiteDisponivel"           , index: "LimiteDisponivel"           , width: 75, align: "right"}
                      , { label: "Limite Opções"             , name: "LimiteOpcoes"               , jsonmap: "LimiteOpcoes"               , index: "LimiteOpcoes"               , width: 89, align: "right"}
                      , { label: "Limite Total"              , name: "LimiteTotal"                , jsonmap: "LimiteTotal"                , index: "LimiteTotal"                , width: 89, align: "right" }
                      , { label: "Prejuízo"                  , name: "Prejuizo"                   , jsonmap: "Prejuizo"                   , index: "Prejuizo"                   , width: 89, align: "center" }
                      , { label: "Volume Total Bmf"          , name: "VolumeTotalFinaceiroBmf"    , jsonmap: "VolumeTotalFinaceiroBmf"    , index: "VolumeTotalFinaceiroBmf"    , width: 110, align: "center" }
                      , { label: "Volume Total Bovespa"      , name: "VolumeTotalFinaceiroBov"    , jsonmap: "VolumeTotalFinaceiroBov"    , index: "VolumeTotalFinaceiroBov"    , width: 110, align: "center" }
                    ]
      , height: '600'
      , width: 1240
      //, pager           : "#pnlBusca_MonitoramentoRisco_Resultados_Pager"
      , rowNum          : 0
      , sortname        : 'Codigo'
      , sortorder       : 'asc'
      , sortable        : true
      , viewrecords     : true
      , gridview        : false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid         : false
      , multiselect     : true
      , jsonReader: {
          root: "Itens"
                       , page       : "PaginaAtual"
                       , total      : "TotalDePaginas"
                       , records    : "TotalDeItens"
                       , cell       : ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id         : "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_MonitoramentoRisco_ItemDataBound
      , onSelectRow: GradIntra_MonitoramentoRisco_SelectRow
      , beforeSelectRow: GradIntra_MonitoramentoRisco_beforeSelectRow
      , loadComplete: GradIntra_MonitoramentoRisco_LoadComplete
      , gridComplete: GradIntra_MonitoramentoRisco_GridComplete
//      , beforeSelectRow: GradIntra_Busca_OrdensSelectRow
//      , subGridRowExpanded: GradIntra_Busca_OrdensStopSubgrid
  }).jqGrid('hideCol', ['Id', 'CodigoBmf', 'SemaforoHide'])

  ; //
}

function GradIntra_MonitoramentoRisco_GridComplete()
{
    var lURL = "MonitoramentoRiscoGeral.aspx";

    var lObjetoDeParametros = { Acao: "BuscarVolumeOperacoes"    };

    GradIntra_CarregarJsonVerificandoErro(lURL
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_MonitoramentoRisco_GridComplete_Callback(pResposta); });
}

function GradIntra_MonitoramentoRisco_GridComplete_Callback(pResposta)
{
    if (!pResposta.TemErro) 
    {
        $("#div_Risco_MonitoramentoRisco_Resultados_Volume_Total_Bovespa")
            .html("R$ " + pResposta.ObjetoDeRetorno.VolumeTotalBovespa)
            .css({ fontWeight: 'Bold', fontStyle: 'italic', fontSize: '13px', color: 'black' });
        
        $("#div_Risco_MonitoramentoRisco_Resultados_Volume_Total_Bmf")
            .html("R$ " + pResposta.ObjetoDeRetorno.VolumeTotalBmf)
            .css({ fontWeight: 'Bold', fontStyle: 'italic',fontSize: '13px', color: 'black' });
        
    }
}

function GradIntra_MonitoramentoRisco_LoadComplete(data) 
{
    $("#cb_tblBusca_MonitoramentoRisco_Resultados").remove();
    jQuery(this).remove($("#cb_tblBusca_MonitoramentoRisco_Resultados"));
}

function GradIntra_MonitoramentoRisco_beforeSelectRow (rowid, e )
{
    e = false;
}
function GradIntra_MonitoramentoRisco_SelectRow(rowid, status, e) 
{
    var rowData = jQuery(this).getRowData(rowid);

    var ch = jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked');
    
    if (ch) 
    {
        jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked', false);
    }
    else 
    {
        jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked', true);
    }

    var lCodigo = rowData.Codigo;

    var lValorAtual = $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val();

    if (lValorAtual.indexOf("|" + lCodigo) ==-1)
    {
        $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val(lValorAtual + "|" + lCodigo);
    } 
    else 
    {
        lValorAtual.replace("|" + lCodigo,"");
        $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val(lValorAtual);
    }
}

function GradIntra_MonitoramentoRisco_DeSelectRow() 
{
    var lCodigo     = $(this).closest("tr").find("td").eq(4).html();

    var lID         = $(this).closest("tr").attr("id");

    var lValorAtual = $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val();

    if ($(this).is(":checked")) 
    {
        if (lValorAtual.indexOf("|" + lCodigo) == -1) 
        {
            $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val(lValorAtual + "|" + lCodigo);
        } 
    }
    else 
    {
        var lNovoValor = lValorAtual.replace("|" + lCodigo, "");

        $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val(lNovoValor);
    }

    jQuery("#tblBusca_MonitoramentoRisco_Resultados").setSelection(lID, false);
}

function GradIntra_MonitoramentoRisco_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados tr[id=" + rowid + "]");

    //lRow.addClass("Risco_Geral_"+rowelem.Semaforo.toUpperCase());

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    var lCheck = jQuery(this).find('#' + rowid + ' input[type=checkbox]');

    lCheck.click(GradIntra_MonitoramentoRisco_DeSelectRow);

    var lValorAtual = $("#hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas").val();

    if (lValorAtual.indexOf(rowelem.Codigo) > -1) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados").setSelection(rowid, true);

        jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked', true);
    }

    if (rowelem.Semaforo == "VERDE" || rowelem.Semaforo == "SEMINFORMACAO" ) 
    {
        lRow.children("td").eq(3).html("<span class=\"SemaforoVerde\"> </span>"); 
    }
    else if (rowelem.Semaforo == "AMARELO") 
    {
        lRow.children("td").eq(3).html("<span class=\"SemaforoAmarelo\"> </span>"); 
    }
    else if ( rowelem.Semaforo == "VERMELHO") 
    {
        lRow.children("td").eq(3).html("<span class=\"SemaforoVermelho\"> </span>"); 
    }

    if (rowelem.ContaCorrenteAbertura.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.CustodiaAbertura.indexOf("-") > -1)
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.LucroPrejuizoBMF.indexOf("-") > -1) 
    {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.LucroPrejuizoBOVESPA.indexOf("-") > -1) 
    {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.LucroPrejuizoTOTAL.indexOf("-") > -1) 
    {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.NetOperacoes.indexOf("-") > -1) 
    {
        lRow.children("td").eq(16).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PatrimonioLiquidoTempoReal.indexOf("-") > -1) 
    {
        lRow.children("td").eq(17).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PLAberturaBMF.indexOf("-") > -1) 
    {
        lRow.children("td").eq(18).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PLAberturaBovespa.indexOf("-") > -1) 
    {
        lRow.children("td").eq(19).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.SaldoBMF.indexOf("-") > -1)
    {
        lRow.children("td").eq(20).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.TotalContaCorrenteTempoReal.indexOf("-") > -1) 
    {
        lRow.children("td").eq(22).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.Prejuizo.indexOf("-") > -1) 
    {
        lRow.children("td").eq(28).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    //lRow.children("td").eq(0).remove(jQuery(this).find('#' + rowid + ' input[type=checkbox]'));
    lRow.children("td").eq(0)
        .html("<img alt=\"\" style=\"cursor: pointer\" onclick=\"return GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_click(this)\" src=\"../../../../Skin/Default/Img/Ico_Busca.png\" />");
}

var gFirstClickBuscarMonitoramentoRiscoDetalhamento = true;

function GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bovespa(pParametros) 
{
    $("#pnlResultado_MonitoramentoRisco").show();

    if (gFirstClickBuscarMonitoramentoRiscoDetalhamento != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_detalhes")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoDetalhamento = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_detalhes").jqGrid(
    {
        url      : "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarOperacoesBovespa"
      , datatype : "json"
      , mtype    : "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel : [
                        { label: "Id"                   , name: "Id"                 , jsonmap: "Instrumento"         , index: "Instrumento"       ,width: 29, align: "center", sortable: true }
                      , { label: "Cliente"              , name: "Cliente"            , jsonmap: "Cliente"             , index: "Cliente"           ,width: 40, align: "center", sortable: true }
                      , { label: "Instrumento"          , name: "Instrumento"        , jsonmap: "Instrumento"         , index: "Instrumento"       ,width: 85, align: "center", sortable: true }
                      
                      , { label: "Cotação"              , name: "Cotação"            , jsonmap: "Cotacao"             , index: "Cotacao"           ,width: 54, align: "center", sortable: true } 
                      , { label: "Fin. Abert."          , name: "FinanceiroAbertura" , jsonmap: "FinanceiroAbertura"  , index: "FinanceiroAbertura",width: 54, align: "center", sortable: true }
                      , { label: "Fin. Comp."           , name: "FinanceiroComprado" , jsonmap: "FinanceiroComprado"  , index: "FinanceiroComprado",width: 85, align: "center", sortable: true }
                      , { label: "Fin. Vend."           , name: "FinanceiroVendido"  , jsonmap: "FinanceiroVendido"   , index: "FinanceiroVendido" ,width: 85, align: "center", sortable: true }
                      , { label: "Preço Médio"          , name: "PrecoMedio"         , jsonmap: "PrecoMedio"          , index: "PrecoMedio"        , width: 85, align: "center", sortable: true }
                      , { label: "Lucro Prejuízo"       , name: "LucroPrejuizo"      , jsonmap: "LucroPrejuizo"       , index: "LucroPrejuizo"     ,width: 85, align: "center", sortable: true }
                      , { label: "Net Operação"         , name: "NetOperacao"        , jsonmap: "NetOperacao"         , index: "NetOperacao"       ,width: 85, align: "center", sortable: true }
                      , { label: "Preço Reversão"       , name: "PrecoReversao"      , jsonmap: "PrecoReversao"       , index: "PrecoReversao"     ,width: 85, align: "center", sortable: true }
                      , { label: "Qtde. Abert."         , name: "QtdeAbertura"       , jsonmap: "QtdeAber"            , index: "QtdeAber"          ,width: 85, align: "center", sortable: true }
                      , { label: "Qtde. Negociada"      , name: "QtdeAtual"          , jsonmap: "QtdeAtual"           , index: "QtdeAtual"         ,width: 85, align: "center", sortable: true }
                      , { label: "Qtde. Compra Exec."   , name: "QtdeComprada"       , jsonmap: "QtdeComprada"        , index: "QtdeComprada"      ,width: 85, align: "center", sortable: true }
                      , { label: "Qtde. Venda Exec."    , name: "QtdeVendida"        , jsonmap: "QtdeVendida"         , index: "QtdeVendida"       ,width: 85, align: "center", sortable: true }
                      , { label: "Qtde. Reversão"       , name: "QtdeReversao"       , jsonmap: "QtReversao"          , index: "QtReversao"        ,width: 85, align: "center", sortable: true }
                      , { label: "Tipo Mercado"         , name: "TipoMercado"        , jsonmap: "TipoMercado"         , index: "TipoMercado"       ,width: 85, align: "center", sortable: true }
                      , { label: "VL. Merc. Compra"     , name: "VLMercadoCompra"    , jsonmap: "VLMercadoCompra"     , index: "VLMercadoCompra"   ,width: 75, align: "center", sortable: true }
                      , { label: "VL. Merc. Venda"      , name: "VLMercadoVenda"     , jsonmap: "VLMercadoVenda"      , index: "VLMercadoVenda"    ,width: 75, align: "center", sortable: true }
                      , { label: "VL. Neg. Venda"       , name: "VLNegocioVenda"     , jsonmap: "VLNegocioVenda"      , index: "VLNegocioVenda"    ,width: 75, align: "center", sortable: true }
                      , { label: "Subtotal (Compra)"    , name: "SubtotalCompra"     , jsonmap: "SubtotalCompra"      , index: "SubtotalCompra"    ,width: 75, align: "center", sortable: true }
                      , { label: "Subtotal (Venda)"     , name: "SubtotalVenda"      , jsonmap: "SubtotalVenda"       , index: "SubtotalVenda"     ,width: 75, align: "center", sortable: true }
                      , { label: "Portas operadas"      , name: "Portas"             , jsonmap: "Portas"              , index: "Portas"            ,width: 75, align: "center", sortable: true }
                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_detalhes"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "asc"
      , sortable: true
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: 
      {
                         root        : "Itens"
                       , page        : "PaginaAtual"
                       , total       : "TotalDePaginas"
                       , records     : "TotalDeItens"
                       , cell        : ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id          : "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems : false
      }
      , afterInsertRow     : GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bovespa_ItemDataBound
      , subGridRowExpanded : GradIntra_MonitoramentoRisco_GridRiscoSubgridExpander
      , gridComplete       : GradIntra_MonitoramentoRisco_Atualizar_TotalLucroPrejuizoBovespa

  }).jqGrid('hideCol', "Id");

}

function GradIntra_MonitoramentoRisco_Atualizar_TotalLucroPrejuizoBovespa() 
{
    var lObjetoDeParametros = { 
        Acao: "BuscarTotalLucroPrejuizoBovespa",
        CodigoCliente   : gCodigoBovespaDetalhes
    };

    var lUrl = "MonitoramentoRiscoGeralDetalhamento.aspx";

    $("#tdTotalLucroPrejuizoBov").html(""); //--> Limpa o valor de lucro e prejuízo de bovespa

    GradIntra_CarregarJsonVerificandoErro(lUrl
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_MonitoramentoRisco_AtualizarTotalLucroPrejuizoBovespa_Callback(pResposta); });

}

function GradIntra_MonitoramentoRisco_AtualizarTotalLucroPrejuizoBovespa_Callback(pResposta) 
{
    if (!pResposta.TemErro) 
    {
        $("#tdTotalLucroPrejuizoBov").html(pResposta.ObjetoDeRetorno.TotalLucroPrejuizo);


        if (pResposta.ObjetoDeRetorno.TotalLucroPrejuizo.indexOf("-") > -1) 
        {
            $("#tdTotalLucroPrejuizoBov").css({ fontWeight: 'bold', fontSize: '14px', color: 'red' });
        }
        else
        {
            $("#tdTotalLucroPrejuizoBov").css({ fontWeight: 'bold', fontSize: '14px', color: 'black' });
        }

        //if ($("#tblBusca_MonitoramentoRisco_Resultados_detalhes tr").length == 0)
//        if (pResposta.ObjetoDeRetorno.PrecoMedio =="0,00")
//        {
//            $("#pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bovespa").hide();
//            $("#pnlMonitoramentoRiscoGeral_DetalhesRisco h4").hide();
//            $("#pnlMonitoramentoRiscoGeral_DetalhesRisco h4").hide();
//            $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Detalhes_Expander").hide();
//        }
//        else 
//        {
            $("#pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bovespa").show();
            $("#pnlMonitoramentoRiscoGeral_DetalhesRisco h4").show();
            $("#pnlMonitoramentoRiscoGeral_DetalhesRisco h4").show();
            $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Detalhes_Expander").show();
        //}
    }
}

var gTotalLucroPrejuizoBov = 0;

function GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bovespa_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.FinanceiroAbertura.indexOf("-") > -1) 
    {
        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.FinanceiroComprado.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.FinanceiroVendido.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.LucroPrejuizo.indexOf("-") > -1) 
    {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.NetOperacao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.PrecoReversao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdeAtual.indexOf("-") > -1) 
    {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdeComprada.indexOf("-") > -1) 
    {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdeVendida.indexOf("-") > -1) 
    {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtReversao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(16).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.VLMercadoCompra.indexOf("-") > -1) 
    {
        lRow.children("td").eq(18).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.VLMercadoVenda.indexOf("-") > -1) 
    {
        lRow.children("td").eq(19).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.VLNegocioVenda.indexOf("-") > -1) 
    {
        lRow.children("td").eq(20).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.SubtotalCompra.indexOf("-") > -1) 
    {
        lRow.children("td").eq(21).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.SubtotalVenda.indexOf("-") > -1) 
    {
        lRow.children("td").eq(22).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gFirstClickBuscarMonitoramentoRiscoDetalhamentoBmf = true;

function GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bmf(pParametros) 
{
    $("#pnlResultado_MonitoramentoRisco").show();

    if (gFirstClickBuscarMonitoramentoRiscoDetalhamentoBmf != true) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoDetalhamentoBmf = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarOperacoesBmf"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                          { label: "Id"                     , name: "Id"                    , jsonmap: "Id"                     , index: "Id"                       , width: 29, align: "center", sortable: true }
                        , { label: "Cliente"	            , name: "Cliente"	            , jsonmap: "Cliente"	            , index: "Cliente"	                , width: 60, align: "center", sortable: true }
                        , { label: "Contrato"               , name: "Contrato"              , jsonmap: "Contrato"               , index: "Contrato"                 , width: 54, align: "center", sortable: true }
                        , { label: "Diferencial Pontos"     , name: "DiferencialPontos"     , jsonmap: "DiferencialPontos"      , index: "DiferencialPontos"        , width: 85, align: "center", sortable: true }
                        , { label: "Fator Multiplicador"    , name: "FatorMultiplicador"    , jsonmap: "FatorMultiplicador"     , index: "FatorMultiplicador"       , width: 105, align: "center", sortable: true }
                        , { label: "Lucro Prejuízo Contrato", name: "LucroPrejuizoContrato" , jsonmap: "LucroPrejuizoContrato"  , index: "LucroPrejuizoContrato"    , width: 105, align: "center", sortable: true }
                        , { label: "Cotação"	            , name: "PrecoContatoMercado"   , jsonmap: "PrecoContatoMercado"    , index: "PrecoContatoMercado"      , width: 105, align: "center", sortable: true }
                        , { label: "Quantidade Contrato"    , name: "QuantidadeContato"     , jsonmap: "QuantidadeContato"      , index: "QuantidadeContato"        , width: 85, align: "center", sortable: true }
                        , { label: "Preço Médio"            , name: "PrecoMedio"            , jsonmap: "PrecoMedio"             , index: "PrecoMedio"               , width: 85, align: "center", sortable: true }
                        , { label: "Qtde. Negócios"         , name: "Count"                 , jsonmap: "Count"                  , index: "Count"                    , width: 85, align: "center", sortable: true }
                        , { label: "Subtotal (Compra)"      , name: "SubtotalCompra"        , jsonmap: "SubtotalCompra"         , index: "SubtotalCompra"           , width: 85, align: "center", sortable: true }
                        , { label: "Subtotal (Venda)"       , name: "SubtotalVenda"         , jsonmap: "SubtotalVenda"          , index: "SubtotalVenda"            , width: 85, align: "center", sortable: true }
                        , { label: "Série"                  , name: "CodigoSerie"           , jsonmap: "CodigoSerie"            , index: "CodigoSerie"              , width: 85, align: "center", sortable: true }
                        //, { label: "Saldo anterior"         , name: "Saldo Anterior"        , jsonmap: "SaldoAnterior"          , index: "SaldoAnterior"            , width: 85, align: "center", sortable: false }
                        //, { label: "Saldo Atual"            , name: "SaldoAtual"            , jsonmap: "SaldoAtual"             , index: "SaldoAtual"               , width: 85, align: "center", sortable: false }


                      ]
      , height: 'auto'
      , width: 1128
        //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_detalhes"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "asc"
      , sortable: true
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "2"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bmf_ItemDataBound
      , subGridRowExpanded: GradIntra_MonitoramentoRisco_SubGrid_Risco_Detalhes_Bmf_Expander
      , gridComplete: GradIntra_MonitoramentoRisco_AtualizarTotalLucroPrejuizoBmf
  }).jqGrid('hideCol', "Id");
}

function GradIntra_MonitoramentoRisco_SubGrid_Risco_Detalhes_Bmf_Expander(subgrid_id, row_id) 
{
    var subgrid_table_id;

    var lInstrumento = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf").find("#" + row_id).find("td").eq(3).html(); ;

    var lCliente = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf").find("#" + row_id).find("td").eq(2).html();

    subgrid_table_id = subgrid_id + "_table";

    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

    jQuery("#" + subgrid_table_id)
        .jqGrid({
            url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarDetalhamentoOperacoeBmf"
        , hoverrows: false
        , datatype: "json"
        , mtype: "POST"
        , postData: { 'Instrumento': lInstrumento, 'CodigoCliente': lCliente }
        , shrinkToFit: false
        , colNames: ['Id', 'Cliente', 'Contrato', 'Data Operação', 'Diferencial Pontos', 'Fator Multiplicador', 'Lucro Prejuízo Contrato', 'Preço Aquisição Contrato', 'Preço Contato Mercado', 'Quantidade Contato','Sentido','Contraparte']
        , colModel: [
                         { label: "Id",                         name: "Id",                             jsonmap: "Id",                      index: "Id",                    width: 29, align: "center", sortable: false }
                       , { label: "Cliente",                    name: "Cliente",                        jsonmap: "Cliente",                 index: "Cliente",               width: 60, align: "center", sortable: false }
                       , { label: "Contrato",                   name: "Contrato",                       jsonmap: "Contrato",                index: "Contrato",              width: 54, align: "center", sortable: false }
                       , { label: "Data Operação",              name: "DataOperacao",                   jsonmap: "DataOperacao",            index: "DataOperacao",          width: 95, align: "center", sortable: false }
                       , { label: "Diferencial Pontos",         name: "DiferencialPontosDetalhe",       jsonmap: "DiferencialPontos",       index: "DiferencialPontos",     width: 85, align: "center", sortable: false }
                       , { label: "Fator Multiplicador",        name: "FatorMultiplicador",             jsonmap: "FatorMultiplicador",      index: "FatorMultiplicador",    width: 105, align: "center", sortable: false }
                       , { label: "Lucro Prejuizo Contrato",    name: "LucroPrejuizoContratoDetalhe",   jsonmap: "LucroPrejuizoContrato",   index: "LucroPrejuizoContrato", width: 105, align: "center", sortable: false }
                       , { label: "Preço Aquisição Contrato",   name: "PrecoAquisicaoContratoDetalhe",  jsonmap: "PrecoAquisicaoContrato",  index: "PrecoAquisicaoContrato",width: 110, align: "center", sortable: false }
                       , { label: "Preço Contrato Mercado",      name: "PrecoContatoMercadoDetalhe",     jsonmap: "PrecoContatoMercado",     index: "PrecoContatoMercado",   width: 105, align: "center", sortable: false }
                       , { label: "Quantidade Contrato",         name: "QuantidadeContatoDetalhe",       jsonmap: "QuantidadeContato",       index: "QuantidadeContato",     width: 85, align: "center", sortable: false }
                       , { label: "Sentido",                    name: "Sentido",                        jsonmap: "Sentido",                 index: "Sentido",               width: 85, align: "center", sortable: false }
                       , { label: "ContraParte",                name: "Contraparte",                    jsonmap: "Contraparte",             index: "Contraparte",           width: 85, align: "center", sortable: false }
          
          ]
        , jsonReader: {
            root: "Itens"
            , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id: "0"              //primeira propriedade do elemento de linha é o Id
            , repeatitems: false
        }
        , width: 1000
        , height: "100%"
        , rowNum: 'auto'
        , sortname: 'num'
        , sortorder: 'asc'
        , afterInsertRow: GradIntra_MonitoramentoRisco_SubGrid_Risco_Detalhes_Bmf_Expander_ItemDataBound
    }).jqGrid("hideCol", "Id");
}

function GradIntra_MonitoramentoRisco_SubGrid_Risco_Detalhes_Bmf_Expander_ItemDataBound(rowid, rowdata, rowelem) {
                   //tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf_1_table
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf_" + rowelem.Contrato + " tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.Sentido == "C") 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'bold', fontSize: '12px', color: 'blue' });
    } 
    else 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' });
    }
}

//function GradIntra_MonitoramentoRisco_SubGrid_Risco_Detalhes_Bmf_Expander_ItemDataBound(rowid, rowdata, rowelem) 
//{
//    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf tr[id=" + rowid + "]");

//    lRow.css("height", 30);

//    lRow.css("background-color", 'white');
//}

function GradIntra_MonitoramentoRisco_AtualizarTotalLucroPrejuizoBmf() 
{
    var lObjetoDeParametros = { 
      Acao              : "BuscarTotalLucroPrejuizoBmf"
    , CodigoClienteBmf  : gCodigoBMFDetalhes
    };

    var lUrl = "MonitoramentoRiscoGeralDetalhamento.aspx";

    $("#tdTotalLucroPrejuizoBmf").html("");//--> limpa os valores de lucro e prejuízo de bmf

    GradIntra_CarregarJsonVerificandoErro(lUrl
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_MonitoramentoRisco_AtualizarTotalLucroPrejuizoBmf_Callback(pResposta); });
}

function GradIntra_MonitoramentoRisco_AtualizarTotalLucroPrejuizoBmf_Callback(pResposta) 
{
    if (!pResposta.TemErro) 
    {
        $("#tdTotalLucroPrejuizoBmf").html(pResposta.ObjetoDeRetorno.TotalLucroPrejuizo);

        if (pResposta.ObjetoDeRetorno.TotalLucroPrejuizo.indexOf("-") > -1) 
        {
            $("#tdTotalLucroPrejuizoBmf").css({ fontWeight: 'bold', fontSize: '14px', color: 'red' });
        } 
        else
        {
            $("#tdTotalLucroPrejuizoBmf").css({fontWeight: 'bold', fontSize: '14px', color: 'black'});
        }

//        if (pResposta.ObjetoDeRetorno.PrecoMedio == "0,00")
//        {
//            $("#pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bmf").hide();
//        }
//        else 
//        {
            $("#pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bmf").show();
//        }
//        if ($("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf tr").length == 0)
//        {
//            $("#pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bmf").hide();
//        }

        //if (pResposta.ObjetoDeRetorno.QuantidadeContratos == "0") { $("#pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bmf").hide(); }
    }
    else 
    {
        //alert();
    }
}

var gTotalLucroPrejuizoBmf = 0;

function GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bmf_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

//    if (rowid == 0) 
//    {
//        gTotalLucroPrejuizoBmf = 0;
//    }

//    gTotalLucroPrejuizoBmf += parseFloat(rowelem.LucroPrejuizoContrato.replace(",", "."));

//    var lTotalLucroPrejuizo = gTotalLucroPrejuizoBmf.toString().replace(".",",");

//    $("#divTotalLucroPrejuizoBmf").html(lTotalLucroPrejuizo);

//    if (lTotalLucroPrejuizo.toString().indexOf("-") > -1) 
//    {
//        $("#divTotalLucroPrejuizoBmf").css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    //    }

//    var lQuantidade = parseFloat(lRow.children("td").eq(10).html().replace(",","."));

//    var lMoeda = lRow.children("td").eq(9).html().replace(".", "");

//    lMoeda = lMoeda.replace(",", ".");

//    var lPreco = parseFloat(lMoeda);

//    var lPrecoMedio = lPreco / lQuantidade;

//    //Setando o preco Médio
//    lRow.children("td").eq(9).html(lPrecoMedio.toFixed(2).replace(".",","));

//    var lPrecoContratoMercado = parseFloat(lRow.children("td").eq(7).html().replace(",", "."));

//    var lDiferencial = lPrecoContratoMercado - lPrecoMedio;

//    //Setando o Diferencial
//    lRow.children("td").eq(4).html(lDiferencial.toFixed(2).replace(".", ","));

    if (rowelem.DiferencialPontos.indexOf("-") > -1) 
    {
        lRow.children("td").eq(4).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.LucroPrejuizoContrato.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.SubtotalCompra.indexOf("-") > -1)
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.SubtotalVenda.indexOf("-") > -1)
    {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

function GradIntra_MonitoramentoRisco_GridRiscoSubgridExpander(subgrid_id, row_id)
{
    var subgrid_table_id;

    var lInstrumento = row_id;

    var lCliente = $("#" + row_id).find("td").eq(2).html();

    subgrid_table_id = subgrid_id + "_table";

    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

    jQuery("#" + subgrid_table_id)
        .jqGrid({
            url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarDetalhamentoOperacoesBovespa"
        , hoverrows: false
        , datatype : "json"
        , mtype: "POST"
        , postData: { 'Instrumento': lInstrumento, 'CodigoCliente': lCliente }
        , shrinkToFit: false
        , colNames : ['Id','Instrumento', 'Lucro Prejuízo', 'Preço Mercado', 'Preço Negócio', 'Quantidade', 'Sentido', 'Total Mercado', 'Total Negócio', 'Porta']
        , colModel : [
            { label: "Id"             , name: "Id"              , index: "Id"          ,  width: 55, align: "center" },
            { label: "Instrumento"    , name: "Instrumento"     , index: "Instrumento",   width: 55, align: "center" },
            { label: "Lucro Prejuizo" , name: "LucroPrejuiso"   , index: "LucroPrejuiso", width: 60, align: "center" },
            { label: "Preço Mercado"  , name: "PrecoMercado"    , index: "PrecoMercado",  width: 60, align: "center" },
            { label: "Preço Negócio"  , name: "PrecoNegocio"    , index: "PrecoNegocio",  width: 60, align: "center" },
            { label: "Quantidade"     , name: "Quantidade"      , index: "Quantidade",    width: 60, align: "center" },
            { label: "Sentido"        , name: "Sentido"         , index: "Sentido",       width: 60, align: "center" },
            { label: "Total Mercado " , name: "TotalMercado"    , index: "TotalMercado",  width: 60, align: "center" },
            { label: "Total Negócio " , name: "TotalNegocio"    , index: "TotalNegocio", width: 60, align: "center" },
            { label: "Porta"          , name: "Porta"           , index: "Porta"        , width: 160, align: "center" },
          ]
        , jsonReader: {
              root: "Itens"
            , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id: "0"              //primeira propriedade do elemento de linha é o Id
            , repeatitems: false
        }
        , width     : 700
        , height    : "100%"
        , rowNum    : 'auto'
        , sortname  : 'num'
        , sortorder: 'asc'
        , afterInsertRow: GradIntra_MonitoramentoRisco_GridRiscoSubgridExpander_Bovespa_ItemDataBound
      }).jqGrid('hideCol', "Id") ;
  }

  function GradIntra_MonitoramentoRisco_GridRiscoSubgridExpander_Bovespa_ItemDataBound(rowid, rowdata, rowelem)
  {
      var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_detalhes_" + rowelem.Instrumento + "_table tr[id=" + rowid + "]");

//      lRow.css("height", 30);

      //      lRow.css("background-color", 'white');

      if (rowelem.Sentido == "C") 
      {
          lRow.children("td").eq(6).css({ fontWeight: 'bold', fontSize: '12px', color: 'blue' })
      } else {
          lRow.children("td").eq(6).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
      }
  }

  var gFirstClickBuscarMonitoramentoRiscoCustodiaAvista = true;

  function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Avista(pParametros) 
  {
      $("#pnlMonitoramentoRiscoGeral_Custodia_Avista").show();

      if (gFirstClickBuscarMonitoramentoRiscoCustodiaAvista != true) {
          $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Avista")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

          return;
      }

      gFirstClickBuscarMonitoramentoRiscoCustodiaAvista = false;

      $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Avista").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows  : false
      , postData   : pParametros
      , autowidth  : false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"               , name: "Id"                , jsonmap: "Id"                 , index: "Id"                   , width: 29, align: "center", sortable: true }  
                      , { label: "Tipo Mercado"     , name: "Tipo Mercado"      , jsonmap: "TipoMercado"        , index: "TipoMercado"          , width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio"     , name: "Codigo Negócio"    , jsonmap: "CodigoNegocio"      , index: "CodigoNegocio"        , width: 75, align: "left", sortable: true }
                      , { label: "Empresa"          , name: "Empresa"           , jsonmap: "Empresa"            , index: "Empresa"              , width: 85, align: "left", sortable: true }
                      , { label: "Cód. Carteira"    , name: "CodigoCarteira"    , jsonmap: "CodigoCarteira"     , index: "CodigoCarteira"       , width: 60, align: "left", sortable: true }
                      , { label: "Carteira"         , name: "Carteira"          , jsonmap: "Carteira"           , index: "Carteira"             , width: 100, align: "left", sortable: true }
                      , { label: "Qtd. Abert."      , name: "Qtd.Abertura"      , jsonmap: "QtdAbertura"        , index: "QtdAbertura"          , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual"       , name: "Qtd.Atual"         , jsonmap: "QtdAtual"           , index: "QtdAtual"             , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D0"          , name: "QtdLiquid"         , jsonmap: "QtdLiquid"          , index: "QtdLiquid"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D1"          , name: "Qtd.D1"            , jsonmap: "QtdD1"              , index: "QtdD1"                , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D2"          , name: "Qtd.D2"            , jsonmap: "QtdD2"              , index: "QtdD2"                , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D3"          , name: "Qtd.D3"            , jsonmap: "QtdD3"              , index: "QtdD3"                , width: 55, align: "center", sortable: true }
                      , { label: "OFC "             , name: "Qtd.Compra"        , jsonmap: "QtdCompra"          , index: "QtdCompra"            , width: 65, align: "center", sortable: true }
                      , { label: "OFV"              , name: "Qtd.Venda"         , jsonmap: "QtdVenda"           , index: "QtdVenda"             , width: 60, align: "center", sortable: true }
                      , { label: "Vl. Fecham."      , name: "Vl. Fecham."       , jsonmap: "ValorDeFechamento"  , index: "ValorDeFechamento"    , width: 55, align: "center", sortable: true }
                      , { label: "Cotação"          , name: "Cotação"           , jsonmap: "Cotacao"            , index: "Cotacao"              , width: 55, align: "center", sortable: true }
                      , { label: "Variação"         , name: "Variação"          , jsonmap: "Variacao"           , index: "Variacao"             , width: 70, align: "center", sortable: true }
                      , { label: "Resultado"        , name: "Resultado"         , jsonmap: "Resultado"          , index: "Resultado"            , width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Avista"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Avista_ItemDataBound
      , gridComplete  : function ()
                        {
                            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Avista tr").length == 0)
                            {$("#pnlMonitoramentoRiscoGeral_Custodia_Avista").hide(); }else{$("#pnlMonitoramentoRiscoGeral_Custodia_Avista").show();}
                        }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Avista_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Avista tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
    } else
    {
        lRow.children("td").eq(7).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QtdLiquid.indexOf("-") > -1) 
    {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD1.indexOf("-") > -1) 
    {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD2.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD3.indexOf("-") > -1) 
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) 
    {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) 
    {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

//    if (rowelem.QtdDATotal.indexOf("-") > -1) 
//    {
//        lRow.children("td").eq(12).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
//    }
//    else 
//    {
//        lRow.children("td").eq(12).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
//    }

    if (rowelem.ValorDeFechamento !=null && rowelem.ValorDeFechamento.indexOf("-") > -1) 
    {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao != null && rowelem.Variacao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(16).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) 
    {
        lRow.children("td").eq(17).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gFirstClickBuscarMonitoramentoRiscoCustodiaOpcoes = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Opcoes(pParametros) 
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_Opcoes").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaOpcoes != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Opcoes")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaOpcoes = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Opcoes").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"               , name: "Id"                , jsonmap: "Id"                 , index: "Id"                  , width: 29, align: "center", sortable: true }
                      , { label: "Tipo Mercado"     , name: "Tipo Mercado"      , jsonmap: "TipoMercado"        , index: "TipoMercado"         , width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio"     , name: "Codigo Negócio"    , jsonmap: "CodigoNegocio"      , index: "CodigoNegocio"       , width: 75, align: "left",   sortable: true } 
                      , { label: "Empresa"          , name: "Empresa"           , jsonmap: "Empresa"            , index: "Empresa"             , width: 85, align: "left",   sortable: true }
                      , { label: "Cód. Carteira"    , name: "CodigoCarteira"    , jsonmap: "CodigoCarteira"     , index: "CodigoCarteira"      , width: 60, align: "left", sortable: true }
                      , { label: "Carteira"         , name: "Carteira"          , jsonmap: "Carteira"           , index: "Carteira"            , width: 100, align: "left",  sortable: true }
                      , { label: "Qtd. Abert."      , name: "Qtd.Abertura"      , jsonmap: "QtdAbertura"        , index: "QtdAbertura"         , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual"       , name: "Qtd.Atual"         , jsonmap: "QtdAtual"           , index: "QtdAtual"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D0"          , name: "QtdLiquid"         , jsonmap: "QtdLiquid"          , index: "QtdLiquid"           , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D1"          , name: "Qtd.D1"            , jsonmap: "QtdD1"              , index: "QtdD1"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D2"          , name: "Qtd.D2"            , jsonmap: "QtdD2"              , index: "QtdD2"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D3"          , name: "Qtd.D3"            , jsonmap: "QtdD3"              , index: "QtdD3"               , width: 55, align: "center", sortable: true }
                      , { label: "OFC"              , name: "Qtd.Compra"        , jsonmap: "QtdCompra"          , index: "QtdCompra"           , width: 55, align: "center", sortable: true }
                      , { label: "OFV"              , name: "Qtd.Venda"         , jsonmap: "QtdVenda"           , index: "QtdVenda"            , width: 55, align: "center", sortable: true }
                      , { label: "Vl. Fecham."      , name: "Vl. Fecham."       , jsonmap: "ValorDeFechamento"  , index: "ValorDeFechamento"   , width: 55, align: "center", sortable: true }
                      , { label: "Cotação"          , name: "Cotação"           , jsonmap: "Cotacao"            , index: "Cotacao"             , width: 55, align: "center", sortable: true }
                      , { label: "Variação"         , name: "Variação"          , jsonmap: "Variacao"           , index: "Variacao"            , width: 70, align: "center", sortable: true }
                      , { label: "Resultado"        , name: "Resultado"         , jsonmap: "Resultado"          , index: "Resultado"           , width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Opcoes"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Opcoes_ItemDataBound
      , gridComplete : function () 
                       {
                            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Opcoes tr").length == 0)
                            { $("#pnlMonitoramentoRiscoGeral_Custodia_Opcoes").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Opcoes").show(); }
                       }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Opcoes_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Opcoes tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) {
        lRow.children("td").eq(7).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(7).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QtdLiquid.indexOf("-") > -1) 
    {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD1.indexOf("-") > -1) {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD2.indexOf("-") > -1) {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD3.indexOf("-") > -1) {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.ValorDeFechamento.indexOf("-") > -1) {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao.indexOf("-") > -1) {
        lRow.children("td").eq(16).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) {
        lRow.children("td").eq(17).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }


}

var gFirstClickBuscarMonitoramentoRiscoCustodiaTermo = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Termo(pParametros) {
    $("#pnlMonitoramentoRiscoGeral_Custodia_Termo").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaTermo != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Termo")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaTermo = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Termo").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"                ,name: "Id"                 , jsonmap: "Id"                 , index: "Id"                  , width: 29, align: "center", sortable: true }
                      , { label: "Tipo Mercado"      ,name: "Tipo Mercado"       , jsonmap: "TipoMercado"        , index: "TipoMercado"         , width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio"      ,name: "Codigo Negócio"     , jsonmap: "CodigoNegocio"      , index: "CodigoNegocio"       , width: 75, align: "left",   sortable: true }
                      , { label: "Empresa"           ,name: "Empresa"            , jsonmap: "Empresa"            , index: "Empresa"             , width: 85, align: "left",   sortable: true } 
                      , { label: "Carteira"          ,name: "Carteira"           , jsonmap: "Carteira"           , index: "Carteira"            , width: 200, align: "left",  sortable: true }
                      , { label: "Qtd. Abert."       ,name: "Qtd.Abertura"       , jsonmap: "QtdAbertura"        , index: "QtdAbertura"         , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Compra"       ,name: "Qtd.Compra"         , jsonmap: "QtdCompra"          , index: "QtdCompra"           , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Venda"        ,name: "Qtd.Venda"          , jsonmap: "QtdVenda"           , index: "QtdVenda"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual"        ,name: "Qtd.Atual"          , jsonmap: "QtdAtual"           , index: "QtdAtual"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D1"           ,name: "Qtd.D1"             , jsonmap: "QtdD1"              , index: "QtdD1"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D2"           ,name: "Qtd.D2"             , jsonmap: "QtdD2"              , index: "QtdD2"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D3"           ,name: "Qtd.D3"             , jsonmap: "QtdD3"              , index: "QtdD3"               , width: 55, align: "center", sortable: true }
                      , { label: "Vl. Fecham."       ,name: "Vl. Fecham."        , jsonmap: "ValorDeFechamento"  , index: "ValorDeFechamento"   , width: 55, align: "center", sortable: true }
                      , { label: "Cotação"           ,name: "Cotação"            , jsonmap: "Cotacao"            , index: "Cotacao"             , width: 55, align: "center", sortable: true }
                      , { label: "Variação"          ,name: "Variação"           , jsonmap: "Variacao"           , index: "Variacao"            , width: 75, align: "center", sortable: true }
                      , { label: "Resultado"         ,name: "Resultado"          , jsonmap: "Resultado"          , index: "Resultado"           , width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Termo"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Termo_ItemDataBound
      , gridComplete : function () 
                       { 
                            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Termo tr").length == 0)
                                { $("#pnlMonitoramentoRiscoGeral_Custodia_Termo").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Termo").show(); }
                       }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Termo_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Termo tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) 
    {
        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) 
    {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD1.indexOf("-") > -1) 
    {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD2.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD3.indexOf("-") > -1) 
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.ValorDeFechamento.indexOf("-") > -1) 
    {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao.indexOf("-") > -1) 
    {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) 
    {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gFirstClickBuscarMonitoramentoRiscoCustodiaPosicaoBmf = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Posicao_Bmf(pParametros) 
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_Bmf").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaPosicaoBmf != true) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Posicao_Bmf")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaPosicaoBmf = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Posicao_Bmf").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaPosicao"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id",              name: "Id",             jsonmap: "Id",                  index: "Id",                width: 29, align: "center", sortable: true }
                      , { label: "Tipo Mercado",    name: "Tipo Mercado",   jsonmap: "TipoMercado",         index: "TipoMercado",       width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio",    name: "Codigo Negócio", jsonmap: "CodigoNegocio",       index: "CodigoNegocio",     width: 75, align: "left", sortable: true }
                      , { label: "Cod. Série",      name: "Codigo Série",   jsonmap: "CodigoSerie",         index: "CodigoSerie",       width: 75, align: "left", sortable: true }
                      , { label: "Empresa",         name: "Empresa",        jsonmap: "Empresa",             index: "Empresa",           width: 85, align: "left", sortable: true }
                      , { label: "Carteira",        name: "Carteira",       jsonmap: "Carteira",            index: "Carteira",          width: 200, align: "left", sortable: true }
                      , { label: "Qtd. Abert.",     name: "Qtd.Abertura",   jsonmap: "QtdAbertura",         index: "QtdAbertura",       width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Compra",     name: "Qtd.Compra",     jsonmap: "QtdCompra",           index: "QtdCompra",         width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Venda",      name: "Qtd.Venda",      jsonmap: "QtdVenda",            index: "QtdVenda",          width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual",      name: "Qtd.Atual",      jsonmap: "QtdAtual",            index: "QtdAtual",          width: 55, align: "center", sortable: true }
                      , { label: "PU",              name: "ValorPU",        jsonmap: "ValorPU",             index: "ValorPU",           width: 55, align: "center", sortable: true }
                      , { label: "Ajuste",          name: "Vl. Fecham.",    jsonmap: "ValorDeFechamento",   index: "ValorDeFechamento", width: 55, align: "center", sortable: true }
                      , { label: "Cotação",         name: "Cotação",        jsonmap: "Cotacao",             index: "Cotacao",           width: 55, align: "center", sortable: true }
                      , { label: "Variação",        name: "Variação",       jsonmap: "Variacao",            index: "Variacao",          width: 75, align: "center", sortable: true }
                      , { label: "Resultado",       name: "Resultado",      jsonmap: "Resultado",           index: "Resultado",         width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
        //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Bmf"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Posicao_Bmf_ItemDataBound
      , gridComplete: function () {
          if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Posicao_Bmf tr").length != 0)
          { $("#pnlMonitoramentoRiscoGeral_Custodia_Posicao_Bmf").show(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Posicao_Bmf").hide(); }
      }
    }).jqGrid('hideCol', "Id" ).jqGrid('hideCol','Resultado');
}
function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Posicao_Bmf_ItemDataBound(rowid, rowdata, rowelem)
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Posicao_Bmf tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) {
        lRow.children("td").eq(9).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(9).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

//    if (rowelem.QtdD1.indexOf("-") > -1) {
//        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }

//    if (rowelem.QtdD2.indexOf("-") > -1) {
//        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }

//    if (rowelem.QtdD3.indexOf("-") > -1) {
//        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }

    if (rowelem.ValorDeFechamento.indexOf("-") > -1) {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao.indexOf("-") > -1) {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

}

var gFirstClickBuscarMonitoramentoRiscoCustodiaBmf = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Bmf(pParametros) 
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_Bmf").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaBmf != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Bmf")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaBmf = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Bmf").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"                ,name: "Id"                 , jsonmap: "Id"                 , index: "Id"                  , width: 29, align: "center", sortable: true }
                      , { label: "Tipo Mercado"      ,name: "Tipo Mercado"       , jsonmap: "TipoMercado"        , index: "TipoMercado"         , width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio"      ,name: "Codigo Negócio"     , jsonmap: "CodigoNegocio"      , index: "CodigoNegocio"       , width: 75, align: "left",   sortable: true }
                      , { label: "Cod. Série"        ,name: "Codigo Série"       , jsonmap: "CodigoSerie"        , index: "CodigoSerie"         , width: 75, align: "left", sortable: true } 
                      , { label: "Empresa"           ,name: "Empresa"            , jsonmap: "Empresa"            , index: "Empresa"             , width: 85, align: "left",   sortable: true } 
                      , { label: "Carteira"          ,name: "Carteira"           , jsonmap: "Carteira"           , index: "Carteira"            , width: 200, align: "left",  sortable: true }
                      , { label: "Qtd. Abert."       ,name: "Qtd.Abertura"       , jsonmap: "QtdAtual"           , index: "QtdAbertura"         , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Compra"       ,name: "Qtd.Compra"         , jsonmap: "QtdCompra"          , index: "QtdCompra"           , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Venda"        ,name: "Qtd.Venda"          , jsonmap: "QtdVenda"           , index: "QtdVenda"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual"        ,name: "Qtd.Atual"          , jsonmap: "QtdAtual"           , index: "QtdAtual"            , width: 55, align: "center", sortable: true }
                      , { label: "PU"                ,name: "ValorPU"            , jsonmap: "ValorPU"            , index: "ValorPU"             , width: 55, align: "center", sortable: true }
                      , { label: "Ajuste"            ,name: "Vl. Fecham."        , jsonmap: "ValorDeFechamento"  , index: "ValorDeFechamento"   , width: 55, align: "center", sortable: true }
                      , { label: "Cotação"           ,name: "Cotação"            , jsonmap: "Cotacao"            , index: "Cotacao"             , width: 55, align: "center", sortable: true }
                      , { label: "Variação"          ,name: "Variação"           , jsonmap: "Variacao"           , index: "Variacao"            , width: 75, align: "center", sortable: true }
                      , { label: "Resultado"         ,name: "Resultado"          , jsonmap: "Resultado"          , index: "Resultado"           , width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Bmf"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Bmf_ItemDataBound
      , gridComplete : function ()
      {
        if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Bmf tr").length == 0)
        { $("#pnlMonitoramentoRiscoGeral_Custodia_Bmf").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Bmf").show(); }
      }
    })  .jqGrid('hideCol' , "Id")
        .jqGrid('hideCol'   ,"Qtd.Atual")
        .jqGrid('hideCol'   ,"Qtd.Compra")
        .jqGrid('hideCol'   ,"Qtd.Venda");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Bmf_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Bmf tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) {
        lRow.children("td").eq(9).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(9).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

//    if (rowelem.QtdD1.indexOf("-") > -1) {
//        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }

//    if (rowelem.QtdD2.indexOf("-") > -1) {
//        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }

//    if (rowelem.QtdD3.indexOf("-") > -1) {
//        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }
    if (rowelem.ValorDeFechamento.indexOf("-") > -1) {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    if (rowelem.ValorDeFechamento.indexOf("-") > -1) {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao.indexOf("-") > -1) {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

}

var gFirstClickBuscarMonitoramentoRiscoCustodiaTesouro = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Tesouro(pParametros)
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_Tesouro").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaTesouro != true) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Tesouro")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaTesouro = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Tesouro").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"                ,name: "Id"                 , jsonmap: "Id"                 , index: "Id"                  , width: 29, align: "center", sortable: true }
                      , { label: "Tipo Mercado"      ,name: "Tipo Mercado"       , jsonmap: "TipoMercado"        , index: "TipoMercado"         , width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio"      ,name: "Codigo Negócio"     , jsonmap: "CodigoNegocio"      , index: "CodigoNegocio"       , width: 75, align: "left",   sortable: true }
                      , { label: "Empresa"           ,name: "Empresa"            , jsonmap: "Empresa"            , index: "Empresa"             , width: 85, align: "left",   sortable: true }
                      , { label: "Carteira"          ,name: "Carteira"           , jsonmap: "Carteira"           , index: "Carteira"            , width: 200, align: "left",  sortable: true }
                      , { label: "Qtd. Abert."       ,name: "Qtd.Abertura"       , jsonmap: "QtdAbertura"        , index: "QtdAbertura"         , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Compra"       ,name: "Qtd.Compra"         , jsonmap: "QtdCompra"          , index: "QtdCompra"           , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Venda"        ,name: "Qtd.Venda"          , jsonmap: "QtdVenda"           , index: "QtdVenda"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual"        ,name: "Qtd.Atual"          , jsonmap: "QtdAtual"           , index: "QtdAtual"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D1"           ,name: "Qtd.D1"             , jsonmap: "QtdD1"              , index: "QtdD1"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D2"           ,name: "Qtd.D2"             , jsonmap: "QtdD2"              , index: "QtdD2"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D3"           ,name: "Qtd.D3"             , jsonmap: "QtdD3"              , index: "QtdD3"               , width: 55, align: "center", sortable: true }
                      , { label: "Vl. Fecham."       ,name: "Vl. Fecham."        , jsonmap: "ValorDeFechamento"  , index: "ValorDeFechamento"   , width: 55, align: "center", sortable: true }
                      , { label: "Cotação"           ,name: "Cotação"            , jsonmap: "Cotacao"            , index: "Cotacao"             , width: 55, align: "center", sortable: true }
                      , { label: "Variação"          ,name: "Variação"           , jsonmap: "Variacao"           , index: "Variacao"            , width: 75, align: "center", sortable: true }
                      , { label: "Resultado"         ,name: "Resultado"          , jsonmap: "Resultado"          , index: "Resultado"           , width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Tesouro_ItemDataBound
        , gridComplete : function () 
        {
            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Tesouro tr").length == 0 )
            { $("#pnlMonitoramentoRiscoGeral_Custodia_Tesouro").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Tesouro").show(); }
        }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Tesouro_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Tesouro tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) {
        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) {
        lRow.children("td").eq(8).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
    } else
    {
        lRow.children("td").eq(8).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QtdD1.indexOf("-") > -1) {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD2.indexOf("-") > -1) {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD3.indexOf("-") > -1) {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.ValorDeFechamento.indexOf("-") > -1) {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao.indexOf("-") > -1) {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gFirstClickBuscarMonitoramentoRiscoCustodiaBTC = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_BTC(pParametros)
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_BTC").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaBTC != true) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_BTC")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaBTC = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_BTC").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"                ,name: "Id"                 , jsonmap: "Id"                 , index: "Id"                  , width: 29, align: "center", sortable: true }
                      , { label: "Tipo Mercado"      ,name: "Tipo Mercado"       , jsonmap: "TipoMercado"        , index: "TipoMercado"         , width: 29, align: "center", sortable: true }
                      , { label: "Cod. Negócio"      ,name: "Codigo Negócio"     , jsonmap: "CodigoNegocio"      , index: "CodigoNegocio"       , width: 75, align: "left",   sortable: true }
                      , { label: "Empresa"           ,name: "Empresa"            , jsonmap: "Empresa"            , index: "Empresa"             , width: 85, align: "left",   sortable: true }
                      , { label: "Carteira"          ,name: "Carteira"           , jsonmap: "Carteira"           , index: "Carteira"            , width: 200, align: "left",  sortable: true }
                      , { label: "Qtd. Abert."       ,name: "Qtd.Abertura"       , jsonmap: "QtdAbertura"        , index: "QtdAbertura"         , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Compra"       ,name: "Qtd.Compra"         , jsonmap: "QtdCompra"          , index: "QtdCompra"           , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Venda"        ,name: "Qtd.Venda"          , jsonmap: "QtdVenda"           , index: "QtdVenda"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. Atual"        ,name: "Qtd.Atual"          , jsonmap: "QtdAtual"           , index: "QtdAtual"            , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D1"           ,name: "Qtd.D1"             , jsonmap: "QtdD1"              , index: "QtdD1"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D2"           ,name: "Qtd.D2"             , jsonmap: "QtdD2"              , index: "QtdD2"               , width: 55, align: "center", sortable: true }
                      , { label: "Qtd. D3"           ,name: "Qtd.D3"             , jsonmap: "QtdD3"              , index: "QtdD3"               , width: 55, align: "center", sortable: true }
                      , { label: "Vl. Fecham."       ,name: "Vl. Fecham."        , jsonmap: "ValorDeFechamento"  , index: "ValorDeFechamento"   , width: 55, align: "center", sortable: true }
                      , { label: "Cotação"           ,name: "Cotação"            , jsonmap: "Cotacao"            , index: "Cotacao"             , width: 55, align: "center", sortable: true }
                      , { label: "Variação"          ,name: "Variação"           , jsonmap: "Variacao"           , index: "Variacao"            , width: 75, align: "center", sortable: true }
                      , { label: "Resultado"         ,name: "Resultado"          , jsonmap: "Resultado"          , index: "Resultado"           , width: 75, align: "center", sortable: true }

                      ]
      , height: 'auto'
      , width: 1128
      //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_BTC_ItemDataBound
        , gridComplete : function () 
        {
            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_BTC tr").length == 0 )
            { $("#pnlMonitoramentoRiscoGeral_Custodia_BTC").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_BTC").show(); }
        }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_BTC_ItemDataBound(rowid, rowdata, rowelem)
{
     var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_BTC tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.QtdAbertura.indexOf("-") > -1) {
        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdCompra.indexOf("-") > -1) {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdVenda.indexOf("-") > -1) {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdAtual.indexOf("-") > -1) {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD1.indexOf("-") > -1) {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD2.indexOf("-") > -1) {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.QtdD3.indexOf("-") > -1) {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.ValorDeFechamento.indexOf("-") > -1) {
        lRow.children("td").eq(12).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Cotacao.indexOf("-") > -1) {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Variacao.indexOf("-") > -1) {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.Resultado.indexOf("-") > -1) {
        lRow.children("td").eq(15).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gFirstClickBuscarMonitoramentoRiscoCustodiaFundos = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos(pParametros)
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_Fundos").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaFundos != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Fundos")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaFundos = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Fundos").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaFundos"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"               , name: "Id"            , jsonmap: "Id"             , index: "Id"           , width: 29, align: "center", sortable: true }
                      , { label: ""                 , name: "TipoMercado"   , jsonmap: "TipoMercado"    , index: "TipoMercado"  , width: 29, align: "center", sortable: true }
                      , { label: "Nome Fundo-Clube" , name: "NomeFundo"     , jsonmap: "NomeFundo"      , index: "NomeFundo"    , width: 200, align: "center", sortable: true }
                      , { label: "Cota"             , name: "Cota"          , jsonmap: "Cota"           , index: "Cota"         , width: 55, align: "center", sortable: true }
                      , { label: "Quantidade"       , name: "Quantidade"    , jsonmap: "Quantidade"     , index: "Quantidade"   , width: 75, align: "center", sortable: true }
                      , { label: "Valor Bruto"      , name: "ValorBruto"    , jsonmap: "ValorBruto"     , index: "ValorBruto"   , width: 75, align: "center", sortable: true }
                      , { label: "IR"               , name: "IR"            , jsonmap: "IR"             , index: "IR"           , width: 75, align: "center", sortable: true }
                      , { label: "IOF"              , name: "IOF"           , jsonmap: "IOF"            , index: "IOF"          , width: 75, align: "center", sortable: true }
                      , { label: "Valor Líquido"    , name: "ValorLiquido"  , jsonmap: "ValorLiquido"   , index: "ValorLiquido" , width: 90, align: "center", sortable: true }
                      ]
      , height: 'auto'
      , width: 1128
        //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , gridComplete  : GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos_GridComplete 
      ,afterInsertRow : GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos_ItemDataBound
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos_GridComplete() 
{
    if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Fundos tr").length == 0) {
        $("#pnlMonitoramentoRiscoGeral_Custodia_Fundos").hide();
    }
    else {
        $("#pnlMonitoramentoRiscoGeral_Custodia_Fundos").show();
    }
}


function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos_ItemDataBound(rowid, rowdata, rowelem)
 {
     var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Fundos tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');
}


var gFirstClickBuscarMonitoramentoRiscoCustodiaRendaFixa = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_RendaFixa(pParametros)
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_RendaFixa").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaRendaFixa != true) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_RendaFixa")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaRendaFixa = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_RendaFixa").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaRendaFixa"
      , datatype: "json"
      , mtype:    "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        //{ label: "Id"               , name: "Id"            , jsonmap: "Id"             , index: "Id"               , width: 29, align: "center", sortable: true }
                        { label: "Titulo"           , name: "Titulo"        , jsonmap: "Titulo"         , index: "Titulo"           , width: 175, align: "left", sortable: true }
                      , { label: "Aplicacao"        , name: "Aplicacao"     , jsonmap: "Aplicacao"      , index: "Aplicacao"        , width: 75, align: "center", sortable: true }
                      , { label: "Vencimento"       , name: "Vencimento"    , jsonmap: "Vencimento"     , index: "Vencimento"       , width: 75, align: "center", sortable: true }
                      , { label: "Taxa"             , name: "Taxa"          , jsonmap: "Taxa"           , index: "Taxa"             , width: 80, align: "center", sortable: true }
                      , { label: "Quantidade"       , name: "Quantidade"    , jsonmap: "Quantidade"     , index: "Quantidade"       , width: 85, align: "center", sortable: true }
                      , { label: "ValorOriginal"    , name: "ValorOriginal" , jsonmap: "ValorOriginal"  , index: "ValorOriginal"    , width: 90, align: "center", sortable: true }
                      , { label: "SaldoBruto"       , name: "SaldoBruto"    , jsonmap: "SaldoBruto"     , index: "SaldoBruto"       , width: 90, align: "center", sortable: true }
                      , { label: "IRRF"             , name: "IRRF"          , jsonmap: "IRRF"           , index: "IRRF"             , width: 55, align: "center", sortable: true }
                      , { label: "IOF"              , name: "IOF"           , jsonmap: "IOF"            , index: "IOF"              , width: 55, align: "center", sortable: true }
                      , { label: "SaldoLiquido"     , name: "SaldoLiquido"  , jsonmap: "SaldoLiquido"   , index: "SaldoLiquido"     , width: 90, align: "center", sortable: true }
                      


                      ]
      , height: 'auto'
      , width: 1128
        //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_RendaFixa_ItemDataBound
        //, subGridRowExpanded: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC_Subgrid_Expander
        , gridComplete: function () 
        {
            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_RendaFixa tr").length == 0)
            { 
                $("#pnlMonitoramentoRiscoGeral_Custodia_RendaFixa").hide(); 
            } 
            else 
            { 
                $("#pnlMonitoramentoRiscoGeral_Custodia_RendaFixa").show(); 
            }
                  
        }
    //}).jqGrid('hideCol', "Id");
    });
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_RendaFixa_ItemDataBound(rowid, rowdata, rowelem)
 {
     var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_RendaFixa tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');
}

var gFirstClickBuscarMonitoramentoRiscoCustodiaOperacoesBTC = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC(pParametros)
 {
    $("#pnlMonitoramentoRiscoGeral_Custodia_Operacoes_BTC").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaOperacoesBTC != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_BTC")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaOperacoesBTC = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_BTC").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaOperacoesBTC"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"               , name: "Id"                , jsonmap: "Id"                 , index: "Id"                   , width: 29, align: "center", sortable: true }
                      , { label: "Carteira"         , name: "Carteira"          , jsonmap: "Carteira"           , index: "Carteira"             , width: 75, align: "center", sortable: true }
                      , { label: "Codigo Cliente"   , name: "CodigoCliente"     , jsonmap: "CodigoCliente"      , index: "CodigoCliente"        , width: 75, align: "center", sortable: true }
                      , { label: "Data Abertura"    , name: "DataAbertura"      , jsonmap: "DataAbertura"       , index: "DataAbertura"         , width: 120, align: "center", sortable: true }
                      , { label: "Data Vencimento"  , name: "DataVencimento"    , jsonmap: "DataVencimento"     , index: "DataVencimento"       , width: 120, align: "center", sortable: true }
                      , { label: "Instrumento"      , name: "Instrumento"       , jsonmap: "Instrumento"        , index: "Instrumento"          , width: 85, align: "center", sortable: true }
                      , { label: "Preco Médio"      , name: "PrecoMedio"        , jsonmap: "PrecoMedio"         , index: "PrecoMedio"           , width: 90, align: "center", sortable: true }
                      , { label: "Preco Mercado"    , name: "PrecoMercado"      , jsonmap: "PrecoMercado"       , index: "PrecoMercado"         , width: 90, align: "center", sortable: true }
                      , { label: "Quantidade"       , name: "Quantidade"        , jsonmap: "Quantidade"         , index: "Quantidade"           , width: 55, align: "center", sortable: true }
                      , { label: "Remuneracao"      , name: "Remuneracao"       , jsonmap: "Remuneracao"        , index: "Remuneracao"          , width: 85, align: "center", sortable: true }
                      , { label: "Taxa"             , name: "Taxa"              , jsonmap: "Taxa"               , index: "Taxa"                 , width: 90, align: "center", sortable: true }
                      , { label: "Tipo Contrato"    , name: "TipoContrato"      , jsonmap: "TipoContrato"       , index: "TipoContrato"         , width: 75, align: "center", sortable: true }
                      , { label: "Subtotal Quant."  , name: "SubtotalQuantidade", jsonmap: "SubtotalQuantidade" , index: "SubtotalQuantidade"   , width: 90, align: "center", sortable: true }
                      , { label: "Subtotal Valor"   , name: "SubtotalValor"     , jsonmap: "SubtotalValor"      , index: "SubtotalValor"        , width: 90, align: "center", sortable: true }
                      


                      ]
      , height: 'auto'
      , width: 1128
        //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC_ItemDataBound
        , subGridRowExpanded: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC_Subgrid_Expander
        , gridComplete: function () {
            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_BTC tr").length == 0)
            { $("#pnlMonitoramentoRiscoGeral_Custodia_Operacoes_BTC").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Operacoes_BTC").show(); }
                  
        }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC_Subgrid_Expander(subgrid_id, row_id)
{
    var subgrid_table_id;

    var lTable = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_BTC");

    var lInstrumento = lTable.find("#" + row_id).find("td").eq(6).html();

    var lCliente     = lTable.find("#" + row_id).find("td").eq(3).html(); 

    subgrid_table_id = subgrid_id + "_table";

    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

    jQuery("#" + subgrid_table_id)
        .jqGrid({
            url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaOperacoesBTCDetalhes"
        , hoverrows: false
        , datatype: "json"
        , mtype: "POST"
        , postData: { 'Instrumento': lInstrumento, 'CodigoCliente': lCliente }
        , shrinkToFit: false
        , colNames: ['Carteira', 'Carteira', 'Data Abertura', 'Data Vencimento', 'Instrumento', 'Preço Médio', 'Preço Mercado', 'Quantidade','Remuneração','Taxa','Tipo Contrato']
        , colModel: [
            { label: "Carteira"         , name: "Carteira"          , index: "Carteira"         , width: 60, align: "center"  ,sortable:true},
            { label: "Codigo Cliente"   , name: "CodigoCliente"     , index: "CodigoCliente"    , width: 60, align: "center"  ,sortable:true},
            { label: "Data Abertura"    , name: "DataAbertura"      , index: "DataAbertura"     , width: 120, align: "center" ,sortable:true},
            { label: "Data Vencimento"  , name: "DataVencimento"    , index: "DataVencimento"   , width: 120, align: "center" ,sortable:true},
            { label: "Instrumento"      , name: "Instrumento"       , index: "Instrumento"      , width: 60, align: "center"  ,sortable:true},
            { label: "Preço Médio"      , name: "PrecoMedio"        , index: "PrecoMedio"       , width: 60, align: "center"  ,sortable:true},
            { label: "Preço Mercado"    , name: "PrecoMercado"      , index: "PrecoMercado"     , width: 60, align: "center"  ,sortable:true},
            { label: "Quantidade"       , name: "Quantidade"        , index: "Quantidade"       , width: 60, align: "center"  ,sortable:true},
            { label: "Remuneração"      , name: "Remuneracao"       , index: "Remuneracao"      , width: 60, align: "center"  ,sortable:true},
            { label: "Taxa"             , name: "Taxa"              , index: "Taxa"             , width: 60, align: "center"  ,sortable:true},
            { label: "Tipo Contrato"    , name: "TipoContrato"      , index: "TipoContrato"     , width: 60, align: "center"  ,sortable:true},
          ]
        , jsonReader: {
            root: "Itens"
            , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id: "0"              //primeira propriedade do elemento de linha é o Id
            , repeatitems: false
        }
        , width: 840
        , height: "100%"
        , rowNum: 'auto'
        , sortname: 'num'
        , sortorder: 'asc'
        , sortable: true
        });
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_BTC tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

//    if (rowelem.LucroPrejuizo.indexOf("-") > -1) 
//    {
//        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
//    }
}

var gFirstClickBuscarMonitoramentoRiscoCustodiaOperacoesTermo = true;

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo(pParametros) 
{
    $("#pnlMonitoramentoRiscoGeral_Custodia_Operacoes_Termo").show();

    if (gFirstClickBuscarMonitoramentoRiscoCustodiaOperacoesTermo != true) {
        $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_Termo")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarMonitoramentoRiscoCustodiaOperacoesTermo = false;

    $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_Termo").jqGrid(
    {
        url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaOperacoesTermo"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"               , name: "Id"                    , jsonmap: "Id"                     , index: "Id"                   , width: 29, align: "center", sortable: true }
                      //, { label: "Data Execução"    , name: "DataExecucao"          , jsonmap: "DataExecucao"           , index: "DataExecucao"         , width: 120, align: "center", sortable: true }
                      //, { label: "Data Vencimento"  , name: "DataVencimento"        , jsonmap: "DataVencimento"         , index: "DataVencimento"       , width: 120, align: "center", sortable: true }
                      , { label: "Codigo Cliente"   , name: "CodigoCliente"         , jsonmap: "CodigoCliente"          , index: "CodigoCliente"        , width: 85, align: "center", sortable: true }
                      , { label: "Instrumento"      , name: "Instrumento"           , jsonmap: "Instrumento"            , index: "Instrumento"          , width: 85, align: "center", sortable: true }
                      //, { label: "Lucro Prejuízo"   , name: "LucroPrejuizo"         , jsonmap: "LucroPrejuizo"          , index: "LucroPrejuizo"        , width: 90, align: "center", sortable: true }
                      //, { label: "Preco Execução"   , name: "PrecoExecucao"         , jsonmap: "PrecoExecucao"          , index: "PrecoExecucao"        , width: 90, align: "center", sortable: true }
                      //, { label: "Preco Mercado"    , name: "PrecoMercado"          , jsonmap: "PrecoMercado"           , index: "PrecoMercado"         , width: 90, align: "center", sortable: true }
                      //, { label: "Quantidade"       , name: "Quantidade"            , jsonmap: "Quantidade"             , index: "Quantidade"           , width: 55, align: "center", sortable: true }
                      , { label: "Subtotal Quant."  , name: "SubtotalQuantidade"    , jsonmap: "SubtotalQuantidade"     , index: "SubtotalQuantidade"   , width: 90, align: "center", sortable: true }
                      , { label: "Preço Médio"      , name: "PrecoMedio"            , jsonmap: "PrecoMedio"             , index: "PrecoMedio"           , width: 90, align: "center", sortable: true }
                      , { label: "Subtotal Contrato", name: "SubtotalContrato"      , jsonmap: "SubtotalContrato"       , index: "SubtotalContrato"     , width: 90, align: "center", sortable: true }
                      , { label: "Subtotal L/P"     , name: "SubtotalLucroPrejuizo" , jsonmap: "SubtotalLucroPrejuizo"  , index: "SubtotalLucroPrejuizo", width: 90, align: "center", sortable: true }
                      

                      ]
      , height: 'auto'
      , width: 1128
        //, pager: "#pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , sortable: true
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo_ItemDataBound
        , subGridRowExpanded: GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo_Subgrid_Expander
        , gridComplete: function () {
            if ($("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_Termo tr").length == 0)
            { $("#pnlMonitoramentoRiscoGeral_Custodia_Operacoes_Termo").hide(); } else { $("#pnlMonitoramentoRiscoGeral_Custodia_Operacoes_Termo").show(); }
        }
    }).jqGrid('hideCol', "Id");
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo_Subgrid_Expander(subgrid_id, row_id) 
{
    var subgrid_table_id;
    var lTabela = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_Termo");
    
    var lInstrumento = lTabela.find("#" + row_id).find("td").eq(3).html();

    var lCliente = lTabela.find("#" + row_id).find("td").eq(2).html();

    subgrid_table_id = subgrid_id + "_table";

    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

    jQuery("#" + subgrid_table_id)
        .jqGrid({
            url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodiaOperacoesTermoDetalhes"
        , hoverrows: false
        , datatype: "json"
        , mtype: "POST"
        , postData: { 'Instrumento': lInstrumento, 'CodigoCliente': lCliente }
        , shrinkToFit: false
        , colNames: ['Instrumento', 'CodigoCliente', 'Data Execução', 'Data Rolagem', 'Data Vencimento', 'Quantidade', 'Lucro Prejuízo', 'Preço Execução', 'Preço Mercado', 'Subtotal Operação']
        , colModel: [
            { label: "Instrumento"      , name: "Instrumento"       , index: "Instrumento"      , width: 60, align: "center" },
            { label: "Codigo Cliente"   , name: "CodigoCliente"     , index: "CodigoCliente"    , width: 60, align: "center" },
            { label: "Data Execução"    , name: "DataExecucao"      , index: "DataExecucao"     , width: 120, align: "center" },
            { label: "Data Rolagem"     , name: "DataRolagem"       , index: "DataRolagem"      , width: 120, align: "center" },
            { label: "Data Vencimento"  , name: "DataVencimento"    , index: "DataVencimento"   , width: 120, align: "center" },
            { label: "Quantidade"       , name: "Quantidade"        , index: "Quantidade"       , width: 75, align: "center" },
            { label: "Lucro Prejuizo"   , name: "LucroPrejuizo"     , index: "LucroPrejuizo"    , width: 75, align: "center" },
            { label: "Preço Execução"   , name: "PrecoExecucao"     , index: "PrecoExecucao"    , width: 75, align: "center" },
            { label: "Preço Mercado"    , name: "PrecoMercado"      , index: "PrecoMercado"     , width: 75, align: "center" },
            { label: "Subtotal Operação", name: "SubtotalOperacao"  , index: "SubtotalOperacao" , width: 120, align: "center" },
          ]
        , jsonReader: {
            root: "Itens"
            , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id: "0"              //primeira propriedade do elemento de linha é o Id
            , repeatitems: false
        }
        , width: 950
        , height: "100%"
        , rowNum: 'auto'
        , sortname: 'num'
        , sortorder: 'asc'
        });
}

function GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_Termo tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.SubtotalQuantidade.indexOf("-") > -1) {
        lRow.children("td").eq(4).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.PrecoMedio.indexOf("-") > -1) {
        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.SubtotalContrato.indexOf("-") > -1) {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.SubtotalLucroPrejuizo.indexOf("-") > -1) {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gAssessorDetalheHeaderRisco;

var gNomeAssessorDetalheHeaderRisco;

var gNomeClienteDetalheHeaderRisco;

var gStatusClienteDetalheHeaderRisco

function GradIntra_MonitoramentoRisco_BuscarSaldoLimite_ContaCorrente(pCodigo, pCodigoBmf, pAssessor, pNomeAssessor, pNomeCliente) 
{
    var lURL = "MonitoramentoRiscoGeralDetalhamento.aspx";
    var lObjetoParametros = { Acao             : "BuscarValoresLabels"
                            , CodigoCliente    : pCodigo
                            , CodigoClienteBmf : pCodigoBmf
    };

    gAssessorDetalheHeaderRisco = pAssessor;

    gNomeAssessorDetalheHeaderRisco = pNomeAssessor;

    gNomeClienteDetalheHeaderRisco = pNomeCliente;

    GradIntra_CarregarJsonVerificandoErro(lURL
                                         , lObjetoParametros
                                         , function (pResposta) { GradIntra_MonitoramentoRisco_BuscarSaldoLimite_ContaCorrente_Callback(pResposta); });
}

function GradIntra_MonitoramentoRisco_BuscarSaldoLimite_ContaCorrente_Callback(pResposta) 
{
//    /*Dados de saldo, limites, conta corrente*/
    $("#tdConta_SaldoD0")      .html(pResposta.ObjetoDeRetorno.ContaCorrenteD0);
    $("#tdConta_SaldoD1")      .html(pResposta.ObjetoDeRetorno.ContaCorrenteD1);
    $("#tdConta_SaldoD2")      .html(pResposta.ObjetoDeRetorno.ContaCorrenteD2);
    $("#tdConta_SaldoD3")      .html(pResposta.ObjetoDeRetorno.ContaCorrenteD3);
    $("#tdConta_SaldoDTotal")  .html(pResposta.ObjetoDeRetorno.ContaCorrenteDTotal);
    
    $("#tdConta_ContaMargem")  .html(pResposta.ObjetoDeRetorno.ContaCorrenteContaMargem);
    $("#tdConta_Projetado")    .html(pResposta.ObjetoDeRetorno.ContaCorrenteProjetado);
    $("#tdConta_BMF_Garantias").html(pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBmf);
    $("#tdConta_BMF_Disponivel").html(pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelBmf);

    $("#tdConta_CompraAvista").html(pResposta.ObjetoDeRetorno.ContaCorrenteCompraAvista);
    $("#tdConta_CompraOpcao") .html(pResposta.ObjetoDeRetorno.ContaCorrenteCompraOpcao);
    $("#tdConta_VendaAvista") .html(pResposta.ObjetoDeRetorno.ContaCorrenteVendaAvista);
    $("#tdConta_VendaOpcao")  .html(pResposta.ObjetoDeRetorno.ContaCorrenteVendaOpcao);
    $("#tdConta_Disponivel")  .html(pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelAvista);
    $("#tdConta_BMF_MargemRequerida").html(pResposta.ObjetoDeRetorno.ContaCorrenteMargemRequeriada);
    $("#tdConta_BOV_Garantias").html(pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBovespa);
    $("#tdConta_BOV_MargemRequerida").html(pResposta.ObjetoDeRetorno.ContaCorrenteMargemRequeridaBovespa);
    $("#tdConta_BOV_Disponivel").html(pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelBovespa);
    $("#tdConta_Financeiro_SFP")     .html(pResposta.ObjetoDeRetorno.Financeiro_SFP);


//    /*Subtotais da grid*/
    $("#SubtotalCustodiaAvista") .html(pResposta.ObjetoDeRetorno.CustodiaSubTotalAvista);
    $("#SubtotalCustodiaOpcoes") .html(pResposta.ObjetoDeRetorno.CustodiaSubTotalOpcoes);
    //$("#SubtotalCustodiaTermo")  .html(pResposta.ObjetoDeRetorno.CustodiaSubTotalTermo);
    //$("SubtotalCustodiaOperacoesTermo").html(pResposta.ObjetoDeRetorno.CustodiaSubTotalTermo);

    $("#SubtotalCustodiaBmf").html(pResposta.ObjetoDeRetorno.CustodiaSubTotalBmfD_1);

    $("#SubtotalCustodiaPosicaoBmf").html(pResposta.ObjetoDeRetorno.CustodiaSubTotalBmf);

    $("#SubtotalCustodiaTesouro").html(pResposta.ObjetoDeRetorno.CustodiaSubTotalTesouroDireto);
    
    //$("#SubtotalCustodiaTermo")  .html(pResposta.ObjetoDeRetorno.CustodiaSubTotalTermo);

    $("#SubtotalCustodiaOperacoesBTC").html(pResposta.ObjetoDeRetorno.OperacoesSubtotalBTC);
    
    $("#SubtotalCustodiaOperacoesTermo").html(pResposta.ObjetoDeRetorno.OperacoesSubtotalTermo);

    $("#SubtotalCustodiaFundos").html(pResposta.ObjetoDeRetorno.CustodiaSubTotalClubesFundos);

    $("#SubtotalCustodiaRendaFixa").html(pResposta.ObjetoDeRetorno.CustodiaSubTotalRendaFixa);

    $("#DetalhesHeader_Cliente").html();

    $("#DetalhesHeader_Cliente").html("Cliente: " + gNomeClienteDetalheHeaderRisco           + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                      "Assessor:" + gAssessorDetalheHeaderRisco + " - " + gNomeAssessorDetalheHeaderRisco + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                      "Conta Cliente:"   + pResposta.ObjetoDeRetorno.DigitoCodigoCliente + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                      //"Codigo Bolsa:" + pResposta.ObjetoDeRetorno.CdCliente  + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                      "Data Atual:"    + pResposta.ObjetoDeRetorno.DataAtual + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                      "Status Bovespa:" + (pResposta.ObjetoDeRetorno.StatusBovespa == "A" ? "Ativo" : "Inativo") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                      "Status BMF:" + (pResposta.ObjetoDeRetorno.StatusBmf == "A" ? "Ativo" : "Inativo")  + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

    if (pResposta.ObjetoDeRetorno.ContaCorrenteD0.indexOf("-") > -1)
    { $("#tdConta_SaldoD0").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteD0 == "0,00")
    { $("#tdConta_SaldoD0").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_SaldoD0").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteD1.indexOf("-") > -1)
    { $("#tdConta_SaldoD1").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteD1 == "0,00")
    { $("#tdConta_SaldoD1").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_SaldoD1").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteD2.indexOf("-") > -1)
    { $("#tdConta_SaldoD2").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteD2 == "0,00")
    { $("#tdConta_SaldoD2").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_SaldoD2").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteD3.indexOf("-") > -1) 
    { $("#tdConta_SaldoD3").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteD3 == "0,00")
    { $("#tdConta_SaldoD3").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_SaldoD3").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteDTotal.indexOf("-") > -1)
    { $("#tdConta_SaldoDTotal").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteDTotal == "0,00")
    { $("#tdConta_SaldoDTotal").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_SaldoDTotal").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteContaMargem.indexOf("-") > -1)
    { $("#tdConta_ContaMargem").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteContaMargem == "0,00")
    { $("#tdConta_ContaMargem").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_ContaMargem").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteProjetado.indexOf("-") > -1)
    { $("#tdConta_Projetado").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteProjetado =="0,00")
    { $("#tdConta_Projetado").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_Projetado").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBmf.indexOf("-") > -1)
    { $("#tdConta_BMF_Garantias").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBmf =="0,00")
    { $("#tdConta_BMF_Garantias").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_BMF_Garantias").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelBmf.indexOf("-") > -1)
    { $("#tdConta_BMF_Disponivel").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelBmf == "0,00")
    { $("#tdConta_BMF_Disponivel").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_BMF_Disponivel").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteMargemRequeriada.indexOf("-") > -1)
    { $("#tdConta_BMF_MargemRequerida").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteMargemRequeriada == "0,00")
    { $("#tdConta_BMF_MargemRequerida").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_BMF_MargemRequerida").css({ fontWeight: 'normal', color: 'blue' });}


    if (pResposta.ObjetoDeRetorno.Financeiro_SFP.indexOf("-") > -1)
    { $("#tdConta_Financeiro_SFP").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.Financeiro_SFP=="0,00")
    { $("#tdConta_Financeiro_SFP").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_Financeiro_SFP").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBovespa.indexOf("-") > -1)
    { $("#tdConta_BOV_Garantias").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBovespa == "0,00")
    { $("#tdConta_BOV_Garantias").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_BOV_Garantias").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteMargemRequeridaBovespa.indexOf("-") > -1)
    { $("#tdConta_BOV_MargemRequerida").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBovespa == "0,00")
    { $("#tdConta_BOV_MargemRequerida").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_BOV_MargemRequerida").css({ fontWeight: 'normal', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelBovespa.indexOf("-") > -1)
    { $("#tdConta_BOV_Disponivel").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteGarantiaBovespa == "0,00")
    { $("#tdConta_BOV_Disponivel").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_BOV_Disponivel").css({ fontWeight: 'normal', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.ContaCorrenteCompraAvista.indexOf("-") > -1) 
    { $("#tdConta_CompraAvista").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteCompraAvista == "0,00")
    { $("#tdConta_CompraAvista").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_CompraAvista").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteCompraOpcao.indexOf("-") > -1) 
    { $("#tdConta_CompraOpcao").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteCompraOpcao == "0,00")
    { $("#tdConta_CompraOpcao").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_CompraOpcao").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteVendaAvista.indexOf("-") > -1) 
    { $("#tdConta_VendaAvista").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteVendaAvista =="0,00")
    { $("#tdConta_VendaAvista").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_VendaAvista").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteVendaOpcao.indexOf("-") > -1) 
    { $("#tdConta_VendaOpcao").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteVendaOpcao =="0,00")
    { $("#tdConta_VendaOpcao").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_VendaOpcao").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelAvista.indexOf("-") > -1) 
    { $("#tdConta_Disponivel").css({ fontWeight: 'normal', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.ContaCorrenteDisponivelAvista =="0,00")
    { $("#tdConta_Disponivel").css({ fontWeight: 'normal', color: 'black' }); }
    else
    { $("#tdConta_Disponivel").css({ fontWeight: 'normal', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalAvista.indexOf("-") > -1) 
    { $("#SubtotalCustodiaAvista").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalAvista == "0,00" )
    { $("#SubtotalCustodiaAvista").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaAvista").css({ fontWeight: 'bold', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalOpcoes.indexOf("-") > -1)
    { $("#SubtotalCustodiaOpcoes").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalOpcoes =="0,00")
    { $("#SubtotalCustodiaOpcoes").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaOpcoes").css({ fontWeight: 'bold', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalTermo.indexOf("-") > -1)
    { $("#SubtotalCustodiaTermo").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalTermo =="0,00")
    { $("#SubtotalCustodiaTermo").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaTermo").css({ fontWeight: 'bold', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBTC.indexOf("-") > -1)
    { $("#SubtotalCustodiaBTC").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBTC == "0,00" )
    { $("#SubtotalCustodiaBTC").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaBTC").css({ fontWeight: 'bold', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalClubesFundos.indexOf("-") > -1)
    { $("#SubtotalCustodiaFundos").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalClubesFundos == "0,00")
    { $("#SubtotalCustodiaFundos").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaFundos").css({ fontWeight: 'bold', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalRendaFixa.indexOf("-") > -1)
    { $("#SubtotalCustodiaRendaFixa").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalRendaFixa == "0,00")
    { $("#SubtotalCustodiaRendaFixa").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaRendaFixa").css({ fontWeight: 'bold', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBmfD_1.indexOf("-") > -1) 
    {
        $("#SubtotalCustodiaBmf").css({ fontWeight: 'bold', color: 'red' });
    } else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBmfD_1 == "0,00") 
    {
        $("#SubtotalCustodiaBmf").css({ fontWeight: 'bold', color: 'black' });
    }
    else
    {
        $("#SubtotalCustodiaBmf").css({ fontWeight: 'bold', color: 'blue' });
    }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBmf.indexOf("-") > -1) 
    {
        $("#SubtotalCustodiaPosicaoBmf").css({ fontWeight: 'bold', color: 'red' });
    }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBmf == "0,00") 
    {
        $("#SubtotalCustodiaPosicaoBmf").css({ fontWeight: 'bold', color: 'black' });
    }
    else 
    {
        $("#SubtotalCustodiaPosicaoBmf").css({ fontWeight: 'bold', color: 'blue' });
    }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalTesouroDireto.indexOf("-") > -1)
    { $("#SubtotalCustodiaTesouro").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalTesouroDireto == "0,00")
    { $("#SubtotalCustodiaTesouro").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaTesouro").css({ fontWeight: 'bold', color: 'blue' }); }


    if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBTC.indexOf("-") > -1)
    { $("#SubtotalCustodiaBTC").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.CustodiaSubTotalBTC == "0,00" )
    { $("#SubtotalCustodiaBTC").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaBTC").css({ fontWeight: 'bold', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.OperacoesSubtotalTermo.indexOf("-") > -1)
    { $("#SubtotalCustodiaOperacoesTermo").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.OperacoesSubtotalTermo == "0,00")
    { $("#SubtotalCustodiaOperacoesTermo").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaOperacoesTermo").css({ fontWeight: 'bold', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.OperacoesSubtotalBTC.indexOf("-") > -1)
    { $("#SubtotalCustodiaOperacoesBTC").css({ fontWeight: 'bold', color: 'red' }); }
    else if (pResposta.ObjetoDeRetorno.OperacoesSubtotalBTC == "0,00")
    { $("#SubtotalCustodiaOperacoesBTC").css({ fontWeight: 'bold', color: 'black' }); }
    else
    { $("#SubtotalCustodiaOperacoesBTC").css({ fontWeight: 'bold', color: 'blue' }); }

    if (pResposta.ObjetoDeRetorno.ListaGarantiasBMF != null && pResposta.ObjetoDeRetorno.ListaGarantiasBMF.length > 0)
    {
        var lListaGarantias= pResposta.ObjetoDeRetorno.ListaGarantiasBMF;
        
        var lTabelaGarantias = $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BMF > tbody");

        $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BMF").show();

        lTabelaGarantias.children("tr").remove();
        lTabelaGarantias.children().html("");
        
        for ( i=0; i< lListaGarantias.length; i++) 
        {
            lTabelaGarantias.append("<tr><td align=\"left\">" + lListaGarantias[i].DescricaoGarantia + "</td><td align=\"right\">" + lListaGarantias[i].ValorGarantiaDeposito + "</td></tr>");
            //lTabelaGarantias.append();
        }

    } else 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BMF").hide();
    }

    if (pResposta.ObjetoDeRetorno.ListaGarantiasBOV != null && pResposta.ObjetoDeRetorno.ListaGarantiasBOV.length > 0) 
    {
        var lListaGarantias = pResposta.ObjetoDeRetorno.ListaGarantiasBOV;

        var lTabelaGarantias = $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BOV > tbody");

        $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BOV").show();

        lTabelaGarantias.children("tr").remove();
        lTabelaGarantias.children().html("");

        lTabelaGarantias.append("<tr><td>Data Movimento</td><td>Finalidade</td><td colspan=\"3\" align=\"left\">Ativo</td><td>Num. Distribuição</td><td align=\"right\"> Quantidade</td><td align=\"right\">Valor Garantia Depositada</td></tr>");

        for (i = 0; i < lListaGarantias.length; i++) 
        {
            var lContent = "<tr>" +
              "<td align=\"center\">" + lListaGarantias[i].DtDeposito + "</td>" +
              "<td align=\"center\">" + lListaGarantias[i].FinalidadeGarantia + "</td>" +
              "<td align=\"left\">" + lListaGarantias[i].CodigoAtividade + "</td>" +
              "<td align=\"left\">" + lListaGarantias[i].NomeEmpresa + "</td>" +
              "<td align=\"left\">" + lListaGarantias[i].CodigoIsin + "</td>" +
              "<td align=\"center\">" + lListaGarantias[i].CodigoDistribuicao + "</td>" +
              "<td align=\"right\">" + lListaGarantias[i].Quantidade + "</td>" +
              "<td align=\"right\">" + lListaGarantias[i].ValorGarantiaDeposito + "</td></tr>";
            
            lTabelaGarantias.append(lContent);
            //lTabelaGarantias.append();
        }

    } else {
        $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BOV").hide();
    }

    if (pResposta.ObjetoDeRetorno.ExtratoLiquidacao != null )
    {
        var lExtratoLiquidacao = pResposta.ObjetoDeRetorno.ExtratoLiquidacao;

        var lTabelaExtratoLiquidacao = $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao > tbody");

        $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao").show();

        lTabelaExtratoLiquidacao.children("tr").remove();
        lTabelaExtratoLiquidacao.children().html("");

        if (lExtratoLiquidacao.SaldoAnterior.indexOf("-") > -1)
        {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Saldo_Anterior").css({ fontWeight: 'bold', color: 'red' });
        }
        else if (lExtratoLiquidacao.SaldoAnterior == "0,00")
        {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Saldo_Anterior").css({ fontWeight: 'bold', color: 'black' });
        }
        else
        {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Saldo_Anterior").css({ fontWeight: 'bold', color: 'blue' });
        }

        $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Saldo_Anterior").html(lExtratoLiquidacao.SaldoAnterior);

        if (lExtratoLiquidacao.ValorDisponivel.indexOf("-") > -1)
        {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Disponivel").css({ fontWeight: 'bold', color: 'red' });
        }
        else if (lExtratoLiquidacao.ValorDisponivel == "0,00")
        {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Disponivel").css({ fontWeight: 'bold', color: 'black' });
        }
        else {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Disponivel").css({ fontWeight: 'bold', color: 'blue' });
        }


        $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Disponivel").html(lExtratoLiquidacao.ValorDisponivel);

        if (lExtratoLiquidacao.TotalCliente.indexOf("-") > -1) {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Total_Cliente").css({ fontWeight: 'bold', color: 'red' });
        }
        else if (lExtratoLiquidacao.TotalCliente == "0,00") {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Total_Cliente").css({ fontWeight: 'bold', color: 'black' });
        }
        else {
            $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Total_Cliente").css({ fontWeight: 'bold', color: 'blue' });
        }

        $("#tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Total_Cliente").html(lExtratoLiquidacao.TotalCliente);

        lTabelaExtratoLiquidacao.append("<tr><td> Data Movimento</td><td> Data Liquidação</td><td> Histórico</td><td align=\"right\"> Débito</td><td align=\"right\"> Crédito</td><td align=\"right\"> Saldo</td></tr>");

        for (i = 0; i < lExtratoLiquidacao.ListaExtratoMovimento.length; i++) 
        {
            var lContentTable = 
            "<tr><td align=\"center\">" + lExtratoLiquidacao.ListaExtratoMovimento[i].DataMovimento        + "</td>" +
            "<td align=\"center\">"    + lExtratoLiquidacao.ListaExtratoMovimento[i].DataLiquidacao       + "</td>" +
            "<td align=\"left\">"     + lExtratoLiquidacao.ListaExtratoMovimento[i].DescricaoHistorico   + "</td>" +
            "<td align=\"right\" style=\"color:red\">"    + lExtratoLiquidacao.ListaExtratoMovimento[i].ValorDebito          + "</td>" +
            "<td align=\"right\" style=\"color:blue\">"    + lExtratoLiquidacao.ListaExtratoMovimento[i].ValorCredito         + "</td>";
            

            if (lExtratoLiquidacao.ListaExtratoMovimento[i].ValorSaldo.indexOf("-") > -1)
            {
                lContentTable += "<td align=\"right\" style=\"color:red\">" + lExtratoLiquidacao.ListaExtratoMovimento[i].ValorSaldo + "</td></tr>";
            }
            else if (lExtratoLiquidacao.ListaExtratoMovimento[i].ValorSaldo == "0,00")
            {
                lContentTable += "<td align=\"right\">" + lExtratoLiquidacao.ListaExtratoMovimento[i].ValorSaldo + "</td></tr>";
            }
            else
            {
                lContentTable += "<td align=\"right\" style=\"color:blue\">" + lExtratoLiquidacao.ListaExtratoMovimento[i].ValorSaldo + "</td></tr>";
            }

            lTabelaExtratoLiquidacao.append(lContentTable);
        }
     }
    else
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao").hide();
    }
    
    if (pResposta.ObjetoDeRetorno.ListaPortas != "")
    {
        $("#lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_Portas_Operadas").html(pResposta.ObjetoDeRetorno.ListaPortas);
    }
    else {
        $("#lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_Portas_Operadas").html("");
    }

}

function rdoRisco_Monitoramento_Intradiario_FiltroSFPxNET(pSender) 
{
    $("input[name='chkRisco_Monitoramento_FiltroSemaforo']:checked").each(function () { $(this).attr('checked', false) });

    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(pSender) 
{
    $("input[name='chkRisco_Monitoramento_FiltroSemaforo']:checked").each(function () { $(this).attr('checked', false) });

    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Monitoramento_LucrosPrejuizos_FiltroSemaforo(pSender) 
{
    $("input[name='chkRisco_Monitoramento_FiltroProporcaoPrejuizo']:checked").each(function () { $(this).attr('checked', false) });

    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal' , fontSize: '11px'}) });

    $("label[for='"+lId+"']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Monitoramento_Intradiario_NETxSFP(pSender) 
{
    //$("input[name='chkRisco_Monitoramento_Intradiario_NETxSFP']:checked").each(function () { $(this).attr('checked', false) });

    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal' , fontSize: '11px'}) });

    $("label[for='"+lId+"']").css({ fontWeight: 'bold', fontSize: '14px' });
}
function rdoRisco_Monitoramento_Intradiario_NET(pSender)
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal' , fontSize: '11px'}) });

    $("label[for='"+lId+"']").css({ fontWeight: 'bold', fontSize: '14px' });
}
function rdoRisco_Monitoramento_Intradiario_EXPxPosicao(pSender)
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal' , fontSize: '11px'}) });

    $("label[for='"+lId+"']").css({ fontWeight: 'bold', fontSize: '14px' });
}
var gFirstClickBuscarRiscoPLD = true;

function GradIntra_Risco_PLD_Grid(pParametros) 
{

    if (gFirstClickBuscarRiscoPLD != true) {
        $("#tblBusca_Risco_PLD_Resultados")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarRiscoPLD = false;

    $("#tblBusca_Risco_PLD_Resultados").jqGrid(
    {
        url: "PLD.aspx?Acao=BuscarItensParaSelecao"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: ""                     , name: "Id"                   , jsonmap: "Id"                  , index: "Id"              , width: 30, align: "center", sortable: false }
                      , { label: ""                     , name: "Indice"               , jsonmap: "Indice"              , index: "Indice"              , width: 30, align: "center", sortable: false }
                      , { label: ""                     , name: "Semaforo"             , jsonmap: "Semaforo"            , index: "Semaforo"            , width: 40, align: "center", sortable: false }
                      , { label: "Criticidade"          , name: "CriticidadeOculta"    , jsonmap: "Criticidade"         , index: "Criticidade"         , width: 85, align: "center", sortable: false }
                      , { label: "Contraparte"          , name: "Contraparte"          , jsonmap: "Contraparte"         , index: "Contraparte"         , width: 75, align: "center", sortable: false }
                      , { label: "Criticidade"          , name: "Criticidade"          , jsonmap: "Criticidade"         , index: "Criticidade"         , width: 85, align: "center", sortable: false }
                      , { label: "Cliente"              , name: "CodigoCliente"        , jsonmap: "CodigoCliente"       , index: "CodigoCliente"       , width: 85, align: "center", sortable: false }
                      , { label: "Data/Hora Negócio"    , name: "HR_NEGOCIO"           , jsonmap: "HR_NEGOCIO"          , index: "HR_NEGOCIO"          , width: 115, align: "center",sortable: false }
                      , { label: "Intenção PLD"         , name: "IntencaoPLD"          , jsonmap: "IntencaoPLD"         , index: "IntencaoPLD"         , width: 65, align: "center", sortable: false }
                      , { label: "Intrumento"           , name: "Intrumento"           , jsonmap: "Intrumento"          , index: "Intrumento"          , width: 55, align: "center", sortable: false }
                      , { label: "Lucro Prejuiso"       , name: "LucroPrejuiso"        , jsonmap: "LucroPrejuiso"       , index: "LucroPrejuiso"       , width: 65, align: "right", sortable: false }
                      , { label: "Min. Restantes PLD"   , name: "MinutosRestantesPLD"  , jsonmap: "MinutosRestantesPLD" , index: "MinutosRestantesPLD" , width: 115, align: "center",sortable: false }
                      , { label: "Número Negócio"       , name: "NumeroNegocio"        , jsonmap: "NumeroNegocio"       , index: "NumeroNegocio"       , width: 90, align: "right", sortable: false }
                      , { label: "Preço Mercado"        , name: "PrecoMercado"         , jsonmap: "PrecoMercado"        , index: "PrecoMercado"        , width: 85, align: "right", sortable: false }
                      , { label: "Preço Negocio"        , name: "PrecoNegocio"         , jsonmap: "PrecoNegocio"        , index: "PrecoNegocio"        , width: 85, align: "right", sortable: false }
                      , { label: "Quantidade"           , name: "Quantidade"           , jsonmap: "Quantidade"          , index: "Quantidade"          , width: 55, align: "right", sortable: false }
                      , { label: "Sentido"              , name: "Sentido"              , jsonmap: "Sentido"             , index: "Sentido"             , width: 55, align: "center", sortable: false }
                      , { label: "STATUS"               , name: "STATUS"               , jsonmap: "STATUS"              , index: "STATUS"              , width: 75, align: "right", sortable: false }
                      ]
      , height: '100%'
      , width: 1205
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow: GradIntra_Risco_PLD_ItemDataBound
  }).jqGrid('hideCol', [ 'Id','CriticidadeOculta', 'Indice']); 
}


function GradIntra_Risco_PLD_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_Risco_PLD_Resultados tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.Criticidade == "PLDAPROVADO" || rowelem.Criticidade == "FOLGA")
    {
        lRow.children("td").eq(2).html("<span class=\"SemaforoVerdePLD\"> </span>");
    }
    else if (rowelem.Criticidade == "ALERTA" )
    {
        lRow.children("td").eq(2).html("<span class=\"SemaforoAmareloPLD\"> </span>");
    }
    else if (rowelem.Criticidade == "CRITICO")
    {
        lRow.children("td").eq(2).html("<span class=\"SemaforoVermelhoPLD\"> </span>");
    }

    if (rowelem.MinutosRestantesPLD.indexOf("-") > -1)
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.LucroPrejuiso.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}
gTimerPLDAtualizacaoAutomatica = new Number();

function chkRisco_PLD_AtualizarAutomaticamente_Click(pSender) 
{
    if ($(pSender).is(":checked")) 
    {
        gTimerPLDAtualizacaoAutomatica = 60;
        btnRisco_PLD_Busca_Click();
        //GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca();
    }
    else 
    {
        gTimerPLDAtualizacaoAutomatica = 0; //--> Zerando o contador.
        $("#lblRisco_PLD_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }

}

function GradIntra_Risco_Monitoramento_LucroPrejuizo_Detalhamento_Busca() 
{
    var lObjetoDeParametros = {
                                Acao: "BuscarOperacoesBovespa"
                              , CodigoCliente   : gCodigoBovespaDetalhes
                              , CodigoClienteBmf: gCodigoBMFDetalhes
    };

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bovespa(lObjetoDeParametros);

    var lObjetoDeParametrosBmf = { 
                                Acao: "BuscarOperacoesBmf"
                              , CodigoCliente: gCodigoBovespaDetalhes
                              , CodigoClienteBmf: gCodigoBMFDetalhes
    };

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bmf(lObjetoDeParametrosBmf);

    var lObjetoParametro = 
    {
         CodigoCliente      : gCodigoBovespaDetalhes
        , CodigoClienteBmf  : gCodigoBMFDetalhes
        , NomeCliente       : gNomeCliente
        , Assessor: gCodigoAssessor
        , NomeAssessor: gNomeAssessor

    };

    GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid(this, lObjetoParametro);

    //GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_Busca(null, lObjetoParametro);

    var lHorarioAtual = new Date();

    window.clearTimeout(gTimerResumoCliente);

    gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 0;

    gStatusPararContagemDetalhamento = true;

    gTimerResumoCliente = window.setTimeout(function () {
    
    
    var lUltimaAtualizacao = lHorarioAtual.getDate()
                             + "/" + (lHorarioAtual.getMonth() + 1)
                             + "/" + (lHorarioAtual.getFullYear())
                             + " " + (lHorarioAtual.getHours().toString().length == 1   ? "0" + lHorarioAtual.getHours().toString()   : lHorarioAtual.getHours().toString())
                             + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                             + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

        gStatusPararContagemDetalhamento = false;
        
        gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60; //--> Setando o contador.
        
        $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                           .effect("highlight", {}, 7000)
                                                           .prev().effect("highlight", {}, 7000);

        //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
        GradIntra_Risco_MonitoramentoDeRisco_Detalhes_AtualizarAutomaticamente();

    }, 700);
}

function btnRisco_Monitoramento_LucroPrejuizo_ColunasGrid_ExpanderColapse_Click(pSender) 
{
    if ($(pSender).hasClass('Risco_MonitoramentoLucroPrejuizo_ExpandirColapsar_ColunasGrid_Colapsado')) 
    {
        $(pSender).removeClass();

        $(pSender).addClass('Risco_MonitoramentoLucroPrejuizo_ExpandirColapsar_ColunasGrid_Expandido');
        
        $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid").show();
    }
    else 
    {
        $(pSender).removeClass();
        
        $(pSender).addClass('Risco_MonitoramentoLucroPrejuizo_ExpandirColapsar_ColunasGrid_Colapsado');
        
        $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid").hide();
    }
    return false;
}

function plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(pSender) 
{
    var lCheck = $(pSender);

    if (lCheck.is(':checked')) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados").jqGrid('showCol', [lCheck.val()]);
    }
    else 
    {

        $("#tblBusca_MonitoramentoRisco_Resultados").jqGrid('hideCol', [lCheck.val()]);
    }

    //return false;
}

function Risco_Monitoramento_LucroPrejuizo_Carregar_Janelas_Usuarios() 
{
    var lObjetoDeParametros = { Acao: "ListarJanelasParametros" };

    GradIntra_CarregarJsonVerificandoErro("Buscas/Risco_MonitoramentoRiscoGeral.aspx"
                                             , lObjetoDeParametros
                                             , function (pResposta) { Risco_Monitoramento_LucroPrejuizo_Carregar_Janelas_Usuarios_CallBack(pResposta); });

}

function Risco_Monitoramento_LucroPrejuizo_Carregar_Janelas_Usuarios_CallBack(pResposta) 
{
    if (!pResposta.TemErro) 
    {
        var lJanela = pResposta.ObjetoDeRetorno;

        $(lJanela).each(function () {
            var lObjJanela = this;

            $("#div_Risco_FooterLinkPaginas")
                .append("<button class=\"btnJanelaLink\" onclick=\"return btnRisco_Monitoramento_LucroPrejuizo_AbrirJanela(" + lObjJanela.IdJanela + ",'" + lObjJanela.NomeJanela + "');\" id=\"btnRisco_Abre_Janela_" + lObjJanela.IdJanela + "\">" + lObjJanela.NomeJanela + "</button><img id=\"lnk_Risco_exclui_Janela_" + lObjJanela.IdJanela + "\"  onclick=\"return btnRisco_Monitoramento_LucroPrejuizo_ExcluirJanela(" + lObjJanela.IdJanela + ", '" + lObjJanela.NomeJanela + "' );\" class=\"btnJanelaFechar\" style=\"border:none; cursor:hand;\" \>&nbsp;&nbsp;&nbsp;&nbsp;");
        });
    }
}


function btnRisco_Monitoramento_LucroPrejuizo_SalvarJanela_Click(pSender) 
{
    var lColunas = '';
    var lNomePagina;

    $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid [type=checkbox]")
        .each(function()
        {
            var lObject = $(this);

            if (!lObject.is(':checked')) 
            {
                lColunas += lObject.val() + "|";
            }
        });

        lColunas = lColunas.slice(0, lColunas.length-1);

        lNomePagina = $("#txtRisco_Monitoramento_LucrosPrejuizos_NomeJanela").val();

        if (lNomePagina.length < 3) {
            
            alert("É necessário inserir o nome da Janela.O nome da janela deve conter mais de 3 letras.");

            $("#txtRisco_Monitoramento_LucrosPrejuizos_NomeJanela").focus();

            return false ;
        }

        var lObjetoDeParametros = {   Acao: "SalvarParametros"
                                    , Colunas: lColunas
                                    , NomePagina: lNomePagina
                                };
    
    GradIntra_CarregarJsonVerificandoErro("MonitoramentoRiscoGeral.aspx"
                                             , lObjetoDeParametros
                                             , function (pResposta) { btnRisco_Monitoramento_LucroPrejuizo_SalvarJanela_Click_CallBack(pResposta); });
    return false;
}

function btnRisco_Monitoramento_LucroPrejuizo_SalvarJanela_Click_CallBack(pResposta) 
{
    if (!pResposta.TemErro)
    {
        var lObjJanela = pResposta.ObjetoDeRetorno.Objeto;

        Risco_Monitoramento_LucroPrejuizo_AdicionarJanelaNoDiv(lObjJanela)
    }
}

function Risco_Monitoramento_LucroPrejuizo_AdicionarJanelaNoDiv(pJanela) 
{
    $("#div_Risco_FooterLinkPaginas", window.opener.document)
    .append("<button class=\"btnJanelaLink\" onclick=\"return btnRisco_Monitoramento_LucroPrejuizo_AbrirJanela(" + pJanela.IdJanela + ",'" + pJanela.NomeJanela + "');\" id=\"btnRisco_Abre_Janela_" + pJanela.IdJanela + "\">" + pJanela.NomeJanela + "</buton>")
    .append("<img id=\"lnk_Risco_exclui_Janela_" + pJanela.IdJanela + "\"  onclick=\"return btnRisco_Monitoramento_LucroPrejuizo_ExcluirJanela(" + pJanela.IdJanela + ", '" + pJanela.NomeJanela + "' );\" class=\"btnJanelaFechar\"  style=\"border:none; cursor:hand;\" \>&nbsp;&nbsp;&nbsp;&nbsp;");

    $("#txtRisco_Monitoramento_LucrosPrejuizos_NomeJanela").val("");

    var lDivExpander = $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Expander")

    var lDiv = $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid")

    if (lDivExpander.hasClass('Risco_MonitoramentoLucroPrejuizo_ExpandirColapsar_ColunasGrid_Expandido')) 
    {
        lDivExpander.removeClass();

        lDivExpander.addClass('Risco_MonitoramentoLucroPrejuizo_ExpandirColapsar_ColunasGrid_Colapsado');

        lDiv.hide();
    }

    alert("Pagina salva com sucesso");
}

function btnRisco_Monitoramento_LucroPrejuizo_AbrirJanela(pIdJanela, pNomeJanela) 
{
    if (lPopUp == null) 
    {
        lPopUp = window.open('Risco/Formularios/Dados/MonitoramentoRiscoGeral.aspx?idJanela=' + pIdJanela, '', 'height=1000, width=1270,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

        //lPopUp.document.title = pNomeJanela;
    }

    if (lPopUp.GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca == null) 
    {
        setTimeout(function () { btnRisco_Monitoramento_LucroPrejuizo_AbrirJanela(pIdJanela, pNomeJanela) }, 2000);
    }
    else
    {
        var lObjetoDeParametros = { idJanela: pIdJanela };

        lPopUp.GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca(this, lObjetoDeParametros);

        lPopUp.document.title = pNomeJanela;
        
        lPopUp.GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas(this, pIdJanela);

        lPopUp.GradIntra_Risco_Monitoramento_LucroPrejuizo_SetaNomeJanela(pNomeJanela);

        lPopUp = null;
    }
    
    return false;
}
function GradIntra_Risco_Monitoramento_LucroPrejuizo_SetaNomeJanela(pNomeJanela) 
{
    $("#pnlNomePagina").html(pNomeJanela);
}

function GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas(pSender, pIdJanela) 
{
    var lParametros = { Acao: "SelecionaColunasInvisiveis",
        idJanela: pIdJanela
    };
    
    GradIntra_CarregarJsonVerificandoErro("MonitoramentoRiscoGeral.aspx"
                                , lParametros
                                , function (pResposta) { GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas_Callback(pResposta); });
    

    return false;
}

function GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas_Callback(pResposta) 
{
    if (!pResposta.TemErro && pResposta.ObjetoDeRetorno.Colunas !="") 
    {
        var lHideColuns = pResposta.ObjetoDeRetorno.Colunas.split("|");

        var lColunas = pResposta.ObjetoDeRetorno.Colunas;

        $("#tblBusca_MonitoramentoRisco_Resultados").jqGrid('hideCol', lHideColuns);

        $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid [type=checkbox]").each(function () {

            if (lColunas.indexOf($(this).val()) != -1) 
            {
                $(this).prop("checked", false);
            }

        });
    }
}

function btnRisco_Monitoramento_LucroPrejuizo_ExcluirJanela(pIdJanela, nmJanela) 
{
    if (confirm("Deseja excluir a janela " + nmJanela + "?")) 
    {
        var lParametros = { Acao: "ExluirJanela",
                            idJanela: pIdJanela
                        };

                        GradIntra_CarregarJsonVerificandoErro("Risco/Formularios/Dados/MonitoramentoRiscoGeral.aspx"
                                , lParametros
                                , function (pResposta) { btnRisco_Monitoramento_LucroPrejuizo_ExcluirJanela_CallBack(pResposta); });
    }
}

function btnRisco_Monitoramento_LucroPrejuizo_ExcluirJanela_CallBack(pResposta)
{
    if (!pResposta.TemErro) 
    {
        var lIdJanela = pResposta.ObjetoDeRetorno.Objeto.IdJanela;

        $("#div_Risco_FooterLinkPaginas").find("#btnRisco_Abre_Janela_" + lIdJanela).remove();

        $("#div_Risco_FooterLinkPaginas").find("#lnk_Risco_exclui_Janela_" + lIdJanela).remove();

        alert("Janela excluída com sucesso!")
    }
    else 
    {
        alert(pResposta);
    }
}

function plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(pSender) 
{
    var lCheck = $(pSender);

    if (lCheck.is(':checked')) 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_detalhes").jqGrid('showCol', [lCheck.val()]);
    }
    else 
    {
        $("#tblBusca_MonitoramentoRisco_Resultados_detalhes").jqGrid('hideCol', [lCheck.val()]);
    }

    //return false;
}

function btnRisco_Monitoramento_LucroPrejuizo_ColunasGrid_Detalhes_ExpanderColapse_Click(pSender) 
{
    if ($(pSender).hasClass('Risco_MonitoramentoLucroPrejuizo_Detalhes_ExpandirColapsar_ColunasGrid_Colapsado')) {
        $(pSender).removeClass();

        $(pSender).addClass('Risco_MonitoramentoLucroPrejuizo_Detalhes_ExpandirColapsar_ColunasGrid_Expandido');

        $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Detalhes").show();
    }
    else {
        $(pSender).removeClass();

        $(pSender).addClass('Risco_MonitoramentoLucroPrejuizo_Detalhes_ExpandirColapsar_ColunasGrid_Colapsado');

        $("#pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Detalhes").hide();
    }
    return false;
}

function GradIntra_Monitoramento_LucroPrejuizo_Gerais_Load() 
{
    var lObjetoDeParametros = { Acao: "BuscarAssessorConectado" };

    GradIntra_CarregarJsonVerificandoErro("Buscas/Risco_MonitoramentoRiscoGeral.aspx"
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Monitoramento_LucroPrejuizo_Gerais_Load_Callback(pResposta); }
                                         );
}

function GradIntra_Monitoramento_LucroPrejuizo_Gerais_Load_Callback(pResposta) 
{
    if (!pResposta.TemErro)
    {
        var lIdAssessorLogado = pResposta.ObjetoDeRetorno;// $("#hddIdAssessorLogado").val();

        if (lIdAssessorLogado && "" != lIdAssessorLogado) 
        {
            var lComboBusca_FiltroRelatorioRisco_Assessor = $("#cboBM_FiltroRelatorio_CodAssessor");

            lComboBusca_FiltroRelatorioRisco_Assessor.prop("disabled", true);

            lComboBusca_FiltroRelatorioRisco_Assessor.val(lIdAssessorLogado);
        }
    }
}

function GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca(pSender)
{
    $(pSender).one('keypress', function(e)
    {
        if (e.keyCode == 13) 
        {

            var lObjetoDeParametros = { Acao            : "BuscarDadosCliente",
                                        CodigoCliente   : $("#txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente").val()
                                      };

                                    GradIntra_CarregarJsonVerificandoErro("MonitoramentoRiscoGeralDetalhamento.aspx"
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Callback(pResposta); }
                                         );
            
//            try 
//            {
//                event.cancelBubble = true;
//            }
//            catch (Erro) { }

            return false;
        }
    });

    //$(pSender).unbind('keypress');
}

function GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid(pObjeto, pParametro) 
{
    var codigo = pParametro.CodigoCliente;

    var codigoBmf = pParametro.CodigoClienteBmf;

    var lAssessor = gCodigoAssessor = pParametro.Assessor;

    var lNomeCliente = gNomeCliente = pParametro.NomeCliente;

    var lNomeAssessor = gNomeAssessor =pParametro.NomeAssessor;

    var lObjetoDeParametros = { Acao         : "BuscarDadosCliente",
                                CodigoCliente: codigo
                              };

    GradIntra_CarregarJsonVerificandoErro("MonitoramentoRiscoGeralDetalhamento.aspx"
                                         , lObjetoDeParametros
                                         , function (pResposta) { GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid_Callback(pResposta, pParametro, pObjeto); }
                                         );
}
function GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid_Callback(pResposta, pParametro, pObjeto)
{
    GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_Busca(pObjeto, pParametro)

    var lHorarioAtual = new Date();

    window.clearTimeout(gTimerResumoCliente);

    gStatusPararContagemDetalhamento = true;

    gTimerResumoCliente = window.setTimeout(function () {
        var lUltimaAtualizacao = lHorarioAtual.getDate()
                             + "/" + (lHorarioAtual.getMonth() + 1)
                             + "/" + (lHorarioAtual.getFullYear())
                             + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                             + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                             + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

        gStatusPararContagemDetalhamento = false;

        gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60; //--> Setando o contador.

        $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                           .effect("highlight", {}, 7000)
                                                           .prev().effect("highlight", {}, 7000);

        //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
        GradIntra_Risco_MonitoramentoDeRisco_Detalhes_AtualizarAutomaticamente();
    }, 700);        
}

function GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Callback(pResposta) 
{
    if (pResposta.TemErro)
    {
        GradIntra_TratarRespostaComErro(pResposta);
    }
    else {

        if (pResposta.ObjetoDeRetorno == null) 
        {
            alert("Cliente não encontrado!");

            $("#txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente").val("");

            $("#txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente").focus();

            return;
        }

        var lCodigoBmf = gCodigoBMFDetalhes = pResposta.ObjetoDeRetorno.MonitorCustodia.CodigoClienteBmf;

        var lCodigoBovespa = gCodigoBovespaDetalhes = pResposta.ObjetoDeRetorno.MonitorCustodia.CodigoClienteBov;

        var lNomeCliente   = gNomeCliente = pResposta.ObjetoDeRetorno.MonitorCustodia.NomeCliente;

        var lAssessor = gCodigoAssessor = pResposta.ObjetoDeRetorno.MonitorCustodia.CodigoAssessor;

        var lNomeAssessor = gNomeAssessor = pResposta.ObjetoDeRetorno.MonitorCustodia.NomeAssessor;

        var lStatusBovespa = gStatusBovespa  = pResposta.ObjetoDeRetorno.MonitorCustodia.StatusBovespa;

        var lStatusBmf =gStatusBmf =  pResposta.ObjetoDeRetorno.MonitorCustodia.StatusBMF;

        if (lCodigoBovespa == undefined && lCodigoBmf == undefined) 
        {
            alert("Cliente não encontrado!");

            $("#txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente").val("");

            $("#txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente").focus();

            return ;
        }


        $("#DetalhesHeader_Cliente").html();

        var data = new Date();

        $("#DetalhesHeader_Cliente").html("Cliente: "       + lNomeCliente                                      + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                          "Assessor:"       + lAssessor + " - " + lNomeAssessor                 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                          "Conta Cliente: "   + pResposta.ObjetoDeRetorno.DigitoCodigoCliente   + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                          "Data Atual:"     + data.getDate() + "/" + (data.getMonth() + 1) + "/" + data.getFullYear() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                          "Status Bovespa: "+ (lStatusBovespa == "A" ? "Ativo" : "Inativo") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                          "Status BMF: "    + (lStatusBmf == "A" ? "Ativo" : "Inativo") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

        var lObjetoDeParametros = { CodigoCliente: lCodigoBovespa, CodigoClienteBmf: lCodigoBmf, Assessor: lAssessor, NomeCliente: lNomeCliente };

        //GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamento_Busca(this, lObjetoDeParametros);
        
        GradIntra_Risco_Monitoramento_LucroPrejuizo_Detalhamento_Busca();

        //GradIntra_Risco_Monitoramento_LucroPrejuizo_Busca();
    }

}

var gTimerResumoCliente;

function GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca(pObjeto, pParametro) 
{
    var codigo = pParametro.CodigoCliente;

    var codigoBmf = pParametro.CodigoClienteBmf;

    var lAssessor = gCodigoAssessor = pParametro.Assessor;

    var lNomeCliente = gNomeCliente = pParametro.NomeCliente;

    var lNomeAssessor = gNomeAssessor = pParametro.NomeAssessor;

    var lObjetoDeParametros = { Acao: "BuscarDetalhamentoBovespa"
                              , CodigoCliente: codigo
                              , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bovespa(lObjetoDeParametros);

    var lObjetoDeParametrosBmf = { Acao: "BuscarDetalhamentoOperacoesBmf"
                              , CodigoCliente: codigo
                              , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_ConfigurarGridDeResultados_MonitoramentoRisco_Operacoes_Bmf(lObjetoDeParametrosBmf);

    var lBuscaSemCarteira = gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira ? "BuscarCustodiaSemCarteira" : "BuscarCustodia";

    var lObjetoDeCustodiaVista = { Acao             : lBuscaSemCarteira
                                 , CodigoCliente    : codigo
                                 , Mercado          : "VIS"
                                 , CodigoClienteBmf : codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Avista(lObjetoDeCustodiaVista);

    var lObjetoDeCustodiaOpcoes = { Acao            : lBuscaSemCarteira
                                  , CodigoCliente   : codigo
                                  , Mercado         : "OPC"
                                  , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Opcoes(lObjetoDeCustodiaOpcoes);

//    var lObjetoDeCustodiaTermo = { Acao: "BuscarCustodia"
//                                 , CodigoCliente: codigo
//                                 , Mercado: "TER"
//                                 , CodigoClienteBmf: codigoBmf
//    };

//    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Termo(lObjetoDeCustodiaTermo);

    var lObjetoDeCustodiaBmf = { Acao: "BuscarCustodia"
                               , CodigoCliente: codigo
                               , Mercado: "FUT"
                               , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Bmf(lObjetoDeCustodiaBmf);

    var lObjetoDeCustodiaBmf = { Acao             : "BuscarCustodiaPosicao"
                               , CodigoCliente    : codigo
                               , Mercado          : "FUT"
                               , CodigoClienteBmf : codigoBmf

    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Posicao_Bmf(lObjetoDeCustodiaBmf);

    var lObjetoDeCustodiaTedi = { Acao: "BuscarCustodia"
                               , CodigoCliente: codigo
                               , Mercado: "TEDI"
                               , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Tesouro(lObjetoDeCustodiaTedi);

//    var lObjetoDeCustodiaBTC = {
//                                Acao: "BuscarCustodia"
//                               , CodigoCliente: codigo
//                               , Mercado: "BTC"
//                               , CodigoClienteBmf: codigoBmf
//    };

//    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_BTC(lObjetoDeCustodiaBTC);

    var lObjetoDeCustodiaFundos = {
                                Acao: "BuscarCustodiaFundos"
                               , CodigoCliente: codigo
                               , Mercado: "FUN"
                               , CodigoClienteBmf: codigoBmf
    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Fundos(lObjetoDeCustodiaFundos);

    var lObjetoDeCustodiaOperacoesTermo = {
                                Acao: "BuscarCustodiaOperacoesTermo"
                               , CodigoCliente: codigo
                               , Mercado: "TER"
                               , CodigoClienteBmf: codigoBmf

    };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_Termo(lObjetoDeCustodiaOperacoesTermo);

    var lObjetoDeCustodiaOperacoesBTC = {
                                Acao: "BuscarCustodiaOperacoesBTC"
                               , CodigoCliente: codigo
                               , Mercado: "BTC"
                               , CodigoClienteBmf: codigoBmf
                               };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_Operacoes_BTC(lObjetoDeCustodiaOperacoesBTC);

    var lObjetoDeCustodiaRendaFixa = {
                                Acao: "BuscarCustodiaRendaFixa"
                               , CodigoCliente: codigo
                               , Mercado: "FIX"
                               , CodigoClienteBmf: codigoBmf
                               };

    GradIntra_Monitoramento_MonitoramentoRisco_Custodia_RendaFixa(lObjetoDeCustodiaRendaFixa);

    GradIntra_MonitoramentoRisco_BuscarSaldoLimite_ContaCorrente(codigo, codigoBmf, lAssessor, lNomeAssessor, lNomeCliente);

    var lHorarioAtual = new Date();
    
    window.clearTimeout(gTimerResumoCliente);

    gStatusPararContagemDetalhamento = true;

    gTimerResumoCliente = window.setTimeout(function () {
                            var lUltimaAtualizacao = lHorarioAtual.getDate()
                             + "/" + (lHorarioAtual.getMonth() + 1)
                             + "/" + (lHorarioAtual.getFullYear())
                             + " " + (lHorarioAtual.getHours().toString().length == 1   ? "0" + lHorarioAtual.getHours().toString()   : lHorarioAtual.getHours().toString())
                             + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                             + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

        gStatusPararContagemDetalhamento = false;

        gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60; //--> Setando o contador.

        $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                           .effect("highlight", {}, 7000)
                                                           .prev().effect("highlight", {}, 7000);

        //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
        GradIntra_Risco_MonitoramentoDeRisco_Detalhes_AtualizarAutomaticamente();
    }, 700);
}

//*****************************************************************//
// Risco - Suitability Lavagem*************************************//
//*****************************************************************//
var gStatusPararContagemSuitabilityLavagem = false;

var gTimerSuitabilityLavagemAtualizacaoAutomatica = 0;

var gFirstClickBuscarRiscoSuitabilityLavagem = true;

function GradIntra_Risco_Suitability_Lavagem_Buscar(pSender, pConsulta) 
{
    gStatusPararContagemSuitabilityLavagem = true;

    gTimerSuitabilityLavagemAtualizacaoAutomatica = 0; //--> Zerando o contador.

    $("#div_Risco_SuitabilityLavagem_Resultados").show();

    $("#tblBusca_SuitabilityLavagem_Resultados").fadeIn();

    GradIntra_Risco_Suitability_Lavagem(pConsulta);

    var lHorarioAtual = new Date();

    window.setTimeout(function () 
    {
        var lUltimaAtualizacao = lHorarioAtual.getDate()
                                 + "/" + (lHorarioAtual.getMonth() + 1)
                                 + "/" + (lHorarioAtual.getFullYear())
                                 + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                                 + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                                 + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

        gStatusPararContagemSuitabilityLavagem = false;

        gTimerSuitabilityLavagemAtualizacaoAutomatica = 60; //--> Setando o contador.

        $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                               .effect("highlight", {}, 7000)
                                                               .prev().effect("highlight", {}, 7000);


        //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
        GradIntra_Risco_Suitability_Lavagem_AtualizarAutomaticamente();
    }, 700);
}
var gParametrosSuitabilityLavagem = null; 
function GradIntra_Risco_Suitability_Lavagem(pParametros) 
{
    if (gFirstClickBuscarRiscoSuitabilityLavagem != true) 
    {
        $("#tblBusca_SuitabilityLavagem_Resultados")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gParametrosSuitabilityLavagem = pParametros;

    gFirstClickBuscarRiscoSuitabilityLavagem = false;

    $("#tblBusca_SuitabilityLavagem_Resultados").jqGrid(
    {
        url:            "SuitabilityLavagem.aspx?Acao=BuscarSuitabilityLavagem"
      , datatype:       "json"
      , mtype:          "POST"
      , hoverrows:      false
      , postData:       pParametros
      , autowidth:      false
      , shrinkToFit:    false
      , colModel: [
                         { label: "",                   name: "",                       jsonmap: "check",                   index: "check",                 width: 30, align: "center", sortable: true }
                       , { label: "Cliente",            name: "CodigoCliente",          jsonmap: "CodigoCliente",           index: "CodigoCliente",         width: 50, align: "center", sortable: true }
                       , { label: "Nome Cliente",       name: "NomeCliente",            jsonmap: "NomeCliente",             index: "NomeCliente",           width: 250, align: "left",  sortable: true }
                       , { label: "Assessor",           name: "CodigoAssessor",         jsonmap: "CodigoAssessor",          index: "CodigoAssessor",        width: 30, align: "center", sortable: true }
                       , { label: "Nome Assessor",      name: "NomeAssessor",           jsonmap: "NomeAssessor",            index: "NomeAssessor",          width: 250, align: "left",  sortable: true }
                       , { label: "Volume",             name: "Volume",                 jsonmap: "Volume",                  index: "Volume",                width: 70, align: "center", sortable: true }
                       , { label: "SFP",                name: "SFP",                    jsonmap: "SFP",                     index: "SFP",                   width: 70, align: "center", sortable: true }
                       , { label: "% Vol./ SFP",        name: "PercentualVOLxSFP",      jsonmap: "PercentualVOLxSFP",       index: "PercentualVOLxSFP",     width: 70, align: "center", sortable: true }
                       , { label: "Suitability",        name: "Suitability",            jsonmap: "Suitability",             index: "Suitability",           width: 90, align: "center", sortable: true }
                       , { label: "ArquivoCiencia",     name: "ArquivoCiencia",         jsonmap: "ArquivoCienciaLink",      index: "ArquivoCiencia",        width: 90, align: "center", sortable: true, formatter:FormatarArquivoCiencia }
                       , { label: "Codigo Bmf",         name: "CodigoClienteBmf",       jsonmap: "CodigoClienteBmf",        index: "CodigoClienteBmf",      width: 90, align: "center", sortable: true }
                       
                    ]
      , height:         600
      , width:          1250
      , rowNum:         0
      , sortname:       "invid"
      , sortorder:      "desc"
      , viewrecords:    true
      , gridview:       false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid:        false
      , jsonReader: {
          root: "Itens"
                       , page:      "PaginaAtual"
                       , total:     "TotalDePaginas"
                       , records:   "TotalDeItens"
                       , cell:      ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id:        "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Risco_Suitability_Lavagem_ItemDataBound
  }).jqGrid('hideCol', ['check']);
    //.jqGrid('hideCol', ['Id', 'Sentido']); ;
}

//ver aqui: http://www.trirand.com/jqgridwiki/doku.php?id=wiki:custom_formatter
function FormatarArquivoCiencia(pValor, pOptions, pRowObject)
{
    return pValor.replace("~/", "../../../../");  //retorna o link que já veio pronto
}

function FormatarTeste(pValor, pOptions, pRowObject)
{
    return "<a href='/Intranet/" + pValor + "'>" + pValor + "</a>";
}

function GradIntra_Risco_Suitability_Lavagem_ItemDataBound(rowid, rowdata, rowelem)
{
    var lRow = $("#tblBusca_SuitabilityLavagem_Resultados tr[id=" + rowid + "]");

    lRow.css("height", 25);

    lRow.css("background-color", 'white');
    lRow.children("td").eq(0).html("<img alt=\"\" style=\"cursor: pointer\" onclick=\"return GradIntra_Risco_SuitabilityLavagem_AbrirDetalhamento_click(this)\" src=\"../../../../Skin/Default/Img/Ico_Busca.png\" />");

}

function GradIntra_Risco_Suitability_Lavagem_AtualizarAutomaticamente() 
{
    if (!gStatusPararContagemSuitabilityLavagem && $("#chkRisco_Suitability_Lavagem_AtualizarAutomaticamente").is(':checked')) 
    {
        $("#lblRisco_Suitability_Lavagem_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerSuitabilityLavagemAtualizacaoAutomatica - 1) >= 0) 
        {
            gTimerSuitabilityLavagemAtualizacaoAutomatica = gTimerSuitabilityLavagemAtualizacaoAutomatica - 1;
            $("#lblRisco_Suitability_Lavagem_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerSuitabilityLavagemAtualizacaoAutomatica + ' segundos.')
            setTimeout('GradIntra_Risco_Suitability_Lavagem_AtualizarAutomaticamente()', 1000);
        }
        else 
        {
            GradIntra_Risco_Suitability_Lavagem_Buscar();
        }
    }

    return false;
}

function chkRisco_Suitability_Lavagem_AtualizarAutomaticamente_Click(pSender) 
{
    if ($(pSender).is(":checked")) 
    {

//        if (gCodigoBovespaDetalhes == null && gCodigoBMFDetalhes == null)
//        {
//            return;    
//        }
        gTimerSuitabilityLavagemAtualizacaoAutomatica = 60;

        GradIntra_Risco_Suitability_Lavagem_Buscar();
    }
    else 
    {
        //window.setTimeout = null;
                
        gTimerSuitabilityLavagemAtualizacaoAutomatica = 0; //--> Zerando o contador.
        
        $("#lblRisco_Suitability_Lavagem_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}

function GradIntra_Risco_SuitabilityLavagem_AbrirDetalhamento_click(pSender) 
{
    var codigo = $(pSender).parent("td").parent("tr").find("td").eq(1).html(); //--> Código Bovespa do cliente

    var codigoBmf = $(pSender).parent("td").parent("tr").find("td").eq(9).html(); //--> Código bmf do cliente

    var lAssessor = $(pSender).parent("td").parent("tr").find("td").eq(3).html(); //--> Assessor do cliente

    var lNomeCliente = $(pSender).parent("td").parent("tr").find("td").eq(2).html(); //--> Nome do cliente

    var lNomeAssessor = $(pSender).parent("td").parent("tr").find("td").eq(4).html(); //--> Nome do assessor

    if (lPopUpDetalhamento == null) 
    {
        lPopUpDetalhamento = window.open('../../../Risco/Formularios/Dados/MonitoramentoRiscoGeralDetalhamento.aspx', codigo + ' - ' + lNomeCliente, 'height=800, width=1200,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

        lPopUpDetalhamento.gCodigoBovespaDetalhes = codigo;

        lPopUpDetalhamento.gCodigoBMFDetalhes = codigoBmf;

        lPopUpDetalhamento.gCodigoAssessor = lAssessor;

        lPopUpDetalhamento.gNomeCliente = lNomeCliente;

        lPopUpDetalhamento.gNomeAssessor = lNomeAssessor;
    }

    if (lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca == null) 
    {
        setTimeout(function () { GradIntra_Risco_SuitabilityLavagem_AbrirDetalhamento_click(pSender) }, 1000);
    }
    else 
    {
        var lObjetoDeParametros = { CodigoCliente: codigo, CodigoClienteBmf: codigoBmf, Assessor: lAssessor, NomeCliente: lNomeCliente, NomeAssessor: lNomeAssessor };

        lPopUpDetalhamento.gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60;

        lPopUpDetalhamento.GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid(this, lObjetoDeParametros);

        ////lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca(this, lObjetoDeParametros);
        //lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas(this, pIdJanela);

        lPopUpDetalhamento = null;
    }

    return false;
}

function GradIntra_Risco_SuitabilityLavagem_ExportarParaExcel()
{
    gStatusPararContagem = true;
    gTimerMonitoramentoAtualizacaoAutomatica = 0; //--> Zerando o contador.

    var lUrl = "SuitabilityLavagem.aspx";

    lUrl += "?Acao=ExportarParaExcel";

    lUrl += "&CodigoCliente="       + gParametrosSuitabilityLavagem.CodigoCliente     ;
         +  "&Enquadrado="          + gParametrosSuitabilityLavagem.Enquadrado        ;
         +  "&DataDe="              + gParametrosSuitabilityLavagem.DataDe            ;
         +  "&DataAte="             + gParametrosSuitabilityLavagem.DataAte           ;
         +  "&Volume="              + gParametrosSuitabilityLavagem.Volume            ;
         +  "&PercentualVOLxSFP="   + gParametrosSuitabilityLavagem.PercentualVOLxSFP ;
         +  "&CodAssessor="         + gParametrosSuitabilityLavagem.CodAssessor       ;

    window.open(lUrl);
}


function btnRisco_MonitoramentoCustodia_Click() 
{

    gStatusPLDPararContagem = true;
    var lUrl = "MonitoramentoCustodia.aspx";

    var lDados = { Acao: "BuscarItensParaSelecao"
                 , CodigoCliente: $("#txtCliente").val()
                 , CodigoAtivo: $("#txtAtivo").val()
                 , CodigoMercado: $("#cboTipoMercado").val()
                 , Descobertos: $("#chkDescoberto").is(':checked')
                 , Nova: true
    };

    $("#div_Risco_MonitoramentoCustodia_Resultados").show();
    
    GradIntra_Risco_MonitoramentoCustodia_Grid(lDados);

    return false;
}

var gFirstClickBuscarRiscoMonitoramentoCustodia = true;

function GradIntra_Risco_MonitoramentoCustodia_Grid(pParametros) 
{

    if (gFirstClickBuscarRiscoPLD != true) {
        $("#tblBusca_MonitoramentoCustodia")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }


    gFirstClickBuscarRiscoPLD = false;

    $("#tblBusca_MonitoramentoCustodia").jqGrid(
    {
      url:            "MonitoramentoCustodia.aspx?Acao=Paginar"
      , datatype:       "json"
      , mtype:          "POST"
      , hoverrows:      false
      , postData:       pParametros
      , autowidth:      false
      , shrinkToFit:    false
      , colModel: [
                        { label: ""                 , name: "Id"                    , jsonmap: "Id"                     , index: "Id"                       , width: 50     , align: "center"   , sortable: false }
                      , { label: "Cliente"          , name: "CodigoCliente"         , jsonmap: "CodigoCliente"          , index: "CodigoCliente"            , width: 65     , align: "center"   , sortable: false }
                      , { label: "Ativo"            , name: "CodigoAtivo"           , jsonmap: "CodigoAtivo"            , index: "CodigoAtivo"              , width: 65     , align: "center"   , sortable: false }
                      , { label: "Mercado"          , name: "CodigoMercado"         , jsonmap: "CodigoMercado"          , index: "CodigoMercado"            , width: 40     , align: "center"   , sortable: false }
                      , { label: "Carteira"         , name: "CodigoCarteira"        , jsonmap: "CodigoCarteira"         , index: "CodigoCarteira"           , width: 65     , align: "center"   , sortable: false }
                      , { label: "Qtde. Disp."      , name: "QuantidadeDisponivel"  , jsonmap: "QuantidadeDisponivel"   , index: "QuantidadeDisponivel"     , width: 75     , align: "center"   , sortable: false }
                      , { label: "Qtde. D1"         , name: "QuantidadeD1"          , jsonmap: "QuantidadeD1"           , index: "QuantidadeD1"             , width: 75     , align: "center"   , sortable: false }
                      , { label: "Qtde. D2"         , name: "QuantidadeD2"          , jsonmap: "QuantidadeD2"           , index: "QuantidadeD2"             , width: 75     , align: "center"   , sortable: false }
                      , { label: "Qtde. D3"         , name: "QuantidadeD3"          , jsonmap: "QuantidadeD3"           , index: "QuantidadeD3"             , width: 75     , align: "center"   , sortable: false }
                      , { label: "Qtde. Pend."      , name: "QuantidadePendente"    , jsonmap: "QuantidadePendente"     , index: "QuantidadePendente"       , width: 75     , align: "center"   , sortable: false }
                      , { label: "Qtde. à Liq."     , name: "QuantidadeALiquidar"   , jsonmap: "QuantidadeALiquidar"    , index: "QuantidadeALiquidar"      , width: 75     , align: "center"   , sortable: false }
                      , { label: "Qtde. Total"      , name: "QuantidadeTotal"       , jsonmap: "QuantidadeTotal"        , index: "QuantidadeTotal"          , width: 90     , align: "right"    , sortable: false }
                      ]
      , height:         400
      , width:          850
      , rowNum        : 50
      , rowList       : [50, 100, 150, 200]
      , pager         : jQuery('#pager')
      , viewrecords:    true
      , gridview:       false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid:        false
      , jsonReader: {
                         root:        "Itens"
                       , page:        "PaginaAtual"
                       , total:       "TotalDePaginas"
                       , records:     "TotalDeItens"
                       , cell:        ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id:          "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_MonitoramentoCustodia_ItemDataBound
      , gridComplete: GridComplete
  }).jqGrid('hideCol', [ 'Id','CriticidadeOculta', 'Indice']);
}

function GridComplete()
{
    var lDados = { Acao: "BuscarItensParaSelecao"
                 , CodigoCliente: $("#txtCliente").val()
                 , CodigoAtivo: $("#txtAtivo").val()
                 , CodigoMercado: $("#cboTipoMercado").val()
                 , Descobertos: $("#chkDescoberto").is(':checked')
                 , Nova: false

    };

        $("#tblBusca_MonitoramentoCustodia")
            .setGridParam({ postData: lDados })

}

function MostraErro(jqXHR, textStatus, errorThrown) 
{
        alert('HTTP status code: ' + jqXHR.status + '\n' + 'textStatus: ' + textStatus + '\n' + 'errorThrown: ' + errorThrown);
        alert('HTTP message body (jqXHR.responseText): ' + '\n' + jqXHR.responseText);
}

function GradIntra_MonitoramentoCustodia_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_MonitoramentoCustodia tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');
    
    if (rowelem.QuantidadeDisponivel.indexOf("-") > -1) 
    {
        lRow.children("td").eq(5).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(5).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QuantidadeD1.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'bold', fontSize: '12px', color: 'red' })
    } 
    else
    {
        lRow.children("td").eq(6).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QuantidadeD2.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(7).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QuantidadeD3.indexOf("-") > -1) 
    {
        lRow.children("td").eq(8).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(8).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }
    
    if (rowelem.QuantidadePendente.indexOf("-") > -1) 
    {
        lRow.children("td").eq(9).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(9).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QuantidadeALiquidar.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(10).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }

    if (rowelem.QuantidadeTotal.indexOf("-") > -1) 
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
    else
    {
        lRow.children("td").eq(11).css({ fontWeight: 'bold', fontSize: '12px', color: 'black' })
    }
}



function chkRisco_Monitoramento_LP_Carregar_SemCarteira_Click(pSender)
{
    gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira = !gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira;

    if (gCodigoBovespaDetalhes == null && gCodigoBMFDetalhes == null)
    {
        return;    
    }

    GradIntra_Risco_Monitoramento_LucroPrejuizo_Detalhamento_Busca();
    /*if (gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira)
    {
        gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira = false;
    }else
    {
        gMonitoramento_Risco_LP_LOad_Custodia_SemCarteira = true
    }*/
}


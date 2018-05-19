
var gGridFundos;
var gMock_CadastroFundosGrid = {};

$(document).ready(function () {

    if ($('#AlteracaoRegulamentoConsultaFundos_pnlFiltroPesquisa')[0] != undefined) {

        AltRequerimentofnCarregarDadosIniciaisFundosConstituicao();
    }
});

function AltRequerimentofnBuscarDadosAlteracaoRegulamentoConsultaFundos()
{
    AltRequerimentoGrid_ResultadoAlteracaoRegulamentoConsultaFundos();

    AltRequerimentoFiltroCadastroFundosEmConstituicao();
}

function AltRequerimentofnCarregarDadosIniciaisFundosConstituicao() {
    
    AltRequerimentofnCarregarListaFundosEmConstituicao();
    AltRequerimentofnCarregarListaGruposAprovacaoFundosEmConstituicao();

    $('#txtDataDe').datepicker();
    $('#txtDataAte').datepicker();

    $('#imgDtDe').click(function() {
        $("#txtDataDe").focus();
    });

    $('#imgDtAte').click(function() {
        $("#txtDataAte").focus();
    });

    //configuração regional do datepicker
    $.datepicker.regional['pt-BR'] = {
        closeText: 'Fechar',
        prevText: '&#x3c;Anterior',
        nextText: 'Pr&oacute;ximo&#x3e;',
        currentText: 'Hoje',
        monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
        'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);

    $("#txtDataDe").blur(function () {
        fnValidarDataFundoConstituicao(this);
    });
    $("#txtDataAte").blur(function () {
        fnValidarDataFundoConstituicao(this);
    });
}

//função que busca dados do grid de cadastro de fundos
function AltRequerimentoFiltroCadastroFundosEmConstituicao() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AlteracaoRegulamentoConsultaFundos.aspx");

    var lDados =
        {
            Concluidos: $('#chkFundosConcluidos').prop("checked"),
            Pendentes: $('#chkFundosPendentes').prop("checked"),
            DtDe: $('#txtDataDe').val(),
            DtAte: $('#txtDataAte').val(),
            Acao: "CarregarGridFundosConstituicao",
            IdFundoCadastro: $('#listaFundosConstituicao').val(),
            IdFundoFluxoGrupo: $('#listaGruposFundosConstituicao').val()
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, AltRequerimentoFiltroCadastroFundosEmConstituicao_CallBack);

    return false;
}

//função de retorno do carregamento do grid de cadastro de fundos
function AltRequerimentoFiltroCadastroFundosEmConstituicao_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {

        $("#gridConsultaFundos").jqGrid("clearGridData", true);

        gMock_CadastroFundosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        NomeFundo: lData.Itens[i].NomeFundo,
                        grupo: lData.Itens[i].Grupo,
                        etapa: lData.Itens[i].Etapa,
                        statusEtapa: lData.Itens[i].StatusEtapa,
                        statusGeral: lData.Itens[i].StatusGeral
                    };

                gMock_CadastroFundosGrid.rows.push(lObjeto);
            }
        }

        $('#gridConsultaFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CadastroFundosGrid.rows })
            .trigger("reloadGrid");

        $('.ui-pg-input').width(20);
    }
    else {
        bootbox.alert(lData.Mensagem);
    }
}

//função que monta o grid
function AltRequerimentoGrid_ResultadoAlteracaoRegulamentoConsultaFundos() {

    gGridFundos =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CadastroFundosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true }
                    //, { label: "Fundo", name: "NomeFundo", jsonmap: "NomeFundo", index: "NomeFundo", width: 110, align: "left", sortable: true }
                    , { name: 'Fundo', index: 'NomeFundo', jsonmap: "NomeFundo", formatter: AltRequerimentolinkFluxoAprovacaoFormatter, sortable: true }
                    , { label: "Grupo", name: "grupo", jsonmap: "grupo", index: "grupo", align: "left", width: 90, sortable: true }
                    , { label: "Última etapa processada", name: "etapa", jsonmap: "etapa", index: "etapa", align: "left", sortable: true }
                    , { label: "Status etapa", name: "statusEtapa", jsonmap: "statusEtapa", index: "statusEtapa", width: 40, align: "left", sortable: true }
                    , { label: "Status geral", name: "statusGeral", jsonmap: "statusGeral", index: "statusGeral", width: 40, align: "left", sortable: true }
                    , { name: '', index: 'compartilhar', width: 12, formatter: AltRequerimentocompartilharFundoConstituicaoFormatter, sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortorder: "asc"
      , viewrecords: true
      , gridview: false
      , pager: '#pgGridConsultaFundos'
      , subGrid: false
      , rowNum: 20
      , rowList: [20,30,40,50,60]
      , caption: 'Fundos'      
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
    };

    $('#gridConsultaFundos').jqGrid(gGridFundos);

    $('#gridCadastroFundos').jqGrid('setGridWidth', 1200);
}

function AltRequerimentocompartilharFundoConstituicaoFormatter(cellvalue, options) {
    return '<span class="glyphicon glyphicon-envelope" onclick="AltRequerimentofnExibirModalEnvioEmailConsultaFundos(' + options.rowId + ')"></span>';
}

function AltRequerimentolinkFluxoAprovacaoFormatter(cellValue, options, rowdata) {
    return "<a href='/CadastroFundos/FluxoAlteracaoRegulamento.aspx?IdFundoCadastro=" + options.rowId + "'>" + rowdata.NomeFundo + "</a>";
}

//função de validação de data
function AltRequerimentofnValidarDataFundoConstituicao(control) {

    if (($(control).val() !== '')) {
        var regExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])      [\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;

        if (!(($(control).val().match(regExPattern)))) {
            $(control).val('');
        }
    }
}


function AltRequerimentofnCarregarListaFundosEmConstituicao() {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AlteracaoRegulamentoConsultaFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarSelectFundosEmConstituicao'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListaFundosEmConstituicao_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function AltRequerimentofnCarregarListaFundosEmConstituicao_CallBack(pResposta) {

    var $select = $('#listaFundosConstituicao');

    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoCadastro).text(this.NomeFundo);

        $select.append($option);
    });
}


function AltRequerimentofnCarregarListaGruposAprovacaoFundosEmConstituicao() {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AlteracaoRegulamentoConsultaFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarSelectGruposAprovacaoFundosConstituicao'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: AltRequerimentofnCarregarListaGruposAprovacaoFundosEmConstituicao_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function AltRequerimentofnCarregarListaGruposAprovacaoFundosEmConstituicao_CallBack(pResposta)
{

    //obtém todos os selects de status da tela
    var $select = $('#listaGruposFundosConstituicao');

    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFluxoAlteracaoRegulamentoGrupo).text(this.DsFluxoAlteracaoRegulamentoGrupo);

        $select.append($option);
    });
}


function AltRequerimentofnExibirModalEnvioEmailConsultaFundos(idFundoCadastro) {
    AltRequerimentofnCarregarModalEnvioEmailAlteracaoRegulamentoConsultaFundos(idFundoCadastro);
}

function AltRequerimentofnCarregarModalEnvioEmailAlteracaoRegulamentoConsultaFundos(idFundoCadastro) {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AlteracaoRegulamentoConsultaFundos.aspx");

    var lDados =
        {
            Acao: "CarregarDadosModalEnvioEmail",
            IdFundoCadastro: idFundoCadastro
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, AltRequerimentofnCarregarModalEnvioEmailAlteracaoRegulamentoConsultaFundos_CallBack);

    return false;
}

function AltRequerimentofnCarregarModalEnvioEmailAlteracaoRegulamentoConsultaFundos_CallBack(pResposta)
{
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {

        $('#mdlEnviarEmail').modal('show');

        var $fundo = lData[0];

        var nomeFundo = $fundo.NomeFundo;
        var statusGeral = $fundo.StatusGeral;
        var etapa = $fundo.Etapa;
        var statusEtapa = $fundo.StatusEtapa;

        $('#hidDadosEmail').val(nomeFundo + ";" + statusGeral + ";" + etapa + ";" + statusEtapa);

        var html =
            '<h3>Fundo: ' +
                nomeFundo +
                '</h3>' +
                '<div class="row" style="height: 80px; padding-left: 16px; padding-top: 20px">' +
                '<h5>Status geral: ' +
                statusGeral +
                '</h5>' +
                '<h5>Última etapa processada: ' +
                etapa +
                ' | Status: ' +
                statusEtapa +
                '</h5></div>' +
                '<div class="row" style="height: 50px"></div>' +
                '<h4>Obrigado.</h4>';

        $('#dvConteudoEmailConsultaFundos').empty();
        $('#dvConteudoEmailConsultaFundos').append(html);
    } else {
        bootbox.alert(lData.Mensagem);
    }
}

function AltRequerimentofnEnviarEmailAlteracaoRegulamentoConsultaFundos() {

    if ($('#txtEmailsConsultaFundos').val() === '') {
        bootbox.alert('Preencha o campo de e-mails');
    }
    else {        
        var corpoEmail = $('#hidDadosEmail').val();

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/AlteracaoRegulamentoConsultaFundos.aspx");

        var lDados =
            {
                Acao: 'EnviarEmailConsultaFundos',
                CorpoEmail: corpoEmail,
                Destinatarios: $('#txtEmailsConsultaFundos').val()
            }

        $.ajax({
            url: lUrl
                , type: "post"
                , cache: false
                , data: lDados
                , success: AltRequerimentofnEnviarEmailAlteracaoRegulamentoConsultaFundos_CallBack
                , async: false
                , error: Aux_TratarRespostaComErro
        });
    }
}

function AltRequerimentofnEnviarEmailAlteracaoRegulamentoConsultaFundos_CallBack(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK") {
        resp.Mensagem = "E-mail enviado com sucesso";
    }

    bootbox.alert(resp.Mensagem);

    $('#mdlEnviarEmail').modal('hide');
}

function AltRequerimentofnLimparFiltrosAlteracaoRegulamentoConsultaFundos() {
    $('#txtDataDe').val('');
    $('#txtDataAte').val('');
    $('#listaFundosConstituicao').val(0);
    $('#listaGruposFundosConstituicao').val(0);
    $('#chkFundosPendentes').prop('checked', true);
    $('#chkFundosConcluidos').prop('checked', true);
}

$.urlParam = function(name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    return results[1] || 0;
}
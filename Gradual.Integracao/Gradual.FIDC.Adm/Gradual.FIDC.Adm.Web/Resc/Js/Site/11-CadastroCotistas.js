var gGridCotistasFidc;
var gGridCotistasFidcProcuradores;

var gMock_CadastroCotistaProcuradoresGrid;

$(document).ready(function () {

    if ($('#hdnIdCotistaFidc')[0] != undefined) {
        carregarGridCotistas();

        $('#chkCadastroCotistaInativar').prop('checked', true);

        carregarDatePickerCadastroCotistasNascimento();
        carregarDatePickerCadastroCotistasVencimentoCadastro();

        carregarMascarasCotistaFidc();

        formatarCamposUploadCadastroCotistasProcuradores();

        $(".cpf").mask("999.999.999-99");
    }
});

function carregarDatePickerCadastroCotistasNascimento() {
    $('#txtCadastroCotistaDtNascCotista').datepicker();

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

    $("#txtCadastroCotistaDtNascCotista").blur(function () {
        fnValidarDataCotistaFidc(this);
    });

    $('#imgDtNascimentoFundacao').click(function () {
        $("#txtCadastroCotistaDtNascCotista").focus();
    });
}

function carregarDatePickerCadastroCotistasVencimentoCadastro() {

    $('#txtCadastroCotistaDtVencimentoCadastro').datepicker();

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

    $("#txtCadastroCotistaDtVencimentoCadastro").blur(function () {
        fnValidarDataCotistaFidc(this);
    });

    $('#imgDtVencimentoCadastro').click(function () {
        $("#txtCadastroCotistaDtVencimentoCadastro").focus();
    });
}

function fnValidarDataCotistaFidc(control) {

    if (($(control).val() !== '')) {
        var regExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])      [\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;

        if (!(($(control).val().match(regExPattern)))) {
            $(control).val('');
        }
    }
}

function carregarMascarasCotistaFidc() {

    $("#txtCadastroCotistaCpfCnpjCotista").keydown(function (e) {

        $("#txtCadastroCotistaCpfCnpjCotista").mask("999999999999999999999999999");

        var $txt = $("#txtCadastroCotistaCpfCnpjCotista").val().replace("/", "").replace("-", "").split(".").join("");

        var tamanho = $txt.length;

        if (tamanho < 11) {
            $("#txtCadastroCotistaCpfCnpjCotista").mask("999.999.999-999");
        } else {
            $("#txtCadastroCotistaCpfCnpjCotista").mask("99.999.999/9999-99");
        }
    });
}

function Grid_Resultado_Cotistas_Fidc() {

    gGridCotistasFidc =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdCotistaFidc", jsonmap: "IdCotistaFidc", index: "IdCotistaFidc", align: "left", sortable: true, hidden: true }
                    , { label: "Nome Cotista", name: "NomeCotista", jsonmap: "NomeCotista", index: "NomeCotista", align: "left", sortable: true }
                    , { label: "E-mail", name: "Email", jsonmap: "Email", index: "Email", width: 150, align: "left", sortable: true }
                    , { label: "Ativo", name: "Ativo", jsonmap: "Ativo", index: "Ativo", width: 21, align: "left", sortable: true, formatter: statusCotistaFidcImageFormatter }                    
                    , { name: "", index: "AddProcuradores", width: 15, align: "left", sortable: true, formatter: cadastroCotistaAdicionarProcuradoresFormatter }
                    , { name: '', id: 'editar', index: 'editar', width: 22, formatter: editarCotistaFormatter, sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      //, sortname: "NomeCotista"
      , sortorder: "asc"
      , ignoreCase: true
      , viewrecords: true
      , gridview: false
      , pager: '#pgCadastroCotistas'
      , subGrid: false
      , rowNum: 20
      , rowList: [20, 30, 40, 50, 60]
      , caption: 'Cadastro de Cotistas'
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
    };

    $('#gridCadastroCotistas').jqGrid(gGridCotistasFidc);

    $('#gridCadastroCotistas').jqGrid('setGridWidth', 800);
}


function statusCotistaFidcImageFormatter(cellvalue, options) {
    if(cellvalue)
        return "<span class='glyphicon glyphicon-ok glyphicongrid'></span>"
    return "<span class='glyphicon glyphicon-remove glyphicongrid'></span>"
}

function editarCotistaFormatter(cellvalue, options) {
    return "<a href='#' style='height:25px;width:5px;' type='button' title='Editar' onclick=\"editarCadastroCotista(" + options.rowId + ")\" >Editar</a>";
}

function cadastroCotistaAdicionarProcuradoresFormatter(cellvalue, options) {
    return "<span title='Procuradores...' onclick=\"cadastroCotistaAdicionarProcuradores(" + options.rowId + ")\" class='glyphicon glyphicon-user glyphicongrid spanAdicionarProcuradores'></span>";
}

function cadastroCotistaAdicionarProcuradores(idCotista) {
    $("#hidAddProcuradoresIdCotistaFidc").val(idCotista);
    $("#mdlAdicionarProcuradores").modal("show");

    LimparDadosProcuradoresCotista();

    carregarGridCotistaProcuradores();

    //editarCadastroCotista(idCotista);

    $('#gridCadastroCotistas').jqGrid('setSelection', idCotista);
}

function FiltroCadastroCotistas_Click() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

    var lDados =
        {
            Acao: "CarregarGridCotistas"
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, FiltroCadastroCotistas_Click_CallBack);

    if ($("#imgFiltro")[0] === undefined) {
        $("#jqgh_gridCadastroCotistas_NomeCotista").append('&nbsp;&nbsp;&nbsp;<span id="imgFiltro" class="glyphicon glyphicon-filter"></span>');

        $("#imgFiltro").click(function (e) {

            if ($('.ui-search-toolbar:visible').length > 0)
                $('.ui-search-toolbar').hide();
            else
                $('.ui-search-toolbar').show();

            e.stopPropagation();

            $('#gs_NomeCotista').focus();
        });
    }

    return false;
}

function FiltroCadastroCotistas_Click_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (lData.TemErro)
    {
        bootbox.alert(lData.Mensagem + ": " + lData.MensagemExtendida);

        return;
    }

    gMock_CadastroCotistaGrid = { page: 1, total: 3, records: 0, rows: [] };

    //adiciona linhas ao grid
    if (lData.Itens.length) {
        for (var i = 0; i < lData.Itens.length; i++) {
            var lObjeto =
                {
                    IdCotistaFidc: lData.Itens[i].IdCotistaFidc,
                    NomeCotista: lData.Itens[i].NomeCotista,
                    Email: lData.Itens[i].Email,
                    Ativo: lData.Itens[i].IsAtivo
                };

            gMock_CadastroCotistaGrid.rows.push(lObjeto);
        }
    }

    $('#gridCadastroCotistas')
        .jqGrid('setGridParam', { datatype: 'local', data: gMock_CadastroCotistaGrid.rows })
        .trigger("reloadGrid");

    $('.ui-pg-input').width(20);

    $("#gridCadastroCotistas").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        ignoreCase: true
    });

    $('#gs_Email').remove();
    $('#gs_Ativo').remove();
    $('#gs_').remove();

    $('.ui-search-toolbar').hide();
}

function carregarGridCotistas() {
    Grid_Resultado_Cotistas_Fidc();

    FiltroCadastroCotistas_Click();
}

//executa atualizacao
function btnCadastroCotistaAtualizar_Click() {

    //verifica dados obrigatórios
    var erros = '';

    if ($("#txtCadastroCotistaNomeCotista").val() === '')
        erros += 'Informe o nome<br>';
    if ($("#txtCadastroCotistaEmailCotista").val() === '')
        erros += 'Informe o e-mail<br>';
    if ($("#txtCadastroCotistaCpfCnpjCotista").val() === '')
        erros += 'Informe o CPF/CNPJ<br>';
    if ($("#txtCadastroCotistaDtNascCotista").val() === '')
        erros += 'Informe a data nascimento/fundação<br>';

    if ($("#txtCadastroCotistaQuantidadeCotas").val() === '')
        erros += 'Informe a quantidade de cotas<br>';
    if ($("#txtCadastroCotistaDtVencimentoCadastro").val() === '')
        erros += 'Informe a data de vencimento do cadastro<br>';
    if ($("#txtCadastroCotistaClasseCotas").val() === '')
        erros += 'Informe a classe de cotas<br>';

    //caso todos os dados estejam preenchidos corretamente, prossegue com a atualização dos dados
    if (erros === "") {

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

        var lDados =
            {
                Acao: "AtualizarCotista"
              , IdCotistaFidc: $('#hdnIdCotistaFidc').val()
              , NomeCotista: $("#txtCadastroCotistaNomeCotista").val()
              , CpfCnpj: $("#txtCadastroCotistaCpfCnpjCotista").val().replace("/", "").replace("-", "").split(".").join("")
              , Email: $("#txtCadastroCotistaEmailCotista").val()
              , DataNascFundacao: $("#txtCadastroCotistaDtNascCotista").val()
              , IsAtivo: $("#chkCadastroCotistaInativar").is(':checked')
              , QtdCotas: $("#txtCadastroCotistaQuantidadeCotas").val()
              , DataVencimentoCadastro: $("#txtCadastroCotistaDtVencimentoCadastro").val()
              , ClasseCotas: $("#txtCadastroCotistaClasseCotas").val()
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnCadastroCotista_Click_Callback);
    }
    else {
        bootbox.alert(erros);
    }

    return false;
}

function btnCadastroCotista_Click_Callback(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK")
        resp.Mensagem = "Cotista atualizado com sucesso";

    LimparDadosCadastroCotista();

    bootbox.alert(resp.Mensagem);
}

function LimparDadosCadastroCotista() {

    $("#txtCadastroCotistaNomeCotista").val("");
    $("#txtCadastroCotistaCpfCnpjCotista").val("");
    $("#txtCadastroCotistaEmailCotista").val("");
    $("#txtCadastroCotistaDtNascCotista").val("");

    $("#txtCadastroCotistaQuantidadeCotas").val("");
    $("#txtCadastroCotistaDtVencimentoCadastro").val("");
    $("#txtCadastroCotistaClasseCotas").val("");

    //limpa o campo que indica o cotista a ser atualizado
    $('#hdnIdCotistaFidc').val('0');

    $('#chkCadastroCotistaInativar').prop('checked', true);

    carregarGridCotistas();
}

function editarCadastroCotista(id) {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

    var lDados =
        {
            Acao: "EditarCotista"
          , IdCotistaFidc: id
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, editarCadastroCotista_Callback);

    jQuery('#gridCadastroCotistas').jqGrid('setSelection', id);

    return false;
}

function editarCadastroCotista_Callback(pResposta) {

    var resp = JSON.parse(pResposta);
    resp = resp[0];
    
    $("#txtCadastroCotistaNomeCotista").val(resp.NomeCotista);
    $("#txtCadastroCotistaCpfCnpjCotista").val(resp.CpfCnpj);
    $("#txtCadastroCotistaEmailCotista").val(resp.Email);
    $("#txtCadastroCotistaDtNascCotista").val(resp.DataNascFundacaoFormatada);

    $("#txtCadastroCotistaQuantidadeCotas").val(resp.QuantidadeCotas);
    $("#txtCadastroCotistaDtVencimentoCadastro").val(resp.DtVencimentoCadastroFormatada);
    $("#txtCadastroCotistaClasseCotas").val(resp.ClasseCotas);
    
    $('#chkCadastroCotistaInativar').prop('checked', resp.IsAtivo);

    $("#hdnIdCotistaFidc").val(resp.IdCotistaFidc);

    if (resp.DataNascFundacaoFormatada.length == 11) {
        $("#txtCadastroCotistaCpfCnpjCotista").mask("999.999.999-99");
    } else {
        $("#txtCadastroCotistaCpfCnpjCotista").mask("99.999.999/9999-99");
    }

    if (resp.CpfCnpj.length <= 11) {
        $("#txtCadastroCotistaCpfCnpjCotista").mask("999.999.999-999");
    } else {
        $("#txtCadastroCotistaCpfCnpjCotista").mask("99.999.999/9999-99");
    }
}

function fnGravarCotistaProcurador() {

    var erros = '';

    if ($("#txtCadastroCotistaNomeProcurador").val() === '')
        erros += 'Informe o nome do procurador<br>';
    if ($("#txtCadastroCotistaCpfProcurador").val() === '')
        erros += 'Informe o cpf do procurador<br>';

    if (erros === "") {

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

        var lDados = new FormData();

        lDados.append("Acao", "GravarCotistaProcurador");
        lDados.append("IdCotistaFidcProcurador", $("#hidAddProcuradoresIdCotistaFidcProcurador").val());
        lDados.append("IdCotistaFidc", $("#hidAddProcuradoresIdCotistaFidc").val());
        lDados.append("NomeProcurador", $("#txtCadastroCotistaNomeProcurador").val());
        lDados.append("CpfProcurador", $("#txtCadastroCotistaCpfProcurador").val().replace("-", "").split(".").join(""));

        lDados.append("ArquivoAnexoDecreto", obterAnexosCadastroCotistaProcuradorDecreto());
        lDados.append("ArquivoAnexoProcuracao", obterAnexosCadastroCotistaProcuradorProcuracao());
        lDados.append("ArquivoAnexoTermo", obterAnexosCadastroCotistaProcuradorTermo());

        $.ajax({
            url: lUrl,
            type: "POST",
            data: lDados,
            processData: false,
            contentType: false,
            async: false,
            success: fnGravarCotistaProcurador_CallBack,
            error: Aux_TratarRespostaComErro
        });

        /*
        var lDados =
            {
                Acao: "GravarCotistaProcurador"
              , IdCotistaFidcProcurador: $('#hidAddProcuradoresIdCotistaFidcProcurador').val()
              , IdCotistaFidc: $('#hidAddProcuradoresIdCotistaFidc').val()
              , NomeProcurador: $("#txtCadastroCotistaNomeProcurador").val()
              , CpfProcurador: $("#txtCadastroCotistaCpfProcurador").val()
            };*/

        //Aux_CarregarHtmlVerificandoErro(lUrl, lDados, fnGravarCotistaProcurador_CallBack);
    }
    else {
        bootbox.alert(erros);
    }

    return false;
}

function fnGravarCotistaProcurador_CallBack(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK")
        resp.Mensagem = "Procurador gravado com sucesso";

    LimparDadosProcuradoresCotista();

    bootbox.alert(resp.Mensagem);
}

function Grid_Resultado_Cotistas_Fidc_Procuradores() {

    gGridCotistasFidcProcuradores =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdCotistaFidcProcurador", jsonmap: "IdCotistaFidcProcurador", index: "IdCotistaFidcProcurador", align: "left", sortable: true, hidden: true }
                    , { label: "Nome Procurador", name: "NomeProcurador", jsonmap: "NomeProcurador", index: "NomeProcurador", align: "left", sortable: true }
                    //, { label: "CPF", name: "CPF", jsonmap: "CPF", index: "CPF", align: "left", sortable: true, hidden: true }
                    , { name: '', id: 'editar', index: 'editar', width: 15, formatter: editarCotistaProcuradorFormatter, sortable: false }
                    , { name: '', id: 'excluir', index: 'excluir', width: 16, formatter: excluirCotistaProcuradorFormatter, sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      //, sortname: "NomeProcurador"
      , sortorder: "asc"
      , ignoreCase: true
      , viewrecords: true
      , gridview: false
      , pager: '#pgGridCotistaProcuradores'
      , subGrid: false
      , rowNum: 5
      , rowList: [5, 10, 15]
      , caption: 'Cadastro de Procuradores'
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
    };

    $('#gridCotistaProcuradores').jqGrid(gGridCotistasFidcProcuradores);

    $('#gridCotistaProcuradores').jqGrid('setGridWidth', 565);
}

function editarCotistaProcuradorFormatter(cellvalue, options) {
    return "<a href='#' style='height:25px;width:5px;' type='button' title='Editar' onclick=\"editarCadastroCotistaProcurador(" + options.rowId + ")\" >Editar</a>";
}

function excluirCotistaProcuradorFormatter(cellvalue, options) {
    return "<a href='#' style='height:25px;width:5px;' type='button' title='Excluir' onclick=\"excluirCadastroCotistaProcurador(" + options.rowId + ")\" >Excluir</a>";
}

function FiltroCadastroCotistasProcuradores_Click() {
       
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

    var lDados =
        {
            Acao: "CarregarGridCotistasProcuradores",
            IdCotistaFidc: $('#hidAddProcuradoresIdCotistaFidc').val()
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, FiltroCadastroCotistasProcuradores_Click_CallBack);
}

function FiltroCadastroCotistasProcuradores_Click_CallBack(pResposta) {

    var lData = JSON.parse(pResposta);

    if (lData.TemErro) {
        bootbox.alert(lData.Mensagem + ": " + lData.MensagemExtendida);

        return;
    }

    gMock_CadastroCotistaProcuradoresGrid = { page: 1, total: 3, records: 0, rows: [] };

    //adiciona linhas ao grid
    if (lData.Itens.length) {
        for (var i = 0; i < lData.Itens.length; i++) {

            var lObjeto =
                {
                    IdCotistaFidcProcurador: lData.Itens[i].IdCotistaFidcProcurador,
                    NomeProcurador: lData.Itens[i].NomeProcurador,
                    CPF: lData.Itens[i].CPF
                };

            gMock_CadastroCotistaProcuradoresGrid.rows.push(lObjeto);
        }
    }

    $('#gridCotistaProcuradores').jqGrid("clearGridData", true).trigger("reloadGrid");

    $('#gridCotistaProcuradores')
        .jqGrid('setGridParam', { datatype: 'local', data: gMock_CadastroCotistaProcuradoresGrid.rows })
        .trigger("reloadGrid");
    
    $('.ui-pg-input').width(20);

    $('.ui-search-toolbar').hide();
}

function carregarGridCotistaProcuradores() {
    Grid_Resultado_Cotistas_Fidc_Procuradores();

    FiltroCadastroCotistasProcuradores_Click();
}

function LimparDadosProcuradoresCotista() {

    $("#txtCadastroCotistaNomeProcurador").val("");
    $("#txtCadastroCotistaCpfProcurador").val("");

    //limpa o campo que indica o procurador a ser atualizado
    $('#hidAddProcuradoresIdCotistaFidcProcurador').val('0');
    
    removerCadastroCotistaProcuradorAnexoDecreto();
    removerCadastroCotistaProcuradorAnexoProcuracao();
    removerCadastroCotistaProcuradorAnexoTermo();

    carregarGridCotistaProcuradores();
}

function formatarCamposUploadCadastroCotistasProcuradores() {

    jQuery("#upAnexarDecreto").change(function () {

        $("#dvBtnRemoverArquivoDecreto a").remove();

        $("#dvBtnRemoverArquivoDecreto").append("<a id='anRemoverArquivoDecretoUpload' class='removerAnexo' title='remover anexo' type='button' onclick=removerCadastroCotistaProcuradorAnexoDecreto() class='removerArquivo'>X</a>");
    });

    jQuery("#upAnexarProcuracao").change(function () {

        $("#dvBtnRemoverArquivoProcuracao a").remove();

        $("#dvBtnRemoverArquivoProcuracao").append("<a id='anRemoverArquivoProcuracaoUpload' class='removerAnexo' title='remover anexo' type='button' onclick=removerCadastroCotistaProcuradorAnexoProcuracao() class='removerArquivo'>X</a>");

    });

    jQuery("#upAnexarTermo").change(function () {

        $("#dvBtnRemoverArquivoTermo a").remove();

        $("#dvBtnRemoverArquivoTermo").append("<a id='anRemoverArquivoTermoUpload' class='removerAnexo' title='remover anexo' type='button' onclick=removerCadastroCotistaProcuradorAnexoTermo() class='removerArquivo'>X</a>");

    });

}

function obterAnexosCadastroCotistaProcuradorDecreto() {

    var input = $("#upAnexarDecreto")[0];

    if (input.files && input.files[0]) {

        var file = input.files[0];
        var fr = new FileReader();

        fr.readAsDataURL(file);

        return file;
    }

    return null;
}

function obterAnexosCadastroCotistaProcuradorProcuracao() {

    var input = $("#upAnexarProcuracao")[0];

    if (input.files && input.files[0]) {

        var file = input.files[0];
        var fr = new FileReader();

        fr.readAsDataURL(file);

        return file;
    }

    return null;
}

function obterAnexosCadastroCotistaProcuradorTermo() {

    var input = $("#upAnexarTermo")[0];

    if (input.files && input.files[0]) {

        var file = input.files[0];
        var fr = new FileReader();

        fr.readAsDataURL(file);

        return file;
    }

    return null;
}

function removerCadastroCotistaProcuradorAnexoDecreto() {

    $("#dvBtnRemoverArquivoDecreto a").remove();

    var input = $("#upAnexarDecreto");

    input.replaceWith(input.val('').clone(true));
}

function removerCadastroCotistaProcuradorAnexoProcuracao() {

    $("#dvBtnRemoverArquivoProcuracao a").remove();

    var input = $("#upAnexarProcuracao");

    input.replaceWith(input.val('').clone(true));
}

function removerCadastroCotistaProcuradorAnexoTermo() {

    $("#dvBtnRemoverArquivoTermo a").remove();

    var input = $("#upAnexarTermo");

    input.replaceWith(input.val('').clone(true));
}


function editarCadastroCotistaProcurador(id) {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

    var lDados =
        {
            Acao: "SelecionarEditarProcurador"
          , IdCotistaFidcProcurador: id
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, editarCadastroCotistaProcurador_Callback);

    jQuery('#gridCadastroCotistas').jqGrid('setSelection', id);

    return false;
}

function editarCadastroCotistaProcurador_Callback(pResposta) {

    var resp = JSON.parse(pResposta);
    resp = resp[0];

    $("#txtCadastroCotistaNomeProcurador").val(resp.NomeProcurador);
    $("#txtCadastroCotistaCpfProcurador").val(resp.CPF);
    $("#hidAddProcuradoresIdCotistaFidcProcurador").val(resp.IdCotistaFidcProcurador)
    
    $("#txtCadastroCotistaCpfProcurador").mask("999.999.999-99");
}

function excluirCadastroCotistaProcurador(id) {

    if (confirm('Tem certeza de que deseja remover o procurador?')) {

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroCotistas.aspx");

        var lDados =
            {
                Acao: "ExcluirProcurador"
              , IdCotistaFidcProcurador: id
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, excluirCadastroCotistaProcurador_Callback);
    }
    return false;
}

function excluirCadastroCotistaProcurador_Callback(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK") {
        resp.Mensagem = "Procurador removido com sucesso";

        carregarGridCotistaProcuradores();

        return false;
    }

    bootbox.alert(resp.Mensagem + ": " + resp.MensagemExtendida);
}

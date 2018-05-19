var gGridCotistasFidc;

$(document).ready(function () {

    if ($('#hdnIdCotistaFundo')[0] != undefined) {
        
        fnCarregarListaFundosCotistasFidc();
        fnCarregarListaCotistas();
    }
});

function carregarGridPorFundo(sel) {

    var $valor = $(sel).val();

    if ($("#hdnGridCarregadoPorCotista").val() === "0" && $valor > 0) {
        $("#hdnGridCarregadoPorFundos").val("1");
        carregarGrid($valor, 0);
    }
}

function carregarGridPorCotista(sel) {

    var $valor = $(sel).val();

    if ($("#hdnGridCarregadoPorFundos").val() === "0" && $valor > 0) {
        $("#hdnGridCarregadoPorCotista").val("1");
        carregarGrid(0, $valor);
    }
}

function carregarGrid(idFundoCadastro, idCotistaFidc) {

    $("#gridCotistasXFundos").jqGrid("clearGridData", true);
    $('#rExportarDadosCsv').hide();

    GridCotistasXFundos();
    carregarGridCotistasXFundos(idFundoCadastro, idCotistaFidc);
}

function recarregarGridCotistasFundos() {
    if ($("#hdnGridCarregadoPorFundos").val() === "1")
        carregarGrid($('#selFundo').val(), 0);
    else
        carregarGrid(0, $('#selCotistaFidc').val());
}

function btnCotistaFundoAtualizar_Click() {
    //verifica dados obrigatórios
    var erros = '';

    if ($("#selCotistaFidc").val() === '0')
        erros += 'Selecione um cotista\n';
    if ($("#selFundo").val() === '0')
        erros += 'Selecione um fundo\n';

    //caso todos os dados estejam preenchidos corretamente, prossegue com a atualização dos dados
    if (erros === "") {

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/AssociacaoCostistasFidcXFundos.aspx");

        var lDados =
            {
                Acao: "InserirAssociacao"
              , IdCotistaFidc: $('#selCotistaFidc').val()
              , IdFundoCadastro: $("#selFundo").val()
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnCotistaFundoAtualizar_Click_Callback);
    }
    else {
        bootbox.alert(erros);
    }

    return false;
}

function btnCotistaFundoAtualizar_Click_Callback(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK")
        resp.Mensagem = "Associação incluída com sucesso";
    
    bootbox.alert(resp.Mensagem);

    recarregarGridCotistasFundos();
}

function btnCotistaFundoLimparDados_Click() {

    $("#gridCotistasXFundos").jqGrid("clearGridData", true);
    $('#rExportarDadosCsv').hide();

    $("#hdnGridCarregadoPorCotista").val("0");
    $("#hdnGridCarregadoPorFundos").val("0");

    $('#hdnCotistaSelecionado').val("0");
    $("#hdnFundoSelecionado").val("0");
    
    $('#selFundo').val(0);
    $("#selCotistaFidc").val(0);

    $('.selectpicker').selectpicker('refresh');
}

function GridCotistasXFundos() {

    gGridCotistasXFundos =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CadastroFundosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdCotistaFidcFundo", jsonmap: "IdCotistaFidcFundo", index: "IdCotistaFidcFundo", align: "left", sortable: true, hidden: true }
                    , { label: "Nome do Cotista", name: "NomeCotista", jsonmap: "NomeCotista", index: "NomeCotista", align: "left", sortable: true }
                    , { label: "Nome do Fundo", name: "NomeFundo", jsonmap: "NomeFundo", index: "NomeFundo", align: "left", sortable: true }
                    , { label: "EmailCotista", name: "EmailCotista", jsonmap: "EmailCotista", index: "EmailCotista", align: "left", sortable: true, hidden: true }
                    , { name: '', id: 'excluir', index: 'excluir', width: 20, formatter: excluirCotistaFundoFormatter, sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortname: "NomeFundo"
      , sortorder: "asc"
      , ignoreCase: true
      , viewrecords: true
      , gridview: false
      , pager: '#pgGridCotistasXFundos'
      , subGrid: false
      , rowNum: 40
      , rowList: [40, 50, 60, 70, 80]
      , caption: 'Cadastro de Cotistas'
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
    };

    $('#gridCotistasXFundos').jqGrid(gGridCotistasXFundos);

    $('#gridCotistasXFundos').jqGrid('setGridWidth', 820);
}

function excluirCotistaFundoFormatter(cellvalue, options) {
    return "<a href='#' style='height:25px;width:5px;' type='button' title='Excluir' onclick=\"excluirAssociacaoCotistaFundo(" + options.rowId + ")\" >Excluir</a>";
}

function carregarGridCotistasXFundos(idFundoCadastro, idCotistaFidc) {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AssociacaoCostistasFidcXFundos.aspx");

    var lDados =
        {
            Acao: "CarregarGrid",
            IdCotistaFidc: idCotistaFidc,
            IdFundoCadastro: idFundoCadastro
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, carregarGridCotistasXFundos_CallBack);

    return false;
}

function carregarGridCotistasXFundos_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_CadastroCotistaFundoGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            $('#rExportarDadosCsv').show();

            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdCotistaFidcFundo: lData.Itens[i].IdCotistaFidcFundo,
                        NomeCotista: lData.Itens[i].NomeCotista,
                        NomeFundo: lData.Itens[i].NomeFundo,
                        EmailCotista: lData.Itens[i].EmailCotista
                    };

                gMock_CadastroCotistaFundoGrid.rows.push(lObjeto);
            }
        }

        $('#gridCotistasXFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CadastroCotistaFundoGrid.rows })
            .trigger("reloadGrid");

        $('.ui-pg-input').width(20);
    }
}

function excluirAssociacaoCotistaFundo(id) {

    if (confirm('Tem certeza de que deseja remover a associação?')) {

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/AssociacaoCostistasFidcXFundos.aspx");

        var lDados =
            {
                Acao: "RemoverAssociacao"
              , IdCotistaFidcFundo: id
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, excluirAssociacaoCotistaFundo_Callback);

    }
    return false;
}

function excluirAssociacaoCotistaFundo_Callback(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK")
        resp.Mensagem = "Associação removida com sucesso";

    recarregarGridCotistasFundos();

    bootbox.alert(resp.Mensagem);
}

function exportarDadosCotistaFundoCsv() {

    var data = [["NomeFundo", "NomeCotista", "EmailCotista"]];

    var linhasGrid = $("#gridCotistasXFundos").jqGrid('getRowData');

    $.each(linhasGrid, function (index, value) {
        data.push([linhasGrid[index].NomeFundo, linhasGrid[index].NomeCotista, linhasGrid[index].EmailCotista]);
    });

    var csvContent = "data:text/csv;charset=utf-8,";

    data.forEach(function (infoArray, index) {

        dataString = infoArray.join(";");
        csvContent += index < data.length ? dataString + "\n" : dataString;

    });

    var encodedUri = encodeURI(csvContent);

    var link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "CotistasXFundos.csv");
    document.body.appendChild(link);

    link.click();
}

function fnCarregarListaFundosCotistasFidc() {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AssociacaoCostistasFidcXFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarSelectFundos'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListaFundosCotistasFidc_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarListaFundosCotistasFidc_CallBack(pResposta) {

    var $select = $('#selFundo');

    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoCadastro).text(this.NomeFundo);

        $select.append($option);
    });
}

function fnCarregarListaCotistas() {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/AssociacaoCostistasFidcXFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarSelectCotistas'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListaCotistas_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarListaCotistas_CallBack(pResposta) {

    var $select = $('#selCotistaFidc');

    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdCotistaFidc).text(this.NomeCotista);

        $select.append($option);
    });
}
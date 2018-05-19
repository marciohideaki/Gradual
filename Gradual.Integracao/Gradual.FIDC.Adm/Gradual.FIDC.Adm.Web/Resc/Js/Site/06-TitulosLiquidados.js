var gGridTitulosLiquidados; //Váriável global do grid de carteira

$(document).ready(function () {
    $("input.txtTitulosLiquidadosADMDataDe").datepicker({
        dateFormat: "dd/mm/y"
        , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
        , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
        , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
    });
    $("input.txtTitulosLiquidadosADMDataDe").val(moment().add(-1, 'days').format('DD/MM/YY'));

    $("input.txtTitulosLiquidadosADMDataAte").datepicker({
        dateFormat: "dd/mm/y"
        , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
        , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
        , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
        //, maxDate: "+0"
    });
    $("input.txtTitulosLiquidadosADMDataAte").val(moment().add(1, 'days').format('DD/MM/YY'));

    $("#txtTermoBusca").keypress(function (event) {
        if (event.keyCode == 13) {
            $("#btnBuscar").click();
            return false;
        }
    });

    $("#txtTitulosLiquidadosADMNomeFundo").autocomplete({
        minLength: 3
        , source: function (request, response) {
            var lUrl = Aux_UrlComRaiz("/Handlers/ListaFundosHandler.ashx");
            $.ajax
           ({
               url: lUrl + "?NomeFundo=" + $("#txtTitulosLiquidadosADMNomeFundo").val(),
               dataType: "json",
               type: "POST",
               success: function (data) {
                   response($.map(data, function (item) {
                       return {
                           label: item.NomeFundo,
                           value: item.CodigoFundo
                       }
                   }))
               },
               error: function (a, b, c) {
                   debugger;
               }
           });
        }
        , select: function (event, ui) {
            event.preventDefault();
            $("#txtTitulosLiquidadosADMCodigoFundo").val(ui.item.value);
            $("#txtTitulosLiquidadosADMNomeFundo").val(ui.item.label);
        }
    });
});

function TitulosLiquidados_ShowPainelPesquisa() {
    if ($("#TitulosLiquidados_pnlFiltroPesquisa").is(':hidden')) {
        $("#TitulosLiquidados_pnlFiltroPesquisa").show();
        $("#titLiqFiltroIcone").removeClass("glyphicon glyphicon-plus").addClass("glyphicon glyphicon-minus");
    }
    else {
        $("#TitulosLiquidados_pnlFiltroPesquisa").hide();
        $("#titLiqFiltroIcone").removeClass("glyphicon glyphicon-minus").addClass("glyphicon glyphicon-plus");
    }
}

var gMock_TitulosLiquidados = {};

function btnFiltroTitulosLiquidadosLimpar_Click() {
    var today = moment();
    var tomorrow = moment(today, 'DD MM YY').add(1, 'day');
    var yesterday = moment(today, 'DD MM YY').subtract(1, 'day');

    $("#txtTitulosLiquidadosCodigoFundo").val("");
    $("#txtTitulosLiquidadosNomeFundo").val("");
    $("#txtTitulosLiquidadosDataDe").val(yesterday.format('DD/MM/YY'));
    $("#txtTitulosLiquidadosDataAte").val(tomorrow.format('DD/MM/YY'));
    $("#chkTitulosLiquidadosDownloadsPendentes").prop('checked', false)

    $('#gridTitulosLiquidados').jqGrid('clearGridData');

    return false;
}

///Evento click do botão buscar da página de TitulosLiquidados
function btnFiltroTitulosLiquidados_Click() {
    var lUrl = Aux_UrlComRaiz("/RoboDownload/TitulosLiquidados.aspx");

    var lDados =
        {
            Acao: "CarregarHtmlComDados"
          , CodigoFundo: $("#txtTitulosLiquidadosCodigoFundo").val()
          , DataDe: $("#txtTitulosLiquidadosDataDe").val()
          , DataAte: $("#txtTitulosLiquidadosDataAte").val()
          , DownloadsPendentes: $("#chkTitulosLiquidadosDownloadsPendentes").is(':checked')
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnFiltroTitulosLiquidados_Click_Callback);

    $('#gridTitulosLiquidados').jqGrid('clearGridData');

    return false;
}

function btnFiltroTitulosLiquidados_Click_Callback(pResposta) {
    if (!pResposta.TemErro) {
        gGridTitulosLiquidados = { page: 1, total: 3, records: 0, rows: [] };

        var lData = JSON.parse(pResposta);

        if (lData.Itens.length) {
            for (i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        NomeFundo: lData.Itens[i].NomeFundo,
                        Data: lData.Itens[i].Data,
                        Status: lData.Itens[i].Status,
                        DownloadLink: lData.Itens[i].DownloadLink,
                    };

                gGridTitulosLiquidados.rows.push(lObjeto);
            }
        }

        $('#gridTitulosLiquidados')
            .jqGrid('setGridParam', { datatype: 'local', data: gGridTitulosLiquidados.rows })
            .trigger("reloadGrid");
    }
}

///Método que monta o grid de TitulosLiquidados
function Grid_Resultado_TitulosLiquidados() {
    gGridTitulosLiquidados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_TitulosLiquidados.rows
        , autowidth: true
        , shrinkToFit: true
        //, colNames: ["Codigo", "Fundo", "Categoria", "Download", "Status", ""]
      , colModel: [
                        //{ label: "Código",      name: "CodigoFundo",    jsonmap: "CodigoFundo",     index: "CodigoFundo",   width: 75, align: "left", sortable: true }
                        { label: "Nome Fundo", name: "NomeFundo", jsonmap: "NomeFundo", index: "NomeFundo", width: 500, align: "left", sortable: true }
                      , { label: "Data", name: "Data", jsonmap: "Data", index: "Data", width: 120, align: "center", sortable: true }
                      , { label: "Status", name: "Status", jsonmap: "Status", index: "Status", width: 75, align: "center", sortable: true }
                      , { label: " ", name: "DownloadLink", jsonmap: "DownloadLink", index: "DownloadLink", width: 200, align: "center", sortable: true }

      ]
      , loadonce: true
      , height: 'auto'
        //, width: '950px'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , pager: '#pgTitulosLiquidados'
      , subGrid: false
      , rowNum: 15
      , rowList: [10, 20, 30]
      , caption: 'Downloads Títulos Liquidados'
        //, subGridRowExpanded: subGridRowExpandendTitulosLiquidados
        //, gridComplete: function () {
        //}
      , afterInsertRow: InsertRowTitulosLiquidados
      , sortable: true
    };

    $('#gridTitulosLiquidados').jqGrid(gGridTitulosLiquidados).jqGrid('hideCol', "CodigoFundo");
    //gGridTitulosLiquidados.jqGrid('hideCol', "CodigoFundo");
}

function InsertRowTitulosLiquidados(rowid, pData) {
    var lGrid = $('#gridTitulosLiquidados');

    if (pData.Status == 'N') {
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<a href="" onclick="return TitulosLiquidados_BaixarManualmente(' + pData.CodigoFundo + ')" >Baixar Manualmente</a>', 'glyphicons circle_arrow_down');
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return TitulosLiquidados_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'fidc\')" >Baixar Manualmente</a><span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>', '');
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div>Arquivo indisponível<span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusNoOk');
    }
    else {
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return TitulosLiquidados_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'fidc\')" >Download Concluído</a> <span class="glyphicon glyphicon-circle-arrow-down" style="padding-left:5px"></span></div>', '');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusOk');
    }
}

function TitulosLiquidados_BaixarManualmente(postData, pTypeName) {
    var lUrl = Aux_UrlComRaiz("/Handlers/DownloadHandler.ashx");

    var postFormStr = "<form method='POST' action='" + lUrl + "'>\n";

    postFormStr += "<input type='hidden' name='FileName' value='" + postData + "'></input>";

    postFormStr += "<input type='hidden' name='TypeFile' value='" + pTypeName + "'></input>";
    /*
    for (var key in postData)
    {
        if (postData.hasOwnProperty(key))
        {
            postFormStr += "<input type='hidden' name='" + key + "' value='" + postData[key] + "'></input>";
        }
    }
    */
    postFormStr += "</form>";

    var formElement = $(postFormStr);

    $('body').append(formElement);

    $(formElement).submit();

    return false;
}

/*********************************************************************/
/****** Pasta de Títulos liquidados***********************************/
/*********************************************************************/

var gGridTitulosLiquidadosADM;
var gMock_TitulosLiquidadosADM = {};

///Método que monta o grid de TitulosLiquidados
function Grid_Resultado_TitulosLiquidadosADM() {
    gGridTitulosLiquidadosADM =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_TitulosLiquidadosADM.rows
        , autowidth: true
        //, colNames: ["Codigo", "Fundo", "Categoria", "Download", "Status", ""]
        , colModel: [
                        { label: "Código Fundo", name: "CodigoFundo", jsonmap: "CodigoFundo", index: "CodigoFundo", width: 100, align: "center", sortable: true, key: true }
                        , { label: "Data Ref.", name: "DataRef", jsonmap: "DataRef", index: "DataRef", width: 100, align: "center", sortable: true }
                        , { label: "Nome Fundo", name: "NomeFundo", jsonmap: "NomeFundo", index: "NomeFundo", width: 500, align: "left", sortable: true }
                        , { label: "Valor", name: "Valor", jsonmap: "Valor", index: "Valor", width: 80, align: "center", sortable: true, editable: true }
        ]
        , loadonce: true
        , height: 'auto'
        , rowNum: 0
        , sortname: "invid"
        , sortorder: "desc"
        , viewrecords: true
        , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
        , pager: '#pgTitulosLiquidadosADM'
        , subGrid: false
        , rowNum: 15
        , rowList: [10, 20, 30]
        , caption: 'Administração Títulos Liquidados'
        //, subGridRowExpanded: subGridRowExpandendTitulosLiquidados
        //, gridComplete: function () {
        //}
        , cellurl: Aux_UrlComRaiz('/TitulosLiquidados/TitulosLiquidados.aspx?Acao=AtualizarValor')
        , cellEdit: true
        , cellsubmit: 'remote'
        , afterInsertRow: InsertRowTitulosLiquidadosADM
        , sortable: true
    };

    $('#gridTitulosLiquidadosADM')
        .jqGrid(gGridTitulosLiquidadosADM)
    //.jqGrid('hideCol', "CodigoFundo");
}

function TitulosLiquidadosADM_Fundos_Change(pSender) {
    var lDados =
        {
            Acao: "CarregarHtmlComDados",
            CodigoFundo: lCodigoFundo,
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, TitulosLiquidadosADM_Fundos_Change_Callback);

    $('#gridTitulosLiquidadosADM').jqGrid('clearGridData');

    return false;
}

function btnFiltroTitulosLiquidadosADM_Click() {
    var lUrl = Aux_UrlComRaiz("/TitulosLiquidados/TitulosLiquidados.aspx");

    var lDados =
        {
            Acao: "CarregarHtmlComDados",
            DataDe: $("#txtTitulosLiquidadosADMDataDe").val(),
            DataAte: $("#txtTitulosLiquidadosADMDataAte").val(),
            NomeFundo: $("#txtTitulosLiquidadosADMNomeFundo").val(),
            CodigoFundo: $("#txtTitulosLiquidadosADMCodigoFundo").val()
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnFiltroTitulosLiquidadosADM_Click_CallBack);

    $('#gridTitulosLiquidadosADM').jqGrid('clearGridData');

    return false;
}

function btnFiltroTitulosLiquidadosADM_Click_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_TitulosLiquidadosADM = { page: 1, total: 3, records: 0, rows: [] };

        if (lData.Itens.length) {
            for (i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        CodigoFundo: lData.Itens[i].CodigoFundo,
                        DataRef: lData.Itens[i].DataRef,
                        NomeFundo: lData.Itens[i].NomeFundo,
                        Valor: lData.Itens[i].Valor,
                    };

                gMock_TitulosLiquidadosADM.rows.push(lObjeto);
            }
        }

        $('#gridTitulosLiquidadosADM')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_TitulosLiquidadosADM.rows })
            .trigger("reloadGrid");
    }
}

function btnTitulosLiquidadosLimpar_Click() {
    var today = moment();
    var tomorrow = moment(today, 'DD MM YY').add(1, 'day');
    var yesterday = moment(today, 'DD MM YY').subtract(1, 'day');

    $("#txtTitulosLiquidadosADMCodigoFundo").val("");
    $("#txtTitulosLiquidadosADMNomeFundo").val("");

    $("#txtTitulosLiquidadosADMDataDe").val(yesterday.format('DD/MM/YY'));
    $("#txtTitulosLiquidadosADMDataAte").val(tomorrow.format('DD/MM/YY'));

    $("#cboCarteiraLocalidade").val(0);

    $('#gridTitulosLiquidadosADM').jqGrid('clearGridData');

    return false;
}

function InsertRowTitulosLiquidadosADM(rowid, pData) {
    var lGrid = $('#gridTitulosLiquidadosADM');

    if (pData.Status == 'N') {
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return TitulosLiquidados_BaixarManualmente(' + pData.CodigoFundo + ')" > </a><span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>', '');
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<a href="" onclick="return TitulosLiquidados_BaixarManualmente(' + pData.CodigoFundo + ')" >Baixar Manualmente</a>', 'glyphicons circle_arrow_down');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusNoOk');
    }
    else {
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div> <span class="glyphicon glyphicon-circle-arrow-down" style="padding-left:5px"></span></div>', '');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusOk');
    }
}

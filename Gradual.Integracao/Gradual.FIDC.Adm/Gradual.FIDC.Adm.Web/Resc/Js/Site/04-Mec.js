///Váriável global do grid de Mec
var gGridMec;

function Mec_ShowPainelPesquisa() {
    if ($("#Mec_pnlFiltroPesquisa").is(':hidden')) {
        $("#Mec_pnlFiltroPesquisa").show();
        $("#lnkShowHidePesquisa").find("i").removeClass("icon-plus").addClass("icon-minus");
    }
    else {
        $("#Mec_pnlFiltroPesquisa").hide();
        $("#lnkShowHidePesquisa").find("i").removeClass("icon-minus").addClass("icon-plus");
    }
}

var gMock_Mec = {};

function btnFiltroMecLimpar_Click()
{
    var today       = moment();
    var tomorrow    = moment(today, 'DD MM YY').add(1, 'day');
    var yesterday   = moment(today, 'DD MM YY').subtract(1, 'day');
    
    $("#txtMecDataDe").val(yesterday.format('DD/MM/YY'));
    $("#txtMecDataAte").val(tomorrow.format('DD/MM/YY'));
    $("#txtMecNomeFundo").val("");
    $("#txtMecCodigoFundo").val("");
    $("#cboMecLocalidade").val(0);
    $("#chkMecDownloadsPendentes").prop("checked", false);

    $('#gridMec').jqGrid('clearGridData');

    return false;
}

///Evento cliecl do botão buscar da página de Mec
function btnFiltroMec_Click()
{

    var lUrl = Aux_UrlComRaiz("/RoboDownload/Mec.aspx");

    var lDados =
        {
            Acao                : "CarregarHtmlComDados",
            DataDe              : $("#txtMecDataDe").val(),
            DataAte             : $("#txtMecDataAte").val(),
            NomeFundo           : $("#txtMecNomeFundo").val(),
            CodigoFundo         : $("#txtMecCodigoFundo").val(),
            CodigoLocalidade    : $("#cboMecLocalidade").val(),
            DownloadsPendentes  : $("#chkMecDownloadsPendentes").is(':checked'),
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnFiltroMec_Click_Callback);

    $('#gridMec').jqGrid('clearGridData');

    return false;
}

function btnFiltroMec_Click_Callback(pResposta)
{
    if (!pResposta.TemErro)
    {
        gMock_Mec = { page: 1, total: 3, records: 0, rows: [] };

        var lData = JSON.parse(pResposta);

        if (lData.Itens.length)
        {
            for (i = 0; i < lData.Itens.length; i++)
            {
                var lObjeto =
                    {
                        CodigoFundo: lData.Itens[i].CodigoFundo,
                        NomeFundo: lData.Itens[i].NomeFundo,
                        Categoria: lData.Itens[i].Categoria,
                        DownloadHora: lData.Itens[i].DownloadHora,
                        Status: lData.Itens[i].Status,
                        DownloadLink: lData.Itens[i].DownloadLink
                    };

                gMock_Mec.rows.push(lObjeto);
            }
        }

        $('#gridMec')
        .jqGrid('setGridParam',
        {
            datatype: 'local',
            data: gMock_Mec.rows
        })
        .trigger("reloadGrid");

        Aux_ExibirMensagem("I", "Retornou " + lData.Itens.length + " registros.", true);

    }

}

///Método que monta o grid de Mec
function Grid_Resultado_Mec() {

    gGridMec =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_Mec.rows
        , autowidth: true
        , shrinkToFit: true
        //, colNames: ["Codigo", "Fundo", "Categoria", "Download", "Status", ""]
      , colModel: [
                      //  { label: "Código"       , name: "CodigoFundo",    jsonmap: "CodigoFundo",     index: "CodigoFundo"  , width: 10 , align: "left"     , sortable: true },
                        { label: "Nome Fundo"   , name: "NomeFundo",      jsonmap: "NomeFundo",       index: "NomeFundo"    , width: 50 , align: "left"     , sortable: true }
                      , { label: "Cat."         , name: "Categoria",      jsonmap: "Categoria",       index: "Categoria"    , width: 10 , align: "left"     , sortable: true }
                      , { label: "Download"     , name: "DownloadHora",   jsonmap: "DownloadHora",    index: "DownloadHora" , width: 10 , align: "left"     , sortable: true }
                      , { label: "Status"       , name: "Status",         jsonmap: "Status",          index: "Status"       , width: 10 , align: "center"   , sortable: true }
                      , { label: " "            , name: "DownloadLink",   jsonmap: "DownloadLink",    index: "DownloadLink" , width: 20 , align: "left"     , sortable: true }

      ]
      , loadonce: true
      , height: 'auto'
      //, width: '100%'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , pager: '#pgMec'
      , subGrid: false
      , rowNum: 15
      , rowList: [10, 20, 30]
      , caption: 'Downloads Mec'
        //, subGridRowExpanded: subGridRowExpandendMec
        //, gridComplete: function () {
        //}
      , afterInsertRow: InsertRowMec
      , sortable: true
    };


    $('#gridMec').jqGrid(gGridMec).jqGrid('hideCol', "CodigoFundo");
    //gGridMec.jqGrid('hideCol', "CodigoFundo");
}

function InsertRowMec(rowid, pData) {

    var lGrid = $('#gridMec');

    if (pData.Status == 'N') {
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return Mec_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'mec\')" >Baixar Manualmente</a><span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>', '');
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<a href="" onclick="return Mec_BaixarManualmente(' + pData.CodigoFundo + ')" >Baixar Manualmente</a>', 'glyphicons circle_arrow_down');

        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div>Arquivo indisponível<span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusNoOk');
    } else {
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return Mec_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'mec\')" >Download Concluído</a><span class="glyphicon glyphicon-circle-arrow-down" style="padding-left:5px"></span></div>', '');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusOk');
    }

}

///Método que efetua o download o do arquivo de Mec
function Mec_BaixarManualmente(postData, pTypeName) {
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
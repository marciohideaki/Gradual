///Váriável global do grid de carteira
var gGridCarteiras;

function Carteiras_ShowPainelPesquisa() {
    if ($("#Carteiras_pnlFiltroPesquisa").is(':hidden')) {
        $("#Carteiras_pnlFiltroPesquisa").show();
        $("#lnkShowHidePesquisa").find("i").removeClass("icon-plus").addClass("icon-minus");
    }
    else {
        $("#Carteiras_pnlFiltroPesquisa").hide();
        $("#lnkShowHidePesquisa").find("i").removeClass("icon-minus").addClass("icon-plus");
    }
}

var gMock_Carteiras = {};

function btnFiltroCarteirasLimpar_Click()
{
    var today       = moment();
    var tomorrow    = moment(today, 'DD MM YY').add(1, 'day');
    var yesterday   = moment(today, 'DD MM YY').subtract(1, 'day');

    $("#txtCarteiraCodigoFundo").val("");
    $("#txtCarteiraNomeFundo").val("");

    $("#txtCarteiraDataDe").val(yesterday.format('DD/MM/YY'));
    $("#txtCarteiraDataAte").val(tomorrow.format('DD/MM/YY'));

    $("#cboCarteiraLocalidade").val(0);
    $("#chkCarteirasDownloadsPendentes").prop('checked', false);

    $('#gridCarteiras').jqGrid('clearGridData');

    return false;
}

///Evento click do botão buscar da página de carteiras
function btnFiltroCarteiras_Click()
{
    var lUrl = Aux_UrlComRaiz( "/RoboDownload/Carteiras.aspx");

    var lDados =
        {
            Acao                : "CarregarHtmlComDados",
            DataDe              : $("#txtCarteiraDataDe").val(),
            DataAte             : $("#txtCarteiraDataAte").val(),
            NomeFundo           : $("#txtCarteiraNomeFundo").val(),
            CodigoFundo         : $("#txtCarteiraCodigoFundo").val(),
            CodigoLocalidade    : $("#cboCarteiraLocalidade").val(),
            DownloadsPendentes  : $("#chkCarteirasDownloadsPendentes").is(':checked'),
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnFiltroCarteiras_Click_Callback);

    $('#gridCarteiras').jqGrid('clearGridData');

    return false;
}

function btnFiltroCarteiras_Click_Callback(pResposta)
{
    if (!pResposta.TemErro)
    {
        gMock_Carteiras = { page: 1, total: 3, records: 0, rows: [] };

        var lData = JSON.parse(pResposta);

        if (lData.Itens.length)
        {
            for (i = 0; i < lData.Itens.length; i++)
            {
                var lObjeto =
                    {
                        CodigoFundo:    lData.Itens[i].CodigoFundo,
                        NomeFundo:      lData.Itens[i].NomeFundo,
                        Categoria:      lData.Itens[i].Categoria,
                        DownloadHora:   lData.Itens[i].DownloadHora,
                        Status:         lData.Itens[i].Status,
                        DownloadLink:   lData.Itens[i].DownloadLink
                    };

                gMock_Carteiras.rows.push(lObjeto);
            }
        }

        $('#gridCarteiras')
        .jqGrid('setGridParam',
        {
            datatype: 'local',
            data    : gMock_Carteiras.rows
        })
        .trigger("reloadGrid");

        /*
        gMock_Carteiras = "page": "1", "total": 3, "records": "13", "rows": 

        $('#gridCarteiras').jqGrid('clearGridData');

        $('#gridCarteiras').jqGrid('setGridParam', { data: gMock_Carteiras.rows });

        $('#gridCarteiras').trigger('reloadGrid');
        */
        Aux_ExibirMensagem("I", "Retornou " +  lData.Itens.length + " registros.", true);

    }
}

///Método que monta o grid de carteiras
function Grid_Resultado_Carteiras() {

    gGridCarteiras = 
    {
        datatype: "jsonstring"
        , hoverrows     : true
        , datastr       : gMock_Carteiras.rows
        , autowidth     : true
        , shrinkToFit   : true
        , colModel: [
                      //  { label: "Código",      name: "CodigoFundo",    jsonmap: "CodigoFundo",     index: "CodigoFundo",   width: 1,       align: "left",   sortable: true },
                        { label: "Nome Fundo",  name: "NomeFundo",      jsonmap: "NomeFundo",       index: "NomeFundo",     width: 50,     align: "left",   sortable: true }
                      , { label: "Categoria",   name: "Categoria",      jsonmap: "Categoria",       index: "Categoria",     width: 10 ,     align: "left",   sortable: true }
                      , { label: "Download",    name: "DownloadHora",   jsonmap: "DownloadHora",    index: "DownloadHora",  width: 10 ,     align: "left",   sortable: true }
                      , { label: "Status",      name: "Status",         jsonmap: "Status",          index: "Status",        width: 10 ,     align: "center", sortable: true }
                      , { label: " ",           name: "DownloadLink",   jsonmap: "DownloadLink",    index: "DownloadLink",  width: 20,    align: "left",   sortable: true }

      ]
      , loadonce        : true
      , height          : 'auto'
      , rowNum          : 0
      , sortname        : "invid"
      , sortorder       : "desc"
      , viewrecords     : true
      , gridview        : false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , pager           : '#pgCarteiras'
      , subGrid         : false
      , rowNum          : 15
      , rowList         : [10, 20, 30]
      , caption         : 'Downloads Carteiras'
      , afterInsertRow: InsertRowCarteiras
      , sortable: true
    };

    $('#gridCarteiras').jqGrid(gGridCarteiras).jqGrid('hideCol', "CodigoFundo");
    //gGridCarteiras.jqGrid('hideCol', "CodigoFundo");
}

function InsertRowCarteiras(rowid, pData) {

    var lGrid = $('#gridCarteiras');

    if (pData.Status == 'N')
    {
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="javascript:void(0);" onclick=\"return Carteiras_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'carteira\')" >Baixar Manualmente</a><span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>', '');
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<a href="" onclick="return Carteiras_BaixarManualmente(' + pData.CodigoFundo + ')" >Baixar Manualmente</a>', 'glyphicons circle_arrow_down');

        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div>Arquivo indisponível<span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusNoOk');
    }
    else
    {
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="javascript:void(0);" onclick="return Carteiras_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'carteira\')" >Download Concluído</a><span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>', '');

        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div>Download Concluído<span class="glyphicon glyphicon-circle-arrow-down" style="padding-left:5px"></span></div>', '');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusOk');
    }
    
}

///Método que efetua o download o do arquivo de carteira
function Carteiras_BaixarManualmente(postData, pTypeName)
{
    var lUrl =Aux_UrlComRaiz( "/Handlers/DownloadHandler.ashx");
    
    var postFormStr = "<form method='POST' action='" + lUrl + "'>\n";

    postFormStr += "<input type='hidden' name='FileName' value='" +  postData + "'></input>";

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



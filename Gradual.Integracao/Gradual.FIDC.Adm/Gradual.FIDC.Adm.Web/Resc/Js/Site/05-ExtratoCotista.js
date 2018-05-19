///Váriável global do grid de carteira
var gGridExtratoCotista;

function ExtratoCotista_ShowPainelPesquisa() {
    if ($("#ExtratoCotista_pnlFiltroPesquisa").is(':hidden')) {
        $("#ExtratoCotista_pnlFiltroPesquisa").show();
        $("#lnkShowHidePesquisa").find("i").removeClass("icon-plus").addClass("icon-minus");
    }
    else {
        $("#ExtratoCotista_pnlFiltroPesquisa").hide();
        $("#lnkShowHidePesquisa").find("i").removeClass("icon-minus").addClass("icon-plus");
    }
}

var gMock_ExtratoCotista = {};

function btnFiltroExtratoCotistaLimpar_Click()
{
    var today       = moment();
    var tomorrow    = moment(today, 'DD MM YY').add(1, 'day');
    var yesterday   = moment(today, 'DD MM YY').subtract(1, 'day');
    
    $("#txtExtratoCotistaCodigoFundo").val("");
    $("#txtExtratoCotistaNomeFundo").val("");
    $("#txtExtratoCotistaCpfCnpj").val("");
    $("#txtExtratoCotistaDataDe").val(yesterday.format('DD/MM/YY'));
    $("#txtExtratoCotistaDataAte").val(tomorrow.format('DD/MM/YY'));
    $("#ContentPlaceHolder1_cboExtratoCotistaNomeCotista").val(0);
    $("#chkExtratoCotistaDownloadsPendentes").prop('checked', false);

    $('#gridExtratoCotista').jqGrid('clearGridData');

    return false;
}


///Evento cliecl do botão buscar da página de ExtratoCotista
function btnFiltroExtratoCotista_Click()
{
    var lUrl = Aux_UrlComRaiz("/RoboDownload/ExtratoCotista.aspx");

    var lDados =
        {
            Acao                : "CarregarHtmlComDados",
            Codigofundo         : $("txtExtratoCotistaCodigoFundo").val(),
            NomeFundo           : $("txtExtratoCotistaNomeFundo").val(),
            CpfCnpj             : $("#txtExtratoCotistaCpfCnpj").val(),
            Data                : $("#txtExtratoCotistaData").val(),
            CodigoCotista       : $(".cboExtratoCotistaNomeCotista").val(),
            DownloadsPendentes  : $("#chkExtratoCotistaDownloadsPendentes").is(':checked')
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, btnFiltroExtratoCotista_Click_Callback);

    $('#gridExtratoCotista').jqGrid('clearGridData');

    return false;
}

function btnFiltroExtratoCotista_Click_Callback(pResposta)
{
    if (!pResposta.TemErro)
    {
        gGridExtratoCotista = { page: 1, total: 3, records: 0, rows: [] };

        var lData = JSON.parse(pResposta);

        if (lData.Itens.length)
        {
            for (i = 0; i < lData.Itens.length; i++)
            {
                var lObjeto =
                    {
                        CpfCnpj     : lData.Itens[i].CpfCnpj,
                        NomeCotista : lData.Itens[i].NomeCotista,
                        NomeFundo   : lData.Itens[i].NomeFundo,
                        Status      : lData.Itens[i].Status,
                        DownloadLink: lData.Itens[i].DownloadLink,
                    };

                gGridExtratoCotista.rows.push(lObjeto);
            }
        }

        $('#gridExtratoCotista')
            .jqGrid('setGridParam', { datatype: 'local', data: gGridExtratoCotista.rows })
            .trigger("reloadGrid");
    }
}

///Método que monta o grid de ExtratoCotista
function Grid_Resultado_ExtratoCotista() 
{

    gGridExtratoCotista =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_ExtratoCotista.rows
        , autowidth: true
        , shrinkToFit: true
        //, colNames: ["Codigo", "Fundo", "Categoria", "Download", "Status", ""]
      , colModel: [
                        { label: "Cpf Cnpj",    name: "CpfCnpj",        jsonmap: "CpfCnpj",         index: "CpfCnpj",       width: 15, align: "left",      sortable: true }
                      , { label: "Nome Cotista",name: "NomeCotista",    jsonmap: "NomeCotista",     index: "NomeCotista",   width: 35, align: "left",      sortable: true }
                      , { label: "Nome Fundo",  name: "NomeFundo",      jsonmap: "NomeFundo",       index: "NomeFundo",     width: 30, align: "left",      sortable: true }
                      , { label: "Status",      name: "Status",         jsonmap: "Status",          index: "Status",        width: 10,  align: "center",    sortable: true }
                      , { label: " ",           name: "DownloadLink",   jsonmap: "DownloadLink",    index: "DownloadLink",  width: 20, align: "left",      sortable: true }

      ]
      , loadonce: true
      , height: 'auto'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , pager: '#pgExtratoCotista'
      , subGrid: false
      , rowNum: 15
      , rowList: [10, 20, 30]
      , caption: 'Downloads Extrato Cotista'
        //, subGridRowExpanded: subGridRowExpandendExtratoCotista
        //, gridComplete: function () {
        //}
      , afterInsertRow: InsertRowExtratoCotista
      , sortable: true
    };


    $('#gridExtratoCotista').jqGrid(gGridExtratoCotista).jqGrid('hideCol', "CodigoFundo");
    //gGridExtratoCotista.jqGrid('hideCol', "CodigoFundo");
}

function InsertRowExtratoCotista(rowid, pData) {

    var lGrid = $('#gridExtratoCotista');

    if (pData.Status == 'N') 
    {
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return ExtratoCotista_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'extrato\')" >Baixar Manualmente</a><span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>', '');
        //lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<a href="" onclick="return ExtratoCotista_BaixarManualmente(' + pData.CodigoFundo + ')" >Baixar Manualmente</a>', 'glyphicons circle_arrow_down');
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div>Arquivo indisponível<span class="glyphicon glyphicon-alert" style="padding-left:5px"></span></div>');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusNoOk');
    } else 
    {
        lGrid.jqGrid('setCell', rowid, 'DownloadLink', '<div><a href="" onclick="return ExtratoCotista_BaixarManualmente(\'' + encodeURIComponent(pData.DownloadLink) + '\',\'extrato\')" >Download Concluído</a><span class="glyphicon glyphicon-circle-arrow-down" style="padding-left:5px"></span></div>', '');

        lGrid.jqGrid('setCell', rowid, 'Status', '', 'ImageStatusOk');
    }

}

function ExtratoCotista_BaixarManualmente(postData, pTypeName)
{
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
/*
function ExtratoCotista_BaixarManualmente_Callback(pResposta) {

}
*/
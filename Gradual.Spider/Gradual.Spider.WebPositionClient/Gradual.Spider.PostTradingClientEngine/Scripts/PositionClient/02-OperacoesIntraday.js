/*----------------------------------------------------------------*/
/*Funções gerais da position client de risco - Operações Intraday---*/
/*----------------------------------------------------------------*/

function ConnectSocketServerOperacoesIntraday()
{
    var lSuport = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

    var lWsConnect = $("#hddConnectionSocketOperacoesIntraday").val();

    if (lSuport == null) {
        alert(NOSUPORTEMESSAGE);
        return;
    }

    var lStatusConnection = $("#lblStatusConnection");

    lStatusConnection.html("***  Connecting to server ....");

    gWebSocketOperacoesIntraday = new WebSocket(lWsConnect);//new window[lSuport](lWsConnect);

    ///Evento que é chamado quando chega mensagem de position client
    gWebSocketOperacoesIntraday.onmessage = function (evt) {
        var pMessage = evt.data;

        AppendMessageOperacoesIntraday(pMessage);
    }

    ///Evento que é chamado quando a conexão é aberta 
    gWebSocketOperacoesIntraday.onopen = function () {
        lStatusConnection.html("* Connection Opened!");
    }

    ///Evento que é chamado quando a conexão é fechada
    gWebSocketOperacoesIntraday.onclose = function () {
        lStatusConnection.html("** Connection closed!");
    }
}

//Método que recebe a mensagem do websocket e trata de acordo 
//com o tipo de mensagem e insere/altera
//no grid específico
function AppendMessageOperacoesIntraday(pMessage) {

    var lGrid = $("#jqGrid_OperacoesIntraday");

    //var lTipo = RetornaAbaAtiva();
    var lData;
    var lRowID;

    if (pMessage == "") return;

    var lJSON = $.parseJSON(pMessage);

    //var lMessage = lJSON[gLastClientSearchedOperacoesIntraday];

    //if (lMessage.Value == null) {
        //console.log("Chegou Mensagem Tamanho ->" + lMessage.length);

        //for (i = 0; i < lMessage.length; i++) {
        var lData = lJSON; //lMessage[i];

        var lDataOperacaoIntraday = TransporteDataOperacoesIntraday(lData);

        console.log("Chegou Dados->" + lDataOperacaoIntraday.CodigoCliente + " Ativo:" + lDataOperacaoIntraday.CodigoInstrumento);

        OperacoesIntradaySetRowDataObjectonGrid(lGrid, lDataOperacaoIntraday);
        //}
    //}


}

/*----------------------------------------------------------------*/
/*--Controle Risco--Operações Intraday-----------------------------*/
/*----------------------------------------------------------------*/

///Ultimo Filtro efetualdo
var gLastClientSearchedOperacoesIntraday;

//Grids de Position Client montagem
var gGridOperacoesIntraday;

var gOperacoesIntradayTimerRefreshGrid;

var gOperacoesIntradayTimerAtualizacaoAutomatica = 0;

/*
Dados mock de preenchimento de grid de Operações Intraday
*/
var lMock_Operacoes_Intraday = {
    "page": "1", "total": 3, "records": "13", "rows": [
        /*
                       { CodigoCliente: 31940, CodigoInstrumento: "PETR4", Mercado: "Vis", Vencimento: '-', QuantAbertura: 200, QuantTotal: 700, PL: "2300", QuantExecutadaCompra: 600, QuantExecutadaVenda: -100, QuantExecutadaNet: 500, QuantAbertaCompra: 0, QuantAbertaVenda: 0, QuantAbertaNet: 0, PrecoMedioCompra: 4.54, PrecoMedioVenda: 4.53, VolumeCompra: 3000, VolumeVenda: 5500, VolumeTotal: 8500 },
                       { CodigoCliente: 31873, CodigoInstrumento: "VALE5", Mercado: "Vis", Vencimento: '-', QuantAbertura: 200, QuantTotal: 700, PL: "2300", QuantExecutadaCompra: 600, QuantExecutadaVenda: -100, QuantExecutadaNet: 500, QuantAbertaCompra: 0, QuantAbertaVenda: 0, QuantAbertaNet: 0, PrecoMedioCompra: 4.54, PrecoMedioVenda: 4.53, VolumeCompra: 3000, VolumeVenda: 5500, VolumeTotal: 8500 },
                       { CodigoCliente: 57125, CodigoInstrumento: "BVMF3", Mercado: "Vis", Vencimento: '-', QuantAbertura: 200, QuantTotal: 700, PL: "2300", QuantExecutadaCompra: 600, QuantExecutadaVenda: -100, QuantExecutadaNet: 500, QuantAbertaCompra: 0, QuantAbertaVenda: 0, QuantAbertaNet: 0, PrecoMedioCompra: 4.54, PrecoMedioVenda: 4.53, VolumeCompra: 3000, VolumeVenda: 5500, VolumeTotal: 8500 },
                       { CodigoCliente: 5355, CodigoInstrumento: "PETR3", Mercado: "Vis", Vencimento: '-', QuantAbertura: 200, QuantTotal: 700, PL: "2300", QuantExecutadaCompra: 600, QuantExecutadaVenda: -100, QuantExecutadaNet: 500, QuantAbertaCompra: 0, QuantAbertaVenda: 0, QuantAbertaNet: 0, PrecoMedioCompra: 4.54, PrecoMedioVenda: 4.53, VolumeCompra: 3000, VolumeVenda: 5500, VolumeTotal: 8500 },
                       { CodigoCliente: 52174, CodigoInstrumento: "ITSA4", Mercado: "Vis", Vencimento: '-', QuantAbertura: 200, QuantTotal: 700, PL: "2300", QuantExecutadaCompra: 600, QuantExecutadaVenda: -100, QuantExecutadaNet: 500, QuantAbertaCompra: 0, QuantAbertaVenda: 0, QuantAbertaNet: 0, PrecoMedioCompra: 4.54, PrecoMedioVenda: 4.53, VolumeCompra: 3000, VolumeVenda: 5500, VolumeTotal: 8500 },
                       { CodigoCliente: 31217, CodigoInstrumento: "INDH16", Mercado: "Fut", Vencimento: '15/03', QuantAbertura: 0, QuantTotal: -10, PL: "-4900", QuantExecutadaCompra: 40, QuantExecutadaVenda: 50, QuantExecutadaNet: -10, QuantAbertaCompra: 0, QuantAbertaVenda: 0, QuantAbertaNet: 0, PrecoMedioCompra: 40100, PrecoMedioVenda: 40050, VolumeCompra: 3000, VolumeVenda: 5500, VolumeTotal: 8500 },*/
    ]
};

///Função de gerenciamento de carregamento de grid de Operações Intraday
function PositionClient_Operacoes_Intraday_Grid() {
    gGridOperacoesIntraday =
    {
        datatype: "jsonstring"
        //, mtype: "GET"
      , hoverrows: true
      , datastr: lMock_Operacoes_Intraday.rows
      , autowidth: false
        //, shrinkToFit: false
      , colModel: [
                        { label: "Conta",       name: "CodigoCliente",          jsonmap: "CodigoCliente",           index: "CodigoCliente",         width: 65, align: "center", sortable: true }
                      , { label: "Ativo",       name: "CodigoInstrumento",      jsonmap: "CodigoInstrumento",       index: "CodigoInstrumento",     width: 65, align: "center", sortable: true }
                      , { label: "Merc",        name: "Mercado",                jsonmap: "Mercado",                 index: "Mercado",               width: 55, align: "center", sortable: true }
//                      , { label: "Vcto",      name: "Vencimento",             jsonmap: "Vencimento",              index: "Vencimento",            width: 75, align: "center", sortable: true }
                      , { label: "Qtd.Aber",    name: "QuantAbertura",          jsonmap: "QuantAbertura",           index: "QuantAbertura",         width: 75, align: "center", sortable: true ,formatter: 'number', formatoptions:{ decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Qtd.Total",   name: "QuantTotal",             jsonmap: "QuantTotal",              index: "QuantTotal",            width: 75, align: "center", sortable: true ,formatter: 'number', formatoptions:{ decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "P&L",         name: "PL",                     jsonmap: "PL",                      index: "PL",                    width: 75, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2,  }}
                      , { label: "Qt.Exec.C",   name: "QuantExecutadaCompra",   jsonmap: "QuantExecutadaCompra",    index: "QuantExecutadaCompra",  width: 75, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Qt.Exec.V",   name: "QuantExecutadaVenda",    jsonmap: "QuantExecutadaVenda",     index: "QuantExecutadaVenda",   width: 75, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Net Qtde.",   name: "QuantExecutadaNet",      jsonmap: "QuantExecutadaNet",       index: "QuantExecutadaNet",     width: 85, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Book Cpa",    name: "QuantAbertaCompra",      jsonmap: "QuantAbertaCompra",       index: "QuantAbertaCompra",     width: 65, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Book Vda",    name: "QuantAbertaVenda",       jsonmap: "QuantAbertaVenda",        index: "QuantAbertaVenda",      width: 65, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Net Book",    name: "QuantAbertaNet",         jsonmap: "QuantAbertaNet",          index: "QuantAbertaNet",        width: 65, align: "center", sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0,  }}
                      , { label: "Pç.Med.Cpa",  name: "PrecoMedioCompra",       jsonmap: "PrecoMedioCompra",        index: "PrecoMedioCompra",      width: 65, align: "center", sortable: true ,formatter: 'number', formatoptions: {  thousandsSeparator: ".",   }}
                      , { label: "Pç.Med.Vda",  name: "PrecoMedioVenda",        jsonmap: "PrecoMedioVenda",         index: "PrecoMedioVenda",       width: 65, align: "center", sortable: true ,formatter: 'number', formatoptions: {  thousandsSeparator: ".",   }}
                      , { label: "Vol Cpa",     name: "VolumeCompra",           jsonmap: "VolumeCompra",            index: "VolumeCompra",          width: 85, align: "center", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, prefix: 'R$ ' }, }
                      , { label: "Vol Vda",     name: "VolumeVenda",            jsonmap: "VolumeVenda",             index: "VolumeVenda",           width: 85, align: "center", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, prefix: 'R$ ' }, }
                      , { label: "Vol Tot",     name: "VolumeTotal",            jsonmap: "VolumeTotal",             index: "VolumeTotal",           width: 85, align: "center", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, prefix: 'R$ ' } }

      ]
      , height: 'auto'
        //, width: '100%'
        , width: '1130'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
        //, pager: '#jqGrid_OperacoesIntraday_Pager'
      , rowNum: 1000
        //, rowList: [10, 20, 30]
      , caption: 'Operações Intraday'
      , multiselect: true
      , loadComplete: function () { }
      , afterInsertRow: SetColorOperacoesIntradayInsertRow
      , beforeSelectRow: Operacoes_Intraday_beforeSelectRow
      , onSelectRow: Operacoes_Intraday_SelectRow
      , sortable: true
    };
}

function Operacoes_Intraday_beforeSelectRow(rowid, e) {
    e = false;
}

//Método disparado ao selecionar a linha no Grid
function Operacoes_Intraday_SelectRow(rowid, status, e) {
    var rowData = jQuery(this).getRowData(rowid);

    var ch = jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked');

    if (ch) {
        jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked', false);
    }
    else {
        jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked', true);
    }

    var lCodigoCliente = rowData.CodigoCliente;

    var lCodigoInstrumento = rowData.CodigoInstrumento;

    var lValorAtual = $("#hddOperacoes_Intraday_Seleted_Row").val();

    if ((lValorAtual.indexOf("|" + lCodigoCliente + lCodigoInstrumento) == -1)) {
        $("#hddOperacoes_Intraday_Seleted_Row").val(lValorAtual + "|" + lCodigoCliente + lCodigoInstrumento);
    }
    else {
        lValorAtual.replace("|" + lCodigoCliente + lCodigoInstrumento, "");
        $("#hddOperacoes_Intraday_Seleted_Row").val(lValorAtual);
    }

}

///Método para mudar a cor das colunas do grid de Operações Intraday
function SetColorOperacoesIntradayInsertRow(rowid, pData) {

    var lColorRed    = "#FF3A44";
    var lColorYelow  = "#F9F9BA";
    var lColorBlue   = "#C4D8F3";
    var lColorGreen  = "#C8D7C1";
    var lColorOrange = "#F3DEC8";
    var lColorAqua   = "#E1EBF8";

    var lColorFont = "#2e7db2";
    var lColorFontWhite = "#FFFFFF";

    var lGrid = $("#jqGrid_OperacoesIntraday");

    lGrid.jqGrid('setCell', rowid, 'PL', '', { 'background-color': lColorYelow, color: lColorFont });

    lGrid.jqGrid('setCell', rowid, 'QuantExecutadaCompra', '', { 'background-color': lColorBlue, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'QuantExecutadaVenda', '', { 'background-color': lColorBlue, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'QuantExecutadaNet', '', { 'background-color': lColorBlue, color: lColorFont, 'font-weight': 'bold' });

    lGrid.jqGrid('setCell', rowid, 'QuantAbertaCompra', '', { 'background-color': lColorGreen, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'QuantAbertaVenda', '', { 'background-color': lColorGreen, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'QuantAbertaNet', '', { 'background-color': lColorGreen, color: lColorFont, 'font-weight': 'bold' });

    lGrid.jqGrid('setCell', rowid, 'PrecoMedioCompra', '', { 'background-color': lColorOrange, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'PrecoMedioVenda', '', { 'background-color': lColorOrange, color: lColorFont });

    lGrid.jqGrid('setCell', rowid, 'VolumeCompra', '', { 'background-color': lColorAqua, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'VolumeVenda', '', { 'background-color': lColorAqua, color: lColorFont });
    lGrid.jqGrid('setCell', rowid, 'VolumeTotal', '', { 'background-color': lColorAqua, color: lColorFont });

    lGrid.jqGrid('setLabel', 'PL', false, { 'background-color': lColorYelow });
    /*
    
    lGrid.jqGrid('setLabel', 'QuantExecutadaCompra', false,     { 'background': lColorBlue });
    lGrid.jqGrid('setLabel', 'QuantExecutadaVenda', false,      { 'background': lColorBlue });
    lGrid.jqGrid('setLabel', 'QuantExecutadaNet', false,        { 'background': lColorBlue });

    lGrid.jqGrid('setLabel', 'QuantAbertaCompra', false,        { 'background': lColorGreen });
    lGrid.jqGrid('setLabel', 'QuantAbertaVenda', false,         { 'background': lColorGreen });
    lGrid.jqGrid('setLabel', 'QuantAbertaNet', false,           { 'background': lColorGreen });

    lGrid.jqGrid('setLabel', 'PrecoMedioCompra', false,         { 'background': lColorOrange });
    lGrid.jqGrid('setLabel', 'PrecoMedioVenda', false,          { 'background': lColorOrange });

    lGrid.jqGrid('setLabel', 'VolumeCompra', false,                { 'background': lColorAqua });
    lGrid.jqGrid('setLabel', 'VolumeVenda', false,                 { 'background': lColorAqua });
    lGrid.jqGrid('setLabel', 'VolumeTotal', false,                 { 'background': lColorAqua });
    */
    if (pData.Vencimento == '01/01/0001')
    {

        pData.Vencimento = '-';

        lGrid.jqGrid('setRowData', rowid, pData);
    }

    if (pData.PL.toString().indexOf('-') > -1)
    {
        lGrid.jqGrid('setCell', rowid, 'PL', '', { 'background-color': lColorRed, 'color': lColorFontWhite });
    }

    if (pData.QuantExecutadaNet.toString().indexOf('-') > -1) {
        lGrid.jqGrid('setCell', rowid, 'QuantExecutadaNet', '', { 'background-color': lColorBlue, color: lColorRed, 'font-weight': 'bold' });
    }

    if (pData.QuantAbertaNet.toString().indexOf('-') > -1) {
        lGrid.jqGrid('setCell', rowid, 'QuantAbertaNet', '', { 'background-color': lColorGreen, color: lColorRed, 'font-weight': 'bold' });
    }

    var lCheck = lGrid.find('#' + rowid + ' input[type=checkbox]');

    lCheck.click(Operacoes_Intraday_Grid_DeSelectRow);

    var lValorAtual = $("#hddOperacoes_Intraday_Seleted_Row").val();

    if ((lValorAtual.indexOf(pData.CodigoCliente) > -1) && (lValorAtual.indexOf(pData.CodigoInstrumento) > -1)) {
        lGrid.setSelection(rowid, true);

        jQuery(this).find('#' + rowid + ' input[type=checkbox]').attr('checked', true);
    }
}

//Método disparado ao deselecionar a linha do grid
function Operacoes_Intraday_Grid_DeSelectRow() {
    var lCodigoCliente = $(this).closest("tr").find("td").eq(1).html();

    var lCodigoInstrumento = $(this).closest("tr").find("td").eq(2).html();

    var lID = $(this).closest("tr").attr("id");

    var lValorAtual = $("#hddOperacoes_Intraday_Seleted_Row").val();

    if ($(this).is(":checked")) {
        if ((lValorAtual.indexOf("|" + lCodigoCliente + lCodigoInstrumento) == -1)) {
            $("#hddOperacoes_Intraday_Seleted_Row").val(lValorAtual + "|" + lCodigoCliente + lCodigoInstrumento);
        }
    }
    else {
        var lNovoValor = lValorAtual.replace("|" + lCodigoCliente + lCodigoInstrumento, "");

        $("#hddOperacoes_Intraday_Seleted_Row").val(lNovoValor);
    }

    jQuery("#jqGrid_OperacoesIntraday").setSelection(lID, false);
}

///Método que prepara a grid de operações intraday com o layout 
///de colunas e funcionalidades de Footer
function PreparaGridOperacoesIntraday() {
    var lGrid = $("#jqGrid_OperacoesIntraday");

    lGrid.jqGrid(gGridOperacoesIntraday);

    lGrid.jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [{ startColumnName: 'CodigoCliente', numberOfColumns: 4, titleText: '' },
                       { startColumnName: 'QuantAbertura', numberOfColumns: 2, titleText: '<em>Saldo Custódia</em>' },
                       { startColumnName: 'PL', numberOfColumns: 1, titleText: '<em>P&L</em>' },
                       { startColumnName: 'QuantExecutadaCompra', numberOfColumns: 3, titleText: '<em>Qtde Exec. Intraday</em>' },
                       { startColumnName: 'QuantAbertaCompra', numberOfColumns: 3, titleText: '<em>Qtde na Pedra </em>' },
                       { startColumnName: 'PrecoMedioCompra', numberOfColumns: 2, titleText: '<em>Pç Médio Exec.</em>' },
                       { startColumnName: 'VolumeCompra', numberOfColumns: 3, titleText: '<em>Volume Operado</em>' }, ]
    });
    /*
    lGrid.jqGrid('navGrid', '#jqGrid_OperacoesIntraday_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });
        */
    lGrid.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });
    /*
    lGrid.jqGrid('navButtonAdd', '#jqGrid_OperacoesIntraday_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGrid[0].toggleToolbar(); }
    });
    */
    lGrid.jqGrid('navButtonAdd', '#jqGrid_OperacoesIntraday_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_OperacoesIntraday', 'GeneralView.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGrid[0].toggleToolbar();
}

var gLastClientSignedOperacoesIntraday;

///Evento Keydown para ativar filtro por cliente
function txtOperacoesIntraday_Cliente_KeyDown(pSender)
{
    $(pSender).one('keypress', function (e)
    {
        if (e.keyCode == 13)
        {
            var lSenderValue = $(pSender).val();

            var lSender = $(pSender);

            var CodigoCliente = $("#txtOperacoesIntradayCliente").val();

            $("#jqGrid_OperacoesIntraday").find(".ui-jqgrid-title").html("Operações Intraday-> Código Cliente: " + CodigoCliente);

            OperacoesIntraday_BuscarDados();

            return false;
        }
    });
}

///Método que envia solicitação de assinatura para a sessão da conexão 
function SendMessageSignatureOperacoesIntraday(CodigoCliente)
{
    if (gWebSocketOperacoesIntraday)
    {
        var lMessageToSend = "SUBSCRIBE " + CodigoCliente;
        gWebSocketOperacoesIntraday.send(lMessageToSend);

        gLastClientSearchedOperacoesIntraday = CodigoCliente;
    }
}

///Remove a assinatura do ultimo cliente assinado
function SendMessageUnsubiscribeOperacoesIntraday()
{
    if (gWebSocketOperacoesIntraday && gLastClientSignedOperacoesIntraday != "") {
        lMessageToSend = "UNSUBSCRIBE " + gLastClientSignedOperacoesIntraday;

        //$("#jqGrid_OperacoesIntraday").jqGrid('clearGridData');

        gWebSocketOperacoesIntraday.send(lMessageToSend);
    }
}

///Evento keydown para ativar filtro Ativo
function txtOperacoesIntraday_Ativo_KeyDown(pSender) {
    $(pSender).one('keypress', function (e) {
        if (e.keyCode == 13) {
            var lSenderValue = $(pSender).val();
            var lSender = $(pSender);

            if (!ValidarCampoBuscaCodigoCliente(pSender)) {
                return;
            }

            var CodigoCliente;

            switch (lSender.attr('id')) {
                case "txtOperacoesIntradayCliente":
                    {
                        CodigoCliente = $("#txtOperacoesIntradayCliente").val();

                        //Grid General View
                        $("#jqGrid_OperacoesIntraday")
                            .find(".ui-jqgrid-title")
                            .html("Operações Intraday-> Código Cliente: " + CodigoCliente);

                        SendMessageUnsubiscribe();
                        SendMessageSignature(CodigoCliente);
                        gLastClientSearchedGeneralView = CodigoCliente;
                    }
                    break;
            }

            return false;
        }
    });
}

///Remove os filtros de market
function OperacoesIntradayRemoveFiltersMarket() {

    $("#chkOperacoesIntradayTodosMercados").prop('checked', false);
    $("#chkOperacoesIntradayAvista").prop('checked', false);
    $("#chkOperacoesIntradayFuturo").prop('checked', false);
    $("#chkOperacoesIntradayOpcoes").prop('checked', false);

    return false;
}

///Remove os filtros de Parametros de intraday
function OperacoesIntradayRemoveFiltersParametrosIntraday() {
    $("#chkOperacoesIntradayOfertasPedra").prop('checked', false);
    $("#chkOperacoesIntradayNetIntradiarioNegativo").prop('checked', false);
    $("chk#OperacoesIntradayPLNegativo").prop('checked', false);

    return false;
}

function OperacoesIntraday_Remover_Filtros_Click() {
    $('input').filter(':text').val("");
    $('input').filter(':checkbox').prop('checked', false);

    return false;
}

///Transporte da grid de operações Intraday
function TransporteDataOperacoesIntraday(pData) {
    var lRetorno =
    {
        CodigoCliente: '',
        CodigoInstrumento: '',
        Mercado: '',
        Vencimento: '',
        QuantAbertura: '',
        QuantTotal: '',
        PL: '',
        QuantExecutadaCompra: '',
        QuantExecutadaVenda: '',
        QuantExecutadaNet: '',
        QuantAbertaCompra: '',
        QuantAbertaVenda: '',
        QuantAbertaNet: '',
        PrecoMedioCompra: '',
        PrecoMedioVenda: '',
        VolumeCompra: '',
        VolumeVenda: '',
        VolumeTotal: '',
    };

    lRetorno.CodigoCliente = pData.Account;
    lRetorno.CodigoInstrumento = pData.Ativo;
    lRetorno.Mercado = pData.TipoMercado;

    lRetorno.QuantAbertura = pData.QtdAbertura;
    lRetorno.QuantTotal = pData.QtdTotal;
    lRetorno.PL = pData.LucroPrej.toFixed(2);

    var date = new Date(pData.DtVencimento);

    if (date.getFullYear() != 4000 &&
        date.getFullYear() != 9999 &&
        date.getFullYear() != 0) {
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();
        lRetorno.Vencimento = (day + '/' + (monthIndex + 1) + '/' + year);
    }
    else {
        lRetorno.Vencimento = '-';
    }

    lRetorno.QuantExecutadaCompra = pData.QtdExecC;
    lRetorno.QuantExecutadaVenda = pData.QtdExecV;
    lRetorno.QuantExecutadaNet = pData.NetExec;

    lRetorno.QuantAbertaCompra = pData.QtdAbC;
    lRetorno.QuantAbertaVenda = pData.QtdAbV;
    lRetorno.QuantAbertaNet = pData.NetAb;

    lRetorno.PrecoMedioCompra = pData.PcMedC.toFixed(2);
    lRetorno.PrecoMedioVenda = pData.PcMedV.toFixed(2);

    lRetorno.VolumeCompra = pData.VolCompra;
    lRetorno.VolumeVenda = pData.VolVenda;
    lRetorno.VolumeTotal = pData.VolTotal;//(pData.QtdExecC + pData.QtdExecV);
    /*
    lRetorno.AveragePriceBuy = pData.PcMedC.toFixed(2);
    lRetorno.ExecQtySell = pData.QtdExecV;
    lRetorno.AveragePriceSell = pData.PcMedV.toFixed(2);
    lRetorno.Realized_PL = pData.LucroPrej.toFixed(2);
    lRetorno.NET_Qty = pData.NetExec;
    lRetorno.Total_Qty = (pData.QtdExecC + pData.QtdExecV);
    lRetorno.Settlement_Amount = (pData.QtdExecC - pData.QtdExecV);
    lRetorno.DtMovimento = pData.DtMovimento.replace('T', ' ').substring(0, 23);

    var datePosicao = new Date(pData.DtPosicao);

    if (pData.TipoMercado == 'OPV' ||
        pData.TipoMercado == 'OPC') {
        datePosicao.setDate(datePosicao.getDate() + 1);
    }
    else {
        datePosicao.setDate(datePosicao.getDate() + 3);
    }

    lRetorno.Settlement_Date =
        (datePosicao.getDate() + "/" +
        (datePosicao.getMonth() + 1) + "/" +
        datePosicao.getFullYear())

    //lMock_GeneralView.rows.push(lRetorno);
    */
    return lRetorno;
}

///Método para inserir ou editar os dados nas grids
function OperacoesIntradaySetRowDataObjectonGrid(pGrid, pObjeto) {

    var lEncontrou = false;

    var rowIds = pGrid.jqGrid('getDataIDs');

    for (row = 0; row <= rowIds.length; row++) {

        rowData = pGrid.jqGrid('getRowData', row);

        if (rowData['CodigoInstrumento'] == undefined) {
            continue;
        }

        /*var lAplicaFiltro = AplicaFiltroAtivadoSocket(pObjeto);

        if (lAplicaFiltro === false) {

            lEncontrou = true;

            break;
        }
        */
        if (rowData['CodigoInstrumento'] == pObjeto.CodigoInstrumento &&
            rowData['CodigoCliente'] == pObjeto.CodigoCliente) {

            var lRow = jQuery('#jqGrid_OperacoesIntraday tr:eq(' + row + ')');

            pGrid.jqGrid('setRowData', row, pObjeto);

            lRow.closest("tr").find("td:gt(0)").effect("highlight", {}, 300);

            lEncontrou = true;


            break;
        }

    }

    if (!lEncontrou) {

        var lastRowDOM = pGrid.jqGrid('getGridParam', 'records');

        lRowID = lastRowDOM + 1;

        pGrid.addRowData(lRowID, pObjeto);
    }
    //timer = setInterval(function () { pGrid.trigger('reloadGrid', [{ current: true }]) }, 5000);
    //pGrid.trigger("reloadGrid");

    //return lRetorno;
}

///Método que aplica o filtro na mensagem de socket de 
//operações intraday que acabou de chegar.
function AplicaFiltroAtivadoSocket(pObjeto) {
    var lFiltro = MontaOperacoesIntradayRequest();

    if (pObjeto.Ativo != "" && pObjeto.Ativo == lFiltro.CodigoInstrumento) {
        return false;
    }


    if (lFiltro.OpcaoMarketAVista) {
        if (pObjeto.TipoMercado.toLowerCase() != "vis") {
            return false;
        }
    }

    if (lFiltro.OpcaoMarketFuturos) {
        if (pObjeto.TipoMercado.toLowerCase() != "dis" ||
            pObjeto.TipoMercado.toLowerCase() != "fut" ||
            pObjeto.TipoMercado.toLowerCase() != "opf") {
            return false;
        }
    }

    if (lFiltro.OpcaoParametroIntradayNetNegativo) {
        if (pObjeto.QuantExecutadaNet >= 0) {
            return false;
        }
    }

    if (lFiltro.OpcaoParametroIntradayOfertasPedra) {
        if (pObjeto.QuantAbertaNet <= 0) {
            return false;
        }
    }

    if (lFiltro.OpcaoParametroIntradayPLNegativo) {
        if (pObjeto.PL >= 0) {
            return false;
        }
    }
    return true;
}

///Aplica Todos os filtros ativos na tela de Operacoes Intraday
function OperacoesIntraday_Aplicar_Filtros_Click(pSender)
{
    OperacoesIntraday_BuscarDados();

    return false;
}

///Método responsável por buscar dados no 
function OperacoesIntraday_BuscarDados()
{
    if (!OperacoesIntraday_Validar_filtro()) {
        return false;
    }

    LimpaLinhasSelecionadasAnteriormente();

    gOperacoesIntradayTimerAtualizacaoAutomatica = 5;

    window.clearTimeout(gOperacoesIntradayTimerRefreshGrid);

    OperacoesIntraday_TemporizadorRefresh_Grid();

    $('#jqGrid_OperacoesIntraday').jqGrid('clearGridData');
}

///Métod que limpa as linhas selecionadas na grid para monitoramento
function LimpaLinhasSelecionadasAnteriormente()
{
    $("#hddOperacoes_Intraday_Seleted_Row").val('');
}

///Método que inicializa o temporizador de refresh na grid de operações intraday
function OperacoesIntraday_TemporizadorRefresh_Grid() {

    var lCarregarComSocket = $("#chkCarregarComSocketOperacoesIntraday").is(":checked");
    var lRequestSocket;

    var lRequest = MontaOperacoesIntradayRequestREST();

    if (!OperacoesIntraday_Validar_filtro())
    {
        window.clearTimeout(gOperacoesIntradayTimerRefreshGrid);

        return;
    }


    $('#jqGrid_OperacoesIntraday').jqGrid('clearGridData');

    if (lCarregarComSocket)
    {
        lRequestSocket = JSON.stringify(lRequest);

        SendMessageSignatureOperacoesIntraday(lRequestSocket);
    }
    else
    {

        var lHorarioAtual = new Date();

        gOperacoesIntradayTimerRefreshGrid = 0;

        window.clearTimeout(gOperacoesIntradayTimerRefreshGrid);

        gOperacoesIntradayTimerRefreshGrid = window.setTimeout("OperacoesIntraday_Verifica_Refresh()", 5000);

        OperacoesIntraday_Refresh_Bind_Grid();
    }
}



function OperacoesIntraday_Verifica_Refresh() {
    if ((gOperacoesIntradayTimerAtualizacaoAutomatica - 1) >= 0) {
        gOperacoesIntradayTimerAtualizacaoAutomatica = gOperacoesIntradayTimerAtualizacaoAutomatica - 1;

        setTimeout('OperacoesIntraday_Verifica_Refresh()', 1000);
    } else {
        OperacoesIntraday_TemporizadorRefresh_Grid();
    }
}

function OperacoesIntraday_Refresh_Bind_Grid() {

    //-->> Verifica se está setado o cliente ou o papel, se não estiver, 
    //-->> para de efetuar o request no REST e para de atualizar a GRID
    if (!OperacoesIntraday_Validar_filtro()) {

        //gOperacoesIntradayTimerAtualizacaoAutomatica = 5;

        clearTimeout(gOperacoesIntradayTimerRefreshGrid);

        $('#jqGrid_OperacoesIntraday').jqGrid('clearGridData');

        return;
    }

    gOperacoesIntradayTimerAtualizacaoAutomatica = 5;

    OperacoesIntradayRESTService();
}

///Método que retorna o Filtro configurado pelo usuário em operações intraday
function MontaOperacoesIntradayRequest() {
    var lRetorno =
        {
            acao: '',
            CodigoCliente                       : $("#txtOperacoesIntradayCliente").val(),
            CodigoInstrumento                   : $("#txtOperacoesIntradayAtivo").val().toUpperCase(),
            OpcaoMarketTodosMercados            : $("#chkOperacoesIntradayTodosMercados").is(':checked'),
            OpcaoMarketAVista                   : $("#chkOperacoesIntradayAvista").is(':checked'),
            OpcaoMarketFuturos                  : $("#chkOperacoesIntradayFuturo").is(':checked'),
            OpcaoMarketOpcao                    : $("#chkOperacoesIntradayOpcoes").is(':checked'),
            OpcaoParametroIntradayOfertasPedra  : $("#chkOperacoesIntradayOfertasPedra").is(':checked'),
            OpcaoParametroIntradayNetNegativo   : $("#chkOperacoesIntradayNetIntradiarioNegativo").is(':checked'),
            OpcaoParametroIntradayPLNegativo    : $("#chkOperacoesIntradayPLNegativo").is(':checked')
        };

    return lRetorno;
}

///Método que retorna o Filtro configurado pelo usuário em operações intraday
function MontaOperacoesIntradayRequestREST() {
    var lRetorno =
        {
            CodigoCliente                       : $("#txtOperacoesIntradayCliente").val() == "" ? 0 : $("#txtOperacoesIntradayCliente").val(),
            CodigoInstrumento                   : $("#txtOperacoesIntradayAtivo").val().toUpperCase(),
            OpcaoMarketTodosMercados            : $("#chkOperacoesIntradayTodosMercados").is(':checked'),
            OpcaoMarketAVista                   : $("#chkOperacoesIntradayAvista").is(':checked'),
            OpcaoMarketFuturos                  : $("#chkOperacoesIntradayFuturo").is(':checked'),
            OpcaoMarketOpcao                    : $("#chkOperacoesIntradayOpcoes").is(':checked'),
            OpcaoParametroIntradayOfertasPedra  : $("#chkOperacoesIntradayOfertasPedra").is(':checked'),
            OpcaoParametroIntradayNetNegativo   : $("#chkOperacoesIntradayNetIntradiarioNegativo").is(':checked'),
            OpcaoParametroIntradayPLNegativo    : $("#chkOperacoesIntradayPLNegativo").is(':checked')
        };

    return lRetorno;
}

function OperacoesIntraday_Verifica_Refresh_Callback(pResposta) {
    if (!pResposta.TemErro) {
        lMock_Operacoes_Intraday = { "page": "1", "total": 3, "records": pResposta.ObjetoDeRetorno.length, "rows": pResposta.ObjetoDeRetorno };

        $('#jqGrid_OperacoesIntraday').jqGrid('clearGridData');

        $('#jqGrid_OperacoesIntraday').jqGrid('setGridParam', { data: lMock_Operacoes_Intraday.rows });

        $('#jqGrid_OperacoesIntraday').trigger('reloadGrid');
    }

    var CodigoCliente = $("#txtOperacoesIntradayCliente").val();

    SendMessageUnsubiscribeOperacoesIntraday();

    if (CodigoCliente != "") {
        //Grid Operações Intraday
        $("#jqGrid_OperacoesIntraday")
            .find(".ui-jqgrid-title")
            .html("Operações Intraday-> Código Cliente: " + CodigoCliente);

        SendMessageSignatureOperacoesIntraday(CodigoCliente);

        gLastClientSignedOperacoesIntraday = CodigoCliente;
    } else {
        gLastClientSignedOperacoesIntraday = "";
    }

}

///Método que efetua o request no serviço de 
//REST de Operações Intraday
function OperacoesIntradayRESTService()
{
    var lRequest = MontaOperacoesIntradayRequestREST();

    var lUrl = $("#hddConnectionRESTOperacoesIntraday").val() + 'rest/BuscarOperacoesIntradayJSON';

    $.getJSON(lUrl, lRequest, function (data)
    {
        OperacoesIntraday_REST_DataBind_Grid(data);
    });
}

///Método que efetua o bind do grid no retorno dos dados 
//no serviço REST de OPerações Intraday
function OperacoesIntraday_REST_DataBind_Grid(pData) {
    var lData = JSON.parse(pData);

    for (i = 0; i < lData.length; i++) {
        OperacoesIntraday_REST_DataBind_Grid_Incremental($('#jqGrid_OperacoesIntraday'), lData[i], (i + 1));
    }
}

///Método para inserir ou editar os dados nas 
//grids de Operações Intraday
function OperacoesIntraday_REST_DataBind_Grid_Incremental(pGrid, pObjeto, numeroLinha) {

    var lEncontrou = false;

    //var cells = $("#jqGrid_OperacoesIntraday > tbody > tr > td:nth-child(2)").filter(function (index)
    //{
    //    var rowData = pGrid.jqGrid('getRowData', index);
        
    //    return (rowData["CodigoCliente"]  == pObjeto.CodigoCliente && rowData["CodigoInstrumento"] == pObjeto.CodigoInstrumento);
    //});

    //var rows = cells.parent();

    //var rowIds = pGrid.jqGrid('getDataIDs');

    //for (row = 0; row <= rowIds.length; row++) {

    //    rowData = pGrid.jqGrid('getRowData', row);

    //    if (rowData['CodigoInstrumento'] == undefined) {
    //        continue;
    //    }

    //    if (rowData['CodigoInstrumento'] == pObjeto.CodigoInstrumento && 
    //        rowData['CodigoCliente'] == pObjeto.CodigoCliente) {

    //        var lRow = jQuery('#jqGrid_OperacoesIntraday tr:eq(' + row + ')');

    //        pGrid.jqGrid('setRowData', row, pObjeto);

    //        lRow.closest("tr").find("td:gt(0)").effect("highlight", {}, 700);

    //        lEncontrou = true;


    //        break;
    //    }

    //}

    //if (!lEncontrou) {

    //var lastRowDOM = pGrid.jqGrid('getGridParam', 'records');

    //lRowID = lastRowDOM + 1;

    pGrid.addRowData(numeroLinha, pObjeto);
    //}
    //timer = setInterval(function () { pGrid.trigger('reloadGrid', [{ current: true }]) }, 5000);
    //pGrid.trigger("reloadGrid");

    //return lRetorno;
}

///Método que efetua a validação do filtro efetuado na tela de operações intraday
function OperacoesIntraday_Validar_filtro() {
    var Objeto =
        {
            CodigoCliente: $("#txtOperacoesIntradayCliente").val(),
            CodigoInstrumento: $("#txtOperacoesIntradayAtivo").val().toUpperCase(),
            OpcaoParametroIntradayPLNegativo: $("#chkOperacoesIntradayPLNegativo").is(':checked')
        };

    var lSocketChecado = $("#chkCarregarComSocketOperacoesIntraday").is(":checked")

    if (lSocketChecado && (Objeto.CodigoCliente == "" && Objeto.CodigoInstrumento == "")) {
        alert("Insira o Código do cliente ou o instrumento");

        return false;
    }


    if (!Objeto.OpcaoParametroIntradayPLNegativo && (Objeto.CodigoCliente == "" && Objeto.CodigoInstrumento == "")) {
        alert("Insira o Código do cliente ou o instrumento");

        return false;
    }
    return true;
}
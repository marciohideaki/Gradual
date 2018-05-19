var NOSUPORTEMESSAGE = "Your browser cannot support WebSocket!";
var gWebSocketOperacoesIntraday;
var gWebSocketRiscoResumido;

var gLastClientSearchedGeneralView;
var gLastClientSearchedPerAssetsClass;
var gLastClientSearchedBuysSells;
var gLastClientSearchedTradeByTrade;

//Grids de Position Client montagem
var gGridGeneralView;
var gGridPerAssetClassEquities;
var gGridPerAssetClassFutures;
var gGridBuysSellsBuy;
var gGridBuysSellsSell;
var gGridTradeByTrade;

//Grids de position Client IDS

var gGridGeneralViewSelector = $("#jqGrid_Daily_Activity_GeneralView");
var gGridPerAssetClassEquitiesSelector = $("#jqGrid_Daily_Activity_PerAssetClass_Equities");
var gGridPerAssetClassFuturesSelector = $("#jqGrid_Daily_Activity_PerAssetClass_Futures");
var gGridBuysSellsBuySelector = $("#jqGrid_Daily_Activity_BuysSells_Buy");
var gGridBuysSellsSellSelector = $("#jqGrid_Daily_Activity_BuysSells_Sell");
var gGridTradeByTradeSelector = $("#jqGrid_Daily_Activity_TradebyTrade");

//Objeto de preenchimento para as grids de Per Asset Class e suas subgrids
var gObjectPerAssetClass =
    [{
        Account: 0,
        Asset: '',
        Market: '',
        QtyBuy: '',
        AvgPriceBuy: '',
        GrossAmountBuy: '',
        QtySell: '',
        AvgPriceSell: '',
        GrossAmountSell: '',
        Realized_PL: 0,
        ExchangeFees: 0,
        GradualFees: 0,
        TipoMercado: '',
        SubgridList: [{
            Papel: '',
            Maturity: '',
            QtyBuy: '',
            AvgPriceBuy: '',
            QtySell: '',
            AvgPriceSell: '',
            Settlement_Date: '',
            Realized_PL: ''
        }]
    }];

///Método de conexão com o WebSocket de position client
function ConnectSocketServer() {
    var lSuport = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

    var lWsConnect = $("#hddConnectionSocket").val();

    if (lSuport == null) {
        alert(NOSUPORTEMESSAGE);
        return;
    }

    var lStatusConnection = $("#lblStatusConnection");

    lStatusConnection.html("***  Connecting to server ....");

    gWebSocket = new window[lSuport](lWsConnect);

    ///Evento que é chamado quando chega mensagem de position client
    gWebSocket.onmessage = function (evt) {
        var pMessage = evt.data;

        AppendMessage(pMessage);
    }

    ///Evento que é chamado quando a conexão é aberta 
    gWebSocket.onopen = function () {
        lStatusConnection.html("* Connection Opened!");
    }

    ///Evento que é chamado quando a conexão é fechada
    gWebSocket.onclose = function () {
        lStatusConnection.html("** Connection closed!");
    }
}

///Método que envia solicitação de assinatura para a sessão da conexão 
function SendMessageSignature(CodigoCliente) {
    if (gWebSocket) {
        var lMessageToSend = "SUBSCRIBE " + CodigoCliente;
        gWebSocket.send(lMessageToSend);
    }
}

//Método que envia solicitação de desassinatura para a sessão da conexão ativa
function SendMessageUnsubiscribe() {
    if (gWebSocket) {

        var lTipo = RetornaAbaAtiva();
        var lMessageToSend = '';

        switch (lTipo) {
            case 'liGeneralView':
                if (gLastClientSearchedGeneralView) {

                    lMessageToSend = "UNSUBSCRIBE " + gLastClientSearchedGeneralView;

                    $("#jqGrid_Daily_Activity_GeneralView").jqGrid('clearGridData');

                    gWebSocket.send(lMessageToSend);
                }
                break;
            case 'liPerAssetClass':
                if (gLastClientSearchedPerAssetsClass) {

                    lMessageToSend = "UNSUBSCRIBE " + gLastClientSearchedPerAssetsClass;

                    $("#jqGrid_Daily_Activity_PerAssetClass_Equities").jqGrid('clearGridData');
                    $("#jqGrid_Daily_Activity_PerAssetClass_Futures").jqGrid('clearGridData');

                    gWebSocket.send(lMessageToSend);
                }
                break;
            case 'liBuysSells':
                if (gLastClientSearchedBuysSells) {

                    lMessageToSend = "UNSUBSCRIBE " + gLastClientSearchedBuysSells;

                    $("#jqGrid_Daily_Activity_BuysSells_Buy").jqGrid('clearGridData');
                    $("#jqGrid_Daily_Activity_BuysSells_Sell").jqGrid('clearGridData');

                    gWebSocket.send(lMessageToSend);
                }
                break;
            case 'liTradebyTrade':
                if (gLastClientSearchedTradeByTrade) {
                    lMessageToSend = "UNSUBSCRIBE " + gLastClientSearchedTradeByTrade;

                    gGridTradeByTrade.jqGrid('clearGridData');

                    gWebSocket.send(lMessageToSend);
                }
                break;

        }
    }
}

///Método que efetua a desconexão da sessão do websocket corrente
function DisconnectWebSocket() {
    if (gWebSocket) {
        gWebSocket.close();
    }
}

///Método que converte array de bytes em string
function ToBinString(array) {
    var lReader = new window.FileReader();

    lReader.readAsText(array);

    lReader.onloadend = function () {
        lBase64data = lReader.result;
    }

    return lBase64data;
}

///Método que retorna a aba ativa no momento
function RetornaAbaAtiva() {
    var lRetorno = null;

    //lRetorno = $("#nav-tabs").find(".ui").attr('id');
    lRetorno = $("#nav-tabs").find("li.ui-state-active").attr("id");
    return lRetorno;
}

//Método que recebe a mensagem do websocket e trata de acordo 
//com o tipo de mensagem e insere/altera
//no grid específico
function AppendMessage(pMessage) {
    var lGrid = $("#jqGrid_Daily_Activity_GeneralView");

    var lTipo = RetornaAbaAtiva();
    var lData;
    var lRowID;

    if (pMessage == "") return;

    switch (lTipo) {
        case "liGeneralView":
            {
                lGrid = $("#jqGrid_Daily_Activity_GeneralView");

                var lJSON = $.parseJSON(pMessage);

                var lMessage = lJSON[gLastClientSearchedGeneralView];

                if (lMessage.Value == null) {
                    console.log("Chegou Mensagem Tamanho ->" + lMessage.length);

                    for (i = 0; i < lMessage.length; i++) {
                        var lData = lMessage[i];

                        var lDataGeneralView = TransporteDataGeneralView(lData);

                        console.log("Chegou Dados->" + lDataGeneralView);

                        if (lDataGeneralView.Total_Qty != 0) {
                            SetRowDataObjectonGrid(lGrid, lDataGeneralView);
                        }
                    }
                }

            }
            break;

        case "liPerAssetClass":
            //case "jqGrid_Daily_Activity_PerAssetClass_Futures":
            {
                //if para verificar se é a vista ou opções
                lGrid = $("#jqGrid_Daily_Activity_PerAssetClass_Equities");

                var lJSON = $.parseJSON(pMessage);

                var lMessage = lJSON[gLastClientSearchedPerAssetsClass];

                if (lMessage.Value == null) {
                    console.log("Chegou Mensagem Tamanho ->" + lMessage.length);

                    for (i = 0; i < lMessage.length; i++) {

                        var lData = lMessage[i];

                        if (lData.QtdExecC != 0 || lData.QtdExecV != 0) {

                            var lDataPerAssetClassEquities = TransporteDataPerAssetClassSubgrid(lData, ['VIS', 'OPC', 'OPV', 'TER', 'BTC']);

                            console.log("Chegou Dados->" + lDataPerAssetClassEquities);

                            if ((lDataPerAssetClassEquities != undefined) &&
                                (lDataPerAssetClassEquities.QtyBuy != 0 ||
                                lDataPerAssetClassEquities.QtySell != 0)) {
                                SetRowDataObjectonGrid(lGrid, lDataPerAssetClassEquities);
                            }
                        }
                    }
                }


                //if para verificar se é a vista ou opções
                var lGridFutures = $("#jqGrid_Daily_Activity_PerAssetClass_Futures");

                var lJSON = $.parseJSON(pMessage);

                var lMessage = lJSON[gLastClientSearchedPerAssetsClass];

                if (lMessage.Value == null) {
                    console.log("Chegou Mensagem Tamanho ->" + lMessage.length);

                    for (i = 0; i < lMessage.length; i++) {
                        var lData = lMessage[i];

                        var lDataPerAssetClassFutures = TransporteDataPerAssetClassSubgrid(lData, ['FUT', 'DIS', 'OPF']);

                        console.log("Chegou Dados->" + lDataPerAssetClassFutures);

                        if ((lDataPerAssetClassFutures != undefined) &&
                            (lDataPerAssetClassFutures.QtyBuy != 0 ||
                            lDataPerAssetClassFutures.QtySell != 0)) {
                            SetRowDataObjectonGrid(lGridFutures, lDataPerAssetClassFutures);
                        }
                    }
                }

            }
            break;

        case "liBuysSells":
            //case "jqGrid_Daily_Activity_BuysSells_Sell":
            {
                //if para verificar se compra ou venda
                var lGridBuy = $("#jqGrid_Daily_Activity_BuysSells_Buy");

                var lJSON = $.parseJSON(pMessage);

                var lMessage = lJSON[gLastClientSearchedPerAssetsClass];

                if (lMessage.Value == null) {
                    console.log("Chegou Mensagem Tamanho ->" + lMessage.length);

                    for (i = 0; i < lMessage.length; i++) {
                        var lData = lMessage[i];

                        var lDataBuys = TransporteDataPerAssetClassSubgrid(lData, ['FUT', 'DIS', 'OPF']);

                        console.log("Chegou Dados->" + lGridBuy);

                        if ((lDataPerAssetClassFutures != undefined) &&
                            (lDataPerAssetClassFutures.QtyBuy != 0 ||
                            lDataPerAssetClassFutures.QtySell != 0)) {
                            SetRowDataObjectonGrid(lGridBuy, lDataBuys);
                        }
                    }
                }

                var lGridSells = $("#jqGrid_Daily_Activity_BuysSells_Sell");

                if (lMessage.Value == null) {
                    console.log("Chegou Mensagem Tamanho ->" + lMessage.length);

                    for (i = 0; i < lMessage.length; i++) {
                        var lData = lMessage[i];

                        var lDatSells = TransporteDataPerAssetClassSubgrid(lData, ['FUT', 'DIS', 'OPF']);

                        console.log("Chegou Dados->" + lGridBuy);

                        if ((lDataPerAssetClassFutures != undefined) &&
                            (lDataPerAssetClassFutures.QtyBuy != 0 ||
                            lDataPerAssetClassFutures.QtySell != 0)) {
                            SetRowDataObjectonGrid(lGridSells, lDatSells);
                        }
                    }
                }
            }
            break;

        case "liTradebyTrade":
            {
                lGrid = $("#jqGrid_Daily_Activity_TradebyTrade");

                var rows = lGrid.rows;
                var lastRowDOM = rows[rows.length - 1];
                lRowID = lastRowDOM;

                lGrid.addRowData(lRowID, lData);
            }
            break;
    }
}

///Método para inserir ou editar os dados nas grids
function SetRowDataObjectonGrid(pGrid, pObjeto) {

    var lEncontrou = false;

    var rowIds = pGrid.jqGrid('getDataIDs');

    for (row = 0; row <= rowIds.length; row++) {

        rowData = pGrid.jqGrid('getRowData', row);

        if (rowData['Asset'] == undefined) {
            continue;
        }

        if (rowData['Asset'] == pObjeto.Asset) {

            pGrid.jqGrid('setRowData', row, pObjeto);

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

///Método de transporte de mensagem para json para a mensagem
function TransporteDataGeneralView(pData) {
    var monthNames = [
  "January", "February", "March",
  "April", "May", "June", "July",
  "August", "September", "October",
  "November", "December"
    ];

    var lRetorno =
    {
        Account: '',
        Asset: '',
        Market: '',
        Maturity: '',
        ExecQtyBuy: '',
        AveragePriceBuy: '',
        ExecQtySell: '',
        AveragePriceBuy: '',
        Realized_PL: '',
        NET_Qty: '',
        Total_Qty: '',
        Settlement_Amount: '',
        Settlement_Date: '',
        DtMovimento: ''
    };

    lRetorno.Account = pData.Account;
    lRetorno.Asset = pData.Ativo;
    lRetorno.Market = pData.Bolsa;
    var date = new Date(pData.DtVencimento);

    if (date.getFullYear() != 4000 &&
        date.getFullYear() != 9999 &&
        date.getFullYear() != 0) {
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();
        lRetorno.Maturity = (day + '/' + (monthIndex + 1) + '/' + year);
    }
    else {
        lRetorno.Maturity = '-';
    }

    lRetorno.ExecQtyBuy = pData.QtdExecC;
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

    return lRetorno;
}


///Método de transporte de mensagem de json para a mensagem 
//de subgrid da per asset class
function TransporteDataPerAssetClassSubgrid(pData, pTiposMercado) {

    var lRetorno;

    var lObject = {
        Account: 0,
        Asset: '',
        Market: '',
        QtyBuy: '',
        AvgPriceBuy: 0,
        GrossAmountBuy: 0,
        QtySell: '',
        AvgPriceSell: 0,
        GrossAmountSell: 0,
        Realized_PL: 0,
        ExchangeFees: 0,
        GradualFees: 0,
        TipoMercado: '',
        DtMovimento: '',
        SubgridList: [{
            Asset: '',
            Maturity: '',
            QtyBuy: 0,
            AvgPriceBuy: 0,
            QtySell: 0,
            AvgPriceSell: 0,
            Settlement_Date: '',
            Realized_PL: ''
        }]
    };


    if ($.inArray(pData.TipoMercado, pTiposMercado) == -1)
        return lRetorno;

    //Procura o Objeto na grid...
    var lObjectPerAssetClass = $.grep(gObjectPerAssetClass, function (e) { return e.Asset === pData.CodPapelObjeto.trim(); });

    lObject.Account = pData.Account;
    lObject.Asset = pData.CodPapelObjeto.trim();
    lObject.Market = pData.Bolsa;
    lObject.QtyBuy = pData.QtdExecC;
    lObject.QtySell = pData.QtdExecV;
    lObject.AvgPriceBuy = pData.PcMedC;         //pData.PcMedC.toFixed(2);
    lObject.AvgPriceSell = pData.PcMedV;         //pData.PcMedV.toFixed(2);
    lObject.GrossAmountBuy = pData.QtdExecC;
    lObject.GrossAmountSell = pData.QtdExecV;
    lObject.ExchangeFees = 0;
    lObject.GradualFees = 0;
    lObject.Realized_PL = pData.LucroPrej;
    lObject.TipoMercado = pData.TipoMercado;
    lObject.Settlement_Amount = (pData.QtdExecC + pData.QtdExecV);
    lObject.DtMovimento = pData.DtMovimento.replace('T', ' ').substring(0, 23);

    var datePosicao = new Date(pData.DtPosicao);

    if (pData.TipoMercado == 'OPV' ||
        pData.TipoMercado == 'OPC') {
        datePosicao.setDate(datePosicao.getDate() + 1);
    } else {
        datePosicao.setDate(datePosicao.getDate() + 3);
    }

    lObject.Settlement_Date =
        (datePosicao.getDate() + "/" +
        (datePosicao.getMonth() + 1) + "/" +
        datePosicao.getFullYear())

    var lObjectSubgrid = {
        Asset: pData.Ativo,
        Maturity: pData.DtVencimento,
        QtyBuy: pData.QtdExecC,
        AvgPriceBuy: pData.PcMedC,
        QtySell: pData.QtdExecV,
        AvgPriceSell: pData.PcMedV,
        Settlement_Date: lObject.Settlement_Date,
        Realized_PL: pData.LucroPrej.toFixed(2),
        PapelBase: pData.CodPapelObjeto.trim()
    };

    if (lObjectPerAssetClass.length == 0) {

        lObject.SubgridList.shift();

        lObject.SubgridList.push(lObjectSubgrid);

        gObjectPerAssetClass.push(lObject);

    } else {

        lObject.GrossAmountBuy += lObjectPerAssetClass[0].GrossAmountBuy;
        lObject.GrossAmountSell += lObjectPerAssetClass[0].GrossAmountSell;
        lObject.AvgPriceBuy = lObjectPerAssetClass[0].AvgPriceBuy;
        lObject.AvgPriceSell = lObjectPerAssetClass[0].AvgPriceSell;

        var lObjectPerAssetClassRemoved = gObjectPerAssetClass.filter(function (el) { return el.Asset !== lObjectPerAssetClass[0].Asset; });

        gObjectPerAssetClass = lObjectPerAssetClassRemoved;

        //Procura na subgrid se o o objeto já está nela
        //var lObjectSubgridFounded = $.grep(lObjectPerAssetClass[0].SubgridList, function (e) { return e.PapelBase === pData.CodPapelObjeto.trim(); });
        var lObjectSubgridFounded = $.grep(lObjectPerAssetClass[0].SubgridList, function (e) { return e.PapelBase === pData.Ativo; });

        if (lObjectSubgridFounded.length == 0) {
            lObject.SubgridList.push(lObjectSubgrid);
        }
        else {
            var lSubgridPerAssetClass = lObjectPerAssetClass[0].SubgridList.filter(function (elem) { return elem.Asset !== pData.Ativo; });

            if (lSubgridPerAssetClass.length != 0) {

                var lObjetoFounded = lSubgridPerAssetClass[0];

                lObject.SubgridList.push(lObjetoFounded);
            }

            //lObject.SubgridList.push(

            lObject.SubgridList.push(lObjectSubgrid);
        }

        gObjectPerAssetClass.push(lObject);
    }

    lRetorno = lObject;

    return lRetorno;
}

//Método de transporte de mensaage de json para a mensagem de Grids buys and sells
function TransporteDataBuysSells(pData, pSentido) {
    var monthNames = ["January", "February", "March",
                      "April", "May", "June", "July",
                      "August", "September", "October",
                      "November", "December"
    ];

    var lRetorno =
    {
        Asset: '',
        Market: '',
        Maturity: '',
        QtyBuy: '',
        AveragePriceBuy: '',
        GrossAmountBuy: '',
        QtySell: '',
        AveragePriceSell: '',
        GrossAmountSell: '',
        ExchangeFees: '',
        GradualFees: '',
        Realized_PL: '',
        Settlement_Amount: '',
        Settlement_Date: '',
    };


    if ($.inArray(pData.TipoMercado, pTiposMercado) == -1)
        return lRetorno;

    lRetorno.Account = pData.Account;
    lRetorno.Asset = pData.Ativo;
    lRetorno.Market = pData.Bolsa;

    var date = new Date(pData.DtVencimento);

    if (date.getFullYear() != 4000 &&
        date.getFullYear() != 9999 &&
        date.getFullYear() != 0) {
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();

        lRetorno.Maturity = (day + '/' + (monthIndex + 1) + '/' + year);
    }
    else {
        lRetorno.Maturity = '-';
    }

    lRetorno.QtyBuy = pData.QtdExecC;
    lRetorno.AveragePriceBuy = pData.PcMedC.toFixed(2);
    lRetorno.GrossAmountBuy = '';
    lRetorno.QtySell = pData.QtdExecV;
    lRetorno.AveragePriceSell = pData.PcMedV.toFixed(2);
    lRetorno.GrossAmountSell = '';
    lRetorno.Realized_PL = pData.LucroPrej.toFixed(2);
    lRetorno.NET_Qty = pData.NetExec;
    lRetorno.GradualFees = '-';
    lRetorno.ExchangeFees = '-';
    //lRetorno.Total_Qty         = (pData.QtdExecC + pData.QtdExecV);
    lRetorno.Settlement_Amount = (pData.QtdExecC - pData.QtdExecV);

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
        datePosicao.getMonth() + "/" +
        datePosicao.getFullYear())

    return lRetorno;
}

$.removeFromArray = function RemoveFromArray(value, arr) {
    return $.grep(arr, function (elem, index) { return elem !== value; });
}

///Método de transporte de mensagem para json para a mensagem
function TransporteDataBuysAndSells(pData) {
}

///Método de transporte de mensagem para json para a mensagem
function TransporteDataTradeByTrade(pData) {
}
//Método que verifica se a key da mensagem já está na grid e se estiver atualiza a row
// se não estiver 
function ContainsKeyjqGrid(pGrid, pMensagem) {
    var index = 0;
    pGrid
        .children('tbody')
        .children('td')
    .each(function () {
        index++;

    });
}

///Método de page load 
function Page_Load() {
    Page_Load_CodeBehind();

    PreparaGridOperacoesIntraday();

    PreparaGridRiscoResumido()    ;


    //PreparaGridGeneralView()    ;

    //PreparaGridPerAssetClass()  ;
    /*
    PreparaGridBuysSells()      ;
    
    PreparaGridTradebyTrade();
    */

    //HabilitarMascaraNumerica($(".tab-content").find("input.Mascara_Hora"), '23:59:59:999')
    //HabilitarMascaraNumerica($("#nav-tabs").find("input.Mascara_Hora"), '99:99:99.999');
    HabilitarMascaraNumerica($("#nav-tabs").find("input.Mascara_Hora"), '99:99');

    //$(".tab-content").find(".ExecutionFrom").val("08:00:00:000");
    //$(".tab-content").find(".ExecutionTo").val("23:59:59:999");
}

//Método que prepara grid de general view.
function PreparaGridGeneralView() {
    var lGrid = $("#jqGrid_Daily_Activity_GeneralView");

    lGrid.jqGrid(gGridGeneralView);

    /*lGrid.jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [{ startColumnName: 'ExecQtyBuy', numberOfColumns: 2, titleText: '<em>Buy</em>' },
                       { startColumnName: 'ExecQtySell', numberOfColumns: 2, titleText: '<em>Sell</em>' }]
    });*/

    lGrid.jqGrid('navGrid', '#jqGrid_Daily_Activity_GeneralView_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });

    lGrid.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    lGrid.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_GeneralView_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGrid[0].toggleToolbar(); }
    });

    lGrid.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_GeneralView_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_Daily_Activity_GeneralView', 'GeneralView.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGrid[0].toggleToolbar();
}

//Método que prepara grids de per asset class.
function PreparaGridPerAssetClass() {
    var lGridEquities = $("#jqGrid_Daily_Activity_PerAssetClass_Equities");

    lGridEquities.jqGrid(gGridPerAssetClassEquities);

    /*lGridEquities.jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [{ startColumnName: 'QtyBuy', numberOfColumns: 3, titleText: '<em>Buy</em>' },
                       { startColumnName: 'QtySell', numberOfColumns: 3, titleText: '<em>Sell</em>' }]
    });*/


    lGridEquities.jqGrid('navGrid', '#jqGrid_Daily_Activity_PerAssetClass_Equities_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });

    lGridEquities.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    lGridEquities.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_PerAssetClass_Equities_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGridEquities[0].toggleToolbar(); }
    });

    lGridEquities.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_PerAssetClass_Equities_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_Daily_Activity_PerAssetClass_Equities', 'PerAssetClass_Equities.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGridEquities[0].toggleToolbar();

    var lGridFutures = $("#jqGrid_Daily_Activity_PerAssetClass_Futures");

    lGridFutures.jqGrid(gGridPerAssetClassFutures);

    /*lGridFutures.jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [{ startColumnName: 'QtyBuy', numberOfColumns: 3, titleText: '<em>Buy</em>' },
                       { startColumnName: 'QtySell', numberOfColumns: 3, titleText: '<em>Sell</em>' }]
    });*/

    lGridFutures.jqGrid('navGrid', '#jqGrid_Daily_Activity_PerAssetClass_Futures_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });

    lGridFutures.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    lGridFutures.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_PerAssetClass_Futures_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGridFutures[0].toggleToolbar(); }
    });

    lGridFutures.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_PerAssetClass_Futures_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_Daily_Activity_PerAssetClass_Futures', 'PerAssetClass_Futures.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGridFutures[0].toggleToolbar();

}

//Método que prepara grids de Buys and Sells
function PreparaGridBuysSells() {
    var lGridBuy = $("#jqGrid_Daily_Activity_BuysSells_Buy");

    lGridBuy.jqGrid(gGridBuysSellsBuy);
    /*
    lGrid.jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [{ startColumnName: 'Asset', numberOfColumns: 1, titleText: '<em>Buy</em>' },
                      // { startColumnName: 'QtySell', numberOfColumns: 3, titleText: '<em>Sell</em>' }
        ]
    });
    */
    lGridBuy.jqGrid('navGrid', '#jqGrid_Daily_Activity_BuysSells_Buy_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });

    lGridBuy.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    lGridBuy.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_BuysSells_Buy_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGridBuy[0].toggleToolbar(); }
    });

    lGridBuy.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_BuysSells_Buy_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_Daily_Activity_BuysSells_Buy', 'BuysSells_Buy.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGridBuy[0].toggleToolbar();

    var lGridSell = $("#jqGrid_Daily_Activity_BuysSells_Sell");

    lGridSell.jqGrid(gGridBuysSellsSell);
    /*
    lGrid.jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [{ startColumnName: 'Asset', numberOfColumns: 1, titleText: '<em>Sell</em>' },
                      // { startColumnName: 'QtySell', numberOfColumns: 3, titleText: '<em>Sell</em>' }
        ]
    });
    */
    lGridSell.jqGrid('navGrid', '#jqGrid_Daily_Activity_BuysSells_Sell_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });

    lGridSell.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    lGridSell.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_BuysSells_Sell_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGridSell[0].toggleToolbar(); }
    });

    lGridSell.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_BuysSells_Sell_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_Daily_Activity_BuysSells_Sell', 'BuysSells_Sell.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGridSell[0].toggleToolbar();
}

//Método que oreoara grids de Trade by trade
function PreparaGridTradebyTrade() {
    var lGridTrade = $("#jqGrid_Daily_Activity_TradebyTrade");

    lGridTrade.jqGrid(gGridTradeByTrade);
    /*
    lGrid.jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [{ startColumnName: 'Asset', numberOfColumns: 1, titleText: '<em>Buy</em>' },
                      // { startColumnName: 'QtySell', numberOfColumns: 3, titleText: '<em>Sell</em>' }
        ]
    });
    */
    lGridTrade.jqGrid('navGrid', '#jqGrid_Daily_Activity_TradebyTrade_Pager',
        { add: false, edit: false, del: false },
        {},
        {},
        {},
        { multipleSearch: false, overlay: false });

    lGridTrade.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    lGridTrade.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_TradebyTrade_Pager',
    {
        caption: 'Filtro',
        title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-pin-s',
        onClickButton: function () { lGridTrade[0].toggleToolbar(); }
    });

    lGridTrade.jqGrid('navButtonAdd', '#jqGrid_Daily_Activity_TradebyTrade_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_Daily_Activity_TradebyTrade', 'TradebyTrade.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    lGridTrade[0].toggleToolbar();
}

//Método de preenchimento da grid de GeneralView
function PostTradingClient_Daily_Activity_GeneralView() {
    var oldFrom = $.jgrid.from;
    $.jgrid.from = function (source, initalQuery) {
        var result = oldFrom(source, initalQuery);
        var old_getStr = result._getStr;
        result._getStr = function (s) {
            var phrase = [];
            if (this._trim) {
                phrase.push("jQuery.trim(");
            }
            phrase.push("myAccentRemovement(String(" + s + "))");
            if (this._trim) {
                phrase.push(")");
            }
            if (!this._usecase) {
                phrase.push(".toLowerCase()");
            }
            return phrase.join("");
        }
        return result;
    }

    gGridGeneralView =
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "local"
        //, mtype: "GET"
        //, hoverrows  : false
      , data: lMock_GeneralView.rows
      , autowidth: true
        //, shrinkToFit: false
      , colNames: ['Account', 'Asset', 'Market', 'Maturity', 'Exec Qty Buy', 'Average Price Buy', 'Exec Qty Sell', 'Average Price Sell', 'Realized PL', 'NET Qty', 'Total Qty', 'Settlement Amount', 'Settlement Date', 'DtMovimento']
      , colModel: [
                        { label: "Account", name: "Account", jsonmap: "Account", index: "Account", width: 75, align: "center", sortable: true, sorttype: 'string' }
                      , { label: "Asset", name: "Asset", jsonmap: "Asset", index: "Asset", width: 75, align: "center", sortable: true, sorttype: 'string' }
                      , { label: "Market", name: "Market", jsonmap: "Market", index: "Market", width: 75, align: "center", sortable: true, sorttype: 'string' }
                      , { label: "Maturity", name: "Maturity", jsonmap: "Maturity", index: "Maturity", width: 75, align: "center", sortable: true, sorttype: 'date' }
                      , { label: "Exec Qty Buy", name: "ExecQtyBuy", jsonmap: "ExecQtyBuy", index: "ExecQtyBuy", width: 75, align: "center", sortable: true, sorttype: 'int' }
                      , { label: "Average Price Buy", name: "AveragePriceBuy", jsonmap: "AveragePriceBuy", index: "AveragePriceBuy", width: 75, align: "right", sortable: true, sorttype: 'decimal', formatter: SetColorFontGeneralViewFormat }
                      , { label: "Exec Qty Sell", name: "ExecQtySell", jsonmap: "ExecQtySell", index: "ExecQtySell", width: 75, align: "center", sortable: true, sorttype: 'int' }
                      , { label: "Average Price Sell", name: "AveragePriceSell", jsonmap: "AveragePriceSell", index: "AveragePriceSell", width: 75, align: "right", sortable: true, sorttype: 'decimal', formatter: SetColorFontGeneralViewFormat }
                      , { label: "Realized P&L", name: "Realized_PL", jsonmap: "Realized_PL", index: "Realized_PL", width: 75, align: "right", sortable: true, sorttype: 'decimal', formatter: SetColorFontGeneralViewFormat }
                      , { label: "NET Qty", name: "NET_Qty", jsonmap: "NET_Qty", index: "NET_Qty", width: 75, align: "center", sortable: true, sorttype: 'int' }
                      , { label: "Total Qty", name: "Total_Qty", jsonmap: "Total_Qty", index: "Total_Qty", width: 75, align: "center", sortable: true, sorttype: 'int' }
                      , { label: "Settlement Amount", name: "Settlement_Amount", jsonmap: "Settlement_Amount", index: "Settlement_Amount", width: 100, align: "center", sortable: true, sorttype: 'date' }
                      , { label: "Settlement Date", name: "Settlement_Date", jsonmap: "Settlement_Date", index: "Settlement_Date", width: 95, align: "center", sortable: true, sorttype: 'date' }
                      , {
                          label: "DtMovimento", name: "DtMovimento", jsonmap: "DtMovimento", index: "DtMovimento", width: 110, align: "center",
                          sortable: true,
                          sorttype: 'date',
                          formatter: 'date',
                          formatoptions: {
                              newformat: 'Y-m-d H:i:s.u',
                              srcformat: 'Y-m-d H:i:s.u',
                          },
                      },
      ]
      , height: 'auto'
      , width: 'auto'
      , sortable: true
      , sortname: "Account"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , pager: '#jqGrid_Daily_Activity_GeneralView_Pager'
        //, toolbarfilter: true

      , rowNum: 30
      , rowList: [10, 20, 30]
      , caption: 'General View'
      , afterInsertRow: SetColorFontGeneralViewInsertRow
      , gridComplete: function () {
      }
        //,rownumbers: true
      , sortable: true
        //, editable:true
        //, afterInsertRow: SetColorFontGeneralView
    };

    //$("#jqGrid_Daily_Activity_GeneralView").jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });
}

function myAccentRemovement(s) {
    // the s parameter is always string
    s = s.replace(/[àáâãäå]/gi, 'a');
    s = s.replace(/[èéêë]/gi, 'e');
    s = s.replace(/[ìíîï]/gi, 'i');
    s = s.replace(/[òóôõöø]/gi, 'o');
    s = s.replace(/[ùúûü]/gi, 'u');
    s = s.replace(/[ýÿ]/gi, 'y');
    s = s.replace(/æ/gi, 'ae');
    s = s.replace(/œ/gi, 'oe');
    s = s.replace(/ç/gi, 'c');
    s = s.replace(/š/gi, 's');
    s = s.replace(/ñ/gi, 'n');
    s = s.replace(/ž/gi, 'z');
    return s;
}

///Método para setar para mudar cor das colunas do grid de General View
function SetColorFontGeneralViewInsertRow(rowid, aData) {
    var lcolorBlue = "#00C6FF";
    var lcolorGreen = "#00C669";
    var lcolorRed = "#FF3A44";

    var lGrid = $("#jqGrid_Daily_Activity_GeneralView");

    lGrid.jqGrid('setCell', rowid, 'ExecQtyBuy', '', { 'color': lcolorBlue });
    lGrid.jqGrid('setCell', rowid, 'ExecQtySell', '', { 'color': lcolorGreen });
    lGrid.jqGrid('setCell', rowid, 'AveragePriceBuy', '', { 'color': lcolorBlue });
    lGrid.jqGrid('setCell', rowid, 'AveragePriceSell', '', { 'color': lcolorGreen });

    if (aData.Realized_PL.toString().indexOf('-') > -1) {
        lGrid.jqGrid('setCell', rowid, 'Realized_PL', '', { 'color': lcolorRed });
    } else {
        lGrid.jqGrid('setCell', rowid, 'Realized_PL', '', { 'color': lcolorBlue });
    }

}

///Método para setar para mudar cor das colunas do grid de General View
function SetColorFontGeneralViewFormat(cellvalue, options, rowObject) {
    var cellHtml;
    var cifrao = "R$ ";
    var column = options.colModel.name.toLowerCase();


    if (column == "execqtybuy" ||
        column == "averagepricebuy") {
        lcolor = "#00C6FF"; //blue

        if (column != "averagepricebuy")
            cifrao = "";
    }
    else if (column == "execqtysell" ||
             column == "averagepricesell") {
        lcolor = "#00C669"; //green

        if (column != "averagepricesell")
            cifrao = "";
    }

    if (column == "realized_pl") {
        cifrao = "R$ ";
    }

    //$("#" + options.gid).jqGrid('setCell', parseInt(options.rowId), options.colModel.name, '', { 'color': lcolor, 'background-color': 'yellow' })

    cellHtml = cifrao + cellvalue.toString().replace('.', ',');

    return cellHtml;
}

//Método de preenchimento da grid de Per Asset Class À vista (Equities)
function PostTradingClient_Daily_Activity_PerAssetClass_Equities() {
    $("#jqGrid_Daily_Activity_PerAssetClass_Equities").jqGrid(
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "jsonstring"
        //, mtype: "GET"
        //, hoverrows: false
      , datastr: lMock_PerAssetClassEquities.rows
      , autowidth: true
      , jsonReader: {
          subgrid: { repeatitems: false }
      }
        //, shrinkToFit: false
      , colNames: ["Asset", "Market", "Quantity Buy", "Average Price Buy", "Gross Amount Buy", "Quantity Sell", "Average Price Sell", "Gross Amount Sell", "Exchange Fees", "Gradual Fees", "Realized P&L", "Settlement Amount", "Settlement Date", "DtMovimento"]
      , colModel: [
                         //{ label: ""                 , name: ""                 , jsonmap: ""                   , index: ""                  , width: 75, align: "center", sortable: true }
                        { label: "Asset", name: "Asset", jsonmap: "Asset", index: "Asset", width: 75, align: "center", sortable: true }
                      , { label: "Market", name: "Market", jsonmap: "Market", index: "Market", width: 75, align: "center", sortable: true }
                      //, { label: "Maturity"         , name: "Maturity"          , jsonmap: "Maturity"           , index: "Maturity"         , width: 85, align: "center", sortable: true }
                      , { label: "Quantity Buy", name: "QtyBuy", jsonmap: "QtyBuy", index: "QtyBuy", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: '.', decimalSeparator: '', decimalPlaces: 0 } }
                      , { label: "Average Price Buy", name: "AvgPriceBuy", jsonmap: "AvgPriceBuy", index: "AvgPriceBuy", width: 75, align: "right", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' }, }
                      , { label: "Gross Amount Buy", name: "GrossAmountBuy", jsonmap: "GrossAmountBuy", index: "GrossAmountBuy", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: ".", decimalSeparator: '', decimalPlaces: 0 } }
                      , { label: "Quantity Sell", name: "QtySell", jsonmap: "QtySell", index: "QtySell", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: ".", decimalSeparador: '', decimalPlaces: 0 }, }
                      , { label: "Average Price Sell", name: "AvgPriceSell", jsonmap: "AvgPriceSell", index: "AvgPriceSell", width: 75, align: "right", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' }, }
                      , { label: "Gross Amount Sell", name: "GrossAmountSell", jsonmap: "GrossAmountSell", index: "GrossAmountSell", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: ".", decimalSeparator: '', decimalPlaces: 0 }, }
                      , { label: "Exchange Fees", name: "ExchangeFees", jsonmap: "ExchangeFees", index: "ExchangeFees", width: 75, align: "center", sortable: true }
                      , { label: "Gradual Fees", name: "GradualFees", jsonmap: "GradualFees", index: "GradualFees", width: 75, align: "center", sortable: true }
                      , { label: "Realized P&L", name: "Realized_PL", jsonmap: "Realized_PL", index: "Realized_PL", width: 85, align: "right", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' } }
                      , { label: "Settlement Amount", name: "Settlement_Amount", jsonmap: "Settlement_Amount", index: "Settlement_Amount", width: 80, align: "center", sortable: true, formatter: 'number', formatoptions: { decimalSeparator: '', thousandsSeparator: '.', decimalPlaces: 0 } }
                      , { label: "Settlement Date", name: "Settlement_Date", jsonmap: "Settlement_Date", index: "Settlement_Date", width: 80, align: "center", sortable: true }
                      , { label: "DtMovimento", name: "DtMovimento", jsonmap: "DtMovimento", index: "DtMovimento", width: 110, align: "center" }
                      /*  sortable: true,
                        sorttype: 'date',
                        formatter: 'date',
                            formatoptions: {
                                newformat: 'Y-m-d H:i:s.u',
                                srcformat: 'Y-m-d H:i:s.u',
                            },
                        },*/

      ]
      , loadonce: true
      , height: 'auto'
      , width: '100%'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , pager: '#jqGrid_Daily_Activity_PerAssetClass_Equities_Pager'
      , subGrid: true
      , rowNum: 30
      , rowList: [10, 20, 30]

      , caption: 'Equities'
      , subGridOptions: {
          // load the subgrid data only once
          // and the just show/hide
          reloadOnExpand: false,
          // select the row when the expand column is clicked
          selectOnExpand: true
      }
      , subGridRowExpanded: subGridRowExpandendPerAssetClassEquities
      , gridComplete: function () {
      }
      , afterInsertRow: SetColorPerAssetClassEquitiesInsertRow
      , sortable: true
    });
}

///Método para mudar a cor das colunas do grid de per asset class
function SetColorPerAssetClassEquitiesInsertRow(rowid, pData) {

    var lcolorBlue = "#00C6FF";
    var lcolorGreen = "#00C669";
    var lcolorRed = "#FF3A44";

    var lGrid = $("#jqGrid_Daily_Activity_PerAssetClass_Equities");

    lGrid.jqGrid('setCell', rowid, 'QtyBuy', '', { 'color': lcolorBlue });
    lGrid.jqGrid('setCell', rowid, 'QtySell', '', { 'color': lcolorGreen });
    lGrid.jqGrid('setCell', rowid, 'AvgPriceBuy', '', { 'color': lcolorBlue });
    lGrid.jqGrid('setCell', rowid, 'AvgPriceSell', '', { 'color': lcolorGreen });
    lGrid.jqGrid('setCell', rowid, 'GrossAmountBuy', '', { 'color': lcolorBlue })
    lGrid.jqGrid('setCell', rowid, 'GrossAmountSell', '', { 'color': lcolorGreen })

    if (pData.Realized_PL.toString().indexOf('-') > -1) {
        lGrid.jqGrid('setCell', rowid, 'Realized_PL', '', { 'color': lcolorRed });
    } else {
        lGrid.jqGrid('setCell', rowid, 'Realized_PL', '', { 'color': lcolorBlue });
    }

}

///Método para mudar a cor das colunas do grid de per asset class
function SetColorPerAssetClassFuturesInsertRow(rowid, pData) {

    var lcolorBlue = "#00C6FF";
    var lcolorGreen = "#00C669";
    var lcolorRed = "#FF3A44";

    var lGrid = $("#jqGrid_Daily_Activity_PerAssetClass_Futures");

    lGrid.jqGrid('setCell', rowid, 'QtyBuy', '', { 'color': lcolorBlue });
    lGrid.jqGrid('setCell', rowid, 'QtySell', '', { 'color': lcolorGreen });
    lGrid.jqGrid('setCell', rowid, 'AveragePriceBuy', '', { 'color': lcolorBlue });
    lGrid.jqGrid('setCell', rowid, 'AveragePriceSell', '', { 'color': lcolorGreen });
    lGrid.jqGrid('setCell', rowid, 'GrossAmountBuy', '', { 'color': lcolorBlue })
    lGrid.jqGrid('setCell', rowid, 'GrossAmountSell', '', { 'color': lcolorGreen })

    if (pData.Realized_PL.toString().indexOf('-') > -1) {
        lGrid.jqGrid('setCell', rowid, 'Realized_PL', '', { 'color': lcolorRed });
    } else {
        lGrid.jqGrid('setCell', rowid, 'Realized_PL', '', { 'color': lcolorBlue });
    }

}

///Evento que é acionado quando o usuário clica no botão subgrid de Equities
function subGridRowExpandendPerAssetClassEquities(subgrid_id, row_id) {
    var subgrid_table_id, pager_id;

    subgrid_table_id = "mySubGridName" + row_id;

    pager_id = "p_" + subgrid_table_id;

    var lPapelBase = $("#jqGrid_Daily_Activity_PerAssetClass_Equities").find("#" + row_id).find("td").eq(1).html();

    $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "'></table>");

    var lGrid = jQuery("#" + subgrid_table_id);

    lGrid.jqGrid({
        hoverrows: true
            , datatype: "jsonstring"
            , datastr: lMock_PerAssetClassEquitiesSubgrid.rows
            , colNames: ['Asset', 'Maturity', 'Qty Buy', 'Avg Price Buy', 'Qty Sell', 'Avg Price Sell', 'Realized_PL', 'Settlement Date']
            , colModel: [
                  { label: 'Asset', name: 'Asset', jsonmap: 'Asset', index: 'Asset', width: 50, align: 'center', sortable: false }
                , { label: 'Maturity', name: 'Maturity', jsonmap: 'Maturity', index: 'Maturity', width: 50, align: 'center', sortable: false, formatter: 'date', formatoptions: { srcformat: 'U', newformat: 'd/m/Y' } }
                , { label: 'Qty Buy', name: 'QtyBuy', jsonmap: 'QtyBuy', index: 'QtyBuy', width: 50, align: 'center', sortable: false, formatter: 'number', formatoptions: { decimalSeparator: '', thousandsSeparator: '.', decimalPlaces: 0 } }
                , { label: 'Avg Price Buy', name: 'AvgPriceBuy', jsonmap: 'AvgPriceBuy', index: 'AvgPriceBuy', width: 50, align: 'center', sortable: false, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' } }
                , { label: 'Qty Sell', name: 'QtySell', jsonmap: 'QtySell', index: 'QtySell', width: 50, align: 'center', sortable: false, formatter: 'number', formatoptions: { decimalSeparator: '', thousandsSeparator: '.', decimalPlaces: 0 } }
                , { label: 'Avg Price Sell', name: 'AvgPriceSell', jsonmap: 'AvgPriceSell', index: 'AvgPriceSell', width: 50, align: 'center', sortable: false, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' } }
                , { label: 'Realized PL', name: 'Realized_PL', jsonmap: 'Realized_PL', index: 'Realized_PL', width: 50, align: 'center', sortable: false, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' } }
                , { label: 'Settlement Date', name: 'Settlement_Date', jsonmap: 'Settlement_Date', index: 'Settlement_Date', width: 50, align: 'center', sortable: false }
            ]
            , rowNum: 'auto'
            , width: 700
            , height: "100%"
            , sortable: true
    });

    var lFoundedObject = $.grep(gObjectPerAssetClass, function (elem, value) { return elem.Asset === lPapelBase; });

    var lIndex = 1;

    for (item = 0; item < lFoundedObject.length; item++) {
        for (itemSubgrid = 0; itemSubgrid < lFoundedObject[item].SubgridList.length; itemSubgrid++) {
            var lData = lFoundedObject[item].SubgridList[itemSubgrid];

            if (lData.Asset != "" && lData.Asset != undefined) {
                lGrid.addRowData(lIndex, lData);
                lIndex++;
            }
        }
    }
}

///Evento que é acionado quando o usuário clica no botão subgrid de Futures
function subGridRowExpandendPerAssetClassFutures(subgrid_id, row_id) {
    var subgrid_table_id, pager_id;

    subgrid_table_id = "mySubGridNameFuture" + row_id;

    pager_id = "p_" + subgrid_table_id;

    var lPapelBase = $("#jqGrid_Daily_Activity_PerAssetClass_Futures").find("#" + row_id).find("td").eq(1).html();

    $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "'></table>");

    var lGrid = jQuery("#" + subgrid_table_id);

    lGrid.jqGrid({
        hoverrows: true
            , datatype: "jsonstring"
            , datastr: lMock_PerAssetClassEquitiesSubgrid.rows
            , colNames: ['Asset', 'Maturity', 'Qty Buy', 'Avg Price Buy', 'Qty Sell', 'Avg Price Sell', 'Realized_PL', 'Settlement Date']
            , colModel: [
                  { label: 'Asset', name: 'Asset', jsonmap: 'Asset', index: 'Asset', width: 50, align: 'center', sortable: false }
                , { label: 'Maturity', name: 'Maturity', jsonmap: 'Maturity', index: 'Maturity', width: 50, align: 'center', sortable: false, formatter: 'date', formatoptions: { srcformat: 'U', newformat: 'd/m/Y' } }
                , { label: 'Qty Buy', name: 'QtyBuy', jsonmap: 'QtyBuy', index: 'QtyBuy', width: 50, align: 'center', sortable: false }
                , { label: 'Avg Price Buy', name: 'AvgPriceBuy', jsonmap: 'AvgPriceBuy', index: 'AvgPriceBuy', width: 50, align: 'center', sortable: false }
                , { label: 'Qty Sell', name: 'QtySell', jsonmap: 'QtySell', index: 'QtySell', width: 50, align: 'center', sortable: false }
                , { label: 'Avg Price Sell', name: 'AvgPriceSell', jsonmap: 'AvgPriceSell', index: 'AvgPriceSell', width: 50, align: 'center', sortable: false }
                , { label: 'Realized PL', name: 'Realized_PL', jsonmap: 'Realized_PL', index: 'Realized_PL', width: 50, align: 'center', sortable: false }
                , { label: 'Settlement Date', name: 'Settlement_Date', jsonmap: 'Settlement_Date', index: 'Settlement_Date', width: 50, align: 'center', sortable: false }
            ]
            , rowNum: 'auto'
            , width: 700
            , height: "100%"
            , sortable: true
    });

    var lFoundedObject = $.grep(gObjectPerAssetClass, function (elem, value) { return elem.Asset === lPapelBase; });

    var lIndex = 1;

    for (item = 0; item < lFoundedObject.length; item++) {
        for (itemSubgrid = 0; itemSubgrid < lFoundedObject[item].SubgridList.length; itemSubgrid++) {
            var lData = lFoundedObject[item].SubgridList[itemSubgrid];

            if (lData.Asset != "" && lData.Asset != undefined) {
                lGrid.addRowData(lIndex, lData);
                lIndex++;
            }
        }
    }
}

//Método de preenchimento da grid de Per Asset Class de Opções (Futures)
function PostTradingClient_Daily_Activity_PerAssetClass_Futures() {
    gGridPerAssetClassFutures =
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "jsonstring"
        //, mtype: "GET"
      , hoverrows: true
      , datastr: lMock_PerAssetClassFutures.rows
      , autowidth: false
        //, shrinkToFit: false
      , colModel: [
                        { label: "Asset", name: "Asset", jsonmap: "Asset", index: "Asset", width: 75, align: "center", sortable: true }
                      , { label: "Market", name: "Market", jsonmap: "Market", index: "Market", width: 75, align: "center", sortable: true }
                      //, { label: "Maturity",            name: "Maturity",           jsonmap: "Maturity",            index: "Maturity",          width: 85, align: "center", sortable: true }
                      , { label: "Quantity Buy", name: "QtyBuy", jsonmap: "QtyBuy", index: "QtyBuy", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: '.', decimalSeparator: '', decimalPlaces: 0 } }
                      , { label: "Average Price Buy", name: "AvgPriceBuy", jsonmap: "AvgPriceBuy", index: "AvgPriceBuy", width: 75, align: "center", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ".", decimalPlaces: 0, defaultValue: '0.00', prefix: 'Pts ' }, }
                      , { label: "Gross Amount Buy", name: "GrossAmountBuy", jsonmap: "GrossAmountBuy", index: "GrossAmountBuy", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: ".", decimalSeparator: '', decimalPlaces: 0 } }
                      , { label: "Quantity Sell", name: "QtySell", jsonmap: "QtySell", index: "QtySell", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: ".", decimalSeparador: '', decimalPlaces: 0 }, }
                      , { label: "Average Price Sell", name: "AvgPriceSell", jsonmap: "AvgPriceSell", index: "AvgPriceSell", width: 75, align: "center", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ".", decimalPlaces: 0, defaultValue: '0.00', prefix: 'Pts ' }, }
                      , { label: "Gross Amount Sell", name: "GrossAmountSell", jsonmap: "GrossAmountSell", index: "GrossAmountSell", width: 75, align: "center", sortable: true, formatter: 'number', formatoptions: { thousandsSeparator: ".", decimalSeparator: '' }, }
                      , { label: "Exchange Fees", name: "ExchangeFees", jsonmap: "ExchangeFees", index: "ExchangeFees", width: 85, align: "center", sortable: true }
                      , { label: "Gradual Fees", name: "GradualFees", jsonmap: "GradualFees", index: "GradualFees", width: 65, align: "center", sortable: true }
                      , { label: "Realized P&L", name: "Realized_PL", jsonmap: "Realized_PL", index: "Realized_PL", width: 85, align: "center", sortable: true, formatter: 'currency', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00', prefix: 'R$ ' } }
                      , { label: "Settlement Amount", name: "Settlement_Amount", jsonmap: "Settlement_Amount", index: "Settlement_Amount", width: 80, align: "center", sortable: true, formatter: 'number', formatoptions: { decimalSeparator: '', thousandsSeparator: '.', decimalPlaces: 0 } }
                      , { label: "Settlement Date", name: "Settlement_Date", jsonmap: "Settlement_Date", index: "Settlement_Date", width: 80, align: "center", sortable: true }
                      //, { label: "DtPosicao",           name: "DtPosicao",          jsonmap: "DtPosicao",           index: "DtPosicao",         width: 80, align: "center", sortable: true, formatter: 'date'  }
                      , {
                          label: "DtMovimento", name: "DtMovimento", jsonmap: "DtMovimento", index: "DtMovimento", width: 110, align: "center",
                          sortable: true,
                          sorttype: 'date',
                          formatter: 'date',
                          formatoptions: {
                              newformat: 'Y-m-d H:i:s.u',
                              srcformat: 'Y-m-d H:i:s.u',
                          },
                      },


      ]
      , height: 'auto'
      , width: '100%'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
        //, viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , pager: '#jqGrid_Daily_Activity_PerAssetClass_Futures_Pager'
      , rowNum: 30
      , rowList: [10, 20, 30]
      , caption: 'Futures'
      , gridComplete: function () {
      }
      , afterInsertRow: SetColorPerAssetClassFuturesInsertRow
      , subGridRowExpanded: subGridRowExpandendPerAssetClassFutures
      , sortable: true
    };
}

//Método de preenchimento da grid de Buys and Sells de venda
function PostTradingClient_Daily_Activity_BuysSells_Buy() {
    //$("#jqGrid_Daily_Activity_BuysSells_Buy").jqGrid(
    gGridBuysSellsBuy =
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "jsonstring"
        //, mtype: "GET"
      , hoverrows: true
      , datastr: lMock_BuysSellsSell.rows
      , autowidth: true
        //, shrinkToFit: false
      , colModel: [
                        { label: "Asset", name: "Asset", jsonmap: "Asset", index: "Asset", width: 75, align: "center", sortable: true }
                      , { label: "Market", name: "Market", jsonmap: "Market", index: "Market", width: 55, align: "center", sortable: true }
                      , { label: "Maturity", name: "Maturity", jsonmap: "Maturity", index: "Maturity", width: 85, align: "center", sortable: true }
                      , { label: "Operador DMA", name: "OperadorDMA", jsonmap: "OperadorDMA", index: "OperadorDMA", width: 75, align: "center", sortable: true }
                      , { label: "Trade ID", name: "TradeID", jsonmap: "TradeID", index: "TradeID", width: 85, align: "center", sortable: true }
                      , { label: "Quantity", name: "Quantity", jsonmap: "Quantity", index: "Quantity", width: 65, align: "center", sortable: true }
                      , { label: "Price", name: "Price", jsonmap: "Price", index: "Price", width: 85, align: "center", sortable: true }
                      , { label: "Settlement Date", name: "Settlement_Date", jsonmap: "Settlement_Date", index: "Settlement_Date", width: 85, align: "center", sortable: true }
                      , {
                          label: "DtMovimento", name: "DtMovimento", jsonmap: "DtMovimento", index: "DtMovimento", width: 110, align: "center",
                          sortable: true,
                          sorttype: 'date',
                          formatter: 'date',
                          formatoptions: {
                              newformat: 'Y-m-d H:i:s.u',
                              srcformat: 'Y-m-d H:i:s.u',
                          },
                      },
      ]
      , height: 'auto'
      , width: 'auto'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , pager: '#jqGrid_Daily_Activity_BuysSells_Buy_Pager'
      , rowNum: 30
      , rowList: [10, 20, 30]
      , caption: 'Buy'
      , sortable: true
    };
}

///Método que preenche a grid de Buys and Sells de compra
function PostTradingClient_Daily_Activity_BuysSells_Sell() {
    //$("#jqGrid_Daily_Activity_BuysSells_Sell").jqGrid(
    gGridBuysSellsSell =
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "jsonstring"
        //, mtype: "GET"
      , hoverrows: true
      , datastr: lMock_BuysSellsSell.rows
      , autowidth: false
        //, shrinkToFit: false
      , colModel: [
                        { label: "Asset", name: "Asset", jsonmap: "Asset", index: "Asset", width: 75, align: "center", sortable: true }
                      , { label: "Market", name: "Market", jsonmap: "Market", index: "Market", width: 55, align: "center", sortable: true }
                      , { label: "Maturity", name: "Maturity", jsonmap: "Maturity", index: "Maturity", width: 85, align: "center", sortable: true }
                      , { label: "Operador DMA", name: "OperadorDMA", jsonmap: "OperadorDMA", index: "OperadorDMA", width: 75, align: "center", sortable: true }
                      , { label: "Trade ID", name: "TradeID", jsonmap: "TradeID", index: "TradeID", width: 85, align: "center", sortable: true }
                      , { label: "Quantity", name: "Quantity", jsonmap: "Quantity", index: "Quantity", width: 65, align: "center", sortable: true }
                      , { label: "Price", name: "Price", jsonmap: "Price", index: "Price", width: 85, align: "center", sortable: true }
                      , { label: "Settlement Date", name: "Settlement_Date", jsonmap: "Settlement_Date", index: "Settlement_Date", width: 85, align: "center", sortable: true }
                      , {
                          label: "DtMovimento", name: "DtMovimento", jsonmap: "DtMovimento", index: "DtMovimento", width: 110, align: "center",
                          sortable: true,
                          sorttype: 'date',
                          formatter: 'date',
                          formatoptions: {
                              newformat: 'Y-m-d H:i:s.u',
                              srcformat: 'Y-m-d H:i:s.u',
                          },
                      },
      ]
      , height: 'auto'
      , width: '100%'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , pager: '#jqGrid_Daily_Activity_BuysSells_Sell_Pager'
      , rowNum: 30
      , rowList: [10, 20, 30]
      , caption: 'Sell'
      , sortable: true
    };
}

///Método de preenchimento da grid de trade by trades
function PostTradingClient_Daily_Activity_TradeByTrade() {
    //$("#jqGrid_Daily_Activity_TradebyTrade").jqGrid(
    gGridTradeByTrade =
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "jsonstring"
        //, mtype: "GET"
      , hoverrows: true
        //, postData: pParametros
      , datastr: lMock_TradebyTrade.rows
      , autowidth: false
        //, shrinkToFit: false
      , colModel: [
                        { label: "Exec Time", name: "ExecTime", jsonmap: "ExecTime", index: "ExecTime", width: 55, align: "center", sortable: true }
                      , { label: "Asset", name: "Asset", jsonmap: "Asset", index: "Asset", width: 85, align: "center", sortable: true }
                      , { label: "Market", name: "Market", jsonmap: "Market", index: "Market", width: 55, align: "center", sortable: true }
                      , { label: "Maturity", name: "Maturity", jsonmap: "Maturity", index: "Maturity", width: 85, align: "center", sortable: true }
                      , { label: "Side", name: "Side", jsonmap: "Side", index: "Side", width: 55, align: "center", sortable: true }
                      , { label: "Quantity", name: "Quantity", jsonmap: "Quantity", index: "Quantity", width: 75, align: "center", sortable: true }
                      , { label: "Price", name: "Price", jsonmap: "Price", index: "Price", width: 85, align: "center", sortable: true }
                      , { label: "Trade ID", name: "TradeID", jsonmap: "TradeID", index: "TradeID", width: 95, align: "center", sortable: true }
                      , { label: "Operador DMA", name: "OperadorDMA", jsonmap: "OperadorDMA", index: "OperadorDMA", width: 95, align: "center", sortable: true }
                      , { label: "Destination", name: "Designation", jsonmap: "Designation", index: "Designation", width: 95, align: "center", sortable: true }
                      , { label: "Exchange Fees", name: "ExchangeFees", jsonmap: "ExchangeFees", index: "ExchangeFees", width: 55, align: "center", sortable: true }
                      , { label: "Gradual Fees", name: "GradualFees", jsonmap: "GradualFees", index: "GradualFees", width: 55, align: "center", sortable: true }
                      , { label: "Settlement Date", name: "Settlement_Date", jsonmap: "Settlement_Date", index: "Settlement_Date", width: 85, align: "center", sortable: true }
                      , {
                          label: "DtMovimento", name: "DtMovimento", jsonmap: "DtMovimento", index: "DtMovimento", width: 110, align: "center",
                          sortable: true,
                          sorttype: 'date',
                          formatter: 'date',
                          formatoptions: {
                              newformat: 'Y-m-d H:i:s.u',
                              srcformat: 'Y-m-d H:i:s.u',
                          },
                      },
      ]
      , height: 'auto'
      , width: '100%'
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , pager: '#jqGrid_Daily_Activity_TradebyTrade_Pager'
      , rowNum: 30
      , rowList: [10, 20, 30]
      , caption: 'Trade by Trade'
      , sortable: true
    };
}

//Método que trata o date time picker do jqgrid
var datePick = function (elem) {
    $(elem).datepicker();
}

//Método que Exporta para excel
function ExportDataToExcel(id_grid, filename) {
    ExportJQGridDataToExcel(id_grid, filename);
}

//Método de validação de campo de busca de código de cliente
function ValidarCampoBuscaCodigoCliente(pSender) {

    var lSender = $(pSender).val();

    if (lSender == "") {
        return false;
    } else {
        return true;
    }
}

//Método de validação de campos de busca do campo from do execution time.
function ValidarCamposBuscaExecutionFrom(pSender) {
    var lSender = $(pSender).val();

    if (lSender == "") {
        return false;
    } else {
        SearchByExecutionTimeGeneralView();
        return true;
    }
}

//Método de validação de campos de busca do campo To do execution time.
function ValidarCamposBuscaExecutionTo(pSender) {
    var lSender = $(pSender).val();

    if (lSender == "") {

        return false;
    } else {
        SearchByExecutionTimeGeneralView();
        return true;
    }
}

//Evento KeyDown do campo de busca para filtro de Código do Cliente
function txtDaily_Activity_SearchFor_KeyDown(pSender) {
    $(pSender).one('keypress', function (e) {
        if (e.keyCode == 13) {
            var lSenderValue = $(pSender).val();
            var lSender = $(pSender);

            if (!ValidarCampoBuscaCodigoCliente(pSender)) {
                return;
            }

            var CodigoCliente;

            switch (lSender.attr('id')) {
                case "txtDaily_Activity_GeneralView_SearchFor":
                    {
                        CodigoCliente = $("#txtDaily_Activity_GeneralView_SearchFor").val();

                        //Grid General View
                        $("#gview_jqGrid_Daily_Activity_GeneralView")
                            .find(".ui-jqgrid-title")
                            .html("General View-> Código Cliente: " + CodigoCliente);

                        SendMessageUnsubiscribe();
                        SendMessageSignature(CodigoCliente);
                        gLastClientSearchedGeneralView = CodigoCliente;
                    }
                    break;
                case "txtDaily_Activity_PerAssetClass_SearchFor":
                    {
                        CodigoCliente = $("#txtDaily_Activity_PerAssetClass_SearchFor").val();

                        //Grid PerAssetClass
                        $("#gview_jqGrid_Daily_Activity_PerAssetClass_Equities")
                            .find(".ui-jqgrid-title")
                            .html("Equities-> Código Cliente: " + CodigoCliente);

                        $("#gview_jqGrid_Daily_Activity_PerAssetClass_Futures")
                            .find(".ui-jqgrid-title")
                            .html("Futures-> Código Cliente: " + CodigoCliente);

                        //Limpando a memória do  das listagens das grids
                        gObjectPerAssetClass.length = 0;

                        SendMessageUnsubiscribe();
                        SendMessageSignature(CodigoCliente);
                        gLastClientSearchedPerAssetsClass = CodigoCliente;

                    }
                    break;
                case "txtDaily_Activity_BuysSells_SearchFor":
                    {
                        CodigoCliente = $("#txtDaily_Activity_BuysSells_SearchFor").val();

                        //Grid BuysSells
                        $("#gview_jqGrid_Daily_Activity_BuysSells_Buy")
                            .find(".ui-jqgrid-title")
                            .html("Buy-> Código Cliente:" + CodigoCliente);

                        $("#gview_jqGrid_Daily_Activity_BuysSells_Sell")
                            .find(".ui-jqgrid-title")
                            .html("Sell-> Código Cliente: " + CodigoCliente);

                        SendMessageUnsubiscribe();
                        SendMessageSignature(CodigoCliente);
                        gLastClientSearchedBuysSells = CodigoCliente;

                    }
                    break;
                case "txtDaily_Activity_TradebyTrade_SearchFor":
                    {
                        var lCodigoCliente = $("#txtDaily_Activity_TradebyTrade_SearchFor").val();

                        var lDataDe = $("#txtDaily_Activity_TradebyTrade_FromDate").val();
                        var lDataAte = $("#txtDaily_Activity_TradebyTrade_ToDate").val();

                        var lHoraDe = $("#txtDaily_Activity_TradebyTrade_From").val();
                        var lHoraAte = $("#txtDaily_Activity_TradebyTrade_To").val();

                        var lUrl = "/PostTradingClient.aspx";
                        var lAcao = "CarregarOperacoesCliente";
                        var lDados = { CodigoCliente: lCodigoCliente, Acao: lAcao, DataDe: lDataDe, DataAte: lDataAte, HoraDe: lHoraDe, HoraAte: lHoraAte };

                        Spider_CarregarJsonVerificandoErro(lUrl, lDados);

                        //Grid TradeByTrade
                        /*
                        $("#gview_jqGrid_Daily_Activity_TradebyTrade")
                            .find(".ui-jqgrid-title")
                            .html("Trade by Trade-> Código Cliente:" + CodigoCliente);

                        SendMessageUnsubiscribe();
                        SendMessageSignature(CodigoCliente);
                        gLastClientSearchedTradeByTrade = CodigoCliente;
                        */




                    }
                    break;
            }



            return false;
        }
    });
}

function Spider_CarregarJsonVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso, pOpcoesPosCarregamento, pDivDeFormulario) {
    ///<summary>Carrega o JSON de uma chamada ajax.</summary>
    ///<param name="pUrl"  type="String">URL que será chamada.</param>
    ///<param name="pDadosDoRequest"  type="Objeto_JSON">Dados para a chamada ajax.</param>
    ///<param name="pCallBackDeSucesso"  type="Função_Javascript">(opcional) Função para chamar em caso de sucesso.</param>
    ///<returns>void</returns>

    $.ajax({
        url: pUrl
            , type: "post"
            , cache: false
            , dataType: "json"
            , data: pDadosDoRequest
            , success: function (pResposta) { Spider_CarregarVerificandoErro_CallBack(pResposta, pDivDeFormulario, pCallBackDeSucesso, pOpcoesPosCarregamento); }
            , error: Spider_TratarRespostaComErro
    });
}

function Spider_TratarRespostaComErro() {
    ///<summary>Função que trata uma resposta que seja um objeto JSON com (.TemErro == true) ou um XmlHttpResponse com (.status != 200).</summary>
    ///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
    ///<returns>void</returns>

    //desmarca todos os divs que estiverem "carregando conteúdo"
    $("." + CONST_CLASS_CARREGANDOCONTEUDO).removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

    if (pPainelParaAtualizar != null) {
        $(pPainelParaAtualizar).find("[disabled]").prop("disabled", false);
    }

    if (pResposta.status && pResposta.status != 200) {   //resposta veio como HTML, provavelmente erro do servidor 

        var lTexto = pResposta.responseText;

        lTexto = lTexto.replace(/</gi, "&lt;");
        lTexto = lTexto.replace(/>/gi, "&gt;");

        Spider_ExibirMensagem("E", "Erro do servidor [" + pResposta.status + " - " + pResposta.statusText + "]                                       ", false, lTexto);
    }
    else {
        if (pResposta.TemErro) {
            if (pResposta.Mensagem == "RESPOSTA_SESSAO_EXPIRADA") {
                Spider_ExibirMensagem("E", GradIntra_MensagemDeRedirecionamento(5));

                gGradIntra_RedirecionarParaLoginEm = 4;

                window.setTimeout(GradIntra_TimeoutParaRedirecionarLogin, 1000);
            }
            else {
                Spider_ExibirMensagem("E", pResposta.Mensagem, false, pResposta.MensagemExtendida);
            }
        }
        else {
            var lMensagemExtendida = "";

            try {
                lMensagemExtendida += $.toJSON(pResposta) + "\r\n\r\n";
            }
            catch (pErro1) { }

            try {
                lMensagemExtendida += pResposta + "\r\n\r\n";
            }
            catch (pErro2) { }

            try {
                if (pResposta.responseText) {
                    var lTexto = pResposta.responseText;

                    lTexto = lTexto.replace("<", "&lt;", "gi");
                    lTexto = lTexto.replace(">", "&gt;", "gi");

                    lMensagemExtendida += lTexto;
                }
            }
            catch (pErro3) { }

            Spider_ExibirMensagem("E", "Erro desconhecido                                       ", false, lMensagemExtendida);
        }
    }
}

function GradIntra_ExibirMensagem(pTipo_IAE, pMensagem, pRetornarAoEstadoNormalAposSegundos, pMensagemAdicional) {
    ///<summary>Exibe uma mensagem no painel de alerta.</summary>
    ///<param name="pTipo_IAE"  type="String">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
    ///<param name="pRetornarAoEstadoNormalAposSegundos" type="Boolean">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
    ///<param name="pMensagemAdicional" type="String">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
    ///<returns>void</returns>

    // As cores não podem ser definidas na css porque animação de cor não pega os valores da css

    var lAmarelo = "#e9f68b";
    var lAzul = "#78ccd2";
    var lVemelho = "#d47d7d";
    var lOriginal = "#9BA2AC";

    var lClasse = "";

    var lCor = (pTipo_IAE == "A") ? lAmarelo : ((pTipo_IAE == "I") ? lAzul : ((pTipo_IAE == "E") ? lVemelho : lOriginal));

    $("#lblMensagem").html(pMensagem);

}
//Evento  
function Spider_CarregarVerificandoErro_CallBack(pResposta, pPainelParaAtualizar, pCallBack, pOpcoesPosCarregamento) {
    ///<summary>[CallBack] Função de CallBack para Spider_CarregarJsonVerificandoErro.</summary>
    ///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
    ///<param name="pPainelParaAtualizar"  type="String_ou_Objeto_jQuery">Seletor ou objeto jQuery que irá receber o HTML da resposta ajax.</param>
    ///<param name="pCallBack"  type="Função_Javascript">(opcional) Função para chamar após o carregamento de pPainelParaAtualizar.</param>
    ///<param name="pOpcoesPosCarregamento"  type="Objeto_JSON">(opcional) Opções de execução pós-carregamento. Propriedades suportadas (case-sensitive): {CustomInputs: string[], HabilitarMascaras: bool, HabilitarValidacoes: bool, AtivarToolTips: bool} .</param>
    ///<returns>void</returns>

    if (pResposta != null) {
        if (pResposta.Mensagem) {
            // resposta jSON

            if (pResposta.TemErro) {
                //erro em chamada json
                Spider_TratarRespostaComErro(pResposta, pPainelParaAtualizar);
            }
            else {
                //sucesso em chamada JSON
                if (pCallBack && pCallBack != null)
                    pCallBack(pResposta, pPainelParaAtualizar, pOpcoesPosCarregamento);
            }
        }
        else {   // resposta html
            if (pResposta.indexOf('"TemErro":true,') != -1) {   //erro, porém retorno json em chamada html (ocorre com timeout por exemplo)

                Spider_TratarRespostaComErro($.evalJSON(pResposta));
            }
            else {   //sucesso em resposta HTML

                if (pResposta.indexOf('<fieldset id="pnlLogin">') != -1) {
                    GradIntra_TratarRespostaComErro({ TemErro: true, Mensagem: "RESPOSTA_SESSAO_EXPIRADA" });
                }
                else {
                    if (pPainelParaAtualizar != null) {
                        if (!pPainelParaAtualizar.attr) {
                            //não é objeto jquery

                            if (pPainelParaAtualizar.indexOf('#') !== 0) pPainelParaAtualizar = "#" + pPainelParaAtualizar;

                            pPainelParaAtualizar = $(pPainelParaAtualizar);
                        }

                        pPainelParaAtualizar
                            .html(pResposta)
                            .addClass(CONST_CLASS_CONTEUDOCARREGADO);

                        if (pOpcoesPosCarregamento) {
                            if (pOpcoesPosCarregamento.CustomInputs) {
                                $(pOpcoesPosCarregamento.CustomInputs).each(function () {
                                    $(this + "").customInput();
                                });
                            }
                            /*
                            if (pOpcoesPosCarregamento.HabilitarMascaras)
                                Spider_HabilitarInputsComMascaras(pPainelParaAtualizar);
                            */
                            if (pOpcoesPosCarregamento.HabilitarValidacoes)
                                pPainelParaAtualizar.validationEngine({ showTriangle: false });

                            if (pOpcoesPosCarregamento.AtivarToolTips)
                                GradIntra_AtivarTooltips(pPainelParaAtualizar);

                            if (pOpcoesPosCarregamento.AtivarAutoComplete)
                                pPainelParaAtualizar.find(".AtivarAutoComplete").ComboBoxAutocomplete();
                        }

                        pPainelParaAtualizar.find("." + CONST_CLASS_PICKERDEDATA).datepicker({ showOn: "button" });

                        pPainelParaAtualizar.find(".pstrength").pstrength();    //ja pra verificar a força da senha

                        gTextBoxes = pPainelParaAtualizar.find("input[type='text'], input[type='radio'], select");

                        if (gTextBoxes.length > 0)
                            gTextBoxes.keydown(GradIntra_FocoProximoInput);

                        $(".ui-datepicker").hide();
                    }

                    if (pCallBack && pCallBack != null)
                        pCallBack(pResposta, pPainelParaAtualizar, pOpcoesPosCarregamento);
                }
            }
        }
    }
}

//Evento KeyDown do Campo de busca para filtro de execution From
function txtDaily_Activity_From_KeyDown(pSender) {
    $(pSender).one('keypress', function (e) {
        if (e.keyCode == 13) {
            if (!ValidarCamposBuscaExecutionFrom(pSender)) {
                return;
            }

            var lFrom;

            switch (pSender.attr('id')) {

                case "txtDaily_Activity_GeneralView_SearchFor":

                    lFrom = $("#txtDaily_Activity_GeneralView_From").val();

                    break;

                case "txtDaily_Activity_PerAssetClass_SearchFor":

                    lFrom = $("#txtDaily_Activity_PerAssetClass_From").val();

                    break;

                case "txtDaily_Activity_BuysSells_SearchFor":

                    lFrom = $("#txtDaily_Activity_BuysSells_From").val();

                    break;

                case "txtDaily_Activity_TradebyTrade_SearchFor":

                    lFrom = $("#txtDaily_Activity_TradebyTrade_SearchFor").val();

                    break;
            }


        }
    });
}

//Evento KeyDown do Campo de busca para o filtro de execution To
function txtDaily_Activity_To_KeyDown(pSender) {
    $(pSender).one('keypress', function (e) {
        if (e.keyCode == 13) {
            if (!ValidarCamposBuscaExecutionTo(pSender)) {
                return;
            }

            var lTo;

            switch ($(pSender).attr('id')) {

                case "txtDaily_Activity_GeneralView_To":

                    lTo = $("#txtDaily_Activity_GeneralView_From").val();

                    break;

                case "txtDaily_Activity_GeneralView_To":

                    lTo = $("#txtDaily_Activity_PerAssetClass_From").val();

                    break;

                case "txtDaily_Activity_GeneralView_To":

                    lTo = $("#txtDaily_Activity_BuysSells_From").val();

                    break;

                case "txtDaily_Activity_GeneralView_To":

                    lTo = $("#txtDaily_Activity_TradebyTrade_To").val();

                    break;
            }
        }
    });
}

///Método para mascara para Campos de fromalário
function HabilitarMascaraNumerica(pCampos, pMascara) {
    pCampos
        .bind("keydown", SomenteNumeros_OnKeyDown)
        .mask(pMascara);
}

//Método para validação para o usuário inserir somente digitos numéricos
function SomenteNumeros_OnKeyDown(evt) {
    var key = evt.keyCode;
    var val = evt.target.value;

    //alert(key);
    //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
    if ((key == 13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key == 8) || (key == 0) || (key > 34 && key < 41) || (key == 46) || (key == 9) || (key == 27) || (key > 47 && key < 58) || (key > 95 && key < 106))
    { return true; }
    else
    { if (navigator.userAgent.indexOf("MSIE") == -1) evt.preventDefault(); return false; }
}

///Método para efetuar filtro de execution time do (from / to)
function SearchByExecutionTimeGeneralView() {

    var lNow = new Date();
    var lNowString = lNow.getFullYear() + "-" + (lNow.getMonth() + 1) + "-" + ("0" + lNow.getDate()) + " ";

    var searchFrom = lNowString + $("#txtDaily_Activity_GeneralView_From").val();
    var searchTo = lNowString + $("#txtDaily_Activity_GeneralView_To").val();

    //var lHourSearchFrom = Date.parse(searchFrom)
    //var lHourSearchTo = Date.parse(searchTo);
    var f = { groupOp: "AND", rules: [] };

    f.rules.push({ field: "DtMovimento", op: 'gt', data: searchFrom });
    f.rules.push({ field: "DtMovimento", op: 'lt', data: searchTo });

    var grid = $("#jqGrid_Daily_Activity_GeneralView");

    grid[0].p.search = f.rules.length > 0;;

    //grid[0].p.search = f.rules.length > 0;

    $.extend(grid[0].p.postData, { filters: JSON.stringify(f) });

    grid.trigger("reloadGrid", [{ page: 1 }]);
}

/*
Dados mock de preenchimento de grid de General view
*/
var lMock_GeneralView = {
    "page": "1", "total": 3, "records": "13", "rows": [
        /*
                       { Asset: "PETR1",    Market: "BOV",  Maturity: '01/01/2010', ExecQtyBuy:'1',     AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015"},
                       { Asset: "PETR2",    Market: "BOV",  Maturity: '02/01/2010', ExecQtyBuy: '2',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR3",    Market: "BOV",  Maturity: '03/01/2010', ExecQtyBuy: '3',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR4",    Market: "BOV",  Maturity: '04/01/2010', ExecQtyBuy: '4',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR5",    Market: "BOV",  Maturity: '05/01/2010', ExecQtyBuy: '5',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR6",    Market: "BOV",  Maturity: '06/01/2010', ExecQtyBuy: '6',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR7",    Market: "BOV",  Maturity: '07/01/2010', ExecQtyBuy: '7',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR8",    Market: "BOV",  Maturity: '08/01/2010', ExecQtyBuy: '8',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR9",    Market: "BOV",  Maturity: '09/01/2010', ExecQtyBuy: '9',    AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR10",   Market: "BOV",  Maturity: '10/01/2010', ExecQtyBuy: '10',   AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR11",   Market: "BOV",  Maturity: '11/01/2010', ExecQtyBuy: '11',   AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR12",   Market: "BOV",  Maturity: '12/01/2010', ExecQtyBuy: '12',   AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" },
                       { Asset: "PETR13",   Market: "BOV",  Maturity: '13/01/2010', ExecQtyBuy: '13',   AveragePriceBuy: '7.30', ExecQtySell: '8',AveragePriceSell: '7.30', Realized_PL: '5.47', NET_Qty: '7', Total_Qty: '85', Settlement_Amount: '10/12/2015', Settlement_Date: "12/12/2015" }
                       */
    ]
};

/*
Dados de mock de preechimento de grid de Per asset class (Equities)
*/
var lMock_PerAssetClassEquities = {
    "page": "1", "total": 3, "records": "13", "rows": [

               { Asset: "PETR1", Market: "BOV", Maturity: '01/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '1', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR2", Market: "BOV", Maturity: '02/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '2', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR3", Market: "BOV", Maturity: '03/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '3', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR4", Market: "BOV", Maturity: '04/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '4', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR5", Market: "BOV", Maturity: '05/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '5', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR6", Market: "BOV", Maturity: '06/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '6', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR7", Market: "BOV", Maturity: '07/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '7', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR8", Market: "BOV", Maturity: '08/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '8', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR9", Market: "BOV", Maturity: '09/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '9', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR10", Market: "BOV", Maturity: '10/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '10', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR11", Market: "BOV", Maturity: '11/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '11', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR12", Market: "BOV", Maturity: '12/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '12', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "PETR13", Market: "BOV", Maturity: '13/01/2010', QtyBuy: '14', AveragePriceBuy: '12.54', GrossAmountBuy: '13', QtySell: '12', AveragePriceSell: '7.30', GrossAmountSell: '4', AveragePriceSell: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" }

    ]
};

/*
Dados de mock de preenchimento de SUBGRID de Per asset class (Equities)
*/

var lMock_PerAssetClassEquitiesSubgrid = {
    "page": "1", "total": 3, "records": "13", "rows": [
    ]
};

/*
Dados de mock de preechimento de grid de Per asset class (Futures)
*/
var lMock_PerAssetClassFutures = {
    "page": "1", "total": 3, "records": "13", "rows": [

               { Asset: "WINV11", Market: "BMF", Maturity: '01/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '1', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV12", Market: "BMF", Maturity: '02/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '2', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV13", Market: "BMF", Maturity: '03/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '3', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV14", Market: "BMF", Maturity: '04/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '4', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV15", Market: "BMF", Maturity: '05/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '5', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV16", Market: "BMF", Maturity: '06/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '6', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV17", Market: "BMF", Maturity: '07/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '7', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV18", Market: "BMF", Maturity: '08/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '8', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "WINV19", Market: "BMF", Maturity: '09/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '9', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "INDQ10", Market: "BMF", Maturity: '10/01/2010', QtyBuy: '5', AveragePriceBuy: '52418', GrossAmountBuy: '10', QtySell: '12', AveragePriceSell: '51896', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "INDQ11", Market: "BMF", Maturity: '11/01/2010', QtyBuy: '3', AveragePriceBuy: '56875', GrossAmountBuy: '11', QtySell: '12', AveragePriceSell: '51265', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "INDQ12", Market: "BMF", Maturity: '12/01/2010', QtyBuy: '3', AveragePriceBuy: '56875', GrossAmountBuy: '12', QtySell: '12', AveragePriceSell: '51265', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" },
               { Asset: "INDQ13", Market: "BMF", Maturity: '13/01/2010', QtyBuy: '3', AveragePriceBuy: '56875', GrossAmountBuy: '13', QtySell: '12', AveragePriceSell: '51265', GrossAmountSell: '5', AveragePriceSell: '51487', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '9.85', Settlement_Amount: '18/01/2015', Settlement_Date: "12/12/2015" }

    ]
};

var lMock_BuysSellsSell = {
    "page": "1", "total": 3, "records": "13", "rows": [
        /*
               { Asset: "WINV11" , Market: "BMF", Maturity: '01/01/2010', OperadorDMA: 'DMA50', TradeID: '1252411', Quantity: '1',   Price: '56212', Settlement_Date: "12/12/2015" },
               { Asset: "WINV12" , Market: "BMF", Maturity: '02/01/2010', OperadorDMA: 'DMA50', TradeID: '1252412', Quantity: '2',   Price: '56212', Settlement_Date: "12/12/2015" },
               { Asset: "WINV13" , Market: "BMF", Maturity: '03/01/2010', OperadorDMA: 'DMA50', TradeID: '1252413', Quantity: '3',   Price: '56212', Settlement_Date: "12/12/2015" },
               { Asset: "WINV14" , Market: "BMF", Maturity: '04/01/2010', OperadorDMA: 'DMA50', TradeID: '1252414', Quantity: '4',   Price: '56212', Settlement_Date: "12/12/2015" },
               { Asset: "WINV15" , Market: "BMF", Maturity: '05/01/2010', OperadorDMA: 'DMA50', TradeID: '1252415', Quantity: '5',   Price: '56212', Settlement_Date: "12/12/2015" },
               { Asset: "WINV16" , Market: "BMF", Maturity: '06/01/2010', OperadorDMA: 'DMA50', TradeID: '1252416', Quantity: '6',   Price: '56212', Settlement_Date: "12/12/2015" },
               { Asset: "PETR7" ,  Market: "BOV", Maturity: '07/01/2010', OperadorDMA: 'DMA25', TradeID: '1252417', Quantity: '7',   Price: '7.52',  Settlement_Date: "12/12/2015" },
               { Asset: "PETR8" ,  Market: "BOV", Maturity: '08/01/2010', OperadorDMA: 'DMA25', TradeID: '1252418', Quantity: '8',   Price: '7.52',  Settlement_Date: "12/12/2015" },
               { Asset: "PETR9" ,  Market: "BOV", Maturity: '09/01/2010', OperadorDMA: 'DMA25', TradeID: '1252419', Quantity: '9',   Price: '7.52',  Settlement_Date: "12/12/2015" },
               { Asset: "PETR1" ,  Market: "BOV", Maturity: '10/01/2010', OperadorDMA: 'DMA25', TradeID: '1252410', Quantity: '10',  Price: '7.52',  Settlement_Date: "12/12/2015" },
               { Asset: "PETR1" ,  Market: "BOV", Maturity: '11/01/2010', OperadorDMA: 'DMA30', TradeID: '1256875', Quantity: '11',  Price: '7.52',  Settlement_Date: "12/12/2015" },
               { Asset: "PETR1" ,  Market: "BOV", Maturity: '12/01/2010', OperadorDMA: 'DMA30', TradeID: '1256877', Quantity: '12',  Price: '7.52',  Settlement_Date: "12/12/2015" },
               { Asset: "PETR1" ,  Market: "BOV", Maturity: '13/01/2010', OperadorDMA: 'DMA30', TradeID: '1256879', Quantity: '13',  Price: '7.52',  Settlement_Date: "12/12/2015" }
               */
    ]
};

var lMock_TradebyTrade = {
    "page": "1", "total": 3, "records": "13", "rows": [
        /*
               {ExecTime:'12:58', Asset: "PETR1", Market: "BOV", Maturity: '01/01/2010', Side:  'C', Quantity: '1',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR2", Market: "BOV", Maturity: '02/01/2010', Side:  'C', Quantity: '2',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR3", Market: "BOV", Maturity: '03/01/2010', Side:  'C', Quantity: '3',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR4", Market: "BOV", Maturity: '04/01/2010', Side:  'C', Quantity: '4',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR5", Market: "BOV", Maturity: '05/01/2010', Side:  'C', Quantity: '5',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR6", Market: "BOV", Maturity: '06/01/2010', Side:  'C', Quantity: '6',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR7", Market: "BOV", Maturity: '07/01/2010', Side:  'V', Quantity: '7',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR8", Market: "BOV", Maturity: '08/01/2010', Side:  'V', Quantity: '8',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR9", Market: "BOV", Maturity: '09/01/2010', Side:  'V', Quantity: '9',   TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47', GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR10", Market: "BOV", Maturity: '10/01/2010', Side: 'V', Quantity: '10', TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47',  GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR11", Market: "BOV", Maturity: '11/01/2010', Side: 'V', Quantity: '11', TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47',  GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR12", Market: "BOV", Maturity: '12/01/2010', Side: 'V', Quantity: '12', TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47',  GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" },
               {ExecTime:'12:58', Asset: "PETR13", Market: "BOV", Maturity: '13/01/2010', Side: 'V', Quantity: '13', TradeID:'123456789', Price: '7.30', OperadorDMA: 'DMA858', Designation: '7.30', ExchangeFees: '1.47',  GradualFees: '0.7', Realized_PL: '12.85', Settlement_Date: "12/12/2015" }
               */
    ]
};

///Fuñcão de Conexão para chamadas REST
function CreateRequestREST() {
    var result = null;
    if (window.XMLHttpRequest) {
        // FireFox, Safari, etc.
        result = new XMLHttpRequest();
        if (typeof xmlhttp.overrideMimeType != 'undefined') {
            result.overrideMimeType('text/xml'); // Or anything else
        }
    }
    else if (window.ActiveXObject) {
        // MSIE
        result = new ActiveXObject("Microsoft.XMLHTTP");
    }
    else {
        // No known mechanism -- consider aborting the application
    }
    return result;
}

function RequestREST(pUrl) {
    var req = CreateRequestREST(); // defined above
    // Create the callback:
    req.onreadystatechange = function () {
        if (req.readyState != 4) return; // Not there yet
        if (req.status != 200) {
            // Handle request failure here...
            return;
        }
        // Request successful, read the response
        var resp = req.responseText;
        

        return resp;
    }
}

function SomenteNumero(e)
{
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58))
    {
        return true;
    }
    else
    {
        if (tecla == 8 || tecla == 0) {
            return true;
        }
        else
        {
            return false;
        }
    }
}
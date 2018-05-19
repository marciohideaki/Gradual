/*----------------------------------------------------------------*/
/*Funções gerais da position client de risco - Risco Resumido------*/
/*----------------------------------------------------------------*/

function ConnectSocketServerRiscoResumido()
{
    var lSuport = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

    var lWsConnect = $("#hddConnectionSocketRiscoResumido").val();

    if (lSuport == null) {
        alert(NOSUPORTEMESSAGE);
        return;
    }

    var lStatusConnection = $("#lblStatusConnection");

    lStatusConnection.html("***  Connecting to server ....");

    gWebSocketRiscoResumido = new WebSocket(lWsConnect); // new window[lSuport](lWsConnect);

    ///Evento que é chamado quando chega mensagem de position client
    gWebSocketRiscoResumido.onmessage = function (evt) {
        var pMessage = evt.data;

        AppendMessageRiscoResumido(pMessage);
    }

    ///Evento que é chamado quando a conexão é aberta 
    gWebSocketRiscoResumido.onopen = function () {
        lStatusConnection.html("* Connection Opened!");
    }

    ///Evento que é chamado quando a conexão é fechada
    gWebSocketRiscoResumido.onclose = function () {
        lStatusConnection.html("** Connection closed!");
    }
}

//Método que recebe a mensagem do websocket e trata de acordo 
//com o tipo de mensagem e insere/altera
//no grid específico
var gMessageCount = 0;
function AppendMessageRiscoResumido(pMessage) {

    var lGrid = $("#jqGrid_RiscoResumido");

    
    var lData;
    var lRowID;

    if (pMessage == "") return;

    var lJSON = $.parseJSON(pMessage);

    var lData = lJSON; 

    var lDataRiscoResumido = TransporteDataRiscoResumido(lData);

    gMessageCount++;

    console.log("Chegou Dados->" + lDataRiscoResumido.CodigoCliente + " Mensagem " + gMessageCount );

    RiscoResumidoSetRowDataObjectonGrid(lGrid, lDataRiscoResumido);
}

///Transporte da grid de Risco Resumido
function TransporteDataRiscoResumido(pData)
{
    var lRetorno =
    {
        Semaforo        :'',        
        CodigoCliente   :'',   
        CustodiaAbertura:'',
        CCAbertura      :'',      
        Garantias       :'',       
        Produtos        :'',        
        TotalAbertura   :'',   
        PLBovespa       :'',       
        PLBmf           :'',           
        PLTotal         :'',         
        SFP             :'',             
        PercAtingido    :'',    
    };

    lRetorno.CodigoCliente      = pData.Account;
    lRetorno.CustodiaAbertura   = pData.TotalCustodiaAbertura;
    lRetorno.CCAbertura         = pData.TotalContaCorrenteAbertura;
    lRetorno.Garantias          = pData.TotalGarantias;
    lRetorno.Produtos           = pData.TotalProdutos;
    lRetorno.TotalAbertura      = pData.SaldoTotalAbertura;
    lRetorno.PLBovespa          = pData.PLBovespa;
    lRetorno.PLBmf              = pData.PLBmf;
    lRetorno.PLTotal            = pData.PLTotal;
    lRetorno.PercAtingido       = pData.TotalPercentualAtingido;
        
    return lRetorno;
}

///Método para inserir ou editar os dados nas grids
function RiscoResumidoSetRowDataObjectonGrid(pGrid, pObjeto) {

    var lEncontrou = false;

    var rowIds = pGrid.jqGrid('getDataIDs');

    for (row = 0; row <= rowIds.length; row++) {

        rowData = pGrid.jqGrid('getRowData', row);

        if (rowData['CodigoCliente'] == pObjeto.CodigoCliente) {

            var lRow = jQuery('#jqGrid_RiscoResumido tr:eq(' + row + ')');

            pGrid.jqGrid('setRowData', row, pObjeto);

            //lRow.closest("tr").find("td:gt(0)").effect("highlight", {}, 700);

            lEncontrou = true;

            break;
        }

    }

    if (!lEncontrou)
    {
        var lastRowDOM = pGrid.jqGrid('getGridParam', 'records');

        lRowID = lastRowDOM + 1;

        pGrid.addRowData(lRowID, pObjeto);
    }
}

///Método que envia solicitação de assinatura para a sessão da conexão 
function SendMessageSignatureRiscoResumido(CodigoCliente)
{
    if (gWebSocketRiscoResumido) {
        var lMessageToSend = "SUBSCRIBE " + CodigoCliente;
        gWebSocketRiscoResumido.send(lMessageToSend);

        gLastClientSearchedRiscoResumido = CodigoCliente;
    }
}

///Remove a assinatura do ultimo cliente assinado
function SendMessageUnsubiscribeRiscoResumido()
{
    if (gWebSocketRiscoResumido && gLastClientSignedOperacoesIntraday != "") {
        lMessageToSend = "UNSUBSCRIBE " + gLastClientSignedOperacoesIntraday;

        //$("#jqGrid_OperacoesIntraday").jqGrid('clearGridData');

        gWebSocketRiscoResumido.send(lMessageToSend);
    }
}
/*----------------------------------------------------------------*/
/*--Controle Risco--Risco Resumido---------------------------------*/
/*----------------------------------------------------------------*/

///Ultimo Filtro efetualdo
var gLastClientSearchedRiscoResumido;

//Grids de Position Client montagem
var gGridRiscoResumido;

//Ultimo Cliente assinado no socket de risco resumido 
var gLastClientSignedRiscoResumido;

var gRiscoResumidoTimerRefreshGrid;

var gRiscoResumidoTimerAtualizacaoAutomatica = 0;

var gRiscoResumidoPrimeiroRequest = true;

var gRiscoResumidoEhParaAtualizar = true;

var gRiscoResumidoListaClientes = [];
/*
Dados mock de preenchimento de grid de Operações Intraday
*/
var lMock_Risco_Resumido = {
    "page": "1", "total": 3, "records": "13", "rows": [
        /*
                       { CodigoCliente: 31940, CustodiaAbertura:10000,  CCAbertura: 10500,  Garantias: 0,       Produtos: 0,     TotalAbertura: 20500,  PLBovespa: -8000,  PLBmf: 0,    PLTotal: -8000,     SFP:12500,  PercAtingido: 6,     },
                       { CodigoCliente: 31873, CustodiaAbertura:0,      CCAbertura: 0,      Garantias: 5000,    Produtos: 0,     TotalAbertura: 5000,   PLBovespa: 0,      PLBmf: 2800, PLTotal: 2800,      SFP:7800,   PercAtingido: 0,     },
                       { CodigoCliente: 55350, CustodiaAbertura:5000,   CCAbertura: -1000,  Garantias: 10000,   Produtos: 50000, TotalAbertura: 64000,  PLBovespa: -1000,  PLBmf: -3000,PLTotal: -17000,    SFP:60000,  PercAtingido: 33.3,  },
                       { CodigoCliente: 31940, CustodiaAbertura:10000,  CCAbertura: 10500,  Garantias: 0,       Produtos: 0,     TotalAbertura: 20500,  PLBovespa: -8000,  PLBmf: 0,    PLTotal: -8000,     SFP:12500,  PercAtingido: 61,    },
                       { CodigoCliente: 31873, CustodiaAbertura:0,      CCAbertura: 0,      Garantias: 5000,    Produtos: 0,     TotalAbertura: 5000,   PLBovespa: 0,      PLBmf: 2800, PLTotal: 2800,      SFP:7800,   PercAtingido: 0,     },
                       { CodigoCliente: 55350, CustodiaAbertura:5000,   CCAbertura: -1000,  Garantias: 10000,   Produtos: 50000, TotalAbertura: 64000,  PLBovespa: -1000,  PLBmf: -3000,PLTotal: -17000,    SFP:60000,  PercAtingido: 33.3,  },
                       { CodigoCliente: 31940, CustodiaAbertura:10000,  CCAbertura: 10500,  Garantias: 0,       Produtos: 0,     TotalAbertura: 20500,  PLBovespa: -8000,  PLBmf: 0,    PLTotal: -8000,     SFP:12500,  PercAtingido: 61,    },
                       { CodigoCliente: 31873, CustodiaAbertura:0,      CCAbertura: 0,      Garantias: 5000,    Produtos: 0,     TotalAbertura: 5000,   PLBovespa: 0,      PLBmf: 2800, PLTotal: 2800,      SFP:7800,   PercAtingido: 0,     },
                       { CodigoCliente: 55350, CustodiaAbertura:5,      CCAbertura: -1000,  Garantias: 1000,    Produtos: 50000, TotalAbertura: 64000,  PLBovespa: -1000,  PLBmf: -3000,PLTotal:  -17000,   SFP:60000,  PercAtingido: 33.3,  },*/
    ]
};

///Função de gerenciamento de carregamento de grid de Operações Intraday
function PositionClient_Risco_Resumido_Grid() {
    gGridRiscoResumido =
    {
        //url: "MonitoramentoRiscoGeralDetalhamento.aspx?Acao=BuscarCustodia"
        datatype: "jsonstring"
        //, mtype: "GET"
      , hoverrows: true
      , datastr: lMock_Risco_Resumido.rows
      , autowidth: false
        //, shrinkToFit: false
      , colModel: [
                        { label: "Remover" ,        name: "Remover",                jsonmap: "Remover",             index: "Remover",               width: 25, align: "center"}
                      , { label: "",                name: "Semaforo",               jsonmap: "Semaforo",            index: "Semaforo",              width: 25, align: "center", sortable: true }
                      , { label: "Cliente",         name: "CodigoCliente",          jsonmap: "CodigoCliente",       index: "CodigoCliente",         width: 65, align: "center", sortable: true }
                      , { label: "R$ Cust Abert",   name: "CustodiaAbertura",       jsonmap: "CustodiaAbertura",    index: "CustodiaAbertura",      width: 65, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "R$ CC Abert",     name: "CCAbertura",             jsonmap: "CCAbertura",          index: "CCAbertura",            width: 55, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "R$ Garantias",    name: "Garantias",              jsonmap: "Garantias",           index: "Garantias",             width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "R$ Produtos",     name: "Produtos",               jsonmap: "Produtos",            index: "Produtos",              width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "Total Abertura",  name: "TotalAbertura",          jsonmap: "TotalAbertura",       index: "TotalAbertura",         width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "P&L Bovespa",     name: "PLBovespa",              jsonmap: "PLBovespa",           index: "PLBovespa",             width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "P&L Bmf",         name: "PLBmf",                  jsonmap: "PLBmf",               index: "PLBmf",                 width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "P&L Total",       name: "PLTotal",                jsonmap: "PLTotal",             index: "PLTotal",               width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "R$ SFP",          name: "SFP",                    jsonmap: "SFP",                 index: "SFP",                   width: 75, align: "right" , sortable: true ,formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',    prefix: 'R$ '}}
                      , { label: "% Atingido",      name: "PercAtingido",           jsonmap: "PercAtingido",        index: "PercAtingido",          width: 75, align: "center", sortable: true ,formatter: 'number'  , formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, defaultValue: '0.00',  suffix: ' %'}}
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
      //, multiselect: true
      //, pager: '#jqGrid_RiscoResumido_Pager'
      , rowNum: 1000
      //, rowList: [10, 20, 30]
      , caption: 'Risco Resumido'
      , loadComplete: function () { }
      , afterInsertRow: SetColorRiscoResumidoInsertRow
        //, subGridRowExpanded: subGridRowExpandendPerAssetClassFutures
      , sortable: true
    };
}

function SetColorRiscoResumidoInsertRow(rowid, pData)
{
    var lColorRed   = "#DE8C8A";
    var lColorYelow = "#F2F2B3";
    var lColorWhite = "#FFFFFF";
    var lColorGreen = "#8CF75E";
    var lColorFont  = "#2e7db2";

    var lGrid = $("#jqGrid_RiscoResumido");

    if (pData.PercAtingido > 0) {
        lGrid.jqGrid('setCell', rowid, 'Semaforo', '', { 'background-color': lColorGreen, 'color': lColorFont });
    }

    if ( pData.PercAtingido > (-50)  && pData.PercAtingido <= 0) {
        lGrid.jqGrid('setCell', rowid, 'Semaforo', '', { 'background-color': lColorWhite, 'color': lColorFont });
    }

    if (pData.PercAtingido < (-50) && pData.PercAtingido > (-75)) {
        lGrid.jqGrid('setCell', rowid, 'Semaforo', '', { 'background-color': lColorYelow, 'color': lColorFont });
    }

    if (pData.PercAtingido < (-75) ) {
        lGrid.jqGrid('setCell', rowid, 'Semaforo', '', { 'background-color': lColorRed, 'color': lColorFont });
    }

    var lRow = $("#jqGrid_RiscoResumido tr[id=" + rowid + "]");

    var lCheck = jQuery(this).find('#' + rowid + ' input[type=checkbox]');

    lRow.children("td").eq(0).html("<button class=\"btn btn-danger btn-xs btn-block\" id=\"Risco_Resumido_Cliente_" + pData.CodigoCliente+
                                   "\" onclick=\"return RiscoResumido_Excluir_Cliente_Click(this, "+ pData.CodigoCliente+", " + rowid + ");\" alt=\"Remover Cliente\" >[X]</button>");
}

function RiscoResumido_Excluir_Cliente_Click(pSender, CodigoCliente, rowid) {

    var lRow = $("#jqGrid_RiscoResumido tr[id=" + rowid + "]");

    $('#jqGrid_RiscoResumido').jqGrid('delRowData', rowid);

    if ($.inArray(gRiscoResumidoListaClientes != -1))
    {
        gRiscoResumidoListaClientes = $.grep(gRiscoResumidoListaClientes, function (value) { return value != CodigoCliente; });
    }

    return false;
}

///Método que prepara a grid de operações intraday com o layout 
///de colunas e funcionalidades de Footer
function PreparaGridRiscoResumido() {
    var lGrid = $("#jqGrid_RiscoResumido");

    lGrid.jqGrid(gGridRiscoResumido);
    /*
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
    */
    //lGrid.jqGrid('navGrid', '#jqGrid_RiscoResumido_Pager',
    //    { add: false, edit: false, del: false },
    //    {},
    //    {},
    //    {},
    //    { multipleSearch: false, overlay: false });

    //lGrid.jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn' });

    //lGrid.jqGrid('navButtonAdd', '#jqGrid_RiscoResumido_Pager',
    //{
    //    caption: 'Filtro',
    //    title: 'Toggle Searching Toolbar',
    //    buttonicon: 'ui-icon-pin-s',
    //    onClickButton: function () { lGrid[0].toggleToolbar(); }
    //});

    lGrid.jqGrid('navButtonAdd', '#jqGrid_RiscoResumido_Pager',
    {
        caption: 'Export to Excell',
        //title: 'Toggle Searching Toolbar',
        buttonicon: 'ui-icon-disk',
        onClickButton: function () {
            ExportDataToExcel('#jqGrid_RiscoResumido', 'GeneralView.xls');
            //lGrid[0].toggleToolbar();
        }
    });

    //lGrid[0].toggleToolbar();
}

function txtRiscoResumido_Cliente_KeyDown(pSender) {
    $(pSender).one('keypress', function (e) {
        if (e.keyCode == 13) {
            var lSenderValue = $(pSender).val();

            var lSender = $(pSender);

            RiscoResumido_Adiciona_Cliente_Monitoramento();

            RiscoResumido_BuscarDados();

            return false;
        }

    }).one('keyup', function (e) {
        if (e.keyCode == 8) {
            gRiscoResumidoEhParaAtualizar = false;
        }
    });
}

///Método que efetua a busca dos dados de risco resumido
function RiscoResumido_BuscarDados()
{
    if (!RiscoResumido_Validar_filtro()) {
        return false;
    }

    gRiscoResumidoEhParaAtualizar = true;

    LimpaLinhasSelecionadasAnteriormente();

    gRiscoResumidoTimerAtualizacaoAutomatica = 5;

    gRiscoResumidoPrimeiroRequest = true;

    window.clearTimeout(gRiscoResumidoTimerRefreshGrid);

    RiscoResumido_TemporizadorRefresh_Grid();

    $('#jqGrid_RiscoResumido').jqGrid('clearGridData');
}

///Aplica Todos os filtros ativos na tela de Risco Resumido
function RiscoResumido_Aplicar_Filtros_Click(pSender) {

    RiscoResumido_BuscarDados();

    return false;
}

function RiscoResumido_TemporizadorRefresh_Grid()
{

    var lCarregarComSocket = $("#chkCarregarViaSocketRiscoResumido").is(":checked");
    var lRequestSocket;

    var lRequest = MontaRiscoResumidoRequestREST();

    if (!RiscoResumido_Validar_filtro())
    {
        window.clearTimeout(gRiscoResumidoTimerRefreshGrid);

        return;
    }

    //$('#jqGrid_RiscoResumido').jqGrid('clearGridData');

    if (lCarregarComSocket)
    {
        lRequestSocket = JSON.stringify(lRequest);

        SendMessageSignatureRiscoResumido(lRequestSocket);
    }
    else
    {
        var lHorarioAtual = new Date();

        gRiscoResumidoTimerRefreshGrid = 0;

        window.clearTimeout(gRiscoResumidoTimerRefreshGrid);

        gRiscoResumidoTimerRefreshGrid = window.setTimeout("RiscoResumido_Verifica_Refresh()", 5000);

        RiscoResumido_Refresh_Bind_Grid();
    }
}

///Método que efetua a validação do filtro efetuado na tela de operações intraday
function RiscoResumido_Validar_filtro() {
    var Objeto =MontaRiscoResumidoRequestREST();

    var lSocketChecado = $("#chkCarregarViaSocketRiscoResumido").is(":checked")

    if (lSocketChecado && Objeto.CodigoCliente == "0" ) {
        alert("Insira o Código do cliente");

        return false;
    }

    return true;
}

function RiscoResumido_Verifica_Refresh()
{
    if (!gRiscoResumidoEhParaAtualizar)
        return;

    if ((gRiscoResumidoTimerAtualizacaoAutomatica - 1) >= 0)
    {
        gRiscoResumidoTimerAtualizacaoAutomatica = gRiscoResumidoTimerAtualizacaoAutomatica - 1;

        setTimeout('RiscoResumido_Verifica_Refresh()', 1000);
    }
    else
    {
        RiscoResumido_TemporizadorRefresh_Grid();
    }
}

function RiscoResumido_Refresh_Bind_Grid() {

    gRiscoResumidoTimerAtualizacaoAutomatica = 5;

    RiscoResmidoRESTService();
    //var lRequest = MontaRiscoResumidoRequest();

    //lRequest.acao = 'CarregarRiscoResumido';

    //Spider_CarregarJsonVerificandoErro('Backend/RiscoResumido.aspx', lRequest, RiscoResumido_Verifica_Refresh_Callback, null, null)
}

///Método que retorna o Filtro configurado pelo usuário em Risco Resumido
function MontaRiscoResumidoRequest() {
    var lRetorno =
        {
            acao: '',
            CodigoCliente:                      $("#txtRiscoResumidoCliente").val(),
            OpcaoPLSomenteComLucro:             $("#chkSomenteComLucro").is(':checked'),
            OpcaoPLSomentePLnegativo:           $("#chkSomentePLNegativo").is(':checked'),
            OpcaoSFPAtingidoAte25:              $("#chkSFPAtingidoAte25").is(':checked'),
            OpcaoSFPAtingidoEntre25e50:         $("#chkSFPAtingidoEntre25e50").is(':checked'),
            OpcaoSFPAtingidoEntre50e75:         $("#chkSFPAtingidoEntre50e75").is(':checked'),
            OpcaoSFPAtingidoAcima75:            $("#chkSFPAtingidoAcima75").is(':checked'),
            OpcaoPrejuizoAtingidoAte2K:         $("#chkRSAtingidoAte2K").is(':checked'),
            OpcaoPrejuizoAtingidoEntre2Ke5K:    $("#chkRSAtingidoEntre2ke5k").is(':checked'),
            OpcaoPrejuizoAtingidoEntre5Ke10K:   $("#chkRSAtingidoEntre5Ke10K").is(':checked'),
            OpcaoPrejuizoAtingidoEntre10Ke20K:  $("#chkRSAtingidoEntre10Ke20K").is(':checked'),
            OpcaoPrejuizoAtingidoEntre20Ke50K:  $("#chkRSAtingidoEntre20Ke50K").is(':checked'),
            OpcaoPrejuizoAtingidoAcima50K:      $("#chkRSAtingidoAcima50K").is(':checked'),
            ListaClientes                       : gRiscoResumidoListaClientes
        };

    return lRetorno;
}

function MontaRiscoResumidoRequestREST() {
    var lRetorno =
        {
            //CodigoCliente:                      $("#txtRiscoResumidoCliente").val() == "" ? "0" : $("#txtRiscoResumidoCliente").val(),
            OpcaoPLSomenteComLucro:             $("#chkSomenteComLucro")        .is(':checked'),
            OpcaoPLSomentePLnegativo:           $("#chkSomentePLNegativo")      .is(':checked'),
            OpcaoSFPAtingidoAte25:              $("#chkSFPAtingidoAte25")       .is(':checked'),
            OpcaoSFPAtingidoEntre25e50:         $("#chkSFPAtingidoEntre25e50")  .is(':checked'),
            OpcaoSFPAtingidoEntre50e75:         $("#chkSFPAtingidoEntre50e75")  .is(':checked'),
            OpcaoSFPAtingidoAcima75:            $("#chkSFPAtingidoAcima75")     .is(':checked'),
            OpcaoPrejuizoAtingidoAte2K:         $("#chkRSAtingidoAte2K")        .is(':checked'),
            OpcaoPrejuizoAtingidoEntre2Ke5K:    $("#chkRSAtingidoEntre2ke5k")   .is(':checked'),
            OpcaoPrejuizoAtingidoEntre5Ke10K:   $("#chkRSAtingidoEntre5Ke10K")  .is(':checked'),
            OpcaoPrejuizoAtingidoEntre10Ke20K:  $("#chkRSAtingidoEntre10Ke20K") .is(':checked'),
            OpcaoPrejuizoAtingidoEntre20Ke50K:  $("#chkRSAtingidoEntre20Ke50K") .is(':checked'),
            OpcaoPrejuizoAtingidoAcima50K:      $("#chkRSAtingidoAcima50K")     .is(':checked'),
            ListaClientes                       : gRiscoResumidoListaClientes
        };

    return lRetorno;
}

///Método para adicionar Cliente na lista de monitoramento de risco resumido
function RiscoResumido_Adiciona_Cliente_Monitoramento() {
    var lCodigoCliente = $("#txtRiscoResumidoCliente").val() == "" ? "0" : $("#txtRiscoResumidoCliente").val();

    if (lCodigoCliente != "0") {

        gRiscoResumidoListaClientes.push(lCodigoCliente);
    }

    $("#txtRiscoResumidoCliente").val("");
}

///Evento disparado no click do botão de adicionar cliente no risco resumido
function RiscoResumido_Adicionar_Cliente_Click(pSender)
{
    if ($("#txtRiscoResumidoCliente").val().length < 2) {

        alert("É necessário inserir o código de um cliente");

        return false;

    }

    RiscoResumido_Adiciona_Cliente_Monitoramento();

    RiscoResumido_BuscarDados();

    return false;
}

function RiscoResumido_Remover_Todos_Filtros_Click(pSender) {

    gRiscoResumidoPrimeiroRequest = true;

    gRiscoResumidoEhParaAtualizar = false;

    gRiscoResumidoListaClientes = [];

    window.clearTimeout(gRiscoResumidoTimerRefreshGrid);

    $('#jqGrid_RiscoResumido').jqGrid('clearGridData');
}

function RiscoResumido_Verifica_Refresh_Callback(pResposta)
{
    if (!pResposta.TemErro)
    {
        lMock_Risco_Resumido = { "page": "1", "total": 3, "records": pResposta.ObjetoDeRetorno.length, "rows": pResposta.ObjetoDeRetorno };

        $('#jqGrid_RiscoResumido').jqGrid('clearGridData');

        $('#jqGrid_RiscoResumido').jqGrid('setGridParam', { data: lMock_Risco_Resumido.rows });

        $('#jqGrid_RiscoResumido').trigger('reloadGrid');
    }

    var CodigoCliente = $("#txtRiscoResumidoCliente").val();

    SendMessageUnsubiscribeRiscoResumido();

    if (CodigoCliente != "")
    {
        //Grid Risco Resumido
        $("#jqGrid_RiscoResumido")
            .find(".ui-jqgrid-title")
            .html("Risco Resumido -> Código Cliente: " + CodigoCliente);

        SendMessageSignatureRiscoResumido(CodigoCliente);

        gLastClientSignedRiscoResumido = CodigoCliente;
    } else
    {
        gLastClientSignedRiscoResumido = "";
    }
}

///Método de Risco Resumido 
function RiscoResmidoRESTService() {

    var lRequest = MontaRiscoResumidoRequestREST();
    
    var lUrl = $("#hddConnectionRESTRiscoResumido").val() +  'rest/BuscarRiscoResumidoJSON';

    var lRequestREST = "pRequestJson="+ JSON.stringify(lRequest);

    $.getJSON(lUrl, lRequestREST,
        function (data) {
            //document.write(data);
            RiscoResumido_REST_DataBind_Grid(data);
        });
}

function RiscoResumido_REST_DataBind_Grid(pData) {
    /*lMock_Risco_Resumido = { "page": "1", "total": 3, "records": pData.length, "rows": JSON.parse(pData) };

    $('#jqGrid_RiscoResumido').jqGrid('clearGridData');

    $('#jqGrid_RiscoResumido').jqGrid('setGridParam', { data: lMock_Risco_Resumido.rows });

    $('#jqGrid_RiscoResumido').trigger('reloadGrid');*/
    var lData = JSON.parse(pData);

    var lGrid = $('#jqGrid_RiscoResumido');

    if (gRiscoResumidoPrimeiroRequest) {
        /*
        for (i = 0; i < lData.length; i++)
        {

            RiscoResumido_REST_DataBind_Grid_Incremental(lGrid, lData[i], (i + 1));
            
            gRiscoResumidoPrimeiroRequest  =false;
        }*/

        lMock_Risco_Resumido.rows = [];

        //gRiscoResumidoListaClientes = [];

        for (i = 0; i < lData.length; i++) {

            PreencheMockRiscoResumido(lData[i]);

            gRiscoResumidoListaClientes.push(lData[i].CodigoCliente);
            //startWorkerGrid(lGrid, lData);
        }

        //$('#jqGrid_RiscoResumido').trigger('reloadGrid');
        lGrid
            .jqGrid('setGridParam',
            {
                datatype: 'local',
                data: lMock_Risco_Resumido.rows
            })
            .trigger("reloadGrid");

        gRiscoResumidoPrimeiroRequest = false;

    } else {
        lMock_Risco_Resumido.rows = [];

        for (i = 0; i < lData.length; i++) {

            PreencheMockRiscoResumido(lData[i]);
            //startWorkerGrid(lGrid, lData);
        }

        lGrid
           .jqGrid('setGridParam',
           {
               datatype: 'local',
               data: lMock_Risco_Resumido.rows
           })
           .trigger("reloadGrid");

    }
}

///Método para inserir ou editar os dados nas 
//grids de Operações Intraday
function RiscoResumido_REST_DataBind_Grid_Incremental(pGrid, pObjeto, numeroLinha)
{
    pGrid.addRowData(numeroLinha, pObjeto);
}

///Métod que limpa as linhas selecionadas na grid para monitoramento
function LimpaLinhasSelecionadasAnteriormente() {
    $("#hddOperacoes_Intraday_Seleted_Row").val('');
}

var gWorker;

function PreencheMockRiscoResumido(pObjeto) 
{
    lMock_Risco_Resumido.rows.push({
        CodigoCliente   : pObjeto.CodigoCliente,   
        CustodiaAbertura: pObjeto.CustodiaAbertura,
        CCAbertura      : pObjeto.CCAbertura,      
        Garantias       : pObjeto.Garantias,       
        Produtos        : pObjeto.Produtos,        
        TotalAbertura   : pObjeto.TotalAbertura,   
        PLBovespa       : pObjeto.PLBovespa,       
        PLBmf           : pObjeto.PLBmf,           
        PLTotal         : pObjeto.PLTotal,         
        SFP             : pObjeto.SFP,             
        PercAtingido    : pObjeto.PercAtingido,    
        });
}

function startWorkerGrid(pGrid, pObjeto)
{
    if (typeof (Worker) !== "undefined") {
        if (typeof (gWorker) == "undefined") {

            gWorker = new Worker("/Scripts/PositionClient/06-GridWorker.js");

            //gWorker.postMessage({ "args": [pGrid, pObjeto] });
        }
    }

    //console.log('Worker não está funcionando.');

    gWorker.addEventListener('message', function (e) {
        console.log('Worker said:', e.data);
    }, false);

    //gWorker.onmessage = function (e) {
    //    console.log('Worker said:', e.data);
    //};

    //gWorker.postMessage({ "args": [pGrid, pObjeto] });
    gWorker.postMessage(pGrid);

    //gWorker.onmessage = function (event) {
    //    $('#jqGrid_RiscoResumido').jqGrid('setRowData', event.Row, pObjeto);
    //}
}

function stopWorkerGrid()
{
    gWorker.terminate();
    gWorker = undefined;
}
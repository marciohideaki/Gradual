/// <reference path="../jquery.numberformatter-1.2.4.js" />
const mainGridPrefix = "main_"
const subGridPrefix = "sub_"

function ConsultarAcompanhamento_Click()
{
    var lSentido = 0;

    //0 - Ambos
    //1 - Compra
    //2 - Venda
    if ($("#chkAcompanhamentoSentidoAmbos").is(":checked"))
    {
        lSentido = 0;
    }
    else if ($("#chkAcompanhamentoSentidoCompra").is(":checked") && $("#chkAcompanhamentoSentidoVenda").is(":checked"))
    {
        lSentido = 0;
    }
    else if ($("#chkAcompanhamentoSentidoCompra").is(":checked"))
    {
        lSentido = 1;
    }
    else if($("#chkAcompanhamentoSentidoVenda").is(":checked"))
    {
        lSentido = 2;
    }

    lExchange = "Ambos"
    // 0 - Ambos
    // 1 - BOV
    // 2 - BMF
    if ($("#chkAcompanhamentoExchangeAmbos").is(":checked"))
    {
        lExchange = "Ambos";
    }
    else if ($("#chkAcompanhamentoExchangeBOV").is(":checked") && $("#chkAcompanhamentoExchangeBMF").is(":checked"))
    {
        lExchange = "Ambos";
    }
    else if ($("#chkAcompanhamentoExchangeBOV").is(":checked"))
    {
        lExchange = "Bovespa";
    }
    else if ($("#chkAcompanhamentoExchangeBMF").is(":checked"))
    {
        lExchange = "BMF";
    }

    var lListaDeChecks = $("#divStatus input[type='checkbox']:checked");
    var lIdsChecados = "";

    lListaDeChecks.each(function () {
        lIdsChecados += $(this).attr("value") + ";";
    });

    var lDados =
        {
            CodigoCliente: $("#txtAcompanhamentoCodigoCliente").val()
            , CodigoInstrumento: $("#txtAcompanhamentoCodigoInstrumento").val()
            , HoraInicio: $("#txtAcompanhamentoHoraInicio").val()
            , HoraFim: $("#txtAcompanhamentoHoraFim").val()
            , Sentido: lSentido
            , Status: lIdsChecados
            , Exchange: lExchange
            , Nova: true
        };

    $("#jqGridAcompanhamento").jqGrid("clearGridData");

    PositionClientRisco_AcompanhamentoGrid(lDados);
    return false;
}

function GridAcompanhamento_Complete() {
    var lSentido = 0;

    //0 - Ambos
    //1 - Compra
    //2 - Venda
    if ($("#chkAcompanhamentoSentidoAmbos").is(":checked")) {
        lSentido = 0;
    }
    else if ($("#chkAcompanhamentoSentidoCompra").is(":checked") && $("#chkAcompanhamentoSentidoVenda").is(":checked")) {
        lSentido = 0;
    }
    else if ($("#chkAcompanhamentoSentidoCompra").is(":checked")) {
        lSentido = 1;
    }
    else if ($("#chkAcompanhamentoSentidoVenda").is(":checked")) {
        lSentido = 2;
    }

    lExchange = "Ambos"
    // 0 - Ambos
    // 1 - BOV
    // 2 - BMF
    if ($("#chkAcompanhamentoExchangeAmbos").is(":checked")) {
        lExchange = "Ambos";
    }
    else if ($("#chkAcompanhamentoExchangeBOV").is(":checked") && $("#chkAcompanhamentoExchangeBMF").is(":checked")) {
        lExchange = "Ambos";
    }
    else if ($("#chkAcompanhamentoExchangeBOV").is(":checked")) {
        lExchange = "Bovespa";
    }
    else if ($("#chkAcompanhamentoExchangeBMF").is(":checked")) {
        lExchange = "BMF";
    }

    var lListaDeChecks = $("#divStatus input[type='checkbox']:checked");
    var lIdsChecados = "";

    lListaDeChecks.each(function () {
        lIdsChecados += $(this).attr("value") + ";";
    });


    var lDados =
        {
            CodigoCliente: $("#txtAcompanhamentoCodigoCliente").val()
            , CodigoInstrumento: $("#txtAcompanhamentoCodigoInstrumento").val()
            , HoraInicio: $("#txtAcompanhamentoHoraInicio").val()
            , HoraFim: $("#txtAcompanhamentoHoraFim").val()
            , Sentido: lSentido
            , Status: lIdsChecados
            , Exchange: lExchange
            , Nova: false
        };

    $("#jqGridAcompanhamento")
        .setGridParam({ postData: lDados })

    var cm = $("#jqGridAcompanhamento").jqGrid('getGridParam', 'colModel');

    $('.jqgrow').mouseover(function (e)
    {
        var $td = $(e.target).closest('td'), $tr = $td.closest('tr.jqgrow'), rowId = $tr.attr('id'), ci;
        if (rowId)
        {
            var rowData = $("#jqGridAcompanhamento").jqGrid('getRowData',rowId);
            var lRow = $("#jqGridAcompanhamento tr[id=" + rowId + "]");
            lRow.removeClass(rowData.Status.toUpperCase());
        }
    });

    $('.jqgrow').mouseleave(function (e)
    {
        var $td = $(e.target).closest('td'), $tr = $td.closest('tr.jqgrow'), rowId = $tr.attr('id'), ci;

        if (rowId)
        {
            var rowData = $("#jqGridAcompanhamento").jqGrid('getRowData', rowId);
            var lRow = $("#jqGridAcompanhamento tr[id=" + rowId + "]");
            lRow.addClass(rowData.Status.toUpperCase());
        }
    });

    var gridCaption = "Acompanhamento - " + $("#jqGridAcompanhamento").getGridParam("records") + " ordem(s)";
    $("#jqGridAcompanhamento").jqGrid("setCaption", gridCaption);
}

gFirstExecutionBuscarAcompanhamento = true;

function PositionClientRisco_AcompanhamentoGrid(pParametros) {
    if (gFirstExecutionBuscarAcompanhamento != true) {
        $("#jqGridAcompanhamento").setGridParam({ postData: pParametros }).trigger("reloadGrid");

        return;
    }

    gFirstExecutionBuscarAcompanhamento = false;

    $("#jqGridAcompanhamento").jqGrid(
    {
        url                 : "Backend/Acompanhamento.aspx?Acao=BuscarAcompanhamentoPaginado"
      , datatype            : "json"
      , mtype               : "GET"
      , hoverrows           : true
      , postData            : pParametros
      , autowidth           : false
      , idPrefix            : mainGridPrefix
      , colModel            :
        [
              { label: "Id"             , name: "Id"                    , jsonmap: "Id"                 , index: "Id"                   , width: 65, align: "center", sortable: true }
            , { label: "Código"         , name: "CodigoOrdem"           , jsonmap: "CodigoOrdem"        , index: "CodigoOrdem"          , width: 65, align: "center", sortable: true }
            , { label: "Cliente"        , name: "Cliente"               , jsonmap: "Cliente"            , index: "Cliente"              , width: 65, align: "center", sortable: true, sorttype: "integer" }
            , { label: "Exchange"       , name: "Exchange"              , jsonmap: "Exchange"           , index: "Exchange"             , width: 65, align: "center", sortable: true }
            , { label: "Ativo"          , name: "Ativo"                 , jsonmap: "Ativo"              , index: "Ativo"                , width: 55, align: "center", sortable: true }
            , { label: "Sentido"        , name: "Sentido"               , jsonmap: "Sentido"            , index: "Sentido"              , width: 55, align: "center", sortable: true }
            , { label: "Status"         , name: "Status"                , jsonmap: "Status"             , index: "Status"               , width: 75, align: "center", sortable: true }
            , { label: "Qtde.Ordem"     , name: "QuantidadeOrdem"       , jsonmap: "QuantidadeOrdem"    , index: "QuantidadeOrdem"      , width: 65, align: "center", sortable: true , formatter:"number"   , formatoptions:{decimalSeparator:",", thousandsSeparator: ".", decimalPlaces: 0 }}
            , { label: "Qtde.Executada" , name: "QuantidadeExecutada"   , jsonmap: "QuantidadeExecutada", index: "QuantidadeExecutada"  , width: 65, align: "center", sortable: true , formatter:"number"   , formatoptions:{decimalSeparator:",", thousandsSeparator: ".", decimalPlaces: 0 }}
            , { label: "Qtde.Aberto"    , name: "QuantidadeAberta"      , jsonmap: "QuantidadeAberta"   , index: "QuantidadeAberta"     , width: 65, align: "center", sortable: true , formatter:"number"   , formatoptions:{decimalSeparator:",", thousandsSeparator: ".", decimalPlaces: 0 }}
            , { label: "Preço"          , name: "Preco"                 , jsonmap: "Preco"              , index: "Preco"                , width: 60, align: "center", sortable: true , formatter:"currency" , formatoptions:{decimalSeparator:",", thousandsSeparator: ".", decimalPlaces: 2, prefix: "R$ "}}
            , { label: "Horário"        , name: "Horario"               , jsonmap: "Horario"            , index: "Horario"              , width: 90, align: "center", sortable: true }
            , { label: "Preço Stop"     , name: "PrecoStop"             , jsonmap: "PrecoStop"          , index: "PrecoStop"            , width: 60, align: "center", sortable: true , formatter:"currency" , formatoptions:{decimalSeparator:",", thousandsSeparator: ".", decimalPlaces: 2, prefix: "R$ "}}
            , { label: "TipoOrdem"      , name: "TipoOrdem"             , jsonmap: "TipoOrdem"          , index: "TipoOrdem"            , width: 75, align: "center", sortable: true }
            , { label: "Dt. Validade"   , name: "DataValidade"          , jsonmap: "DataValidade"       , index: "DataValidade"         , width: 75, align: "center", sortable: true }
            , { label: "ExecBroker"     , name: "ExecBroker"            , jsonmap: "ExecBroker"         , index: "ExecBroker"           , width: 75, align: "center", sortable: true }
        ]
      , height              : "auto"
      , width               : "auto"
      , rowNum              : 0
      , sortname            : "Cliente"
      , sortorder           : "desc"
      , viewrecords         : true
      , loadonce            : false
      , autoencode          : false
      , gridview            : false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader          :
        {
            root            : "Itens"
            , page          : "PaginaAtual"
            , total         : "TotalDePaginas"
            , records       : "TotalDeItens"
            , cell          : ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id            : "0"              //primeira propriedade do elemento de linha é o Id
            , repeatitems   : false
        }
      , subGrid             : true
      , multiselect         : false
      , pager               : '#jqGridAcompanhamentoPager'
      , rowNum              : 200
      , rowList             : [100, 200, 300]
      , caption             : "Acompanhamento"
      , sortable            : true
      , emptyrecords        : "Nenhum item encontrado!"
      , subGridRowExpanded: AcompanhamentoSubgridExpander
      , afterInsertRow: SetColorAcompanhamentoInsertRow
      , gridComplete        : GridAcompanhamento_Complete
    }).jqGrid("hideCol", "Id");
}

function SetColorAcompanhamentoInsertRow(rowid, pData, rowelem)
{
    var lRow = $("#jqGridAcompanhamento tr[id=" + rowid + "]");

    if (rowelem.Status.indexOf("ENVIADA") > -1)
    {
        lRow.addClass("SOLICITADA");
    }
    else
    {
        lRow.addClass(rowelem.Status.toUpperCase());
    }
}

function AcompanhamentoSubgridExpander(subgrid_id, row_id)
{
    var lCodigoOrdem = $("#jqGridAcompanhamento").find("#" + row_id).find("td").eq(2).html();
    var subgrid_table_id;

    subgrid_table_id = subgrid_id + "_t";
    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
    jQuery("#" + subgrid_table_id).jqGrid(
    {
        url                 : "Backend/Acompanhamento.aspx?Acao=BuscarDetalhe"
        , hoverrows         : false
        , datatype          : "json"
        , mtype             : "GET"
        , postData          : { CodigoOrdem: lCodigoOrdem }
        , shrinkToFit       : false
        , autowidth         : true
        , idPrefix          : subGridPrefix
        , colModel          :
          [
              { label: "Id"         , name: "Id"                , jsonmap: "Id"             , index: "Id"           , width: 65     , align: "center", sortable: true }
            , { label: "Dt. Hora"   , name: "TransactTime"      , jsonmap: "TransactTime"   , index: "TransactTime" , width: 120    , align: "center", sortable: true }
            , { label: "Status"     , name: "Status"            , jsonmap: "Status"         , index: "Status"       , width: 180    , align: "center", sortable: true }
            , { label: "Preço"      , name: "Price"             , jsonmap: "Price"          , index: "Price"        , width: 85     , align: "center", sortable: true }
            , { label: "Quantidade" , name: "OrderQty"          , jsonmap: "OrderQty"       , index: "OrderQty"     , width: 50     , align: "center", sortable: true }
            , { label: "Qtde.Exec"  , name: "CumQty"            , jsonmap: "CumQty"         , index: "CumQty"       , width: 50     , align: "center", sortable: true }
            , { label: "Saldo"      , name: "Saldo"             , jsonmap: "Saldo"          , index: "Saldo"        , width: 50     , align: "center", sortable: true }
            , { label: "Descrição"  , name: "Description"       , jsonmap: "Description"    , index: "Description"  , width: 250    , align: "center", sortable: true }
            //, { label: "OrderDetailID"      , name: "OrderDetailID"         , jsonmap: "OrderDetailID"    , index: "OrderDetailID"    , width:60  , align: "center"   , sortable: true }
            //, { label: "TransactID"         , name: "TransactID"            , jsonmap: "TransactID"       , index: "TransactID"       , width:60  , align: "center"   , sortable: true }
            //, { label: "TradeQty"           , name: "TradeQty"              , jsonmap: "TradeQty"         , index: "TradeQty"         , width:60  , align: "center"   , sortable: true }
            //, { label: "FixMsgSeqNum"       , name: "FixMsgSeqNum"          , jsonmap: "FixMsgSeqNum"     , index: "FixMsgSeqNum"     , width:60  , align: "center"   , sortable: true }
            //, { label: "CxlRejResponseTo"   , name: "CxlRejResponseTo"      , jsonmap: "CxlRejResponseTo" , index: "CxlRejResponseTo" , width:60  , align: "center"   , sortable: true }
            //, { label: "CxlRejReason"       , name: "CxlRejReason"          , jsonmap: "CxlRejReason"     , index: "CxlRejReason"     , width:60  , align: "center"   , sortable: true }
            //, { label: "MsgFixDetail"       , name: "MsgFixDetail"          , jsonmap: "MsgFixDetail"     , index: "MsgFixDetail"     , width:60  , align: "center"   , sortable: true }
            //, { label: "Contrabroker"       , name: "Contrabroker"          , jsonmap: "Contrabroker"     , index: "Contrabroker"     , width:60  , align: "center"   , sortable: true }
            //, { label: "OrderID"            , name: "OrderID"               , jsonmap: "OrderID"          , index: "OrderID"            , width:60   , align: "center"   , sortable: true }
            //, { label: "OrdQtyRemaining"    , name: "OrdQtyRemaining"       , jsonmap: "OrdQtyRemaining"  , index: "OrdQtyRemaining"    , width:60   , align: "center"   , sortable: true }
            //, { label: "StopPx"             , name: "StopPx"                , jsonmap: "StopPx"           , index: "StopPx"             , width:60   , align: "center"   , sortable: true }
            //, { label: "OrderStatusID"      , name: "OrderStatusID"         , jsonmap: "OrderStatusID"    , index: "OrderStatusID"      , width:60   , align: "center"   , sortable: true }
            //, { label: "Description"        , name: "Description"           , jsonmap: "Description"      , index: "Description"        , width:60   , align: "center"   , sortable: true }
        ]
        , viewrecords: true
        , jsonReader: {
            root            : "Itens"
            , cell          : ""  //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id            : "0" //primeira propriedade do elemento de linha é o Id
            , repeatitems   : false
        }
        , width             : 725
        , height            : 'auto'
        , rowNum            : 'auto'
        , sortname          : 'invid'
        , sortorder         : 'asc'
        , sortable          : true
        , subgrid           : false
        , emptyrecords      : "Nenhum item encontrado!"
    }).jqGrid("hideCol", "Id").jqGrid("hideCol", "OrderDetailID");
}

function AcompanhamentoCollapseSubGridRow(rowid, selected)
{
    var rowIds = $("#jqGridAcompanhamento").getDataIDs();
    $.each(rowIds, function (index, rowId) {
        $("#jqGridAcompanhamento").collapseSubGridRow(rowId);
    });
}

function chkSentido_Click(pSender)
{
    if (pSender.id == "chkAcompanhamentoSentidoAmbos")
    {
        var lListaDeChecks = $("#divSentido input[type='checkbox']");
        lListaDeChecks.each(function ()
        {
            if ($(this).attr("id") != "chkAcompanhamentoSentidoAmbos")
            {
                $(this).prop("checked", false);
            }
        });
    }
    else if(pSender.id != "chkAcompanhamentoSentidoAmbos")
    {
        $("#chkAcompanhamentoSentidoAmbos").prop("checked", false);
    }
    
}

function chkExchange_Click(pSender)
{
    if (pSender.id == "chkAcompanhamentoExchangeAmbos")
    {
        var lListaDeChecks = $("#divExchange input[type='checkbox']");
        lListaDeChecks.each(function ()
        {
            if ($(this).attr("id") != "chkAcompanhamentoExchangeAmbos")
            {
                $(this).prop("checked", false);
            }
        });
    }
    else if (pSender.id != "chkAcompanhamentoExchangeAmbos")
    {
        $("#chkAcompanhamentoExchangeAmbos").prop("checked", false);
    }
}

function PositionClientRisco_Acompanhamento_RemoveFilterCliente()
{
    // Limpa filtros de cliente e hora
    $("#txtAcompanhamentoCodigoCliente").val("");
    $("#txtAcompanhamentoCodigoInstrumento").val("");
    $("#txtAcompanhamentoHoraInicio").val("08:00");
    $("#txtAcompanhamentoHoraFim").val("18:00");
    return false;
}

function PositionClientRisco_Acompanhamento_RemoveFilterSentido()
{
    // Limpa filtros de sentido
    $("#chkAcompanhamentoSentidoAmbos").prop('checked', false);
    $("#chkAcompanhamentoSentidoCompra").prop('checked', false);
    $("#chkAcompanhamentoSentidoVenda").prop('checked', false);
    return false;
}

function PositionClientRisco_Acompanhamento_RemoveFilterExchange()
{
    // Limpa filtros de bolsa
    $("#chkAcompanhamentoExchangeAmbos").prop('checked', false);
    $("#chkAcompanhamentoExchangeBOV").prop('checked', false);
    $("#chkAcompanhamentoExchangeBMF").prop('checked', false);

    return false;
}

function PositionClientRisco_Acompanhamento_RemoveFilterStatus()
{
    // Limpa filtros de status
    $("#chkAcompanhamentoStatusExecutada").prop('checked', false);
    $("#chkAcompanhamentoStatusParcialmenteExecutada").prop('checked', false);
    $("#chkAcompanhamentoStatusSubstituida").prop('checked', false);
    $("#chkAcompanhamentoStatusAberta").prop('checked', false);
    $("#chkAcompanhamentoStatusCancelada").prop('checked', false);
    $("#chkAcompanhamentoStatusExpirada").prop('checked', false);
    $("#chkAcompanhamentoStatusRejeitada").prop('checked', false);
    $("#chkAcompanhamentoStatusWaiting").prop('checked', false);
    $("#chkAcompanhamentoStatusSuspensa").prop('checked', false);
    $("#chkAcompanhamentoStatusSolicitada").prop('checked', false);
    return false;
}

function Positionclientrisco_Acompanhamento_RemoveFilterExchange()
{
    // Limpa filtros de sentido
    $("#chkAcompanhamentoOrigemHomeBroker").prop('checked', false);
    $("#chkAcompanhamentoOrigemGLTrade").prop('checked', false);
    $("#chkAcompanhamentoOrigemProfitChart").prop('checked', false);
    $("#chkAcompanhamentoOrigemGTI").prop('checked', false);
    $("#chkAcompanhamentoOrigemTryd").prop('checked', false);
    $("#chkAcompanhamentoOrigemAgenciaEstado").prop('checked', false);
    return false;
}

function 
    (pSender)
{
    // Not implemented
}

function RemoverTodosFiltrosAcompanhamento_Click()
{
    PositionClientRisco_Acompanhamento_RemoveFilterCliente();
    PositionClientRisco_Acompanhamento_RemoveFilterSentido();
    Positionclientrisco_Acompanhamento_RemoveFilterExchange();
    PositionClientRisco_Acompanhamento_RemoveFilterStatus();
    return false;
}
var lColorRed       = "#FF3A44";
var lColorYelow     = "#F9F9BA";
var lColorGreen     = "#C8D7C1";
var lColorFont      = "#2e7db2";
var lColorFontWhite = "#FFFFFF";

function ConsultarVendasDescobertas_Click()
{
    var lListaDeChecks = $("#divVendasDescobertasMercado input[type='checkbox']:checked");
    var lMercadosChecados = "";

    lListaDeChecks.each(function ()
    {
        lMercadosChecados += $(this).attr("value") + ";";
    });

    var lDados =
        {
            CodigoCliente: $("#txtVendasDescobertasCodigoCliente").val()
            , CodigoInstrumento: $("#txtVendasDescobertasCodigoInstrumento").val()
            , Mercados: lMercadosChecados
            , Nova: true
        };

    $("#jqGridVendasDescobertas").jqGrid("clearGridData");

    PositionClientRisco_VendasDescobertasGrid(lDados);

    return false;
}

function GridVendasDescobertas_Complete()
{
    var lListaDeChecks = $("#divVendasDescobertasMercado input[type='checkbox']:checked");
    var lMercadosChecados = "";

    lListaDeChecks.each(function ()
    {
        lMercadosChecados += $(this).attr("value") + ";";
    });

    var lDados =
        {
            CodigoCliente: $("#txtVendasDescobertasCodigoCliente").val()
            , CodigoInstrumento: $("#txtVendasDescobertasCodigoInstrumento").val()
            , Mercados: lMercadosChecados
            , Nova: false
        };

    $("#jqGridVendasDescobertas")
        .setGridParam({ postData: lDados })

    var cm = $("#jqGridVendasDescobertas").jqGrid("getGridParam", "colModel");

    // Pinta ao receber o foco / hover (remove o estilo customizado)
    $('.jqgrow').mouseover(function (e)
    {
        var $td = $(e.target).closest('td'), $tr = $td.closest('tr.jqgrow'), rowId = $tr.attr('id'), ci;
        if (rowId)
        {
                var rowData = $("#jqGridVendasDescobertas").jqGrid('getRowData', rowId);

                $("#jqGridVendasDescobertas").jqGrid("setCell", rowId, "LucroPrej", "", { "background-color": "", color: "" });
        }
    });

    // Pinta a linha ao perder o foco / hover (adiciona o estilo customizado)
    $('.jqgrow').mouseleave(function (e)
    {
        var $td = $(e.target).closest('td'), $tr = $td.closest('tr.jqgrow'), rowId = $tr.attr('id'), ci;

        if (rowId)
        {
            var rowData = $("#jqGridVendasDescobertas").jqGrid("getRowData", rowId);

            var lGrid = $("#jqGridVendasDescobertas");

            if (rowData.LucroPrej < 0)
            {
                lGrid.jqGrid("setCell", rowId, "LucroPrej", "", { "background-color": lColorRed, "color": lColorFontWhite });
            }
            else if (rowData.LucroPrej == 0)
            {
                lGrid.jqGrid("setCell", rowId, "LucroPrej", "", { "background-color": lColorYelow, "color": lColorFont });
            }
            else
            {
                lGrid.jqGrid("setCell", rowId, "LucroPrej", "", { "background-color": lColorGreen, color: lColorFont });
            }
        }
    });
    
    // Adiciona o total de registros no caption da grid
    var gridCaption = "Vendas Descobertas - " + $("#jqGridVendasDescobertas").getGridParam("records") + " registro(s)";
    $("#jqGridVendasDescobertas").jqGrid("setCaption", gridCaption);
}

gFirstExecutionBuscarVendasDescobertas = true;

function PositionClientRisco_VendasDescobertasGrid(pParametros)
{

    if (gFirstExecutionBuscarVendasDescobertas != true)
    {
        $("#jqGridVendasDescobertas").setGridParam({ postData: pParametros }).trigger("reloadGrid");

        return;
    }

    gFirstExecutionBuscarVendasDescobertas = false;


    $("#jqGridVendasDescobertas").jqGrid(
    {
        url                 : "backend/VendasDescobertas.aspx?Acao=BuscarVendasDescobertasPaginado"
      , datatype            : "json"
      , mtype               : "GET"
      , hoverrows           : true
      , postData            : pParametros
      , autowidth           : false
      , colModel            :
        [
              { label: "Id"             , name: "Id"            , jsonmap: "Id"             , index: "Id"               , width: 65 , align: "center"   , sortable: true }
            , { label: "Cliente"        , name: "Account"       , jsonmap: "Account"        , index: "Account"          , width: 65 , align: "center"   , sortable: true }
            , { label: "Ativo"          , name: "Ativo"         , jsonmap: "Ativo"          , index: "Ativo"            , width: 55 , align: "center"   , sortable: true }
            , { label: "Qtde. Abert."   , name: "QtdDisponivel" , jsonmap: "QtdDisponivel"  , index: "QtdDisponivel"    , width: 75 , align: "center"   , sortable: true, formatter: "number", formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0 } }
            , { label: "Net Exec."      , name: "NetExec"       , jsonmap: "NetExec"        , index: "NetExec"          , width: 75 , align: "center"   , sortable: true, formatter: "number", formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0 } }
            , { label: "Qtde. Total"    , name: "QtdTotal"      , jsonmap: "QtdTotal"       , index: "QtdTotal"         , width: 75 , align: "center"   , sortable: true, formatter: "number", formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 0 } }
            , { label: "L&P"            , name: "LucroPrej"     , jsonmap: "LucroPrej"      , index: "LucroPrej"        , width: 75 , align: "center"   , sortable: true , formatter:'currency', formatoptions:{decimalSeparator:",", thousandsSeparator: ".", decimalPlaces: 2, prefix: "R$ "}}
        ]
      , height              : "auto"
      , width               : 450
      , rowNum              : 0
      , sortname            : "Cliente"
      , sortorder           : "desc"
      , viewrecords         : true
      , gridview            : false //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader:
        {
            root            : "Itens"
            , page          : "PaginaAtual"
            , total         : "TotalDePaginas"
            , records       : "TotalDeItens"
            , cell          : ""   //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id            : "0"  //primeira propriedade do elemento de linha é o Id
            , repeatitems   : false
        }
      , multiselect         : false
      , pager               : "#jqGridVendasDescobertas"
      , rowNum              : 200
      , rowList             : [100, 200, 300]
      , caption             : "Vendas Descobertas"
      , sortable            : true
      , emptyrecords        : "Nenhum item encontrado!"
      , gridComplete        : GridVendasDescobertas_Complete
      , afterInsertRow      : PositionClientRisco_VendasDescobertas_AfterInsertRow
    }).jqGrid("hideCol", "Id");
}

function chkMercado_Click(pSender)
{
    if (pSender.id == "chkMercadoTodos")
    {
        var lListaDeChecks = $("#divVendasDescobertasMercado input[type='checkbox']");
        lListaDeChecks.each(function ()
        {
            if ($(this).attr("id") != "chkMercadoTodos")
            {
                $(this).prop("checked", false);
            }
        });
    }
    else if (pSender.id != "chkMercadoTodos")
    {
        $("#chkMercadoTodos").prop("checked", false);
    }
}

function PositionClientRisco_VendasDescobertas_AfterInsertRow(rowid, pData)
{
    var lGrid = $("#jqGridVendasDescobertas");

    if (pData.LucroPrej.toPrecision() < 0)
    {
        lGrid.jqGrid("setCell", rowid, "LucroPrej", "", { "background-color": lColorRed, "color": lColorFontWhite });
    }
    else if (pData.LucroPrej.toPrecision() == 0)
    {
        lGrid.jqGrid("setCell", rowid, "LucroPrej", "", { "background-color": lColorYelow, "color": lColorFont });
    }
    else
    {
        lGrid.jqGrid("setCell", rowid, "LucroPrej", "", { "background-color": lColorGreen, color: lColorFont });
    }
}

var estiloAntigo;
var linhaOver;
var linhaLeave;

function PositionClientRisco_HighLight()
{
    var lGrid = $("#jqGridVendasDescobertas");
    estiloAntigo = lGrid.css("style");
}

function PositionClientRisco_VendasDescobertas_RemoveFilterCliente()
{
    // Limpa filtros de cliente e ativo
    $("#txtVendasDescobertasCodigoCliente").val("");
    $("#txtVendasDescobertasCodigoInstrumento").val("");
    return false;
}

function PositionClientRisco_VendasDescobertas_RemoveFilterMercado()
{
    // Limpa filtros de mercado
    $("#chkMercadoTodos").prop('checked', false);
    $("#chkMercadoVista").prop('checked', false);
    $("#chkMercadoFuturo").prop('checked', false);
    $("#chkMercadoOpcoes").prop('checked', false);

    return false;
}

function RemoverTodosFiltrosVendasDescobertas_Click()
{
    PositionClientRisco_VendasDescobertas_RemoveFilterMercado();
    PositionClientRisco_VendasDescobertas_RemoveFilterCliente();

    return false;
}
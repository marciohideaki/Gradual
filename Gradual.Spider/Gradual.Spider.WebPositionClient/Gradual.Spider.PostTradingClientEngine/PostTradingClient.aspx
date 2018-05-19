<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostTradingClient.aspx.cs" Inherits="Gradual.Spider.PostTradingClientEngine.PostTradingClient" %>
<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    
    
    <%--Includes Css--%>
    <link href="Skins/css/bootstrap.css"                        rel="stylesheet" />
    <link href="Skins/css/Principal.css"                        rel="stylesheet" />
    <link href="Skins/ui-darkness/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <link href="Skins/css/custom.css" rel="stylesheet" />
    <link href="/Scripts/jqGrid/src/css/ui.jqgrid.css"          rel="stylesheet" />
    

    <%--Includes Javascript--%>
    
    <script type="text/javascript" src="Scripts/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="/Scripts/jqGrid/src/i18n/grid.locale-pt-br.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.9.2.custom/js/jquery-ui-1.9.2.custom.js"></script>
    <%--<script type="text/javascript" src="/Scripts/jqGrid/src/grid.base.js"></script>--%>
    <script type="text/javascript" src="/Scripts/jqGrid/src/jquery.jqGrid.js"></script>

    <script type="text/javascript" src="/Scripts/PositionClient.js"                 ></script>
    <%--<script type="text/javascript" src="/Scripts/jqGrid/src/grid.subgrid.js"></script>--%>

    <script type="text/javascript" src="Scripts/jqGridExportToExcel.js"></script>
    <script type="text/javascript" src="Scripts/jquery.maskedinput-1.2.2.js"></script>
    
    <title></title>
</head>
<body>
    <input type="hidden" runat="server" id="hddConnectionSocket" />
    <div class="row">
        <div class="col-md-11">
            <div class="row cabecalho-cliente">
            <div class="col-md-11 titulo">
                <label>Daily Activity</label>
                <span class="lblStatusConnection"><label id="lblStatusConnection"></label></span>
            </div>
            </div>
            <div id="nav-tabs" style="font-size:11px;">
                <ul  style="margin: 0px 12px 0px 12px;">
                    <li id="liGeneralView"  runat="server" >  <a href="#pnlGeneralView"     >General View</a></li>
                    <li id="liPerAssetClass" runat="server">  <a href="#pnlPerAssetClass"   >Per Assets Class</a></li>
                    <li id="liBuysSells"    runat="server">   <a href="#pnlBuysSells"       >Buys and Sells</a></li>
                    <li id="liTradebyTrade" runat="server">   <a href="#pnlTradebyTrade"    >Trade by Trade</a></li>
                </ul>
                <div id="pnlGeneralView" runat="server">
                    <div class="FilterClass">
                    Search for: <input type="text" id="txtDaily_Activity_GeneralView_SearchFor" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtDaily_Activity_SearchFor_KeyDown(this)" />
                    <div style="float:right; margin-right:70px">
                        Execution time from <input type="text" id="txtDaily_Activity_GeneralView_From" class=" ui-widget ui-widget-content ui-corner-all ExecutionFrom Mascara_Hora" onkeydown="return txtDaily_Activity_From_KeyDown(this)" value="08:00:00:000" /> to 
                        <input type="text" id="txtDaily_Activity_GeneralView_To" class="ui-widget ui-widget-content ui-corner-all ExecutionTo Mascara_Hora" onkeydown="return txtDaily_Activity_To_KeyDown(this)" value="23:59:59.999" />
                    </div>
                    </div>
                    <table id="jqGrid_Daily_Activity_GeneralView"><tr><td/></tr></table>
                    <div id="jqGrid_Daily_Activity_GeneralView_Pager"></div>
                </div>
                <div id="pnlPerAssetClass" runat="server" >
                    <div class="FilterClass">
                    Search for: <input type="text" id="txtDaily_Activity_PerAssetClass_SearchFor" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtDaily_Activity_SearchFor_KeyDown(this)"  />
                    <div style="float:right; margin-right:70px">
                    Execution time from <input type="text" id="txtDaily_Activity_PerAssetClass_From" class=" ui-widget ui-widget-content ui-corner-all ExecutionFrom Mascara_Hora"  onkeydown="return txtDaily_Activity_From_KeyDown(this)" value="08:00:00:000" /> to 
                    <input type="text" id="txtDaily_Activity_PerAssetClass_To" class="ui-widget ui-widget-content ui-corner-all ExecutionTo Mascara_Hora" onkeydown="return txtDaily_Activity_To_KeyDown(this)" value="23:59:59.999" />
                    </div>
                    </div>
                    <table id="jqGrid_Daily_Activity_PerAssetClass_Equities"></table>
                    <div id="jqGrid_Daily_Activity_PerAssetClass_Equities_Pager"></div>
                    <table id="jqGrid_Daily_Activity_PerAssetClass_Futures"></table>
                    <div id="jqGrid_Daily_Activity_PerAssetClass_Futures_Pager"></div>
                </div>
                <div id="pnlBuysSells" runat="server" >
                    <div class="FilterClass">
                    Search for: <input type="text" id="txtDaily_Activity_BuysSells_SearchFor" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtDaily_Activity_SearchFor_KeyDown(this)" />
                    <div style="float:right; margin-right:70px">
                    Execution time from <input type="text" id="txtDaily_Activity_BuysSells_From" class=" ui-widget ui-widget-content ui-corner-all ExecutionFrom Mascara_Hora"  onkeydown="return txtDaily_Activity_From_KeyDown(this)" value="08:00:00:000" /> to 
                    <input type="text" id="txtDaily_Activity_BuysSells_To" class="ui-widget ui-widget-content ui-corner-all ExecutionTo Mascara_Hora" onkeydown="return txtDaily_Activity_To_KeyDown(this)"  value="23:59:59.999"/>
                    </div>
                    </div>
                    <table id="jqGrid_Daily_Activity_BuysSells_Buy"></table>
                    <div id="jqGrid_Daily_Activity_BuysSells_Buy_Pager"></div>
                    <table id="jqGrid_Daily_Activity_BuysSells_Sell"></table>
                    <div id="jqGrid_Daily_Activity_BuysSells_Sell_Pager"></div>
                   
                </div>
                <div id="pnlTradebyTrade" runat="server" >
                    <div class="FilterClass">
                    Search for: <input type="text" id="txtDaily_Activity_TradebyTrade_SearchFor" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtDaily_Activity_SearchFor_KeyDown(this)"  />
                    <div style="float:right; margin-right:70px">
                    Execution time from 
                        <input type="text" id="txtDaily_Activity_TradebyTrade_FromDate" class="ui-widget ui-widget-content ui-corner-all ExecutionFromDate datepicker"  />
                        <input type="text" id="txtDaily_Activity_TradebyTrade_From" class=" ui-widget ui-widget-content ui-corner-all ExecutionFrom Mascara_Hora"  onkeydown="return txtDaily_Activity_From_KeyDown(this)" value="08:00:00:000" /> 
                        to 
                        <input type="text" id="txtDaily_Activity_TradebyTrade_ToDate" class="ui-widget ui-widget-content ui-corner-all ExecutionFromDate datepicker"  />
                        <input type="text" id="txtDaily_Activity_TradebyTrade_To" class="ui-widget ui-widget-content ui-corner-all ExecutionTo Mascara_Hora" onkeydown="return txtDaily_Activity_To_KeyDown(this)" value="23:59:59.999" />
                    </div>
                    </div>
                    <table id="jqGrid_Daily_Activity_TradebyTrade"></table>
                    <div id="jqGrid_Daily_Activity_TradebyTrade_Pager"></div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
<script language="javascript">
    $(function() {
        $( "#nav-tabs" ).tabs();
    });
    /*
    $("input.datepicker").datepicker({
        dateFormat: "dd/mm/yy"
            , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
            , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
            , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
    });
    */
    function Page_Load_CodeBehind()
    {
        <asp:literal id="litJavascriptOnLoad" runat="server"></asp:literal>
    }

    $(document).ready(function (){
        Page_Load();
        PostTradingClient_Daily_Activity_PerAssetClass_Equities();
    });
</script>
<style type="text/css" media="screen">
    th.ui-th-column div{
        white-space:normal !important;
        height:auto !important;
        padding:2px;
    }
</style>
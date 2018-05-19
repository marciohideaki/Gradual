<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="MsgFix.aspx.cs" Inherits="Gradual.Spider.Www.MsgFix" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
    MsgFix
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">
    
<div id="page-content" class="telaMsgFix">
    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon  icon-list-alt"></i>
            Ordens
        </span>
    </div>

    <h3 class="mainTitle page-title">Message Fix</h3>

    <div class="content-box box-cinza-topo box-topo-ordens">
        <div class="content-box-header content-box-header-alt bg-default">
            <div class="form-inline">
                <ul>
                    <li><label>Conta</label> <br /><input type="text" name="conta" class="form-control form-element-sm"/></li>
                    <li><label>Symbol</label><br /><input type="text" name="Símbolo" class="form-control form-element-sm" /></li>
                    <li class="data">
                        <label>Data inicial
                        </label><br />
                        <input type="text" name="dtInicial" class="form-control form-element-sm " id="dtInicial" />
                        <i class="glyph-icon icon-calendar datepicker"></i>
                    </li>
                    <li class="data">
                        <label>Data final</label><br />
                            <input type="text" name="dtFinal" class="form-control form-element-sm" id="dtFinal"/>
                            <i class="glyph-icon icon-calendar datepicker"></i>
                        
                    </li>
                    <li><label>Sessão</label>
                        <select name="sessao" id="Select1" class="form-control form-element-sm ">
                            <option value="1">item 1</option>
                            <option value="2">item 2</option>
                            <option value="3">item 3</option>
                        </select>
                    </li>
                    <li><label>Sentido</label><br />
                        <select name="sentido" id="Select2" class="form-control form-element-sm ">
                            <option value="1">Compra</option>
                            <option value="2">Venda</option>
                        </select>
                    </li>
                    <li><label>Bolsa</label><br />
                        <select name="bolsa" id="Select3" class="form-control form-element-sm ">
                            <option value="1">Bovespa</option>
                        </select>
                    </li>
                    <li class="botao"><input type="button" value="Consultar" class="btn btn-sm btn-primary"/></li>
                    
                </ul>
            </div>
        </div>
    </div>

    
    <div class="row">
        <div class="col-md-12">
            <div class="content-box tabs ui-tabs ui-widget ui-widget-content ui-corner-all">
                    
                <div class="bg-primary">
                    <ul id="tabs" class="nav nav-tabs ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
                        <li class="ui-state-default ui-corner-top active"><a href="#todas" data-toggle="tab">Todas</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#executadas" data-toggle="tab">Executadas</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#abertas" data-toggle="tab">Abertas</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#canceladas" data-toggle="tab">Canceladas</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#rejeitadas" data-toggle="tab">Rejeitadas</a></li>
                    </ul>
                </div>
                <div id="tabsConteudo" class="tab-content">
                    <div class="tab-pane fade page-box active in" id="todas">
                        <div class="chkBoxAutoUpdate"><label class="label-light"><input type="checkbox" name="autoUpdate" />Atualizar automaticamente</label></div>
                        <p class="info">Foram encontradas <span><strong>15.324</strong> mensagens</span>.</p>
                        
                        <div class="copyMsg"><input type="button" value="Copiar Mensagens" class="btn btn-sm btn-default"/></div>

                        <table id="grid" class="table table-condensed table-striped"></table> 
                        <div id="paging"></div>
                        <p class="acoes"><a href="#" class="btn btn-default">Cancelar Selecionadas</a></p>

                    </div>

                    <div class="tab-pane page-box fade" id="executadas">
                        <p>Food truck fixie locavore, accusamus mcsweeney's marfa nulla single-origin coffee squid. Exercitation +1 labore velit, blog sartorial PBR leggings next level wes anderson artisan four loko farm-to-table craft beer twee. Qui photo booth letterpress, commodo enim craft beer mlkshk aliquip jean shorts ullamco ad vinyl cillum PBR. Homo nostrud organic, assumenda labore aesthetic magna delectus mollit. Keytar helvetica VHS salvia yr, vero magna velit sapiente labore stumptown. Vegan fanny pack odio cillum wes anderson 8-bit, sustainable jean shorts beard ut DIY ethical culpa terry richardson biodiesel. Art party scenester stumptown, tumblr butcher vero sint qui sapiente accusamus tattooed echo park.</p>
                    </div>

                    <div class="tab-pane page-box fade" id="abertas">
                        <p>Etsy mixtape wayfarers, ethical wes anderson tofu before they sold out mcsweeney's organic lomo retro fanny pack lo-fi farm-to-table readymade. Messenger bag gentrify pitchfork tattooed craft beer, iphone skateboard locavore carles etsy salvia banksy hoodie helvetica. DIY synth PBR banksy irony. Leggings gentrify squid 8-bit cred pitchfork. Williamsburg banh mi whatever gluten-free, carles pitchfork biodiesel fixie etsy retro mlkshk vice blog. Scenester cred you probably haven't heard of them, vinyl craft beer blog stumptown. Pitchfork sustainable tofu synth chambray yr.</p>
                    </div>

                    <div class="tab-pane page-box fade" id="canceladas">
                        <p>Trust fund seitan letterpress, keytar raw denim keffiyeh etsy art party before they sold out master cleanse gluten-free squid scenester freegan cosby sweater. Fanny pack portland seitan DIY, art party locavore wolf cliche high life echo park Austin. Cred vinyl keffiyeh DIY salvia PBR, banh mi before they sold out farm-to-table VHS viral locavore cosby sweater. Lomo wolf viral, mustache readymade thundercats keffiyeh craft beer marfa ethical. Wolf salvia freegan, sartorial keffiyeh echo park vegan.</p>
                    </div>

                    <div class="tab-pane page-box fade" id="rejeitadas">
                        <p>Trust fund seitan letterpress, keytar raw denim keffiyeh etsy art party before they sold out master cleanse gluten-free squid scenester freegan cosby sweater. Fanny pack portland seitan DIY, art party locavore wolf cliche high life echo park Austin. Cred vinyl keffiyeh DIY salvia PBR, banh mi before they sold out farm-to-table VHS viral locavore cosby sweater. Lomo wolf viral, mustache readymade thundercats keffiyeh craft beer marfa ethical. Wolf salvia freegan, sartorial keffiyeh echo park vegan.</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
        
</div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
    $(function () {

        $("#dtInicial").datepicker({
            defaultDate: "-1m",
            maxDate: "0",
            gotoCurrent: true,
            defaultDate: "-32d"
        });
        $("#dtFinal").datepicker({
            gotoCurrent: true,
            maxDate: "0",
            defaultDate: null
        });
    });
    

    //Grid aba 1 
    var mydata = {"page": "1", "total": 3, "records": "13", "rows": [
                { id: "msg1",  dataHora: "01/01/2000 12:40","symbol": "PETR4",  message: "message1"},
                { id: "msg2",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message2"},
                { id: "msg3",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message3"},
                { id: "msg4",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message4"},
                { id: "msg5",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message5"},
                { id: "msg6",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message6"},
                { id: "msg7",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message7"},
                { id: "msg8",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message8"},
                { id: "msg9",  dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message9"},
                { id: "msg10", dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message10"},
                { id: "msg11", dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message11"},
                { id: "msg12", dataHora:"01/01/2000 12:00", "symbol": "PETR4", message:"message12"},
                { id: "msg13", dataHora: "01/01/2000 12:00","symbol": "PETR4",  message: "message13" }
    ]
    };



    var subgrid = {
        msg1: [
            { id: "1", qtdeExec: 100, hora: "12:00" },
            { id: "2", qtdeExec: 200, hora: "12:00" },
            { id: "3", qtdeExec: 300, hora: "12:00" },
            { id: "4", qtdeExec: 400, hora: "12:00" },
            { id: "5", qtdeExec: 500, hora: "12:00" }
        ],
        msg2: [
            { id: "1", qtdeExec: 100, hora: "12:00" },
            { id: "2", qtdeExec: 200, hora: "12:00" },
            { id: "3", qtdeExec: 300, hora: "12:00" }
        ]
    };
        
    $("#grid").jqGrid({
        datatype: "local"           
        , data: mydata.rows
        , height: "100%"
        , jsonReader: {
            repeatitems: false
        }
        , sortable: true
        , autoencode: true
        , loadonce: true
        , viewrecords: true
        , rowNum: 5            
        , pager: "#paging"
        , colNames: ['Id','Data e hora' ,'Symbol', 'Mensagem']
        , colModel: [
            { name: 'id', index: 'id', sorttype: "int" , hidden:true}
            , { name: 'dataHora', index: 'dataHora', width: 120 }
            , { name: 'symbol', index: 'symbol', width: 100 }
            , { name: 'message', index: 'message', width: 800 }
        ]

        , subGrid: true
        , subGridRowExpanded: function (subgridDivId, rowId) {

            var subgridTableId = subgridDivId + "_t";
            $("#" + subgridDivId).html("<table id='" + subgridTableId + "'></table>");
            $("#" + subgridTableId).jqGrid({
                datatype: 'local',
                data: subgrid[rowId],
                colNames: ['', 'Executadas', 'Hora'],
                colModel: [
                    { name: 'id', hidden: true },
                    { name: 'qtdeExec', width: 100 },
                    { name: 'hora', width: 100 }
                ],
                gridview: true,
                rownumbers: true,
                autoencode: true,
                sortname: 'hora',
                sortorder: 'desc',
                height: '100%',
            });
        },
        subGridOptions: {

            plusicon: "glyph-icon icon-plus",
            minusicon: "glyph-icon icon-minus",
            openicon: ""
        }
            

    });

    </script>
</asp:Content>

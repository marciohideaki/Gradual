<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="Ordens.aspx.cs" Inherits="Gradual.Spider.Www.Ordens" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
    Ordens
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">


<div id="page-content" class="telaOrdens">
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

    <h3 class="mainTitle page-title">Ordens</h3>

    <div class="content-box box-cinza-topo box-topo-ordens">
        <div class="content-box-header content-box-header-alt bg-default">
            <div class="form-inline">
                <ul>
                    <li><label>Conta</label> <br /><input type="text" name="conta" class="form-control form-element-sm"/></li>
                    <li><label>Symbol</label><br /><input type="text" name="Símbolo" class="form-control form-element-sm" /></li>
                    <li class="data">
                        <label>Data inicial</label><br />
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
                        <li class="ui-state-default ui-corner-top"><a href="#historicas" data-toggle="tab">Históricas</a></li>
                    </ul>
                </div>
                <div id="tabsConteudo" class="tab-content">
                    <div class="tab-pane fade page-box active in" id="todas">
                        <p class="info">Foram encontradas <span><strong>15.324</strong> ordens</span>.</p>

                        <table id="gridOrdens_Todas" class="table table-condensed table-striped"></table> 
                        <div id="pagOrdens_Todas"></div>

                        <p class="acoes"><a href="#" class="btn btn-default">Cancelar Selecionadas</a></p>

                    </div>

                    <div class="tab-pane page-box fade" id="executadas">
                        <p></p>
                        <table id="gridOrdens_Executadas" class="table table-condensed table-striped"></table> 
                        <div id="pagOrdens_Executadas"></div>
                    </div>

                    <div class="tab-pane page-box fade" id="abertas">
                        <p></p>
                        <table id="gridOrdens_Abertas" class="table table-condensed table-striped"></table> 
                        <div id="pagOrdens_Abertas"></div>
                    </div>

                    <div class="tab-pane page-box fade" id="canceladas">
                        <p></p>
                        <table id="gridOrdens_Canceladas" class="table table-condensed table-striped"></table> 
                        <div id="pagOrdens_Canceladas"></div>
                    </div>

                    <div class="tab-pane page-box fade" id="rejeitadas">
                        <p></p>
                        <table id="gridOrdens_Rejeitadas" class="table table-condensed table-striped"></table> 
                        <div id="pagOrdens_Rejeitadas"></div>
                    </div>

                    <div class="tab-pane page-box fade" id="historicas">
                        <p></p>
                        <table id="gridOrdens_Historicas" class="table table-condensed table-striped"></table> 
                        <div id="pagOrdens_Historicas"></div>
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
</script>
</asp:Content>

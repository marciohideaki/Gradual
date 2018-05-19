<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="DireitosCreditorios.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.CadastroFundos" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">                        
            <div class="container-fluid">
                <input type="hidden" value="" id="hdnDireitos" name="hdnDireitos"/>
                <div class="row" style="height: 60px; padding-top: 20px">
                    <div class="col-md-1 col-xs-1"></div>
                    <div class="col-md-3 col-xs-3" style="width: 280px"><h4>Olá, FUNDO XYZ</h4></div>
                    <div class="col-md-3 col-xs-3"><h4>Ref: 10/12/2015</h4></div>
                    <div class="col-md-3 col-xs-3" style="width: 250px"><h4>Deseja fazer uma busca? </h4></div>
                    <div class="col-md-2 col-xs-2">
                        <input class="form-control input-sm" type="text" style="width:140px"/>
                        <span class="glyphicon glyphicon-search" title="Procurar"></span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1 col-xs-1"></div>
                    <div class="col-md-5 col-xs-5"><h2>Fundo D</h2></div>
                </div>
                <div class="row">
                    <%--<div class="col-md-1 col-xs-1"></div>--%>
                    <div class="col-md-8 col-xs-8">
                        <table id="gridDireitosCreditorios" class="table-condensed table-striped"></table>
                        <div id="pgGridDireitosCreditorios"></div>
                    </div>
                    <%--<div class="col-md-2 col-xs-2">
                        <input type="button" value="Gráfico" class="btn btn-success  btn-block"/>
                        <input type="button" value="Apontamentos" class="btn btn-info  btn-block"/>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    
    <style>
        .colspan2lines {
            height: 100px
        }

        a.glyphicons i:before {
            font-size: 15px;
            color: #37a6cd;
            display: inline-block;
            padding: 5px 0px 5px 0px;
        }

        /*altera cor da linha selecionada no grid desta tela*/
        .ui-widget-content tr.ui-row-ltr.ui-state-highlight,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight th,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight td {
            background: #efefb2;
        }

        span.glyphicon-search {
            font-size: 1.5em;
        }
        
    </style>
</asp:Content>

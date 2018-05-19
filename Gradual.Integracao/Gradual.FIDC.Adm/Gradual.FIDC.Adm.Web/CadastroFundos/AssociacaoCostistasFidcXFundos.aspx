<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="AssociacaoCostistasFidcXFundos.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.AssociacaoCostistasFidcXFundos" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <style>
        .colspan2lines {
            height: 100px
        }

        a.glyphicons i:before {
            font-size: 15px;
            color: #37a6cd;
            display: inline-block;
            padding: 5px 0 5px 0;
        }

        /*altera cor da linha selecionada no grid desta tela*/
        .ui-widget-content tr.ui-row-ltr.ui-state-highlight,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight th,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight td {
            background: #efefb2;
        }
        
        .glyphicon.glyphicon-file {
            font-size: 20px;
            cursor: pointer;
        }

        .dropdown-menu.open {
            width: 100% !important;
        }

        .bootstrap-select.form-control {
            border: 1px solid #ccc !important;
        }
                
    </style>
    <link rel="stylesheet" href="../Resc/Js/Lib/bootstrap-select-1.12.1/dist/css/bootstrap-select.css" rel="stylesheet" />
    <script type="text/javascript" src="../Resc/Js/Lib/bootstrap-select-1.12.1/dist/js/bootstrap-select.js"></script>

    <div class="innerLR">        
        <div id="AssociacaoCotistasFundos_pnlFiltroPesquisa">
                
            <!-- Container do filtro -->
            <div class="container-fluid" style="padding-top: 20px">
                <input type="hidden" value="" id="hdnIdCotistaFundo" name="hdnIdCotistaFundo"/>
                <input type="hidden" value="0" id="hdnGridCarregadoPorFundos" name="hdnGridCarregadoPorFundos"/>
                <input type="hidden" value="0" id="hdnGridCarregadoPorCotista" name="hdnGridCarregadoPorCotista"/>

                <div class="col-md-2"></div>
                <div class="col-md-8" id="colFormBusca">
                   
                    <div class="row">
                        <div class="col-md-12">
                            <select id="selFundo" class="form-control selectpicker show-tick" onchange="carregarGridPorFundo(this)" data-live-search="true">
                                <option value="0">Selecione um fundo</option>
                            </select>
                        </div>
                    </div>
                    <div class="row" style="height: 20px"></div>
                    <div class="row">
                        <div class="col-md-12">
                            <select id="selCotistaFidc" class="form-control selectpicker show-tick" onchange="carregarGridPorCotista(this)" data-live-search="true">
                                <option value="0">Selecione um cotista</option>
                            </select>
                        </div>
                    </div>
                    <div class="row" style="height: 20px"></div>
                    <div class="row" style="padding-right: 15px">
                        <div style="padding-bottom: 5px; float: right; width: 120px">
                            <input type="button" id="btnCotistasFundosAtualizar" value="Incluir" onclick="return btnCotistaFundoAtualizar_Click();" class="btn btn-primary btn-block" />
                        </div>
                        <div style="padding-bottom: 5px; float: right; width: 135px; padding-right: 15px">
                            <input type="button" id="btnCotistasFundosLimpar" value="Limpar" onclick="return btnCotistaFundoLimparDados_Click();" class="btn btn-warning  btn-block" />
                        </div> 
                        
                    </div>       
                    
                </div>
            </div>
        </div>  
        <div class="separator"></div>
        <!-- Container da Grid -->
        <div class="container-fluid">
            <div class="row" id="rExportarDadosCsv" hidden="hidden">
                <div class="col-md-9"></div>
                <div class="col-md-2" style="padding-left:72px"><span id="btnExportarDadosCsv" onclick="exportarDadosCotistaFundoCsv()" title="Exportar Dados" class='glyphicon glyphicon-file'></span></div>
            </div>
            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-md-10">
                    <table id="gridCotistasXFundos" class="table-condensed table-striped"></table>
                    <div id="pgGridCotistasXFundos"></div>
                </div>
            </div>
        </div>
    </div>
        
</asp:Content>

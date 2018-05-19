<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="TitulosLiquidados.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.TitulosLiquidados.TitulosLiquidados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .colspan2lines {
            height: 100px;
        }

        a.glyphicons i:before {
            font-size: 15px;
            color: #37a6cd;
            display: inline-block;
            padding: 5px 0px 5px 0px;
        }

        .ui-pg-input {
            width: auto !important;
        }
    </style>

    <div class="innerLR">
        <div class="row">
            <div class="col-md-offset-1 col-md-10">
                <div class="formulario box-generic">
                    <div>
                    <a id="lnkShowHidePesquisa" href="javascript:void(0);" onclick="TitulosLiquidados_ShowPainelPesquisa()">
                        <i id="titLiqFiltroIcone" class="glyphicon glyphicon-minus table-bordered"></i>&nbsp;Filtro
                    </a>
                    </div>
                    <div id="TitulosLiquidados_pnlFiltroPesquisa">
                        <div class="container-fluid">
                            <br />
                            <div class="row">
                                <!-- Container do filtro -->
                                <div class="col-md-offset-1 col-md-10">
                                    <!--Primeira linha -->
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input type="text" id="txtTitulosLiquidadosADMCodigoFundo" class="txtTitulosLiquidadosCodigoFundo" placeholder="Codigo " />
                                        </div>
                                        <div class="col-md-9">
                                            <div class="input-group">
                                                <input type="text" id="txtTitulosLiquidadosADMNomeFundo" class="form-control txtTitulosLiquidadosNomeFundo" placeholder="Digite o nome do fundo" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-search"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="input-group date" id="dvTitulosLiquidadosADMDataDe">
                                                <input type="datetime" class="form-control txtTitulosLiquidadosADMDataDe" id="txtTitulosLiquidadosADMDataDe" maxlength="10" placeholder="____/____/____" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="input-group date" id="dvTitulosLiquidadosADMDataAte">
                                                <input type="datetime" class="form-control txtTitulosLiquidadosADMDataAte" id="txtTitulosLiquidadosADMDataAte" maxlength="10" placeholder="____/____/____" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-2 col-md-offset-2">
                                            <input type="button" id="btnTitulosLiquidadosLimpar" value="Limpar" onclick="return btnTitulosLiquidadosLimpar_Click();" class="btn btn-warning  btn-block btn-sm" />
                                        </div>
                                        <div class="col-md-2">
                                            <input type="button" id="btnTitulosLiquidadosBuscar" value="Buscar" onclick="return btnFiltroTitulosLiquidadosADM_Click();" class="btn btn-primary btn-block btn-lg" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="separator">
        </div>

        <div class="row">
            <div class="col-md-offset-1 col-md-10">
                <div class="formulario box-generic">
                    <!-- Container da Grid -->
                    <div class="container-fluid">
                        <table id="gridTitulosLiquidadosADM" class="table-condensed table-striped"></table>
                        <div id="pgTitulosLiquidadosADM"></div>
                    </div>
                    <br />
                </div>
            </div>
        </div>

    </div>
</asp:Content>

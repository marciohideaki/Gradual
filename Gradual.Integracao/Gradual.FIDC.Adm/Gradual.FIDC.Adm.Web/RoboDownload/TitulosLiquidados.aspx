<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TitulosLiquidados.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.RoboDownload.TitulosLiquidados"  MasterPageFile="~/Principal.Master"  %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        
        <div class="formulario box-generic">
            <h7><a id="lnkShowHidePesquisa" href="javascript:void(0);" onclick="TitulosLiquidados_ShowPainelPesquisa()"><i class="icon-minus"></i>Filtro</a></h7>

            <div id="TitulosLiquidados_pnlFiltroPesquisa">
                
                <!-- Container do Filtro -->
                <div class="container-fluid">

                    <!-- Primeira linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtTitulosLiquidadosCodigoFundo" class="txtTitulosLiquidadosCodigoFundo" placeholder="Codigo " />
                        </div>
                        <div class="col-sm-6">
                            <input type="text" id="txtTitulosLiquidadosNomeFundo" class="txtTitulosLiquidadosNomeFundo" placeholder="Digite o nome do fundo"/>
                        </div>
                    </div>
                    
                    <!-- Segunda linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtTitulosLiquidadosDataDe"  class="txtTitulosLiquidadosDataDe" placeholder="De:" value="<% Response.Write(DateTime.Now.AddDays(-1).ToString("dd/MM/yy")); %>"/>
                        </div>
                        <div class="col-sm-2">
                            <input type="text" id="txtTitulosLiquidadosDataAte"  class="txtTitulosLiquidadosDataAte" placeholder="Até:" value="<% Response.Write(DateTime.Now.AddDays(1).ToString("dd/MM/yy")); %>"/>
                        </div>
                        <div class="col-sm-2 col-sm-offset-2">
                            <input type="button" id="btnTitulosLiquidadosLimpar" value="Limpar" onclick="return btnFiltroTitulosLiquidadosLimpar_Click();" class="btn btn-warning btn-block btn-sm" />
                        </div>
                    </div>

                    <!-- Terceira linha -->
                    <div class="row">

                        <div class="col-sm-4">
                            <div class="checkbox">
                                <label>
                                    <input class="checkbox" type="checkbox" value="1" id="chkTitulosLiquidadosDownloadsPendentes" name="chkTitulosLiquidadosDownloadsPendentes" />
                                    Downloads Pendentes
                                </label>
                            </div>
                        </div>

                        <div class="col-sm-2 col-sm-offset-2">
                            <input type="button" id="btnTitulosLiquidadosBuscar" value="Buscar" onclick="return btnFiltroTitulosLiquidados_Click();" class="btn btn-primary btn-block btn-lg" /> 
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="separator"></div>

        <!-- Container da Grid -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <table id="gridTitulosLiquidados"></table>
                    <div   id="pgTitulosLiquidados"></div>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            
            $("input.txtTitulosLiquidadosDataDe").datepicker({
                dateFormat: "dd/mm/y"
                , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
                , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
                , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
            });

            $("input.txtTitulosLiquidadosDataAte").datepicker({
                dateFormat: "dd/mm/y"
                , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
                , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
                , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
            });

            $("#txtTitulosLiquidadosNomeFundo").autocomplete({
                minLength: 3
                , source: function (request, response) {
                    var lUrl = Aux_UrlComRaiz("/Handlers/ListaFundosHandler.ashx");
                    $.ajax
                    ({
                        url: lUrl + "?NomeFundo=" + $("#txtTitulosLiquidadosNomeFundo").val(),
                        dataType: "json",
                        type: "POST",
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                    label: item.NomeFundo,
                                    value: item.CodigoFundo
                                }
                            }))
                        },
                        error: function (a, b, c) {
                            debugger;
                        }
                    });
                }
                , select: function (event, ui) {
                    event.preventDefault();
                    $("#txtTitulosLiquidadosCodigoFundo").val(ui.item.value);
                    $("#txtTitulosLiquidadosNomeFundo").val(ui.item.label);
                }
            });

        });


    </script>
</asp:content>

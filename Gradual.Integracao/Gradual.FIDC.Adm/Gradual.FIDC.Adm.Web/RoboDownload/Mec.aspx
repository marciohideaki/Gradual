<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mec.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.RoboDownload.Mec"  MasterPageFile="~/Principal.Master"  %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        
        <div class="formulario box-generic">
            <h7><a id="lnkShowHidePesquisa" href="javascript:void(0);" onclick="Mec_ShowPainelPesquisa()"><i class="icon-minus"></i>Filtro</a></h7>
            
            <div id="Mec_pnlFiltroPesquisa">
                
                <!-- Container do Filtro -->
                <div class="container-fluid">
                    
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtMecCodigoFundo" class="txtMecCodigoFundo" placeholder="Codigo "/>
                        </div>
                        <div class="col-sm-6">
                            <input type="text" id="txtMecNomeFundo" class="txtMecNomeFundo" placeholder="Digite o nome do fundo"/>
                        </div>
                    </div>
                        
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtMecDataDe"  class="txtMecDataDe" placeholder="De:" value="<% Response.Write(DateTime.Now.AddDays(-1).ToString("dd/MM/yy")); %>"/>
                        </div>
                        <div class="col-sm-2">
                            <input type="text" id="txtMecDataAte" class="txtMecDataAte" placeholder="Até:" value="<% Response.Write(DateTime.Now.AddDays(1).ToString("dd/MM/yy")); %>"/>
                        </div>

                        <div class="col-sm-2">
                            <select  id="cboMecLocalidade" class="cboMecLocalidade"> 
                                <option value="0">Localidade</option>
                                <option value="1">Banco Paulista</option>
                                <option value="2">Banco Santander</option>
                            </select>
                        </div>
                        <div class="col-sm-2">
                            <input type="button" id="btnMecLimpar" value="Limpar" onclick="return btnFiltroMecLimpar_Click();" class="btn btn-warning btn-block btn-sm" /> 
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="checkbox">
                                <label>
                                    <input class="checkbox" type="checkbox" value="1" id="chkMecDownloadsPendentes" name="chkMecDownloadsPendentes" />
                                    Downloads Pendentes
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-2 col-sm-offset-2">
                            <input type="button" id="btnMecBuscar" value="Buscar" onclick="return btnFiltroMec_Click();" class="btn btn-primary btn-block btn-lg" /> 
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="separator"></div>

        <!-- Container da Grid -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12 ">
                    <!--<table id="gridMec" class="table-condensed table-striped"></table>-->
                    <table id="gridMec" class="table-condensed table-striped"></table>
                    <div   id="pgMec"></div>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            /*
            $("#txtHoraOperacaoIni").mask("00:00");
            $("#txtHoraOperacaoFim").mask("00:00");
            */

            $("input.txtMecDataDe").datepicker({
                dateFormat: "dd/mm/y"
                , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
                , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
                , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
            });

            $("input.txtMecDataAte").datepicker({
                dateFormat: "dd/mm/y"
                , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
                , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
                , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
                //, maxDate: "+0"
            });

            $("#txtTermoBusca").keypress(function (event) {
                if (event.keyCode == 13) {
                    $("#btnBuscar").click();
                    return false;
                }
            });

            $("#txtMecNomeFundo").autocomplete({
                minLength: 3
                , source: function (request, response) {
                    var lUrl = Aux_UrlComRaiz("/Handlers/ListaFundosHandler.ashx");
                    $.ajax
                    ({
                        url: lUrl + "?NomeFundo=" + $("#txtMecNomeFundo").val(),
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
                    $("#txtMecCodigoFundo").val(ui.item.value);
                    $("#txtMecNomeFundo").val(ui.item.label);
                }
            });

        });
    </script>
</asp:content>

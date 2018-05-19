<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Carteiras.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.RoboDownload.Carteiras" MasterPageFile="~/Principal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">

            <h7><a id="lnkShowHidePesquisa" href="javascript:void(0);" onclick="Carteiras_ShowPainelPesquisa()"><i class="icon-minus"></i>Filtro</a></h7>
            <div id="Carteiras_pnlFiltroPesquisa">
                
                <!-- Container do filtro -->
                <div class="container-fluid">

                    <!-- Primeira linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtCarteiraCodigoFundo" class="txtCarteiraCodigoFundo" placeholder="Codigo " />
                        </div>
                        <div class="col-sm-6">
                            <input type="text" id="txtCarteiraNomeFundo" class="txtCarteiraNomeFundo" placeholder="Digite o nome do fundo"/>
                        </div>
                    </div>

                    <!-- Segunda linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtCarteiraDataDe" class="txtCarteiraDataDe" placeholder="De:" value="<% Response.Write(DateTime.Now.AddDays(-1).ToString("dd/MM/yy")); %>" />
                        </div>
                        <div class="col-sm-2">
                            <input type="text" id="txtCarteiraDataAte" class="txtCarteiraDataAte" placeholder="Até:" value="<% Response.Write(DateTime.Now.AddDays(1).ToString("dd/MM/yy")); %>" />
                        </div>
                        <div class="col-sm-2">
                            <select id="cboCarteiraLocalidade" class="cboCarteiraLocalidade">
                                <option value="0">Localidade</option>
                                <asp:repeater id="rptOrigens" runat="server">
                                    <itemtemplate>
                                        <option value="<%#Eval("Codigo") %>"><%# Eval("Descricao") %></option>
                                    </itemtemplate>
                                </asp:repeater>
                            </select>
                        </div>
                        <div class="col-sm-2">
                            <input type="button" id="btnCarteirasLimpar" value="Limpar" onclick="return btnFiltroCarteirasLimpar_Click();" class="btn btn-warning  btn-block btn-sm" />
                        </div>
                    </div>

                    <!-- Terceira linha -->
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="checkbox">
                                <label>
                                    <input class="checkbox" type="checkbox" value="1" id="chkCarteirasDownloadsPendentes" name="chkCarteirasDownloadsPendentes" />
                                    Downloads Pendentes
                                </label>
                            </div>
                        </div>

                        <div class="col-sm-2 col-sm-offset-2">
                            <input type="button" id="btnCarteirasBuscar" value="Buscar" onclick="return btnFiltroCarteiras_Click();" class="btn btn-primary btn-block btn-lg" />
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
                    <table id="gridCarteiras" class="table-condensed table-striped"></table>
                    <div   id="pgCarteiras"></div>
                </div>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        $(document).ready(function () {

            $("input.txtCarteiraDataDe").datepicker({
                dateFormat: "dd/mm/y"
                , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
                , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
                , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
            });

            $("input.txtCarteiraDataAte").datepicker({
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

            $("#txtCarteiraNomeFundo").autocomplete({
                minLength: 3
                , source: function (request, response)
                {
                    var lUrl = Aux_UrlComRaiz("/Handlers/ListaFundosHandler.ashx");
                    $.ajax
                   ({
                       url: lUrl + "?NomeFundo=" + $("#txtCarteiraNomeFundo").val(),
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
                , select: function (event, ui)
                {
                    event.preventDefault();
                    $("#txtCarteiraCodigoFundo").val(ui.item.value);
                    $("#txtCarteiraNomeFundo").val(ui.item.label);
                }
            });
        });
    </script>
</asp:Content>

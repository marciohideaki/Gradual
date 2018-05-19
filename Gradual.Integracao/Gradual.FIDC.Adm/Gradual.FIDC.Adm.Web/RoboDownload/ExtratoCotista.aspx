<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtratoCotista.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.RoboDownload.ExtratoCotista"  MasterPageFile="~/Principal.Master"  %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        
        <div class="formulario box-generic">
            <h7><a id="lnkShowHidePesquisa" href="javascript:void(0);" onclick="ExtratoCotista_ShowPainelPesquisa()"><i class="icon-minus"></i>Filtro</a></h7>
            
            <div id="ExtratoCotista_pnlFiltroPesquisa">

                <!-- Container do Filtro -->
                <div class="container-fluid">
                    
                    <!-- Primeira linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtExtratoCotistaCodigoFundo" class="txtExtratoCotistaCodigoFundo" placeholder="Codigo " />
                        </div>
                        <div class="col-sm-6">
                            <input type="text" id="txtExtratoCotistaNomeFundo" class="txtExtratoCotistaNomeFundo" placeholder="Digite o nome do fundo"/>
                        </div>
                    </div>

                    <!-- Segunda linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtExtratoCotistaCpfCnpj" class="txtExtratoCotistaCpfCnpj" placeholder="CPF"/>
                        </div>
                        <div class="col-sm-6">
                            <select  id="cboExtratoCotistaNomeCotista" class="cboExtratoCotistaNomeCotista" runat="server" style="margin-bottom:10px"> 
                                <option value="0">Nome Cotista</option>
                            </select>
                        </div>
                    </div>

                    <!-- Segunda linha -->
                    <div class="row">
                        <div class="col-sm-2">
                            <input type="text" id="txtExtratoCotistaData"  class="txtExtratoCotistaData" placeholder="Período:" value="<% Response.Write(DateTime.Now.AddDays(-1).ToString("MM/yy")); %>"/>
                        </div>
<%--                        <div class="col-sm-2">
                            <input type="text" id="txtExtratoCotistaDataAte" class="txtExtratoCotistaDataAte" placeholder="Até:" value="<% Response.Write(DateTime.Now.AddDays(1).ToString("dd/MM/yy")); %>"/>
                        </div>--%>
                        <div class="col-sm-2 col-sm-offset-4">
                            <input type="button" id="btnExtratoCotistaLimpar" value="Limpar" onclick="return btnFiltroExtratoCotistaLimpar_Click();" class="btn btn-warning btn-block btn-sm" /> 
                        </div>
                    </div>

                    <!-- terceira linha -->
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="checkbox">
                                <label><input class="checkbox" type="checkbox" value="1" id="chkExtratoCotistaDownloadsPendentes" name="chkExtratoCotistaDownloadsPendentes" /> Downloads Pendentes</label>
                            </div>
                        </div>
                        <div class="col-sm-2 col-sm-offset-2">
                            <input type="button" id="btnExtratoCotistaBuscar" value="Buscar" onclick="return btnFiltroExtratoCotista_Click();" class="btn btn-primary btn-block btn-lg" /> 
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
                    <table id="gridExtratoCotista" class="table-condensed table-striped"></table>
                    <div   id="pgExtratoCotista"></div>
                </div>
            </div>
        </div>

    </div>
    <style type="text/css">
        .ui-datepicker-calendar 
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        //var dateAsObject = new Date();
        var dateAsObject = new Date();

        $(document).ready(function () {
            /*
            $("#txtHoraOperacaoIni").mask("00:00");
            $("#txtHoraOperacaoFim").mask("00:00");
            */

            $("input.txtExtratoCotistaData").datepicker({
                dateFormat: "mm/y"
                , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
                , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
                , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
                //, numberOfMonths: [ 2, 3 ]
                //, changeMonth: true
                //, changeYear: true
                , showButtonPanel: true
                , onClose: function (dateText, inst)
                {

                }
                , currentText: "Hoje"
                , closeText: "Fechar"
                , show: function () {
                    //dateAsObject = new Date();
                    $("input.txtExtratoCotistaData").datepicker('setDate', dateAsObject);
                }
                , beforeShow: function()
                {
                    $("input.txtExtratoCotistaData").datepicker('setDate', dateAsObject);
                    
                }
            });
            

            $(document).on('click', '.ui-datepicker-next', function ()
            {
                dateAsObject = new Date(dateAsObject.getFullYear(), dateAsObject.getMonth() + 1, 1);
                $("input.txtExtratoCotistaData").datepicker('setDate', dateAsObject);
            })

            $(document).on('click', '.ui-datepicker-prev', function ()
            {
                dateAsObject = new Date(dateAsObject.getFullYear(), dateAsObject.getMonth() - 1, 1);
                $("input.txtExtratoCotistaData").datepicker('setDate', dateAsObject);
            })
            
            $(document).on('click', '.ui-datepicker-current', function ()
            {
                dateAsObject = new Date();
                $("input.txtExtratoCotistaData").datepicker('setDate', dateAsObject);
            })
            //,navigationAsDateFormat: true, nextText: 'MM', prevText: 'MM'

            //$("input.txtExtratoCotistaDataAte").datepicker({
            //    dateFormat: "dd/mm/y"
            //    , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
            //    , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
            //    , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
            //    //, maxDate: "+0"
            //});

            $("#txtTermoBusca").keypress(function (event) {
                if (event.keyCode == 13) {
                    $("#btnBuscar").click();
                    return false;
                }
            });

            $("#txtExtratoCotistaNomeFundo").autocomplete({
                minLength: 3
                , source: function (request, response)
                {
                    var lUrl = Aux_UrlComRaiz("/Handlers/ListaFundosHandler.ashx");
                    $.ajax
                    ({
                        url: lUrl + "?NomeFundo=" + $("#txtExtratoCotistaNomeFundo").val(),
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
                    $("#txtExtratoCotistaCodigoFundo").val(ui.item.value);
                    $("#txtExtratoCotistaNomeFundo").val(ui.item.label);
                }
            });

        });
    </script>

</asp:content>
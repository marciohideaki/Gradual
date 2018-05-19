<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoramentoRisco.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.MonitoramentoDeRisco" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery-1.4.2.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- jQuery UI master: -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery-ui-1.8rc3.custom.js?v=<%= this.VersaoDoSite %>"></script> 

    <!-- jQuery UI arquivos de core que os efeitos e widgets precisam: -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.core.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.effects.core.js?v=<%= this.VersaoDoSite %>"></script>

    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.widget.js?v=<%= this.VersaoDoSite %>"></script>
    
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.autocomplete.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.position.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.mouse.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- jQuery UI arquivos dos efeitos e widgets que nós utilizamos: -->

    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.datepicker.js?v=<%= this.VersaoDoSite %>"></script> 
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.datepicker-pt-BR.js?v=<%= this.VersaoDoSite %>"></script> 
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/ui/jquery.ui.draggable.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- jQuery arquivos de outros plugins que nós utilizamos: -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.cookie.js?v=<%= this.VersaoDoSite %>"></script>                  <!-- Suporte a cookies -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.format-1.0.js?v=<%= this.VersaoDoSite %>"></script>              <!-- Formatação de números -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.json-2.2.js?v=<%= this.VersaoDoSite %>"></script>                <!-- Serialização JSON -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.validationEngine.js?v=<%= this.VersaoDoSite %>"></script>        <!-- Validação -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.validationEngine-ptBR.js?v=<%= this.VersaoDoSite %>"></script>   <!-- Mensagens de Validação -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.customInput.js?v=<%= this.VersaoDoSite %>"></script>             <!-- Checks e Radios customizados -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.maskedinput-1.2.2.js?v=<%= this.VersaoDoSite %>"></script>       <!-- Máscara para inputs -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.bt.js?v=<%= this.VersaoDoSite %>"></script>                      <!-- Tooltips customizados -->
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jquery.ajaxfileupload.js?v=<%= this.VersaoDoSite %>"></script>          <!-- Input de upload ajax -->

    <!-- jqGrid -->

<%--    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/i18n/grid.locale-pt-br.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/jquery.jqGrid.js?v=<%= this.VersaoDoSite %>"></script>--%>

    <!-- Biblioteca de gráfico em canvas -->
    <script type="text/javascript" src="../../../../Js/Lib/Etc/Dev/raphael.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/Etc/Dev/g.raphael-min.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/Etc/Dev/g.pie.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Bibliotecas da Gradual: -->
    <script type="text/javascript" src="../../../../Js/Lib/Gradual/Dev/00-GradAuxiliares.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/Gradual/Dev/01-GradSettings.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Javascripts de Páginas (Janelas, etc): -->

    <!--script type="text/javascript" src="Js/Pages/Dev/01-AcompanhamentoDeOrdem.aspx.js"></script-->

    <script type="text/javascript" src="../../../../Js/Paginas/Dev/01-GradIntra-Principal.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/02-GradIntra-Navegacao.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/03-GradIntra-Busca.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/04-GradIntra-Cadastro.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/05-GradIntra-Relatorio.js?v=<%= this.VersaoDoSite %>"></script>
    
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/11-Clientes.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/12-Risco.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/14-Seguranca.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/15-Monitoramento.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/16-Sistema.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/17-Relatorios.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/18-HomeBroker.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/19-Solicitacoes.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/42-Default.aspx.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/43-PoupeOperacoes.js?v=<%= this.VersaoDoSite %>"></script>

    <link rel="Stylesheet" media="all" href="../../../../Skin/Default/Principal.css" />
    <link rel="Stylesheet" media="screen" href="../../../../Skin/Default/ValidationEngine.jquery.css" />
    <link rel="Stylesheet" media="screen" href="../../../../Skin/Default/Intranet/Default.aspx.css" />
    <link rel="Stylesheet" media="all" href="../../../../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"    href="../../../../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" media="print"  href="../../../../Skin/Default/Relatorio.print.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../../../../Skin/Default/gradintra-theme/jquery-ui-1.8.1.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../../../../Skin/Default/ui.jqgrid.css" />

    <title>Gradual Investimentos - Intranet</title>

</head>
<body>
    <form id="form1" runat="server">

    <h1 style="width: 100%;"><span>Header Intranet Gradual</span></h1>

    <div id="pnlRisco_Monitoramento_Risco_Pesquisa" class="Busca_Formulario" style="width: 99.4%; height: 260px;">
    
        <h3 style="margin: 15px 0pt 15px 25px;">- Monitoramento de Risco</h3>

        <div style="float: left; width: 25em;">

            <p style="width: 80%;">
                <label for="txtDBM_FiltroRelatorio_CodAssessor">Cód. Assessor:</label>
                <input  id="txtDBM_FiltroRelatorio_CodAssessor" type="text" runat="server" Propriedade="CodAssessor" class="ProibirLetras" />
            </p>

            <p style="width: 80%;">
                <label for="txtDBM_FiltroRelatorio_CodigoCliente">Cód. Cliente:</label>
                <input  id="txtDBM_FiltroRelatorio_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" class="ProibirLetras" />
            </p>

        </div>

        <div style="float: left; width: 40em;">

            <p style="width: 80%;">
                <label for="cboRisco_Monitoramento_FiltroGrupoAlavancagem">Grupo Alavanc:</label>
                <select id="cboRisco_Monitoramento_FiltroGrupoAlavancagem" Propriedade="grupoalavanc" style="width:20.2em" onchange="GradIntra_Risco_Monitoramento_Buscar();">
                    <option value="">[Todos]</option>
                    <asp:Repeater runat="server" ID="rptRisco_Monitoramento_FiltroGrupoAlavancagem">
                        <ItemTemplate>
                            <option value="<%# Eval("Id") %>"><%# Eval("Descricao") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </p>

            <p style="width: 80%;">
                <label for="cboRisco_Monitoramento_FiltroParametro">Parâmetro:</label>
                <select id="cboRisco_Monitoramento_FiltroParametro" Propriedade="parametro" style="width:20.2em" onchange="GradIntra_Risco_Monitoramento_Buscar();">
                    <option value="">[Todos]</option>
                    <asp:Repeater runat="server" ID="rptRisco_Monitoramento_FiltroParametro">
                        <ItemTemplate>
                            <option value="<%# Eval("Id") %>"><%# Eval("Descricao") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </p>

        </div>

        <h4 style="margin-left: 10px; width: 60%; margin-top: 8em;">Utilização do limite:</h4>
        <p style="margin-left: 9em; width: 60%;">
            <input type="checkbox" onclick="GradIntra_Risco_Monitoramento_Buscar()" id="chkRisco_Monitoramento_LimiteMaior75" checked="checked" Propriedade="chkMaior75Menor100" />
            <span class="SemaforoVermelho"></span>
            <label for="chkRisco_Monitoramento_LimiteMaior75">Limite maior igual a 75% e menor igual a 100%</label>
        </p>

        <p style="margin-left: 9em; width: 60%;">
            <input type="checkbox" onclick="GradIntra_Risco_Monitoramento_Buscar()" id="chkRisco_Monitoramento_LimiteMaior50Menor75" checked="checked" Propriedade="chkMaior50Menor75" />
            <span class="SemaforoAmarelo"></span>
            <label for="chkRisco_Monitoramento_LimiteMaior50Menor75">Limite maior igual a 50% e menor que 75%</label>
        </p>

        <p style="margin-left: 9em; width: 60%;">
            <input type="checkbox" onclick="GradIntra_Risco_Monitoramento_Buscar()" id="chkRisco_Monitoramento_LimiteMenor50" checked="checked" Propriedade="chkMenor50" />
            <span class="SemaforoVerde"></span>
            <label for="chkRisco_Monitoramento_LimiteMenor50">Limite menor que 50%</label>
        </p>

        <p style="width: 60%; padding-bottom: 15px; margin-left: 21.2em; position: absolute; margin-top: 1px;">
            <label  id="lblRisco_Monitoramento_AtualizarAutomaticamente_ContagemRegressiva" style="width: 60%; display: block; float: left; text-align: left; margin-left: 171px;"></label>
        </p>

        <p style="margin-left: 39em; width: 60%; position: absolute; margin-top: 30px;">
            <input  id="chkRisco_MonitoramentoRisco_AtualizarAutomaticamente" type="checkbox" style="float: left;" onclick="return chkRisco_MonitoramentoRisco_AtualizarAutomaticamente_Click(this);" />
            <label for="chkRisco_MonitoramentoRisco_AtualizarAutomaticamente" style="width: 60%; float: left; text-align: left;">Atualizar automaticamente.</label>
        </p>
        
        <p style="text-align: left; float: left; margin-left: 37em; width: 60%; position: absolute; margin-top: 60px;">
            <button class="btnBusca" onclick="return btnRisco_Monitoramento_Busca_Click(this)" id="btnRisco_Monitoramento_Busca">Buscar</button>
            <button class="btnBusca" onclick="window.print(); return false;" id="btnClienteRelatorioImprimir" style="width:auto;margin-left:8px;" disabled="disabled">Imprimir Relatório</button>
            <img id="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../../../../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="cursor: pointer; float: none; margin-left: 5px;">
        </p>

        <p style="display: block; width: 60%; text-align: left; position: absolute; margin: 95px 0pt 0pt 43.25em;">
            <button class="btnBusca_" onclick="return btnRisco_Monitoramento_ExportarParaExcel_Click()" style="width: auto;">Exportar para Excel</button>
        </p>

    </div>

    <div id="pnlResultado_MonitoramentoRisco" style="background-color: #fff; display: none; margin: 24.5em 0pt 0pt 3px; position: absolute; height: auto; width: auto;">
    
        <h2 style="margin: 20px 0pt 0pt 47px;">Monitoramento de Risco</h2>

        <div style="float: left; margin: 16px 0pt 10px 50px; width: 80%;">
            <p style="float: left; width: 100%;">
                <label style="float:left; text-align:right; width: 17em;">Última consulta: </label>
                <label style="margin-left: 5px; width: auto;" id="lblRisco_Monitoramento_HoraUltimaConsulta"></label>
            </p>
            <p style="float: left; width: 100%">
                <label style="float:left;text-align:right; width: 17em;">Assessor consultado: </label>
                <label style="margin-left: 5px; width: auto;" id="lblRisco_Monitoramento_AssessorConsultado"></label>
            </p>
            <p style="float: left; width: 100%;">
                <label style="float:left;text-align:right; width: 17em;">Cliente consultado: </label>
                <label style="margin-left: 5px; width: auto;" id="lblRisco_Monitoramento_ClienteConsultado"></label>
            </p>
            <p style="float: left; width: 100%;">
                <label style="float:left;text-align:right; width: 17em;">Grupo de alavancagem filtrado por: </label>
                <label style="margin-left: 5px; width: auto;" id="lblRisco_Monitoramento_AlavancagemConsultado"></label>
            </p>
            <p style="float: left; width: 100%;">
                <label style="float:left;text-align:right; width: 17em;">Parâmetro filtrado por: </label>
                <label style="margin-left: 5px; width: auto;" id="lblRisco_Monitoramento_ParametroConsultado"></label>
            </p>
        </div>

        <table id="tblResultado_MonitoramentoRisco" class="GridRelatorio" style="font-size: 11px; margin-left:45px; margin-right: 45px; width: 120em;">
            <thead>
                <tr>
                    <td style="text-align: center; width: auto;"></td>
                    <td style="text-align: left;   width: auto;">Cliente</td>
                    <td style="text-align: center; width: auto;">Assessor</td>
                    <td style="text-align: left;   width: auto;">Parâmetro</td>
                    <td style="text-align: left;   width: auto;">Grupo</td>
                    <td style="text-align: right;  width: auto;">Limite (R$)</td>
                    <td style="text-align: right;  width: auto;">Alocado (R$)</td>
                    <td style="text-align: right;  width: auto;">Disponível (R$)</td>
                </tr>
            </thead>
            <tbody>
                <tr class="LinhaMatrix" style="display: none;">
                    <td id="status"     style="text-align: center" align="center" align="center"></td>
                    <td id="cliente"    style="text-align: left"></td>
                    <td id="assessor"   style="text-align: left"></td>
                    <td id="parametro"  style="text-align: left"></td>
                    <td id="grupo"      style="text-align: right"></td>
                    <td id="limite"     style="text-align: right"></td>
                    <td id="alocado"    style="text-align: right; color: #FF0000"></td>
                    <td id="disponivel" style="text-align: right"></td>
                </tr>
            </tbody>
        </table>

    </div>

    <script  type="text/javascript">
        GradIntra_HabilitarInputsComMascaras($("#pnlRisco_Monitoramento_Risco_Pesquisa"));
    </script>

    </form>
</body>
</html>

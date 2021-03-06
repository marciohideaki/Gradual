﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoramentoCustodia.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.MonitoramentoCustodia" %>
<head id="Head1" runat="server">
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
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/i18n/grid.locale-pt-br.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/jquery.jqGrid.MonitoramentoRisco.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Biblioteca de gráfico em canvas -->
    <script type="text/javascript" src="../../../../Js/Lib/Etc/Dev/raphael.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/Etc/Dev/g.raphael-min.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/Etc/Dev/g.pie.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Bibliotecas da Gradual: -->
    <script type="text/javascript" src="../../../../Js/Lib/Gradual/Dev/00-GradAuxiliares.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/Gradual/Dev/01-GradSettings.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Javascripts de Páginas (Janelas, etc): -->
    <script type="text/javascript" src="../Js/Paginas/Min/99-Paginas.min.js?v=<%= this.VersaoDoSite %>"></script>

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
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/44-Compliance.js?v=<%= this.VersaoDoSite %>"></script>

    <link rel="Stylesheet" media="all"     href="../../../../Skin/Default/Principal.css" />
    <link rel="Stylesheet" media="screen"  href="../../../../Skin/Default/ValidationEngine.jquery.css" />
    <link rel="Stylesheet" media="screen"  href="../../../../Skin/Default/Intranet/Default.aspx.css" />
    <link rel="Stylesheet" media="all"     href="../../../../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"     href="../../../../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" media="print"   href="../../../../Skin/Default/Relatorio.print.css" />
    
    <link rel="stylesheet" type="text/css" media="screen" href="../../../../Skin/Default/gradintra-theme/jquery-ui-1.8.1.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../../../../Skin/Default/ui.jqgrid.css" />
    <style type="text/css">
        .ui-jqgrid {font-size:0.6em}
        .ui-jqgrid tr.jqgrow td {font-size:0.7em}
        .ui-jqgrid-htable {font-size: 1.1em}
        .ui-jqgrid-pager{font-size: 1.1em}
        .ui-pg-table {font-size: 1.1em}
        .ui-jqgrid-hbox {font-size: 1.1em}
        .ui-jqgrid-hdiv {font-size: 1.1em}
        .ui-widget .ui-widget {font-size: 0.4em}
        .ui-jqgrid-title{font-size: 1.0em}
        .ui-state-highlight { background: #FDFFA5 !important; }
        
        /*
        .ui-jqgrid {font-size:0.6em}
        .ui-jqgrid tr.jqgrow td {font-size:0.7em}
        .ui-jqgrid-htable {font-size: 1.1em}
        .ui-jqgrid-pager{font-size: 1.1em}
        .ui-pg-table {font-size: 1.1em}
        .ui-jqgrid-hbox {font-size: 1.1em}
        .ui-jqgrid-hdiv {font-size: 1.1em}
        .ui-widget .ui-widget {font-size: 0.4em}
        .ui-jqgrid-title{font-size: 1.0em}
        .ui-jqgrid tr.ui-row-ltr td {
            border-right-style: none;
            border-left-style: none;
        }
        */
        /*.ui-jqgrid .ui-jqgrid-bdiv 
        {
              position: relative; 
              margin: 0em; 
              padding:0; 
              overflow-x:hidden;
              overflow-y:auto; 
              text-align:left;
        }*/
    </style>
    <!--[if IE]>
    <link rel="Stylesheet" media="screen" href="../Skin/<%= this.SkinEmUso %>/IE.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" media="print"  href="../Skin/<%= this.SkinEmUso %>/IE.print.css?v=<%= this.VersaoDoSite %>" />
    <![endif]-->
    
    <title>Gradual Investimentos - Intranet</title>

</head>
<body>
    <form id="fMonitoramentoCustodia" runat="server">
    <h1 style="width: 100%; height: 3em;"><span>Header Intranet Gradual</span></h1>
        
    <h3 style="margin: 10px 0pt 15px 25px; height: 15px">Monitoramento de Custódia</h3>

    <div id="pnlRisco_MonitoramentoCustodia_Pesquisa" class="Busca_Formulario" style="width: 99.25%; height: 11.0em;">
    
        <div style="width: 100%;">

            <p style="margin-left: 12px; width: 100%;">
                <label for="txtCliente">Cliente:</label>
                <input  id="txtCliente" type="text" maxlength="10" Propriedade="Cliente" style="width:150px;"/>
            </p>
            <p style="margin-left: 12px; width: 100%;">                
                <label for="txtAtivo">Ativo:</label>
                <input  id="txtAtivo" type="text" maxlength="10" Propriedade="Ativo" style="width:150px;"/>
            </p>

            <p style="margin-left: 12px; width: 100%;">
                <label for="cboTipoMercado">Mercado:</label>
                <select id="cboTipoMercado" Propriedade="Mercado" style="width:150px">
                    <!-- A opção todos é validada na CustodiaDbLib -->
                    <option value="(Todos)"> (Todos) </option>
                    <option value="TER" >TER - Termo</option>
                    <option value="VIS" >VIS - À Vista</option>
                    <option value="OPC" >OPC - Opção de Compra</option>
                    <option value="OPF" >OPF - Opções Futuro</option>
                    <option value="DIS" >DIS - Mercado Disponível</option>
                    <option value="FUT" >FUT - Futuro</option>
                    <option value="OPC" >OPC - Opção de Compra</option>
                </select>
            </p>

            <p style="margin-left: 40px; width: 100%;">
                
                <input id="chkDescoberto" type="checkbox" onclick="return chkDescoberto_Click(this);"/>
                <label for="chkDescoberto" style="text-align: left;">Somente descobertos.</label>

            </p>
            <p style="text-align: left; float: left; margin-left: 46.8em; width: 60%; position: absolute; margin-top: 95px;">
                <button class="btnBusca" onclick="return btnRisco_MonitoramentoCustodia_Click(this)" id="btnRisco_PLD_Busca">Buscar</button>
            </p>
        </div>
    </div>

    <div style="margin-top:11.5em;  position:absolute; background-color: White; width: 100%; text-align: center" >
        <div id="div_Risco_MonitoramentoCustodia_Resultados" style="margin-top: 2em; margin-left: 2em; margin-bottom: 2em; display: none">
        <div id="pager"></div>
        <table  id="tblBusca_MonitoramentoCustodia"></table>
        </div>
        <%--<div id="pnlBusca_MonitoramentoRisco_Resultados_Pager"></div>--%>
    </div>
    </form>
</body>
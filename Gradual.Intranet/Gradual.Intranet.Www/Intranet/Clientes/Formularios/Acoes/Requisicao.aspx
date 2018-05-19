<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Requisicao.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.TEDs.Requisicao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

    <!--script type="text/javascript" src="Js/Pages/Dev/01-AcompanhamentoDeOrdem.aspx.js"></script-->

    <script type="text/javascript" src="../../../../Js/Paginas/Dev/01-GradIntra-Principal.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/02-GradIntra-Navegacao.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/03-GradIntra-Busca.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/04-GradIntra-Cadastro.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/05-GradIntra-Relatorio.js?v=<%= this.VersaoDoSite %>"></script>
    
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/11-Clientes.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Paginas/Dev/45-TED.js?v=<%= this.VersaoDoSite %>"></script>
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
        .ui-jqgrid tr.ui-row-ltr td {
            border-right-style: none;
            border-left-style: none;
        }
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
    <form id="form1" runat="server">
    <div>
        <h1 style="width: 100%; height: 3em;"></h1>
        
        <h3 style="margin: 2px; height: 15px">RETIRADAS</h3>

        <div id="Div1" class="Busca_Formulario" style="margin: 2px; height: 23em; width: 100%">
            <p>            
                <label for="txtRequisicao_Conta">CONTA:</label>
                <input id="txtRequisicao_Conta" type="text" value="31940" maxlength="10" style="width: 5em;"/>
            </p>

            <p>
                <label for="txtRequisicao_Nome">NOME:</label>
                <input  id="txtRequisicao_Nome" type="text" value="RAFAEL SANCHES GARCIA" maxlength="10" style="width: 20em;"/>
            </p>

            <p>
                <label for="txtRequisicao_CPF">CPF:</label>
                <input name="txtRequisicao_CPF" id="txtTed_Filtro_DataInicial" type="text" maxlength="10" style="width: 20em;" value="999 999 999 99"/>
            </p>

            <p>
                <label for="txtRequisicao_CC">CC:</label>
                <input  id="txtRequisicao_CC" type="text" value="HSBC BANK BRASIL AS - AG 1191-3 CTA:" maxlength="10" style="width: 30em;" />
            </p>

            <p>
                <label for="txtRequisicao_Data">DATA:</label>
                <input  id="txtRequisicao_Data" type="text" value="09/10/2015" maxlength="10" style="width: 10em;" />
            </p>

            <p>
                <label for="txtRequisicao_Saldo">SALDO DISP.:</label>
                <input  id="txtRequisicao_Saldo" type="text" value="471,44" maxlength="10" style="width: 10em;" />
            </p>

            <p>
                <label for="txtRequisicao_Valor">VALOR DA RETIRADA:</label>
                <input  id="txtRequisicao_Valor" type="text" maxlength="10" style="width: 15em;" />
            </p>
            
            <p></p>
                <button class="" onclick="return btnTed_Busca_Click(this)" id="btnAprovar">CONFIRMAR</button>
            </p>

        </div>
    </div>
    </form>
</body>
</html>

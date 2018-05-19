<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoramentoRiscoGeralDetalhamento.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.MonitoramentoRiscoGeralDetalhamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/i18n/grid.locale-pt-br.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/jquery.jqGrid.MonitoramentoRisco.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../../../../Js/Lib/jQuery/Dev/jqgrid/grid.subgrid.js?v=<%= this.VersaoDoSite %>"></script>
    
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
        .ui-jqgrid {font-size:0.8em}
        .ui-jqgrid tr.jqgrow td {font-size:1.3em}
        .ui-jqgrid tr.jqgrow ui-row-ltr {font-size:1.5em}
        
        /*.ui-jqgrid-htable {font-size: 1.1em}
        .ui-jqgrid-pager{font-size: 1.1em}
        .ui-pg-table {font-size: 1.1em}
        .ui-jqgrid-hbox {font-size: 1.1em}
        .ui-jqgrid-hdiv {font-size: 1.1em}
        .ui-widget .ui-widget {font-size: 0.4em}
        
        .ui-jqgrid-title{font-size: 2.0em}
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
    <form id="form1" runat="server">
    
    <h1 style="width: 100%; height: 3em;">

        <span>Header Intranet Gradual</span></h1>

    <h3 style="margin: 10px 0pt 15px 25px; height: 15px">
        <label>Monitoramento de Lucros e Prejuízos</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input  id="chkRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente" type="checkbox"  onclick="return chkRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_Click(this);" checked="checked" />
        <label for="chkRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente" >Atualizar automaticamente.</label>&nbsp; &nbsp;
        <label  id="lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_AtualizarAutomaticamente_ContagemRegressiva" style="width: 60%; "></label>
        <label  id="lblRisco_Monitoramento_LucrosPrejuizos_Detalhes_Portas_Operadas" style="float:right; left:800px; margin-right:30px" ></label>
        <input  id="chkRisco_Monitoramento_LP_Carregar_SemCarteira" type="checkbox"  onclick="return chkRisco_Monitoramento_LP_Carregar_SemCarteira_Click(this);"  />
        <label for="chkRisco_Monitoramento_LP_Carregar_SemCarteira" >Agrupar por Papel</label>&nbsp; &nbsp;
        
    </h3>

        <!--Header da Div-->
        <div id="pnlMonitoramentoRiscoGeral_DetalhesHeader" >
            <%--<span>Detalhamento do Monitor de Risco</span>--%>
            <div id="DetalhesHeader_Cliente"></div>
            <%--<button class="IconButton Cancelar"  style="float: right" onclick="return GradIntra_MonitoramentoRisco_Detalhamento_Fechar();"></button>--%>
        </div>
        
        <div style="position:absolute; left: 500px; top:22px;">
            <label for="txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente" style="color:White">Cód. Cliente:</label>
            <input id="txtMonitoramentoRisco_BuscaDetalhesCliente_CodigoCliente" type="text" maxlength="10" style="width: 18.5em;" class="ProibirLetras" onkeydown="return GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca(this)">
        </div>

        <!-- Dados de Custódia -->
        <div id="pnlMonitoramentoRiscoGeral_Custodia" style=" position:relative; height: auto; text-align:center">

            <%--Dados de Custodia a vista--%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Avista" >
                <h4>Custódia A vista</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Avista"></table>
                <div class="SubTotal" >
                    <div class="tdLabel">Subtotal (Mercado à Vista):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaAvista"></div>
                </div>
                <br /><br />
            </div>
            
            <%--Dados de Custodia Opções--%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Opcoes">
                <h4>Custódia Opções</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Opcoes"></table>
                <%--<div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Opcoes"></div>--%>
                

                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Mercado de Opções):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaOpcoes"></div>
                </div>
                <br /><br />
            </div>
            

            <%--Dados de Custodia Termo--%>
            <%--<div id="pnlMonitoramentoRiscoGeral_Custodia_Termo">
                <h4>Custódia Termo</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Termo"></table>
                <%--<div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Termo"></div>

                <div class="SubTotal">
                    <div class="tdLabel">Subtotal (Mercado de Termo):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaTermo"></div>
                </div>
                <br /><br />
            </div>--%>
            

            <%--Dados de Custodia Bmf--%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Bmf">
                <h4>Custódia BMF</h4>

                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Bmf"></table>

                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Mercado Futuro):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaBmf"></div>
                </div>
                <br /><br />
            </div>
            
            <%--Dados de Custodia Posicao Bmf--%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Posicao_Bmf">
                <h4>Custódia Posicao Dia BMF</h4>


                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Posicao_Bmf"></table>

                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Mercado Futuro):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaPosicaoBmf"></div>
                </div>
                <br /><br />
            </div>


            <%--Dados de Custodia --%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Tesouro">
                <h4>Custódia Tesouro Direto</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Tesouro"></table>
                <div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Tesouro"></div>
                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Tesouro Direto):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaTesouro"></div>
                </div>
                <br /><br />
            </div>
            
            <%--Dados de Custodia --%>
            <%--<div id="pnlMonitoramentoRiscoGeral_Custodia_BTC">
                <h4>Custódia BTC</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_BTC"></table>
                <div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_BTC"></div>
                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (BTC):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaBTC"></div>
                </div>
                <br /><br />
            </div>--%>

            <%--Dados de Custodia de Termo --%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Operacoes_Termo">
                <h4>Custódia de operações de Termo</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_Termo"></table>
                <div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Operacoes_Termo"></div>
                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Operações Termo):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaOperacoesTermo"></div>
                </div>
                <br /><br />
            </div>

            <%--Dados de Custodia de BTC --%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Operacoes_BTC">
                <h4>Custódia de operações de BTC</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Operacoes_BTC"></table>
                <div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Operacoes_BTC"></div>
                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Operações BTC):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaOperacoesBTC"></div>
                </div>
                <br /><br />
            </div>

            <%--Dados de Renda Fixa --%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_RendaFixa">
                <h4>Custódia de Renda Fixa</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_RendaFixa"></table>
                <div id="tblBusca_MonitoramentoRisco_Resultados_Pager_Custodia_RendaFixa"></div>
                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Renda Fixa):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaRendaFixa"></div>
                </div>
                <br /><br />
            </div>

            <%--Dados de Custodia de Fundos Clubes --%>
            <div id="pnlMonitoramentoRiscoGeral_Custodia_Fundos">
                <h4>Custódia de Fundos/Clubes</h4>
                <table id="tblBusca_MonitoramentoRisco_Resultados_Custodia_Fundos"></table>
                <div id="pnlBusca_MonitoramentoRisco_Resultados_Pager_Custodia_Fundos"></div>
                <div class="SubTotal">
                    <div class="tdLabel" >Subtotal (Fundos/Clubes):</div>
                    <div class="ValorNumericoContaCorrente" id="SubtotalCustodiaFundos"></div>
                </div>
                <br /><br />
            </div>

            <!-- Dados de Saldos e Limites-->
            <div id="pnlMonitoramentoRiscoGeral_SaldosLimites">
                <h4>Conta Corrente/Limites</h4>
                <table id="tblSaldoDeContaSaldosLimites" style="width:1100px;" align="center">
                    <thead>
                    <tr>
                        <td colspan="11">Saldos/Bovespa</td>
                        <td style="width: 20px; border:none">&nbsp;</td>
                        <td colspan="3">BM&F</td>
                        <td style="width: 20px; border:none">&nbsp;</td>
                    </tr>
                    </thead>
                    <tr>
                        
                        <td class="tdLabel">D0</td>
                        <td class="tdLabel">D1</td>
                        <td class="tdLabel">D2</td>
                        <td class="tdLabel">D3</td>
                        <td class="tdLabel">Total</td>
                        
                        <td style="width: 20px; border:none">&nbsp;</td>

                        <td class="tdLabel">Conta Margem</td>
                        <td class="tdLabel">Projetado</td>
                        <td class="tdLabel">Garantias BOV.</td>
                        <td class="tdLabel">Margem Requerida BOV.</td>
                        <td class="tdLabel">Disponível</td>

                        <td style="width: 20px; border:none">&nbsp;</td>
                        
                        <td class="tdLabel">Garantias BMF</td>
                        <td class="tdLabel">Margem Requerida</td>
                        <td class="tdLabel">Disponível</td>
                        
                        <td style="width: 20px; border:none">&nbsp;</td>

                    </tr>
                    <tr>
                        <td class="ValorNumericoContaCorrente" id="tdConta_SaldoD0"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_SaldoD1"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_SaldoD2"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_SaldoD3"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_SaldoDTotal"></td>
                        
                        <td style="width: 20px; border:none">&nbsp;</td>
                        
                        <td class="ValorNumericoContaCorrente" id="tdConta_ContaMargem"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Projetado"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BOV_Garantias"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BOV_MargemRequerida"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BOV_Disponivel"></td>

                        <td style="width: 20px; border:none">&nbsp;</td>
                        
                        <td class="ValorNumericoContaCorrente" id="tdConta_BMF_Garantias"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BMF_MargemRequerida"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BMF_Disponivel"></td>
                        
                        <td style="width: 20px; border:none">&nbsp;</td>
                    </tr>
                    </table>

                    <table>
                    <thead>
                    <tr>
                        <td>Financeiro</td>
                    </tr>
                    </thead>

                    <tr>
                        <td class="tdLabel">(SFP) Situação Financeira Patrimonial</td>
                    </tr>
                    <tr>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Financeiro_SFP"></td>
                    </tr>
                </table>
                <br />
                <table id="tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BOV" style="width:1000px; display:none" align="center">
                <thead>
                    <td colspan="8">Garantias Bovespa</td>
                </thead>
                <tbody>

                </tbody>
                </table>
                <br />
                <table id="tblBusca_MonitoramentoRisco_Resultados_Financeiro_Garantias_BMF" style="width:1000px; display:none" align="center">
                <thead>
                    <td colspan="2">Garantias BM&F</td>
                </thead>
                <tbody>

                </tbody>
                </table>
                <br />

                <table id="tblBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao" style="width:1000px; display:none" align="center">
                <thead>
                    <td colspan="4">Extrato Liquidação Dia Corrente</td>
                    <td>Saldo Anterior</td>
                    <td style="text-align: right" id="tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Saldo_Anterior"></td>
                </thead>
                <tbody>
                

                </tbody>
                <tfoot>
                <td  align="left">Disponível</td>
                <td  align="right" id="tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Disponivel" ></td>
                <td colspan="2"></td>
                <td  align="left">Total Cliente:</td>
                <td  align="right" id="tdBusca_MonitoramentoRisco_Resultados_Financeiro_Extrato_Liquidacao_Total_Cliente" ></td>
                </tfoot>
                </table>
                <br />


                <table id="tblBusca_MonitoramentoRisco_Resultados_ContaCorrente" style="width:1000px" align="center">
                    <thead>
                    <tr>
                        <td colspan="5">Limites</td>
                    </tr>
                    </thead>
                    <tr>
                        <td class="tdLabel">Lim. Compra a Vista</td>
                        <td class="tdLabel">Lim.Compra Opção</td>
                        <td class="tdLabel">Lim. Venda a vista</td>
                        <td class="tdLabel">Lim. Venda Opção</td>
                        <td class="tdLabel">Lim. Disponível</td>
                    </tr>
                    <tr>
                        <td class="ValorNumericoContaCorrente" id="tdConta_CompraAvista"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_CompraOpcao"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_VendaAvista"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_VendaOpcao"></td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Disponivel"></td>
                    </tr>
                </table>
                <br />
                <br />
            </div>



            <br /><br />
            <!--Dados do serviço do  Risco-->
            <div id="pnlMonitoramentoRiscoGeral_DetalhesRisco">
                <h4>Operações Intraday Bovespa</h4>
                <div id="pnlRisco_Monitoramento_LucrosPrejuizos_Gerenciamento_GridDetalhes">
                    
                    <a onclick="return btnRisco_Monitoramento_LucroPrejuizo_ColunasGrid_Detalhes_ExpanderColapse_Click(this)" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Detalhes_Expander" class="Risco_MonitoramentoLucroPrejuizo_Detalhes_ExpandirColapsar_ColunasGrid_Colapsado" >Selecionar Colunas</a>&nbsp;&nbsp;
                </div>
                <div id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Detalhes">
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Cliente"            onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="Cliente"            checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Cliente"            >Cliente            </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Cotação"            onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="Cotação"            checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Cotação"            >Cotação            </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_FinanceiroAbertura" onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="FinanceiroAbertura" checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_FinanceiroAbertura" >Financeiro Abertura </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_FinanceiroComprado" onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="FinanceiroComprado" checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_FinanceiroComprado" >Financeiro Comprado </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_FinanceiroVendido"  onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="FinanceiroVendido"  checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_FinanceiroVendido"  >Financeiro Vendido  </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Instrumento"        onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="Instrumento"        checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Instrumento"        >Instrumento         </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizo"      onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="LucroPrejuizo"      checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizo"      >Lucro Prejuizo      </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NetOperacao"        onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="NetOperacao"        checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NetOperacao"        >Net Operacao        </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PrecoReversao"      onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="PrecoReversao"      checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PrecoReversao"      >Preço Reversão      </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeAbertura"       onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="QtdeAbertura"       checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeAbertura"       >Qtde. Abertura       </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeAtual"          onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="QtdeAtual"          checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeAtual"          >Qtde. Atual          </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeComprada"       onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="QtdeComprada"       checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeComprada"       >Qtde. Comprada       </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeVendida"        onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="QtdeVendida"        checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeVendida"        >Qtde. Vendida        </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeReversao"       onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="QtdeReversao"       checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_QtdeReversao"       >Qtde. Reversão       </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_TipoMercado"        onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="TipoMercado"        checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_TipoMercado"        >Tipo Mercado        </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VLMercadoCompra"    onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="VLMercadoCompra"    checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VLMercadoCompra"    >VL. Mercado Compra  </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VLMercadoVenda"     onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="VLMercadoVenda"     checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VLMercadoVenda"     >VL. Mercado Venda   </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VLNegocioVenda"     onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="VLNegocioVenda"     checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VLNegocioVenda"     >VL. Negocio Venda   </label><br/>
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SubtotalCompra"     onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="SubtotalCompra"     checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SubtotalCompra"     >Subtotal (compra)   </label><br />
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SubtotalVenda"      onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="SubtotalVenda"      checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SubtotalVenda"      >Subtotal (venda)    </label><br />
                        <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Portas"             onclick="return plnRisco_Monitoramento_LucrosPrejuizos_Detalhes_ColunasGrid_Click(this)" value="Portas"             checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Portas"             >Portas              </label><br />
                    </div>
            </div>
            <div style="margin-bottom:12.0em; position:absolute; width: 100%" id="pnlMonitoramentoRiscoGeral_Custodia_Detalhamento" >
                <div id="pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bovespa">
                    <table id="tblBusca_MonitoramentoRisco_Resultados_detalhes"></table>
                    <table id="tbTotalLucroPrejuizoBovespa">
                        <tr>

                            <td >Total Lucro Prejuízo</td>
                            <td id="tdTotalLucroPrejuizoBov"></td>
                        </tr>
                        <%--<tr>
                            <td >Quantidade Contratos</td>
                            <td id="tdQuantidadeContratosBov"></td>
                        </tr>--%>
                        <%--<tr>
                            <td >Preço Médio</td>
                            <td id="tdPrecoMedioBov"></td>
                        </tr>--%>
                    </table>
                </div>
                <div id="pnlMonitoramentoRiscoGeral_Custodia_Detalhamento_Bmf">
                    <h4>Operações Intraday BMF</h4>
                    <table id="tblBusca_MonitoramentoRisco_Resultados_detalhes_Bmf"></table>
                    <table   id="tbTotalLucroPrejuizoBmf">
                        <tr>
                            <td>Total Lucro Prejuízo</td>
                            <td id="tdTotalLucroPrejuizoBmf"></td>
                        </tr>
                        <%--<tr>
                            <td >Quantidade Contratos</td>
                            <td id="tdQuantidadeContratosBmf"></td>
                        </tr>--%>
                        <%--<tr>
                            <td >Preço Médio</td>
                            <td id="tdPrecoMedioBmf"></td>
                        </tr>--%>
                    </table>
                </div>
            </div>      
        </div>
        <!--Server IP: <%=Request.ServerVariables["LOCAL_ADDR"] %>-->
    </form>
</body>
</html>

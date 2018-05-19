<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoramentoRiscoGeral.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.MonitoramentoLucrosPrejuizos" %>

<html xmlns="http://www.w3.org/1999/xhtml">
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
        <%--.ui-jqgrid tr.jqgrow ui-row-ltr {font-size:1.5em}--%>
        
        .ui-jqgrid-htable {font-size: 1.1em}
        .ui-jqgrid-pager{font-size: 1.1em}
        .ui-pg-table {font-size: 1.1em}
        .ui-jqgrid-hbox {font-size: 1.1em}
        .ui-jqgrid-hdiv {font-size: 1.1em}
        .ui-widget .ui-widget {font-size: 0.4em}
        .ui-jqgrid-title{font-size: 1.0em}
        .ui-state-highlight { background: #FDFFA5 !important; }
        
        
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

    <h1 style="width: 100%; height: 3em;"><span>Header Intranet Gradual</span></h1>
        
    <h3 style="margin: 10px 0pt 15px 25px; height: 15px">- Monitoramento de Lucros e Prejuízos - <div id="pnlNomePagina" style="display:inline-block"></div></h3>
    
    <input type="hidden" id="hddRisco_Monitoramento_LucrosPrejuizos_Linhas_Selecionadas" />

    <div id="pnlRisco_Monitoramento_Risco_LucroPrejuizo_Pesquisa" class="Busca_Formulario" style="width: 90%; height: 3em; margin-left:10px">
        <div style="margin-top:7px;" >
            <input  id="chkRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente" type="checkbox" onclick="return chkRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente_Click(this);" checked="checked" />
            <label for="chkRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente">Atualizar automaticamente.</label>&nbsp;&nbsp;
            <label for="txtRisco_Monitoramento_LucrosPrejuizos_NomeJanela">Nome da Janela:</label>
            <input  id="txtRisco_Monitoramento_LucrosPrejuizos_NomeJanela" type="text" maxlength="70"  Propriedade="NomeJanela" style="width: 18.5em;" />&nbsp;&nbsp;
            <a onclick="return btnRisco_Monitoramento_LucroPrejuizo_ColunasGrid_ExpanderColapse_Click(this)" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Expander" class="Risco_MonitoramentoLucroPrejuizo_ExpandirColapsar_ColunasGrid_Colapsado">Selecionar Colunas</a>&nbsp;&nbsp;
            <button class="btnBusca" onclick="return btnRisco_Monitoramento_LucroPrejuizo_SalvarJanela_Click(this)" id="btnRisco_Monitoramento_LucroPrejuizo_SalvarJanela">Salvar</button>
            <label  id="lblRisco_Monitoramento_LucrosPrejuizos_AtualizarAutomaticamente_ContagemRegressiva" style="width: 25%; float:right"></label>
        </div>
        <div id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid">
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NmCliente"                   onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="NmCliente"                   checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NmCliente"                  >Nome Cliente                  </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Assessor"                    onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="Assessor"                    checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Assessor"                   >Assessor                      </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NomeAssessor"                onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="NomeAssessor"                checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NomeAssessor"               >Nome Assessor                 </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_ContaCorrenteAbertura"       onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="ContaCorrenteAbertura"       checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_ContaCorrenteAbertura"      >C.C.Abertura                  </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_CustodiaAbertura"            onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="CustodiaAbertura"            checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_CustodiaAbertura"           >Custodia Abertura             </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_DtAtualizacao"               onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="DtAtualizacao"               checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_DtAtualizacao"              >DtAtualizacao                 </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizoBMF"            onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LucroPrejuizoBMF"            checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizoBMF"           >Lucro Prejuizo BMF            </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizoBOVESPA"        onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LucroPrejuizoBOVESPA"        checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizoBOVESPA"       >Lucro Prejuizo BOVESPA        </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizoTOTAL"          onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LucroPrejuizoTOTAL"          checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LucroPrejuizoTOTAL"         >Lucro Prejuizo TOTAL          </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NetOperacoes"                onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="NetOperacoes"                checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_NetOperacoes"               >Net Operacoes                 </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PatrimonioLiquidoTempoReal"  onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="PatrimonioLiquidoTempoReal"  checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PatrimonioLiquidoTempoReal" >Patrimonio Liquido Tempo Real </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PLAberturaBMF"               onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="PLAberturaBMF"               checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PLAberturaBMF"              >PL Abertura BMF               </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PLAberturaBovespa"           onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="PLAberturaBovespa"           checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_PLAberturaBovespa"          >PL Abertura Bovespa           </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SaldoBMF"                    onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="SaldoBMF"                    checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SaldoBMF"                   >Saldo BMF                     </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SaldoContaMargem"            onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="SaldoContaMargem"            checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_SaldoContaMargem"           >Saldo ContaMargem             </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_TotalContaCorrenteTempoReal" onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="TotalContaCorrenteTempoReal" checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_TotalContaCorrenteTempoReal">Total C. C. Tempo Real        </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_TotalGarantias"              onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="TotalGarantias"              checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_TotalGarantias"             >Total Garantias               </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteAVista"                onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LimiteAVista"                checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteAVista"               >Limite A Vista                </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteDisponivel"            onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LimiteDisponivel"            checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteDisponivel"           >Limite Disponível             </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteOpcoes"                onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LimiteOpcoes"                checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteOpcoes"               >Limite Opções                 </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteTotal"                 onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="LimiteTotal"                 checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_LimiteTotal"                >Limite Total                  </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Prejuizo"                    onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="Prejuizo"                    checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Prejuizo"                   >Prejuízo                      </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VolumeTotalBmf"              onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="VolumeTotalFinaceiroBmf"     checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VolumeTotalBmf"             >Volume Total Bmf              </label><br/>
            <input type="checkbox" id="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VolumeTotalBov"              onclick="return plnRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_Click(this)" value="VolumeTotalFinaceiroBov"     checked="checked" /><label for="pnlRisco_Monitoramento_LucrosPrejuizos_ColunasGrid_VolumeTotalBov"             >Volume Total Bovespa          </label><br/>
           
        </div>
    </div>
    
    <%--margin: 12.5em 0pt 0pt 0px;--%>
    <div style="margin-top:6.0em;  position:absolute; background-color: White; width: 100%" >
        <div id="div_Risco_MonitoramentoRisco_Resultados" style="margin-top: 1em; margin-left: 1em; margin-bottom: 2em; display: none; ">
            <table id="tblBusca_MonitoramentoRisco_Resultados"></table>
        </div>
        <table style="width: 100%">
            <tr>
                <td style="width:500px">&nbsp;</td>
                <td style="font-style:italic" >Volume Total Bovespa:</td>
                <td id="div_Risco_MonitoramentoRisco_Resultados_Volume_Total_Bovespa"></td>
                <td style="font-style:italic">Volume Total BM&F:</td>
                <td id="div_Risco_MonitoramentoRisco_Resultados_Volume_Total_Bmf"></td>
            </tr>
        </table>
    </div>
    <script  type="text/javascript">

        $(function () {
            $("#div_MonitoramentoRiscoGeral_Detalhes").draggable(
            {
                //containment: '#div_MonitoramentoRiscoGeral_Detalhes',
                cursor      : 'move',
                zIndex      : 20000,
                handle: 'div#pnlMonitoramentoRiscoGeral_DetalhesHeader'
            });
        });

        GradIntra_HabilitarInputsComMascaras($("#pnlRisco_Monitoramento_Risco_LucroPrejuizo_Pesquisa"));

    </script>
    </form>
</body>
</html>

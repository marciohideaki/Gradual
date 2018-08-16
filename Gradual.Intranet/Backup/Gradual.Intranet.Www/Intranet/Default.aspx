<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Default" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title id="TitSistemaDeAdministracaoDeCadastro">Sistema de Administração de Cadastro</title>

    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <!--[if IE]><script src="../Js/Lib/Etc/Dev/excanvas.js"></script><![endif]-->

    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery-1.7.1.min.js?v=<%= this.VersaoDoSite %>"></script>--%>
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery-3.2.1.min.js?v=<%= this.VersaoDoSite %>"></script>
    
    <!-- jQuery UI master: -->
    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery-ui-1.8rc3.custom.js?v=<%= this.VersaoDoSite %>"></script> --%>
    <script src="../Js/Lib/jQuery/Dev/ui/jquery-ui.min.js?v=<%= this.VersaoDoSite %>" type="text/javascript"></script>

    <!-- jQuery UI arquivos de core que os efeitos e widgets precisam: -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.core.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.effects.core.js?v=<%= this.VersaoDoSite %>"></script>

    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.widget.js?v=<%= this.VersaoDoSite %>"></script>
    
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.autocomplete.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.position.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.mouse.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- jQuery UI arquivos dos efeitos e widgets que nós utilizamos: -->

    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.datepicker.js?v=<%= this.VersaoDoSite %>"></script>--%> 
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.datepicker-pt-BR.js?v=<%= this.VersaoDoSite %>"></script>

    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/ui/jquery.ui.draggable.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- jQuery arquivos de outros plugins que nós utilizamos: -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.cookie.js?v=<%= this.VersaoDoSite %>"></script>                  <!-- Suporte a cookies -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.format-1.0.js?v=<%= this.VersaoDoSite %>"></script>              <!-- Formatação de números -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.json-2.2.js?v=<%= this.VersaoDoSite %>"></script>                <!-- Serialização JSON -->
    
    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.validationEngine.js?v=<%= this.VersaoDoSite %>"></script>--%>        <!-- Validação -->
    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.validationEngine-ptBR.js?v=<%= this.VersaoDoSite %>"></script>--%>   <!-- Mensagens de Validação -->
    <script src="../Js/Lib/jQuery/Dev/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Js/Lib/jQuery/Dev/jquery.validationEngine-ptBR.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Js/Lib/jQuery/Dev/jquery.validationEngine-ptBR.default.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.customInput.js?v=<%= this.VersaoDoSite %>"></script>             <!-- Checks e Radios customizados -->
    
    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.maskedinput-1.2.2.js?v=<%= this.VersaoDoSite %>"></script>--%>       <!-- Máscara para inputs -->
    <script src="../Js/Lib/jQuery/Dev/jquery.maskedinput.min.js?v=<%= this.VersaoDoSite %>" type="text/javascript"></script>


    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.bt.js?v=<%= this.VersaoDoSite %>"></script>                      <!-- Tooltips customizados -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.ajaxfileupload.js?v=<%= this.VersaoDoSite %>"></script>          <!-- Input de upload ajax -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.password.js?v=<%= this.VersaoDoSite %>"></script>                <!-- Complexidade de Senha -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.fileupload.js?v=<%= this.VersaoDoSite %>"></script>              <!-- Upload ajax de arquivos -->
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jquery.fileupload.js?v=<%= this.VersaoDoSite %>"></script>              <!-- Upload ajax de arquivos -->
    <!-- jqGrid -->

    <%--<script type="text/javascript" src="../Js/Lib/jQuery/Dev/jqgrid/i18n/grid.locale-pt-br.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jqgrid/jquery.jqGrid.js?v=<%= this.VersaoDoSite %>"></script>--%>
    <script src="../Js/Lib/jQuery/Dev/jqgrid/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../Js/Lib/jQuery/Dev/jqgrid/i18n/grid.locale-pt-br.js" type="text/javascript"></script>

    <!-- Biblioteca de gráfico em canvas -->
    <script type="text/javascript" src="../Js/Lib/Etc/Dev/raphael.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/Etc/Dev/g.raphael-min.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/Etc/Dev/g.pie.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Bibliotecas da Gradual: -->
    <script type="text/javascript" src="../Js/Lib/Gradual/Dev/00-GradAuxiliares.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/Gradual/Dev/01-GradSettings.js?v=<%= this.VersaoDoSite %>"></script>

    <!-- Javascripts de Páginas (Janelas, etc): -->

    <!--script type="text/javascript" src="Js/Pages/Dev/01-AcompanhamentoDeOrdem.aspx.js"></script-->

    <script type="text/javascript" src="../Js/Paginas/Dev/01-GradIntra-Principal.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/02-GradIntra-Navegacao.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/03-GradIntra-Busca.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/04-GradIntra-Cadastro.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/05-GradIntra-Relatorio.js?v=<%= this.VersaoDoSite %>"></script>
    
    <script type="text/javascript" src="../Js/Paginas/Dev/11-Clientes.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/12-Risco.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/14-Seguranca.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/15-Monitoramento.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/16-Sistema.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/17-Relatorios.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/18-HomeBroker.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/19-Solicitacoes.js?v=<%= this.VersaoDoSite %>"></script>
    
    <script type="text/javascript" src="../Js/Paginas/Dev/42-Default.aspx.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/43-PoupeOperacoes.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/44-Compliance.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jqgrid/grid.celledit.js"></script>            
    <script type="text/javascript" src="../Js/Lib/jQuery/Dev/jqgrid/grid.inlinedit.js"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/45-TED.js?v=<%= this.VersaoDoSite %>"></script>
    <script type="text/javascript" src="../Js/Paginas/Dev/46-IntegracaoRocket.js?v=<%= this.VersaoDoSite %>"  ></script>

    <link rel="Stylesheet" media="all" href="../Skin/<%= this.SkinEmUso %>/Principal.css?v=<%= this.VersaoDoSite %>" />

    <link rel="Stylesheet" media="screen" href="../Skin/<%= this.SkinEmUso %>/ValidationEngine.jquery.css?v=<%= this.VersaoDoSite %>" />

    <link rel="Stylesheet" media="screen" href="../Skin/<%= this.SkinEmUso %>/Intranet/Default.aspx.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" media="all" href="../Skin/<%= this.SkinEmUso %>/Intranet/Clientes.css?v=<%= this.VersaoDoSite %>" />

    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" media="print"  href="../Skin/<%= this.SkinEmUso %>/Relatorio.print.css?v=<%= this.VersaoDoSite %>" />

    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/<%= this.SkinEmUso %>/gradintra-theme/jquery-ui-1.8.1.custom.css?v=<%= this.VersaoDoSite %>" />
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/<%= this.SkinEmUso %>/ui.jqgrid.css?v=<%= this.VersaoDoSite %>" />

    <!-- Folhas de estilo -->
    <link href="../Skin/Default/jquery-ui.css?v=<%= this.VersaoDoSite %>" rel="stylesheet"                 type="text/css" />
    <link href="../Skin/Default/dataTables.jqueryui.min.css?v=<%= this.VersaoDoSite %>" rel="stylesheet"   type="text/css" />
    <link href="../Skin/Default/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    

    <!-- Scripts de terceiross -->
    
<%--    <script src="../../../../../Js/Lib/jQuery/Dev/jquery.dataTables.min.js?v=<%= this.VersaoDoSite %>"     type="text/javascript"></script>--%>
    <script src="../Js/Lib/jQuery/Dev/dataTables.jqueryui.min.js" type="text/javascript"></script>
    <!--[if IE]>
    <link rel="Stylesheet" media="screen" href="../Skin/<%= this.SkinEmUso %>/IE.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" media="print"  href="../Skin/<%= this.SkinEmUso %>/IE.print.css?v=<%= this.VersaoDoSite %>" />
    <![endif]-->
    <script src="../Js/Lib/Etc/Dev/moment-with-locales.js?v=<%= this.VersaoDoSite %>" type="text/javascript"></script>
    
    <script src="../Js/Lib/jQuery/Dev/dataTables.bootstrap.min.js?v=<%= this.VersaoDoSite %>" type="text/javascript"></script>
    <%--<link href="../Skin/<%= this.SkinEmUso %>/dataTables.bootstrap.min.css" rel="stylesheet" type="text/css" />--%>
</head>
<!--   onbeforeunload="onBeforUnload_Logout()" onunload="onCloseEfetuarLogout()"-->
<body onload="PageLoad()" onbeforeunload="return onBeforUnload_Logout()">

<form id="form1" runat="server">

    <h1><span>Sistema de Administração Cadastral</span></h1>
    
    <h2><span><a id="lnkUsuario_logout" href="../Login.aspx?Acao=Logout" title="Efetuar Logout"></a></span></h2>
    <%--<%= this.UsuarioLogado.Nome %>--%>
    <ul id="pnlMenuPrincipal">
        <asp:Repeater runat="server" id="rptMenuPrincipal">
            <ItemTemplate>
                <li>
                    <a href="#" onclick='return <%# Eval("Tag").ToString().Split(';')[0] %>' rel='<%# Eval("Tag").ToString().Split(';')[1] %>'><%# Eval("Nome") %></a>

                    <div class="pnlMenuScroller">
                    <button class="btnMenuScroller btnMenuScroller_Visivel" onmouseover="return btnMenuScrollerEsquerdo_MouseOver(this)"><span>Rolar Menu</span></button>
                    <button class="btnMenuScroller btnMenuScroller_Direito btnMenuScroller_Visivel" onmouseover="return btnMenuScrollerDireito_MouseOver(this)"><span>Rolar Menu</span></button>

                    <ul class="<%# Eval("Tag").ToString().Split(';')[2] %>">
                        <asp:Repeater runat="server" id="rptMenuFilho" DataSource='<%# Eval("Filhos") %>' >
                            <ItemTemplate>
                                <li><a href="#" onclick='return <%# Eval("Tag").ToString().Split(';')[0] %>' rel='<%# Eval("Tag").ToString().Split(';')[1] %>'><%# Eval("Nome") %></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    
    <%--<div id="pnlFerramentas" runat="server">
        <span style="cursor:pointer;" onclick="return GradIntra_AbrePop_CotacaoRapida();">Cotação Rápida</span>
    </div>--%>

    <div id="pnlMensagem" class="BotaoEscondido">
        <button id="btnMensagem_Fechar" onclick="return GradIntra_RetornarMensagemAoEstadoNormal()"></button>

        <span><%--Bem-Vindo, <%= this.UsuarioLogado.Nome %>--%></span>
    </div>

    <div id="pnlMensagemAdicional" style="display:none">

        <textarea readonly="readonly"></textarea>
        <a href="#" onclick="return lnlMensagemAdicional_Fechar_Click(this)">fechar</a>

    </div>

    <div id="pnlConteudo">

        <div id="pnlBusca_Clientes_Busca"                     class="Busca_Container"></div>   <!-- Sistema_SubSistema pra aparecer busca -->
        <div id="pnlBusca_Clientes_Relatorios"                class="Busca_Container"></div>
        <div id="pnlBusca_Clientes_ReservaIPO"                class="Busca_Container"></div>

        <div id="pnlBusca_Risco_Busca"                        class="Busca_Container"></div>
        <div id="pnlBusca_Risco_GruposDeRisco"                class="Busca_Container"></div>
        <div id="pnlBusca_Risco_Relatorios"                   class="Busca_Container"></div>
        <div id="pnlBusca_Risco_MonitoramentoRiscoGeral"      class="Busca_Container"></div>
        <div id="pnlBusca_Risco_SuitabilityLavagem"           class="Busca_Container"></div>
        <div id="pnlBusca_Risco_MonitoramentoIntradiario"     class="Busca_Container"></div>
        <div id="pnlBusca_Risco_MonitoramentoCustodia"        class="Busca_Container"></div>

        <div id="pnlBusca_Compliance_EstatisticaDayTrade"     class="Busca_Container"></div>
        <div id="pnlBusca_Compliance_OrdensAlteradasDayTrade" class="Busca_Container"></div>
        <div id="pnlBusca_Compliance_NegociosDiretos"         class="Busca_Container"></div>
        <div id="pnlBusca_Compliance_Churning"                class="Busca_Container"></div>
                                                            
        <div id="pnlBusca_Seguranca_Busca"                    class="Busca_Container"></div>
        

        <div id="pnlBusca_Monitoramento_Ordens"               class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_OrdensNovoOMS"        class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_OrdensSpider"         class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_Termos"               class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_Relatorios"           class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_OrdensStop"           class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_UsuariosLogados"      class="Busca_Container"></div>
        <div id="pnlBusca_Monitoramento_DesbloqueioCustodia"  class="Busca_Container"></div>

        <div id="pnlBusca_Sistema_ObjetosDoSistema"           class="Busca_Container"></div>
        <div id="pnlBusca_Sistema_ValidacaoCadastralRocket"   class="Busca_Container"></div>
        <div id="pnlBusca_Sistema_Migracao"                   class="Busca_Container"></div>
        <div id="pnlBusca_Sistema_Relatorios"                 class="Busca_Container"></div>

        <div id="pnlBusca_Relatorios_Gerais"                  class="Busca_Container"></div>

        <div id="pnlBusca_DBM_RelatoriosDBM"                  class="Busca_Container"></div>
        
        <div id="pnlBusca_Solicitacoes_PoupeDirect"           class="Busca_Container"></div>
        <div id="pnlBusca_Solicitacoes_PoupeOperacoes"        class="Busca_Container"></div>
        <%--<div id="pnlBusca_Solicitacoes_GerenciamentoIPO"      class="Busca_Container"></div>--%>
        <div id="pnlBusca_Solicitacoes_VendasDeFerramentas"   class="Busca_Container"></div>

        <!-- Painéis de Conteúdo do Sistema "Cliente": -->

        <div id="pnlConteudo_Clientes_Busca" class="Conteudo_Container">
            <!-- aqui entra o default.aspx do cliente -->
        </div>
        
        <div id="pnlConteudo_Clientes_Relatorios"         class="Conteudo_Container">
            <div class="pnlRelatorio" id="pnlRelatorio_Geral" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlClientes_Relatorios_Resultados" style="display:none"></div>
            </div>
        </div>

        <div id="pnlConteudo_Clientes_ReservaIPO"         class="Conteudo_Container">
            <div class="pnlFormulario pnlFormularioExtendido" style="">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlClientes_Reserva_IPO_Resultados" style="display:none">
                    <table id="tblBusca_ReservaIPO_Resultados"></table>
                    <div id="pnlBusca_ReservaIPO_Resultados_Pager"></div>
                </div>
            </div>
        </div>

        <div id="pnlConteudo_Clientes_LinkProspect"        class="Conteudo_Container">
              <div class="pnlFormulario pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlClientes_LinkProspect" style="display:none"></div>
              </div>
         </div>

        <div id="pnlConteudo_Clientes_IntegracaoRocket"        class="Conteudo_Container">
            <div id="pnlClientes_IntegracaoRocket" style="display:none"></div>
         </div>

         <!-- Painéis de Conteúdo do Sistema "Risco" -->

        <div id="pnlConteudo_Risco_Busca" class="Conteudo_Container">
            <!-- aqui entra o default.aspx do risco -->
        </div>

        <div id="pnlConteudo_Risco_GruposDeRisco" class="Conteudo_Container">
            <!-- aqui entra o default.aspx do risco -->
        </div>

        <div id="pnlConteudo_Risco_GruposDeRiscoRestricoes" class="Conteudo_Container">
            <!-- aqui entra o default.aspx do risco -->
        </div>

        <div id="pnlConteudo_Risco_GruposDeRiscoRestricoesSpider" class="Conteudo_Container">
            <!-- aqui entra o default.aspx do risco -->
        </div>

        <!--Painel de Risco-->
        <div id="pnlConteudo_Risco_MonitoramentoRiscoGeral" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <div id="pnlConteudo_Risco_MonitoramentoIntradiario" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <div id="pnlConteudo_Risco_SuitabilityLavagem" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <!--Painéis de Conteúdo do Sistema de Compliance-->
        <div id="pnlConteudo_Compliance_EstatisticaDayTrade" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <div id="pnlConteudo_Compliance_OrdensAlteradasDayTrade" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <div id="pnlConteudo_Compliance_NegociosDiretos" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <div id="pnlConteudo_Compliance_Churning" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                
            </div>
        </div>

        <div id="pnlConteudo_Risco_Relatorios" class="Conteudo_Container">
            <div class="pnlRelatorio" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlRisco_Relatorios_Resultados" style="display:none">
    
                </div>
    
            </div>
        </div>
        
        <!-- Painéis de Conteúdo do Sistema "Segurança" -->

        <div id="pnlConteudo_Seguranca_Busca" class="Conteudo_Container"></div>
        
        <div id="pnlConteudo_Seguranca_AlterarSenha" class="Conteudo_Container" >
            <div class="pnlFormulario" style="display:none;left:18%">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlSeguranca_Formularios_Dados_AlterarSenha" style="display:none"></div>
            </div>
        </div>


        <div id="pnlConteudo_Seguranca_ParametrosGlobais" class="Conteudo_Container" >
            <div class="pnlFormulario" style="display:none;left:18%">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlSeguranca_Formularios_Dados_ParametrosGlobais" style="display:none"></div>
            </div>
        </div>

        <!-- Painéis de Conteúdo do Sistema "Monitoramento" -->
        
        <div id="pnlConteudo_Monitoramento_Ordens" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlMonitoramento_ListaDeResultados" style="display:none">
                    <table id="tblBusca_Ordens_Resultados"></table>
                    <div id="pnlBusca_Ordens_Resultados_Pager"></div>
                    <div style="text-align:right"><button class="btnBusca" id="btnCancelarOrdens" onclick="return btnMonitoramento_ExcluirOrdensSelecionadas_Click(this)" style="width:230px; margin-right: 0px;">Cancelar Ordens Selecionadas</button></div>
                </div>
            </div>
        </div>
        
        <div id="pnlConteudo_Monitoramento_OrdensNovoOMS" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlMonitoramento_OrdenNovoOMS_ListaDeResultados" style="display:none">
                    <table id="tblBusca_OrdensNovoOMS_Resultados"></table>
                    <div id="pnlBusca_OrdensNovoOMS_Resultados_Pager"></div>
                    <div style="text-align:right"><button class="btnBusca" id="Button1" onclick="return btnMonitoramento_ExcluirOrdensNovoOMSSelecionadas_Click(this)" style="width:230px; margin-right: 0px;">Cancelar Ordens Selecionadas</button></div>
                </div>
            </div>
        </div>

        <div id="pnlConteudo_Monitoramento_OrdensSpider" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlMonitoramento_OrdenSpider_ListaDeResultados" style="display:none">
                    <table id="tblBusca_OrdensSpider_Resultados"></table>
                    <div id="pnlBusca_OrdensSpider_Resultados_Pager"></div>
                    <div style="text-align:right"><button class="btnBusca" id="Button2" onclick="return btnMonitoramento_ExcluirOrdensSpiderSelecionadas_Click(this)" style="width:230px; margin-right: 0px;">Cancelar Ordens Selecionadas</button></div>
                </div>
            </div>
        </div>

        <div id="pnlConteudo_Monitoramento_Termos" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlMonitoramento_Termos_ListaDeResultados" style="display:none">
                    
                    <!--table id="tblBusca_Ordens_Resultados"></table>
                    <div id="pnlBusca_Ordens_Resultados_Pager"></div>
                    <div style="text-align:right"><button class="btnBusca" id="btnCancelarOrdens" onclick="return btnMonitoramento_ExcluirOrdensSelecionadas_Click(this)" style="width:230px; margin-right: 0px;">Cancelar Ordens Selecionadas</button></div-->

                </div>
            </div>
        </div>
        
        <div id="pnlConteudo_Monitoramento_Relatorios" class="Conteudo_Container">
            <div class="pnlRelatorio" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlConteudo_Monitoramento_Relatorios_Resultados" style="display:none">
                    <table id="tblBusca_Monitoramento_Relatorios_Resultados"></table>
                    <div id="pnlConteudo_Monitoramento_Relatorios_Pager"></div>
                </div>
            </div>
        </div>

        <div id="pnlConteudo_Monitoramento_OrdensStop" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none; width: 1110px">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlMonitoramento_ListaDeResultados" style="display:none">
                    <table id="tblBusca_OrdensStop_Resultados"></table>
                    <div id="pnlBusca_OrdensStop_Resultados_Pager"></div>
                    <div style="text-align:right"><button class="btnBusca" id="btnCancelarOrdensStop" onclick="return btnMonitoramento_ExcluirOrdensStartStopSelecionados_Click(this)" style="width: 230px; margin-right: 20px">Cancelar Ordens Selecionados</button></div>
                </div>
            </div>
        </div>

       
       



        <div id="pnlConteudo_Monitoramento_UsuariosLogados" class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlConteudo_Monitoramento_UsuariosLogados_ListaDeResultados" style="display:none"></div>
            </div>
        </div>

        <div id="pnlConteudo_Monitoramento_DesbloqueioCustodia" class="Conteudo_Container">
            <div id="pnlFormularioDesbloqueio" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlConteudo_Monitoramento_DesbloqueioCustodia_ListaDeResultados" style="display:none"></div>
            </div>
        </div>

        <!-- Painéis de Conteúdo do Sistema "Admin Cadastro" -->

        <div id="pnlConteudo_Sistema_AutorizacoesDeCadastro" class="Conteudo_Container">
            <div id="pnlSistema_AutorizacoesDeCadastro"      class="pnlFormulario pnlFormularioExtendido" style="display:none; width:96%"></div>
        </div>

        <div id="pnlConteudo_Sistema_ObjetosDoSistema" class="Conteudo_Container">
            <div  class="pnlFormulario pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlSistema_ObjetosDoSistema" style="display:none"></div>
            </div>
        </div>

        <div id="pnlConteudo_Sistema_Migracao"           class="Conteudo_Container">
            <div class="pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlClientes_MigracaoEntreAssessores" style="display:none">
                </div>
            </div>
        </div>
        
        <div id="pnlConteudo_Sistema_Importacao"           class="Conteudo_Container">
            <div class="pnlFormulario pnlFormularioExtendido" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlClientes_Importacao" style="display:none"></div>
            </div>
        </div>

        <div id="pnlConteudo_Sistema_VariaveisDoSistema" class="Conteudo_Container">
            <div id="pnlSistema_VariaveisDoSistema"           class="pnlFormulario pnlFormularioExtendido" style="display:none;width:450px"></div>
        </div>

        <div id="pnlConteudo_Sistema_PessoasVinculadas" class="Conteudo_Container">
            <div id="pnlSistema_PessoasVinculadas"            class="pnlFormulario pnlFormularioExtendido" style="display:none;"></div>
        </div>

        <div id="pnlConteudo_Sistema_PessoasExpostasPoliticamente" class="Conteudo_Container">
            <div id="pnlSistema_PessoasExpostasPoliticamente" class="pnlFormulario pnlFormularioExtendido" style="display:none;"></div>
        </div>

        <div id="pnlConteudo_Sistema_ValidacaoCadastralRocket" class="Conteudo_Container">
            <div id="pnlSistema_ValidacaoCadastralRocket" class="pnlFormulario pnlFormularioExtendido" style="display:none;"></div>
        </div>

        <div id="pnlConteudo_Sistema_Relatorios" class="Conteudo_Container">
            <div class="pnlRelatorio" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlConteudo_Sistema_Relatorios_Resultados" style="display:none">
                    
                </div>
            </div>
        </div>
        
        <div id="pnlConteudo_DBM_RelatoriosDBM" class="Conteudo_Container">
            <div class="pnlRelatorio" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>

                <div id="pnlDBM_Relatorios_Resultados" style="display:none">
    
                </div>
    
            </div>
        </div>
        
        <!-- HomeBroker -->

        <div id="pnlConteudo_HomeBroker" class="Conteudo_Container">
            <div id="pnlHomeBroker_Avisos"        class="pnlFormulario pnlFormularioExtendido" style="display:none;"></div>
        </div>
        
        <!-- Relatorios -->

        <div id="pnlConteudo_Relatorios_Gerais" class="Conteudo_Container">
        <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
            <div class="pnlRelatorio" style="display:none">
                <span class="Aguarde">Carregando, por favor aguarde...</span>
                <div id="pnlRelatorios_Resultados" style="display: block; float: left; width: 98%;">
                    
                </div>
            </div>
        </div>

        <div id="pnlConteudo_Solicitacoes_VendasDeFerramentas" class="Conteudo_Container"></div>

    </div>
        
    <!-- SOLICITAÇÕES -->
    <div id="pnlConteudo_Solicitacoes_PoupeDirect" class="Conteudo_Container">
        <div id="pnlConteudo_Solicitacoes_PoupeDirect_Dados" style="display:none;"></div>
    </div>

    <div id="pnlConteudo_Solicitacoes_PoupeOperacoes" class="Conteudo_Container">
        <div id="pnlConteudo_Solicitacoes_PoupeOperacoes_Dados" style="height: 66em; display:none;"></div>
        <div id="pnlConteudo_Solicitacoes_PoupeOperacoes_Resultado" style="height: 66em; display:none;"></div>
    </div>

    <div id="pnlConteudo_Solicitacoes_CadastroDeCambio" class="Conteudo_Container">
        <div id="pnlConteudo_Solicitacoes_CadastroDeCambio_Dados"        class="pnlFormulario pnlFormularioExtendido" style="display:none;"></div>
    </div>
    
    <div id="pnlConteudo_Solicitacoes_SolicitacoesResgate" class="Conteudo_Container">
        <div id="pnlConteudo_Solicitacoes_SolicitacoesResgate_Dados" class="pnlFormulario pnlFormularioExtendido" style="display:none;"></div>
    </div>

    <div id="pnlConteudo_Solicitacoes_GerenciamentoIPO" class="Conteudo_Container">
        <div id="pnlConteudo_Solicitacoes_GerenciamentoIPO_Dados"        class="pnlFormulario pnlFormularioExtendido" style="display:none; width:1210px"></div>
    </div>
    <div id="pnlNovoItem" style="display:none">

        <div class="pnlFormulario">
            <a class="AbinhaFechar" href="#" onclick="return pnlFormulario_btnCancelar_Click()" title="Cancelar"><span>Cancelar</span></a>
            <span class="Aguarde">Carregando, por favor aguarde...</span>

            <div id="pnlNovoItem_Formulario"></div>
        </div>

    </div>

    <div id="pnlVersaoDoSite" title="Versão <%= this.VersaoDoSite %>"><%= this.VersaoDoSite %></div>

<div id="pnlCotacaoRapida" style="display:none">

    <div id="pnlCotacaoRapidaHeader">
        <span>Cotação Rápida</span>
        <button class="IconButton Cancelar"  style="float: right" onclick="return GradIntra_ContacaoRapida_Fechar(this);"></button>
    </div>

    <p class="SubFormulario">

        <label for="txtCotacaoRapida_Papel">Papel:</label>
        <input  id="txtCotacaoRapida_Papel" type="text" class="txtPapel GradWindow_TextBoxDePapel" style="width:6em; text-transform:uppercase" value="" />

        <button onclick="return CotacaoRapida_AssinarPapel(this);" style="font-size:0.8em">OK</button>

        <a href="#" class="IconeEstadoPapel EstadoPapel_1" title="" style="display:none">&nbsp;</a>
    </p>

    <table cellspacing="0" class="CotacaoRapida" >
    
        <thead>
            <tr>
                <td colspan="4" class="tdNomeDoAtivo"></td>
            </tr>
        </thead>
    
        <tbody>
            <tr>
                <td class="tdLabel">Últ.:</td>
                <td class="tdValor" Propriedade="Preco" Format="###,###.00"></td>
                <td class="tdLabel">Var:</td>
                <td class="tdValor ValorVariacao" Propriedade="Variacao" ></td>
            </tr>
            
            <tr>
                <td class="tdLabel">Compra:</td>
                <td class="tdValor ValorCompra AtualizacaoSemSetas" Propriedade="MelhorPrecoCompra"></td>
                <td class="tdLabel">Venda:</td>
                <td class="tdValor ValorVenda AtualizacaoSemSetas" Propriedade="MelhorPrecoVenda"></td>
            </tr>
            
            <tr>
                <td class="tdLabel">Qtd. CP:</td>
                <td class="tdValor ValorQtdCompra AtualizacaoSemSetas" Propriedade="QuantidadeMelhorPrecoCompra"></td>
                <td class="tdLabel">Qtd. VD:</td>                                    
                <td class="tdValor ValorQtdVenda AtualizacaoSemSetas" Propriedade="QuantidadeMelhorPrecoVenda"></td>
            </tr>
            
            <tr>
                <td class="tdLabel">Máx.:</td>
                <td class="tdValor ValorMaxima AtualizacaoSemSetas" Propriedade="MaxDia" Format="###,###.00"></td>
                <td class="tdLabel">Mín.:</td>
                <td class="tdValor ValorMinima AtualizacaoSemSetas" Propriedade="MinDia" Format="###,###.00"></td>
            </tr>
            
            <tr>
                <td class="tdLabel">Abert.:</td>
                <td class="tdValor ValorAbertura NaoExibirAtualizacao" Propriedade="ValorAbertura"></td>
                <td class="tdLabel">Fech.:</td>
                <td class="tdValor ValorFechamento NaoExibirAtualizacao" Propriedade="ValorFechamento"></td>
            </tr>
            
            <tr>
                <td class="tdLabel">Vol.:</td>
                <td class="tdValor ValorVolume AtualizacaoSemSetas" Propriedade="VolumeAcumulado"></td>
                <td class="tdLabel">N° de Neg.:</td>
                <td class="tdValor ValorNumeroDeNegocios AtualizacaoSemSetas" colspan="2" Propriedade="NumNegocio"></td>
            </tr>
        </tbody>
    
    </table>

</div>

<div id="pnlListaDeMensagens" style="display:none">

    <a href="#" onclick="return false"><span>Abrir histórico de mensagens</span></a>

    <ul>
        <li class="Template" style="display:none">
            <label></label>
            <button onclick="return pnlListaDeMensagens_li_btn_Click(this)"></button>
            <textarea></textarea>
        </li>
    </ul>

</div>

</form>
</body>
</html>

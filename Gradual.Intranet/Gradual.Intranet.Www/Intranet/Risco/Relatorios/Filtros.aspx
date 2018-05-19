<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Filtros.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Relatorios.Filtros" %>

<form id="form2" runat="server">

    <input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />

    <div id="pnlRisco_FiltroRelatorio" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

        <p style="width:98%">
            <label for="cboRisco_FiltroRelatorioRisco_Relatorio">Relatório:</label>
            <select id="cboRisco_FiltroRelatorioRisco_Relatorio" style="width:504px" onchange="cboRelatorios_TipoDeRelatorioRisco_Change(this)">
                <option value="">[ Selecione ]</option>
                <option value="R001">R-001: Permissões Gerais</option>
                <option value="R002">R-002: Permissões por Cliente</option>
                <option value="R003">R-003: Parâmetros Gerais</option>
                <option value="R004">R-004: Parâmetros por Cliente</option>
                <option value="R005">R-005: Limite por Cliente</option>
                <option value="R006" style="display: none;">R-006: Posição de Conta Corrente</option>
                <option value="R007">R-007: Cliente Bloqueio por Ativos</option>
                <%--<option value="R008">R-008: Cliente Bloqueio por Grupos</option>--%>
                <option value="R009">R-009: Composição de Saldo Bloqueado</option>
                <option value="R010">R-010: Custódia - Mercado a Vista x Opção</option>
            </select>
        </p>

        <!-- Linhas Variaveis -->

        <p class="R002 R004 R005 R006 R007 R009"  style="width:237em;display:none;">
            <label for="cboRisco_BuscarPor">Buscar Por:</label>
            <select id="cboRisco_BuscarPor" Propriedade="TipoBusca" style="float:left;width:118px;">
                <option value="1">CPF/CNPJ</option>
                <option value="2">Nome do Cliente</option>
                <option value="0" selected>Código Bovespa</option>
            </select>
            <input id="txbRisco_BuscarPor" Propriedade="ParametroBusca" type="text" style="width:375px;margin:2px 0px 0px 5px;" onkeyup="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);" />
        </p>
        
        <p class="R001 R002 R003 R004 " style=" display:none">
            <label for="cboRisco_FiltroRelatorio_Bolsa">Bolsa:</label>
            <select id="cboRisco_FiltroRelatorio_Bolsa" style="width:100px" Propriedade="Bolsa">
                <option value="">[ Todas ]</option>
                <option value="1">Bovespa</option>
                <option value="2">BMF</option>
            </select>
        </p> 

        <p class="R002 R004 R008" style="width: 227px; margin-right:6px; float: left; display: none;">
            <label for="txtBusca_FiltroRelatorioRisco_Grupo">Grupo:</label>
            <select id="txtBusca_FiltroRelatorioRisco_Grupo" propriedade="Grupo" style="width: 118px;">
                <asp:repeater id="rptRisco_FiltroRelatorioRisco_Grupo" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("idgrupo") %>'><%# Eval("dsgrupo")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p class="R007 R009 R010" style="width: 36.25em; float: left; display: none;">
            <label for="txbRisco_FiltrarPorPapel">Papel:</label>
            <input type="text" id="txbRisco_FiltrarPorPapel" Propriedade="Papel" maxlength="10" style="width:285px;margin-top:2px;" onkeyup="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);" />
        </p>

        <p class="R003 R004 R005 R008 " style="width: 45em; float: left; display: none;">
            <label for="txtBusca_FiltroRelatorioRisco_Parametros">Parâmetros:</label>
            <select id="txtBusca_FiltroRelatorioRisco_Parametros" propriedade="Parametro" style="width: 268px;">
                <asp:repeater id="rptRisco_FiltroRelatorioRisco_Parametro" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("idParametro") %>'><%# Eval("dsParametro")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>
                
        <p class="R001 " style="width: 45em; float: left; display: none;">
            <label for="txtBusca_FiltroRelatorioRisco_Permissao">Permissão:</label>
            <select id="txtBusca_FiltroRelatorioRisco_Permissao" propriedade="Permissao" style="width: 287px;">
                <asp:repeater id="rptRisco_FiltroRelatorioRisco_Permissao" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("idPermissao") %>'><%# Eval("dsPermissao")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p class="R009 " style="width: 55em; float: left; display: none;">
            <label for="txtBusca_FiltroRelatorioRisco_TipoDeOrdem">Sentido:</label>
            <select id="txtBusca_FiltroRelatorioRisco_TipoDeOrdem" propriedade="TipoDeOrdem" style="width: 291px;">
                <option value="">[Todas]</option>
                <option value="V">Custódia</option>
                <option value="C">Financeiro</option>
            </select>
        </p>

        <p class="R009 " style="width: 55em; float: left; display: none;">
            <label for="txtBusca_FiltroRelatorioRisco_DataInicio">Movimento de:</label>
            <input id="txtBusca_FiltroRelatorioRisco_DataInicio" type="text" propriedade="DataInicio" maxlength="10" style="width:63px;"value="<%= DateTime.Now.ToString("01/MM/yyyy") %>" class="Mascara_Data Picker_Data" >
            <label for="txtBusca_FiltroRelatorioRisco_DataFim" style="padding-left: 20px;">até:</label>
            <input id="txtBusca_FiltroRelatorioRisco_DataFim" type="text" propriedade="DataFim" maxlength="10" style="width:63px;" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" class="Mascara_Data Picker_Data">
        </p>

        <p class=" " style="width: 60em; float: left; display: none;">
            <label for="txtBusca_FiltroRelatorioRisco_Historico">Histórico:</label>
            <input id="txtBusca_FiltroRelatorioRisco_Historico" type="text" propriedade="Historico" maxlength="500" style="width:500px;">
        </p>

        <p class="R006 " style="width: 60em; float: left; display: none;">
            <label>Negativo em:</label>
            <label for="ckbBusca_FiltroRelatorioRisco_Todos">Selecionar todos</label>
            <input type="checkbox" id="ckbBusca_FiltroRelatorioRisco_Todos" class="ckbBusca_FiltroRelatorioRisco" title="Todos" style="width:15px;" onclick="GradIntra_Relatorio_Risco_SelecionarTodos(this);GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
            <label for="ckbBusca_FiltroRelatorioRisco_D0">D0</label>
            <input type="checkbox" id="ckbBusca_FiltroRelatorioRisco_D0" class="ckbBusca_FiltroRelatorioRisco ckbBusca_FiltroRelatorioRiscoConsultaPrincipal" propriedade="D0" title="D0" style="width:15px;" onclick="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
            <label for="ckbBusca_FiltroRelatorioRisco_D1">D1</label>
            <input type="checkbox" id="ckbBusca_FiltroRelatorioRisco_D1" class="ckbBusca_FiltroRelatorioRisco ckbBusca_FiltroRelatorioRiscoConsultaPrincipal" propriedade="D1" title="D1" style="width:15px;" onclick="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
            <label for="ckbBusca_FiltroRelatorioRisco_D2">D2</label>                                          
            <input type="checkbox" id="ckbBusca_FiltroRelatorioRisco_D2" class="ckbBusca_FiltroRelatorioRisco ckbBusca_FiltroRelatorioRiscoConsultaPrincipal" propriedade="D2" title="D2" style="width:15px;" onclick="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
            <label for="ckbBusca_FiltroRelatorioRisco_D3">D3</label>                                       
            <input type="checkbox" id="ckbBusca_FiltroRelatorioRisco_D3" class="ckbBusca_FiltroRelatorioRisco ckbBusca_FiltroRelatorioRiscoConsultaPrincipal" propriedade="D3" title="D3" style="width:15px;" onclick="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
            <label for="ckbBusca_FiltroRelatorioRisco_CM">Conta Margem</label>
            <input type="checkbox" id="ckbBusca_FiltroRelatorioRisco_CM" class="ckbBusca_FiltroRelatorioRisco" propriedade="CM" title="Conta Margem" style="width:15px;" onclick="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
        </p>

        <p class="R006 " style="width: 45em; float: left; display: none;">
            <label for="cmbBusca_FiltroRelatorioRisco_Assessor">Assessor:</label>
            <select id="cmbBusca_FiltroRelatorioRisco_Assessor" propriedade="Assessor" style="width: 268px;" onchange="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
                <option value="">[Todos]</option>
                <asp:repeater id="rptRisco_FiltroRelatorioRisco_Assessor" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <!--  /  Linhas Variaveis -->
        
        <p style="margin-right:6px;text-align:right;float:right;display:none">
            <button class="btnBusca" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this)">Buscar</button>
            <button class="btnBusca" onclick="window.print();return false;" id="btnRiscoRelatorioImprimir" style="width:auto;margin-left:8px;" disabled="disabled">Imprimir Relatório</button>
            <img id="btnRisco_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnRisco_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="margin:3px 0px 0px 5px;float:right;cursor:pointer;">
        </p>
        
        <div id="divRisco_Relatorios_TextoConfigurarImpressao" class="PopUpDeAjuda" style="display:none; left: 660px; top: 200px; width: 280px;">
            <div>
                <div style="text-align:center;width:auto;position:float;text-decoration:underline;margin-bottom:1em">Instruções de impressão</div>
                <div style="text-align:justify;margin-top:5px;">Para imprimir este relatório sem cabeçalho e rodapé padrão do navegador siga os seguintes passos:</div>
                <div>
                    <ol style="margin:15px 0px 0px 15px;list-style-type:decimal">
                        <li style="list-style-type:decimal;margin-bottom:1em">No menu superior do navagador clique em 'Arquivo' (<span style="text-decoration:underline">caso não esteja aparecendo, pressione primeiramente a tecla 'Alt' do seu teclado e aguarde o menu aparecer</span>);</li>
                        <li style="list-style-type:decimal;margin-bottom:1em">Clique em 'Configurar página';</li>
                        <li style="list-style-type:decimal;margin-bottom:1em">Selecione o item 'Paisagem' em Orientação de impressão;</li>
                        <li style="list-style-type:decimal;margin-bottom:1em">Exclua o conteúdo dos campos 'Cabeçalho' e 'Rodapé';</li>
                        <li style="list-style-type:decimal;margin-bottom:1em">Clique em 'OK'.</li>
                        <li style="list-style-type:decimal;margin-bottom:1em">Imprima novamente.</li>
                    </ol>
                </div>
                <div style="margin-top:15px;">* Estas instruções são compatíveis com a versão 8.0 do MS Internet Explorer.</div>
            </div>
        </div>
    </div>
    <script>
        $.ready(GradIntra_Risco_Relatorios_Load());
    </script>
</form>

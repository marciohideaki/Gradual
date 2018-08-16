<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Relatorios.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Relatorios" %>

<form id="form1" runat="server">

    <input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />

    <div id="pnlClientes_FiltroRelatorio_FF1" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

        <p style="width:98%">
            <label for="cboClientes_FiltroRelatorio_Relatorio">Relatório:</label>
            <select id="cboClientes_FiltroRelatorio_Relatorio" style="width:504px" onchange="cboRelatorios_TipoDeRelatorio_Change(this)">
                <option value="">[ Selecione ]</option>
                <option value="R001">R-001: Clientes cadastrados por período</option>
                <option value="R002">R-002: Pendências cadastrais</option>
                <option value="R003">R-003: Solicitação de alteração cadastral</option>
                <option value="R004">R-004: Clientes em período de renovação cadastral</option>
                <option value="R005">R-005: Clientes exportados/importados do Sinacor</option>
                <option value="R006">R-006: Clientes que realizaram o Suitability (Pessoa Física)</option>
                <option value="R007">R-007: Clientes suspeitos</option>
                <asp:literal runat="server" id="litRelAssessor">
                    <option value="R008">R-008: Emails disparados por período</option>
                    <option value="R009">R-009: Clientes que não realizaram o primeiro acesso</option>
                    <option value="R010">R-010: Clientes inativos (Exportados)</option>
                </asp:literal>
            </select>
        </p>

        <!-- Linhas Variaveis -->

        <p class="R001 R002 R003 R004 R005 R006 R007 R008 R009 " style="width:17em;display:none;">
            <label for="txtClientes_FiltroRelatorio_DataInicial">Data Inicial:</label>
            <input  id="txtClientes_FiltroRelatorio_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" />
        </p>

        <p class="R001 R002 R003 R004 R005 R006 R007 R008 R009 " style="width:17em;display:none">
            <label for="txtClientes_FiltroRelatorio_DataFinal" style="width:6em;">Data Final:</label>
            <input  id="txtClientes_FiltroRelatorio_DataFinal" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" />
        </p>

        <p class="R001 R002 R003 R005 R006 R009 " style="width: 75em; float: left; display: none;">
            <label for="cmbBusca_Monitoramento_Relatorio_Assessor">Assessor:</label>
            <select id="cmbBusca_FiltroRelatorioRisco_Assessor" propriedade="Assessor" style="width: 268px;" onchange="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);">
                <option value="">[Todos]</option>
                <asp:repeater id="rptRisco_FiltroRelatorioRisco_Assessor" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p class="R008" style="display:none; width:25em">
            <label for="cboClientes_FiltroRelatorio_TipoEmailDisparado">Tipo E-mail:</label>
            <select id="cboClientes_FiltroRelatorio_TipoEmailDisparado" style="width:100px" Propriedade="TipoEmail">
                <option value="">[ Todas ]</option>
                <option value="1">Assessor</option>
                <option value="2">Compliance</option>
            </select>
        </p>

        <p class="R008" style="display:none; clear: left; width: 30em">
            <label for="txtClientes_FiltroRelatorio_Email_Disparado">E-Mail:</label>
            <input  id="txtClientes_FiltroRelatorio_Email_Disparado" type="text"  Propriedade="EmailDisparado" style="width:150px" />
        </p>

        <p class="R002 " style="display:none;clear: left; width: 380px;">
            <label for="cboClientes_FiltroRelatorio_TipoDePendencia" style="width:7em;">Tipo:</label>
            <select id="cboClientes_FiltroRelatorio_TipoDePendencia" Propriedade="TipoDePendencia">
                <option value="">[ Todos ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_TipoDePendencia" runat="server">
                <ItemTemplate>
                    <option value='<%# Eval("IdTipoPendencia") %>'><%# Eval("DsPendencia")%></option>
                </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <p class="R002 R003" style="display:none;">
            <label for="cboClientes_FiltroRelatorio_Resolvida" style="width:7em;">Resolvida:</label>
            <select id="cboClientes_FiltroRelatorio_Resolvida" Propriedade="PendenciaResolvida">
                <option value="">[ Todos ]</option>
                <option value="1">Sim</option>
                <option value="0">Não</option>
            </select>
        </p>
        
        <p class="R006" style="display:none;">
            <label for="cboClientes_FiltroRelatorio_PendenciaResolvida" style="width:7em;">Realizado:</label>
            <select id="cboClientes_FiltroRelatorio_PendenciaResolvida" Propriedade="Realizado">
                <option value="1">Sim</option>
                <option value="0" selected="selected">Não</option>
            </select>
        </p>

        <p class="R003 R005 R006" style="display:none;">
            <label for="txtClientes_FiltroRelatorio_CpfCnpj">CPF/CNPJ:</label>
            <input  id="txtClientes_FiltroRelatorio_CpfCnpj" type="text" maxlength="14"  Propriedade="CpfCnpj" />
        </p>

        <!-- Este combo recebe tratamento especial de filtro de atividades cadastradas na BlackList, OBSERVE a restrição antes de usá-lo em outro relatório -->
        <p class="R007 " style="display:none;clear: left; width: 375px;">
            <label for="cboClientes_FiltroRelatorio_AtividadeIlicita">Atividade:</label>
            <select id="Select1" style="width:23.2em" Propriedade="AtividadeIlicita">
                <option value="">[ Todas ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_AtividadeIlicita" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("IdAtividadeIlicita") %>'><%# Eval("CdAtividade")%></option>
                </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <!-- Este combo recebe tratamento especial de filtro de países cadastrados na BlackList, OBSERVE a restrição antes de usá-lo em outro relatório -->
        <p class="R007 " style="display:none">
            <label for="cboClientes_FiltroRelatorio_Pais" style="width:4em">País:</label>
            <select id="cboClientes_FiltroRelatorio_Pais" style="width:14em" Propriedade="Pais">
                <option value="">[ Todos ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_Pais" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("CdPais") %>'><%# Eval("DsNomePais")%></option>
                </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>
        
        <!--  /  Linhas Variaveis -->
        
        <p style="margin-right:6px;text-align:right;float:right;display:none">
            <button class="btnBusca" onclick="return btnClientes_Relatorios_FiltrarRelatorio_Click(this)">Buscar</button>
            <button class="btnBusca" onclick="window.print();return false;" id="btnClienteRelatorioImprimir" style="width:auto;margin-left:8px;" disabled="disabled">Imprimir Relatório</button>
            <img id="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="margin:3px 0px 0px 5px;float:right;cursor:pointer;">
        </p>
        
        <div id="divClientes_Relatorios_TextoConfigurarImpressao" class="PopUpDeAjuda" style="display:none; left: 684px; top: 200px; width: 280px;">
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

</form>

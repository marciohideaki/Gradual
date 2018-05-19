<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Filtros.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.Relatorios.Filtros" %>

<form id="form3" runat="server">

    <div id="pnlSistema_FiltroRelatorio" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

        <p style="width:98%">
            <label for="cboSistema_RelatorioTipo">Relatório:</label>
            <select id="cboSistema_RelatorioTipo" style="width:660px" onchange="cboSistema_RelatorioTipo_Change(this)">
                <option value="">[ Selecione ]</option>
                <option value="R001">R-001: Controle de Log da Intranet</option>
                <option value="R002">R-002: Autorizações de Cadastro</option>
            </select>
        </p>

        <!-- Linhas Variaveis -->

        <p class="R001 " style="width: 35em; float: left; display: none;">
            <label for="txbBusca_Sistema_Relatorio_FiltrarPorEmailUsuario">E-mail Usuário:</label>
            <input type="text" id="txbBusca_Sistema_Relatorio_FiltrarPorEmailUsuario" Propriedade="EmailUsuario" maxlength="128" style="width:250px;margin-top:2px;" onkeyup="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);" />
        </p>

        <p class="R001 " style="width: 35em; float: left; display: none;">
            <label for="cmbBusca_Sistema_Relatorio_FiltrarPorAcao">Ação:</label>
            <select id="cmbBusca_Sistema_Relatorio_FiltrarPorAcao" propriedade="TipoAcao" style="width: 268px;">
                <option value="">[Todos]</option>
                <option value="1">Consulta</option>
                <option value="2">Edição</option>
                <option value="3">Exclusão</option>
                <option value="4">Inclusão</option>
            </select>
        </p>

        <p class="R001 R002" style="width: 35em; float: left; display: none;">
            <label for="txtBusca_Sistema_Relatorio_DataInicio">Data de:</label>
            <input id="txtBusca_Sistema_Relatorio_DataInicio" type="text" propriedade="DataInicio" maxlength="10" style="width:63px;"value="<%= DateTime.Now.ToString("01/MM/yyyy") %>" class="Mascara_Data Picker_Data" >
            <label for="txtBusca_Sistema_Relatorio_DataFim" style="padding-left: 20px;">até:</label>
            <input id="txtBusca_Sistema_Relatorio_DataFim" type="text" propriedade="DataFim" maxlength="10" style="width:63px;" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" class="Mascara_Data Picker_Data">
        </p>

        <p class=" " style="width: 35em; float: left; display: none;">
            <label for="cmbBusca_Sistema_Relatorio_FiltrarPorTela">Tela:</label>
            <select id="cmbBusca_Sistema_Relatorio_FiltrarPorTela" propriedade="Tela" style="width: 268px;">
                <option value="">[Todos]</option>
                <asp:repeater id="repeaterSistema_FiltroRelatorio_Tela" runat="server">
                    <itemtemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </itemtemplate>
                </asp:repeater>
            </select>
        </p>

        <p class=" "  style="width:70em;display:none;">
            <label for="cboBusca_Monitoramento_Relatorio_BuscarPor">Buscar Por Cliente:</label>
            <select id="cboBusca_Monitoramento_Relatorio_BuscarPor" Propriedade="TipoBusca" style="float:left;width:118px;">
                <option value="1">CPF/CNPJ</option>
                <option value="2">Nome do Cliente</option>
                <option value="0" selected>Código Bovespa</option>
            </select>
            <input id="txbRisco_BuscarPor" Propriedade="ParametroBusca" type="text" style="width:530px;margin:2px 0px 0px 5px;" onkeyup="GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(this);" />
        </p>

        <!--  /  Linhas Variaveis -->
        
        <p style="margin-right:6px;text-align:right;float:right;display:none">
            <button class="btnBusca" onclick="return btnSistema_Relatorios_FiltrarRelatorio_Click(this)">Buscar</button>
            <button class="btnBusca" onclick="window.print();return false;" id="btnRiscoRelatorioImprimir" style="width:auto;margin-left:8px;" disabled="disabled">Imprimir Relatório</button>
            <img id="btnRisco_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnMonitoramento_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="margin:3px 0px 0px 5px;float:right;cursor:pointer;">
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
        //$.ready(GradIntra_Monitoramento_Relatorios_Load());
    </script>
</form>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Filtros.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.Filtros" %>

<form id="form1" runat="server">
<input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />
<input type="hidden" id="hddIdAssessorLogadoFilial" value="<%= gIdAssessorLogadoFilial %>" />

<div id="pnlDBM_FiltroRelatorio_FF1" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

    <p style="width:98%">
        <label for="cboDBM_FiltroRelatorio_Relatorio">Relatório:</label>
        <select id="cboDBM_FiltroRelatorio_Relatorio" style="width:504px" onchange="cboDBM_FiltroRelatorio_Relatorio_Change(this)">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptRelatorios" runat="server">
            <ItemTemplate>
                <option value="<%# Eval("Key") %>"><%# Eval("Value")%></option>
            </ItemTemplate>
        </asp:Repeater>
        </select>
    </p>

    <p class=" " style="width: 100%; display: none;">
        <label for="txtDBM_FiltroRelatorio_NomeCliente">Nome Cliente:</label>
        <input  id="txtDBM_FiltroRelatorio_NomeCliente" type="text" maxlength="40" Propriedade="NomeCliente" class="validate[required]" style="width: 41.5em;" />
    </p>

    <p class="R003 R004 " style="width: 100%; display: none;">
        <label for="txtDBM_FiltroRelatorio_CodigoCliente">Cód. Cliente:</label>
        <input  id="txtDBM_FiltroRelatorio_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" class="validate[required,custom[onlyNumber]] ProibirLetras" />
    </p>

    <p class="R002 R005 R006 " style="display:none">
        <label for="txtDBM_FiltroRelatorio_CodAssessor">Cód. Assessor:</label>
        <input  id="txtDBM_FiltroRelatorio_CodAssessor" type="text" runat="server" Propriedade="CodAssessor" class="ProibirLetras" />
    </p>

    <p class="R005 " style="width: 21em; display:none;">
        <label for="txtDBM_FiltroRelatorio_OperacaoInicio" style="width: 10em;">Data da Operação Início:</label>
        <input  id="txtDBM_FiltroRelatorio_OperacaoInicio" type="text" Propriedade="DataOperacaoInicio" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>

    <p class="R005 " style="width: 21em; display:none;">
        <label for="txtDBM_FiltroRelatorio_OperacaoFim" style="width: 10em;">Data da Operação Fim:</label>
        <input  id="txtDBM_FiltroRelatorio_OperacaoFim" type="text" Propriedade="DataOperacaoFim" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>

    <p class="R006 R007 " style="width: 21em; display:none;">
        <label for="txtDBM_FiltroRelatorio_Operacao" style="width: 10em;">Data da Operação:</label>
        <input  id="txtDBM_FiltroRelatorio_Operacao" type="text" Propriedade="DataOperacao" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>

    <p class="R001 R002 R004 " style="width: 17em; display:none;">
        <label for="txtDBM_FiltroRelatorio_DataInicial">Data Inicial:</label>
        <input  id="txtDBM_FiltroRelatorio_DataInicial" type="text" Propriedade="DataInicial" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>

    <p class="R001 R002 R004 " style="width: 17em; display :none">
        <label for="txtDBM_FiltroRelatorio_DataFinal" style="width:6em;">Data Final:</label>
        <input  id="txtDBM_FiltroRelatorio_DataFinal" type="text" Propriedade="DataFinal" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>

    <p class="R001 " style="display:none;  width: 600px;">
        <label for="cboDBM_FiltroRelatorio_Filial">Filial:</label>
        <select id="cboDBM_FiltroRelatorio_Filial" Propriedade="Filial" style="width:20.2em">
            <asp:Repeater id="rptFilial" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("CodigoFilial") %>'><%# Eval("NomeFilial")%></option>
                </ItemTemplate>
            </asp:Repeater>
        </select>
    </p>

    <p class="R001 " style="display:none;  width: 600px;">
        <label for="cboDBM_FiltroRelatorio_Mercado">Mercado:</label>
        <select id="cboDBM_FiltroRelatorio_Mercado" Propriedade="mercado" style="width:20.2em">
            <option value="0">Bovespa | BMF</option>
            <option value="1">Bovespa</option>
            <option value="2">BMF</option>
        </select>
    </p>
        
    <p style="margin-right:6px;text-align:right;float:right;display:none">
        <button class="btnBusca" onclick="return btnDBM_Relatorios_FiltrarRelatorio_Click(this)">Buscar</button>
        <button class="btnBusca" onclick="window.print();return false;" id="btnClienteRelatorioImprimir" style="width:auto;margin-left:8px;" disabled="disabled">Imprimir Relatório</button>
        <img id="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="margin:3px 0px 0px 5px;float:right;cursor:pointer;">
    </p>

    <p style="display: block; width: 100%;text-align:right;">
        <button class="btnBusca_" onclick="return btnDBM_ImprimirRelatorio_Click()" style="float: right; width: auto; margin-right: 35px;">Exportar para Excel</button>
    </p>
                
</div>

</form>

<script type="text/javascript" language="javascript">GradIntra_FiltrosDBM_PageLoad(this);</script>
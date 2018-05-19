<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManutencaoCliente.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.ManutencaoCliente" %>

<form id="form1" runat="server">

    <h4>Manutenção de Cliente</h4>
    
    <p style="width: 53em;;">
        <label for="cmbGradIntra_Risco_ManutencaoCliente_Grupos">Grupo</label>
        <select id="cmbGradIntra_Risco_ManutencaoCliente_Grupos">
		    <option value="">[Selecione]</option>
            <asp:repeater id="rptGradIntra_Risco_ManutencaoCliente_Grupo" runat="server">
                <itemtemplate>
		            <option value="<%# Eval("IdGrupo") %>"><%# Eval("DsGrupo")%></option>
                </itemtemplate>
            </asp:repeater>
	    </select>
    </p>

    <p style="width: 53em;">
        <label for="txtGradIntra_Risco_ManutencaoCliente_Cliente">Cliente</label>
        <input type="text" id="txtGradIntra_Risco_ManutencaoCliente_Cliente" class="ProibirLetras" maxlength="10" onkeyup="txtGradIntra_Risco_ManutencaoCliente_Cliente_KeyUp(this);" />
    </p>

    <p style="width: 53em;">
        <label for="cmbGradIntra_Risco_ManutencaoCliente_Assessor">Assessor</label>
        <select id="cmbGradIntra_Risco_ManutencaoCliente_Assessor" onchange="cmbGradIntra_Risco_ManutencaoCliente_Assessor_Change(this);">
		    <option value="">[Selecione]</option>
            <asp:repeater id="rptGradIntra_Risco_ManutencaoCliente_Assessor" runat="server">
                <itemtemplate>
		            <option value="<%# Eval("Id") %>"><%# Eval("Value")%></option>
                </itemtemplate>
            </asp:repeater>
	    </select>
    </p>

    <p style="margin-left: 9.5em; width: 36em; display: none;">
        <label for="txtGradIntra_Risco_ManutencaoCliente_DataDe" style="width: 6em;">Data Final:</label>
        <input  id="txtGradIntra_Risco_ManutencaoCliente_DataDe" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="width: 6em; float: left;" />
    </p>

    <p style="width: 16em; margin-left: -16em; display: none;">
        <label for="txtGradIntra_Risco_ManutencaoCliente_DataAte" style="width: 6em;">Data Final:</label>
        <input  id="txtGradIntra_Risco_ManutencaoCliente_DataAte" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="width: 6em; float: left;" />
    </p>

    <p>
        <button id="btnGradIntra_Risco_ManutencaoCliente_Buscar" style="width: 7em; margin: 1em 0pt 0pt 40em;" onclick="return btnGradIntra_Risco_ManutencaoCliente_Buscar_Click(this);">Buscar</button>
    </p>
    
    <div id="divGradIntra_Risco_ManutencaoCliente_Grid" style="margin: 185px 100px 0pt 37px; display: none;">
        <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo divGradIntra_Risco_ManutencaoCliente_Grid" style="width:99%;">
            <thead>
                <div>
                    <table id="tblGradIntra_Risco_ManutencaoCliente_Grupos"></table>
                    <div id="pnltGradIntra_Risco_ManutencaoCliente_Grupos_Pager"></div>
                </div>
            </thead>
        </table>
    </div>

    <h5 id="txtGradIntra_Risco_ManutencaoCliente_ItemNaoEncontrado" style="margin: 20em 0pt 0pt 23em; display: none;">Nenhum item encontrado</h5>

    <script>GradIntra_Risco_ManutencaoCliente_Grid_Load();</script>

</form>
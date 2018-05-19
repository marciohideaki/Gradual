<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClienteGrupo.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.ClienteGrupo" %>

<form id="form1" runat="server">

    <h4>Cliente Grupo</h4>
    
    <p style="width: 53em;;">
        <label for="cmbGradIntra_Risco_ClienteGrupo_Grupos">Grupo</label>
        <select id="cmbGradIntra_Risco_ClienteGrupo_Grupos">
		    <option value="">[Selecione]</option>
            <asp:repeater id="rptGradIntra_Risco_ClienteGrupo_Grupo" runat="server">
                <itemtemplate>
		            <option value="<%# Eval("IdGrupo") %>"><%# Eval("DsGrupo")%></option>
                </itemtemplate>
            </asp:repeater>
	    </select>
    </p>
    
    <p style="width: 53em;;">
        <label for="txtGradIntra_Risco_ClienteGrupo_Cliente">Cliente</label>
        <input type="text" id="txtGradIntra_Risco_ClienteGrupo_Cliente" class="ProibirLetras" maxlength="10" onkeyup="return txtGradIntra_Risco_ClienteGrupo_Cliente_KeyUp(this);" />
    </p>
    
    <p style="width: 53em;;">
        <label for="cmbGradIntra_Risco_ClienteGrupo_Assessor">Assessor</label>
        <select id="cmbGradIntra_Risco_ClienteGrupo_Assessor" onchange="return cmbGradIntra_Risco_ClienteGrupo_Assessor_Change(this);">
		    <option value="">[Selecione]</option>
            <asp:repeater id="rptGradIntra_Risco_ClienteGrupo_Assessor" runat="server">
                <itemtemplate>
		            <option value="<%# Eval("Id") %>"><%# Eval("Value")%></option>
                </itemtemplate>
            </asp:repeater>
	    </select>
    </p>

    <p style="width: 42.5em;">
        <input type="button" id="btnGradIntra_Risco_ClienteGrupo_Gravar" onclick="return btnGradIntra_Risco_ClienteGrupo_Gravar_Click(this);" value="Gravar" style="float: right;" />
    </p>

</form>

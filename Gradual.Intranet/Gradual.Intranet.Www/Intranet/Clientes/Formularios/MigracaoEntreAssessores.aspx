<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="MigracaoEntreAssessores.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.MigracaoEntreAssessores" %>

<form id="form1" runat="server">

    <p id="pnlClientes_MigracaoEntreAssessores_AlertaSelecionar" style="width:942px">
        Selecione ao menos um cliente da lista para realizar a transferência
    </p>

    <p>
        <label for="ckbClientes_MigracaoEntreAssessores_MigrarTodos">Migrar todos os clientes deste assessor</label>
        <input type="checkbox" id="ckbClientes_MigracaoEntreAssessores_MigrarTodos" onclick="return ckbClientes_MigracaoEntreAssessores_MigrarTodos_Click(this)" />
    </p>

    <p id="pnlClientes_MigracaoEntreAssessores_FormularioPara" style="width:98%; padding:25px 0em 1.1em 2.6em; display:none">

        <label for="cboClientes_MigracaoEntreAssessores_AssessorPara">Migrar [X] clientes para:</label>
        <select id="cboClientes_MigracaoEntreAssessores_AssessorPara" style="width: 400px;margin-right:4px" Propriedade="IdAssessorPara" onchange="cboClientes_MigracaoEntreAssessores_AssessorPara_Change(this)">
            <option value="">[ Selecione ]</option>

            <asp:repeater id="rptAssessorPara" runat="server">
            <itemtemplate>
            <option value='<%# Eval("Id") %>'><%# Eval("Id").ToString().PadLeft(4, '0') %> - <%# Eval("Value") %></option>
            </itemtemplate>
            </asp:repeater>

        </select>

        <button disabled="disabled" style="padding:3px 6px 3px 6px" onclick="return btnClientes_MigracaoEntreAssessores_RealizarMigracao_Click(this)">Realizar a Migração</button>
    </p>
    
    <div style="float:left;">

        <div style="padding-top:0px;"><table id="tblClientes_MigracaoEntreAssessores_ListaDeCliente" cellspacing="0"></table></div>
        <div id="pnlClientes_MigracaoEntreAssessores_ListaDeCliente_Pager"></div>

    </div>

</form>

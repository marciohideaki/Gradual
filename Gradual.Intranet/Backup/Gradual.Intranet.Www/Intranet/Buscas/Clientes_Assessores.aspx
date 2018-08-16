<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Clientes_Assessores.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Clientes_Assessores" %>

<form id="form1" runat="server">

    <div id="pnlClientes_Assessores" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_1Linha" style="width:600px">

        <p style="width:500px">
            <label for="cboClientes_MigracaoEntreAssessores_AssessorDe">Assessor:</label>
            <select id="cboClientes_MigracaoEntreAssessores_AssessorDe" style="width:400px" Propriedade="IdAssessor" onchange="cboClientes_MigracaoEntreAssessores_AssessorDe_Change(this)">
                <option value="">[ Selecione ]</option>

                <asp:repeater id="rptAssessorDe" runat="server">
                <itemtemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Id").ToString().PadLeft(4, '0') %> - <%# Eval("Value") %></option>
                </itemtemplate>
                </asp:repeater>
            </select>
        </p>

        <p style="margin-right:6px;text-align:right;float:right;width:80px">
            <button class="btnBusca" onclick="return btnClientes_Assessores_BuscarClientesDoAssessor_Click(this)">Buscar</button>
        </p>

    </div>

</form>

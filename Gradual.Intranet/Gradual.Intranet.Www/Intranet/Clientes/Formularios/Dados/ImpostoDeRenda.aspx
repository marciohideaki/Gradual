<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpostoDeRenda.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.ImpostoDeRenda" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<form id="form1" runat="server">

    <h4>Imposto de Renda - Comprovante de Rendimento</h4>

    <p style="width: 100%;">
        <label for="cmbGradIntra_Clientes_ImpostoDeRenda_AnoVigencia">Selecione o ano de exercício:</label>
        <select id="cmbGradIntra_Clientes_ImpostoDeRenda_AnoVigencia">
            <asp:repeater runat="server" id="rptGradIntra_Clientes_ImpostoDeRenda_AnoVigencia">
                <itemtemplate>
                    <option value="<%# Eval("Key") %>"><%# Eval("Value") %></option>
                </itemtemplate>
            </asp:repeater>
        </select>
    </p>

    <h5 style="position: absolute; text-align: center; width: 100%; margin-top: 62px;">Escolha o tipo de comprovante :</h5>

    <p class="BotoesSubmit" style="margin-top: 60px;">
        <button id="btnGradIntra_Clientes_ImpostoDeRenda_Operacoes" onclick="return GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(this)">Operações</button>
        <button id="btnGradIntra_Clientes_ImpostoDeRenda_TesouroDireto" onclick="return GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(this)">Tesouro Direto</button>
        <button id="btnGradIntra_Clientes_ImpostoDeRenda_DayTrade" onclick="return GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(this)">Day Trade</button>
    </p>

</form>
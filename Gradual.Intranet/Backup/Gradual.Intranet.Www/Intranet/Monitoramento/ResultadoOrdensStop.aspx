<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="ResultadoOrdensStop.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.ResultadoOrdensStop" %>

<form id="form1" runat="server">

        <%--<table cellspacing="0" style="width:98%;" >--%>
        <table cellspacing="0" style="width:98%;" class="pnlOrdensStop_Busca_Resultados GridIntranet GridIntranet_CheckSemFundo">
        <thead>
            <tr>
                <td><input type="checkbox" id="chkMonitoramento_ResultadoOrdens_SelecionarTodos" onclick="chkMonitoramento_ResultadoOrdensStop_SelecionarTodosResultados_OnClick(this)" /> <label for="chkMonitoramento_ResultadoOrdens_SelecionarTodos">&nbsp;</label></td>
                <td style="text-align:center">Número da Ordem</td>
                <td style="text-align:center">Tipo</td>
                <td style="text-align:center">Papel</td>
                <td style="text-align:center">Hora</td>
                <td style="text-align:center">Preço Stop</td>
                <td style="text-align:center">Preço Gain.</td>
                <td style="text-align:center">Preço Env.</td>
                <td style="text-align:center">Status</td>
                <td style="text-align:center">L/G</td>
                <td style="text-align:center">Cliente</td>
                <td style="text-align:center">Validade</td>
                <td style="text-align:center">Inicio Móvel</td>
                <td style="text-align:center">Ajuste</td>
            </tr>
        </thead>
    
        <tfoot>
            <tr>
                <td colspan="14" style="text-align:right; padding-top:1em">
                    <button class="" id="btnCancelarOrdensStop" onclick="return btnMonitoramento_ExcluirOrdensStartStopSelecionados_Click(this)" runat="server">Cancelar Selecionados</button>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
                        
            <asp:repeater id="rptResultadoOrdensStop" runat="server">
            
            <itemtemplate>

            <tr class="<%# Eval("Status").ToString().ToUpper()%>">
                <td><input type="checkbox" rel="<%# Eval("Id") %>" id="chkMonitoramento_ResultadoOrdensStop_<%# Eval("Id") %>" /><label for="chkMonitoramento_ResultadoOrdensStop_<%# Eval("Id") %>">&nbsp;</label></td>
                <td><%# Eval("Id") %></td>
                <td><%# Eval("Tipo")%></td>
                <td><%# Eval("Papel")%></td>
                <td><%# Eval("Hora")%></td>
                <td><%# Eval("PrecoStop")%></td>
                <td><%# Eval("PrecoLim")%></td>
                <td><%# Eval("Env")%></td>
                <td id="tdStatus"><%# Eval("Status")%></td>
                <td><%# Eval("LossGain")%></td>
                <td><%# Eval("Cliente")%></td>
                <td><%# Eval("Validade")%></td>
                <td><%# Eval("InicioMovel")%></td>
                <td><%# Eval("Ajuste")%></td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhuma ordem encontrada.</td>
            </tr>
        </tbody>
    
    </table>

</form>

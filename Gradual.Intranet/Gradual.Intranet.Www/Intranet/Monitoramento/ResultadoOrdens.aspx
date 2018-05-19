<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoOrdens.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.ResultadoOrdens" %>

<form id="form1" runat="server">

    <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo" style="width:99%;">
    
        <thead>
            <div>
                <table id="tblBusca_Ordens_Resultados"></table>
                <div id="pnlBusca_Ordens_Resultados_Pager"></div>
            </div>
           <%-- <tr>
                <td><input type="checkbox" id="chkBusca_SelecionarTodosResultados" onclick="chkBusca_SelecionarTodosResultados_OnClick(this)" /> <label for="chkBusca_SelecionarTodosResultados">&nbsp;</label> </td>
                <td>Número da Ordem</td>
                <td>Cliente</td>
                <td>Hora</td>
                <td>Status</td>
                <td>C/V</td>
                <td>Papel</td>
                <td>Quantidade</td>
                <td>Preço</td>
                <td>Qtd. Exec.</td>
                <td>Tipo</td>
                <td>Validade</td>--%>
                <%--<td>Enviada</td>--%>
           <%-- </tr>--%>
        </thead>
    
        <tfoot>
            <tr>
                <td colspan="14" style="text-align:right; padding-top:1em;">
                    <button class="btnBusca" id="btnCancelarOrdensStop" onclick="return btnMonitoramento_ExcluirOrdensSelecionadas_Click(this)" runat="server">Cancelar Selecionados</button>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
           

            <%--<asp:repeater id="rptResultadoOrdens" runat="server">
            
            <itemtemplate>

            <tr class="<%# Eval("Status").ToString().ToUpper()%>">
                <td><input type="checkbox" rel="<%# Eval("NumeroOrdem") %>" id="chkMonitoramento_ResultadoOrdens_<%# Eval("NumeroOrdem") %>" /> <label for="chkMonitoramento_ResultadoOrdens_<%# Eval("NumeroOrdem") %>">&nbsp;</label></td>
                <td><%# Eval("NumeroOrdem")%></td>
                <td><%# Eval("CodigoCliente")%></td>
                <td><%# Eval("Hora")%></td>
                <td id="tdStatus"><%# Eval("Status")%></td>
                <td><%# Eval("CompraVenda")%></td>
                <td><%# Eval("Papel")%></td>
                <td><%# Eval("Quantidade")%></td>
                <td><%# Eval("Preco")%></td>
                <td><%# Eval("QuantidadeExecutada")%></td>
                <td><%# Eval("Tipo")%></td>
                <td><%# Eval("Validade")%></td>
                <%--<td><%# Eval("Envida")%></td>--%>
            <%--</tr>
            
            </itemtemplate>
            </asp:repeater>--%>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhuma ordem encontrada.</td>
            </tr>
        </tbody>
        
    </table>
</form>

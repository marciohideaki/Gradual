<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesSaldo.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesSaldo" %>
<h4>Consulta de Saldos</h4>
                                    
<p>Confira a seguir o saldo atualizado dos seus investimentos em Fundos através da Gradual. </p>
                                    
<%--<div class="form-padrao clear">
    
                                        
    <div class="row">
        <div class="col1">
            <div class="lista-checkbox">
                <label class="checkbox"><asp:CheckBox ID="chkPossuiSaldoBloqueado" Text="Possui saldo bloqueado" runat="server" /></label>
            </div>
        </div>    
    </div>
                                        
    <div class="row">
        <div class="col1">                                         
            <div class="col2">
                <input type="submit" value="Buscar" name="buscar" class="botao btn-padrao btn-erica margeando">
            </div>
        </div>
    </div>
</div>--%>
                                    
<table class="tabela">
    <tr class="tabela-titulo">
        <td>Saldo Disponível</td>
        <td>Saldo Bloqueado</td>
        <td>Saldo Total</td>
        <td>Extrato</td>
    </tr>
    <asp:Repeater ID="rptListaDeSaldos" runat="server">
    <itemtemplate>
    <tr class="tabela-type-small">
        <td><%# Eval("SaldoDisponivel") %></td>
        <td><%# Eval("SaldoBloqueado") %> </td>
        <td><%# Eval("SaldoTotal") %>     </td>
        <td><asp:HyperLink NavigateUrl="~/MinhaConta/Financeiro/Extrato.aspx" Text="Ver extrato" runat="server" ID="lnkVerExtrato" > </asp:HyperLink>      </td>
    </tr>
    </itemtemplate>
    </asp:Repeater>  
    <tr id="trNenhumSaldos" runat="server" visible="false">
        <td colspan="4">Nenhum item encontrado.</td>
    </tr>                              
</table>
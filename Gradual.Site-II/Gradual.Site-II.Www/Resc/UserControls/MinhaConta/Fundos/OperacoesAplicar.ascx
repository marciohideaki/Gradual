<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesAplicar.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesAplicar" %>
<h4>Aplicar</h4>
<p>Confira na Plataforma de Fundos Gradual os Fundos de Investimento dos melhores gestores do mercado e aplique já!</p>
<table class="tabela">
    <tr class="tabela-titulo">
        <td>Fundos</td>
        <td>Risco</td>
        <td>Apl.<br /> Inicial</td>
        <td>Apl.<br /> Adic.</td>
        <td>Sld.<br /> Mín.</td>
        <td>Resg. Mín.</td>
        <td>Hor.</td>
        <td>Rent. Mês(%)</td>
        <td>12 Meses(%)</td>
        <td>Saiba Mais</td>
        <td class="alignCenter">Investir</td>
    </tr>
    <asp:Repeater runat="server" ID="rptListaDeOperacoesAplicar">
    <itemtemplate>             
    <tr class="tabela-type-small">
        <td><%# Eval("Fundo") %></td>
        <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde'" : "class='bolinha-amarelo'"  %> ></span></td>
        <td><%# Eval("AplicacaoInicial")%></td>
        <td><%# Eval("AplicacaoAdicional")%></td>
        <td><%# Eval("SaldoMinimo")%></td>
        <td><%# Eval("ResgateMinimo")%></td>
        <td><%# Eval("HorarioLimite")%></td>
        <td><%# Eval("RentabilidadeMes")%></td>
        <td><%# Eval("Rentabilidade12meses") %></td>
        <td><a class="botao btn-saibamais" href="Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Saiba +</a></td>
        <td><a class="botao btn-invista" href="Aplicar.aspx?idFundo=<%#Eval("IdProduto")%>">Aplicar</a></td>
    </tr>             
    </itemtemplate>
    </asp:Repeater>                                    

</table>

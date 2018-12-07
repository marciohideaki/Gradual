<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesPosicaoConsolidada.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesPosicaoConsolidada" %>
<h4>Posição Consolidada</h4>
                                    
<p>Consulte a seguir a sua posição consolidada nos Fundos de Investimento e veja as informações mais importantes para você gerir as suas aplicações.</p>
                                    
<table class="tabela">
    <tr class="tabela-titulo">
        <td>Fundo</td>
        <td>Risco</td>
        <td>Vlr. Cota</td>
        <td>Qtd. Cotas</td>
        <td>Vlr. Bruto</td>
        <td>IR</td>
        <td>Vlr. Líq.</td>
        <td>Aplicação. Em</td>
    </tr>
    <asp:Repeater runat="server" ID="rptListaDePosicaoConsolidada">
            <itemtemplate>
                <tr class="tabela-type-small" >
                    <td><%# Eval("NomeFundo")%></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde'" : "class='bolinha-amarelo'"  %> ></span></td>
                    <td><%# Eval("ValorCota")%></td>
                    <td><%# Eval("QtdCotas")%></td>
                    <td><%# Eval("ValorBruto")%></td>
                    <td><%# Eval("IR")%></td>
                    <td><%# Eval("ValorLiquido")%></td>
                    <td><%# Eval("DataAtualizacao")%></td>
                </tr>
            </itemtemplate>
            </asp:Repeater> 
            <tr id="trNenhumPosicaoConsolidada" runat="server" visible="false">
                <td colspan="8">Nenhum item encontrado.</td>
            </tr>                  
</table>
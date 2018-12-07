<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReferenciadoDI.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.Fundos.ReferenciadoDI" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/AbasFundosInvestimentos.ascx" tagname="AbasFundosInvestimentos" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="PaginaConteudo">
<div class="row">
    <div class="col1">
        <uc1:AbasFundosInvestimentos ID="AbasFundosInvestimentos1" modo="menu" runat="server" />
    </div>
</div>

<div class="row">
    <div class="col1">
        <div class="menu-exportar clear">
            <h3>Long and Short</h3>
        </div>
                        
        <p>Veja a seguir os Fundos de Long and Short disponíveis no mercado. </p>
                        
        <table class="tabela">
            <tr class="tabela-titulo">
                <td>Fundos</td>
                <td>Risco</td>
                <td>Apl.<br /> Inicial</td>
                <td>Apl.<br /> Adic.</td>
                <td>Tx.<br /> Admin.</td>
                <td>Resg. Mín.</td>
                <td>Hor.</td>
                <td>Rent. Mês(%)</td>
                <td>12 Meses(%)</td>
                <td>Saiba Mais</td>
                <td class="alignCenter">Investir</td>
            </tr>
            <asp:Repeater runat="server" ID="rptListaDeReferenciadoDI">
            <itemtemplate>                
            <tr class="tabela-type-small">
            <td><%# Eval("Fundo") %></td>
            <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
            <td><%# Eval("AplicacaoInicial")%></td>
            <td><%# Eval("AplicacaoAdicional")%></td>
            <td><%# Eval("TaxaAdministracao")%></td>
            <td><%# Eval("ResgateMinimo")%></td>
            <td><%# Eval("HorarioLimite")%></td>
            <td><%# Eval("RentabilidadeMes")%></td>
            <td><%# Eval("Rentabilidade12meses") %></td>
            <td><a class="botao btn-saibamais" href="Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
            <td><%# Eval("BotaoAplicar") %></td>
            </tr>
            </itemtemplate>
            </asp:Repeater>
        </table>
    </div>
</div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Prod - Fds - Referenciado DI" />

</asp:Content>


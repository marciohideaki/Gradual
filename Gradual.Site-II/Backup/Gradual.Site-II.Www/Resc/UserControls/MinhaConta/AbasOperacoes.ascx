<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbasOperacoes.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.AbasOperacoes" %>
<% if(this.Modo == "menu") { %>

<h2>Envio de Ordens</h2>

<ul id="menu-tabs" class="menu-tabs-2">
    <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "e") ? "ativo" : "" %>">
    <asp:hyperlink id="lnk1" runat="server" navigateurl="~/MinhaConta/operacoes/operacoes.aspx">Envio de Ordens</asp:hyperlink></li>
    <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "a") ? "ativo" : "" %>">
    <asp:hyperlink id="Hyperlink1" runat="server" navigateurl="~/MinhaConta/operacoes/acompanhamentoordens.aspx">Acompanhamento de Ordens</asp:hyperlink></li>
</ul>
<% }  %>
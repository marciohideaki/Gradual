<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbasFundosInvestimentos.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.AbasFundosInvestimentos" %>
<% if(this.Modo == "menu") { %>

    <h2>Fundos de Investimentos</h2>

    <ul class="clear menu-tabs menu-tabs-8 menu-tabs-100">
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "r") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk1" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/Recomendados.aspx">Recomendados</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "f") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk2" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/RendaFixa.aspx">Renda Fixa</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "m") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk3" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/Multimercados.aspx">Multimercados</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "a") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk4" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/Acoes.aspx">Ações</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "d") ? "ativo" : "" %>">
            <asp:hyperlink id="Hyperlink1" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/ReferenciadoDI.aspx">Long and Short</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "c") ? "ativo" : "" %>">
            <asp:hyperlink id="Hyperlink2" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/Cambial.aspx">Cambial</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "e") ? "ativo" : "" %>">
            <asp:hyperlink id="Hyperlink3" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/Exterior.aspx">Exterior</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "s") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk5" runat="server" navigateurl="~/MinhaConta/Produtos/Fundos/Simular.aspx">Simular</asp:hyperlink>
         </li>

         
    </ul>

<% }  %>

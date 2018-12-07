<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbasPosicao.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.AbasPosicao" %>
<% if(this.Modo == "menu") { %>

    <h2>Posição</h2>

    <ul id="menu-tabs" class="menu-tabs-3" >
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "s") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk1" runat="server" navigateurl="~/MinhaConta/Posicao/RendaVariavel.aspx">Renda Variável</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "e") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk2" runat="server" navigateurl="~/MinhaConta/Posicao/RendaFixa.aspx">RendaFixa</asp:hyperlink>
         </li>
         <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "f") ? "ativo" : "" %>">
            <asp:hyperlink id="Hyperlink1" runat="server" navigateurl="~/MinhaConta/Posicao/Fundos.aspx">Fundos</asp:hyperlink>
         </li>
    </ul>

<% } else { %>

    <ul id="abas" class="abas-menu">
        <%--<li class="<%= (this.AbaSelecionada == "c") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk10" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/Custodia.aspx">Custódia</asp:hyperlink>
        </li>--%>
        <li class="<%= (this.AbaSelecionada == "d") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk20" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/DayTrade.aspx">Day Trade</asp:hyperlink>
        </li>
        <li class="<%= (this.AbaSelecionada == "o") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk30" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/Operacoes.aspx">Operações</asp:hyperlink>
        </li>
        <li class="<%= (this.AbaSelecionada == "t") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk40" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/TesouroDireto.aspx">Tesouro Direto</asp:hyperlink>
        </li>
        
    </ul>

<% } %>
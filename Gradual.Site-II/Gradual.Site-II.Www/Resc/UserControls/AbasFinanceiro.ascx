<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbasFinanceiro.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.AbasFinanceiro" %>

<% if(this.Modo == "menu") { %>

    <%--<h2>Posição Consolidada</h2>--%>

    <ul class="clear menu-tabs menu-tabs-5 menu-tabs-100">

        
<%--        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "s") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk1" runat="server" navigateurl="~/MinhaConta/Financeiro/SaldosELimites.aspx">Saldos e Limites</asp:hyperlink>
        </li>--%>
        
        
        
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "m") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk0" runat="server" navigateurl="~/MinhaConta/Financeiro/MinhaConta.aspx">Minha Conta</asp:hyperlink>
        </li>
        
        
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "e") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk2" runat="server" navigateurl="~/MinhaConta/Financeiro/Extrato.aspx">Extrato</asp:hyperlink>
        </li>
        
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "n") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk3" runat="server" navigateurl="~/MinhaConta/Financeiro/NotasDeCorretagem.aspx">Notas de Corretagem</asp:hyperlink>
        </li>
        
        <%--
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "n") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk3" runat="server" navigateurl="~/MinhaConta/Financeiro/NotasDeCorretagem.aspx">Financeiro</asp:hyperlink>
        </li>
        --%>

        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "r") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk4" runat="server" navigateurl="~/MinhaConta/Financeiro/Retiradas.aspx">Retiradas</asp:hyperlink>
        </li>
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "i") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk5" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/daytrade.aspx">Informe de Rendimentos</asp:hyperlink>
        </li>
        <%--         
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "i") ? "ativo" : "" %>">
            <asp:hyperlink id="Hyperlink1" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/daytrade.aspx"></asp:hyperlink>
        </li>
        --%>
    </ul>

<% } else { %>

    <ul class="abas-menu">
        <%--<li class="<%= (this.AbaSelecionada == "c") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk10" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/Custodia.aspx">Custódia</asp:hyperlink>
        </li>--%>
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "d") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk20" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/DayTrade.aspx">Day Trade</asp:hyperlink>
        </li>
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "o") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk30" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/Operacoes.aspx">Operações</asp:hyperlink>
        </li>
        <li data-tipolink="Link" class="<%= (this.AbaSelecionada == "t") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk40" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/TesouroDireto.aspx">Tesouro Direto</asp:hyperlink>
        </li>
        <%--<li class="<%= (this.AbaSelecionada == "f") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk50" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/Fundos.aspx">Fundos</asp:hyperlink>
        </li>--%>
        <%--<li class="<%= (this.AbaSelecionada == "s") ? "ativo" : "" %>">
            <asp:hyperlink id="lnk60" runat="server" navigateurl="~/MinhaConta/Financeiro/Informe/SaldoFinanceiro.aspx">Saldo Financeiro</asp:hyperlink>
        </li>--%>
    </ul>

<% } %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuPrincipal.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MenuPrincipal" %>


<div id="menu">
    <a id="gradual" href="<%= this.HostERaiz %>/"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/gradual_logo.png"/></a>

    <asp:literal id="litMenuPrincipal" runat="server"></asp:literal>

    <%--<a id="lnkAbraSuaConta" runat="server" href="~/MinhaConta/NovoCadastro.aspx" class="abra-sua-conta">Abra sua conta</a>--%>

    <div id="pnlMinhaconta" runat="server" class="minha-conta">
        <a href="#" class="botao botao-hb" onclick="return AbrirOHB()">Home Broker</a>
        <!--button type="button" onclick="return false">Minha Conta</button-->

        <div class="BotaoMinhaConta">
            <%--<a href="#" class="botao botao-minhaconta">Minha Conta</a>--%>

            <a href="<%= this.HostERaiz %>/Minhaconta/Financeiro/MinhaConta.aspx" class="botao botao-minhaconta">Minha Conta</a>

            <div>
                <ul>
                    <li><a href="<%= this.HostERaiz %>/Minhaconta/Financeiro/MinhaConta.aspx">Posição Consolidada</a></li>
                    <%--<li><a href="<%= this.HostERaiz %>/Minhaconta/Financeiro/SaldosELimites.aspx">Financeiro</a></li>--%>
                    <li><a href="<%= this.HostERaiz %>/Minhaconta/Posicao/RendaVariavel.aspx">Posições</a></li>
                    <li><a href="<%= this.HostERaiz %>/Minhaconta/Produtos/Produtos.aspx#Aba-Contratados">Produtos</a></li>
                    <li><a href="<%= this.HostERaiz %>/Minhaconta/Cadastro/MeuCadastro.aspx">Meu Cadastro</a></li>
                    <li><a href="<%= this.HostERaiz %>/PlataformasdeNegociacao/PlataformadeFundos.aspx">Plataforma de Fundos</a></li>
                    <li><a href="<%= this.HostERaiz %>/Investimentos/Cambio.aspx">Câmbio</a></li>
                    <%--<li><a href="<%= this.HostERaiz %>/Minhaconta/Posicao/RendaVariavel.aspx">Posições</a></li>--%>
                    <%---<li><a href="<%= this.HostERaiz %>/Minhaconta/Operacoes/operacoes.aspx">Envio de Ordens</a></li>--%>
                </ul>
            </div>
        </div>
    </div>

</div>

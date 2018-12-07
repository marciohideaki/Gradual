<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="PaginaDinamica.aspx.cs" Inherits="Gradual.Site.Www.PaginaDinamica" %>

<%@ Register src="Resc/UserControls/RenderizadorDeWidgets.ascx" tagname="RenderizadorDeWidgets" tagprefix="ucRenderizador" %>

<%@ Register src="Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="PaginaConteudo">

        <ucRenderizador:RenderizadorDeWidgets id="rdrConteudo" runat="server" />

        <div id="pnlMensagemSemEstrutura" runat="server" visible="false">

            <h3>Página Não Cadastrada</h3>
            <p>
                Não foi encontrada uma estrutura cadastrada para a URL [/<asp:literal id="lblURLNaoEncontrada" runat="server"></asp:literal>] na versão [<asp:literal id="lblVersaoNaoEncontrada" runat="server"></asp:literal>].
            </p>

        </div>

    </section>

    <input type="hidden" id="hidVersao" value="<%= this.Versao %>" />
    <input type="hidden" id="hidVersaoPublicada" value="<%= this.VersaoPublicada %>" />

    <ucSauron:Sauron id="Sauron1" runat="server" />

</asp:Content>

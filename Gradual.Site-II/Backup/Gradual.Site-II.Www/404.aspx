<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Gradual.Site.Www._404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="PaginaConteudo">

        <h1>Erro 404</h1>

        <p>
            <asp:literal id="lblMensagem" runat="server"></asp:literal>
        </p>

    </section>

</asp:Content>

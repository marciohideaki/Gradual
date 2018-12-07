<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuTesouro.ascx.cs" Inherits="Gradual.Site.Www.MinhaConta.TesouroDireto.MenuTesouro" %>

<p class="Abas">

    <asp:HyperLink ID="lnkCompra" NavigateUrl="~/MinhaConta/TesouroDireto/Compra.aspx" runat="server">Compra</asp:HyperLink>

    <asp:HyperLink ID="lnkVenda" NavigateUrl="~/MinhaConta/TesouroDireto/Venda.aspx" runat="server">Venda</asp:HyperLink>

    <asp:HyperLink ID="lnkConsultaTitulos" NavigateUrl="~/MinhaConta/TesouroDireto/Consulta.aspx" runat="server">Consulta</asp:HyperLink>

    <asp:HyperLink ID="lnkExtrato" NavigateUrl="~/MinhaConta/TesouroDireto/Extrato.aspx" runat="server">Extrato</asp:HyperLink>

    <asp:HyperLink ID="lnkProtocolo" NavigateUrl="~/MinhaConta/TesouroDireto/ConsultarProtocolo.aspx" runat="server">Consultar Protocolo</asp:HyperLink>
</p>

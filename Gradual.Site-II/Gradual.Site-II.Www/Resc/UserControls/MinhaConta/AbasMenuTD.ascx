<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbasMenuTD.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.AbasMenuTD" %>
 <ul id="abas" class="abas-menu">
    <li class="<%= (this.AbaSelecionada == "c") ? "ativo" : "" %>">Compra</li>
    <li class="<%= (this.AbaSelecionada == "v") ? "ativo" : "" %>">Venda</li>
    <li class="<%= (this.AbaSelecionada == "s") ? "ativo" : "" %>">Consulta</li>
    <li class="<%= (this.AbaSelecionada == "e") ? "ativo" : "" %>">Extrato</li>
    <li class="<%= (this.AbaSelecionada == "p") ? "ativo" : "" %>">Consultar Protocolo</li>
</ul>

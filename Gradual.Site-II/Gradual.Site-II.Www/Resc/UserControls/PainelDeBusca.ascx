<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PainelDeBusca.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.PainelDeBusca" %>

<div id="pnlBusca" class="PainelLinkNavegacao" style="display:none">
        
    <p>
        <label for="txtBusca_Termo">Buscar:</label>
        <input  id="txtBusca_Termo" type="text" onkeydown="return txtBusca_Termo_KeyDown(this, event)" />
        <button class="BotaoVerde BotaoPequeno" style="width:4em" onclick="return btnBusca_Click(this)">ok</button>
    </p>
    <p class="f2">
        <a href="<%= this.HostERaiz %>/Aprendizado/DuvidasFrequentes.aspx">Dúvidas Frequentes</a>
        <a href="#">Atendimento em Chat</a>
    </p>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbasMeuCadastro.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.AbasMeuCadastro" %>

<div class="row">
    <div class="col1">
        <ul class="menu-tabs menu-tabs-full menu-tabs-3" style="margin-bottom:18px">
            <li id="liMeuCadastro"  runat="server"><a href="MeuCadastro.aspx" onclick="return lnkMeuCadastro_Click(this)">Meus Dados</a></li>
            <li id="liMeuPerfil"    runat="server" class="inativo"><a href="MeuPerfil.aspx"    onclick="return lnkMeuPerfil_Click(this)">Meu Perfil</a></li>
            <li id="liSeguranca" runat="server"><a href="Seguranca.aspx" onclick="return lnkSeguranca_Click(this)">Segurança</a></li>
        </ul>
    </div>
</div>
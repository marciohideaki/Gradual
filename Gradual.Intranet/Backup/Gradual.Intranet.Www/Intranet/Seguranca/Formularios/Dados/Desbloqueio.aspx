<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Desbloqueio.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.Desbloqueio" %>
<form id="form1" runat="server">
<h4>Desbloqueio</h4>
<p class="Menor1">
    <label for="txtSeguranca_Desbloqueio_Codigo_cliente">Código do Cliente:</label>
    <input type="text" id="txtSeguranca_Desbloqueio_Codigo_cliente" Propriedade="CodigoCliente" style="width:14em;" maxlength="10" class="validate[required,custom[onlyNumber],length[4,10]]" />
    
    <button id="btnSeguranca_Desbloqueio_Salvar" onclick="return btnSeguranca_Desbloqueio_Salvar_Click(this)">Desbloquear Cliente</button>
</p>
        
<p class="BotoesSubmit">
    
</p>
</form>

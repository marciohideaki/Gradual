<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EfetivarRenovacao.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.EfetivarRenovacao" %>

<form id="form1" runat="server">

    <h4>Efetivar Renovação Cadastral</h4>
    <p>
        <label for="txtClientes_EfetivarRenovacao_Data">Data da Assinatura da Ficha de Renovação:</label>
        <input type="text" id="txtClientes_EfetivarRenovacao_Data" Propriedade="Data" maxlength="10" class="validate[required,custom[data]] Mascara_Data" />
    </p>
    <p class="BotoesSubmit">
        <button onclick="return btnClientes_EfetivarRenovacao_Click(this)">Confirmar Renovação</button>
    </p>

</form>
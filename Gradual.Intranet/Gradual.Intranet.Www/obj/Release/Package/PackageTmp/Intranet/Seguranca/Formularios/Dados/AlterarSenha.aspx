<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="AlterarSenha.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.AlterarSenha" %>

<form id="form1" runat="server">

    <h4>Alterar Senha</h4>
    
    <div  id="pnlSeguranca_AlterarSenha_Campos" class="pnlFormulario_Campos">
    
        <input type="hidden" id="txtSeguranca_AlterarSenha_ParentId" Propriedade="ParentId" />
       

        <p class="Maior1">
            <label for="txt_Seguranca_AlterarSenha_NovaSenha">Senha atual:</label>
            <input type="password" id="txt_Seguranca_AlterarSenha_SenhaAtual" Propriedade="SenhaAtual" maxlength="20" class="validate[required]" style="width:14em;" />
        </p>

        <p class="Maior1" style="height:4em">
            <label for="txt_Seguranca_AlterarSenha_NovaSenha">Nova senha:</label>
            <input type="password" id="txt_Seguranca_AlterarSenha_NovaSenha" Propriedade="NovaSenha" maxlength="15" class="validate[required,funcCall[ValidatePasswordField]] pstrength" style="float:left; width:14em;" />
        </p>

        <p class="Maior1" style="height:4em">
            <label for="txt_Seguranca_AlterarSenha_ConfirmaNovaSenha">Confirmar nova senha:</label>
            <input type="password" id="txt_Seguranca_AlterarSenha_ConfirmaNovaSenha" Propriedade="ConfirmacaoSenha" maxlength="15" class="validate[required,funcCall[ValidatePasswordField]] pstrength" style="float:left; width:14em;" />
        </p>

        <p>&nbsp;</p>

        <p class="BotoesSubmit">
            <button id="btnSeguranca_AlterarSenha_Salvar" onclick="return btnSeguranca_AlterarSenha_Salvar_Click(this)">Alterar Senha</button>
        </p>

    </div>

</form>
<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_Perfil.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.DadosCompletos_Perfil" %>

<form id="form1" runat="server">

    <h4>Dados do Perfil</h4>

    <input type="hidden" id="txtPerfil_Id" Propriedade="Id" runat="server" />
    <input type="hidden" id="txtPerfil_TIpo" Propriedade="TipoDeObjeto" value="Perfil" />
    <input type="hidden" id="hidDadosCompletos_Seguranca_Perfil" class="DadosJson" runat="server" value="" />

    <p>
        <label for="txtSeguranca_DadosCompletos_Perfil_Nome">Nome:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_Perfil_Nome" Propriedade="Nome" maxlength="100" class="validate[required,length[5,100]]" />
    </p>

    <p class="BotoesSubmit">
        
        <button id="btnSalvar" onclick="return btnSalvar_Seguranca_DadosCompletos_Click(this)">Salvar Alterações</button>
        
    </p>

</form>

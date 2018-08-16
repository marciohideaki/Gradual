<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_Grupo.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.DadosCompletos_Grupo" %>

<form id="form1" runat="server">

    <h4>Dados do Grupo</h4>
    
    <input type="hidden" id="txtGrupo_Id" Propriedade="Id" runat="server" />

    <input type="hidden" id="txtGrupo_TIpo" Propriedade="TipoDeObjeto" value="Grupo" />

    <input type="hidden" id="hidDadosCompletos_Seguranca_Grupo" class="DadosJson" runat="server" value="" />

    <p>
        <label for="txtSeguranca_DadosCompletos_Grupo_Nome">Nome:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_Grupo_Nome" Propriedade="Nome" maxlength="100" class="validate[required,length[5,100]]" />
    </p>

    <p class="BotoesSubmit">
        
        <button id="btnSalvar" onclick="return btnSalvar_Seguranca_DadosCompletos_Click(this)">Salvar Alterações</button>
        
    </p>

</form>

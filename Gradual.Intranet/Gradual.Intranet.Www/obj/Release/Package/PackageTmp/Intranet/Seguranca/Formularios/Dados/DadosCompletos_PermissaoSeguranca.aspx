<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_PermissaoSeguranca.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.DadosCompletos_PermissaoSeguranca" %>

<form id="form1" runat="server">

    <h4>Dados da Permissão</h4>

    <input type="hidden" id="hidDadosCompletos_Seguranca_PermissaoSeguranca" class="DadosJson" runat="server" value="" />
    <input type="hidden" id="txtPermissaoSeguranca_Id" Propriedade="Id" runat="server" />
    <input type="hidden" id="txtPermissaoSeguranca_TIpo" Propriedade="TipoDeObjeto" value="PermissaoSeguranca" />

<%--    <p>
        <label for="txtSeguranca_DadosCompletos_PermissaoSeguranca_Sistema">Sistema:</label>
        <select id="txtSeguranca_DadosCompletos_PermissaoSeguranca_Sistema" Propriedade="Sistema">
            <option value="">[Selecione]</option>
            <option value="1">Desktop</option>
            <option value="2">Home Broker</option>
            <option value="3">Intranet</option>
        </select>
    </p>--%>

    <p>
        <label for="txtSeguranca_DadosCompletos_PermissaoSeguranca_Nome">Nome:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_PermissaoSeguranca_Nome" Propriedade="Nome" maxlength="255" class="validate[required,length[5,100]]" />
    </p>
    
    <p runat="server" id="lblSeguranca_DadosCompletos_PermissaoSeguranca_Descricao">
        <label for="txtSeguranca_DadosCompletos_PermissaoSeguranca_Senha" >Descrição:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_PermissaoSeguranca_Senha" Propriedade="DescricaoPermissao" maxlength="256" class="validate[required,length[0,256]]" />
    </p>

    <p>&nbsp;</p>

    <p class="BotoesSubmit">
        
        <button id="btnSalvar" onclick="return btnSalvar_Seguranca_DadosCompletos_Click(this)">Salvar Alterações</button>
        
    </p>
     
</form>
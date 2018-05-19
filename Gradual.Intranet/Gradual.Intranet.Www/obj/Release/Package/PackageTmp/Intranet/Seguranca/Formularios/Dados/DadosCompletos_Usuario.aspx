<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_Usuario.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.DadosCompletos_Usuario" %>

<form id="form1" runat="server">

    <script type="text/javascript">
        $.ready(cboSeguranca_DadosCompletos_Usuario_TipoAcesso_Change($("#cboSeguranca_DadosCompletos_Usuario_TipoAcesso")));
    </script>

    <h4>Dados do Usuário</h4>

    <input type="hidden" id="hidDadosCompletos_Seguranca_Usuario" class="DadosJson" runat="server" value="" />
    <input type="hidden" id="txtUsuario_Id" Propriedade="Id" runat="server" />
    <input type="hidden" id="txtUsuario_TIpo" Propriedade="TipoDeObjeto" value="Usuario" />

    <p>
        <label for="txtSeguranca_DadosCompletos_Usuario_Nome">Nome:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_Usuario_Nome" Propriedade="Nome" maxlength="100" class="validate[required,length[5,100]]" />
    </p>
    
    <p>
        <label for="txtSeguranca_DadosCompletos_Usuario_Email">Email:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_Usuario_Email" Propriedade="Email" maxlength="256" class="validate[required,length[0,256]]" /> <!-- custom[Email] -->
    </p>

    <p runat="server" id="lblSeguranca_DadosCompletos_Usuario_Senha">
        <label for="txtSeguranca_DadosCompletos_Usuario_Senha" >Senha:</label>
        <input type="password" id="txtSeguranca_DadosCompletos_Usuario_Senha" Propriedade="Senha" maxlength="256" class="validate[required,funcCall[ValidatePasswordField]]" style="width:14em;" />
    </p>

    <p runat="server" id="lblSeguranca_DadosCompletos_Usuario_TipoAcesso">
        <label for="cboSeguranca_DadosCompletos_Usuario_TipoAcesso" >Tipo Acesso:</label>
        <select id="cboSeguranca_DadosCompletos_Usuario_TipoAcesso" Propriedade="TipoAcesso" class="validate[required]" onchange="return cboSeguranca_DadosCompletos_Usuario_TipoAcesso_Change(this);">
            <option value="">[Selecione]</option>
            <option value="1">Cadastro</option>
            <option value="0">Cliente</option>
            <option value="2">Assessor</option>
            <option value="3">Atendimento</option>
            <option value="4">TeleMarketing</option>
            <option value="49">Officer</option>
            <option value="50">Ponta de Mesa</option>
            <option value="51">Supervisor</option>
            <option value="52">Risco</option>
            <option value="52">Cambio</option>
            <option value="53">Mesa Produtos</option>
            <option value="5">Outros</option>
        </select>
    </p>

    <p runat="server" id="lblSeguranca_DadosCompletos_Usuario_CodAssessor">
        <label for="txtSeguranca_DadosCompletos_Usuario_CodAssessor" >Cod. Assessor:</label>
        <input type="text" id="txtSeguranca_DadosCompletos_Usuario_CodAssessor" Propriedade="CodAssessor" maxlength="5" style="width:14em;" />
    </p>

    <p runat="server" id="lblSeguranca_DadosCompletos_Usuario_CodAssessorAssociado">
        <label for="txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado" >Cod. Assessor(es) Associado(s):</label>
        <input type="text" id="txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado" Propriedade="CodAssessorAssociado" maxlength="256" style="width:14em;" />
    </p>
    <p>&nbsp;</p>

    <p class="BotoesSubmit">
        
        <button id="btnSalvar" onclick="return btnSalvar_Seguranca_DadosCompletos_Click(this)">Salvar Alterações</button>
        
    </p>
     
</form>
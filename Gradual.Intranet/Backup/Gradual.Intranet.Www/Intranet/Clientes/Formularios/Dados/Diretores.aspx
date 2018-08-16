<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Diretores.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Diretores" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Diretores</title>
</head>
<body>
    <form id="form1" runat="server">

    <h4>Diretores Autorizados a Emitir Ordens</h4>

    <h5>Diretores Cadastrados:</h5>

    <table id="tblCadastro_Diretores" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Diretor</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovoDiretor" runat="server" class="Novo">Novo Diretor</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Diretor Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return  GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return  GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return  GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/Diretores.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_Diretor" class="pnlFormulario_Campos" style="display:none">

        <!--h5>Novo Diretor:</h5-->
        <input type="hidden" id="hidClientes_Diretores_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtClientes_Diretores_Id" Propriedade="Id" />
        <input type="hidden" id="txtClientes_Diretores_ParentId" Propriedade="ParentId" />

        <p>
            <label for="txtClientes_Diretores_NomeCompleto">Nome Completo:</label>
            <input type="text" id="txtClientes_Diretores_NomeCompleto" Propriedade="Nome" maxlength="30" class="validate[required,length[5,30]]" />
        </p>
        
        <p>
            <label for="txtClientes_Diretores_CPF">CPF/CNPJ:</label>
            <input type="text" id="txtClientes_Diretores_CPF" Propriedade="CPF" maxlength="15" class="validate[required,funcCall[validatecpfcnpj]] text-input"/>
        </p>

        <p>
            <label for="txtClientes_Diretores_Identidade">Identidade:</label>
            <input type="text" id="txtClientes_Diretores_Identidade" Propriedade="Identidade" maxlength="16"class="validate[required,custom[onlyNumber]]" />
        </p>

        <p>
            <label for="cboClientes_Diretores_OrgaoEmissor">Órgão Emissor:</label>
            <select id="cboClientes_Diretores_OrgaoEmissor"class="validate[required]" Propriedade="OrgaoEmissor">
                <option value="-1">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Diretores_OrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  

            </select>
        </p>
        <p>
            <label for="cboClientes_Diretores_UfOrgaoEmissor">UF Órgão Emissor:</label>
            <select id="cboClientes_Diretores_UfOrgaoEmissor"class="validate[required]" Propriedade="UfOrgaoEmissor">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Diretores_UfOrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>

        <p class="BotoesSubmit">
            <button id="btnSalvar" runat="server" onclick="return btnClientes_Diretores_Salvar_Click(this)">Salvar Alterações</button>
        </p>

    </div>
        
    </form>
</body>
</html>

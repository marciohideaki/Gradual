<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Telefones.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Telefones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Telefones do Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <h4>Telefones do Cliente</h4>
    
    <h5>Telefones Cadastrados:</h5>

    <table id="tblCadastro_TelefonesDoCliente" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Telefone</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovoTelefone" runat="server" class="Novo">Novo Telefone</a>
                </td>
            </tr>
        </tfoot>

        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Telefone Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/Telefones.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_Telefone" class="pnlFormulario_Campos" style="display:none">

        <!--h5>Novo Telefone:</h5-->

        <input type="hidden" id="hidClientes_Telefones_ListaJson" class="ListaJson" runat="server" value="" />

        <input type="hidden" id="txtClientes_Telefones_Id" Propriedade="Id" />
        <input type="hidden" id="txtClientes_Telefones_ParentId" Propriedade="ParentId" />

        <p>
            <label for="cboClientes_Telefones_Tipo">Tipo:</label>
            <select id="cboClientes_Telefones_Tipo" Propriedade="Tipo" class="validate[required]" style="width: 27.6em;" onchange="cboClientes_Telefones_Tipo_Change(this)">
                <option value="">[ Selecione ]</option>
                <option value="1">Residencial</option>
                <option value="2">Comercial</option>
                <option value="3">Celular</option>
                <option value="4">Fax</option>
            </select>
        </p>

        <p>
            <label for="txtClientes_Telefones_DDD">DDD:</label>
            <input type="text" id="txtClientes_Telefones_DDD" Propriedade="DDD" maxlength="2" style="float:left;width:3em" class="validate[required,custom[onlyNumber],length[2,7]] ProibirLetras" onblur="txtClientes_Telefones_DDD_OnBlur(this)" />


            <label for="txtClientes_Telefones_Numero" class="MesmaLinha">Número:</label>
            <input type="text" id="txtClientes_Telefones_Numero" Propriedade="Numero" maxlength="10" style="width:7em" class="MesmaLinha Mascara_TEL validate[required,custom[telefone]]" onfocus="txtClientes_Telefones_Numero_OnFocus(this)" />
       
            <label for="txtClientes_Telefones_Ramal" class="MesmaLinha">Ramal:</label>
            <input type="text" id="txtClientes_Telefones_Ramal" Propriedade="Ramal" maxlength="5" style="width:3em" class="MesmaLinha validate[length[0,5]] ProibirLetras"  />
        </p>

        <p style="margin-left:29%;width: 30em;">
            <input type="checkbox" id="txtClientes_Telefones_Principal" Propriedade="Principal" />
            <label for="txtClientes_Telefones_Principal" class="CheckLabel" >Utilizar este telefone como principal</label>
        </p>

        <p class="BotoesSubmit">

            <button id="btnClientes_Telefones_Salvar"  runat="server" onclick="return btnClientes_Telefones_Salvar_Click(this)">Salvar Alterações</button>

        </p>
        
    </div>
    
    </form>
</body>
</html>

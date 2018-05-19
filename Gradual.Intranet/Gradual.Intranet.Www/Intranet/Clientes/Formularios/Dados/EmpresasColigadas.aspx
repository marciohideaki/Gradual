<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpresasColigadas.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.EmpresasColigadas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro - Empresas Coligadas / Controladoras</title>
</head>
<body>
    <form id="form1" runat="server">
        <h4>Empresas Coligadas/Controladoras</h4>

        <h5>Empresas Coligadas/Controladoras Cadastradas:</h5>

        <table id="tblCadastro_EmpresasColigadas" class="Lista" cellspacing="0">
    
            <thead>
                <tr>
                    <td colspan="2">Empresas Coligadas/Controladoras</td>
                    <td>Ações</td>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td colspan="3">
                        <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovaControladora" runat="server" class="Novo">Nova Controladora</a>
                    </td>
                </tr>
            </tfoot>
    
            <tbody>
                <tr class="Nenhuma">
                    <td colspan="3">Nenhuma Coligada/Controladora Cadastrada</td>
                </tr>
                <tr class="Template" style="display:none">
                    <td style="width:1px;">
                        <input type="hidden" Propriedade="json" />
                    </td>
                    <td style="width:84%" Propriedade="Resumo()"></td>
                    <td>
                        <button class="IconButton Editar"          onclick="return  GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                        <button class="IconButton CancelarEdicao"  onclick="return  GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                        <button class="IconButton Excluir"         onclick="return  GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/EmpresasColigadas.aspx', this)"><span>Excluir</span></button>
                    </td>
                </tr>
            </tbody>
        
        </table>
        <div id="pnlFormulario_Campos_Controladora" class="pnlFormulario_Campos" style="display:none">

            <!--h5>Nova Controladora:</h5-->
            <input type="hidden" id="hidClientes_EmpresasColigadas_ListaJson" class="ListaJson" runat="server" value="" />
            <input type="hidden" id="txtClientes_EmpresasColigadas_Id" Propriedade="Id" />
            <input type="hidden" id="txtClientes_EmpresasColigadas_ParentId" Propriedade="ParentId" />

            <p>
                <label for="txtClientes_EmpresasColigadas_NomeCompleto">Razão Social:</label>
                <input type="text" id="txtClientes_EmpresasColigadas_RazaoSocial" Propriedade="RazaoSocial" maxlength="60" class="validate[required,length[5,100]]" />
            </p>
        
            <p>
                <label for="txtClientes_EmpresasColigadas_CPF">CNPJ:</label>
                <input type="text" id="txtClientes_EmpresasColigadas_CPF" Propriedade="CNPJ" maxlength="15" class="validate[required,custom[cnpj]] Mascara_CNPJ" />
            </p>

            <p class="BotoesSubmit">
                <button id="btnSalvar" runat="server" onclick="return btnClientes_EmpresasColigadas_Salvar_Click(this)">Salvar Alterações</button>
            </p>

        </div>

    </form>
</body>
</html>

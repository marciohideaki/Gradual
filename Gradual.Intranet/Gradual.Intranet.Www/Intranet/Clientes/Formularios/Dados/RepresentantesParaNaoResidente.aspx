<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepresentantesParaNaoResidente.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.RepresentantesParaNaoResidente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Representantes para Não Residente</title>
</head>
<body>
<form id="form1" runat="server">

    <h4>Representantes para Investidor Não Residente</h4>

    <h5>Representantes Cadastrados:</h5>

    <table id="tblCadastro_RepresentantesParaNaoResidente" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Representante</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovoRepresentante" runat="server" class="Novo">Novo Representante</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Representante Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/RepresentantesParaNaoResidente.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_RepresentanteParaNaoResidente" class="pnlFormulario_Campos" style="display:none">

        <!--h5>Novo Representante:</h5-->
        <input type="hidden" id="hidClientes_RepresentantesParaNaoResidente_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtClientes_RepresentantesParaNaoResidente_Id" Propriedade="Id" />
        <input type="hidden" id="txtClientes_RepresentantesParaNaoResidente_ParentId" Propriedade="ParentId" />

        
        <p>
            <label for="txtClientes_RepresentantesParaNaoResidente_NomeCompleto">Nome Completo:</label>
            <input type="text" id="txtClientes_RepresentantesParaNaoResidente_NomeCompleto" Propriedade="Nome" maxlength="500" class="validate[required,length[5,500]]" />
        </p>
        
        <p>
            <label for="txtClientes_RepresentantesParaNaoResidente_CustodianteNoPAS">Custodiante no PAS:</label>
            <input type="text" id="txtClientes_RepresentantesParaNaoResidente_CustodianteNoPAS" Propriedade="CustodianteNoPAS" maxlength="500" class="validate[required,length[5,500]]" />
        </p>
        
        <p>
            <label for="txtClientes_RepresentantesParaNaoResidente_RDE" title="Retificação de Dados do Empregador">RDE:</label>
            <input type="text" id="txtClientes_RepresentantesParaNaoResidente_RDE" Propriedade="RDE" maxlength="20" class="validate[required,custom[onlyNumber]]" />
        </p>
        
        <p>
            <label for="txtClientes_RepresentantesParaNaoResidente_CodigoCVM">Código CVM:</label>
            <input type="text" id="txtClientes_RepresentantesParaNaoResidente_CodigoCVM" Propriedade="CodigoCVM" maxlength="20" class="validate[required,custom[onlyNumber]]" />
        </p>
        
        <p>
            <label for="txtClientes_RepresentantesParaNaoResidente_PaisDeOrigem">País de Origem:</label>
            <select id="cboClientes_RepresentantesParaNaoResidente_PaisDeOrigem" Propriedade="PaisDeOrigem" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_RepresentantesParaNaoResidente_PaisDeOrigem" runat='server'>
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:Repeater>     
            </select> 
        </p>
        
        <p>
            <label for="txtClientes_RepresentantesParaNaoResidente_NomeDoAdministrador">Nome do Administrador:</label>
            <input type="text" id="txtClientes_RepresentantesParaNaoResidente_NomeDoAdministrador" Propriedade="NomeDoAdministrador" maxlength="100" class="validate[required,length[5,100]]" />
        </p>
        
        <p class="BotoesSubmit">
            <button id="btnSalvar" runat="server" onclick="return btnClientes_RepresentantesParaNaoResidente_Salvar_Click(this)">Salvar Alterações</button>
        </p>

    </div>
    
</form>
</body>
</html>

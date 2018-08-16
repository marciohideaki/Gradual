<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.Usuarios" %>


<form id="form1" runat="server">

    <h4>Usuários</h4>

    <h5>Usuários Cadastrados:</h5>

    <table class="Lista" cellspacing="0">

        <thead>
            <tr>
                <td>Usuário</td>
                <td>Restrição</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" class="Novo">Novo Usuário</a>
                </td>
            </tr>
        </tfoot>

        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhuma Usuário Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Excluir" onclick="return GradIntra_Cadastro_ExcluirItemFilho('Seguranca/Formularios/Dados/Usuarios.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlSeguranca_Usuarios_Campos" class="pnlFormulario_Campos" style="display:none">

        <input type="hidden" id="hidSeguranca_Usuarios_ListaJson" class="ListaJson" runat="server" value="" />

        <input type="hidden" id="txtSeguranca_Usuarios_Id" Propriedade="Id" />
        <input type="hidden" id="txtSeguranca_Usuarios_ParentId" Propriedade="ParentId" />

        <p class="Menor1">
            <label for="cboSeguranca_Usuarios_Usuario">Usuário:</label>
            <select id="cboSeguranca_Usuarios_Usuario" Propriedade="Item" class="validate[required]" style="width:20.6em">
                <option value="">[ Selecione ]</option>
                <asp:repeater runat="server" id="rptSeguranca_Usuarios_Usuario">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoUsuario") %>'><%# Eval("Nome") %></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>
        
        <p class="BotoesSubmit">
            
            <button id="btnSeguranca_Usuarios_Salvar" onclick="return btnSeguranca_Usuarios_Salvar_Click(this)">Salvar Alterações</button>
            
        </p>
        
    </div>

</form>
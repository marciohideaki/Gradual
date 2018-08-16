<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Grupos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.Grupos" %>


<form id="form1" runat="server">

    <h4>Grupos</h4>

    <h5>Grupos Cadastrados:</h5>

    <table class="Lista" cellspacing="0">

        <thead>
            <tr>
                <td>Grupo</td>
                <td>Restrição</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" class="Novo">Novo Grupo</a>
                </td>
            </tr>
        </tfoot>

        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhuma Grupo Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Excluir" onclick="return GradIntra_Cadastro_ExcluirItemFilho('Seguranca/Formularios/Dados/Grupos.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlSeguranca_Grupos_Campos" class="pnlFormulario_Campos" style="display:none">

        <input type="hidden" id="hidSeguranca_Grupos_ListaJson" class="ListaJson" runat="server" value="" />

        <input type="hidden" id="txtSeguranca_Grupos_Id" Propriedade="Id" />
        <input type="hidden" id="txtSeguranca_Grupos_ParentId" Propriedade="ParentId" />

        <p class="Menor1">
            <label for="cboSeguranca_Grupos_Grupo">Grupo:</label>
            <select id="cboSeguranca_Grupos_Grupo" Propriedade="Item" class="validate[required]" style="width:20.6em">
                <option value="">[ Selecione ]</option>
                <asp:repeater runat="server" id="rptSeguranca_Grupos_Grupo">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoUsuarioGrupo") %>'><%# Eval("NomeUsuarioGrupo")%></option>
                    </ItemTemplate>
                </asp:repeater>

            </select>
        </p>
        
        <p class="BotoesSubmit">
            
            <button id="btnSeguranca_Grupos_Salvar" onclick="return btnSeguranca_Grupos_Salvar_Click(this)">Salvar Alterações</button>
            
        </p>
        
    </div>

</form>
<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Itens.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.Itens" %>
	
    <form id="form1" runat="server">
    
    <h4>Itens do Grupo</h4>
    
    <div>
    
        <p style="width:50px; padding-right:20px; margin-top:5px;">
            <label style="float:right;" for="GradIntra_Risco_ItensDoGrupo_Grupos">Grupos</label>
	    </p> 

        <p class="ui-widget" style="width:350px;">
            <select class="GradIntra_ComboBox_Pesquisa AtivarAutoComplete" id="GradIntra_Risco_ItensDoGrupo_Grupos" >
		        <option value="">[Selecione]</option>
                <asp:repeater id="rptGradIntra_ComboBox_Digitavel" runat="server">
                    <itemtemplate>
		                <option value="<%# Eval("Id") %>"><%# Eval("Descricao")%></option>
                    </itemtemplate>
                </asp:repeater>
	        </select>
        </p>
    
        <p style="float: left; width: 20%; margin-left: -25px; margin-top: 5px;">
            <button class="" id="GradIntra_Risco_ItensDoGrupo_SelecionarGrupo" onclick="return GradIntra_Risco_ItensDoGrupo_SelecionarGrupo_Click(this)">Selecionar</button>
        </p>
    </div>

    <fieldset id="GradIntra_Risco_ItemDeGrupo_Fieldset" style="margin:20px 35px 35px 35px; display:none;--moz-border-radius: 7px;width: 450px;height:auto">
        
        <p style="margin-top: -20px;"></p>

        <p style="width:80px;">
            <label id="lblGradIntra_Risco_ItemDeGrupo_Descricao" style="width:100%">Adicionar Item</label>
        </p>
        <p style="width: 250px;">
            <input type="text" id="GradIntra_Risco_ItemDeGrupo_Descricao" class="validate[required,length[5,100]]" maxlength="100" style="width: 100%;"/>
        </p>
        <p style="width: 50px; float: left; margin-top: -1px">
            <button class="" id="btnGradIntra_Risco_ItemDeGrupo_AdicionarItem" onclick="return GradIntra_Risco_ItemDeGrupo_AdicionarItem_Click(this);">Incluir</button>
        </p>

        <table id="tblGradIntra_Risco_ItemGrupo" class="GridIntranet" style="margin: 0.75em 3em 1em 3em; width: 85%; z-index:-40" cellspacing="0">
            <thead>
                <tr>
                    <td style="display:none"></td>
                    <td style="display:none"></td>
                    <td align="left">Item do Grupo</td>
                    <td align="center">Excluir</td>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td colspan="4"></td>
                </tr>
            </tfoot>
            <tbody>
                <tr class="tdGradIntra_Risco_ItemGrupo_TrMatriz" style="display: none;">
                    <td class="tdGradIntra_Risco_ItemGrupo_IdItem" style="display:none"></td>
                    <td class="tdGradIntra_Risco_ItemGrupo_ItemDoBanco" style="display:none"></td>
                    <td class="tdGradIntra_Risco_ItemGrupo_DsItem" align="left"></td>
                    <td class="tdGradIntra_Risco_ItemGrupo_Excluir" align="center"><input type="button" onclick="return GradIntra_Risco_ItensDoGrupo_Excluir_Click(this);" class="IconButton Excluir" style="float: none;" /></td>
                </tr>
            </tbody>
        </table>    

        <button id="btnGradIntra_Risco_ItemDeGrupo_Gravar" onclick="return GradIntra_Risco_ItemDeGrupo_Gravar_Click(this);" style="float:right;margin-right:35px; margin-bottom:15px">Gravar</button>

    </fieldset>

    <%--<div style="float: left; display:none;">

        <table id="tblRisco_Regras" class="Lista" cellspacing="0">
    
            <thead class="ExibirSeTiverItem">
                <tr>
                    <td></td>
                    <td>Ativo</td>
                    <td>Ação</td>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td colspan="7">
                        <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" class="Novo">Nova Regra</a>
                    </td>
                </tr>
            </tfoot>

            <tbody>
                <tr class="Nenhuma">
                    <td colspan="7">Nenhum Item Cadastrado</td>
                </tr>
                <tr class="Template" style="display:none">
                    <td style="width:1px;">
                        <input type="hidden" Propriedade="json" />
                    </td>
                    <td Propriedade="Nome"></td>
                    <td>
                        <button class="IconButton Editar"          onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                        <button class="IconButton CancelarEdicao"  onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                        <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Risco/Formularios/Dados/Itens.aspx', this)"><span>Excluir</span></button>
                    </td>
                </tr>
            </tbody>
        
        </table>

        <div id="pnlFormulario_Campos_Regras" class="pnlFormulario_Campos" style="display:none">

            <!--h5>Novo Telefone:</h5-->

            <input type="hidden" id="hidPerfilDeRisco_Regras_ListaJson" class="ListaJson" runat="server" value="" />

            <input type="hidden" id="txtPerfilDeRisco_Regras_Id" Propriedade="Id" />
            <input type="hidden" id="txtPerfilDeRisco_Regras_ParentId" Propriedade="ParentId" />

            <p class="Menor1">
                <label for="txtPerfilDeRisco_Regras_Ativo">Ativo:</label>
                <input type="text" id="txtPerfilDeRisco_Regras_Ativo" Propriedade="Nome" maxlength="30" class="validate[required],length[2,30]]" />
            </p>
         
            <p class="BotoesSubmit">
            
                <button id="btnPerfilDeRisco_Regras_Salvar" onclick="return btnPerfilDeRisco_Regras_Salvar_Click(this)">Salvar Alterações</button>
            
            </p>
        
        </div>
    </div>--%>
</form>
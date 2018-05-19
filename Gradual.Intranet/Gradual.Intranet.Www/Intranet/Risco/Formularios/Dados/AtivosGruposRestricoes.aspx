<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AtivosGruposRestricoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.AtivosGruposRetricoes" %>

    <form id="form1" runat="server">
    
    <h4>Ativos / Grupo de Restrições</h4>
    
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
            <button class="" id="GradIntra_Risco_ItensDoGrupo_SelecionarGrupo" onclick="return GradIntra_Risco_Restricoes_ItensDoGrupo_SelecionarGrupo_Click(this)">Selecionar</button>
        </p>
    </div>

    <fieldset id="GradIntra_Risco_ItemDeGrupo_Fieldset" style="margin:20px 35px 35px 35px; display:none;--moz-border-radius: 7px;width: 450px;height:auto">
        
        <p style="margin-top: -20px;"></p>

        <p style="width:80px;">
            <label id="lblGradIntra_Risco_ItemDeGrupo_Descricao" style="width:100%">Instrumento</label>
        </p>
        <p style="width: 250px;">
            <input type="text" id="GradIntra_Risco_ItemDeGrupo_Descricao" class="validate[required,length[5,100]]" maxlength="100" style="width: 100%;"/>
        </p>
        <p style="width: 50px; float: left; margin-top: -1px">
            <button class="" id="btnGradIntra_Risco_ItemDeGrupo_AdicionarItem" onclick="return GradIntra_Risco_Restricoes_ItemDeGrupo_AdicionarItem_Click(this);">Gravar</button>
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
                <tr class="tdGradIntra_Risco_Restricoes_ItemGrupo_TrMatriz" style="display: none;">
                    <td class="tdGradIntra_Risco_ItemGrupo_IdItem" style="display:none"></td>
                    <td class="tdGradIntra_Risco_ItemGrupo_ItemDoBanco" style="display:none"></td>
                    <td class="tdGradIntra_Risco_ItemGrupo_DsItem" align="left"></td>
                    <td class="tdGradIntra_Risco_ItemGrupo_Excluir" align="center"><input type="button" onclick="return GradIntra_Risco_ItensDoGrupo_Excluir_Click(this);" class="IconButton Excluir" style="float: none;" /></td>
                </tr>
            </tbody>
        </table>    

        <button id="btnGradIntra_Risco_ItemDeGrupo_Gravar" onclick="return GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Click(this);" style="float:right;margin-right:35px; margin-bottom:15px">Salvar Dados</button>

    </fieldset>
</form>

<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_Permissao.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.DadosCompletos_Permissao" %>
<form id="form1" runat="server">
    <h4>Dados do Permissão</h4>

    <input type="hidden" id="hidDadosCompletos_Risco_Permissao" class="DadosJson" runat="server" value="" />
        
    <input type="hidden" id="txtPermissao_Id" Propriedade="Id" runat="server" />

    <input type="hidden" id="txtPermissao_Tipo" Propriedade="TipoDeObjeto" value="Permissao" />

    <p>
        <label for="txtRisco_DadosCompletos_Permissao_Descricao">Descrição:</label>
        <input type="text" id="txtRisco_DadosCompletos_Permissao_Descricao" Propriedade="Descricao" maxlength="100" class="validate[required,length[5,100]]" />
    </p>

    <p>
        <label for="cboRisco_DadosCompletos_Permissao_Bolsa">Bolsa:</label>
        <select id="cboRisco_DadosCompletos_Permissao_Bolsa" Propriedade="Bolsa" class="validate[required]">
            <option value="">[Selecione]</option>
            <option value="0">TODAS</option>
            <asp:Repeater id="rptRisco_DadosCompletos_Permissao_Bolsa" runat='server'>
                <ItemTemplate>
                     <option value='<%# Eval("CodigoBolsa") %>'><%# Eval("DescricaoBolsa")%></option>
                </ItemTemplate>
            </asp:Repeater>   
        </select>
    </p>
    <p class="BotoesSubmit">

        <button onclick="return btnSalvar_Risco_DadosCompletos_Click(this)">Salvar Alterações</button>

    </p>    
</form>
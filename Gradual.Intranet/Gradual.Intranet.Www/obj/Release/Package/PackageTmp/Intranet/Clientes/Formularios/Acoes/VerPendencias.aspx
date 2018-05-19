<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="VerPendencias.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.VerPendencias" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Ações - Pendências do Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <h4>Pendências Cadastrais do Cliente</h4>
    
    <h5>Pendências Cadastradas:</h5>
    
    <div class="PainelScroll">

    <table id="tblCadastro_EnderecosDoCliente" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Pendências</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="2" style="padding-right: 100%;">
                    <input type="button" id="GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor" onclick="return GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor_Click(this);" value="Notificar pendências ao assessor" />
                </td>
                <td>
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovaPendencia" runat="server" class="Novo">Nova Pendência</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhuma Pendência Cadastrada</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:80%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"         onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao" onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"        onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Acoes/VerPendencias.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_PendenciaCadastral" class="pnlFormulario_Campos" style="display:none">
        
        <input type="hidden" id="txtAcoes_Pendenciacadastral_Id" Propriedade="Id" />
        <input type="hidden" id="hidAcoes_PendenciaCadastral_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtAcoes_PendenciaCadastral_ParentId" Propriedade="ParentId" />
        <input type="hidden" id="txtAcoes_PendenciaCadastral_DataPendencia" Propriedade="DataPendencia" />
        
        <p class="Menor1">

            <label for="cboAcoes_PendenciaCadastral_Tipo">Tipo:</label>
            <select id="cboAcoes_PendenciaCadastral_Tipo" Propriedade="Tipo" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptAcoes_PendenciaCadastral_Tipo" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("IdTipoPendencia") %>'><%# Eval("DsPendencia")%></option>
                </ItemTemplate>
                </asp:Repeater>    
            </select>

        </p>

        <p class="Menor1" style="height:auto">

            <label for="txtAcoes_PendenciaCadastral_Descricao">Descrição:</label>
            <textarea id="txtAcoes_PendenciaCadastral_Descricao" style="height:6em" Propriedade="Descricao" ></textarea>        

        </p>
        
        <p class="Menor1" style="margin-left: 29%; width: 30em;">

            <input type="checkbox" id="txtAcoes_PendenciaCadastral_Resolvido" Propriedade="FlagResolvido" />
            <label for="txtAcoes_PendenciaCadastral_Resolvido"  class="CheckLabel" style="margin-right:48px;width:29%">Resolvido</label>

        </p>

        <p class="Menor1">
            <label id ="lblAcoes_PendenciaCadastral_DataResolucao" for="txtAcoes_PendenciaCadastral_DataResolucao">Resolvido em:</label>
            <input type="text" id="txtAcoes_PendenciaCadastral_DataResolucao" Propriedade="DataResolucao" TipoPropriedade="Data"  maxlength="10"  disabled="disabled" />
        </p>

        <p class="Menor1">
            <label id ="lblAcoes_PendenciaCadastral_LoginResolucao" for="txtAcoes_PendenciaCadastral_LoginResolucao">Resolvido por:</label>
            <input type="text" id="txtAcoes_PendenciaCadastral_LoginResolucao" Propriedade="DsLogin"  disabled="disabled" /> 
        </p>

        <p class="Menor1" style="height:auto">
            <label for="txtAcoes_PendenciaCadastral_Resolucao">Descrição Resolução:</label>
            <textarea id="txtAcoes_PendenciaCadastral_Resolucao"  style="height:6em" Propriedade="Resolucao" ></textarea>
        </p>

        <p  style="height:auto; text-align:center">
            <button id="btnSalvar" runat="server" onclick="return btnAcoes_Pendenciacadastral_Salvar_Click(this)">Salvar Alterações</button>
        </p>    
        
    </div>
    </div>

    </form>
</body>
</html>
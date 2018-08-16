<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Procuradores.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Procuradores" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Procuradores</title>
</head>
<body>
    <form id="form1" runat="server">

    <h4>Controladores / Adminstradores e Procuradores</h4>

    <h5>Procuradores Cadastrados:</h5>

    <table id="tblCadastro_Procuradores" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Procurador</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovoProcurador" runat="server" class="Novo">Novo Procurador</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Procurador Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return                                               GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return                                     GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/Procuradores.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_Procurador" class="pnlFormulario_Campos" url="Procuradores.aspx" TipoDeObjeto="PessoaFisica" style="display:none">

        <!--h5>Novo Representante:</h5-->
        
        <input type="hidden" id="txtClientes_Procuradores_Id" Propriedade="Id" />
        <input type="hidden" id="hidClientes_Procuradores_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtClientes_Procuradores_ParentId" Propriedade="ParentId" />

        <p>
            <label for="cboClientes_Procuradores_TipoDoRepresentante">Tipo do representante:</label>
            <select id="cboClientes_Procuradores_TipoDoRepresentante" Propriedade="TipoDoRepresentante">
                <ItemTemplate>
                    <option value="1">Administrador</option>
                    <option value="2">Controlador</option>
                    <option value="3">Procurador</option>
                </ItemTemplate>
            </select>
        </p>
        
        <p>
            <label for="txtClientes_Procuradores_NomeCompleto">Nome completo:</label>
            <input type="text" id="txtClientes_Procuradores_NomeCompleto" Propriedade="Nome" maxlength="60" class="validate[required,length[5,60]]" />
        </p>
        
        <p>
            <label for="txtClientes_Procuradores_CPF">CPF/CNPJ:</label>
            <input type="text" id="txtClientes_Procuradores_CPF" Propriedade="CPF" maxlength="15" class="validate[required,funcCall[validatecpfcnpj]] text-input" />
        </p>

        <p>
            <label for="txtClientes_Procuradores_DataNasc">Data de Nasc.:</label>
            <input type="text" id="txtClientes_Procuradores_DataNasc" Propriedade="DataNascimento" TipoPropriedade="Data" maxlength="10" class="validate[custom[data]] Mascara_Data" />
        </p>

        <p>
            <label for="txtClientes_Procuradores_Identidade">Identidade:</label>
            <input type="text" id="txtClientes_Procuradores_Identidade" Propriedade="Identidade" maxlength="16" class="validate[required,custom[onlyNumber]]" />
        </p>

        <p>
            <label for="cboClientes_Procuradores_OrgaoEmissor">Órgão Emissor:</label>
            <select id="cboClientes_Procuradores_OrgaoEmissor" Propriedade="OrgaoEmissor">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Procuradores_OrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>
        
        <p>
            <label for="cboClientes_Procuradores_UfOrgaoEmissor">UF Órgão Emissor:</label>
            <select id="cboClientes_Procuradores_UfOrgaoEmissor" Propriedade="UfOrgaoEmissor">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Procuradores_UfOrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>
        
        <p>
            <label for="cboClientes_Procuradores_TipoDocumento">Tipo Documento:</label>
            <select id="cboClientes_Procuradores_TipoDocumento" Propriedade="TipoDocumento">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Procuradores_TipoDocumento" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>

           <p>
            <label for="txtClientes_Procuradores_DataValidade">Data de Vencimento Proc./Doc.:</label>
            <input type="text" id="txtClientes_Procuradores_DataValidade" Propriedade="DataValidade" title="Deixe a Data Vazia para Procurações por Tempo Indeterminado" TipoPropriedade="Data" maxlength="10" class="validate[custom[data]] Mascara_Data" />
        </p>

        <p class="BotoesSubmit">
            <button id="btnSalvar" runat="server" onclick="return btnClientes_Procuradores_Salvar_Click(this)">Salvar Alterações</button>
        </p>
        
    </div>
    
    </form>
</body>
<!--script type="text/javascript">
    $.ready(GradIntra_Clientes_Procuradores());
</script-->
</html>

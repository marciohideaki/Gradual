<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepresentantesLegais.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.RepresentantesLegais" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Procuradores</title>
</head>
<body>
    <form id="form1" runat="server">

    <h4>Representantes Legais</h4>

    <h5>Representantes Cadastrados:</h5>

    <table id="tblCadastro_RepresentantesLegais" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Representantes Legais</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" runat="server" id="NovoRepresentante" class="Novo">Novo Representante</a>
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
                    <button class="IconButton Editar"          onclick="return                                               GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return                                     GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/RepresentantesLegais.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_Representates" class="pnlFormulario_Campos" url="RepresentantesLegais.aspx" TipoDeObjeto="PessoaFisica" style="display:none">

        <!--h5>Novo Representante:</h5-->
        
        <input type="hidden" id="txtClientes_Representantes_Id" Propriedade="Id" />
        <input type="hidden" id="hidClientes_Representantes_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtClientes_Representantes_ParentId" Propriedade="ParentId" />
        
        <p>
            <label for="txtClientes_Representantes_NomeCompleto">Nome completo:</label>
            <input type="text" id="txtClientes_Representantes_NomeCompleto" Propriedade="Nome" maxlength="60" class="validate[required,length[5,60]]" />
        </p>
        
        <p>
            <label for ="txtClientes_Representantes_CPF">CPF / CNPJ:</label>
            <input type="text" id="txtClientes_Representantes_CPF" Propriedade="CPF" maxlength="15" class="validate[required,funcCall[validatecpfcnpj]] text-input" />
        </p>

        <p>
            <label for="txtClientes_Representantes_DataNasc">Data de Nasc.:</label>
            <input type="text" id="txtClientes_Representantes_DataNasc" Propriedade="DataNascimento" TipoPropriedade="Data" maxlength="10" class="Mascara_Data validate[required,custom[data]]" />
        </p>
        
        <p>
            <label for="cboClientes_Representantes_TipoSituacaoLegal">Tipo Situação Legal:</label>
            <select id="cboClientes_Representantes_TipoSituacaoLegal" class="validate[required]"  Propriedade="TipoSituacaoLegal">
                 <option value="">[ Selecione ]</option>
                  <asp:Repeater id="rptClientes_Representantes_TipoSituacaoLegal" runat='server'>
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>
        
        <p>
            <label for="cboClientes_Representantes_TipoDocumento">Tipo Documento:</label>
            <select id="cboClientes_Representantes_TipoDocumento"class="validate[required]" Propriedade="TipoDocumento">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Representantes_TipoDocumento" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>

        <p>
            <label for="txtClientes_Representantes_Identidade">Número do documento:</label>
            <input type="text" id="txtClientes_Representantes_Identidade" Propriedade="Identidade" maxlength="16" class="validate[required]" />
        </p>

        <p>
            <label for="cboClientes_Representantes_OrgaoEmissor">Órgão Emissor:</label>
            <select id="cboClientes_Representantes_OrgaoEmissor"class="validate[required]" Propriedade="OrgaoEmissor">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Representantes_OrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>
        
        <p>
            <label for="cboClientes_Representantes_UfOrgaoEmissor">UF Órgão Emissor:</label>
            <select id="cboClientes_Representantes_UfOrgaoEmissor"class="validate[required]" Propriedade="UfOrgaoEmissor">
                 <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Representantes_UfOrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>  
            </select>
        </p>

        <p class="BotoesSubmit">

            <button id="btnSalvar" runat="server" onclick="return btnClientes_Representantes_Salvar_Click(this)">Salvar Alterações</button>

        </p>
        
    </div>
    
    </form>
</body>
</html>

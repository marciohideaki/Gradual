<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Enderecos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Enderecos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Endereços do Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <h4>Endereços do Cliente</h4>
    
    <h5>Endereços Cadastrados:</h5>
    
    <table id="tblCadastro_EnderecosDoCliente" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Endereço</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovoEndereco" runat="server" class="Novo">Novo Endereço</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Endereço Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return              GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return  GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"          onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/Enderecos.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_Endereco" class="pnlFormulario_Campos" style="display:none">
        
        <!--h5>Novo Endereço:</h5-->
        
        <input type="hidden" id="txtClientes_Enderecos_Id" Propriedade="Id" />
        <input type="hidden" id="hidClientes_Enderecos_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtClientes_Enderecos_ParentId" Propriedade="ParentId" />

        
        <p>
            <label for="cboClientes_Enderecos_Tipo">Tipo:</label>
            <select id="cboClientes_Enderecos_Tipo" Propriedade="Tipo" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Enderecos_Tipo" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("IdTipoEndereco") %>'><%# Eval("DsEndereco") %></option>
                </ItemTemplate>
                </asp:Repeater>    
            </select>
        </p>

        <p>
            <label for="txtClientes_Enderecos_Logradouro">Logradouro:</label>
            <input type="text" id="txtClientes_Enderecos_Logradouro" Propriedade="Logradouro" maxlength="30" class="validate[required,length[0,30]]" />
        </p>

        <p>
            <label for="txtClientes_Enderecos_Numero">Número:</label>
            <input type="text" id="txtClientes_Enderecos_Numero" Propriedade="Numero" maxlength="5" class="MesmaLinha validate[required]" style="width:5em" />
        
            <label for="txtClientes_Enderecos_Complemento" class="MesmaLinha">Complemento:</label>
            <input type="text" id="txtClientes_Enderecos_Complemento" Propriedade="Complemento" maxlength="10" class="MesmaLinha validate[length[0,10]]" style="width:6.3em" />
        </p>

        <p>
            <label for="txtClientes_Enderecos_Bairro">Bairro:</label>
            <input type="text" id="txtClientes_Enderecos_Bairro" Propriedade="Bairro" maxlength="18" class="MesmaLinha validate[required,length[0,18]]"  style="width:25.8%" />
        
            <label for="txtClientes_Enderecos_CEP" class="MesmaLinha">CEP:</label>
            <input type="text" id="txtClientes_Enderecos_CEP" Propriedade="CEP" maxlength="9" class="MesmaLinha Mascara_CEP validate[required,custom[cep]]"  style="width:10em" />
        </p>
        
        <p>
            <label for="cboClientes_Enderecos_Pais">País:</label>
            <select id="cboClientes_Enderecos_Pais" Propriedade="Pais" class="validate[required]" onchange='return GradIntra_Cadastro_Enderecos_InabilitarEstadoQuandoEstrangeiro($("#cboClientes_Enderecos_Pais"),$("#cboClientes_Enderecos_Estado"));'>
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Enderecos_Pais" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>    
            </select>
        </p>

        <p>
            <label for="cboClientes_Enderecos_Estado">Estado:</label>
            <select id="cboClientes_Enderecos_Estado" Propriedade="UF" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Enderecos_Estado" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
                </asp:Repeater>    
            </select>
        </p>

        <p>
            <label for="txtClientes_Enderecos_Cidade">Cidade:</label>
            <input type="text" id="txtClientes_Enderecos_Cidade" Propriedade="Cidade" maxlength="28" class="validate[required,length[0,28]]" />
        </p>

        <p style="margin-left:29%;width: 30em;">
            <input type="checkbox" id="txtClientes_Enderecos_Correspondencia" Propriedade="FlagCorrespondencia" />
            <label for="txtClientes_Enderecos_Correspondencia"  class="CheckLabel" style="margin-right:48px">Utilizar este endereço para correspondência</label>
        </p>
        
        <p class="BotoesSubmit">
            
            <button id="btnSalvar" runat="server" onclick="return btnClientes_Enderecos_Salvar_Click(this)">Salvar Alterações</button>
            
        </p>
        
    </div>

    </form>
</body>
</html>

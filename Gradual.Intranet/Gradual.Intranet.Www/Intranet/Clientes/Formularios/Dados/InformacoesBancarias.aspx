<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="InformacoesBancarias.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.InformacoesBancarias" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Contas do Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
 
    <h4>Contas do Cliente</h4>
    
    <h5>Contas Cadastradas:</h5>

    <table id="tblCadastro_ContasDoCliente" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Conta</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" runat="server" id="NovaConta" class="Novo">Nova Conta</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhuma Conta Cadastrada</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/InformacoesBancarias.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>

    </table>

    <div id="pnlFormulario_Campos_Conta" class="pnlFormulario_Campos" style="display:none">

        <!--h5>Nova Conta:</h5-->
        <input type="hidden" id="hidClientes_Conta_ListaJson" class="ListaJson" runat="server" value="" />

        <input type="hidden" id="txtClientes_Conta_Id" Propriedade="Id" />
        <input type="hidden" id="txtClientes_Conta_ParentId" Propriedade="ParentId" />

        <p>
            <label for="cboClientes_Conta_Tipo">Tipo:</label>
            <select id="cboClientes_Conta_Tipo" Propriedade="TipoConta" style="width:240px;" tabindex="4">
                <option>[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Conta_Tipo" runat='server'>
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:Repeater>        
            </select>
        </p>
        
        <p>
            <label for="cboClientes_Conta_Banco">Banco:</label>
            <select id="cboClientes_Conta_Banco" class="validate[required]" style="width:240px;" Propriedade="Banco" onchange="cboClientes_Conta_Banco_OnChange(this);" tabindex="5">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptClientes_Conta_Banco" runat='server'>
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:Repeater>        
            </select>
        </p>

        <p>
            <label for="txtClientes_Conta_Agencia">Agência:</label>
            <input type="text" id="txtClientes_Conta_Agencia"        class="validate[required,custom[onlyNumber]] ProibirLetras" maxlength="5" style="width:186px;" propriedade="Agencia" onkeyup="TabProximoControleAposAtingirTamanhoMaximoDoCampo(this);" title="Agência" tabindex="5" />
            <input type="text" id="txtClientes_Conta_Agencia_Digito" class="validate[custom[onlyNumber]] ProibirLetras" maxlength="2"style="width: 39px;"  propriedade="AgenciaDigito"  onkeyup="TabProximoControleAposAtingirTamanhoMaximoDoCampo(this);" title="Dígito da agência"  tabindex="7"/>
        </p>

        <p>
            <label for="txtClientes_Conta_NumeroDaConta">Conta:</label>
            <input type="text" id="txtClientes_Conta_NumeroDaConta" maxlength="13" Propriedade="ContaCorrente" style="width: 186px;" class="validate[required,custom[onlyNumber]] ProibirLetras" onblur="return txtClientes_Conta_DigitoDaConta_OnBlur(this);" onkeyup="TabProximoControleAposAtingirTamanhoMaximoDoCampo(this);" title="Conta"  tabindex="8"/>
            <input type="text" id="txtClientes_Conta_DigitoDaConta" maxlength="02" Propriedade="ContaDigito"   style="width: 39px !important" title="Dígito da conta" onkeyup="TabProximoControleAposAtingirTamanhoMaximoDoCampo(this);" class="validate[required,custom[onlyNumber]]" tabindex="9"/>
        </p>

        <p style="margin-left:29%;width:24em;">
            <input type="checkbox" id="txtClientes_Conta_Principal" Propriedade="Principal" />
            <label for="txtClientes_Conta_Principal" class="CheckLabel" style="margin-right:110px" tabindex="10">Utilizar esta conta como principal</label>
        </p>

        <p>
            Caso a conta seja de outra titularidade:
        </p>
        <p>
            <label for="txtClientes_Conta_NomeDoTitular">Nome do Titular da Conta:</label>
            <input type="text" id="txtClientes_Conta_NomeDoTitular" maxlength="60" Propriedade="NomeTitular" style="width: 186px;" class="" title="Nome do Titular da Conta" />
        </p>
        
        <p>
            <label for="txtClientes_Conta_CPFDoTitular">CPF do Titular da Conta:</label>
            <input type="text" id="txtClientes_Conta_CPFDoTitular" maxlength="13" Propriedade="CPFTitular" style="width: 186px;" class="ProibirLetras" title="Nome do Titular da Conta" />
        </p>

        <p class="BotoesSubmit">
            
            <button id="btnSalvar" runat="server" onclick="return btnClientes_InformacaoBancaria_Salvar_Click(this)">Salvar Alterações</button>
            
        </p>

    </div>

    </form>
</body>
</html>

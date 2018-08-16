<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PessoasAutorizadas.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.PessoasAutorizadas" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Procuradores</title>
</head>
<body>
    <form id="form1" runat="server">

    <h4>Emitentes</h4>

    <h5>Emitentes Cadastrados:</h5>

    <table id="tblCadastro_PessoasAutorizadas" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Emitentes</td>
                <td>Ações</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovaPessoaAutorizada" runat="server" class="Novo">Novo Emitente</a>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Emitente Cadastrado</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:84%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return                                               GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return                                     GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                    <button class="IconButton Excluir"         onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Dados/PessoasAutorizadas.aspx', this)"><span>Excluir</span></button>
                </td>
            </tr>
        </tbody>
        
    </table>

    <div id="pnlFormulario_Campos_PessoasAutorizadas" class="pnlFormulario_Campos" url="PessoasAutorizadas.aspx" TipoDeObjeto="PessoaFisica" style="display:none">

        <!--h5>Novo Representante:</h5-->
        
        <input type="hidden" id="txtClientes_PessoasAutorizadas_Id" Propriedade="Id" />
        <input type="hidden" id="hidClientes_PessoasAutorizadas_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtClientes_PessoasAutorizadas_ParentId" Propriedade="ParentId" />
        
        <p>
            <label for="txtClientes_PessoasAutorizadas_NomeCompleto">Nome completo:</label>
            <input type="text" id="txtClientes_PessoasAutorizadas_NomeCompleto" Propriedade="Nome" maxlength="30" class="validate[required,length[5,30]]" />
        </p>
        
        <p>
            <label for="txtClientes_PessoasAutorizadas_CPFCNPJ">CPF / CNPJ:</label>
            <input type="text" id="txtClientes_PessoasAutorizadas_CPFCNPJ" Propriedade="CPFCNPJ" maxlength="15" class="validate[required,funcCall[validatecpfcnpj]] text-input ProibirLetras" />
           
        </p>

        <p>
            <label for="txtClientes_PessoasAutorizadas_DataNasc">Data de Nasc.:</label>
            <input type="text" id="txtClientes_PessoasAutorizadas_DataNasc" Propriedade="DataNascimento" TipoPropriedade="Data" maxlength="10" class="Mascara_Data validate[required,custom[data]]" />
        </p>

        <p>
            <label for="txtClientes_PessoasAutorizadas_Identidade">Identidade:</label>
            <input type="text" id="txtClientes_PessoasAutorizadas_Identidade" Propriedade="Identidade" maxlength="16" class="validate[required]" />
        </p>

        <p>
            <label for="cboClientes_PessoasAutorizadas_CodigoSistema">Sistema:</label>
            <select id="cboClientes_PessoasAutorizadas_CodigoSistema" Propriedade="CodigoSistema" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <option value="BOL">BOVESPA</option>
                <option value="BMF">BM&F</option>
                
            </select>
        </p>

        <p>
            <label for="txtClientes_PessoasAutorizadas_Email">E-mail:</label>
            <input type="text" id="txtClientes_PessoasAutorizadas_Email" Propriedade="Email" maxlength="80"  />
        </p>
        
        <p style="margin-left: 29%; width:24em;">
            <input type="checkbox" id="txtClientes_PessoasAutorizadas_Correspondencia" Propriedade="FlagPrincipal" />
            <label for="txtClientes_PessoasAutorizadas_Correspondencia"  class="CheckLabel" style="margin-right:110px">Emitente principal </label>
        </p>

        <p class="BotoesSubmit">
            <button id="btnSalvar" runat="server" onclick="return btnClientes_PessoasAutorizadas_Salvar_Click(this)">Salvar Alterações</button>
        </p>
        
    </div>
    
    </form>
</body>
</html>



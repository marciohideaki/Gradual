<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitacaoAlteracaoCadastral.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.SolicitacaoAlteracaoCadastral" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Ações - Solicitação de Alteração Cadastral</title>
</head>
<body>
    <form id="form1" runat="server" >
    <%--class="pnlFichaCadastral"--%>
    
    <h4>Solicitações de Alterações Cadastrais do Cliente</h4>
    
    <h5>Solicitações Cadastradas:</h5>
    
    <div class="PainelScroll">
    
        <table id="tblCadastro_SolicitacoesDoCliente" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Solicitações</td>
                <td>Ações</td>
            </tr>
        </thead>
           <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovaSolicitacaoAlteracao" runat="server" class="Novo">Nova Alteração</a>
                </td>
            </tr>
        </tfoot>
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhuma Solicitação Cadastrada</td>
            </tr>
            <tr class="Template" style="display:none">
                <td style="width:1px;">
                    <input type="hidden" Propriedade="json" />
                </td>
                <td style="width:80%" Propriedade="Resumo()"></td>
                <td>
                    <button class="IconButton Editar"          onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                    <button class="IconButton CancelarEdicao"  onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                </td>
            </tr>
        </tbody>
    </table>

      <div id="pnlFormulario_Campos_SolicitacaoAlteracaoCadastral" class="pnlFormulario_Campos" style="display:none">
        <input type="hidden" id="txtAcoes_Alteracaocadastral_Id" Propriedade="Id" />
        <input type="hidden" id="hidAcoes_AlteracaoCadastral_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtAcoes_AlteracaoCadastral_ParentId" Propriedade="ParentId" />
        <p class="Menor1">
            <label for="cboClientes_AlteracaoCadastral_Tipo">Tipo:</label>
               <select id="cboClientes_AlteracaoCadastral_Tipo" Propriedade="CdTipo" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <option value="A">Alteração</option>
                <option value="I">Inclusão</option>
                <option value="E">Exclusão</option>
              </select>  
        </p>
            <p class="Menor1">
            <label for="cboClientes_AlteracaoCadastral_Informacao">Informação:</label>
            <select id="cboClientes_AlteracaoCadastral_Informacao" Propriedade="DsInformacao" class="validate[required]">
                <option value="">[ Selecione ]</option>
                <option value="Dados Cadastrais">Dados Cadastrais</option>
                <option value="Dados do Representante Legal/P">Dados do Representante Legal/Procurador</option>
                <option value="Adesão ao contrato de Intermediação" style="display:none">Adesão ao contrato de Intermediação e Custódia</option>
                <option value="Telefone">Telefone</option>
                <option value="Endereço">Endereço</option>
                <option value="Conta Bancária">Conta Bancária</option>
                <option value="Documento">Documento</option>
                <option value="Documento não confere">Documento não confere com a ficha cadastral</option>
            </select> 
        </p>
        <p class="Menor1" style="height:auto">

            <label for="txtAcoes_AlteracaoCadastral_Descricao">Descrição:</label>
             <textarea class="validate[length[0,4000]"  id="txtAcoes_AlteracaoCadastral_Descricao" Propriedade="DsDescricao" style="height:6em" ></textarea>
        </p>
        <p class="Menor1" style="height:auto">
            <label for="txtAcoes_AlteracaoCadastral_LoginSolicitacao">Solicitado por:</label>
            <input type="text" id="txtAcoes_AlteracaoCadastral_LoginSolicitacao" Propriedade="DsLoginSolicitante" disabled="disabled" />             
        </p>
        <p class="Menor1">
            <label for="txtAcoes_AlteracaoCadastral_DataSolicitacao">Solicitado em:</label>
            <input type="text" id="txtAcoes_AlteracaoCadastral_DataSolicitacao" Propriedade="DtSolicitacao" TipoPropriedade="Data" maxlength="10"  disabled="disabled" />
        </p>
        <p class="Menor1"style="margin-left: 29%; width: 30em;">
            <input type="checkbox" id="txtAcoes_AlteracaoCadastral_Resolvido" Propriedade="StResolvido" />
            <label for="txtAcoes_AlteracaoCadastral_Resolvido" class="CheckLabel" style="margin-right:48px;width:29%">Resolvido</label>
        </p>
        <p class="Menor1">
            <label id ="lblAcoes_AlteracaoCadastral_DataResolucao" for="txtAcoes_AlteracaoCadastral_DataResolucao">Resolvido em:</label>
            <input type="text" id="txtAcoes_AlteracaoCadastral_DataResolucao" Propriedade="DtRealizacao" TipoPropriedade="Data"  maxlength="10"  disabled="disabled" />
        </p>
        <p class="Menor1" style="height:auto">
            <label id ="lblAcoes_AlteracaoCadastral_LoginResolucao" for="txtAcoes_AlteracaoCadastral_LoginResolucao">Resolvido por:</label>
            <input type="text" id="txtAcoes_AlteracaoCadastral_LoginResolucao" Propriedade="DsLoginRealizacao"  disabled="disabled" />             
        </p>
        <p class="BotoesSubmit" style="height:auto">
            <button id="btnSalvar" runat="server" onclick="return btnAcoes_alteracaocadastral_Salvar_Click(this)">Salvar</button>
        </p>
    </div>
    </div>
    </form>
</body>
</html>

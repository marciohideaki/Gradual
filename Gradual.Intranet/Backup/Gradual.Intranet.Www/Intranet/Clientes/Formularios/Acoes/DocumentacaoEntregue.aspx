<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentacaoEntregue.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.DocumentacaoEntregue" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clientes - Histórico de Envio de Documentação Cadastral</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h4>Documentação Entregue</h4>

        <input type="hidden" id="hidAcoes_DocumentacaoEntregue_ListaJson" class="ListaJson" runat="server" />

        <p style="display: none;">
            <label for="chkGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue">Documentação entregue</label>
            <input type="checkbox" id="chkGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue" onclick="chkGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue_Click(this);" />
        </p>
        
        <div class="PainelScroll">

            <table id="tblGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue" class="Lista" cellspacing="0">
    
                <thead>
                    <tr>
                        <td colspan="2">Data/Hora Recebimento</td>
                        <td>Ação</td>
                    </tr>
                </thead>

                <tfoot>
                    <tr>
                        <td colspan="3">
                            <a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" id="NovaDocumentacao" runat="server" class="Novo">Registrar recebimento de documentação</a>
                        </td>
                    </tr>
                </tfoot>
    
                <tbody>
                    <tr class="Nenhuma">
                        <td colspan="3">Nenhum recebimento documentação cadastrado</td>
                    </tr>
                    <tr class="Template" style="display:none">
                        <td style="width:1px;">
                            <input type="hidden" Propriedade="json" />
                        </td>
                        <td style="width:80%" Propriedade="Resumo()"></td>
                        <td>
                            <button class="IconButton Editar"         onclick="return GradIntra_Cadastro_EditarItemFilho(this)"><span>Editar</span></button>
                            <button class="IconButton CancelarEdicao" onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)"><span>Cancelar Edição</span></button>
                            <button class="IconButton Excluir"        onclick="return GradIntra_Cadastro_ExcluirItemFilho('Clientes/Formularios/Acoes/DocumentacaoEntregue.aspx', this)"><span>Excluir</span></button>
                        </td>
                    </tr>
                </tbody>
        
            </table>

            <div id="pnlGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue" class="pnlFormulario_Campos" style="display:none">
        
                <input type="hidden" id="txtAcoes_DocumentacaoEntregue_IdCliente" Propriedade="IdCliente" />
                <input type="hidden" id="txtAcoes_DocumentacaoEntregue_IdDocumentacaoEntregue" Propriedade="Id" />
                <input type="hidden" id="txtAcoes_DocumentacaoEntregue_IdLoginUsuarioLogado" Propriedade="IdLoginUsuarioLogado" />

                <p class="Menor1" style="display: none;">
                    <label id ="txtAcoes_DocumentacaoEntregue_DtAdesaoDocumento" for="txtAcoes_PendenciaCadastral_LoginResolucao">Data de adesão do documento:</label>
                    <input type="text" id="txtAcoes_DocumentacaoEntregue_DtAdesaoDocumento" Propriedade="DtAdesaoDocumento"  disabled="disabled" /> 
                </p>

                <p class="Menor1" style="height:auto;">
                    <label for="txtAcoes_DocumentacaoEntregue_DsObservacao">Descrição:</label>
                    <textarea id="txtAcoes_DocumentacaoEntregue_DsObservacao" style="height:6em" Propriedade="DsObservacao" ></textarea>        
                </p>

                <p  style="height:auto; text-align:center">
                    <button id="btnGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue_Salvar" runat="server" onclick="return btnGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue_Salvar_Click(this)">Salvar Alterações</button>
                </p>    

            </div>

        </div>

    </div>
    </form>
</body>
</html>

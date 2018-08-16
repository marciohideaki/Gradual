<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Permissoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.Permissoes" %>


<form id="form1" runat="server">

    <script type="text/javascript">
        $.ready(GradIntra_Seguranca_AssociarPermissoes());
    </script>
    <h4>Permissões</h4>


    <div class="PainelScroll">
    <div id="pnlSeguranca_Permissoes_Campos" class="pnlFormulario_Campos">

        <input type="hidden" id="hidSeguranca_Permissoes_ListaJson" class="ListaJson" runat="server" value="" />

        <input type="hidden" id="txtSeguranca_Permissoes_Id" Propriedade="Id" />
        <input type="hidden" id="txtSeguranca_Permissoes_ParentId" Propriedade="ParentId" />

        <table cellspacing="0" style="width:98%;" id="pnlSeguranca_Permissoes_Resultados" class="GridIntranet">
        <thead>
            <tr>
                <td><input type="checkbox" id="chkSeguranca_Associacoes_Permissoes_SelecionarTodos" onclick="chkMonitoramento_ResultadoOrdensStop_SelecionarTodosResultados_OnClick(this)" /> <label for="chkMonitoramento_ResultadoOrdens_SelecionarTodos">&nbsp;</label></td>
                <td style="text-align:center">Permissão</td>
                <td style="text-align:center">Descrição</td>
            </tr>
        </thead>

        <tbody>
        
        <asp:repeater runat="server" id="rptSeguranca_Permissoes_Permissao">
            
            <ItemTemplate>

                <tr>
                    <td>
                        <label for="chkSeguranca_Associacoes_Permissoes_<%# Eval("CodigoPermissao") %>" style="text-align:left;">&nbsp;</label>
                        <input name="chkSeguranca_Associacoes_Permissoes" type="checkbox" TipoPropriedade="Lista" Propriedade="Permissoes" ValorQuandoSelecionado="<%# Eval("CodigoPermissao") %>" id="chkSeguranca_Associacoes_Permissoes_<%# Eval("CodigoPermissao") %>"  />
                    </td>
                    <td><%# Eval("NomePermissao") %></td>
                    <td><%# Eval("DescricaoPermissao") %></td>
                </tr>
            
            </ItemTemplate>

        </asp:repeater>
        
        </tbody>
        
        </table>
       
    </div>
    </div>
    <p>  
        <button class="BotoesSubmit" id="btnSeguranca_Permissoes_Salvar" onclick="return btnSeguranca_Permissoes_Salvar_Click('pnlSeguranca_Permissoes_Campos')">Salvar Permissões</button>
    </p>

    <script>
    
        //$.ready(VerificaBotaoSalvar());

        function VerificaBotaoSalvar() 
        {
            if (gGradIntra_Cadastro_ItemSendoEditado.TipoDeObjeto == "Usuario") 
            {
                $("#btnSeguranca_Permissoes_Salvar").hide();
            }
        }

    </script>
</form>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssociarPermissoesParametros.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.AssociarPermissoesParametros"
    EnableViewState="false" %>

<form id="form1" runat="server">

<script type="text/javascript">
    $.ready(GradIntra_Risco_AssociarPermissoes());
    $.ready(rboRisco_Associacoes_Permissoes_Click());
</script>

<h4>Parâmetros e Permissões Associadas</h4>

<div class="PainelScroll">
    <table id="tblRisco_AssociarPermissoesParametros" class="Lista" cellspacing="0">
    
        <thead>
            <tr>
                <td colspan="2">Associações</td>
                <td>Ações</td>
            </tr>
        </thead>
    
        <tfoot>
            <tr>
                <td colspan="3"><a href="#" onclick="return GradIntra_Cadastro_NovoItemFilho(this)" class="Novo">Nova Regra de Limite</a></td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr class="Nenhuma">
                <td colspan="3">Nenhum Item Cadastrado</td>
            </tr>
            <tr class="Template" style="display: none">
                <td style="width: 1px;"><input type="hidden" propriedade="json" /></td>
                <td style="width: 84%" propriedade="Resumo()"></td>
                <td>
                    <!-- <button class="IconButton Salvar"          onclick="return GradIntra_Cadastro_SalvarItemFilhoDiretoDoGrid(this, 'Clientes/Formularios/Acoes/AssociarPermissoesParametros.aspx')"><span>Salvar</span></button>-->
                    <button class="IconButton Editar" onclick="return GradIntra_Cadastro_EditarItemFilho(this)">
                        <span>Editar</span></button>
                    <button class="IconButton CancelarEdicao" onclick="return GradIntra_Cadastro_CancelarEdicaoDeItemFilho(this)" title="Cancelar a edição">
                        <span>Cancelar Edição</span></button>
                </td>
            </tr>
        </tbody>
    </table>
    
    <div id="pnlFormulario_Campos_AssociarPermissoesParametros" class="pnlFormulario_Campos">
        
        <input type="hidden" id="txtRisco_AssociarPermissoesParametros_Id" propriedade="Id" />
        <input type="hidden" id="hidRisco_AssociarPermissoesParametros_ListaJson" class="ListaJson" runat="server" value="" />
        <input type="hidden" id="txtRisco_AssociarPermissoesParametros_ParentId" propriedade="ParentId" />
        <input type="hidden" id="hidRisco_AssociarPermissoesParametros_CBLC" propriedade="CodBovespa" runat="server" />
        <input type="hidden" id="hidRisco_AssociarPermissoesParametros_BMF" propriedade="CodBMF" runat="server" />
        <input type="hidden" id="hidRisco_ListaPermissoesAssociadas" runat="server" value="" />
        
        <p class="Menor1" style="padding-left: 120px; margin-bottom: 24px">
            <label for="rdoRisco_Associacoes_Permissao" style="text-align: left">Permissão</label>
            <input name="rdoRisco_Associacoes_ParametroPermissao" type="radio" propriedade="EhPermissao" id="rdoRisco_Associacoes_Permissao" onclick="rboRisco_Associacoes_Permissoes_Click()" checked />
            
            <label for="rdoRisco_Associacoes_Parametro" style="text-align: left">Limite</label>
            <input name="rdoRisco_Associacoes_ParametroPermissao" type="radio" propriedade="EhParametro" id="rdoRisco_Associacoes_Parametro" onclick="rboRisco_Associacoes_Parametro_Click()" />
        </p>
        
        <p style="padding-bottom:10px; padding-left:120px; display:none;" class="Risco_Associacoes_Atualizacao">
            <label for="rdoRisco_Associacoes_RenovacaoLimite" style="text-align: left; width: 8em;" title="Esta opção habilita a alteração de Validade e Valor do limite.">Renovação</label>
            <input name="rdoRisco_Associacoes_Limite" type="radio" propriedade="EhRenovacaoLimite" id="rdoRisco_Associacoes_RenovacaoLimite" onclick="rdoRisco_Associacoes_RenovacaoLimite_Click()" />
            
            <label for="rdoRisco_Associacoes_ExpirarLimite" style="text-align: left" title="Esta opção cancela o limite atual do cliente.">Expirar Limite</label>
            <input name="rdoRisco_Associacoes_Limite" type="radio" propriedade="EhExpirarLimite" id="rdoRisco_Associacoes_ExpirarLimite" onclick="rdoRisco_Associacoes_ExpirarLimite_Click()" />
        </p>

        <div id="pnlFormulario_Campos_AssociarPermissoesParametros_Parametro" style="padding-left:12px; display: none;">

            <fieldset>
                <legend>Configurar Limite</legend>
                <p style="padding-top:10px;">
                    <label for='cbo_Risco_AssociarPermissoesParametros_Parametro' id="lbl_Risco_AssociarPermissoesParametrosAssociarPermissoesParametros_Parametro">Parametros</label>
                    <label id="txt_Risco_AssociarPermissoesParametros_Parametro" style="text-align:left; width:25em; padding-left:7.5em; display:none;"></label>
                    <select id="cbo_Risco_AssociarPermissoesParametros_Parametro" propriedade="CodigoParametro">
                        <option value="0">[Selecione]</option>
                        <asp:repeater runat="server" id="rpt_Risco_AssociarPermissoesParametros_Parametros">
                        <ItemTemplate>
                            <option value='<%# Eval("CodigoParametro") %>'><%# Eval("NomeParametro")%></option>
                        </ItemTemplate>
                    </asp:repeater>
                    </select>
                </p>
                <p>
                    <label for='cbo_Risco_AssociarPermissoesParametros_Grupo'>Restringir por Grupo</label>
                    <label id="txt_Risco_AssociarPermissoesParametros_Grupo" style="text-align:left; display:none;"></label>
                    <select id="cbo_Risco_AssociarPermissoesParametros_Grupo" propriedade="CodigoGrupo">
                    <option value="0">[Selecione]</option>
                    <option value="0">Não Restringir</option>
                    <asp:repeater runat="server" id="rpt_Risco_AssociarPermissoesParametros_Grupo">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoGrupo") %>'><%# Eval("NomeDoGrupo") %></option>
                    </ItemTemplate>
                </asp:repeater>
                </select>
                </p>
                <p>
                    <label for='txt_Risco_AssociarPermissoesParametros_Valor'>Valor</label>
                    <input type="text" id="txt_Risco_AssociarPermissoesParametros_Valor" maxlength="14" propriedade="ValorParametro" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
                </p>
                <p>
                    <label for='txt_Risco_AssociarPermissoesParametros_Data'>Validade</label>
                    <input type="text" id="txt_Risco_AssociarPermissoesParametros_Data" propriedade="DataValidadeParametro" maxlength="10" style="width: 6em" class="Mascara_Data" />
                </p>
            
            </fieldset>
        </div>

        <div id="pnlFormulario_Campos_AssociarPermissoesParametros_Permissao">
            <asp:repeater runat="server" id="rpt_Risco_AssociarPermissoesParametros_Permissoes_chk">
            <ItemTemplate>
            <p class="CheckboxAEsquerda">
                <label for="chkRisco_Associacoes_Permissoes_<%# Eval("CodigoPermissao") %>" style="text-align:left;"><%# Eval("NomePermissao") %></label>
                <input name="chkRisco_Associacoes_Permissoes" type="checkbox" TipoPropriedade="Lista" Propriedade="Permissoes" ValorQuandoSelecionado="<%# Eval("CodigoPermissao") %>" id="chkRisco_Associacoes_Permissoes_<%# Eval("CodigoPermissao") %>"  />
            </p>
            </ItemTemplate>
            </asp:repeater>
        </div>
    </div>
</div>
<p class="BotoesSubmit">
    <button onclick="return btnSalvar_Risco_AssociarPermissoesParametros_Click(this)">
        Salvar Alterações</button>
</p>
</form>

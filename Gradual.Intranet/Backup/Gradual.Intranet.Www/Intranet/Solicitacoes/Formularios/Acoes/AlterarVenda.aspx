<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlterarVenda.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Acoes.AlterarVenda" %>



<form id="form1" runat="server">

    <h4>Alterar Venda</h4>

    <div id="pnlFormulario_AlterarVenda" class="pnlFormulario_Campos">

        <p>
            <label for="cboAlterarVenda_Propriedade">Alterar:</label>
            <select id="cboAlterarVenda_Propriedade" Propriedade="DsPropriedades">
                <option value="Status" selected>Status</option>
            </select>
            
            <input id="hidAlterarVenda_Propriedade_IdVenda" type="hidden" Propriedade="IdVenda" />
            <input id="hidAlterarVenda_Propriedade_ValorOriginal" type="hidden" Propriedade="DsValoresAnteriores" />
        </p>
        <p data-propriedade="Status">
            <label for="cboAlterarVenda_Status_NovoValor">Alterar Para:</label>
            <select id="cboAlterarVenda_Status_NovoValor" Propriedade="DsValoresModificados">
                <option value="1">1 - Aguardando Pagamento</option>
                <option value="2">2 - Em Análise</option>
                <option value="3">3 - Paga</option>
                <option value="4">4 - Disponível</option>
                <option value="5">5 - Em Disputa</option>
                <option value="6">6 - Devolvida</option>
                <option value="7">7 - Cancelada</option>
            </select>
        </p>
        <p>
            <label for="txtAlterarVenda_Motivo">Motivo / Descrição da Alteração:</label>
            <input  id="txtAlterarVenda_Motivo" type="text" class="validate[required]" Propriedade="DsMotivo" />
        </p>
        <p class="BotoesSubmit">

            <button id="btnAlterarVenda_Salvar"  runat="server" onclick="return btnAlterarVenda_Salvar_Click(this)">Salvar Alteração</button>

        </p>

    </div>

    <div class="PainelScroll" style="height:306px">
    
        <table id="tblAlteracoesDaVenda" cellspacing="0" class="GridIntranet" style="width:99%">
    
            <thead>
                <tr>
                    <td style="width: 5%">ID</td>
                    <td style="width:15%">Propriedade</td>
                    <td style="width:15%">Valor Anterior</td>
                    <td style="width:15%">Valor Modificado</td>
                    <td style="width:15%">Data / Hora</td>
                    <td style="width:10%">Usuário</td>
                    <td style="width:20%">Motivo</td>
                </tr>
            </thead>
            <tfoot>
                <td colspan="8">&nbsp;</td>
            </tfoot>

            <tbody>

                <asp:repeater id="rptHistoricoDeAlteracoes" runat="server">
                <itemtemplate>

                <tr>
                    <td style="white-space:nowrap"><%# Eval("IdVendaAlteracao") %></td>
                    <td style="white-space:nowrap"><%# Eval("DsPropriedades") %></td>
                    <td style="white-space:nowrap"><%# Eval("DsValoresAnteriores") %></td>
                    <td style="white-space:nowrap"><%# Eval("DsValoresModificados") %></td>
                    <td style="white-space:nowrap"><%# Eval("DtData") %></td>
                    <td style="white-space:nowrap"><%# Eval("DsUsuario") %></td>
                    <td style="white-space:nowrap"><%# Eval("DsMotivo") %></td>
                </tr>

                </itemtemplate>
                </asp:repeater>

            </tbody>

        </table>


    </div>

</form>

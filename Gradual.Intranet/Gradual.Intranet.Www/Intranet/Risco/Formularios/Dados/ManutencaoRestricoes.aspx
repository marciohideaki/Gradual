<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManutencaoRestricoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.ManutencaoRestricoes" %>

    <form id="form1" runat="server">
    <input type="hidden" id="hidClienteLimites" runat="server" />

        <ul class="pnlFormulario_Abas_Container">
            <li class="Selecionada"><a href="#" rel="pnlCliente_Restricoes_Grupo" onclick="return pnlFormulario_Abas_li_a_Restricoes_Click(this)">Regra para o Grupo</a></li>
            <li><a href="#" rel="pnlCliente_Restricoes_Grupo_Global" onclick="return pnlFormulario_Abas_li_a_Restricoes_Click(this)">Regra para o Grupo Global</a></li>
            <li><a href="#" rel="pnlCliente_Restricoes_Individual" onclick="return pnlFormulario_Abas_li_a_Restricoes_Click(this)">Regra Individual</a></li>
        </ul>

        <div id="pnlRestricoesPorCliente" style="padding-top:3%">
                <div id="pnlCliente_Restricoes_RestricaoPorAtivo">
                    <h4>Restrições para envio de ordens pelo OMS</h4>

                     <%--<p style="width:50px; padding-right:20px; margin-top:5px;">
                     <label style="float:right;" for="GradIntra_Risco_ItensDoGrupo_Grupos">Grupos:</label>
	                </p> --%>

                    <div id="pGradIntra_Risco_ItensDoGrupo_Grupos">
                        <p style="width: 50px; padding-right: 20px; margin-top: 5px">
                            <label for="GradIntra_Risco_ItensDoGrupo_Grupos" style="width:60px">Grupos:</label>
                        </p>

                        <p class="ui-widget" style="width:350px" >
                            <select class="GradIntra_ComboBox_Pesquisa AtivarAutoComplete" id="GradIntra_Risco_ItensDoGrupoRestricoes_Grupos" style="width:250px" >
		                        <option value="">[Selecione]</option>
                                <asp:repeater id="rptGradIntra_ComboBox_Digitavel" runat="server">
                                    <itemtemplate>
		                                <option value="<%# Eval("Id") %>"><%# Eval("Descricao")%></option>
                                    </itemtemplate>
                                </asp:repeater>
	                        </select>
                        </p>

                        <p style="float: left; width: 20%; margin-left: -25px; margin-top: 5px;">
                            <button class="" id="GradIntra_Risco_Restricoes_SelecionarGrupo" onclick="return GradIntra_Risco_Restricoes_SelecionarGrupo_Click(this)">Selecionar</button>
                        </p>
                    </div>

                    <br />
                    <p id="pCliente_Restricoes_RestricaoPorCliente" >
                        <label style="width:76px;">Cliente:</label>
                        <input type="text" id="txtCliente_Restricoes_RestricaoPorCliente" style="width: 12em;" maxlength="10" />
                    </p>
                    <br />
                    <p id="pCliente_Restricoes_RestricaoPorAtivo" style="display:none">
                        <label style="width:76px">Instrumento:</label>
                        <input type="text" id="txtCliente_Restricoes_RestricaoPorAtivo" style="width: 12em; text-transform: uppercase" maxlength="10" />
                    </p>

                    <%--style="display:none;"--%>
                    <div class="custom-checkbox" id="DivGradIntraRisco_Restricao_Global_OMS" style="display:none">
                        <input type="checkbox" id="ckbGradIntraRisco_Restricao_Global_OMS" propriedade="ckbGradIntraRisco_Restricao_Global_OMS" checked="checked">
                        <label for="ckbGradIntraRisco_Restricao_Global_OMS">Restringir Global para o OMS</label>
                    </div>
                    <p class="RadiosRestricaoPorAtivos" style="width: 300px;">
                        <label style="width:60px">Direção</label>
                        <input type="radio" name="RestricaoPorAtivo" id="rdbRestricaoPorAtivoAmbos" value="A" checked="true" />
                        <label id="Label2" for="rdbRestricaoPorAtivoAmbos">Ambos</label>

                        <input type="radio" name="RestricaoPorAtivo" id="rdbRestricaoPorAtivoCompra" value="C" />
                        <label id="lblRestricaoPorAtivo" for="rdbRestricaoPorAtivoCompra">Compra</label>
                
                        <input type="radio" name="RestricaoPorAtivo" id="rdbRestricaoPorAtivoVenda" value="V" />
                        <label id="Label1" for="rdbRestricaoPorAtivoVenda">Venda</label>
                    </p>
                </div>
                <p style="width: 50px; float: left; margin-top: -1px">
                    <button class="" id="btnGradIntra_Risco_ItemDeGrupo_AdicionarItem" onclick="return GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItem_Click(this);">Gravar</button>
                </p>
                <div id="DivtblGradIntra_Risco_ItemCliente" style="display:none; margin-top:75px;">
                <%--margin: 0.75em 3em 1em 3em; width: 85%; z-index:-40;--%>
                <table id="tblGradIntra_Risco_ItemCliente" class="GridIntranet" style="width: 70%; margin: 0em 0em 0em 0em; z-index:-40;" cellspacing="0" >
                    <thead>
                        <tr>
                            <td style="display:none"></td>
                            <td style="display:none"></td>
                            <td style="display:none"></td>
                            <td align="left">Cliente</td>
                            <td align="left">Instrumento</td>
                            <td align="left">Sentido</td>
                            <td align="center">Excluir</td>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <td colspan="7"></td>
                        </tr>
                    </tfoot>
                    <tbody>
                        <tr class="tdGradIntra_Risco_ItemCliente_TrMatriz"        style="display:none;">
                            <td class="tdGradIntra_Risco_ItemCliente_IdItem"      style="display:none;"></td>
                            <td class="tdGradIntra_Risco_ItemCliente_ItemDoBanco" style="display:none;"></td>
                            <td class="tdGradIntra_Risco_ItemCliente_IdAcao"        style="display:none"></td>
                            <td class="tdGradIntra_Risco_ItemCliente_Cliente"     align="left"></td>
                            <td class="tdGradIntra_Risco_ItemCliente_Instrumento" align="left"></td>
                            <td class="tdGradIntra_Risco_ItemCliente_Sentido"     align="left"></td>
                            <td class="tdGradIntra_Risco_ItemCliente_Excluir"     align="center"><input type="button" onclick="return GradIntra_Risco_Restricoes_ItensDoCliente_Excluir_Click(this);" class="IconButton Excluir" style="float: none;" /></td>
                        </tr>
                    </tbody>
                </table> 
                </div>
                <%--margin: 0.75em 3em 1em 3em; width: 85%; z-index:-40;--%>
                <div id="DivtblGradIntra_Risco_ItemGrupo" style="margin-top:70px">
                    <table id="tblGradIntra_Risco_ItemRegraGrupo" class="GridIntranet" style="width: 70%" cellspacing="0">
                        <thead>
                            <tr>
                                <td style="display:none"></td>
                                <td style="display:none"></td>
                                <td style="display:none"></td>
                                <td align="left">Cliente</td>
                                <td align="left">Sentido</td>
                                <td align="center">Excluir</td>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <td colspan="5"></td>
                            </tr>
                        </tfoot>
                        <tbody>
                            <tr class="tdGradIntra_Risco_ItemRegraGrupo_TrMatriz"        style="display: none;">
                                <td class="tdGradIntra_Risco_ItemRegraGrupo_IdItem"      style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemRegraGrupo_ItemDoBanco" style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemRegraGrupo_IdAcao"      style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemRegraGrupo_Cliente"     align="left"></td>
                                <td class="tdGradIntra_Risco_ItemRegraGrupo_Sentido"     align="center"></td>
                                <td class="tdGradIntra_Risco_ItemRegraGrupo_Excluir"     align="center"><input type="button" onclick="return GradIntra_Risco_Restricoes_ItensDoGrupo_Excluir_Click(this);" class="IconButton Excluir" style="float: none;" /></td>
                            </tr>
                        </tbody>
                    </table>    
                </div>
                <div id="DivtblGradIntra_Risco_ItemGrupo_Global" style="display: none; margin-top: 50px">
                    <table id="tblGradIntra_Risco_ItemGrupo_Global" class="GridIntranet" style="width: 70%" cellspacing="0">
                        <thead>
                            <tr>
                                <td style="display:none"></td>
                                <td style="display:none"></td>
                                <td style="display:none"></td>
                                <td style="display:none"></td>
                                <td align="left">Nome Grupo</td>
                                <td align="left">Nome Ação</td>
                                <td align="center">Sentido</td>
                                <td align="center">Excluir</td>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <td colspan="5"></td>
                            </tr>
                        </tfoot>
                        <tbody>
                            <tr class="tdGradIntra_Risco_ItemGrupo_Global_TrMatriz"        style="display: none;">
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_IdItem"      style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_ItemDoBanco" style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_IdAcao"      style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_IdGrupo"     style="display:none"></td>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_NomeGrupo"   align="left"></td>
                                <%--<td class="tdGradIntra_Risco_ItemGrupo_Cliente"     align="left"></td>--%>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_NomeAcao"    align="left"></td>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_Sentido"     align="center"></td>
                                <td class="tdGradIntra_Risco_ItemGrupo_Global_Excluir"     align="center"><input type="button" onclick="return GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Excluir_Click(this);" class="IconButton Excluir" style="float: none;" /></td>
                            </tr>
                        </tbody>
                    </table>    
                </div>

                <%--<p class="BotoesSubmit" style="margin-top:3em">
                    <button id="btnCliente_Restricoes" onclick="return btnCliente_Risco_Restricoes_Click(this)">Salvar Dados</button>
                </p>--%>
</form>

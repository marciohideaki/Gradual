<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Risco_MonitoramentoRiscoGeral.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Risco_MonitoramentoRiscoGeral" %>
<form id="form1" runat="server">
    
    <input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />

    <div id="pnlRisco_Monitoramento_Risco_LucroPrejuizo_Pesquisa" class="Busca_Formulario" style="width:88%; height: 9.5em; ">
            <div style="float: left; width: 30em; ">
                <p style="margin-left: 12px; padding-bottom: 5px; width: 30em;">
                    <label for="txtDBM_FiltroRelatorio_CodigoCliente">Cód. Cliente:</label>
                    <input  id="txtDBM_FiltroRelatorio_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" style="width: 18.5em;" class="ProibirLetras" />
                </p>

                <p style="margin-left: 12px; padding-bottom: 5px; width: 29em;">
                    <label for="txtDBM_FiltroRelatorio_CodAssessor">Cód. Assessor:</label>
                
                    <select id="cboBM_FiltroRelatorio_CodAssessor" Propriedade="CodAssessor" style="width:20.2em">
                        <option value="" id="opcBM_FiltroRelatorio_CodAssessor_Todos">[ Todos ]</option>
                        <asp:Repeater id="rptBM_FiltroRelatorio_CodAssessor" runat="server">
                            <ItemTemplate>
                                <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                            </ItemTemplate>
                        </asp:Repeater>        
                    </select>
                </p>
            
                <%--<p style="margin-left: 12px; padding-bottom: 5px; width: 28em;">
                    <label for="cboRisco_Monitoramento_FiltroPerda">Perda:</label>
                    <select id="cboRisco_Monitoramento_FiltroPerda" Propriedade="Perda" style="width:20.2em" onchange="GradIntra_Risco_MonitoramentoLucrosPrejuizos_Buscar();">
                        <option value="">[Todos]</option>
                        <option value="SemaforoVerde">Faixa 1 (Verde)</option>
                        <option value="SemaforoAmarelo">Faixa 2 (Amarela)</option>
                        <option value="SemaforoVermelho">Faixa 3 (Vermelha)</option>
                    </select>
                </p>--%>
            </div>

            <div style="float: left;  width: 40em;" id="pnlMonitoramentoRiscoGeral_FiltroPrejuizo">
                <%--<p style="margin-left: 20px; padding-bottom: 5px;">--%>
                    <span class="SemaforoVermelhoMonitoramento"></span><input name="chkRisco_Monitoramento_FiltroSemaforo" id="chkRisco_Monitoramento_FiltroPrejuizo_1" type="radio" value="1" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroSemaforo(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_1">Prej >= 70% ( PL)          </label><br />
                    <span class="SemaforoAmareloMonitoramento"></span><input name="chkRisco_Monitoramento_FiltroSemaforo" id="chkRisco_Monitoramento_FiltroPrejuizo_3" type="radio" value="3" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroSemaforo(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_3">Prej  > 20% & <  70% ( PL) </label><br />
                    <span class="SemaforoVerdeMonitoramento"></span><input name="chkRisco_Monitoramento_FiltroSemaforo" id="chkRisco_Monitoramento_FiltroPrejuizo_4" type="radio" value="4" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroSemaforo(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_4">Prej < 20% (PL)            </label><br /><br />
                <%--</p>--%>
            </div>

            <%--<div >
                <p style="text-align: left; float: left; margin-left: 46.8em; width: 40%; position: absolute; margin-top: 130px;">
                    
                </p>
            </div>--%>
            <div style="float:left; position: absolute; margin-top: 75px; margin-left: 1em">
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_SemFiltro" type="radio" value="0"onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)" checked="checked"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_SemFiltro">Todos</label>
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_10" type="radio" value="10"onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_10">Prej < R$2.000                    </label>
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_11" type="radio" value="11"onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_11">R$ 2.000 >= Prej < R$5.000        </label>
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_5"  type="radio" value="5" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_5">R$ 5.000  >= Prej < R$10.000        </label>
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_2"  type="radio" value="2" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_2">R$ 10.000 >= Prej < R$15.000       </label>                                                                        
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_6" type="radio"  value="6" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_6">R$ 15.000 >= Prej < R$20.000       </label>
                <input name="chkRisco_Monitoramento_FiltroProporcaoPrejuizo" id="chkRisco_Monitoramento_FiltroPrejuizo_7" type="radio"  value="7" onclick="return rdoRisco_Monitoramento_LucrosPrejuizos_FiltroProporcaoPrejuizao(this)"/><label for="chkRisco_Monitoramento_FiltroPrejuizo_7">Prej >=  R$20.000       </label>&nbsp;&nbsp;&nbsp;
                <button class="btnBusca" onclick="return btnRisco_Monitoramento_LucroPrejuizo_Busca_Click(this)" id="btnRisco_Monitoramento_Busca">Buscar</button>
            </div>
        </div>
        <div class="Risco_FooterJanelas">Janelas configuradas</div>
        <div class="Risco_FooterLinkPaginas" id="div_Risco_FooterLinkPaginas"></div>
</form>
<script language=javascript>
    Risco_Monitoramento_LucroPrejuizo_Carregar_Janelas_Usuarios();
    GradIntra_Monitoramento_LucroPrejuizo_Gerais_Load();

</script>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Risco_MonitoramentoIntradiario.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Risco_MonitoramentoIntradiario" %>
<form id="form1" runat="server">
    
    <input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />

    <div id="pnlRisco_Monitoramento_Risco_Intradiario_Pesquisa" class="Busca_Formulario" style="width:88%; height: 11.5em; ">
            <div style="float: left; width: 30em; ">
             <p>
                <label for="txtBusca_Monitoramento_Intraday_DataInicial">Data Inicial:</label>
                <input name="txtBusca_Monitoramento_Intraday_DataInicial" id="txtBusca_Monitoramento_Intraday_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" />
            </p>
            <p>
                <label for="txtBusca_Monitoramento_Intraday_DataFinal">Data Final:</label>
                <input  id="txtBusca_Monitoramento_Intraday_DataFinal" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="width: 64px;" />
            </p>
        
            <p style="margin-left: 12px; padding-bottom: 5px; width: 30em;">
                <label for="txtBuscar_Monitoramento_Intradiario_CodigoCliente">Cód. Cliente:</label>
                <input  id="txtBuscar_Monitoramento_Intradiario_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" style="width: 18.5em;" class="ProibirLetras" />
            </p>

            <p style="margin-left: 12px; padding-bottom: 5px; width: 29em;">
                <label for="cboBM_Monitoramento_Intradiario_CodAssessor">Cód. Assessor:</label>
                
                <select id="cboBM_Monitoramento_Intradiario_CodAssessor" Propriedade="CodAssessor" style="width:20.2em">
                    <option value="" id="opcBM_Monitoramento_Intradiario_CodAssessor_Todos">[ Todos ]</option>
                    <asp:Repeater id="rptBM_FiltroRelatorio_CodAssessor" runat="server">
                        <ItemTemplate>
                            <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                        </ItemTemplate>
                    </asp:Repeater>        
                </select>
            </p>
            
        </div>
        <div style="float: left;  width: 18em;" id="pnlMonitoramentoRiscoGeral_FiltroPrejuizo">
                <%--<p style="margin-left: 20px; padding-bottom: 5px;">--%>
                    <span style="margin-right:18px"></span>         <input name="chkRisco_Monitoramento_Intradiario_NETxSFP" id="chkRisco_Monitoramento_Intradiario_NETxSFP_0" type="radio"  value="0"  onclick="return rdoRisco_Monitoramento_Intradiario_NETxSFP(this)" checked="checked"/><label for="chkRisco_Monitoramento_Intradiario_NETxSFP_0">Todos             </label><br />
                    <span class="SemaforoVerdeMonitoramento"></span><input name="chkRisco_Monitoramento_Intradiario_NETxSFP" id="chkRisco_Monitoramento_Intradiario_NETxSFP_1" type="radio"  value="1"  onclick="return rdoRisco_Monitoramento_Intradiario_NETxSFP(this)"/><label for="chkRisco_Monitoramento_Intradiario_NETxSFP_1">% NET por SFP < 20%                 </label><br />
                    <span class="SemaforoAmareloMonitoramento"></span><input  name="chkRisco_Monitoramento_Intradiario_NETxSFP" id="chkRisco_Monitoramento_Intradiario_NETxSFP_2" type="radio"  value="2"  onclick="return rdoRisco_Monitoramento_Intradiario_NETxSFP(this)"/><label for="chkRisco_Monitoramento_Intradiario_NETxSFP_2">% NET por SFP > 20% & < 50%      </label><br />
                    <span class="SemaforoVermelhoMonitoramento"></span><input    name="chkRisco_Monitoramento_Intradiario_NETxSFP" id="chkRisco_Monitoramento_Intradiario_NETxSFP_3" type="radio"  value="3"  onclick="return rdoRisco_Monitoramento_Intradiario_NETxSFP(this)"/><label for="chkRisco_Monitoramento_Intradiario_NETxSFP_3">% NET por SFP > 50%           </label><br /><br />
                <%--</p>--%>
            </div>
            <%--<div >
                <p style="text-align: left; float: left; margin-left: 46.8em; width: 40%; position: absolute; margin-top: 130px;">
                    
                </p>
            </div>--%>
            <div style="float:left; width: 20em; margin-left: 1em">
                <input name="chkRisco_Monitoramento_Intradiario_FiltroEXPxPosicao" id="chkRisco_Monitoramento_Intradiario_EXPxPosicao_0" type="radio" value="0" onclick="return rdoRisco_Monitoramento_Intradiario_EXPxPosicao(this)"  checked="checked"/><label for="chkRisco_Monitoramento_Intradiario_EXPxPosicao_0">Todos</label>                             <br />
                <input name="chkRisco_Monitoramento_Intradiario_FiltroEXPxPosicao" id="chkRisco_Monitoramento_Intradiario_EXPxPosicao_1" type="radio" value="1" onclick="return rdoRisco_Monitoramento_Intradiario_EXPxPosicao(this)" />                  <label for="chkRisco_Monitoramento_Intradiario_EXPxPosicao_1">% EXP por Posição < 20%          </label> <br />
                <input name="chkRisco_Monitoramento_Intradiario_FiltroEXPxPosicao" id="chkRisco_Monitoramento_Intradiario_EXPxPosicao_2" type="radio" value="2" onclick="return rdoRisco_Monitoramento_Intradiario_EXPxPosicao(this)" />                  <label for="chkRisco_Monitoramento_Intradiario_EXPxPosicao_2">% EXP por Posição > 20% & < 50%  </label> <br />
                <input name="chkRisco_Monitoramento_Intradiario_FiltroEXPxPosicao" id="chkRisco_Monitoramento_Intradiario_EXPxPosicao_3" type="radio" value="3" onclick="return rdoRisco_Monitoramento_Intradiario_EXPxPosicao(this)" />                  <label for="chkRisco_Monitoramento_Intradiario_EXPxPosicao_3">% EXP por Posição > 50%          </label> <br />
            </div>

            <div style="float:left; width: 25em; margin-left: 1em">
                <input name="chkRisco_Monitoramento_Intradiario_NET" id="chkRisco_Monitoramento_Intradiario_NET_0" type="radio" value="0" onclick="return rdoRisco_Monitoramento_Intradiario_NET(this)" checked="checked"/><label for="chkRisco_Monitoramento_Intradiario_NET_0">Todos</label>                                   <br />
                <input name="chkRisco_Monitoramento_Intradiario_NET" id="chkRisco_Monitoramento_Intradiario_NET_1" type="radio" value="1" onclick="return rdoRisco_Monitoramento_Intradiario_NET(this)"/>                  <label for="chkRisco_Monitoramento_Intradiario_NET_1">NET < R$ 500.000,00                    </label> <br />
                <input name="chkRisco_Monitoramento_Intradiario_NET" id="chkRisco_Monitoramento_Intradiario_NET_2" type="radio" value="2" onclick="return rdoRisco_Monitoramento_Intradiario_NET(this)"/>                  <label for="chkRisco_Monitoramento_Intradiario_NET_2">NET > R$ 500.000,00 & < R$ 1.000.000,00</label> <br />
                <input name="chkRisco_Monitoramento_Intradiario_NET" id="chkRisco_Monitoramento_Intradiario_NET_3" type="radio" value="3" onclick="return rdoRisco_Monitoramento_Intradiario_NET(this)"/>                  <label for="chkRisco_Monitoramento_Intradiario_NET_3">NET > R$ 1.000.000,00                  </label> <br />
            </div>
            <button class="btnBusca" onclick="return btnRisco_Monitoramento_Intradiario_Busca_Click(this)" id="btnRisco_Monitoramento_Intradiario_Busca">Buscar</button>
        </div>
        <%--<div class="Risco_FooterJanelas">Janelas configuradas</div>
        <div class="Risco_FooterLinkPaginas" id="div_Risco_FooterLinkPaginas"></div>--%>
    
</form>

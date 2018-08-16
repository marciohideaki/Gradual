<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Risco_SuitabilityLavagem.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Risco_SuitabilityLavagem" %>
   <form id="form1" runat="server">
    
    <input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />

    <div id="pnlRisco_SuitabilityLavagem_Pesquisa" class="Busca_Formulario" style="width:88%; height: 11.5em; ">
            <div style="float: left; width: 30em; ">
             <p>
                <label for="txtBusca_Suitability_Lavagem_DataInicial">Data Inicial:</label>
                <input name="txtBusca_Suitability_Lavagem_DataInicial" id="txtBusca_Suitability_Lavagem_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" />
            </p>
            <p>
                <label for="txtBusca_Suitability_Lavagem_DataFinal">Data Final:</label>
                <input  id="txtBusca_Suitability_Lavagem_DataFinal" type="text"  value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="width: 64px;" />
            </p>
        
            <p style="margin-left: 12px; padding-bottom: 5px; width: 30em;">
                <label for="txtBuscar_Suitability_Lavagem_CodigoCliente">Cód. Cliente:</label>
                <input  id="txtBuscar_Suitability_Lavagem_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" style="width: 18.5em;" class="ProibirLetras" />
            </p>

            <p style="margin-left: 12px; padding-bottom: 5px; width: 29em;">
                <label for="cboBM_Suitability_Lavagem_CodAssessor">Cód. Assessor:</label>
                
                <select id="cboBM_Suitability_Lavagem_CodAssessor" Propriedade="CodAssessor" style="width:20.2em">
                    <option value="" id="opcBM_Suitability_Lavagem_CodAssessor_Todos">[ Todos ]</option>
                    <asp:Repeater id="rptBM_FiltroRelatorio_CodAssessor" runat="server">
                        <ItemTemplate>
                            <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                        </ItemTemplate>
                    </asp:Repeater>        
                </select>
            </p>
            </div>
            <div style="float: left;  width: 18em;" id="pnlMonitoramentoRiscoGeral_FiltroPrejuizo">
                <input name="chkRisco_Suitability_Lavagem_VOLxSFP" id="chkRisco_Suitability_Lavagem_VOLxSFP_0" type="radio"  value="0"  onclick="return rdoRisco_Suitability_Lavagem_VOLxSFP(this)" checked="checked"/><label for="chkRisco_Suitability_Lavagem_VOLxSFP_0">Todos             </label><br />
                <input name="chkRisco_Suitability_Lavagem_VOLxSFP" id="chkRisco_Suitability_Lavagem_VOLxSFP_1" type="radio"  value="1"  onclick="return rdoRisco_Suitability_Lavagem_VOLxSFP(this)"/><label for="chkRisco_Suitability_Lavagem_VOLxSFP_1">% VOL por SFP < 20%                 </label><br />
                <input name="chkRisco_Suitability_Lavagem_VOLxSFP" id="chkRisco_Suitability_Lavagem_VOLxSFP_2" type="radio"  value="2"  onclick="return rdoRisco_Suitability_Lavagem_VOLxSFP(this)"/><label for="chkRisco_Suitability_Lavagem_VOLxSFP_2">% VOL por SFP > 20% & < 50%      </label><br />
                <input name="chkRisco_Suitability_Lavagem_VOLxSFP" id="chkRisco_Suitability_Lavagem_VOLxSFP_3" type="radio"  value="3"  onclick="return rdoRisco_Suitability_Lavagem_VOLxSFP(this)"/><label for="chkRisco_Suitability_Lavagem_VOLxSFP_3">% VOL por SFP > 50%           </label><br /><br />
            </div>
            <div style="float:left; width: 20em; margin-left: 1em">
                <input name="chkRisco_Suitability_Lavagem_Volume" id="chkRisco_Suitability_Lavagem_Volume_0" type="radio" value="0" onclick="return rdoRisco_Suitability_Lavagem_Volume(this)"  checked="checked"/><label for="chkRisco_Suitability_Lavagem_Volume_0">Todos</label>           <br />
                <input name="chkRisco_Suitability_Lavagem_Volume" id="chkRisco_Suitability_Lavagem_Volume_1" type="radio" value="1" onclick="return rdoRisco_Suitability_Lavagem_Volume(this)" />                  <label for="chkRisco_Suitability_Lavagem_Volume_1">Volume < 500M           </label> <br />
                <input name="chkRisco_Suitability_Lavagem_Volume" id="chkRisco_Suitability_Lavagem_Volume_2" type="radio" value="2" onclick="return rdoRisco_Suitability_Lavagem_Volume(this)" />                  <label for="chkRisco_Suitability_Lavagem_Volume_2">Volume > 500M & < 1000M </label> <br />
                <input name="chkRisco_Suitability_Lavagem_Volume" id="chkRisco_Suitability_Lavagem_Volume_3" type="radio" value="3" onclick="return rdoRisco_Suitability_Lavagem_Volume(this)" />                  <label for="chkRisco_Suitability_Lavagem_Volume_3">Volume > 1000M          </label> <br />
            </div>

            <div style="float:left; width: 25em; margin-left: 1em">
                <input name="chkRisco_Suitability_Lavagem_Enquadrado" id="chkRisco_Suitability_Lavagem_Enquadrado_Todos" type="radio" value="-1" onclick="return rdoRisco_Suitability_Lavagem_Enquadrado(this) />                 <label for="chkRisco_Suitability_Lavagem_Enquadrado_0">Todos</label><br />
                <input name="chkRisco_Suitability_Lavagem_Enquadrado" id="chkRisco_Suitability_Lavagem_Enquadrado_0"     type="radio" value="1" onclick="return rdoRisco_Suitability_Lavagem_Enquadrado(this)" checked="checked"/><label for="chkRisco_Suitability_Lavagem_Enquadrado_0">Enquadrado</label><br />
                <input name="chkRisco_Suitability_Lavagem_Enquadrado" id="chkRisco_Suitability_Lavagem_Enquadrado_1"     type="radio" value="0" onclick="return rdoRisco_Suitability_Lavagem_Enquadrado(this)"/>                  <label for="chkRisco_Suitability_Lavagem_Enquadrado_1">Não enquadrado</label> <br />
            </div>
            <button class="btnBusca" onclick="return btnRisco_Suitability_Lavagem_Busca_Click(this)" id="btnRisco_Suitability_Lavagem_Busca">Buscar</button>
        </div>


    </form>

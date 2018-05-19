<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compliance_Churning.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Compliance_Churning" %>
    <form id="form1" runat="server">
    <div id="pnlCompliance_Churning_Intraday_Pesquisa" class="Busca_Formulario" style="width:90%; height: 14em; ">
            <div style="float: left; width: 20em; ">
             <p>
                <label for="txtBusca_Churning_Intraday_DataInicial">Data Inicial:</label>
                <input name="txtBusca_Churning_Intraday_DataInicial" id="txtBusca_Churning_Intraday_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" />
            </p>
            <p>
                <label for="txtBusca_Churning_Intraday_DataFinal">Data Final:</label>
                <input  id="txtBusca_Churning_Intraday_DataFinal" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="width: 64px;" />
            </p>
        
            <p style="padding-bottom: 5px;">
                <label for="txtBuscar_Churning_Intraday_CodigoCliente">Cód. Cliente:</label>
                <input  id="txtBuscar_Churning_Intraday_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" style="width: 8em;" class="ProibirLetras" />
            </p>

            <p style="padding-bottom: 5px;">
                <label for="cboBM_Churning_Intraday_CodAssessor">Cód. Assessor:</label>
                
                <select id="cboBM_Churning_Intraday_CodAssessor" Propriedade="CodAssessor" style="width:10.2em">
                    <option value="" id="opcBM_Churning_Intraday_CodAssessor_Todos">[ Todos ]</option>
                    <asp:Repeater id="rptBM_FiltroRelatorio_CodAssessor" runat="server">
                        <ItemTemplate>
                            <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                        </ItemTemplate>
                    </asp:Repeater>        
                </select>
            </p>
            <p>
                <label for="txtBuscar_Churning_Intraday_Porta">Porta</label>
                <input  id="txtBuscar_Churning_Intraday_Porta" type="text" maxlength="10" Propriedade="Porta" style="width: 5em;" class="ProibirLetras" />
            </p>

            
        </div>
        <div style="float: left;  width: 13em;" id="pnlMonitoramentoRiscoGeral_FiltroPrejuizo">
                <%--<p style="margin-left: 20px; padding-bottom: 5px;">--%>
                    <input name="chkRisco_Churning_Intraday_PercentalTR" id="chkRisco_Churning_Intraday_PercentalTR_0" type="radio"  value="0"  onclick="return rdoRisco_Churning_Intraday_PercentualTR(this)" checked="checked"/><label for="chkRisco_Churning_Intraday_PercentalTR_0">Todos   </label><br />
                    <input name="chkRisco_Churning_Intraday_PercentalTR" id="chkRisco_Churning_Intraday_PercentalTR_1" type="radio"  value="1"  onclick="return rdoRisco_Churning_Intraday_PercentualTR(this)"/><label for="chkRisco_Churning_Intraday_PercentalTR_1">TR  <= 2                </label><br />
                    <input name="chkRisco_Churning_Intraday_PercentalTR" id="chkRisco_Churning_Intraday_PercentalTR_2" type="radio"  value="2"  onclick="return rdoRisco_Churning_Intraday_PercentualTR(this)"/><label for="chkRisco_Churning_Intraday_PercentalTR_2">TR  < 2  && TR <= 8   </label><br />
                    <input name="chkRisco_Churning_Intraday_PercentalTR" id="chkRisco_Churning_Intraday_PercentalTR_3" type="radio"  value="3"  onclick="return rdoRisco_Churning_Intraday_PercentualTR(this)"/><label for="chkRisco_Churning_Intraday_PercentalTR_3">TR  > 8                 </label><br /><br />
                <%--</p>--%>
            </div>
            <div style="float:left; width: 13em; margin-left: 1em">
                <input name="chkRisco_Churning_Intraday_PercentualCE" id="chkRisco_Churning_Itranday_PercentualCE_0" type="radio" value="0" onclick="return rdoRisco_Churning_Intraday_PercentualCE(this)"  checked="checked"/><label for="chkRisco_Churning_Intraday_PercentualCE_0">Todos</label>                 <br />
                <input name="chkRisco_Churning_Intraday_PercentualCE" id="chkRisco_Churning_Itranday_PercentualCE_1" type="radio" value="1" onclick="return rdoRisco_Churning_Intraday_PercentualCE(this)" />                  <label for="chkRisco_Churning_Intraday_PercentualCE_1">CE %  < 10%          </label> <br />
                <input name="chkRisco_Churning_Intraday_PercentualCE" id="chkRisco_Churning_Itranday_PercentualCE_2" type="radio" value="2" onclick="return rdoRisco_Churning_Intraday_PercentualCE(this)" />                  <label for="chkRisco_Churning_Intraday_PercentualCE_2">CE %  > 10% & < 15%  </label> <br />
                <input name="chkRisco_Churning_Intraday_PercentualCE" id="chkRisco_Churning_Itranday_PercentualCE_3" type="radio" value="3" onclick="return rdoRisco_Churning_Intraday_PercentualCE(this)" />                  <label for="chkRisco_Churning_Intraday_PercentualCE_3">CE %  < 15% & < 20%  </label> <br />
                <input name="chkRisco_Churning_Intraday_PercentualCE" id="chkRisco_Churning_Itranday_PercentualCE_4" type="radio" value="4" onclick="return rdoRisco_Churning_Intraday_PercentualCE(this)" />                  <label for="chkRisco_Churning_Intraday_PercentualCE_4">CE %  > 20%          </label> <br />
            </div>
            <%--<div style="float:left; width: 15em; margin-left: 1em">
                <input name="chkRisco_Churning_Intraday_TotalCompras" id="chkRisco_Churning_Itranday_TotalCompras_0" type="radio" value="0" onclick="return rdoRisco_Churning_Intraday_TotalCompras(this)"  checked="checked"/><label for="chkRisco_Churning_Intraday_TotalCompras_0">Todos</label>                     <br />
                <input name="chkRisco_Churning_Intraday_TotalCompras" id="chkRisco_Churning_Itranday_TotalCompras_1" type="radio" value="1" onclick="return rdoRisco_Churning_Intraday_TotalCompras(this)" />                  <label for="chkRisco_Churning_Intraday_TotalCompras_1">Compras < 500M            </label><br />
                <input name="chkRisco_Churning_Intraday_TotalCompras" id="chkRisco_Churning_Itranday_TotalCompras_2" type="radio" value="2" onclick="return rdoRisco_Churning_Intraday_TotalCompras(this)" />                  <label for="chkRisco_Churning_Intraday_TotalCompras_2">Compras > 500M & < 1000M  </label><br />
                <input name="chkRisco_Churning_Intraday_TotalCompras" id="chkRisco_Churning_Itranday_TotalCompras_3" type="radio" value="3" onclick="return rdoRisco_Churning_Intraday_TotalCompras(this)" />                  <label for="chkRisco_Churning_Intraday_TotalCompras_3">Compras < 1000M           </label><br />
            </div>
            <div style="float:left; width: 14em; margin-left: 1em">
                <input name="chkRisco_Churning_Intraday_TotalVendas" id="chkRisco_Churning_Itranday_TotalVendas_0" type="radio" value="0" onclick="return rdoRisco_Churning_Intraday_TotalVendas(this)"  checked="checked"/><label for="chkRisco_Churning_Intraday_TotalCompras_0">Todos</label>                       <br />
                <input name="chkRisco_Churning_Intraday_TotalVendas" id="chkRisco_Churning_Itranday_TotalVendas_1" type="radio" value="1" onclick="return rdoRisco_Churning_Intraday_TotalVendas(this)" />                  <label for="chkRisco_Churning_Intraday_TotalCompras_1">Vendas   < 500M            </label> <br />
                <input name="chkRisco_Churning_Intraday_TotalVendas" id="chkRisco_Churning_Itranday_TotalVendas_2" type="radio" value="2" onclick="return rdoRisco_Churning_Intraday_TotalVendas(this)" />                  <label for="chkRisco_Churning_Intraday_TotalCompras_2">Vendas   > 500M & < 1000M  </label> <br />
                <input name="chkRisco_Churning_Intraday_TotalVendas" id="chkRisco_Churning_Itranday_TotalVendas_3" type="radio" value="3" onclick="return rdoRisco_Churning_Intraday_TotalVendas(this)" />                  <label for="chkRisco_Churning_Intraday_TotalCompras_3">Vendas   < 1000            </label> <br />
            </div>
            <div style="float:left; width: 18em; margin-left: 1em">
                <input name="chkRisco_Churning_Intraday_CarteiraMedia" id="chkRisco_Churning_Itranday_CarteiraMedia_0" type="radio" value="0" onclick="return rdoRisco_Churning_Intraday_CarteiraMedia(this)"  checked="checked"/><label for="chkRisco_Churning_Intraday_CarteiraMedia_0">Todos</label>                           <br />
                <input name="chkRisco_Churning_Intraday_CarteiraMedia" id="chkRisco_Churning_Itranday_CarteiraMedia_1" type="radio" value="1" onclick="return rdoRisco_Churning_Intraday_CarteiraMedia(this)" />                  <label for="chkRisco_Churning_Intraday_CarteiraMedia_1">Carteira Media < 500M          </label> <br />
                <input name="chkRisco_Churning_Intraday_CarteiraMedia" id="chkRisco_Churning_Itranday_CarteiraMedia_2" type="radio" value="2" onclick="return rdoRisco_Churning_Intraday_CarteiraMedia(this)" />                  <label for="chkRisco_Churning_Intraday_CarteiraMedia_2">Carteira Media > 500M & < 1000M</label> <br />
                <input name="chkRisco_Churning_Intraday_CarteiraMedia" id="chkRisco_Churning_Itranday_CarteiraMedia_3" type="radio" value="3" onclick="return rdoRisco_Churning_Intraday_CarteiraMedia(this)" />                  <label for="chkRisco_Churning_Intraday_CarteiraMedia_3">Carteira Media < 1000M         </label> <br />
            </div>--%>
            <button class="btnBusca" onclick="return btnCompliance_Churning_Intraday_Busca_Click(this)" id="btnCompliance_Churning_Intraday_Busca">Buscar</button>
            
            
        </div>
    </form>


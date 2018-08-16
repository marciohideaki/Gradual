<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compliance_EstatisticaDayTrade.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Compliance_EstatisticaDayTrade" %>
<form id="form1" runat="server">
<div id="pnlBusca_Compliance_EstatisticaDayTrade_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_1Linha">
     <p class="LadoALado_Pequeno" style="width:150px ">
        <label for="txtDBM_FiltroRelatorio_CodigoCliente">Cód. Cliente:</label>
        <input  id="txtDBM_FiltroRelatorio_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" style="width: 5.5em;" class="ProibirLetras" />
    </p>

    <p class="LadoALado_Pequeno" style="width:250px ">
        <label for="txtDBM_FiltroRelatorio_CodAssessor">Cód. Assessor:</label>
        <select id="cboBM_FiltroRelatorio_CodAssessor" Propriedade="CodAssessor" style="width:15.2em">
            <option value="" id="opcBM_FiltroRelatorio_CodAssessor_Todos">[ Todos ]</option>
            <asp:Repeater id="rptBM_FiltroRelatorio_CodAssessor" runat="server">
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>
    
    <p style="width:280px">
        <label for="cboBM_FiltroRelatorio_TipoBolsa">Tipo Bolsa:</label>
        <select id="cboBM_FiltroRelatorio_TipoBolsa" Propriedade="TipoBolsa" style="width:15.2em">
            <option value="" id="opcBM_FiltroRelatorio_TipoBolsa_Todos">[ Todos ]</option>
            <option value="BOVESPA" >BOVESPA</option>
            <option value="BMF" >BMF</option>
        </select>
    </p>
            
    <p class="LadoALado_Pequeno" style="width:150px">
        <button class="btnBusca" onclick="return btnCompliance_EstatisticaDayTrade_Busca_Click(this)" id="btnCompliance_EstatisticaDayTrade_Busca">Buscar</button>
        <%--<img id="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../../../../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="cursor: pointer;  ">--%>
    </p>
    <p class="LadoALado_Pequeno" style="width:100px;">
        <button class="btnBusca" onclick="return btnCompliance_ImprimirRelatorio_EstatisticaDayTrade(this)" id="btnCompliance_EstatisticaDayTrade_Exportar">Exportar</button>
    </p>

</div>
<div style="position:absolute; top: 185px; background-color: White; width: 1400">
    <div id="div_Compliance_EstatisticaDayTrade_Resultados" style="margin-top: 2em; margin-left: 2em; margin-bottom: 2em; display: none">
    <table id="tblBusca_Compliance_Resultados_EstatisticaDayTrade" ></table>
    </div>
</div>
</form>
<style type="text/css">
    .ui-jqgrid tr.ui-row-ltr td 
    {
        border-right-style: none;
        border-left-style: none;
    }
</style>
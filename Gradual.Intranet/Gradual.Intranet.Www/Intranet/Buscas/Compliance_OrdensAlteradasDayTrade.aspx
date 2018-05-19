<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compliance_OrdensAlteradasDayTrade.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Compliance_OrdensAlteradasDayTrade" %>
<form id="form1" runat="server">
<div id="pnlBusca_Compliance_OrdensAlteradasDayTrade_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_1Linha">
     <p class="LadoALado_Pequeno" style="width:250px ">
        <label for="txtCompliance_Filtro_OrdensAlteradasDayTrade_DataDe">Data Inicial:</label>
        <input  id="txtCompliance_Filtro_OrdensAlteradasDayTrade_DataDe" type="text" maxlength="10" Propriedade="DataDe" style="width: 5.5em;" class="Mascara_Data Picker_Data" />
    </p>

    <p class="LadoALado_Pequeno" style="width:250px ">
        <label for="txtCompliance_Filtro_OrdensAlteradasDayTrade_DataAte">Data Final:</label>
        <input  id="txtCompliance_Filtro_OrdensAlteradasDayTrade_DataAte" type="text" maxlength="10" Propriedade="DataAte" style="width: 5.5em;" class="Mascara_Data Picker_Data" />
    </p>
    
          
    <p class="LadoALado_Pequeno" style="width:150px">
        <button class="btnBusca" onclick="return btnCompliance_OrdensAlteradasDayTrade_Busca_Click(this)" id="btnCompliance_OrdensAlteradasDayTrade_Busca">Buscar</button>
        <%--<img id="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../../../../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="cursor: pointer;  ">--%>
    </p>
    <p class="LadoALado_Pequeno" style="width:100px;">
        <button class="btnBusca" onclick="return btnCompliance_ImprimirRelatorio_OrdensAlteradasDayTrade(this)" id="btnCompliance_OrdensAlteradasDayTrade_Exportar">Exportar</button>        
    </p>
</div>
<div style="position:absolute; top: 185px; background-color: White; width: 100%">
    <div id="div_Compliance_OrdensAlteradasDayTrade_Resultados" style="margin-top: 2em; margin-left: 2em; margin-bottom: 2em; display: none">
        <table id="tblBusca_Compliance_Resultados_OrdensAlteradasDayTrade" ></table>
    </div>
</div>
</form>

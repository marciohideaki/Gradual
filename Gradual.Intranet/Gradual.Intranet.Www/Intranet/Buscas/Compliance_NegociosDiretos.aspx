<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compliance_NegociosDiretos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Compliance_NegociosDiretos" %>
<form id="form1" runat="server">
<div id="pnlBusca_Compliance_NegociosDiretos_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_1Linha">
     <p class="LadoALado_Pequeno" style="width:200px ">
        <label for="txtCompliance_Filtro_NegociosDiretos_Data">Data :</label>
        <input  id="txtCompliance_FiltroNegociosDiretos_Data" type="text" maxlength="10" Propriedade="DataDe" style="width: 5.5em;" class="Mascara_Data Picker_Data" />
    </p>

    <%--<p class="LadoALado_Pequeno" style="width:200px ">
        <label for="txtCompliance_FiltroNegociosDiretos_DataAte">Data Final:</label>
        <input  id="txtCompliance_FiltroNegociosDiretos_DataAte" type="text" maxlength="10" Propriedade="DataAte" style="width: 5.5em;" class="Mascara_Data Picker_Data" />
    </p>--%>
    
    <%--<p class="LadoALado_Pequeno" style="width:200px ">
        <label for="cboCompliance_FiltroNegociosDiretos_Sentido">Sentido:</label>
        <select id="cboCompliance_FiltroNegociosDiretos_Sentido" Propriedade="Sentido" style="width:10.2em">
            <option value="C" >Compra</option>
            <option value="V" >Venda</option>
        </select>
    </p>--%>
    
    <%--<p class="LadoALado_Pequeno" style="width:200px ">
        <label for="txtCompliance_FiltroNegociosDiretos_CodigoCliente">Codigo Cliente:</label>
        <input  id="txtCompliance_FiltroNegociosDiretos_CodigoCliente" type="text" maxlength="10" Propriedade="CodigoCliente" style="width: 5.5em;" class="ProibirLetras" />
    </p>--%>

    <p class="LadoALado_Pequeno" style="width:130px; left: 800px">
        <button class="btnBusca" onclick="return btnCompliance_NegociosDiretos_Busca_Click(this)" id="btnCompliance_NegociosDiretos_Busca">Buscar</button>
    </p>
    <p class="LadoALado_Pequeno" style="width:100px; ">
        <button class="btnBusca" onclick="return btnCompliance_ImprimirRelatorio_NegociosDiretos(this)" id="btnCompliance_NegociosDiretos_Exportar">Exportar</button>
    </p>
</div>
<div style="position:absolute; top: 185px; background-color: White; width: 100%">
    <div id="div_Compliance_NegociosDiretos_Resultados" style="margin-top: 2em; margin-left: 2em; margin-bottom: 2em; display: none">
        <table id="tblBusca_Compliance_Resultados_NegociosDiretos" ></table>
    </div>
</div>
</form>


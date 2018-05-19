<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Risco.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Risco" %>

<form id="form1" runat="server">
    
    
<div id="pnlBusca_Risco_Form" class="Busca_Formulario">
    <p>
        <label for="cboBusca_Risco_BuscarPor">Buscar Por:</label>
        <select id="cboBusca_Risco_BuscarPor" style="width:8.3em">
            <option value="grupo">Grupos</option>
            <option value="permissao">Permissões</option>
            <option value="parametro">Parametros</option>
        </select>
    </p>

    <p style="width:auto;margin:-2.3em 0 0 17em">

        <label for="txtBusca_Risco_Termo" style="display:none">Termo de busca:</label>
        <input type="text" id="txtBusca_Risco_Termo" maxlength="30" style="width:14.6em;margin-right:0.4em" />

        <button onclick="return btnBusca_Click()" class="btnBusca">Buscar</button>
    </p>

</div>


<div id="pnlBusca_Risco_Resultados" class="Busca_Resultados">

    <table id="tblBusca_Risco_Resultados"></table>
    <div id="pnlBusca_Risco_Resultados_Pager"></div>
    
    <button class="ExpandirPraBaixo" title="Clique para expandir o painel e ver mais resultados" onclick="return btnResultadoBusca_ExpandirPainel_Click(this, 'tblBusca_Risco_Resultados')" style="">
        <span>Expandir tabela</span>
    </button>

</div>

</form>

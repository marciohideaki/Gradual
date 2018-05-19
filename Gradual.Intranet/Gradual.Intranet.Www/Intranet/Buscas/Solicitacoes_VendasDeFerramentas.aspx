<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solicitacoes_VendasDeFerramentas.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Solicitacoes_VendasDeFerramentas" %>

<form id="form1" runat="server">

<div id="pnlBusca_Solicitacoes_VendasDeFerramentas_Form" class="Busca_Formulario">

    <p>
        <label for="cboBusca_Solicitacoes_VendasDeFerramentas_BuscarPor">Buscar Por:</label>
        <select id="cboBusca_Solicitacoes_VendasDeFerramentas_BuscarPor" style="width:9.5em">
            <option value="CodBovespa" selected ="selected">Código Bovespa</option>
            <option value="CpfCnpj">CPF/CNPJ</option>
        </select>
    </p>

    <p style="width:auto;margin:-2.3em 0 0 18em">

        <label for="txtBusca_Solicitacoes_VendasDeFerramentas_Termo" style="display:none">Termo de busca:</label>
        <input type="text" id="txtBusca_Solicitacoes_VendasDeFerramentas_Termo" maxlength="30" style="width:13.6em;margin-right:0.4em" />

        <button class="btnBusca" onclick="return btnBusca_Click()">Buscar</button>
    </p>

    <p>
        <label for="cboBusca_Solicitacoes_VendasDeFerramentas_Status">Status:</label>
        <select id="cboBusca_Solicitacoes_VendasDeFerramentas_Status" style="width:9.5em">
            <option value="">Todos</option>
            <option value="1">1 - Aguardando Pagamento</option>
            <option value="2">2 - Em Análise</option>
            <option value="3">3 - Paga</option>
            <option value="4">4 - Disponível</option>
            <option value="5">5 - Em Disputa</option>
            <option value="6">6 - Devolvida</option>
            <option value="7">7 - Cancelada</option>
        </select>
    </p>

    <p style="width:42%">
        <label for="txtBusca_Solicitacoes_VendasDeFerramentas_DataInicial">Data De:</label>
        <input  id="txtBusca_Solicitacoes_VendasDeFerramentas_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>

    <p style="width:45%">
        <label for="txtBusca_Solicitacoes_VendasDeFerramentas_DataFinal" style="width:auto">Até:</label>
        <input  id="txtBusca_Solicitacoes_VendasDeFerramentas_DataFinal" type="text" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
    </p>


</div>

<div id="pnlBusca_Solicitacoes_VendasDeFerramentas_Resultados" class="Busca_Resultados">

    <table id="tblBusca_Solicitacoes_VendasDeFerramentas_Resultados"></table>
    <div id="pnlBusca_Solicitacoes_VendasDeFerramentas_Resultados_Pager"></div>

    <button class="ExpandirPraBaixo" title="Clique para expandir o painel e ver mais resultados" onclick="return btnResultadoBusca_ExpandirPainel_Click(this, 'tblBusca_Solicitacoes_VendasDeFerramentas_Resultados')" style="">
        <span>Expandir tabela</span>
    </button>

</div>

</form>


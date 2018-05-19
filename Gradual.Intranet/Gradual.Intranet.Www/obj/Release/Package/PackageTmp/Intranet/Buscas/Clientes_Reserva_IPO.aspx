<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientes_Reserva_IPO.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Clientes_Reserva_IPO" EnableViewState="false" %>

<form id="form1" runat="server">

<div id="pnlBusca_Clientes_Reserva_IPO_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_1Linha" style="width:600px">

    <p>
        <label for="cboBusca_Clientes_BuscarPor_IPO">Buscar Por:</label>
        <select id="cboBusca_Clientes_BuscarPor_IPO" style="width:9.5em" Propriedade="BuscarPor">
            <option value="CodBovespa" selected="selected">Código Bovespa</option>
            <option value="CpfCnpj">CPF/CNPJ</option>
            <option value="NomeCliente">Nome do Cliente</option>
        </select>
    </p>

    <p style="width:auto;margin:-2.3em 0 0 18em">

        <label for="txtBusca_Clientes_Termo_IPO" style="display:none">Termo de busca:</label>
        <input type="text" id="txtBusca_Clientes_Termo_IPO" maxlength="30" style="width:13.6em;margin-right:0.4em" Propriedade="TermoDeBusca" onkeydown="return txtBusca_Clientes_Termo_IPO_KeyDown(this)" />

        <button class="btnBusca" onclick="return btnBusca_Click(this)">Buscar</button>
    </p>


</div>

   
</form>
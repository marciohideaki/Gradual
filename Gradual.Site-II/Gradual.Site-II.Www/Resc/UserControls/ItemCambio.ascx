<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemCambio.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.ItemCambio" %>


<h3><%= this.NomeProduto %></h3>
<img src="<%= this.ImageSrc %>" class="ImagemCambio" />

<fieldset class="form-padrao" style="margin-top:-28px;">

    <input type="hidden" class="DadosDoProduto" value='<%= this.ProdutoJSON %>' />

    <table class="TabelaCambio">
        <tr>
            <td style="text-align:left"><label>Quantidade</label></td>
            <td><input type="text" maxlength="12" class="txtQtd validate[custom[onlyNumber]] ProibirLetras" data-HabilitarNoLoad="true" style="height:28px" onkeyup="txtCambioQtd_Change(this)" onkeydown="txtCambioQtd_KeyDown(this, event)" data-TxCambio="<%= this.dataTxCambio %>" data-IOF="<%= this.dataIOF %>" /></td>
        </tr>
        <tr>
            <td style="text-align:left"><label>Tx de Câmbio</label></td>
            <td> <%= this.TaxaDeCambio %> </td>
        </tr>
        <tr>
            <td style="text-align:left"><label>IOF</label></td>
            <td data-ValorTaxas="0"> <%= this.IOF %> </td>
        </tr>
        <tr>
            <td style="text-align:left"><label>Total</label></td>
            <td data-ValorTotal="0">-</td>
        </tr>
        <tr>
            <% if(this.Modo == "Moeda"){ %>

            <td colspan="2" style="text-align:center">
              <button class="botao btn-padrao" style="width:50%" onclick="return btnCarrinho_Comprar_Click(this)" data-Modo="Moeda" data-Tipo="Compra" data-Usuario="<%= (SessaoClienteLogado != null) %>" disabled="disabled" data-HabilitarNoLoad="true">Comprar</button> 
            </td>

            <% }else{ %>
            <td> <button class="botao btn-padrao" onclick="return btnCarrinho_Comprar_Click(this)" data-Modo="<%= this.Modo %>" data-Tipo="Compra" data-Usuario="<%= (SessaoClienteLogado != null) %>">Comprar</button> </td>
            <td> <button class="botao btn-padrao" onclick="return btnCarrinho_Comprar_Click(this)" data-Modo="<%= this.Modo %>" data-Tipo="Recarga" data-Usuario="<%= (SessaoClienteLogado != null) %>">Recarregar</button> </td>
            <% } %>
        </tr>
    </table>

</fieldset>

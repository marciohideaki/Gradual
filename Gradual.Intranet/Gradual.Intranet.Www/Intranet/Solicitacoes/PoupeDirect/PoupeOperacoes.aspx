<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PoupeOperacoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.PoupeDirect.PoupeOperacoes" %>

<form id="form1" runat="server">

    <div class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_6Linhas" style="width: 710px;">

    <h4>Solicitações de produtos Poupe Direct</h4>

    <p style="width: 75em; float: left;">
        <label for="cmbBusca_Status_Poupe_Operacoes">Status:</label>
        <select id="cmbBusca_Status_Poupe_Operacoes" style="width: 268px;">
            <option value="0">Aguardando Compra</option>
            <option value="1">Compra Efetuada</option>
        </select>
    </p>

    <p style="width: 75em;">
        <label for="txt_DataInicial">Data Inicial:</label>
        <input type="text" id="txt_DataInicial" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
    </p>

    <p style="width: 75em;">
        <label for="txt_DataFinal">Data Final:</label>
        <input type="text" id="txt_DataFinal" value='<%=DateTime.Now.AddDays(30).ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
    </p>

    <p style="width: 75em; float:left;">
        <label for="txt_CodigoCliente">Código Cliente:</label>
        <input id="txt_CodigoCliente" type="text" propriedade="CodigoCliente" style="width:100px;" />
    </p>

    <p style="float: left; text-align: right; width: 61em;">
        <button id="btnBuscarProduto" onclick="return btnBuscarProduto_Click(this);"> Buscar</button>
    </p>
    
    <p style="float: left; text-align: right; width: 63em;">
        <button class="btnBusca" onclick="window.print();return false;" id="btnPoupeOperacoesImprimir" style="width:auto;margin-left:8px;">Imprimir Relatório</button>
    </p>

    <p>
    
    </p>

</div>

<div style="padding-top: 18em; padding-left: 10px; display:block" id="PnlGrad_intra_PoupeOperacoes_Resultado">

    

</div>


<script>btnBuscarProduto_Click(this);</script>


</form>

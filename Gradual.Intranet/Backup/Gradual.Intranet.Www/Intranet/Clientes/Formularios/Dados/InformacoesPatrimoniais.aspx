<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="InformacoesPatrimoniais.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.InformacoesPatrimoniais" %>

<form id="form1" runat="server">

    <h4>Informações Patrimoniais</h4>

    <input type="hidden" id="hidClientes_InfPatrimoniais_DadosCompletos" class="DadosJson" runat="server" value="" />

    <input type="hidden" id="hidClientes_InfPatrimoniais_Id" Propriedade="Id" value="" />
    <input type="hidden" id="hidClientes_InfPatrimoniais_ParentId" Propriedade="ParentId" value="" />

    <p class="Maior1">
        <label for="txtClientes_InfPatrimoniais_TotalBensImoveis">Bens Imóveis:</label>
        <input type="text" id="txtClientes_InfPatrimoniais_TotalBensImoveis" Propriedade="TotalBensImoveis" maxlength="14" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
    </p>

    <p class="Maior1">
        <label for="txtClientes_InfPatrimoniais_TotalBensMoveis">Bens Móveis:</label>
        <input type="text" id="txtClientes_InfPatrimoniais_TotalBensMoveis" Propriedade="TotalBensMoveis" maxlength="14" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
    </p>

    <p class="Maior1">
        <label for="txtClientes_InfPatrimoniais_TotalAplicacoesFinanceiras">Aplicações Financeiras:</label>
        <input type="text" id="txtClientes_InfPatrimoniais_TotalAplicacoesFinanceiras" Propriedade="TotalAplicacoesFinanceiras" maxlength="14" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
    </p>

    <p class="Maior1">
        <label for="txtClientes_InfPatrimoniais_TotalSalarioProLabore">Salário / Pró-Labore:</label>
        <input type="text" id="txtClientes_InfPatrimoniais_TotalSalarioProLabore" Propriedade="TotalSalarioProLabore" maxlength="14" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
    </p>

    <p class="Maior1">
        <label for="txtClientes_InfPatrimoniais_TotalOutrosRendimentos">Outros Rendimentos:</label>
        <input type="text" id="txtClientes_InfPatrimoniais_TotalOutrosRendimentos" Propriedade="TotalOutrosRendimentos" onfocus="GradIntra_Clientes_Verificar_InfPatrimoniais_OutrosRendimentos();" onblur="GradIntra_Clientes_Verificar_InfPatrimoniais_OutrosRendimentos();" maxlength="14" class="ValorMonetario validate[custom[numeroFormatado]]" />
    </p>

    <p class="Maior1" style="height:8em">
        <label for="txtClientes_InfPatrimoniais_DescricaoOutrosRendimentos">Descrição:</label>        
        <textarea id="txtClientes_InfPatrimoniais_DescricaoOutrosRendimentos" Propriedade="DescricaoOutrosRendimentos" style="height:6em" onfocus="GradIntra_Clientes_Verificar_InfPatrimoniais_OutrosRendimentos();" onblur="GradIntra_Clientes_Verificar_InfPatrimoniais_OutrosRendimentos();"></textarea>
    </p>

    <p class="BotoesSubmit">

        <button id="btnSalvar" runat="server" onclick="return btnSalvar_Clientes_InformacoesPatrimoniais_Click(this)">Salvar Alterações</button>

    </p>

</form>

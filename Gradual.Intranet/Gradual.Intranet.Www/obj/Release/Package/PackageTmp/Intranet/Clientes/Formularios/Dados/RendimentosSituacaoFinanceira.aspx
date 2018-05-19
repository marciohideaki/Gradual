<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RendimentosSituacaoFinanceira.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.RendimentosSituacaoFinanceira" %>

<form id="form1" runat="server">

    <h4>Informações sobre Situção financeira e Patrimonial</h4>

    <h5>Data da última atualização: <input type="text" id="txtClientes_Rendimentos_DtAtualizacao" Propriedade="DtAtualizacao" readonly="readonly" /></h5>

    <input type="hidden" id="hidClientes_Rendimentos_DadosCompletos" class="DadosJson" runat="server" value="" />

    <input type="hidden" id="hidClientes_Rendimentos_Id" Propriedade="Id" value="" />
    <input type="hidden" id="hidClientes_Rendimentos_ParentId" Propriedade="ParentId" value="" />

    <p class="Menor1 Medade_Esquerda">
        <label for="txtClientes_Rendimentos_DtCapitalSocial">Capital Social em :</label>
        <input type="text" id="txtClientes_Rendimentos_DtCapitalSocial" Propriedade="DtCapitalSocial" maxlength="10" class="Mascara_Data validate[required,custom[data]]" />
    </p>

    <p class="Menor1 Medade_Direita">
        <label for="txtClientes_Rendimentos_TotalCapitalSocial">Valor em R$:</label>
        <input type="text" id="txtClientes_Rendimentos_TotalCapitalSocial" Propriedade="TotalCapitalSocial" maxlength="17" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
    </p>

    <p class="Menor1 Medade_Esquerda">
        <label for="txtClientes_Rendimentos_DtPatrimonio">Patrimônio em:</label>
        <input type="text" id="txtClientes_Rendimentos_DtPatrimonio" Propriedade="DtPatrimonioLiquido" maxlength="10" class="Mascara_Data validate[required,custom[data]]" />
    </p>

    <p class="Menor1 Medade_Direita">
        <label for="txtClientes_Rendimentos_TotalPatrimonio">Valor em R$:</label>
        <input type="text" id="txtClientes_Rendimentos_TotalPatrimonio" Propriedade="TotalPatrimonioLiquino" maxlength="17" class="ValorMonetario validate[required,custom[numeroFormatado]]" />
    </p>

    <p class="BotoesSubmit">
        <button id="btnSalvar" runat="server" onclick="return btnSalvar_Clientes_Rendimentos_Click(this)">Salvar Alterações</button>
    </p>

</form>
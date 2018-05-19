<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Variaveis.aspx.cs" Inherits="Gradual.Intranet.Www.SAC.Sistema.Variaveis" %>

<form id="form1" runat="server">

    <input type="hidden" id="hidSistema_Variaveis_PeriodicidadeRenovacaoCadastral" class="DadosJson" runat="server" value="" />
    <input type="hidden" id="hidSistema_Variaveis_PeriodicidadeRenovacaoCadastral_Id" class="Id" runat="server" />

    <p style="margin-top:2em">
        <label for="txtSistema_Variaveis_PeriodicidadeRenovacaoCadastral" style="width:26em">Periodicidade de Renovação Cadastral:</label>
        <input  id="txtSistema_Variaveis_PeriodicidadeRenovacaoCadastral" type="text"  runat="server" Propriedade="PeriodicidadeRenovacaoCadastral" class="validate[required,custom[onlyNumber]]" style="width:4em" />
    </p>

    <p class="BotoesSubmit" style="width:450px">
        
        <button onclick="return btnSistema_Variaveis_Salvar_Click(this)">Salvar Variáveis</button>
        
    </p>

</form>

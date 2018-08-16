<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesbloqueioCustodia.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.DesbloqueioCustodia" %>

<div class="pnlFormulario" style="left:30px; top:130px">
    <form id="form1" runat="server" class="pnlFormulario_Campos">
        <h4>Desbloqueio de Custodia</h4>

        <p>
            <label for="txtRisco_DesbloqueioCustodia_CodBovespa">Código Bovespa:</label>
            <input type="text" id="txtRisco_DesbloqueioCustodia_CodBovespa"  maxlength="12" class="validate[required,length[3,12]]" />
        </p>
        <p>
            <label for="txtRisco_DesbloqueioCustodia_Instrumento">Instrumento:</label>
            <input type="text" id="txtRisco_DesbloqueioCustodia_Instrumento"  maxlength="12" class="validate[required,length[5,12]]" />
        </p>

        <p>
            <label for="txtRisco_DesbloqueioCustodia_Quantidade">Quantidade:</label>
            <input type="text" id="txtRisco_DesbloqueioCustodia_Quantidade"  maxlength="8" class="validate[required,length[1,8]]" />
        </p>
    
        <p class="BotoesSubmit">
            <button onclick="return btnSalvar_Monitoramento_DebloqueioCustodia_Click(this)">Salvar Alterações</button>
        </p>    
    </form>
</div>
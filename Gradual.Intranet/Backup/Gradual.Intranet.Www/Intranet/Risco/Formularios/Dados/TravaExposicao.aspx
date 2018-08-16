<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TravaExposicao.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.TravaExposicao" %>
    
    <form id="form1" runat="server">
        <div id="pnlRestricoesPorCliente" style="padding-top:3%">
            <div id="pnlCliente_Restricoes_TravaExposicao">
                <h4>Trava exposição</h4>
                    <p >
                        <label style="width:150px;">Perc(%) Perda máxima:</label>
                        <input type="text" runat="server" class="ValorMonetario" id="txtPercentagem_Restricoes_PerdaMaxima" style="width: 9em;" maxlength="10" />
                    </p>
                    <br />
                    <p >
                        <label style="width:150px">Prej. Máximo atingido (R$):</label>
                        <input type="text" runat="server" class="ValorMonetario" id="txtPrejuizo_Restricoes_MaximoAtingido" style="width: 9em;" maxlength="10" />
                    </p>
                    
                    <p class="BotoesSubmit" style="margin-top:3em">
                        <button id="btnCliente_Restricoes" onclick="return GradIntra_Risco_Restricoes_TravaExposicao_Salvar_Click(this)">Gravar</button>
                    </p>
            </div>
        </div>
    </form>
    <%--<script>        $.ready(GradIntra_Risco_Restricoes_TravaExposicao_Load_Pagina());</script>--%>
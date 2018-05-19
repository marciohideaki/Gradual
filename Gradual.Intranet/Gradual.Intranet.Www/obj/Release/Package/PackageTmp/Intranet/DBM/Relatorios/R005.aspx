<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R005.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R005" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao RelatorioDBMImpressao">
    <h1>Relatório de <span>Total de Cliente por Assessor</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em
        <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

    <h3 style="margin-left: 12px; width: 75%;">
        <label><%= gTotalClientesOperaram %></label>
        <label>Clientes operaram entre os dias</label>
        <label><%= gDataConsultaInicio %></label>
        <label> e <%= gDataConsultaFim %></label>
    </h3>

    <table id="tblTotalDeAssessorPorCliente" class="GridRelatorio" style="margin-left: 10px; width: 97%;">
        <thead>
            <tr>
                <td style="text-align: left; width: auto;">Assessor: <%= base.UsuarioLogado.Nome %></td>
                <td style="text-align: left; width: auto;">Bolsa</td>
                <td style="text-align: center; width: auto;">C.B.</td>
                <td style="text-align: center; width: auto;" colspan="2">DV</td>
                <td style="text-align: center; width: auto;">C.L.</td>
                <td style="text-align: center; width: auto;">F.G.</td>
                <td style="text-align: center; width: auto;">P.C.</td>
                <td style="text-align: center; width: auto;">V.C.</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td style="text-align: right; width: auto;">Totais: </td>
                <td style="text-align: left;  width: auto;"></td>
                <td style="text-align: right; width: auto;"><%= gTotalCB %></td>
                <td style="text-align: right; width: auto;"><%= gTotalDV %></td>
                <td style="text-align: right; width: auto;">-</td>
                <td style="text-align: right; width: auto;"><%= gTotalCL %></td>
                <td style="text-align: right; width: auto;"><%= gTotalFG %></td>
                <td style="text-align: right; width: auto;"><%= gTotalPC %></td>
                <td style="text-align: right; width: auto;"><%= gTotalVC %></td>
            </tr>
        </tfoot>
        <tbody>
            <asp:repeater runat="server" id="rptTotalDeAssessorPorCliente">
                <itemtemplate>
                    <tr>
                        <td style="text-align: left; width: auto;"> <%# Eval("Cliente") %></td>
                        <td style="text-align: left; width: auto;"> <%# Eval("Bolsa") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("CB") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("DVDesconto") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("DVPercentual") %> %</td>
                        <td style="text-align: right; width: auto;"><%# Eval("CL") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("FG") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("PC") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("VC") %></td>
                    </tr>
                </itemtemplate>
            </asp:repeater>
        </tbody>
    </table>

</div>
</form>
<script language="javascript" type="text/javascript"> DefinirCoresValores_Load();</script>
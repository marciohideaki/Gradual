<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R006.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R006" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao RelatorioDBMImpressao">

    <h1>Relatório de <span>Saldo e Projeções em Conta Corrente</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

    <h2>
        Retirado em
        <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

    <h3 style="margin-left: 12px; width: 75%;">
        <label><%= gTotalClientesOperaram %> </label>
        <label>registros em </label>
        <label><%= gDataConsulta %></label>
    </h3>

    <table id="tblSaldoProjecoesEmContaCorrente" class="GridRelatorio" style="margin-left: 10px; width: 97%;">

        <thead>
            <tr>
                <td style="text-align: center; width: auto;">Código</td>
                <td style="text-align: center; width: auto;">Nome</td>
                <td style="text-align: center; width: auto;">Total</td>
                <td style="text-align: center; width: auto;">A Liquidar</td>
                <td style="text-align: center; width: auto;">Disponível</td>
                <td style="text-align: center; width: auto;">D + 1</td>
                <td style="text-align: center; width: auto;">D + 2</td>
            </tr>
        </thead>

        <tbody>
            <asp:repeater runat="server" id="rptSaldoProjecoesEmContaCorrente">
                <itemtemplate>
                    <tr>
                        <td style="text-align: right; width: auto;"><%# Eval("CodigoCliente") %></td>
                        <td style="text-align: left;  width: auto;"><%# Eval("NomeCliente") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("Total") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("ALiquidar") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("Disponivel") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("D1") %></td>
                        <td style="text-align: right; width: auto;"><%# Eval("D2") %></td>
                    </tr>
                </itemtemplate>
            </asp:repeater>
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="6">
                    Nenhum item encontrado.
                </td>
            </tr>
            <tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
                <td colspan="6">
                    Carregando dados, favor aguardar...
                </td>
            </tr>
        </tbody>
    </table>

</div>
</form>
<script language="javascript" type="text/javascript">DefinirCoresValores_Load();</script>
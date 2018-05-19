<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R007.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R007" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao RelatorioDBMImpressao">

    <h1>Relatório de <span>Movimento de Conta Corrente</span></h1>

    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

    <h2>
        Retirado em
        <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>
    
    <table id="tblMovimentoDeContaCorrente" class="GridRelatorio" style="margin-left: 10px; width: 97%;">
        <thead>
            <tr>
                <td style="text-align: center; width: auto;">Cliente</td>
                <td style="text-align: center; width: auto;">Data de Lançamento</td>
                <td style="text-align: center; width: auto;">Data de Referência</td>
                <td style="text-align: center; width: auto;">Data de Liquidação</td>
                <td style="text-align: center; width: auto;">Lançamento</td>
                <td style="text-align: center; width: auto;">Valor (R$)</td>
            </tr>
        </thead>
        <tbody>
            <asp:repeater runat="server" ID="rptMvtoContaCorrente">
                <itemtemplate>
                    <tr>
                        <td style="text-align: left;   width: auto;"><%# Eval("Cliente")    %></td>
                        <td style="text-align: center; width: auto;"><%# Eval("DataLanc")   %></td>
                        <td style="text-align: center; width: auto;"><%# Eval("DataRef")    %></td>
                        <td style="text-align: center; width: auto;"><%# Eval("DataLiq")    %></td>
                        <td style="text-align: left;   width: auto;"><%# Eval("Lancamento") %></td>
                        <td style="text-align: right;  width: auto;"><%# Eval("Valor")      %></td>
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
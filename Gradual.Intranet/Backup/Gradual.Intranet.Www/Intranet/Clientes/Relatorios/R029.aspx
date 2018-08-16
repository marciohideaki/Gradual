<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R029.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R029" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Monitoramento de TED's</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 6em;">
                Data Movto.
            </td>
            <td style="text-align: left; width: 6em;">
                Código
            </td>
            <td style="text-align: left; width: auto;">
                Nome
            </td>
            <td style="text-align: left; width: 6em;">
                Nr. Lançamento
            </td>
            <td style="text-align: left; width: auto;">
                Descrição
            </td>
            <td style="text-align: left; width: 6em;">
                Valor (R$)
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="14">
                &nbsp;
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:center">  <%# Eval("DataMovimento") %>            </td>
                <td style="text-align:left">    <%# Eval("CodigoCliente") %>            </td>
                <td style="text-align:left">    <%# Eval("NomeCliente") %>              </td>
                <td style="text-align:left">    <%# Eval("NumeroLancamento") %>         </td>
                <td style="text-align:left">    <%# Eval("Descricao") %>                </td>
                <td style="text-align:right">   <%# Eval("Valor") %>                    </td>
             </tr>

            </itemtemplate>
            </asp:repeater>
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">
                    Nenhum item encontrado.
                </td>
            </tr>
            <tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
                <td colspan="14">
                    Carregando dados, favor aguardar...
                </td>
            </tr>
    </tbody>
</table>
</form>

<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R027.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R027" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Conversão de contas Plural x Gradual</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 150px;">
                Conta Gradual
            </td>
            <td style="text-align: left; width: 350px;">
                Nome
            </td>
            <td style="text-align: center; width: 150px;">
                Assessor
            </td>
            <td style="text-align: center; width: 150px;">
                Conta Plural
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="4">
                &nbsp;
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:center">  <%# Eval("CodigoGradual")   %></td>
                <td style="text-align:left">    <%# Eval("Nome")            %></td>
                <td style="text-align:center">  <%# Eval("CodigoAssessor")  %></td>
                <td style="text-align:center">  <%# Eval("CodigoExterno")   %></td>
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

<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R008.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R008" %>


<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Emails Disparados por Período</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: left; width: 50em">
                Nome
            </td>
            <td style="text-align: left; width: 50em">
                Email Remetente
            </td>
            <td style="text-align: left; width: 50em">
                Email Destinatário
            </td>
            <td style="text-align: left; width: 16em">
                Cód. Bovespa
            </td>
            <td style="text-align: center; width: 16em">
                CPF / CNPJ
            </td>
            <td style="text-align: center; width: 16em">
                Data de Disparo
            </td>
            <td style="text-align: left; width: 75em">
                Assunto do E-mail
            </td>
            <td style="text-align: left; width: 75em">
                Perfil
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
                <td style="text-align:left">    <%# Eval("Nome")%>    </td>
                <td style="text-align:left">    <%# Eval("DsEmailRemetente")%>    </td>
                <td style="text-align:left">    <%# Eval("DsEmailDestinatario")%> </td>
                <td style="text-align:left">    <%# Eval("Bovespa")%>             </td>
                <td style="text-align:center">  <%# Eval("CpfCnpj")%>             </td>
                <td style="text-align:center">  <%# Eval("DataDeEnvio") %>        </td>
                <td style="text-align:left">    <%# Eval("Assunto") %>            </td>
                <td style="text-align:left">    <%# Eval("Perfil") %>             </td>
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

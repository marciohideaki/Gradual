<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R024.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R024" %>

<form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Clientes do Assessor </span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td colspan="5">Relatório de Clientes por assessor</td>
        </tr>
        <tr>
            <td style="text-align: center; width: 1em">
                Cód.Cliente Bov
            </td>
            <td style="text-align: center; width: 1em">
                CPF/CNPJ
            </td>
            <td style="text-align: left; width: 2.5em">
                Nome Cliente
            </td>
            <td style="text-align: left; width: 2.5em">
                E-mail Cliente
            </td>
            <td style="text-align: center; width: 0.8em">
                Cod.Assessor
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="5">
                <span>Relatório de Clientes do Assessor</span><spanem
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:Repeater id="rptRelatorioRebate" runat="server">
            <ItemTemplate>
                <tr>
                <td style="text-align:center">  <%# Eval("CodigoClienteBov")    %></td>
                <td style="text-align:center">  <%# Eval("CpfCnpj")             %></td>
                <td style="text-align:left">    <%# Eval("NomeCliente")         %></td>
                <td style="text-align:left">    <%# Eval("EmailCliente")        %></td>
                <td style="text-align:center">  <%# Eval("CodigoAssessor")      %></td>
                <%--<td style="text-align:center">  <%# Eval("ValorRepasse", "{0:N2}")   %></td>--%>
                </tr>
            </itemtemplate>
        </asp:repeater>
        <%--<tr id="rowsLinhaTotalRepasse" class="NenhumItem" runat="server">
            <td colspan="7" style="font-style:italic">Total de Repasse</td>
            <td style="font-style:italic"><b><%=TotalRepasse %></b></td>
        </tr>--%>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="5">
                Nenhum item encontrado.
             </td>
        </tr>

    </tbody>
</table>
    </form>

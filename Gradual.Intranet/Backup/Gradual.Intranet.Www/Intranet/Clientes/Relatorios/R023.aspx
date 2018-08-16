<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R023.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R023" %>

<form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Rebate de Fundos </span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td colspan="8">Relatório de Rebates</td>
        </tr>
        <tr>
            <td style="text-align: center; width: 1em">
                Cód.Cliente
            </td>
            <td style="text-align: center; width: 2.5em">
                Nome de Cliente
            </td>
            <td style="text-align: center; width: 0.8em">
                Cod.Assessor
            </td>
            <td style="text-align: center; width: 3em">
                Nome Fundo
            </td>
            <td style="text-align: center; width: 0.8em">
                Cod.Anbima
            </td>
            <td style="text-align: center; width: 1em">
                Valor Aplic.
            </td>
            <td style="text-align: center; width: 0.8em">
            Data Aplic.
            </td>
            <td style="text-align: center; width: 0.8em">
                Valor Repasse
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="8">
                <span>Relatório de Rebate de Fundos</span><spanem
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:Repeater id="rptRelatorioRebate" runat="server">
            <ItemTemplate>
                <tr>
                <td style="text-align:center">  <%# Eval("CodigoCliente")  %></td>
                <td style="text-align:left">    <%# Eval("NomeCliente")    %></td>
                <td style="text-align:center">  <%# Eval("CodigoAssessor") %></td>
                <td style="text-align:left">    <%# Eval("NomeFundo")      %></td>
                <td style="text-align:center">  <%# Eval("CodigoAnbima")   %></td>
                <td style="text-align:center">  <%# Eval("ValorAplicacao") %></td>
                <td style="text-align:center">  <%# Eval("DataAplicacao") %></td>
                <td style="text-align:center">  <%# Eval("ValorRepasse", "{0:N2}")   %></td>
                </tr>
            </itemtemplate>
        </asp:repeater>
        <tr id="rowsLinhaTotalRepasse" class="NenhumItem" runat="server">
            <td colspan="7" style="font-style:italic">Total de Repasse</td>
            <td style="font-style:italic"><b><%=TotalRepasse %></b></td>
        </tr>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="8">
                Nenhum item encontrado.
             </td>
        </tr>

    </tbody>
</table>
    </form>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R022.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R022" %>

<form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <%--<link rel="Stylesheet" media="print"  href="../Skin/<%= this.SkinEmUso %>/Relatorio.print.css?v=<%= this.VersaoDoSite %>" />--%>
    <h1>
        Relatório de <span>Papel por Cliente por período </span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    - Período De: <span><%= DataInicial.ToString("dd/MM/yyyy")%> </span> Até <span><%= DataFinal.ToString("dd/MM/yyyy")%> </span>
    </h2>

</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 2em">
                Cliente
            </td>
            <td style="text-align: center; width: 2em">
                Cod. Assessor
            </td>
            <td style="text-align: center; width: 2em">
                Data do Pregão
            </td>
            <td style="text-align: center; width: 2em">
                Papel
            </td>
            <td style="text-align: center; width: 2em">
                Qtde. Compras
            </td>
            <td style="text-align: center; width: 2em">
                Qtde. Vendas
            </td>
            <td style="text-align: center; width: 2em">
                Qtde. Líquida
            </td>
            <%--<td style="text-align: center; width: 2em">
                Preço Médio
            </td>--%>
            <td style="text-align: center; width: 2em">
                Preço Negócio
            </td>
            <td style="text-align: center; width: 2em">
                Vol. Compras
            </td>
            <td style="text-align: center; width: 2em">
                Vol. Vendas
            </td>
            <td style="text-align: center; width: 2em">
                Vol. Líquido
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="17">
                <span>Papel por Clientes por período </span><span>em
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <%# Eval("MostraTotal").ToString() == "nao" ? string.Format(
                "<tr>"+
                "<td style=\"text-align:center\">" + Eval("CodigoCliente")  + "</td>"+
                "<td style=\"text-align:center\">" + Eval("CodigoAssessor") + "</td>"+
                "<td style=\"text-align:center\">" + Eval("DataPregao")     + "</td>"+
                "<td style=\"text-align:center\">" + Eval("Papel")          + "</td>"+
                "<td style=\"text-align:center\">" + Eval("QtdeCompras")    + "</td>"+
                "<td style=\"text-align:center\">" + Eval("QtdeVendas")     + "</td>"+
                "<td style=\"text-align:center\"></td>"                     +
                "<td style=\"text-align:center\">" + Eval("VlNegocio")      + "</td>"+
                "<td style=\"text-align:center\">" + Eval("VolCompras")     + "</td>"+
                "<td style=\"text-align:center\">" + Eval("VolVendas")      + "</td>"+
                "<td style=\"text-align:center\">                             </td>" +
            "</tr>") : "" %>
            
            <%# Eval("MostraTotal").ToString() == "sim" ? string.Format("<tr>"+
                                                                "<td colspan=\"3\" style=\"font-weight:bold;text-align:center;border-top: 1px solid #000000;\" >Total </td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("Papel") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("TotalQtdeCompras") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("TotalQtdeVendas") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("QtdeLiquida") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("Preco") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("TotalVolCompras") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("TotalVolVendas") + "</td>" +
                                                                "<td style=\"font-weight:bold; text-align:center;border-top: 1px solid #000000;\">" + Eval("VolLiquido") + "</td>" +
                            "</tr>") : ""%>
            </itemtemplate>
        </asp:repeater>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="12">
                Nenhum item encontrado.
            </td>
        </tr>
        <%--<tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
            <td colspan="14">
                Carregando dados, favor aguardar...
            </td>
        </tr>--%>
    </tbody>
</table>
    </form>


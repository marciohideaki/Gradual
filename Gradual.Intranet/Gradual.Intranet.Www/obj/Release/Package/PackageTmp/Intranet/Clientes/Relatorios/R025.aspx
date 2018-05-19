<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R025.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R025" %>
<form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Renda Fixa por Assessor </span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td colspan="11">Relatório de Renda Fixa por assessor</td>
        </tr>
        <tr>
            <td colspan="11" align="Right">
                <table>
                <tr>
                    <td colspan="7"></td>
                    <td>Total Clientes: <asp:Label ID="lblTotalClientes" runat="server" /></td>
                    <td>Total Bruto:    <asp:Label ID="lblTotalBruto" runat="server" /></td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="7"></td>
                    <td>Total Aplicações: <asp:Label id="lblTotalAplicacoes" runat="server" /> </td>
                    <td>Total Liquido:    <asp:Label ID="lblTotalLiquido" runat="server" /></td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="8"></td>
                    <td colspan="3">Data Processamento: <asp:Label ID="lblDataProcessamento" runat="server" /> </td>
                </tr>
                </table>
            </td>
        </tr>

        <tr>
            <tr>
                <td align="left">Codigo Cliente</td>
                <td align="left">Título</td>
                <td align="center">Aplicação</td>
                <td align="center">Vencimento</td>
                <td align="center">Taxa</td>
                <td align="center">Quantidade</td>
                <td align="center">Valor Original</td>
                <td align="center">Saldo Bruto</td>
                <td align="center">IRRF</td>
                <td align="center">IOF</td>
                <td align="right">L&iacute;quido</td>
            </tr>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater id="rptRelatorioRendaFixa" runat="server">
            <ItemTemplate>
                <tr align="right" class="Template" style="font-size:x-small" >
                <td propriedade="Cliente"               align="left">           <%# Eval("CodigoCliente")  %></td>
                <td propriedade="Titulo"                align="left">           <%# Eval("Titulo")  %>       </td>
                <td propriedade="Aplicacao"             align="left">           <%# Eval("Aplicacao")  %>    </td>
                <td propriedade="Vencimento"                        >           <%# Eval("Vencimento")  %>   </td>
                <td propriedade="Taxa"                 style="width:58px">      <%# Eval("Taxa")  %>         </td>
                <td propriedade="Quantidade"                         >          <%# Eval("Quantidade")  %>   </td>
                <td propriedade="ValorOriginal"                      >          <%# Eval("ValorOriginal")  %></td>
                <td propriedade="SaldoBruto"                         >          <%# Eval("SaldoBruto")  %>   </td>
                <td propriedade="IRRF"                               >          <%# Eval("IRRF")  %>         </td>
                <td propriedade="IOF"                                >          <%# Eval("IOF")  %>          </td>
                <td propriedade="SaldoLiquido"                       >          <%# Eval("SaldoLiquido")  %> </td>
            </tr>
            </itemtemplate>
        </asp:repeater>
        <%--<tr id="rowsLinhaTotalRepasse" class="NenhumItem" runat="server">
            <td colspan="7" style="font-style:italic">Total de Repasse</td>
            <td style="font-style:italic"><b><%=TotalRepasse %></b></td>
        </tr>--%>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="11">
                Nenhum item encontrado.
             </td>
        </tr>

    </tbody>
    <tfoot>
        <tr>
            <td colspan="11">
                <span>Relatório de Renda Fixa por assessor</span><span>
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
</table>
    </form>
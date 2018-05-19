<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R014.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R014" %>

<form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
        <h1><span>Relatório de <span>Posição Consolidada Por Ativo</span></h1>
        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
        <h2>Retirado em
            <span>
                <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %>
            </span>
        </h2>
    </div>

    <table cellspacing="0" class="GridRelatorio">
        <thead>
            <tr>
                <td style="text-align: right;"> Código Cliente</td>
                <td style="text-align: left;">  Nome Cliente</td>
                <td style="text-align: left;">  Tipo de Cliente</td>
                <td style="text-align: center;">Assessor</td>
                <td style="text-align: center;">Papel</td>
                <td style="text-align: center;">Carteira</td>
                <td style="text-align: center;">Locador</td>
                <td style="text-align: right;"> Quantidade Total</td>
                <td style="text-align: right;"> Quantidade Disponível</td>
                <td style="text-align: right;"> Quantidade D + 1</td>
                <td style="text-align: right;"> Quantidade D + 2</td>
                <td style="text-align: right;"> Quantidade D + 3</td>
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
            <asp:repeater id="rptGradIntra_Clientes_Relatorios_PosicaoConsolidadaPorPapel" runat="server">
                <itemtemplate>
                    <tr>
                        <td><%# Eval("ClienteCodigo")   %></td>
                        <td><%# Eval("ClienteNome")     %></td>
                        <td><%# Eval("TipoCliente")     %></td>
                        <td><%# Eval("Assessor")        %></td>
                        <td><%# Eval("CodNeg")          %></td>
                        <td><%# Eval("Carteira")        %></td>
                        <td style="text-align: center;"><%# Eval("Locador")       %></td>
                        <td style="text-align: right;"> <%# Eval("Total")         %></td>
                        <td style="text-align: right;"> <%# Eval("Disponivel")    %></td>
                        <td style="text-align: right;"> <%# Eval("D1")            %></td>
                        <td style="text-align: right;"> <%# Eval("D2")            %></td>
                        <td style="text-align: right;"> <%# Eval("D3")            %></td>
                    </tr>
                </itemtemplate>
            </asp:repeater>
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="12">
                    Nenhum item encontrado.
                </td>
            </tr>
        </tbody>
    </table>

</form>

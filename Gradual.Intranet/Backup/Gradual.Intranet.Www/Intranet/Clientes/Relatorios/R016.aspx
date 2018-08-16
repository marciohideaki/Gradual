<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R016.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R016" %>

<form id="form1" runat="server">
<div>
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
        <h1>
            Relatório de <span>Clientes Poupe Direct Gradual</span></h1>
        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
        <h2>
            Retirado em <span>
                <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
    </div>
    <table cellspacing="0" class="GridRelatorio">
        <thead>
            <tr>
                <td style="text-align: left; width: auto">Cliente</td>
                <td style="text-align: left; width: auto">CPF/CNPJ</td>
                <td style="text-align: left; width: auto">Produto</td>
                <td style="text-align: left; width: auto">Ativo</td>
                <td style="text-align: center; width: auto">Adesão</td>
                <td style="text-align: center; width: auto">Data Retroativa para Troca de Ativo</td>
                <td style="text-align: center; width: auto">Vencimento</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="7"></td>
            </tr>
        </tfoot>
        <tbody>
            <asp:repeater runat="server" id="rptClientePlanoPoupe">
                <itemtemplate>
                    <tr>
                        <td style="text-align:left"><%# Eval("Cliente")%></td>
                        <td style="text-align:left"><%# Eval("CpfCnpj")%></td>
                        <td style="text-align:left"><%# Eval("Produto")%></td>
                        <td style="text-align:left"><%# Eval("Ativo")%></td>
                        <td style="text-align:center"><%# Eval("DataAdesao")%></td>
                        <td style="text-align:center"><%# Eval("DataRetroativaTrocaAtivo")%></td>
                        <td style="text-align:center"><%# Eval("DataVencimento")%></td>
                    </tr>
                </itemtemplate>
            </asp:repeater>
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="7">
                    Nenhum item encontrado.
                </td>
            </tr>
            <tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
                <td colspan="7">
                    Carregando dados, favor aguardar...
                </td>
            </tr>
        </tbody>
    </table>

</div>
</form>

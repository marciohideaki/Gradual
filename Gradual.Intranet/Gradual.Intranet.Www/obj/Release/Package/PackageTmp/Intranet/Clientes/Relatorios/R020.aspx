<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R020.aspx.cs" EnableViewState="false" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R020" %>

    <form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Total de Clientes Cadastrados por Assessor e Período</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 6em">
                Codigo do assessor
            </td>
            <td style="text-align: left; width: 3em">
                Nome do Assessor
            </td>
            <td style="text-align: center; width: 2em">
                Total de Clientes
            </td>
            <td style="text-align: center; width: 11em">
                Data de Cadastro
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="14">
                <span>Total de Clientes Cadastrados por Assessor e Período </span><span>em
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:center"> <%# Eval("CodigoAssessor") %>    </td>
                <td style="text-align:left"> <%# Eval("NomeAssessor")   %>    </td>
                <td style="text-align:center"> <%# Eval("TotalClientes") %>     </td>
                <td style="text-align:center"> <%# Eval("DataCadastro")  %>     </td>
            </tr>
            
            </itemtemplate>
        </asp:repeater>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="14">
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


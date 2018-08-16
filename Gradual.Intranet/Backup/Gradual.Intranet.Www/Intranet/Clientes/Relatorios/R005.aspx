<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R005.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R005" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Clientes Exportados para o Sinacor</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 6em">
                ID Cliente
            </td>
            <td style="text-align: center; width: auto">
                Bovespa
            </td>
            <td style="text-align: left; width: 20%">
                Nome
            </td>
            <td style="text-align: center; width: 11em">
                CPF / CNPJ
            </td>
            <td style="text-align: center; width: auto">
                Assessor
            </td>
            <td style="text-align: center; width: auto">
                Tipo Pessoa
            </td>
            <td style="text-align: center; width: 6em">
                Data do Passo 1
            </td>
            <td style="text-align: center; width: 6em">
                Primeira Exportação
            </td>
            <td style="text-align: center; width: 6em">
                Última Exportação
            </td>
            <td style="text-align: center; width: auto">
                Telefone Principal
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
                <td style="text-align:center"> <%# Eval("Id")                 %> </td>
                <td style="text-align:center"> <%# Eval("CodigoDeBolsa")      %> </td>
                <td style="text-align:left">   <%# Eval("Nome")               %> </td>
                <td style="text-align:right">  <%# Eval("CpfCnpj")            %> </td>
                <td style="text-align:center"> <%# Eval("Assessor")           %> </td>
                <td style="text-align:center"> <%# Eval("TipoDePessoa")       %> </td>
                <td style="text-align:center"> <%# Eval("DataDeCadastro")     %> </td>
                <td style="text-align:center"> <%# Eval("PrimeiraExportacao") %> </td>
                <td style="text-align:center"> <%# Eval("UltimaExportacao")   %> </td>
                <td style="text-align:center"> <%# Eval("Telefones")          %> </td>
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

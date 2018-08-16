<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R007.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R007" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Clientes Suspeitos</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: right; width: 9em">
                ID Cliente
            </td>
            <td style="text-align: right; width: 9em">
                Bovespa
            </td>
            <td style="text-align: left; width: 33%">
                Nome
            </td>
            <td style="text-align: center; width: 11em">
                CPF / CNPJ
            </td>
            <td style="text-align: right; width: 11em">
                Assessor
            </td>
            <td style="text-align: center; width: auto">
                Tipo Pessoa
            </td>
            <td style="text-align: center; width: 6em">
                Data de Cadastro
            </td>
            <td style="text-align: center; width: 6em">
                Exportado
            </td>
            <td style="text-align: center; width: auto">
                País Ilícito
            </td>
            <td style="text-align: center; width: auto">
                Atividade Ilícita
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
                <td style="text-align:right">  <%# Eval("Id") %>              </td>
                <td style="text-align:right">  <%# Eval("Bovespa")%>          </td>
                <td style="text-align:left">   <%# Eval("Nome") %>            </td>
                <td style="text-align:right">  <%# Eval("CpfCnpj") %>         </td>
                <td style="text-align:right">  <%# Eval("Assessor")%>         </td>
                <td style="text-align:center"> <%# Eval("TipoDePessoa") %>    </td>
                <td style="text-align:center"> <%# Eval("DataDeCadastro") %>  </td>
                <td style="text-align:center"> <%# Eval("Exportado") %>       </td>
                <td style="text-align:center"> <%# Eval("Pais") %>            </td>
                <td style="text-align:center"> <%# Eval("AtividadeIlicita")%> </td>
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

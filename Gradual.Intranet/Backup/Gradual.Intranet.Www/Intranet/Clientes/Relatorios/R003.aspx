<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R003.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R003" %>


<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Solicitações de Alteração Cadastral</span></h1>
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
            <td style="text-align: center; width: 6em">
                Bovespa
            </td>
            <td style="text-align: left; width: 25%">
                Nome
            </td>
            <td style="text-align: right; width: 11em">
                CPF / CNPJ
            </td>
            <td style="text-align: center; width: 6em">
                Assessor
            </td>
            <td style="text-align: center; width: 6em">
                Tipo Pessoa
            </td>
            <td style="text-align: center; width: 6em">
                Data da Solicitação
            </td>
            <td style="text-align: center; width: 6em">
                Data da Resolução
            </td>
            <td style="text-align: center; width: 6em">
                Tipo de Solicitação
            </td>
            <td style="text-align: left; width: auto">
                Informação
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
                <td style="text-align:center"> <%# Eval("Id") %>                 </td>
                 <td style="text-align:center">  <%# Eval("Bovespa") %>           </td>
                <td style="text-align:left">  <%# Eval("Nome") %>               </td>
                <td style="text-align:right"> <%# Eval("CpfCnpj") %>            </td>
                <td style="text-align:center">  <%# Eval("Assessor")%>            </td>
                  <td style="text-align:center">  <%# Eval("TpPessoa")%>            </td>
                <td style="text-align:center"> <%# Eval("DataDaSolicitacao") %>  </td>
                  <td style="text-align:center"> <%# Eval("DataDaResolucao") %>  </td>
                <td style="text-align:center">  <%# Eval("TipoDeSolicitacao") %>  </td>
                <td style="text-align:left">  <%# Eval("Campo")%>  </td>
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

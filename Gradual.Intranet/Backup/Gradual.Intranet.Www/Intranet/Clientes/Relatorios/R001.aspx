<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R001.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R001" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Clientes Cadastrados por Período</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 3em">
                ID Cliente
            </td>
            <td style="text-align: right; width: 7em">
                Bovespa
            </td>
            <td style="text-align: left; width: auto">
                Nome
            </td>
            <td style="text-align: center; width: 11em">
                CPF / CNPJ
            </td>
            <td style="text-align: center; width: auto">
                Assessor
            </td>
            <td style="text-align: center; width: 3em">
                Tipo Pessoa
            </td>
            <td style="text-align: center; width: 6em">
                Data do Passo 1
            </td>
            <td style="text-align: center; width: 6em">
               Dt Ult Atualização
            </td>
            <td style="text-align: center; width: 6em">
                Passo Atual
            </td>
            <td style="text-align: center; width: 3em">
                Exportado
            </td>
            <td style="text-align: left; width: auto">
                E-mail
            </td>
            <td style="text-align: center; width: auto">
                Telefone Principal
            </td>
            <td style="text-align: center; width: auto">
                Operar em
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="14">
                <span>Clientes Cadastrados por Período </span><span>em
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:center"> <%# Eval("Id") %>                </td>
                <td style="text-align:right">  <%# Eval("Bovespa") %>           </td>
                <td style="text-align:left">   <%# Eval("Nome") %>              </td>
                <td style="text-align:right">  <%# Eval("CpfCnpj") %>           </td>
                <td style="text-align:center"> <%# Eval("Assessor") %>          </td>
                <td style="text-align:center"> <%# Eval("TipoDePessoa") %>      </td>
                <td style="text-align:center"> <%# Eval("DataDeCadastro") %>    </td>
                <td style="text-align:center"> <%# Convert.ToDateTime(Eval("DataUltAtualizacao")).ToString("dd/MM/yyyy HH:mm")%> </td>
                <td style="text-align:center"> <%# Eval("PassoAtual")%>         </td>
                <td style="text-align:center"> <%# Eval("Exportado") %>         </td>
                <td style="text-align:left">   <%# Eval("DsEmail")%>            </td>
                <td style="text-align:center"> <%# Eval("Telefones") %>         </td>
                <td style="text-align:center"> <%# Eval("DsDesejaOperarEm")%>   </td>

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
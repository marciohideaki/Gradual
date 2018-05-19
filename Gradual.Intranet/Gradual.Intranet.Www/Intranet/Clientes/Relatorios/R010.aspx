<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R010.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R010" %>

<form id="form1" runat="server">
<table cellspacing="0" class="GridRelatorio">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <thead>
        <tr>
            <td style="text-align: center; width: 6em">
                ID Cliente
            </td>
            <td style="text-align: left; width: auto">
                Nome
            </td>
            <td style="text-align: left; width: 12em">
                Cpf / Cnpj
            </td>
            <td style="text-align: left; width: 7em">
                Tipo Pessoa
            </td>
            <td style="text-align: center; width: 6em">
                Data de Cadastro
            </td>
            <td style="text-align: left; width: auto">
                Email
            </td>
            <td style="text-align: left; width: 6em">
                Assessor
            </td>
            <td style="text-align: left; width: 6em">
                Conta
            </td>
            <td style="text-align: left; width: 6em">
                Atividade
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
               <td style="text-align:center">   <%# Eval("Id") %>               </td>
                <td style="text-align:left">    <%# Eval("Nome") %>             </td>
                <td style="text-align:left">    <%# Eval("CpfCnpj") %>          </td>
                <td style="text-align:center">  <%# Eval("TipoDePessoa") %>     </td>
                <td style="text-align:center">  <%# Eval("DataDeCadastro") %>   </td>
                <td style="text-align:left">    <%# Eval("Email") %>            </td>
                <td style="text-align:center">  <%# Eval("Assessor") %>         </td>
                <td style="text-align:center">  <%# Eval("Conta") %>            </td>
                <td style="text-align:center">  <%# Eval("TipoConta") %>        </td>
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

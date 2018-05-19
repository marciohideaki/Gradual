<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R012.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R012" %>


<form id="form1" runat="server">

    <table>
        <thead>
            <tr>
                <td>Clientes Encontrados: <span style="font-size:15px;font-weight:bold"><%= gTotalClientes %></span> </td>
            </tr>
        </thead>
    </table>

    <table cellspacing="0" class="GridRelatorio">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
        <thead>
            <tr>
                <td style="text-align: left;">
                    Nome Cliente
                </td>
                <td style="text-align: left;">
                    Nome Assessor
                </td>
                <td style="text-align: right;">
                    Data de Cadastro
                </td>
                <td style="text-align: right;">
                    Data da última operação
                </td>
                <td style="text-align: right;">
                    Total
                </td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
                <td style="border-top: 1px solid #666;font-size:1em;font-weight:bold;color:#666">
                    R$ <%= string.Format("{0:n}", gTotalRelatorio) %>
                </td>
            </tr>
        </tfoot>
        <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:left">   <%# Eval("NM_Cliente")%>  </td>
                <td style="text-align:left">   <%# Eval("NM_Assessor")%>  </td>
                <td style="text-align:right">  <%# Eval("DT_Criacao",  "{0:dd/MM/yyyy}")%>  </td>
                <td style="text-align:right">  <%# Eval("DT_Ult_Oper", "{0:dd/MM/yyyy}")%>  </td>
                <td style="text-align:right">  <%# Eval("Total", "{0:n}")%>  </td>
            </tr>
            </itemtemplate>
        </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="5">
                    Nenhum item encontrado.
                </td>
            </tr>

        </tbody>
    </table>

</form>

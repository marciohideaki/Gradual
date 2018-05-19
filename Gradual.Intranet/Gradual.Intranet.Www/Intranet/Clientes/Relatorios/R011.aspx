<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R011.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R011" %>
<form id="form1" runat="server">
<table>
    <thead>
        <tr>
            <td align="center">Clientes Encontrados: <span style="font-size:15px;"><b><%=gTotalClientes %></b></span> </td>
        </tr>
    </thead>
</table>
<table cellspacing="0" class="GridRelatorio">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <thead>
        <tr>
            <td style="text-align: center; width: 6em">
                Cod. Bolsa
            </td>
            <td style="text-align: left; width: 20em">
                Nome Cliente
            </td>
            <td style="text-align: left; width: 6em">
                Assessor
            </td>
            <td style="text-align: left; width: 12em">
                Cpf / Cnpj
            </td>
            <td style="text-align: left; width: 7em">
                Produtos
            </td>
            <td style="text-align: center; width: 6em">
                Data de Adesão
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="6">
                &nbsp;
            </td>
        </tr>
    </tfoot>
    <tbody>
     <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
               <td style="text-align:center">   <%# Eval("CodBolsa")%>   </td>
               <td style="text-align:left">    <%# Eval("NomeCliente")%> </td>
               <td style="text-align:left">    <%# Eval("CodAssessor")%> </td>
               <td style="text-align:left">    <%# Eval("CpfCnpj")%>     </td>
               <td style="text-align:center">  <%# Eval("Produto")%>     </td>
               <td style="text-align:center">  <%# Eval("DataAdesao")%>  </td>
            </tr>
            </itemtemplate>
            </asp:repeater>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="6">
                Nenhum item encontrado.
            </td>
        </tr>
        <tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
            <td colspan="6">
                Carregando dados, favor aguardar...
            </td>
        </tr>
    </tbody>
</table>
</form>
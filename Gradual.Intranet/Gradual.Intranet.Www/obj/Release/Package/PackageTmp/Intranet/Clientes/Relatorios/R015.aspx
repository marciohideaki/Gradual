<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R015.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R015" %>

<form id="form1" runat="server">
<table>
    <thead>
        <tr>
            <td align="center">
                Clientes Encontrados: <span style="font-size: 15px;"><b>
                    <%=gTotalClientes %></b></span>
            </td>
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
            <td style="text-align: left; width: 12em">
                Cpf / Cnpj
            </td>
            <td style="text-align: left; width: 7em">
                Descrição Produto
            </td>
            <td style="text-align: center; width: 6em">
                Data Início do Plano
            </td>
            <td style="text-align: center; width: 6em">
                Data Fim do Plano
            </td>
            <td style="text-align: center; width: 6em">
                Data Utima Cobrança
            </td>
            <td style="text-align:center; width: 6em">
                Origem
            </td>
            <td style="text-align:center; width: 6em">
                Usuario Logado
            </td>
            <td style="text-align: left; width: 6em">
                Valor Cobrado
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="10">
                &nbsp;
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
                <tr>
                   <td style="text-align:center"> <%# Eval("CodBolsa")      %> </td>
                   <td style="text-align:left">   <%# Eval("NomeCliente")   %> </td>
                   <td style="text-align:left">   <%# Eval("CpfCnpj")       %> </td>
                   <td style="text-align:left">   <%# Eval("Produto")       %> </td>
                   <td style="text-align:center"> <%# Eval("DataAdesao")    %> </td>
                   <td style="text-align:center"> <%# Eval("DataFimAdesao") %> </td>
                   <td style="text-align:center"> <%# Eval("DataCobranca" ) %> </td>
                   <td style="text-align:center"> <%# Eval("Origem" )       %> </td>
                   <td style="text-align:left">   <%# Eval("UsuarioLogado" )%> </td>
                   <td style="text-align:center"> <%# Eval("ValorCobranca") %> </td>
                </tr>
            </itemtemplate>
        </asp:repeater>

        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="10">
                Nenhum item encontrado.
            </td>
        </tr>

        <tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
            <td colspan="10">
                Carregando dados, favor aguardar...
            </td>
        </tr>
    </tbody>
</table>
</form>

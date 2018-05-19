<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PoupeOperacoesResultado.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.PoupeDirect.PoupeOperacoesResultado" %>


    <form id="form1" runat="server">
    
    <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo" style="width: 710px;">
        <thead>
            <tr>
                <td style="text-align: center; width: 8em">
                    Notificação de Compra
                </td>
                <td style="text-align: center; width: 10em">
                    Cód Cliente
                </td>
                <td style="text-align: center; width: 10em">
                    CC Poupe
                </td>
                <td style="text-align: center; width: 20em">
                    Produto
                </td>
                <td style="text-align: center; width: 10em">
                    Ativo
                </td>
                <td style="text-align: center; width: 10em">
                    Valor Ativo
                </td>
                <td style="text-align: center; width: 10em">
                    Qtd. Compra
                </td>
                <td style="text-align: center; width: 10em">
                    Data Limite
                </td>
                <td style="text-align: center; width: 10em">
                    Dt Notificação Compra
                </td>
            
            </tr>
        </thead>
        <tbody>
            <asp:repeater id="rptRelatorio" runat="server">
                <itemtemplate>

                 <tr>
                    <td style="text-align:center; width:  8em; height: 3em">  <%# Eval("DataCompra") == null ? "<input class='chkSolicitacoes_PoupeOperacoes_Compra' type='checkbox' /> " : "" %>      <input type="hidden"  value='<%# Eval("CodigoVencimento") %>' /> </td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# ToCodigoClienteComDigito(Eval("CodigoCliente")) %></td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# Eval("ValorDisponivel","{0:N2}")%></td>
                    <td style="text-align:center; width: 20em; height: 3em">  <%# Eval("DescricaProduto")                         %></td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# Eval("CodigoAtivo")                             %></td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# Eval("ValorPapel","{0:N2}")%></td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# Eval("QuantidadeCompra", "{0:N2}")%></td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# Eval("DataRetroTrocaPlano", "{0:dd/MM/yyyy}")   %></td>
                    <td style="text-align:center; width: 10em; height: 3em">  <%# Eval("DataCompra", "{0:dd/MM/yyyy}")            %></td>
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
        <tfoot>
            <tr>
                <td colspan="9" align="right">
                  <button id="btnCompra" onclick="return btnPoupeCompra_Click(this);"> Efetivar Compra</button>
                </td>
            </tr>
        </tfoot>
    </table>
    </form>


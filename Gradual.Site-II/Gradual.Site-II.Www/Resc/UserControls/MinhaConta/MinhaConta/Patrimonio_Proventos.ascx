<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patrimonio_Proventos.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta.Patrimonio_Proventos" %>

<h4>Total em Proventos: <%= String.Format("R$ {0}", CurrentTotalProventos.ToString("N2"))%></h4>

<asp:Repeater ID="rptProventos" runat="server">
    <HeaderTemplate>
        <table class="tabela">
            <tr class="tabela-titulo">
                <td colspan="4">Evento          </td>
                <td>Ativo                       </td>
                <td>Quantidade                  </td>
                <td>Dt. Pagamento               </td>
                <td style="width: 120px;">Valor </td>
            </tr>
    </HeaderTemplate>
    <itemtemplate>
            <tr class="tabela-type-small">
                <td colspan="4"><%# Eval("Evento")              %></td>
                <td><%# Eval("Ativo")                           %></td>
                <td><%# Eval("Quantidade", "{0:N0}")            %></td>
                <td><%# Eval("DataPagamento", "{0:dd/MM/yyyy}") %></td>
                <td><%# Eval("Valor", "{0:c}")                  %></td>
            </tr>
    </itemtemplate>
    <FooterTemplate>
            <tr id="trEmpty" runat="server" visible="false">
                <td colspan="8" align = "center">
                    Nenhum registro.
                </td>
            </tr>
            <tr id="trError" runat="server" visible="false">
                <td colspan="8" align = "center">
                    Nenhum registro.
                </td>
            </tr>
            <tr class="tabela-titulo">
                <td colspan="7"><%= "Total"%></td>
                <td style="width: 120px;"><%= String.Format("R$ {0}", CurrentTotalProventos.ToString("N2"))%></td>
            </tr>
        </table>
    </FooterTemplate>
</asp:Repeater>
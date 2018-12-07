<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patrimonio_RendaFixa.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta.Patrimonio_RendaFixa" %>

<h4>Total em Renda Fixa <%= String.Format(CurrentCustodiaTesouroTotal_Rotulo, CurrentTotalGeral.ToString("N2")) %></h4>
<div class="panel-group">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <a data-toggle="collapse" href="#collapse13"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Títulos Públicos (Tesouro Direto): <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaTesouroTotal_Rotulo, CurrentCustodiaTesouroTotal.ToString("N2"))%></span></a>
      </h4>
    </div>
    <div id="collapse13" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptCustodiaTesouro" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td colspan="5">Títulos Públicos (Tesouro Direto)</td>
                            <td>Quantidade</td>
                            <td>Preço Unitário</td>
                            <td style="width: 120px; text-align:right;">Valor Bruto</td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td colspan="5">
                                <%# Eval("Instrumento")%>
                            </td>
                            <td>
                                <%# Eval("Quantidade", "{0:N2}")%>
                            </td>
                            <td>
                                <%# Eval("Preco", "{0:c}")%>
                            </td>
                            <td style="width: 120px; text-align:right;">
                                <%# Eval("ValorPosicao", "{0:c}") %>
                            </td>
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
                            <td colspan="7">Total</td>
                            <td style="width: 120px; text-align:right;"><%= String.Format(CurrentCustodiaTesouroTotal_Rotulo, CurrentCustodiaTesouroTotal.ToString("N2"))%></td>
                        </tr>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="panel-footer">
        <!-- Panel Footer -->
        </div>
    </div>
  </div>
</div>

<div class="panel-group">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <a data-toggle="collapse" href="#collapse14"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Títulos Privados: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentTituloPrivadoTotal_Rotulo, CurrentTituloPrivadoTotal.ToString("N2"))%></span></a>
      </h4>
    </div>
    <div id="collapse14" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptListaTituloPrivado" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td colspan="2">Títulos Privados        </td>
                            <td colspan="2">Emissor                 </td>
                            <td>Data da Posição                     </td>
                            <td>Quantidade                          </td>
                            <td>Preço unitário                      </td>
                            <td style="width: 120px; text-align:right;">Valor Bruto   </td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td colspan="2"><%# Eval("Instrumento")                     %></td>
                            <td colspan="2"><%# Eval("Emissor")                         %></td>
                            <td><%# Eval("DataPosicao", "{0:dd/MM/yyyy}")               %></td>
                            <td><%# Eval("Quantidade", "{0:N2}")                        %></td>
                            <td><%# Eval("Preco","{0:c}")                               %></td>
                            <td style="width: 120px; text-align:right;"><%# Eval("ValorBruto", "{0:c}")   %></td>
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
                            <td colspan="7">Total</td>
                            <td style="width: 120px; text-align:right;"><%= String.Format(CurrentTituloPrivadoTotal_Rotulo, CurrentTituloPrivadoTotal.ToString("N2"))%></td>
                        </tr>
                    </table>
                </FooterTemplate>
                </asp:Repeater>
            </div>
        <div class="panel-footer">
        <!-- Panel Footer -->
        </div>
    </div>
  </div>
</div>
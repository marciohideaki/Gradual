<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patrimonio_FundosInvestimentos.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta.Patrimonio_FundosInvestimentosWebUserControl1" %>

<h4>Total em Fundos de Investimentos: <%= String.Format(CurrentCustodiaFundoAcaoTotal_Rotulo, CurrentTotalGeral.ToString("N2")) %></h4>

<div class="panel-group">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <a data-toggle="collapse" href="#collapse10"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Fundos de Ações: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaFundoAcaoTotal_Rotulo, CurrentCustodiaFundoAcaoTotal.ToString("N2"))%></span></a>
      </h4>
    </div>
    <div id="collapse10" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptFundoPosicaoAcao" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td>Fundos de Ações                     </td>
                            <td>Data da Posição                     </td>
                            <td>Quantidade de Cotas                 </td>
                            <td>Valor da Cota                       </td>
                            <%--<td>Valor Bruto da Posição              </td>--%>
                            <td>IR                                  </td>
                            <td>IOF                                 </td>
                            <td style="width: 120px; text-align:right;">Valor Líquido                       </td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td><%# Eval("NomeFundo")               %></td>
                            <td><%# Eval("DataPosicao","{0:dd/MM/yyyy}")             %></td>
                            <td><%# Eval("QtdCotas", "{0:N0}")      %></td>
                            <td><%# Eval("ValorCota", "{0:c}")      %></td>
                            <%--<td><%# Eval("ValorBruto", "{0:c}")     %></td>--%>
                            <td><%# Eval("IR")                      %></td>
                            <td><%# Eval("IOF")                     %></td>
                            <td style="width: 120px; text-align:right;" class="<%# GetClassFundo_ValorLiquido( Container.DataItem ) %>">><%# Eval("ValorLiquido", "{0:c}")   %></td>
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
                            <td colspan="6"><%= "Total"%></td>
                            <td style="width: 120px; text-align:right;"><%= String.Format(CurrentCustodiaFundoAcaoTotal_Rotulo, CurrentCustodiaFundoAcaoTotal.ToString("N2"))%></td>
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
        <a data-toggle="collapse" href="#collapse11"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Fundos de Renda Fixa: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaFundoRendaFixaTotal_Rotulo, CurrentCustodiaFundoRendaFixaTotal.ToString("N2"))%></span></a>
      </h4>
    </div>
    <div id="collapse11" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptFundoPosicaoRendaFixa" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td>Fundos de Renda Fixa                </td>
                            <td>Data da Posição                     </td>
                            <td>Quantidade de Cotas                 </td>
                            <td>Valor da Cota                       </td>
                            <%--<td>Valor Bruto da Posição              </td>--%>
                            <td>IR                                  </td>
                            <td>IOF                                 </td>
                            <td style="width: 120px; text-align:right;">Valor Líquido                       </td>
                        </tr>
                </HeaderTemplate>    
                <itemtemplate>
                         <tr class="tabela-type-small">
                            <td><%# Eval("NomeFundo")               %></td>
                            <td><%# Eval("DataPosicao", "{0:dd/MM/yyyy}")%></td>
                            <td><%# Eval("QtdCotas", "{0:N0}")      %></td>
                            <td><%# Eval("ValorCota", "{0:c}")      %></td>
                            <%--<td><%# Eval("ValorBruto", "{0:c}")     %></td>--%>
                            <td><%# Eval("IR")                      %></td>
                            <td><%# Eval("IOF")                     %></td>
                            <td style="width: 120px; text-align:right;" class="<%# GetClassFundo_ValorLiquido( Container.DataItem) %>"><%# Eval("ValorLiquido", "{0:c}")   %></td>
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
                            <td colspan="6"><%= "Total"%></td>
                            <td class="<%= GetClass_Valor(CurrentCustodiaFundoRendaFixaTotal) %>"><%= String.Format(CurrentCustodiaFundoRendaFixaTotal_Rotulo, CurrentCustodiaFundoRendaFixaTotal.ToString("N2")) %></td>
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
        <a data-toggle="collapse" href="#collapse12"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Fundos Multimercado: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaFundoMultimercadoTotal_Rotulo, CurrentCustodiaFundoMultimercadoTotal.ToString("N2")) %></span></a>
      </h4>
    </div>
    <div id="collapse12" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptFundoPosicaoMultimercado" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td>Fundos Multimercados</td>
                            <td>Data da Posição</td>
                            <td>Quantidade de Cotas                 </td>
                            <td>Valor da Cota                       </td>
                            <%--<td>Valor Bruto da Posição              </td>--%>
                            <td>IR                                  </td>
                            <td>IOF                                 </td>
                            <td style="width: 120px; text-align:right;">Valor Liquido</td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td><%# Eval("NomeFundo")               %></td>
                            <td><%# Eval("DataPosicao", "{0:dd/MM/yyyy}")%></td>
                            <td><%# Eval("QtdCotas", "{0:N0}")      %></td>
                            <td><%# Eval("ValorCota", "{0:c}")      %></td>
                            <%--<td><%# Eval("ValorBruto", "{0:c}")     %></td>--%>
                            <td><%# Eval("IR")                      %></td>
                            <td><%# Eval("IOF")                     %></td>
                            <td style="width: 120px; text-align:right;"><%# Eval("ValorLiquido", "{0:c}")   %></td>
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
                            <td colspan="6"><%= "Total"%></td>
                            <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentCustodiaFundoMultimercadoTotal) %>"><%= String.Format(CurrentCustodiaFundoMultimercadoTotal_Rotulo, CurrentCustodiaFundoMultimercadoTotal.ToString("N2")) %></td>
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
        <a data-toggle="collapse" href="#collapse15"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Outros: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaFundoOutrosTotal_Rotulo, CurrentCustodiaFundoOutrosTotal.ToString("N2")) %></span></a>
      </h4>
    </div>
    <div id="Div1" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptFundoPosicaoOutros" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td>Fundo</td>
                            <td>Data da Posição</td>
                            <td>Quantidade de Cotas                 </td>
                            <td>Valor da Cota                       </td>
                            <%--<td>Valor Bruto da Posição              </td>--%>
                            <td>IR                                  </td>
                            <td>IOF                                 </td>
                            <td style="width: 120px; text-align:right;">Valor Liquido</td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td><%# Eval("NomeFundo")               %></td>
                            <td><%# Eval("DataPosicao", "{0:dd/MM/yyyy}")%></td>
                            <td><%# Eval("QtdCotas", "{0:N0}")      %></td>
                            <td><%# Eval("ValorCota", "{0:c}")      %></td>
                            <%--<td><%# Eval("ValorBruto", "{0:c}")     %></td>--%>
                            <td><%# Eval("IR")                      %></td>
                            <td><%# Eval("IOF")                     %></td>
                            <td style="width: 120px; text-align:right;"><%# Eval("ValorLiquido", "{0:c}")   %></td>
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
                            <td colspan="6"><%= "Total"%></td>
                            <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentCustodiaFundoOutrosTotal) %>"><%= String.Format(CurrentCustodiaFundoMultimercadoTotal_Rotulo, CurrentCustodiaFundoMultimercadoTotal.ToString("N2")) %></td>
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patrimonio_RendaVariavel.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta.Patrimonio_RendaVariavel" %>

<h4>Total em Renda Variável: <%= String.Format(CurrentCustodiaAcaoTotal_Rotulo, CurrentTotalGeral.ToString("N2"))%></h4>

<div class="panel-group">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <a data-toggle="collapse" href="#collapse4">
            <span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Ações: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaAcaoTotal_Rotulo, CurrentCustodiaAcaoTotal.ToString("N2")) %></span></a>
      </h4>
    </div>
    <div id="collapse4" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptCustodiaAcao" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
                        <tr class="tabela-titulo">
                            <td>Ações           </td>
                            <td>Quantidade      </td>
                            <td>Preço           </td>
                            <td style="width: 120px; text-align:right;">Valor da Posição</td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td><%# Eval("Instrumento")             %></td>
                            <td><%# Eval("Quantidade", "{0:N0}")       %></td>
                            <td><%# Eval("Preco", "{0:c}")          %></td>
                            <td style="width: 120px; text-align:right;" class="<%# this.GetClassCustodiaAcao_ValorPosicao( Container.DataItem ) %>"><%# Eval("ValorPosicao", "{0:c}")   %></td>
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
                            <td colspan="3"><%= "Total"%></td>
                            <td style="width: 120px; text-align:right;"><%= String.Format(CurrentCustodiaAcaoTotal_Rotulo, CurrentCustodiaAcaoTotal.ToString("N2")) %></td>
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
        <a data-toggle="collapse" href="#collapse5"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Operações à Termo: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaTermoTotal_Rotulo, CurrentCustodiaTermoTotal.ToString("N2")) %></span></a>
      </h4>
    </div>
    <div id="collapse5" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <asp:Repeater ID="rptCustodiaTermo" runat="server">
                <HeaderTemplate>
                    <table class="tabela-minhaconta">
<%--                        
                        <tr class="tabela-titulo">
                            <td colspan="9">Operações à Termo</td>   
                        </tr>
--%>
                        <tr class="tabela-titulo">
                            <td colspan="3">Termo           </td>
                            <td>Data Vencimento             </td>
                            <td>Data da Rolagem             </td>
                            <td>Quantidade Disponível       </td>
                            <td>Valor à Pagar               </td>
                            <td>Valor Atual                 </td>
                            <td style="width: 120px; text-align:right;">Resultado Termo </td>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>
                        <tr class="tabela-type-small">
                            <td colspan="3"><%# Eval("CodigoNegocio")           %></td>
                            <td><%# Eval("DataVencimento","{0:dd/MM/yyyy}")     %></td>
                            <td><%# Eval("DataRolagem","{0:dd/MM/yyyy}")        %></td>
                            <td><%# Eval("QuantidadeDisponivel", "{0:N0}")                %></td>
                            <td><%# Eval("FinanceiroATermo","{0:c}")            %></td>
                            <td><%# Eval("Financeiro", "{0:c}")                 %></td>
                            <td style="width: 120px; text-align:right;"><%#Eval("ResultadoTermo", "{0:c}") %></td>
                        </tr>
                </itemtemplate>
                <FooterTemplate>
                        <tr id="trEmpty" runat="server" visible="false">
                            <td colspan="9" align = "center">
                                Nenhum registro.
                            </td>
                        </tr>
                        <tr id="trError" runat="server" visible="false">
                            <td colspan="9" align = "center">
                                Nenhum registro.
                            </td>
                        </tr>
                        <tr class="tabela-titulo">
                            <td colspan="8"><%= "Total"%></td>
                            <td style="width: 120px; text-align:right;"><%= String.Format(CurrentCustodiaTermoTotal_Rotulo, CurrentCustodiaTermoTotal.ToString("N2")) %></td>
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
                <a data-toggle="collapse" href="#collapse6"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Opções: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaOpcaoTotal_Rotulo, CurrentCustodiaOpcaoTotal.ToString("N2")) %></span></a>
            </h4>
        </div>
        <div id="collapse6" class="panel-collapse collapse">
            <div class="panel-body-minhaconta">
                <asp:Repeater ID="rptCustodiaOpcao" runat="server">
                    <HeaderTemplate>
                        <table class="tabela-minhaconta">
<%--                            
                            <tr class="tabela-titulo">
                                <td colspan="8">Opções</td>   
                            </tr>
--%>
                            <tr class="tabela-titulo">
                                <td>Instrumento</td>
                                <td>Quantidade</td>
                                <td>Preço</td>
                                <td style="width: 120px; text-align:right;">Valor da Posição</td>
                            </tr>
                    </HeaderTemplate>
                    <itemtemplate>
                            <tr class="tabela-type-small">
                                <td><%# Eval("Instrumento")     %></td>
                                <td><%# Eval("Quantidade", "{0:N0}")      %></td>
                                <td><%# Eval("Preco", "{0:c}")  %></td>
                                <td style="width: 120px; text-align:right;" class="<%# this.GetClassCustodiaOpcao_ValorPosicao( Container.DataItem ) %>"><%# Eval("ValorPosicao", "{0:c}")%></td>
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
                                <td colspan=3><%= "Total"%></td>
                                <td style="width: 120px; text-align:right;"><%= String.Format(CurrentCustodiaOpcaoTotal_Rotulo, CurrentCustodiaOpcaoTotal.ToString("N2")) %></td>
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
                <a data-toggle="collapse" href="#collapse7"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>BTC: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaOpcaoTotal_Rotulo, CurrentTotalBTC.ToString("N2"))%></span></a>
            </h4>
        </div>
        <div id="collapse7" class="panel-collapse collapse">
            <div class="panel-body-minhaconta">
                <asp:Repeater ID="rptBTC" runat="server">
                    <HeaderTemplate>
                        <table class="tabela-minhaconta">
<%--                            
                            <tr class="tabela-titulo">
                                <td colspan="9">BTC</td>   
                            </tr>
--%>
                             <tr class="tabela-titulo">
                                 <td>Ativo</td>
                                 <td>Tipo</td>
                                 <td>Taxa Final</td>
                                 <td>Nr. Contrato</td>
                                 <td>Abertura</td>
                                 <td>Vencimento</td>
                                 <td>Quantidade</td>
                                 <td>Custo</td>
                                 <td style="width: 120px; text-align:right;">Financeiro</td>
                            </tr>
                    </HeaderTemplate>
                    <itemtemplate>
                            <tr class="tabela-type-small">
                                <td><%# Eval("CodigoNegocio")               %></td>
                                <td><%# Eval("TipoContrato")                %></td>
                                <td><%# Eval("TaxaFinal")                   %></td>
                                <td><%# Eval("NumeroContrato")              %></td>
                                <td><%# Eval("Abertura", "{0:dd/MM/yyyy}")  %></td>
                                <td><%# Eval("Vencimento", "{0:dd/MM/yyyy}")%></td>
                                <td><%# Eval("Quantidade", "{0:N0}")        %></td>
                                <td><%# Eval("Custo", "{0:c}")              %></td>
                                <td style="width: 120px; text-align:right;" class="<%# this.GetClassCustodiaBTC_ValorPosicao(Container.DataItem) %>"><%# Eval("Financeiro", "{0:c}")%></td>
                            </tr>
                    </itemtemplate>
                    <FooterTemplate>
                            <tr id="trEmpty" runat="server" visible="false">
                                <td colspan="9" align = "center">
                                    Nenhum registro.
                                </td>
                            </tr>
                            <tr id="trError" runat="server" visible="false">
                                <td colspan="9" align = "center">
                                    Nenhum registro.
                                </td>
                            </tr>
                            <tr class="tabela-titulo">
                                <td colspan="8">Total</td>
                                <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentTotalBTC) %>" colspan="1"><%= String.Format(CurrentCustodiaOpcaoTotal_Rotulo, CurrentTotalBTC.ToString("N2"))%></td>
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
                <a data-toggle="collapse" href="#collapse8"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Fundos Imobiliários: <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentCustodiaFundoTotal_Rotulo, CurrentCustodiaFundoTotal.ToString("N2")) %></span></a>
            </h4>
        </div>
        <div id="collapse8" class="panel-collapse collapse">
            <div class="panel-body-minhaconta">
                <asp:Repeater ID="rptCustodiaFundo" runat="server">
                    <HeaderTemplate>
                        <table class="tabela-minhaconta">
                            <tr class="tabela-titulo">
                                <td>Fundos Imobiliários</td>
                                <td>Quantidade</td>
                                <td>Preço</td>
                                <td style="width: 120px; text-align:right;">Valor da Posição</td>
                            </tr>
                    </HeaderTemplate>
                    <itemtemplate>
                            <tr class="tabela-type-small">
                                <td><%# Eval("Instrumento")     %></td>
                                <td><%# Eval("Quantidade")      %></td>
                                <td><%# Eval("Preco", "{0:c}")  %></td>
                                <td style="width: 120px; text-align:right;" class="<%# this.GetClassCustodiaFI_ValorPosicao( Container.DataItem ) %>"><%# Eval("ValorPosicao", "{0:c}")%></td>
                            </tr>
                    </itemtemplate>
                        <FooterTemplate>
                            <tr id="trEmpty" runat="server" visible="false">
                                <td colspan="4" align = "center">
                                    Nenhum registro.
                                </td>
                            </tr>
                            <tr id="trError" runat="server" visible="false">
                                <td colspan="8" align = "center">
                                    Nenhum registro.
                                </td>
                            </tr>
                            <tr class="tabela-titulo">
                                <td colspan=3>Total</td>
                                <td style="width: 120px; text-align:right;"><%= String.Format(CurrentCustodiaFundoTotal_Rotulo, CurrentCustodiaFundoTotal.ToString("N2")) %></td>
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
                <a data-toggle="collapse" href="#collapse9"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Proventos: <span style="font-family: sans-serif; float:right;"><%= String.Format("R$ {0}", CurrentTotalProventos.ToString("N2"))%></span></a>
            </h4>
        </div>
        <div id="collapse9" class="panel-collapse collapse">
            <div class="panel-body-minhaconta">
                <asp:Repeater ID="rptProventos" runat="server">
                    <HeaderTemplate>
                        <table class="tabela-minhaconta">
                            <tr class="tabela-titulo">
                                <td colspan="4">Provento       </td>
                                <td>Ativo                       </td>
                                <td>Quantidade                  </td>
                                <td>Dt. Pagamento               </td>
                                <td style="width: 120px; text-align:right;">Valor </td>
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
                                <td style="width: 120px; text-align:right;"><%= String.Format("R$ {0}", CurrentTotalProventos.ToString("N2"))%></td>
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patrimonio_Dinheiro.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta.Patrimonio_Dinheiro" %>


<link rel="Stylesheet" type="text/css" href="<%= this.RaizDoSite %>/Resc/Skin/<%=this.SkinEmUso %>/bootstrap.css" />

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

<h4>Total em Dinheiro: <%= String.Format(CurrentTotalValor_Rotulo, CurrentTotalGeral.ToString("N2")) %></h4>

<div class="panel-group">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <%--<a data-toggle="collapse" href="#collapse1"><span class="pull-right glyphicon glyphicon-plus">Conta Corrente:</span> <%= String.Format(CurrentTotalValor_Rotulo, CurrentTotalGeral.ToString("N2")) %></a>--%>
        <a data-toggle="collapse" href="#collapse1">
            <span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Conta Corrente
            <span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentTotalValor_Rotulo, CurrentTotalGeral.ToString("N2")) %></span></a>
      </h4>
    </div>
    <div id="collapse1" class="panel-collapse collapse">
        <div class="panel-body-minhaconta">
            <table class="tabela-minhaconta" style="margin-top: 0px; margin-bottom: 0px;">
<%--                
                <tr class="tabela-titulo">
                    <td colspan="8">Conta Corrente</td>
                </tr>
--%>
                <tr class="tabela-titulo">
                    <td colspan="6">Saldo Final Ontem:</td>
                    <td style="width: 120px; text-align:right;"><%= String.Format(CurrentSaldoD0Valor_Rotulo, CurrentSaldoAbertura.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-type-small">
                    <td colspan="6"><%= CurrentMinhaConta.SaldoD0_Rotulo%></td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoD0) %>"><%= String.Format(CurrentSaldoD0Valor_Rotulo, CurrentSaldoD0.ToString("N2"))%></td>
                </tr>
                <tr class="tabela-titulo">
                    <td colspan="6">Saldo Final Hoje:</td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoFinalD0)%>"><%= String.Format(CurrentSaldoD0Valor_Rotulo, CurrentSaldoFinalD0.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-type-small">
                    <td colspan="6"><%= CurrentMinhaConta.SaldoD1_Rotulo%></td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoD1) %>"><%= String.Format(CurrentSaldoD1Valor_Rotulo, CurrentSaldoD1.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-titulo">
                    <td colspan="6">Saldo Final em D+1:</td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoFinalD1)%>"><%= String.Format(CurrentSaldoD0Valor_Rotulo, CurrentSaldoFinalD1.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-type-small">
                    <td colspan="6"><%= CurrentMinhaConta.SaldoD2_Rotulo%></td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoD2) %>"><%= String.Format(CurrentSaldoD2Valor_Rotulo, CurrentSaldoD2.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-titulo">
                    <td colspan="6">Saldo Final em D+2:</td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoFinalD2)%>"><%= String.Format(CurrentSaldoD0Valor_Rotulo, CurrentSaldoFinalD2.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-type-small">
                    <td colspan="6"><%= CurrentMinhaConta.SaldoD3_Rotulo%></td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoD3) %>"><%= String.Format(CurrentSaldoD3Valor_Rotulo, CurrentSaldoD3.ToString("N2")) %></td>
                </tr>
                <tr class="tabela-titulo">
                    <td colspan="6">Saldo Final em D+3:</td>
                    <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentSaldoFinalD3)%>"><%= String.Format(CurrentSaldoD3Valor_Rotulo, CurrentSaldoFinalD3.ToString("N2")) %></td>
                </tr>
            </table>
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
                <a data-toggle="collapse" href="#collapse2"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Financeiro das Operações Realizadas no Dia<span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentSaldoProjetadoValor_Rotulo, CurrentOperacoesDia.ToString("N2"))%></span></a>
            </h4>
        </div>
        <div id="collapse2" class="panel-collapse collapse">
            <div class="panel-body-minhaconta">
                <table class="tabela-minhaconta">
<%--                    
                    <tr class="tabela-titulo">
                        <td colspan="8">Financeiro das Operações Realizadas no Dia</td>
                    </tr>
--%>
                    <tr class="tabela-type-small">
                        <td colspan="6">(D+0) [Renda Fixa / Fundos] <%--A Liquidar em: <%= CurrentMinhaConta.SaldoD0_Rotulo%>--%></td>
                        <td style="width: 120px; text-align:right;">R$ 0,00</td>
                    </tr>
                    <tr class="tabela-type-small">
                        <td colspan="6">(D+1) [Opções / Futuro] <%--A Liquidar em: <%= CurrentMinhaConta.SaldoD1_Rotulo%>--%></td>
                        <td style="width: 120px; text-align:right;" title="Opções:<%= String.Format(CurrentSaldoD1Valor_Rotulo, (OperacoesCompraOpcao + OperacoesVendaOpcao).ToString("N2"))%> / Futuro: <%= String.Format(CurrentSaldoD1Valor_Rotulo, (PosicaoBMFLucroPrejuizo + OperacoesBMFLucroPrejuizo).ToString("N2")) %>"><%= String.Format(CurrentSaldoD2Valor_Rotulo, CurrentOperacoesD1.ToString("N2"))%></td>
                    </tr>
                        <tr class="tabela-type-small">
                        <td colspan="6">(D+3) [Ações] <%--A Liquidar em: <%= CurrentMinhaConta.SaldoD3_Rotulo%>--%></td>
                        <td style="width: 120px; text-align:right;" title="Compra: <%= String.Format(CurrentSaldoD1Valor_Rotulo, OperacoesCompraVista.ToString("N2"))%> / Venda:<%= String.Format(CurrentSaldoD1Valor_Rotulo, OperacoesVendaVista.ToString("N2"))%>"><%= String.Format(CurrentSaldoD3Valor_Rotulo, CurrentOperacoesD3.ToString("N2")) %></td>
                    </tr>
                    <tr class="tabela-titulo">
                        <td colspan="6">Total das Operações Realizadas no Dia</td>
                        <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentOperacoesDia) %>"><%= String.Format(CurrentSaldoProjetadoValor_Rotulo, CurrentOperacoesDia.ToString("N2"))%></td>
                    </tr>
                    <tr id="trNenhumSaldos" runat="server" visible="false">
                        <td colspan="8">Nenhum item encontrado.</td>
                    </tr>                              
                </table>
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
            <h4 class="panel-title" id="panel-title">
                <a data-toggle="collapse" href="#collapse3"><span style="font-size: 12px; color:gray;" class="glyphicon glyphicon-plus">&nbsp;</span>Garantias<span style="font-family: sans-serif; float:right;"><%= String.Format(CurrentGarantiaValorTotal_Rotulo, CurrentGarantiaGeral.ToString("N2"))%></span></a>
            </h4>
        </div>
        <div id="collapse3" class="panel-collapse collapse">
            <div class="panel-body-minhaconta">
                <asp:Repeater ID="rptGarantia" runat="server">
                    <HeaderTemplate>
                        <table class="tabela-minhaconta">
<%--
                            <tr class="tabela-titulo">
                                <td colspan="8">Garantias</td>   
                            </tr>
--%>
                                <tr class="tabela-titulo">
                                <td colspan="6">Descrição       </td>
                                <td style="width: 120px; text-align:right;">Valor           </td>
                            </tr>
                    </HeaderTemplate>
                    <itemtemplate>
                            <tr class="tabela-type-small">
                                <td colspan="6"><%# Eval("Descricao")       %></td>
                                <td style="width: 120px; text-align:right;"><%# Eval("Valor", "{0:c}")  %></td>
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
                                <td colspan="6">Total</td>
                                <td style="width: 120px; text-align:right;" class="<%= GetClass_Valor(CurrentGarantiaGeral) %>" colspan="1"><%= String.Format(CurrentGarantiaValorTotal_Rotulo, CurrentGarantiaGeral.ToString("N2"))%></td>
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

<style type="text/css">
    /*
    .panel-header 
    {
        padding: 10px 15px;
        background: red; /* For browsers that do not support gradients
        background: -webkit-linear-gradient(white, silver); /* For Safari 5.1 to 6.0
        background: -o-linear-gradient(white, silver); /* For Opera 11.1 to 12.0
        background: -moz-linear-gradient(white, silver); /* For Firefox 3.6 to 15
        background: linear-gradient(white, silver); /* Standard syntax
    }
    */
</style>

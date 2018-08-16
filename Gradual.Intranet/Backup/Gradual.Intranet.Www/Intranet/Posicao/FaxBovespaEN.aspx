<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaxBovespaEN.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.FaxBovespaEN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/Default/gradintra-theme/jquery-ui-1.8.1.custom.css?v=<%= this.VersaoDoSite %>" />
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/Default/ui.jqgrid.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Principal.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../Skin/Default/ValidationEngine.jquery.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../Skin/Default/Intranet/Default.aspx.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Intranet/Clientes.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="print"  href="../Skin/Default/Relatorio.print.css?v=<%= this.VersaoDoSite %>" />

    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" media="print" href="../../Skin/Default/Relatorio.Print.css" />
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Principal.css" />    
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Intranet/Default.css" />
</head>
<body>
    <form id="form1" runat="server">
         <div class="Relatorio">
            <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
                <%--<a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodosBmf_Click(this)">Imprimir Todos</a> | --%>
                <%--<a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> |--%>
                <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Fax_Bov_EN_Click_Print(this);">Imprimir PDF</a> |
                <a href="#" onclick="GradIntra_Clientes_Financeiro_FecharRelatorio_Default(this);GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this);">Fechar</a>
            </div>

            <input type="hidden" id="hddDatasPaginacao" runat="server" />
            <input type="hidden" id="hddBolsa" value="Bmf" />
             <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

            <h1 style="min-width: 586px;font-size:20px;">
                <span>Report: Fax Bovespa:</span>
                <span><img src="../../Skin/Default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></span>
            </h1>
           <br />
                <table cellpadding="0" border="0" style="font-size:0.9em">
                    <tr>
                        <td>
                            <div><label style="padding-right:17px"><b>Gradual C.C.T.V.M. S/A&nbsp; </b></label>&nbsp;</div>
                            <div><label>Av. Juscelino Kubitscheck, 50 6 andar ITAIM</label></div>
                            <div><label>Internet: www.gradualinvestimentos.com.br</label></div>
                            <div><label>Ouvidoria: tel.: 0800-655-1466</label></div>
                        </td>
                        <td>
                            <div>Fone: 11 33728300</div>
                            <div>Cep: 04543-000 São Paulo - SP</div>
                            <div>E-mail: atendimento@gradualinvestimentos.com.br</div>
                            <div>e-mail ouvidoria: ouvidoria@gradualinvestimentos.com.br</div>
                        </td>
                        <td>
                            C.N.P.J.: 33.918.160/0001-73<br />
                            Número da Corretora: 227-5
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <label>TO:</label><br />
                            <label>Company:</label><br />
                            <label>Fax:</label><br /><br />

                            <label>From:</label><br />
                            <label>Company:</label><br />
                            <label>Phone:</label><br />
                            <label>Fax:</label><br />
                            <label>Date:</label><br />
                         </td>
                        <td align="left">
                            <br />
                            <asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal><br />
                            <br /><br />
                            <asp:literal id="lblNomeAssessor" runat="server"></asp:literal><br />
                            GRADUAL CCTVM SA<br />
                            011-40071873<br />
                            011-33728301<br />
                            <asp:literal id="lblCabecalho_DataEmissao" runat="server"></asp:literal><br />
                        </td>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><label>lRef.: Trades Executions</label><br />
                        </td>
                        <td></td>
                    </tr>
                </table>
        <br />
        <div id="divRelatorioOpcao" runat="server">
        <asp:repeater id="rptLinhasDoRelatorioOpcao" runat="server" onitemdatabound="rptLinhasDoRelatorioOpcao_ItemDataBound">
            <itemtemplate>
                <label class=tdLabel"> Date of Settle: <%# DataBinder.Eval(Container.DataItem, "DataLiquidacao")%>  </label><br />
                <table cellspacing="0">

                    <thead>
                        <tr style="font-size:0.8em">
                            <td style="font-weight:bold" > <%# DataBinder.Eval(Container.DataItem, "Sentido").ToString() == "C" ? "BOUGHT" : "SOLD"%></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "TipoMercado") %></td>
                            <td align="center" colspan="2"><%# DataBinder.Eval(Container.DataItem, "NomeRes")%></td>
                            <td align="center" colspan="2"><%# DataBinder.Eval(Container.DataItem, "CodigoIsin")%></td>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td colspan="2"></td>
                            <td align="right">Quant</td>
                            <td align="right">Price</td>
                            <td align="right">Total</td>
                            <td align="right">FEE</td>
                        </tr>
                        <asp:repeater id="rptLinhasDoRelatorioDetalhesOpcao" runat="server">
                        <itemtemplate>
                            <tr style="font-size:0.9em">
                                <td class="tdLabel">                  1-BOVESPA </td>
                                <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelCodigoNegocio").ToString() %></td>
                                <td class="tdLabel ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelQuantidade")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelPreco")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelTotal")%> </td>
                                <td class="tdValor ValorNumerico" >   (<%# DataBinder.Eval(Container.DataItem, "PapelCorretagem")%>) </td>
                            </tr>                
                        </itemtemplate>
                        </asp:repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>SOMA</td>
                            <td></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaQuantidade")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaPreco")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaTotal")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaCorretagem")%></td>
                        </tr>
                        <tr>
                            <td>TOTAL</td>
                            <td></td>
                            <td></td>
                            <td class ="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "TotalPrecoNet")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "TotalNet")%></td>
                            <td></td>
                        </tr>
                    </tfoot>

                </table>
            </itemtemplate>
        </asp:repeater>                
        <br />
        <table class="RelatorioNotasResumo" cellspacing="0">
        
            <thead>
            </thead>
            
            <tfoot>
            </tfoot>
        
            <tbody>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Bought (Opt)</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalComprasOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total Sold (Opt)</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalVendasOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Others</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalTermoOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total daily settlement</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalAjusteDiarioOpcao" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total of trades</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalNegociosOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">FEE</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalCorretagemOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Exchange Fees CBLC</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxasCBLCOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Exchange Fees Bovespa</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaBovespaOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Exchange Fees</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxasOperacionaisOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Other rates</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_OutrasDespesasOpcao" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel"> Day-trade income tax</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRDayTradeOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Income Tax</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_AjustePosicaoOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total NET</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalLiquidoOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Debit</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_DataLiquidacaoOpcao" runat="server"></asp:literal></td>
                    
                </tr>
            </tbody>
        
        </table>
        </div>
        <div id="divRelatorioVista" runat="server">
        <asp:repeater id="rptLinhasDoRelatorioVista" runat="server" onitemdatabound="rptLinhasDoRelatorioVista_ItemDataBound">
            <itemtemplate>
                <label class=tdLabel"> Date of Trade: <%# DataBinder.Eval(Container.DataItem, "DataLiquidacao")%>  </label><br />
                <table cellspacing="0">

                    <thead>
                        <tr style="font-size:0.8em">
                            <td style="font-weight:bold" > <%# DataBinder.Eval(Container.DataItem, "Sentido").ToString() == "C" ? "COMPRA" : "VENDA"%></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "TipoMercado") %></td>
                            <td align="center" colspan="2"><%# DataBinder.Eval(Container.DataItem, "NomeRes")%></td>
                            <td align="center" colspan="2"><%# DataBinder.Eval(Container.DataItem, "CodigoIsin")%></td>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td colspan="2"></td>
                            <td align="right">Quant</td>
                            <td align="right">Price</td>
                            <td align="right">Total</td>
                            <td align="right">Fee</td>
                        </tr>
                        <asp:repeater id="rptLinhasDoRelatorioDetalhesVista" runat="server">
                        <itemtemplate>
                            <tr style="font-size:0.9em">
                                <td class="tdLabel">                  1-BOVESPA </td>
                                <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelCodigoNegocio").ToString() %></td>
                                <td class="tdLabel ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelQuantidade")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelPreco")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelTotal")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelCorretagem")%> </td>
                            </tr>                
                        </itemtemplate>
                        </asp:repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>SOMA</td>
                            <td></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaQuantidade")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaPreco")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaTotal")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaCorretagem")%></td>
                        </tr>
                        <tr>
                            <td>TOTAL</td>
                            <td></td>
                            <td></td>
                            <td class ="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "TotalPrecoNet")%></td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "TotalNet")%></td>
                            <td></td>
                        </tr>
                    </tfoot>

                </table>
            </itemtemplate>
        </asp:repeater>

        <table class="RelatorioNotasResumo" cellspacing="0">
        
            <thead>
            </thead>
            
            <tfoot>
            </tfoot>
        
            <tbody>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Bought</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalComprasVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total Sold</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalVendasVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Others</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalTermoVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total Daily Settlement</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalAjusteDiarioVista" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total of trades</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalNegociosVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Fee</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalCorretagemVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Exchange fees CBLC</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxasCBLCVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Exchanges fees Bovespa</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaBovespaVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Taxa Exchanges Fees</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxasOperacionaisVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Other rates</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_OutrasDespesasVista" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Day-trade income Tax</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRDayTradeVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Income Tax</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_AjustePosicaoVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Net</td>
                    <td class="tdValor ValorNumerico BordaDireita"> <asp:literal id="lblRodape_DataLiquidacaoVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Debit</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblRodape_TotalLiquidoVista" runat="server"></asp:literal></td>
                    
                </tr>
            </tbody>
        
        </table>
        </div>
        </div>
    </div>

    </form>
</body>
</html>

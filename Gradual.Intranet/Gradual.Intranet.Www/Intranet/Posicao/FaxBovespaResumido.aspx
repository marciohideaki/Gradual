<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaxBovespaResumido.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.FaxBovespaResumido" %>

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
                <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Fax_Bov_Resumido_Click_Print(this);">Imprimir PDF</a> |
                <a href="#" onclick="GradIntra_Clientes_Financeiro_FecharRelatorio_Default(this);GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this);">Fechar</a>
            </div>

            <input type="hidden" id="hddDatasPaginacao" runat="server" />
            <input type="hidden" id="hddBolsa" value="Bmf" />
             <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

            <h1 style="min-width: 586px;font-size:20px;">
                <span>Relatório: Fax Bovespa Resumido</span>
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
                            <label>Para:</label><br />
                            <label>Empresa:</label><br />
                            <label>Fax:</label><br /><br />

                            <label>De:</label><br />
                            <label>Empresa:</label><br />
                            <label>Telefone:</label><br />
                            <label>Fax:</label><br />
                            <label>Data:</label><br />
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
                        <td colspan="2"><label>lRef.: Confirmação de Operações</label><br />
                        </td>
                        <td></td>
                    </tr>
                </table>
            <br />
            <div runat="server" id="divRelatorioVista">
            <label class=tdLabel"> Data de Liquidação:  <asp:Literal ID="lblDataLiquidacaoVista" runat="server"></asp:Literal>   </label><br />
            <asp:repeater id="rptLinhasDoRelatorioVista" runat="server" OnItemDataBound="rptLinhasDoRelatorioVista_ItemDataBound">
                <itemtemplate>
                    <table cellspacing="0" >
                        <thead>
                            <tr style="font-size:0.8em">
                                <td >BOLSA</td>
                                <td >C/V</td>
                                <td >Mercado</td>
                                <td >Titulo</td>
                                <td align="right">Quantidade</td>
                                <td align="right">Preço</td>
                                <td align="right">Volume</td>
                            </tr>
                        </thead>

                        <tbody>
                            <asp:repeater id="rptLinhasDoRelatorioVistaDetalhes" runat="server">
                            <itemtemplate>
                                <tr style="font-size:0.9em">
                                    <td class="tdLabel">                  1-BOVESPA </td>
                                    <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelSentido").ToString() %></td>
                                    <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelTipoMercado").ToString() %></td>
                                    <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelNomeRes").ToString()%> <%# DataBinder.Eval(Container.DataItem, "PapelCodigoNegocio").ToString() %></td>
                                    <td class="tdLabel ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelQuantidade")%> </td>
                                    <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelPreco")%> </td>
                                    <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelVolume")%> </td>
                                </tr>        
       
                            </itemtemplate>
                            </asp:repeater>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td class="tdLabel" colspan="4" align="right" >Total</td>
                                <td class="tdLabel ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "SomaQuantidade")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "SomaPreco")%> </td>
                                <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "SomaTotal")%> </td>
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
                    <td class="tdLabel">Compras</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalComprasVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Vendas </td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalVendasVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Termo</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalTermoVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa de Liquidação</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaLiquidacaoVista" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Emolumento Bolsa</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_EmolumentoBolsa" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Corretagem</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalCorretagemVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Emolumento Total</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_EmolumentoTotalVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa  de Registro Bolsa</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaRegistroBolsaVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Taxa Registro</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxaRegistroVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Tatxa Registro Total</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaRegitroTotalVista" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Base Day trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_BaseDayTradeVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Base s/ Operações</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_BaseSemOperacoesVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Imposto Calc. s/ Day Trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ImpostoSemDayTradeVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Imposto Calc. s/ Oper.</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_ImpostoSemOperacoesVista" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Líquido</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalLiquidoVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Crédito</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_DataLiquidacaoVista" runat="server"></asp:literal></td>
                    
                </tr>
            </tbody>
        </table>
        </div>

        <br />
        <div id="divRelatorioOpcao" runat="server">
            <label class=tdLabel"> Data de Liquidação: <asp:Literal ID="lblDataLiquidacaoOpcao" runat="server"></asp:Literal>  </label><br />
            <asp:repeater id="rptLinhasDoRelatorioOpcao" runat="server" OnItemDataBound="rptLinhasDoRelatorioOpcao_ItemDataBound">
                    <itemtemplate>
            <table cellspacing="0" >
                <thead>
                    <tr style="font-size:0.8em">
                        <td >BOLSA</td>
                        <td >C/V</td>
                        <td >Mercado</td>
                        <td >Titulo</td>
                        <td align="right">Quantidade</td>
                        <td align="right">Preço</td>
                        <td align="right">Volume</td>
                    </tr>
                </thead>

                <tbody>
                    <asp:repeater id="rptLinhasDoRelatorioOpcaoDetalhes" runat="server">
                    <itemtemplate>
                        <tr style="font-size:0.9em">
                            <td class="tdLabel">                  1-BOVESPA </td>
                            <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelSentido").ToString() %></td>
                            <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelTipoMercado").ToString() %></td>
                            <td class="tdLabel">                  <%# DataBinder.Eval(Container.DataItem, "PapelNomeRes").ToString()%> <%# DataBinder.Eval(Container.DataItem, "PapelCodigoNegocio").ToString() %></td>
                            <td class="tdLabel ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelQuantidade")%> </td>
                            <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "PapelPreco")%> </td>
                            <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem,"PapelVolume")%> </td>
                        </tr>                
                    </itemtemplate>
                    </asp:repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td class="tdLabel" colspan="4" align="right" >Total</td>
                        <td class="tdLabel ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "SomaQuantidade")%> </td>
                        <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "SomaPreco")%> </td>
                        <td class="tdValor ValorNumerico" >   <%# DataBinder.Eval(Container.DataItem, "SomaTotal")%> </td>
                    </tr>
                </tfoot>
            </table>
            </itemtemplate>
          </asp:repeater>
        <br />
        
        <table class="RelatorioNotasResumo" cellspacing="0" >
            <thead>
            </thead>
            <tfoot>
            </tfoot>
            <tbody>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Compras</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalComprasOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Vendas </td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalVendasOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Termo</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalTermoOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa de Liquidação</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaLiquidacaoOpcao" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Emolumento Bolsa</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_EmolumentoBolsaOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Corretagem</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalCorretagemOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Emolumento Total</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_EmolumentoTotalOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa  de Registro Bolsa</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaRegistroBolsaOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Taxa Registro</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxaRegistroOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Tatxa Registro Total</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaRegitroTotalOpcao" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Base Day trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_BaseDayTradeOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Base s/ Operações</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_BaseSemOperacoesOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Imposto Calc. s/ Day Trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ImpostoSemDayTradeOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Imposto Calc. s/ Oper.</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_ImpostoSemOperacoesOpcao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Líquido</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalLiquidoOpcao" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel"></td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_DataLiquidacaoOpcao" runat="server"></asp:literal></td>
                    
                </tr>
            </tbody>
        </table>
        </div>
        </div>
    </div>
    </form>
</body>
</html>


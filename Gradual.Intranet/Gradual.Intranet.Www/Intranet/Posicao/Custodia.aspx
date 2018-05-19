<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Custodia.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.Custodia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Relatório - Custódia</title>

    <link rel="Stylesheet" media="all"   href="../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" media="print" href="../Skin/Default/Relatorio.Print.css" />
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Principal.css" />

</head>
<body>
<form id="form1" runat="server">
    
    <div class="Relatorio">
    
        <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> | 
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this)">Fechar</a>
        </div>
        
        <br />
                
        <div class="RelatorioImpressao">
            <h1><span>Relatório: Custódia</span><img src="../Skin/default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></h1>
            <h2 class="InformacoesDoCliente">
                <label>Cliente:</label>
                <span>
                    <asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal>
                </span>
            </h2>
        </div>
            
        <table cellspacing="0" class="RelatorioCustodia">
            <thead>
                <tr style="font-size:0.8em">
                    <td>Mercado</td>
                    <td>Código</td>
                    <td>Empresa</td>
                    <td>Carteira</td>
                    <td style="text-align:right">Qtd. Abertura</td>
                    <td style="text-align:right">Qtd. Compra</td>
                    <td style="text-align:right">Qtd.Venda</td>
                    <td style="text-align:right">Qtd. Atual</td>
                  
                    <td style="text-align:right">Projetado D1</td>
                    <td style="text-align:right">Projetado D2</td>
                    <td style="text-align:right">Projetado D3</td>

                    <td style="text-align:right">F. Anterior</td>
                    <td style="text-align:right"">Cotação (R$)</td>
                    <td style="text-align:right">Variação (%)</td>
                    <td style="text-align:right">Valor (R$)</td>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td class="tdLabel" colspan="14">Totais:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblTotal" runat="server"></asp:literal></td>   
                </tr>
            </tfoot>
            <tbody>
                <asp:literal id="lblRelatorioVazio" runat="server" visible="false" text="<tr><td colspan='12' class='RelatorioVazio'>Nenhuma custódia encontrada.</td></tr>"></asp:literal>
                <asp:repeater id="rptLinhasDoRelatorio" runat="server">
                    <itemtemplate>
                        <tr style="font-size:0.9em">
                            <td Propriedade="TipoMercado">                     <%# DataBinder.Eval(Container.DataItem, "TipoMercado")       %></td>
                            <td Propriedade="CodigoNegocio">                   <%# DataBinder.Eval(Container.DataItem, "CodigoNegocio")     %></td>
                            <td Propriedade="Empresa">                         <%# DataBinder.Eval(Container.DataItem, "Empresa")           %></td>
                            <td Propriedade="Carteira">                        <%# DataBinder.Eval(Container.DataItem, "Carteira")          %></td>
                                                                                                                                           
                            <td Propriedade="QtdAbertura"       align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdAbertura")       %></td>
                            <td Propriedade="QtdCompra"         align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdCompra")         %></td>
                            <td Propriedade="QtdVenda"          align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdVenda")          %></td>
                            <td Propriedade="QtdAtual"          align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdAtual")          %></td>
                                                                
                            <td Propriedade="QtdD1"             align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdD1")             %></td>
                            <td Propriedade="QtdD2"             align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdD2")             %></td>
                            <td Propriedade="QtdD3"             align="right"> <%# DataBinder.Eval(Container.DataItem, "QtdD3")             %></td>

                            <td Propriedade="ValorDeFechamento" align="right"> <%# DataBinder.Eval(Container.DataItem, "ValorDeFechamento") %></td>
                            <td Propriedade="Cotacao"           align="right"> <%# DataBinder.Eval(Container.DataItem, "Cotacao")           %></td>
                            <td Propriedade="Variacao"          align="right"> <%# DataBinder.Eval(Container.DataItem, "Variacao")          %></td>
                            <td Propriedade="Resultado"         align="right"> <%# DataBinder.Eval(Container.DataItem, "Resultado")         %></td> 
                        </tr>
                    </itemtemplate>
                </asp:repeater>

                <tr class="SubTotal SubTotalMercadoAVista">
                    <td class="tdLabel" colspan="14" style="border-top:1px solid #EFEFEF;padding-top:2em !important">Subtotal (Mercado à Vista):</td>
                    <td class="tdValor ValorNumerico" style="border-top:1px solid #EFEFEF;padding-top:2em !important"><asp:literal id="lblSubTotalMercadoAVista" runat="server"></asp:literal></td>                    
                </tr>

                <tr class="SubTotal SubTotalMercadoFuturo">
                    <td class="tdLabel" colspan="14">Subtotal (Mercado Futuro):</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblSubTotalMercadoFuturo" runat="server"></asp:literal></td>                    
                </tr>
                
                <tr class="SubTotal SubTotalMercadoDeOpcoes">
                    <td class="tdLabel" colspan="14">Subtotal (Mercado de Opções):</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblSubTotalMercadoDeOpcoes" runat="server"></asp:literal></td>                    
                </tr>

                <tr class="SubTotal SubTotalMercadoDeTermo">
                    <td class="tdLabel" colspan="14">Subtotal (Mercado de Termo):</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblSubTotalMercadoTermo" runat="server"></asp:literal></td>                    
                </tr>
            </tbody>
        </table>
        
    </div>
    
    </form>
</body>
</html>

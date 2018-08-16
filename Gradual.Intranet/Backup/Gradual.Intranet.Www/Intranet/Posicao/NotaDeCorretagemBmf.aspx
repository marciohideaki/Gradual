<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotaDeCorretagemBmf.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.NotaDeCorretagemBmf" %>

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
    <form id="form1" runat="server">
    <span id="spanPaginacao" style="margin-left: 25%;" >
            <a href="#" id="btnPaginacaoAnterior">&#60; Anterior</a>
            <span id="SpanLinksPaginacao"></span>
            <a href="#" id="btnPaginacaoProximo">Próxima &#62;</a>
        </span>
    <div class="Relatorio">
        
        <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodosBmf_Click(this)">Imprimir Todos</a> | 
            <%--<a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> |--%>
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Click_Print(this);">Imprimir PDF</a> |
            <a href="#" onclick="GradIntra_Clientes_Financeiro_FecharRelatorio_Default(this);GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this);">Fechar</a>
        </div>

        <input type="hidden" id="hddDatasPaginacao" runat="server" />
        <input type="hidden" id="hddBolsa" value="Bmf" />
        
        <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

            <h1 style="min-width: 586px;font-size:20px;">
                <span>Relatório: Nota de Corretagem Bm&F</span>
                <span><asp:Label runat="server" ID="lblCabecalho_Provisorio" Text="(Provisória)" Font-Bold="true" Visible="false"></asp:Label></span>
                <span class="Direita"><img src="../../Skin/Default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></span>
            </h1>
        
           <br />
                <table cellpadding="0" style="font-size:0.9em">
                    <thead></thead>
                    <tfoot></tfoot>
                    <tr>
                        <td>
                            <label><b>Gradual C.C.T.V.M. S/A&nbsp; </b></label>&nbsp;<br />
                            <label>Av. Juscelino Kubitscheck, 50 6 andar ITAIM</label><br />
                            <label>Internet: www.gradualinvestimentos.com.br</label><br />
                            <label>Ouvidoria: tel.: 0800-655-1466</label>
                        </td>
                        <td>
                            Fone: 11 33728300<br />
                            Cep: 04543-000 São Paulo - SP<br />
                            E-mail: atendimento@gradualinvestimentos.com.br<br />
                            e-mail ouvidoria: ouvidoria@gradualinvestimentos.com.br<br />
                        </td>
                        <td>
                            C.N.P.J.: 33.918.160/0001-73<br />
                            Número da Corretora: 227-5
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <label>Data:</label><span><asp:literal id="lblCabecalho_DataEmissao" runat="server"></asp:literal></span><br />
                            <label>Cliente:</label><span><asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal></span><br />
                            <label>Número da Nota:</label><span><asp:literal id="lblNumeroDaNota" runat="server"></asp:literal> </span><br />
                        </td>
                        <td>
                            <label>C.N.P.J./C.P.F.:</label><span><asp:literal id="lblCpfCnpjCliente" runat="server"></asp:literal> </span><br />
                            <label>Código do cliente:</label><span><asp:literal id="lblCodigoDoCliente" runat="server"></asp:literal> </span>
                        </td>
                    </tr>
                </table>
        <br />
        <table cellspacing="0" >

            <thead>
                <tr style="font-size:0.8em">
                    
                    <td>C/V</td>
                    <td>Mercadoria</td>
                    <td align="center">Vencimento</td>
                    <td>Quantidade</td>
                    <td align="center">Preço/Ajuste</td>
                    <td style="text-align:right">Tipo de negócio</td>
                    <td style="text-align:right">Vlr. de Operação / Ajuste</td>
                    <td>D/C</td>
                    <td style="text-align:right">Taxa Operacional</td>
                </tr>
            </thead>

            <tbody>
                <asp:repeater id="rptLinhasDoRelatorio" runat="server">
                <itemtemplate>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">                                <%# DataBinder.Eval(Container.DataItem, "Sentido")%> </td>
                    <td class="tdLabel">                                <%# DataBinder.Eval(Container.DataItem, "Mercadoria").ToString()+ " " + DataBinder.Eval(Container.DataItem, "Mercadoria_Serie").ToString()%>  </td>
                    <td class="tdLabel" align="center">                 <%# DataBinder.Eval(Container.DataItem, "Vencimento")%> </td>
                    <td class="tdLabel">                                <%# DataBinder.Eval(Container.DataItem, "Quantidade")%> </td>
                    <td class="tdValor ValorNumerico" align="center">   <%# DataBinder.Eval(Container.DataItem, "PrecoAjuste")%> </td>
                    <td class="tdLabel ">                               <%# DataBinder.Eval(Container.DataItem, "TipoNegocio")%> </td>
                    <td class="tdValor ValorNumerico">                  <%# DataBinder.Eval(Container.DataItem, "ValorOperacao")%> </td>
                    <td class="tdLabel">                                <%# DataBinder.Eval(Container.DataItem, "DC")%>   </td>
                    <td class="tdValor ValorNumerico">                  <%# DataBinder.Eval(Container.DataItem, "TaxaOperacional")%> </td>
                </tr>                
                </itemtemplate>
                </asp:repeater>                
            </tbody>

        </table>
        <br />
        <table class="RelatorioNotasResumo" cellspacing="0">
        
            <thead>
            </thead>
            
            <tfoot>
            </tfoot>
        
            <tbody>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Venda Disponível</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_VendaDisponivel" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Compra Dispoível</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_CompraDisponivel" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Venda Opções</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_VendaOpcoes" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Compra Opções</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_CompraOpcoes" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor dos Negócios</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ValorNegocio" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">IRRF</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_IRRF" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Day Trade(Projeção)</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRRFDayTrade" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa Operacional</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxaOperacional" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Taxa Registro Bmf</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TaxaRegistroBmf" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxas Bmf</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TaxasBmf" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">ISS</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ISS" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Ajuste de Posição</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_AjustePosicao" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Ajuste Day Trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_AjusteDayTrade" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total de Despesas</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalDespesas" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Corretagem</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRRFCorretagem" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total Conta Normal</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalContaNormal" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Líquido</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_TotalLiquido" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total Líquido da Nota</td>
                    <td class="tdValor ValorNumerico"> <asp:literal id="lblRodape_TotalLiquidoNota" runat="server"></asp:literal></td>
                    
                </tr>
                <tr style="font-size:0.7em">
                    <td class="tdLabel" colspan="2">+ Custos BM&F, conforme Ofício Circular BM&F 09/2007-DG<br />
                     - Exercício de opções = EXO<br />
                     - OZ1 = 249,75 grs / OZ2 = 9,90 grs. / OZ3 = 0,225 grs.<br />
                     @ Corretora ou Pessoa Vinculada atuaou na Contra parte.<br />
                     *Negocíos gerados automaticamente pelo sistema.<br />
                     **Valores pagos conforme previsão do Contrato de Transfência de <br />
                     negócios realizados na BMF (Repasse/Brokerage), celebrado entre <br />
                     as Corretoras itermediadoras e a Corretora.<br />
                     ***Taxa referente à liquidação das operações intermediadas por Terceiros<br />
                     e as operações feitas integralmente pela Corretora.
                    </td>
                    <td class="tdLabel" colspan="2">
                    <br /><br /><br /><br /><br />
                    
                    </td>
                </tr>
                <tr style="font-size:0.7em">
                    <td colspan="4">
                    ______________________________________________<br />
                                Gradual C.C.T.V.M. S/A
                    </td>
                </tr>
            </tbody>
        
        </table>
        </div>
    </div>
        
    </form>
</body>
</html>


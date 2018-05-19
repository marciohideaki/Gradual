<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotaDeCorretagem.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.NotaDeCorretagem" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Principal.css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="Relatorio">
    
        <span id="spanPaginacao" style="margin-left: 25%;">
            <a href="#" id="btnPaginacaoAnterior">&#60; Anterior</a>
            <span id="SpanLinksPaginacao"></span>
            <a href="#" id="btnPaginacaoProximo">Próxima &#62;</a>
        </span>
    
        <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodos_Click(this)">Imprimir Todos</a> | 
            <%--<a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> |--%>
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Click_Bovespa_Print(this);">Imprimir PDF</a> |
            <a href="#" onclick="GradIntra_Clientes_Financeiro_FecharRelatorio_Default(this);GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this)">Fechar</a>
        </div>
        <input type="hidden" id="hddDatasPaginacao" runat="server" />
        <input type="hidden" id="hddBolsa" value="Bovespa" />
        <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

            <h1 style="min-width: 586px;font-size:20px;">
                <span>Relatório: Nota de Corretagem</span>
                <span><asp:Label runat="server" ID="lblCabecalho_Provisorio" Text="(Provisória)" Font-Bold="true" Visible="false"></asp:Label></span>
                <span class="Direita"><img src="../../Skin/default/Img/HB01_Relatorio_Titulo_FundoGradual.png" width="112" height="40" /></span>
            </h1>
        
            <%--<h2 class="InformacoesDoCliente">
                <div><label style="padding-right:17px;">Data:</label><span><asp:literal id="lblCabecalho_DataEmissao" runat="server"></asp:literal></span></div>

                <div>
                    <label>Número da nota: </label><span><asp:literal id="lblCabecalho_Cliente_NumeroDaNota" runat="server"></asp:literal></span>
                </div>
        
                <div><label>Cliente:</label><span><asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal></span></div>
        
                <div><label>Número da Nota:</label><span><asp:literal id="lblNumeroDaNota" runat="server"></asp:literal> </span></div>
        
                <div>
                    <label>Conta:</label><span>
                    <asp:literal id="lblCabecalho_Cliente_NumeroDaConta" runat="server"></asp:literal></span>

                    <label>Agência:</label><span>
                    <asp:literal id="lblCabecalho_Cliente_NumeroDaAgencia" runat="server"></asp:literal></span>

                    <label>Banco:</label>
                    <span><asp:literal id="lblCabecalho_Cliente_NumeroDoBanco" runat="server"></asp:literal></span>
                </div>
            </h2>--%>

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
                            <label>Cliente:</label><span><asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server" /></span><br />
                            <label>Número da Nota:</label><span><asp:literal id="lblCabecalho_Cliente_NumeroDaNota" runat="server"></asp:literal> </span><br />
                        </td>
                        <td>
                            <label>C.N.P.J./C.P.F.:</label><span><asp:literal id="lblCpfCnpjCliente" runat="server"></asp:literal> </span><br />
                            <label>Código do cliente:</label><span><asp:literal id="lblCodigoDoCliente" runat="server"></asp:literal> </span>
                        </td>
                    </tr>
                </table>
        <br />
        </div>

        <table cellspacing="0">

            <thead>
                <tr style="font-size:0.8em">
                    <td>Praça</td>
                    <td>C/V</td>
                    <td align="center">Tipo de Mercado</td>
                    <td>Espec. do Título</td>
                    <td align="center">Obs.</td>
                    <td style="text-align:right">Quantidade</td>
                    <td style="text-align:right">Preço</td>
                    <td style="text-align:right">Valor (R$)</td>
                    <td>D/C</td>
                </tr>
            </thead>

            <tbody>
                <asp:repeater id="rptLinhasDoRelatorio" runat="server">
                <itemtemplate>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "NomeBolsa")          %> </td>
                    <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "TipoOperacao")       %> </td>
                    <td class="tdLabel" align="center"> <%# DataBinder.Eval(Container.DataItem, "TipoMercado")        %> </td>
                    <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "EspecificacaoTitulo")%> </td>
                    <td class="tdLabel" align="center"> <%# DataBinder.Eval(Container.DataItem, "Observacao")         %> </td>
                    <td class="tdLabel ValorNumerico">  <%# DataBinder.Eval(Container.DataItem, "Quantidade")         %> </td>
                    <td class="tdValor ValorNumerico    <%# DataBinder.Eval(Container.DataItem, "ValorNegocioPosNeg") %>"><%# DataBinder.Eval(Container.DataItem, "ValorNegocio")%> </td>
                    <td class="tdValor ValorNumerico    <%# DataBinder.Eval(Container.DataItem, "ValorTotalPosNeg")   %>"><%# DataBinder.Eval(Container.DataItem, "ValorTotal")  %> </td>
                    <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "DC")                 %> </td>
                </tr>                
                </itemtemplate>
                </asp:repeater>                
            </tbody>

        </table>

        <table class="RelatorioNotasResumo" cellspacing="0">
        
            <thead>
                <tr style="font-size:0.89em">
                    <td colspan="2" style="width:50%">Resumo dos Negócios</td>
                    <td colspan="3" style="width:50%">Resumo Financeiro</td>
                </tr>
            </thead>
            
            <tfoot>
                <tr style="font-size:0.9em">
                    <td colspan="2" style="padding-top:3em">Especificações Diversas</td>
                    <td colspan="3" style="padding-top:3em">Observações:</td>
                </tr>
                <tr style="font-size:0.9em">
                    <td colspan="2" style="font-weight:normal; text-align:center; font-size:0.8em;border:none">A coluna Q indica liquidação no Agente do Qualificado</td>
                    <td colspan="3" style="font-weight:normal; text-align:left; font-size:0.8em;border:none">
                        <span  style="font-size: 0.9em; padding-left: 9px;">As operações a futuro não são computadas no líquido da fatura.</span>
                        <table style="font-size: 0.9em; text-align: left;">
                            <tr>
                                <td>2 - Corretora ou pessoa vinculada atuou na contra parte.</td>
                                <td># - Negócio direto</td>
                                <td>8 - Liquidação Institucional</td>
                            </tr>
                            <tr>
                                <td>D - Day Trade</td>
                                <td>F - Cobertura</td>
                                <td>B - Debêntures</td>
                            </tr>
                            <tr>
                                <td>A - Posição Futuro</td>
                                <td>C - Clubes e Fundos de Ações</td>
                                <td>P - Carteira Própria</td>
                            </tr>
                            <tr>
                                <td>H - Home Broker</td>
                                <td>X - Box</td>
                                <td>Y - Despachante de Box</td>
                            </tr>
                            <tr>
                                <td>L - Precatório</td>
                                <td>T - Liquidação pelo Bruto</td>
                                <td>I - POP</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tfoot>
        
            <tbody>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Debêntures</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_Debentures" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Valor Líquido das Operações(1)</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_ValorLiquidoDasOperacoesPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_ValorLiquidoDasOperacoes" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_ValorLiquidoDasOperacoes_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Vendas à Vista</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_VendasAVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa de Registro(3)</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_TaxaDeRegistroPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_TaxaDeRegistro" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_TaxaDeRegistro_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Compras à Vista</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ComprasAVista" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa de Liquidação(2)</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_TaxaDeLiquidacaoPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_TaxaDeLiquidacao" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_TaxaDeLiquidacao_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Opções - Compras</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_OpcoesCompras" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total (1+2+3) A</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_Total_123_APosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_Total_123_A" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_Total_123_A_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Opções - Vendas</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_OpcoesVendas" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa de Termo/Opções/Futuro</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_TaxasTermo_Opcoes_FuturoPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_TaxasTermo_Opcoes_Futuro" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_TaxasTermo_Opcoes_Futuro_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Operações a Termo</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_OperacoesTermo" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Taxa A.N.A</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_TaxaANAPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_TaxaANA" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_TaxaANA_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Operações a Futuro</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_OperacoesFuturo" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Emolumentos</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_EmolumentosPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_Emolumentos" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_Emolumentos_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor das Oper. com Tit. Publ.</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ValorOperacoesTitPub" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Total Bolsa B</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_Total_Bolsa_BPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_Total_Bolsa_B" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_Total_Bolsa_B_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor das Operações</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_ValorDasOperacoes" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Corretagem</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_CorretagemPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_Corretagem" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_Corretagem_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor do Ajuste p/Futuro</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_AjusteFuturo" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">ISS</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_ISSPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_ISS" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_ISS_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IR Sobre Corretagem</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRSobreCorretagem" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">I.R.R.F. s/ operações, base <asp:Literal runat="server" ID="lblRodape_IRRF_BaseOperacoes"></asp:Literal></td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_IRRF_SobreOperacoesPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_IRRF_SobreOperacoes" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_IRRF_SobreOperacoes_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Sobre Day Trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRRF_SobreDayTrade" runat="server"></asp:literal></td>
                    
                    <td class="tdLabel">Outras</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_OutrasPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_Outras" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_Outras_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Sobre Day Trade Base</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRRF_SobreOperacoesBase" runat="server"></asp:literal></td>
                
                    <td class="tdLabel">Líquido (A+B) para <asp:Literal runat="server" ID="lblRodape_Liquido_Para"></asp:Literal></td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_Liquido_ABPosNeg" runat="server"></asp:literal>"><asp:literal id="lblRodape_Liquido_AB" runat="server"></asp:literal></td>
                    <td class="tdLabel"><asp:literal id="lblRodape_Liquido_AB_DC" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Sobre Day Trade Projeção</td>
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="lblRodape_IRRF_SobreOperacoesProjecao" runat="server"></asp:literal></td>

                    <td colspan="3"></td>
                </tr>
 
            </tbody>
        
        </table>

    </div>
        
    </form>
</body>
</html>

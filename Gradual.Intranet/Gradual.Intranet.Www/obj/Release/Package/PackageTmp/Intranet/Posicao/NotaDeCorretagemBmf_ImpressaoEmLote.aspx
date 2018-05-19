<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotaDeCorretagemBmf_ImpressaoEmLote.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.NotaDeCorretagemImpressaoEmLoteBmf" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
  
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/Default/gradintra-theme/jquery-ui-1.8.1.custom.css?v=<%= this.VersaoDoSite %>" />
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/Default/ui.jqgrid.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Principal.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../Skin/Default/ValidationEngine.jquery.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../Skin/Default/Intranet/Default.aspx.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Intranet/Clientes.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="print"  href="../Skin/Default/Relatorio.print.css?v=<%= this.VersaoDoSite %>" />
                                                          
    <link rel="Stylesheet" type="text/css" media="all"    href="../../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" type="text/css" media="print"  href="../../Skin/Default/Relatorio.Print.css" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../../Skin/Default/Principal.css" />

</head>
<body>
    <form id="form1" runat="server">

    <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
        <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this)">Fechar</a>
    </div>
    
    <asp:Repeater runat="server" ID="rptNotaDeCorretagemImpressaoPorLote" OnItemDataBound="rptNotaDeCorretagemImpressaoPorLote_ItemDataBound">
        <ItemTemplate>
        <div class="Relatorio" style="background-color: #FFF;">

            <div style="padding-left:20px;" class="RelatorioImpressao">

                <h1 style="min-width: 586px;font-size:20px;">
                    <span>Relatório: Nota de Corretagem</span>
                    <span><asp:Label runat="server" ID="lblCabecalho_Provisorio" Text="(Provisória)" Font-Bold="true" Visible="false"></asp:Label></span>
                    <span class="Direita"><img src="../../Skin/default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></span>
                </h1>
        
                <%--<h2 class="InformacoesDoCliente">
                    <div><label style="padding-right:17px;">Data:</label><span><%# Eval("DataPregao") %></span></div>

                    <div>
                        <label>Número da nota: </label><span><%# Eval("NrNota") %></span>
                    </div>
        
                    <div><label>Cliente:</label><span><%# Eval("CodCliente") + " - " + Eval("NomeCliente") %></span></div>
        
                    <div>
                        <label>Conta:</label><span><%# Eval("ContaCorrente")%></span>

                        <label>Agência:</label><span><%# Eval("Agencia")%></span>

                        <label>Banco:</label><span><%# Eval("NrBanco")%></span>
                    </div>
                </h2>--%>
                <br />
                 <table cellpadding="0" border="0" style="font-size:0.9em">
                    <tr>
                        <td>
                            <div><lable style="padding-right:17px"><b>Gradual C.C.T.V.M. S/A&nbsp; </b></lable>&nbsp;</div>
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
                        <td colspan="2">
                            <div><label style="padding-right:17px;">Data:</label><span><%# Eval("DataPregao") %></span></div>
        
                            <div><label>Cliente:</label><span><%# Eval("CodCliente") + " - " + Eval("NomeCliente") %></span></div>
        
                            <div><label>Número da Nota:</label><span><%# Eval("NrNota") %> </span></div>
                        </td>
                        <td>
                            <label>C.N.P.J./C.P.F.:</label><span><%# Eval("CpfCnpj")%> </span><br />
                            <label>Código do cliente:</label><span><%# Eval("CodCliente") %> </span>
                        </td>
                    </tr>
                </table>

            <table cellspacing="0">

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
                   <%-- <tr style="font-size:0.8em">
                        <td>Praça</td>
                        <td>C/V</td>
                        <td align="center">Tipo de Mercado</td>
                        <td>Espec. do Título</td>
                        <td align="center">Obs.</td>
                        <td style="text-align:right">Quantidade</td>
                        <td style="text-align:right">Preço</td>
                        <td style="text-align:right">Valor (R$)</td>
                        <td>D/C</td>
                    </tr>--%>
                </thead>

                <tbody>
                    <asp:repeater id="rptDetalhesDaNota" runat="server">
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
                    <%--<tr style="font-size:0.9em">
                        <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "NomeBolsa")          %> </td>
                        <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "TipoOperacao")       %> </td>
                        <td class="tdLabel" align="center"> <%# DataBinder.Eval(Container.DataItem, "TipoMercado")        %> </td>
                        <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "EspecificacaoTitulo")%> </td>
                        <td class="tdLabel" align="center"> <%# DataBinder.Eval(Container.DataItem, "Observacao")         %> </td>
                        <td class="tdLabel ValorNumerico">  <%# DataBinder.Eval(Container.DataItem, "Quantidade")         %> </td>
                        <td class="tdValor ValorNumerico    <%# DataBinder.Eval(Container.DataItem, "ValorNegocioPosNeg") %>"><%# DataBinder.Eval(Container.DataItem, "ValorNegocio")%> </td>
                        <td class="tdValor ValorNumerico    <%# DataBinder.Eval(Container.DataItem, "ValorTotalPosNeg")   %>"><%# DataBinder.Eval(Container.DataItem, "ValorTotal")  %> </td>
                        <td class="tdLabel">                <%# DataBinder.Eval(Container.DataItem, "DC")                 %> </td>
                    </tr>   --%>             
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
                    <%--<tr style="font-size:0.9em">
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
                    </tr>--%>
                </tfoot>
        
                <tbody>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Venda Disponível</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("VendaDisponivel")%></td>
                    
                    <td class="tdLabel">Compra Dispoível</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("CompraDisponivel")%></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Venda Opções</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("VendaOpcoes")%></td>
                    
                    <td class="tdLabel">Compra Opções</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("CompraOpcoes")%></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor dos Negócios</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("ValorNegocios")%></td>
                    
                    <td class="tdLabel">IRRF</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("IRRF")%></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Day Trade(Projeção)</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("IRRFDayTrade")%></td>
                    
                    <td class="tdLabel">Taxa Operacional</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("TaxaOperacional")%></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Taxa Registro Bmf</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("TaxaRegistroBmf")%></td>
                    
                    <td class="tdLabel">Taxas Bmf</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("TaxaBmf")%></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">ISS</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("ISS")%></td>
                    
                    <td class="tdLabel">Ajuste de Posição</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("AjustePosicao")%></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Ajuste Day Trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("AjusteDayTrade")%></td>
                    
                    <td class="tdLabel">Total de Despesas</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("TotalDespesas")%></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Corretagem</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("IrrfCorretagem")%></td>
                    
                    <td class="tdLabel">Total Conta Normal</td>
                    <td class="tdValor ValorNumerico"> <%# Eval("TotalContaNormal")%></td>
                    
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Total Líquido</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%# Eval("TotalLiquido")%></td>
                    
                    <td class="tdLabel">Total Líquido da Nota</td>
                    <td class="tdValor ValorNumerico"><%# Eval("TotalLiquidoNota")%></td>
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
                <%--<tbody>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Debêntures</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("Debentures")%></td>
                    
                        <td class="tdLabel">Valor Líquido das Operações(1)</td>
                        <td class="tdValor ValorNumerico"><%# Eval("ValorLiquidoOperacoes")%></td>
                        <td class="tdLabel"><%# Eval("ValorLiquidoOperacoes_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Vendas à Vista</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("VendaVista")%></td>
                    
                        <td class="tdLabel">Taxa de Registro(3)</td>
                        <td class="tdValor ValorNumerico"><%# Eval("TaxaDeRegistro")%></td>
                        <td class="tdLabel"><%# Eval("TaxaDeRegistro_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Compras à Vista</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("CompraVista")%></td>
                    
                        <td class="tdLabel">Taxa de Liquidação(2)</td>
                        <td class="tdValor ValorNumerico"><%# Eval("TaxaLiquidacao")%></td>
                        <td class="tdLabel"><%# Eval("TaxaLiquidacao_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Opções - Compras</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("CompraOpcoes")%></td>
                    
                        <td class="tdLabel">Total (1+2+3) A</td>
                        <td class="tdValor ValorNumerico"><%# Eval("Total_123_A")%></td>
                        <td class="tdLabel"><%# Eval("Total_123_A_DC") %></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Opções - Vendas</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("VendaOpcoes")%></td>
                    
                        <td class="tdLabel">Taxa de Termo/Opções/Futuro</td>
                        <td class="tdValor ValorNumerico"><%# Eval("TaxaTerOpcFut")%></td>
                        <td class="tdLabel"><%# Eval("TaxaTerOpcFut_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Operações a Termo</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("OperacoesTermo")%></td>
                    
                        <td class="tdLabel">Taxa A.N.A</td>
                        <td class="tdValor ValorNumerico"><%# Eval("TaxaANA")%></td>
                        <td class="tdLabel"><%# Eval("TaxaANA_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Operações a Futuro</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("OperacoesFuturo")%></td>
                    
                        <td class="tdLabel">Emolumentos</td>
                        <td class="tdValor ValorNumerico"><%# Eval("Emolumentos")%></td>
                        <td class="tdLabel"><%# Eval("Emolumentos_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Valor das Oper. com Tit. Publ.</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("OperacoesTitulosPublicos")%></td>
                    
                        <td class="tdLabel">Total Bolsa B</td>
                        <td class="tdValor ValorNumerico"><%# Eval("TotalBolsaB")%></td>
                        <td class="tdLabel"><%# Eval("TotalBolsaB_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Valor das Operações</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("ValorDasOperacoes")%></td>
                    
                        <td class="tdLabel">Corretagem</td>
                        <td class="tdValor ValorNumerico"><%# Eval("Corretagem")%></td>
                        <td class="tdLabel"><%# Eval("Corretagem_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">Valor do Ajuste p/Futuro</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("ValorAjusteFuturo")%></td>
                    
                        <td class="tdLabel">ISS</td>
                        <td class="tdValor ValorNumerico"><%# Eval("ISS")%></td>
                        <td class="tdLabel"><%# Eval("ISS_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">IR Sobre Corretagem</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("IRSobreCorretagem")%></td>
                    
                        <td class="tdLabel">I.R.R.F. s/ operações, base R$ <%# Eval("VLBaseOperacoesIRRF")%></td>
                        <td class="tdValor ValorNumerico"><%# Eval("IRRFOperacoes") %></td>
                        <td class="tdLabel"><%# Eval("IRRFOperacoes_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td class="tdLabel">IRRF Sobre Day Trade</td>
                        <td class="tdValor ValorNumerico BordaDireita"><%# Eval("IRRFSobreDayTrade")%></td>
                    
                        <td class="tdLabel">Outras</td>
                        <td class="tdValor ValorNumerico"><%# Eval("Outras")%></td>
                        <td class="tdLabel"><%# Eval("Outras_DC")%></td>
                    </tr>
                    <tr style="font-size:0.8em">
                        <td colspan="2" class="BordaDireita"></td>
                
                        <td class="tdLabel">Líquido (A+B) para <%# Eval("LiquidoPara") %></td>
                        <td class="tdValor ValorNumerico"><%# Eval("ValorLiquidoNota") %></td>
                        <td class="tdLabel"><%# Eval("ValorLiquidoNota_DC")%></td>
                    </tr>
 
                </tbody>--%>
        
            </table>
            </div>
            <div style="page-break-before: always"></div>

        </div>
        </ItemTemplate>
    </asp:Repeater>

        
    </form>
    <script language="javascript" type="text/javascript">
        window.print();
//        window.close();
    </script>
</body>
</html>

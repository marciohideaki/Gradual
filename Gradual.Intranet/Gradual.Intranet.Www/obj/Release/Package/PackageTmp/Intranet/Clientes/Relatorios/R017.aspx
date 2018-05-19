<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R017.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R017" %>
<form id="form1" class="RelatorioDBMImpressao" runat="server">

    <div class="Relatorios_Gerais_NotaDeCorretagem_QuebraDePagina" style="page-break-before: always;"></div>
    
    <div class="Relatorio" id="divRelatorio" runat="server">
        <div style="padding-left:20px;" class="RelatorioImpressao">
        <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
            <h1 style="min-width: 586px;font-size:20px;">
                <span>Relatório: Nota de Corretagem</span>
                <span><asp:Label runat="server" ID="lblCabecalho_Provisorio" Text="(Provisória)" Font-Bold="true" Visible="false"></asp:Label></span>
                <span><img src="../Skin/default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></span>
            </h1>
        
            <h2 class="InformacoesDoCliente">
                <div><label style="padding-right:17px;">Data:</label><span><%= this.gTransporteRelatorio.NotaCabecalhoCorretora.DataPregao %></span></div>

                <div>
                    <label>Número da nota: </label><span><%= this.gTransporteRelatorio.NotaCabecalhoCorretora.NumeroNota%></asp:literal></span>
                </div>
        
                <div><label>Cliente:</label><span><%= this.gTransporteRelatorio.NotaCabecalhoCliente.CodCliente%> - <%= this.gTransporteRelatorio.NotaCabecalhoCliente.NomeCliente %></span></div>
        
                <div><label>Número da Nota:</label><span><%= this.gTransporteRelatorio.NotaCabecalhoCorretora.NumeroNota %> </span></div>
        
                <div>
                    <label>Conta:</label>
                    <span><%= this.gTransporteRelatorio.NotaCabecalhoCliente.ContaCorrente %></span>

                    <label>Agência:</label>
                    <span><%= this.gTransporteRelatorio.NotaCabecalhoCliente.Agencia %></span>

                    <label>Banco:</label>
                    <span><%= this.gTransporteRelatorio.NotaCabecalhoCliente.NrBanco %></span>
                </div>
            </h2>
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
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Debentures %></td>
                    
                    <td class="tdLabel">Valor Líquido das Operações(1)</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorLiquidoOperacoesPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorLiquidoOperacoes %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorLiquidoOperacoes_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Vendas à Vista</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.VendaVista %></td>
                    
                    <td class="tdLabel">Taxa de Registro(3)</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaDeRegistroPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaDeRegistro %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaDeRegistro_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Compras à Vista</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.CompraVista %></td>
                    
                    <td class="tdLabel">Taxa de Liquidação(2)</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaLiquidacaoPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaLiquidacao %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaLiquidacao_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Opções - Compras</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.CompraOpcoes %></td>
                    
                    <td class="tdLabel">Total (1+2+3) A</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.Total_123_APosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Total_123_A %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Total_123_A_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Opções - Vendas</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.VendaOpcoes %></td>
                    
                    <td class="tdLabel">Taxa de Termo/Opções/Futuro</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaTerOpcFutPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaTerOpcFut %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaTerOpcFut_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Operações a Termo</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.OperacoesTermo %></td>
                    
                    <td class="tdLabel">Taxa A.N.A</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaANAPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaANA %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TaxaANA_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Operações a Futuro</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.OperacoesFuturo %></td>
                    
                    <td class="tdLabel">Emolumentos</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.EmolumentosPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Emolumentos %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Emolumentos_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor das Oper. com Tit. Publ.</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.OperacoesTitulosPublicos %></td>
                    
                    <td class="tdLabel">Total Bolsa B</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.TotalBolsaBPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TotalBolsaB %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.TotalBolsaBPosNeg_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor das Operações</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorDasOperacoes %></td>
                    
                    <td class="tdLabel">Corretagem</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.CorretagemPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Corretagem %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Corretagem_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Valor do Ajuste p/Futuro</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorAjusteFuturo %></td>
                    
                    <td class="tdLabel">ISS</td>
                    <td class="tdValor ValorNumerico <%= this.gTransporteRelatorio.NotaRodapeCorretora.ISSPosNeg %>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ISS %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ISS_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IR Sobre Corretagem</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.IRSobreCorretagem %></td>
                    
                    <td class="tdLabel">I.R.R.F. s/ operações, base R$ <%= this.gTransporteRelatorio.NotaRodapeCorretora.IRRFSemOperacoesBase %></td>
                    <td class="tdValor ValorNumerico"><%= this.gTransporteRelatorio.NotaRodapeCorretora.IRRFSemOperacoesValor %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.IRRFOperacoes_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Sobre Day Trade</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.IRRFSobreDayTrade %></td>
                    
                    <td class="tdLabel">Outras</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_OutrasPosNeg" runat="server"></asp:literal>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Outras %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.Outras_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Sobre Day Trade Base</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.IRRFSobreDayTradeBase %></td>
                   
                    <td class="tdLabel">Líquido (A+B) para <%= this.gTransporteRelatorio.NotaRodapeCorretora.LiquidoPara %></td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblRodape_Liquido_ABPosNeg" runat="server"></asp:literal>"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorLiquidoNota %></td>
                    <td class="tdLabel"><%= this.gTransporteRelatorio.NotaRodapeCorretora.ValorLiquidoNota_DC %></td>
                </tr>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">IRRF Sobre Day Trade Projecao</td>
                    <td class="tdValor ValorNumerico BordaDireita"><%= this.gTransporteRelatorio.NotaRodapeCorretora.IRRFSobreDayTradeProjecao %></td>
                   
                    <td colspan="3"></td>
                </tr>
 
            </tbody>
        
        </table>

    </div>

    <div id="divCarregandoMais" class="MensagemCarregarQuantoDeTanto" runat="server" visible="false" style="width: 100%; text-align: center; font-size: 2em; margin: 25px;">
        <p><asp:label id="lblQuantoDeTanto" runat="server"></asp:label></p>
    </div>

</form>
<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="NotasDeCorretagem.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Financeiro.NotasDeCorretagem" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasFinanceiro.ascx"  tagname="AbasFinanceiro"  tagprefix="ucAbasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro1" modo="menu" runat="server" />

    <div class="menu-exportar clear">
        <h3>Notas de Corretagem <span class="icon3"> <%= this.Descricao %> </span></h3>

        <ul>
<%--            <li class="conta">
                <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                <div class="conta_msg" style="display:none">
                Confira os dados bancários da Gradual:<br />
                Banco BM&F - 096<br />
                Agência: 001<br />
                C/C: 326-5<br />
                Favorecido: Gradual CCTVM S/A<br />
                CNPJ: 33.918.160/0001-73
                </div>
            </li>--%>
            <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /><div class="ajuda">Acesse as suas Notas de Corretagem das operações no mercado à vista no próprio dia e para negociações BM&F, no dia seguinte</div></li>
            <li class="exportar">
                <input title="Exportar" type="button" />
                <ul>
                    <li><asp:Button ID="btnImprimirNotaCorretagem" runat="server" CssClass="pdf" Text="PDF" OnClick="btnImprimirPDF_Click" /> </li>
                    <li><asp:Button ID="btnImprimirExcel" Text="Excel" OnClick="btnExcel_Click" runat="server"  Cssclass="excel" /></li>
                </ul>
            </li>
            <li class="email"><asp:Button ID="btnEnviarEmail" runat="server" OnClick="btnEnviarEmail_Click" title="Enviar por e-mail" /></li>
        </ul>
    </div>
    
    <div class="row">
        <div class="col1">
            <div class="form-consulta form-padrao clear">
                <div class="row">
                    <div class="col6">
                        <div class="campo-consulta-nc">
                        <label>Bolsa:</label>
                            <asp:DropDownList ID="cboBolsa" runat="server" style="width:89%">
                                <asp:ListItem Value="bov" Selected="True">Bovespa</asp:ListItem>
                                <asp:ListItem Value="bmf">Bmf</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col6">
                        <div class="campo-consulta-nc">
                            <label>Tipo de Mercado:</label>
                            <asp:DropDownList ID="cboTipoMercado" runat="server" style="width:89%">
                                <asp:ListItem Value="VIS" Selected="True">À Vista</asp:ListItem>
                                <asp:ListItem Value="OPC">Opção</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col6">
                        <div class="campo-consulta-nc">
                            <label>De:</label>
                            <asp:TextBox ID="txtDataInicial" runat="server" name="txtDataInicial" MaxLength="10" cssclass="calendario" class= "calendario"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col6">
                        <div class="campo-consulta-nc">
                            <label>Ate:</label>
                            <asp:TextBox ID="txtDataFinal" runat="server" name="txtDataFinal" MaxLength="10" cssclass="calendario" class="calendario"></asp:TextBox>
                        </div>
                        <%--<asp:DropDownList id="DropDownList1" runat="server"></asp:DropDownList>--%>
                    </div>
                    <div class="colBot alignRight">
                        <label></label>
                        <asp:Button ID="btnPesquisar" runat="server" Text="Visualizar" CssClass="botao btn-padrao btn-erica" OnCommand="btnPesquisar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="pnlRelatorio" runat="server" visible="false">
        <asp:Panel ID="pnlPaginacao" runat="server" CssClass="navegacao" ></asp:Panel>        
        <div>
            <p>
            <table cellpadding="0" style="font-size:0.9em" >
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
                            <label>Data:</label><span><asp:Label id="lblData" runat="server" /></span><br />
                            <label>Cliente:</label><span><asp:Label id="lblCodigoCliente" runat="server" /> - <asp:Label id="lblNomeCliente" runat="server" /></span><br />
                            <label>Número da Nota:</label><span><asp:Label id="lblCabecalho_Cliente_NumeroDaNota" runat="server" /></span><br />
                        </td>
                        <td>
                            <label>C.N.P.J./C.P.F.:</label><span><asp:literal id="lblCpfCnpjCliente" runat="server"></asp:literal> </span><br />
                            <label>Código do cliente:</label><span><asp:literal id="lblCodigoDoCliente" runat="server"></asp:literal> </span>
                        </td>
                    </tr>
                </table>
            </p>
            
            
        </div>

        <table cellspacing="0" class="tabela">

            <thead>
                <tr class="tabela-area">
                    <td>Praça</td>
                    <td>C/V</td>
                    <td>Tipo Mercado</td>
                    <td>Espec Título</td>
                    <td>Obs</td>
                    <td>Quantidade</td>
                    <td>Preço</td>
                    <td>Valor (R$)</td>
                    <td>D/C</td>
                </tr>
            </thead>
            <tbody>

                <asp:repeater id="rptCorretagem" runat="server">
                <itemtemplate>

                <tr>
                    <td style="text-align:left">    <%# Eval("NomeBolsa")%>             </td>
                    <td style="text-align:center">  <%# Eval("TipoOperacao")%>          </td>
                    <td style="text-align:left">    <%# Eval("TipoMercado")%>           </td>
                    <td style="text-align:left">    <%# Eval("EspecificacaoTitulo")%>   </td>
                    <td style="text-align:left">    <%# Eval("Observacao")%>            </td>
                    <td class="ValorNumerico">      <%# Eval("Quantidade",   "{0:N}")%> </td>
                    <td class="ValorNumerico">      <%# Eval("ValorNegocio", "{0:N}")%> </td>
                    <td class="ValorNumerico">      <%# Eval("ValorTotal",   "{0:N}")%> </td>
                    <td style="text-align:center">  <%# Eval("DC",    "{0:N}")%>        </td>
                </tr>

                </itemtemplate>
                </asp:repeater>

                <tr id="trNeNhumItem" runat="server" visible="false">
                    <td colspan="9">Não há nota de corretagem disponível no período.</td>
                </tr>

            </tbody>
        </table>


        <table class="tabela">
            <thead>
                <tr>
                    <td colspan="2">Resumo dos Negócios</td>
                    <td>&nbsp;</td>
                    <td colspan="3">Resumo Financeiro</td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Debêntures</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_Debentures" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Valor Líquido das Operações(1)</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_ValorLiquidoDasOperacoes" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_ValorLiquidoDasOperacoes_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Vendas à Vista</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_VendasAVista" runat="server"></asp:literal></td>
                    
                    <td>&nbsp;</td>

                    <td>Taxa de Registro(3)</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_TaxaDeRegistro" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_TaxaDeRegistro_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Compras à Vista</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_ComprasAVista" runat="server"></asp:literal></td>
                    
                    <td>&nbsp;</td>

                    <td>Taxa de Liquidação(2)</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_TaxaDeLiquidacao" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_TaxaDeLiquidacao_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Opções - Compras</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_OpcoesCompras" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Total (1+2+3) A</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_Total_123_A" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_Total_123_A_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Opções - Vendas</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_OpcoesVendas" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Taxa de Termo/Opções/Futuro</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_TaxasTermo_Opcoes_Futuro" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_TaxasTermo_Opcoes_Futuro_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Operações a Termo</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_OperacoesTermo" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Taxa A.N.A</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_TaxaANA" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_TaxaANA_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Operações a Futuro</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_OperacoesFuturo" runat="server"></asp:literal></td>
                    
                    <td>&nbsp;</td>

                    <td>Emolumentos</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_Emolumentos" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_Emolumentos_DC" runat="server"></asp:literal></td>
                </tr>
                <tr >
                    <td>Valor das Oper. com Tit. Publ.</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_ValorOperacoesTitPub" runat="server"></asp:literal></td>
                    
                    <td>&nbsp;</td>

                    <td>Total Bolsa B</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_Total_Bolsa_B" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_Total_Bolsa_B_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Valor das Operações</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_ValorDasOperacoes" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Corretagem</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_Corretagem" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_Corretagem_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>Valor do Ajuste p/Futuro</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_AjusteFuturo" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>ISS</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_ISS" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_ISS_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>IR Sobre Corretagem</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_IRSobreCorretagem" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>I.R.R.F. s/ operações, base <asp:Literal runat="server" ID="lblRodape_IRRF_BaseOperacoes"></asp:Literal></td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_IRRF_SobreOperacoes" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_IRRF_SobreOperacoes_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>IRRF Sobre Day Trade</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_IRRF_SobreDayTrade" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Outras</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_Outras" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_Outras_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>IRRF Sobre Day Trade Base</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_IRRF_SobreDayTradeBase" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                    <td>Líquido (A+B) para <asp:Literal runat="server" ID="lblRodape_Liquido_Para"></asp:Literal></td>
                    <td class="ValorNumerico <%=this.cssLiquidoAB %>"><asp:literal id="lblRodape_Liquido_AB" runat="server"></asp:literal></td>
                    <td><asp:literal id="lblRodape_Liquido_AB_DC" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td>IRRF Sobre Day Trade Projeção</td>
                    <td class="ValorNumerico"><asp:literal id="lblRodape_IRRF_SobreDayTradeProjecao" runat="server"></asp:literal></td>

                    <td>&nbsp;</td>

                </tr>

            </tbody>
            <tfoot>
                <tr>
                    <td colspan="6">
                        <table cellspacing="0" class="legenda">
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
        </table>

    </div>

    <div id="pnlRelatorioBmf" runat="server" visible="false">
        <asp:Panel ID="pnlPaginacaoBmf" runat="server" CssClass="navegacao"></asp:Panel>
        <div >
            <p>
            <table cellpadding="0" style="font-size:0.9em" class="tabela">
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
                            <label>Data:</label><span><asp:Label id="lblDataBmf" runat="server" /></span><br />
                            <label>Cliente:</label><span><asp:Label id="lblCodigoClienteBmf" runat="server" /> - <asp:Label id="lblNomeClienteBmf" runat="server" /></span><br />
                            <label>Número da Nota:</label><span><asp:Label id="lblNumeroNotaBmf" runat="server" /></span><br />
                        </td>
                        <td>
                            <label>C.N.P.J./C.P.F.:</label><span><asp:literal id="lblCpfCnpjClienteBmf" runat="server"></asp:literal> </span><br />
                            <label>Código do cliente:</label><span><asp:literal id="lblCodigoDoClienteBmf" runat="server"></asp:literal> </span>
                        </td>
                    </tr>
                </table>
            </p>
        </div>

        <table cellspacing="0" class="tabela">

            <thead>
                <tr class="tabela-area">
                    <td>C/V</td>
                    <td>Mercado</td>
                    <td>Vencimento</td>
                    <td>Quantidade</td>
                    <td>Preço/Ajuste</td>
                    <td>Tipo de Negocio</td>
                    <td>Vlr. de Operação/Ajuste</td>
                    <td>D/C</td>
                    <td>Taxa Operacional</td>
                    
                </tr>
            </thead>
            <tbody>

                <asp:repeater id="rptLinhasDoRelatorioBmf" runat="server">
                <itemtemplate>

                <tr>
                    <td style="text-align:left">    <%# Eval("Sentido")%>                   </td>
                    <td style="text-align:center">  <%# Eval("Mercadoria").ToString() + Eval("Mercadoria_Serie")%>          </td>
                    <td style="text-align:left">    <%# Eval("Vencimento")%>                </td>
                    <td style="text-align:left">    <%# Eval("Quantidade")%>                </td>
                    <td style="text-align:left">    <%# Eval("PrecoAjuste")%>               </td>
                    <td style="text-align:left">    <%# Eval("TipoNegocio")%>               </td>
                    <td class="ValorNumerico">      <%# Eval("ValorOperacao", "{0:N}")%>    </td>
                    <td style="text-align:center">  <%# Eval("DC")%>                       </td>
                    <td class="ValorNumerico">      <%# Eval("TaxaOperacional", "{0:N}")%> </td>
                </tr>

                </itemtemplate>
                </asp:repeater>
                <tr id="trNenhumItemBmf" runat="server" visible="false">
                    <td colspan="9">Nenhum item encontrado.</td>
                </tr>

            </tbody>
        </table>

        <table class="tabela" cellspacing="0">
        
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
                    <td class="tdValor ValorNumerico BordaDireita"><asp:literal id="Literal1" runat="server"></asp:literal></td>
                    
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
            </tbody>
        
        </table>

    </div>
        
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Notas de Corretagem" />

</asp:Content>

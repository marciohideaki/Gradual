<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RendaVariavel.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Posicao.RendaVariavel" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasPosicao.ascx"  tagname="AbasPosicao"  tagprefix="ucAbasP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="PaginaConteudo">

    
    <div class="row">
            <div class="col2">
                <ucAbasP:AbasPosicao id="ucAbasPosica1" modo="menu" runat="server" />
            </div>
        </div>
    <div class="row">
            <div class="col1">

                <div class="menu-exportar clear">
                    <h3>Renda Variável <button type="button" class="one-for-all">+</button> <span class="icon3">(de <%=DateTime.Now.ToString("dd/MM/yyyy") %> às <%=DateTime.Now.ToString("HH:mm")%>)</span></h3>

                    <ul>
                        <li class="conta">
                            <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                            <div class="conta_msg" style="display:none">
                            Confira os dados bancários da Gradual:<br />
                            Banco BM&F - 096<br />
                            Agência: 001<br />
                            C/C: 326-5<br />
                            Favorecido: Gradual CCTVM S/A<br />
                            CNPJ: 33.918.160/0001-73
                            </div>
                        </li>
                        <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /><div class="ajuda">Consulte a sua posição em BM&F, Bovespa, BTC e Termo.</div></li>
                        <li class="exportar">
                            <input type="button" title="Exportar"  />
                            <ul>
                                <li><asp:Button ID="btnImprimirRendaVariavel" runat="server" CssClass="pdf" Text="PDF" OnClick="btnImprimirPDF_Click" /></li>
                                <li><asp:Button ID="Button1" runat="server" CssClass="excel" Text="Excel" OnClick="btnImprimirExcel_Click" /></li>
                            </ul>
                        </li>
                        <li class="email"><asp:Button ID="btnEnviarEmail" runat="server" OnClick="btnEnviarEmail_Click" title="Enviar por e-mail" /></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row">
                    <div class="col1">
                        <ul class="acordeon">
                            <li>
                                <div class="acordeon-opcao">BM&amp;F / Bovespa</div>
                                <div class="acordeon-conteudo">

                                    <table class="tabela">
                                        <tr class="tabela-titulo tabela-type-small">
                                            <td>Cód</td>
                                            <td>Nome</td>
                                            <td>MERC.</td>
                                            <td>Tp.Cart.</td>
                                            <td>Qtd. Exec. Cpa</td>
                                            <td>Qtd. Exec. Vda</td>
                                            <td>D1</td>
                                            <td>D2</td>
                                            <td>D3</td>
                                            <td>Qtd. Total</td>
                                            <td>Cotação</td>
                                            <td>Vl. Financ.</td>
                                            <td>Dt.Venc.</td>
                                        </tr>
                                        <asp:repeater id="rptPosicaoAtualBovespa" runat="server">
                                        <itemtemplate>
                                        <tr class="tabela-type-small">
                                            <td><%# Eval("CodigoAtivo")               %></td>
                                            <td><%# Eval("NomeAtivo")                 %></td>
                                            <td><%# Eval("Mercado")                   %></td>
                                            <td><%# Eval("TipoCarteira")              %></td>
                                            <td><%# Eval("QuantidadeAexecutarCompra") %></td>
                                            <td><%# Eval("QuantidadeAexecutarVenda")  %></td>
                                            <td><%# Eval("SaldoD1")                   %></td>
                                            <td><%# Eval("SaldoD2")                   %></td>
                                            <td><%# Eval("SaldoD3")                   %></td>
                                            <td><%# Eval("QuantidadeTotal")           %></td>
                                            <td><%# Eval("ValorCotacao")              %></td>
                                            <td><%# Eval("ValorFinanceiro")           %></td>
                                            <td><%# Eval("DataVencimento")            %></td>
                                        </tr>
                                        </itemtemplate>
                                        </asp:repeater>

                                        <tr id="trNenhumBovespa" runat="server" visible=false>
                                            <td colspan="13">Nenhum item encontrado.</td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table class="tabela">
                                        <tr class="tabela-titulo tabela-type-small">
                                            <td>Tp. Mercado     </td>
                                            <td>Instrumento     </td>
                                            <td>Cod. Série      </td>
                                            <td>Empresa         </td>
                                            <td>Tp.Cart.        </td>
                                            <td>Qtde. Abertura  </td>
                                            <td>Qtde. Compra    </td>
                                            <td>Qtde. Venda     </td>
                                            <td>Qtde. Atual     </td>
                                            <td>Ajuste          </td>
                                            <td>Cotação         </td>
                                            <td>Variação        </td>
                                            <td>Resultado       </td>
                                        </tr>
                                        <asp:repeater id="rptPosicaoAtualBmf" runat="server">
                                        <itemtemplate>
                                        <tr class="tabela-type-small">
                                            <td><%# Eval("TpMercado")                   %></td>
                                            <td><%# Eval("CodigoInstrumento")%></td>
                                            <td><%# Eval("CodigoSerie")               %></td>
                                            <td><%# Eval("Empresa")                   %></td>
                                            <td><%# Eval("Carteira")                  %></td>
                                            <td><%# Eval("QtdeAbertura")              %></td>
                                            <td><%# Eval("QtdeCompra")                %></td>
                                            <td><%# Eval("QtdeVenda")                 %></td>
                                            <td><%# Eval("QtdeAtual")                 %></td>
                                            <td><%# Eval("Ajuste")                    %></td>
                                            <td><%# Eval("Cotacao")                   %></td>
                                            <td><%# Eval("Variacao")                  %></td>
                                            <td><%# Eval("Resultado")                 %></td>
                                        </tr>
                                        </itemtemplate>
                                        </asp:repeater>

                                        <tr id="trNenhumBmf" runat="server" visible="false">
                                            <td colspan="13">Nenhum item encontrado.</td>
                                        </tr>

                                    </table>
                                </div>
                            </li>
                            
                            <li>
                                <div class="acordeon-opcao">BTC</div>
                                <div class="acordeon-conteudo">
                                    <table class="tabela">
                                        <tr class="tabela-titulo tabela-type-small">
                                            <td>Carteira</td>
                                            <td>Código Cliente  </td>
                                            <td>Data Abertura   </td>
                                            <td>Data Vencimento </td>
                                            <td>Instrumento     </td>
                                            <td>Preço Médio     </td>
                                            <td>Preço Mercado   </td>
                                            <td>Quantidade      </td>
                                            <td>Remuneração     </td>
                                            <td>Taxa            </td>
                                            <td>Tipo Contrato   </td>
                                            <td>Total Quant. </td>
                                            <td>Total Valor  </td>
                                        </tr>

                                        <asp:repeater id="rptPosicaoBtc" runat="server">
                                        <itemtemplate>
                                        
                                        <tr class="tabela-type-small">
                                            <td><%# Eval("Carteira")%>          </td>
                                            <td><%# Eval("CodigoCliente")%>     </td>
                                            <td><%# Eval("DataAbertura")%>      </td>
                                            <td><%# Eval("DataVencimento")%>    </td>
                                            <td><%# Eval("Instrumento")%>       </td>
                                            <td><%# Eval("PrecoMedio")%>        </td>
                                            <td><%# Eval("PrecoMercado")%>      </td>
                                            <td><%# Eval("Quantidade")%>        </td>
                                            <td><%# Eval("Remuneracao")%>       </td>
                                            <td><%# Eval("Taxa")%>              </td>
                                            <td><%# Eval("TipoContrato")%>      </td>
                                            <td><%# Eval("SubtotalQuantidade")%></td>
                                            <td><%# Eval("SubtotalValor")%>     </td>
                                        </tr>
                                        </itemtemplate>
                                        </asp:repeater>
                                        
                                        <tr id="trNenhumPosicaoBtc" runat="server" visible="false">
                                            <td colspan="13">Nenhum item encontrado.</td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                            
                            <li>
                                <div class="acordeon-opcao">Termo</div>
                                <div class="acordeon-conteudo">
                                    <table class="tabela">
                                        <tr class="tabela-titulo tabela-type-small">
                                            <td>Código Cliente</td>
                                            <td>Instrumento</td>
                                            <td>Subtotal Quant.</td>
                                            <td>Preço Médio</td>
                                            <td>Total Contrato</td>
                                            <td>Total L/P</td>
                                        </tr>

                                        <asp:repeater id="rptPosicaoTermo" runat="server">
                                        <itemtemplate>
                                        
                                        <tr class="tabela-type-small">
                                            <td><%# Eval("CodigoCliente")%>             </td>
                                            <td><%# Eval("Instrumento")%>               </td>
                                            <td><%# Eval("SubtotalQuantidade")%>        </td>
                                            <td><%# Eval("PrecoMedio")%>                </td>
                                            <td><%# Eval("SubtotalContrato")%>          </td>
                                            <td><%# Eval("SubtotalLucroPrejuizo")%>     </td>
                                        </tr>
                                        </itemtemplate>
                                        </asp:repeater>
                                        
                                        <tr id="trNenhumPosicaoTermo" runat="server" visible="false">
                                            <td colspan="6">Nenhum item encontrado.</td>
                                        </tr>
                                    </table>
                                </div>
                            </li>

                            <%--<li>
                                <div class="acordeon-opcao">Termo</div>
                                <div class="acordeon-conteudo">
                                    <table class="tabela">
                                        <tr class="tabela-titulo tabela-type-small">
                                            <td>Papel</td>
                                            <td>ISIN</td>
                                            <td>Quantidade</td>
                                            <td>Data Mvto</td>
                                            <td>Data Início</td>
                                            <td>Data Venc.</td>
                                            <td>Taxa</td>
                                            <td>Preço Médio</td>
                                            <td>Sentido</td>
                                        </tr>
                                    
                                        <tr class="tabela-type-small">
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                            <td>0,00</td>
                                        </tr>
                                    </table>
                                </div>
                            </li>--%>
                            
                        </ul>
                    </div>
                </div>

                <div class="row">
                    <div class="col1">
                        <h5>Controle de Investimentos</h5>
                        <p>Confira abaixo a tabela para controle de seus negócios, dividida proporcionalmente de acordo com investimento por produto: </p>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <table cellspacing="0" class="Grafico_Carteira" style="display:none">
                            <tbody>
                            </tbody>
                        </table>

                        <table class="tabela-grafico">
                            <tr class="grafico-titulo">
                                <td colspan="3">Saldo Total: R$ <strong><asp:literal id="lblComposicaoRendaVariavel" runat="server" /></strong></td>
                            </tr>
                            
                            <asp:repeater id="rptGrafico_RendaVariavel" runat="server">
                                <itemtemplate>
                                <tr data-label="<%#Eval("Key") %>" data-valor="<%#  Eval("Value").ToString().Replace(",","").Replace(".","")%>" data-color="<%#Eval("Color") %>">
                                    <td style='background-color:<%#Eval("Color") %>'  > </td> <%--class="grafico-cor-3"--%>
                                    <td> <%#Eval("Key") %>   </td>
                                    <td class="grafico-valor"> R$ <%# Eval("Value", "{0:N2}")%> </td>
                                </tr>
                                </itemtemplate>
                            </asp:repeater>

                            <tr>
                                <td colspan="3">&nbsp;</td>
                            </tr>

                        </table>
                    </div>
                    <div class="col2">
                        <div id="pnlGrafico_RendaVariavel" style="height:369px"></div>
                    </div>
                </div>
                <div class="row">
                    <div>
                        <p>Os dados não são atualizados em tempo real.</p>
                    </div>
                </div>
    </section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Posição - Renda Variável" />

</asp:Content>

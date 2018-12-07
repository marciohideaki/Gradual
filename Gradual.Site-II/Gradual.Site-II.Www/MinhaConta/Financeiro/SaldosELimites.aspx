<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="SaldosELimites.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Financeiro.SaldosELimites" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasFinanceiro.ascx"  tagname="AbasFinanceiro"  tagprefix="ucAbasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro1" modo="menu" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Saldos e Limites <button type="button" class="one-for-all">+</button> <span class="icon3">(de <%=DateTime.Now.ToString("dd/MM/yyyy") %> às <%=DateTime.Now.ToString("HH:mm")%>)</span></h3>

                <ul>
                    <li class="conta">
                        <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                        <div class="conta_msg" style="display:none">
                        Confira os dados bancários da Gradual:<br />
                        135 - Gradual CCTVM S/A<br />
                        Agência: 001<br />
                        C/C: <%= lCodigoClienteformatado %><br />
                        Transferência de mesma titularidade<br />
                        Código da corretora: 227-5
                        </div>
                    </li>
                    <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /><div class="ajuda">Consulte os Saldos e Limites disponíveis na sua conta.</div></li>
                    <li class="exportar">
                        <input type="button" title="Exportar" />
                        <ul>
                            <li><asp:Button ID="btnImprimirSaldosLimites" runat="server" CssClass="pdf" Text="PDF" OnClick="btnImprimirSaldosLimitesPDF_Click" /></li>
                            <li><asp:Button ID="btnImprimirExcel" runat="server" CssClass="excel" Text="Excel" OnClick="btnImprimirSaldosLimitesExcell_Click" /></li>
                        </ul>
                    </li>
                    <li class="email"><asp:Button ID="btnEnviarEmail" runat="server" OnClick="btnEnviarEmail_Click" title="Enviar por e-mail" /></li>
                </ul>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col2">
            <ul class="acordeon">
                <li>
                    <div class="acordeon-opcao ativo">Saldo Projetado em C/C</div>
                    <div class="acordeon-conteudo" style="display:block">
                        <table style="margin:0px 10px; width:50%">
                            <tr class="tabela-type-small">
                                <td>Saldo D+0 </td>
                                <td class="<%=this.cssLimiteAcoesD0 %>">R$ <%=TransporteSaldoDeConta.Acoes_SaldoD0.ToString("N2")%></td>
                            </tr>
                            <tr class="tabela-type-small">
                                <td>Saldo D+1</td>
                                <td class="<%=this.cssLimiteAcoesD1 %>">R$ <%=TransporteSaldoDeConta.Acoes_SaldoD1.ToString("N2")%></td>
                            </tr>
                            <tr class="tabela-type-small">
                                <td>Saldo D+2</td>
                                <td class="<%=this.cssLimiteAcoesD2 %>">R$ <%=TransporteSaldoDeConta.Acoes_SaldoD2.ToString("N2")%></td>
                            </tr>
                            <tr class="tabela-type-small">
                                <td>Saldo D+3</td>
                                <td class="<%=this.cssLimiteAcoesD3 %>">R$ <%=TransporteSaldoDeConta.Acoes_SaldoD3.ToString("N2")%></td>
                            </tr>
                            <tr class="tabela-type-small">
                                <td>Total à vista</td>
                                <td class="<%=this.cssSaldoProjetado %>">R$ <%=TransporteSaldoDeConta.SaldoProjetado.ToString("N2")%></td>
                            </tr>
                        </table>
                    </div>
                </li>
                <!--
                <li>
                    <div class="acordeon-opcao">Saldo em Ações</div>
                    <div class="acordeon-conteudo">
                        <p class="valor <%=this.cssAcoesLimiteTotalAVista %>">R$ <%=(TransporteSaldoDeConta.Acoes_LimiteTotalAVista).ToString("N2")%>   </p>
                    </div>
                </li>

                <li class="desativo">
                    <div class="acordeon-opcao">Saldo em Opções</div>
                    <div class="acordeon-conteudo">
                        <p class="valor <%=this.cssOpcoesLimiteTotal %>">R$ <%=TransporteSaldoDeConta.Opcoes_LimiteTotal.ToString("N2")%> </p>
                    </div>
                </li>

                <li>
                    <div class="acordeon-opcao">Saldo em Fundos</div>
                    <div class="acordeon-conteudo">
                        <p class="valor">R$ <%=TransporteSaldoDeConta.TotalFundos.ToString("N2")%>   </p>
                    </div>
                </li>

                <li>
                    <div class="acordeon-opcao">Saldo Total C/C + Fundos</div>
                    <div class="acordeon-conteudo">
                        <p class="valor <%=this.cssSaldoTotal %> ">R$ <%=TransporteSaldoDeConta.SaldoTotal.ToString("N2")%>  </p>
                    </div>
                </li>
                -->

                <li>
                    <div class="acordeon-opcao ativo">Composição da Carteira</div>
                    <div class="acordeon-conteudo" style="display:block">
                        <p class="valor <%=this.cssSomatoriaCustodia %>">R$ <%=TransporteSaldoDeConta.SomatoriaCustodia.ToString("N2")%>  </p>
                        <p class="tabela-type-small">*Dados disponíveis após abertura do mercado.</p>
                    </div>
                </li>
            </ul>
        </div>

        <div class="col2" style="display: none;>
            <h4 class="titulo-acordeon">Limite Operacional de Compras</h4>
            <ul class="acordeon">
                <li>
                    <div class="acordeon-opcao">Compra Ações</div>
                    <div class="acordeon-conteudo">
                        <p class="grafico-titulo">Valor Alocado:    <strong>R$ <asp:Label ID="lblLimiteAcoesCompraAlocado"  Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Disponível: <strong>R$ <asp:Label ID="lblLimiteAcoesCompra"         Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Total:      <strong>R$ <asp:Label ID="lblLimiteAcoesCompraTotal"    Text="0,00" runat="server" /> </strong></p>
                    </div>
                </li>
                            
                <li>
                    <div class="acordeon-opcao">Compra Opções</div>
                    <div class="acordeon-conteudo">
                        <p class="grafico-titulo">Valor Alocado:    <strong>R$ <asp:Label ID="lblLimiteOpcoesCompraAlocado" Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Disponível: <strong>R$ <asp:Label ID="lblLimiteOpcoesCompra"        Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Total:      <strong>R$ <asp:Label ID="lblLimiteOpcoesCompraTotal"   Text="0,00" runat="server" /> </strong></p>
                    </div>
                </li>
            </ul>
                        
            <h4 class="titulo-acordeon">Limite Operacional de Vendas</h4>
            <ul class="acordeon">
                <li>
                    <div class="acordeon-opcao">Venda Ações</div>
                    <div class="acordeon-conteudo">
                        <p class="grafico-titulo">Valor Alocado:    <strong>R$ <asp:Label ID="lblLimiteAcoesVendaAlocado"   Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Disponível: <strong>R$ <asp:Label ID="lblLimiteAcoesVenda"          Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Total:      <strong>R$ <asp:Label ID="lblLimiteAcoesVendaTotal"     Text="0,00" runat="server" /> </strong></p>
                    </div>
                </li>
                            
                <li>
                    <div class="acordeon-opcao">Venda Opções</div>
                    <div class="acordeon-conteudo">
                        <p class="grafico-titulo">Valor Alocado:    <strong>R$ <asp:Label ID="lblLimiteOpcoesVendaAlocado"  Text="0,00" runat="server" /></strong></p>
                        <p class="grafico-titulo">Valor Disponível: <strong>R$ <asp:Label ID="lblLimiteOpcoesVenda"         Text="0,00" runat="server" /> </strong></p>
                        <p class="grafico-titulo">Valor Total:      <strong>R$ <asp:Label ID="lblLimiteOpcoesVendaTotal"    Text="0,00" runat="server" /></strong></p>
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col2">

            <div class="box_cinza">
                
<%--                
                <em>Dados Bancários Gradual CCTVM S.A.</em><br />
                <em>Banco:</em> BM&F SA (nº 096)<br />
                <em>Agência:</em> 001 <em>C/C:</em> 326-5<br />
                <em>Código da Corretora:</em> 227-5<br />
                <em>CNPJ:</em> 33.918.160/0001-73
--%>

                <em>Dados Bancários</em><br />
                <em>135 - SC Gradual</em><br />
                <em>Agência:</em> 001<br />
                <em>C/C:</em> <%= lCodigoClienteformatado %><br />
                <em>Transferência de mesma titularidade</em><br />
            </div>

        </div>
    </div>
    
    <div class="row">
        <div class="col2">
            <div class="acordeon-conteudo" style="display:block"><strong>No caso de conta conjunta do Bradesco, é necessário fazer a TED para titularidade diferente e preencher com seus dados (nome e CPF) no campo do destinatário</strong></div>
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
                    <td colspan="3">Saldo Total: R$ <strong><asp:literal id="lblComposicaoCarteira" runat="server"></asp:literal></strong></td>
                </tr>
                            
                <asp:repeater id="rptGrafico_Carteira" runat="server">
                    <itemtemplate>
                    <tr data-label="<%#Eval("Key") %>" data-valor="<%# Convert.ToDecimal( Eval("Value")).ToString().Replace(",","").Replace(".","")%>" data-color="<%#Eval("Color") %>">
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

            <%--<div id="pnlGrafico_Carteira" style="height:369px"></div>--%>
            <div id="pnlGrafico_Patrimonio" style="height:369px"></div>

        </div>
    </div>

<!--

    <table  cellpadding="0" cellspacing="0">

        <tfoot>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
        </tfoot>
        <thead>
            <tr>
                <td colspan="2" align="center" >
                    Limites Intradiários
                </td>
            </tr>
            <tr>
                <td style="width:50%">Saldos</td>
                <td>Conta Depósito</td>
            </tr>
        </thead>

        <tbody>
            <tr>
                <th colspan="2" class="TituloCategoria">Para Compra de Ações</th>
            </tr>

            <tr>
                <th>Disponível (D0+D1+D2+D3)</th>
                <td class="ValorNumerico <%= this.cssLimiteAcoes %>">R$ </td>
            </tr>

                <tr class="trB">
                    <th class="SubTitulo">Saldo em D0</th>
                    <td class="ValorNumerico <%= this.cssLimiteAcoesD0 %>">R$ <%= TransporteSaldoDeConta.Acoes_SaldoD0.ToString("N2")%></td>
                </tr>
                <tr class="trB">
                    <th class="SubTitulo">Saldo em D1</th>
                    <td class="ValorNumerico <%= this.cssLimiteAcoesD1 %>">R$ <%= TransporteSaldoDeConta.Acoes_SaldoD1.ToString("N2")%></td>
                </tr>
                <tr class="trB">
                    <th class="SubTitulo">Saldo em D2</th>
                    <td class="ValorNumerico <%= this.cssLimiteAcoesD2 %>">R$ <%= TransporteSaldoDeConta.Acoes_SaldoD2.ToString("N2")%></td>
                </tr>
                <tr class="trB">
                    <th class="SubTitulo">Saldo em D3</th>
                    <td class="ValorNumerico <%= this.cssLimiteAcoesD3 %>">R$ <%= TransporteSaldoDeConta.Acoes_SaldoD3.ToString("N2")%></td>
                </tr>
            <tr>
                <th>Conta Margem</th>
                <td class="ValorNumerico <%= this.cssAcoesContaMargem %>">R$ <%= TransporteSaldoDeConta.Acoes_SaldoContaMargem.ToString("N2")%></td>
            </tr>
            <tr>
                <th>Limite para Compra</th>
                <td class="ValorNumerico <%= this.cssAcoesLimiteParaCompra %>">R$   </td>
            </tr>
            <tr>
                <th>Limite para Venda</th>
                <td class="ValorNumerico <%= this.cssAcoesLimiteParaVenda %>">R$ </td>
            </tr>
            <tr class="trB">
                <th>Saldo Bloqueado</th>
                <td class="ValorNumerico <%= this.cssAcoesSaldoBloqueado %>">R$ <%= TransporteSaldoDeConta.SaldoBloqueado.ToString("N2")%></td>
            </tr>

            <tr>
                <th colspan="2" class="TituloCategoria">Para Compra de Opções</th>
            </tr>
            <tr>
                <th>Disponível (D0+D1)</th>
                <td class="ValorNumerico <%= this.cssLimiteOpcoes %>">R$ </td>
            </tr>
                <tr class="trB">
                    <th class="SubTitulo">Saldo em D0</th>
                    <td class="ValorNumerico <%= this.cssLimiteOpcoesD0 %>">R$ <%= TransporteSaldoDeConta.Opcoes_SaldoD0.ToString("N2")%></td>
                </tr>
                <tr class="trB">
                    <th class="SubTitulo">Saldo Bolqueado em D1</th>
                    <td class="ValorNumerico <%= this.cssLimiteOpcoesD1 %>">R$ <%= TransporteSaldoDeConta.Opcoes_SaldoD1.ToString("N2")%></td>
                </tr>
            <tr>
                <th>Limite para Compra</th>
                <td class="ValorNumerico <%= this.cssOpcoesLimiteParaCompra %>">R$ </td>
            </tr>
            <tr>
                <th>Limite para Venda</th>
                <td class="ValorNumerico <%= this.cssOpcoesLimiteParaVenda %>">R$ </td>
            </tr>
            <tr class="trB">
                <th>Saldo Bloqueado</th>
                <td class="ValorNumerico <%= this.cssOpcoesSaldoBloqueado %>">R$ <%= TransporteSaldoDeConta.SaldoBloqueado.ToString("N2")%></td>
            </tr>
        </tbody>

    </table>

-->

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Saldos" />

</asp:Content>

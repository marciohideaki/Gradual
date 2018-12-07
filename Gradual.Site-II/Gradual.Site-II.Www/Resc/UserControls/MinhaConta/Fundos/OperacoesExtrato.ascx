<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesExtrato.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesExtrato" %>
<h4>Extrato</h4>
                                    
                    <p>Veja abaixo os extratos mensal e consolidado dos seus investimentos em Fundos. </p>
                                    
                    <ul class="acordeon acordeon-box">
                        <li>
                            <div class="acordeon-opcao">
                                <strong>Extrato Consolidado</strong>
                            </div>
                                            
                            <div class="acordeon-conteudo">
                                <%--<div class="form-padrao clear">
                                    <div class="row">
                                        <div class="col4">
                                            <div class="campo-consulta">
                                                <label>Produto</label>
                                                <select>
                                                    <option>Todos</option>
                                                </select>
                                            </div>
                                        </div>
                                                        
                                        <div class="col2 colBot">
                                            <input type="submit" value="Buscar" name="buscar" class="botao btn-padrao btn-erica">
                                        </div>
                                    </div>
                                </div>--%>
                                                
                                <table class="tabela">
                                    <tr class="tabela-titulo">
                                        <td>Fundo</td>
                                        <td>Saldo Bruto</td>
                                        <td>Saldo Cotas</td>
                                        <td>Valor Cota</td>
                                        <td>IR</td>
                                        <td>IOF</td>
                                        <td>Saldo Líquido</td>
                                    </tr>
                                    <asp:Repeater runat="server" ID="rptListaDeExtratoPosicaoConsolidado">
                                    <itemtemplate>
                                        <tr class="tabela-type-small">
                                            <td><%# Eval("NomeFundo")%>     </td>
                                            <td><%# Eval("ValorBruto")%>    </td>
                                            <td><%# Eval("QtdCotas")%>      </td>
                                            <td><%# Eval("ValorCota")%>     </td>
                                            <td><%# Eval("IR")%>            </td>
                                            <td><%# Eval("IOF")%>           </td>
                                            <td><%# Eval("ValorLiquido")%>  </td>
                                        </tr>
                                    </itemtemplate>
                                    </asp:Repeater>         
                                    <tr id="trNenhumExtratoConsolidada" runat="server" visible="false">
                                        <td colspan="7">Nenhum item encontrado.</td>
                                    </tr>

                                </table>
                                 <div class="form-padrao clear">
                                    <div class="row">
                                        <div class="col4">
                                            <div class="campo-consulta">
                                                <label>Total Bruto: <asp:Label ID="lblOperacoesExtratoTotalBruto" runat="server" /></label>
                                            </div>
                                        </div>
                                        <div class="col4">
                                            <div class="campo-consulta">
                                                <label>Total IOF: <asp:Label ID="lblOperacoesExtratoTotalIOF" runat="server" /></label>
                                                
                                            </div>
                                        </div> 
                                    </div>
                                    <div class="row">
                                        <div class="col4">
                                            <div class="campo-consulta">
                                                <label>Total IR: <asp:Label ID="lblOperacoesExtratoTotalIR" runat="server" /></label>
                                                
                                            </div>
                                        </div>
                                        <div class="col4">
                                            <div class="campo-consulta">
                                                <label>Saldo Líquido: <asp:Label ID="lblOperacoesExtratoSaldoTotal" runat="server" /> </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>               
                            </div>
                        </li>
                                        
                        <li>
                            <div class="acordeon-opcao">
                                <strong>Extrato Mensal</strong>
                            </div>
                                            
                            <div class="acordeon-conteudo">
                                <%--<div class="form-padrao clear">
                                    <div class="row">
                                        <div class="col4">
                                            <div class="campo-consulta">
                                                <label>Produto</label>
                                                <select>
                                                    <option>Todos</option>
                                                </select>
                                            </div>
                                        </div>
                                                        
                                        <div class="col2 colBot">
                                            <input type="submit" value="Buscar" name="buscar" class="botao btn-padrao btn-erica">
                                        </div>
                                    </div>
                                </div>--%>
                                                
                                <table class="tabela">
                                    <tr class="tabela-titulo">
                                        <td>Fundo</td>
                                        <td>Saldo Bruto</td>
                                        <td>Saldo Cotas</td>
                                        <td>Valor Cota</td>
                                        <td>IR</td>
                                        <td>IOF</td>
                                        <td>Saldo Líquido</td>
                                    </tr>
                                   <asp:Repeater runat="server" ID="rptListaDeExtratoMensal">
                                    <itemtemplate>
                                        <tr class="tabela-type-small">
                                            <td><%# Eval("NomeFundo")%>     </td>
                                            <td><%# Eval("ValorBruto")%>    </td>
                                            <td><%# Eval("QtdCotas")%>      </td>
                                            <td><%# Eval("ValorCota")%>     </td>
                                            <td><%# Eval("IR")%>            </td>
                                            <td><%# Eval("IOF")%>           </td>
                                            <td><%# Eval("ValorLiquido")%>  </td>
                                        </tr>
                                    </itemtemplate>
                                    </asp:Repeater>         
                                    <tr id="trNenhumExtratoMensal" runat="server" visible="false">
                                        <td colspan="7">Nenhum item encontrado.</td>
                                    </tr>                 
                                </table>
                                                
                                <table class="tabela">
                                    <tr class="tabela-area">
                                        <td colspan="7">Rentabilidade</td>
                                    </tr>
                                    <tr class="tabela-titulo">
                                        <td>Fundo</td>
                                        <td>Data Início</td>
                                        <td>PL Médio (Milhões)</td>
                                        <td>(%) Mês</td>
                                        <td>(%) Ano</td>
                                        <td>(%) Acum. 12 Meses</td>
                                    </tr>
                                    <asp:Repeater runat="server" ID="rptListaDeRentabilidade">
                                    <itemtemplate>                
                                    <tr class="tabela-type-small">
                                        <td><%# Eval("NomeProduto")%></td>
                                        <td><%# Eval("DataInicioFundo")%></td>
                                        <td><%# Eval("PatrimonioLiquido")%></td>
                                        <td><%# Eval("RentabilidadeMes")%></td>
                                        <td><%# Eval("RentabilidadeAno")%></td>
                                        <td><%# Eval("rentabilidade12Meses")%></td>
                                    </tr>
                                    </itemtemplate>
                                    </asp:Repeater>         
                                    <tr id="trNenhumRentabilidade" runat="server" visible="false">
                                        <td colspan="7">Nenhum item encontrado.</td>
                                    </tr>                  
                                    
                                </table>
                                <div style="visibility:hidden" class="row">
                                    <table class="tabela_grafico_Extrato"  style="display:none">
                                        <asp:Repeater runat="server" ID="rptListaDeValoresGrafico">
                                            <itemtemplate>
                                                <tr data-label="<%#Eval("NomeFundo") %>" data-valor="<%#Eval("Valor") %>">
                                                    <td> <%#Eval("NomeFundo") %>   </td>
                                                    <td class="grafico-valor"> R$ <%# Eval("Valor", "{0:N2}")%> </td>
                                                </tr>
                                            </itemtemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>                                                
                                <div  id="chrtRentabilidade" style=" width: 100%; height:200px; margin-bottom:10px"></div>
                                <div id="chartLegend" style="margin-bottom: 20px; width: 50%"></div>            
                                <div class="row">
                                    <div class="col2">
                                        <table class="tabela">
                                            <tr class="tabela-area">
                                                <td colspan="7">Índice</td>
                                            </tr>
                                            <tr class="tabela-titulo">
                                                <td>Indexador</td>
                                                <td>(%) Mês</td>
                                                <td>(%) Ano</td>
                                                <td>(%) Acum. 12 Meses</td>
                                            </tr>
                                            <asp:Repeater runat="server" ID="rptListaDeRentabilidadeIndices">
                                            <itemtemplate>                  
                                            <tr class="tabela-type-small">
                                                <td><%# Eval("NomeIndexador")%></td>
                                                <td><%# Eval("RetornoMes")%></td>
                                                <td><%# Eval("RetornoAno")%></td>
                                                <td><%# Eval("Retorno12Meses")%></td>
                                            </tr>
                                            </itemtemplate>
                                            </asp:Repeater>         
                                            <tr id="trNenhumRentabilidadeIndices" runat="server" visible="false">
                                                <td colspan="4">Nenhum item encontrado.</td>
                                            </tr>                
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
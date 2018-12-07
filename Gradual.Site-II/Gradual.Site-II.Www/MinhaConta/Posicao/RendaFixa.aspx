<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RendaFixa.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Posicao.RendaFixa" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasPosicao.ascx"  tagname="AbasPosicao"  tagprefix="ucAbasP" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/CompraTD.ascx" tagname="CompraTD" tagprefix="uc1" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/VendaTD.ascx" tagname="VendaTD" tagprefix="uc2" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/ConsultaTD.ascx" tagname="ConsultaTD" tagprefix="uc3" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/ExtratoTD.ascx" tagname="ExtratoTD" tagprefix="uc4" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/ConsultarProtocolo.ascx" tagname="ConsultarProtocolo" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="PaginaConteudo">
    <div class="row">
        <div class="col2">
            <ucAbasP:AbasPosicao id="AbasPosicao1" modo="menu" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col1">
            
            <div class="menu-exportar clear">
                <h3>Renda Fixa <button type="button" class="one-for-all">+</button> <span class="icon3">(de <%=DateTime.Now.ToString("dd/MM/yyyy") %> às <%=DateTime.Now.ToString("HH:mm")%>)</span></h3>
                            
                <ul>
<%--                    <li class="conta">
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
                    <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /> <div class="ajuda">Confira sua posição em Renda Fixa, Clubes e Tesouro Direto.</div></li>
                    <li class="exportar">
                        <input type="button" title="Exportar"/>
                        <ul>
                            <li><asp:Button ID="btnImprimirRendaVariavel" runat="server" CssClass="pdf" Text="PDF" OnClick="btnImprimirPDF_Click" /></li>
                            <li><asp:Button ID="btnImprimirExcel" runat="server" CssClass="excel" Text="Excel" OnClick="btnImprimirExcel_Click" /></li>
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
                <%--<li >
                    <div class="acordeon-opcao">Fundos</div>
                    <div class="acordeon-conteudo">
                        <table class="tabela">
                            <tr class="tabela-titulo tabela-type-small">
                                <td>Nome do Fundo</td>
                                <td>Cota</td>
                                <td>Quantidade</td>
                                <td>Valor Bruto(R$)</td>
                                <td>IR %</td>
                                <td>IOF %</td>
                                <td>Valor Líquido(R$)</td>
                            </tr>
                            <asp:repeater id="rptPosicaoFundo" runat="server">
                            <itemtemplate>        
                            <tr class="tabela-type-small">
                                <td><%# Eval("NomeFundo")%>             </td>
                                <td><%# Eval("ValorCota",     "{0:N}")%></td>
                                <td><%# Eval("QtdCotas",    "{0:N}")%></td>
                                <td><%# Eval("ValorBruto",    "{0:N}")%></td>
                                <td><%# Eval("IR",            "{0:N}")%></td>
                                <td><%# Eval("IOF",           "{0:N}")%></td>
                                <td><%# Eval("ValorLiquido",  "{0:N}")%></td>
                            </tr>
                            </itemtemplate>
                            </asp:repeater>
                            <tr id="trNehumFundos" runat="server" visible="false">
                                <td colspan="7">Nenhum item encontrado.</td>
                            </tr>
                        </table>
                        
                    </div>
                </li>--%>
                <li>
                    <div class="acordeon-opcao"  >Renda Fixa</div>
                    <div class="acordeon-conteudo">
                        <table class="tabela">
                            <tr class="tabela-titulo tabela-type-small">
                                <td align="left">Título</td>
                                <td align="center">Aplicação</td>
                                <td align="center">Vencimento</td>
                                <td align="center">Taxa</td>
                                <td align="center">Quantidade</td>
                                <td align="center">Valor Original</td>
                                <td align="center">Saldo Bruto</td>
                                <td align="center">IRRF</td>
                                <td align="center">IOF</td>
                                <td align="right">L&iacute;quido</td>
                            </tr>

                            <asp:repeater id="rptRendaFixa" runat="server">
                            <itemtemplate>       
                            
                            <tr class="tabela-type-small">
                                <td ><%# Eval("Titulo"                            )%></td>
                                <td ><%# Eval("Aplicacao"       , "{0:dd/MM/yyyy}")%></td>
                                <td ><%# Eval("Vencimento"      , "{0:dd/MM/yyyy}")%></td>
                                <td ><%# Eval("Taxa"            , "{0:N4}")%></td>
                                <td ><%# Eval("Quantidade"      , "{0:N8}")%></td>
                                <td ><%# Eval("ValorOriginal"   , "{0:N2}")%></td>
                                <td ><%# Eval("SaldoBruto"      , "{0:N2}")%></td>
                                <td ><%# Eval("IRRF"            , "{0:N2}")%></td>
                                <td ><%# Eval("IOF"             , "{0:N2}")%></td>
                                <td ><%# Eval("SaldoLiquido"    , "{0:N2}")%></td>
                            </tr>
                            
                            </itemtemplate>
                            </asp:repeater>

                            <tr id="trNenhumItemRenda" runat="server" visible="false">
                                <td colspan="10">Nenhum item encontrado.</td>
                            </tr>
                            <tr id="trTotalRendaFixa" runat="server" visible="false">
                                <td colspan="4"></td>
                                <td ><asp:Label ID="lblTotalQuantidade"     runat="server"/></td>
                                <td ><asp:Label ID="lblTotalValorOriginal"  runat="server"/></td>
                                <td ><asp:Label ID="lblTotalSaldoBruto"     runat="server"/></td>
                                <td ><asp:Label ID="lblTotalIRRF"           runat="server"/></td>
                                <td ><asp:Label ID="lblTotalIOF"            runat="server"/></td>
                                <td ><asp:Label ID="lblTotalSaldoLiquido"   runat="server"/></td>
                            </tr>

                        </table>
                    </div>
                </li>            
                <li>
                    <div class="acordeon-opcao"  >Clubes</div>
                    <div class="acordeon-conteudo">
                        <table class="tabela">
                            <tr class="tabela-titulo tabela-type-small">
                                <td>Nome  do Clube</td>
                                <td>Cota</td>
                                <td>Quantidade</td>
                                <td>Valor Bruto(R$)</td>
                                <td>IR %</td>
                                <td>IOF %</td>
                                <td>Valor Líquido(R$)</td>
                            </tr>

                            <asp:repeater id="rptPosicaoClubes" runat="server">
                            <itemtemplate>       
                            
                            <tr class="tabela-type-small">
                                <td><%# Eval("NomeClube")%></td>
                                <td><%# Eval("Cota",          "{0:N}")%></td>
                                <td><%# Eval("Quantidade",    "{0:N}")%></td>
                                <td><%# Eval("ValorBruto",    "{0:N}")%></td>
                                <td><%# Eval("IR",            "{0:N}")%></td>
                                <td><%# Eval("IOF",           "{0:N}")%></td>
                                <td><%# Eval("ValorLiquido",  "{0:N}")%></td>
                            </tr>
                            
                            </itemtemplate>
                            </asp:repeater>

                            <tr id="trNenhumClube" runat="server" visible="false">
                                <td colspan="7">Nenhum item encontrado.</td>
                            </tr>

                        </table>
                    </div>
                </li>
                            
                <li>
                    <input type="hidden" id="Posicao_Acordeon_Selecionado" runat="server" />
                    <input type="hidden" id="Posicao_Aba_Selecionada" runat="server" />
                    <div class="acordeon-opcao" id="AcordeonTesouroDireto" >Tesouro Direto</div>
                        <div class="acordeon-conteudo">
                        
                        <div id="abas">
                            <ul class="abas-menu" id="abas-menu">
                                <li data-IdConteudo="Compra" class="ativo"><a href="#" id="li_Compra">Compra</a></li>
                                <li data-IdConteudo="Venda"><a href="#" id="li_Venda">Venda</a></li>
                                <li data-IdConteudo="Consulta"><a href="#" id="li_Consulta">Consulta</a></li>
                                <li data-IdConteudo="Extrato"><a href="#" id="li_Extrato">Extrato</a></li>
                                <li data-IdConteudo="ConsultarProtocolo"><a href="#" id="li_Protocolo">Consultar Protocolo</a></li>
                            </ul>							
                                        
                            <div class="abas-conteudo" id="abas-conteudo">
                            
                                <div class="aba" data-IdConteudo="Compra">
                                    <uc1:CompraTD ID="CompraTD1" runat="server" />
                                </div>
                                            
                                <div class="aba" data-IdConteudo="Venda">
                                    <uc2:VendaTD ID="VendaTD1" runat="server" />
                                </div>
                                            
                                <div class="aba" data-IdConteudo="Consulta">
                                    <uc3:ConsultaTD ID="ConsultaTD1" runat="server" />
                                </div>
                                            
                                <div class="aba" data-IdConteudo="Extrato">
                                    <uc4:ExtratoTD ID="ExtratoTD1" runat="server" />
                                </div>
                                               
                                <div class="aba" data-IdConteudo="ConsultarProtocolo">
                                    <uc5:ConsultarProtocolo ID="ConsultarProtocolo1" runat="server" />
                                </div>

                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="row">
        <div class="col1">
            <h5>Controle de investimentos</h5>
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
                    <td colspan="3">Saldo Total: R$ <strong><asp:literal id="lblComposicaoRendaFixa" runat="server" /></strong></td>
                </tr>
                            
                <asp:repeater id="rptGrafico_RendaFixa" runat="server">
                    <itemtemplate>
                    <tr data-label="<%#Eval("Key") %>" data-valor="<%# Eval("Value")%>" data-color="<%#Eval("Color") %>">
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
            <div id="pnlGrafico_RendaFixa" style="height:369px"></div>
        </div>
    </div>
    <div class="row">
        <div class="col2">
        <p>Os dados não são atualizados em tempo real.</p>
        </div>
    </div>
    </section>
    
<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Posição - Renda Fixa" />

</asp:Content>

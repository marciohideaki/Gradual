<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fundos.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Posicao.Fundos" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasPosicao.ascx"  tagname="AbasPosicao"  tagprefix="ucAbasP" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesPosicaoConsolidada.ascx" tagname="OperacoesPosicaoConsolidada" tagprefix="uc3" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesExtrato.ascx" tagname="OperacoesExtrato" tagprefix="uc5" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesResgate.ascx" tagname="OperacoesResgate" tagprefix="uc4" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesRelatorio.ascx" tagname="OperacoesRelatorio" tagprefix="uc7" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesSaldo.ascx" tagname="OperacoesSaldo" tagprefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="PaginaConteudo">
    <input type="hidden" id="Posicao_Aba_Simular_Selecionada" runat="server" />
    <div class="row">
        <div class="col2">
            <ucAbasP:AbasPosicao id="AbasPosicao1" modo="menu" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col1">
            
            <div class="menu-exportar clear">
                <h3>Fundos<button type="button" class="one-for-all">+</button> <span class="icon3">(de <%=DateTime.Now.ToString("dd/MM/yyyy") %> às <%=DateTime.Now.ToString("HH:mm")%>)</span></h3>
                            
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
                    <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /> <div class="ajuda">Confira a sua posição em Fundos.</b></div></li>
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
                <li >
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
                                <td>Resgatar</td>
                                <td>Aplicar</td>
                            </tr>
                            <asp:repeater id="rptPosicaoFundo" runat="server" OnItemDataBound="rptPosicaoFundo_ItemDataBound">
                            <itemtemplate>        
                            <tr class="tabela-type-small">
                                <td><%# Eval("NomeFundo")%>             </td>
                                <td style="text-align:center"><%# Eval("ValorCota",     "{0:N}")%></td>
                                <td style="text-align:center"><%# Eval("QtdCotas",      "{0:N6}")%></td>
                                <td style="text-align:center"><%# Eval("ValorBruto",    "{0:N}")%></td>
                                <td style="text-align:center"><%# Eval("IR",            "{0:N}")%></td>
                                <td style="text-align:center"><%# Eval("IOF",           "{0:N}")%></td>
                                <td style="text-align:center"><%# Eval("ValorLiquido",  "{0:N}")%></td>
                                <td><asp:Button CssClass="botao btn-invista" ID="btnResgatar" runat="server" Text="Resgate" OnClick="btnResgatar_Click" /></td>
                                <td><a class="botao btn-invista" href="../Produtos/Fundos/Aplicar.aspx?idFundo=<%#Eval("IdFundo")%>">Aplicar</a></td>
                            </tr>
                            </itemtemplate>
                            </asp:repeater>
                            <tr id="trNehumFundos" runat="server" visible="false">
                                <td colspan="9">Nenum item encontrado.</td>
                            </tr>
                        </table>
                        
                    </div>
                </li>
                <li>
                    <div class="acordeon-opcao">Posição Consolidada</div>
                    <div class="acordeon-conteudo">
                    <uc3:OperacoesPosicaoConsolidada ID="OperacoesPosicaoConsolidada1" runat="server" />
                    </div>
                </li>
                <li>
                    <div class="acordeon-opcao">Resgate</div>
                    <div class="acordeon-conteudo">
                    <uc4:OperacoesResgate ID="OperacoesResgate1" runat="server" />
                    </div>
                </li>
                <li>
                    <div class="acordeon-opcao">Extrato</div>
                    <div class="acordeon-conteudo">
                    <uc5:OperacoesExtrato ID="OperacoesExtrato1" runat="server" />
                    </div>
                </li>
                <li id="li_AbaRelatorios">
                    <div class="acordeon-opcao">Relatório</div>
                    <div class="acordeon-conteudo">
                    <uc7:OperacoesRelatorio ID="OperacoesRelatorio1" runat="server" />
                    </div>
                </li>
                <li>
                    <div class="acordeon-opcao">Saldo</div>
                    <div class="acordeon-conteudo">
                    <uc8:OperacoesSaldo ID="OperacoesSaldo1" runat="server" />
                    </div>
                </li>
            </ul>
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
                    <td colspan="3">Saldo Total: R$ <strong><asp:literal id="lblComposicaoFundos" runat="server" /></strong></td>
                </tr>
                            
                <asp:repeater id="rptGrafico_Fundos" runat="server">
                    <itemtemplate>
                    <tr data-label="<%#Eval("Key") %>" data-valor="<%# Eval("Value")%>" data-color="<%#Eval("Color") %>" class="tabela-type-small">
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
            <div id="pnlGrafico_Fundos" style="height:369px"></div>
        </div>
    </div>
    <script>
        $(".one-for-all").click();
    </script>
    </section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Posição - Fundos" />

</asp:Content>

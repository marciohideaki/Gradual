<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R019.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R019" %>

<form id="form1" runat="server">
   
    <div class="DivNavegacaoIniciais" style="padding-top: 15px; padding-bottom: 30px; text-align: center; width: 100%;">
  <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente DestaqueLetraSelecaoRelatorioContaCorrente">A</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">B</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">C</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">D</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">E</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">F</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">G</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">H</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">I</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">J</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">K</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">L</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">M</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">N</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">O</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">P</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">Q</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">R</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">S</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">T</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">U</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">V</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">X</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">Y</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">W</label>
        <label onclick="return btnGradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra_Click(this);" class="FiltroLetrasRelatorioContaCorrente">Z</label>

    </div>

    <span id="divGradIntra_Relatorios_Gerais_ContaCorrente_CarregandoConteudo" class="CarregandoConteudo" style="width: 100%; display: none;">
        Carregando, por favor aguarde...
    </span>
    
    <div id="divRelatorio" class="divRelatorioFinanceiroPorAssessorResultado" runat="server" style="margin-bottom: 15px;">
        <table cellspacing="1" width="100%" style="border-color: #000; border-style: solid; border-width: 1px;">
            <thead>
                <tr style="background-color: #CFCFCF">
                    <td colspan="24" align="left"  style="text-align: left; font-weight: bold;">Cliente: <strong><%= this.gTransporteRelatorio.Cabecalho.DadosCliente %></strong></td>
                </tr>
                <tr style="background-color: #9C9C9C;">
                    <td style="width: 30px; text-align: center;">Tipo</td>
                    <td style="width: 80px; text-align: center;">ISIN</td>
                    <td style="width: 45px; text-align: center;">Mercado</td>
                    <td style="width: 54px; text-align: center;">Código</td>
                    <td style="width: 67px; text-align: center;">Empresa</td>
                    <td style="width: 90px; text-align: center;">Carteira</td>
                    <td style="width: 65px; text-align: center;">Origem</td>
                    <td style="width: 65px; text-align: center;">Abertura</td>
                    <td style="width: 65px; text-align: center;">Vencimento</td>
                    <td style="width: 70px; text-align: center;">Q<sup>de</sup> Abertura</td>
                    <td style="width: 67px; text-align: center;">Q<sup>de</sup> Compra</td>
                    <td style="width: 60px; text-align: center;">Q<sup>de</sup> Venda</td>
                    <td style="width: 50px; text-align: center;">Q<sup>de</sup> Atual</td>
                    <td style="width: 45px; text-align: center;">Médio</td>
                    <td style="width: 45px; text-align: center;">Remuneração</td>
                    <td style="width: 45px; text-align: center;">Proj. D1</td>
                    <td style="width: 45px; text-align: center;">Proj. D2</td>
                    <td style="width: 45px; text-align: center;">Proj. D3</td>
                    <td style="width: 65px; text-align: center;">F. Anterior</td>
                    <td style="width: 45px; text-align: center;">Cotação</td>
                    <td style="width: 45px; text-align: center;">Variação</td>
                    <td style="width: 45px; text-align: center;">Líquido</td>
                    <td style="width: 45px; text-align: center;">Valor Atual</td>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td colspan="23"></td>
                </tr>
            </tfoot>
            <tbody>
                <asp:repeater runat="server" id="rptClientes_Custodia_Detalhes">
                    <itemtemplate>
                        <tr style="background-color: #D3D3D3; height: 10px;">
                            <td style="width: auto; text-align: center;"><%# Eval("Tomador") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("ISIN") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Mercado") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("Codigo") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("Empresa") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("Carteira") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Origem") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Abertura") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Vencimento") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtAbertura") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtCompra") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtVenda") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtAtual") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Medio") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Remuneracao") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtdD1")%></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtdD2")%></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtdD3")%></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("FechamentoAnterior") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Cotacao") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Variacao") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Liquido") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("ValorAtual") %></td>
                        </tr>
                    </itemtemplate>
                    <AlternatingItemTemplate>
                        <tr style="background-color: #BEBEBE; height: 10px;">
                            <td style="width: auto; text-align: center;"><%# Eval("Tomador") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("ISIN") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Mercado") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("Codigo") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("Empresa") %></td>
                            <td style="width: auto; text-align: left;">  <%# Eval("Carteira") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Origem") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Abertura") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("Vencimento") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtAbertura") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtCompra") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtVenda") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtAtual") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Medio") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Remuneracao") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtdD1")%></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtdD2")%></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("QtdD3")%></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("FechamentoAnterior") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Cotacao") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Variacao") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("Liquido") %></td>
                            <td style="width: auto; text-align: right;"> <%# Eval("ValorAtual") %></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr id="trClienteSemCustodia" runat="server" visible="false" style="background-color: #D3D3D3; height: 20px;">
                    <td colspan="24" style="text-align: center;">Este cliente não possui posição de Custódia</td>
                </tr>
            </tbody>
        </table>
    </div>

    <div id="divCarregandoMais" class="MensagemCarregarQuantoDeTanto" runat="server" visible="false" style="width: 100%; text-align: center; font-size: 2em; margin: 25px;">
        <p><asp:label id="lblQuantoDeTanto" runat="server"></asp:label></p>
    </div>

    <%--<div id="divGradualAssinaturaRodape" class="divGradualAssinaturaRodape" runat="server" visible="false" style="margin-top: 2.75em;">
        <p style="text-align: center; width: 100%;">Gradual C.C.T.V.M. S/A</p>
    </div>--%>

</form>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R018.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R018" %>

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
                    <td colspan="4" align="left"  style="text-align: left; font-weight: bold;">Cliente: <%= this.gTransporteRelatorio.CabecalhoExtrato.NomeCliente %></td>
                    <td colspan="1" align="right" style="text-align: right; padding-right: 2px;">Saldo Anterior: </td>
                    <td colspan="1" align="left"  style="text-align: left; padding-left: 3px; font-weight: bold;"> R$ <%= this.gTransporteRelatorio.CabecalhoExtrato.SaldoAnterior %></td>
                </tr>
                <tr style="background-color: #9C9C9C;">
                    <td style="text-align: left;  padding-left:  2px; width: 6em;">Data Mov.</td>
                    <td style="text-align: left;  padding-left:  2px; width: 6em;">Data Liq.</td>
                    <td style="text-align: left;  padding-left:  2px; width: auto;">Descrição do lançamento</td>
                    <td style="text-align: right; padding-right: 2px; width: 7.5em;">Débito</td>
                    <td style="text-align: right; padding-right: 2px; width: 7.5em;">Crédito</td>
                    <td style="text-align: right; padding-right: 2px; width: 7.5em;">Saldo atual</td>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td colspan="6"></td>
                </tr>
            </tfoot>
            <tbody>
                <asp:repeater runat="server" id="rptClientes_ExtratoContaCorrente_Movimento">
                    <itemtemplate>
                        <tr style="background-color: #D3D3D3; height: 10px;">
                            <td style="text-align: left;  padding-left:  2px;"><%# Eval("Mov") %></td>
                            <td style="text-align: left;  padding-left:  2px;"><%# Eval("Liq") %></td>
                            <td style="text-align: left;  padding-left:  2px;"><%# Eval("Historico") %></td>
                            <td style="text-align: right; padding-right: 2px;"><%# Eval("Debito") %></td>
                            <td style="text-align: right; padding-right: 2px;"><%# Eval("Credito") %></td>
                            <td style="text-align: right; padding-right: 2px;"><%# Eval("Saldo") %></td>
                        </tr>
                    </itemtemplate>
                    <AlternatingItemTemplate>
                        <tr style="background-color: #BEBEBE; height: 10px;">
                            <td style="text-align: left;  padding-left:  2px;"><%# Eval("Mov") %></td>
                            <td style="text-align: left;  padding-left:  2px;"><%# Eval("Liq") %></td>
                            <td style="text-align: left;  padding-left:  2px;"><%# Eval("Historico") %></td>
                            <td style="text-align: right; padding-right: 2px;"><%# Eval("Debito") %></td>
                            <td style="text-align: right; padding-right: 2px;"><%# Eval("Credito") %></td>
                            <td style="text-align: right; padding-right: 2px;"><%# Eval("Saldo") %></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
            </tbody>
        </table>
    </div>

    <div id="divCarregandoMais" class="MensagemCarregarQuantoDeTanto" runat="server" visible="false" style="width: 100%; text-align: center; font-size: 2em; margin: 25px;">
        <p><asp:label id="lblQuantoDeTanto" runat="server"></asp:label></p>
    </div>

    <div style="page-break-before: always;"></div>
</form>
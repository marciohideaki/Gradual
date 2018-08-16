<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R006.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Relatorios.R006" %>

<form id="form1" runat="server" >

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de Risco <span>Posição de Conta Corrente</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Última consolidação realizada em <span><%= string.Format("{0} às {1}.", gDataHoraUltimaAtualizacao.ToString("dd/MM/yyyy"), gDataHoraUltimaAtualizacao.ToString("HH:mm")) %></span></h2>
        <h2>Retirado em <span><%= string.Format("{0} às {1}.", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
        <h2>*Este relatório é consolidado a cada 30 minutos.</h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left;  width: 30%"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this);" title="Ordernar por Nome">Cliente</a></td>
                <td style="text-align:left;  width: 4em"> CPF/CNPJ </td>
                <td style="text-align:left;  width: 30%"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this, 'assessor');" title="Ordernar por Assessor">Assessor</a></td>
                <td style="text-align:right; width: 6em"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this, 'D0');" title="Ordernar por D0">Saldo (D + 0)*</a></td>
                <td style="text-align:right; width: 6em"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this, 'D1');" title="Ordernar por D1">(D + 1)</a></td>
                <td style="text-align:right; width: 6em"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this, 'D2');" title="Ordernar por D2">(D + 2)</a></td>
                <td style="text-align:right; width: 6em"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this, 'D3');" title="Ordernar por D3">(D + 3)</a></td>
                <td style="text-align:right; width: 6em"><a style="cursor:pointer" class="RiscoPosicaoCCOrdernarPor" onclick="return btnRisco_Relatorios_FiltrarRelatorio_Click(this, 'CM');" title="Ordernar por Conta Margem">Conta Margem</a></td>
            </tr>
        </thead>

        <tfoot>
            <tr>
                <td colspan="2">&nbsp;</td>
                <td>Totais</td>
                <td style="text-align:right; width: 6em" class="SaldoTotalD0"><%= gSubTotalD0 %></td>
                <td style="text-align:right; width: 6em" class="SaldoTotalD1"><%= gSubTotalD1 %></td>
                <td style="text-align:right; width: 6em" class="SaldoTotalD2"><%= gSubTotalD2 %></td>
                <td style="text-align:right; width: 6em" class="SaldoTotalD3"><%= gSubTotalD3 %></td>
                <td style="text-align:right; width: 6em" class="SaldoTotalCM"><%= gSubTotalCM %></td>
            </tr>
        </tfoot>
    
        <tbody>

            <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr> 
                <td style="text-align:left;  width: 30%"> <%# Eval("NomeCliente") %> </td>
                <td style="text-align:left;  width: 4em"> <%# Eval("CpfCnpj")     %> </td>
                <td style="text-align:left;  width: 30%"> <%# Eval("Assessor")    %> </td>
                <td style="text-align:right; width: 6em" class="SaldoParcialD0">  <%# Eval("D0")%> </td>
                <td style="text-align:right; width: 6em" class="SaldoParcialD1">  <%# Eval("D1")%> </td>
                <td style="text-align:right; width: 6em" class="SaldoParcialD2">  <%# Eval("D2")%> </td>
                <td style="text-align:right; width: 6em" class="SaldoParcialD3">  <%# Eval("D3")%> </td>
                <td style="text-align:right; width: 6em" class="SaldoParcialCM">  <%# Eval("CM")%> </td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>
</form>

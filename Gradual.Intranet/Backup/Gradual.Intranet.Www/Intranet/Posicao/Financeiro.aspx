<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Financeiro.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.Financeiro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Relatório Financeiro</title>

    <link rel="Stylesheet" media="all"   href="../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" media="print" href="../Skin/Default/Relatorio.Print.css" />
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Principal.css" />

</head>
<body>
<form id="form1" runat="server">

    <div class="Relatorio">
    
        <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> | 
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this)">Fechar</a>
        </div>
        
        <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

            <h1 style="font-size: 18px;margin-top: 10px;">
                <span>Relatório: Saldo em Conta Corrente</span>
                <span><%=DateTime.Now.ToString("dd/MM/yyyy")%></span>
                <img src="../Skin/default/Img/HB01_Relatorio_Titulo_FundoGradual.png" />
            </h1>

            <h2 class="InformacoesDoCliente">
                <label>Cliente:</label>
                <span>
                    <asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal>
                </span>
            </h2>
        
        </div>

        <table cellspacing="0">
            <thead>
                <tr style="font-size:0.8em">
                    <td>Saldo Disponível</td>
                    <td class="tdLabel" style="text-align:right">Saldo (R$)</td>
                </tr>
            </thead>
            <tbody>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Valor para o Dia:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblSaldoDisponivel_ValorParaDia_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblSaldoDisponivel_ValorParaDia" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Resgate para o Dia:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblSaldoDisponivel_ResgateParaDia_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblSaldoDisponivel_ResgateParaDia" runat="server"></asp:literal></td>
                </tr>
            </tbody>
        </table>

        <table cellspacing="0">
            <thead>
                <tr style="font-size:0.8em">
                    <td>Limite</td>
                    <td class="tdLabel" style="text-align:right">Saldo (R$)</td>
                </tr>
            </thead>
            <tfoot>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Total Disponível à Vista:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLimite_TotalDisponivelAVista_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLimite_TotalDisponivelAVista" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Total Disponível para Opções:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLimite_TotalDisponivelParaOpcoes_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLimite_TotalDisponivelParaOpcoes" runat="server"></asp:literal></td>
                </tr>
            </tfoot>
            <tbody>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Limite Total de Crédito à Vista:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblLimite_DeCreditoAVista" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Limite Total de Crédito para Opções:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblLimite_DeCreditoParaOpcoes" runat="server"></asp:literal></td>
                </tr>
            </tbody>
        </table>
        
        <table cellspacing="0">
            <thead>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Lançamentos Projetados para</td>
                    <td class="tdLabel" style="text-align:right">Saldo (R$)</td>
                </tr>
            </thead>
            <tfoot>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Total Projetado:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLancamento_AcoesProjetadoTotal_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLancamento_AcoesProjetadoTotal" runat="server"></asp:literal></td>
                </tr>
            </tfoot>
            <tbody>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Projetado D+1:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLancamento_AcoesProjetadoD1_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLancamento_AcoesProjetadoD1" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Projetado D+2:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLancamento_AcoesProjetadoD2_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLancamento_AcoesProjetadoD2" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Projetado D+3:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLancamento_AcoesProjetadoD3_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLancamento_AcoesProjetadoD3" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Saldo Cta. Margem:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblLancamento_AcoesProjetadoContaMargem_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblLancamento_AcoesProjetadoContaMargem" runat="server"></asp:literal></td>
                </tr>
            </tbody>
        </table>
        
        
        <table cellspacing="0">
            <thead>
                <tr style="font-size:0.8em">
                    <td class="tdLabel">Operações Realizadas do Dia</td>
                    <td class="tdLabel" style="text-align:right">Saldo (R$)</td>
                </tr>
            </thead>
            <tbody>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Compras Executadas à Vista:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblOperacoesRealizadasDoDia_ComprasExecutadas" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Vendas Executadas à Vista:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblOperacoesRealizadasDoDia_VendasExecutadas" runat="server"></asp:literal></td>
                </tr>
                <tr class="SubTotal" style="font-size:0.9em">
                    <td class="tdLabel">Total à Vista:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblOperacoesRealizadasDoDia_TotalAVista_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblOperacoesRealizadasDoDia_TotalAVista" runat="server"></asp:literal></td>
                </tr>
                
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Compras Executadas de Opções:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblOperacoesRealizadasDoDia_ComprasDeOpcoes" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Vendas Executadas de Opções:</td>
                    <td class="tdValor ValorNumerico"><asp:literal id="lblOperacoesRealizadasDoDia_VendasDeOpcoes" runat="server"></asp:literal></td>
                </tr>
                <tr class="SubTotal" style="font-size:0.9em">
                    <td class="tdLabel">Total de Opções:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblOperacoesRealizadasDoDia_TotalDeOpcoes_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblOperacoesRealizadasDoDia_TotalDeOpcoes" runat="server"></asp:literal></td>
                </tr>
            </tbody>
        </table>
        
        <table cellspacing="0">
            <thead style="font-size:0.89em">
                <tr>
                    <td>Operações Não Realizadas do Dia</td>
                    <td class="tdLabel" style="text-align:right">Quantidade</td>
                </tr>
            </thead>
            <tfoot>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Total em Aberto:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblOperacoesNaoRealizadasDoDia_TotalEmAberto_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblOperacoesNaoRealizadasDoDia_TotalEmAberto" runat="server"></asp:literal></td>
                </tr>
            </tfoot>
            <tbody>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Compras em Aberto:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblOperacoesNaoRealizadasDoDia_ComprasEmAberto_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblOperacoesNaoRealizadasDoDia_ComprasEmAberto" runat="server"></asp:literal></td>
                </tr>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Vendas em Aberto:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblOperacoesNaoRealizadasDoDia_VendasEmAberto_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblOperacoesNaoRealizadasDoDia_VendasEmAberto" runat="server"></asp:literal></td>
                </tr>
            </tbody>
        </table>
        
        <table cellspacing="0">
            <tbody>
                <tr style="font-size:0.9em">
                    <td class="tdLabel">Saldo Projetado:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblSaldoProjetado_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblSaldoProjetado" runat="server"></asp:literal></td>
                </tr>
                <tr>
                    <td class="tdLabel">Saldo Total em Conta corrente:</td>
                    <td class="tdValor ValorNumerico <asp:literal id="lblSaldoTotalEmContaCorrente_PosNeg" runat="server"></asp:literal>"><asp:literal id="lblSaldoTotalEmContaCorrente" runat="server"></asp:literal></td>
                </tr>
            </tbody>
        </table>

        </div>

</form>
</body>
</html>

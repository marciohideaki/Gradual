<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R002.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R002" %>

<form id="form1" runat="server">
<div id="divRelatorioGerencial" runat="server" class="CabecalhoRelatorio RelatorioImpressao RelatorioDBMImpressao">
 
    <h1>Resumo <span>Gerencial</span></h1>
  
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    
    <h2>
        Retirado em
        <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

    <h3 style="margin-left: 12px;">
        <span>Buscar por: <%= gCadastroNomeAssessor %></span>
    </h3>

    <h3 style="margin-left: 12px;">
        <span>Entre <%= gRelatorioDataInicial %> e <%= gRelatorioDataFinal %>.</span>
    </h3>

    <h3 style="margin: 12px;">Cadastro</h3>

    <table class="GridRelatorio" style="height: 12px; border: 0px solid black; width: 74.5em;">
        <thead>
            <tr>
                <td style="text-align:left;  width: 30em;">Cadastro</td>
                <td style="text-align:right; width: 26.5em;">Quantidade</td>
                <td style="text-align:right; width: auto;">%</td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="text-align: left">Total</td>
                <td style="text-align: right"><%= gCadastroQuantidadeTotal %></td>
                <td style="text-align: right"><%= gCadastroPercentualTotal %></td>
            </tr>
            <tr>
                <td  style="text-align: left">Ativos</td>
                <td style="text-align: right"><%= gCadastroQuantidadeAtivos %></td>
                <td style="text-align: right"><%= gCadastroPercentualAtivos %></td>
            </tr>
            <tr>
                <td  style="text-align: left">Inativos</td>
                <td style="text-align: right"><%= gCadastroQuantidadeInativos %></td>
                <td style="text-align: right"><%= gCadastroPercentualInativos %></td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td  style="text-align: left">Novos clientes mês</td>
                <td style="text-align: right"><%= gCadastroQuantidadeClientesNovos%></td>
                <td style="text-align: right"></td>
            </tr>
            <tr>
                <td  style="text-align: left">Varejo</td>
                <td style="text-align: right"><%= gCadastroQuantidadeVarejo %></td>
                <td style="text-align: right"></td>
            </tr>
            <tr>
                <td  style="text-align: left">Institucional</td>
                <td style="text-align: right"><%= gCadastroQuantidadeInstitucional %></td>
                <td style="text-align: right"></td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td  style="text-align: left">% Operou no mês</td>
                <td style="text-align: right"></td>
                <td style="text-align: right"><%= gCadastroPercentualOperouNoMes %></td>
            </tr>
            <tr>
                <td  style="text-align: left">% Com custódia</td>
                <td style="text-align: right"></td>
                <td style="text-align: right"><%= gCadastroPercentualComCustodia %></td>
            </tr>
        </tbody>
    </table>
    
    <h3 style="float: left; margin-left: 12px; width: 90%">Receita no período</h3>
    
    <table class="GridRelatorio" style="height: 12px; border: 0px solid black; width: 74.5em;">
        <thead>
            <tr>
                <td style="text-align: center; width: 30em;"></td>
                <td style="text-align: right;  width: 16.1em;">R$</td>
                <td style="text-align: right;  width: 9.5em;">%</td>
                <td style="text-align: center;">Meta</td>
                <td style="text-align: center;">% Meta</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td style="text-align: left;">Total</td>
                <td style="text-align: right;"><%= gReceitaTotalValor %></td>
                <td style="text-align: right;"><%= gReceitaTotalClientes %></td>
                <td style="text-align: right;"></td>
                <td style="text-align: right;"></td>
            </tr>
        </tfoot>
        <tbody>
            <tr>
                <td style="text-align: left;">Bovespa</td>
                <td style="text-align: right;"><%= gReceitaBovespaValor %></td>
                <td style="text-align: right;"><%= gReceitaBovespaClientes %></td>
                <td style="text-align: right;"></td>
                <td style="text-align: right;"></td>
            </tr>
            <tr>
                <td style="text-align: left;">BM&F</td>
                <td style="text-align: right;"><%= gReceitaBMFValor %></td>
                <td style="text-align: right;"><%= gReceitaBMFClientes %></td>
                <td style="text-align: right;"></td>
                <td style="text-align: right;"></td>
            </tr>
            <tr>
                <td style="text-align: left;">BTC</td>
                <td style="text-align: right;"><%= gReceitaBTCValor %></td>
                <td style="text-align: right;"><%= gReceitaBTCClientes %></td>
                <td style="text-align: right;"></td>
                <td style="text-align: right;"></td>
            </tr>
            <tr>
                <td style="text-align: left;">Tesouro</td>
                <td style="text-align: right;"><%= gReceitaTesouroValor %></td>
                <td style="text-align: right;"><%= gReceitaTesouroClientes %></td>
                <td style="text-align: right;"></td>
                <td style="text-align: right;"></td>
            </tr>
            <tr>
                <td style="text-align: left;">Outras</td>
                <td style="text-align: right;"><%= gReceitaOutrasValor %></td>
                <td style="text-align: right;"><%= gReceitaOutrasClientes %></td>
                <td style="text-align: right;"></td>
                <td style="text-align: right;"></td>
            </tr>
        </tbody>
    </table>
    
    <h3 style="float: left; margin-left: 12px; width: 90%">Canal no período</h3>
    
    <table class="GridRelatorio" style="height: 12px; border: 0px solid black; width: 74.5em;">
        <thead>
            <tr>
                <td style="text-align: center; width: 30em;"></td>
                <td style="text-align: right;  width: 26.5em;">R$</td>
                <td style="text-align: right;  width: auto;">%</td>  
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td style="text-align: left;">Total</td>
                <td style="text-align: right;"><%= gCanalTotalValor %></td>
                <td style="text-align: right;"><%= gCanalTotalPercentual %></td>
            </tr>
        </tfoot>
        <tbody>
            <tr>
                <td style="text-align: left;">HB</td>
                <td style="text-align: right;"><%= gCanalHbValor %></td>
                <td style="text-align: right;"><%= gCanalHbPercentual %></td>
            </tr>
            <tr>
                <td style="text-align: left;">Repassador</td>
                <td style="text-align: right;"><%= gCanalRepassadorValor %></td>
                <td style="text-align: right;"><%= gCanalRepassadorPercentual %></td>
            </tr>
            <tr>
                <td style="text-align: left;">Mesa</td>
                <td style="text-align: right;"><%= gCanalMesaValor %></td>
                <td style="text-align: right;"><%= gCanalMesaPercentual %></td>
            </tr>
        </tbody>
    </table>
    
    <h3 style="float: left; margin-left: 12px; width: 90%">Métricas</h3>
    
    <table class="GridRelatorio" style="height: 12px; border: 0px solid black; width: 74.5em;">
        <thead>
            <tr>
                <td style="text-align: center; width: 30em;"></td>
                <td style="text-align: right;  width:26.5em;">Corretagem</td>
                <td style="text-align: right;  width:auto;">Cadastros</td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="text-align: left;">No mês</td>
                <td style="text-align: right;"><%= gMetricasCorretagemNoMes %></td>
                <td style="text-align: right;"><%= gMetricasCadastrosNoMes %></td>
            </tr>
            <tr>
                <td style="text-align: left;">No mês anterior</td>
                <td style="text-align: right;"><%= gMetricasCorretagemNoMesAnterior%></td>
                <td style="text-align: right;"><%= gMetricasCadastrosNoMesAnterior %></td>
            </tr>
            <tr>
                <td style="text-align: left;">Média no período</td>
                <td style="text-align: right;"><%= gMetricasCorretagemNoAno%></td>
                <td style="text-align: right;"><%= gMetricasCadastrosNoAno %></td>
            </tr>
        </tbody>
    </table>


    <h3 style="float: left; margin-left: 12px; width: 90%">Top 10 Clientes no período</h3>
    
    <table class="GridRelatorio" style="height: 12px; border: 0px solid black; width: 74.5em;">
        <thead>
            <tr>
                <td style="text-align: center; width: 30em;">Clientes</td>
                <td style="text-align: right;  width: 16.5em;">R$</td>
                <td style="text-align: right;  width: 9em;">% total</td>
                <td style="text-align: right;  width: 91px;">Dev. média %</td>
                <td style="text-align: right;  width: auto;">Custódia (R$)</td>
            </tr>
        </thead>
        <tbody>
            <asp:repeater runat="server" id="rptDBM_ResumoAssessor_Top10">
                <itemtemplate>
                    <tr>
                        <td style="text-align: left;"> <%# Eval("NomeCliente")         %></td>
                        <td style="text-align: right;"><%# Eval("Corretagem")          %></td>
                        <td style="text-align: right;"><%# Eval("PercentualTotal")     %></td>
                        <td style="text-align: right;"><%# Eval("PercentualDevMedia")  %></td>
                        <td style="text-align: right;"><%# Eval("Custodia")            %></td>
                    </tr>
                </itemtemplate>
            </asp:repeater>
        </tbody>
    </table>

</div>
<div id="divMensagemDeErroDeAcesso" runat="server" visible="false" style="text-align: center; width: 100%">
    Você não tem permissão para visualizar este relatório
</div>
</form>
<script>DefinirCoresValores_Load();</script>
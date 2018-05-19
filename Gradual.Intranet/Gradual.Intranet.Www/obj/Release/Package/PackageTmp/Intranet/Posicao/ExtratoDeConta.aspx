<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtratoDeConta.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.ExtratoDeConta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Relatório - Extrato De Conta</title>
    
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Relatorio.css" />        
    <link rel="Stylesheet" media="print" href="../Skin/Default/Relatorio.Print.css" />
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"   href="../Skin/Default/Principal.css" />
    <style>
        div.Relatorio h1 img {
        display: block;
        float: right;
        width: 112px;
        height: 40px;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
<style>
        div.Relatorio h1 img {
        display: block;
        float: right;
        width: 112px;
        height: 40px;
        }
    </style>
    <div class="Relatorio" id="RelatorioExtratoConta" >
    
        <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
            <link rel="Stylesheet" media="all" href="../Skin/<%= this.SkinEmUso %>/Principal.css?v=<%= this.VersaoDoSite %>" />
            <link rel="Stylesheet" media="all" href="../Skin/<%= this.SkinEmUso %>/Intranet/Default.aspx.css?v=<%= this.VersaoDoSite %>" />
            <link rel="Stylesheet" media="all"   href="../Skin/Default/Intranet/Clientes.css" />
            <link rel="Stylesheet" media="all" href="../Skin/Default/Principal.css" />
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_Imprimir_Click(this)">Imprimir PDF</a> | 
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> | 
            <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this)">Fechar</a>
        </div>
        
       <br />

        <div class="RelatorioImpressao">
            <%--<link rel="Stylesheet" media="all" href="../../Skin/Default/Intranet/Clientes.css" /> --%>
            <h1>
                <span>Relatório: Extrato</span>
                <span style="font-size: small; margin-top:8px; font-weight:bold">período <%= this.GetDataInicial.ToString("dd/MM/yyyy") %> à <%= this.GetDataFinal.ToString("dd/MM/yyyy") %></span>
                <img src="../Skin/default/Img/HB01_Relatorio_Titulo_FundoGradual.png" style="float:right" />
            </h1>
            <h2 class="InformacoesDoCliente">
                <label>Cliente:</label>
                <span>
                    <asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal>
                </span>
            </h2>
        </div>

        <h5 style="text-align:right;padding: 0em 2em 1em 0em;">
            <p style="padding-bottom:2px;">Saldo Anterior:</p>
            <p><%= gSaldoAnterior %></p>
        </h5>

        <div style="text-align:center; width: 100%;">
            <asp:literal  runat="server" id="lblRelatorioVazio" visible="false" text="<tr><td colspan='10' class='RelatorioVazio'>Nenhum lançamento encontrado</td></tr>"></asp:literal>
        </div>

        <div id="divExtratoDeConta" runat="server">

            <%--<table cellspacing="0" class="ExtratoDeConta" style="float: left;">--%>
            <table cellspacing="0" class="ExtratoDeConta" style="width: 95%; float:left" >
                <thead>
                    <tr style="font-size:0.8em">
                        <td align="left">Mov.</td>
                        <td align="left">Liq.</td>
                        <td align="left">Histórico</td>
                        <td align="right">Débito</td>
                        <td align="right">Crédito</td>
                        <td align="right">Saldo</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:repeater runat="server" id="rptLinhasDoRelatorio">
                        <itemtemplate>
                            <tr style="font-size:0.9em">
                                <td class="tdLabel"><%# DataBinder.Eval(Container.DataItem, "Mov") %></td>
                                <td class="tdLabel"><%# DataBinder.Eval(Container.DataItem, "Liq") %></td>
                                <td class="tdLabel"><%# DataBinder.Eval(Container.DataItem, "Historico") %></td>
                                <td class="tdValor ValorNumerico ValorNegativo"><%# DataBinder.Eval(Container.DataItem, "Debito") %></td>
                                <td class="tdValor ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "Credito") %></td>
                                <td class="tdValor ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "Saldo") %></td>
                            </tr>
                        </itemtemplate>
                    </asp:repeater>
                </tbody>
            </table>

        </div>
        <div>
        <table cellspacing="0" class="ExtratoDeConta" id="tbTotaisCliente" runat="server" style="float:left">
            <tbody>
                <tr>
                    <td class="tdLabel">Disponível:</td>
                    <td class="tdValor ValorNumerico <%= gSaldoDisponivelPosNeg %>"><%= gSaldoDisponivel %></asp:literal></td>
                </tr>
                <tr>
                    <td class="tdLabel">Total Cliente:</td>
                    <td class="tdValor ValorNumerico <%= gTotalClientePosNeg %>"><%= gTotalCliente %></td>
                </tr>
            </tbody>
        </table>
        </div>
    </div>

</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContaCorrente.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.ContaCorrente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h4>Conta Corrente</h4>
        <div id="pnlSaldoDeConta">
        <input type="hidden" id="hidSaldoEmContaJson" runat="server" />
        

            <table id="tblSaldoDeConta" cellspacing="2">
                <thead>
                    <tr>
                        <td colspan="2" style="width:23%"><span>Saldos</span></td>
                        <td colspan="4" style="width:54%"><span>Limites</span></td>
                        <td colspan="2" style="width:23%"><span>BMF</span></td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="2">&nbsp;</td>

                        <td class="SubLabel" colspan="2"><span class="PadContaCorrente">Compras à Vista</span></td>

                        <td class="SubLabel" colspan="2"><span class="PadContaCorrente">Vendas à Vista</span></td>

                        <td colspan="2">&nbsp;</td>

                    </tr>

                    <tr>
                        <td class="tdLabel">Saldo D+0</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Acoes_SaldoD0" title="Saldo para compras à vista"><%= this.SaldoDeConta.Acoes_SaldoD0.ToString("n") %></td>

                        <td class="tdLabel">Limite Total</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_CompraAVista_Total"><%= this.SaldoDeConta.Limite_CompraAVista_Total.ToString("n")%></td>

                        <td class="tdLabel">Limite Total</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_VendaAVista_Total"><%= this.SaldoDeConta.Limite_VendaAVista_Total.ToString("n")%></td>

                        <td class="tdLabel">Limite Operacional</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BMF_LimiteOperacional" title="Limite operacional disponível para o mercado de BMF"><%= this.SaldoDeConta.BMF_LimiteOperacional.ToString("n") %></td>
                    </tr>

                    <tr>
                        <td class="tdLabel">Saldo D+1</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Acoes_SaldoD1" title="Saldo projetado para liquidações futuras em D+1"><%= this.SaldoDeConta.Acoes_SaldoD1.ToString("n") %></td>

                        <td class="tdLabel">Utilizado</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_CompraAVista_Utilizado"><%= this.SaldoDeConta.Limite_CompraAVista_Utilizado.ToString("n")%></td>

                        <td class="tdLabel">Utilizado</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_VendaAVista_Utilizado"><%= this.SaldoDeConta.Limite_VendaAVista_Utilizado.ToString("n")%></td>

                        <td class="tdLabel">Saldo Garantia</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BMF_SaldoMargem" title="Todo dinheiro ou papel alocado para operações no mercado futuro"><%= this.SaldoDeConta.BMF_SaldoMargem.ToString("n") %></td>
                    </tr>

                    <tr>
                        <td class="tdLabel">Saldo D+2</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Acoes_SaldoD2" title="Saldo projetado para liquidações futuras em D+2"><%= this.SaldoDeConta.Acoes_SaldoD2.ToString("n") %></td>

                        <td class="tdLabel">Disponível</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_CompraAVista_Disponivel"><%= this.SaldoDeConta.Limite_CompraAVista_Disponivel.ToString("n")%></td>

                        <td class="tdLabel">Disponível</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_VendaAVista_Disponivel"><%= this.SaldoDeConta.Limite_VendaAVista_Disponivel.ToString("n")%></td>

                        <td class="tdLabel">Disp. para Resgate</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_BMF_DisponivelParaResgate" title="(Limite Operacional + Saldo Garantia)"><%= this.SaldoDeConta.BMF_DisponivelParaResgate.ToString("n")%></td>
                    </tr>

                    <tr>
                        <td class="tdLabel">Saldo D+3</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Acoes_SaldoD3" title="Saldo projetado para liquidações futuras em D+3"><%= this.SaldoDeConta.Acoes_SaldoD3.ToString("n") %></td>

                        <td class="SubLabel" colspan="2"><span class="PadContaCorrente">Compras de Opções</span></td>

                        <td class="SubLabel" colspan="2"><span class="PadContaCorrente">Vendas de Opções</span></td>

                        <td colspan="2">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="tdLabel">Saldo Cta. Margem</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Acoes_SaldoContaMargem" title="Saldo em Conta Margem"><%= this.SaldoDeConta.Acoes_SaldoContaMargem.ToString("n")%></td>

                        <td class="tdLabel">Limite Total</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_CompraOpcoes_Total"><%= this.SaldoDeConta.Limite_CompraOpcoes_Total.ToString("n")%></td>

                        <td class="tdLabel">Limite Total</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_VendaOpcoes_Total"><%= this.SaldoDeConta.Limite_VendaOpcoes_Total.ToString("n")%></td>

                        <td colspan="2">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="tdLabel">Saldo Total à Vista</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Acoes_LimiteTotalAVista" title="Saldo D0 + Saldo D1 + Saldo D2 + Saldo D3 + Conta Margem"><%= this.SaldoDeConta.Acoes_LimiteTotalAVista.ToString("n") %></td>

                        <td class="tdLabel">Utilizado</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_CompraOpcoes_Utilizado"><%= this.SaldoDeConta.Limite_CompraOpcoes_Utilizado.ToString("n")%></td>

                        <td class="tdLabel">Utilizado</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_VendaOpcoes_Utilizado"><%= this.SaldoDeConta.Limite_VendaOpcoes_Utilizado.ToString("n")%></td>

                        <td colspan="2">&nbsp;</td>
                    </tr>

                    <tr>
                        <td colspan="2">&nbsp;</td>
                        
                        <td class="tdLabel">Disponível</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_CompraOpcoes_Disponivel"><%= this.SaldoDeConta.Limite_CompraOpcoes_Disponivel.ToString("n")%></td>

                        <td class="tdLabel">Disponível</td>
                        <td class="ValorNumericoContaCorrente" id="tdConta_Limite_VendaOpcoes_Disponivel"><%= this.SaldoDeConta.Limite_VendaOpcoes_Disponivel.ToString("n")%></td>

                        <td colspan="2">&nbsp;</td>
                    </tr>

                </tbody>
            </table>
            

            <p class="BotoesSubmit" style="margin-top:4em">

                <button id="btnConta_AtualizarSaldoEmConta" onclick="return btnConta_AtualizarSaldoEmConta_Click(this)">Atualizar Saldo</button>

            </p>

        </div>
    </form>
</body>
</html>

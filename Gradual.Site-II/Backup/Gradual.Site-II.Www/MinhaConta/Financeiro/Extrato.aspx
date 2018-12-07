<%@ Page Language="C#"  MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Extrato.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Financeiro.Extrato" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasFinanceiro.ascx"  tagname="AbasFinanceiro"  tagprefix="ucAbasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro1" modo="menu" runat="server" />

    <div class="menu-exportar clear">
        <h3>Extrato <span class="icon3"><%= this.Descricao %></span></h3>

        <ul>
            <li class="conta">
                <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                <div class="conta_msg" style="display:none">
                Confira os dados bancários da Gradual:<br />
                Banco BM&F - 096<br />
                Agência: 001<br />
                C/C: 326-5<br />
                Favorecido: Gradual CCTVM S/A<br />
                CNPJ: 33.918.160/0001-73
                </div>
            </li>
            <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /><div class="ajuda">Para consultar o extrato das suas negociações, você pode buscar pelo período de 7, 15, 30 dias, ou selecionar as datas, conforme desejar.</div></li>
            <li class="exportar">
                <input title="Exportar" type="button" />
                <ul>
                    <li><asp:Button ID="btnImprimirExtrato" runat="server" CssClass="pdf" Text="PDF" OnClick="btnImprimirPDF_Click" /> </li>
                    <li><asp:Button ID="btnImprimirExtratoExcel" runat="server" CssClass="excel"  Text="Excel" OnClick="btnExcel_Click"/></li>
                </ul>
            </li>
            <li class="email"><asp:Button ID="btnEnviarEmail" runat="server" OnClick="btnEnviarEmail_Click" title="Enviar por e-mail" /></li>
        </ul>
    </div>
    
    <div class="row">
        <div class="col1">
            <div class="form-consulta form-padrao clear">
                <div class="clear">
                    <div class="campo-consulta">
                        <label>Tipo:</label>

                        <asp:DropDownList ID="cboTipo" runat="server" >
                            <asp:ListItem Text="Liquidação" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Movimento" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="campo-consulta">
                        <label>Período:</label>

                        <asp:DropDownList ID="cboPeriodo" runat="server" onchange="return cboPeriodo_Change(this)">
                            <asp:ListItem Text="Últimos 7 dias" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Últimos 15 dias" Value="15" ></asp:ListItem>
                            <asp:ListItem Text="Últimos 30 dias" Value="30"></asp:ListItem>
                            <asp:ListItem Text="Por Data" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="campo-consulta">
                        <label>De:</label>
                        <asp:TextBox ID="txtDataInicial" runat="server" name="txtDataInicial" MaxLength="10" cssclass="calendario" class= "calendario"></asp:TextBox>
                    </div>

                    <div class="campo-consulta">
                        <label>Até:</label>

                        <asp:TextBox ID="txtDataFinal" runat="server" name="txtDataFinal" MaxLength="10" cssclass="calendario" class="calendario"></asp:TextBox>
                    </div>

                    <div class="campo-consulta">
                    <label></label>
                        <asp:button id="btnCarregarRelatorio" runat="server" onclick="btnCarregarRelatorio_Click" cssclass="botao btn-padrao btn-erica" text="Visualizar" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col1">
            <table class="tabela">
                <tr class="tabela-area">
                    <td class="alignRight" colspan="5">Saldo Anterior: R$</td>
                    <td class="alignRight <%=this.cssSaldoAnterior %>" colspan="1" > <asp:literal id="lblSaldoAnterior" runat="server"></asp:literal> </td>
                </tr>

                <tr class="tabela-titulo">
                    <td>Data Movimento</td>
                    <td>Data Liquidação</td>
                    <td>Histórico</td>
                    <td>Débito</td>
                    <td>Crédito</td>
                    <td>Saldo</td>
                </tr>

                <asp:repeater id="rptExtrato" runat="server">
                <itemtemplate>

                <tr>
                    <td style="text-align:center">  <%# Eval("DataMovimento",  "{0:dd/MM/yyyy}")%>    </td>
                    <td style="text-align:center">  <%# Eval("DataLiquidacao", "{0:dd/MM/yyyy}")%>    </td>
                    <td>                            <%# Eval("Historico")%>                           </td>
                    <td class="ValorNumerico ValorSaldo <%#DefinirCorDoValor(Eval("ValorDebito")) %>" style="text-align:right"> <%# Eval("ValorDebito",    "{0:N}")%> </td>
                    <td class="ValorNumerico ValorSaldo" style="text-align:right">  <%# Eval("ValorCredito",   "{0:N}")%>             </td>
                    <td class="ValorNumerico ValorSaldo <%#DefinirCorDoValor(Eval("ValorSaldo")) %>" style="text-align:right"> <%# Eval("ValorSaldo",     "{0:N}")%> </td>
                </tr>

                </itemtemplate>
                </asp:repeater>

                <tr id="trNenhumItem" runat="server" class="NenhumItem">
                    <td colspan="6">Não há extratos a serem exibidos.</td>
                </tr>

                <tr>
                    <td class="alignRight " colspan="5" ><strong>Saldo Disponível: R$</strong></td>
                    <td class="alignRight <%=this.cssSaldoDisponivel %>" colspan="1" ><strong> <asp:literal id="lblSaldoDisponivel" runat="server"></asp:literal> </strong></td>
                </tr>

                <tr>
                    <td class="alignRight" colspan="5"><strong>Saldo Total: R$</strong></td>
                    <td class="alignRight <%=this.cssSaldoTotal %>" colspan="1"><strong> <asp:literal id="lblSaldoTotal" runat="server"></asp:literal> </strong></td>
                </tr>
            </table>
        </div>
    </div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Extrato" />

</asp:Content>
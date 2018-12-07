<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExtratoTD.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.ExtratoTD" %>
<div class="form-consulta-tesouro form-padrao clear">
    <div class="row">
        <div class="col2">
            <div class="campo-consulta">
                <label>Mês:</label>
                <asp:DropDownList ID="Extrato_FiltroMes" runat="server">
                <asp:ListItem Value="-">Selecione</asp:ListItem>
                <asp:ListItem Value="1">Janeiro</asp:ListItem>
                <asp:ListItem Value="2">Fevereiro</asp:ListItem>
                <asp:ListItem Value="3">Março</asp:ListItem>
                <asp:ListItem Value="4">Abril</asp:ListItem>
                <asp:ListItem Value="5">Maio</asp:ListItem>
                <asp:ListItem Value="6">Junho</asp:ListItem>
                <asp:ListItem Value="7">Julho</asp:ListItem>
                <asp:ListItem Value="8">Agosto</asp:ListItem>
                <asp:ListItem Value="9">Setembro</asp:ListItem>
                <asp:ListItem Value="10">Outubro</asp:ListItem>
                <asp:ListItem Value="11">Novembro</asp:ListItem>
                <asp:ListItem Value="12">Dezembro</asp:ListItem>
            </asp:DropDownList>
            </div>
        </div>
                                                        
        <div class="col2">
            <div class="campo-consulta">
                <label>Vencimento:</label>
                <asp:DropDownList ID="Extrato_FiltroAno" runat="server">
                </asp:DropDownList>
            </div>
        </div>
    </div>
                                                    
    <div class="row">
        <div class="col1">
            <asp:Button id="Extrato_BtnConsultar" runat="server" cssclass="botao btn-padrao btn-erica" text="Consultar" OnClick="Extrato_BtnConsultar_Click" OnClientClick="btnTesouroCompra_Click(this, 'Extrato')" />
        </div>
    </div> 
</div>
<asp:Label ID="Extrato_BuscaSaida" runat="server" Visible="true" CssClass="GradualSaidaErro"></asp:Label>

    <asp:Panel ID="pnlResultadoExtrato" runat="server" Visible="false">

        <table class="tabela">
            <thead>
                <tr>
                    <td style="text-align: center;">Titulo</td>
                    <td style="text-align: center;">Vencimento</td>
                    <td style="text-align: center;">Crédito</td>
                    <td style="text-align: center;">Débito</td>
                    <td style="text-align: center;">Qtd. Bloqueada</td>
                    <td style="text-align: center;">Saldo Anterior</td>
                    <td style="text-align: center;">Saldo Atual</td>
                    <td style="text-align: center;">Título Valor </td>
                    <td style="text-align: center;">Valor Atual</td>
                    <td style="text-align: center;">Tx Dev. até a Data</td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptResultadoExtrato" onitemdatabound="rptResultadoExtrato_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 6em; text-align: center;"><asp:LinkButton runat="server" ID="lnkTituloNome" OnClick="lnkTituloNome_Click" OnClientClick="btnTesouroCompra_Click(this, 'Extrato')"></asp:LinkButton></td>
                            <td style="width: auto; text-align: center;"><%# Eval("DataVencimento") != "" ? DateTime.Parse(Eval("DataVencimento").ToString()).ToString("dd/MM/yyyy") : "-"%></td>
                            <td style="width: auto; text-align: right;"><%# Eval("QuantidadeCredito") != "" ? Eval("QuantidadeCredito") : "-"%></td>
                            <td style="width: auto; text-align: right;"><%# Eval("QuantidadeDebito") != "" ? Eval("QuantidadeDebito") : "-"%></td>
                            <td style="width: auto; text-align: right;"><%# Eval("QuantidadeBloqueada") != "" ? Eval("QuantidadeBloqueada").ToString() : "-"%></td>
                            <td style="width: auto; text-align: right;"><%# Eval("SaldoAnterior") != "" ? Eval("SaldoAnterior") : "-"%></td>
                            <td style="width: auto; text-align: right;"><%# Eval("SaldoAtual") %></td>
                            <td style="width: auto; text-align: right;"><%# Eval("TituloValor") %></td>
                            <td style="width: auto; text-align: right;"><%# Eval("ValorAtual") %></td>
                            <td style="width: auto; text-align: center;"><%# Eval("ValorTaxaDevida") != "" ? Eval("ValorTaxaDevida") : "-"%></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Literal ID="litNenhumRegistroEncontrado" runat="server" Visible="false">
                    <tr>
                        <td colspan="10" style="text-align: center;">Nenhum registro encontrado</td>
                    </tr>
                </asp:Literal>

            </tbody>
        </table>

    </asp:Panel>

    <asp:Panel ID="pnlResultadoExtratoDetalhes" Visible="false" runat="server" cssClass="form-consulta-tesouro form-padrao clear">
    <div class="row">
        <div class="col2">
        <label></label>
        <label>Detalhes do Título</label>
        </div>
    </div>

        <table class="tabela" style="margin-bottom:20px" >
            <thead>
                <tr>
                    <td style="text-align: center; width: 6em;">Título</td>
                    <td style="text-align: center; width: auto;">Data da Compra</td>
                    <td style="text-align: center; width: auto;">Valor Título</td>
                    <td style="text-align: center; width: auto;">Taxa CBLC</td>
                    <td style="text-align: center; width: auto;">Taxa AC</td>
                    <td style="text-align: center; width: auto;">Qde. Bloqueada</td>
                    <td style="text-align: center; width: auto;">Qde. Compra</td>
                    <td style="text-align: center; width: auto;">Qde. Crédito</td>
                    <td style="text-align: center; width: auto;">Qde. Débito</td>
                </tr>            
            </thead>
            <tbody>
                <asp:repeater ID="rptResultadoExtratoDetalhes" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: center;"><%# Eval("NomeTitulo") %></td>
                            <td style="text-align: center;"><%# Eval("DataCompra") %></td>
                            <td style="text-align: right;"> <%# Eval("ValorTitulo") %></td>
                            <td style="text-align: center;"><%# Eval("ValorTaxaCBLC") %></td>
                            <td style="text-align: center;"><%# Eval("ValorTaxaAC") %></td>
                            <td style="text-align: center;"><%# Eval("QuantidadeBloqueada") %></td>
                            <td style="text-align: center;"><%# Eval("QuantidadeCompra") %></td>
                            <td style="text-align: center;"><%# Eval("QuantidadeCredito") %></td>
                            <td style="text-align: center;"><%# Eval("QuantidadeDebito") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </tbody>
        </table>
        <div class="row">
            <div class="col2">
                <asp:Button ID="btnExtrato_DetalhesBtnVoltar" runat="server" OnClick="btnExtrato_DetalhesBtnVoltar_Click" OnClientClick="btnTesouroCompra_Click(this, 'Extrato')" Text="Voltar" CssClass="botao btn-padrao btn-erica" />
            </div>
        </div>

    </asp:Panel>
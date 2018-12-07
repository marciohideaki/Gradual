<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultarProtocolo.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.ConsultarProtocolo" %>
<div class="form-consulta-tesouro form-padrao clear">
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Nº do Protocolo:</label>
                <asp:TextBox ID="txtProtocolo_Numero" runat="server" />
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta ">
                <label>Mercado:</label>
                <asp:TextBox ID="txtProtocolo_Mercado" runat="server" />
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta ">
                <label>Data:</label>
                <asp:TextBox ID="txtProtocolo_DataTransacao" runat="server" CssClass="calendario" />
            </div>
        </div>
    </div>
                                                    
    <div class="row">
        <div class="col3">
            <div class="campo-consulta ">
                <div class="lista-checkbox">
                    <label class="checkbox"><input type="checkbox" id="chkProtocolo_Tipo_Compra" runat="server" /> Compra</label>
                </div>
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta ">
                <div class="lista-checkbox">
                    <label class="checkbox"><input type="checkbox" id="chkProtocolo_Tipo_Venda" runat="server" /> Venda</label>
                </div>
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta ">
                <label>Situação:</label>
                <asp:DropDownList ID="cboProtocolo_Situacao" runat="server" >
                    <asp:ListItem Value="-">Selecione</asp:ListItem>
                    <asp:ListItem Value="1">Em Liquidação</asp:ListItem>
                    <asp:ListItem Value="2">Liquidado</asp:ListItem>
                    <asp:ListItem Value="3">Cancelado</asp:ListItem>
                    <asp:ListItem Value="4">Pendente</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
                                                    
    <div class="row">
        <div class="col1">
            <asp:Button ID="btnConsultarCompra" runat="server" text="Consultar" cssclass="botao btn-padrao btn-erica" OnClick="btnConsultarCompra_Click" OnClientClick="btnTesouroCompra_Click(this, 'Protocolo')" />
        </div>
    </div>
</div>

<asp:Panel runat="server" ID="pnlConsultarProtocolo" Visible="false">

        <table class="tabela" style="margin-bottom:20px">
            <thead>
                <tr>
                    <td style="width: 6em; text-align: center;">Cód. Cesta</td>
                    <td style="width: 4em; text-align: center;">Tipo</td>
                    <td style="width: 3em; text-align: center;">Merc.</td>
                    <td style="width: 5em; text-align: center;">Situação</td>
                    <td style="width: 6em; text-align: center;">Título</td>
                    <td style="width: 4em; text-align: center;">Qde.</td>
                    <td style="width: 9em; text-align: center;">Valor da Compra</td>
                    <td style="width: 6em; text-align: center;">Taxa CBLC</td>
                    <td style="width: 5em; text-align: center;">Taxa Ag. de Cust.</td>
                    <td style="width: 7em; text-align: center;">Valor após pgto taxas</td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptConsultarProtocolo" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: left;"><%# Eval("CodigoCesta")%></td>
                            <td style="text-align: left;"> <%# Eval("TipoCesta")%></td>
                            <td style="text-align: center;"><%# Eval("Mercado")%></td>
                            <td style="text-align: left;"><%# Eval("Situacao")%></td>
                            <td style="text-align: left;"><%# Eval("NomeTitulo")%></td>
                            <td style="text-align: right;"><%# Eval("QuantidadeCompra")%></td>
                            <td style="text-align: right;"><%# Eval("ValorTitulo")%></td>
                            <td style="text-align: right;"><%# Eval("ValorTaxaCBLC")%></td>
                            <td style="text-align: right;"><%# Eval("ValorTaxaAC")%></td>
                            <td style="text-align: right;"><%# Eval("ValorAposPgtoTaxas")%></td>
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
    <script>
        $('input[type="checkbox"], input[type="radio"]').iCheck();
    </script>
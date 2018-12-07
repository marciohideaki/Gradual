<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultaTD.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.ConsultaTD" %>
<div class="form-consulta-tesouro form-padrao clear">
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Tipo de Título:</label>
                <asp:DropDownList ID="cboTipoTitulo" runat="server">
                </asp:DropDownList>
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta">
                <label>Vencimento:</label>
                <asp:TextBox ID="txtVencimento" runat="server" CssClass="calendario" MaxLength="10" />
                
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta">
                <label>Indexadores:</label>
                <asp:DropDownList ID="cboIndexadores" runat="server" />
            </div>
        </div>
    </div>
                                                    
    <div class="row" style="margin-bottom:20px">
        <div class="col1">
            <asp:Button id="btnConsultar" runat="server" cssclass="botao btn-padrao btn-erica" Text="Consultar" OnClick="btnConsultar_Click" OnClientClick="btnTesouroCompra_Click(this, 'Consulta')"  />
        </div>
    </div>
</div>

<div id="divResultado" runat="server" visible="false">
    <table class="tabela">
        <thead>
            <tr>
                <td style="text-align: center;">Tipo</td>
                <td style="text-align: center; width: 14em;">Título</td>
                <td style="text-align: center; width: 7em;">Vencimento</td>
                <td style="text-align: center; width: 5em;">Indexador</td>
                <td style="text-align: center; width: 8em;">Taxa de Juros (% ao ano)</td>
                <td style="text-align: center; width: 9em;">Preço de 1 Título</td>
                <td style="text-align: center; width: 8em;">&nbsp;</td>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater runat="server" ID="rptMinhaConta_TesouroDireto_Consulta_Resultado" onitemdatabound="rptMinhaConta_TesouroDireto_Consulta_Resultado_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td style="text-align: center;"><%# Eval("TipoNome")%></td>
                        <td style="text-align: left;"><%# Eval("NomeTitulo") %></td>
                        <td style="text-align: center;"><%# Eval("Vencimento")%></td>
                        <td style="text-align: left;"><%# Eval("IndexadorNome")%></td>
                        <td style="text-align: right;"><%# Eval("ValorTaxaCompra") + "%"%></td>
                        <td style="text-align: right;"><%# Eval("ValorCompra")%></td>
                        <td style="text-align: center;">
                        <asp:Button runat="server" ID="btnMinhaConta_TesouroDireto_Consulta_Resultado_DescricaoTitulo" Text="Descrição" OnClick="btnMinhaConta_TesouroDireto_Consulta_Resultado_DescricaoTitulo_Click"  cssclass="botao btn-invista" style="width:86px" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Literal ID="litNenhumRegistroEncontrado" runat="server" Visible="false">
                <tr>
                    <td colspan="7" style="text-align: center;">Nenhum registro encontrado</td>
                </tr>
            </asp:Literal>
        </tbody>
    </table>
</div>

<div class="PainelBusca" id="pnlDescricaoTitulo" runat="server" visible="false" style="height:18em">
    <p style="width:49%">
        <label>Tipo:</label>
        <asp:Label ID="lblConsulta_DetalhesTipo" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>Nome:</label>
        <asp:Label ID="lblConsulta_DetalhesTitulo" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>Data Emissão:</label>
        <asp:Label ID="lblConsulta_DetalhesDataEmissao" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>Vencimento:</label>
        <asp:Label ID="lblConsulta_DetalhesVencimento" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>Indexador:</label>
        <asp:Label ID="lblConsulta_DetalhesIndexador" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>ISIN:</label>
        <asp:Label ID="lblConsulta_DetalhesISIN" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>*Taxa Ag. Custódia:</label>
        <asp:Label ID="lblConsulta_DetalhesTaxaAC" runat="server"></asp:Label>
    </p>
    <p style="width:49%">
        <label>*Taxa CBLC:</label>
        <asp:Label ID="lblConsulta_DetalhesTaxaCBLC" runat="server"></asp:Label>
    </p>

    <p style="width:98%;text-align:center; padding:1em">
        <asp:button id="Consulta_DetalhesBtnVoltar" runat="server" OnClick="Consulta_DetalhesBtnVoltar_Click" cssclass="botao btn-padrao btn-erica" text="Voltar" style="color:#fff;width:8em;float:none" />
    </p>
</div>

<p id="lblAvisoTitulos" runat="server" visible="false">
        *Para adquirir um dos títulos desta lista, escolha a opção "Compra" do menu. 
    </p>
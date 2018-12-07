<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompraTD.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.CompraTD" %>

<asp:panel id="pnlTesouroDiretoBusca" runat="server" onload="pnlTesouroDiretoBusca_Load" CssClass="form-consulta-tesouro form-padrao">
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Tipo de Título:</label>
                <asp:DropDownList ID="cboTipoDeTitulo" datatextfield="Nome" datavaluefield="Codigo" runat="server" />
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta">
                <label>Vencimento:</label>
                <asp:TextBox ID="txtConsultaVencimento" runat="server" class="calendario" />
            </div>
        </div>
                                                        
        <div class="col3">
            <div class="campo-consulta">
                <label>Indexadores:</label>
                <asp:DropDownList ID="cboIndexadores" datatextfield="Nome" datavaluefield="Codigo" runat="server" />
            </div>
        </div>
    </div>
                                                    
    <div class="row">
        <div class="col1">
            <asp:Button CssClass="botao btn-padrao btn-erica" runat="server" id="btnConsultar" Text="Consultar" OnClick="btnConsultar_Click" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" />
            <asp:button id="Compra_BtnVerCesta" runat="server" onclick="Compra_BtnVerCesta_Click" text="Minha Cesta" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" style="width:110px"/>
            
        </div>
    </div>
</asp:panel>

                               
<asp:Panel id="pnlTesouroDiretoCompra" runat="server" Visible="false" CssClass="row">
    <table class="tabela">
        <tr class="tabela-titulo tabela-type-small">
            <td>Tipo</td>
            <td>Título</td>
            <td>Vencimento</td>
            <td>Indexador</td>
            <td>Juros (% ao ano)</td>
            <td>Valor (1 Título)</td>
            <td></td>
        </tr>
        <asp:Repeater runat="server" ID="rptTesouroDiretoCompraResultado" onitemdatabound="rptTesouroDiretoCompraResultado_ItemDataBound">
        <ItemTemplate>        
        <tr class="tabela-type-small">
            <td><%# Eval("TipoNome")%>       </td>
            <td><%# Eval("TituloNome")%>     </td>
            <td><%# Eval("DataVencimento")%> </td>
            <td><%# Eval("IndiceNome")%>     </td>
            <td><%# Eval("ValorTaxaCompra")%></td>
            <td><%# Eval("ValorCompra")%>    </td>
            <td class="alignCenter">
            <asp:Button runat="server" ID="btnTesouroDiretoCompra" Text="Investir" OnClick="btnTesouroDiretoCompra_Click"  CssClass="botao btn-invista" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" />
            <%--<a class="botao btn-invista" href="#">Investir</a>--%>
            </td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
        <tr id="trNenhumTesouroCompra" runat="server" visible="false">
            <td colspan="7">Nenhum item encontrado</td>
        </tr>            
    </table>
</asp:Panel>

<asp:Panel id="pnlTesouroDiretoCompraBoleta" runat="server" Visible="false"  CssClass="form-consulta-tesouro form-padrao clear">
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Titulo</label>
                <asp:Label ID="lblBoletaTitulo" runat="server" CssClass="campo-basico"></asp:Label>
            </div>
       </div>
   
        <div class="col3">
            <div class="campo-consulta">
                <label>Vencimento</label>
                <asp:Label ID="lblBoletaDataVencimento" runat="server" CssClass="campo-basico"></asp:Label>
            </div>
       </div>
   
        <div class="col3">
            <div class="campo-consulta">
                <label>Preço</label>
                <asp:Label ID="lblBoletaPrecoUnitario" runat="server" CssClass="campo-basico"></asp:Label>
            </div>
       </div>
    </div>

    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Quantidade</label>
                <asp:TextBox ID="txtBoletaQuantidade" runat="server" class="ValorMonetario ProibirLetras"></asp:TextBox>
            </div>
       </div>
       <div class="col3">
            <div class="campo-consulta">
                <label></label>
                <asp:button id="btnCalcularTotalDaBoleta" runat="server" OnClick="btnCalcularTotalDaBoleta_Click" cssclass="botao btn-invista" text="Calcular Total" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" style="width:200px" />
            </div>
       </div>
       </div>
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Valor da Compra</label>
                <asp:TextBox ID="txtBoletaValorCompra" runat="server" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Taxa CBLC</label>
                <asp:TextBox ID="txtBoletaTaxaCBLC" runat="server" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Taxa Ag. Custódia</label>
                <asp:TextBox ID="txtBoletaTaxaAgCustodia" runat="server" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col3">
            <div class="campo-consulta">
                <label>Valor Total</label>
                <asp:TextBox ID="txtBoletaValorTotal" runat="server" onkeypress="reais(this,event)" class="ValorMonetario ProibirLetras"></asp:TextBox>
            </div>
        </div>

        <div class="col3">
            <div class="campo-consulta">
                <label></label>
                <asp:Button id="btnBoletaCalcularQuantidade" runat="server" onclick="btnBoletaCalcularQuantidade_Click" cssclass="botao btn-invista" text="Calcular Qtd." OnClientClick="btnTesouroCompra_Click(this, 'Compra')" style="width:200px" />
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col2">
            <div class="campo-consulta-tesouro">
            <asp:button id="btnIncluirTituloNaCesta" runat="server" onclick="btnIncluirTituloNaCesta_Click" cssclass="botao btn-invista" text="Incluir na Cesta" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" style="width:200px" />
            <asp:button id="btnVoltar" runat="server" OnClick="btnVoltar_Click" text="Voltar" cssclass="botao btn-invista" OnClientClick="btnTesouroCompra_Click(this, 'Compra')"  />
            </div>
        </div>
        <%--<div class="col2">
            <div class="campo-consulta-tesouro">
            
            </div>
        </div>--%>
    </div>
    </asp:Panel>

    <asp:Panel id="pnlTesouroDiretoCompraCesta" runat="server" Visible="false">

        <table style="width:86%">
            <thead>
                <tr>
                    <td style="width: auto; text-align: center;">Título                         </td>
                    <td style="width: auto; text-align: center;">Vencimento                     </td>
                    <td style="width: auto; text-align: center;">Qde. Compra                    </td>
                    <td style="width: auto; text-align: center;">Valor Compra                   </td>
                    <td style="width: auto; text-align: center;">Valor CBLC                     </td>
                    <td style="width: auto; text-align: center;">Valor AC                       </td>
                    <td style="width: auto; text-align: center;">Valor Após Pgto de Taxas       </td>
                    <td style="width: auto; text-align: center;">&nbsp;                         </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptTesouroDiretoCompraCesta" OnItemDataBound="rptTesouroDiretoCompraCesta_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: center;"><%# Eval("NomeTitulo") %>           </td>
                            <td style="text-align: center;"><%# Eval("DataVencimento") %>       </td>
                            <td style="text-align: center;"><%# Eval("QuantidadeCompra") %>     </td>
                            <td style="text-align: center;"><%# Eval("ValorCompra") %>          </td>
                            <td style="text-align: center;"><%# Eval("ValorCBLC") %>            </td>
                            <td style="text-align: center;"><%# Eval("ValorAC") %>              </td>
                            <td style="text-align: center;"><%# Eval("ValorAposPgtoTaxas") %>   </td>
                            <td style="text-align: center;"><asp:Button runat="server" ID="btnTesouroDiretoCompraCestaExcluir" OnClick="btnTesouroDiretoCompraCestaExcluir_Click" style="cursor: pointer;" Text="Excluir" cssclass="botao btn-invista" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="litNenhumRegistroEncontradoCesta" runat="server" Visible="false">
                    <tr>
                        <td colspan="8" style="text-align: center;">Nenhum registro encontrado</td>
                    </tr>
                </asp:Literal>
            </tbody>
        </table>

        <%--<fieldset class="FormularioPadrao" style="margin: 2em 0em 2em 5.2em; width: 55.7%;">--%>
        <div class="col" style="width:312px; margin-top:20px">
            <div class="form-padrao">
                <div class="campo-basico campo-senha">
                    <label style="width:34.8%">Ass. Eletrônica:</label>
                    <input type="password" id="txtAssinaturaEletronica" runat="server" class="teclado-dinamico" onclick="javascript:return showKeyboard($('#ContentPlaceHolder1_CompraTD1_btnCesta_ConfirmarCompra'), event, { Controle: '#ContentPlaceHolder1_CompraTD1_txtAssinaturaEletronica', Mensagem: 'Assinatura' } );"/>
                    <button class="teclado-virtual" type="button"><img alt="Teclado Virtual" src="../../Resc/Skin/Default/Img/teclado.png" /></button>
                    <asp:button id="btnCesta_ConfirmarCompra" runat="server" onclick="btnCesta_ConfirmarCompra_Click"  OnClientClick="return btnGenerico_Validar(this);btnTesouroCompra_Click(this, 'Compra');" Text="Confirmar" cssclass="botao btn-padrao btn-erica"  />
                </div>
            </div>
        </div>
        <%--</fieldset>--%>

        <p style="text-align:center; padding:1em;">

            <asp:button id="btnCesta_Desistir" runat="server" onclick="btnCesta_Desistir_Click" text="Desistir" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Compra')" />

            &nbsp;&nbsp;
            <asp:button id="btnCesta_Voltar" runat="server" onclick="btnCesta_Voltar_Click" text="Voltar" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Compra')"/>
        </p>

    </asp:Panel>
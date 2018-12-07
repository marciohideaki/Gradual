<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PainelDeCarrinho.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.PainelDeCarrinho" %>

<div id="pnlCarrinhoOverlay" style="display:none">

    <div id="pnlCarrinho" class="pnlCarrinho" runat="server">

        <h3></h3>

        <a href="#" onclick="return GradSite_FecharCarrinho(false)" class="btnFecharPainel" title="Fechar" style="margin-top:-14px;">&nbsp;</a>

        <div class="row">

            <table class="tabela">

                <thead>
                    <tr>
                        <td>Produto</td>
                        <td>Preço Un.</td>
                        <td>Qtd.</td>
                        <td>Subtotal</td>
                        <td>Taxas</td>
                        <td colspan="2">Total</td>
                    </tr>
                </thead>
                <tfoot style="display:none">
                    <tr>
                        <td colspan="5" style="text-align:right;">Total:&nbsp;</td>
                        <td colspan="2">10,50</td>
                    </tr>
                </tfoot>
                <tbody id="tblProdutosCarrinho">
                    <!--tr>
                        <td>Câmbio Dólar Americano (USD)</td>
                        <td>2,53</td>
                        <td>10</td>
                        <td>1</td>
                        <td>26,30</td>
                        <td> &nbsp; </td>
                    </tr-->
                    <tr class="NenhumItem">
                        <td colspan="7">Nenhum item no carrinho.</td>
                    </tr>
                </tbody>

            </table>

        </div>

        <div id="pnlAlertaEstadoIndisponivel" class="row" style="background:#fac8b4;padding:0.4em;line-height:1.4em;border-radius:6px;display:none">
            <div class="col1">
            Atenção: O estado deste endereço não está disponível para entrega, somente o(s) seguinte(s): <%= this.ListaDeEstadosLegivel %><br />
            Favor escolher outro endereço para entrega.
            </div>
        </div>
        
        <div id="pnlCarrinho_NomeEmail" runat="server" class="row form-padrao SubForm_Dados">
            <div class="col3">
                <div class="campo-basico">
                    <label>Nome:</label>
                    <input  id="txtCarrinho_NomeCliente" type="text" title="Nome"  maxlength="150" class="validate[required]" />
                </div>
            </div>
            <div class="col3">
                <div class="campo-basico">
                    <label>Email:</label>
                    <input  id="txtCarrinho_EmailCliente" type="text" maxlength="150" class="validate[required,custom[EmailGambizento]]" />
                </div>
            </div>
        </div>

        <div class="row form-padrao SubForm_Telefones">
            <div class="col3" style="width:80px">
                <div class="campo-basico">
                    <label style="width:auto; white-space: nowrap">Celular (DDD):</label>
                    <div class="clear ContainerPequeno">

                        <input  id="txtCarrinho_Cel_DDD" runat="server" type="text" title="DDD"  maxlength="2" onblur="txtCelular_DDD_Blur(this)" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras" style="width:72px;"  />

                    </div>
                </div>
            </div>
            <div class="col3" style="width:220px">
                <div class="campo-basico">
                    <label>Celular (Número):</label>
                    <div class="clear ContainerMedio">

                        <input  id="txtCarrinho_Cel_Numero" runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone Mascara_CEL ProibirLetras validate[required]" style="width:208px" />

                    </div>
                </div>
            </div>
            <div class="col3" style="width:80px">
                <div class="campo-basico">
                    <label>Tel (DDD):</label>
                    <div class="clear ContainerPequeno">

                        <input  id="txtCarrinho_Tel_DDD" runat="server" type="text" title="DDD"  maxlength="2" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras " style="width:72px;" />

                    </div>
                </div>
            </div>
            <div class="col3" style="width:208px">
                <div class="campo-basico">
                    <label>Tel (Número):</label>
                    <div class="clear ContainerMedio">

                        <!--label for="txtCarrinho_Tel_Numero" style="width:0.7em;">-</label-->
                        <input  id="txtCarrinho_Tel_Numero" runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone validate[required] Mascara_TEL ProibirLetras" style="width:208px" />

                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col6">
            Endereço de entrega: 
            </div>
            <div class="col2" style="line-height: 2em;vertical-align:top;margin-top: -0.4em">
                <label data-EnderecoDaContaPermitido="<%= this.EnderecoDaContaPermitido.ToString().ToLower() %>">
                    <input type="radio" id="rdoCarrinho_EndEntrega_Conta" name="rdoCarrinho_EndEntrega" checked="checked" onclick="return rdoCarrinho_EndEntrega_Click(this)" />
                    <asp:literal id="lblEnderecoDaConta" runat="server"></asp:literal>
                </label>
                <br />
                <label>
                    <input type="radio" id="rdoCarrinho_EndEntrega_Outro" name="rdoCarrinho_EndEntrega" onclick="return rdoCarrinho_EndEntrega_Click(this)" />
                    Outro.
                </label>
            </div>
        </div>

        <div id="pnlCarrinho_EnderecoDeEntrega" class="form-padrao SubForm_Endereco" style="display:none">

            <div class="row">
                <div class="col6">
                    <div class="campo-basico">
                        <label>CEP:</label>
                        <input  id="txtCarrinho_EndEntrega_CEP" type="text" maxlength="8" class="Mascara_CEP ProibirLetras EstiloCampoObrigatorio validate[required]" onkeyup="txtCEP_KeyUp(this, event)"  data-CEPGroup="endc" data-CEPProp="cep" title="CEP" />
                    </div>
                </div>

                <div class="col2" style="width:530px">
                    <div class="campo-basico">
                        <label>Logradouro:</label>
                        <input  id="txtCarrinho_EndEntrega_Logradouro" type="text" data-CaixaAlta="true" maxlength="30" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="endc" data-CEPProp="logradouro" title="Logradouro" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col6">
                    <div class="campo-basico">
                        <label>Número:</label>
                        <input  id="txtCarrinho_EndEntrega_Numero" type="text" maxlength="5" class="EstiloCampoObrigatorio validate[required]" title="Número" />
                    </div>
                </div>

                <div class="col6">
                    <div class="campo-basico">
                        <label>Complemento:</label>
                        <input  id="txtCarrinho_EndEntrega_Complemento" type="text" maxlength="10" title="Complemento" />
                    </div>
                </div>

                <div class="col3" style="width:365px">
                    <div class="campo-basico">
                        <label>Bairro:</label>
                        <input  id="txtCarrinho_EndEntrega_Bairro" type="text" data-CaixaAlta="true" maxlength="18" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="endc" data-CEPProp="bairro" title="Bairro" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3" style="width:318px">
                    <div class="campo-basico">
                        <label>Cidade:</label>
                        <input  id="txtCarrinho_EndEntrega_Cidade" type="text" data-CaixaAlta="true" maxlength="28" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="endc" data-CEPProp="cidade" title="Cidade" />
                    </div>
                </div>

                <div class="col3" style="width:370px">
                    <div class="campo-basico">
                        <label>Estado:</label>
                        <select id="cboCarrinho_EndEntrega_Estado" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="endc" data-CEPProp="uf_sigla">
                        <asp:repeater id="rptEndEntrega_Estado" runat="server">
                        <itemtemplate>
                            <option value="<%# Eval("Id") %>"><%# Eval("Value") %></option>
                        </itemtemplate>
                        </asp:repeater>
                        </select>

                        <input type="hidden" id="hidEstadosPermitidosParaEntrega" data-CEPGroupStateRestriction="endc" value='<%= this.ListaDeEstados %>' />

                    </div>
                </div>

                <!--div class="col3">
                    <div class="campo-basico">
                        <label>País:</label>
                        <select id="cboCarrinho_EndEntrega_Pais" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="endc" data-CEPProp="pais">
                        <asp:repeater id="rptEndEntrega_Pais" runat="server">
                        <itemtemplate>
                            <option value="<%= Eval("Id") %>"><%= Eval("Value") %></option>
                        </itemtemplate>
                        </asp:repeater>
                        </select>
                    </div>
                </div-->
            </div>

        </div>

        <div class="row">

            <div class="col3">
                <button class="botao btn-padrao" onclick="return GradSite_FecharCarrinho(false)">Continuar Comprando</button>
            </div>

            <div class="col3" style="float:right;margin-right:5px">
                <button class="botao btn-padrao" onclick="return GradSite_FinalizarCarrinho(this)">Finalizar Compra</button>
            </div>
        </div>

    </div>

</div>

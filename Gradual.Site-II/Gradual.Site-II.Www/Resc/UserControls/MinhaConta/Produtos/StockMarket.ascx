<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockMarket.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Produtos.StockMarket" %>

<div class="row">
    <div class="col3-3">
        <div class="form-padrao">

        <div style="width:540px">

            <h5>O que é?</h5>
            <div class="row">
                <div class="col1">
                    <p>Você ainda não possui o Gradual Stock Market?</p>
                    <p>Adquira já a tecnologia do Home Broker na sua planilha de Excel e facilite ainda mais a sua tomada de decisão.</p>
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    <p>Confira as principais vantagens da ferramenta e aproveite! </p>
                    <ul style="list-style-type:circle; padding-left:25px">
                        <li>Cotações Bovespa e BM&F;</li>
                        <li>Facilidade nos cálculos e acompanhamento do mercado;</li>
                        <li>Livro de Oferta, ticker e carteira à sua disposição;</li>
                        <li>Importação de dados salvos no Home Broker para sua grid de cotações;</li>
                        <li>Personalização das janelas conforme sua preferência.</li>
                    </ul>

                    <h5>Quanto custa?</h5>

                    <h6 class="Preco">
            
                    <!--
                     <asp:literal id="lblPrecoDoProduto2" runat="server"></asp:literal> 
                     -->
                     </h6>

                    <p>
                    O Stock Market está com uma super promoção na mensalidade: R$ 20,00. Não perca essa oportunidade!
                    </p>

                    <p>
                        O custo será debitado mensalmente da sua Conta Corrente na Gradual.
                    </p>

                </div>
            </div>

            <div class="row">
                <div class="col1">
                    <div class="form-padrao"  id="pnlDadosDeCompra_RealizarCompra" runat="server" visible="false">

                        <div class="FormularioPadrao">
                            <div class="row" style="height:30px">
                                <div class="col1">
                                   <label class="checkbox" style="padding-left:0px"> <input  id="chkTermoLido" type="checkbox" onclick="chkTermoLido_Click(this)" />Li e concordo com os <a href="../../Resc/PDFs/Termo Adesao StockMarket.pdf" target="_blank">Termos de Serviço</a></label>
                       
                                </div>
                            </div>
                
                            <div class="row">
                                <div class="col2">
                                    <div class="form-padrao">
                                        <div class="campo-basico campo-senha">
                                            <label >Ass. Eletrônica:</label>
                                            <input  id="txtSenhaEletronica" type="password" value="" name="input-padrao" class="mostrar-teclado"  />
                                            <button class="teclado-virtual" type="button" style="margin-left:195px"> <img alt="Teclado Virtual" src="../../Resc/Skin/Default/Img/teclado.png"></button>
                                        </div>
                                        <small class="esqueceu-assinatura"><a href="../../MinhaConta/Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
                                    </div>
                                </div>
                            </div>
                
                            <div class="row">
                                <div class="col1">
                                    <input type="hidden" id="hidIdDoProduto" value="<%= this.IdDoProduto %>" />
                                    <button class="botao btn-contratar" disabled="disabled" type="button" id="btnIniciarCompraStockMarket" onclick="return btnIniciarCompraStockMarket_Click(this)" style="opacity:0; padding:2px 12px">Contratar</button>
                                    <%--<button id="btnIniciarCompra" class="BotaoVerde" onclick="return btnIniciarCompra_Click(this)" disabled="disabled" style="opacity:0; padding:2px 12px">Continuar &gt;</button>--%>
                                </div>
                            </div>

                        </div>

                        <div class="MensagemDeErro" style="display:none">

                            <p>Assinatura Eletrônica não confere, favor tentar novamente.</p>
                            <p>
                                <button class="botao btn-contratar" onclick="return btnMensagemDeErroOk_Click(this)">Ok</button>
                            </p>

                        </div>

                    </div>
                </div>
            </div>

        </div>

        <div id="pnlDadosDeCompra_AguardandoPagamento" runat="server" visible="false">

            <h5>Você já iniciou a compra desse produto!</h5>

            <p>
                Estamos aguardando a confirmação de pagamento.
            </p>

        </div>

        <div id="pnlDadosDeCompra_ProdutoJaAdquirido" runat="server" visible="false">

            <h5>Você já tem esse produto!</h5>

            <div class="instalador">
                <p>
                    <!--button class="BotaoInstalador" onclick="return btnBaixarAtualizador_Click(this)"><span>Instalador Gradual</span></button-->
                    <a href="http://atualizador.gradualinvestimentos.com.br:8712" class="botao btn-download"><span>Instalador Gradual</span></a>
                </p>
                <p style="line-height:1.8em">
                    para instalar e manter atualizadas as ferramentas de software que você adquiriu
                </p>

                <h5>Pré-requisitos para a instalação:</h5>
                <ul class="PreRequisito">
                    <li><a href="http://www.microsoft.com/pt-br/download/details.aspx?id=17851" class="botao btn-download">FRAMEWORK .NET4</a>
                        <br />
                        <small>(Já vem instalado com Windows Vista 7 e 8)</small>
                    </li>
                </ul>
            </div>
        </div>



        </div>
    </div>
</div>

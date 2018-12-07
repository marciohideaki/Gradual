<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GTI.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Produtos.GTI" %>
<div class="row">
    <div class="col3-3">
        <div class="form-padrao">

            <div style="width:540px">
                
                <h5>O que é?</h5>
                <p>
                Gradual Trader Interface (GTI): a plataforma de negociação profissional, desenvolvida para gerar segurança, alta performance e interatividade para suas operações. 
                </p>
                <p>
                O GTI permite:
                </p>
                <ul style="list-style-type:circle;  margin-left:25px; line-height:20px">
                    <li>Operações de alavancagem no intraday</li>
                    <li>Estudos gráficos</li>
                    <li>Personalização de tela e muito mais!</li>
                </ul>

                <h5>Quanto custa?</h5>

                <p>O GTI está com uma super promoção na mensalidade: R$ 100,00 ou isenção de pagamento para investidor que gerar a partir de R$ 1.000,00 de corretagem.
                 Não perca essa oportunidade! </p>
                <p>
                    O custo será debitado mensalmente da sua Conta Corrente na Gradual.
                </p>

                <div class="FormularioPadrao" id="pnlDadosDeCompra_RealizarCompra" runat="server" visible="false">
                    <div class="row" style="height:30px">
                        <div class="col1">
                            <label></label>
                            <label class="checkbox" style="padding-left:0px"> <input  id="chkTermoLido" type="checkbox" onclick="chkTermoLidoGTI_Click(this)" />Li e concordo com os <a href="../../Resc/PDFs/TERMO-DE-ADESÃO-DE-TRADER-INTERFACE_0213-n.pdf" target="_blank">Termos de Serviço</a></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col2">
                            <div class="form-padrao">
                                <div class="campo-basico campo-senha">
                                    <label >Ass. Eletrônica:</label>
                                    <input  id="txtSenhaEletronicaGTI" type="password" maxlength="15" class="mostrar-teclado"  />
                                    <button class="teclado-virtual-produtos" type="button" style="margin-left:195px;"> <img alt="Teclado Virtual" src="../../Resc/Skin/Default/Img/teclado.png"></button>
                                </div>
                                <small class="esqueceu-assinatura"><a href="../../MinhaConta/Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col3-2">
                            <input type="hidden" id="hidIdDoProdutoGTI" value="<%= this.IdDoProduto %>" />
                            <button class="botao btn-contratar" disabled="disabled" type="button" id="btnIniciarCompraGTI" onclick="return btnIniciarCompraGTI_Click(this)">Contratar</button>
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
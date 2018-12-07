<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelCard.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Produtos.TravelCard" %>
<h5>O que é?</h5>
<p>Com o Gradual Travel Card você garante conforto, segurança e comodidade às suas viagens. </p>

<p>Representado pela bandeira Mastecard, você pode realizar compras e saques em moeda estrangeira (dólar, euro e libra) e brasileira (real) em mais de 2 milhões de caixas eletrônicos e 32 milhões de estabelecimentos credenciados em todo mundo. Para utilizá-lo, basta carregá-lo com a moeda desejada.</p>                                                

<p>Conheça as principais vantagens do cartão e aproveite!</p>

<ul style="list-style-type:circle;  margin-left:25px">
<li>Seguro por CHIP e senha eletrônica;                                             </li>
<li>Diminui a necessidade de carregar alta quantia em espécie;                      </li>
<li>Não possui variação cambial;                                                    </li>
<li>Acesso online ao extrato e demais informações;                                  </li>
<li>Cartão adicional;                                                               </li>
<li>Recarregável e sem taxa de anuidade;                                            </li>
<li>Habilitado para uso de compras pela internet;                                   </li>
<li>Variedade de locais onde é aceito;                                              </li>
<li>O mesmo saldo pode ser usado em outro cartão;                                   </li>
<li>O saldo remanescente pode ser usado em outro cartão ou viagens posteriores;     </li>
<li>Atendimento especializado 24 horas.                                             </li>
</ul>

<p>
*Alguns operadores de caixas eletrônicos e estabelecimentos podem cobrar uma tarifa e estabelecer seus próprios limites para saque ou compra. Confirme se há tarifas ou limites adicionais antes de fazer saques ou compras.

</p>                                                
<p>
**Tarifa aplicada a cada transferência de fundos entre as moedas disponíveis no cartão (sujeita a uma taxa de câmbio, conforme determinado por nós).
</p>

<p>
    Para aderir ao Travel Card, entre em contato com nosso Atendimento: <br />
    4007-1873 | Capitais e Regiões Metropolitanas<br />
    0800 655 1873 | Demais Localidades
</p>


<div id="pnlDadosDeCompra_SemLogin" runat="server">

    <h5>Adquira já o seu Gradual Travel Card!</h5>

    <p>
        Para poder adquirir esse produto, <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/CadastroPF.aspx">cadastre-se</a> ou <a href="<%= this.HostERaizHttps %>/MinhaConta/Login.aspx?r=~/Produtos/TravelCard.aspx">faça login</a>.
    </p>

</div>
<div id="pnlDadosDeCompra_RealizarCompra" runat="server">

    <h5>Adquira Agora o Gradual Travel Card</h5>

    <div class="FormularioPadrao">

        <p >
            <input type="hidden" id="hidIdDoProduto" value="<%= this.IdDoProduto %>" />

            <button id="btnIniciarCompra" class="botao btn-contratar" onclick="return btnIniciarCompraTravelCard_Click(this)" >Continuar &gt;</button>
        </p>

        <p class="PagSeguro">

            <span style="background:none">Você será redirecionado para o site FoxCambio para efetuar o pagamento e depois de recebermos a confirmação, o produto estará disponível.</span>

        </p>

    </div>

    <div class="MensagemDeProgresso" style="display:none">

        <p> Iniciando, favor aguardar um momento... </p>

    </div>

    <div class="MensagemDeErro" style="display:none">

        <p>Assinatura Eletrônica não confere, favor tentar novamente.</p>
        <p>
            <button class="botao btn-contratar" onclick="return btnMensagemDeErroOk_Click(this)">Ok</button>
        </p>

    </div>

    <pre id="hidQueryString" style="display:none"><asp:literal id="hidQueryString" runat="server"></asp:literal></pre>

</div>
<div id="pnlDadosDeCompra_SemSaldo" runat="server">

    <h5>Sem saldo para aquisição</h5>

    <p>
        Para poder adquirir esse produto, você precisa estar com saldo em D0; Saldo atual: R$ <asp:literal id="lblSaldo" runat="server"></asp:literal>
    </p>

</div>
<div id="pnlDadosDeCompra_ProdutoJaAdquirido" runat="server">

    <h1>Você já tem esse produto!</h1>

    <p>
        Para recarregar o seu cartão, <a href="http://www.foxcambio.com.br/ext/cadastro2011.asp?master=GRADUAL&CPF=<%= this.CpfDoUsuario %>&Nome=<%= this.NomeDoUsuario %>">clique aqui</a>.
    </p>

</div>
<%--<div class="row">
    <div class="col2">
        <div class="form-padrao">
            <div class="campo-basico campo-senha">
                <label>Assinatura Eletrônica:</label>
                <input type="password" value="" name="input-padrao" />
                <button class="teclado-virtual" type="button"><img alt="Teclado Virtual" src="../../Resc/Skin/Default/Img/teclado.png"></button>
            </div>
            <small class="esqueceu-assinatura"><a href="../../MinhaConta/Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
        </div>
    </div>
</div>
                                                
<button class="botao btn-contratar" type="button">Contratar</button>--%>
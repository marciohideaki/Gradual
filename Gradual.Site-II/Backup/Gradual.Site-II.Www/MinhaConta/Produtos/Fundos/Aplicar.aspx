<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Aplicar.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.Fundos.Aplicar" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="PaginaConteudo">
<h2>Aplicar</h2>
<p>
Para investir neste Fundo, preencha os campos abaixo com o valor desejado, data da aplicação e informe a sua assinatura eletrônica para concluir a operação. 
</p>
<div class="row">
<div class="col3-2">
    <div class="box quadro-cinza quadro-aplicacao">
        <div class="form-padrao">
            <h5><%=NomeFundo %></h5>

            <p><strong>Cliente:</strong> <asp:Label  ID="lblCliente" runat="server" /> </p>
                                
            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>Valor (R$)</strong></label>
                        <asp:TextBox   id="txtValorSolicitado" value="0,00" runat="server"  />
                        <small>Movimentação mínima: R$ <%=this.AplicacaoMinima %>  </small>
                    </div>
                </div>
                                    
                <div class="col4">
                    <p>Disponível: <span><asp:Label  ID="lblSaldoDisponivel" runat="server" /> </span></p>
                </div>
            </div>
             <input type="hidden" id="hddProduto" runat="server" />
            <p><strong>Aplicar:</strong></p>
            <div class="lista-radio aplicar-agendar">
                <label class="radio">           <input type="radio" name="aplicar" id="rdAplicarHoje"  runat="server"/> Hoje</label>
                <label class="radio clear">     <input type="radio" name="aplicar" id="rdAplicarAgendado" runat="server"  /> Agendar para:</label>
                <asp:TextBox ID="txtAgendarAplicacao" runat="server" CssClass="calendario voa" />
                <label class="radio clear">     <input type="radio" name="aplicar" runat="server" id="rdMensalDia" value="aplicar-mensalmente" /> Mensalmente no dia:</label>

                <asp:DropDownList id="cboDiaProgramado" runat="server" style="width:160px" >
                <asp:ListItem></asp:ListItem>
                    <asp:ListItem>Dia</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                </asp:DropDownList>
            </div>
                                
            <p><strong>Caso a data escolhida não seja útil, deseja antecipar a aplicação?</strong></p>
            <div class="lista-radio">
                <label class="radio"><input type="radio" name="data-escolhida" id="rdAntecipaAplicacaoSim" runat="server" /> Sim</label>
                <label class="radio"><input type="radio" name="data-escolhida" id="rdAntecipaAplicacaoNao" runat="server" /> Não</label>
            </div>
            <div class="row">
                <div class="col3-2">
                    <div runat="server" id= "divTermoSuitability">
                        <p style="width: 600px">
                            Atenção! o Fundo <b><%=NomeFundo %></b> não é recomendado para o seu perfil de investidor <b><%=PerfilSuitability %></b>. 
                            Para investir neste fundo seu perfil será alterado para <b><%=GetPerfilFundoCliente()%></b>.
                            Este produto prevê maior liberdade na alocação de sua carteira, podendo atuar em diversas modalidades de 
                            investimento disponíveis no mercado nacional, estando sujeito portanto, às flutuações de preços e cotações do mercado, 
                            bem como, aos riscos de crédito e liquidez, o que pode gerar perda patrimonial ao cliente, não podendo a Gradual CCTVM S/A, 
                            em hipótese alguma, excetuadas as ocorrências resultantes de comprovada culpa ou má-fé, serem responsabilizados por 
                            qualquer depreciação dos bens da carteira do cliente. Ao utilizar este produto você afirma que está ciente dos riscos 
                            associados à estratégias de operações nesta categoria de fundos e que seu perfil de investimento se assemelha 
                            ao de investidores arrojados que perseguem as melhores oportunidades de rentabilidade no longo prazo e toleram altas 
                            volatilidades e possíveis perdas patrimoniais.  Se você entende os riscos deste perfil e quer prosseguir, 
                            marque o quadro abaixo e altere seu perfil.
                        </p>
                        <label class="checkbox"><input type="checkbox" id="chkSuitability" runat="server" />&nbsp;&nbsp;Entendo os riscos envolvidos e aceito ser enquadrado no perfil <b><%=GetPerfilFundoCliente() %></b></label>  
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col1">
                    <div runat="server" id="divTermoAdesao">
                    <p>
                    Adira eletronicamente ao Termo de Adesão do fundo e envie os recursos via DOC ou TED <br />
                    para a conta da Gradual no <%=NomeFundo %> <br />
                    Banco BM&F:<br />
                    código: 096<br />
                    agência: 001<br />
                    conta corrente: 326-5<br />
                    Favorecido: Gradual CCTVM S/A<br />
                    CNPJ: 33.918.160/0001-73<br /><br />
                    
                    <asp:HyperLink  ID="lnkTermoAdesaoPF" runat="server" Target="_blank"  >Termo de Adesão PF</asp:HyperLink>&nbsp&nbsp&nbsp
                    <asp:HyperLink  ID="lnkTermoAdesaoPJ" runat="server" Target="_blank"  >Termo de Adesão PJ</asp:HyperLink>

                    </p>
                    
                        <label class="checkbox"><input type="checkbox" id="chkTermoAdesao" runat="server" />&nbsp;&nbsp; Li e aceito os Termo de Adesão, Prospecto e Lâminas</label>  
                    </div>
                </div>
                
            </div>
            <div class="row">
                <div class="col2" style="width:312px; margin-top:20px">
                    <div class="form-padrao">
                        <div class="campo-basico campo-senha">
                            <label>Assinatura Eletrônica:</label>
                            <asp:textbox id="txtAssDigital" runat="server" type="password" class="mostrar-teclado"></asp:textbox>

                            <button class="teclado-virtual" type="button"><img alt="Teclado Virtual" src="../../../Resc/Skin/Default/Img/teclado.png"></button>
                            
                        </div>
                        <small class="esqueceu-assinatura"><a href="../../Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
                    </div>
                </div>
            </div>
            
            
        </div>
    </div>
                        
    <div class="row">
        <div class="col1">
            <button type="button" ID="btnCancelar"  class= "botao btn-padrao btn-erica" onclick="javascript: window.history.go(-1);" >CANCELAR</button>
            <asp:Button ID="btnAplicar"  runat="server" cssclass= "botao btn-padrao btn-erica" Text= "APLICAR" onclick="btnAplicar_Click"  />
        </div>
    </div>
</div>
</div>
             
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Prod - Fds - Aplicar" />

</asp:Content>


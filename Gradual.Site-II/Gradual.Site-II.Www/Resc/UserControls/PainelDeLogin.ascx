<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PainelDeLogin.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.PainelDeLogin" %>

        <link href="<%= this.RaizDoSite %>/Resc/Skin/Default/bootstrap.custom.css" rel="stylesheet" type="text/css" />
        <script src="<%= this.RaizDoSite %>/Resc/Js/Lib/bootstrap.custom.js?v=<%=this.VersaoDoSite %>" type="text/javascript"></script>
        <script src="<%= this.RaizDoSite %>/Resc/Js/Site/99-Teclado.js?v=<%=this.VersaoDoSite %>" type="text/javascript"></script>
   
<div id="<%= this.IdDoPainel %>" class="header-login <%= (this.ExibirInline) ? "PainelLoginInline" : "" %>" style="<%= this.Estilo %>">

    <div id="pnlNavegacaoLogin_DadosDoUsuario" class="DadosDeLogin" runat="server" visible="false">

        <a href="<%= this.PaginaBase.HostERaizHttps %>/MinhaConta/" title="Minha Conta" class="Minhaconta">

        <span class="SoTexto">Olá, </span>
        <span class="NomeDoUsuario"> <asp:literal id="lblUsuario_Nome" runat="server"></asp:literal> </span>
        <span class="TextoLabel"> Conta:</span>
        <span class="CodigoDoUsuario"> <asp:literal id="lblUsuario_Conta" runat="server"></asp:literal> </span>
        <span class="TextoLabel"> Perfil:</span>
        <span class="CodigoDoUsuario"> <asp:literal id="lblUsuario_Perfil" runat="server"></asp:literal> </span>
        <span class="TextoLabel"> Último Acesso:</span>
        <span class="CodigoDoUsuario"> <asp:literal id="lblUsuario_Acesso" runat="server"></asp:literal> </span>

        </a>

        <%--<input type="button" onclick="return lnkGradSite_MinhaConta_Logout_Click(this)" class="btn-acesso" value="SAIR" />--%>
        <button type="button" onclick="return lnkGradSite_MinhaConta_Logout_Click(this)" class="btn-acesso" style="float: left; margin-right: 5px; margin-top: 9px;">SAIR</button>


        

        <!--a href="<%= this.PaginaBase.HostERaiz %>/Atendimento/Default.aspx" style="margin-left:8px;"><span class="LabelInline" style="text-decoration:underline">Atendimento</span></a-->

    </div>

    <asp:Panel runat="server" ID="pnlGradSite_MinhaConta_Cadastro_Login_ClienteLogin">

        <p>
            <label class="sua-conta tooltip">
                <span class="LabelInline">Sua Conta</span>
                <span class="LabelForm">Usuário</span>
                <%--<span class="tooltiptext">Email ou Código do Cliente</span>--%>
            </label>
            <!--input class="login" type="text" name="Login" value="" placeholder="Login" /-->
            <input  id="txtGradSite_Login_Usuario<%= this.SufixoInline %>" type="text" maxlength="255" class="login" placeholder="Email ou Código" onkeypress="javascript: GradSiteLoginUsuario_KeyPress(this, event);"/>
        </p>
        <p>
            <%--<label class="sua-conta sua-conta-senha">
                <span class="LabelInline">&nbsp;</span>
                <span class="LabelForm">Senha</span>
            </label>--%>
 
            <%if (!String.IsNullOrEmpty(this.SufixoInline))
            {%>
            <%--<input  id="txtGradSite_Login_Senha<%= this.SufixoInline %>"   type="password" maxlength="15"  class="" placeholder="&#x25cf;&#x25cf;&#x25cf;&#x25cf;&#x25cf;"  />--%>
            <%} %>

            <%--<a href="#" class="botao btn-padrao btn-erica" id="btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin" data-ClicarNoEnter="true" onclick="return btnGradSite_Login_EfetuarLogin(this, event)" style="">ACESSO</a>--%>
            <!--<a href="#" class="botao btn-padrao btn-erica" id="btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin" data-toggle="modal" data-target="#loginPanel" style="">ACESSAR</a>-->
            <a href="#" class="botao btn-padrao btn-erica" id="btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin" style="" onclick="javascript:return showKeyboard(this, event, '');">ACESSAR</a>
        </p>

        <p class="esqueci-senha">

            <a id="btnGradSite_MinhaConta_Cadastro_Login_EsqueciSenha" href="#" OnClick="return btnGradSite_Login_EsqueciSenha_Click(this)">
                <span class="LabelInline">Esqueceu seus dados de acesso?</span>
                <span class="LabelForm">Esqueci minha senha</span>
            </a>


            <!--a href="<%= this.PaginaBase.HostERaiz %>/Atendimento/Default.aspx" style="margin-left:28px;"><span class="LabelInline" style="text-decoration:underline">Atendimento</span></a-->

        </p>


    </asp:Panel>

    <asp:Panel runat="server" ID="pnlGradSite_MinhaConta_Cadastro_Login_ClienteLogado" visible="false">

        <p class="f5"><label runat="server" id="txtGradSite_MinhaConta_Cadastro_Login_UsuarioLogado_Nome"></label></p>

        <p class="f6"><asp:LinkButton runat="server" 
                ID="txtGradSite_MinhaConta_Cadastro_Login_Usuario_Logout" 
                onclick="txtGradSite_MinhaConta_Cadastro_Login_Usuario_Logout_Click">Logout</asp:LinkButton>
        </p>

    </asp:Panel>
    
    <div class="container">
        <!-- Modal -->
        <div class="modal fade" id="passwordPanel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h2 id="KeyBoard_Header">Teclado Virtual</h2>
                        <p id="KeyBoard_Description">Atenção: digite a sua senha eletrônica no teclado abaixo.</p>
                        <p class="esqueci-senha" id="KeyBoard_ForgotPass">
                            <a id="A1" href="#" onclick="return btnGradSite_Login_EsqueciSenha_Click(this)">
                                <span class="LabelInline">Esqueceu seus dados de acesso?</span> 
                                <span class="LabelForm">Esqueci minha senha</span>
                            </a>
                        </p>
                        <p class="esqueci-senha" id="KeyBoard_ForgotSign">
                            <a id="A2" href="#" onclick="return btnGradSite_Login_EsqueciAssinatura_Click(this)">
                                <span class="LabelInline">Esqueceu seus dados de acesso?</span> 
                                <span class="LabelForm">Esqueci minha assinatura</span>
                            </a>
                        </p>
                    </div>
                    <div class="container">
                        <div class="modal-body">
                        <asp:Panel runat="server" ID="pnlGradSite_Password">
                            <div class="centerBlock" id="passwordContainer">
                                <input  id="txtGradSite_Login_Senha" type="password" maxlength="15"  class="senha mostrar-teclado" placeholder="&#x25cf;&#x25cf;&#x25cf;&#x25cf;&#x25cf;&#x25cf;"  style="float: none; margin: 0 auto;"/>
                                <br />
                            </div>
                            <div class="centerBlock">
                                <div id="virtualKeys">
                                    <section class="gradient" id="EntryKeyboard" name="EntryKeyBoard">
                                        <button id="btnSeguranca_key0" onclick="javascript:Key_Click(this, event, 0); return false;">0</button>
                                        <button id="btnSeguranca_key1" onclick="javascript:Key_Click(this, event, 1); return false;">1</button>
                                        <button id="btnSeguranca_key2" onclick="javascript:Key_Click(this, event, 2); return false;">2</button>
                                        <button id="btnSeguranca_key3" onclick="javascript:Key_Click(this, event, 3); return false;">3</button>
                                        <button id="btnSeguranca_key4" onclick="javascript:Key_Click(this, event, 4); return false;">4</button>
                                        <button id="btnSeguranca_key5" onclick="javascript:Key_Click(this, event, 5); return false;">5</button>
                                        <button id="btnSeguranca_key6" onclick="javascript:Key_Click(this, event, 6); return false;">6</button>
                                        <button id="btnSeguranca_key7" onclick="javascript:Key_Click(this, event, 7); return false;">7</button>
                                        <button id="btnSeguranca_key8" onclick="javascript:Key_Click(this, event, 8); return false;">8</button>
                                        <button id="btnSeguranca_key9" onclick="javascript:Key_Click(this, event, 9); return false;">9</button>
                                    </section>
                                    <section class="gradient" id="ValidationKeyboard" name="ValidationKeyBoard">
                                        <button id="btn_key1" onmouseover="javascript:Key_MouseOver(this, event, 0); return false;" onmouseout="javascript:Key_MouseOut(this, event, 0); return false;" onclick="javascript:Key_Click(this, event, 0); return false;">X</button>
                                        <button id="btn_key2" onmouseover="javascript:Key_MouseOver(this, event, 1); return false;" onmouseout="javascript:Key_MouseOut(this, event, 1); return false;" onclick="javascript:Key_Click(this, event, 1); return false;">X</button>
                                        <button id="btn_key3" onmouseover="javascript:Key_MouseOver(this, event, 2); return false;" onmouseout="javascript:Key_MouseOut(this, event, 2); return false;" onclick="javascript:Key_Click(this, event, 2); return false;">X</button>
                                        <button id="btn_key4" onmouseover="javascript:Key_MouseOver(this, event, 3); return false;" onmouseout="javascript:Key_MouseOut(this, event, 3); return false;" onclick="javascript:Key_Click(this, event, 3); return false;">X</button>
                                        <button id="btn_key5" onmouseover="javascript:Key_MouseOver(this, event, 4); return false;" onmouseout="javascript:Key_MouseOut(this, event, 4); return false;" onclick="javascript:Key_Click(this, event, 4); return false;">X</button>
                                    </section>
                                </div>
                            </div>
                            <br />
                            <div class="centerBlock">
                                <button id="btn_clear"      class="btn btn-default" onclick="javascript:Key_Clear(this, event);return false;">Limpar</button>
                                <button id="btn_confirm"    class="btn btn-default" onclick="javascript:return btnGradSite_Login_EfetuarLogin(this, event); return false;"           style="margin-left:10px;">Confirmar</button>
                                <button id="btn_Close"      class="btn btn-default" data-dismiss="modal" style="margin-left:10px;">Fechar</button>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            </div>
        </div>
    </div>

</div>


<%--<div class="container">
  
  <!-- Modal -->
  <div class="modal fade" id="loginPanel" role="dialog">
    <div class="modal-dialog">
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h2>Teclado Virtual</h2>
          <p><strong>Atenção:</strong>digite a sua senha eletrônica no teclado abaixo.</p>
            <p class="esqueci-senha" style="">
                <a id="A1" href="#" onclick="return btnGradSite_Login_EsqueciSenha_Click(this)">
                    <span class="LabelInline">Esqueceu seus dados de acesso?</span> 
                    <span class="LabelForm">Esqueci minha senha</span>
                </a>
            </p>
            
        </div>
        <br />
        <div class="modal-body">
            <asp:Panel runat="server" ID="pnlGradSite_Password">
                <div class="centerBlock">
                    <input  id="txtGradSite_Login_Senha"   type="password" maxlength="15"  class="senha mostrar-teclado" placeholder="&#x25cf;&#x25cf;&#x25cf;&#x25cf;&#x25cf;&#x25cf;"  style="float: none; margin: 0 auto;"/>
                </div>

                <div class="centerBlock">
                    <div id="virtualKeys">
                        <section class="gradient" id="virtualKeyBoard" name="virtualKeyBoard">
                            <button id="btn_key1" onmouseover="javascript:Key_MouseOver(this, event, 0); return false;" onmouseout="javascript:Key_MouseOut(this, event, 0); return false;" onclick="javascript:Key_Click(this, event, 0); return false;">X</button>
                            <button id="btn_key2" onmouseover="javascript:Key_MouseOver(this, event, 1); return false;" onmouseout="javascript:Key_MouseOut(this, event, 1); return false;" onclick="javascript:Key_Click(this, event, 1); return false;">X</button>
                            <button id="btn_key3" onmouseover="javascript:Key_MouseOver(this, event, 2); return false;" onmouseout="javascript:Key_MouseOut(this, event, 2); return false;" onclick="javascript:Key_Click(this, event, 2); return false;">X</button>
                            <button id="btn_key4" onmouseover="javascript:Key_MouseOver(this, event, 3); return false;" onmouseout="javascript:Key_MouseOut(this, event, 3); return false;" onclick="javascript:Key_Click(this, event, 3); return false;">X</button>
                            <button id="btn_key5" onmouseover="javascript:Key_MouseOver(this, event, 4); return false;" onmouseout="javascript:Key_MouseOut(this, event, 4); return false;" onclick="javascript:Key_Click(this, event, 4); return false;">X</button>
                        </section>
                    </div>
                </div>
                <br />
                <div class="centerBlock">
                        <button id="btn_clear"      class="btn-sm" onclick="javascript:Limpar();return false;">Limpar</button>
                        <button id="btn_confirm"   class="btn-sm" onclick="javascript:return btnGradSite_Login_EfetuarLogin(this, event); return false;"           style="margin-left:10px;">Confirmar</button>
                </div>
            </asp:Panel>
            
        </div>
        <div class="modal-footer">

          <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>

        </div>
      </div>
      
    </div>
    </div>
    </div>--%>

<% if (this.PaginaBase.SessaoClienteLogado == null) { %>


<% } %>

<style type="text/css">
#virtualKeyboard
{
    background-color: #ffffff;
    padding: 5px 0 0 0;
}
section {
  margin-bottom: 60px
}
section:last-child {
  margin-bottom: 0
}
section h2 {
  margin-bottom: 30px
}
section.gradient button {
  display: inline-block;
  margin: 0 0 0 0;
  padding: 0 0 ;
  font-size: 16px;
  font-family: "Roboto",serif;
  line-height: 1.8;
  vertical-align: bottom;
  -webkit-appearance: none;
  -moz-appearance: none;
  appearance: none;
  -webkit-box-shadow: none;
  -moz-box-shadow: none;
  box-shadow: none;
  -webkit-border-radius: 0;
  -moz-border-radius: 0;
  border-radius: 0;
  width: 50px;
  height: 50px;
}
button:focus {
  outline: none
}
section.flat button {
  color: #fff;
  background-color: #6496c8;
  text-shadow: -1px 1px #417cb8;
  border: none;
}
section.flat button:hover,
section.flat button.hover {
  background-color: #346392;
  text-shadow: -1px 1px #27496d;
}
section.flat button:active,
section.flat button.active {
  background-color: #27496d;
  text-shadow: -1px 1px #193047;
}
section.border button {
  color: #6496c8;
  background: rgba(0,0,0,0);
  border: solid 5px #6496c8;
}
section.border button:hover,
section.border button.hover {
  border-color: #346392;
  color: #346392;
}
section.border button:active,
section.border button.active {
  border-color: #f9c000;
  color: #f9c000;
}
section.gradient button {
  color: #fff;
  text-shadow: -1px 1px #676767;
  background-color: #ff9664;
  background-image: -webkit-gradient(linear, left top, left bottom, from(#f9c000), to(#cfa000));
  background-image: -webkit-linear-gradient(top, #f9c000, #cfa000);
  background-image: -moz-linear-gradient(top, #f9c000, #cfa000);
  background-image: -o-linear-gradient(top, #f9c000, #cfa000);
  background-image: -ms-linear-gradient(top, #f9c000, #cfa000);
  background-image: linear-gradient(top, #f9c000, #cfa000);
  filter: progid:DXImageTransform.Microsoft.gradient(GradientType=0,StartColorStr='#f9c000', EndColorStr='#ce9f00');
  -webkit-box-shadow: inset 0 0 0 1px #f9c000;
  -moz-box-shadow: inset 0 0 0 1px #f9c000;
  box-shadow: inset 0 0 0 1px #f9c000;
  border: none;
  -webkit-border-radius: 5px;
  -moz-border-radius: 5px;
  border-radius: 5px;
}
section.gradient button:hover,
section.gradient button.hover {
  -webkit-box-shadow: inset 0 0 0 1px #676767,0 5px 15px #193047;
  -moz-box-shadow: inset 0 0 0 1px #676767,0 5px 15px #193047;
  box-shadow: inset 0 0 0 1px #676767,0 5px 15px #193047;
}
section.gradient button:active,
section.gradient button.active {
  -webkit-box-shadow: inset 0 0 0 1px #f9c000,inset 0 5px 30px #193047;
  -moz-box-shadow: inset 0 0 0 1px #f9c000,inset 0 5px 30px #193047;
  box-shadow: inset 0 0 0 1px #f9c000,inset 0 5px 30px #193047;
}
section.press button {
  color: #fff;
  background-color: #6496c8;
  border: none;
  -webkit-border-radius: 15px;
  -moz-border-radius: 15px;
  border-radius: 15px;
  -webkit-box-shadow: 0 10px #f9c000;
  -moz-box-shadow: 0 10px #f9c000;
  box-shadow: 0 10px #f9c000;
}
section.press button:hover,
section.press button.hover {
  background-color: #417cb8
}
section.press button:active,
section.press button.active {
  background-color: #417cb8;
  -webkit-box-shadow: 0 5px #f9c000;
  -moz-box-shadow: 0 5px #f9c000;
  box-shadow: 0 5px #f9c000;
  -webkit-transform: translateY(5px);
  -moz-transform: translateY(5px);
  -ms-transform: translateY(5px);
  -o-transform: translateY(5px);
  transform: translateY(5px);
}
@media all and (max-width: 960px) { 
  button {
    font-size: 30px
  }
}
@media all and (max-width: 720px) { 
  button {
    font-size: 20px;
    padding: 0 0;
  } 
}
@media all and (max-width: 540px) { 
  section {
    text-align: center
  }
  button {
    margin: 0 10px 20px 10px;
    font-size: 16px;
  }
}
.large-div {
  height:60px;
}
.short-div {
  height:20px;
}

.botao-acao {
  color: #fff;
  text-shadow: -1px 1px #676767;
  background-color: #ff9664;
  background-image: -webkit-gradient(linear, left top, left bottom, from(#747474), to(#646464));
  background-image: -webkit-linear-gradient(top, #747474, #646464);
  background-image: -moz-linear-gradient(top, #747474, #646464);
  background-image: -o-linear-gradient(top, #747474, #646464);
  background-image: -ms-linear-gradient(top, #747474, #646464);
  background-image: linear-gradient(top, #747474, #646464);
  filter: progid:DXImageTransform.Microsoft.gradient(GradientType=0,StartColorStr='#747474', EndColorStr='#646464');
  -webkit-box-shadow: inset 0 0 0 1px #747474;
  -moz-box-shadow: inset 0 0 0 1px #747474;
  box-shadow: inset 0 0 0 1px #747474;
  border: none;
  -webkit-border-radius: 5px;
  -moz-border-radius: 5px;
  border-radius: 5px;
  width:80px;
}

#btnConfirmar
{
    margin-top: 10px;
}

.centerBlock {
                display: table;
                margin: 15 auto;
            }
            
/* Tooltip container */
.tooltip {
    position: relative;
    display: inline-block;
}

/* Tooltip text */
.tooltip .tooltiptext {
    visibility: hidden;
    width: 180px;
    background-color: #f9c000;
    color: #4e443c;
    text-align: center;
    padding: 5px 0;
    border-radius: 6px;
 
    /* Position the tooltip text - see examples below! */
    position: absolute;
    z-index: 1;
}

/* Show the tooltip text when you mouse over the tooltip container */
.tooltip:hover .tooltiptext {
    visibility: visible;
}


::-webkit-input-placeholder { /* Chrome/Opera/Safari */
  color: #4e443c;
}
::-moz-placeholder { /* Firefox 19+ */
  color: #4e443c;
}
:-ms-input-placeholder { /* IE 10+ */
  color: #4e443c;
}
:-moz-placeholder { /* Firefox 18- */
  color: #4e443c;
}
</style>
<script type="text/javascript">

</script>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gradual.Spider.Www.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Resc/Js/Lib/jquery-core.js"></script>
      <script src="Resc/Js/Lib/jquery-ui.js"></script>
      <script src="Resc/Js/Lib/jquery-cookie.js"></script>
      <script src="Resc/Js/Lib/jquery.json-2.2.js"></script>
    <script type="text/javascript" src="Resc/js/site/04-Login.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset id="pnlLogin">
        <legend>Login</legend>
    
            <p style="margin-left:24px">
                <label for="txtLogin">Usuário:</label>
                <input type="text" id="txtLogin" maxlength="256" />
            </p>
        
            <p>
                <label for="txtSenha">Senha:</label>
                <input type="password" id="txtSenha" maxlength="20" />
            </p>
    
            <p id="pnlErro" style="display:none">
                Erro: Usuário / Senha inválidos
            </p>
    
            <p class="btnOk">
                <button id="btnLogin" onclick="return GradSpider_EfetuarLogin()">Entrar</button>
            </p>

            <p style="height: 10px; float: left; margin: -5px 0px 0px 182px;">
                <label id="btnGradSpider_Login_EsqueciMinhaSenha" style="cursor: pointer; width: 180px; font-size:10px;" onclick="return btnGradSpider_Login_EsqueciMinhaSenha_Click(this)">Esqueci minha senha</label>
            </p>
    
            <p id="pnlErroExtendido" style="display:none">
                <textarea>
                </textarea>
            </p>

        </fieldset>

        <div id="GradSpider_DivModalEsqueciMinhaSenha" style="display:none;">
        <div>
            <p>
                <label id="btnGradSpider_Login_EsqueciMinhaSenha_Fechar" style="cursor:pointer; font-size: 10px; float:right; margin-right:10px;" onclick="return btnGradSpider_Login_EsqueciMinhaSenha_Fechar_Click(this);">Fechar [x]</label>
            </p>
            <p style="margin: 20px 20px 0px 140px;">
                <label style="text-align:center; text-decoration:underline;">Esqueci minha senha</label>
            </p>
            <p style="margin: 20px 0px 0px 25px;">
                <label for="txtGradSpider_Login_EsqueciMinhaSenha_Email">E-mail:</label>
                <input type="text" id="txtGradSpider_Login_EsqueciMinhaSenha_Email" maxlength="60" style="width: 280px" />
            </p>
            <p style="margin-left: 240px;padding-top:5px;">
                <button id="btnGradSpider_Login_EnviarNovaSenha" onclick="return btnGradSpider_Login_EnviarNovaSenha_Click(this);">Enviar nova senha</button>
            </p>
            <p style="margin-top:10px;margin-bottom:-30px;text-align:center;color:#FF0066;width: 95%;display:none;">
                <label id="lblGradSpider_LoginEsqueciMinhaSenha_MensagemRetorno"></label>
            </p>
            <p style="padding: 40px 20px 0px; text-align: justify; width: 85%; font-size: 10px;">
                <label>Informe acima o seu e-mail comercial de acesso à Intranet da Gradual e clique em 'Enviar nova senha'.</label>
                <br />
                <label>Você receberá em instantes sua nova senha no e-mail informado.</label>
            </p>
        </div>
    </div>
    </form>
</body>
</html>

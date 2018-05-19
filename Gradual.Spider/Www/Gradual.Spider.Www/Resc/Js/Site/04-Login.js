
function Login_PageLoad() 
{
    $("#txtLogin").focus();
}

function GradSpider_EfetuarLogin() 
{
    $("#btnLogin").attr("disabled", "disabled");

    $.ajax({
        url: "Login.aspx",
        type: "post",
        dataType: "json",
        cache: false,
        data: {
            acao: "EfetuarLogin",
            usuario: $("#txtLogin").val(),
            senha: $("#txtSenha").val()
        },
        success: GradSpider_EfetuarLogin_CallBack,
        error: GradSpider_EfetuarLogin_Error
    });

    return false;
}

function GradSpider_EfetuarLogin_CallBack(pRetorno)
 {
     if (pRetorno.TemErro) 
    {
        $("#pnlErro")
            .html(pRetorno.Mensagem)
            .show();

        $("#btnLogin").attr("disabled", null);
    }
    else 
    {
        document.location = pRetorno.Mensagem;
    }
}

function GradSpider_EfetuarLogin_Error(pResposta) 
{
    $("#pnlErro")
        .html("Erro durante a requisição: [" + pResposta.status + "]")
        .show();

    $("#pnlErroExtendido")
        .find("textarea")
            .html(pResposta.responseText.replace(/</gi, "&lt;").replace(/>/gi, "&gt;"))
        .parent()
        .show();

    $("#btnLogin").attr("disabled", null);
}

function btnGradSpider_Login_EsqueciMinhaSenha_Click(pSender) 
{
    $("#GradSpider_DivModalEsqueciMinhaSenha").show();
    $("#txtGradSpider_Login_EsqueciMinhaSenha_Email")
        .val($("#txtLogin").val())
        .addClass("validate[required,custom[Email]]")
        .focus();

    return false;
}

function btnGradSpider_Login_EsqueciMinhaSenha_Fechar_Click(pSender)
{
    btnGradSpider_Login_FecharModal();
    return false;
}

function btnGradSpider_Login_FecharModal() 
{
    $("#txtLogin").focus();
    $("#GradSpider_DivModalEsqueciMinhaSenha").hide();
    $("#txtGradSpider_Login_EsqueciMinhaSenha_Email")
        .removeClass("validate[required,custom[Email]]")
        .val('');
    $("#lblGradSpider_LoginEsqueciMinhaSenha_MensagemRetorno")
        .html('')
        .parent()
            .hide();
}

function btnGradSpider_Login_EnviarNovaSenha_Click(pSender) 
{
    $.ajax({
        url: "Login.aspx",
        type: "post",
        dataType: "json",
        cache: false,
        data: 
        {
            acao: "EnviarEmailEsqueciMinhaSenha",
            Email: $("#txtGradSpider_Login_EsqueciMinhaSenha_Email").val()
        },
        success: btnGradSpider_Login_EnviarNovaSenha_Callback,
        error:  btnGradSpider_Login_EnviarNovaSenha_Callback
    });

    return false;
}

function btnGradSpider_Login_EnviarNovaSenha_Callback(pResposta) 
{
    var lLabelMensagem = $("#lblGradSpider_LoginEsqueciMinhaSenha_MensagemRetorno");

    if (pResposta.TemErro)
        lLabelMensagem.html("Houve um erro durante o envio do e-mail. Verifique se está informando o seu e-mail correto na corretora e tente novamente.");
    else
        lLabelMensagem.html("Sua senha foi alterada com sucesso. Aguarde o recebimento do email informando a nova senha.");

    lLabelMensagem.parent().show();
}

function EfetuarLogout() 
{
    //document.location = "Login.aspx?Acao=Logout";
    document.location = GradSpider_RaizDoSite() + "/Login.aspx";
}
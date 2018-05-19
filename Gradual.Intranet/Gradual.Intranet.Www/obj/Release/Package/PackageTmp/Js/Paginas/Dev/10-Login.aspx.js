
function Login_PageLoad()
{
    $("#txtLogin").focus();
}

function GradIntra_EfetuarLogin()
{
    $("#btnLogin").prop("disabled", true);

    $.ajax({
            url:      "Login.aspx",
            type:     "post",
            dataType: "json",
            cache:    false,
            data:     {
                        acao:    "EfetuarLogin",
                        usuario: $("#txtLogin").val(),
                        senha:   $("#txtSenha").val()
                      },
            success:  GradIntra_EfetuarLogin_CallBack,
            error:    GradIntra_EfetuarLogin_Error
            });

    return false;
}

function GradIntra_EfetuarLogin_CallBack(pRetorno)
{
    if(pRetorno.TemErro)
    {
        $("#pnlErro")
            .html(pRetorno.Mensagem)
            .show();
    
        $("#btnLogin").prop("disabled", false);
    }
    else
    {
        document.location = pRetorno.Mensagem;
    }
}

function GradIntra_EfetuarLogin_Error(pResposta)
{
    $("#pnlErro")
        .html("Erro durante a requisição: [" + pResposta.status + "]")
        .show();
    
    $("#pnlErroExtendido")
        .find("textarea")
            .html(pResposta.responseText.replace(/</gi, "&lt;").replace(/>/gi, "&gt;"))
        .parent()
        .show();

    $("#btnLogin").prop("disabled", false);
}

function btnGradIntra_Login_EsqueciMinhaSenha_Click(pSender)
{
    $("#GradIntra_DivModalEsqueciMinhaSenha").show();
    $("#txtGradIntra_Login_EsqueciMinhaSenha_Email")
        .val($("#txtLogin").val())
        .addClass("validate[required,custom[Email]]")
        .focus();

    return false;
}

function btnGradIntra_Login_EsqueciMinhaSenha_Fechar_Click(pSender)
{
    btnGradIntra_Login_FecharModal();
    return false;
}

function btnGradIntra_Login_FecharModal()
{
    $("#txtLogin").focus();
    $("#GradIntra_DivModalEsqueciMinhaSenha").hide();
    $("#txtGradIntra_Login_EsqueciMinhaSenha_Email")
        .removeClass("validate[required,custom[Email]]")
        .val('');
    $("#lblGradIntra_LoginEsqueciMinhaSenha_MensagemRetorno")
        .html('')
        .parent()
            .hide();
}

function btnGradIntra_Login_EnviarNovaSenha_Click(pSender)
{
    $.ajax({
        url:      "Login.aspx",
        type:     "post",
        dataType: "json",
        cache:    false,
        data:     {
                    acao:    "EnviarEmailEsqueciMinhaSenha",
                    Email: $("#txtGradIntra_Login_EsqueciMinhaSenha_Email").val()
                  },
        success:  btnGradIntra_Login_EnviarNovaSenha_Callback,
        error:    btnGradIntra_Login_EnviarNovaSenha_Callback
        });

    return false;
}

function btnGradIntra_Login_EnviarNovaSenha_Callback(pResposta)
{
    var lLabelMensagem = $("#lblGradIntra_LoginEsqueciMinhaSenha_MensagemRetorno");

    if (pResposta.TemErro)
        lLabelMensagem.html("Houve um erro durante o envio do e-mail. Verifique se está informando o seu e-mail correto na corretora e tente novamente.");
    else
        lLabelMensagem.html("Sua senha foi alterada com sucesso. Aguarde o recebimento do email informando a nova senha.");

    lLabelMensagem.parent().show();
}
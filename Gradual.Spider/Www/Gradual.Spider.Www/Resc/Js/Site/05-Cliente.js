/// <reference path="00-Base.js" />
/// <reference path="06-Risco.js" />
function Cliente_Filtro_Seleciona(pSender) 
{
    var lObjetoJson = 
    {
        PesquisaCodigo:      $(".Cliente_Pesquisa_Codigo").val(),
        PesquisaBovespa:     $(".Cliente_Pesquisa_Bovespa").is(":checked"),
        PesquisaBmf:         $(".Cliente_Pesquisa_Bmf").is(":checked"),
        PesquisaContaMaster: $(".Cliente_Pesquisa_Conta_Master").is(":checked"),
    };

    if (!lObjetoJson.PesquisaBovespa && ! lObjetoJson.PesquisaBmf && !lObjetoJson.PesquisaContaMaster)
    {
        alert("É necessário selecionar um tipo de conta para efetuar a pesquisa");

        return false;
    }

    if (lObjetoJson.PesquisaCodigo == "")
    {
        alert("É necessário inserir um código para efetuar a pesquisa");

        return false;
    }

    var lAcao = "Selecionar";

    var lUrl = "Clientes.aspx";

    var pDivDeFormulario = $("#pnlMensagem");

    var pOpcoesPosCarregamento = "";

    var lCliente =
    {
        Acao: lAcao,
        ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lCliente, Cliente_Filtro_Seleciona_Callback, pOpcoesPosCarregamento, pDivDeFormulario)

    return false;
}

function Cliente_Filtro_Seleciona_Callback(pResposta, pPainelParaAtualizar)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        GradSpider_ExibirMensagemFormulario("A",pResposta.Mensagem, pPainelParaAtualizar) ;

        var lCliente = pResposta.ObjetoDeRetorno;

        $(".Cliente_Form_Dados").fadeIn("fast");
        $(".Risco_Form_Dados").fadeIn("fast");

        $("#Cliente_Form_IdLogin")      .val(lCliente.IdLogin);
        $(".Cliente_Form_Nome")         .val(lCliente.Nome);
        $(".Cliente_Form_Email")        .val(lCliente.Email);
        $(".Cliente_Form_Assessor")     .val(lCliente.Assessor);
        $(".Cliente_Form_CodigoBovespa").val(lCliente.CodigoBovespa);
        $(".Cliente_Form_CodigoBmf")    .val(lCliente.CodigoBmf);
        $(".Cliente_Form_Sessao")       .val(lCliente.Sessao);
        $(".Cliente_Form_Localidade")   .val(lCliente.Localidade);
        //$(".Cliente_Form_ContaMae")     .val(lCliente.ContaMae);

        if (lCliente.CodigoBovespa != "" && lCliente.CodigoBovespa!= "0")
        {        
            Cliente_Risco_Listar_Dados_Cliente(lCliente.CodigoBovespa, lCliente.CodigoBmf);
        }
    }

    return false;
}

function Cliente_Form_Novo(pSender)
{
    $(".Cliente_Form_Dados").fadeOut("fast");

    $(".Cliente_Form_Dados").find("input[class*='Cliente_Form']").not("input[type=button]").val("");

    $(".Cliente_Form_Dados").find("select").each(function()
    {
        $(this).find("option:eq(0)").prop('selected',true)
    });

    var pDivDeFormulario = $("#pnlMensagem");

    GradSpider_ExibirMensagemFormulario("A","Insira um novo usuário", pDivDeFormulario);

    $(".Cliente_Form_Dados").fadeIn("fast");
    $(".Risco_Form_Dados").fadeOut("fast");

    return false;
}

function Cliente_Salvar(pSender)
{
    var lObjetoJson = 
    {
        IdLogin:        $("#Cliente_Form_IdLogin")      .val(),
        Nome:           $(".Cliente_Form_Nome")         .val(),
        Email:          $(".Cliente_Form_Email")        .val(),
        Assessor:       $(".Cliente_Form_Assessor")     .val(),
        CodigoBovespa:  $(".Cliente_Form_CodigoBovespa").val(),
        CodigoBmf:      $(".Cliente_Form_CodigoBmf")    .val(),
        Sessao:         $(".Cliente_Form_Sessao")       .val(),
        Localidade:     $(".Cliente_Form_Localidade")   .val(),
        //ContaMae:       $(".Cliente_Form_ContaMae")     .val(),
    }

    var lAcao = "Cadastrar";

    if (lObjetoJson.IdLogin == null)
    {
        lAcao = "Cadastrar";
    }
    else
    {
        lAcao = "Atualizar";
    }

    var lCliente =
    {
        Acao       : lAcao,
        ObjetoJson : $.toJSON(lObjetoJson)
    };

    var lUrl = "Clientes.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = $("#pnlMensagem");

    GradSpider_CarregarJsonVerificandoErro(lUrl, lCliente, Cliente_Salvar_Callback, pOpcoesPosCarregamento, pDivDeFormulario)

    return false;
}

function Cliente_Salvar_Callback(pResposta, pDivDeFormulario)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        GradSpider_ExibirMensagemFormulario("A","Usuário Salvo com sucesso.", pDivDeFormulario);
        
        $(".Risco_Form_Dados").fadeIn("fast");
    }
}



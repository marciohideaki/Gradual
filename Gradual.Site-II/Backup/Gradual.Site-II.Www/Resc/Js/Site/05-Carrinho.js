/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />


var pnlDadosDeCompra_RealizarCompra = null;

var pnlFormularioDeCompra, pnlMensagemDeProgresso, pnlMensagemDeErro;

var gCompraOffLine = null;
var gComprandoOffLine = false;

function Carrinho_IniciarCarrinho()
{
    if(pnlDadosDeCompra_RealizarCompra == null)
    {
        pnlDadosDeCompra_RealizarCompra = $("#ContentPlaceHolder1_StockMarket1_pnlDadosDeCompra_RealizarCompra");

        pnlFormularioDeCompra  = pnlDadosDeCompra_RealizarCompra.find(".FormularioPadrao");

        pnlMensagemDeProgresso = pnlDadosDeCompra_RealizarCompra.find(".MensagemDeProgresso");

        pnlMensagemDeErro = pnlDadosDeCompra_RealizarCompra.find(".MensagemDeErro");
    }
}

function Carrinho_IniciarCarrinhoTravelCard()
{
    if (pnlDadosDeCompra_RealizarCompra == null)
    {
        pnlDadosDeCompra_RealizarCompra = $("#ContentPlaceHolder1_TravelCard1_pnlDadosDeCompra_RealizarCompra");

        pnlFormularioDeCompra = pnlDadosDeCompra_RealizarCompra.find(".FormularioPadrao");

        pnlMensagemDeProgresso = pnlDadosDeCompra_RealizarCompra.find(".MensagemDeProgresso");

        pnlMensagemDeErro = pnlDadosDeCompra_RealizarCompra.find(".MensagemDeErro");
    }
}

function Carrinho_IniciarCarrinhoGTI() 
{
    if (pnlDadosDeCompra_RealizarCompra == null) 
    {
        pnlDadosDeCompra_RealizarCompra = $("#ContentPlaceHolder1_GTI1_pnlDadosDeCompra_RealizarCompra");

        pnlFormularioDeCompra = pnlDadosDeCompra_RealizarCompra.find(".FormularioPadrao");

        pnlMensagemDeProgresso = pnlDadosDeCompra_RealizarCompra.find(".MensagemDeProgresso");

        pnlMensagemDeErro = pnlDadosDeCompra_RealizarCompra.find(".MensagemDeErro");
    }
}

function Carrinho_ExibirMensagemDeProgresso(pMensagem)
{
    if(!pnlMensagemDeProgresso.is(":visible"))
    { 
        pnlFormularioDeCompra.hide();

        pnlMensagemDeProgresso.show();
    }

    pnlMensagemDeProgresso.find("p").html(pMensagem);
}

function Carrinho_ExibirMensagemDeErro(pMensagem)
{
    if(!pnlMensagemDeErro.is(":visible"))
    { 
        pnlFormularioDeCompra.hide();

        pnlMensagemDeProgresso.hide();

        pnlMensagemDeErro.show();
    }

    pnlMensagemDeErro.find("p:eq(0)").html(pMensagem);
}

function Carrinho_IniciarCompra()
{
/*
    Carrinho_IniciarCarrinho();

    Carrinho_ExibirMensagemDeProgresso("Iniciando negociação com o PagSeguro, favor aguardar um momento...");

    var lDados = { Acao: "IniciarCompra", IdProduto: $("#hidIdDoProduto").val(), Quantidade: 1, Assinatura: $("#txtSenhaEletronica").val() };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, Carrinho_IniciarCompra_CallBack);
    */
}


function Carrinho_IniciarCompraGTI_CallBack(pResposta) 
{
    $("[data-reabilitar]").text("CONTRATAR").attr("disabled", null).attr("data-reabilitar", null);

    if (pResposta.Mensagem == "ERRO_ASSINATURA")
    {
        GradSite_ExibirMensagem("I", "Assinatura eletrônica não confere, favor tentar novamente...");
    }
    else if (pResposta.Mensagem == "ERRO_EMAIL")
    {
        GradSite_ExibirMensagem("I", "Sua compra foi efetuada porém ocorreu um erro ao enviar o email para o departamento responsável, favor entrar em contato com o atendimento.");
    }
    else if (pResposta.Mensagem == "ERRO_HABILITACAO")
    {
        GradSite_ExibirMensagem("I", "Sua compra foi efetuada porém ocorreu um erro ao habilitar o produto, favor entrar em contato com o atendimento.");
    }
    else 
    {
        GradSite_ExibirMensagem("I", "Sua solicitação foi enviada com sucesso!<br /><br />A adesão ao produto será realizada em até duas horas úteis.");

        //document.location = "http://localhost/Gradual.TestePagSeguro.Www/Default.aspx?IdDaTransacao=" + pResposta.Mensagem + "&PrecoDoProduto=99,00&NomeDoProduto=Gradual Trader Interface";
    }
}

function Carrinho_IniciarCompra_CallBack(pResposta)
{
    /*
    if(pResposta.Mensagem == "ERRO_ASSINATURA")
    {
        Carrinho_ExibirMensagemDeErro("Assinatura eletrônica não confere, favor tentar novamente...");
    }
    else
    {
        alert("Você será redirecionado pro PagSeguro agora.");
        
        var lURL = "https://pagseguro.uol.com.br/v2/checkout/payment.html?code=" + pResposta.Mensagem;

        var lRaiz = $("#hidRaiz").val();

        if(lRaiz.indexOf("gsp-srv") != -1)
        {
            if(lRaiz.indexOf(":8") != -1)
            {
                lRaiz = lRaiz.substr(0, lRaiz.indexOf(":8"));
            }

            lURL = lRaiz + "/Gradual.TestePagSeguro.Www/Default.aspx?IdDaTransacao=" + pResposta.Mensagem + "&PrecoDoProduto=99,00&NomeDoProduto=Gradual Trader Interface";
        }

        if (lRaiz.indexOf("localhost") != -1)
        {
            lURL = "http://localhost/Gradual.TestePagSeguro.Www/Default.aspx?IdDaTransacao=" + pResposta.Mensagem + "&PrecoDoProduto=99,00&NomeDoProduto=Gradual Trader Interface";
        }

        document.location = lURL;

        //document.location = "http://localhost/Gradual.TestePagSeguro.Www/Default.aspx?IdDaTransacao=" + pResposta.Mensagem + "&PrecoDoProduto=99,00&NomeDoProduto=Gradual Trader Interface";
    }
    */
}




/*  Event Handlers de "Carrinho"   */

function chkTermoLido_Click(pSender)
{
    pSender = $(pSender);

    var lBtn = pSender.closest(".FormularioPadrao").find(".btn-contratar");

    if(pSender.is(":checked"))
    {
        lBtn.attr("disabled", null).css( { opacity: 1} );
    }
    else
    {
        lBtn.attr("disabled", "disabled").css( { opacity: 0 } );
    }
}

function chkTermoLidoGTI_Click(pSender) 
{
    pSender = $(pSender);

    if (pSender.is(":checked")) 
    {
        $("#btnIniciarCompraGTI").attr("disabled", null).css({ opacity: 1 });
    }
    else {
        $("#btnIniciarCompraGTI").attr("disabled", "disabled").css({ opacity: 0 });
    }
}

function btnMensagemDeErroOk_Click(pSender)
{
    pnlFormularioDeCompra.show();

    pnlMensagemDeProgresso.hide();

    pnlMensagemDeErro.hide();

    return false;
}



function btnIniciarCompraStockMarket_Click(pSender)
{
    $(pSender).attr("disabled", "disabled").attr("data-reabilitar", "true").text("Aguarde...");

    var lDados = { Acao: "IniciarCompra", IdProduto: $("#hidIdDoProduto").val(), Assinatura: $("#txtSenhaEletronica").val() };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, Carrinho_IniciarCompraGTI_CallBack);

    return false;
}

function btnIniciarCompraGTI_Click(pSender)
{
    $(pSender).attr("disabled", "disabled").attr("data-reabilitar", "true").text("Aguarde...");

    var lDados = { Acao: "IniciarCompra", IdProduto: $("#hidIdDoProdutoGTI").val(), Assinatura: $("#txtSenhaEletronicaGTI").val() };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, Carrinho_IniciarCompraGTI_CallBack);

    return false;
}

function btnIniciarCompraTravelCard_Click(pSender)
{
    Carrinho_IniciarCarrinhoTravelCard();

    Carrinho_ExibirMensagemDeProgresso("Iniciando negociação com a FoxCambio, favor completar a compra no site deles; as informações serão recebidas pela Gradual posteriormente.");

    var lURL = $("#hidQueryString").text();

    document.location = lURL;

    return false;
}

function txtCambioQtd_Change(pSender)
{
    //pSender = $(pSender);

    var lQtd = new Number(pSender.value);

    var lTD = $(pSender).closest("table").find("td[data-ValorTotal]");

    var lTDTaxa = $(pSender).closest("table").find("td[data-ValorTaxas]");

    if(!isNaN(lQtd))
    {
        var lTxCambio = new Number(pSender.getAttribute("data-TxCambio"));
        var lIOF = new Number(pSender.getAttribute("data-IOF"));

        var lTotal = (lQtd * lTxCambio);

        var lTaxas = ((lIOF/100) * lTotal);

        lTD.attr("data-ValorSubTotal", lTotal);

        lTotal = lTotal + lTaxas;

        lTD.html("R$ " + NumConv.NumToStr(lTotal, 2));

        lTD.attr("data-ValorTotal", lTotal);

        lTDTaxa.attr("data-ValorTaxas", lTaxas).html("R$ " + NumConv.NumToStr(lTaxas, 2));
    }
    else
    {
        lTD.html("-");
    }
}

function txtCambioQtd_KeyDown(pSender, pEvent)
{
    if(pEvent.keyCode == 13)
    {
        $(pSender).closest("table").find("button").click();

        pEvent.preventDefault();
        pEvent.stopPropagation();

        return false;
    }
}

function rdoCarrinho_EndEntrega_Click(pSender)
{
    if($("#rdoCarrinho_EndEntrega_Conta").is(":checked"))
    {
        $("#pnlCarrinho_EnderecoDeEntrega").hide();
    }
    else
    {
        $("#pnlCarrinho_EnderecoDeEntrega").show();
    }
}

function txtCelular_DDD_Blur(pSender)
{
    pSender = $(pSender);

    var lID = "#" + pSender.attr("id").replace("DDD", "Numero");

    if(pSender.val() == "11" || pSender.val() == "21")
    {
        Validacao_HabilitarMascaraNumerica($(lID), "99999-9999");
    }
    else
    {
        Validacao_HabilitarMascaraNumerica($(lID), "9999-9999");
    }
}

function btnTopoCarrinho_Click(pSender)
{
    GradSite_AbrirCarrinho();

    return false;
}

function GradSite_AbrirCarrinho()
{
    $("#pnlCarrinhoOverlay").find("button[disabled]").attr("disabled", null);

    $("#pnlCarrinhoOverlay").show();
}

function GradSite_FecharCarrinho(pZerar)
{
    $("#pnlCarrinhoOverlay").hide();

    if(pZerar)
    {
        var lTabela = $("#tblProdutosCarrinho");

        lTabela.find("tr[class!='NenhumItem']").remove();

        lTabela.find("tr.NenhumItem").show();

        $(".btnTopoCarrinho").hide();
    }

    return false;
}

function btnCarrinho_Comprar_Click(pSender)
{
    pSender = $(pSender);

    var lComUsuario = (pSender.attr("data-Usuario").toLowerCase() == "true");
    /*
    if(pSender.attr("data-Usuario").toLowerCase() == "false")
    {
        document.location = GradSite_BuscarRaiz("/MinhaConta/Login.aspx?redir=/Investimentos/Cambio.aspx");

        return false;
    }
    */

    var lQtd = pSender.closest("fieldset").find("input.txtQtd").val();

    if(lQtd.trim() == "")
    {
        GradSite_ExibirMensagem("E", "Favor especificar a quantidade.");

        //pSender.closest("fieldset").find("input.txtQtd").focus();
    }
    else
    {
        lQtd = parseInt(lQtd);

        if(isNaN(lQtd))
        {
            GradSite_ExibirMensagem("E", "Quantidade inválida.");

            //pSender.closest("fieldset").find("input.txtQtd").focus();
        }
        else
        {
            var lDados = pSender.closest("fieldset").find("input.DadosDoProduto").val();

            var lForm = pSender.closest("fieldset");

            lDados = $.evalJSON(lDados);

            lDados.QuantidadeProduto = lQtd;

            lDados.TaxasTotal = parseFloat(  lForm.find("td[data-ValorTaxas]").attr("data-ValorTaxas")  );

            lDados.SubTotal = parseFloat(  lForm.find("td[data-ValorSubTotal]").attr("data-ValorSubTotal")  );

            lDados.Total = parseFloat(  lForm.find("td[data-ValorTotal]").attr("data-ValorTotal")  );

            lDados.Tipo = pSender.attr("data-Tipo");

            lDados.Modo = pSender.attr("data-Modo");

            pSender.attr("disabled", "disabled").attr("data-RetornoCarrinho", "true");

            if(lComUsuario)
            {
                GradSite_AdicionarUmProduto(lDados);
            }
            else
            {
                GradSite_AdicionarUmProdutoOffLine(lDados);
            }
            
        }
    }

    return false;
}

var gProdutoNovo;

function GradSite_AdicionarUmProduto(pDadosDoProduto)
{
    gProdutoNovo = pDadosDoProduto;

    var lDados = { Acao: "AdicionarItemAoCarrinho", DadosDoProduto: $.toJSON(gProdutoNovo) };

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, GradSite_AdicionarUmProduto_CallBack);
}

function GradSite_AdicionarUmProdutoOffLine(pDadosDoProduto)
{
    gProdutoNovo = pDadosDoProduto;

    gComprandoOffLine = true;

    if(gCompraOffLine == null)
    {
        gCompraOffLine = { Produtos: [] };
    }

    gProdutoNovo.PreID = Date.now(); //somente para dar um ID provisório inútil abaixo

    gCompraOffLine.Produtos.push(gProdutoNovo);

    GradSite_AdicionarUmProduto_CallBack( { Mensagem: gProdutoNovo.PreID } );
}

function GradSite_AdicionarUmProduto_CallBack(pResposta)
{
    $("button[data-RetornoCarrinho]").attr("disabled", null).attr("data-RetornoCarrinho", null);

    var lTR = "<tr data-PreId='" + pResposta.Mensagem + "' data-TotalItem='" + gProdutoNovo.Total + "'>" + 
                "<td>" + gProdutoNovo.NomeProduto + " - " + gProdutoNovo.Tipo + " " + gProdutoNovo.Modo + "</td>" +
                "<td>" + NumConv.NumToStr(gProdutoNovo.ValorProduto, 2) + "</td>" +
                "<td>" + gProdutoNovo.QuantidadeProduto + "</td>" +
                "<td>" + NumConv.NumToStr(gProdutoNovo.SubTotal, 2) + "</td>" +
                "<td>" + NumConv.NumToStr(gProdutoNovo.TaxasTotal, 2) + "</td>" +
                "<td>" + NumConv.NumToStr(gProdutoNovo.Total, 2) + "</td>" +
                "<td class='CarrinhoBotaoExcluir'> <button onclick='return btnCarrinho_ExcluirProduto_Click(this);' class='BotaoIcone IconeRemoverMarrom'><span>Excluir</span></button> </td>" +
              "</tr>";

    var lTable = $("#tblProdutosCarrinho")
    var lFoot = null;

    lTable.append(lTR);

    var lTotal = lTable.attr("data-Total");

    if(lTotal === undefined || lTotal == "" || lTotal == null)
    {
        lTotal = gProdutoNovo.Total;
    }
    else
    {
        lTotal = new Number(lTotal) + gProdutoNovo.Total;
    }

    lTable.attr("data-Total", lTotal);

    if(lTable.find(">tr").length > 2)
    {
        lFoot = lTable.closest("table").find("tfoot");

        lFoot.show();

        lFoot.find("tr td:eq(1)").html(NumConv.NumToStr(lTotal, 2));
    }

    $(".btnTopoCarrinho").addClass("btnCarrinhoCheiro").show();

    $("#tblProdutosCarrinho tr.NenhumItem").hide();

    $("#pnlCarrinhoOverlay").show();

    if(gComprandoOffLine)
    {
        //$("#pnlAlertaEstadoIndisponivel").show();

        $("#rdoCarrinho_EndEntrega_Outro").click().attr("disabled", "disabled").closest("div").hide();

        $("#rdoCarrinho_EndEntrega_Conta").attr("disabled", "disabled").hide();

        $("#pnlCarrinho_EnderecoDeEntrega").show();
    }
    else
    {
        if($("label[data-EnderecoDaContaPermitido='false']").length > 0)
        {
            $("#pnlAlertaEstadoIndisponivel").show();

            $("#rdoCarrinho_EndEntrega_Outro").click().attr("disabled", "disabled");

            $("#rdoCarrinho_EndEntrega_Conta").attr("disabled", "disabled");

            $("#pnlCarrinho_EnderecoDeEntrega").show();
        }
    }
}


function btnCarrinho_ExcluirProduto_Click(pSender)
{
    pSender = $(pSender);

    var lDados = { Acao: "RemoverItemDoCarrinho", PreID: pSender.closest("tr").attr("data-PreId") };

    if(gComprandoOffLine)
    {
        GradSite_RemoverUmProduto_CallBack( { Mensagem: lDados.PreID } );
    }
    else
    {
        GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, GradSite_RemoverUmProduto_CallBack);
    }

    return false;
}

function GradSite_RemoverUmProduto_CallBack(pResposta)
{
    var lTR = $("tr[data-PreId='" + pResposta.Mensagem + "']");

    var lTbody = lTR.closest("tbody");

    lTR.remove();

    if(lTbody.find("tr").length == 1)
    {
        lTbody.find("tr").show();
        
        lTbody.parent().find("tfoot").hide();

        $(".btnTopoCarrinho").removeClass("btnCarrinhoCheiro").hide();
    }
    else
    {
        //re-calcula o total:
        var lTotal = 0;

        lTbody.find("tr[data-PreId]").each(function()
        {
            lTotal += new Number( $(this).attr("data-TotalItem") );
        });

        lTbody.attr("data-Total", lTotal);

        lTbody.parent().find("tfoot tr td:eq(1)").html( NumConv.NumToStr(lTotal, 2) );
    }
}

function GradSite_FinalizarCarrinho(pSender)
{
    pSender = $(pSender);

    var lForm = $("#pnlCarrinho_EnderecoDeEntrega");

    pSender.attr("disabled", "disabled");

    var lDados = {      Acao: "FinalizarCarrinho"
                  , Endereco: "Conta"
                  , Cel_DDD : $("#Carrinho1_txtCarrinho_Cel_DDD").val()
                  , Cel_Num : $("#Carrinho1_txtCarrinho_Cel_Numero").val()
                  , Tel_DDD : $("#Carrinho1_txtCarrinho_Tel_DDD").val()
                  , Tel_Num : $("#Carrinho1_txtCarrinho_Tel_Numero").val() 
                 };

    if($(".SubForm_Dados").is(":visible"))
    {
        if(!$(".SubForm_Dados").validationEngine("validate"))
        {
            GradSite_ExibirMensagem("E", "Dado(s) inválido(s), favor preencher seu nome e email.");

            pSender.attr("disabled", null);

            return false;
        }
        else
        {
            lDados.NomeCliente = $("#txtCarrinho_NomeCliente").val();
            lDados.EmailCliente = $("#txtCarrinho_EmailCliente").val();
        }
    }
    
    if(!$(".SubForm_Telefones").validationEngine("validate"))
    {
        GradSite_ExibirMensagem("E", "Telefone(s) inválido(s), favor preencher os telefones.");

        pSender.attr("disabled", null);

        return false;
    }

    if($("#rdoCarrinho_EndEntrega_Outro").is(":checked"))
    {
        if(lForm.validationEngine("validate"))
        {
            lDados.Endereco = {
                  CEP          : $("#txtCarrinho_EndEntrega_CEP").val()
                , Logradouro   : $("#txtCarrinho_EndEntrega_Logradouro").val()
                , Numero       : $("#txtCarrinho_EndEntrega_Numero").val()
                , Complemento  : $("#txtCarrinho_EndEntrega_Complemento").val()
                , Bairro       : $("#txtCarrinho_EndEntrega_Bairro").val()
                , Cidade       : $("#txtCarrinho_EndEntrega_Cidade").val()
                , Estado       : $("#cboCarrinho_EndEntrega_Estado").val()
                , Pais         : "BRA"
            };

            lDados.Endereco = $.toJSON(lDados.Endereco);
        }
        else
        {
            GradSite_ExibirMensagem("E", "Endereço inválido, favor preencher o endereço de entrega.");

            pSender.attr("disabled", null);

            return false;
        }
    }


    if(gComprandoOffLine)
    {
        lDados.Acao = "FinalizarCarrinhoOffline";
        lDados.Produtos = $.toJSON(gCompraOffLine.Produtos);
    }

    GradSite_CarregarJsonVerificandoErro(GradSite_BuscarRaiz("/Async/Geral.aspx"), lDados, GradSite_FinalizarCarrinho_CallBack);

    return false;
}

function GradSite_FinalizarCarrinho_CallBack(pResposta)
{
    GradSite_FecharCarrinho(true);

    var lTel = "(21) 3388-9090";

    if($("#cboCarrinho_EndEntrega_Estado").val() == "SP") lTel = "(11) 3074-1244";

    GradSite_ExibirMensagem("I", "Sua solicitação de compra foi enviada com sucesso!<br/> <br/> Em breve a equipe especializada da nossa <br/> Mesa de Câmbio entrará em contato com você. <br/>Se preferir faça o contato através do telefone " + lTel + ".");
}
/// <reference path="00-Auxiliares.js" />

var pnlEsquerdo, pnlConteudo, pnlDashBoard;

function Page_Load()
{
    Page_Load_CodeBehind();
    /*
    pnlEsquerdo = $("#pnlEsquerdo");
    pnlConteudo = $("#pnlConteudo");
    pnlDashBoard = $("#pnlDashBoard");


    Inv_AjustarLayout();

    $(window).bind("resize", Inv_AjustarLayout);

    $("input[data-LidarComEnter='ClicarProximo']").bind("keydown", txtLidarComEnter_ClicarProximo_KeyDown);

    Page_Load_DashBoard();
    */
    var lLinkPreSelecionado = $("#hidLinkPreSelecionado").val();

    try {
        $("#" + lLinkPreSelecionado)
            .addClass("Selecionado")
            .closest(".hasSubmenu").find(">a:eq(0)").click();
    }
    catch (erro) { }
    
    /*
        var idDoCliente = $("#hidClienteSelecionado").val();
           
        if(idDoCliente && idDoCliente.length > 0)
        {
            
            //$("li[data-iddocliente='" + idDoCliente + "']").find("a:first-child").hide();
            //alert("oi");
        }*/

}


function Inv_AjustarLayout() {
    var lPlusHeight = $.getDocHeight() - $("body").height();

    var lPrevHeight = pnlEsquerdo.height();

    var lHeight = lPrevHeight + lPlusHeight - 10;

    if (lHeight < 510)
        lHeight = 510;

    pnlEsquerdo.css({ height: lHeight });
    pnlConteudo.css({ height: lHeight });
}




function Inv_BuscarFundo() {
    var lstResultadosDeBusca = $("#lstResultadosDeBusca");

    var lDados = { Acao: "BuscarFundo", TermoDeBusca: $("#txtTermoDeBuscaDeFundos").val() };

    if (lDados.TermoDeBusca != lstResultadosDeBusca.attr("data-UltimoTermoDeBusca")) {
        lstResultadosDeBusca.attr("data-UltimoTermoDeBusca", lDados.TermoDeBusca).hide();

        Aux_CarregarJsonVerificandoErro(Aux_UrlComRaiz("/Default.aspx"), lDados, Inv_BuscarFundo_CallBack);
    }
    else {
        lstResultadosDeBusca.show();
    }
}


function Inv_BuscarFundo_CallBack(pResposta) {
    if (pResposta.Mensagem == "ok" && pResposta.ObjetoDeRetorno) {
        var lstResultadosDeBusca = $("#lstResultadosDeBusca");

        var lHTML = "";

        var lCliente;

        if (pResposta.ObjetoDeRetorno.length == 0) {
            lHTML += "<li class='NenhumItem'> (nenhum cliente encontrado) </li>";
        }
        else {
            for (var a = 0; a < pResposta.ObjetoDeRetorno.length; a++) {
                lCliente = pResposta.ObjetoDeRetorno[a];

                lHTML += "<li data-IdDoCliente='" + lCliente.IdCliente + "'> <a href='#' class='glyphicons btn-small display' onclick='return lnkSelecionarDashBoard_Click(this)'><i></i> <span>" + lCliente.Nome + "</span> </a></li>";
            }

            lHTML += "<li class='btn-close'> <a href='#' onclick='return lnkFecharResultadosDeBusca_Click(this)'>fechar</a> </li>";
        }

        if (pResposta.ObjetoDeRetorno.length == 1) {
            //seleciona direto
        }
        else {
            lstResultadosDeBusca.html(lHTML).show();
        }
    }
    else {
        alert(pResposta.Mensagem);
    }
}


function txtLidarComEnter_ClicarProximo_KeyDown(event) {
    if (event.keyCode == 13) {
        $(event.currentTarget).next().click();

        event.preventDefault();
        event.stopPropagation();

        return false;
    }
}


function lnkAbrirTema_Click(pSender) {
    $("#themer").toggleClass("in");
}

function lnkFecharTema_Click(pSender) {
    $("#themer").removeClass("in").attr("style", null);
    $.cookie("themerOpened", null);
    return false;
}

function lnkAvisoDoCliente_Click(pSender) {
    if (confirm("Deseja marcar esse aviso como lido?")) {
        Inv_MarcarAvisoComoLido($(pSender).attr("data-IdDoAviso"));
    }

    return false;
}

function btnBuscarFundo_Click(pSender) {
    Inv_BuscarFundo();

    return false;
}

function lnkFecharResultadosDeBusca_Click(pSender) {
    $("#lstResultadosDeBusca").hide();

    return false;
}

function lnlMensagemAdicional_Fechar_Click(pSender)
{
    Aux_OcultarMensagemAdicional();

    return false;
}
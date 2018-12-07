/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />

var gDadosDaOrdem = {  Ativo: null,  AtivoValido: false  };

var CONST_TIPOORDEM_ORDEM = "2";
var CONST_TIPOORDEM_STARTCOMPRA = "5";
var CONST_TIPOORDEM_STOPVENDA = "3";
var CONST_TIPOORDEM_STOPMOVEL = "4";

var gDadosDoAtivoIntegral = null, gDadosDoAtivoFracionario = null;

function AcompanhamentoDeOrdem_AbrirFecharHistorico(pBotao, pTodos)
{
    pBotao = $(pBotao);

    pBotao.toggleClass("IconExpandColap_Expandido");

    if(pTodos == true)
    {
        if(pBotao.hasClass("IconExpandColap_Expandido"))
        {
            pBotao.parent().parent().parent().parent().find("tbody tr.Historico").show();
        }
        else
        {
            pBotao.parent().parent().parent().parent().find("tbody tr.Historico").hide();
        }
    }
    else
    {
        pBotao.parent().parent().next("tr").toggle();
    }

    return false;
}

function AcompanhamentoDeOrdem_CancelarOrdem(pBotao, pId, pControlNumber, pSymbol)
{
    if (confirm("Deseja cancelar a ordem [" + pControlNumber + "] ?"))
    {
        //alert("Cancelar ordem " + lNumero);
        $.ajax({  
                url:  "Operacoes.aspx", 
                type: "POST",
                data: {
                           Acao   : "CancelarOrdem",
                           IdOrdem: pControlNumber,
                           Ativo  : pSymbol
                      },
                dataType: "json", 
                cache:    false, 
                complete: AcompanhamentoDeOrdem_CancelarOrdem_CallBack
               });
    }

    return false;
}

function AcompanhamentoDeOrdem_CancelarOrdem_CallBack(pResposta)
{
    if(!pResposta.TemErro && pResposta.TemErro == undefined)
    {
        alert("Ordem cancelada com sucesso!\r\n\r\nRecarregue a página para atualizar o status.");
        $(".btn-buscar").click();

        //location.reload(true);
    }
    else
    {
        alert("Erro ao cancelar ordem: " + pResposta.Mensagem);
    }
}


function AcompanhamentoDeOrdemStopStart_Cancelar(pBotao, pId)
{
///<summary>Método para cancelar uma ordem do grid.</summary>
///<returns>void</returns>
    //var lNumero = $(pBotao).closest("tr").attr("Codigo");

    var lNumero = pId;
    var lPapel  = $(pBotao).parent().parent().children("td:eq(2)").html();

    if(confirm("Deseja cancelar a ordem [" + lNumero + "] ?"))
    {
        //alert("Cancelar ordem " + lNumero);
        $.ajax({  
                url:  "EnvioDeOrdens.aspx", 
                type: "POST",
                data: {
                           Acao:   "CancelarOrdemStopStart",
                           Ordem : lNumero,
                           Ativo : lPapel
                      },
                dataType: "json", 
                cache:    false, 
                complete: AcompanhamentoDeOrdem_CancelarOrdem_CallBack
               });
    }

    return false;
}

/* ----- Ordens ------------------------------------------------------ */



function txtOrdem_Ativo_KeyDown(pSender, pEvent)
{
    if(pEvent.keyCode == 13)
    {
        pSender = $(pSender);

        try { pEvent.preventDefault();  } catch(erro1) {}
        try { pEvent.stopPropagation(); } catch(erro2) {}
        
        Ordem_BuscarDadosDoAtivo( pSender.val(), pSender, false );

        return false;
    }

    return true;
}

function txtOrdem_Ativo_Blur(pSender, pEvent)
{
    pSender = $(pSender);

    Ordem_BuscarDadosDoAtivo( pSender.val(), pSender, false );
}

function txtOrdem_Quantidade_Blur(pSender)
{
    Ordem_CalcularValorDaOperacao(pSender);
}

function cboOrdem_Tipo_Change(pSender)
{
    pSender = $(pSender);

    //pSender.parent().parent().attr("class", "pnlOrdem_Formulario Ordem_Tipo_" + pSender.val());

    var cboValidade = pSender.closest("div").find("select.cboOrdem_Validade");

    if (pSender.val() == "2") 
    {
        cboValidade.attr("disabled", null);
        $(".rowStopStart").hide();
        $(".txtOrdem_OrdemStartStop_PrecoDisparo").val("");
    }
    else if (pSender.val() == "5" || pSender.val() == "3") 
    {
        $(".rowStopStart").show();
    }
    else 
    {
        cboValidade.val("4").attr("disabled", "disabled");
        $(".rowStopStart").hide();
        $(".txtOrdem_OrdemStartStop_PrecoDisparo").val("");
    }
}

function txtOrdem_LossGain_Focus(pSender)
{
    $(pSender).css("background-position", "-100px -100px");
}

function txtOrdem_LossGain_Blur(pSender)
{
    pSender = $(pSender);
    
    if(pSender.val() == "")
    {
        if(pSender.attr("class").indexOf("Disparo") != -1)
        {
            pSender.css("background-position", "4px 2px");
        }
        else
        {
            pSender.css("background-position", "1px -16px");
        }
    }
}

function btnOrdem_Comprar_Click(pSender)
{
    Ordem_ValidarEExibirConfirmacao("COMPRA");

    return false;
}

function btnOrdem_Vender_Click(pSender)
{
    Ordem_ValidarEExibirConfirmacao("VENDA");

    return false;
}

function btnOrdem_Confirmar_Click(pSender)
{
    $("#btnOrdem_Comprar").attr("disabled", "disabled");
    $("#btnOrdem_Vender").attr("disabled", "disabled");

    Ordem_RealizarEnvio();

    return false;
}

function btnOrdem_Cancelar_Click(pSender)
{
    var lMessageBox = $("#pnlMessageBox");

    var lMessageBoxCriticas = lMessageBox.find("div.pnlMessageBox_Content_MensagemQualquer");

    var lDivCriticas = lMessageBox.find("div.pnlMessageBox_Content_Criticas");
    
    lDivCriticas.html("");

    Ordem_HabilitarDesabilitarFormulario(true);

    lMessageBox.hide();

    lMessageBoxCriticas.hide();

    lDivCriticas.hide();
    
    lMessageBox.prev("div").show();

    return false;
}

function btnOrdem_Ok_Click(pSender)
{
    btnOrdem_Cancelar_Click(pSender);

    return false;
}



function Ordem_ZerarDadosDaOrdem(pAtivo)
{
    gDadosDaOrdem = {  Ativo: pAtivo
                     , AtivoValido:     false
                     , AtivoEFracionario: function()
                       {
                            if(this.Ativo != null && this.Ativo != "")
                            {
                                return (this.Ativo.charAt(this.Ativo.length - 1) == "F");
                            }

                            return false;
                       }
                     };
}

function Ordem_ExibirMensagem(pTipo_AEI, pMensagem, pMostrarLoader)
{
    var lPainel = $("#pnlMensagens");

    lPainel.removeClass("Alerta").removeClass("Erro").removeClass("Informacao");
    
    if(pTipo_AEI == "A") lPainel.addClass("Alerta");
    if(pTipo_AEI == "E") lPainel.addClass("Erro");
    if(pTipo_AEI == "I") lPainel.addClass("Informacao");

    if(pMostrarLoader == true) lPainel.addClass("ExibirLoader");

    lPainel.html(pMensagem);

    lPainel.show();
}

function Ordem_RecarregarDadosDoAtivo()
{
    if(gDadosDaOrdem != null && gDadosDaOrdem.AtivoValido)
    {
        if(gDadosDaOrdem.Ativo != $(".lblDadosDoAtivo_Codigo").html())
            gDadosDaOrdem.Ativo = $(".lblDadosDoAtivo_Codigo").html();

        Ordem_BuscarDadosDoAtivo(gDadosDaOrdem.Ativo, null, true);
    }

    return false;
}

function Ordem_BuscarDadosDoAtivo(pAtivo, pTextBox, pFlagRecarregar)
{
    pAtivo = pAtivo.toUpperCase();

    if(pTextBox != null)
        pTextBox.val(pAtivo);

    if(gDadosDaOrdem.Ativo != pAtivo || (pFlagRecarregar == true))
    {
        if(!pFlagRecarregar)
            Ordem_ZerarDadosDaOrdem(pAtivo);

        Ordem_ExibirMensagem("I", "Carregando dados do ativo [" + pAtivo + "], por favor aguarde...")

        $.ajax({  url:      "Operacoes.aspx"
                , type:     "POST"
                , data:     {
                                 Acao:  "BuscarDadosDoAtivo"
                               , Ativo: pAtivo
                            }
                , dataType: "json"
                , cache:    false
                , complete: Ordem_BuscarDadosDoAtivo_CallBack
               });
    }
}

function Ordem_BuscarDadosDoAtivo_CallBack(pResposta)
{
    if(pResposta.status == 200)
    {
        pResposta = $.evalJSON(pResposta.responseText);

        if(!pResposta.TemErro)
        {
            gDadosDaOrdem.AtivoValido = true;

            $("a.BotaoAtualizarCotacao").css( { visibility:"visible" } );

            Ordem_ExibirMensagem("I", pResposta.Mensagem);

            Ordem_PreencherDadosDoAtivo(pResposta.ObjetoDeRetorno, false);

            Ordem_HabilitarDesabilitarFormulario(true);

            $(".txtOrdem_Quantidade").focus();
        }
        else
        {
            gDadosDaOrdem.AtivoValido = false;

            Ordem_ExibirMensagem("E", pResposta.Mensagem);

            Ordem_HabilitarDesabilitarFormulario(false);
        }
    }
    else
    {
        Ordem_ExibirMensagem("E", "Erro ao receber resposta: status [" + pResposta.status + "]");

        Ordem_HabilitarDesabilitarFormulario(false);
    }
}

function Ordem_PreencherDadosResumoPapel(pDadosCotacao) 
{
    var lTbody = $(".resumo tbody td[data-propriedade]");

    var lPropriedade, lValor, lSomenteLeilao;

    var pDados = pDadosCotacao.DadosDeCotacao;

    if (lTbody.length > 0) 
    {
        lTbody.each(function () {
            lTD = $(this);

            var lPropriedade = lTD.attr("data-propriedade");

            if (lPropriedade == "jPC") 
            {
                var lValorTemp = pDados.jVR;

                if (lValorTemp.indexOf("-") == 0) 
                {
                    lTD.removeClass("bg-azul").addClass("bg-vermelho");
                } 
                else 
                {
                    lTD.removeClass("bg-vermelho").addClass("bg-azul");
                }

                lValor = NumConv.NumToStr(pDados.jPC.replace(",", "."), 2);
            }
            else if (lPropriedade == "jVR") {
                lValor = pDados.jVR;

                if (lValor.indexOf("-") == 0) {
                    lTD.removeClass("Oscilacao_Positiva").addClass("Oscilacao_Negativa");
                }
                else {
                    lTD.removeClass("Oscilacao_Negativa").addClass("Oscilacao_Positiva");
                }
            }
            else if (lPropriedade == "jMPC") {
                lValor = pDados.jMPC;
            }
            else if (lPropriedade == "jCN") {
                lValor = pDados.jCN;
            }
            else if (lPropriedade == "jHN") {
                lValor = pDados.jHN;
            }
            else if (lPropriedade == "jMPV") {
                lValor = pDados.jMPV;
            }
            else if (lPropriedade == "jQT") {
                lValor = pDados.jQT;
            }
            else if (lPropriedade == "jQMPC") {
                lValor = NumConv.NumToStr(pDados.jQMPC, 0);
            }
            else if (lPropriedade == "jQMPV") {
                lValor = NumConv.NumToStr(pDados.jQMPV, 0);
            }
            else if (lPropriedade == "jXD") {
                lValor = NumConv.NumToStr(pDados.jXD.replace(",", "."), 2);
            }
            else if (lPropriedade == "jPM") {
                lValor = NumConv.NumToStr(pDados.jPM.replace(",", "."), 2);
            }
            else if (lPropriedade == "jND") {
                lValor = NumConv.NumToStr(pDados.jND.replace(",", "."), 2);
            }
            else if (lPropriedade == "jVA") {
                lValor = NumConv.NumToStr(pDados.jVA.replace(",", "."), 2);
            }
            else if (lPropriedade == "jPTA") {
                lValor = NumConv.NumToStr(pDados.jPTA.replace(",", "."), 2);
            }
            else if (lPropriedade == "jVF") {
                lValor = pDados.jVF;
            }
            else if (lPropriedade == "jQAMC") {
                lValor = pDados.jQAMC + "";
            }
            else if (lPropriedade == "jQAMV") {
                lValor = pDados.jQAMV + "";
            }
            else if (lPropriedade == "jVTO") {
                lValor = pDados.jVTO + "";
            }
            else if (lPropriedade == "jLA") {
                lValor = ToNumeroAbreviado((pDados.jLA.replace(',', '.') * 1), 2);
                lValor = (lValor + "").replace('.', ',');
            }
            else if (lPropriedade == "jNN") {
                lValor = NumConv.NumToStr(pDados.jNN, 0);
            }
            else if (lPropriedade == "jPE") {
                //lValor = NumConv.NumToStr(pDados.jPE.replace(",", "."), 2);
            }
            else if (lPropriedade == "jDE" && pDados.jDE != null) {
                lValor = pDados.jDE.substring(6, 8)  //--> dia
                         + "/" + pDados.jDE.substring(4, 6)  //--> mes
                         + "/" + pDados.jDE.substring(0, 4); //--> ano
            }
            else if (lPropriedade == "jDN" && pDados.jDN != null) {
                lValor = pDados.jDN.substring(6, 8)  //--> dia
                         + "/" + pDados.jDN.substring(4, 6)  //--> mes
                         + "/" + pDados.jDN.substring(0, 4)  //--> ano
                         + " às " + pDados.jHN;                 //--> hora
            }

            if (lValor != "") {
                lTD.html(lValor);
            }

        });
    }


}

function Ordem_PreencherDadosDoAtivo(pDados, pSelecionandoViaAbas)
{
    /*
    "ObjetoDeRetorno":
    {
        "DadosDeCotacao":
        {
            "Papel":null
            ,"CodigoNegocio":"PETR4"
            ,"Preco":"26,66"
            ,"IndicadorVariacao":"-"
            ,"Variacao":"-3,02"
            ,"InformacoesCadastrais":
            {
                "FatorDeCotacao":"1"
                ,"NomeDaEmpresa":"PETROBRAS"
                ,"VencimentoDaOpcao":null
                ,"LoteMinimo":"1"
            }
        }
    }
    */

    var lPainel = $("#abas-conteudo");

    var lblVariacao = lPainel.find(".lblDadosDoAtivo_Variacao");

    var lLinks = $("#pnlOrdem_LadoDireito p.Abas a");

    if(pDados.DadosDeCotacao.HeaderTipoDeBolsa == "BF")
    {
        //papel BMF, desativa o fracionário:

        lLinks.filter(":eq(1)").hide();
    }
    else
    {
        lLinks.filter(":eq(1)").show();
    }

    Ordem_PreencherDadosResumoPapel(pDados);

    if(!pSelecionandoViaAbas)
    {
        // "seleciona" a aba conforme o tipo de ativo:
        if( gDadosDaOrdem.AtivoEFracionario() )
        {
            lLinks.filter(":eq(0)").removeClass("Selecionado");
            lLinks.filter(":eq(1)").addClass("Selecionado");

            gDadosDoAtivoFracionario = { Ativo: gDadosDaOrdem.Ativo, Dados: pDados };

            if(gDadosDoAtivoIntegral != null && gDadosDoAtivoIntegral.Ativo + "F" != gDadosDaOrdem.Ativo)
            {
                //se estiver guardado no cache o integral de outro ativo, apaga
                gDadosDoAtivoIntegral = null;
            }
        }
        else
        {
            lLinks.filter(":eq(0)").addClass("Selecionado");
            lLinks.filter(":eq(1)").removeClass("Selecionado");

            gDadosDoAtivoIntegral = { Ativo: gDadosDaOrdem.Ativo, Dados: pDados };

            if(gDadosDoAtivoFracionario != null && gDadosDoAtivoIntegral.Ativo + "F" != gDadosDoAtivoFracionario.Ativo)
            {
                //se estiver guardado no cache o integral de outro ativo, apaga
                gDadosDoAtivoFracionario = null;
            }
        }
    }


    lPainel.find(".lblDadosDoAtivo_Codigo").html(                          pDados.DadosDeCotacao.jCN         );
    lPainel.find(".lblDadosDoAtivo_Preco").html(      NumConv.StrToPretty( pDados.DadosDeCotacao.jPC, 2 )    );
    lPainel.find(".lblDadosDoAtivo_LoteMinimo").html(                      pDados.DadosCadastrais.LoteMinimo );
    
    lblVariacao.html(  pDados.DadosDeCotacao.jVR  );

    lblVariacao
        .removeClass("ValorNegativo")
        .removeClass("ValorPositivo")
        .addClass( (pDados.DadosDeCotacao.jIV == "-") ? "ValorNegativo" : "ValorPositivo" );

    lPainel.find(".lblDadosDoAtivo_Financeiro_Maxima"    ).html(  NumConv.StrToPretty( pDados.DadosDeCotacao.jXD, 2 ) );
    lPainel.find(".lblDadosDoAtivo_Financeiro_Minima"    ).html(  NumConv.StrToPretty( pDados.DadosDeCotacao.jND, 2 ) );
    lPainel.find(".lblDadosDoAtivo_Financeiro_Abertura"  ).html(  NumConv.StrToPretty( pDados.DadosDeCotacao.jVA, 2 ) );
    lPainel.find(".lblDadosDoAtivo_Financeiro_Fechamento").html(  NumConv.StrToPretty( pDados.DadosDeCotacao.jVF, 2 ) );
    lPainel.find(".lblDadosDoAtivo_Financeiro_Volume"    ).html(  NumConv.StrToPretty( pDados.DadosDeCotacao.jLA, 0 ) );
    lPainel.find(".lblDadosDoAtivo_Financeiro_Quantidade").html(  NumConv.StrToPretty( pDados.DadosDeCotacao.jQT, 0 ) );

    var lTRs = gDadosDaOrdem.AtivoEFracionario() ? lPainel.find(".OfertasDeCompraVendaFracionario tbody tr") : lPainel.find(".OfertasDeCompraVenda tbody tr"); ;
    var lTR;
    var lIndice = 0;

    lTRs.each(function()
    {
        lTR = $(this);
        
        if(pDados.LivroDeOferta.OfertasDeCompra.length > lIndice)
        {
            lTR.children("td:eq(0)").html(  pDados.LivroDeOferta.OfertasDeCompra[lIndice].NomeCorretora  );
            lTR.children("td:eq(1)").html(  pDados.LivroDeOferta.OfertasDeCompra[lIndice].Quantidade     );
            lTR.children("td:eq(2)").html(  pDados.LivroDeOferta.OfertasDeCompra[lIndice].Preco          );
        }
        else
        {
            lTR.children("td:lt(3)").html("&nbsp;");
        }
        
        if(pDados.LivroDeOferta.OfertasDeVenda.length > lIndice)
        {
            lTR.children("td:eq(3)").html(  pDados.LivroDeOferta.OfertasDeVenda[lIndice].Preco          );
            lTR.children("td:eq(4)").html(  pDados.LivroDeOferta.OfertasDeVenda[lIndice].Quantidade     );
            lTR.children("td:eq(5)").html(  pDados.LivroDeOferta.OfertasDeVenda[lIndice].NomeCorretora  );
        }
        else
        {
            lTR.children("td:gt(2)").html("&nbsp;");
        }

        lIndice++;
    });
}


function Ordem_HabilitarDesabilitarFormulario(pHabilitar)
{
    var lDisabled = null;
    
    if(pHabilitar != true) lDisabled = "disabled";

    var lFormulario = $(".form-consulta");  //$(" #pnlOrdem_LadoEsquerdo div.pnlOrdem_Formulario");

    lFormulario.find("input[class!='txtOrdem_Ativo'], select, button").attr("disabled", lDisabled);

//    if(pHabilitar)
//        lFormulario.find("input[type='password']").attr("readonly", "readonly");

    //lFormulario.find(".txtOrdem_Ativo").attr("disabled", null);
}



function DadosDoAtivo_SelecionarAba(pLink, pAba)
{
    pLink = $(pLink);

    if( ! pLink.hasClass("ativo") )
    {
        pLink.parent().find("a").removeClass("ativo");

        pLink.addClass("ativo");

        if(pAba == 1)
        {
            //Ativo Integral
            $("#pnlDadosDoAtivo").show();
            $("#pnlGraficoDoAtivo").hide();

            if(gDadosDaOrdem.AtivoValido)
            {
                //clicou no integral vindo da aba fracionário
                if(gDadosDoAtivoIntegral != null)
                {
                    //já tinha os dados do integral em cache
                    Ordem_PreencherDadosDoAtivo(gDadosDoAtivoIntegral.Dados, true);
                }
                else
                {
                    //não tinha em cache, pega do servidor:
                    Ordem_BuscarDadosDoAtivo(gDadosDaOrdem.Ativo.substr(0, gDadosDaOrdem.Ativo.length - 1), null, false);
                }
            }
        }
        else if(pAba == 2)
        {
            //Ativo Fracionário
            $("#pnlDadosDoAtivo").show();
            $("#pnlGraficoDoAtivo").hide();
            
            if(gDadosDaOrdem.AtivoValido)
            {
                //clicou no fracionário vindo da aba integral
                if(gDadosDoAtivoFracionario != null)
                {
                    //já tinha os dados do fracionário em cache
                    Ordem_PreencherDadosDoAtivo(gDadosDoAtivoFracionario.Dados, true);
                }
                else
                {
                    //não tinha em cache, pega do servidor:
                    Ordem_BuscarDadosDoAtivo(gDadosDaOrdem.Ativo + "F", null, false);
                }
            }
        }
        else if(pAba == 3)
        {
            //Gráfico
            $("#pnlDadosDoAtivo").hide();
            $("#pnlGraficoDoAtivo").show();
            
            var lAtivo = gDadosDaOrdem.Ativo;

            if(lAtivo.charAt(lAtivo.length - 1) == "F")
                lAtivo = lAtivo.substr(0, lAtivo.length - 1);

            //alert("buscando gráfico para [" + lAtivo + "]");

            if(gDadosDaOrdem.AtivoValido)
            {
                $("#fraGraficoDoAtivo").attr("src", "http://web.infomoney.com.br//clientes/gradualV2/popup.asp?TipoPop=G&codigo=" + lAtivo);
            }
        }
    }

    return false;
}

function Ordem_PreencheBoletaAlteracao(pOrdem)
{
    var lOrdem = pOrdem.ObjetoDeRetorno;

    if (pOrdem.Mensagem == "TemOrdem")
    {
        $("#txtOrdem_Ativo")     .val(lOrdem.Symbol);
        $("#cboOrdem_Validade")  .val(lOrdem.TimeInForce);
        $(".txtOrdem_Quantidade").val(lOrdem.OrderQtyRemmaining);

        $(".txtOrdem_Preco")     .val(NumConv.NumToStr(lOrdem.Price, 2));
        $("#txtOrdem_Ativo")     .blur();
        $(".txtOrdem_Preco")     .blur();
        $(".lblTituloAlteracao") .html(" - Alteração de " + lOrdem.Symbol);

        gDadosDaOrdem.IdOrdem =  lOrdem.ClOrdID;
        $("#txtOrdem_Ativo").prop('disabled', true);

        if (lOrdem.Side == 1) $(".btnVender").hide();
        if (lOrdem.Side == 2) $(".btnComprar").hide();

    }
}

function Ordem_CalcularValorDaOperacao(pTextBox)
{
    var lPanelConteudo = $(pTextBox).parent().parent();

    var lLabelResultado = $("input.txtOrdem_Valor");

    var lInputQuantidade = $("input.txtOrdem_Quantidade");

    var lQuantidade = lInputQuantidade.val();

    var lInputPreco = $("input.txtOrdem_Preco");

    var lPreco = lInputPreco.val();

    var lResultado = "&nbsp;";

    if(lPreco != "" && lQuantidade != "")
    {
        //limita 4 casas após a vírgula pro preco
        var lIndiceVirgula = lPreco.indexOf(",");

        if(lIndiceVirgula != -1)
        {
            if(lPreco.length - lIndiceVirgula > 5)
            {
                lPreco = lPreco.substr(0, lIndiceVirgula + 5);

                lPanelConteudo.find("input.txtOrdem_Preco").val(lPreco);
            }
        }

        lResultado = GradAux_Multiplicacao(lPreco, lQuantidade);
        
        lPreco      = GradAux_NumeroFromStringPtBR(lPreco);
        lQuantidade = GradAux_NumeroFromStringPtBR(lQuantidade);

        //lLabelResultado.html( NumConv.NumToStr(lResultado, 2) );
        lLabelResultado.val(NumConv.NumToStr(lResultado, 2));
    }
    else
    {
        lLabelResultado.html( lResultado );
    }
    
    //gPerfMon.MarcarMonitoramentoDeProcesso("UsoDaJanela_Ordens", "Ordem_CalcularValorDaOperacao");
}


function Ordem_ValidarOrdem(pTipoDeOrdem, pFormulario)
{
    //gPerfMon.MarcarMonitoramentoDeProcesso("UsoDaJanela_Ordens", "Ordem_ValidarOrdem ==> Inicio");

    gDadosDaOrdem.TipoOrdem = pTipoDeOrdem;
    gDadosDaOrdem.Valido = true;

    gDadosDaOrdem.Validade     = pFormulario.find("select.cboOrdem_Validade").val()
    gDadosDaOrdem.ValidadeDesc = pFormulario.find("select.cboOrdem_Validade option:selected").text()
    gDadosDaOrdem.Quantidade   = pFormulario.find("input.txtOrdem_Quantidade").val();
    gDadosDaOrdem.Total        = pFormulario.find("input.txtOrdem_Valor").val();
    gDadosDaOrdem.Assinatura   = pFormulario.find("input.txtOrdem_Assinatura").val();
    

    if (gDadosDaOrdem.Valido 
    && (gDadosDaOrdem.Quantidade == "" || isNaN( new Number(gDadosDaOrdem.Quantidade) )))
    {
        alert("Favor especificar uma quantidade numérica válida para a operação.");

        pFormulario.find("input.txtOrdem_Quantidade").focus();

        gDadosDaOrdem.Valido = false;
    }

    if (gDadosDaOrdem.Valido)
    {
        switch(pTipoDeOrdem)
        {
            case CONST_TIPOORDEM_ORDEM :

                gDadosDaOrdem.Preco      = pFormulario.find("input.txtOrdem_Preco").val();

                if(gDadosDaOrdem.Preco == "" || isNaN( GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Preco) ))
                {
                    alert("Favor especificar um preço numérico válido para a operação.");

                    pFormulario.find("input.txtOrdem_Preco").focus();

                    gDadosDaOrdem.Valido = false;
                }

            break;

            case CONST_TIPOORDEM_STARTCOMPRA :

                gDadosDaOrdem.Preco = pFormulario.find("input.txtOrdem_Preco").val();

                if (gDadosDaOrdem.Preco == "" || isNaN(GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Preco))) 
                {
                    alert("Favor especificar um preço numérico válido para a operação.");

                    pFormulario.find("input.txtOrdem_Preco").focus();

                    gDadosDaOrdem.Valido = false;
                }

                gDadosDaOrdem.PrecoStopStart = pFormulario.find("input.txtOrdem_OrdemStartStop_PrecoDisparo").val();

                if (gDadosDaOrdem.PrecoStopStart == "" || isNaN(GradAux_NumeroFromStringPtBR(gDadosDaOrdem.PrecoStopStart)))
                {
                    alert("Favor especificar um preço de disparo válido para a operação.");

                    pFormulario.find("input.txtOrdem_OrdemStartStop_PrecoDisparo").focus();

                    gDadosDaOrdem.Valido = false;
                }
//                else
//                {
//                    if (gDadosDaOrdem.PrecoStopStart == "" || isNaN(GradAux_NumeroFromStringPtBR(gDadosDaOrdem.PrecoStopStart)))
//                    {
//                        alert("Favor especificar um preço de limite válido para a operação.");

//                        pFormulario.find("input.txtOrdem_OrdemStartStop_PrecoDisparo").focus();

//                        gDadosDaOrdem.Valido = false;
//                    }
//                }

            break;

            case CONST_TIPOORDEM_STOPVENDA :

                gDadosDaOrdem.PrecoDisparo_Loss = pFormulario.find("input.txtOrdem_StopVenda_Loss_PrecoDisparo").val();
                gDadosDaOrdem.PrecoLimite_Loss  = pFormulario.find("input.txtOrdem_StopVenda_Loss_PrecoLancamento").val();

                gDadosDaOrdem.PrecoDisparo_Gain = pFormulario.find("input.txtOrdem_StopVenda_Gain_PrecoDisparo").val();
                gDadosDaOrdem.PrecoLimite_Gain  = pFormulario.find("input.txtOrdem_StopVenda_Gain_PrecoLancamento").val();

                if(gDadosDaOrdem.PrecoDisparo_Loss != "" || gDadosDaOrdem.PrecoLimite_Loss != "")
                {
                    //se colocou valor de Loss em uma das textbox de loss, ambas são obrigatórias

                    if(gDadosDaOrdem.PrecoDisparo_Loss == "" && gDadosDaOrdem.PrecoLimite_Loss != "")
                    {
                        alert("Favor especificar um preço de disparo LOSS válido para a operação.");

                        pFormulario.find("input.txtOrdem_StopVenda_Loss_PrecoDisparo").focus();

                        gDadosDaOrdem.Valido = false;
                    }
                    else if(gDadosDaOrdem.PrecoDisparo_Loss != "" && gDadosDaOrdem.PrecoLimite_Loss == "")
                    {
                        alert("Favor especificar um preço de lançamento LOSS válido para a operação.");

                        pFormulario.find("input.txtOrdem_StopVenda_Loss_PrecoLancamento").focus();

                        gDadosDaOrdem.Valido = false;
                    }
                    else
                    {
                        //ambas preenchidas, ok
                    }
                }

                if(gDadosDaOrdem.PrecoDisparo_Gain != "" || gDadosDaOrdem.PrecoLimite_Gain != "")
                {
                    //se colocou valor de Gain em uma das textbox de gain, ambas são obrigatórias

                    if(gDadosDaOrdem.PrecoDisparo_Gain == "" && gDadosDaOrdem.PrecoLimite_Gain != "")
                    {
                        alert("Favor especificar um preço de disparo GAIN válido para a operação.");

                        pFormulario.find("input.txtOrdem_StopVenda_Gain_PrecoDisparo").focus();

                        gDadosDaOrdem.Valido = false;
                    }
                    else if(gDadosDaOrdem.PrecoDisparo_Gain != "" && gDadosDaOrdem.PrecoLimite_Gain == "")
                    {
                        alert("Favor especificar um preço de limite GAIN válido para a operação.");

                        pFormulario.find("input.txtOrdem_StopVenda_Gain_PrecoLancamento").focus();

                        gDadosDaOrdem.Valido = false;
                    }
                    else
                    {
                        //ambas preenchidas, ok
                    }
                }

                if(gDadosDaOrdem.PrecoDisparo_Loss == "" 
                && gDadosDaOrdem.PrecoLimite_Loss  == "" 
                && gDadosDaOrdem.PrecoDisparo_Gain == "" 
                && gDadosDaOrdem.PrecoLimite_Gain  == "")
                {
                    alert("Favor especificar preços de LOSS ou GAIN para a operação.");

                    pFormulario.find("input.txtOrdem_StopVenda_Loss_PrecoDisparo").focus();

                    gDadosDaOrdem.Valido = false;
                }

            break;

            case CONST_TIPOORDEM_STOPMOVEL :
            
                gDadosDaOrdem.PrecoDisparo_Loss  = pFormulario.find("input.txtOrdem_StopMovel_PrecoDisparo").val();
                gDadosDaOrdem.PrecoDisparo_Gain  = pFormulario.find("input.txtOrdem_StopMovel_PrecoLancamento").val();
                gDadosDaOrdem.Inicio             = pFormulario.find("input.txtOrdem_StopMovel_Inicio").val();
                gDadosDaOrdem.Ajuste             = pFormulario.find("input.txtOrdem_StopMovel_Ajuste").val();
                
                if(gDadosDaOrdem.PrecoDisparo_Loss == "" || isNaN( GradAux_NumeroFromStringPtBR(gDadosDaOrdem.PrecoDisparo_Loss))  )
                {
                    alert("Favor especificar um preço de disparo válido para a operação.");

                    pFormulario.find("input.txtOrdem_StopMovel_PrecoDisparo").focus();

                    gDadosDaOrdem.Valido = false;
                }
                else if(gDadosDaOrdem.PrecoDisparo_Gain == "" || isNaN( GradAux_NumeroFromStringPtBR(gDadosDaOrdem.PrecoDisparo_Gain))  )
                {
                    alert("Favor especificar um preço limite válido para a operação.");

                    pFormulario.find("input.txtOrdem_StopMovel_PrecoLancamento").focus();

                    gDadosDaOrdem.Valido = false;
                }
                else if(gDadosDaOrdem.Inicio == "" || isNaN( GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Inicio))  )
                {
                    alert("Favor especificar um preço de início válido para a operação.");

                    pFormulario.find("input.txtOrdem_StopMovel_Inicio").focus();

                    gDadosDaOrdem.Valido = false;
                }
                else if(gDadosDaOrdem.Ajuste == "" || isNaN( GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Ajuste))  )
                {
                    alert("Favor especificar um preço de ajuste válido para a operação.");

                    pFormulario.find("input.txtOrdem_StopMovel_Ajuste").focus();

                    gDadosDaOrdem.Valido = false;
                }

            break;
        }
    }
    
}


function Ordem_ExibirConfirmacao(pSentido)
{
    var lFormulario = $("#conteudo");  //$("#pnlOrdem_LadoEsquerdo div.pnlOrdem_Formulario"); //lPainelConteudo.find("div.FormularioDePagina:visible")

    var lMessageBoxContainer = $("#pnlMessageBox");

    var lMessageBox = lMessageBoxContainer.find("div.pnlMessageBox_Content_Confirmacao");

    if(!gDadosDaOrdem.Valido) return false;

    gDadosDaOrdem.Direcao = pSentido;

    Ordem_HabilitarDesabilitarFormulario(false);

    lMessageBox.find("em.lblSentido").html(    gDadosDaOrdem.Direcao);
    lMessageBox.find("em.lblPapel").html(      gDadosDaOrdem.Ativo);
    lMessageBox.find("em.lblValidade").html(   gDadosDaOrdem.ValidadeDesc);
    lMessageBox.find("em.lblQuantidade").html( gDadosDaOrdem.Quantidade);
    lMessageBox.find("em.lblPreco").html(      gDadosDaOrdem.Preco);
    lMessageBox.find("em.lblTotal").html(      gDadosDaOrdem.Total);

    if(gDadosDaOrdem.TipoOrdem == CONST_TIPOORDEM_ORDEM)
    {
        lMessageBox.find("span.pnlSomatoria").show();
    }
    else
    {
        lMessageBox.find("span.pnlSomatoria").hide();
    }

    var lFormularioEnvio = $(".form-consulta");

    var lFormularioEnvioPosition = lFormularioEnvio.position();

//    lMessageBoxContainer.prev("div").hide();

//    lMessageBoxContainer.children("div").hide();

    lMessageBox.css(
    {
        position: 'absolute',
        top:    lFormularioEnvioPosition.top  ,
        left:   lFormularioEnvioPosition.left ,
        width:  336,
        height: 546,
        //backgroundColor:'#222'
    });

    lMessageBox.show();

    lMessageBoxContainer
        .show()
        .find(".btnConfirmar")
            .focus();

    //passadas as validações e exibições na tela, convertendo para números as propriedades que são int:

    try
    {
        gDadosDaOrdem.Quantidade = GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Quantidade) + "";
        gDadosDaOrdem.Preco      = GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Preco) + "";
        gDadosDaOrdem.Total = GradAux_NumeroFromStringPtBR(gDadosDaOrdem.Total) + "";
        gDadosDaOrdem.PrecoStopStart = GradAux_NumeroFromStringPtBR(gDadosDaOrdem.PrecoStopStart) + "";
    }
    catch(ErroConversao){}

    //guarda o json completo do objeto:

    //lMessageBox.find("input.DadosDaOperacaoJson").val( $.toJSON(gDadosDaOrdem) );

    return false;
}

function Ordem_ValidarEExibirConfirmacao(pSentido)
{
    if(gDadosDaOrdem.AtivoValido)
    {
        //var lFormulario = $("#pnlOrdem_LadoEsquerdo div.pnlOrdem_Formulario");

        var lFormulario = $("#conteudo");

        var lComboTipo = lFormulario.find(".cboOrdem_Tipo");

        // só pra garantir:

        cboOrdem_Tipo_Change(lComboTipo);

        Ordem_CalcularValorDaOperacao(lFormulario.find(".txtOrdem_Quantidade"));

        // valida a ordem:

        Ordem_ValidarOrdem(lComboTipo.val(), lFormulario);

        if(gDadosDaOrdem.Valido)
        {
            // tudo ok, exibe a confirmação:

            Ordem_ExibirConfirmacao(pSentido);
        }
    }
}

function Ordem_ExibirMensagemFinalDeConfirmacao(pTitulo, pMensagem)
{
    var lPainelConteudo = $("#pnlMessageBox");

    var lFormularioEnvio = $(".form-consulta");

    var lFormularioEnvioPosition = lFormularioEnvio.position();

    lPainelConteudo
        .find(".pnlMessageBox_Content")
        .hide()
            .find(".pnlMessageBox_Content_Criticas")
            .hide();
    
    $(".pnlMessageBox_Content_Criticas").css(
    {
//        position:'absolute',
//        //top:lFormularioEnvioPosition.top,
//        //left: lFormularioEnvioPosition.left,
//        width:  628,
//        height: 335,
        textAlign: 'center'
    });

    var lDiv = lPainelConteudo.find(".pnlMessageBox_Content_MensagemQualquer");

    lDiv.find("h5").html(pTitulo);

    lDiv.find(".pnlMessageBox_Content_Criticas").html("<span style='font-size:0.75em; line-height:2em'>" + pMensagem + "</span>").show();

    lDiv.show();

//    lDiv.find(".pnlMessageBox_Content_Criticas").css({
//    
//    position:'absolute',
////        top:lFormularioEnvioPosition.top,
////        left: lFormularioEnvioPosition.left,
//        width:  628,
//        height: 100,

//    });

    lDiv.css(
    {
        position:'absolute',
        top:lFormularioEnvioPosition.top,
        left: lFormularioEnvioPosition.left,
        width:  336,
        height: 546,
        
    });
}


function Ordem_RealizarEnvio()
{
    gDadosDaOrdem.Bolsa = "BOVESPA";

//    if(gDadosDaOrdem.TipoOrdem == CONST_TIPOORDEM_ORDEM)
//    {
        gDadosDaOrdem.Acao = "RegistrarOrdem";
//    }
//    else
//    {
//        gDadosDaOrdem.Acao = "RegistrarStartStop";
//    }

    var lFormularioEnvio = $(".form-consulta");

    var lFormularioEnvioPosition = lFormularioEnvio.position();

    $("#pnlMessageBox")
        .find("div.pnlMessageBox_Content")
        .hide()
        .filter(".pnlMessageBox_Content_Aguarde")
            .show();

    $(".pnlMessageBox_Content_Aguarde").css(
    {
        position:'absolute',
        top:lFormularioEnvioPosition.top,
        left: lFormularioEnvioPosition.left,
        width:  336,
        height: 546,
    });

    window.setTimeout(function()
    {
        $.ajax({
                  url:      "operacoes.aspx"
                , type:     "post"
                , cache:    false
                , dataType: "json"
                , data:     gDadosDaOrdem
                , success:  Ordem_Confirmacao_CallBack
                , error:    Ordem_TratarRespostaComErro
               });

    }, 300);

    return false;
}


function Ordem_Confirmacao_CallBack(pResposta)
{
    //gPerfMon.MarcarMonitoramentoDeProcesso("UsoDaJanela_Ordens", "Ordem_Confirmacao_CallBack ==> Inicio");

    Ordem_HabilitarDesabilitarFormulario(true);

    if(pResposta.TemErro)
    {
        if(pResposta.Mensagem == "Assinatura não confere")
        {
            Ordem_ExibirMensagemFinalDeConfirmacao("Assinatura eletrônica não confere!", "Favor verificar a assinatura e tentar novamente");
        }
        else if (pResposta.Mensagem == "Há Críticas")
        {
            Ordem_ExibirCriticas(pResposta.ObjetoDeRetorno);
        }
        else if(pResposta.Mensagem == "Termo não Assinado")
        {
            Ordem_ExibirMensagemFinalDeConfirmacao("Termo não assinado.", "Você ainda não assinou o <br/>termo de responsabilidade de risco de envio de ordens.<br/> Favor acessar o HomeBroker para enviar sua ordem.");
        }
        else if(pResposta.Mensagem == "Alavancagem não Assinada")
        {
            Ordem_ExibirMensagemFinalDeConfirmacao("Termo de Alavancagem não assinado.", "Você ainda não assinou o <br/>termo de alavancagem financeira.<br/> Favor acessar o HomeBroker para enviar sua ordem.");
        }
        else 
        {
            var lRetorno = { Criticas: [] };

            lRetorno.Criticas.push(pResposta.Mensagem);

            Ordem_ExibirCriticas(lRetorno);
            //GradHomeBroker_TratarRespostaComErro(pResposta);
        }
    }
    else
    {
        //apaga o formulário:
        var lSenha = $(".txtOrdem_Assinatura").val();

        //$(".pnlOrdem_Formulario p input[type!='checkbox']").val("");
        $(".form-consulta input[type!='checkbox']").filter("[type!='submit']").val("")

        if($("#chkOrdem_ManterAssinatura").is(":checked"))
            $(".txtOrdem_Assinatura").val(lSenha);

        //Limpando o formulario


        Ordem_ExibirMensagemFinalDeConfirmacao("Ordem enviada com sucesso.", "Utilize a página de <a href='acompanhamentoordens.aspx'>Acompanhamento de Ordem</a> para verificar a execução.");

        gDadosDaOrdem = null;
        $(".lblTituloAlteracao").html("");
    }

    //gPerfMon.MarcarMonitoramentoDeProcesso("UsoDaJanela_Ordens", "Ordem_Confirmacao_CallBack");
}

function Ordem_TratarRespostaComErro(pResposta)
{
    
}


function Ordem_ExibirCriticas(pObjetoDeRetorno)
{
    var lPainelConteudo = $("#pnlMessageBox");

    var lFormularioEnvio = $(".form-consulta");

    var lFormularioEnvioPosition = lFormularioEnvio.position();

    var lMessageBox = $("div.pnlMessageBox_Content_MensagemQualquer");

    lMessageBox.find("p.pnlOrdem_MensagemDeConfirmacao_BotoesDeConfirmacao").attr("style", "padding-top: 0px; text-align:center");

    var lDivCriticas = lMessageBox.find("div.pnlMessageBox_Content_Criticas");

    var lH5 = lMessageBox.find("h5");

    var lHTML = "<ul class='ListaComBolinhas' style='line-height:1.6em'>";

    if(pObjetoDeRetorno.Criticas && pObjetoDeRetorno.Criticas != null)
    {
        for(var a = 0; a < pObjetoDeRetorno.Criticas.length; a++)
        {
            lHTML += "<li style='text-align:center'>" + pObjetoDeRetorno.Criticas[a] + "</li>";
        }
    }
    else
    {
        lHTML += "<li style='text-align:center'>" + pObjetoDeRetorno.Mensagem + "</li>";
    }

    lHTML += "</ul>";

    lH5.html(pObjetoDeRetorno.MensagemCritica);

    lDivCriticas.html(lHTML).show();

    lMessageBox.show();

    lMessageBox.css({
        position:'absolute',
        top:lFormularioEnvioPosition.top,
        left: lFormularioEnvioPosition.left,
        width:  628,
        height: 335,
    });

    lPainelConteudo.find(".pnlMessageBox_Content_Aguarde").hide();

    return false;
}


var CONST_CLASS_CARREGANDOCONTEUDO   = "CarregandoConteudo";       // div que está com um request ajax em andamento para carregar seu 
var CONST_CLASS_CONTEUDOCARREGADO    = "ConteudoCarregado";        // div que já carregou seu conteúdo
var CONST_CLASS_CAMPOSPREENCHIDOS    = "CamposPreenchidos";        // indica que os campos já foram preenchidos dentro do próprio item selecionado //TODO: ainda não foi implementada a diferença dessa classe estar setada ou não (02-Navegaca.js função GradIntra_Navegacao_ExibirFormulario > linha 655)
var CONST_CLASS_ITEM_SELECIONADO     = "Selecionada";              // indica um item de interface que está selecionado
var CONST_CLASS_ITEM_EXPANDIDO       = "Expandida";                // indica um item de interface que está expandido
var CONST_CLASS_TEMPLATE             = "Template";                 // indica um objeto HTML que serve como "template" pra clonagem de "databind" javascript
var CONST_CLASS_ITEM_ITEMDINAMICO    = "ItemDinamico";             // indica um objeto HTML que foi clonado a partir de um template (usado pra poder selecionar os itens que são "clones" facilmente)
var CONST_CLASS_PICKERDEDATA         = "Picker_Data";              // indica um botão que serve como picker pro controle DatePicker javascript
var CONST_CLASS_BLOQUEADOPORINCLUSAO = "BloqueadoPorNovaInclusao"  // indica controles desabilitados porque está acontecendo uma inclusão de novo item

var gGradSpider_Config_DelayDeRequest = 200;                      // Delay em ms para fazer um request pro servidor (geralmente os de "CarregarHtml") - usado para que o indicador de "aguarde" não pisque muito rápido, o usuário consiga ver uma indicação antes do request ir pro servidor.
var gGradSpider_Config_DuracaoDaAnimacaoDeMensagem = 500;         // Duração em ms da animação de "ExibirMensagem"

var gGradSpider_MensagemOriginal = null;                          // Mensagem inicial que aparece no div de mensagens quando a página é carregada, que deve exibir um "Olá, <usuário>". Assim o js sabe como "voltar" pra mensagem original depois de exibir um alerta por <gGradSpider_MensagemOriginalTimeout> segundos.
var gGradSpider_MensagemOriginalTimeout = 4000;                   // Tempo em ms de duração da mensagem quando usarem a flag para "voltar" pra mensagem original

var gGradSpider_RedirecionarParaLoginEm = 0;                      // Variável de controle de tempo quando o usuário será redirecionado para o login

var gGradSpider_MensagemRedirecionamentoLogin = "Sua sessão expirou. você será <a href='[URL]'>redirecionado para o login</a> em [SEG] segundos";

var gGradSpider_AplicacaoEmModoDeCompatibilidade = false;


function Page_Load() 
{
    Page_Load_CodeBehind();
}

function GradSpider_CarregarJsonVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso, pOpcoesPosCarregamento, pDivDeFormulario) 
{
    ///<summary>Carrega o JSON de uma chamada ajax.</summary>
    ///<param name="pUrl"  type="String">URL que será chamada.</param>
    ///<param name="pDadosDoRequest"  type="Objeto_JSON">Dados para a chamada ajax.</param>
    ///<param name="pCallBackDeSucesso"  type="Função_Javascript">(opcional) Função para chamar em caso de sucesso.</param>
    ///<returns>void</returns>

    $.ajax({
        url: pUrl
            , type: "post"
            , cache: false
            , dataType: "json"
            , data: pDadosDoRequest
            , success: function (pResposta) { GradSpider_CarregarVerificandoErro_CallBack(pResposta, pDivDeFormulario, pCallBackDeSucesso, pOpcoesPosCarregamento); }
            , error: GradSpider_TratarRespostaComErro
    });
}

function GradSpider_CarregarVerificandoErro_CallBack(pResposta, pPainelParaAtualizar, pCallBack, pOpcoesPosCarregamento) 
{
    ///<summary>[CallBack] Função de CallBack para GradIntra_CarregarHtmlVerificandoErro.</summary>
    ///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
    ///<param name="pPainelParaAtualizar"  type="String_ou_Objeto_jQuery">Seletor ou objeto jQuery que irá receber o HTML da resposta ajax.</param>
    ///<param name="pCallBack"  type="Função_Javascript">(opcional) Função para chamar após o carregamento de pPainelParaAtualizar.</param>
    ///<param name="pOpcoesPosCarregamento"  type="Objeto_JSON">(opcional) Opções de execução pós-carregamento. Propriedades suportadas (case-sensitive): {CustomInputs: string[], HabilitarMascaras: bool, HabilitarValidacoes: bool, AtivarToolTips: bool} .</param>
    ///<returns>void</returns>

    if (pResposta != null) 
    {
        if (pResposta.Mensagem) 
        {
            // resposta jSON

            if (pResposta.TemErro) 
            {
                //erro em chamada json
                GradSpider_TratarRespostaComErro(pResposta, pPainelParaAtualizar);
            }
            else {
                //sucesso em chamada JSON
                if (pCallBack && pCallBack != null)
                    pCallBack(pResposta, pPainelParaAtualizar, pOpcoesPosCarregamento);
            }
        }
        else {   // resposta html
            if (pResposta.indexOf('"TemErro":true,') != -1) 
            {   //erro, porém retorno json em chamada html (ocorre com timeout por exemplo)

                GradSpider_TratarRespostaComErro($.evalJSON(pResposta));
            }
            else 
            {   //sucesso em resposta HTML

                if (pResposta.indexOf('<fieldset id="pnlLogin">') != -1) 
                {
                    GradSpider_TratarRespostaComErro({ TemErro: true, Mensagem: "RESPOSTA_SESSAO_EXPIRADA" });
                }
                else 
                {
                    if (pPainelParaAtualizar != null) 
                    {
                        if (!pPainelParaAtualizar.attr) 
                        {
                            //não é objeto jquery

                            if (pPainelParaAtualizar.indexOf('#') !== 0) pPainelParaAtualizar = "#" + pPainelParaAtualizar;

                            pPainelParaAtualizar = $(pPainelParaAtualizar);
                        }

                        pPainelParaAtualizar
                            .html(pResposta)
                            .addClass(CONST_CLASS_CONTEUDOCARREGADO);

                        if (pOpcoesPosCarregamento) 
                        {
                            if (pOpcoesPosCarregamento.CustomInputs) 
                            {
                                $(pOpcoesPosCarregamento.CustomInputs).each(function () 
                                {
                                    $(this + "").customInput();
                                });
                            }

//                            if (pOpcoesPosCarregamento.HabilitarMascaras)
//                                GradIntra_HabilitarInputsComMascaras(pPainelParaAtualizar);

//                            if (pOpcoesPosCarregamento.HabilitarValidacoes)
//                                pPainelParaAtualizar.validationEngine({ showTriangle: false });

//                            if (pOpcoesPosCarregamento.AtivarToolTips)
//                                GradIntra_AtivarTooltips(pPainelParaAtualizar);

//                            if (pOpcoesPosCarregamento.AtivarAutoComplete)
//                                pPainelParaAtualizar.find(".AtivarAutoComplete").ComboBoxAutocomplete();
                        }

                        pPainelParaAtualizar.find("." + CONST_CLASS_PICKERDEDATA).datepicker({ showOn: "button" });

                        pPainelParaAtualizar.find(".pstrength").pstrength();    //ja pra verificar a força da senha

                        gTextBoxes = pPainelParaAtualizar.find("input[type='text'], input[type='radio'], select");

                        if (gTextBoxes.length > 0)
                            gTextBoxes.keydown(GradIntra_FocoProximoInput);

                        $(".ui-datepicker").hide();
                    }

                    if (pCallBack && pCallBack != null)
                        pCallBack(pResposta, pPainelParaAtualizar, pOpcoesPosCarregamento);
                }
            }
        }
    }
}

function GradSpider_TratarRespostaComErro(pResposta, pPainelParaAtualizar) 
{
    ///<summary>Função que trata uma resposta que seja um objeto JSON com (.TemErro == true) ou um XmlHttpResponse com (.status != 200).</summary>
    ///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
    ///<returns>void</returns>

    //desmarca todos os divs que estiverem "carregando conteúdo"
    $("." + CONST_CLASS_CARREGANDOCONTEUDO).removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

    if (pPainelParaAtualizar != null) {
        $(pPainelParaAtualizar).find("[disabled]").attr("disabled", null);
    }

    if (pResposta.status && pResposta.status != 200) {   //resposta veio como HTML, provavelmente erro do servidor 

        var lTexto = pResposta.responseText;

        lTexto = lTexto.replace(/</gi, "&lt;");
        lTexto = lTexto.replace(/>/gi, "&gt;");

        GradSpider_ExibirMensagem("E", "Erro do servidor [" + pResposta.status + " - " + pResposta.statusText + "]                                       ", false, lTexto);
    }
    else {
        if (pResposta.TemErro) 
        {
            if (pResposta.Mensagem == "RESPOSTA_SESSAO_EXPIRADA") 
            {
                GradSpider_ExibirMensagem("E", GradSpider_MensagemDeRedirecionamento(5));

                gGradSpider_RedirecionarParaLoginEm = 4;

                window.setTimeout(GradSpider_TimeoutParaRedirecionarLogin, 1000);
            }
            else 
            {
                GradSpider_ExibirMensagem("E", pResposta.Mensagem, false, pResposta.MensagemExtendida);
            }
        }
        else 
        {
            var lMensagemExtendida = "";

            try 
            {
                lMensagemExtendida += $.toJSON(pResposta) + "\r\n\r\n";
            }
            catch (pErro1) { }

            try 
            {
                lMensagemExtendida += pResposta + "\r\n\r\n";
            }
            catch (pErro2) { }

            try 
            {
                if (pResposta.responseText) 
                {
                    var lTexto = pResposta.responseText;

                    lTexto = lTexto.replace("<", "&lt;", "gi");
                    lTexto = lTexto.replace(">", "&gt;", "gi");

                    lMensagemExtendida += lTexto;
                }
            }
            catch (pErro3) { }

            GradSpider_ExibirMensagem("E", "Erro desconhecido", false, lMensagemExtendida);
        }
    }
}

function GradSpider_ExibirMensagem(pTipo_IAE, pMensagem, pRetornarAoEstadoNormalAposSegundos, pMensagemAdicional) 
{
    ///<summary>Exibe uma mensagem no painel de alerta.</summary>
    ///<param name="pTipo_IAE"  type="String">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
    ///<param name="pRetornarAoEstadoNormalAposSegundos" type="Boolean">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
    ///<param name="pMensagemAdicional" type="String">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
    ///<returns>void</returns>

    // Guarda a mensagem "original" que estava no div se ela ainda não estiver na memória:

    if (gGradSpider_MensagemOriginal == null)
        gGradSpider_MensagemOriginal = $("#pnlMensagem span").html();

    // Multiplicador de pixels por caracter; esse valor é uma aproximação que fica boa tanto pra mensagens curtas quanto longas.
    var lMultiplicador = 6.5;

    // Se a mensagem tiver </ provavelmente inclui tags, então temos que "baixar" o multiplicador porque as tags não aparecem visualmente 
    //(usado por exemplo na mensagem de redirecionamento, que tem um link)
    if (pMensagem.indexOf("</") != -1)
        lMultiplicador = 3.5;

    //233 246 139 e9f68b  amarelo
    //120 204 210 78ccd2  azul
    //212 125 125 d47d7d  vermelho

    // As cores não podem ser definidas na css porque animação de cor não pega os valores da css

    var lAmarelo = "#e9f68b";
    var lAzul = "#78ccd2";
    var lVemelho = "#d47d7d";
    var lOriginal = "#9BA2AC";

    var lClasse = "";

    var lCor = (pTipo_IAE == "A") ? lAmarelo : ((pTipo_IAE == "I") ? lAzul : ((pTipo_IAE == "E") ? lVemelho : lOriginal));

    var lLargura = Math.ceil((pMensagem.length * lMultiplicador));

    // Verifica valores mínimos do comprimento do painel, para que mensagens curtas não fiquem curtas demais
    // (especialmente quando tem erro adicional, que expande o painel maior; se a mensagem de cima for curta, o painel fica curto demais

    if (lCor == lOriginal) 
    {
        if (lLargura < 142) lLargura = 142;  // mínimo 142 pra "original"
    }
    else {
        if (pTipo_IAE == "E") {
            if (lLargura < 400) lLargura = 400;  //minimo 400 pra erro
        }
        else {
            if (lLargura < 342) lLargura = 342;  //minimo 342 pros outros alertas
        }
    }

    //    if (pTipo_IAE == "O")
    //    {
    //        $("#pnlMensagem").addClass("BotaoEscondido");
    //    }

    // Se for uma cor de fundo escura, tem que por uma classe de ForeColor que seja clara:

    if (lCor == lVemelho)
        lClasse = "ForeClara";

    $("button.btnMenuScroller_Direito").animate({ right: (lLargura + 1) }, gGradSpider_Config_DuracaoDaAnimacaoDeMensagem);

    $("#pnlMensagem")
        .addClass("BotaoEscondido")
        .animate({
            backgroundColor: lCor
                     , width: lLargura
        }
                 , gGradSpider_Config_DuracaoDaAnimacaoDeMensagem
                 , function () {
                     if (pMensagemAdicional && pMensagemAdicional != null && pMensagemAdicional != "")
                         GradSpider_ExibirMensagemAdicional(pMensagemAdicional);

                     //$("button.btnMenuScroller_Direito").css( { right: $("#pnlMensagem").width() - 28 } )
                 }
                )
        .children("span")
            .html(pMensagem)
            .attr("class", lClasse);

                 if (pRetornarAoEstadoNormalAposSegundos == true) 
    {
        window.setTimeout(GradSpider_RetornarMensagemAoEstadoNormal, 4000);
    }

    if (pTipo_IAE != "O") 
    {
        $("#pnlMensagem").removeClass("BotaoEscondido")
    }

    //adiciona ao "histórico" de mensagens:

    if (lCor != lOriginal) 
    {
        var lPainel = $("#pnlListaDeMensagens");
        var lNovoLi = lPainel.find("ul li.Template").clone();

        lNovoLi
            .addClass("Tipo_" + pTipo_IAE)
            .removeClass("Template")
            .show()
            .find("label")
                .html(pMensagem);

        if (pMensagemAdicional && pMensagemAdicional != null && pMensagemAdicional != "") 
        {
            lNovoLi.find("textarea").html(pMensagemAdicional);
        }
        else 
        {
            lNovoLi.find("textarea").remove();
            lNovoLi.find("button").remove();
        }

        lPainel
            .find("ul")
                .append(lNovoLi)
            .parent()
            .show();
    }
}

function GradSpider_RetornarMensagemAoEstadoNormal() 
{
    ///<summary>Retorna o painel de alerta ao estado original (quando a página foi carregada).</summary>
    ///<returns>void</returns>

    if ($("#pnlMensagem span").html() == gGradSpider_MensagemOriginal) 
    {
        return;
    }

    if ($("#pnlMensagemAdicional").is(":visible")) 
    {
        // Se tinha mensagem adicional, oculta ela...

        GradIntra_OcultarMensagemAdicional();

        // ... depois espera o tempo da animação pra executar essa própria função novamente. Tem que dar +50 ms pra ter 
        //     uma "distância" segura de tempo e certificarmos que ele já vai estar oculto mesmo, se não cai nesse "if" de novo

        window.setTimeout(GradIntra_RetornarMensagemAoEstadoNormal, (gGradIntra_Config_DuracaoDaAnimacaoDeMensagem + 150));
    }
    else {
        GradSpider_ExibirMensagem("O", gGradSpider_MensagemOriginal, true);
    }

    return false;
}

function GradSpider_TimeoutParaRedirecionarLogin() 
{
    ///<summary>Reduz em 1 segundo o timeout para redirecionar e se chama novamente caso ainda seja > 0.</summary>
    ///<returns>void</returns>

    gConfirmacaoDeSaidaDesnecessaria = true;

    window.onbeforeunload = null;

    if (gGradIntra_RedirecionarParaLoginEm >= 0) 
    {
        $("#pnlMensagem span").html(GradSpider_MensagemDeRedirecionamento(gGradIntra_RedirecionarParaLoginEm));

        gGradIntra_RedirecionarParaLoginEm--;

        window.setTimeout(GradSpider_TimeoutParaRedirecionarLogin, 1000);
    }
    else {
        document.location = GradSpider_RaizDoSite() + "/Login.aspx";
    }
}

function GradSpider_RaizDoSite() 
{
    var lRetorno = document.location.protocol + "//" + document.location.host;

    if (document.location.host.indexOf("localhost") != -1)
        lRetorno += "/Gradual.Spider.Www";
    else
        lRetorno += "";

    return lRetorno;
}

function GradSpider_MensagemDeRedirecionamento(pSegundos) 
{
    ///<summary>Retorna a string da mensagem de redirecionamento.</summary>
    ///<returns>void</returns>

    return gGradSpider_MensagemRedirecionamentoLogin.replace("[SEG]", pSegundos).replace("[URL]", GradSpider_RaizDoSite() + "/Login.aspx");
}

function GradSpider_TratarRespostaComErro(pResposta, pPainelParaAtualizar) 
{
    ///<summary>Função que trata uma resposta que seja um objeto JSON com (.TemErro == true) ou um XmlHttpResponse com (.status != 200).</summary>
    ///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
    ///<returns>void</returns>

    //desmarca todos os divs que estiverem "carregando conteúdo"
    $("." + CONST_CLASS_CARREGANDOCONTEUDO).removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

    if (pPainelParaAtualizar != null) {
        $(pPainelParaAtualizar).find("[disabled]").attr("disabled", null);
    }

    if (pResposta.status && pResposta.status != 200) 
    {   //resposta veio como HTML, provavelmente erro do servidor 

        var lTexto = pResposta.responseText;

        lTexto = lTexto.replace(/</gi, "&lt;");
        lTexto = lTexto.replace(/>/gi, "&gt;");

        GradSpider_ExibirMensagem("E", "Erro do servidor [" + pResposta.status + " - " + pResposta.statusText + "]                                       ", false, lTexto);
    }
    else 
    {
        if (pResposta.TemErro) 
        {
            if (pResposta.Mensagem == "RESPOSTA_SESSAO_EXPIRADA") 
            {
                GradSpider_ExibirMensagem("E", GradSpider_MensagemDeRedirecionamento(5));

                gGradIntra_RedirecionarParaLoginEm = 4;

                window.setTimeout(GradSpider_TimeoutParaRedirecionarLogin, 1000);
            }
            else 
            {
                GradSpider_ExibirMensagem("E", pResposta.Mensagem, false, pResposta.MensagemExtendida);
            }
        }
        else {
            var lMensagemExtendida = "";

            try {
                lMensagemExtendida += $.toJSON(pResposta) + "\r\n\r\n";
            }
            catch (pErro1) { }

            try {
                lMensagemExtendida += pResposta + "\r\n\r\n";
            }
            catch (pErro2) { }

            try {
                if (pResposta.responseText) {
                    var lTexto = pResposta.responseText;

                    lTexto = lTexto.replace("<", "&lt;", "gi");
                    lTexto = lTexto.replace(">", "&gt;", "gi");

                    lMensagemExtendida += lTexto;
                }
            }
            catch (pErro3) { }

            GradSpider_ExibirMensagem("E", "Erro desconhecido                                       ", false, lMensagemExtendida);
        }
    }
}

function GradSpider_ExibirMensagemAdicional(pMensagem) 
{
    ///<summary>Exibe uma mensagem na caixa de texto no painel de erro.</summary>
    ///<param name="pMensagem"  type="String">Mensagem para exibir.</param>
    ///<returns>void</returns>

    if (pMensagem && pMensagem != "") {
        var lPainel = $("#pnlMensagemAdicional");

        if (lPainel.is(":visible"))                                               // Se já estiver visível, só atualiza o conteúdo da textarea 
        {
            lPainel.children("textarea").html(pMensagem);
        }
        else {
            var lLargura = $("#pnlMensagem").width();                            // Se não estiver visível, pega a largura do painel de mensagem pra igualar no painel da mensagem adicional

            lPainel
                .css({
                    width: lLargura
                    //, heigth: 60
                })
                .effect("blind"                                                // Esse é o efeito de animação pra "abrir" o painel
                        , {
                            mode: "show"                                    // Flag pra abrir
                             , easing: "easeOutBounce"                           // Easign que faz o "boing boing" em baixo
                        }
                        , (gGradSpider_Config_DuracaoDaAnimacaoDeMensagem * 2))   // Duração da animação
                .children("textarea").html(pMensagem)
        }
    }
}

function GradSpider_ExibirMensagemFormulario(pAIE, pMensagem, pPainelParaAtualizar) 
{
    //vermelho: alert-danger
    //amarelo: alert-warning
    //verde: alert-success
    var lCss = "";

    switch (pAIE) 
    {
        case "A":
            lCss = "alert-success";
            break;
        case "I":
            lCss = "alert-warning";
            break;
        case "E":
            lCss = "alert-danger";
            break;
    }

    pPainelParaAtualizar
            .html(pMensagem)
                .addClass(lCss)
                    .slideUp(300)
                        .delay(800)
                    .fadeIn(400)
                        .delay(2000)
                    .fadeOut(500);
}

var Spider = {
    notificacoes: {
        aceitar: function (pWho, pIdMsg) {
            //TODO: aceitar
            console.log('Aceitar', pWho, pIdMsg);
        },

        rejeitar: function (pWho, pIdMsg, pEvento) {
            //TODO: rejeitar, mostrar box de motivo + botao ok
            // pEvento pode ser tipo event da janela (vindo pelo growl) ou click (vindo da lista de notificacoes)
            pEvento.stopPropagation();
            
            var growlBox = $(pEvento.target).parents('.acoesBotoes'), //
                motivo = "";

            if (growlBox.find('.motivo').length < 1) {
                motivo = '<span class="motivo"><input type="text" name="motivo" /> <input type="button" class="btn btn-xs btn-default" value="enviar" /></span>';

                growlBox.append(motivo);
            }
            $(growlBox).find('.motivo input[type=text]').click(function (event) {
                event.stopPropagation();
                //console.log('box');
            });
            $(growlBox).find('.motivo input[type=button]').click(function () {
                //console.log('button');
                $(pEvento.target).parents(".jGrowl-notification").find("div.jGrowl-close").trigger('click');
            });

            //console.log('Recusar', pWho, pIdMsg, pEvento);
        },

        openGrowl: function (pListaGrowls){
            
            var that = this;
            //update notificações                       
            if (pListaGrowls.mensagens.length > 0) {
                $.each(pListaGrowls.mensagens, function (index) {
                    var lObjItem = pListaGrowls.mensagens[index];
                    $.jGrowl(lObjItem.texto, {
                        sticky: false,
                        position: 'top-right',
                        theme: 'bg-black',
                        open: function (e, m, o) {
                            var botoes = '<div class="acoesBotoes">';
                            botoes += '<a class="btn btn-xs btn-primary label-success aceitar"><i class="glyph-icon icon-check"></i> Aceitar</a> &nbsp;';
                            botoes += '<a class="btn btn-xs btn-primary label-danger rejeitar"><i class="glyph-icon icon-clock-os"></i>&nbsp;Rejeitar</a>';
                            botoes += '</div>';

                            $(e).append(botoes);
                        },

                        afterOpen: function (e, m, o) {
                            $(e).find(".aceitar").click(function () {
                                that.aceitar(this, lObjItem.id);
                                $(e).children("div.jGrowl-close").trigger('click');
                            });
                            $(e).find(".rejeitar").click(function (event) {
                                //e.stopPropagation();
                                that.rejeitar(this, lObjItem.id, event);
                                //$(e).children("div.jGrowl-close").trigger('click');
                            });
                        }
                    });
                });
           

                if($("#hd-sound .icon-typicons-volume-high").length > 0) {
                    $(".alertSound audio").trigger('play');
                }
            }

        },
        getNotificacoes: function (pJsonNotificacoes) {
            var container = $("#hd-notificacoes ul.notifications-box");

            $.each(pJsonNotificacoes.mensagens, function (index) {
                var obj = pJsonNotificacoes.mensagens[index];
                switch (obj.tipo) {
                    case "oferta":
                        obj.icone = "icon-bullhorn";
                        obj.bg = "green";
                        break;
                    case "warn":
                        obj.icone = "icon-anchor";
                        obj.bg = "danger";
                        break;
                    default:
                        obj.icone = "icon-bell-o";
                        obj.bg = "blue";
                }

                var tpl = '<li class="notificacao' + obj.id + '">';
                tpl += '    <span class="bg-' + obj.bg + ' icon-notification glyph-icon ' + obj.icone + '"></span>';
                tpl += '    <span class="notification-text">' + obj.texto + '</span>';
                tpl += '    <div class="notification-time">' + obj.hora + '</div>';

                if (obj.botoes) {
                    tpl += '    <div class="acoesBotoes">';
                    tpl += '    <a class="btn btn-xs btn-primary label-success aceitar" onclick="Spider.notificacoes.aceitar(this, ' + obj.id + ')"><i class="glyph-icon icon-check"></i> Aceitar</a>';
                    tpl += '    <a class="btn btn-xs btn-primary label-danger rejeitar" onclick="Spider.notificacoes.rejeitar(this, ' + obj.id + ', event)"><i class="glyph-icon icon-clock-os"></i>&nbsp;Rejeitar</a>';
                    tpl += '    </div>';
                }
                tpl += '</li>';

                container.prepend(tpl);
            });
           
        }

    },
    timers: {},
    atualizarIbovespa: function (pIbov) { 
        // espera um obj no formato: 
        //var ibov = {
        //    "delta": -0.44,
        //    "hora": "10:32:55",
        //    "indice": 40000
        //}

        var delta = $("#ibovTicker .ibovDelta"),
            hora = $("#ibovTicker .ibovHora"),
            status = $("#ibovTicker .ibovTendencia"),
            indice = $("#ibovTicker .ibovIndice");

        delta.html(pIbov.delta + "%");
        hora.html(pIbov.hora);

        indice.html(pIbov.indice);

        if (pIbov.delta < 0) {
            status.removeClass("icon-chevron-up font-green");
            status.addClass("icon-chevron-down font-red");

        } else {
            status.removeClass("icon-chevron-down font-red");
            status.addClass("icon-chevron-up font-green");
        }

    }

};


//para chamar: Spider.notificacoes.open("pIdMsg", 123)


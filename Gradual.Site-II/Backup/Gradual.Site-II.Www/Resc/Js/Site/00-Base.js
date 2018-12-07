
var CONST_SEM_USUARIO_NA_SESSAO = "SEM_USUARIO_NA_SESSAO";

var gGradSite_MensagemOriginal = null;
var gGradSite_Config_DuracaoDaAnimacaoDeMensagem = 300;

var gValidationEngineDefaults = { showTriangle: false, promptPosition : "inline", scroll:false, returnIsValid:true, scroll:false, noAnimation:true };


function GradSite_ExibirMensagem(pTipo_IAE, pMensagem, pRetornarAoEstadoNormalAposSegundos, pMensagemAdicional)
{
///<summary>Exibe uma mensagem no painel de alerta.</summary>
///<param name="pTipo_IAE"  type="String">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
///<param name="pRetornarAoEstadoNormalAposSegundos" type="Boolean">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
///<param name="pMensagemAdicional" type="String">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
///<returns>void</returns>

    // Guarda a mensagem "original" que estava no div se ela ainda não estiver na memória:

    if(gGradSite_MensagemOriginal == null)
        gGradSite_MensagemOriginal = $("#pnlMensagem p:eq(0)").html();

    var lClasse = "TipoDeAlerta_" + pTipo_IAE;


    $("#pnlMensagem")
        .css( { opacity: 0, display: "block" } )
        .attr("class", lClasse)
        .animate( { opacity: 1 }, 500 )
        .children("p:eq(0)")
            .html(pMensagem);

    //if($.browser.msie)
    //    $("#pnlMensagemContainer").css( { width: $("body").width(), height: ($("form:eq(0)").height() + 200) } ); 

    $("#pnlMensagemContainer").show();

    if(pRetornarAoEstadoNormalAposSegundos == true)
    {
       window.setTimeout(GradSite_RetornarMensagemAoEstadoNormal, 4000);
    }

    if(pMensagemAdicional != null)
    {
        GradSite_ExibirMensagemAdicional(pMensagemAdicional);
    }
    else
    {
        $("#pnlMensagemAdicional").hide();
    }
}

function GradSite_RecarregarComAncora()
{
    var lURL = document.location.href;

    var lHash = document.location.hash; 

    if(lURL.indexOf("#") != -1)
    {
        lURL = lURL.substr(0, lURL.indexOf("#"));
    }

    if(lURL.indexOf("?") ==  -1)
    {
        lURL += "?";
    }

    lURL += "&r=" + Date.now() + lHash;

    document.location = lURL;
}

function GradSite_BuscarRaiz(pURLAdicional)
{
    var lURL = $("#hidRaiz").val();

    var lLocation = document.location.href.toLowerCase();

    if(pURLAdicional.indexOf(lLocation) != -1)
    {
        pURLAdicional = pURLAdicional.replace(lLocation, "/");
    }

    if (lURL.indexOf("#") != -1)
        lURL = lURL.substr(0, lURL.indexOf("#"));

    if (pURLAdicional && pURLAdicional != "")
    {
        if(pURLAdicional.toLowerCase().indexOf("/gradual.site-ii.www") != -1)
        {
            pURLAdicional = pURLAdicional.substr(pURLAdicional.toLowerCase().indexOf("/gradual.site-ii.www") + 20);
        }

        if(pURLAdicional.indexOf("http") == 0)
        {
            //veio o caminho completo do redirecionamento, usa ele:
            lURL = pURLAdicional;
        }
        else
        {
            lURL += pURLAdicional;
        }
    }

    return lURL;
}

function GradSite_ExibirMensagemAdicional(pMensagem)
{ 
///<summary>Exibe uma mensagem na caixa de texto no painel de erro.</summary>
///<param name="pMensagem"  type="String">Mensagem para exibir.</param>
///<returns>void</returns>

    if(pMensagem && pMensagem != "")
    {
        $("#pnlMensagem").addClass("ComMensagemAdicional");

        var lPainel = $("#pnlMensagemAdicional");

        if(lPainel.is(":visible"))                                               // Se já estiver visível, só atualiza o conteúdo da textarea 
        {
            lPainel.children("textarea").html(pMensagem);
        }
        else
        {
            lPainel.show().children("textarea").html(pMensagem);
        }
    }
}

function GradSite_OcultarMensagemAdicional()
{
///<summary>Oculta o painel de mensagem adicional.</summary>
///<returns>void</returns>

    $("#pnlMensagemAdicional").hide();

    return false;
}

function GradSite_RetornarMensagemAoEstadoNormal()
{
///<summary>Retorna o painel de alerta ao estado original (quando a página foi carregada).</summary>
///<returns>void</returns>

    if($("#pnlMensagemAdicional").is(":visible"))
    {
        // Se tinha mensagem adicional, oculta ela...

        GradSite_OcultarMensagemAdicional();
        
        // ... depois espera o tempo da animação pra executar essa própria função novamente. Tem que dar +50 ms pra ter 
        //     uma "distância" segura de tempo e certificarmos que ele já vai estar oculto mesmo, se não cai nesse "if" de novo

        window.setTimeout(GradSite_RetornarMensagemAoEstadoNormal, (gGradSite_Config_DuracaoDaAnimacaoDeMensagem + 50));
    }
    else
    {
        $("#pnlMensagem")
            .animate(    { opacity: 0 }
                        , 300
                        , function()
                          {
                              $("#pnlMensagemContainer").hide();
                          }
                    );
    }

    var lMsg = $("#pnlMensagem > p:eq(0)").text().toLowerCase();

    if(lMsg.indexOf("sessão expirada") != -1 || lMsg.indexOf("sessão foi terminada") != -1 )
    {
        lnkGradSite_MinhaConta_Logout_Click();
    }

    return false;
}

var gBloquearAjax = false;

function GradSite_CarregarJsonVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso)
{
///<summary>Carrega o JSON de uma chamada ajax.</summary>
///<param name="pUrl"  type="String">URL que será chamada.</param>
///<param name="pDadosDoRequest"  type="Objeto_JSON">Dados para a chamada ajax.</param>
///<param name="pCallBackDeSucesso"  type="Função_Javascript">(opcional) Função para chamar em caso de sucesso.</param>
///<returns>void</returns>

    if(!gBloquearAjax)
    {
        //console.log("GradSite_CarregarJsonVerificandoErro(" + pUrl + ")");

        $.ajax({
                  url:      pUrl
                , type:     "post"
                , cache:    false
                , dataType: "json"
                , data:     pDadosDoRequest
                , success:  function(pResposta) { GradSite_CarregarVerificandoErro_CallBack(pResposta, pCallBackDeSucesso); }
                , error:    GradSite_TratarRespostaComErro
               });

        gBloquearAjax = true;

        //por segurança, caso um request não retorne ou dê problema, a flag é removida de qualquer forma depois de 3 segundos:

        window.setTimeout(function(){ gBloquearAjax = false; }, 3000);
    }
    else
    {
        //console.log("Chamada ajax bloqueada: " + pUrl);
    }
}

function GradSite_CarregarVerificandoErro_CallBack(pResposta, pCallBack)
{
///<summary>[CallBack] Função de CallBack para GradSite_CarregarHtmlVerificandoErro.</summary>
///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
///<param name="pCallBack"  type="Função_Javascript">(opcional) Função para chamar após o carregamento de pPainelParaAtualizar.</param>
///<returns>void</returns>

    //console.log("CarregarVerificandoErro_CallBack: " + JSON.stringify(pResposta) );

    if(pResposta != null)
    {
        if(!pResposta.indexOf)
        {
            // resposta jSON
        
            if(pResposta.TemErro)
            {
                //erro em chamada json
                GradSite_TratarRespostaComErro(pResposta);
            }
            else
            {
                //sucesso em chamada JSON
                if(pCallBack && pCallBack != null)
                    pCallBack(pResposta);  
            }   
        }
        else
        {
            //resposta html
        
            if(pResposta.indexOf('"TemErro":true,') != -1)
            {
                //erro, porém retorno json em chamada html (ocorre com timeout por exemplo)
            
                GradSite_TratarRespostaComErro($.evalJSON(pResposta));
            }
            else
            {
                //sucesso em resposta HTML
            
                if(pResposta.indexOf('<fieldset id="pnlLogin">') != -1)
                {
                    GradSite_TratarRespostaComErro({ TemErro: true, Mensagem: "RESPOSTA_SESSAO_EXPIRADA" });
                }
                else
                {
                    if(pCallBack && pCallBack != null)
                        pCallBack(pResposta);  
                }
            }
        }
    }

    gBloquearAjax = false;
}


function GradSite_TratarRespostaComErro(pResposta)
{
///<summary>Função que trata uma resposta que seja um objeto JSON com (.TemErro == true) ou um XmlHttpResponse com (.status != 200).</summary>
///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
///<returns>void</returns>

    //desmarca todos os divs que estiverem "carregando conteúdo"
    //$("." + CONST_CLASS_CARREGANDOCONTEUDO).removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

    //console.log("GradSite_TratarRespostaComErro: " + JSON.stringify(pResposta) );

    if(pResposta.status && pResposta.status != 200)
    {
        //resposta veio como HTML, provavelmente erro do servidor 
        
        var lTexto = pResposta.responseText;
        
        if(lTexto.indexOf(CONST_SEM_USUARIO_NA_SESSAO) != -1)
        {
            if(!gFlagRedirecionandoParaRelogar)
            {
                GradSite_ExibirMensagem("E", "Sessão expirada, favor efetuar login novamente.", false);

                window.onbeforeunload = null;

                window.onunload = null;
            }
            else
            {
                //se já estiver indo pro redirecionamento, ignora outros callbacks que deram logout
            }
        }
        else
        {
            lTexto = lTexto.replace(/</gi, "&lt;");
            lTexto = lTexto.replace(/>/gi, "&gt;");

            GradSite_ExibirMensagem("E", "Erro do servidor [" + pResposta.status + " - " + pResposta.statusText + "]                                       ", false, lTexto);
        }
    }
    else
    {
        if(pResposta.TemErro)
        {
            if(pResposta.Mensagem == CONST_SEM_USUARIO_NA_SESSAO)
            {
                GradSite_ExibirMensagem("E", "Sessão expirada, favor efetuar login novamente.", false);
            }
            else
            {
                GradSite_ExibirMensagem("E", pResposta.Mensagem, false, pResposta.MensagemExtendida);
            }
        }
        else
        {
            var lMensagemExtendida = "";
            
            try
            {
                lMensagemExtendida += $.toJSON(pResposta) + "\r\n\r\n";
            }
            catch(pErro1){}
            
            try
            {
                lMensagemExtendida += pResposta + "\r\n\r\n";
            }
            catch(pErro2){}

            try
            {
                if(pResposta.responseText)
                {
                    var lTexto = pResposta.responseText;
                    
                    lTexto = lTexto.replace("<", "&lt;", "gi");
                    lTexto = lTexto.replace(">", "&gt;", "gi");
                    
                    lMensagemExtendida += lTexto;
                }
            }
            catch(pErro3){}

            GradSite_ExibirMensagem("E", "Erro desconhecido                                       ", false, lMensagemExtendida);
        }
    }
}

function GradSite_AtivarInputs(pContainerDosInputs)
{
    var lContainerDosInputs = $(pContainerDosInputs);

    lContainerDosInputs.addClass("validationEngineContainer");

    if(lContainerDosInputs.validationEngine)
        lContainerDosInputs.validationEngine(gValidationEngineDefaults);

    //lContainerDosInputs.validationEngine("attach", { onValidationComplete: GradSite_OnValidacaoFormulario });

    lContainerDosInputs.find("[class*='required']").addClass("EstiloCampoObrigatorio");

    lContainerDosInputs.find('.mostrar-teclado').keyboard({
        // *** choose layout ***
        layout: 'qwerty',
        customLayout: { 'default': ['{cancel}'] },
        autoAccept: true,
        preventPaste: true,
        usePreview: false
    }).keydown(function(ev)
    {
        //console.log(ev.keyCode);

        //console.log("Valor senha pré: " + $("#txtGradSite_Login_Senha").val());

        if($("#hidRaiz").val().toLowerCase().indexOf("localhost") == -1)
        {
            var key = ev.keyCode;

            //    enter      backspace     null        espaço       home 35, end, setas 40                 tab                
            if( (key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(ev.keyCode == 13)
            {
                ev.preventDefault();

                $("#btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin").click();
            }
        }
    });

    jQuery.extend($.keyboard.keyaction, 
    {
        enter : function(kb) 
        {
            kb.close(true);

            $(kb.el).parent().find("[data-ClicarNoEnter]").click();

            return false;
        }
    });

    lContainerDosInputs.find('.teclado-virtual').click(function () 
    {
        $(this).parent().find('input').getkeyboard().reveal();
        return false;
    });

    lContainerDosInputs.find('.teclado-virtual-produtos').click(function () 
    {
        $(this).parent().find('input').getkeyboard().reveal();
        return false;
    });

    lContainerDosInputs.find('.teclado-virtual-envioordens').click(function () {
        $(this).parent().find('input').getkeyboard().reveal();
        return false;
    });

    lContainerDosInputs.find("input.PassarParaProximo").keyup(txtPassarParaProximo_KeyUp);

    GradSite_HabilitarInputsComMascaras(lContainerDosInputs);

    $(".DatePicker").datepicker($.datepicker.regional['pt-BR']);

    $(".input-nopaste").each(function()
    {
        var lInput = $(this);

        lInput.attr("onselectstart", "return false");
        lInput.attr("onpaste", "return false")
        lInput.attr("onCopy", "return false")
        lInput.attr("onCut", "return false");
        lInput.attr("onDrag", "return false");
        lInput.attr("onDrop", "return false");
        lInput.attr("autocomplete", "off");
    });

    $("#txtGradSite_Login_Senha").change(function()
    {
        //console.log($("#txtGradSite_Login_Senha").val());
    });
}

function GradSite_HabilitarInputsComMascaras(pContainerDosInputs)
{
    Validacao_HabilitarDataComFormatacao(pContainerDosInputs.find("input.Mascara_Data"));

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CEP"), "99999-999");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_TEL"), "9999-9999");
    
    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CEL"), "99999-9999");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CPF"), "999.999.999-99");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_Hora"), "99:99");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CNPJ"), "99.999.999/9999-99");

    Validacao_HabilitarSomenteNumeros(pContainerDosInputs.find("input.ProibirLetras"));

    Validacao_HabilitarSomenteNumerosEoX(pContainerDosInputs.find("input.ProibirLetrasMenosX"));

    Validacao_HabilitarSomenteNumerosComFormatacao(pContainerDosInputs.find("input.ValorMonetario"));
}


function GradSite_BuscarListaDePropriedadesAPartirDeJSON(pJSON)
{
    var lRetorno = [];

    var lRegexPropriedades = /\"[^\":]*(?=\":)/gi;
    
    var lPropriedade = lRegexPropriedades.exec(pJSON);

    while(lPropriedade != null)
    {
        lPropriedade = lPropriedade[0].substr(1);  //tira a primeira aspas que nao deu pra fazer no backreference da regex =P

        lRetorno.push(lPropriedade);

        lPropriedade = lRegexPropriedades.exec(pJSON);
    }

    return lRetorno;
}


function GradSite_DataDeHoje()
{
    var lData = new Date();
    
    var lDia = "0" + lData.getDate();

    if(lDia.length == 3)
        lDia = lDia.substr(1);

    var lMes = "0" + (lData.getMonth() + 1);
    
    if(lMes.length == 3)
        lMes = lMes.substr(1);

    return lDia + "/" + lMes + "/" + lData.getFullYear();
}

function GradSite_IniciarAnalytics()
{
    //var pageTracker = _gat._getTracker("UA-6374743-1");

    //pageTracker._trackPageview();

    var lTipos = [ "pdf", "xls", "xlsx", "zip", "doc" ];

    var lLinks;

    for(var a = 0; a < lTipos.length; a++)
    {
        lLinks = $("a[href$='" + lTipos[a] + "']");

        lLinks.each(function()
        {
            $(this).click(function()
            {
                var lLink = $(this);

                var lHref = lLink.attr("href").toLowerCase();

                var lExtensao;

                if(lHref.indexOf("/resc/") > 0)
                    lHref = lHref.substr(lHref.indexOf("/resc/"));

                lExtensao = lHref.substr(lHref.indexOf(".") + 1);

                _gaq.push(["_trackEvent", "Downloads", lExtensao, lHref]);
            });
        });
    }
}

function GradSite_ExtirparAcentuacao(pString)
{
    pString = pString.replace(/[áâàã]/gi, "a");
    pString = pString.replace(/[éêè]/gi,  "e");
    pString = pString.replace(/[íîì]/gi,  "i");
    pString = pString.replace(/[óôòõ]/gi, "o");
    pString = pString.replace(/[úûùü]/gi, "u");
    pString = pString.replace(/[ç]/gi,    "c");

    pString = pString.replace(/[ÁÂÀÃ]/gi, "A");
    pString = pString.replace(/[ÉÊÈ]/gi,  "E");
    pString = pString.replace(/[ÍÎÌ]/gi,  "I");
    pString = pString.replace(/[ÓÔÒÕ]/gi, "O");
    pString = pString.replace(/[ÚÛÙÜ]/gi, "U");
    pString = pString.replace(/[Ç]/gi,    "C");

    return pString;
}

function GradSite_ExtirparAcentuacaoEEspaco(pString)
{
    return GradSite_ExtirparAcentuacao(pString).replace(/ /gi, "");
}

function GradSite_CriarElementosHtml5() {

    var tags = [
        { 'tag': 'article', 'display': 'block' },
        { 'tag': 'aside', 'display': 'block' },
        { 'tag': 'audio', 'display': 'auto' },
        { 'tag': 'canvas', 'display': 'auto' },
        { 'tag': 'command', 'display': 'auto' },
        { 'tag': 'datalist', 'display': 'auto' },
        { 'tag': 'details', 'display': 'block' },
        { 'tag': 'embed', 'display': 'auto' },
        { 'tag': 'figcaption', 'display': 'block' },
        { 'tag': 'figure', 'display': 'block' },
        { 'tag': 'footer', 'display': 'block' },
        { 'tag': 'header', 'display': 'block' },
        { 'tag': 'hgroup', 'display': 'block' },
        { 'tag': 'keygen', 'display': 'inline-block' },
        { 'tag': 'mark', 'display': 'auto' },
        { 'tag': 'meter', 'display': 'inline-block' },
        { 'tag': 'nav', 'display': 'block' },
        { 'tag': 'output', 'display': 'inline' },
        { 'tag': 'progress', 'display': 'inline-block' },
        { 'tag': 'rp', 'display': 'auto' },
        { 'tag': 'rt', 'display': 'auto' },
        { 'tag': 'ruby', 'display': 'auto' },
        { 'tag': 'section', 'display': 'block' },
        { 'tag': 'source', 'display': 'auto' },
        { 'tag': 'summary', 'display': 'block' },
        { 'tag': 'time', 'display': 'auto' },
        { 'tag': 'video', 'display': 'auto' },
        { 'tag': 'wbr', 'display': 'auto' }
    ];

    //alert("criando!");

    for (var i = 0; i < tags.length; i++) {
        document.createElement(tags[i].tag);
    }

}

if(typeof String.prototype.trim !== 'function') {
  String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, ''); 
  }
}

var gCharts =
{
    // utility class
    utility:
    {
        chartColors: ["#524646", "#B4837A", "#D1D2D3", "#EE9620", "#F6BC15"],
        chartBackgroundColors: ["#fff", "#fff"],

        applyStyle: function (that) {
            that.options.colors               = gCharts.utility.chartColors;
            that.options.grid.backgroundColor = { colors: gCharts.utility.chartBackgroundColors };
            that.options.grid.borderColor     = gCharts.utility.chartColors[0];
            that.options.grid.color           = gCharts.utility.chartColors[0];
        }
    }
}

//GradSite_CriarElementosHtml5();
function Aux_IniciarGraficoLines(pDivContainer, pDados, pExibirLegenda) 
{

    if (!pExibirLegenda || pExibirLegenda == null)
        pExibirLegenda = false;

    var traffic_sources_line =
    {
        container_element: pDivContainer,
        data: pDados,
        plot: null,     // will hold the chart object
        options:          // chart options
            {
            grid: {
                show: true,
                aboveData: true,
                color: "#D1D1D3",
                labelMargin: 5,
                axisMargin: 0,
                borderWidth: 0,
                borderColor: null,
                minBorderMargin: 5,
                clickable: true,
                hoverable: true,
                autoHighlight: false,
                mouseActiveRadius: 30,
                backgroundColor: {}
            },
            series: {
                grow: { active: false },
                lines: {
                    show: true,
                    fill: false,
                    lineWidth: 2,
                    steps: false,
                    fillColor: null
                },
                points: { show: false }
            },

            legend: {
                position: "se",
                container: $("#chartLegend")
            },
            //yaxis: { min: 0 },
            xaxis: { mode: "time", timeformat: "%b %y", monthNames: ["jan", "fev", "mar", "abr", "mai", "jun", "jul", "ago", "set", "out", "nov", "dez"] },
            colors: ["#524646", "#B4837A", "#D1D2D3", "#EE9620", "#F6BC15"],
            shadowSize: 1,
            tooltip: true,
            tooltipOpts: {
                content: function (xval, yval) {
                    var prefix = "";
                    var sufix = "";
//                    if (g_Aux_TipoSimulacao == "R") {
                        var number = $.formatNumber(yval.toString(), { format: "0.00", locale: "br" }).toString();
                        //prefix = "";
                        //sufix = " %";
                        prefix = "R$ ";
                        sufix = "";
//                    }
                    //if (g_Aux_TipoSimulacao == "V") {
//                        var number = $.formatNumber(yval.toString(), { format: "#,###.00", locale: "br" }).toString();
//                        prefix = "R$ ";
//                        sufix = "";
                    //}
                    var addZero = false;
                    return "%s <BR/>%x : " + (prefix + number + sufix).toString();
                }
                //, xDateFormat: "%0d/%0m/%y"
                    , xDateFormat: "%d/%m/%Y"
                    , onHover: function (flotItem, $tooltipEl) // para posicionar quando estiver fora da área de visualização
                    {
                        var larguraTotal = $(window).width();
                        var posicaoTooltip = $tooltipEl.offset().left + $tooltipEl.width();

                        if (posicaoTooltip >= larguraTotal) {
                            this.shifts.x = -$tooltipEl.width()
                        }
                        else {
                            this.shifts.x = 10
                        }
                    },
                shifts: {
                    x: -30,
                    y: -50
                }
                    , defaultTheme: false
            }
        },

        // initialize
        init: function () {
            // apply styling
            gCharts.utility.applyStyle(this);

            /*
            // generate some data
            this.data.d1 = [[1, 3 + charts.utility.randNum()], [2, 6 + charts.utility.randNum()], [3, 9 + charts.utility.randNum()], [4, 12 + charts.utility.randNum()], [5, 15 + charts.utility.randNum()], [6, 18 + charts.utility.randNum()], [7, 21 + charts.utility.randNum()], [8, 15 + charts.utility.randNum()], [9, 18 + charts.utility.randNum()], [10, 21 + charts.utility.randNum()], [11, 24 + charts.utility.randNum()], [12, 27 + charts.utility.randNum()], [13, 30 + charts.utility.randNum()], [14, 33 + charts.utility.randNum()], [15, 24 + charts.utility.randNum()], [16, 27 + charts.utility.randNum()], [17, 30 + charts.utility.randNum()], [18, 33 + charts.utility.randNum()], [19, 36 + charts.utility.randNum()], [20, 39 + charts.utility.randNum()], [21, 42 + charts.utility.randNum()], [22, 45 + charts.utility.randNum()], [23, 36 + charts.utility.randNum()], [24, 39 + charts.utility.randNum()], [25, 42 + charts.utility.randNum()], [26, 45 + charts.utility.randNum()], [27, 38 + charts.utility.randNum()], [28, 51 + charts.utility.randNum()], [29, 55 + charts.utility.randNum()], [30, 60 + charts.utility.randNum()]];
            this.data.d2 = [[1, charts.utility.randNum() - 5], [2, charts.utility.randNum() - 4], [3, charts.utility.randNum() - 4], [4, charts.utility.randNum()], [5, 4 + charts.utility.randNum()], [6, 4 + charts.utility.randNum()], [7, 5 + charts.utility.randNum()], [8, 5 + charts.utility.randNum()], [9, 6 + charts.utility.randNum()], [10, 6 + charts.utility.randNum()], [11, 6 + charts.utility.randNum()], [12, 2 + charts.utility.randNum()], [13, 3 + charts.utility.randNum()], [14, 4 + charts.utility.randNum()], [15, 4 + charts.utility.randNum()], [16, 4 + charts.utility.randNum()], [17, 5 + charts.utility.randNum()], [18, 5 + charts.utility.randNum()], [19, 2 + charts.utility.randNum()], [20, 2 + charts.utility.randNum()], [21, 3 + charts.utility.randNum()], [22, 3 + charts.utility.randNum()], [23, 3 + charts.utility.randNum()], [24, 2 + charts.utility.randNum()], [25, 4 + charts.utility.randNum()], [26, 4 + charts.utility.randNum()], [27, 5 + charts.utility.randNum()], [28, 2 + charts.utility.randNum()], [29, 2 + charts.utility.randNum()], [30, 3 + charts.utility.randNum()]];
            */

            // make chart
            this.plot = $.plot(this.container_element, this.data, this.options);

            grafico = this;

            /*
            [{
            label: "Visits",
            data: this.data.d1,
            lines: { fillColor: "#fff8f2" },
            points: { fillColor: "#88bbc8" }
            },
            {
            label: "Unique Visits",
            data: this.data.d2,
            lines: { fillColor: "rgba(0,0,0,0.1)" },
            points: { fillColor: "#ed7a53" }
            }],
            this.options);*/
        }
    };

    return traffic_sources_line;
}

var NumConv =
{
    NumToStr: function (pNumber, pCasasDecimais) {
        var lRetorno = "";
        var lStringOriginal = pNumber + "";

        var lParteNum, lParteDec;

        var lSinal = "";

        if (lStringOriginal.indexOf(".") == -1) {
            //número sem parte decimal

            lParteNum = lStringOriginal;
            lParteDec = "";
        }
        else {
            //número com parte decimal

            lParteNum = lStringOriginal.substr(0, lStringOriginal.indexOf("."));
            lParteDec = lStringOriginal.substr(lStringOriginal.indexOf(".") + 1);
        }

        if (lStringOriginal.charAt(0) == "-") {
            lSinal = "-";

            lParteNum = lParteNum.substr(1);
        }

        //var lQtdMil = Math.floor(lParteNum.length / 3);
        var lQtdMilIns = 0;

        for (var a = (lParteNum.length - 1); a >= 0; a--) {
            lRetorno = lParteNum.charAt(a) + lRetorno;

            if ((lRetorno.length - lQtdMilIns) % 3 == 0 && a > 0) {
                lRetorno = this.MilSep + lRetorno;

                lQtdMilIns++;
            }
        }

        if (pCasasDecimais && pCasasDecimais != "" && pCasasDecimais != null && !isNaN(pCasasDecimais)) {
            while (lParteDec.length < pCasasDecimais) {
                lParteDec = lParteDec + "0";
            }

            if (lParteDec > pCasasDecimais) {
                lParteDec = lParteDec.substr(0, pCasasDecimais);
            }
        }

        if (lParteDec != "") {
            lRetorno = lRetorno + this.DecSep + lParteDec;
        }

        lRetorno = lSinal + lRetorno;

        return lRetorno;
    }
    , StrToNum: function (pString) {
        var lStringOriginal = pString.replace(/ /gi, "");

        var lStringFinal = "";

        var lParteNum, lParteDec;

        if (lStringOriginal.indexOf(this.DecSep) == -1) {
            //número sem parte decimal

            lParteNum = lStringOriginal;
            lParteDec = "0";
        }
        else {
            //número com parte decimal

            lParteNum = lStringOriginal.substr(0, lStringOriginal.indexOf(this.DecSep));
            lParteDec = lStringOriginal.substr(lStringOriginal.indexOf(this.DecSep) + 1);
        }

        lParteNum = lParteNum.replace(/\./gi, "").replace(/,/gi, "");

        return new Number(lParteNum + "." + lParteDec);
    }
    , StrToPretty: function (pString, pCasasDecimais) {
        return NumConv.NumToStr(new Number(pString.replace(".", "").replace(",", ".")), pCasasDecimais);
    }
    , MilSep: "."
    , DecSep: ","
}

function ToNumeroAbreviado(number, decPlaces) 
{
    decPlaces = Math.pow(10, decPlaces);

    var abbrev = ["K", "M", "B", "T"];

    for (var i = abbrev.length - 1; i >= 0; i--) {
        var size = Math.pow(10, (i + 1) * 3);

        if (size <= number) {

            number = Math.round(number * decPlaces / size) / decPlaces;

            number += abbrev[i];

            break;
        }
    }

    return number;
}

function valNum_OnKeyDown(evt) 
{
    var key = evt.keyCode;
    //window.status = key;
    //    enter      backspace     null        espaço       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
    if ((key == 13) || (key == 8) || (key == 0) || (key == 32) || (key > 34 && key < 41) || (key == 46) || (key == 9) || (key == 27) || (key > 47 && key < 58) || (key > 95 && key < 106))
    { return true; }
    else
    { if (navigator.userAgent.indexOf("MSIE") == -1) evt.preventDefault(); return false; }
}

function valF2Num_OnKeyDown(evt) {
    var key = evt.keyCode;
    var val = pValidation_getEvtObj(evt).value;

    try {
        var lchar = String.fromCharCode(key);

        //window.status += " " + key + "(" + lchar + ") ";

        //window.status = val.indexOf(",");

    } catch (erro) { }

    try {
        //if(window.status.length > 30)
        //    window.status = "";

        var lchar = String.fromCharCode(key);

        //console.log( key + "(" + lchar + ") " );

    } catch (erro) { }

    //alert(key);
    //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
    if ((key == 13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key == 8) || (key == 0) || (key > 34 && key < 41) || (key == 46) || (key == 9) || (key == 27) || (key > 47 && key < 58) || (key > 95 && key < 106)) {
        //window.status += " a";

        //se for numero, formata:
        if ((key > 47 && key < 58) || (key > 95 && key < 106) || (key == 8) || (key == 110) || (key == 188)) {
            //window.status += " b";

            if (key > 95 && key < 106) key = key - 48;

            var nchar = String.fromCharCode(key);

            if (key == 8) nchar = "<";

            if (key == 110 || key == 188) nchar = ",";

            //window.status += " c(" + nchar + ")";

            return valF2Num_Format(evt, nchar);
        }
        else {
            //window.status += " t";

            return true;
        }
    }
    else {
        if (navigator.userAgent.indexOf("MSIE") == -1) {
            //window.status += " p";
            evt.preventDefault();
        }

        return false;
    }
}

function pValidation_getEvtObj(evt) {
    var ev;

    //pega o objeto da pagina que está recebendo o evento, dependendo do browser
    if (navigator.userAgent.indexOf("MSIE") != -1)
    { ev = event.srcElement; }
    else
    { ev = evt.currentTarget; }

    return ev;
}

function valF2Num_Format(evt, nextChar) {
    var obj = pValidation_getEvtObj(evt);
    var val = obj.value;

    var nval;

    if (nextChar == "<") {
        nval = val.substr(0, val.length - 1);
    }
    else {

        if (obj.selectionStart || obj.selectionStart == 0) {
            var lValorPreCursor, lValorPosCursor;

            //selectionStart é método do firefox pra pegar o texto selecionado (se houver)

            lValorPreCursor = val.substr(0, obj.selectionStart);
            lValorPosCursor = val.substr(obj.selectionEnd);

            nval = lValorPreCursor + nextChar + lValorPosCursor;
        }
        else {
            //no IE só pega o texto selecionado a partir dessa função:
            var lTextoSelecionado = document.selection.createRange().text;

            if (lTextoSelecionado == val) {
                //o cara (provavelmente) selecionou todo o texto na textbox, então substitui:

                nval = nextChar;

                if (nextChar != "")
                    document.selection.empty();

            }
            else {
                nval = val + nextChar;
            }
        }
    }

    //alert(nval);

    /*
    alert("curvalue = " + val);
    alert("nextvalue = " + nval);
    */
    //verifica se está tudo certinho (so numeros, ignorando , e .)

    var v = valF2Num_validate(nval, false);
    if (v) {
        //só formata se nao tiver , inserida:
        if (nval.indexOf(",") == -1) {
            //tira os pontos e virgulas para reformatar:
            nval = nval.replace(/\./gi, "").replace(/,/gi, "");
            //alert(nval);

            //reinsere a pontuacao:
            if (nval.length > 3) nval = strInsert(nval, nval.length - 3, ".");
            if (nval.length > 7) nval = strInsert(nval, nval.length - 7, ".");
            if (nval.length > 11) nval = strInsert(nval, nval.length - 11, ".");
            if (nval.length > 15) nval = strInsert(nval, nval.length - 15, ".");

            //sobrescreve o valor do objeto
            //alert("nval = " + nval);
        }

        obj.value = nval;

        obj.focus();
    }

    //pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);

    //retorna falso pra nao repetir
    if (navigator.userAgent.indexOf("MSIE") == -1) evt.preventDefault();
    return false;
}

function valF2Num_validate(value, forcaInvalido) {
    //a validacao ocorre aqui:

    var v = true;
    var rgx = /\d*/;

    //se nao tiver nada, deixa quieto. se for obrigatorio,
    //vai cair na validacao de obrigatorio. se nao for, ok.                    
    if (value != "") {
        var nvalue = value.replace(/\./gi, "").replace(/,/gi, "");

        //alert("Validando " + nvalue);
        //para o match dos digitos, ignora ponto e virgula
        var s = nvalue.match(rgx);
        v = (s[0].length == nvalue.length);
    }

    if (forcaInvalido == true) v = false;

    return v;
    //chama o display da validacao conforme o resultado (true ou false):
    //pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);
}

function GradAux_Multiplicacao(pTermoDecimal, pTermoInteiro) 
{
    var lTermoA = pTermoDecimal + "";
    var lTermoB = pTermoInteiro + "";

    if (lTermoA.indexOf(",") == -1)
        lTermoA = lTermoA + ",00";

    var lCasas = lTermoA.length - lTermoA.indexOf(",") - 1;

    lTermoA = lTermoA.replace(",", "");

    lTermoA = GradAux_NumeroFromStringPtBR(lTermoA);
    lTermoB = GradAux_NumeroFromStringPtBR(lTermoB + "");

    return lTermoA * lTermoB / Math.pow(10, lCasas);
}

function GradAux_NumeroFromStringPtBR(pString) {
    var lString = pString.replace(/\./gi, "");

    var lRetorno;

    if (lString.indexOf(",") != -1) {
        var lCasas = lString.length - lString.indexOf(",") - 1;

        lRetorno = new Number(new Number(lString.replace(",", "")) / Math.pow(10, lCasas));
    }
    else {
        lRetorno = new Number(lString);
    }

    return lRetorno;
}

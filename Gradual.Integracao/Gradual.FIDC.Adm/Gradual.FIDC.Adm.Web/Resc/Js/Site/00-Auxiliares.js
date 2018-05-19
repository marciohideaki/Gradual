
var g_Aux_TipoSimulacao = "V";

var grafico;

var Aux_Config_DuracaoDaAnimacaoDeMensagem = 500;         // Duração em ms da animação de "ExibirMensagem"

var Aux_MensagemOriginal = null;                          // Mensagem inicial que aparece no div de mensagens quando a página é carregada, que deve exibir um "Olá, <usuário>". Assim o js sabe como "voltar" pra mensagem original depois de exibir um alerta por <gGradIntra_MensagemOriginalTimeout> segundos.

function Aux_UrlComRaiz(pURL)
{
    var lURL = $("#hidRaizDoSite").val();

    if((lURL.charAt(lURL.length -1) == "/") && (pURL.charAt(0) == "/"))
        lURL = lURL.substr(0, lURL.length - 1);

    lURL = lURL + pURL;

    return lURL;
}


function Aux_CarregarHtmlVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso)
{
    
    $.ajax({
              url:      pUrl
            , type:     "post"
            , cache:    false
            , data:     pDadosDoRequest
            , success:  pCallBackDeSucesso
            , error:    Aux_TratarRespostaComErro
           });
}

function Aux_CarregarJsonVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso)
{
    $.ajax({
        url: pUrl
            , type: "post"
            , cache: false
            , dataType: "json"
            , data: pDadosDoRequest
            , success: function (pResposta)
            {
                Aux_CarregarVerificandoErro_CallBack(pResposta, pCallBackDeSucesso);
            }
            , error: Aux_TratarRespostaComErro
    });
}


function Aux_CarregarVerificandoErro_CallBack(pResposta, pCallBack)
{
    if (pResposta != null)
    {
        if (pResposta.Mensagem)
        {
            // resposta jSON

            if (pResposta.TemErro)
            {
                //erro em chamada json
                //GradIntra_TratarRespostaComErro(pResposta, pPainelParaAtualizar);
                alert(pResposta.Mensagem);
            }
            else
            {
                //sucesso em chamada JSON
                if (pCallBack && pCallBack != null)
                    pCallBack(pResposta);
            }
        }
        else
        {   // resposta html
            pCallBack(pResposta);
//            if (pResposta.indexOf('"TemErro":true,') != -1)
//            {   //erro, porém retorno json em chamada html (ocorre com timeout por exemplo)
//                //GradIntra_TratarRespostaComErro($.evalJSON(pResposta));
//            }
        }
    }
}


function Aux_TratarRespostaComErro(pResposta, pPainelParaAtualizar)
{
    if (pResposta.status && pResposta.status != 200)
    {
       //resposta veio como HTML, provavelmente erro do servidor 

        var lTexto = pResposta.responseText;

        lTexto = lTexto.replace(/</gi, "&lt;");
        lTexto = lTexto.replace(/>/gi, "&gt;");

        alert(lTexto);

        //GradIntra_ExibirMensagem("E", "Erro do servidor [" + pResposta.status + " - " + pResposta.statusText + "]                                       ", false, lTexto);
    }
    else
    {
        if (pResposta.TemErro)
        {
            if (pResposta.Mensagem == "RESPOSTA_SESSAO_EXPIRADA")
            {
                //GradIntra_ExibirMensagem("E", GradIntra_MensagemDeRedirecionamento(5));

                //gGradIntra_RedirecionarParaLoginEm = 4;

                //window.setTimeout(GradIntra_TimeoutParaRedirecionarLogin, 1000);
            }
            else
            {
                //GradIntra_ExibirMensagem("E", pResposta.Mensagem, false, pResposta.MensagemExtendida);
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
                if (pResposta.responseText)
                {
                    var lTexto = pResposta.responseText;
                    
                    lTexto = lTexto.replace("<", "&lt;", "gi");
                    lTexto = lTexto.replace(">", "&gt;", "gi");
                    
                    lMensagemExtendida += lTexto;
                }
            }
            catch(pErro3){}

            //GradIntra_ExibirMensagem("E", "Erro desconhecido                                       ", false, lMensagemExtendida);
        }
    }
}

var gCharts = 
{
    // utility class
    utility:
    {
        chartColors: [ "#37a6cd", "#444", "#777", "#999", "#DDD", "#EEE" ],
        chartBackgroundColors: ["#fff", "#fff"],

        applyStyle: function(that)
        {
            that.options.colors = gCharts.utility.chartColors;
            that.options.grid.backgroundColor = { colors: gCharts.utility.chartBackgroundColors };
            that.options.grid.borderColor = gCharts.utility.chartColors[0];
            that.options.grid.color = gCharts.utility.chartColors[0];
        }
    }
}


function Aux_IniciarGraficoPie(pDivContainer, pDados, pFuncaoDeLabelsDaLegenda, pDivLegenda)
{
    // usa o plug-in "Flot"

    var lExibirLegenda = false;

    if(pFuncaoDeLabelsDaLegenda && pFuncaoDeLabelsDaLegenda != null)
    {
        lExibirLegenda = true;
    }
    if (pDivLegenda)
    {
        pDivLegenda = $("#" + pDivLegenda);
    }

    var traffic_sources_pie =
    {
        container_element: pDivContainer,
        // data
        data: pDados,
        /*
        [
        { label: "organic",  data: 60 },
        { label: "direct",   data: 22.1 },
        { label: "referral", data: 16.9 },
        { label: "cpc",      data: 1 }
        ],
        */

        // chart object
        plot: null,

        // chart options
        options: {
            series: {
                pie: {
                    show: true,
                    redraw: true,
                    radius: 1,
                    tilt: 1,
                    label: {
                        show: true,
                        radius: 1,
                        formatter: function (label, series)
                        {
                            return '<div style="font-size:8pt;text-align:center;padding:5px;color:#fff;">' + Math.round(series.percent) + '%</div>';
                        },
                        background: { opacity: 0.8 }
                    }
                }
            },
            legend: {
                show: lExibirLegenda
                , position: "se"
                , margin: [2, 2, ]
                , container: pDivLegenda
                , labelFormatter: pFuncaoDeLabelsDaLegenda
            },
            colors: [],
            grid: { hoverable: true, clickable: true },
            tooltip: true,
            tooltipOpts: {
                content: "<strong>%s</strong>",
                dateFormat: "%y-%0m-%0d",
                onHover: function (flotItem, $tooltipEl) // para posicionar quando estiver fora da área de visualização
                {
                    var larguraTotal = $(window).width();
                    var posicaoTooltip = $tooltipEl.offset().left + $tooltipEl.width();

                    if (posicaoTooltip >= larguraTotal)
                    {
                        this.shifts.x = - $tooltipEl.width()
                    }
                    else
                    {
                        this.shifts.x = 10
                    }
                },
                shifts: {
                    x: 10,
                    y: (-30)

                },

                defaultTheme: false
            }
        },

        // initialize
        init: function ()
        {
            // apply styling
            gCharts.utility.applyStyle(this);

            this.plot = $.plot(this.container_element, this.data, this.options);
            grafico = this;
        }
    };
    
    return traffic_sources_pie;
}



function Aux_IniciarGraficoLines(pDivContainer, pDados, pExibirLegenda)
 {

    if (!pExibirLegenda || pExibirLegenda == null)
        pExibirLegenda = false;

    var traffic_sources_line =
    {
        container_element: pDivContainer,
        data:   pDados,
        plot:   null,     // will hold the chart object
        options:          // chart options
            {
                grid: {
                    show: true,
                    aboveData: true,
                    color: "#3f3f3f",
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
                colors: ["#fff111", "#aff000", "#cd5e37", "#55cfff", "#e5db80"],
                shadowSize: 1,
                tooltip: true,
                tooltipOpts: {
                    content: function(xval, yval)
                    {
                        var prefix = "";
                        var sufix = "";
                        if(g_Aux_TipoSimulacao == "R")
                        {
                            var number = $.formatNumber(yval.toString(), {format:"0.00", locale:"br"}).toString();
                            prefix = "";
                            sufix = " %";
                        }
                        if(g_Aux_TipoSimulacao == "V")
                        {
                            var number = $.formatNumber(yval.toString(), {format:"#,###.00", locale:"br"}).toString();
                            prefix = "R$ ";
                            sufix = "";
                        }
                        var addZero = false;
                        return "%s <BR/>%x : " + (prefix + number + sufix).toString();
                    }
                    , xDateFormat: "%0d/%0m/%y"
                    , onHover: function (flotItem, $tooltipEl) // para posicionar quando estiver fora da área de visualização
                    {
                        var larguraTotal = $(window).width();
                        var posicaoTooltip = $tooltipEl.offset().left + $tooltipEl.width();

                        if (posicaoTooltip >= larguraTotal)
                        {
                            this.shifts.x = - $tooltipEl.width()
                        }
                        else
                        {
                            this.shifts.x = 10
                        }
                    },
                    shifts: {
                        x: -30,
                        y: -50
                    }
                    ,defaultTheme: false
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

var g_xAxisLabelsRentabilidade = ['(%) Mês', '(%) Ano', 'Ult. 12 Meses'];

function xAxisLabelsRentabilidadeGen(x)
{
    return g_xAxisLabelsRentabilidade[x];
}

function Aux_IniciarGraficoBars(pDivContainer, pDados, pExibirLegenda)
 {

    if (!pExibirLegenda || pExibirLegenda == null)
        pExibirLegenda = false;

    var traffic_sources_line =
    {
        container_element: pDivContainer,
        data:   pDados,
        plot:   null,     // will hold the chart object
        options:          // chart options
            {
                grid: {
                    show: true,
                    aboveData: true,
                    color: "#3f3f3f",
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
                    //grow: { active: false }
                    bars:{
                        barWidth: 0.2
                        ,align: "center"
                        //,horizontal: false
                        //,fill: true
                        ,show: true
                        ,order: null
                    }
                    ,stack: false
                }
                
                ,legend: {
                    position: "se",
                    container: $("#chartLegend")
                }
                //,yaxis: { min: 0 }
                //xaxis: { mode: "time", timeformat: "%b %y", monthNames: ["jan", "fev", "mar", "abr", "mai", "jun", "jul", "ago", "set", "out", "nov", "dez"] },
                ,xaxis: {
                    ticks: [[0,'(%) Mês'],[1,'(%) Ano'],[2,'Ult. 12 Meses']]
                         //mode: "time",
                         //transform: xAxisLabelsRentabilidadeGen,
                         //tickFormatter: xAxisLabelsRentabilidadeGen 
                         }
                //,colors: ["#fff111", "#aff000", "#cd5e37", "#55cfff", "#e5db80"]
                ,shadowSize: 1
                ,tooltip: true
                ,tooltipOpts: {
                    content: function(xval, yval)
                    {
//                        var prefix = "";
//                        var sufix = "";
//                        if(g_Aux_TipoSimulacao == "R")
//                        {
//                            var number = $.formatNumber(yval.toString(), {format:"0.00", locale:"br"}).toString();
//                            prefix = "";
//                            sufix = " %";
//                        }
//                        if(g_Aux_TipoSimulacao == "V")
//                        {
//                            var number = $.formatNumber(yval.toString(), {format:"#,###.00", locale:"br"}).toString();
//                            prefix = "R$ ";
//                            sufix = "";
//                        }
//                        var addZero = false;
                        return "%s";//<BR/>%x : " + (prefix + number + sufix).toString();
                    } 
                    ,xDateFormat: "%0d/%0m/%y"
                    ,shifts: {
                        x: -30,
                        y: -50
                    }
                    ,defaultTheme: false
                }
            },

        // initialize
        init: function () {
            // apply styling
            gCharts.utility.applyStyle(this);

            // make chart
            this.plot = $.plot(this.container_element, this.data, this.options);

            grafico = this;
        }
    };

    return traffic_sources_line;
}

function numberWithPoints(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function AUX_numberWithoutPoints(number, decimalPoint, thousandPoint)
{
    if (!number)
    {
        return;
    }

    var parts = number.toString().split(thousandPoint);
    var lRetorno = "";
    for(var i = 0; i < parts.length; i++)
    {
        lRetorno += parts[i];
    }
    //parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //return parts.join(".");
    //return lRetorno.replace(decimalPoint, thousandPoint);
    return lRetorno;
}

function Aux_ValidarSeHorarioValido(pHorario) {
         var re = /^([01]\d|2[0-3]):?([0-5]\d)$/;
         var time = pHorario;
         if (!time.toString().match(re)) {
             //alert("Formato do horário inválido");
             return false;
         }
         return true;
}

$.getDocHeight = function () {
    var D = document;
    return Math.max(Math.max(D.body.scrollHeight, D.documentElement.scrollHeight), Math.max(D.body.offsetHeight, D.documentElement.offsetHeight), Math.max(D.body.clientHeight, D.documentElement.clientHeight));
};

function Aux_ValidarOpcoesBuscaCliente()
{
    var itemSel = $("#cboOpcaoBuscaCli").val();
    var strBusca = $("#txtTermoBusca").val();

    /*if (strBusca.length == 0) {
    alert("Para realizar a busca, informe um valor para '" + itemSel + "'.");
    return false;
    }*/


    switch (itemSel)
    {
        case "CodigoCBLC":
            if (strBusca.indexOf('.') > -1)
            {
                alert("Para busca por Código Bovespa não são aceitos pontos");
                return false;
            }
            if (strBusca.indexOf('-') > -1)
            {
                alert("Para busca por Código Bovespa não são aceitos traços nem o dígito");
                return false;
            }
            if (isNaN(strBusca))
            {
                alert("Para busca por Código Bovespa só são aceitos caracteres numéricos");
                return false;
            }
            break;
        //case "NomeCliente":    
        //    $("#txtTermoBusca").mask("a");    
        //    break;    
        case "CpfCnpj":
            if (strBusca.legth < 11)
            {
                alert("CPF ou CNPJ inválidos");
                return false;
            }
            break;
        case "Email":
            if ((strBusca.indexOf("@") < 0) && (strBusca.indexOf(".") < 0))
            {
                alert("e-mail informado para a busca inválido.");
                return false;
            }
            break;
    }
    return true;
}
function Aux_formatarPorcentagem(cellvalue, options, rowObject)
{
    var lVlrFormatado = cellvalue.replace(',', '.');
    lVlrFormatado = parseFloat(lVlrFormatado) * 100;
    lVlrFormatado = lVlrFormatado + "%";

    return lVlrFormatado;
}


function Aux_Extrair_IdCliente_Pela_URL(pUrl)
{
    if (pUrl.indexOf('idCli=') < 0 && pUrl.indexOf('idCliente=') < 0)
    {
        return;
    }

    var nomeParametro;
    if (pUrl.indexOf('idCli=') > -1)
    {
        nomeParametro = 'idCli=';
    }
    else
    {
        nomeParametro = 'idCliente=';
    }

    pUrl = pUrl.slice(pUrl.indexOf(nomeParametro));

    var end;

    if (pUrl.indexOf("&") > -1)
    {
        end = pUrl.indexOf("&");
    } else
    {
        end = undefined;
    };

    var id = (pUrl.slice(pUrl.indexOf(nomeParametro) + nomeParametro.length, end));
    
    return id;
}

function Aux_Extrair_IdProduto_Pela_URL(pUrl)
{
    // pegar/extrair o id do produto a partir de uma url

    var parNames = ["idProduto=", "idProd="];
    var lIdProd = "";
    $.each(parNames, function (item)
    {
        var nomePar = parNames[item]

        if (pUrl.indexOf(nomePar) > -1)
        {
            lIdProd = pUrl.slice(pUrl.indexOf(nomePar));

            if (lIdProd.indexOf("&") > -1)
            {
                lIdProd = lIdProd.slice(0, lIdProd.indexOf("&"));
            }
            lIdProd = lIdProd.slice((nomePar).length);
        }
    });

    return lIdProd;
}


$.extend({
    handleError: function (s, xhr, status, e)
    {
        // If a local callback was specified, fire it
        if (s.error)
            s.error(xhr, status, e);
        // If we have some XML response text (e.g. from an AJAX call) then log it in the console
        else if (xhr.responseText)
            console.log(xhr.responseText);
    }
});

function Aux_FormataNegativo()
{
    var td = $('.ValorNumerico');
    td.each(function (index)
    {
        var lTd = $(this);
        if (lTd.text().indexOf("-") > -1 || lTd.text().indexOf("(") > -1)
        {
            if (lTd.attr("class").indexOf("negativo") < 0)
            {
                lTd.addClass("negativo");
            }
        }
    }
    );
}


function Aux_inserirUrlRaiz()
{
    // insere url raiz no href quando encontrar a classe 'inserirUrlRaiz' 
    // usado nos widgets
    var lRaiz = $("#hidRaizDoSite").val();
    var elements = $('.inserirUrlRaiz');

    elements.each(function ()
    {
        var that = $(this);
        if (that.attr('href').indexOf(lRaiz) < 0)
        {
            var url = that.attr('href');
            url = lRaiz + url;
            that.attr('href', url);
        }
    });
}

function Aux_SeparaFundosPorCliente(pItens)
{ //separa dados fundos por cliente e retorna o novo objeto
// usado na tela de extratoConsolidado
    var novoObjeto = {
        clientes: []
    };



    for (var i = 0; i < pItens.ObjetoDeRetorno.length; i++)
    {
        var codCliente = pItens.ObjetoDeRetorno[i].CodCliente;
        var isInObject = false;
        var fundo = {};
        var cliente = {
            CodCliente: pItens.ObjetoDeRetorno[i].CodCliente,
            NomeCliente: pItens.ObjetoDeRetorno[i].NomeCliente,
            //DataProcessamento: pItens.ObjetoDeRetorno[i].DataProcessamento,
            Fundos: []
        };

        fundo = pItens.ObjetoDeRetorno[i].Fundo;
        fundo.IOF = pItens.ObjetoDeRetorno[i].IOF;
        fundo.IR = pItens.ObjetoDeRetorno[i].IR;
        fundo.QtdCotas = pItens.ObjetoDeRetorno[i].QtdCotas;
        fundo.ValorBruto = pItens.ObjetoDeRetorno[i].ValorBruto;
        fundo.ValorCota = pItens.ObjetoDeRetorno[i].ValorCota;
        fundo.ValorLiquido = pItens.ObjetoDeRetorno[i].ValorLiquido;

        //var temp = pItens.ObjetoDeRetorno[i].DataProcessamento.substring(6, 24);
        //var dataProc = new Date(temp);
        //dataProc.ToString('yyyy-MM-dd')
        fundo.DataProcessamento = pItens.ObjetoDeRetorno[i].DataProcessamento;
        


        for (var j = 0; j < novoObjeto.clientes.length; j++)
        {

            if (codCliente == novoObjeto.clientes[j].CodCliente)
            {
                novoObjeto.clientes[j].Fundos.push(fundo);
                isInObject = true;
                break;
            }
        }

        if (!isInObject)
        {
            cliente.Fundos.push(fundo);
            novoObjeto.clientes.push(cliente);
        }
    }
    return novoObjeto;

}


function Aux_isCheckBox(pIdCheck)
{
    return $("#" + pIdCheck).is(":checked")
}

function Aux_ValidaData(campo,valor) 
{
	var date  =valor;
	var ardt  =new Array;
	var ExpReg=new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
	ardt      =date.split("/");
	erro      =false;

	if ( date.search(ExpReg)==-1)
    {
		erro = true;
	}
	else if (((ardt[1]==4)||(ardt[1]==6)||(ardt[1]==9)||(ardt[1]==11))&&(ardt[0]>30))
		erro = true;
	else if ( ardt[1]==2) 
    {
		if ((ardt[0]>28)&&((ardt[2]%4)!=0))
			erro = true;
		if ((ardt[0]>29)&&((ardt[2]%4)==0))
			erro = true;
}

	if (erro) 
    {
		alert(""+ valor + " não é uma data válida!!!");
		campo.focus();
		campo.value = "";
		return false;
	}
	return true;
}

function Aux_ExibirMensagem(pTipo_IAE, pMensagem, pRetornarAoEstadoNormalAposSegundos, pMensagemAdicional)
{
    ///<summary>Exibe uma mensagem no painel de alerta.</summary>
    ///<param name="pTipo_IAE"  type="String">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
    ///<param name="pRetornarAoEstadoNormalAposSegundos" type="Boolean">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
    ///<param name="pMensagemAdicional" type="String">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
    ///<returns>void</returns>

    // Guarda a mensagem "original" que estava no div se ela ainda não estiver na memória:

    if (Aux_MensagemOriginal == null)
        Aux_MensagemOriginal = $("#pnlMensagem span").html();

    // Multiplicador de pixels por caracter; esse valor é uma aproximação que fica boa tanto pra mensagens curtas quanto longas.
    var lMultiplicador = 10.5;

    // Se a mensagem tiver </ provavelmente inclui tags, então temos que "baixar" o multiplicador porque as tags não aparecem visualmente 
    //(usado por exemplo na mensagem de redirecionamento, que tem um link)
    if (pMensagem.indexOf("</") != -1)
        lMultiplicador = 3.5;

    //233 246 139 e9f68b  amarelo
    //120 204 210 78ccd2  azul
    //212 125 125 d47d7d  vermelho

    // As cores não podem ser definidas na css porque animação de cor não pega os valores da css

    var lAmarelo  = "#e9f68b";
    var lAzul       = "#37A6CD";
    var lVermelho = "#d47d7d";
    var lOriginal = "#37A6CD";

    var lClasse = "";

    var lCor = (pTipo_IAE == "A") ? lAmarelo : ((pTipo_IAE == "I") ? lAzul : ((pTipo_IAE == "E") ? lVermelho : lOriginal));

    var lLargura = Math.ceil((pMensagem.length * lMultiplicador));

    if (lCor == lVermelho)
        lClasse = "ForeClara";

    //$("button.btnMenuScroller_Direito").animate({ right: (lLargura + 1) }, gGradIntra_Config_DuracaoDaAnimacaoDeMensagem);

    $("#pnlMensagem")
        .addClass("BotaoEscondido")
        .animate({
            backgroundColor: lCor
                     , width: lLargura
        }
                 , Aux_Config_DuracaoDaAnimacaoDeMensagem
                 , function ()
                 {
                     if (pMensagemAdicional && pMensagemAdicional != null && pMensagemAdicional != "")
                         Aux_ExibirMensagemAdicional(pMensagemAdicional);

                     //$("button.btnMenuScroller_Direito").css( { right: $("#pnlMensagem").width() - 28 } )
                 }
                )
        .children("span")
            .html(pMensagem)
            .attr("class", lClasse);

    if (pRetornarAoEstadoNormalAposSegundos == true) {
        window.setTimeout(function () { Aux_RetornarMensagemAoEstadoNormal(pTipo_IAE, pMensagem, false) }, 4000);
    }

    if (pTipo_IAE != "O") {
        $("#pnlMensagem").removeClass("BotaoEscondido")
    }

    //adiciona ao "histórico" de mensagens:

    if (lCor != lOriginal)
    {
        var lPainel = $("#pnlMensagem span");
        //var lNovoLi = lPainel.find("ul li.Template").clone();
        /*
        lNovoLi
            .addClass("Tipo_" + pTipo_IAE)
            .removeClass("Template")
            .show()
            .find("label")
            */

        lPainel.html(pMensagem);
        /*
        if (pMensagemAdicional && pMensagemAdicional != null && pMensagemAdicional != "") {
            lNovoLi.find("textarea").html(pMensagemAdicional);
        }
        else {
            lNovoLi.find("textarea").remove();
            lNovoLi.find("button").remove();
        }
        */
        lPainel
            //.find("ul")
              //  .append(lNovoLi)
            .parent()
            .show();
    }
}

function Aux_ExibirMensagemAdicional(pMensagem)
{
    ///<summary>Exibe uma mensagem na caixa de texto no painel de erro.</summary>
    ///<param name="pMensagem"  type="String">Mensagem para exibir.</param>
    ///<returns>void</returns>

    if (pMensagem && pMensagem != "")
    {
        var lPainel = $("#pnlMensagemAdicional");

        if (lPainel.is(":visible"))                                    // Se já estiver visível, só atualiza o conteúdo da textarea 
        {
            lPainel.children("textarea").html(pMensagem);
        }
        else
        {
            var lLargura = $("#pnlMensagem").width();                  // Se não estiver visível, pega a largura do painel de mensagem pra igualar no painel da mensagem adicional

            lPainel
                .css({
                    width: lLargura
                    //, heigth: 60
                })
                .effect("blind"                                        // Esse é o efeito de animação pra "abrir" o painel
                        , {
                            mode: "show"                               // Flag pra abrir
                             , easing: "easeOutBounce"                 // Easign que faz o "boing boing" em baixo
                        }
                        , (Aux_Config_DuracaoDaAnimacaoDeMensagem * 2))   // Duração da animação
                .children("textarea").html(pMensagem)
        }
    }
}

function Aux_RetornarMensagemAoEstadoNormal()
{
    ///<summary>Retorna o painel de alerta ao estado original (quando a página foi carregada).</summary>
    ///<returns>void</returns>

    //if ($("#pnlMensagem span").html() == Aux_MensagemOriginal) {
    //    return;
    //}

    if ($("#pnlMensagemAdicional").is(":visible")) {
        // Se tinha mensagem adicional, oculta ela...

        Aux_OcultarMensagemAdicional();

        // ... depois espera o tempo da animação pra executar essa própria função novamente. Tem que dar +50 ms pra ter 
        //     uma "distância" segura de tempo e certificarmos que ele já vai estar oculto mesmo, se não cai nesse "if" de novo

        window.setTimeout(Aux_RetornarMensagemAoEstadoNormal, (Aux_Config_DuracaoDaAnimacaoDeMensagem + 150));
    }
    else
    {
        Aux_ExibirMensagem("O", Aux_MensagemOriginal, true);
    }

    return false;
}

function Aux_OcultarMensagemAdicional()
{
    ///<summary>Oculta o painel de mensagem adicional.</summary>
    ///<returns>void</returns>

    $("#pnlMensagemAdicional")
        .effect("blind"
                  , { mode: "hide", easing: "easeInOutExpo" }
                  , (Aux_Config_DuracaoDaAnimacaoDeMensagem)
               );

    return false;
}

function PesquisarFundo(request, response) {
    var lUrl = Aux_UrlComRaiz("/Handlers/ListaFundosHandler.ashx");
    $.ajax
    ({
        url: lUrl + "?NomeFundo=" + $("#ContentPlaceHolder1_txtCarteiraNomeFundo").val(),
        dataType: "json",
        type: "POST",
        success: function (data)
        {
            response($.map(data, function (item) {
                return {
                    label: item.NomeFundo,
                    value: item.CodigoFundo
                }
            }))
        },
        error: function (a, b, c) {
            debugger;
        }
    });
}
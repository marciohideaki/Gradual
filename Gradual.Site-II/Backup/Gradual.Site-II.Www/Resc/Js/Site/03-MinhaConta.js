/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />
function Minhaconta_ExibirAjuda(pTitulo, pIndice)
{
    var lPainelAjuda;

    if(pIndice == null || pIndice == undefined) pIndice = 0;

    lPainelAjuda = $(".pnlAjuda:eq(" + pIndice + ")");

    var lBotaoAjuda = $(".BotaoAjuda");

    if(lPainelAjuda.length > 0)
    {
        if(lPainelAjuda.attr("class").indexOf("Iniciado") == -1)
        {
            var lNovoDiv = $("<div class='pnlAjuda Iniciado'><h4> <span></span><a href='#' onclick='return MinhaConta_FecharAjuda()'>x</a> </h4>  <div class='pnlAjuda_Conteudo'></div></div>");

            lNovoDiv.attr("style", lPainelAjuda.attr("style")); //copia os styles

            if(pTitulo == null || pTitulo == "") pTitulo = "Ajuda";

            lNovoDiv.find("div.pnlAjuda_Conteudo").html(  lPainelAjuda.html()  );
            lNovoDiv.find("h4 span").html(  pTitulo  );

            lPainelAjuda.after(lNovoDiv);

            lPainelAjuda.remove();

            lNovoDiv.show();
        }
        else
        {
            lPainelAjuda.show();
        }
    }

    return false;
}

function MinhaConta_FecharAjuda(pTitulo)
{
    $(".pnlAjuda").hide();

    return false;
}

function ExpandirDetalhesPoupe()
{
    var p = $("p.Detalhes");

    p.toggle();

    if (p.is(":visible"))
    {
        $(".PainelBusca:eq(0)").css({ height: "950px" });
    }
    else
    {
        $(".PainelBusca:eq(0)").css({ height: "210px" });
    }

    return false;
}


function EnibirDivTrocaAtivo()
{
    var control = document.getElementById("ctl00_cphConteudo_divTrocaAtivo");
    control.style.visibility = "hidden";
}


function MinhaConta_ExecutarFakeKeyPress()
{
    //executa um evento de "keyup" nas textboxes de senha para que o script de força da senha rode mesmo o cara estando usando o teclado
    //virtual (que não solta o evento keypress, aí parece que não está avaliando a força da senha

    $(".FormularioPadrao .password").keyup();
}

function MinhaConta_ExibirMensagemDeCadastroNecessario(pPasso)
{
    var lRedirecionamento = document.location.href;

    $("#pnlConteudo").html("<h1>Complete seu Cadastro</h1><p style='text-align:center;padding:4em'>Caso não seja redirecionado, <a href='#'>clique aqui</a></p>");

    lRedirecionamento = lRedirecionamento.substr(0, lRedirecionamento.toLowerCase().indexOf("MinhaConta/") + 11);

    if(pPasso < 4)
    {
        lRedirecionamento += "Cadastro/Cadastro_PF_Passo" + pPasso + ".aspx";

        GradSite_ExibirMensagem("I", "Caro cliente,<br/><br/>Para acessar essa funcionalidade, você precisa completar seu cadastro.<br/><br/>Após fechar essa mensagem, você será redirecionado.");
    }
    else
    {
        lRedirecionamento += "Cadastro/Cadastro_PF_Passo5.aspx";

        GradSite_ExibirMensagem("I", "Para acessar o Minha Conta, envie a documentação necessária e finalize seu cadastro. Caso já tenha enviado, aguarde o processamento de seu cadastro.");
    }

    $("#pnlConteudo").find("a").attr("href", lRedirecionamento);

    $("#pnlMensagem p button.BotaoVerde").bind("click", function()
    {
        window.location = lRedirecionamento;
    });
}

function MinhaConta_GerarGrafico_RendaFixa() {
    var values = [],
        labels = [],
        lColors = [];

    var lTabela, lTR;

    lTbody = $("table.tabela-grafico tbody");

    if (lTbody.length > 0) {
        lTbody.children("tr").each(function () {
            lTR = $(this);

            var lAttr = lTR.attr("data-label");

            if (typeof lAttr !== 'undefined' && lAttr !== false) {
                labels.push(lTR.attr("data-label")); //.children("th").html()  );
                values.push(parseInt(lTR.attr("data-valor"), 10)); //children("td").html(), 10)  );
                lColors.push(lTR.attr("data-color"));
            }
        });

        var r = Raphael("pnlGrafico_RendaFixa");

        r.g.txtattr.font = "10px 'Tahoma', sans-serif";
        r.g.colors = lColors;
        var pie;

        pie = r.g.piechart(   170                        // ponto X do centro do gráfico
                            , 170                        // ponto Y do centro do gráfico
                            , 130                        // raio do gráfico
                            , values
                            , { 
                                  legend: labels         // com legenda
                                , legendcolor: values
                                , legendpos: "east"      // na parte do lado direito
                                // colors: ['#FFDE7B']
                            }
                            );
    }
}

function MinhaConta_GerarGrafico_Fundos() 
{
    var values = [],
        labels = [],
        lColors = [];

    var lTabela, lTR;

    lTbody = $("table.tabela-grafico tbody");

    if (lTbody.length > 0) {
        lTbody.children("tr").each(function () {
            lTR = $(this);

            var lAttr = lTR.attr("data-label");

            if (typeof lAttr !== 'undefined' && lAttr !== false) {
                labels.push(lTR.attr("data-label")); //.children("th").html()  );
                values.push(parseInt(lTR.attr("data-valor"), 10)); //children("td").html(), 10)  );
                lColors.push(lTR.attr("data-color"));
            }
        });

        var r = Raphael("pnlGrafico_Fundos");

        r.g.txtattr.font = "10px 'Tahoma', sans-serif";
        r.g.colors = lColors;
        var pie;

        pie = r.g.piechart(170                        // ponto X do centro do gráfico
                            , 170                        // ponto Y do centro do gráfico
                            , 130                        // raio do gráfico
                            , values
                            , {
                                legend: labels         // com legenda
                                , legendcolor: values
                                , legendpos: "south"      // na parte do lado direito
                                // colors: ['#FFDE7B']
                            }
                            );
    }
}

function MinhaConta_GerarGrafico_RendaVariavel() 
{
    var values = [],
        labels = [],
        lColors = [];

    var lTabela, lTR;

    lTbody = $("table.tabela-grafico tbody");

    if (lTbody.length > 0) {
        lTbody.children("tr").each(function () {
            lTR = $(this);

            var lAttr = lTR.attr("data-label");

            if (typeof lAttr !== 'undefined' && lAttr !== false) {
                labels.push(lTR.attr("data-label")); //.children("th").html()  );
                values.push((parseInt(lTR.attr("data-valor"))/100)); //children("td").html(), 10)  );
                lColors.push(lTR.attr("data-color"));
            }
        });

        var r = Raphael("pnlGrafico_RendaVariavel");

        r.g.txtattr.font = "10px 'Tahoma', sans-serif";
        r.g.colors = lColors;
        var pie;

        pie = r.g.piechart(   170                       // ponto X do centro do gráfico
                            , 170                        // ponto Y do centro do gráfico
                            , 130                        // raio do gráfico
                            , values
                            , {   legend: labels     // com legenda
                                , legendcolor: values
                                , legendpos: "east"    // na parte de baixo
                                // colors: ['#FFDE7B']
                            }
                            );


//        pie.hover(function () {
//            this.sector.stop();
//            this.sector.scale(1.1, 1.1, this.cx, this.cy);
//            if (this.label) {
//                this.label[0].stop();
//                this.label[0].scale(1.5);
//                this.label[1].attr({ "font-weight": 800 });
//            }
//        }, function () {
//            this.sector.animate({ scale: [1, 1, this.cx, this.cy] }, 500, "bounce");
//            if (this.label) {
//                this.label[0].animate({ scale: 1 }, 500, "bounce");
//                this.label[1].attr({ "font-weight": 400 });
//            }
//        });
    }
}

function MinhaConta_GerarGrafico()
{
    var values = [],
        labels = [],
        lColors = [];

    var lTabela, lTR;

    lTbody = $("table.tabela-grafico tbody");

    if(lTbody.length > 0)
    {
        lTbody.children("tr").each(function () {
            lTR = $(this);

            var lAttr = lTR.attr("data-label");

            if (typeof lAttr !== 'undefined' && lAttr !== false) 
            {
                labels.push(lTR.attr("data-label"));
                values.push((parseInt(lTR.attr("data-valor")) /100) ); 
                //values.push(lTR.attr("data-valor")); 
                lColors.push(lTR.attr("data-color"));
            }
        });

        var r = Raphael("pnlGrafico_Carteira");

        //lColors = ["#B4837A", "#524646"];
        //values = [83.25, 19.3];
        r.g.txtattr.font = "10px 'Tahoma', sans-serif";
        r.g.colors = lColors;
        var pie;

        pie = r.g.piechart(   170                       // ponto X do centro do gráfico
                            , 170                        // ponto Y do centro do gráfico
                            , 130                        // raio do gráfico
                            , values
                            , { legend: labels           // com legenda
                                , legendcolor: values
                                , legendpos: "east"    // na parte de baixo
                                // colors: ['#FFDE7B']
                                }
                            );
         

//        pie.hover(function () {
//            this.sector.stop();
//            this.sector.scale(1.1, 1.1, this.cx, this.cy);
//            if (this.label) {
//                this.label[0].stop();
//                this.label[0].scale(1.5);
//                this.label[1].attr({"font-weight": 800});
//            }
//        }, function () {
//            this.sector.animate({scale: [1, 1, this.cx, this.cy]}, 500, "bounce");
//            if (this.label) {
//                this.label[0].animate({scale: 1}, 500, "bounce");
//                this.label[1].attr({"font-weight": 400});
//            }
//        });
    }
}




/*  Event Handlers  */

function cboCalculadoraIR_TipoDePlano_Change()
{
    var lCombo = $("#ContentPlaceHolder1_cboTipoDePlano");

    if(lCombo.val() == "Aberto")
    {
        lCombo.closest("fieldset").find("p.DataFim").hide();
    }
    else
    {
        lCombo.closest("fieldset").find("p.DataFim").show();
    }
}




function btnCalculadoraIR_Aderir_Validar(pSender)
{
    var lValido = GradSite_ValidacaoComDoubleCheck( $(".PaginaConteudo") );

    return lValido;
}

function btnBaixarAtualizador_Click(pSender)
{
    document.location = "http://atualizador.gradualinvestimentos.com.br:8712";

    return false;
}

function btnTesouroCompra_Click(pSender, pAba) 
{
    $("#ContentPlaceHolder1_Posicao_Acordeon_Selecionado").val("s");

    $("#ContentPlaceHolder1_Posicao_Aba_Selecionada").val(pAba);
}

function MinhaConta_RendaFixa_TD_LoadAccordeon_Click( pAba) 
{
    $("#AcordeonTesouroDireto").click();

    $("#li_"+ pAba).click();
}

function btnSimularFundos_Click(pSender, pAba) 
{
    $("#ContentPlaceHolder1_Posicao_Aba_Simular_Selecionada").val(pAba);
}

function btnRelatoriosFundos_Click(pSender, pAba) 
{
    $("#ContentPlaceHolder1_Posicao_Aba_Simular_Selecionada").val(pAba);
}

function btnAcompanhamentoHistorico_Click(pSender, pAba) 
{
    $("#ContentPlaceHolder1_Operacoes_Aba_selecionada").val(pAba);
}

function MinhaConta_Operacoes_Fundos_Simular_LoadAccordeon_Click(pAba) 
{
    //$("#AcordeonTesouroDireto").click();

    $("#li_" + pAba).click();
}

function MinhaConta_Operacoes_Fundos_Relatorios_LoadAccordeon_Click(pAba) 
{
    //$("#AcordeonTesouroDireto").click();

    $("#li_" + pAba).find("div.acordeon-opcao").click();
}
function MinhaConta_Operacoes_Acompanhamento_Click(pAba) 
{
    $("#li_" + pAba).click();
}

function MinhaConta_GerarGrafico_Fundos_ExtratoMensal() 
{
    var lDados =[];
    var lTabela, lTR;
    var lObjeto = [];

    lTbody = $(".tabela_grafico_Extrato tbody");

    if (lTbody.length > 0) 
    {
        lTbody.children("tr").each(function () {
            lTR = $(this);

            var lAttr = lTR.attr("data-label");

            if (typeof lAttr !== 'undefined' && lAttr !== false) 
            {
                var lValorData = [];

                var lObjetoArray = $.parseJSON(lTR.attr("data-valor"));

                lObjeto.push({ label: lTR.attr("data-label"), data: lObjetoArray});
            }
        });

        var lGrafico = Aux_IniciarGraficoLines($("#chrtRentabilidade"), lObjeto, true);
        lGrafico.init();
    }
}

function MinhaConta_GerarGrafico_Fundos_Simular() 
{
    var lDados = [];
    var lTabela, lTR;
    var lObjeto = [];

    lTbody = $(".tabela_grafico_Simular tbody");

    if (lTbody.length > 0) {
        lTbody.children("tr").each(function () {
            lTR = $(this);

            var lAttr = lTR.attr("data-label");

            if (typeof lAttr !== 'undefined' && lAttr !== false) 
            {
                var lValorData = [];

                var lObjetoArray = $.parseJSON(lTR.attr("data-valor"));

                lObjeto.push({ label: lTR.attr("data-label"), data: lObjetoArray });
            }
        });

        var lGrafico = Aux_IniciarGraficoLines($("#chrtRentabilidadeSimular"), lObjeto, true);
        lGrafico.init();
    }
}

///************************Aba simular da página de operações do minha conta********************************************//


function Simular_Produto_Add(pIdProduto, pNomeProduto) 
{ // usado nas telas 'ComparaProdutos.aspx' e 'MatrizProduto.aspx'

    if ($("#produtos_adicionados").children().length == 5) 
    {
        alert("Máximo de 5 fundos para comparação.");
        return false;
    }

    $("#Simular_Produto_Lista_Add").find("#fundo_" + pIdProduto).detach();

    //var hddSelecao = $("#ContentPlaceHolder1_OperacoesSimular1_hddSelect_produtos_adicionados");
    var hddSelecao = $("#ContentPlaceHolder1_hddSelect_produtos_adicionados"); 
                             
    var hddSelecionados = hddSelecao.val();

    hddSelecionados += "[" + pIdProduto + "]";

    hddSelecao.val(hddSelecionados);

    var htmlElem = "<li id='produtos_adicionados_escolhidos_" + pIdProduto + "' onclick='Simular_Produto_Remove(" + pIdProduto + ");return false;' >" + pNomeProduto;
    htmlElem += "</li>";

    $("#produtos_adicionados").append(htmlElem);
}

function Simular_Produto_Remove(pIdProduto) 
{
    var lVoltar = $("#produtos_adicionados").find("#produtos_adicionados_escolhidos_" + pIdProduto);

    var lNomeProduto = lVoltar.text();

    //var hddSelecao = $("#ContentPlaceHolder1_OperacoesSimular1_hddSelect_produtos_adicionados");
    var hddSelecao = $("#ContentPlaceHolder1_hddSelect_produtos_adicionados"); 
    var hddSelecionados = hddSelecao.val().replace("["+ pIdProduto + "]","");

    hddSelecao.val(hddSelecionados);
   
    $("#produtos_adicionados_escolhidos_" + pIdProduto).detach();

    var lliVoltar = "<li id='fundo_" + pIdProduto + "' onclick='Simular_Produto_Add(\"" + pIdProduto + "\", \"" + lNomeProduto + "\");return false;'>" + lNomeProduto + "</li>";

    $("#Simular_Produto_Lista_Add").append(lliVoltar);
}

function MinhaConta_Fundos_Simular_RecarregaGridEscolhidos() 
{
    //var hddSelecao = $("#ContentPlaceHolder1_OperacoesSimular1_hddSelect_produtos_adicionados").val();
    var hddSelecao = $("#ContentPlaceHolder1_hddSelect_produtos_adicionados").val();
    

    var lSelecao =  hddSelecao.replace(/\[|\]/g, ' ');

    var ArrSelecao = lSelecao.split(' ');

    //$.parseJSON(ArrSelecao).each(function () 
    for (i=0; i < ArrSelecao.length; i++)
    {
        if (ArrSelecao[i] != '') 
        {
            var pIdProduto = ArrSelecao[i];
            
            var Produto = $("#Simular_Produto_Lista_Add").find("#fundo_" + pIdProduto);
            
            var pNomeProduto = Produto.html();
            
            Produto.detach();

            var htmlElem = "<li id='produtos_adicionados_escolhidos_" + pIdProduto + "' onclick='Simular_Produto_Remove(" + pIdProduto + ");return false;' >" + pNomeProduto;
            htmlElem += "</li>";

            $("#produtos_adicionados").append(htmlElem);
        }
    }
}

function Mostra_divAjuda( pDiv) 
{
    $("."+pDiv).show();
}

function Esconde_divAjuda( pDiv)
{
    $("."+pDiv).hide();
}
 
function lnkAcordeon_Conteudo_Tesouro_Direto_Extrato(pSender, event)
{
    $("#li_Extrato").click();
}
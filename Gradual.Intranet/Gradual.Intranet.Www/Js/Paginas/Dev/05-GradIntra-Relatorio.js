/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="03-GradIntra-Cadastro.js" />

var gIdClienteParametroSelecionado; //--> Item IdClienteParametro selecionado.

var gObjetoChamadorLimitePorClienteAtualizando;

function GradIntra_Relatorio_OcultarExibirFiltros(pDivDeFormulario, pCodigoDoRelatorio)
{
    var lParagrafoBotao = pDivDeFormulario.find(".btnBusca").closest("p");

    var lblTipoPessoa = $("label[for='cboClientes_FiltroRelatorio_TipoPessoa']");

    pDivDeFormulario.find("p[class^=R0]").hide();

    if(pCodigoDoRelatorio != "")
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = pCodigoDoRelatorio;
        
        pDivDeFormulario.find("p." + pCodigoDoRelatorio).show();

        lParagrafoBotao.show();
    }
    else
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = null;

        lParagrafoBotao.hide();
    }
    
    if (pCodigoDoRelatorio == "R001"
    ||  pCodigoDoRelatorio == "R002"
    ||  pCodigoDoRelatorio == "R004"
    ||  pCodigoDoRelatorio == "R005"
    ||  pCodigoDoRelatorio == "R008"
    ||  pCodigoDoRelatorio == "R009"
    ||  pCodigoDoRelatorio == "R011"
    ||  pCodigoDoRelatorio == "R015"
    ||  pCodigoDoRelatorio == "R023")
    {
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_7Linhas");
    }
    else if (pCodigoDoRelatorio == "R003"
         || pCodigoDoRelatorio == "R007"
         ||  pCodigoDoRelatorio == "R006"
         )
    {
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_5Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_7Linhas");
    }
    else if (pCodigoDoRelatorio == "R017"
         ||  pCodigoDoRelatorio == "R018"
         ||  pCodigoDoRelatorio == "R019"
         )
    {
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_7Linhas");
    }
    
    else
    {
        pDivDeFormulario.addClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_7Linhas");
    }

    if(pCodigoDoRelatorio == "R002" || pCodigoDoRelatorio == "R003")
    {
        lblTipoPessoa.css( { "margin-left": "-3.1em" });
    }
    else
    {
        lblTipoPessoa.attr("style", null).css( { "width": "7em" });
    }

//    if (pCodigoDoRelatorio == "R018") 
//    {
//        var labelFiltroRelatorio_Assessor = $("label[for='cmbBusca_FiltroRelatorio_Assessor']");
//        var comboFiltrorelatorio_Assessor = $("#cmbBusca_FiltroRelatorio_Assessor");
//        
//        labelFiltroRelatorio_Assessor.html("");
//        labelFiltroRelatorio_Assessor.css({ "width": "0" });
//        
//        comboFiltrorelatorio_Assessor.css({ "width": "175px" });
//    }
//    else {
//        var labelFiltroRelatorio_Assessor = $("label[for='cmbBusca_FiltroRelatorio_Assessor']");
//        var comboFiltrorelatorio_Assessor = $("#cmbBusca_FiltroRelatorio_Assessor");

//        labelFiltroRelatorio_Assessor.html("Assessor:");
//        labelFiltroRelatorio_Assessor.css({ "width": "17em" });

//        comboFiltrorelatorio_Assessor.css({ "width": "250px" });
//    }
}

function GradIntra_RelatorioDBM_OcultarExibirFiltros(pDivDeFormulario, pCodigoDoRelatorio)
{
    var lParagrafoBotao = pDivDeFormulario.find(".btnBusca").closest("p");

    var lblTipoPessoa = $("label[for='cboClientes_FiltroRelatorio_TipoPessoa']");

    pDivDeFormulario.find("p[class^=R0]").hide();

    if (pCodigoDoRelatorio != "")
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = pCodigoDoRelatorio;

        pDivDeFormulario.find("p." + pCodigoDoRelatorio).show();

        lParagrafoBotao.show();
    }
    else
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = null;

        lParagrafoBotao.hide();
    }

    if (pCodigoDoRelatorio == "R001")
    {
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_5Linhas");
    }
    else if (pCodigoDoRelatorio == "R002"
         || (pCodigoDoRelatorio == "R005")
         || (pCodigoDoRelatorio == "R006")
         || (pCodigoDoRelatorio == "R007"))
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
    }
    else if (pCodigoDoRelatorio == "R003"
         || (pCodigoDoRelatorio == "R004"))
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
    }
}

function GradInfra_Relatorio_ImprimirFichaDuc()
{
    var lAcao = "";

    if (gGradIntra_Cadastro_ItemSendoEditado.TipoCliente == "PJ")
        lAcao = "ImprimirFichaDuc_PJ";
    else 
    {
        lAcao = "ImprimirFichaDuc_PF";
    }
    
    var lDados = {Acao: lAcao, Id: gGradIntra_Cadastro_ItemSendoEditado.Id, CodBov: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa, CodBMF: gGradIntra_Cadastro_ItemSendoEditado.CodBMF, Tipo: gGradIntra_Cadastro_ItemSendoEditado.TipoCliente };
    
    GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Acoes/VerFichaCadastral.aspx"
                                         , lDados
                                         , GradInfra_Relatorio_ImprimirFichaDuc_Callback);

   return false;
}

function GradInfra_Relatorio_ImprimirFichaCambio() {
    var lAcao = "";

    if (gGradIntra_Cadastro_ItemSendoEditado.TipoCliente == "PJ")
        lAcao = "ImprimirFichaCambio_PJ";
    else {
        lAcao = "ImprimirFichaCambio_PF";
    }

    var lDados = { Acao: lAcao, Id: gGradIntra_Cadastro_ItemSendoEditado.Id, CodBov: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa, CodBMF: gGradIntra_Cadastro_ItemSendoEditado.CodBMF, Tipo: gGradIntra_Cadastro_ItemSendoEditado.TipoCliente };

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/VerFichaCadastral.aspx"
                                         , lDados
                                         , GradInfra_Relatorio_ImprimirFichaDuc_Callback);

    return false;
}

function GradInfra_Relatorio_Contrato_PF() 
{
    var lAcao = "VisualizarTermo_PF";

    var lDados = { Acao: lAcao, Id: gGradIntra_Cadastro_ItemSendoEditado.Id, CodBov: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa, CodBMF: gGradIntra_Cadastro_ItemSendoEditado.CodBMF, Tipo: gGradIntra_Cadastro_ItemSendoEditado.TipoCliente };

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/VerFichaCadastral.aspx"
                                         , lDados
                                         , GradInfra_Relatorio_Contrato_PF_Callback);

    return false;
}

function GradInfra_Relatorio_Contrato_PF_Callback(pResposta) 
{
    window.open(pResposta.Mensagem);

    GradIntra_ExibirMensagem("I", "Termo de contrato gerado com sucesso.");
}

function GradInfra_Relatorio_ImprimirFichaDuc_Callback(pResposta)
{
   
    if (pResposta.ObjetoDeRetorno.StatusAtualizaPasso)
    {
        gGradIntra_Cadastro_ItemSendoEditado.Passo = 3;
        GradIntra_Clientes_ReverterTipoDeClienteAtual( gGradIntra_Cadastro_ItemSendoEditado.TipoCliente,  true, gGradIntra_Cadastro_ItemSendoEditado);
    }

    window.open(pResposta.Mensagem);

    GradIntra_ExibirMensagem("I", "Ficha cadastral gerada com sucesso.");
}

function GradIntra_Relatorio_Risco_OcultarExibirFiltros(pDivDeFormulario, pCodigoDoRelatorio)
{
    var lEntidadeBtnBusca = pDivDeFormulario.find(".btnBusca");

    if (pCodigoDoRelatorio == "R006")
    {
        lEntidadeBtnBusca.prop("disabled", true);
    }
    else
    {
        lEntidadeBtnBusca.prop("disabled", false);
    }

    var lParagrafoBotao = lEntidadeBtnBusca.closest("p");

    pDivDeFormulario.find("p[class^=R0]").hide();

    if(pCodigoDoRelatorio != "")
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = pCodigoDoRelatorio;
        
        pDivDeFormulario.find("p." + pCodigoDoRelatorio).show();

        lParagrafoBotao.show();
    }
    else
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = null;

        lParagrafoBotao.hide();
    }
    
    if (pCodigoDoRelatorio == "R002"
    || (pCodigoDoRelatorio == "R007"))
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
    }
    else if (pCodigoDoRelatorio == "R004"
         || (pCodigoDoRelatorio == "R005")
         || (pCodigoDoRelatorio == "R006"))
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
    }
    else if (pCodigoDoRelatorio == "R009")
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_5Linhas");
    }
    else
    {
        pDivDeFormulario.addClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
    }
}

function GradIntra_Relatorio_Risco_ExibirDivAlteracaoDataVencimento(pSender)
{
    gObjetoChamadorLimitePorClienteAtualizando = $(pSender);

    gIdClienteParametroSelecionado = gObjetoChamadorLimitePorClienteAtualizando.closest("tr")
                                                                               .find("#txtRelatorio_LimitePorCliente_IdClienteParametro")
                                                                               .val();

    $("#divRelatorio_LimitePorCliente_DataAtualizacao").show();

    return false;
}

function GradIntra_Relatorio_Risco_DataVencimentoCancelar_Click(pSender)
{
    $(pSender).closest("#divRelatorio_LimitePorCliente_DataAtualizacao").hide();

    gIdClienteParametroSelecionado = null;

    return false;
}

function GradIntra_Relatorio_Risco_DataVencimentoAtualizar_Click(pSender)
{
    var lDataAtualizacao = $(pSender).closest("p").find("#txtRelatorio_LimitePorCliente_DataAtualizacao").val();

    var lDados = {Acao: "AtualizarDataVencimento", IdClienteParametroSelecionado: gIdClienteParametroSelecionado, DataValidadeAtualizada: lDataAtualizacao };

    GradIntra_CarregarJsonVerificandoErro( "Risco/Relatorios/R005.aspx"
                                         , lDados
                                         , GradIntra_Relatorio_Risco_DataVencimentoAtualizar_Callback);

    gIdClienteParametroSelecionado = null;

    return false;
}

function GradIntra_Relatorio_Risco_DataVencimentoAtualizar_Callback(pResposta)
{
    $("#divRelatorio_LimitePorCliente_DataAtualizacao").hide();
    
    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);

    var lDataAtualizada = $("#txtRelatorio_LimitePorCliente_DataAtualizacao").val();

    gObjetoChamadorLimitePorClienteAtualizando.closest("tr")
                                              .find(".Relatorio_LimitePorCliente_DtValidade")
                                              .html(lDataAtualizada);

    gObjetoChamadorLimitePorClienteAtualizando = null;
}

function GradIntra_Relatorio_Risco_HabilitarConsultaRelatorio(pSender)
{
    if ($("#cboRisco_FiltroRelatorioRisco_Relatorio").val() != "R006")
        return false;

    var lContemTextoPesquisa = false;
    var lNegativoEmSelecionado = false;
    var lAssessorSelecionado = false;

    lContemTextoPesquisa = $("#txbRisco_BuscarPor").val() != "" && $("#txbRisco_BuscarPor").val().length > 2;
    
    $(".ckbBusca_FiltroRelatorioRisco").each(function()
    {
        lNegativoEmSelecionado = lNegativoEmSelecionado || $(this).is(":checked");        
    });
        
    lAssessorSelecionado = $("#cmbBusca_FiltroRelatorioRisco_Assessor").val() != "";

    if ((lContemTextoPesquisa)
    || ((lNegativoEmSelecionado))
    || ((lAssessorSelecionado)))
    {
        $(".btnBusca").prop("disabled", false);
    }
    else
    {
        $(".btnBusca").prop("disabled", true);
    }

    return false;
}

function GradIntra_Relatorio_Risco_SelecionarTodos(pSender)
{
    var lCheckBoxes = $(pSender).closest("p");

    if (pSender.checked)
        lCheckBoxes.find("input").filter(":not(:last)").prop("checked", true).next("label").addClass("checked");
    else
        lCheckBoxes.find("input").filter(":not(:last)").prop("checked", false).next("label").removeClass("checked");
    
    return false;
}

function GradIntra_Relatorio_Risco_PosicaoContaCorrente_AtualizarTotais(pSender)
{
    var lTotais = ["D0", "D1", "D2", "D3", "CM"];
    var lResultados = [0, 0, 0, 0, 0];

    for(var a = 0; a <= lTotais.length; a++)
    {
        $(".SaldoParcial" + lTotais[a]).each(function()
        {
             lResultados[a] = parseFloat(lResultados[a]) + parseFloat($(this).html().replace(".","").replace(",","."));
        });
        
//      $(".SaldoTotal" + lTotais[a]).html(  $.format.number(lResultados[a]).replace("-.", "-").replace(",-", ",") );
        $(".SaldoTotal" + lTotais[a]).html(  $.format.number(lResultados[a]).replace("-.", "-").replace(",-", ",") );
    }
}

function GradIntra_Relatorio_Risco_ExpandCollapse(pSender)
{
    var lLinha = $(pSender).closest("tr").next("tr");
    var lSender = $(pSender);

    if (lSender.html() == "[ + ]")
    {
        lSender.html("[ - ]");
        lSender.attr("title", "Recolher histórico do limite");
    }
    else
    {
        lSender.attr("title", "Expandir histórico do limite");
        lSender.html("[ + ]");
    }
    
    if (lLinha.is(":visible"))
        lLinha.hide();
    else
        lLinha.show();

    return false; 
}

function GradIntra_Relatorio_Monitoramento_OcultarExibirFiltros(pDivDeFormulario, pCodigoDoRelatorio)
{
    var lEntidadeBtnBusca = pDivDeFormulario.find(".btnBusca");
    
    var lParagrafoBotao = lEntidadeBtnBusca.closest("p");

    pDivDeFormulario.find("p[class^=R0]").hide();

    if(pCodigoDoRelatorio != "")
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = pCodigoDoRelatorio;
        
        pDivDeFormulario.find("p." + pCodigoDoRelatorio).show();

        lParagrafoBotao.show();
    }
    else
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = null;

        lParagrafoBotao.hide();
    }

    if (pCodigoDoRelatorio == "R001" || pCodigoDoRelatorio == "R002")
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_5Linhas");
    }
}

function DefinirCoresValores_Load()
{
    $(".GridRelatorio tbody tr td").each(function()
    {
        if (gRelatorios_UrlDoUltimo.indexOf("015.aspx") == -1 && $(this).html().length != 14 && !isNaN($(this).html().replace("-", "").replace(".", "").replace(".", "").replace(".", "").replace(".", "").replace(".", "").replace(".", "").replace(",", "")))
        {
           if ($(this).html() == "0" || $(this).html() == "0,00")
               $(this).css("color", "Black");
           else if ($(this).html().indexOf("-") >= 0)
               $(this).css("color", "Red");
           else
               $(this).css("color", "Blue");
        }
    });
}
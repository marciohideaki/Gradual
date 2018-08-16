/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />

function GradIntra_Solicitacoes_AoSelecionarSistema()
{
    gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

    if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_POUPE_DIRECT)
    {
            GradIntra_CarregarHtmlVerificandoErro("Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx"
                                                 , null
                                                 , $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados")
                                                 , GradIntra_Solicitacoes_ExibirConteudo_CallBack
                                                 );
    }
    else if(gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_POUPE_OPERACAO)
    {
            GradIntra_CarregarHtmlVerificandoErro("Solicitacoes/PoupeDirect/PoupeOperacoes.aspx"
                                                 , null
                                                 , $("#pnlConteudo_Solicitacoes_PoupeOperacoes_Dados")
                                                 , GradIntra_Solicitacoes_ExibirConteudo_PoupeOperacoes_CallBack
                                                 );
    }
    else if(gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_CAMBIO)
    {
        GradIntra_Navegacao_CarregarFormulario("Solicitacoes/Formularios/Dados/CadastroDeCambio.aspx"
                                                , $("#pnlConteudo_Solicitacoes_CadastroDeCambio_Dados")
                                                , {Acao:"CarregarHtmlComDados"}
                                                , GradIntra_Solicitacoes_ExibirConteudo_CadastroDeCambio_CallBack)

    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_IPO) {

        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

        //GradIntra_Busca_ConfigurarGridDeResultados_GerenciamentoIPO();
        
        GradIntra_Navegacao_CarregarFormulario("Solicitacoes/Formularios/Dados/GerenciamentoIPO.aspx"
                                                , $("#pnlConteudo_Solicitacoes_GerenciamentoIPO_Dados")
                                                , { Acao: "CarregarHtmlComDados" }
                                                , GradIntra_Solicitacoes_ExibirConteudo_GerenciamentoIPO_CallBack);
        
        $("#pnlConteudo_Solicitacoes_GerenciamentoIPO_Dados").show(); 
    }
    else if(gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_VENDAS_DE_FERRAMENTAS )
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_SOLICITACOES_RESGATE) 
    {
        GradIntra_Navegacao_CarregarFormulario("Solicitacoes/Formularios/Dados/SolicitacoesResgate.aspx"
                                                , $("#pnlConteudo_Solicitacoes_SolicitacoesResgate_Dados")
                                                , { Acao: "CarregarHtmlComDados" }
                                                , GradIntra_Solicitacoes_ExibirConteudo_SolicitacoesResgate_CallBack)
    }
}

function GradIntra_Solicitacoes_ExibirConteudo_CallBack()
{
    $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados").show();
}

function GradIntra_Solicitacoes_ExibirConteudo_PoupeOperacoes_CallBack()
{
    $("#pnlConteudo_Solicitacoes_PoupeOperacoes_Dados").show();
}

function GradIntra_Solicitacoes_ExibirConteudo_CadastroDeCambio_CallBack()
{
    $("#pnlConteudo_Solicitacoes_CadastroDeCambio_Dados").show();
}

function GradIntra_Solicitacoes_ExibirConteudo_GerenciamentoIPO_CallBack() 
{
    //$("#pnlConteudo_Solicitacoes_GerenciamentoIPO_Dados").show(); 
    GradIntra_Busca_ConfigurarGridDeResultados_GerenciamentoIPO();
}

function GradIntra_Solicitacoes_ExibirConteudo_SolicitacoesResgate_CallBack() 
{
    $("#pnlConteudo_Solicitacoes_SolicitacoesResgate_Dados").show();
}

function GradIntra_Solicitacoes_InstanciarParametrosDeBusca()
{
    $("#pnlConteudo_Solicitacoes_GerenciamentoIPO_Dados").show();
}

function GradIntra_Solicitacoes_IPO_InstanciarParametrosDeBusca()
{
    var lDados = {};
    
    lDados.TermoDeBusca = $("#txtBusca_Solicitacoes_IPO_Termo").val();
    lDados.BuscarPor    = $("#cboBusca_Solicitacoes_IPO_BuscarPor").val();
    lDados.Status       = $("#cboBusca_Solicitacoes_IPO_Status").val();

    lDados.DataInicial  = $("#txtBusca_Solicitacoes_IPO_DataInicial").val();
    lDados.DataFinal    = $("#txtBusca_Solicitacoes_IPO_DataFinal").val();

    return lDados;
}

function GradIntra_Solicitacoes_InstanciarParametrosDeBusca() 
{
    var lDados = {};

    lDados.TermoDeBusca = $("#txtBusca_Solicitacoes_VendasDeFerramentas_Termo").val();
    lDados.BuscarPor    = $("#cboBusca_Solicitacoes_VendasDeFerramentas_BuscarPor").val();
    lDados.Status       = $("#cboBusca_Solicitacoes_VendasDeFerramentas_Status").val();

    lDados.DataInicial = $("#txtBusca_Solicitacoes_VendasDeFerramentas_DataInicial").val();
    lDados.DataFinal   = $("#txtBusca_Solicitacoes_VendasDeFerramentas_DataFinal").val();

    return lDados;
}

function Solicitacao_AtualizarResgate()
{
    var lIdsSelecionados = "";

    $(".checkResgateAprovar:checked").each(function()
    {
       lIdsSelecionados += $(this).parent().next().val() + ",";
    });

    var lURL = "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx";
    var lObjetoDeParametros = { Acao      : "AtualizarResgate"
                              , IdsResgate: lIdsSelecionados
                              };
    
    var pDivDeFormulario = $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados");

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function(pResposta) { Solicitacao_AtualizarResgate_Callback(pResposta) ;}
                                         , null
                                         , pDivDeFormulario
                                         );

}

function Solicitacao_AtualizarAplicacao_Click()
{

    var lIdsSelecionados = "";

    $(".checkAplicacaoAprovar:checked").each(function()
    {
       lIdsSelecionados += $(this).parent().next().val() + ",";
    });

    var lURL = "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx";
    var lObjetoDeParametros = { Acao      : "AtualizarAplicacao"
                              , IdsAplicacao: lIdsSelecionados
                              };
    
    var pDivDeFormulario = $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados");

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function(pResposta) { Solicitacao_AtualizarAplicacao_Callback(pResposta) ;}
                                         , null
                                         , pDivDeFormulario
                                         );

}

function Buscar_AplicacaoResgate_Click()
{
     var lComboBusca_Selecionado    = $("#cmbBusca_Status").val();
     var ltxtDataInicial            = $("#txtAplicacaoResgate_DataInicial").val();
     var ltxtDataFinal              = $("#txtAplicacaoResgate_DataFinal").val();
     var ltxtCodigoCliente          = $("#txtAplicacaoResgate_CodigoCliente").val();
     var lComboStatus               = $("#cmbBusca_Status").val();


      var lURL = "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx";
    var lObjetoDeParametros = { Acao            : "SelecionarAprovacoes"
                              , IdStatus        : lComboBusca_Selecionado
                              , DataInicial     : ltxtDataInicial
                              , DataFinal       : ltxtDataFinal
                              , CodigoCliente   : ltxtCodigoCliente
                              , status          : lComboStatus
                              };

  var pDivDeFormulario = $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados");
    


GradIntra_Busca_BuscarAplicacao(null,$("#pnlConteudo_Solicitacoes_PoupeDirect_Dados") ,$(".GridIntranet_CheckSemFundo"), "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx",lObjetoDeParametros);

}

function Solicitacao_Busca_Aplicacao_callback(pResposta)
{
      var lComboBusca_Selecionado    = $("#cmbBusca_Status").val();
     var ltxtDataInicial            = $("#txtAplicacaoResgate_DataInicial").val();
     var ltxtDataFinal              = $("#txtAplicacaoResgate_DataFinal").val();
     var ltxtCodigoCliente          = $("#txtAplicacaoResgate_CodigoCliente").val();
     var lComboStatus               = $("#cmbBusca_Status").val();


      var lObjetoDeParametros = { Acao            : "SelecionarAprovacoes"
                              , IdStatus        : lComboBusca_Selecionado
                              , DataInicial     : ltxtDataInicial
                              , DataFinal       : ltxtDataFinal
                              , CodigoCliente   : ltxtCodigoCliente
                              , status          : lComboStatus
                              };

    GradIntra_Busca_BuscarAplicacao(null,$("#pnlConteudo_Solicitacoes_PoupeDirect_Dados") ,$(".GridIntranet_CheckSemFundo"), "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx",lObjetoDeParametros);
    
}

function Solicitacao_AtualizarResgate_Callback(pResposta)
{

    var lIdsSelecionados = "";
     $(".checkResgateAprovar:checked").each(function()
    {
            if($.inArray( $(this).parent().next().val(), pResposta.ObjetoDeRetorno))
            {
                $(this).closest("tr").css("background", "#8FBC8F");
                $(this).closest("tr").children("td").eq(0).html("");
                $(this).closest("tr").children("td").eq(6).html("Efetivado");
            }
    });

    this.alert(pResposta.Mensagem);
}

function Solicitacao_AtualizarAplicacao_Callback(pResposta)
{
    var lIdsSelecionados = "";
     $(".checkAplicacaoAprovar:checked").each(function()
    {
            if($.inArray( $(this).parent().next().val(), pResposta.ObjetoDeRetorno))
            {
                $(this).closest("tr").css("background", "#8FBC8F");
                $(this).closest("tr").children("td").eq(0).html("");
                $(this).closest("tr").children("td").eq(6).html("Efetivado");
            }
    });

    this.alert(pResposta.Mensagem);
}

function GradIntra_Solicitacoes_PrepararFormularioDeAlteracao()
{
    $("#hidAlterarVenda_Propriedade_ValorOriginal").val( gGradIntra_Cadastro_ItemSendoEditado.Status );
    $("#hidAlterarVenda_Propriedade_IdVenda").val( gGradIntra_Cadastro_ItemSendoEditado.Id );

    $("#cboAlterarVenda_Status_NovoValor").find("option[value='" + gGradIntra_Cadastro_ItemSendoEditado.Status + "']").remove();
}




function GradIntra_Solicitacoes_SalvarAlteracoes()
{
    var lDiv = $("#pnlFormulario_AlterarVenda");

    if (lDiv.validationEngine({returnIsValid:true}))
    {
        gGradIntra_Cadastro_ItemFilhoSendoEditado = GradIntra_InstanciarObjetoDosControles(lDiv);

        var lDados = { Acao: "Salvar", ObjetoJson: $.toJSON(gGradIntra_Cadastro_ItemFilhoSendoEditado) };

        GradIntra_CarregarJsonVerificandoErro( "Solicitacoes/Formularios/Acoes/AlterarVenda.aspx", lDados, GradIntra_Solicitacoes_SalvarAlteracoes_CallBack, null, null );
    }
}

function GradIntra_Solicitacoes_SalvarAlteracoes_CallBack(pResposta)
{
    gGradIntra_Cadastro_ItemFilhoSendoEditado.IdVendaAlteracao = pResposta.ObjetoDeRetorno;

    gGradIntra_Cadastro_ItemFilhoSendoEditado.DtData = GradAux_DataDeHojeComHoraCompleta();

    gGradIntra_Cadastro_ItemFilhoSendoEditado.DsUsuario = $("#lnkUsuario_logout").text();

    var lTabela = $("#tblAlteracoesDaVenda tbody");

    var lTR =   "<tr>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.IdVendaAlteracao + "</td>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DsPropriedades + "</td>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DsValoresAnteriores + "</td>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DsValoresModificados + "</td>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DtData + "</td>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DsUsuario + "</td>"
              + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DsMotivo + "</td>"
              + "</tr>";

    gGradIntra_Cadastro_ItemFilhoSendoEditado = null;

    lTabela.append(lTR);
}


function btnProduto_Cambios_IncluirEditar_Click(pSender)
{
    var lDiv = $("#pnlFormulario_Cambio");

    if (lDiv.validationEngine({returnIsValid:true}))
    {
        gGradIntra_Cadastro_ItemFilhoSendoEditado = GradIntra_InstanciarObjetoDosControles(lDiv);

        var lDados = { Acao: "Salvar", ObjetoJson: $.toJSON(gGradIntra_Cadastro_ItemFilhoSendoEditado) };

        GradIntra_CarregarJsonVerificandoErro( "Solicitacoes/Formularios/Dados/CadastroDeCambio.aspx", lDados, GradIntra_Cambio_Salvar_CallBack, null, null );
    }

    return false;
}

function btnProduto_GerenciamentoIPO__IncluirEditar_Validar() {
    var lRetorno = true;

    var lHoraMaxima  = $("#txtProduto_IPO_HoraMaxima").val();
    var lValorMinimo = $("#txtProduto_IPO_VlMinimo").val();
    var lValorMaximo = $("#txtProduto_IPO_VlMaximo").val();
    var lModalidade  = $("#cboProduto_IPO_Modalidade").val();
    var lDataInicial = $("#txtProduto_IPO_DataInicial").val();
    var lDataFinal   = $("#txtProduto_IPO_DataFinal").val();

    if (lDataInicial == "") {
        GradIntra_ExibirMensagem("I", "É necessário inserir uma data inicial");

        lRetorno = false;
    }

    if (lDataFinal == "") {
        GradIntra_ExibirMensagem("I", "É necessário inserir uma data final");

        lRetorno = false;
    }

    if (lModalidade == "") {
        GradIntra_ExibirMensagem("I", "É necessário selecionar um tipo de IPO");

        lRetorno = false;
    }

    if (lHoraMaxima == "__:__") {

        GradIntra_ExibirMensagem("I", "É necessário inserir uma hora máxima válida");

        lRetorno = false;
    }

    if (lValorMinimo == "" || lValorMinimo == "0,00") {
        
        GradIntra_ExibirMensagem("I", "É necessário inserir um valor mínimo válido");

        lRetorno = false;
    }

    if (lValorMaximo == "" || lValorMaximo == "0,00") {

        GradIntra_ExibirMensagem("I", "É necessário inserir um valor máximo válido");

        lRetorno = false;
    }



    return lRetorno;
}

function btnProduto_GerenciamentoIPO_IncluirEditar_Click(pSender) 
{
    var lDiv = $("#pnlFormulario_GerenciamentoIPO");

    if (!btnProduto_GerenciamentoIPO__IncluirEditar_Validar())
        return false;

    if (lDiv.validationEngine({ returnIsValid: true })) 
    {
        gGradIntra_Cadastro_ItemFilhoSendoEditado = GradIntra_InstanciarObjetoDosControles(lDiv);

        var lDados = { Acao: "Salvar", ObjetoJson: $.toJSON(gGradIntra_Cadastro_ItemFilhoSendoEditado) };

        GradIntra_CarregarJsonVerificandoErro("Solicitacoes/Formularios/Dados/GerenciamentoIPO.aspx", lDados, GradIntra_GerenciamentoIPO_Salvar_CallBack, null, null);
    }

    return false;
}

function GradIntra_GerenciamentoIPO_Salvar_CallBack(pResposta) {

    var lTabela = $("#tblProdutosGerenciamentoIPO_ListaDeProdutos tbody");

    gGradIntra_Cadastro_ItemFilhoSendoEditado.CodigoIPO = pResposta.ObjetoDeRetorno.CodigoIPO;

    var lTR = $("#tblProdutosGerenciamentoIPO_ListaDeProdutos tbody tr[rel='" + gGradIntra_Cadastro_ItemFilhoSendoEditado.CodigoIPO + "']");

    if (lTR.length > 0) {

        lTR.find("td:eq(0)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.Ativo);
        lTR.find("td:eq(1)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.CodigoISIN);
        lTR.find("td:eq(2)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.DsEmpresa);
        lTR.find("td:eq(3)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.Modalidade);
        lTR.find("td:eq(4)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.DataInicial);
        lTR.find("td:eq(5)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.DataFinal);
        lTR.find("td:eq(6)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.HoraMaxima);
        lTR.find("td:eq(7)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.VlMinimo);
        lTR.find("td:eq(8)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.VlMaximo);
        lTR.find("td:eq(9)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.VlPercentualGarantia);
        lTR.find("td:eq(10)").text((gGradIntra_Cadastro_ItemFilhoSendoEditado.StAtivo == false ? "NÃO" : "SIM"));
        lTR.find("td:eq(11)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.Observacoes);

    } else {

        lTR = "<tr rel='" + gGradIntra_Cadastro_ItemFilhoSendoEditado.CodigoIPO + "'>"

            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.Ativo       + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.CodigoISIN  + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DsEmpresa   + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.Modalidade  + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DataInicial + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.DataFinal   + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.HoraMaxima + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.VlMinimo + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.VlMaximo + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.VlPercentualGarantia + "</td>"
            + "   <td                     >" + (gGradIntra_Cadastro_ItemFilhoSendoEditado.StAtivo == false ? "NÃO" : "SIM") + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.Observacoes + "</td>"
            + "   <td><button style='margin-top:2px;margin-bottom:2px' onclick='return btnSolicitacoes_GerenciamentoIPO_Editar_Click(this)' class='IconButton Editar'><span>Editar</span></button></td>"
            + "</tr>";

        lTabela.find('tr:first').before(lTR);
    }

    gGradIntra_Cadastro_ItemFilhoSendoEditado = null;

    $("#hidProduto_Codigo_IPO").val("");
    $("#pnlFormulario_GerenciamentoIPO").find("input").val("");
    $("#pnlFormulario_GerenciamentoIPO").find("textarea").val("");
    $("#cboProduto_IPO_Modalidade").val("");
    $("#chkProduto_IPO_AtivoSite").prop("checked", false).next("label").removeClass("checked");

    GradIntra_ExibirMensagem("A", "Produto de IPO salvo com sucesso");
}

function btnSolicitacoes_GerenciamentoIPO_Editar_Click(pSender) {
    pSender = $(pSender);

    var lTR = pSender.closest("tr");

    $("#hidProduto_Codigo_IPO")                 .val(lTR.attr("rel"));

    $("#txtProduto_IPO_Ativo")                  .val(lTR.find("td:eq(0)").text().trim());
    $("#txtProduto_IPO_CodigoISIN")             .val(lTR.find("td:eq(1)").text().trim());
    $("#txtProduto_IPO_Empresa")                .val(lTR.find("td:eq(2)").text().trim());
    $("#cboProduto_IPO_Modalidade")             .val(lTR.find("td:eq(3)").text().trim());
    $("#txtProduto_IPO_DataInicial")            .val(lTR.find("td:eq(4)").text().trim());
    $("#txtProduto_IPO_DataFinal")              .val(lTR.find("td:eq(5)").text().trim());
    $("#txtProduto_IPO_HoraMaxima")             .val(lTR.find("td:eq(6)").text().trim());
    $("#txtProduto_IPO_VlMinimo")               .val(lTR.find("td:eq(7)").text().trim());
    $("#txtProduto_IPO_VlMaximo")               .val(lTR.find("td:eq(8)").text().trim());
    $("#txtProduto_IPO_VlPercentualGarantia")   .val(lTR.find("td:eq(9)").text().trim());
    $("#txtProduto_IPO_Observacoes")            .val(lTR.find("td:eq(11)").text().trim())

    if (lTR.find("td:eq(10)").text().trim().toLowerCase() == "sim") {
        $("#chkProduto_IPO_AtivoSite").prop("checked", true).next("label").addClass("checked");
    }
    else {
        $("#chkProduto_IPO_AtivoSite").prop("checked", false).next("label").removeClass("checked");
    }

    return false;
}

function GradIntra_Cambio_Salvar_CallBack(pResposta)
{
    var lTabela = $("#tblProdutosCambio_ListaDeProdutos tbody");

    gGradIntra_Cadastro_ItemFilhoSendoEditado.IdProduto = pResposta.ObjetoDeRetorno.IdProduto;

    var lTR = $("#tblProdutosCambio_ListaDeProdutos tbody tr[rel='" + gGradIntra_Cadastro_ItemFilhoSendoEditado.IdProduto + "']");

    if(lTR.length > 0)
    {
        lTR.find("td:eq(1)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.NomeDoProduto);
        lTR.find("td:eq(2)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.Preco);
        lTR.find("td:eq(3)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.PrecoCartao);
        lTR.find("td:eq(4)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.Taxas);
        lTR.find("td:eq(5)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.Taxas2);
        lTR.find("td:eq(6)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem);
        lTR.find("td:eq(7)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem2);
        lTR.find("td:eq(8)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem3);
        lTR.find("td:eq(9)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem4);
        lTR.find("td:eq(10)").text(gGradIntra_Cadastro_ItemFilhoSendoEditado.ProdutoSuspenso);
    }
    else
    {
        lTR = "<tr rel='" + gGradIntra_Cadastro_ItemFilhoSendoEditado.IdProduto + "'>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.IdProduto + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.NomeDoProduto + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.Preco + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.PrecoCartao + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.Taxas + "</td>"
            + "   <td>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.Taxas2 + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem2 + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem3 + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.UrlImagem4 + "</td>"
            + "   <td style='display:none'>" + gGradIntra_Cadastro_ItemFilhoSendoEditado.ProdutoSuspenso + "</td>"
            + "   <td><button style='margin-top:2px;margin-bottom:2px' onclick='return btnSolicitacoes_Cambio_Editar_Click(this)' class='IconButton Editar'><span>Editar</span></button></td>"
            + "</tr>";

        lTabela.append(lTR);
    }

    gGradIntra_Cadastro_ItemFilhoSendoEditado = null;

    $("#pnlFormulario_Cambio").find("input").val("");

    $("#chkProduto_Cambio_Suspenso").prop("checked", false).next("label").removeClass("checked");

    GradIntra_ExibirMensagem("A", "Produto salvo com sucesso");
}

function btnSolicitacoes_Cambio_Editar_Click(pSender)
{
    pSender = $(pSender);

    var lTR = pSender.closest("tr");

    $("#hidProduto_Cambio_Id").val(lTR.attr("rel"));
    $("#txtProduto_Cambio_Nome").val(lTR.find("td:eq(1)").text().trim());
    $("#txtProduto_Cambio_Preco").val(lTR.find("td:eq(2)").text().trim());
    $("#txtProduto_Cambio_PrecoCartao").val(lTR.find("td:eq(3)").text().trim());
    $("#txtProduto_Cambio_Taxas").val(lTR.find("td:eq(4)").text().trim());
    $("#txtProduto_Cambio_Taxas2").val(lTR.find("td:eq(5)").text().trim());
    $("#txtProduto_Cambio_UrlImagem").val(lTR.find("td:eq(6)").text().trim());
    $("#txtProduto_Cambio_UrlImagem2").val(lTR.find("td:eq(7)").text().trim());
    $("#txtProduto_Cambio_UrlImagem3").val(lTR.find("td:eq(8)").text().trim());
    $("#txtProduto_Cambio_UrlImagem4").val(lTR.find("td:eq(9)").text().trim());

    if(lTR.find("td:eq(10)").text().trim().toLowerCase() == "true")
    {
        $("#chkProduto_Cambio_Suspenso").prop("checked", true).next("label").addClass("checked");
    }
    else
    {
        $("#chkProduto_Cambio_Suspenso").prop("checked", false).next("label").removeClass("checked");
    }

    return false;
}

///Método que captura os status alterados das solicitações de reserva de IPO
function btnSolicitacoes_SalvarStatusSolicitacoes_Click(pSender) {

    if (confirm("Você realmente deseja alterar o(s) status da(s) das solicitações de reservas?")) {

        var lDataCliente = { 
            CodigoIPOCliente: "", 
            Status: "", 
            CodigoCliente:"", 
            CodigoAssessor:"", 
            CodigoISIN:"", 
            CpfCnpj:"", 
            Data:"", 
            Limite:"",
            Modalidade:"",
            NomeCliente:"",
            TaxaMaxima:"",
            ValorMaximo:"",
            ValorReserva: "",
            NumeroProtocolo:"",
            Observacoes:""
        };

        var lGrid        = $('#tblBusca_Solicitacoes_IPO_Resultados');
        var lRowID = lGrid.jqGrid('getGridParam', 'selrow');

        if (lRowID == null) {
            GradIntra_ExibirMensagem("I", "É necesário selecionar uma solicitação de reserva de oferta pública.");
            return false;
        }

        var lRowData = lGrid.jqGrid('getRowData', lRowID);

        lDataCliente.CodigoIPOCliente   = lRowData["CodigoIPOCliente"];

        var lDropDown                   = jQuery('#' + lRowID + '_Status')[0];

        var lSelectedOption             = lDropDown.options[lDropDown.selectedIndex];

        lDataCliente.Status             = lSelectedOption.value; ; // lRowData["Status"];

        lDataCliente.CodigoCliente      = lRowData["CodigoCliente"];

        lDataCliente.CodigoAssessor     = lRowData["CodigoAssessor"];

        lDataCliente.CodigoISIN         = lRowData["CodigoISIN"];

        lDataCliente.CpfCnpj            = lRowData["CpfCnpj"];

        lDataCliente.Data               = lRowData["Data"];

        lDataCliente.Empresa            = lRowData["Empresa"];

        lDataCliente.Limite             = lRowData["Limite"];

        lDataCliente.Modalidade         = lRowData["Modalidade"];

        lDataCliente.NomeCliente        = lRowData["NomeCliente"];

        lDataCliente.TaxaMaxima         = lRowData["TaxaMaxima"];

        lDataCliente.ValorMaximo        = lRowData["ValorMaximo"];

        lDataCliente.ValorReserva       = lRowData["ValorReserva"];

        lDataCliente.NumeroProtocolo    = lRowData["NumeroProtocolo"];

        var lTextAreaObservacoes        = jQuery('#' + lRowID + '_Observacoes').val();

        lDataCliente.Observacoes        = lTextAreaObservacoes;

        var lDados                      = { Acao: "SalvarSolicitacao", ObjetoJson: $.toJSON(lDataCliente) };

        GradIntra_CarregarJsonVerificandoErro("Solicitacoes/Formularios/Dados/GerenciamentoIPO.aspx", lDados, btnSolicitacoes_SalvarStatusSolicitacoes_Callback_Click(lRowID, lSelectedOption.text, lTextAreaObservacoes), null, null);
    }

    return false;
}

///Método de calback do métodode salvar para
function btnSolicitacoes_SalvarStatusSolicitacoes_Callback_Click(pRowID, pStatus, pObservacoes) 
{
    var lGrid = $('#tblBusca_Solicitacoes_IPO_Resultados');
    var lRows = lGrid.jqGrid('getDataIDs');

    for (i = 0; i <= lRows.length; i++) {
        lGrid.jqGrid('restoreRow', lRows[i]);
    }

    lGrid.jqGrid('setCell', pRowID, 'Status', pStatus); //atribuindo valor para a celula da grid na coluna status

    lGrid.jqGrid('setCell', pRowID, 'Observacoes', pObservacoes); //atribuindo valor para a celula da grid na col

    lGrid.jqGrid("resetSelection");

    GradIntra_ExibirMensagem("A", "Status da Reserva de IPO salvo com sucesso");
}

function btnSolicitacoesGerenciamentoIPO_Limites_Click() {

    var lGrid = $('#tblBusca_Solicitacoes_IPO_Resultados');
    //var lRows = lGrid.jqGrid('getDataIDs');

    var lRowID = lGrid.jqGrid('getGridParam', 'selrow');

    if (lRowID == null) {
        GradIntra_ExibirMensagem("I", "É necesário selecionar um cliente para atualizar limites");
        return false;
    }

    var lDataCliente = { CodigoCliente:"", CpfCnpj:"", CodigoIPOCliente: "" };

    lDataCliente.CodigoCliente    = lGrid.jqGrid ('getCell', lRowID, 'CodigoCliente');
    lDataCliente.CpfCnpj          = lGrid.jqGrid ('getCell', lRowID, 'CpfCnpj');
    lDataCliente.CodigoIPOCliente = lGrid.jqGrid ('getCell', lRowID, 'CodigoIPOCliente');

    var lDados = { Acao: "BuscarLimites", CodigoCliente: lDataCliente.CodigoCliente, CpfCnpj: lDataCliente.CpfCnpj, CodigoIPOCliente: lDataCliente.CodigoIPOCliente };

    GradIntra_CarregarJsonVerificandoErro("Solicitacoes/Formularios/Dados/GerenciamentoIPO.aspx", lDados, btnSolicitacoesGerenciamentoIPO_Limites_Callback_Click, null, null);

    return false;
}

function btnSolicitacoesGerenciamentoIPO_Limites_Callback_Click(pResposta) 
{
    if (!pResposta.TemErro) {

        var lRetorno = pResposta.ObjetoDeRetorno;

        var lGrid = $('#tblBusca_Solicitacoes_IPO_Resultados');

        var lRowID = lGrid.jqGrid('getGridParam', 'selrow');

        lGrid.jqGrid('setCell', lRowID, 'Limite', lRetorno.TotalLimiteSomado);

        lGrid.jqGrid('restoreRow', lRowID);

        lGrid.jqGrid("resetSelection");

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}


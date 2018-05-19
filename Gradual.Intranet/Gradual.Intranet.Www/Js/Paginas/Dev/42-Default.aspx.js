/// <reference path="../../Lib/Gradual/Dev/01-GradSettings.js" />
/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

var gPosX = -1;
var gPosY = -1;
var gClickY = -1;
var gClickX = -1;
var altKey  = false;
var keyCode = 0;

var gConfirmacaoDeSaidaDesnecessaria = false;
var gDataDaNotaParaCSV = ""

window.onmousemove = onMouseMovePosition;
window.onkeydown = whatKey;

function PageLoad()
{
    gGradIntra_AplicacaoEmModoDeCompatibilidade = (document.documentMode && document.documentMode == 7);

    if (gGradIntra_AplicacaoEmModoDeCompatibilidade)
    {
        $(document.body).append("<div class='AlertaDeModoDeCompatibilidade'>&nbsp;</div>");

        return false;
    }

    Preferencias_IniciarPreferencias();

    GradIntra_Navegacao_IrParaSubSistema(gPreferenciasDoUsuario.UltimoSistema, gPreferenciasDoUsuario.UltimoSubSistema);

    GradIntra_AtivarTooltips();

    $.format.locale({ number:  { format: '#,##0.00', groupingSeparator: ".", decimalSeparator: "," } }); 

    $(window).bind("resize", PageResize);

    $("#pnlCotacaoRapida").draggable();

    // fixa a largura de cada UL de submenu para fazer o scroll lateral depois:
    
    $("ul.Sistema_Clientes").css(      { width: 2580 } );
    $("ul.Sistema_Risco").css(         { width: 2780 } );
    $("ul.Sistema_Seguranca").css(     { width: 2740 } );
    $("ul.Sistema_Monitoramento").css( { width: 2440 } );
    $("ul.Sistema_Sistema").css(       { width: 3240 } );
    $("ul.SubMenu_Relatorios").css(    { width: 2240 } );

    VerificarMenuScroller();

    ManterSessao();
}

function PageResize()
{
    if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES
    || gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA
    || gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RISCO)
    {
        if ( $("#pnlBusca_" + gGradIntra_Navegacao_SistemaAtual + "_Resultados").is(":visible") )
        {
            if ( $("#gbox_tblBusca_" + gGradIntra_Navegacao_SistemaAtual + "_Resultados").is(":visible") )
            {
                //TODO: Magic number, constante do painel do lado esquerdo que é sempre fixo (500) e tamanho mínimo, que também está fixo em 510

                var lWidth = $("#pnlBusca_" + gGradIntra_Navegacao_SistemaAtual + "_Resultados").width() - 500;
            
                if (lWidth > 510)
                    $("#tblBusca_" + gGradIntra_Navegacao_SistemaAtual + "_Resultados").setGridWidth( lWidth );
            }
        }
    }

    RedimensionarConteudo();
}

function RedimensionarConteudo()
{
    var lConteudo = $("div.pnlFormulario:visible");

    if (lConteudo.length > 0 
    && ($(".ListaDeObjetos:visible").length > 0))
    {
        var lWidth = $(window).width();

        var lEspaco = lWidth - 1014;

        //magic number; 1014 é o mínimo que precisa pro menu de objetos selecionados + objeto + mínimo do painel formulário, que é 480

        lConteudo.width(480 + ((lEspaco > 0) ? lEspaco : 0));
    }

    VerificarMenuScroller();
}

function VerificarMenuScroller()
{
    var lMenuScroller = $("div.pnlMenuScroller:visible");
    var lUL           = lMenuScroller.find("ul.SubMenu");
    var lMinWidth     = lUL.width() - 1500;
    //var lMinWidth = lUL.width();

    if (lMenuScroller.width() <= lMinWidth)
    {
        lMenuScroller.children("button").addClass("btnMenuScroller_Visivel");
    }
    else
    {
        lMenuScroller.children("button").removeClass("btnMenuScroller_Visivel");

        lUL.css( { left: 22 } );
    }
}

function lnkMenuPrincipal_Click(pSender)
{
    GradIntra_Navegacao_IrParaSistema($(pSender).attr("rel"), true);

    return false;
}

function lnkMenuSecundario_Click(pSender, pEventArgs)
{
    GradIntra_Navegacao_IrParaSubSistema(gGradIntra_Navegacao_SistemaAtual, $(pSender).attr("rel"));
    
    return false;
}

function btnMenuScrollerEsquerdo_MouseOver(pSender)
{
    var lBotao = $(pSender);

    if (lBotao.hasClass("btnMenuScroller_Visivel"))
    {
        var lUL  = $("div.pnlMenuScroller:visible ul");

        lUL.animate( { left: 22 }, 300);
    }
}

function btnMenuScrollerDireito_MouseOver(pSender)
{
    var lBotao = $(pSender);

    if (lBotao.hasClass("btnMenuScroller_Visivel"))
    {
        var lDiv = $("div.pnlMenuScroller:visible");
        var lUL  = lDiv.children("ul");

        var lDiff = lDiv.width() + (lUL.width() - 2000) - 40;

        //lUL.animate( { left: lDiff }, 300);
        lUL.animate({ left: -100 }, 300);
    }
}

function lnkSubMenu_Sistema_Objetos_Click(pSender)
{
    Sistema_ExibirObjetosDoSistema();
    
    return false;
}

function lnkSubMenu_Sistema_Variaveis_Click(pSender)
{
    Sistema_ExibirVariaveisDoSistema();
    
    return false;
}

function lnlMensagemAdicional_Fechar_Click(pSender)
{
    GradIntra_OcultarMensagemAdicional();

    return false;
}

function btnBusca_Click(pObjeto)
{
    switch(gGradIntra_Navegacao_SistemaAtual)
    {
        case CONST_SISTEMA_CLIENTES:

            var strBusca = "";
            var cboBusca = "";

            if ($(pObjeto).closest("div").attr("id") == "pnlBusca_Clientes_Reserva_IPO_Form")
            {
                strBusca = $("#txtBusca_Clientes_Termo_IPO").val();
                cboBusca = $("#cboBusca_Clientes_BuscarPor_IPO").val();
            }
            else
            {
                strBusca = $("#txtBusca_Clientes_Termo").val();
                cboBusca = $("#cboBusca_Clientes_BuscarPor").val();
            }
        
            if ( cboBusca == "CodBovespa" )
            {
                //var strBusca = $("#txtBusca_Clientes_Termo").val();
                if (strBusca.indexOf('.') > -1)
                {
                    GradIntra_ExibirMensagem("E", "Para busca por Código Bovespa não são aceitos pontos");
                    return false;
                }
                if (strBusca.indexOf('-') > -1)
                {
                    GradIntra_ExibirMensagem("E", "Para busca por Código Bovespa não são aceitos traços nem o dígito");
                    return false;
                }
                if (isNaN(strBusca))
                {
                    GradIntra_ExibirMensagem("E", "Para busca por Código Bovespa só são aceitos caracteres numéricos");
                    return false;
                }
            }
          
            if ($(pObjeto).closest("div").attr("id") == "pnlBusca_Clientes_Reserva_IPO_Form")
            {
                var lObjetoSeletor =  $("#pnlBusca_Clientes_Reserva_IPO_Form");
                var lDados = GradIntra_InstanciarObjetoDosControles(lObjetoSeletor);
                GradIntra_Busca_BuscarItensParaSelecaoReservaIPO(lDados, lObjetoSeletor, "Clientes/Formularios/ReservarIPO.aspx");
            }
            else
            {
                if (strBusca == "" && !confirm("Caro usuário, você optou por uma consulta sem parâmetro pesquisa o que resultará num retorno mais lento dos resultados. Realmente deseja continuar?"))
                    return false;
        
                var lDados = GradIntra_Clientes_InstanciarParametrosDeBusca();

                GradIntra_Busca_BuscarItensParaSelecao(lDados, $("#pnlBusca_Clientes_Form"), "Buscas/Clientes.aspx");
            }
        break;

        case CONST_SISTEMA_SEGURANCA:

            GradIntra_Busca_BuscarItensParaSelecao(null, $("#pnlBusca_Seguranca_Form"), "Buscas/Seguranca.aspx");

            break;

        case CONST_SISTEMA_PAPEIS:

            GradIntra_Busca_BuscarItensParaSelecao(null, $("#pnlBusca_Papeis_Form"), "Buscas/Papeis.aspx");

            break;

        case CONST_SISTEMA_SISTEMA:

            GradIntra_Sistema_ExibirConteudo();

            break;

        case CONST_SISTEMA_RISCO:

            var lDados = GradIntra_Risco_InstanciarParametrosDeBusca();
            GradIntra_Busca_BuscarItensParaSelecao(lDados, $("#pnlBusca_Risco_Form"), "Buscas/Risco.aspx");

            break;

        case CONST_SISTEMA_MONITORAMENTO:

            var lPainel = $(pObjeto).closest("div").attr("id");

            if (lPainel == "pnlBusca_Monitoramento_Ordens_Form") {
                var lDataInicial = ConvertToDate($("#txtBusca_Monitoramento_Ordens_DataInicial").val());
                var lDataFinal = ConvertToDate($("#txtBusca_Monitoramento_Ordens_DataFinal").val());

                if ($("#txtBusca_Monitoramento_Ordens_DataInicial").val() == "" || $("#txtBusca_Monitoramento_Ordens_DataFinal").val() == "") {
                    alert("Filtro de pesquisa inválido! Preencha a data inicial e a data final");
                    return false;
                }

                if (lDataFinal < lDataInicial) {
                    alert("Filtro de pesquisa inválido! A data inicial deve ser menor que a data final.");
                    return false;
                }

                if ($("#txtBusca_Monitoramento_Ordens_DataInicial").val() != $("#txtBusca_Monitoramento_Ordens_DataFinal").val()) {
                    $("#txtBusca_Monitoramento_Ordens_HoraInicial").val("");
                    $("#txtBusca_Monitoramento_Ordens_HoraFinal").val("");
                }

                GradIntra_Busca_BuscarItensParaSelecaoOrdens(null, $("#pnlBusca_Monitoramento_Ordens_Form"), $(".GridIntranet_CheckSemFundo"), "Monitoramento/ResultadoOrdens.aspx");
            }
            else if (lPainel == "pnlBusca_Monitoramento_OrdensStop_Form") {
                GradIntra_Busca_BuscarItensParaSelecaoOrdens(null, $("#pnlBusca_Monitoramento_OrdensStop_Form"), $(".GridIntranet_CheckSemFundo"), "Monitoramento/ResultadoOrdensStop.aspx");
            }
            else if (lPainel == "pnlBusca_Monitoramento_Termos_Form") {
                //GradIntra_Busca_BuscarItensParaSelecaoOrdens(null, $("#pnlBusca_Monitoramento_Termos_Form"), $(".GridIntranet_CheckSemFundo"), "Monitoramento/ResultadoTermos.aspx");

                var lDados = GradIntra_InstanciarObjetoDosControles($("#pnlBusca_Monitoramento_Termos_Form"));

                lDados.Acao = "CarregarHtml";

                GradIntra_Navegacao_CarregarFormulario("Monitoramento/ResultadoTermos.aspx", "pnlMonitoramento_Termos_ListaDeResultados", lDados, null);

            } else if (lPainel == "pnlBusca_Monitoramento_OrdensNovoOMS_Form") {
                var lDataInicial = ConvertToDate($("#txtBusca_Monitoramento_OrdensNovoOMS_DataInicial").val());
                var lDataFinal = ConvertToDate($("#txtBusca_Monitoramento_OrdensNovoOMS_DataFinal").val());

                if ($("#txtBusca_Monitoramento_OrdensNovoOMS_DataInicial").val() == "" || $("#txtBusca_Monitoramento_OrdensNovoOMS_DataFinal").val() == "") {
                    alert("Filtro de pesquisa inválido! Preencha a data inicial e a data final");
                    return false;
                }

                if (lDataFinal < lDataInicial) {
                    alert("Filtro de pesquisa inválido! A data inicial deve ser menor que a data final.");
                    return false;
                }

                if ($("#txtBusca_Monitoramento_OrdensNovoOMS_DataInicial").val() != $("#txtBusca_Monitoramento_OrdensNovoOMS_DataFinal").val()) {
                    $("#txtBusca_Monitoramento_OrdensNovoOMS_HoraInicial").val("");
                    $("#txtBusca_Monitoramento_OrdensNovoOMS_HoraFinal").val("");
                }

                GradIntra_Busca_BuscarItensParaSelecaoOrdens(null, $("#pnlBusca_Monitoramento_OrdensNovoOMS_Form"), $(".GridIntranet_CheckSemFundo"), "Monitoramento/ResultadoOrdensNovoOMS.aspx");
            } 
            else if (lPainel == "pnlBusca_Monitoramento_OrdensSpider_Form") 
            {
                var lDataInicial = ConvertToDate($("#txtBusca_Monitoramento_OrdensSpider_DataInicial").val());
                var lDataFinal = ConvertToDate($("#txtBusca_Monitoramento_OrdensSpider_DataFinal").val());

                if ($("#txtBusca_Monitoramento_OrdensSpider_DataInicial").val() == "" || $("#txtBusca_Monitoramento_OrdensSpider_DataFinal").val() == "") 
                {
                    alert("Filtro de pesquisa inválido! Preencha a data inicial e a data final");
                    return false;
                }

                if (lDataFinal < lDataInicial) 
                {
                    alert("Filtro de pesquisa inválido! A data inicial deve ser menor que a data final.");
                    return false;
                }

                if ($("#txtBusca_Monitoramento_OrdensSpider_DataInicial").val() != $("#txtBusca_Monitoramento_OrdensSpider_DataFinal").val()) 
                {
                    $("#txtBusca_Monitoramento_OrdensSpider_HoraInicial").val("");
                    $("#txtBusca_Monitoramento_OrdensSpider_HoraFinal").val("");
                }

                GradIntra_Busca_BuscarItensParaSelecaoOrdens(null, $("#pnlBusca_Monitoramento_OrdensSpider_Form"), $(".GridIntranet_CheckSemFundo"), "Monitoramento/ResultadoOrdensSpider.aspx");
            }


            break;

        case CONST_SISTEMA_SOLICITACOES:

            if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_IPO) {
                var lDados = GradIntra_Solicitacoes_IPO_InstanciarParametrosDeBusca();

                GradIntra_Busca_BuscarItensParaSelecaoGerenciamentoIPO(lDados, $("#pnlBusca_Solicitacoes_IPO_Form"), "Buscas/Solicitacoes_IPO.aspx", 'pnlSolicitacoes_IPO_Cliente');
            }
            else {
                var lDados = GradIntra_Solicitacoes_InstanciarParametrosDeBusca();

                GradIntra_Busca_BuscarItensParaSelecao(lDados, $("#pnlBusca_Solicitacoes_VendasDeFerramentas_Form"), "Buscas/Solicitacoes_VendasDeFerramentas.aspx");
            }
            break;

        default:
            GradIntra_ExibirMensagem("E", "Sem função de busca para o sistema " + gGradIntra_Navegacao_SistemaAtual);

            break;
    }

    return false;
}

function btnResultadoBusca_ExpandirPainel_Click(pSender, pIdDoGrid)
{
    pSender = $(pSender);

    GradIntra_Navegacao_ExpandirTabelaDeBusca(pSender.closest("div.Busca_Resultados"), pIdDoGrid);

    return false;
}

function btnResultadoBusca_ExpandirPainelAbaixo_Click(pSender, pIdDoGrid) 
{
    pSender = $(pSender);

    GradIntra_Navegacao_ExpandirTabelaDeBuscaAbaixo(pSender.closest("div.Busca_Resultados_Abaixo"), pIdDoGrid);

    return false;
}

function lstItensSelecionados_btnCancelarNovo_OnClick(pSender, pTipoDeItem)
{
    /*
    if (confirm("Tem certeza que deseja cancelar o cadastro do novo " + pTipoDeItem + "?"))
    {
        GradIntra_Navegacao_OcultarFormularioDeNovoItem();
    }
    */

    return false;
}

function pnlFormulario_btnCancelar_Click()
{
    if($("#pnlNovoItem_Formulario").find("#pnlBotoes").length > 0)
    {
        GradIntra_Navegacao_OcultarFormularioDeNovoItem(true);

        return false;
    }

    if (confirm("Tem certeza que deseja cancelar o cadastro ?"))
        GradIntra_Navegacao_OcultarFormularioDeNovoItem(true);

    return false;
}

function lstItensSelecionados_btnFechar_Click(pSender)
{
    GradIntra_ExcluirItemDaListaDeItensSelecionados($(pSender).closest("li"));

    return false;
}

function lstClientesSelecionados_H3_OnClick(pSender)
{
    var lLI = $(pSender).parent();

    if (!lLI.hasClass("Expandida"))
        GradIntra_Navegacao_ExpandirItemSelecionado(lLI);

    return false;
}

function lstItensSelecionados_LI_MouseEnter(pSender)
{
    var li = $(pSender.target).closest("li." + CONST_CLASS_ITEM_ITEMDINAMICO);
    
    if (!li.hasClass(CONST_CLASS_ITEM_EXPANDIDO))
        li.css({opacity:1});
}

function lstItensSelecionados_LI_MouseLeave(pSender)
{
    var li = $(pSender.target).closest("li." + CONST_CLASS_ITEM_ITEMDINAMICO);
    
    if (!li.hasClass(CONST_CLASS_ITEM_EXPANDIDO))
        li.css({opacity:0.5});
}

function lstMenuCliente_A_OnClick(pSender, pCallBack)
{
    if ($("div." + CONST_CLASS_CARREGANDOCONTEUDO).length > 0) return false;

    pSender = $(pSender);

    var lURL = pSender.attr("href");

    GradIntra_Navegacao_ExibirFormulario(lURL, pCallBack);
    
    RedimensionarConteudo();

    return false;
}

function btnSalvar_Clientes_DadosCompletos_Click(pSender)
{
    GradIntra_Clientes_SalvarDadosCompletosDoCliente();
    
    return false;
}

function btnSalvar_Clientes_Rendimentos_Click(pSender) 
{
    GradIntra_Clientes_SalvarItemFilhoDoCliente("RendimentosSituacaoFinanceira");
    
    return false; 
}

function btnSalvar_Clientes_InformacoesPatrimoniais_Click(pSender)
{
    GradIntra_Clientes_SalvarItemFilhoDoCliente("InformacoesPatrimoniais");
    
    return false;
}

function btnClientes_InformacaoBancaria_Salvar_Click(pSender)
{
    var txtCPF = $("#txtClientes_Conta_CPFDoTitular");

    if(txtCPF.val() != "")
    {
        //se tiver algum valor, precisa validar
        txtCPF.addClass("validate[funcCall[validatecpf]]");
    }
    else
    {
        txtCPF.removeClass("validate[funcCall[validatecpf]]");
    }

    if (GradIntra_Clientes_ValidarContaCorrente($(pSender).closest("div.pnlFormulario_Campos")))
    {
        GradIntra_Cadastro_SalvarItemFilho( "Conta"
                                          , $(pSender).closest("div.pnlFormulario_Campos")
                                          , "Clientes/Formularios/Dados/InformacoesBancarias.aspx");
    }

    return false;
}

function btnClientes_Telefones_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Telefone"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/Telefones.aspx");
    return false;
}

function btnClientes_Diretores_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Diretor"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/Diretores.aspx");
    return false;
}

function btnClientes_RepresentantesParaNaoResidente_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho("RepresentanteParaNaoResidente"
                                        , $(pSender).closest("div.pnlFormulario_Campos")
                                        , "Clientes/Formularios/Dados/RepresentantesParaNaoResidente.aspx");
    return false;
}

function btnClientes_EmpresasColigadas_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "EmpresaColigada"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/EmpresasColigadas.aspx");
    return false;
}

function btnClientes_Enderecos_Salvar_Click(pSender)
{
    ///$("#hidClientes_Enderecos_ListaJson").val($("#hidClientes_Enderecos_ListaJson").val().replace('"FlagCorrespondencia":true', '"FlagCorrespondencia":false', "gi"));

    GradIntra_Cadastro_SalvarItemFilho( "Endereco"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/Enderecos.aspx");
    return false;
}
function btnClientes_Procuradores_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Procurador"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/Procuradores.aspx");
    return false;
}

function btnClientes_Representantes_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Representante"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/RepresentantesLegais.aspx");
    return false;
}

function btnBusca_Clientes_Reserva_IPO_Click()
{
    GradIntra_Busca_BuscarItensParaListagemSimples( $("#pnlBusca_Clientes_Reserva_IPO_Form")
                                                  , $("#pnlClientes_Reserva_IPO_Resultados")
                                                  , "Clientes/Formularios/ReservarIPO.aspx"
                                                  , {
                                                        CamposObrigatorios: null //["txtBusca_Monitoramento_Ordens_Data"] 
                                                    }
                                                  );
    return false;
}

function btnAcoes_Pendenciacadastral_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "PendenciaCadastral"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Acoes/VerPendencias.aspx");
    return false;
}

function btnAcoes_alteracaocadastral_Salvar_Click(pSender)
{
//    var lObjetoJson = { Id            : $("#txtAcoes_Alteracaocadastral_Id").val()
//                      , ParentId      : $("#txtAcoes_AlteracaoCadastral_ParentId").val()
//                      , CdTipo        : $("#cboClientes_AlteracaoCadastral_Tipo").val()
//                      ,  DsInformacao : $("#cboClientes_AlteracaoCadastral_Informacao").val()
//                      ,  StResolvido  : $("#txtAcoes_AlteracaoCadastral_Resolvido").val()
//                      };
                    
    GradIntra_Cadastro_SalvarItemFilho( "SolicitacaoAlteracaoCadastral"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Acoes/SolicitacaoAlteracaoCadastral.aspx");
    return false;
}

function btnAcoes_AtivarInativarCliente_Salvar_Click(pSender)
{
    GradIntra_Cadastro_AtivarInativarCliente(pSender, "Clientes/Formularios/Acoes/Inativar.aspx");

    return false;
}
                    
function GradIntra_Acoes_AtivarInativar_Login_ZerarTentativasInvalidasDeLogin_Click(pSender)
{
    if ($(pSender).is(":checked") && confirm("Esta operação irá liberar o acesso do cliente para quantidade de tentativas de login inválido excedida. Deseja continuar?"))
    {
        var lDados = { Acao    : "ZerarTentativasInvalidasDeLogin"
                     , Nome    : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                     , Email   : gGradIntra_Cadastro_ItemSendoEditado.Email
                     , CdCodigo: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa }

        GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Acoes/Inativar.aspx"
                                             , lDados
                                             , GradIntra_Acoes_AtivarInativar_Login_ZerarTentativasInvalidasDeLogin_CallBack
                                             );
    }

    return true;
}

function GradIntra_Acoes_AtivarInativar_Login_ZerarTentativasInvalidasDeLogin_CallBack(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_TratarRespostaComErro(pResposta);
    }
    else
    {
        GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);
    }
}

function btnClientes_PessoasAutorizadas_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Emitente"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Dados/PessoasAutorizadas.aspx");
    return false;
}


function btnClientes_Acoes_SincronizarComSinacor_Click(pSender)
{
    if (confirm("Tem certeza que deseja iniciar a sincronização?"))
        GradIntra_Clientes_IniciarSincronizacaoComSinacor();

    return false;
}

function btnClientes_Importar_Click(pSender)
{
    GradIntra_Clientes_IniciarImportacao();

    return false;
}

function cboClientes_MigracaoEntreAssessores_AssessorDe_Change(pSender)
{
    if ( $("#pnlClientes_Assessores").is(":visible") )
    {
        $("#pnlClientes_MigracaoEntreAssessores")
            .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
            .html("")
            .parent()
                .hide();
    }
}

function btnClientes_Assessores_BuscarClientesDoAssessor_Click(pSender)
{
    return Grad_Intra_Sistema_MigracaoEntreAssessores_Buscar(pSender);
}

function trClientes_MigracaoEntreAssessores_Click(pSender)
{
    pSender = $(pSender);

    var lCheck = pSender.find("td:first-child input[type='checkbox']");

    if (lCheck.is(":checked"))
    {
        lCheck.prop("checked", false).next("label").removeClass("checked");
    }
    else
    {
        lCheck.prop("checked", true).next("label").addClass("checked");
    }

    GradIntra_Clientes_MigracaoEntreAssessores_VerificarMarcados();
}

function cboClientes_MigracaoEntreAssessores_AssessorPara_Change(pSender)
{
    pSender = $(pSender);

    var lValor = pSender.val();
    var lBotao = pSender.next("button");

    if (lValor  == "")
    {
        lBotao.prop("disabled", true);
    }
    else if (lValor  == $("#cboClientes_MigracaoEntreAssessores_AssessorDe").val())
    {
        lBotao.prop("disabled", true);

        GradIntra_ExibirMensagem("A", "O acessor de destino é o mesmo de origem, favor escolher outro.", true);
    }
    else
    {
        lBotao.prop("disabled", false);
    }
}

function btnClientes_MigracaoEntreAssessores_RealizarMigracao_Click(pSender)
{
    GradIntra_Clientes_MigracaoEntreAssessores_RealizarMigracao();
    
    return false;
}

function btnCliente_Suitability_Enviar_Click(pSender)
{
    GradIntra_Clientes_Suitability_EnviarRespostas();

    return false;
}

function btnClientes_Suitability_Refazer_Click(pSender)
{
    GradIntra_Clientes_Suitability_RefazerTeste();

    return false;
}

function btnClientes_Suitability_DownloadPDF_Click(pSender)
{
    GradIntra_Clientes_Suitability_DownloadPDF();

    return false;
}

function btnClientes_Suitability_EnviarPorEmail_Click(pSender)
{
    GradIntra_Clientes_Suitability_EnviarPorEmail();

    return false;
}

function btnClientes_Suitability_UploadDeclaracao_Click(pSender)
{
    $("#pnlClientes_Suitability_UploadDeclaracao").show();

    var txt = $("#txtClientes_Suitability_UploadDeclaracao");

    if(!txt.hasClass("Iniciado"))
    {
        txt.fileupload({
                            dataType: "json"
                          , url:      ("Clientes/Formularios/Acoes/Suitability.aspx?id=" + gGradIntra_Cadastro_ItemSendoEditado.Id)
                          , done:     txtClientes_Suitability_UploadDeclaracao_Callback
                      });

        txt.addClass("Iniciado");
    }

    return false;
}

function txtClientes_Suitability_UploadDeclaracao_Callback(pUpload, pResposta)
{
    pResposta = pResposta.result;

    if(pResposta.Mensagem == "ok" || pResposta.type == "fileuploaddone")
    {
        GradIntra_ExibirMensagem("I", "Arquivo gravado com sucesso", true);

        var lLink = $("#id_Cliente_Suitability_Resultado a");

        var lData = GradAux_DataDeHojeComHora();
        
        var lApend = "";

        if(GradIntra_RaizDoSite().indexOf(":4242") != -1)
            lApend = "/Intranet";

        if(lLink.length == 0)
        {

            $("#id_Cliente_Suitability_Resultado").append(" <a href='" + lApend + pResposta.ObjetoDeRetorno.replace("~", "") + "'>ciência: " + lData + "</a>");
        }
        else
        {
            lLink.attr("href", lApend + pResposta.ObjetoDeRetorno.replace("~", ""));
            lLink.text("ciência: " + GradAux_DataDeHojeComHora());
        }
    }
    else
    {
        GradIntra_ExibirMensagem("E", pResposta.Mensagem, false);
    }
    
    $("#pnlClientes_Suitability_UploadDeclaracao").hide();
}

function btnClientes_Suitability_UploadDeclaracao_Fechar_Click(pSender)
{
    $("#pnlClientes_Suitability_UploadDeclaracao").hide();

    return false;
}

function btnClientes_EfetivarRenovacao_Click(pSender)
{
    GradIntra_Clientes_EfetivarRenovacao();

    return false;
}


function cboRelatorios_TipoDeRelatorio_Change(pSender)
{
    pSender = $(pSender);
    
    var lDivFormulario = pSender.closest(".Busca_Formulario");
    var lValor = pSender.val();

    $(".pnlRelatorio").hide();

    GradIntra_Relatorio_OcultarExibirFiltros(lDivFormulario, lValor);
        
    $('#btnClienteRelatorioImprimir').prop("disabled", true);

    Clientes_Relatorios_ConfiguraToolTipsCamposData(pSender);
}

function Clientes_Relatorios_ConfiguraToolTipsCamposData(pSender)
{
    var txtToolTip = "";

    if ("R001" == $(pSender).val())
    {
        txtToolTip ="Data do Passo 1";
    }
    else if ("R002" == $(pSender).val())
    {
        txtToolTip ="Data da Pendência";
    }
    else if ("R003" == $(pSender).val())
    {
        txtToolTip ="Data da silicitação";
    }
    else if ("R004" == $(pSender).val())
    {
        txtToolTip ="Data de Renovação";
    }
    else if ("R005" == $(pSender).val())
    {
        txtToolTip ="Data da Última Exportação";
    }
    else if ("R006" == $(pSender).val())
    {
        txtToolTip ="Data do Suitability para os que realizaram e Data de Passo 1 para os que não realizaram";
    }
    else if ("R007" == $(pSender).val())
    {
        txtToolTip ="Data do Passo 1";
    }
    else if ("R008" == $(pSender).val())
    {
        txtToolTip ="Data de Envio do Email";
    }
    else if ("R009" == $(pSender).val())
    {
        txtToolTip ="Data de Cadastro";
    }

    $("#txtClientes_FiltroRelatorio_DataInicial").attr("title", txtToolTip);
    $("#txtClientes_FiltroRelatorio_DataFinal").attr("title", txtToolTip);
}

function btnClientes_Relatorios_FiltrarRelatorio_Click(pSender)
{
    var lRelatorio = $("#cboClientes_FiltroRelatorio_Relatorio").val();

    if (lRelatorio == "R018") 
    {
        var lClientes = $("#lstRelatorio018_ClientesAssessor").val()

        if (lClientes != null) 
        {
            if (lClientes.length < 1) 
            {
                alert('É necessário selecionar ao menos um cliente!');
                return false;
            }
        }
        else 
        {
            alert('É necessário selecionar ao menos um cliente!');
            return false;
        }

    }

    $(".pnlRelatorio").show();

    if (lRelatorio != "")
    {
        if (Clientes_Relatorios_FiltrarRelatorio_ValidarBusca(pSender))
            return false;

        var lUrl = "Clientes/Relatorios/" + lRelatorio + ".aspx";

        GradIntra_Relatorios_IniciarRecebimentoProgressivo( $("#pnlClientes_FiltroRelatorio_FF1")
                                                          , $("#pnlRelatorios_Resultados")
                                                          , lUrl
                                                          , lRelatorio);
    }
    else
    {   //--> não deve aparecer, porque o botão se oculta quando não tem um selecionado; #JIC.
        GradIntra_ExibirMensagem("A", "Favor selecionar um relatório");
    }

    return false;
}

function btnClientes_Relatorios_ImprimirRelatorio_Click(pSender) 
{
    var newWindow ;

    newWindow = window.open("Clientes/Relatorios/RelImpressao.aspx");

    //

    newWindow.onload = function () 
    {
        newWindow.document.write(document.getElementById("pnlRelatorios_Resultados").innerHTML);
        newWindow.focus();
        newWindow.print();
    };

    return false;
}

function Clientes_Relatorios_FiltrarRelatorio_ValidarBusca(pSender)
{
    var lRelatorio   = $("#cboClientes_FiltroRelatorio_Relatorio").val();
    var lDataInicial = $("#txtClientes_FiltroRelatorio_DataInicial").val();
    var lDataFinal   = $("#txtClientes_FiltroRelatorio_DataFinal").val();
    var lAssessor    = $("#cmbBusca_FiltroRelatorio_Assessor").val();
    var lCpfCnpj     = $("#txtClientes_FiltroRelatorio_CpfCnpj").val();
    var lPapel       = $("#txtClientes_FiltroRelatorio_Papel").val();
    var lCliente     = $("#txtClientes_FiltroRelatorio_CodCliente").val(); ;

    if (lRelatorio   == "R004")
    {
        var lDataInicialArray = lDataInicial.split('/');
        var lDataFinalArray = lDataFinal.split('/');

        var lTrocaDeAnoValida = (parseFloat(lDataInicialArray[1]) == 10 && parseFloat(lDataFinalArray[1]) > 1)
                             || (parseFloat(lDataInicialArray[1]) == 11 && parseFloat(lDataFinalArray[1]) > 2)
                             || (parseFloat(lDataInicialArray[1]) == 12 && parseFloat(lDataFinalArray[1]) > 3)
                             || (parseFloat(lDataInicialArray[1]) <= 09);

        if ((parseFloat(lDataFinalArray[2]) - parseFloat(lDataInicialArray[2]) != 0
        || (parseFloat(lDataFinalArray[1])  - parseFloat(lDataInicialArray[1]) < -3 || parseFloat(lDataFinalArray[1]) - parseFloat(lDataInicialArray[1]) > 3))
        && (lTrocaDeAnoValida))
        {
            alert('A pesquisa deste relatório está limitada a um período máximo de 90 dias por consulta.');

            var lMaximoMes = parseFloat(lDataInicialArray[1]) + 3;
            var lMaximoAno = lDataInicialArray[2];

            if (lMaximoMes > 12)
            {
                lMaximoMes = lMaximoMes - 12;
                lMaximoAno++;
            }

            $("#txtClientes_FiltroRelatorio_DataFinal").val(lDataFinalArray[0] + "/" + lMaximoMes + "/" + lMaximoAno);

            return true;
        }
    }

    if (lRelatorio == "R023") 
    {
        if (lAssessor == "") {
            alert("É necessário selecionar um assessor para efetuar a pesquisa.");
            return true;
        }
    }

    if (lRelatorio   == "R006"
    && (lDataInicial == "")
    && (lDataFinal   == "")
    && (lAssessor    == "")
    && (lCpfCnpj     == "")
    && !confirm("Caro usuário, você optou por uma consulta sem parâmetro pesquisa o que resultará num retorno mais lento dos resultados. Realmente deseja continuar?"))
    {
        return true;
    }
    
    if (lRelatorio   == "R014"
    && (lPapel == "" || lAssessor == ""))
    {
        GradIntra_ExibirMensagem("E", "Favor selecione um assessor e informe um papel");

        return true;
    }

    if (lRelatorio == "R017") {

        var lDataInicial = $("#txtClientes_FiltroRelatorio_DataInicial").val().split('/');
        var lDataFinal = $("#txtClientes_FiltroRelatorio_DataFinal").val().split('/');

        var lDateInicial = new Date(lDataInicial[2], lDataInicial[1] - 1, lDataInicial[0]);
        var lDateFinal   = new Date(lDataFinal[2], lDataFinal[1] - 1, lDataFinal[0]);

        var lDiferencaDias = Math.round((lDateFinal - lDateInicial) / (1000 * 60 * 60 * 24));

        if (lDateInicial > lDateFinal) {
            
            GradIntra_ExibirMensagem("I", "Data inicial deve ser menor que a data final.");

            return true;
        }

        if (lDiferencaDias > 31) {

            GradIntra_ExibirMensagem("I", "Insira um intervalo de data menor que 30 dias, entre data inicial e data final.");

            return true;
        }
    }

    if (lRelatorio == "R026") {

        if (lCliente == "") 
        {
            alert("É necessário inserir um cliente para efetuar a pesquisa.");

            return true;
        }
    }

    return false;
}

/*
    
    Segurança

*/

function btnSalvar_Seguranca_DadosCompletos_Click()
{
    GradIntra_Seguranca_SalvarDadosCompletos();

    return false;
}
/* Risco */
function GradIntra_Risco_Restricoes_TravaExposicaoSpider_Salvar_Click(pSender) 
{
    GradIntra_Risco_Restricoes_TravaExposicaoSpider_Salvar(pSender);

    return false;
}


function GradIntra_Risco_Restricoes_TravaExposicao_Salvar_Click(pSender) 
{
    GradIntra_Risco_Restricoes_TravaExposicao_Salvar(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_TravaExposicao_Load_Pagina() 
{
    GradIntra_Risco_Restricoes_TravaExposicao_Load();

    return false;
}

function btnSalvar_Risco_DadosGruposRestricoesSpider_Click() 
{
    GradIntra_Risco_SalvarDadosGrupoRestricaoSpider();

    return false;
}

function btnSalvar_Risco_DadosGruposRestricoes_Click() 
{
    GradIntra_Risco_SalvarDadosGrupoRestricao();

    return false;
}

function btnSalvar_Risco_DadosCompletos_Click()
{
    GradIntra_Risco_SalvarDadosCompletos();

    return false;
}


function btnSalvar_Risco_GrupoAlavancagem_Click(pSender)
{
    GradIntra_Risco_GrupoAlavancagem(pSender);

    return false;
}

/*
    Monitoramento
*/

function btnBusca_Monitoramento_UsuariosLogados_Click()
{
    $("#pnlConteudo_Monitoramento_UsuariosLogados")
        .find(".pnlFormularioExtendido")
        .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
        .addClass(CONST_CLASS_CARREGANDOCONTEUDO);
                
    $("#pnlConteudo_Monitoramento_UsuariosLogados")
        .find(".Aguarde").attr("style", "");

    $("#pnlConteudo_Monitoramento_UsuariosLogados_ListaDeResultados").hide();

    GradIntra_Navegacao_CarregarFormulario("Monitoramento/ResultadoUsuariosLogados.aspx", "pnlConteudo_Monitoramento_UsuariosLogados_ListaDeResultados", null, null);

    return false;
}

function btnBusca_Monitoramento_Ordens_Click(pSender)
{
    GradIntra_Busca_BuscarItensParaListagemSimples( $("#pnlBusca_Monitoramento_Ordens_Form")
                                                  , $("#pnlMonitoramento_ListaDeResultados")
                                                  , "Monitoramento/ResultadoOrdens.aspx"
                                                  , { 
                                                       CamposObrigatorios: [ "txtBusca_Monitoramento_Ordens_DataInicial"
                                                                           , "txtBusca_Monitoramento_Ordens_DataFinal"
                                                                           , "txtBusca_Monitoramento_Ordens_Papel"]
                                                    }
                                                  );
    return false;
}

function btnBusca_Monitoramento_OrdensStop_Click(pSender)
{
    GradIntra_Busca_BuscarItensParaListagemSimples( $("#pnlBusca_Monitoramento_OrdensStop_Form")
                                                  , $("#pnlMonitoramento_ListaDeResultados")
                                                  , "Monitoramento/ResultadoOrdensStop.aspx"
                                                  , { CamposObrigatorios: ["txtBusca_Monitoramento_OrdensStop_Data"] }
                                                  );
    return false;
}

function chkMonitoramento_ResultadoOrdensStop_SelecionarTodosResultados_OnClick(pSender) 
{
    ///<summary>[Event Handler] Event Handler da checkbox que marca/desmarca todas as linhas da tabela de resultado.</summary>
    ///<param name="pCheckbox" type="Objeto_HTML">Checkbox que foi clicada.</param>
    ///<returns>void</returns>

    var lTbody = $(pSender).closest("table").find("tbody");

    if (pSender.checked)
    {
        lTbody.find("tr input[type='checkbox']").prop("checked", true);
        lTbody.find("tr div.custom-checkbox label").addClass("checked");
    }
    else
    {
        lTbody.find("tr input[type='checkbox']").prop("checked", false);
        lTbody.find("tr div.custom-checkbox label").removeClass("checked");
    }
}

function btnMonitoramento_ExcluirOrdensStartStopSelecionados_Click(pSender) 
{
    if (confirm("Você realmente deseja cancelar as ordens selecionadas?"))
    {
        var lChecados = $(pSender)
                            .parent()
                                .parent().find("table.ui-jqgrid-btable")
                                    .find("tbody tr input[type='checkbox']:checked");
        var lIds = "";

        var lPapeis = "";

        var lStatus = "";

        var lPermitido = true;

        lChecados.each(function()
        {
            lIds += $(this).attr("rel") + ",";

            lPapeis +=$(this).closest('tr').children('td').eq(4).html() + ",";

            lStatus = $(this).closest('tr').children('td').eq(13).html();

            if (lStatus.toLowerCase() != "aberta")
                lPermitido = false;
        });

        if (lPermitido)
            GradIntra_Monitoramento_ExcluirOrdensStartStopSelecionadas(lIds, lPapeis);
        else
            alert("O cancelamento é permitido somente para ordens abertas.");
    }

    return false;
}

function chkBusca_SelecionarTodosResultados_OnClick(pSender) 
{
    ///<summary>[Event Handler] Event Handler da checkbox que marca/desmarca todas as linhas da tabela de resultado.</summary>
    ///<param name="pCheckbox" type="Objeto_HTML">Checkbox que foi clicada.</param>
    ///<returns>void</returns>

    var lTbody = $(pSender).closest("table").find("tbody");

    if (pSender.checked)
    {
        lTbody.find("tr input[type='checkbox']").prop("checked", true);
        lTbody.find("tr div.custom-checkbox label").addClass("checked");
    }
    else
    {
        lTbody.find("tr input[type='checkbox']").prop("checked", false);
        lTbody.find("tr div.custom-checkbox label").removeClass("checked");
    }
}
function btnMonitoramento_ExcluirOrdensNovoOMSSelecionadas_Click(pSender) 
{
    if (confirm("Você realmente deseja cancelar as ordens selecionadas?")) 
    {
    
        var lChecados = $(pSender)
                            .parent()
                                .parent().find("table.ui-jqgrid-btable")
                                    .find("tbody tr input[type='checkbox']:checked");
        var lIds = "";

        var lStatus = "";

        var lPortas = "";

        var lPermitido = true;

        lChecados.each(function () 
        {
            lPortas += $(this).closest('tr').children('td').eq(14).html() + ",";
            
            lIds    += $(this).attr("rel") + ",";

            lStatus  = $(this).closest('tr').children('td').eq(5).html();

            if (lStatus.toLowerCase() != "aberta" && lStatus.toLowerCase() != "parcialmente executada" && lStatus.toLowerCase() != "substituída") 
            {
                lPermitido = false;
            }
        });

        if (lPermitido) 
        {
            GradIntra_Monitoramento_ExcluirOrdensNovoOMSSelecionadas(lIds, lPortas);
        }
        else 
        {
            alert("O cancelamento é permitido somente para ordens abertas.");
        }
    }

    return false;
}

function btnMonitoramento_ExcluirOrdensSpiderSelecionadas_Click(pSender) 
{
    if (confirm("Você realmente deseja cancelar as ordens selecionadas?")) {
        //var lChecados = $(pSender).closest("table").find("tbody tr input[type='checkbox']:checked");
        var lChecados = $(pSender)
                            .parent()
                                .parent().find("table.ui-jqgrid-btable")
                                    .find("tbody tr input[type='checkbox']:checked");
        var lIds = "";

        var lStatus = "";

        var lPortas = "";

        var lAccounts = "";

        var lSymbols = "";

        var lPermitido = true;

        lChecados.each(function () 
        {
            lPortas += $(this).closest('tr').children('td').eq(14).html() + ",";

            lIds += $(this).attr("rel") + ",";

            lAccounts += $(this).closest('tr').children('td').eq(3).html() + ",";

            lSymbols += $(this).closest('tr').children('td').eq(7).html() + ",";

            lStatus = $(this).closest('tr').children('td').eq(5).html();

            if (lStatus.toLowerCase() != "aberta" && lStatus.toLowerCase() != "parcialmente executada" && lStatus.toLowerCase() != "substituída") {
                lPermitido = false;
            }
        });

        if (lPermitido) {
            GradIntra_Monitoramento_ExcluirOrdensSelecionadasSpider(lIds, lPortas, lAccounts, lSymbols);
        }
        else 
        {
            alert("O cancelamento é permitido somente para ordens abertas.");
        }
    }

    return false;
}

function btnMonitoramento_ExcluirOrdensSelecionadas_Click(pSender) 
{
    if (confirm("Você realmente deseja cancelar as ordens selecionadas?"))
    {
        //var lChecados = $(pSender).closest("table").find("tbody tr input[type='checkbox']:checked");
        var lChecados = $(pSender)
                            .parent()
                                .parent().find("table.ui-jqgrid-btable")
                                    .find("tbody tr input[type='checkbox']:checked");
        var lIds = "";

        var lStatus = "";

        var lPortas = "";

        var lPermitido = true;

        lChecados.each(function () 
        {
            lPortas += $(this).closest('tr').children('td').eq(13).html() + ",";
            
            lIds    += $(this).attr("rel") + ",";

            lStatus  = $(this).closest('tr').children('td').eq(5).html();

            if (lStatus.toLowerCase() != "aberta" && lStatus.toLowerCase() != "parcialmente executada" && lStatus.toLowerCase() != "substituída") 
            {
                lPermitido = false;
            }
        });

        if (lPermitido) 
        {
            GradIntra_Monitoramento_ExcluirOrdensSelecionadas(lIds, lPortas);
        }
        else 
        {
            alert("O cancelamento é permitido somente para ordens abertas.");
        }
    }

    return false;
}

function btnSistema_Variaveis_Salvar_Click(pSender)
{
    GradIntra_Sistema_SalvarVariaveis();
    return false;
}

function btnSistema_ObjetosDoSistema_Incluir_Click(pSender)
{
    GradIntra_Sistema_IncluirObjetoDeSistema();
    return false;
}

function btnSistema_ObjetosDoSistema_Excluir_Click(pUrl, pSender)
{
    GradIntra_Sistema_ExcluirObjetoDeSistema(pUrl, pSender);
    return false;
}

function btnSistema_PessoasVinculadas_Excluir_Click(pUrl, pSender)
{
    GradIntra_Sistema_ExcluirPessoasVinculadas(pUrl, pSender);
    return false;
}

function btnSistema_PessoasVinculadas_Incluir_Click( pSender)
{
    GradIntra_Sistema_SalvarPessoasVinculadas($(pSender).closest("div.pnlFormulario_Campos"));
    return false;
}

function btnSalvar_Papeis_DadosCompletos_Click(pSender)
{
    return false;
}

function btnSistema_PessoasVinculadas_BuscarCliente_Click(pSender)
{
    GradIntra_Sistema_BuscarClientePessoasVinculadas();
    return false;
}

function btn_imprimir_ficha_click()
{
    GradInfra_Relatorio_ImprimirFichaDuc();
    return false;
}

function btn_imprimir_ficha_cambio_click() {
    GradInfra_Relatorio_ImprimirFichaCambio();
    return false;
}

function btn_imprimir_contratoPF_click()
{
    //window.open('../Extras/Contratos/ContratoPF.pdf', null, null, null);
    GradInfra_Relatorio_Contrato_PF();
    return false;
}

function btn_imprimir_contratoPJ_click()
{
    var lLocal = '../Extras/Contratos/ContratoPJ-1501.pdf';

    /*
    if(document.location.href.indexOf("192.168") != -1)
        lLocal = '../Intranet/Extras/Contratos/ContratoPJ-1501.pdf';
    */

    window.open(lLocal, null, null, null);

    return false;
}

function lstMenuSeguranca_Excluir_OnClick(pSender) 
{
    GradIntra_Seguranca_ExcluirItem()
    return false;
}

function lstMenuRisco_Excluir_OnClick(pSender) 
{
    GradIntra_Risco_ExcluirItem()
    return false;
}

function btnSeguranca_Usuarios_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Usuarios"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Seguranca/Formularios/Dados/Usuarios.aspx");
    return false;
}

function btnSeguranca_Grupos_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "Grupos"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Seguranca/Formularios/Dados/Grupos.aspx");
    return false;
}

function btnSeguranca_Permissoes_Salvar_Click(pSender) 
{
    GradIntra_Cadastro_SalvarItemFilho( "Permissoes"
                                      , $("#pnlSeguranca_Permissoes_Campos")
                                      , "Seguranca/Formularios/Dados/Permissoes.aspx");
    return false;
}

function btnSeguranca_Perfis_Salvar_Click(pSender) 
{
    GradIntra_Cadastro_SalvarItemFilho( "Perfis"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Seguranca/Formularios/Dados/Perfis.aspx");
    return false;
}

function btnSeguranca_Desbloqueio_Salvar_Click(pSender) 
{

    if ($("#pnlSeguranca_Formularios_Dados_Desbloqueio").validationEngine({ returnIsValid: true })) 
    {
        var lDados = { Acao: "Salvar"
                 , CodigoCliente: $("#txtSeguranca_Desbloqueio_Codigo_cliente").val()
        };

        GradIntra_CarregarJsonVerificandoErro("Seguranca/Formularios/Dados/Desbloqueio.aspx"
                                         , lDados
                                        , btnSeguranca_Desbloqueio_Salvar_CallBack);
    }

    return false;
}

function btnSeguranca_Desbloqueio_Salvar_CallBack(pResposta)
{
    $("#txtSeguranca_Desbloqueio_Codigo_cliente").val("");

    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function btnSeguranca_AlterarSenha_Salvar_Click(pSender) 
{
    var lDados = { Acao          : "AlterarSenha"
                 , SenhaAtual    : $("#txt_Seguranca_AlterarSenha_SenhaAtual").val()
                 , SenhaNova     : $("#txt_Seguranca_AlterarSenha_NovaSenha").val()
                 , SenhaConfirma : $("#txt_Seguranca_AlterarSenha_ConfirmaNovaSenha").val()};

    if (ValidatePasswordFieldForm("#txt_Seguranca_AlterarSenha_NovaSenha")) 
    {
        GradIntra_CarregarJsonVerificandoErro("Seguranca/Formularios/Dados/AlterarSenha.aspx"
                                         , lDados
                                         , btnSeguranca_AlterarSenha_Salvar_CallBack);
    }

    return false;
}

function btnClientes_LinkProspect_Click(pSender) 
{
    var lDados = { Acao: "ConsultarLinkProspect"
                 , Nome: $("#txtCliente_LinkProspect_Nome").val()};

    GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/LinkParaProspect.aspx"
                                         , lDados
                                         , btnClientes_LinkProspect_CallBack);
    return false;
}

function btnClientes_LinkProspect_CallBack(pResposta)
{
    var lTransporte = pResposta.ObjetoDeRetorno;

    $(lTransporte).each(function()
    {
        $("#pnlClientes_LinkProspect_Resultado")
            .append("<p style='margin-left:20px;font-size:1.1em;line-height:1.6em'><span style='float:left;width:6em'>Assessor:</span> "+(this.Value)+
            "<br/><span style='float:left;width:6em;margin-right:-4px'>Link:</span><input type='text' readonly='readonly' style='border:0;background:transparent;width:90%' value='https://www.gradualinvestimentos.com.br/MinhaConta/Cadastro/CadastroPF.aspx?a=" + (this.Id) + "'></input></p><p></p>");
    });

     $("#pnlClientes_LinkProspect_Resultado").show();

    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function btnSeguranca_AlterarSenha_Salvar_CallBack(pResposta)
{
   $("#txt_Seguranca_AlterarSenha_SenhaAtual").val("");
   $("#txt_Seguranca_AlterarSenha_NovaSenha").val("");
   $("#txt_Seguranca_AlterarSenha_ConfirmaNovaSenha").val("");

    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function btnClientes_Contratos_Salvar_Click(pSender)
{
    GradIntra_Clientes_SalvarContratos($(pSender).closest("div.pnlFormulario_Campos"));

    return false;
}

function cboPerfilDeRisco_Regras_Regra_Change(pSender)
{
    GradIntra_Risco_CarregarParametrosDaRegra();
}


function btnPerfilDeRisco_Regras_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "ItemGrupo"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Risco/Formularios/Dados/Itens.aspx");
    return false;
}

function btnSalvar_Risco_AssociarPermissoesParametros_Click(pSender) 
{
    GradIntra_Cadastro_SalvarItemFilho( "Associacao"
                                      , $("#pnlFormulario_Campos_AssociarPermissoesParametros")
                                      , "Clientes/Formularios/Acoes/AssociarPermissoesParametros.aspx");
    return false;
}

function btnSistema_PEP_btnBuscar_Click(psender)
{
    GradIntra_Sistema_PEP_Buscar();
    return false;
}

function btnSistema_PEP_EnviarArquivoParaImportacao_Click(pSender)
{
    GradIntra_Sistema_PEP_EnviarArquivoParaImportacao();

    return false;
}

function cmbRelatorio_Posicao_Carregar_Change()
{
    var lVal = $("#cmbRelatorio_Posicao_Carregar").val();
    
    var lPainelOpcoes = $("#pnlRelatorio_Posicao");
    
    var lBotaoCarregar = $("p.BotaoCarregarRelatorio");
    
    var lSubFormularios = $(".PainelEmTelaCheia_Opcoes_SubFormulario");
    
        lSubFormularios.hide();
        
    if (lVal == "-1")
    {        
        //lPainelOpcoes.children("p.DescricaoDaOpcao").show();
        
        lSubFormularios.hide();
        lBotaoCarregar.hide();        
    }
    else
    {
        //lPainelOpcoes.children("p.DescricaoDaOpcao").hide();
        
        lSubFormularios.eq(lVal).show(); 
        if (lVal == "6")
            lBotaoCarregar.hide();
        else
            lBotaoCarregar.show();
    }

    if (lVal == "2")
    {        
        FiltrarComboMercadoPorBolsa($("#cboRelatorio_Custodia_Bolsa"));
    }
}

function FiltrarComboMercadoPorBolsa(pSender)
{
    if ("bov" == $(pSender).val()) {
        $("#GrdIntra_Posicao_Cliente_Nota_Corretagem_PainelTipoMercado").show();
        $("#cboRelatorio_Custodia_Mercado").val("").find("option").hide().parent().find(".Visivel_Bovespa").show();
    }
    else {
        $("#GrdIntra_Posicao_Cliente_Nota_Corretagem_PainelTipoMercado").hide();
        $("#cboRelatorio_Custodia_Mercado").val("").find("option").hide().parent().find(".Visivel_Bmf").show();
    }

    return false;
}

var gRelatorioCarregando = false;
var gIndiceRelatorioSelecionadoAnteriormente = -1;

function ValidacaoCarregarRelatorioNotaCorretagem() {
    var lRetorno = true;

    var lDataInicial = $("#txtRelatorio_ExtratoFatura_Data_Inicial").val().split('/');
    var lDataFinal =  $("#txtRelatorio_ExtratoFatura_Data_Final").val().split('/');

    var lDateInicial = new Date(lDataInicial[2], lDataInicial[1]-1, lDataInicial[0]);
    var lDateFinal   = new Date(lDataFinal[2], lDataFinal[1]-1, lDataFinal[0]);

    var lDiferencaDias = Math.round((lDateFinal - lDateInicial) / (1000 * 60 * 60 * 24));

    if (lDiferencaDias > 31) {

        GradIntra_ExibirMensagem("I", "Insira um intervalo de data menor que 30 dias, entre data inicial e data final ");

        lRetorno = false;
    }

    return lRetorno;
}
function CarregarRelatorioPosicaoCliente(pCarregarComoCSV)
{
    if (!gRelatorioCarregando)
    {   
        if (gDataDaNotaParaCSV == "") //--> Seta o valor inicial para a variável. Esta é alterada depois somente na
            gDataDaNotaParaCSV= $("#txtRelatorio_ExtratoFatura_Data_Inicial").val(); // função 'PaginarNotaDeCorretagem'.

        var lUrl = null;
        var lDados = null;
        
        var lValue = $("#cmbRelatorio_Posicao_Carregar").val();

        if (lValue == -1)
        {
            lValue = gIndiceRelatorioSelecionadoAnteriormente;
        }
        else
        {
            gIndiceRelatorioSelecionadoAnteriormente = lValue;
        }
            
        gRelatorio_DescricaoDoRelatorioEscolhido = "";
            
        switch(lValue)
        {
            case "0" :  
            
                lUrl = "Posicao/ExtratoDeConta.aspx";            
                                
                lDados = { CdBovespaCliente : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                         , CdBmfCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                         , NomeCliente      : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                         , DataInicial      : $("#txtRelatorio_Extrato_DataInicial").val()
                         , DataFinal        : $("#txtRelatorio_Extrato_DataFinal").val()
                         , TipoExtrato      : $("#fdsTipoDeExtrato :radio[checked]").val() };
                
                gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Extrato de Conta, de " + lDados.DataInicial + " até " + lDados.DataFinal;
                
                break;

            case "1":
                {
                    var lBolsa = $("#cboRelatorio_Custodia_Bolsa").val();

                    if (!ValidacaoCarregarRelatorioNotaCorretagem()) 
                    {
                        return false;
                    }

                    if (lBolsa == "bov") {

                        lUrl = "Posicao/NotaDeCorretagem.aspx";

                        lDados = { CdBovespaCliente: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                                 , CdBmfCliente: gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                                 , NomeCliente: gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                                 , TipoMercado: $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val()
                                 , Provisorio: $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked")
                                 , Bolsa: $("#cboRelatorio_NotasDeCorretagem_Bolsa").val()
                                 , DataInicial: $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                                 , DataInicialPaginacao: $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                                 , DataFinal: $("#txtRelatorio_ExtratoFatura_Data_Final").val()
                        };
                    }
                    else if (lBolsa == "bmf") {
                        lUrl = "Posicao/NotaDeCorretagemBmf.aspx"

                        lDados = { CdBovespaCliente: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                                 , CdBmfCliente: gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                                 , NomeCliente: gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                                 , TipoMercado: $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val()
                                 , Provisorio: $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked")
                                 , Bolsa: $("#cboRelatorio_NotasDeCorretagem_Bolsa").val()
                                 , DataInicial: $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                                 , DataInicialPaginacao: $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                                 , DataFinal: $("#txtRelatorio_ExtratoFatura_Data_Final").val()
                        };
                    }

                    gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Notas de Corretagem, bolsa [" + lDados.Bolsa + "], de " + lDados.DataFinal;
                }
                break;
                
            case "2" :  
            
                lUrl = "Posicao/Custodia.aspx";                  
                
                lDados = { CdBovespaCliente : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                         , CdBmfCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                         , NomeCliente      : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                         , Bolsa            : $("#cboRelatorio_Custodia_Bolsa").val()
                         , Agrupar          : $("#cboRelatorio_Custodia_Agrupar").val()
                         , Mercado          : $("#cboRelatorio_Custodia_Mercado").val()
                         };
                
                gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Notas de Corretagem, bolsa [" + lDados.Bolsa + "], agrupamento [" + lDados.Agrupar + "], mercado " + lDados.Mercado;
                
                break;
                
            case "3" :  
            
                lUrl = "Posicao/Financeiro.aspx";                
                
                lDados = { CdBovespaCliente : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                         , CdBmfCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                         , NomeCliente      : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                         };
                         
                gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório Financeiro (Saldo de Conta)";
                
                break;
                
            case "4" :  
            
                lUrl = "Posicao/VolumesDeCorretagem.aspx";       
                
                lDados = { CdBovespaCliente : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                         , CdBmfCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                         , NomeCliente      : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                         , DataInicial      : $("#txtRelatorio_Volume_DataInicial").val()
                         , DataFinal        : $("#txtRelatorio_Volume_DataFinal").val()
                         };
                         
                gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Volume de Corretagem, de " + lDados.DataInicial + " até " + lDados.DataFinal;
                
                break;
                
            case "5" :  
            
                lUrl = "Posicao/HistoricoDeMovimentacoes.aspx";  
                
                lDados = { CdBovespaCliente : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                         , CdBmfCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                         , NomeCliente      : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                         , Data             : $("#txtRelatorio_HistoricoMovimentacoes_Data").val()
                         };
                         
                gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Histórico de Movimentações, a partir de " + lDados.Data;
                
                break;

            case "7":
                {
                    var lTipoFax = $("#cboRelatorio_Fax_Tipo").val();

//                    var now = GradAux_DataDeHoje();
//                    var lTime =  $("#txtRelatorio_Fax_Data_Inicial").val();

//                    if (now != lTime)
//                    {
//                        if (!confirm("Ao rodar o fax para pregões passados será recalculado o financeiro do cliente, deseja continuar?"))
//                            return false;
//                    }


                    switch (lTipoFax) {
                        case "bov_br":
                            lUrl = "Posicao/FaxBovespaBR.aspx";
                            break;
                        case "bov_resumido":
                            lUrl = "Posicao/FaxBovespaResumido.aspx";
                            break;
                        case "bov_en":
                            lUrl = "Posicao/FaxBovespaEN.aspx";
                            break;
                        case "bmf_br":
                            lUrl = "Posicao/FaxBmfBR.aspx";
                            break;
                        case "bmf_volatilidade":
                            lUrl = "Posicao/FaxBmfVolatilidade.aspx";
                            break;
                        case "bmf_en":
                            lUrl = "Posicao/FaxBmfEN.aspx";
                            break;
                    }

                    lDados = {
                        CdBovespaCliente: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                            , CdBmfCliente: gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                            , NomeCliente: gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                            , DataInicial: $("#txtRelatorio_Fax_Data_Inicial").val()
                        //, DataFinal:        $("#txtRelatorio_Fax_Data_Final").val()
                    };

                    gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório Fax Posição  " + lDados.DataInicial + " até " + lDados.DataFinal;
                }
                break;

            case "8":
                {
                    var lTipoFax = $("#cboRelatorio_ExtratoOperacoes_Tipo").val();
                    var lUrl = "Posicao/ExtratoOperacoesBovespa.aspx";

                    lDados = 
                    {
                              CdBovespaCliente  : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                            , CdBmfCliente      : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                            , NomeCliente       : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                            , DataInicial:      $("#txtRelatorio_Operacoes_Data_Inicial").val()
                        //, DataFinal:        $("#txtRelatorio_Fax_Data_Final").val()
                    };

                    gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório Extrato Posição  " + lDados.DataInicial + " até " + lDados.DataFinal;
                }
                break;
        }
        
        if (lUrl != null)
        {    
            if (pCarregarComoCSV)
            {
                lUrl += "?Acao=CarregarComoCSV"

                switch(lValue)
                {
                    case "0" :  lUrl += "&DataInicial="          + $("#txtRelatorio_Extrato_DataInicial").val()
                                     +  "&DataFinal="            + $("#txtRelatorio_Extrato_DataFinal").val()
                                     +  "&CdBovespaCliente="     + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                                     +  "&CdBmfCliente="         + gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                                     +  "&NomeCliente="          + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
                                break;                           
                                                                 
                    case "1" :  lUrl += "&CdBovespaCliente="     + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                                     +  "&CdBmfCliente="         + gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                                     +  "&TipoMercado="          + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val()
                                     +  "&Provisorio="           + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked")
                                     +  "&Bolsa="                + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val()
                                     +  "&DataInicial="          + gDataDaNotaParaCSV
                                     +  "&DataInicialPaginacao=" + $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                                     +  "&DataFinal="            + $("#txtRelatorio_ExtratoFatura_Data_Final").val()
                                     +  "&NomeCliente="          + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
                                break;                           
                                                                 
                    case "2" :  lUrl += "&CdBovespaCliente="     + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                                     +  "&CdBmfCliente="         + gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                                     +  "&Bolsa="                + $("#cboRelatorio_Custodia_Bolsa").val()
                                     +  "&Agrupar="              + $("#cboRelatorio_Custodia_Agrupar").val()
                                     +  "&Mercado="              + $("#cboRelatorio_Custodia_Mercado").val()
                                     +  "&NomeCliente="          + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
                                break;                           
                                                                 
                    case "3" :  lUrl += "&CdBovespaCliente="     + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                                     +  "&CdBmfCliente="         + gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                                     +  "&NomeCliente="          + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
                                break;                           
                                                                 
                    case "4" :  lUrl += "&DataInicial="          + lDados.DataInicial + "&DataFinal=" + lDados.DataInicial; break;
                                                                 
                    case "5" :  lUrl += "&Data="                 + lDados.Data; break;
                }
                
                window.open(lUrl);
            }
            else
            {
                gRelatorioCarregando = true;

                GradIntra_CarregarHtmlVerificandoErro( lUrl
                                                     , lDados
                                                     , null
                                                     , CarregarRelatorioPosicaoCliente_CallBack);
            }
        }
        else
        {
            alert("Relatório Indisponível!");
        }
    }
    
    return false;
}

function CarregarRelatorioPosicaoCliente_CallBack(pResposta)
{
    gRelatorioCarregando = false;
    
    if (pResposta.indexOf("Erro:") >= 0)
        GradIntra_ExibirMensagem("E", pResposta);
    else
    {
        $("#pnlCliente_Posicao_Relatorio").html(pResposta).show();

        $("#divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio").show();

        if ($("#cmbRelatorio_Posicao_Carregar").val() == 1)
            ConfigurarPaginacaoNotaDeCorretagem();

        CarregarRelatorioPosicaoClienteOcultarCampos();

        $("#cmbRelatorio_Posicao_Carregar").val(""); //--> Setando o compo para o primeiro item.

        $(".PainelEmTelaCheia_Opcoes_SubFormulario").hide(); //--> Ocultando o painel anterior.
     }   

    return false;
}

function CarregarRelatorioPosicaoClienteOcultarCampos()
{
    if ($("#cmbRelatorio_Posicao_Carregar").val() == "2")
    {
        if ($("#trExtratoCustodiaRelatorioVazio").length > 0
        && ($("#trExtratoCustodiaRelatorioVazio").is(":visible")))
        {
            $(".SubTotal").hide();
            $(".Total").html("");
        }
        else if ("VIS" == $("#cboRelatorio_Custodia_Mercado").val())
        {
            $(".SubTotal").not(".SubTotalMercadoAVista").hide();
        }
        else if ("FUT" == $("#cboRelatorio_Custodia_Mercado").val())
        {
            $(".SubTotal").not(".SubTotalMercadoFuturo").hide();
        }
        else if ("OPC" == $("#cboRelatorio_Custodia_Mercado").val())
        {
            $(".SubTotal").not(".SubTotalMercadoDeOpcoes").hide();
        }
        else if ("TER" == $("#cboRelatorio_Custodia_Mercado").val())
        {
            $(".SubTotal").not(".SubTotalMercadoDeTermo").hide();
        }
    }
}

function GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodos(pSender)
{
    var lUrl = "Posicao/NotaDeCorretagem_ImpressaoEmLote.aspx?DatasNC=" + $("#hddDatasPaginacao").val();
    
    lUrl += "&CdBovespaCliente=" + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="     + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="      + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="       + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="            + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente=" + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    
    gRelatorioCarregando = false;
    
    var lWindow = window.open(lUrl);

    //lWindow.print();

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodosBmf(pSender) 
{
    var lUrl = "Posicao/NotaDeCorretagemBmf_ImpressaoEmLote.aspx?DatasNC=" + $("#hddDatasPaginacao").val();

    lUrl += "&CdBovespaCliente=" + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente=" + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado=" + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio=" + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa=" + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente=" + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;

    gRelatorioCarregando = false;
    
    var lWindow = window.open(lUrl);

    //lWindow.print();

    return false;
}

//function CarregarRelatorioPosicaoCliente_CallBack()
//{
//    if ($("#divClientes_Relatorios_TextoConfigurarImpressao").is(":visible"))
//    {
//        $("#divClientes_Relatorios_TextoConfigurarImpressao")
//            .hide()
//    }
//    else
//    {
//        $("#divClientes_Relatorios_TextoConfigurarImpressao")
//            .show()
//            .css(
//                {
//                    left:$(pSender).position().left - 270,
//                    top:$(pSender).position().top + 20
//                });
//    }
//    return false;
//}

var gGradIntra_Risco_Spider_Restricoes_UL_Selecionada = "";
function pnlFormulario_Abas_li_a_Restricoes_Spider_Click(pSender) 
{
    pSender = $(pSender);

    var lUL = pSender.closest("ul");

    lUL.find("li.Selecionada").removeClass("Selecionada");

    lUL.find("li a[rel]").each(function () { $("#" + $(this).attr("rel")).hide(); });

    $("#" + pSender.attr("rel")).show();

    pSender.parent().addClass("Selecionada");

    if (pSender.attr("rel") == "pnlCliente_Restricoes_Individual") 
    {
        $("#DivGradIntraRisco_Restricao_Global_OMS_Spider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemCliente_Spider").css("display", "block");
        $("#DivtblGradIntra_Risco_ItemGrupoSpider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemGrupo_GlobalSpider").css("display", "none");
        $("#pGradIntra_Risco_ItensDoGrupo_Grupos").css("display", "none");
        $("#pCliente_Restricoes_RestricaoPorCliente").css("display", "block");
        $("#pCliente_Restricoes_RestricaoPorAtivo").css("display", "block");

        gGradIntra_Risco_Spider_Restricoes_UL_Selecionada = "pnlCliente_Restricoes_Individual";

        GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItemSpider();
    }
    else if (pSender.attr("rel") == "pnlCliente_Restricoes_Grupo") 
    {
        $("#DivGradIntraRisco_Restricao_Global_OMS_Spider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemCliente_Spider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemGrupoSpider").css("display", "block");
        $("#DivtblGradIntra_Risco_ItemGrupo_GlobalSpider").css("display", "none");
        $("#pCliente_Restricoes_RestricaoPorCliente").css("display", "block");
        $("#pCliente_Restricoes_RestricaoPorAtivo").css("display", "none");
        $("#pGradIntra_Risco_ItensDoGrupo_Grupos").css("display", "block");

        gGradIntra_Risco_Spider_Restricoes_UL_Selecionada = "pnlCliente_Restricoes_Grupo";
    }
    else if (pSender.attr("rel") == "pnlCliente_Restricoes_Grupo_Global") 
    {
        $("#DivGradIntraRisco_Restricao_Global_OMS_Spider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemCliente_Spider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemGrupoSpider").css("display", "none");
        $("#DivtblGradIntra_Risco_ItemGrupo_GlobalSpider").css("display", "block");
        $("#pGradIntra_Risco_ItensDoGrupo_Grupos").css("display", "block");
        $("#pCliente_Restricoes_RestricaoPorCliente").css("display", "none");
        $("#pCliente_Restricoes_RestricaoPorAtivo").css("display", "none");

        gGradIntra_Risco_Spider_Restricoes_UL_Selecionada = "pnlCliente_Restricoes_Grupo_Global";
    }

    return false;
}


var gGradIntra_Risco_Restricoes_UL_Selecionada = "";
function pnlFormulario_Abas_li_a_Restricoes_Click(pSender) 
{
    pSender = $(pSender);

    var lUL = pSender.closest("ul");

    lUL.find("li.Selecionada").removeClass("Selecionada");

    lUL.find("li a[rel]").each(function () { $("#" + $(this).attr("rel")).hide(); });

    $("#" + pSender.attr("rel")).show();

    pSender.parent().addClass("Selecionada");

    if (pSender.attr("rel") == "pnlCliente_Restricoes_Individual") 
    {
        $("#DivGradIntraRisco_Restricao_Global_OMS")  .css("display", "none");
        $("#DivtblGradIntra_Risco_ItemCliente")       .css("display", "block");
        $("#DivtblGradIntra_Risco_ItemGrupo")         .css("display", "none");
        $("#DivtblGradIntra_Risco_ItemGrupo_Global")  .css("display", "none");
        $("#pGradIntra_Risco_ItensDoGrupo_Grupos")    .css("display", "none");
        $("#pCliente_Restricoes_RestricaoPorCliente") .css("display", "block");
        $("#pCliente_Restricoes_RestricaoPorAtivo")   .css("display", "block");
        
        gGradIntra_Risco_Restricoes_UL_Selecionada     = "pnlCliente_Restricoes_Individual";
        
        GradIntra_Risco_Restricoes_RegraIndividual_AdicionarItem();
    }
    else if (pSender.attr("rel") == "pnlCliente_Restricoes_Grupo") 
    {
        $("#DivGradIntraRisco_Restricao_Global_OMS") .css("display", "none" );
        $("#DivtblGradIntra_Risco_ItemCliente")      .css("display", "none" );
        $("#DivtblGradIntra_Risco_ItemGrupo")        .css("display", "block");
        $("#DivtblGradIntra_Risco_ItemGrupo_Global") .css("display", "none" );
        $("#pCliente_Restricoes_RestricaoPorCliente").css("display", "block");
        $("#pCliente_Restricoes_RestricaoPorAtivo")  .css("display", "none" );
        $("#pGradIntra_Risco_ItensDoGrupo_Grupos").css("display", "block");

        gGradIntra_Risco_Restricoes_UL_Selecionada = "pnlCliente_Restricoes_Grupo";
    }
    else if (pSender.attr("rel") == "pnlCliente_Restricoes_Grupo_Global") 
    {
        $("#DivGradIntraRisco_Restricao_Global_OMS") .css("display", "none");
        $("#DivtblGradIntra_Risco_ItemCliente")      .css("display", "none" );
        $("#DivtblGradIntra_Risco_ItemGrupo")        .css("display", "none" );
        $("#DivtblGradIntra_Risco_ItemGrupo_Global") .css("display", "block");
        $("#pGradIntra_Risco_ItensDoGrupo_Grupos")   .css("display", "block");
        $("#pCliente_Restricoes_RestricaoPorCliente").css("display", "none");
        $("#pCliente_Restricoes_RestricaoPorAtivo").css("display", "none");

        gGradIntra_Risco_Restricoes_UL_Selecionada = "pnlCliente_Restricoes_Grupo_Global";
    }

    return false;
}

function pnlFormulario_Abas_li_a_Click(pSender)
{
    pSender = $(pSender);

    var lUL = pSender.closest("ul");

    lUL.find("li.Selecionada").removeClass("Selecionada");

    lUL.find("li a[rel]").each(function() { $("#" + $(this).attr("rel")).hide(); });

    $("#" + pSender.attr("rel")).show();

    pSender.parent().addClass("Selecionada");

    return false;
}

function cboCliente_Limites_AdicionarContrato_Change(pSender)
{
    pSender = $(pSender);

    if (pSender.val() != "")
    {
        var lContrato = $(pSender.find("option:selected"));

        var lInstrumentos = $( lContrato.attr("Instrumentos").split(",") );

        var lInstrumento;

        $("#cboCliente_Limites_AdicionarInstrumento").find("option[value!='']").remove();

        lInstrumentos.each(function()
        {
            lInstrumento = (this + "").trim();

            $("#cboCliente_Limites_AdicionarInstrumento")
                .append( $("<option>").html(lInstrumento).val(lInstrumento) );
        });

        pSender.parent().find("button").prop("disabled", false);
    }
    else
    {
        $("#cboCliente_Limites_AdicionarInstrumento")
            .children("option[value!='']")
                .hide();

        pSender.parent().find("button").prop("disabled", true);
    }
}

function lstCliente_Limites_btnExpandirContrato_Click(pSender)
{   //botão expandir / collapsear linha de contrato da tabela de limites BMF

    var lTR = $(pSender).closest("tr");

    var lContrato = lTR.attr("Contrato");

    var lSubTRs = $(pSender).closest("tbody").find("tr[ContratoPai='" + lContrato + "']");

    if (lTR.hasClass(CONST_CLASS_ITEM_EXPANDIDO))
    {
        lTR.removeClass(CONST_CLASS_ITEM_EXPANDIDO);

        lSubTRs.hide();
    }
    else
    {
        lTR.addClass(CONST_CLASS_ITEM_EXPANDIDO);

        lSubTRs.show();
    }

    return false;
}

function lstCliente_Limites_btnAdicionar_Click(pSender)
{
    GradIntra_Clientes_Limites_AdicionarContrato();

    return false;
}

function lstCliente_Limites_btnExcluir_Click(pSender)
{
    GradIntra_Clientes_Limites_ExcluirContrato( $(pSender).closest("tr") );

    return false;
}

function btnClientes_Limites_BMF_Click(pSender)
{
    GradIntra_Clientes_Limites_SalvarLimitesBMF();

    return false;
}

function btnClientes_Limites_Bovespa_NovoOMS_Click(pSender) 
{
    GradIntra_Clientes_Limites_SalvarLimitesBovespa_NovoOMS();

    return false;
}

function btnClientes_Spider_Limites_Bovespa_Click(pSender) 
{
    GradIntra_Clientes_Spider_Limites_SalvarLimitesBovespa();

    return false;
}

function btnClientes_Limites_Bovespa_Click(pSender)
{
    GradIntra_Clientes_Limites_SalvarLimitesBovespa();

    return false;
}

function btnClientes_Produtos_Click(pSender)
{
    GradIntra_Clientes_Produtos_Salvar();

    return false;
}

function btnClientes_Adesao_Termo_Fundos_Click(pSender) 
{
    GradIntra_Clientes_Produtos_Termo_Adesao_Fundos_Salvar();
    
    return false;
}

function lstCliente_Limites_txtLimite_KeyDown(pEvento, pSender)
{
    var key = pEvento.keyCode;
    var val = pEvento.target.value;

    if (pEvento.ctrlKey)
    {
        if (key == 39)   //seta pra direita
        {
            // com ctrl apertado seta pra direita joga o mesmo valor desta txtbox pras outras à direita na mesma linha:

            var lIndice = $(pSender).closest("td").index();

            var lTDs = $(pSender).closest("tr").find("td:gt(" + lIndice + ")");

            var lTextBoxes = lTDs.find("input[type='text']");

            lTextBoxes.val(  $.format.number(val)  );
        }
        else if (key == 37)   //seta pra esquerda
        {
            var lIndice = $(pSender).closest("td").index();

            var lTDs = $(pSender).closest("tr").find("td:lt(" + lIndice + ")");

            var lTextBoxes = lTDs.find("input[type='text']");

            lTextBoxes.val(  $.format.number(val)  );
        }
        else if (key == 40)   //seta pra baixo
        {
            // com ctrl apertado seta pra baixo joga o mesmo valor desta txtbox pras outras abaixo dentro do mesmo Contrato:
            var lContrato = $(pSender).closest("tr").attr("Contrato");

            if (lContrato != null && lContrato != "")
            {
                var lValorDividido = new Number(val);

                if (!isNaN(lValorDividido))
                {
                    var lIndice = $(pSender).closest("td").index();
                    var lTRs    = $(pSender).closest("tbody").find("tr[ContratoPai='" + lContrato + "']");

                    lValorDividido = (lValorDividido / lTRs.length);

                    lTRs.each(function() {   $(this).find("td:eq(" + lIndice + ") input[type='text']").val(  $.format.number(lValorDividido, "#")  );   });
                }
            }
        }
    }

    //alert(key);
    //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
    if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
        {return true;}
    else
        {if (navigator.userAgent.indexOf("MSIE") == -1)pEvento.preventDefault(); return false;}
}


function rboSeguranca_Associacoes_Usuario_Click()
{
    $("#cboSeguranca_Associacoes_Grupo").parent().hide();
    $("#cboSeguranca_Associacoes_Usuario").parent().show();
}

function rboSeguranca_Associacoes_Grupo_Click()
{
    $("#cboSeguranca_Associacoes_Usuario").parent().hide();
    $("#cboSeguranca_Associacoes_Grupo").parent().show();
}

function cboSeguranca_Associacoes_Subsistemas_Change(pSender)
{
    GradIntra_Seguranca_ListarInterfaces(pSender);
}

function btnSeguranca_Associacoes_Salvar_Click(pSender)
{
    GradIntra_Seguranca_AssossiarPermissoes(pSender);
}

function cboSeguranca_Associacoes_Interfaces_Change(pSender)
{
    GradIntra_Seguranca_Associacoes_ReceberPermissoes(pSender);
}

function rboRisco_Associacoes_Parametro_Click()
 {
     $("#txt_Risco_AssociarPermissoesParametros_Valor").prop("disabled", false);
     $("#txt_Risco_AssociarPermissoesParametros_Data").prop("disabled", false);
     $(".Editar").prop("disabled", false);
     $(".Menor1").show();
     $(".Risco_Associacoes_Atualizacao").hide();

     $("#cbo_Risco_AssociarPermissoesParametros_Parametro").show();
     $("#cbo_Risco_AssociarPermissoesParametros_Grupo").show();
     $("#lbl_Risco_AssociarPermissoesParametrosAssociarPermissoesParametros_Parametro").show();
     $("#txt_Risco_AssociarPermissoesParametros_Parametro").hide();
     $("#txt_Risco_AssociarPermissoesParametros_Grupo").hide();

    $("#pnlFormulario_Campos_AssociarPermissoesParametros_Permissao").hide();
    $("#pnlFormulario_Campos_AssociarPermissoesParametros_Parametro").show();
    if ($("#txt_Risco_AssociarPermissoesParametros_Valor").hasClass("validate[required, custom[numeroFormatado]]"))
        $("#txt_Risco_AssociarPermissoesParametros_Valor").removeClass("validate[required,custom[numeroFormatado]]")
}

function rboRisco_Associacoes_Permissoes_Click() {
    $("#pnlFormulario_Campos_AssociarPermissoesParametros_Permissao").show();
    $("#pnlFormulario_Campos_AssociarPermissoesParametros_Parametro").hide();
    if ($("#txt_Risco_AssociarPermissoesParametros_Valor").hasClass("validate[required,custom[numeroFormatado]]"))
        $("#txt_Risco_AssociarPermissoesParametros_Valor").removeClass("validate[required,custom[numeroFormatado]]")
}

function CarregarRelatorioClubesEFundos_Click()
{
    var lUrl = "Clientes/Formularios/Dados/ClubesEFundos.aspx";
    
    var lDados = { Acao        : "CarregarClubesEFundos"
                 , DataInicial : $("#txtBusca_Clubes_Fundos_DataInicial").val()
                 , DataFinal   : $("#txtBusca_Clubes_Fundos_DataFinal").val()
                 , Id: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                 , CpfCnpj: gGradIntra_Cadastro_ItemSendoEditado.CPF
                 };
    
    GradIntra_CarregarJsonVerificandoErro( lUrl
                                         , lDados
                                         , CarregarRelatorioClubesEFundos_Callback);
    return false;
}
 
function CarregarRelatorioClubesEFundos_Callback(pResposta)
{
    CarregarGridClubes(pResposta);

    CarregarGridFundos(pResposta)

    $("#pnlClientes_Formularios_Dados_ClubesEFundos table.GridIntranet")
        .show()
        .prev("h4")
            .show();

    return false;
}

function CarregarGridClubes(pResposta)
{
    var lTable = $("#tblCliente_Clubes");

    var lTemplate = lTable.find("tr.Template");

    var lTr;

    lTable.find("tbody tr.ItemDinamico").remove();

    $(pResposta.ObjetoDeRetorno.ListaClubes).each(function()
    {
        lTr = $(lTemplate.clone());
        lTr.removeClass("Template").addClass("ItemDinamico");
        
        lTr.find("td[propriedade='NomeClube']").html(this.NomeClube);
        lTr.find("td[propriedade='Cota']").html(this.Cota);
        lTr.find("td[propriedade='Quantidade']").html(this.Quantidade);
        lTr.find("td[propriedade='ValorBruto']").html(this.ValorBruto);
        lTr.find("td[propriedade='IR']").html(this.IR);
        lTr.find("td[propriedade='IOF']").html(this.IOF);
        lTr.find("td[propriedade='ValorLiquido']").html(this.ValorLiquido);

        lTr.show();

        lTable.find("tbody").append(lTr);
    });

    if (pResposta.ObjetoDeRetorno.ListaClubes.length > 0)
       lTable.find("tbody tr.Nenhuma").hide();
    else
       lTable.find("tbody tr.Nenhuma").show();
}

function CarregarGridFundos(pResposta) {

    /*
    Preenche tabela de fundos
    */
    var lTable = $("#tblCliente_Fundos");

    var lTemplate = lTable.find("tr.Template");

    var lTr;

    lTable.find("tbody tr.ItemDinamico").remove();

    $(pResposta.ObjetoDeRetorno.ListaFundos).each(function () {
        lTr = $(lTemplate.clone());
        lTr.removeClass("Template").addClass("ItemDinamico");

        lTr.find("td[propriedade='CodigoAnbima']").html(this.CodigoAnbima);
        lTr.find("td[propriedade='NomeFundo']").html(this.NomeFundo);
        lTr.find("td[propriedade='Cota']").html(this.Cota);
        lTr.find("td[propriedade='Quantidade']").html(this.Quantidade);
        lTr.find("td[propriedade='ValorBruto']").html(this.ValorBruto);
        lTr.find("td[propriedade='IR']").html(this.IR);
        lTr.find("td[propriedade='IOF']").html(this.IOF);
        lTr.find("td[propriedade='ValorLiquido']").html(this.ValorLiquido);

        lTr.show();

        lTable.find("tbody").append(lTr);
    });

    if (pResposta.ObjetoDeRetorno.ListaFundos.length > 0)
       lTable.find("tbody tr.Nenhuma").hide();
    else
        lTable.find("tbody tr.Nenhuma").show();

    /*
    preenche tabela de RendaFixa
    */
    
    lTable = $("#tbCliente_RendaFixa");

     lTemplate = lTable.find("tr.Template");

     var lTotalQuantidade = 0;
     var lTotalValorOriginal = 0;
     var lTotalSaldoBruto = 0;
     var lTotalIRRF = 0;
     var lTotalIOF = 0;
     var lTotalSaldoLiquido = 0;

     $(pResposta.ObjetoDeRetorno.ListaRendaFixa).each(function () 
     {
         lTr = $(lTemplate.clone());
         lTr.removeClass("Template").addClass("ItemDinamico");

         lTr.find("td[propriedade='Titulo']"       ).html(this.Titulo);
         lTr.find("td[propriedade='Aplicacao']"    ).html(this.Aplicacao);
         lTr.find("td[propriedade='Vencimento']"   ).html(this.Vencimento);
         lTr.find("td[propriedade='Taxa']"         ).html(this.Taxa);
         lTr.find("td[propriedade='Quantidade']"   ).html(this.Quantidade);
         lTr.find("td[propriedade='ValorOriginal']").html(this.ValorOriginal);
         lTr.find("td[propriedade='SaldoBruto']"   ).html(this.SaldoBruto);
         lTr.find("td[propriedade='IRRF']"         ).html(this.IRRF);
         lTr.find("td[propriedade='IOF']"          ).html(this.IOF);
         lTr.find("td[propriedade='SaldoLiquido']" ).html(this.SaldoLiquido);

         lTr.show();

         lTable.find("tbody").append(lTr);
     });
     

     if (pResposta.ObjetoDeRetorno.ListaRendaFixa.length > 0) 
     {
         lTable.find("tbody tr.Nenhuma").hide();

         var lTotalQuantidade = 0;
         var lTotalValorOriginal = 0;
         var lTotalSaldoBruto = 0;
         var lTotalIRRF = 0;
         var lTotalIOF = 0;
         var lTotalSaldoLiquido = 0;



         /*

         var lLinha = "<tr>" +
                        "<td colspan=\"4\"></td>" +
                        "<td>" + lTotalQuantidade + "</td>" +
                        "<td>" + lTotalQuantidade + "</td>" +
                        "<td>" + lTotalQuantidade + "</td>" +
                        "<td>" + lTotalQuantidade + "</td>" +
                        "<td>" + lTotalQuantidade + "</td>" +
                        "<td>" + lTotalQuantidade + "</td>" +
                        "<td>" 
                      "</tr>";

                      lTable.find("tbody").append(lTr);
         */
     }
     else
         lTable.find("tbody tr.Nenhuma").show();

     lTable.find("tr.Template").remove();
}

function lnkMenuPersonalizado_Click(pSender, pDistancia)
{
    pSender = $(pSender);

    var lMenuDoObjeto = pSender.closest("ul.MenuDoObjeto");

    if (pSender.parent().hasClass("Topo"))
    {   //clicou no botao de cima, desce o menu

        lMenuDoObjeto.find("ul.MenuRolavel").animate( { marginTop: 0 }, 500 );

        lMenuDoObjeto.find("li.Topo  a").hide();
        lMenuDoObjeto.find("li.Fundo a").show();
    }
    else
    {
        //clicou no botao de baixo, sobe o menu

        lMenuDoObjeto.find("ul.MenuRolavel").animate( { marginTop: pDistancia }, 500 );

        lMenuDoObjeto.find("li.Topo  a").show();
        lMenuDoObjeto.find("li.Fundo a").hide();
    }

    return false;
}
/*-------------------------------------------------------------------------------------------------------*/
/*---------   Código para o ticker de cotação rápida ----------------------------------------------------*/
/*-------------------------------------------------------------------------------------------------------*/

//variáveis globais
var gTimeoutApagarTDsRapida = null;

var gGradIntra_Ticker_CotacaoRapida =null;

var CONST_ESTADO_PAPEL = [ "Papel Não Negociado", "Papel Em Leilão", "Papel Em Negociação", "Papel Suspenso", "Papel Congelado", "Papel Inibido" ];

function CotacaoRapida_AssinarPapel(pBotao)
{
///<summary>Registra uma assinatura de Papel para Cotação Rápida na janela onde está o pBotao</summary>
///<param name="pBotao" type="Objeto HTML">Botão que chamou a função, da janela que registrará a assinatura</param>
///<returns>false (para não dar post)</returns>

    var lTxtPapel = $(pBotao).parent().find("input.txtPapel");
    //var lJanelaAtual = $(pBotao).closest("div.GradWindow").attr("id");

    var lPapel = lTxtPapel.val().toUpperCase();

    if (lPapel == "")
    {
        alert("É necessário inserir um Ativo.");
        return false;
    }

    $("#pnlCotacaoRapidaHeader span").html("Cotação Rápida - " + lPapel);

    var lDados = 
    {
        Acao : "BuscarPapelCotacaoRapida",
        Papel: lPapel
    };

    if ( gGradIntra_Ticker_CotacaoRapida != null )
    {
        window.clearInterval(gGradIntra_Ticker_CotacaoRapida);
        gGradIntra_Ticker_CotacaoRapida = null;
    }

        //Centralizador_AssinarPapelParaCotacaoRapida(lJanelaAtual , lPapel);

        //GradWindow_AnexarTextAoTitulo(lJanelaAtual, lPapel);

    gGradIntra_Ticker_CotacaoRapida = window.setInterval(
        function (){
          GradIntra_CarregarJsonVerificandoErro(
                "Default.aspx",
                lDados,
                function (pResposta) {  CotacaoRapida_ChegadaDeDados_CallBack("pnlCotacaoRapida", pResposta.ObjetoDeRetorno);}
            );
        }, 1000);

    return false;

}

/*Chamada de Callback para a chegada de dados para cotação*/
function CotacaoRapida_ChegadaDeDados_CallBack(pIdDaJanela, pDados)
{
///<summary>Callback do centralizador quando chega informação de cotação rápida para o papel que a janela assinou</summary>
///<param name="pIdDaJanela" type="String">ID da janela que assinou o papel</param>
///<param name="pDados"      type="Objeto Javascript">Objeto de Cotação Rápida que tem as informações de cotação</param>
///<returns>void</returns>

    var lTBody = $("#" + pIdDaJanela + " table.CotacaoRapida td[Propriedade]");
    var lTD = null;

    var lValorNumericoAnterior, lValorNumericoModificado;

    var lPropertyName, lFormatString;

    var lHtmlAnterior,lPropertyValue;

    var lIconeEstado = $("#" + pIdDaJanela + " a.IconeEstadoPapel");

    if (pDados.EstadoDoPapel && pDados.EstadoDoPapel != "")
    {
        lIconeEstado
            .attr("title", CONST_ESTADO_PAPEL[pDados.EstadoDoPapel])
            .attr("class", "IconeEstadoPapel EstadoPapel_" + pDados.EstadoDoPapel)
            .show();
    }
    else
    {
        lIconeEstado.hide();
    }

    lTBody.each(function()
    {
        lTD = $(this);

        lTD.removeClass("CotacaoAtualizada").removeClass("CotacaoAtualizada_Subiu").removeClass("CotacaoAtualizada_Desceu");

        lPropertyName  = lTD.attr("Propriedade");

        lFormatString = lTD.attr("Format");

        lPropertyValue = eval("pDados." + lTD.attr("Propriedade"));

        lHtmlAnterior  = lTD.html();

         if (lPropertyValue && lHtmlAnterior != lPropertyValue && lPropertyValue != "")
         {

                if (lPropertyName == "VolumeAcumulado" && lPropertyValue != "&nbsp;")
                {
                    lPropertyValue = abbrNum((lPropertyValue.replace(',','.') * 1), 2);
                    lPropertyValue = lPropertyValue.replace('.',',');
                }

                if ((lPropertyName  == "Preco"
                ||   lPropertyName  == "MaxDia"
                ||   lPropertyName  == "MinDia" )
                &&   lPropertyValue != "&nbsp;")
                {
                    lPropertyValue = $.format.number((lPropertyValue.replace(',','.') * 1), lFormatString);
                }

                lTD.html(  lPropertyValue  );

                if (!lTD.hasClass("NaoExibirAtualizacao"))
                {
                    lTD.addClass("CotacaoAtualizada");

                    if (!lTD.hasClass("AtualizacaoSemSetas"))
                    {
                        try
                        {
                            //lValorNumericoAnterior =   new Number(   lHtmlAnterior.replace(/./, "").replace(",", ".")  );
                            //lValorNumericoModificado = new Number(  lPropertyValue.replace(/./, "").replace(",", ".")  );

                             lValorNumericoAnterior   = NumConv.StrToNum(lHtmlAnterior); 
                             lValorNumericoModificado = NumConv.StrToNum(lPropertyValue);

                            if (!isNaN(lValorNumericoAnterior) && !isNaN(lValorNumericoModificado))
                            {
                                if (lValorNumericoAnterior < lValorNumericoModificado)
                                {
                                    lTD.addClass("CotacaoAtualizada_Subiu");
                                }
                                else if (lValorNumericoAnterior > lValorNumericoModificado)
                                {
                                    lTD.addClass("CotacaoAtualizada_Desceu");
                                }
                                else
                                {
                                    lTD.addClass("CotacaoAtualizada_Manteve");
                                }
                            }

                        }
                        catch(err1){}
                    }

                    if (gTimeoutApagarTDsRapida == null)
                        gTimeoutApagarTDsRapida = window.setTimeout( function () {ApagarTDsRapidas(pIdDaJanela);}, 500 );
                }
         }
    });
}

/*Método para formato do volume 1K, 1M 1B, 1T*/
function abbrNum(number, decPlaces)
{
    decPlaces = Math.pow(10,decPlaces);

    var abbrev = [ "K", "M", "B", "T" ];

    for (var i=abbrev.length-1; i>=0; i--)
    {
        var size = Math.pow(10,(i+1)*3);

        if (size <= number)
        {
             number = Math.round(number*decPlaces/size)/decPlaces;

             number += abbrev[i];

             break;
        }
    }

    return number;
}

/*Tirar a css das tabela de cotação rápida*/
function ApagarTDsRapidas(pIdJanela)
{
///<summary>"Apaga" as TDs que foram atualizadas</summary>
///<param name="pIdJanela" type="String">ID da janela onde estão as TDs</param>
///<returns>void</returns>

    $("#" + pIdJanela + " table.CotacaoRapida td[Propriedade]")
        .removeClass("CotacaoAtualizada")
        .removeClass("CotacaoAtualizada_Desceu")
        .removeClass("CotacaoAtualizada_Subiu");

    gTimeoutApagarTDsRapida = null;
}

function GradIntra_ContacaoRapida_Fechar()
{
    if ( gGradIntra_Ticker_CotacaoRapida != null )
    {
        window.clearInterval(gGradIntra_Ticker_CotacaoRapida);

        gGradIntra_Ticker_CotacaoRapida = null;

        ApagarTDsRapidas("pnlCotacaoRapida");

        var lObjetoInicial = new Object();

        lObjetoInicial.CodNegocio                  = "";
        lObjetoInicial.Data                        = 
        lObjetoInicial.Hora                        = 
        lObjetoInicial.DataNeg                     = 
        lObjetoInicial.HoraNeg                     = 
        lObjetoInicial.CorretoraCompradora         = 
        lObjetoInicial.CorretoraVendedora          = 
        lObjetoInicial.Preco                       = 
        lObjetoInicial.Quantidade                  = 
        lObjetoInicial.MaxDia                      = 
        lObjetoInicial.MinDia                      = 
        lObjetoInicial.VolumeAcumulado             = 
        lObjetoInicial.NumNegocio                  = 
        lObjetoInicial.IndicadorVariacao           = 
        lObjetoInicial.Variacao                    = 
        lObjetoInicial.ValorAbertura               = 
        lObjetoInicial.ValorFechamento             = 
        lObjetoInicial.MelhorPrecoCompra           =
        lObjetoInicial.MelhorPrecoVenda            = 
        lObjetoInicial.MelhorOfertaCompra          = 
        lObjetoInicial.MelhorOfertaVenda           = 
        lObjetoInicial.QuantidadeMelhorPrecoCompra = 
        lObjetoInicial.QuantidadeMelhorPrecoVenda  = "&nbsp;";

        CotacaoRapida_ChegadaDeDados_CallBack("pnlCotacaoRapida", lObjetoInicial);
    }

    $("#pnlCotacaoRapida").hide();

    return false;
}

function GradIntra_AbrePop_CotacaoRapida()
{
    $("#pnlCotacaoRapida").show();

    return false;
}

function pnlListaDeMensagens_li_btn_Click(pSender)
{
    $(pSender).parent().toggleClass(CONST_CLASS_ITEM_EXPANDIDO);

    return false;
}

function cboRelatorios_TipoDeRelatorioRisco_Change(pSender)
{
    pSender = $(pSender);
    
    var lDivFormulario = pSender.closest(".Busca_Formulario");
    var lValor = pSender.val();

    $(".pnlRelatorio").hide();

    GradIntra_Relatorio_Risco_OcultarExibirFiltros(lDivFormulario, lValor);
        
    $('#btnClienteRelatorioImprimir').prop("disabled", true);
}

function btnRisco_Relatorios_FiltrarRelatorio_Click(pSender, pOrdernarPor)
{
    var lRelatorio = $("#cboRisco_FiltroRelatorioRisco_Relatorio").val();

    if (lRelatorio == "R010") 
    {
        var lPapel = $("#txbRisco_FiltrarPorPapel").val();

        if (lPapel == "") 
        {
            alert("É necessário inserir um papel");

            return false;
        }
    }

    $(".pnlRelatorio").show();

    if (lRelatorio != "")
    {
        var lUrl = "Risco/Relatorios/" + lRelatorio + ".aspx";
    
        GradIntra_Busca_BuscarItensParaListagemSimples( $("#pnlRisco_FiltroRelatorio")
                                                      , $("#pnlRisco_Relatorios_Resultados")
                                                      , lUrl
                                                      , null
                                                      , pOrdernarPor
                                                      );
                                                
//        $(".RiscoPosicaoCCOrdernarPor").removeClass("RiscoPosicaoCCOrdernarPorSelecionado");

//        
//        if (pOrdernarPor != undefined)
//        {
//            $(pSender).addClass("RiscoPosicaoCCOrdernarPorSelecionado");
//        }
    }
    else
    {   //Não merece aparecer, porque o botão se oculta quando não tem um selecionado; #JIC.
        GradIntra_ExibirMensagem("A", "Favor selecionar um relatório");
    }

    return false;
}

function btnHomeBroker_Avisos_Editar_Click(pSender)
{
    GradIntra_HomeBroker_EditarAviso(pSender);
    
    return false;
}

function btnHomeBroker_Avisos_IncluirEditar_Click(pSender)
{
    GradIntra_HomeBroker_IncluirEditarAviso(pSender);

    return false;
}

function Risco_Relatorios_ConfiguraCamposObrigatorios(pRelatorio)
{
    var lArrReturn ;
//    switch(pRelatorio)
//    {
//        case "R002":
//            lArrReturn = [ "txtClientes_FiltroRelatorio_DataInicial"  
//                         , "txtClientes_FiltroRelatorio_DataFinal"  
//                         , "txtClientes_FiltroRelatorio_Assessor"  
//                         ];    
//        break;
//    }

    return lArrReturn;
}

var gBoolRisco_Relatorios_ExibirMensagemConfigurarImpressao = false;

function btnRisco_Relatorios_ExibirMensagemConfigurarImpressao_Click(pSender)
{
    pSender = $(pSender);

    if (gBoolRisco_Relatorios_ExibirMensagemConfigurarImpressao)
        pSender.parent()
               .parent()
               .find(".PopUpDeAjuda")
               .hide();
    else
        pSender.parent()
               .parent()
               .find(".PopUpDeAjuda")
               .show();

   gBoolRisco_Relatorios_ExibirMensagemConfigurarImpressao = !gBoolRisco_Relatorios_ExibirMensagemConfigurarImpressao;

}

function onCloseEfetuarLogout() 
{
    var lConfirmado = gConfirmacaoDeSaidaDesnecessaria;

    if (!lConfirmado)
    {
        lConfirmado = confirm("Tem certeza que deseja sair desta página?");
    }

    if (lConfirmado) 
    {
        Efetuar_LogOut();

        window.onbeforeunload = null;
        
        window.close();

        return true;
    }
    else 
    {
        return false;
    }
}

function Efetuar_LogOut() 
{
    if (keyCode == 116)
        return false;

    var lUrl = "../Login.aspx";

    var lDados = { Acao: "Logout" };

    GradIntra_CarregarJsonVerificandoErro( lUrl
                                         , lDados
                                         , onCloseEfetuarLogout_Callback);
}

function onCloseEfetuarLogout_Callback(pResposta) 
{
    return false;
}

function onBeforUnload_Logout(evt) 
{
    if (keyCode == 116)
    {
        return false;
    }
    else
    {
        return true;
    }
}

function onMouseMovePosition(e) 
{
    var ev = (!e) ? window.event : e; //IE:Moz

    if (ev.pageX) 
    {
        gPosX = ev.pageX;
        gPosY = ev.pageY;
    }
    else 
    {
        //alert(e);
        gPosX = ev.clientX;
        gPosY = ev.clientY;
    }
}

function whatKey(evt) 
{
     evt = (evt) ? evt : event;
     keyVals = document.getElementById('ffKeyTrap');
     altKey  = evt.altKey;
     keyCode = evt.keyCode;

     if (altKey && keyCode == 115)
         keyVals.value = String(altKey) + String(keyCode);
}

function btnConta_AtualizarSaldoEmConta_Click(pSender)
{

    $("#pnlClientes_Formularios_Dados_ContaCorrente").removeClass("ConteudoCarregado");
    $("#lstMenuCliente ul.MenuRolavel li.Selecionada a").click();

    return false;
}

function btnCustodia_AtualizarCustodia_Click(pSender)
{
    $("#pnlClientes_Formularios_Dados_Custodia").removeClass("ConteudoCarregado");
    $("#lstMenuCliente ul.MenuRolavel li.Selecionada a").click();

    return false;
}

function cboSeguranca_DadosCompletos_Usuario_TipoAcesso_Change(pSender)
{
    lSender = $(pSender);
    if (lSender.val() == "2")
    {
        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessor").prop("disabled", false);
        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessor").addClass("validate[required,custom[onlyNumber],length[0,5]] ProibirLetras");

        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado").prop("disabled", false);
        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado").addClass("validate[custom[number]] ProibirLetras");
    }
    else
    {
        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessor").prop("disabled", true);
        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessor").removeClass("validate[required,custom[onlyNumber],length[0,5]] ProibirLetras");

        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado").prop("disabled", true);
        $("#txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado").removeClass("validate[custom[number]] ProibirLetras");
    }

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Relatorios_Imprimir_Click(pSender) 
{
    var newWindow;

    newWindow = window.open("RelImpressao.aspx");

    newWindow.onload = function () 
    {
        newWindow.document.write(document.getElementById("RelatorioExtratoConta").innerHTML);
        newWindow.focus();
        newWindow.print();
    };

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(pSender)
{
    return CarregarRelatorioPosicaoCliente(true);

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodos_Click(pSender)
{
    return GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodos(pSender);

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodosBmf_Click(pSender) 
{
    return GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodosBmf(pSender);

    return false;
}

var gBoolMonitoramento_Relatorios_ExibirMensagemConfigurarImpressao = false;

function btnMonitoramento_Relatorios_ExibirMensagemConfigurarImpressao_Click(pSender)
{
    pSender = $(pSender);

    if (gBoolMonitoramento_Relatorios_ExibirMensagemConfigurarImpressao)
        pSender.parent()
               .parent()
               .find(".PopUpDeAjuda")
               .hide();
    else
        pSender.parent()
               .parent()
               .find(".PopUpDeAjuda")
               .show();

   gBoolMonitoramento_Relatorios_ExibirMensagemConfigurarImpressao = !gBoolMonitoramento_Relatorios_ExibirMensagemConfigurarImpressao;

}

function GradIntra_Risco_Restricoes_ItensDoGrupo_SelecionarGrupoSpider_Click(pSender) {
    //GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem(pSender);

    GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricaoSpider(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_SelecionarGrupo_Click(pSender) 
{
    //GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem(pSender);

    GradIntra_Risco_ItensDoGrupo_SelecionarGrupoRestricao(pSender);

    return false;
}

//function GradIntra_Risco_Restricoes_Regras_SelecionarGrupo_Click(pSender) 
//{
//    GradIntra_Risco_Restricoes_Regras_SelecionarGrupo(pSender);
//    
//    return false;
//}

function GradIntra_Risco_Restricoes_SelecionarGrupo_Spider_Click(pSender) 
{
    GradIntra_Risco_Restricoes_Regras_SelecionarGrupoSpider(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_SelecionarGrupo_Click(pSender) 
{
    GradIntra_Risco_Restricoes_Regras_SelecionarGrupo(pSender);

    return false;
}

function GradIntra_Risco_ItensDoGrupo_SelecionarGrupo_Click(pSender)
{
    GradIntra_Risco_ItensDoGrupo_SelecionarGrupo(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_Salvar_Dados_Click(pSender)
{
    GradIntra_Risco_Restricoes_Salvar_Dados(pSender);

    return false;
}

function GradIntra_Risco_ItemDeGrupo_Gravar_Click(pSender)
{
    GradIntra_Risco_ItemDeGrupo_Gravar(pSender);

    return false;
}

function GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Spider_Click(pSender) 
{
    GradIntra_Risco_ItemDeGrupoRestricao_Gravar_Spider(pSender);

    return false;
}

function GradIntra_Risco_ItemDeGrupoRestricoes_Gravar_Click(pSender) 
{
    GradIntra_Risco_ItemDeGrupoRestricao_Gravar(pSender);

    return false;
}

function GradIntra_Risco_ItensDoGrupoRestricoes_Excluir_Click(pSender) 
{
    GradIntra_Risco_ItensDoGrupoRestricoes_Excluir(pSender);

    return false;
}

function GradIntra_Risco_ItensDoGrupo_Excluir_Click(pSender)
{
    GradIntra_Risco_ItensDoGrupo_Excluir(pSender);

    return false;
}

function GradIntra_Risco_ItensDoGrupo_Excluir_Spider_Click(pSender) {
    GradIntra_Risco_ItensDoGrupo_Excluir_Spider(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoCliente_Excluir_Spider_Click(pSender) 
{
    GradIntra_Risco_Restricoes_ItensDoCliente_Spider_Excluir(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoCliente_Excluir_Click(pSender) 
{
    GradIntra_Risco_Restricoes_ItensDoCliente_Excluir(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Excluir_Spider_Click(pSender) 
{
    GradIntra_Risco_Restricoes_ItensDoGrupo_Spider_Excluir(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Excluir_Click(pSender) 
{
    GradIntra_Risco_Restricoes_ItensDoGrupo_Excluir(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Excluir_Spider_Click(pSender) 
{
    GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Spider_Excluir(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Excluir_Click(pSender) 
{
    GradIntra_Risco_Restricoes_ItensDoGrupo_Global_Excluir(pSender);

    return false;
}

function GradIntra_Risco_ItemDeGrupo_AdicionarItem_Click(pSender)
{
    GradIntra_Risco_ItemDeGrupo_AdicionarItem(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItemSpider_Click(pSender) 
{
    GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItemSpider(pSender, gGradIntra_Risco_Spider_Restricoes_UL_Selecionada);

    return false;
}

function GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItem_Click(pSender) 
{
    GradIntra_Risco_Restricoes_RegraDeGrupo_AdicionarItem(pSender, gGradIntra_Risco_Restricoes_UL_Selecionada);

    return false;
}

function GradIntra_Risco_Restricoes_ItemDeGrupo_AdicionarItem_Spider_Click(pSender) {
    //GradIntra_Risco_Restricoes_ItemDeGrupo_AdicionarItem(pSender);

    GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem_Spider(pSender);

    return false;
}

function GradIntra_Risco_Restricoes_ItemDeGrupo_AdicionarItem_Click(pSender) 
{
    //GradIntra_Risco_Restricoes_ItemDeGrupo_AdicionarItem(pSender);

    GradIntra_Risco_Restricoes_ItemDeGrupoRestricoes_AdicionarItem(pSender);

    return false;
}

function btnGradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo_Click(pSender)
{
     GradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo(pSender);

    return false;
}

function GradIntra_Risco_ParametroDeAlavancagem_SalvarParametros_Click(pSender)
{
    GradIntra_Risco_ParametroDeAlavancagem_SalvarParametros(pSender);

    return false;
}

function btnSalvar_Clientes_DadosCompletos_PF_Click(pSender)
{
    Salvar_Clientes_DadosCompletos_PF(pSender);

    return false;
}

function btnSalvar_Clientes_DadosCompletos_PJ_Click(pSender)
{
    Salvar_Clientes_DadosCompletos_PJ(pSender);

    return false;
}

function GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(pSender)
{
    GradIntra_Clientes_ImpostoDeRenda_GerarComprovante(pSender);

    return false;
}

function btnGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue_Salvar_Click(pSender)
{
    GradIntra_Cadastro_SalvarItemFilho( "DocumentacaoEntregue"
                                      , $(pSender).closest("div.pnlFormulario_Campos")
                                      , "Clientes/Formularios/Acoes/DocumentacaoEntregue.aspx");
    return false;
}

function cboDBM_FiltroRelatorio_Relatorio_Change(pSender)
{
    pSender = $(pSender);
    
    var lDivFormulario = pSender.closest(".Busca_Formulario");
    var lValor = pSender.val();

    $(".pnlRelatorio").hide();

    GradIntra_RelatorioDBM_OcultarExibirFiltros(lDivFormulario, lValor);
        
    $('#btnClienteRelatorioImprimir').prop("disabled", true);
}

function btnDBM_Relatorios_FiltrarRelatorio_Click(pSender)
{
    var lRelatorio = $("#cboDBM_FiltroRelatorio_Relatorio").val();

    $(".pnlRelatorio").show();

    if (lRelatorio != "")
    {
        var lUrl = "DBM/Relatorios/" + lRelatorio + ".aspx";

        GradIntra_Relatorios_IniciarRecebimentoProgressivo( $("#pnlDBM_FiltroRelatorio_FF1")
                                                          , $("#pnlRelatorios_Resultados")
                                                          , lUrl
                                                          , lRelatorio);
    }
    else
    {   //--> não deve aparecer, porque o botão se oculta quando não tem um selecionado; #JIC.
        GradIntra_ExibirMensagem("A", "Favor selecionar um relatório");
    }

    return false;
}

function txtAreaLabel_Click(pSender)
{
    pSender = $(pSender);

    if( !pSender.hasClass("Expandido"))
    {
        pSender.closest("tbody").find("textarea.Expandido").removeClass("Expandido");

        pSender.addClass("Expandido");
    }
    else
    {
        pSender.removeClass("Expandido");
    }
}



function ManterSessao()
{
    var lDados = { Acao: "ManterSessaoDoUsuario" };

    GradIntra_CarregarHtmlVerificandoErro( "../Intranet/Default.aspx"
                                         , lDados
                                         , null
                                         , function(pResposta) { ManterSessao_CallBack(pResposta); });
   return false;
}

function ManterSessao_CallBack(pResposta)
{
    setTimeout("ManterSessao()", 45000);
}


function btnAlterarVenda_Salvar_Click(pSender)
{
    GradIntra_Solicitacoes_SalvarAlteracoes();

    return false;
}

function cboClientes_Telefones_Tipo_Change(pSender) 
{
    pSender = $(pSender);

    var lTelefone = $("#txtClientes_Telefones_Numero");

    //if (pSender.val() == '11' && $("#txtClientes_Telefones_DDD").val() == '3') 
    if ($("#txtClientes_Telefones_DDD").val() == '3') 
    {
        lTelefone.removeClass();

        //lTelefone.addClass("MesmaLinha Mascara_CEL_SP validate[required,custom[celular_sp]]");

        lTelefone.addClass("MesmaLinha");

        lTelefone.unbind("keydown", Validacao_SomenteNumeros_OnKeyDown).unmask("99999-9999");

        //Validacao_HabilitarMascaraNumerica(lTelefone, "99999-9999");
    }
    else 
    {
        lTelefone.removeClass();

        lTelefone.addClass("MesmaLinha Mascara_TEL validate[required,custom[telefone]]");

        lTelefone.unbind("keydown", Validacao_SomenteNumeros_OnKeyDown).unmask("9999-9999");

        Validacao_HabilitarMascaraNumerica(lTelefone, "9999-9999");
    }
}
function txtClientes_Telefones_Numero_OnFocus(pSender) 
{
    var lTelefone = $("#txtClientes_Telefones_Numero");

    pSender = $(pSender);

    //if (pSender.val() == "11" && $("#cboClientes_Telefones_Tipo").val() == "3") 
    if ($("#cboClientes_Telefones_Tipo").val() == "3") {
        lTelefone.removeClass();

        //lTelefone.addClass("MesmaLinha Mascara_CEL_SP validate[required,custom[celular_sp]]");

        lTelefone.addClass("MesmaLinha");

        lTelefone.unbind("keydown", Validacao_SomenteNumeros_OnKeyDown).unmask("99999-9999");

        //Validacao_HabilitarMascaraNumerica(lTelefone, "99999-9999");
    }
    else {
        lTelefone.removeClass();

        lTelefone.addClass("MesmaLinha Mascara_TEL validate[required,custom[telefone]]");

        lTelefone.unbind("keydown", Validacao_SomenteNumeros_OnKeyDown).unmask("9999-9999");

        Validacao_HabilitarMascaraNumerica(lTelefone, "9999-9999");
    }
}

function txtClientes_Telefones_DDD_OnBlur(pSender) 
{
    var lTelefone = $("#txtClientes_Telefones_Numero");

    pSender = $(pSender);

    //if (pSender.val() == "11" && $("#cboClientes_Telefones_Tipo").val() == "3") 
    if ($("#cboClientes_Telefones_Tipo").val() == "3") 
    {
        lTelefone.removeClass();

        //lTelefone.addClass("MesmaLinha Mascara_CEL_SP validate[required,custom[celular_sp]]");

        lTelefone.addClass("MesmaLinha");

        lTelefone.unbind("keydown", Validacao_SomenteNumeros_OnKeyDown).unmask("99999-9999");

        //Validacao_HabilitarMascaraNumerica(lTelefone, "99999-9999");
    }
    else 
    {
        lTelefone.removeClass();

        lTelefone.addClass("MesmaLinha Mascara_TEL validate[required,custom[telefone]]");

        lTelefone.unbind("keydown", Validacao_SomenteNumeros_OnKeyDown).unmask("9999-9999");

        Validacao_HabilitarMascaraNumerica(lTelefone, "9999-9999");
    }
}

function rdoCadastro_PFPasso3_USPerson_Click(pSender)
{
    pSender = $(pSender);

    var lNome = pSender.prop("name");
    var  lDiv = $("#" + $(pSender[0]).attr("data-PainelDoSim"));

    if($("[name='" + lNome + "']:checked").prop("id").indexOf("_Sim") != -1)
    {
        lDiv.show();
    }
    else
    {
        lDiv.hide();
    }
}

function GradIntra_Clientes_Financeiro_FecharRelatorio_Default(pSender) 
{
    gRelatorioCarregando = false;
}


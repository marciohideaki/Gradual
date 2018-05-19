/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="../../Lib/Etc/Dev/waitingDialog.js" />

var gGradIntra_Clientes_TipoDeClienteAtual = "PF";

var gGradIntra_Clientes_Posicao_ImpostoDeRenda_Acao;

function GradIntra_Clientes_AoSelecionarSistema()
{
    if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_LINK_PROSPECT)
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca=false;
        
        $("#pnlConteudo_Clientes_LinkProspect")
            .show()
            .children(".pnlFormulario")
                .show()
                .find("#pnlClientes_LinkProspect")
                    .show();

        GradIntra_Navegacao_CarregarFormulario("Clientes/Formularios/LinkParaProspect.aspx", "pnlClientes_LinkProspect",{Acao:'CarregarHtmlComDados'}, null);
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_INTEGRCAO_ROCKET)
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca  =   false;
        
        $("#pnlConteudo_Clientes_IntegracaoRocket")
            .show()
            .children(".pnlFormulario")
                .show()
                .find("#pnlClientes_IntegracaoRocket")
                    .show();
        

        GradIntra_Navegacao_CarregarFormulario("Clientes/Formularios/Acoes/IntegracaoRocket/Sumario.aspx", "pnlClientes_IntegracaoRocket",{Acao:'CarregarHtmlComDados'}, null);
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_NOVO_PF)
    {
        //--> Corrige o problema do radiobutoon não receber o check na inclusão de cliente.
        $("#pnlClientes_Formularios_Dados_DadosCompletos").find("form").remove();
    }
}

function lnkNovoClienteCB_Click(pSender)
{
    window.open(GradIntra_RaizDoSite() + "/Extras/Contratos/Fichas Gradual PF Cambio.zip", "_blank");

    return false;
}

function GradIntra_Clientes_InstanciarParametrosDeBusca()
{
    var lDados = new Object();
    
    lDados.TermoDeBusca                      = $("#txtBusca_Clientes_Termo").val();
    lDados.BuscarPor                         = $("#cboBusca_Clientes_BuscarPor").val();
    lDados.Tipo                              = $("#cboBusca_Clientes_Tipo").val();

    lDados.Status_Ativo                      = $("#chkBusca_Clientes_Status_Ativo").prop("checked");
    lDados.Status_Inativo                    = $("#chkBusca_Clientes_Status_Inativo").prop("checked");

    lDados.Passo_Visitante                   = $("#chkBusca_Clientes_Passo_Visitante").prop("checked");
    lDados.Passo_Cadastrado                  = $("#chkBusca_Clientes_Passo_Cadastrado").prop("checked");
    lDados.Passo_ExportadoSinacor            = $("#chkBusca_Clientes_Passo_ExportadoSinacor").prop("checked");

    lDados.Pendencia_ComPendenciaCadastral   = $("#chkBusca_Clientes_Pendencia_ComPendenciaCadastral").prop("checked");
    lDados.Pendencia_ComSolicitacaoAlteracao = $("#chkBusca_Clientes_Pendencia_ComSolicitacaoAlteracao").prop("checked");
    
    return lDados;
}

function GradIntra_Clientes_ReverterTipoDeClienteAtual(pPessoaFisicaOuJuridica, pRecarregarConteudo, pObjetoClienteSelecionado)
{
///<summary>Função geral centralizadora para tratar o cliente sendo editado como PF ou PJ. Ao invés de setar a flag direto, usar essa função.</summary>
///<param name="pTipoFisicaOuJuridica" type="String">Tipo de cliente: 'PF' ou 'PJ'.</param>
///<param name="pRecarregarConteudo" type="Boolean">Flag se deve marcar o formulário de dados básicos para recarregar o conteúdo.</param>
///<returns>void</returns>

    $("#lstMenuCliente").find(".Tipo_Condicional_Bloquear_Passo3").show();

    gGradIntra_Clientes_TipoDeClienteAtual = pPessoaFisicaOuJuridica;

    if(pRecarregarConteudo)
        $("#pnlClientes_Formularios_Dados_DadosCompletos").removeClass(CONST_CLASS_CONTEUDOCARREGADO);    //mudou o tipo, tem que mudar o conteudo

    if(gGradIntra_Clientes_TipoDeClienteAtual == "PF")
    {
        $("#lstMenuCliente li.Tipo_J").hide();
        $("#lstMenuCliente li.Tipo_F").show();
    }
    else
    {
        $("#lstMenuCliente li.Tipo_F").hide();
        $("#lstMenuCliente li.Tipo_J").show();
    }
    
    if(gGradIntra_Cadastro_ItemSendoEditado != null)
    {
        if(gGradIntra_Cadastro_ItemSendoEditado.CodBovespa == null || gGradIntra_Cadastro_ItemSendoEditado.CodBovespa == "" || gGradIntra_Cadastro_ItemSendoEditado.CodBovespa == "n/d")
        {
            $("#lstMenuCliente li.Tipo_SomenteComCodBovespa").hide();
        }
        else
        {
            $("#lstMenuCliente li.Tipo_SomenteComCodBovespa").show();
        }
    }

    //--> Valida os itens do menu que devem ser exibidos para PF e PJ em relação ao passo cadastral.
    if (pObjetoClienteSelecionado && pObjetoClienteSelecionado.Passo)
    {
        if (pObjetoClienteSelecionado.Passo < 4)
        {
            $("#lstMenuCliente").find(".Tipo_Condicional_Passo").hide();
        }
        else
        {
            $("#lstMenuCliente").find(".Tipo_Condicional_Passo").show();
        }

        var lPermissaoEditarPasso3e4 = $("#hdMenuClientesPermissaoAlterarPasso_3_4").val();
        if (pObjetoClienteSelecionado.Passo > 2 && lPermissaoEditarPasso3e4 == "False")
        {
            $("#lstMenuCliente").find(".Tipo_Condicional_Bloquear_Passo3").hide();
        }
    }    
}

function GradIntra_Clientes_SalvarDadosCompletosDoCliente()
{
    var lDiv;
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";
    var lUrl = "Clientes/Formularios/Dados/DadosCompletos_Grupo.aspx";
//  var lUrl = "Clientes/Formularios/Dados/DadosCompletos_" + gGradIntra_Clientes_TipoDeClienteAtual + ".aspx";

    if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
    {
        lDiv = $("#pnlNovoItem_Formulario");
    }
    else
    {
        lDiv = $("#pnlClientes_Formularios_Dados_DadosCompletos");
    }

    GradIntra_Cadastro_SalvarFormulario(lDiv, lUrl, lAcao);

    return false;
}

function GradIntra_Clientes_ItemIncluidoComSucesso(pItemIncluido)
{
    gGradIntra_Navegacao_SubSistemaAtual = CONST_SUBSISTEMA_BUSCA;
    
    gGradIntra_Navegacao_PainelDeBuscaAtual    = $("#pnlBusca_Clientes_Busca");

    gGradIntra_Navegacao_PainelDeConteudoAtual = $("#pnlConteudo_Clientes_Busca");

    var lNovaLI = GradIntra_Navegacao_AdicionarItemNaListaDeItensSelecionados(pItemIncluido, CONST_SISTEMA_CLIENTES, true);

    //se já tinha algum selecionado, deseleciona:
    $("#lstClientesSelecionados li").removeClass(CONST_CLASS_ITEM_EXPANDIDO);

    //se haviam painéis carregados de outras abas, remove:

    gGradIntra_Navegacao_PainelDeConteudoAtual
        .find("div." + CONST_CLASS_CONTEUDOCARREGADO)
            .removeClass(CONST_CLASS_CONTEUDOCARREGADO);

    lNovaLI.addClass(CONST_CLASS_ITEM_EXPANDIDO);

    $("ul.Sistema_Clientes li." + CONST_CLASS_ITEM_SELECIONADO).removeClass(CONST_CLASS_ITEM_SELECIONADO);

    $("ul.Sistema_Clientes li a[rel='Busca']").parent().addClass(CONST_CLASS_ITEM_SELECIONADO);

    gGradIntra_Navegacao_PainelDeBuscaAtual
        .show();

    gGradIntra_Navegacao_PainelDeConteudoAtual
        .show()
        .find("ul.MenuDoObjeto")
            .show()
            .find("li.SubTitulo, li.Tipo_" + pItemIncluido.TipoCliente.charAt(1))
                .show();

    GradIntra_Clientes_ReverterTipoDeClienteAtual( pItemIncluido.TipoCliente, false);
}

function GradIntra_Clientes_SalvarItemFilhoDoCliente(pTipoDeFilho)
{
    var lDiv, lURL;

    lDiv = $("#pnlClientes_Formularios_Dados_" + pTipoDeFilho);
    lURL = "Clientes/Formularios/Dados/" + pTipoDeFilho + ".aspx";

    GradIntra_Cadastro_SalvarFormulario(lDiv, lURL, "Salvar");
}

function GradIntra_Clientes_IniciarImportacao(pMetodoAlvo)
{  
    $("#btnClientes_Importar_Submit").prop("disabled", true);

    var lImportar = GradIntra_Clientes_Receber_Importavao();

    var lJsonDeImportacao = $.toJSON(lImportar);
        
    var lDados = { Acao: pMetodoAlvo ? pMetodoAlvo : "ResponderImportacao", ObjetoJson: lJsonDeImportacao }       
        
    GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/ImportarClienteDoSinacor.aspx"
                                         , lDados
                                         , GradIntra_Clientes_IniciarImportacao_CallBack);
}

function GradIntra_Clientes_IniciarImportacao_CallBack(pResposta)
{
    if (pResposta.Mensagem == "Erro ao Tentar Importar o Cliente: CPF/CNPJ já cadastrado"
    && (confirm("Cliente já cadastrado.\n\nImportante: Ao reimportar, o cliente será excluído do cadastro e receberá as informações oriundas do Sinacor. Deseja continuar?")))
    {
        GradIntra_Clientes_IniciarImportacao("ResponderReimportacao");
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
        $("#btnClientes_Importar_Submit").prop("disabled", false);
    }
}

function GradIntra_Clientes_Receber_Importavao()
{
    var lDados = { Acao: "ResponderImportacao"
                 , CPF_CNPJ: $("#txtClientes_Importar_CPF").val().replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "")
                 , DataNascimento:$("#txtClientes_Importar_DataNascimento").val()
                 , CondicaoDependente:$("#txtClientes_Importar_CondicaoDependente").val() };

    return lDados;
}

function GradIntra_Clientes_IniciarSincronizacaoComSinacor()
{    
    if(gGradIntra_Cadastro_ItemSendoEditado.Id)
    {
        $("#btnClientes_SincronizarComSinacor_Submit").prop("disabled", true);

        $("#tblClientes_SincronizarComSinacor_Resultado").hide();

        var lDados = { Acao: "SincronizarComSinacor", Id: gGradIntra_Cadastro_ItemSendoEditado.Id };

        GradIntra_CarregarJsonVerificandoErro(  "Clientes/Formularios/Acoes/SincronizarComSinacor.aspx"
                                              , lDados
                                              , GradIntra_Clientes_IniciarSincronizacaoComSinacor_CallBack);
    }
}

function GradIntra_Clientes_IniciarSincronizacaoComSinacor_CallBack(pResposta)
{
    $("#btnClientes_SincronizarComSinacor_Submit").prop("disabled", false);

    var lTransporte = pResposta.ObjetoDeRetorno;
    
    $("#pnlClientes_SincronizarComSinacor_Requisicao").hide();
    
    $("#pnlClientes_SincronizarComSinacor_Resultado")
        .html(lTransporte.Resultado)
        .show();
    
    if(lTransporte.Mensagens.length > 0)
    {
        $(lTransporte.Mensagens).each(function()
        {
            $("#pnlClientes_SincronizarComSinacor_Mensagens")
                .append("<li>" + (this) + "</li>");
        });
        
        $("#pnlClientes_SincronizarComSinacor_Mensagens").show();
    }
    else
    {
        $("#pnlClientes_SincronizarComSinacor_Mensagens").hide();
    }

    if(lTransporte.GridErro.length > 0)
    {
        var lTRs = "";

        for(var a = 0; a < lTransporte.GridErro.length; a++)
        {
            lTRs += "<tr>"
                 +  "   <td>" + lTransporte.GridErro[a].Tipo      + "</td>"
                 +  "   <td>" + lTransporte.GridErro[a].Descricao + "</td>"
                 +  "</tr>";
        }

        $("#tblClientes_SincronizarComSinacor_Resultado tbody")
            .html("")
            .append(lTRs)
            .parent()            
            .show();
    }

    $("#btnClientes_SincronizarComSinacor_Submit").prop("disabled", false);

    //GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);

    //gGradIntra_Clientes_SincronizacaoComSinacorAtual = pResposta.ObjetoDeRetorno;

    //gGradIntra_Clientes_SincronizacaoComSinacorAtual.Interval = window.setInterval(GradIntra_Clientes_VerificarSincronizacaoComSinacor, 5000);
}

/*
function GradIntra_Clientes_VerificarSincronizacaoComSinacor()
{
    
    if(gGradIntra_Clientes_SincronizacaoComSinacorAtual)
    {
        var lDados = {  Acao:              "VerificarSincronizacao"
                      , IdDaSincronizacao: gGradIntra_Clientes_SincronizacaoComSinacorAtual.IdDaSincronizacao 
                     };

        GradIntra_CarregarJsonVerificandoErro(  "Clientes/Formularios/Acoes/SincronizarComSinacor.aspx"
                                              , lDados
                                              , GradIntra_Clientes_VerificarSincronizacaoComSinacor_CallBack);
    }
}
*/

/*
function GradIntra_Clientes_VerificarSincronizacaoComSinacor_CallBack(pResposta)
{
    if(pResposta.Mensagem != "INEXISTENTE")
    {
        gGradIntra_Clientes_SincronizacaoComSinacorAtual = pResposta.ObjetoDeRetorno;

        if(gGradIntra_Clientes_SincronizacaoComSinacorAtual.StatusDaSincronizacao == "Finalizada")
        {
            GradIntra_ExibirMensagem("I", "Sincronização finalizada com sucesso!", false);
            
            GradIntra_Clientes_SincronizacaoComSinacor_FinalizarSincronizacaoAtual();
        }
        else if(gGradIntra_Clientes_SincronizacaoComSinacorAtual.StatusDaSincronizacao == "Com Erro")
        {
            GradIntra_ExibirMensagem("E", gGradIntra_Clientes_SincronizacaoComSinacorAtual.MensagemDeFinalizacao);
            
            GradIntra_Clientes_SincronizacaoComSinacor_FinalizarSincronizacaoAtual();
        }
        else
        {
            GradIntra_ExibirMensagem("I", "Sincronização em andamento...", false);
        }
    }
    else
    {
        //failsafe se acontecer alguma coisa com o application e perder o ID
        window.clearInterval(gGradIntra_Clientes_SincronizacaoComSinacorAtual.Interval);
    }
}
*/

function GradIntra_Clientes_SincronizacaoComSinacor_FinalizarSincronizacaoAtual()
{
    window.clearInterval(gGradIntra_Clientes_SincronizacaoComSinacorAtual.Interval);

    gGradIntra_Clientes_SincronizacaoComSinacorAtual = null;

    $("#pnlClientes_SincronizarComSinacor_Requisicao").show();
    $("#pnlClientes_SincronizarComSinacor_Submit").show();

    $("#pnlClientes_SincronizarComSinacor_EmAndamento").hide();
}

function GradIntra_Clientes_Suitability_VerificarExistencia()
{
    var lTemResultado = false;

    var lResultado = $("#hidCliente_Suitability_Resultado").val();

    if(lResultado && lResultado != "")
    {
        lResultado = $.evalJSON(lResultado);

        lTemResultado = true;
    }

    if(lTemResultado)
    {
        $(".Resultado_" + lResultado.Resultado).parent().find("img").hide();

        $(".Resultado_" + lResultado.Resultado).show();

        var lApend = "";

        if(GradIntra_RaizDoSite().indexOf(":4242") != -1)
            lApend = "/Intranet";

        if(lResultado.LinkParaArquivoCiencia == null || lResultado.LinkParaArquivoCiencia == "")
        {
            $("#id_Cliente_Suitability_Resultado").html("Realizado por: " + lResultado.Usuario  + " Data: " + lResultado.DataDaRealizacao + " Local: " + lResultado.Sistema);
        }
        else
        {
            $("#id_Cliente_Suitability_Resultado").html("Realizado por: " + lResultado.Usuario  + " Data: " + lResultado.DataDaRealizacao + " Local: " + lResultado.Sistema + " " + "<a target='_blank' href='" + lApend + lResultado.LinkParaArquivoCiencia.replace("~", "") + "'>ciência: " + lResultado.DataArquivoCiencia + "</a>");
        }

        

        $("#pnlCliente_Suitability_Questionario").hide();
        $("#pnlCliente_Suitability_BotaoEnviar").hide();

        $("#pnlCliente_Suitability_Resultado").show();
    }
    else
    {
        $("#pnlCliente_Suitability_Resultado").hide();

        $("#pnlCliente_Suitability_Questionario").show();
        $("#pnlCliente_Suitability_BotaoEnviar").show();
    }
}

function GradIntra_Clientes_Suitability_EnviarRespostas()
{
    var lDados = { Acao: "Salvar", Id: gGradIntra_Cadastro_ItemSendoEditado.Id, Respostas: "", Email: gGradIntra_Cadastro_ItemSendoEditado.Email};

    if($("[name^='rdoSuit_']:checked").length < 4)
    {
        alert("Existem questões não respondidas, favor preencher o formulário completo.");

        return false;
    }

    var lId;

    for(var a = 1; a <= 11; a++)
    {

        if(a < 10)
        {
            lId = "rdoSuit_0" + a;
        }
        else
        {
            lId = "rdoSuit_" + a;
        }

        if(a != 9)
        {
            lDados.Respostas += "" + a + ":" + $("[name^='" + lId + "']:checked").val() + ",";
        }
        else
        {
            var respostas = document.querySelectorAll('[name=rdoSuit_09]:checked');
            var values = [];
            var opcoes = [];
            for (var i = 0; i < respostas.length; i++)
            {
                opcoes += (parseInt(respostas[i].value.split("|")[0])+1)+"|"; // Devido a troca para valor quebrado
                values.push(parseFloat(respostas[i].value.split("|")[1]));
            }

            var pontos = values.reduce(function (a, b) { return a + b; }, 0);
            lDados.Respostas += "" + a + ":" + opcoes + ",";
        }
    }

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/Suitability.aspx"
                                          , lDados
                                          , GradIntra_Clientes_Suitability_EnviarRespostas_CallBack);
}

function GradIntra_Clientes_Suitability_EnviarRespostas_CallBack(pResposta)
{
    $("#hidCliente_Suitability_Resultado").val($.toJSON(pResposta.ObjetoDeRetorno));

    GradIntra_Clientes_Suitability_VerificarExistencia();

    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function GradIntra_Clientes_Suitability_RefazerTeste()
{
    $("#pnlCliente_Suitability_Resultado").hide();

    $("#pnlCliente_Suitability_Questionario").show();
    $("#pnlCliente_Suitability_BotaoEnviar").show();
}

function GradIntra_Clientes_Suitability_DownloadPDF()
{
    var lResultado = $.evalJSON(  $("#hidCliente_Suitability_Resultado").val()  );

    if(lResultado && lResultado.Resultado)
        window.open("../Extras/Perfil_" + lResultado.Resultado + ".pdf");
}

function GradIntra_Clientes_Suitability_EnviarPorEmail()
{
    GradIntra_ExibirMensagem("A", "Não Implementado...");
}

function GradIntra_Clientes_EfetivarRenovacao()
{
    var lDados = { Acao             : "EfetivarRenovacao"
                 , CPF              : gGradIntra_Cadastro_ItemSendoEditado.CPF
                 , CdClienteBMF     : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                 , CdClienteBovespa : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                 , IdAssessor       : gGradIntra_Cadastro_ItemSendoEditado.CodAssessor
                 , Data             : $("#txtClientes_EfetivarRenovacao_Data").val()
                 , IdCliente        : gGradIntra_Cadastro_ItemSendoEditado.Id
                 };

    GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Acoes/EfetivarRenovacao.aspx"
                                         , lDados
                                         , GradIntra_Clientes_EfetivarRenovacao_CallBack);
}

function GradIntra_Clientes_EfetivarRenovacao_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function GradIntra_Clientes_SalvarContratos(pDivDeFormulario)
{
    if($("[name^='chkClientes_Contratos_']:checked").length == 0 )
    {
        alert("É necessário selecionar um contrato!");

        return false;
    }

    var lDados = 
    { 
         Acao: "Salvar", 
         ParentId: $("#txtClientes_Contratos_ParentId").val(),
         Contratos: "",
         Email: gGradIntra_Cadastro_ItemSendoEditado.Email
    };

    for (var i = 0 ; i < $("[name^='chkClientes_Contratos_']:checked").length; i++)
        lDados.Contratos += $("[name^='chkClientes_Contratos_']:checked")[i].value + ",";
    
    GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Dados/Contratos.aspx"
                                         , lDados
                                         , function(){GradIntra_ExibirMensagem("A", "Dados salvos com sucesso.", true);});
}

function GradIntra_Clientes_Limites_AdicionarContrato()
{
    var lContrato = $("#cboCliente_Limites_AdicionarContrato").val();

    var lInstrumento = $("#cboCliente_Limites_AdicionarInstrumento").val();

    var lTbody = $("#lstCliente_Limites_PorContrato tbody");

    if(lContrato != "")
    {
        var lTRContrato = lTbody.find("tr[Contrato='" + lContrato + "']");

        var lTRInstrumento = lTbody.find("tr[ContratoPai='" + lContrato + "'][Instrumento='" + lInstrumento + "']");
        
        var lAdicionarContrato = false;
        var lAdicionarInstrumento = false;

        if(lTRContrato.length == 0)
        {
            //ainda não tinha sido adicionado o contrato
            lAdicionarContrato= true;
            
            if(lInstrumento != "") lAdicionarInstrumento = true;
        }
        else
        {
            //contrato já havia sido adicionado...

            if(lInstrumento == "")
            {
                //não escolheu instrumento, então só pisca a de contrato
                lTRContrato.effect("highlight").effect("highlight");
            }
            else
            {
                if(lTRInstrumento.length == 0)
                {
                    // o instrumento ainda não havia sido incluído
                    lAdicionarInstrumento = true;
                }
                else
                {
                    // o instrumento já havia sido incluído
                    lTRInstrumento.effect("highlight").effect("highlight");
                }
            }
        }

        if(lAdicionarContrato)
        {
            lTRContrato = lTbody.find("tr.Template_Contrato").clone();

            lTRContrato
                .attr("class", "Contrato")
                .attr("Contrato", lContrato)
                .find("td:eq(0) label")
                    .html(lContrato);

            lTbody.append(lTRContrato);

            lTRContrato
                .show()
                .find("td:eq(1) input").focus();

            lTRContrato.find("input.ProibirLetras").bind("keydown", function(event) { Validacao_SomenteNumeros_OnKeyDown(event) } );
        }

        if(lAdicionarInstrumento)
        {
            var lNovaTRInstrumento = lTbody.find("tr.Template_Instrumento").clone();

            lNovaTRInstrumento
                .attr("class", "Instrumento")
                .attr("ContratoPai", lContrato)
                .attr("Instrumento", lInstrumento)
                .find("td:eq(0) label")
                    .html(lInstrumento);

            lTRContrato
                .addClass(CONST_CLASS_ITEM_EXPANDIDO)
                .after(lNovaTRInstrumento);

            lNovaTRInstrumento.show();
            
            lNovaTRInstrumento.find("input.ProibirLetras").bind("keydown", function(event) { Validacao_SomenteNumeros_OnKeyDown(event) } );
        }
        lTbody.find("tr.Nenhuma").hide();
    }
    else
    {
        GradIntra_ExibirMensagem("A", "Favor escolher um contrato para adicionar", true);
    }
}

function GradIntra_Clientes_Limites_ExcluirContrato(pTR)
{
    var lTBody = pTR.closest("tbody");

    if(pTR.hasClass("Contrato"))
    {
        if(confirm("Tem certeza que você deseja excluir esse contrato? Todos os instrumentos dele também serão excluídos!"))
        {
            var lContrato = pTR.attr("Contrato");

            pTR.parent().find("tr[ContratoPai='" + lContrato + "']").remove();

            pTR.remove();
        }
    }
    else if(pTR.hasClass("Instrumento"))
    {
        pTR.remove();
    }

    if(lTBody.find("tr:visible").length == 0)
    {
        lTBody.find("tr.Nenhuma").show();
    }
}

function GradIntra_Clientes_Spider_Limites_SalvarLimitesBovespa() 
{
    var lLimites = GradIntra_Clientes_Spider_Limites_ValidarValoresBovespa();

    if (lLimites.Valido) {
        var lLimitesJson = $.toJSON(lLimites);

        var lDados = { Acao: "SalvarLimitesBovespa", ObjetoJson: lLimitesJson };

        GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/SpiderLimitesBVSP.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Limites_SalvarLimitesBovespa_CallBack
                                             );
    }
    else 
    {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar", true);
    }
}

function GradIntra_Clientes_Limites_SalvarLimitesBovespa_NovoOMS() 
{
    var lLimites = GradIntra_Clientes_Limites_ValidarValoresBovespa_NovoOMS();

    if (lLimites.Valido) {
        var lLimitesJson = $.toJSON(lLimites);

        var lDados = { Acao: "SalvarLimitesBovespa", ObjetoJson: lLimitesJson };

        GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/ConfigurarLimitesNovoOMS.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Limites_SalvarLimitesBovespa_CallBack
                                             );
    }
    else {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar", true);
    }
}

function GradIntra_Clientes_Limites_SalvarLimitesBovespa()
{
    var lLimites = GradIntra_Clientes_Limites_ValidarValoresBovespa();

    if( lLimites.Valido )
    {
        var lLimitesJson = $.toJSON(lLimites);

        var lDados = { Acao: "SalvarLimitesBovespa", ObjetoJson: lLimitesJson };

        GradIntra_CarregarJsonVerificandoErro(  "Clientes/Formularios/Acoes/ConfigurarLimites.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Limites_SalvarLimitesBovespa_CallBack
                                             );
    }
    else
    {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar", true);
    }
}

function GradIntra_Clientes_Limites_SalvarLimitesBovespa_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Clientes_Produtos_Termo_Adesao_Fundos_Salvar()
{
    var lFundo = $("#cboFundoTermo");
    
    var lNomeFundo = $("#cboFundoTermo option:selected").text();

    var lAdesao = { Valido: true, CodigoFundo : 0, NomeFundo: "", CodBovespa: 0,  Acao: "AderirTermo" };

    lAdesao.CodBovespa = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lAdesao.CodigoFundo = lFundo.val();
    lAdesao.NomeFundo = lNomeFundo;

    if (lAdesao.Valido)
    {
        GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Dados/DadosPlanosCliente.aspx"
                                             , lAdesao
                                             , GradIntra_Clientes_Produtos_Termo_Adesao_Fundos_Salvar_CallBack
                                             );
    }else
    {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar", true);
    }


}

function GradIntra_Clientes_Produtos_Termo_Adesao_Fundos_Salvar_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Clientes_Produtos_Salvar()
{
    var lProdutos = { Valido: true, Lista: [], CodBovespa: 0 };
    var lIdIncluido = 0;

    $(".AdicionadoParaExclusao")
        .each(function () {
            lIdIncluido = $(this).next("label").attr("idProduto");

            if (lIdIncluido.indexOf("Poupe") > -1) 
            {
                lIdIncluido = lIdIncluido.replace("Poupe", "");
            }

            if (!isNaN(lIdIncluido))
                lProdutos.Lista.push(lIdIncluido);
        });

    if (lProdutos.Lista.length == 0)
        return false;

    lProdutos.CodBovespa    = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lProdutos.GetCodCliente = gGradIntra_Cadastro_ItemSendoEditado.Id;
    lProdutos.DsCpfCnpj     = gGradIntra_Cadastro_ItemSendoEditado.CPF;

    if (lProdutos.Valido)
    {
        var lProdutosJson = $.toJSON(lProdutos);

        var lDados = { Acao: "SalvarDadosProdutos", ObjetoJson: lProdutosJson };

        GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Dados/DadosPlanosCliente.aspx"
                                             , lDados
                                             , GradIntra_Clientes_Produtos_Salvar_CallBack
                                             );
    }
    else
    {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar", true);
    }
}

function GradIntra_Clientes_Produtos_Salvar_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Clientes_Limites_ValidarValoresBovespa()
{
    var lRetorno = { Valido: true, FlagOperaAVista: false, LimiteAVista: 0, VencimentoAVista: "", FlagOperaAVistaDescoberto: false, LimiteAVistaDescoberto: 0, VencimentoAVistaDescoberto: ""
                   , FlagOperaOpcao: false,  LimiteOpcao: 0,  VencimentoOpcao: "",  FlagOperaOpcaoDescoberto: false,  LimiteOpcaoDescoberto: 0,  VencimentoOpcaoDescoberto: ""
                   , FlagMaximoDaOrdem: false, ValorMaximoDaOrdem: 0, VencimentoMaximoDaOrdem: ""
                   , FlagOperaOrdemStop: false, FlagOperaContaMargem: false, FlagOperaBovespa: false, FlagOperaBMF: false, Permissoes: [], CodBovespa: 0
                   , FlagOperaAVistaExpirarLimite: false, FlagOperaAVistaDescobertoExpirarLimite: false, FlagOperaOpcaoExpirarLimite: false, FlagOperaOpcaoDescobertoExpirarLimite: false, FlagMaximoDaOrdemExpirarLimite: false
                   , FlagOperaAVistaIncluirLimite: false, FlagOperaAVistaDescobertoIncluirLimite: false, FlagOperaOpcaoIncluirLimite: false, FlagOperaOpcaoDescobertoIncluirLimite: false, FlagMaximoDaOrdemIncluirLimite: false 
                   , FlagOperaAVistaRenovarLimite: false, FlagOperaAVistaDescobertoRenovarLimite: false, FlagOperaOpcaoRenovarLimite: false, FlagOperaOpcaoDescobertoRenovarLimite: false, FlagMaximoDaOrdemRenovarLimite: false
    };

    lRetorno.CodBovespa = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

    $(".pnlCliente_Limites_Container")
        .find(".custom-checkbox input[type='checkbox']:checked")
        .each(function () 
        {
            lRetorno.Permissoes.push($(this).next("label").attr("CodigoPermissao")) 
        });

    lRetorno.Valido = ($("#pnlCliente_Limites_Bovespa").find("div.formError").length == 0);

    if(lRetorno.Valido)
    {
        lRetorno.FlagOperaAVista = $("#chkCliente_Limites_Bosvespa_AVista_Opera").is(":checked");
        lRetorno.FlagOperaAVistaExpirarLimite = $("#txtCliente_Limites_Bosvespa_AVista_Opera_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaAVistaIncluirLimite = $("#txtCliente_Limites_Bosvespa_AVista_Opera_IncluirLimite").val() == "true";
        lRetorno.FlagOperaAVistaRenovarLimite = $("#txtCliente_Limites_Bosvespa_AVista_Opera_RenovarLimite").val() == "true";

        if(lRetorno.FlagOperaAVista)
        {
            lRetorno.LimiteAVista     = $.format.number(  $("#txtCliente_Limites_Bosvespa_AVista_Limite").val() );
            lRetorno.VencimentoAVista = $("#txtCliente_Limites_Bosvespa_AVista_DataDeVencimento").val();
        }

        lRetorno.FlagOperaAVistaDescoberto = $("#chkCliente_Limites_Bosvespa_AVista_OperaDescoberto").is(":checked");
        lRetorno.FlagOperaAVistaDescobertoExpirarLimite = $("#txtCliente_Limites_Bosvespa_AVista_OperaDescoberto_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaAVistaDescobertoIncluirLimite = $("#txtCliente_Limites_Bosvespa_AVista_OperaDescoberto_IncluirLimite").val() == "true";
        lRetorno.FlagOperaAVistaDescobertoRenovarLimite = $("#txtCliente_Limites_Bosvespa_AVista_OperaDescoberto_RenovarLimite").val() == "true";

        if(lRetorno.FlagOperaAVistaDescoberto)
        {
            lRetorno.LimiteAVistaDescoberto     = $.format.number(  $("#txtCliente_Limites_Bosvespa_AVista_LimiteDescoberto").val() );
            lRetorno.VencimentoAVistaDescoberto = $("#txtCliente_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto").val();
        }

        lRetorno.FlagOperaOpcao = $("#chkCliente_Limites_Bovespa_Opcao_Opera").is(":checked");
        lRetorno.FlagOperaOpcaoExpirarLimite = $("#txtCliente_Limites_Bovespa_Opcao_Opera_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaOpcaoIncluirLimite = $("#txtCliente_Limites_Bovespa_Opcao_Opera_IncluirLimite").val() == "true";
        lRetorno.FlagOperaOpcaoRenovarLimite = $("#txtCliente_Limites_Bovespa_Opcao_Opera_RenovarLimite").val() == "true";

        if(lRetorno.FlagOperaOpcao)
        {
            lRetorno.LimiteOpcao     = $.format.number(  $("#txtCliente_Limites_Bovespa_Opcao_Limite").val() );
            lRetorno.VencimentoOpcao = $("#txtCliente_Limites_Bovespa_Opcao_DataDeVencimento").val();
        }

        lRetorno.FlagOperaOpcaoDescoberto = $("#chkCliente_Limites_Bovespa_Opcao_OperaDescoberto").is(":checked");
        lRetorno.FlagOperaOpcaoDescobertoExpirarLimite = $("#txtCliente_Limites_Bovespa_Opcao_OperaDescoberto_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaOpcaoDescobertoIncluirLimite = $("#txtCliente_Limites_Bovespa_Opcao_OperaDescoberto_IncluirLimite").val() == "true";
        lRetorno.FlagOperaOpcaoDescobertoRenovarLimite = $("#txtCliente_Limites_Bovespa_Opcao_OperaDescoberto_RenovarLimite").val() == "true";

        if(lRetorno.FlagOperaOpcaoDescoberto)
        {
            lRetorno.LimiteOpcaoDescoberto     = $.format.number(  $("#txtCliente_Limites_Bovespa_Opcao_LimiteDescoberto").val() );
            lRetorno.VencimentoOpcaoDescoberto = $("#txtCliente_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto").val();
        }

        lRetorno.FlagMaximoDaOrdem = $("#chkCliente_Limites_Bovespa_ValorMaximoDaOrdem").is(":checked");
        lRetorno.FlagMaximoDaOrdemExpirarLimite = $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_ExpirarLimite").val() == "true";
        lRetorno.FlagMaximoDaOrdemIncluirLimite = $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_IncluirLimite").val() == "true";
        lRetorno.FlagMaximoDaOrdemRenovarLimite = $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_RenovarLimite").val() == "true";

        if (lRetorno.FlagMaximoDaOrdem)
        {
            lRetorno.ValorMaximoDaOrdem = $.format.number($("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem").val());
            lRetorno.VencimentoMaximoDaOrdem = $("#txtCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento").val();
        }

        lRetorno.FlagOperaOrdemStop   = $("#chkCliente_Limites_Bovespa_OperaOrdemStop").is(":checked");
        lRetorno.FlagOperaContaMargem = $("#chkCliente_Limites_Bovespa_OperaContaMargem").is(":checked");
        lRetorno.FlagOperaBovespa     = $("#chkCliente_Limites_Bovespa_OperaBovespa").is(":checked");
        lRetorno.FlagOperaBMF         = $("#chkCliente_Limites_Bovespa_OperaBMF").is(":checked");
    }

    return lRetorno;
}

function GradIntra_Clientes_Limites_ValidarValoresBovespa_NovoOMS() 
{
    var lRetorno = { Valido: true, FlagOperaAVista: false, LimiteAVista: 0, VencimentoAVista: "", FlagOperaAVistaDescoberto: false, LimiteAVistaDescoberto: 0, VencimentoAVistaDescoberto: ""
                   , FlagOperaOpcao: false, LimiteOpcao: 0, VencimentoOpcao: "", FlagOperaOpcaoDescoberto: false, LimiteOpcaoDescoberto: 0, VencimentoOpcaoDescoberto: ""
                   , FlagMaximoDaOrdem: false, ValorMaximoDaOrdem: 0, VencimentoMaximoDaOrdem: ""
                   , FlagOperaOrdemStop: false, FlagOperaContaMargem: false, FlagOperaBovespa: false, FlagOperaBMF: false, Permissoes: [], CodBovespa: 0
                   , FlagOperaAVistaExpirarLimite: false, FlagOperaAVistaDescobertoExpirarLimite: false, FlagOperaOpcaoExpirarLimite: false, FlagOperaOpcaoDescobertoExpirarLimite: false, FlagMaximoDaOrdemExpirarLimite: false
                   , FlagOperaAVistaIncluirLimite: false, FlagOperaAVistaDescobertoIncluirLimite: false, FlagOperaOpcaoIncluirLimite: false, FlagOperaOpcaoDescobertoIncluirLimite: false, FlagMaximoDaOrdemIncluirLimite: false
                   , FlagOperaAVistaRenovarLimite: false, FlagOperaAVistaDescobertoRenovarLimite: false, FlagOperaOpcaoRenovarLimite: false, FlagOperaOpcaoDescobertoRenovarLimite: false, FlagMaximoDaOrdemRenovarLimite: false
    };

    lRetorno.CodBovespa = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

    $(".pnlCliente_Limites_Container_NovoOMS")
        .find(".custom-checkbox input[type='checkbox']:checked")
        .each(function () 
        {
            lRetorno.Permissoes.push($(this).next("label").attr("CodigoPermissao"))
        });

    lRetorno.Valido = ($("#pnlCliente_Limites_Bovespa").find("div.formError").length == 0);

    if (lRetorno.Valido) 
    {
        lRetorno.FlagOperaAVista              = $("#chkCliente_Limites_Bosvespa_AVista_Opera_NovoOMS").is(":checked");
        lRetorno.FlagOperaAVistaExpirarLimite = $("#txtCliente_Limites_Bosvespa_AVista_Opera_ExpirarLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaAVistaIncluirLimite = $("#txtCliente_Limites_Bosvespa_AVista_Opera_IncluirLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaAVistaRenovarLimite = $("#txtCliente_Limites_Bosvespa_AVista_Opera_RenovarLimite_NovoOMS").val() == "true";

        if (lRetorno.FlagOperaAVista) 
        {
            lRetorno.LimiteAVista     = $.format.number($("#txtCliente_Limites_Bosvespa_AVista_Limite_NovoOMS").val());
            lRetorno.VencimentoAVista = $("#txtCliente_Limites_Bosvespa_AVista_DataDeVencimento_NovoOMS").val();
        }

        lRetorno.FlagOperaAVistaDescoberto              = $("#chkCliente_Limites_Bosvespa_AVista_OperaDescoberto_NovoOMS").is(":checked");
        lRetorno.FlagOperaAVistaDescobertoExpirarLimite = $("#txtCliente_Limites_Bosvespa_AVista_OperaDescoberto_ExpirarLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaAVistaDescobertoIncluirLimite = $("#txtCliente_Limites_Bosvespa_AVista_OperaDescoberto_IncluirLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaAVistaDescobertoRenovarLimite = $("#txtCliente_Limites_Bosvespa_AVista_OperaDescoberto_RenovarLimite_NovoOMS").val() == "true";

        if (lRetorno.FlagOperaAVistaDescoberto) 
        {
            lRetorno.LimiteAVistaDescoberto     = $.format.number($("#txtCliente_Limites_Bosvespa_AVista_LimiteDescoberto_NovoOMS").val());
            lRetorno.VencimentoAVistaDescoberto = $("#txtCliente_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto_NovoOMS").val();
        }

        lRetorno.FlagOperaOpcao              = $("#chkCliente_Limites_Bovespa_Opcao_Opera_NovoOMS").is(":checked");
        lRetorno.FlagOperaOpcaoExpirarLimite = $("#txtCliente_Limites_Bovespa_Opcao_Opera_ExpirarLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaOpcaoIncluirLimite = $("#txtCliente_Limites_Bovespa_Opcao_Opera_IncluirLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaOpcaoRenovarLimite = $("#txtCliente_Limites_Bovespa_Opcao_Opera_RenovarLimite_NovoOMS").val() == "true";

        if (lRetorno.FlagOperaOpcao) 
        {
            lRetorno.LimiteOpcao = $.format.number($("#txtCliente_Limites_Bovespa_Opcao_Limite_NovoOMS").val());
            lRetorno.VencimentoOpcao = $("#txtCliente_Limites_Bovespa_Opcao_DataDeVencimento_NovoOMS").val();
        }

        lRetorno.FlagOperaOpcaoDescoberto              = $("#chkCliente_Limites_Bovespa_Opcao_OperaDescoberto_NovoOMS").is(":checked");
        lRetorno.FlagOperaOpcaoDescobertoExpirarLimite = $("#txtCliente_Limites_Bovespa_Opcao_OperaDescoberto_ExpirarLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaOpcaoDescobertoIncluirLimite = $("#txtCliente_Limites_Bovespa_Opcao_OperaDescoberto_IncluirLimite_NovoOMS").val() == "true";
        lRetorno.FlagOperaOpcaoDescobertoRenovarLimite = $("#txtCliente_Limites_Bovespa_Opcao_OperaDescoberto_RenovarLimite_NovoOMS").val() == "true";

        if (lRetorno.FlagOperaOpcaoDescoberto) 
        {
            lRetorno.LimiteOpcaoDescoberto     = $.format.number($("#txtCliente_Limites_Bovespa_Opcao_LimiteDescoberto_NovoOMS").val());
            lRetorno.VencimentoOpcaoDescoberto = $("#txtCliente_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto_NovoOMS").val();
        }

        lRetorno.FlagMaximoDaOrdem              = $("#chkCliente_Limites_Bovespa_ValorMaximoDaOrdem_NovoOMS").is(":checked");
        lRetorno.FlagMaximoDaOrdemExpirarLimite = $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_ExpirarLimite_NovoOMS").val() == "true";
        lRetorno.FlagMaximoDaOrdemIncluirLimite = $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_IncluirLimite_NovoOMS").val() == "true";
        lRetorno.FlagMaximoDaOrdemRenovarLimite = $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_RenovarLimite_NovoOMS").val() == "true";

        if (lRetorno.FlagMaximoDaOrdem) 
        {
            lRetorno.ValorMaximoDaOrdem      = $.format.number($("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem_NovoOMS").val());
            lRetorno.VencimentoMaximoDaOrdem = $("#txtCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento_NovoOMS").val();
        }

        lRetorno.FlagOperaOrdemStop   = $("#chkCliente_Limites_Bovespa_OperaOrdemStop").is(":checked");
        lRetorno.FlagOperaContaMargem = $("#chkCliente_Limites_Bovespa_OperaContaMargem").is(":checked");
        lRetorno.FlagOperaBovespa     = $("#chkCliente_Limites_Bovespa_OperaBovespa").is(":checked");
        lRetorno.FlagOperaBMF         = $("#chkCliente_Limites_Bovespa_OperaBMF").is(":checked");
    }

    return lRetorno;
}

function GradIntra_Clientes_Spider_Limites_ValidarValoresBovespa() 
{
    var lRetorno = { Valido: true, FlagOperaAVista: false, LimiteAVista: 0, VencimentoAVista: "", FlagOperaAVistaDescoberto: false, LimiteAVistaDescoberto: 0, VencimentoAVistaDescoberto: ""
                   , FlagOperaOpcao: false, LimiteOpcao: 0, VencimentoOpcao: "", FlagOperaOpcaoDescoberto: false, LimiteOpcaoDescoberto: 0, VencimentoOpcaoDescoberto: ""
                   , FlagMaximoDaOrdem: false, ValorMaximoDaOrdem: 0, VencimentoMaximoDaOrdem: ""
                   , FlagOperaOrdemStop: false, FlagOperaContaMargem: false, FlagOperaBovespa: false, FlagOperaBMF: false, Permissoes: [], CodBovespa: 0
                   , FlagOperaAVistaExpirarLimite: false, FlagOperaAVistaDescobertoExpirarLimite: false, FlagOperaOpcaoExpirarLimite: false, FlagOperaOpcaoDescobertoExpirarLimite: false, FlagMaximoDaOrdemExpirarLimite: false
                   , FlagOperaAVistaIncluirLimite: false, FlagOperaAVistaDescobertoIncluirLimite: false, FlagOperaOpcaoIncluirLimite: false, FlagOperaOpcaoDescobertoIncluirLimite: false, FlagMaximoDaOrdemIncluirLimite: false
                   , FlagOperaAVistaRenovarLimite: false, FlagOperaAVistaDescobertoRenovarLimite: false, FlagOperaOpcaoRenovarLimite: false, FlagOperaOpcaoDescobertoRenovarLimite: false, FlagMaximoDaOrdemRenovarLimite: false
    };

    lRetorno.CodBovespa = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

    $(".pnlCliente_Spider_Limites_Container")
        .find(".custom-checkbox input[type='checkbox']:checked")
        .each(function () 
        {
            lRetorno.Permissoes.push($(this).next("label").attr("CodigoPermissao"))
        });

    lRetorno.Valido = ($("#pnlCliente_Limites_Bovespa").find("div.formError").length == 0);

    if (lRetorno.Valido) 
    {
        lRetorno.FlagOperaAVista              = $("#chkCliente_Spider_Limites_Bosvespa_AVista_Opera").is(":checked");
        lRetorno.FlagOperaAVistaExpirarLimite = $("#txtCliente_Spider_Limites_Bosvespa_AVista_Opera_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaAVistaIncluirLimite = $("#txtCliente_Spider_Limites_Bosvespa_AVista_Opera_IncluirLimite").val() == "true";
        lRetorno.FlagOperaAVistaRenovarLimite = $("#txtCliente_Spider_Limites_Bosvespa_AVista_Opera_RenovarLimite").val() == "true";

        if (lRetorno.FlagOperaAVista) 
        {
            lRetorno.LimiteAVista     = $.format.number($("#txtCliente_Spider_Limites_Bosvespa_AVista_Limite").val());
            lRetorno.VencimentoAVista = $("#txtCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimento").val();
        }

        lRetorno.FlagOperaAVistaDescoberto              = $("#chkCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto").is(":checked");
        lRetorno.FlagOperaAVistaDescobertoExpirarLimite = $("#txtCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaAVistaDescobertoIncluirLimite = $("#txtCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto_IncluirLimite").val() == "true";
        lRetorno.FlagOperaAVistaDescobertoRenovarLimite = $("#txtCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto_RenovarLimite").val() == "true";

        if (lRetorno.FlagOperaAVistaDescoberto) 
        {
            lRetorno.LimiteAVistaDescoberto     = $.format.number($("#txtCliente_Spider_Limites_Bosvespa_AVista_LimiteDescoberto").val());
            lRetorno.VencimentoAVistaDescoberto = $("#txtCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto").val();
        }

        lRetorno.FlagOperaOpcao              = $("#chkCliente_Spider_Limites_Bovespa_Opcao_Opera").is(":checked");
        lRetorno.FlagOperaOpcaoExpirarLimite = $("#txtCliente_Spider_Limites_Bovespa_Opcao_Opera_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaOpcaoIncluirLimite = $("#txtCliente_Spider_Limites_Bovespa_Opcao_Opera_IncluirLimite").val() == "true";
        lRetorno.FlagOperaOpcaoRenovarLimite = $("#txtCliente_Spider_Limites_Bovespa_Opcao_Opera_RenovarLimite").val() == "true";

        if (lRetorno.FlagOperaOpcao) 
        {
            lRetorno.LimiteOpcao = $.format.number($("#txtCliente_Spider_Limites_Bovespa_Opcao_Limite").val());
            lRetorno.VencimentoOpcao = $("#txtCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimento").val();
        }

        lRetorno.FlagOperaOpcaoDescoberto              = $("#chkCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto").is(":checked");
        lRetorno.FlagOperaOpcaoDescobertoExpirarLimite = $("#txtCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto_ExpirarLimite").val() == "true";
        lRetorno.FlagOperaOpcaoDescobertoIncluirLimite = $("#txtCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto_IncluirLimite").val() == "true";
        lRetorno.FlagOperaOpcaoDescobertoRenovarLimite = $("#txtCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto_RenovarLimite").val() == "true";

        if (lRetorno.FlagOperaOpcaoDescoberto) 
        {
            lRetorno.LimiteOpcaoDescoberto     = $.format.number($("#txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto").val());
            lRetorno.VencimentoOpcaoDescoberto = $("#txtCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto").val();
        }

        lRetorno.FlagMaximoDaOrdem              = $("#chkCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem").is(":checked");
        lRetorno.FlagMaximoDaOrdemExpirarLimite = $("#txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem_ExpirarLimite").val() == "true";
        lRetorno.FlagMaximoDaOrdemIncluirLimite = $("#txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem_IncluirLimite").val() == "true";
        lRetorno.FlagMaximoDaOrdemRenovarLimite = $("#txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem_RenovarLimite").val() == "true";

        if (lRetorno.FlagMaximoDaOrdem) 
        {
            lRetorno.ValorMaximoDaOrdem      = $.format.number($("#txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem").val());
            lRetorno.VencimentoMaximoDaOrdem = $("#txtCliente_Spider_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento").val();
        }

        lRetorno.FlagOperaOrdemStop   = $("#chkCliente_Spider_Limites_Bovespa_OperaOrdemStop").is(":checked");
        lRetorno.FlagOperaContaMargem = $("#chkCliente_Spider_Limites_Bovespa_OperaContaMargem").is(":checked");
        lRetorno.FlagOperaBovespa     = $("#chkCliente_Spider_Limites_Bovespa_OperaBovespa").is(":checked");
        lRetorno.FlagOperaBMF         = $("#chkCliente_Spider_Limites_Bovespa_OperaBMF").is(":checked");
    }

    return lRetorno;
}


function GradIntra_Clientes_Limites_SalvarLimitesBMF()
{
    var lContratos = GradIntra_Clientes_Limites_ValidarValoresBMF();

    if( lContratos.Valido )
    {
        var lContratosJson = $.toJSON(lContratos);

        var lDados = { Acao: "SalvarLimitesBMF", ObjetoJson: lContratosJson };

        GradIntra_CarregarJsonVerificandoErro(  "Clientes/Formularios/Acoes/ConfigurarLimites.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Limites_SalvarLimitesBMF_CallBack
                                             );
    }
}

function GradIntra_Clientes_Limites_SalvarLimitesBMF_CallBack(pResposta)
{
    GradIntra_ExibirMensagem(pResposta.Mensagem);
}

function GradIntra_Clientes_Limites_ValidarValoresBMF()
{
    var lTRsContratos = $("#lstCliente_Limites_PorContrato tbody tr[Contrato]:visible");

    var lRetorno = { Valido: true, Contratos: new Array(), UltimoContrato: function() { return this.Contratos[this.Contratos.length - 1]; } };

    if(lTRsContratos.length > 0)
    {
        var  txtTCD,  txtTVD,  txtLCI,  txtLVI;
        var _txtTCD, _txtTVD, _txtLCI, _txtLVI;
        
        var  lTCD,  lTVD,  lLCI,  lLVI;
        var _lTCD, _lTVD, _lLCI, _lLVI; 
        
        var lTotalTCD, lTotalTVD, lTotalLCI, lTotalLVI;

        var lTRAtual, lTRsInstrumentos, lTRInstrumentoAtual;

        var lTextBox, lValorAtual;

        var lContrato;

        lTRsContratos.each(function()
        {
            lTRAtual = $(this);

            lTRAtual.find("input[type='text']").removeClass("ValorInvalido").attr("title", null);

            lContrato = lTRAtual.attr("Contrato");

            txtTCD = lTRAtual.find("input.txtCliente_Limite_TCD");
            txtTVD = lTRAtual.find("input.txtCliente_Limite_TVD");
            txtLCI = lTRAtual.find("input.txtCliente_Limite_LCI");
            txtLVI = lTRAtual.find("input.txtCliente_Limite_LVI");

            if (txtTCD.val() == "") { GradIntra_Clientes_Limites_InvalidarTextBox(txtTCD, "Campo não pode ser vazio"); lRetorno.Valido = false; }
            if (txtTVD.val() == "") { GradIntra_Clientes_Limites_InvalidarTextBox(txtTVD, "Campo não pode ser vazio"); lRetorno.Valido = false; }
            if (txtLCI.val() == "") { GradIntra_Clientes_Limites_InvalidarTextBox(txtLCI, "Campo não pode ser vazio"); lRetorno.Valido = false; }
            if (txtLVI.val() == "") { GradIntra_Clientes_Limites_InvalidarTextBox(txtLVI, "Campo não pode ser vazio"); lRetorno.Valido = false; }
            
            if (lRetorno.Valido)
            {
                lTCD = new Number( txtTCD.val() );
                lTVD = new Number( txtTVD.val() );
                lLCI = new Number( txtLCI.val() );
                lLVI = new Number( txtLVI.val() );

                if (isNaN(lTCD)) { GradIntra_Clientes_Limites_InvalidarTextBox(txtTCD, "Valor inválido"); lRetorno.Valido = false; }
                if (isNaN(lTVD)) { GradIntra_Clientes_Limites_InvalidarTextBox(txtTVD, "Valor inválido"); lRetorno.Valido = false; }
                if (isNaN(lLCI)) { GradIntra_Clientes_Limites_InvalidarTextBox(txtLCI, "Valor inválido"); lRetorno.Valido = false; }
                if (isNaN(lLVI)) { GradIntra_Clientes_Limites_InvalidarTextBox(txtLVI, "Valor inválido"); lRetorno.Valido = false; }

                if (lRetorno.Valido)
                {
                    lRetorno.Contratos.push( {   Codigo: lContrato
                                               , Nome:   lContrato
                                               , TCD:    lTCD
                                               , TVD:    lTVD
                                               , LCI:    lLCI
                                               , LVI:    lLVI
                                               , Instrumentos: new Array() 
                                             } );

                    //números válidos, valida somatória
                    lTotalTCD = lTotalTVD = lTotalLCI = lTotalLVI = 0;

                    lTRsInstrumentos = lTRAtual.parent().find("tr[ContratoPai='" + lContrato + "']");

                    lTRsInstrumentos.each(function()
                    {
                        lTRInstrumentoAtual = $(this);

                        _txtTCD = lTRInstrumentoAtual.find("input.txtCliente_Limite_TCD");
                        _txtTVD = lTRInstrumentoAtual.find("input.txtCliente_Limite_TVD");
                        _txtLCI = lTRInstrumentoAtual.find("input.txtCliente_Limite_LCI");
                        _txtLVI = lTRInstrumentoAtual.find("input.txtCliente_Limite_LVI");
                    
                        if (_txtTCD.val() == "") _txtTCD.val("0");
                        if (_txtTVD.val() == "") _txtTVD.val("0");
                        if (_txtLCI.val() == "") _txtLCI.val("0");
                        if (_txtLVI.val() == "") _txtLVI.val("0");

                        _lTCD = new Number( _txtTCD.val() );
                        _lTVD = new Number( _txtTVD.val() );
                        _lLCI = new Number( _txtLCI.val() );
                        _lLVI = new Number( _txtLVI.val() );

                        if (isNaN(_lTCD)) { GradIntra_Clientes_Limites_InvalidarTextBox(_txtTCD, "Valor inválido"); lRetorno.Valido = false; }
                        if (isNaN(_lTVD)) { GradIntra_Clientes_Limites_InvalidarTextBox(_txtTVD, "Valor inválido"); lRetorno.Valido = false; }
                        if (isNaN(_lLCI)) { GradIntra_Clientes_Limites_InvalidarTextBox(_txtLCI, "Valor inválido"); lRetorno.Valido = false; }
                        if (isNaN(_lLVI)) { GradIntra_Clientes_Limites_InvalidarTextBox(_txtLVI, "Valor inválido"); lRetorno.Valido = false; }

                        if(lRetorno.Valido)
                        {
                            lTotalTCD += _lTCD;
                            lTotalTVD += _lTVD;
                            lTotalLCI += _lLCI;
                            lTotalLVI += _lLVI;

                            lRetorno.UltimoContrato().Instrumentos.push( { Codigo: lTRInstrumentoAtual.attr("Instrumento")
                                                                         , Nome:   lTRInstrumentoAtual.attr("Instrumento")
                                                                         , TCD: _lTCD
                                                                         , TVD: _lTVD
                                                                         , LCI: _lLCI
                                                                         , LVI: _lLVI
                                                                         });
                        }
                    });
                
                    if(lTotalTCD > lTCD)
                    {
                        //coluna de TCD inválida, limite dos instrmentos somados são maiores que o do contrato
                        GradIntra_Clientes_Limites_InvalidarTextBox(txtTCD, "Somatória dos valores dos títulos é maior que a do Contrato");

                        lRetorno.Valido = false;
                    }

                    if(lTotalTVD > lTVD)
                    {
                        GradIntra_Clientes_Limites_InvalidarTextBox(txtTVD, "Somatória dos valores dos títulos é maior que a do Contrato");

                        lRetorno.Valido = false;
                    }

                    if(lTotalLCI > lLCI)
                    {
                        GradIntra_Clientes_Limites_InvalidarTextBox(txtLCI, "Somatória dos valores dos títulos é maior que a do Contrato");

                        lRetorno.Valido = false;
                    }

                    if(lTotalLVI > lLVI)
                    {
                        GradIntra_Clientes_Limites_InvalidarTextBox(txtLVI, "Somatória dos valores dos títulos é maior que a do Contrato");

                        lRetorno.Valido = false;
                    }
                }
            }
        });
    }

//    if(!lRetorno.Valido)
//    {
//        GradIntra_AtivarTooltips($("#lstCliente_Limites_PorContrato"))
//    }

    return lRetorno;
}

function GradIntra_Clientes_Limites_InvalidarTextBox(pTextBox, pMensagem)
{
    pTextBox.addClass("ValorInvalido").attr("title", pMensagem);
}

function GradIntra_Clientes_Limites_InabilitarDivParametroLimites(pDivParametroLimite)
{
    pDivParametroLimite.find("input[type='checkbox']").prop("checked", false).next("label").removeClass("checked");
    pDivParametroLimite.find(".Mascara_Data").prop("disabled", true);
    pDivParametroLimite.find(".ValorMonetario").prop("disabled", true);
    pDivParametroLimite.find(".ui-datepicker-trigger").prop("disabled", true);
    pDivParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite").prop("disabled", true);
    pDivParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite").prop("disabled", true);
}

function GradIntra_Clientes_Limites_HabilitarDivParametroLimites(pDivParametroLimite)
{
    pDivParametroLimite.find("input[type='checkbox']").prop("checked", true).next("label").addClass("checked");
    pDivParametroLimite.find(".Mascara_Data").prop("disabled", false);
    pDivParametroLimite.find(".ValorMonetario").prop("disabled", false);
    pDivParametroLimite.find(".ui-datepicker-trigger").prop("disabled", false);
    pDivParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite").prop("disabled", false);
    pDivParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite").prop("disabled", false);
}

function pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite_Click(pSender)
{
    if ($(pSender).prop("disabled") == true)
        return false;

    if ($(pSender).html() == "Expirar" && confirm("Realmente deseja expirar este limite?"))
    {
        alert("Para efetivar a expiração clique em 'Salvar Dados'");

        var divParametroLimite = $(pSender).closest(".divParametroLimite");
        GradIntra_Clientes_Limites_InabilitarDivParametroLimites(divParametroLimite);
        divParametroLimite.find("input[type='checkbox']").prop("disabled", true);
        divParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite").prop("disabled", true);
        divParametroLimite.find(".FlagExpirarLimite").val("true");
        divParametroLimite.find(".ValorMonetario").val("");
        divParametroLimite.find(".Mascara_Data").val("");

        $(pSender).html("Cancelar expiração");
        
        $(pSender).prop("disabled", false);
    }
    else if ($(pSender).html() == "Cancelar expiração")
    {
        var divParametroLimite = $(pSender).closest(".divParametroLimite");
        GradIntra_Clientes_Limites_InabilitarDivParametroLimites(divParametroLimite);
        divParametroLimite.find("input[type='checkbox']").prop("disabled", false);
        divParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite").prop("disabled", false);
        divParametroLimite.find("input[type='checkbox']").prop("checked", true).next("label").addClass("checked");
        divParametroLimite.find("input[type='checkbox']").prop("disabled", true);
        divParametroLimite.find(".FlagExpirarLimite").val("");
        divParametroLimite.find(".ValorMonetario").val(divParametroLimite.find(".LimiteCadastrado").val());
        divParametroLimite.find(".Mascara_Data").val(divParametroLimite.find(".DataVencimentoCadastrada").val());
        $(pSender).html("Expirar");

        $(pSender).prop("disabled", false);
    }

    return false;
}

function pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_Click(pSender)
{
    if ($(pSender).prop("disabled") != true)
    {
        var divParametroLimite = $(pSender).closest(".divParametroLimite");

        if ($(pSender).html() == "Renovar")
        {
            ValidaDadosAtualizacaoLimite(pSender);
            GradIntra_Clientes_Limites_InabilitarDivParametroLimites(divParametroLimite);
            divParametroLimite.find(".Mascara_Data").prop("disabled", false);
            divParametroLimite.find(".ValorMonetario").prop("disabled", false);
            divParametroLimite.find("input[type='checkbox']").prop("checked", true).next("label").addClass("checked");
            divParametroLimite.find("input[type='checkbox']").prop("disabled", true); 
            divParametroLimite.find(".ui-datepicker-trigger").prop("disabled", false);
            $(pSender).prop("disabled", false);
            $(pSender).html("Confirmar renovação");
        }
        else if ($(pSender).html() == "Confirmar renovação")
        {
            ValidaDadosAtualizacaoLimite(pSender);
            GradIntra_Clientes_Limites_InabilitarDivParametroLimites(divParametroLimite);
            divParametroLimite.find("input[type='checkbox']").prop("checked", true).next("label").addClass("checked");
            divParametroLimite.find(".pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite").prop("disabled", false);
            divParametroLimite.find("input[type='checkbox']").prop("disabled", false);
            divParametroLimite.find(".ui-datepicker-trigger").prop("disabled", true);
            $(pSender).prop("disabled", false);
            $(pSender).html("Renovar");
        }
    }
    return false;
}

function ValidaDadosAtualizacaoLimite(pSender)
{
    var lDivParametroLimite = $(pSender).closest("div");
    
    if ($(pSender).html() == "Confirmar renovação")
    {
        if (lDivParametroLimite.hasClass("divParametroLimiteOperaAVista")) {
            if (ConvertToDate(lDivParametroLimite.find(".DataVencimentoCadastrada").val()) > ConvertToDate(lDivParametroLimite.find(".Mascara_Data").val())
            || (lDivParametroLimite.find(".ValorMonetario ").val() == "")) {
                lDivParametroLimite.find(".Mascara_Data").val(lDivParametroLimite.find(".DataVencimentoCadastrada").val());
                GradIntra_ExibirMensagem("A", "Não é permitido configurar uma data menor do que a atual para o vencimento.", true);
                lDivParametroLimite.find("FlagRenovar").val("");
            }
            else {
                lDivParametroLimite.find("FlagRenovar").val("true");
            }
        }
        else if (lDivParametroLimite.hasClass("divParametroLimiteOperaAVistaDescoberto")) 
        {
            if (ConvertToDate(lDivParametroLimite.find(".DataVencimentoCadastrada").val()) > ConvertToDate(lDivParametroLimite.find(".Mascara_Data").val())
            || (lDivParametroLimite.find(".ValorMonetario ").val() == "")) {
                lDivParametroLimite.find(".Mascara_Data").val(lDivParametroLimite.find(".DataVencimentoCadastrada").val());
                GradIntra_ExibirMensagem("A", "Não é permitido configurar uma data menor do que a atual para o vencimento.", true);
                lDivParametroLimite.find("FlagRenovar").val("");
            }
            else {
                lDivParametroLimite.find("FlagRenovar").val("true");
            }
        }
        else if (lDivParametroLimite.hasClass("divParametroLimiteOperaOpcao")) 
        {
            if (ConvertToDate(lDivParametroLimite.find(".DataVencimentoCadastrada").val()) > ConvertToDate(lDivParametroLimite.find(".Mascara_Data").val())
            || (lDivParametroLimite.find(".ValorMonetario ").val() == "")) {
                lDivParametroLimite.find(".Mascara_Data").val(lDivParametroLimite.find(".DataVencimentoCadastrada").val());
                GradIntra_ExibirMensagem("A", "Não é permitido configurar uma data menor do que a atual para o vencimento.", true);
                lDivParametroLimite.find("FlagRenovar").val("");
            }
            else {
                lDivParametroLimite.find("FlagRenovar").val("true");
            }
        }
        else if (lDivParametroLimite.hasClass("divParametroLimiteOperaOpcaoDescoberto")) 
        {
            if (ConvertToDate(lDivParametroLimite.find(".DataVencimentoCadastrada").val()) > ConvertToDate(lDivParametroLimite.find(".Mascara_Data").val())
            || (lDivParametroLimite.find(".ValorMonetario ").val() == "")) {
                lDivParametroLimite.find(".Mascara_Data").val(lDivParametroLimite.find(".DataVencimentoCadastrada").val());
                GradIntra_ExibirMensagem("A", "Não é permitido configurar uma data menor do que a atual para o vencimento.", true);
                lDivParametroLimite.find("FlagRenovar").val("");
            }
            else {
                lDivParametroLimite.find("FlagRenovar").val("true");
            }
        }
        else if (lDivParametroLimite.hasClass("divParametroLimiteValorMaximoDaOrdem")) 
        {
            if (ConvertToDate(lDivParametroLimite.find(".DataVencimentoCadastrada").val()) > ConvertToDate(lDivParametroLimite.find(".Mascara_Data").val())
            || (lDivParametroLimite.find(".ValorMonetario ").val() == "")) 
            {
                lDivParametroLimite.find(".Mascara_Data").val(lDivParametroLimite.find(".DataVencimentoCadastrada").val());
                GradIntra_ExibirMensagem("A", "Não é permitido configurar uma data menor do que a atual para o vencimento.", true);
                lDivParametroLimite.find("FlagRenovar").val("");
            }
            else 
            {
                lDivParametroLimite.find("FlagRenovar").val("true");
            }
        }
    }
}

function pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Click(pSender)
{
    if ($(pSender).next("label").hasClass("checked"))
    {
        var divParametroLimite = $(pSender).closest(".divParametroLimite");
        GradIntra_Clientes_Limites_InabilitarDivParametroLimites(divParametroLimite);
        
        if (divParametroLimite.find(".ValorMonetario").val() == ""
        || (divParametroLimite.find(".Mascara_Data").val()   == ""))
        {
            divParametroLimite.find(".ValorMonetario").val("");
            divParametroLimite.find(".Mascara_Data").val(""); 
        }
    }
    else
    {
        var divParametroLimite = $(pSender).closest(".divParametroLimite");
        GradIntra_Clientes_Limites_HabilitarDivParametroLimites(divParametroLimite);
        
        if (divParametroLimite.find(".ValorMonetario").val() == ""
        && (divParametroLimite.find(".Mascara_Data").val()   == ""))
        {
            divParametroLimite.find(".ValorMonetario").prop("disabled", false);
            divParametroLimite.find(".Mascara_Data").prop("disabled", false);
        }
        else
        {
            divParametroLimite.find(".ValorMonetario").prop("disabled", true);
            divParametroLimite.find(".Mascara_Data").prop("disabled", true);
        }
    }
}

function pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Load(pSender)
{
    if (!$(pSender).is(":checked"))
    {
        var divParametroLimite = $(pSender).closest(".divParametroLimite");
        GradIntra_Clientes_Limites_InabilitarDivParametroLimites(divParametroLimite);
    }
    else
    {
        var divParametroLimite = $(pSender).closest(".divParametroLimite");
        GradIntra_Clientes_Limites_HabilitarDivParametroLimites(divParametroLimite);
        
        if (divParametroLimite.find(".ValorMonetario").val() == ""
        && (divParametroLimite.find(".Mascara_Data").val()   == ""))
        {
            divParametroLimite.find(".ValorMonetario").prop("disabled", false);
            divParametroLimite.find(".Mascara_Data").prop("disabled", false);
        }
        else
        {
            divParametroLimite.find(".ValorMonetario").prop("disabled", true);
            divParametroLimite.find(".Mascara_Data").prop("disabled", true);
            $(pSender).prop("disabled", true);
        }
    }

    return false;
}


function btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(pSender)
{
    if($("#divClientes_Relatorios_TextoConfigurarImpressao").is(":visible"))
    {
        $("#divClientes_Relatorios_TextoConfigurarImpressao")
            .hide()
    }
    else
    {
        $("#divClientes_Relatorios_TextoConfigurarImpressao")
            .show()
            .css(
                {
                    left:$(pSender).position().left - 263,
                    top:$(pSender).position().top + 25
                });
    }
    return false;
}

function txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(pSender) 
{
    var lLimite = "";
    var lVencimento = "";

    if ($(pSender).hasClass("ValorMonetario")) 
    {
        lLimite = $(pSender).val();
        lVencimento = $(pSender).closest("div").find(".Mascara_Data").val();
    }
    else 
    {
        lLimite = $(pSender).closest("div").find(".ValorMonetario").val();
        lVencimento = $(pSender).val();
        //$(pSender).focus();
    }

    //if (lLimite != "" && lVencimento != "") 
    if (lLimite != "") 
    {
        $(pSender).closest("div").find(".FlagIncluirLimite").val("true");
    }
    else 
    {
        $(pSender).closest("div").find(".FlagIncluirLimite").val("");
    }

    return false;
}


function txtCliente_Limites_Bovespa_Opcao_LimiteDescoberto_NovoOMS_OnBlur(pSender) 
{
    var lLimite = "";
    var lVencimento = "";

    if ($(pSender).hasClass("ValorMonetario")) 
    {
        lLimite = $(pSender).val();
        lVencimento = $(pSender).closest("div").find(".Mascara_Data").val();
    }
    else 
    {
        lLimite = $(pSender).closest("div").find(".ValorMonetario").val();
        lVencimento = $(pSender).val();
        //$(pSender).focus();
    }

    //if (lLimite != "" && lVencimento != "") 
    if (lLimite != "") 
    {
        $(pSender).closest("div").find(".FlagIncluirLimite").val("true");
    }
    else 
    {
        $(pSender).closest("div").find(".FlagIncluirLimite").val("");
    }

    return false;
}

function txtCliente_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(pSender)
{
    var lLimite = "";
    var lVencimento = "";

    if ($(pSender).hasClass("ValorMonetario"))
    {
        lLimite = $(pSender).val();
        lVencimento = $(pSender).closest("div").find(".Mascara_Data").val();
    }
    else
    {
        lLimite = $(pSender).closest("div").find(".ValorMonetario").val();
        lVencimento = $(pSender).val();
        //$(pSender).focus();
    }

    if (lLimite != "" && lVencimento != "")
    {
        $(pSender).closest("div").find(".FlagIncluirLimite").val("true");
    }
    else
    {
        $(pSender).closest("div").find(".FlagIncluirLimite").val("");
    }

    return false;
}

function GradIntra_Clientes_NovoClientePF_Load()
{
    $("#cboClientes_DadosCompletos_TipoDeDocumento").val("RG");
    $("#cboClientes_DadosCompletos_OrgaoEmissor").val("SSP");
}

function GradIntra_Clientes_Verificar_InfPatrimoniais_OutrosRendimentos() 
{

    if ($("#txtClientes_InfPatrimoniais_TotalOutrosRendimentos").val() == ""
        | $("#txtClientes_InfPatrimoniais_TotalOutrosRendimentos").val().indexOf(',') == 0 
        | eval($("#txtClientes_InfPatrimoniais_TotalOutrosRendimentos").val().split(',')[0]) == 0 
           && eval($("#txtClientes_InfPatrimoniais_TotalOutrosRendimentos").val().split(',')[1]) == 0) 
    {
        $("#txtClientes_InfPatrimoniais_DescricaoOutrosRendimentos").removeClass("validate[required]");
    }
    else 
    {
        $("#txtClientes_InfPatrimoniais_DescricaoOutrosRendimentos").addClass("validate[required]");
    }
}

function GradIntra_Clientes_Restricoes_RestricaoPorAtivos_NovoOMS_Add(Sender) 
{
    var lNivelPermissao = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();
    var lTr = $(".Template_RetricaoPorAtivo_NovoOMS").clone();

    switch (lNivelPermissao) 
    {
        case "C": lNivelPermissao = "Compra"; break;

        case "V": lNivelPermissao = "Venda"; break;

        default: lNivelPermissao = "Ambos"; break;
    }

    $(lTr.find("td")[0]).html($("#txtCliente_Restricoes_RestricaoPorAtivo_NovoOMS").val().toUpperCase());
    $(lTr.find("td")[1]).html(lNivelPermissao);

    $("#txtCliente_Restricoes_RestricaoPorAtivo_NovoOMS").val("");

    lTr.removeClass("Template_RetricaoPorAtivo_NovoOMS");
    lTr.show();

    $(".Template_RetricaoPorAtivo_NovoOMS").parent().append(lTr);

    return false;
}

function GradIntra_Clientes_Restricoes_Spider_RestricaoPorAtivos_Add(Sender)
{
    var lNivelPermissao = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();
    var lTr = $(".Template_RetricaoPorAtivo").clone();

    switch (lNivelPermissao) 
    {
        case "C": lNivelPermissao = "Compra"; break;
        case "V": lNivelPermissao = "Venda"; break;
        default:  lNivelPermissao = "Ambos";  break;
    }

    $(lTr.find("td")[0]).html($("#txtCliente_Restricoes_RestricaoPorAtivo").val().toUpperCase());
    $(lTr.find("td")[1]).html(lNivelPermissao);

    $("#txtCliente_Restricoes_RestricaoPorAtivo").val("");

    lTr.removeClass("Template_RetricaoPorAtivo");
    lTr.show();

    $(".Template_RetricaoPorAtivo").parent().append(lTr);
 
    return false;
}

function GradIntra_Clientes_Restricoes_RestricaoPorAtivos_Add(Sender)
{
    var lNivelPermissao = $(".RadiosRestricaoPorAtivos").find("input[type='radio']:checked").val();
    var lTr = $(".Template_RetricaoPorAtivo").clone();

    switch (lNivelPermissao) 
    {
        case "C": lNivelPermissao = "Compra"; break;
        case "V": lNivelPermissao = "Venda"; break;
        default:  lNivelPermissao = "Ambos";  break;
    }

    $(lTr.find("td")[0]).html($("#txtCliente_Restricoes_RestricaoPorAtivo").val().toUpperCase());
    $(lTr.find("td")[1]).html(lNivelPermissao);

    $("#txtCliente_Restricoes_RestricaoPorAtivo").val("");

    lTr.removeClass("Template_RetricaoPorAtivo");
    lTr.show();

    $(".Template_RetricaoPorAtivo").parent().append(lTr);
 
    return false;
}

function GradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_Spider_Click(Sender) 
{
    var lNomePapel = $($(Sender).closest("tr").find("td")[0]).html();

    if (confirm('Realmente deseja remover o papel \'' + lNomePapel + '\'?\n\nPara efetivar a alteração clique em \'Salvar Dados\'.'))
        $(Sender).closest("tr").remove();

    return false;
}

function GradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_click(Sender) 
{
    var lNomePapel = $($(Sender).closest("tr").find("td")[0]).html();

    if (confirm('Realmente deseja remover o papel \'' + lNomePapel + '\'?\n\nPara efetivar a alteração clique em \'Salvar Dados\'.'))
        $(Sender).closest("tr").remove();

    return false;
}

function GradIntra_Clientes_Restricoes_RestricaoPorAtivos_NovoOMS_Remover_Click(Sender)
{
    var lNomePapel = $($(Sender).closest("tr").find("td")[0]).html();

    if (confirm('Realmente deseja remover o papel \'' + lNomePapel + '\'?\n\nPara efetivar a alteração clique em \'Salvar Dados\'.'))
        $(Sender).closest("tr").remove();
    
    return false;
}

function btnCliente_Risco_Restricoes_Click(Sender) 
{
    //--> Recuperando os dados dos grupos.

    var lListaGruposRestritos = new Array()

    $(".ClienteRestricaoClienteParametroGrupo").each(function () 
    {
        var lGruposSelecionados = { 
                                        IdCliente  : 0
                                        , IdParametro: 0
                                        , ListaGrupos: []
                                    };

        lGruposSelecionados.IdCliente = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

        $(this)
               .find(".Cliente_ResticoesPorGrupoRestringe:checked")
               .each(function () {
                   lGruposSelecionados.IdParametro = $(this).closest("p").find(".Cliente_ResticoesPorGrupoParametro").val();
                   lGruposSelecionados.ListaGrupos.push($(this).val());
               });

        lListaGruposRestritos.push(lGruposSelecionados);
    });

    //--> Recuperando os dados dos ativos.

    var lListaAtivosRestritos = { Ativos: [], Direcoes: [] }

    $(".Clientes_Restricoes_RestricaoPorAtivos")
        .find("tr")
        .each(function () {
            if ($($(this).find("td")[0]).html() != "") {
                lListaAtivosRestritos.Ativos.push($($(this).find("td")[0]).html());
                lListaAtivosRestritos.Direcoes.push($($(this).find("td")[1]).html());
            }
        });

    //--><--\\

    var lUrl = "Risco/Formularios/Dados/ManutencaoRestricoes.aspx";

    var ObjetoJSonAtivosRestritos = $.toJSON(lListaAtivosRestritos);
    var ObjetoJSonGruposRestritos = $.toJSON(lListaGruposRestritos);

    var lDados =
        {
            acao: "SalvarDadosPermissoes",
            ObjetoJSonAtivos: ObjetoJSonAtivosRestritos,
            ObjetoJSonGrupos: ObjetoJSonGruposRestritos,
            Id: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        Cliente_Restricoes_Callback
    );

    return false;
}


function btnClientes_Atualizar_Dados_Spider_Limites_BMF_Click(Sender)
{
    $("#cmbClientes_Spider_Instrumentos_Selecionados").find("option").remove();
    $("#cmbCliente_Spider_Indices_Bmf").find("option:selected").removeAttr("selected");
        
    $("#txtCliente_Spider_Qtde_Compra").val("");
    $("#txtCliente_Spider_Qtde_Venda").val("");
    $("#txtCliente_Spider_Data_validade").val("");
    $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val("");
    $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val("");
    $("#txtCliente_Spider_Instrumento").val("");
    $("#txtCliente_Spider_Qtde_Compra_Instrumento").val("");
    $("#txtCliente_Spider_Qtde_Venda_Instrumento").val("");
        
    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

    var lDados =
    {
        acao                             : "AtualizarLimitesBmf",
        CodBmf                           : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
    };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        btnClientes_Atualizar_Dados_Spider_Limites_BMF_Click_CallBack
    );

    return false;
}

function btnClientes_Atualizar_Dados_Spider_Limites_BMF_Click_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("A", "Dados de Limite BMF atualizados com sucesso.");
}

function btnClientes_Atualizar_Dados_Limites_BMF_Click(Sender)
{
    $("#cmbClientes_Instrumentos_Selecionados").find("option").remove();
    $("#cmbCliente_Indices_Bmf").find("option:selected").removeAttr("selected");
        
    $("#txtCliente_Qtde_Compra").val("");
    $("#txtCliente_Qtde_Venda").val("");
    $("#txtCliente_Data_validade").val("");
    $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val("");
    $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val("");
    $("#txtCliente_Instrumento").val("");
    $("#txtCliente_Qtde_Compra_Instrumento").val("");
    $("#txtCliente_Qtde_Venda_Instrumento").val("");
        
    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesBMF.aspx";

    var lDados =
    {
        acao                             : "AtualizarLimitesBmf",
        CodBmf                           : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
    };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        btnClientes_Atualizar_Dados_Limites_BMF_Click_CallBack
    );

    return false;
}

function btnClientes_Atualizar_Dados_Limites_BMF_Click_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("A", "Dados de Limite BMF atualizados com sucesso.");
}
/*
function btnClientes_Salvar_Spider_Limites_BMF_Valida_Valores()
{
    var lCliente_Qtde_Compra             = parseInt($("#txtCliente_Spider_Qtde_Compra").val());
    var lCliente_Qtde_Venda              = parseInt($("#txtCliente_Spider_Qtde_Venda").val());
    var lCliente_Qtde_Venda_Instrumento  = parseInt($("#txtCliente_Spider_Qtde_Venda_Instrumento").val());
    var lCliente_Qtde_Compra_Instrumento = parseInt($("#txtCliente_Spider_Qtde_Compra_Instrumento").val());
    
    if (isNaN(lCliente_Qtde_Compra) || isNaN(lCliente_Qtde_Venda) || isNaN(lCliente_Qtde_Compra_Instrumento) || isNaN(lCliente_Qtde_Venda_Instrumento))
    {
        alert("Campo de quantidade inválida. Preencha os campos de quantidades de venda e compra de contrato e instrumento.");
        return false;
    }

    if (lCliente_Qtde_Compra == 0)
    {
         alert("Quantidade de compra de contrato é inválida.");
         return false;
    }

    if (lCliente_Qtde_Venda == 0)
    {
         alert("Quantidade de venda de contrato é inválida.");
         return false;
    }

    if (lCliente_Qtde_Compra_Instrumento > lCliente_Qtde_Compra)
    {
        alert("Quantidade de compra de instrumento é inválida. É necessário inserir uma quantidade menor que quantidade de compra contrato");
        return false;
    }

    if (lCliente_Qtde_Venda_Instrumento > lCliente_Qtde_Venda)
    {
        alert("Quantidade de venda de instrumento é inválida. É necessário inserir uma quantidade menor que quantidade de venda de contrato");
        return false;
    }

    return true;
}
*/

function btnClientes_Salvar_Spider_Limites_BMF_Valida_Valores()
{
    var lCliente_Qtde_Compra             = parseInt($("#txtCliente_Spider_Qtde_Compra").val());
    var lCliente_Qtde_Venda              = parseInt($("#txtCliente_Spider_Qtde_Venda").val());
    var lCliente_Qtde_Venda_Instrumento  = parseInt($("#txtCliente_Spider_Qtde_Venda_Instrumento").val());
    var lCliente_Qtde_Compra_Instrumento = parseInt($("#txtCliente_Spider_Qtde_Compra_Instrumento").val());
    
    var lCodBmf = gGradIntra_Cadastro_ItemSendoEditado.CodBMF;

    if (lCodBmf== undefined || lCodBmf == 0 || lCodBmf == "0")
    {
        alert("Cliente sem código bmf para incluir Limites bmf");
        return false;
    }

    if (isNaN(lCliente_Qtde_Compra) || 
    isNaN(lCliente_Qtde_Venda)      //|| 
    //isNaN(lCliente_Qtde_Compra_Instrumento) || 
    //isNaN(lCliente_Qtde_Venda_Instrumento)
    )
    {
        alert("Campo de quantidade inválida. Preencha os campos de quantidades de venda e compra de contrato e instrumento.");
        return false;
    }

    if (lCliente_Qtde_Compra == 0)
    {
         alert("Quantidade de compra de contrato é inválida.");
         return false;
    }

    if (lCliente_Qtde_Venda == 0)
    {
         alert("Quantidade de venda de contrato é inválida.");
         return false;
    }

    if (lCliente_Qtde_Compra_Instrumento > lCliente_Qtde_Compra)
    {
        alert("Quantidade de compra de instrumento é inválida. É necessário inserir uma quantidade menor que quantidade de compra contrato");
        return false;
    }

    if (lCliente_Qtde_Venda_Instrumento > lCliente_Qtde_Venda)
    {
        alert("Quantidade de venda de instrumento é inválida. É necessário inserir uma quantidade menor que quantidade de venda de contrato");
        return false;
    }

    return true;
}

function btnClientes_Salvar_Limites_BMF_Valida_Valores()
{
    var lCliente_Qtde_Compra             = parseInt($("#txtCliente_Qtde_Compra").val());
    var lCliente_Qtde_Venda              = parseInt($("#txtCliente_Qtde_Venda").val());
    var lCliente_Qtde_Venda_Instrumento  = parseInt($("#txtCliente_Qtde_Venda_Instrumento").val());
    var lCliente_Qtde_Compra_Instrumento = parseInt($("#txtCliente_Qtde_Compra_Instrumento").val());

    var lCodBmf = gGradIntra_Cadastro_ItemSendoEditado.CodBMF;

    if (lCodBmf== undefined || lCodBmf == 0 || lCodBmf == "0")
    {
        alert("Cliente sem código bmf para incluir Limites bmf");
        return false;
    }
    
    if (isNaN(lCliente_Qtde_Compra) || isNaN(lCliente_Qtde_Venda) || isNaN(lCliente_Qtde_Compra_Instrumento) || isNaN(lCliente_Qtde_Venda_Instrumento))
    {
        alert("Campo de quantidade inválida. Preencha os campos de quantidades de venda e compra de contrato e instrumento.");
        return false;
    }

    if (lCliente_Qtde_Compra == 0)
    {
         alert("Quantidade de compra de contrato é inválida.");
         return false;
    }

    if (lCliente_Qtde_Venda == 0)
    {
         alert("Quantidade de venda de contrato é inválida.");
         return false;
    }

    if (lCliente_Qtde_Compra_Instrumento > lCliente_Qtde_Compra)
    {
        alert("Quantidade de compra de instrumento é inválida. É necessário inserir uma quantidade menor que quantidade de compra contrato");
        return false;
    }

    if (lCliente_Qtde_Venda_Instrumento > lCliente_Qtde_Venda)
    {
        alert("Quantidade de venda de instrumento é inválida. É necessário inserir uma quantidade menor que quantidade de venda de contrato");
        return false;
    }

    return true;
}

function btnClientes_Salvar_Spider_Limites_BMF_Click(Sender) 
{
    if (!btnClientes_Salvar_Spider_Limites_BMF_Valida_Valores())
    {
        return false;
    }

    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

    var lDados =
        {
            acao                             : "SalvarLimitesBmf",
            CodBmf                           : gGradIntra_Cadastro_ItemSendoEditado.CodBMF,
            
            Contrato                         : $("#cmbCliente_Spider_Indices_Bmf").val(),
            lCliente_Qtde_Compra             : $("#txtCliente_Spider_Qtde_Compra").val(),
            lCliente_Qtde_Venda              : $("#txtCliente_Spider_Qtde_Venda").val(),
            lCliente_Qtde_Maxima_Contrato    : $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val(),
            lCliente_Data_validade           : $("#txtCliente_Spider_Data_validade").val(),
            instrumento                      : $("#cmbClientes_Spider_Instrumentos_Selecionados").val(),
            lCliente_Qtde_Compra_Instrumento : $("#txtCliente_Spider_Qtde_Compra_Instrumento").val(),
            lCliente_Qtde_Venda_Instrumento  : $("#txtCliente_Spider_Qtde_Venda_Instrumento").val(),
            lCliente_Qtde_Maxima_Instrumento : $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val(),
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        btnClientes_Salvar_Spider_Limites_BMF_Click_CallBack
    );

    return false;
}

function btnClientes_Salvar_Spider_Limites_BMF_Click_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        $("#cmbClientes_Spider_Instrumentos_Selecionados").find("option").remove();
        $("#cmbCliente_Spider_Indices_Bmf").find("option:selected").removeAttr("selected");
        
        $("#txtCliente_Spider_Qtde_Compra").val("");
        $("#txtCliente_Spider_Qtde_Venda").val("");
        $("#txtCliente_Spider_Data_validade").val("");
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val("");
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val("");
        $("#txtCliente_Spider_Instrumento").val("");
        $("#txtCliente_Spider_Qtde_Compra_Instrumento").val("");
        $("#txtCliente_Spider_Qtde_Venda_Instrumento").val("");

        GradIntra_ExibirMensagem("A", "Dados de Limite BMF salvos com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
    }
}

function btnClientes_Salvar_Limites_BMF_Click(Sender) 
{
    if (!btnClientes_Salvar_Limites_BMF_Valida_Valores())
    {
        return false;
    }

    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesBMF.aspx";

    var lDados =
        {
            acao                             : "SalvarLimitesBmf",
            CodBmf                           : gGradIntra_Cadastro_ItemSendoEditado.CodBMF,
            
            Contrato                         : $("#cmbCliente_Indices_Bmf").val(),
            lCliente_Qtde_Compra             : $("#txtCliente_Qtde_Compra").val(),
            lCliente_Qtde_Venda              : $("#txtCliente_Qtde_Venda").val(),
            lCliente_Qtde_Maxima_Contrato    : $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val(),

            instrumento                      : $("#cmbClientes_Instrumentos_Selecionados").val(),
            lCliente_Qtde_Compra_Instrumento : $("#txtCliente_Qtde_Compra_Instrumento").val(),
            lCliente_Qtde_Venda_Instrumento  : $("#txtCliente_Qtde_Venda_Instrumento").val(),
            lCliente_Qtde_Maxima_Instrumento : $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val(),
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        btnClientes_Salvar_Limites_BMF_Click_CallBack
    );

    return false;
}
function btnClientes_Salvar_Limites_BMF_Click_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        $("#cmbClientes_Instrumentos_Selecionados").find("option").remove();
        $("#cmbCliente_Indices_Bmf").find("option:selected").removeAttr("selected");
        
        $("#txtCliente_Qtde_Compra").val("");
        $("#txtCliente_Qtde_Venda").val("");
        $("#txtCliente_Data_validade").val("");
        $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val("");
        $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val("");
        $("#txtCliente_Instrumento").val("");
        $("#txtCliente_Qtde_Compra_Instrumento").val("");
        $("#txtCliente_Qtde_Venda_Instrumento").val("");
        GradIntra_ExibirMensagem("A", "Dados de Limite BMF salvos com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
    }

    
}

function btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click(pSender) 
{
    if (!btnClientes_Salvar_Spider_Limites_BMF_Valida_Valores())
    {
        return false;
    }

    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

    if ($("#cmbCliente_Spider_Indices_Bmf").val() == null) 
    {
        alert("É necessário selecionar um contrato.");
        return false;
    }

    if ($("#txtCliente_Spider_Qtde_Compra").val() == "0" || $("#txtCliente_Spider_Qtde_Compra").val() == "" ||
        $("#txtCliente_Spider_Qtde_Venda").val() == "0" ||  $("#txtCliente_Spider_Qtde_Venda").val() == "" ) 
    {
        alert("É necessário inserir valores de compra e venda.");
        return false;
    }

    if ($("#txtCliente_Spider_Data_validade").val() == "") 
    {
        alert("É necessário inserir a data de validade.");
        return false;
    }

    if ($("#txtCliente_Spider_Instrumento").val() == "0" || $("#txtCliente_Spider_Instrumento").val() == "") 
    {
        alert("É necessário inserir o instrumento.");
        return false;
    }

    //$("#txtCliente_Qtde_Maxima_Ordem_Contrato").val() == "0" || $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val() == "" ||
    if ($("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val() == "0" || 
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val() == "") 
    {
        alert("É necessário inserir valores de Quantidade Máxima da orde de Instrumento.");
        return false;
    }

    var lDados =
        {
            acao                             : "InsereIntrumentoLimitesBmf",
            CodBmf                           : gGradIntra_Cadastro_ItemSendoEditado.CodBMF,
            lCliente_Contrato                : $("#cmbCliente_Spider_Indices_Bmf").val(),
            lCliente_Qtde_Compra             : $("#txtCliente_Spider_Qtde_Compra").val(),
            lCliente_Qtde_Venda              : $("#txtCliente_Spider_Qtde_Venda").val(),
            lCliente_Data_validade           : $("#txtCliente_Spider_Data_validade").val(),
            lCliente_Qtde_Maxima_Contrato    : $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val(),
            lCliente_Qtde_Maxima_Instrumento : $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val(),
            lCliente_Instrumento             : $("#txtCliente_Spider_Instrumento").val(),
            lCliente_Qtde_Compra_Instrumento : $("#txtCliente_Spider_Qtde_Compra_Instrumento").val(),
            lCliente_Qtde_Venda_Instrumento  : $("#txtCliente_Spider_Qtde_Venda_Instrumento").val(),
        };

        GradIntra_CarregarJsonVerificandoErro
        (
            lUrl,
            lDados,
            btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click_CallBack
        );

    return false;
}

function btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#cmbClientes_Spider_Instrumentos_Selecionados").find("option").remove();

        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
        {
            $("#cmbClientes_Spider_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao,
             LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
        }

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
    }
}

//function btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click(pSender) 
//{
//    if (!btnClientes_Salvar_Limites_BMF_Valida_Valores())
//    {
//        return false;
//    }

//    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

//    if ($("#cmbCliente_Spider_Indices_Bmf").val() == null) 
//    {
//        alert("É necessário selecionar um contrato.");
//        return false;
//    }

//    if ($("#txtCliente_Spider_Qtde_Compra").val() == "0" || $("#txtCliente_Spider_Qtde_Compra").val() == "" ||
//        $("#txtCliente_Spider_Qtde_Venda").val() == "0" ||  $("#txtCliente_Spider_Qtde_Venda").val() == "" ) 
//    {
//        alert("É necessário inserir valores de compra e venda.");
//        return false;
//    }

//    if ($("#txtCliente_Spider_Data_validade").val() == "") 
//    {
//        alert("É necessário inserir a data de validade.");
//        return false;
//    }

//    if ($("#txtCliente_Spider_Instrumento").val() == "0" || $("#txtCliente_Spider_Instrumento").val() == "") 
//    {
//        alert("É necessário inserir o instrumento.");
//        return false;
//    }

//    //$("#txtCliente_Qtde_Maxima_Ordem_Contrato").val() == "0" || $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val() == "" ||
//    if ($("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val() == "0" || $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val() == "") 
//    {
//        alert("É necessário inserir valores de Quantidade Máxima da orde de Instrumento.");
//        return false;
//    }
//   
//    var lDados =
//        {
//            acao                                    : "InsereIntrumentoLimitesBmf",
//            CodBmf                                  : gGradIntra_Cadastro_ItemSendoEditado.CodBMF,
//            lCliente_Contrato                       : $("#cmbCliente_Spider_Indices_Bmf").val(),
//            lCliente_Qtde_Compra                    : $("#txtCliente_Spider_Qtde_Compra").val(),
//            lCliente_Qtde_Venda                     : $("#txtCliente_Spider_Qtde_Venda").val(),
//            lCliente_Data_validade                  : $("#txtCliente_Spider_Data_validade").val(),
//            lCliente_Qtde_Maxima_Contrato           : $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val(),
//            lCliente_Qtde_Maxima_Instrumento        : $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val(),
//            lCliente_Instrumento                    : $("#txtCliente_Spider_Instrumento").val(),
//            lCliente_Qtde_Compra_Instrumento        : $("#txtCliente_Spider_Qtde_Compra_Instrumento").val(),
//            lCliente_Qtde_Venda_Instrumento         : $("#txtCliente_Spider_Qtde_Venda_Instrumento").val(),
//        };

//        GradIntra_CarregarJsonVerificandoErro
//        (
//            lUrl,
//            lDados,
//            btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click_CallBack
//        );

//    return false;
//}

function btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#cmbClientes_Spider_Instrumentos_Selecionados").find("option").remove();

        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
        {
            $("#cmbClientes_Spider_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao, LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
        }

        //$("#txtCliente_Qtde_Compra").val("");
        //$("#txtCliente_Qtde_Venda").val("");
        //$("#txtCliente_Data_validade").val("");
        //$("#txtCliente_Qtde_Maxima_Ordem_Contrato").val("");
        //$("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val("");
        //$("#txtCliente_Instrumento").val("");
        //$("#txtCliente_Qtde_Venda_Instrumento").val("");
        //$("#txtCliente_Qtde_Compra_Instrumento").val("");

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
    }
}


function btnClientes_Limites_BMF_Adiciona_Instrumento_Click(pSender) 
{
    if (!btnClientes_Salvar_Limites_BMF_Valida_Valores())
    {
        return false;
    }

    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesBMF.aspx";

    if ($("#cmbCliente_Indices_Bmf").val() == null) 
    {
        alert("É necessário selecionar um contrato.");
        return false;
    }

    if ($("#txtCliente_Qtde_Compra").val() == "0" || $("#txtCliente_Qtde_Compra").val() == "" ||
        $("#txtCliente_Qtde_Venda").val() == "0" || $("#txtCliente_Qtde_Venda").val() == "" ) 
    {
        alert("É necessário inserir valores de compra e venda.");
        return false;
    }

    if ($("#txtCliente_Data_validade").val() == "") 
    {
        alert("É necessário inserir a data de validade.");
        return false;
    }

    if ($("#txtCliente_Instrumento").val() == "0" || $("#txtCliente_Instrumento").val() == "") 
    {
        alert("É necessário inserir o instrumento.");
        return false;
    }

    //$("#txtCliente_Qtde_Maxima_Ordem_Contrato").val() == "0" || $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val() == "" ||
    if ($("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val() == "0" || $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val() == "") {
        alert("É necessário inserir valores de Quantidade Máxima da orde de Instrumento.");
        return false;
    }
   

    var lDados =
        {
            acao                                    : "InsereIntrumentoLimitesBmf",
            CodBmf                                  : gGradIntra_Cadastro_ItemSendoEditado.CodBMF,
            lCliente_Contrato                       : $("#cmbCliente_Indices_Bmf").val(),
            lCliente_Qtde_Compra                    : $("#txtCliente_Qtde_Compra").val(),
            lCliente_Qtde_Venda                     : $("#txtCliente_Qtde_Venda").val(),
            lCliente_Data_validade                  : $("#txtCliente_Data_validade").val(),
            lCliente_Qtde_Maxima_Contrato           : $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val(),
            lCliente_Qtde_Maxima_Instrumento        : $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val(),
            lCliente_Instrumento                    : $("#txtCliente_Instrumento").val(),
            lCliente_Qtde_Compra_Instrumento        : $("#txtCliente_Qtde_Compra_Instrumento").val(),
            lCliente_Qtde_Venda_Instrumento         : $("#txtCliente_Qtde_Venda_Instrumento").val(),
        };

        GradIntra_CarregarJsonVerificandoErro
        (
            lUrl,
            lDados,
            btnClientes_Limites_BMF_Adiciona_Instrumento_Click_CallBack
        );

    return false;
}

function btnClientes_Limites_BMF_Adiciona_Instrumento_Click_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#cmbClientes_Instrumentos_Selecionados").find("option").remove();

        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
        {
            $("#cmbClientes_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao,
             LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
        }

        //$("#txtCliente_Qtde_Compra").val("");
        //$("#txtCliente_Qtde_Venda").val("");
        //$("#txtCliente_Data_validade").val("");
        //$("#txtCliente_Qtde_Maxima_Ordem_Contrato").val("");
        //$("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val("");
        //$("#txtCliente_Instrumento").val("");
        //$("#txtCliente_Qtde_Venda_Instrumento").val("");
        //$("#txtCliente_Qtde_Compra_Instrumento").val("");

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
    }
}

function btnClientes_Limites_BMF_Remove_Instrumento_Click(pSender)
{
    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesBMF.aspx";

    if ($("#cmbCliente_Indices_Bmf").val() == "")
    {
        alert("Selecione um Instrumento para remover");
        return false;
    }

    var lDados =
        {
            acao        : "RemoveIntrumentoLimitesBmf",
            Instrumento : $("#cmbClientes_Instrumentos_Selecionados").val(),
            CodBmf      :gGradIntra_Cadastro_ItemSendoEditado.CodBMF,
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        btnClientes_Limites_BMF_Remove_Instrumento_Click_CallBack
    );

    return false;
}

function btnClientes_Limites_BMF_Remove_Instrumento_Click_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var lInstrumento = pResposta.ObjetoDeRetorno;

        $("#cmbClientes_Instrumentos_Selecionados").find("option").remove();

//        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
//        {
//            $("#cmbClientes_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao,
//             LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
//        }

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem("I", pResposta.Mensagem);
    }
}

function cmbCliente_Spider_Instrumentos_Bmf_OnChange(pSender)
{
    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

    var lInstrumento = $("#cmbClientes_Spider_Instrumentos_Selecionados").val();
    var lContrato    = $("#cmbCliente_Spider_Indices_Bmf").val();

    var lDados =
        {
            acao: "SelecionaIntrumentoLimitesBmf",
            Contrato    : lContrato,
            instrumento : lInstrumento,
            CodBmf      : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
        };
    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        cmbCliente_Spider_Instrumentos_Bmf_OnChange_CallBack
    ); 

    return false;
}

function cmbCliente_Spider_Instrumentos_Bmf_OnChange_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#txtCliente_Spider_Qtde_Compra").val(LimiteBMF.QtdeCompraDisponivel);
        $("#txtCliente_Spider_Qtde_Venda").val(LimiteBMF.QtdeVendaDisponivel);
        $("#txtCliente_Spider_Data_validade").val(LimiteBMF.DtValidade);
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val(LimiteBMF.QtdeMaximaOrdemInstrumento);
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val(LimiteBMF.QtdeMaximaOrdemContrato);
        $("#txtCliente_Spider_Qtde_Venda_Instrumento").val(LimiteBMF.QtdeVendaInstrumentoDisponivel);
        $("#txtCliente_Spider_Qtde_Compra_Instrumento").val(LimiteBMF.QtdeCompraInstrumentoDisponivel);
       
        GradIntra_ExibirMensagem("A", "Dados carregados com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", "Este cliente não possui.");
    }
}

function cmbCliente_Spider_Instrumentos_Bmf_OnChange(pSender)
{
    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

    var lInstrumento = $("#cmbClientes_Spider_Instrumentos_Selecionados").val();
    var lContrato    = $("#cmbCliente_Spider_Indices_Bmf").val();

    var lDados =
        {
            acao        : "SelecionaIntrumentoLimitesBmf",
            Contrato    : lContrato,
            instrumento : lInstrumento,
            CodBmf      : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        cmbCliente_Spider_Instrumentos_Bmf_OnChange_CallBack
    ); 

    return false;
}

function cmbCliente_Spider_Instrumentos_Bmf_OnChange_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#txtCliente_Spider_Qtde_Compra")                  .val(LimiteBMF.QtdeCompraDisponivel);
        $("#txtCliente_Spider_Qtde_Venda")                   .val(LimiteBMF.QtdeVendaDisponivel);
        $("#txtCliente_Spider_Data_validade")                .val(LimiteBMF.DtValidade);
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val(LimiteBMF.QtdeMaximaOrdemInstrumento);
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato")   .val(LimiteBMF.QtdeMaximaOrdemContrato);
        $("#txtCliente_Spider_Qtde_Venda_Instrumento")       .val(LimiteBMF.QtdeVendaInstrumentoDisponivel);
        $("#txtCliente_Spider_Qtde_Compra_Instrumento")      .val(LimiteBMF.QtdeCompraInstrumentoDisponivel);

        GradIntra_ExibirMensagem("A", "Dados carregados com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", "Este cliente não possui.");
    }
}


function cmbCliente_Instrumentos_Bmf_OnChange(pSender)
{
    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesBMF.aspx";

    var lInstrumento = $("#cmbClientes_Instrumentos_Selecionados").val();
    var lContrato    = $("#cmbCliente_Indices_Bmf").val();

    var lDados =
        {
            acao: "SelecionaIntrumentoLimitesBmf",
            Contrato    : lContrato,
            instrumento : lInstrumento,
            CodBmf      : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
        };
    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        cmbCliente_Instrumentos_Bmf_OnChange_CallBack
    ); 

    return false;
}

function cmbCliente_Instrumentos_Bmf_OnChange_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#txtCliente_Qtde_Compra").val(LimiteBMF.QtdeCompraDisponivel);
        $("#txtCliente_Qtde_Venda").val(LimiteBMF.QtdeVendaDisponivel);
        $("#txtCliente_Data_validade").val(LimiteBMF.DtValidade);
        $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val(LimiteBMF.QtdeMaximaOrdemInstrumento);
        $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val(LimiteBMF.QtdeMaximaOrdemContrato);
        $("#txtCliente_Qtde_Venda_Instrumento").val(LimiteBMF.QtdeVendaInstrumentoDisponivel);
        $("#txtCliente_Qtde_Compra_Instrumento").val(LimiteBMF.QtdeCompraInstrumentoDisponivel);


//        $("#cmbClientes_Instrumentos_Selecionados").find("option").remove();

//        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
//        {
//            $("#cmbClientes_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao,
//             LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
//        }
        
        GradIntra_ExibirMensagem("A", "Dados carregados com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", "Este cliente não possui.");
    }
}

function cmbCliente_Spider_Indices_Bmf_OnChange(pSender)
{
    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBMF.aspx";

    var lContrato = $("#cmbCliente_Spider_Indices_Bmf").val();

    var lDados =
        {
            acao: "SelecionaContratoLimitesBmf",
            Contrato : lContrato,
            CodBmf : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
        };
    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        cmbCliente_Spider_Indices_Bmf_OnChange_CallBack
    ); 

    return false;
}

function cmbCliente_Spider_Indices_Bmf_OnChange_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#txtCliente_Spider_Qtde_Compra").val(LimiteBMF.QtdeCompra);
        $("#txtCliente_Spider_Qtde_Venda").val(LimiteBMF.QtdeVenda);
        $("#txtCliente_Spider_Data_validade").val(LimiteBMF.DtValidade);
        //$("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val(LimiteBMF.QtdeMaximaOrdemInstrumento);
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Contrato").val(LimiteBMF.QtdeMaximaOrdemContrato);
        $("#txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento").val("");
        $("#txtCliente_Spider_Qtde_Compra_Instrumento").val("");
        $("#txtCliente_Spider_Qtde_Venda_Instrumento").val("");
        
        
        $("#cmbClientes_Spider_Instrumentos_Selecionados").find("option").remove();

        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
        {
            $("#cmbClientes_Spider_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao,
             LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
        }
        
        GradIntra_ExibirMensagem("A", "Dados carregados com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", "Este cliente não possui.");
    }
}

function cmbCliente_Indices_Bmf_OnChange(pSender)
{
    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesBMF.aspx";

    var lContrato = $("#cmbCliente_Indices_Bmf").val();

    var lDados =
        {
            acao: "SelecionaContratoLimitesBmf",
            Contrato : lContrato,
            CodBmf : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
        };
    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        cmbCliente_Indices_Bmf_OnChange_CallBack
    ); 

    return false;
}

function cmbCliente_Indices_Bmf_OnChange_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        var LimiteBMF = pResposta.ObjetoDeRetorno;

        $("#txtCliente_Qtde_Compra").val(LimiteBMF.QtdeCompra);
        $("#txtCliente_Qtde_Venda").val(LimiteBMF.QtdeVenda);
        $("#txtCliente_Data_validade").val(LimiteBMF.DtValidade);
        //$("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val(LimiteBMF.QtdeMaximaOrdemInstrumento);
        $("#txtCliente_Qtde_Maxima_Ordem_Contrato").val(LimiteBMF.QtdeMaximaOrdemContrato);
        $("#txtCliente_Qtde_Maxima_Ordem_Instrumento").val("");
        $("#txtCliente_Qtde_Compra_Instrumento").val("");
        $("#txtCliente_Qtde_Venda_Instrumento").val("");
        
        
        $("#cmbClientes_Instrumentos_Selecionados").find("option").remove();

        for(i = 0; i < LimiteBMF.InstrumentosSelecionados.length ;i++)
        {
            $("#cmbClientes_Instrumentos_Selecionados").append(new Option(LimiteBMF.InstrumentosSelecionados[i].Descricao,
             LimiteBMF.InstrumentosSelecionados[i].Id, true, true));
        }
        
        GradIntra_ExibirMensagem("A", "Dados carregados com sucesso");
    }
    else
    {
        GradIntra_ExibirMensagem("I", "Este cliente não possui.");
    }
}

function btnCliente_Restricoes_Spider_Click(Sender) 
{
    //--> Recuperando os dados dos grupos.

    var lListaGruposRestritos = new Array()

    $(".ClienteRestricaoClienteParametroGrupo").each(function () {
        var lGruposSelecionados = { IdCliente: 0
                                  , IdParametro: 0
                                  , ListaGrupos: []
        };

        lGruposSelecionados.IdCliente = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

        $(this)
               .find(".Cliente_ResticoesPorGrupoRestringe:checked")
               .each(function () {
                   lGruposSelecionados.IdParametro = $(this).closest("p").find(".Cliente_ResticoesPorGrupoParametro").val();
                   lGruposSelecionados.ListaGrupos.push($(this).val());
               });

        lListaGruposRestritos.push(lGruposSelecionados);
    });

    //--> Recuperando os dados dos ativos.

    var lListaAtivosRestritos = { Ativos: [], Direcoes: [] }

    $(".Clientes_Restricoes_RestricaoPorAtivosSpider")
        .find("tr")
        .each(function () {
            if ($($(this).find("td")[0]).html() != "") {
                lListaAtivosRestritos.Ativos.push($($(this).find("td")[0]).html());
                lListaAtivosRestritos.Direcoes.push($($(this).find("td")[1]).html());
            }
        });

    //--><--\\

    var lUrl = "Clientes/Formularios/Acoes/SpiderLimitesBVSP.aspx";

    var ObjetoJSonAtivosRestritos = $.toJSON(lListaAtivosRestritos);
    var ObjetoJSonGruposRestritos = $.toJSON(lListaGruposRestritos);

    var lDados =
        {
            acao: "SalvarDadosPermissoes",
            ObjetoJSonAtivos: ObjetoJSonAtivosRestritos,
            ObjetoJSonGrupos: ObjetoJSonGruposRestritos,
            Id: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        Cliente_Restricoes_Callback
    );

    return false;
}

function btnCliente_Restricoes_NovoOMS_Click(Sender) 
{
    //--> Recuperando os dados dos grupos.

    var lListaGruposRestritos = new Array()

    $(".ClienteRestricaoClienteParametroGrupo").each(function () {
        var lGruposSelecionados = { IdCliente: 0
                                  , IdParametro: 0
                                  , ListaGrupos: []
        };

        lGruposSelecionados.IdCliente = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

        $(this)
               .find(".Cliente_ResticoesPorGrupoRestringe:checked")
               .each(function () {
                   lGruposSelecionados.IdParametro = $(this).closest("p").find(".Cliente_ResticoesPorGrupoParametro").val();
                   lGruposSelecionados.ListaGrupos.push($(this).val());
               });

        lListaGruposRestritos.push(lGruposSelecionados);
    });

    //--> Recuperando os dados dos ativos.

    var lListaAtivosRestritos = { Ativos: [], Direcoes: [] }

    $(".Clientes_Restricoes_RestricaoPorAtivos")
        .find("tr")
        .each(function () {
            if ($($(this).find("td")[0]).html() != "") {
                lListaAtivosRestritos.Ativos.push($($(this).find("td")[0]).html());
                lListaAtivosRestritos.Direcoes.push($($(this).find("td")[1]).html());
            }
        });

    //--><--\\

    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimitesNovoOMS.aspx";

    var ObjetoJSonAtivosRestritos = $.toJSON(lListaAtivosRestritos);
    var ObjetoJSonGruposRestritos = $.toJSON(lListaGruposRestritos);

    var lDados =
        {
            acao: "SalvarDadosPermissoes",
            ObjetoJSonAtivos: ObjetoJSonAtivosRestritos,
            ObjetoJSonGrupos: ObjetoJSonGruposRestritos,
            Id: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        Cliente_Restricoes_Callback
    );

    return false;
}

function btnCliente_Restricoes_Click(Sender)
{
    //--> Recuperando os dados dos grupos.

    var lListaGruposRestritos = new Array()

    $(".ClienteRestricaoClienteParametroGrupo").each(function()
    {
        var lGruposSelecionados = { IdCliente: 0
                                  , IdParametro: 0
                                  , ListaGrupos: []};
                              
        lGruposSelecionados.IdCliente = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;

        $(this)
               .find(".Cliente_ResticoesPorGrupoRestringe:checked")
               .each(function()
                     {
                        lGruposSelecionados.IdParametro = $(this).closest("p").find(".Cliente_ResticoesPorGrupoParametro").val();
                        lGruposSelecionados.ListaGrupos.push($(this).val());
                     });

        lListaGruposRestritos.push(lGruposSelecionados);
    });

    //--> Recuperando os dados dos ativos.

    var lListaAtivosRestritos = { Ativos:[], Direcoes:[] }

    $(".Clientes_Restricoes_RestricaoPorAtivos")
        .find("tr")
        .each(function() 
        {
            if($($(this).find("td")[0]).html() != "")
            {
                lListaAtivosRestritos.Ativos.push($($(this).find("td")[0]).html());
                lListaAtivosRestritos.Direcoes.push($($(this).find("td")[1]).html());
            }
        });

    //--><--\\

    var lUrl = "Clientes/Formularios/Acoes/ConfigurarLimites.aspx";

    var ObjetoJSonAtivosRestritos = $.toJSON(lListaAtivosRestritos);
    var ObjetoJSonGruposRestritos = $.toJSON(lListaGruposRestritos);

    var lDados = 
        {
            acao            : "SalvarDadosPermissoes",
            ObjetoJSonAtivos: ObjetoJSonAtivosRestritos,
            ObjetoJSonGrupos: ObjetoJSonGruposRestritos,
            Id              : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
        };

    GradIntra_CarregarJsonVerificandoErro
    (
        lUrl,
        lDados,
        Cliente_Restricoes_Callback
    );

    return false;
}

function Cliente_Restricoes_Callback(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function GradIntra_Clientes_Restricoes_RestricaoPorGrupo_ExpandCollapse_Click(pSender)
{
    var lBotao = $(pSender);

    if (lBotao.parent().next().is(":visible"))
    {
        $(pSender).parent().next().hide();
        lBotao.html("[ + ]");   
    }
    else
    {
        lBotao.parent().next().show();
        lBotao.html("[ - ]");
    }

    return false;
}

function GradIntra_Clientes_Relatorios_ValidaDataNula(pSender)
{
    if ($(pSender).val() == "")
    {
        var lData = new Date();

        $(pSender).val(lData.getDate().toPrecision(2) + "/" + (lData.getMonth() + 1) + "/" + lData.getFullYear());
    }

    return false;
}

function FadeOutMensagemAlertaCliente(pSender)
{
    $(pSender).fadeOut()('slow', function() { });
    return false;
}


function cboClientes_Conta_Banco_OnChange(pSender)
{
    var lTxtAgencia = $("#txtClientes_Conta_Agencia");
    var lTxtAgenciaDigito = $("#txtClientes_Conta_Agencia_Digito");

    var lTxtConta = $("#txtClientes_Conta_NumeroDaConta");
    var lTxtContaDigito = $("#txtClientes_Conta_DigitoDaConta");

    lTxtContaDigito.prop("disabled", false);
    
    lTxtAgencia.attr("maxlength", 4);
    lTxtAgenciaDigito.attr("maxlength", 1);

    var lVal;

    switch ($(pSender).val())
    {
        case "001": //--> Banco do Brasil

            lTxtConta.attr("maxlength", 8);
            lTxtContaDigito.attr("maxlength", 1);

            lVal = lTxtContaDigito.val();

            if(lVal.length > 1) lTxtContaDigito.val(lVal.charAt(0));

            break;

        case "341": //--> Itaú

            lTxtConta.attr("maxlength", 5);
            lTxtContaDigito.attr("maxlength", 1);
            
            lVal = lTxtContaDigito.val();

            if(lVal.length > 1) lTxtContaDigito.val(lVal.charAt(0));

            break;

        case "399": //--> HSBC

            lTxtConta.attr("maxlength", 5);
            lTxtContaDigito.attr("maxlength", 2);

            break;

        case "745": //--> Citibank

            lTxtConta.attr("maxlength", 8);
            lTxtContaDigito.attr("maxlength", 2);


            txtClientes_Conta_DigitoDaConta_OnBlur("#txtClientes_Conta_NumeroDaConta");

            lTxtContaDigito.attr('disabled', 'disabled');

            break;

        case "151": //--> Nossa Caixa

            lTxtConta.attr("maxlength", 4);
            lTxtContaDigito.attr("maxlength", 1);

            $(pSender).val("001")
            
            lVal = lTxtContaDigito.val();

            if(lVal.length > 1) lTxtContaDigito.val(lVal.charAt(0));

            break;

        default:   //--> Demais bancos

            lTxtConta.attr("maxlength", 8);
            lTxtContaDigito.attr("maxlength", 2);

            break;
    }
  
    return false;
}

function TabProximoControleAposAtingirTamanhoMaximoDoCampo(pSender)
{
    var lSender = $(pSender);

    if ($("#cboClientes_Conta_Banco").val() == "001"
    && (lSender.val().indexOf("x") > -1))
    {
        lSender.val($("#txtClientes_Conta_DigitoDaConta").val().replace('x', '0'))
    }

    if (lSender.attr("maxlength") == pSender.value.length)
    {
        if ("745" == $("#cboClientes_Conta_Banco").val() //--> Citibank
        && (lSender.attr('id') == "txtClientes_Conta_NumeroDaConta"))
        {
            return false;
        }
        else
        {
            var lProximo = lSender.next();

            if (lProximo.length > 0)
                lProximo.focus();
            else
                $($("#txtClientes_Conta_Agencia_Digito").parent().next().find("input")[0]).focus();
        }
    }

    return false;
}

function GradIntra_Clientes_Spider_Limite_DetalheAlocado(pSender) 
{
    pSender = $(pSender);

    var lDados = { Acao: "AtualizarLimitesBloqueados", CodBovespa: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa };

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/SpiderLimitesBVSP.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Spider_Limite_DetalheAlocado_CallBack
                                             );
}

function GradIntra_Clientes_Spider_Limite_DetalheAlocado_CallBack(pResposta) 
{
    for (i = 0; i < pResposta.ObjetoDeRetorno.length; i++) 
    {
        if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("descoberto no mercado a vista") > 0) 
        {
            $("#txtCliente_Spider_Detalhes_AVista_Descoberto_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Spider_Detalhes_AVista_Descoberto_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Spider_Detalhes_AVista_Descoberto_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("descoberto no mercado de opcoes") > 0) 
        {
            $("#txtCliente_Spider_Detalhes_Opcoes_Descoberto_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Spider_Detalhes_Opcoes_Descoberto_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Spider_Detalhes_Opcoes_Descoberto_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("compra mercado a vista") > 0) 
        {
            $("#txtCliente_Spider_Detalhes_AVista_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Spider_Detalhes_AVista_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Spider_Detalhes_AVista_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("compra no mercado de opções") > 0) 
        {
            $("#txtCliente_Spider_Detalhes_Opcoes_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Spider_Detalhes_Opcoes_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Spider_Detalhes_Opcoes_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
    }
}

function GradIntra_Clientes_Limite_DetalheAlocado_NovoOMS(pSender) 
{
    pSender = $(pSender);

    var lDados = { Acao: "AtualizarLimitesBloqueados", CodBovespa: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa };

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/ConfigurarLimitesNovoOMS.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Limite_DetalheAlocado_NovoOMS_CallBack
                                             );
}

function GradIntra_Clientes_Limite_DetalheAlocado_NovoOMS_CallBack(pResposta) 
{
    for (i = 0; i < pResposta.ObjetoDeRetorno.length; i++) 
    {
        if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("descoberto no mercado a vista") > 0) 
        {
            $("#txtCliente_Detalhes_AVista_Descoberto_Limite_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_AVista_Descoberto_Alocado_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_AVista_Descoberto_Disponivel_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("descoberto no mercado de opcoes") > 0) 
        {
            $("#txtCliente_Detalhes_Opcoes_Descoberto_Limite_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_Opcoes_Descoberto_Alocado_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_Opcoes_Descoberto_Disponivel_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("compra mercado a vista") > 0) 
        {
            $("#txtCliente_Detalhes_AVista_Limite_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_AVista_Alocado_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_AVista_Disponivel_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("compra no mercado de opções") > 0) 
        {
            $("#txtCliente_Detalhes_Opcoes_Limite_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_Opcoes_Alocado_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_Opcoes_Disponivel_NovoOMS").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
    }
}

function GradIntra_Clientes_Limite_DetalheAlocado(pSender)
{
    pSender = $(pSender);

    var lDados = { Acao: "AtualizarLimitesBloqueados", CodBovespa: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa };

        GradIntra_CarregarJsonVerificandoErro(  "Clientes/Formularios/Acoes/ConfigurarLimites.aspx"
                                              , lDados
                                              , GradIntra_Clientes_Limite_DetalheAlocado_CallBack
                                             );
}

function GradIntra_Clientes_Limite_DetalheAlocado_CallBack(pResposta)
{
//    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    for (i = 0; i < pResposta.ObjetoDeRetorno.length; i++)
    {
        if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("descoberto no mercado a vista") > 0)
        {
            $("#txtCliente_Detalhes_AVista_Descoberto_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_AVista_Descoberto_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_AVista_Descoberto_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("descoberto no mercado de opcoes") > 0)
        {
            $("#txtCliente_Detalhes_Opcoes_Descoberto_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_Opcoes_Descoberto_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_Opcoes_Descoberto_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("compra mercado a vista") > 0)
        {
            $("#txtCliente_Detalhes_AVista_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_AVista_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_AVista_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
        else if (pResposta.ObjetoDeRetorno[i].Parametro.indexOf("compra no mercado de opções") > 0)
        {
            $("#txtCliente_Detalhes_Opcoes_Limite").val(pResposta.ObjetoDeRetorno[i].ValorLimite);
            $("#txtCliente_Detalhes_Opcoes_Alocado").val(pResposta.ObjetoDeRetorno[i].ValorAlocado);
            $("#txtCliente_Detalhes_Opcoes_Disponivel").val(pResposta.ObjetoDeRetorno[i].ValorDisponivel);
        }
    }
}


function GradIntra_Clientes_ValidarContaCorrente(pDivDeFormulario)
{
    /*
    Validação de Conta Corrente específica de acordo com:

    http://gsp-srv-tfs02/sites/Projetos/Gradual.Intranet/QA/Lists/Apontamentos/DispForm.aspx?ID=198

     1) Critica do numero de digitos dos campos agencia e digito: Agencia = 04 números + 01 digito 
     2) Banco do Brasil (bco 001), substituir o X por 0 (ex:- ver anexo) 
     3) Itau (bco 341) agencia = 05 números + 01 digito Ex:- 52524 - 6 
     4) HSBC agencia = 05 números + 02 digitos Ex:- 65235 - 23 
     5) Citibank colocar o último número como digito da conta automaticamente como digito Ex:- 1234567 irá ficar 123456 - 7 
     6) Se o cilente preencher banco Nossa Caixa Nosso banco (151), substituir pelo número do Bco do Brasil (001). 

    */
    
    if(pDivDeFormulario.validationEngine({returnIsValid:true}))
    {
        var lBanco = $("#cboClientes_Conta_Banco").val();

        var lAgencia  = $("#txtClientes_Conta_Agencia").val();
        var lDigitoAgencia = $("#txtClientes_Conta_Agencia_Digito").val();
    
        var lConta = $("#txtClientes_Conta_NumeroDaConta").val();
        var lDigitoConta = $("#txtClientes_Conta_DigitoDaConta").val();

        if(lBanco == "151")
        {
            alert("Aviso: o banco 'Nossa Caixa' será alterado para 'Banco do Brasil' automaticamente.");

            $("#cboClientes_Conta_Banco").val("001");

            lBanco = "001";
        }

        if(lBanco == "001")     // número 2 acima
        {
            lAgencia  =  lAgencia.replace(/X/gi, "0");
            lDigitoAgencia = lDigitoAgencia.replace(/X/gi, "0");
        }

//        if(lBanco == "341") // número 3 acima
//        {
//            if(lAgencia.length != 5)
//            {
//                alert("Para o banco Itaú, favor preencher o número da agência com 5 dígitos.");     return false;
//            }

//            if(lDigitoAgencia.length != 1)
//            {
//                alert("Para o banco Itaú, favor preencher o dígito da agência com 1 dígito."); return false;
//            }
//        }
//        else
        if(lBanco == "399")    //HSBC, número 4 acima
        {
            if(lConta.length != 5)
            {
                alert("Para o banco HSBC, favor preencher o número da conta com 5 dígitos."); return false;
            }

            if(lDigitoConta.length != 2)
            {
                alert("Para o banco HSBC, favor preencher o dígito da conta com 2 dígitos."); return false;
            }
        }
        else if(lBanco == "745" || lBanco == "477") //citibank, número 5 acima
        {
            if(lDigitoConta == "")
            {
                alert("Para o Citibank, o último dígito da sua conta será considerado como 'dígito' em separado.")

                lDigitoConta = lConta.substr(lConta.length - 1, 1);

                lConta = lConta.substr(0, lConta.length - 1);

                $("#txtClientes_Conta_NumeroDaConta").val(lConta);
                $("#txtClientes_Conta_DigitoDaConta").val(lDigitoConta);
            }
        }
        else  //número 1 acima, valendo pros outros
        {
            if(lAgencia.length != 4)
            {
                alert("Favor preencher o número da agência com 4 dígitos.");     return false;
            }
        }

        return true;
    }
    else
    {
        return false;
    }
}

function txtClientes_Conta_DigitoDaConta_OnBlur(pSender)
{
    var lCampoConta = $(pSender);

    if ("745" == $("#cboClientes_Conta_Banco").val()
    && ($(pSender).val().length > 0))
    {
        var lCampoContaDigito = $("#txtClientes_Conta_DigitoDaConta");
        var lValorConta = "";
        var lValorContaDigito = "";

        lValorConta = lCampoConta.val().substr(0, lCampoConta.val().length - 1);
        lValorContaDigito = lCampoConta.val().substr(lCampoConta.val().length - 1);

        lCampoConta.val(lValorConta);
        lCampoContaDigito.val(lValorContaDigito);
    }

    return false;
}

function GradIntra_Clientes_Consulta_PendenciasCadastrais_Load()
{
    if ($("#tblCadastro_EnderecosDoCliente").find("tr").hasClass("ItemDinamico"))
        $("#GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor").show();
    else
        $("#GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor").hide();
}

function Salvar_Clientes_DadosCompletos_PF(pSender)
{
    var lDiv;
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";
    var lUrl = "Clientes/Formularios/Dados/DadosCompletos_PF.aspx";

    if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
    {
        lDiv = $("#pnlNovoItem_Formulario");
    }
    else
    {
        lDiv = $("#pnlClientes_Formularios_Dados_DadosCompletos");
    }

    GradIntra_Cadastro_SalvarFormulario(lDiv, lUrl, lAcao);

    return false;
}

function Salvar_Clientes_DadosCompletos_PJ(pSender)
{
    var lDiv;
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";
    var lUrl = "Clientes/Formularios/Dados/DadosCompletos_PJ.aspx";

    if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
    {
        lDiv = $("#pnlNovoItem_Formulario");
    }
    else
    {
        lDiv = $("#pnlClientes_Formularios_Dados_DadosCompletos");
    }

    GradIntra_Cadastro_SalvarFormulario(lDiv, lUrl, lAcao);

    return false;
}

function GradIntra_Clientes_ImpostoDeRenda_GerarComprovante(pSender)
{
    if ($(pSender).attr("id").indexOf("Operacoes") > 0)
        gGradIntra_Clientes_Posicao_ImpostoDeRenda_Acao = "GerarComprovanteOperacoes";

    else if ($(pSender).attr("id").indexOf("DayTrade") > 0)
        gGradIntra_Clientes_Posicao_ImpostoDeRenda_Acao = "GerarComprovanteDayTrade";

    else if ($(pSender).attr("id").indexOf("TesouroDireto") > 0)
        gGradIntra_Clientes_Posicao_ImpostoDeRenda_Acao = "GerarComprovanteTesouroDireto";
        
    var lEndereco= "Clientes/Formularios/Dados/ImpostoDeRenda.aspx?Ano="       + $("#cmbGradIntra_Clientes_ImpostoDeRenda_AnoVigencia").val()             +
                                                                 "&cpfcnpj="   + gGradIntra_Cadastro_ItemSendoEditado.CPF                                 +
                                                                 "&IdCliente=" + gGradIntra_Cadastro_ItemSendoEditado.Id                                  +
                                                                 "&nome="      + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente.replace(/ {1}/gi,"%20") +
                                                                 "&Acao="      + gGradIntra_Clientes_Posicao_ImpostoDeRenda_Acao                                                                    +
                                                                 "&GerarRelatorio=false";
    GradIntra_CarregarJsonVerificandoErro( lEndereco
                                         , null
                                         , GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_CallBack
                                         );
}

function GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_CallBack(pResposta)
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem("I", "Este cliente não possui.");
    }
    else
    {
        var lEndereco= "Clientes/Formularios/Dados/ImpostoDeRenda.aspx?Ano="       + $("#cmbGradIntra_Clientes_ImpostoDeRenda_AnoVigencia").val()             +
                                                                     "&cpfcnpj="   + gGradIntra_Cadastro_ItemSendoEditado.CPF                                 +
                                                                     "&IdCliente=" + gGradIntra_Cadastro_ItemSendoEditado.Id                                  +
                                                                     "&nome="      + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente.replace(/ {1}/gi,"%20") +
                                                                     "&Acao="      + gGradIntra_Clientes_Posicao_ImpostoDeRenda_Acao                                                                    +
                                                                     "&GerarRelatorio=true";
        window.open(lEndereco);
    }
}

function chkGradIntra_Clientes_Formularios_Acoes_DocumentacaoEntregue(pSender)
{
    alert($(pSender).val());
}

function Cliente_limites_Bovespa_AtualizarDados(pSender)
{
    var lDados = { Acao: "CarregarDadosAtualizados", CodBovespa: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa };

//    GradIntra_CarregarHtmlVerificandoErro( "Clientes/Formularios/Acoes/ConfigurarLimites.aspx"
//                                         , lDados
//                                         , null
//                                         , Cliente_limites_Bovespa_AtualizarDados_CallBack);

    GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Acoes/ConfigurarLimites.aspx"
                                         , lDados
                                         , Cliente_limites_Bovespa_AtualizarDados_CallBack
                                         );
    return false;
}

function Cliente_limites_Bovespa_AtualizarDados_CallBack(pResposta)
{
    if (!pResposta.TemErro)
    {
        $(pResposta.ObjetoDeRetorno).each(function()
        {
            var lItem = $(this)[0];

            if ("1" == lItem.TipoLimite)
            {
                $("#txtCliente_Limites_Bosvespa_AVista_Limite").val(lItem.Limite);
                $("#txtCliente_Limites_Bosvespa_AVista_DataDeVencimento").val(lItem.Vencimento);
            }
            else if ("2" == lItem.TipoLimite)
            {
                $("#txtCliente_Limites_Bovespa_Opcao_Limite").val(lItem.Limite);
                $("#txtCliente_Limites_Bovespa_Opcao_DataDeVencimento").val(lItem.Vencimento);
            }
            else if ("3" == lItem.TipoLimite)
            {
                $("#txtCliente_Limites_Bosvespa_AVista_LimiteDescoberto").val(lItem.Limite);
                $("#txtCliente_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto").val(lItem.Vencimento);
            }
            else if ("4" == lItem.TipoLimite)
            {
                $("#txtCliente_Limites_Bovespa_Opcao_LimiteDescoberto").val(lItem.Limite);
                $("#txtCliente_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto").val(lItem.Vencimento);
            }
            else if ("5" == lItem.TipoLimite)
            {
                $("#txtCliente_Limites_Bovespa_ValorMaximoDaOrdem").val(lItem.Limite);
                $("#txtCliente_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento").val(lItem.Vencimento);
            }
        });

        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
}

function GradItra_Clientes_Relatorios_Financeiro_Fax_Bov_Resumido_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/FaxBovespaResumido.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Fax_Bov_EN_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/FaxBovespaEN.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Fax_Bmf_EN_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/FaxBmfEN.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="     + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="         + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="          + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="           + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="          + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="          + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao=" + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="            + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Fax_Bmf_Volatilidade_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/FaxBmfVolatilidade.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    return false;
}
function GradItra_Clientes_Relatorios_Financeiro_Extrato_Bovespa_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/ExtratoOperacoesBovespa.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    return false;

}

function GradItra_Clientes_Relatorios_Financeiro_Fax_Bmf_BR_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/FaxBmfBR.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Fax_Bov_BR_Click_Print(pSender)
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/FaxBovespaBR.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_Fax_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Click_Print(pSender) 
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/NotaDeCorretagemBmf.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#SpanLinksPaginacao").find(".Selecionado").attr("title");
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_ExtratoFatura_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    

    return false;
}

function GradItra_Clientes_Relatorios_Financeiro_Click_Bovespa_Print(pSender) 
{
    var oPrint, oJan;
    
    var lUrl = "Posicao/NotaDeCorretagem.aspx?Acao=CarregarComPaginacao";

    lUrl += "&CdBovespaCliente="        + gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
    lUrl += "&CdBmfCliente="            + gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
    lUrl += "&TipoMercado="             + $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val();
    lUrl += "&Provisorio="              + $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked");
    lUrl += "&Bolsa="                   + $("#cboRelatorio_NotasDeCorretagem_Bolsa").val();
    lUrl += "&NomeCliente="             + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente;
    lUrl += "&DataInicial="             + $("#SpanLinksPaginacao").find(".Selecionado").attr("title");
    lUrl += "&DataInicialPaginacao="    + $("#txtRelatorio_ExtratoFatura_Data_Inicial").val();
    lUrl += "&DataFinal="               + $("#txtRelatorio_ExtratoFatura_Data_Final").val();

    gRelatorioCarregando = false;

    oJan = window.open(lUrl);
    //oJan.document.open();
    oJan.onload = function () {oJan.window.focus();
    oJan.window.print();}
    
    

    return false;
}

function GradItra_Clientes_PJ_CarregarDetalhesUS()
{
    if($("#rdoCadastro_DadosCompletos_USPersonNacional_Sim").is(":checked"))
        $("#pnlCadastro_DadosCompletos_USPersonNacional_Detalhes").show();

    if($("#rdoCadastro_PFPasso3_USPersonResidente_Sim").is(":checked"))
        $("#pnlCadastro_DadosCompletos_USPersonResidente_Detalhes").show();

    if($("#rdoCadastro_PFPasso3_USPersonNascido_Sim").is(":checked"))
        $("#pnlCadastro_DadosCompletos_USPersonRenuncia_Detalhes").show();
}

function ConfigurarPaginacaoNotaDeCorretagem(pNumeroDaPagina)
{
    var lClasse               = "";
    var lSpanLinksPaginacao   = $("#SpanLinksPaginacao");
    var lBtnPaginacaoProxima  = $("#btnPaginacaoProximo");
    var lBtnPaginacaoAnterior = $("#btnPaginacaoAnterior");
    var lListaDeDatas         = $("#hddDatasPaginacao").val()
                                                       .replace('\"', '')
                                                       .replace('\"', '')
                                                       .replace('\"', '')
                                                       .replace('\"', '')
                                                       .split(';');
    var lBolsa = $("#hddBolsa").val();

    if (pNumeroDaPagina == null)
        pNumeroDaPagina = 1;

    pNumeroDaPagina = parseInt(pNumeroDaPagina);

    lSpanLinksPaginacao.html("");

    for (var i = 0; i < lListaDeDatas.length; i++)
    {
        lClasse = (i + 1 == pNumeroDaPagina) ? "Selecionado" : "";

        if (lBolsa == "Bovespa")
        {
            lSpanLinksPaginacao.append(" <a href=\"#\" onclick=\"return PaginarNotaDeCorretagem('" + lListaDeDatas[lListaDeDatas.length - i - 1] + "', '" + parseInt(i + 1) + "');\" title=\"" + lListaDeDatas[lListaDeDatas.length - i - 1] + "\" class='" + lClasse + "'>" + parseInt(i + 1) + "</a>");
        }
        else if (lBolsa == "Bmf")
        {
            lSpanLinksPaginacao.append(" <a href=\"#\" onclick=\"return PaginarNotaDeCorretagemBmf('" + lListaDeDatas[lListaDeDatas.length - i - 1] + "', '" + parseInt(i + 1) + "');\" title=\"" + lListaDeDatas[lListaDeDatas.length - i - 1] + "\" class='" + lClasse + "'>" + parseInt(i + 1) + "</a>");
        }
    }

    if (pNumeroDaPagina - 1 == 0)
        lBtnPaginacaoAnterior.bind("click", function() { 
        if (lBolsa == "Bovespa") { PaginarNotaDeCorretagem(lListaDeDatas[0], lListaDeDatas.length); } else {PaginarNotaDeCorretagemBmf(lListaDeDatas[0], lListaDeDatas.length); }
        } );
    else
        lBtnPaginacaoAnterior.bind("click", function() { 
        if (lBolsa == "Bovespa") { PaginarNotaDeCorretagem(lListaDeDatas[lListaDeDatas.length - pNumeroDaPagina + 1], pNumeroDaPagina - 1); } else { PaginarNotaDeCorretagemBmf(lListaDeDatas[lListaDeDatas.length - pNumeroDaPagina + 1], pNumeroDaPagina - 1); }
        } );
    
    if (pNumeroDaPagina == lListaDeDatas.length)
        lBtnPaginacaoProxima.bind("click", function() { 
        if (lBolsa == "Bovespa") { PaginarNotaDeCorretagem(lListaDeDatas[lListaDeDatas.length - 1], 1); } else { PaginarNotaDeCorretagemBmf(lListaDeDatas[lListaDeDatas.length - 1], 1); }
        } );
    else
        lBtnPaginacaoProxima.bind("click", function() { 
        if (lBolsa == "Bovespa") { PaginarNotaDeCorretagem(lListaDeDatas[lListaDeDatas.length - pNumeroDaPagina - 1], pNumeroDaPagina + 1); } else { PaginarNotaDeCorretagemBmf(lListaDeDatas[lListaDeDatas.length - pNumeroDaPagina - 1], pNumeroDaPagina + 1); }
        } );
}

function PaginarNotaDeCorretagemBmf(pDataDaNota, pNumeroDaPagina)
{
    gDataDaNotaParaCSV = pDataDaNota;

    var lUrl = "Posicao/NotaDeCorretagemBmf.aspx?Acao=CarregarComPaginacao&NumeroDaPagina=" + pNumeroDaPagina;

    var lDados = { CdBovespaCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                 , CdBmfCliente         : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                 , NomeCliente          : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                 , TipoMercado          : $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val()
                 , Provisorio           : $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked")
                 , Bolsa                : $("#cboRelatorio_NotasDeCorretagem_Bolsa").val()
                 , DataInicial          : pDataDaNota
                 , DataInicialPaginacao : $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                 , DataFinal            : $("#txtRelatorio_ExtratoFatura_Data_Final").val()
                 };

    gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Notas de Corretagem, bolsa [" + lDados.Bolsa + "], de " + lDados.DataFinal;

    GradIntra_CarregarHtmlVerificandoErro( lUrl
                                         , lDados
                                         , null
                                         , function(pResposta) { PaginarNotaDeCorretagem_CallBack(pResposta, pNumeroDaPagina) });
    return false;
}

function PaginarNotaDeCorretagem(pDataDaNota, pNumeroDaPagina)
{
    gDataDaNotaParaCSV = pDataDaNota;

    var lUrl = "Posicao/NotaDeCorretagem.aspx?Acao=CarregarComPaginacao&NumeroDaPagina=" + pNumeroDaPagina;

    var lDados = { CdBovespaCliente     : gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                 , CdBmfCliente         : gGradIntra_Cadastro_ItemSendoEditado.CodBMF
                 , NomeCliente          : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
                 , TipoMercado          : $(".Cliente_Relatorio_NotaCorretagem_TipoMercado:checked").val()
                 , Provisorio           : $("#ckbRelatorio_NotaCorretagem_Provisorio").prop("checked")
                 , Bolsa                : $("#cboRelatorio_NotasDeCorretagem_Bolsa").val()
                 , DataInicial          : pDataDaNota
                 , DataInicialPaginacao : $("#txtRelatorio_ExtratoFatura_Data_Inicial").val()
                 , DataFinal            : $("#txtRelatorio_ExtratoFatura_Data_Final").val()
                 };

    gRelatorio_DescricaoDoRelatorioEscolhido = "Relatório de Notas de Corretagem, bolsa [" + lDados.Bolsa + "], de " + lDados.DataFinal;

    GradIntra_CarregarHtmlVerificandoErro( lUrl
                                         , lDados
                                         , null
                                         , function(pResposta) { PaginarNotaDeCorretagem_CallBack(pResposta, pNumeroDaPagina) });
    return false;
}

function PaginarNotaDeCorretagem_CallBack(pResposta, pNumeroDaPagina)
{
    gRelatorioCarregando = false;

    if (pResposta.indexOf("Erro:") >= 0)
        GradIntra_ExibirMensagem("E", pResposta);
    else
    {
        $("#pnlCliente_Posicao_Relatorio").html(pResposta).show();

        $("#divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio").show();

        ConfigurarPaginacaoNotaDeCorretagem(pNumeroDaPagina);

        CarregarRelatorioPosicaoClienteOcultarCampos();

        $("#cmbRelatorio_Posicao_Carregar").val(""); //--> Setando o compo para o primeiro item.

        $(".PainelEmTelaCheia_Opcoes_SubFormulario").hide(); //--> Ocultando o painel anterior.
    }

    return false;
}

function GradIntra_Clientes_Produtos_DadosPlanosCliente_Click(pSender)
{
    lSender = $(pSender);

    if (lSender.next().html() == "Direct Trade - Calculadora IR para um intervalo de datas retrocedente")
    {
        lSender.next().addClass("checked");
        lSender.prop("checked", true);
        return false;
    }
    else if (!lSender.next().hasClass("checked"))
    {
        alert("As regras atuais de não permitem que um produto seja assossiado ao cliente através desta tela.\nÉ habilitada apenas a desassociação.");
        lSender.next().removeClass("checked");
        lSender.prop("checked", false);
    }
    else
    {
        lSender.addClass("AdicionadoParaExclusao");
    }

    return false;
}

function GradIntra_Cliente_Produto_Detalhes_Click(pSender)
{
    var lSender = $(pSender);
    var lIdProduto = lSender.attr("IdProduto");
    var lDiv = $("#divCliente_Produtos_Detalhes_" + lIdProduto);

    if (lDiv.length == 0)
        lDiv = $("#divCliente_Produtos_Detalhes_Poupe" + lIdProduto);

    if ("[ + ]" == lSender.html())
    {
        lSender.html("[ - ]");
        lDiv.show();
    }
    else
    {
        lSender.html("[ + ]");
        lDiv.hide();
    }
}
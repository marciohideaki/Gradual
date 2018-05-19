/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />

var gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual = CONST_SEGURANCA_TIPODEOBJETO_USUARIO;  //Usuario, Grupo ou Perfil

function GradIntra_Seguranca_AoSelecionarSistema()
{
    if(gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_ASSOCIAR_PERMISSOES)
    {
        $("#pnlBusca_Seguranca").hide();
        
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_ALTERAR_SENHA)
    {
        $("#pnlBusca_Seguranca").hide();
        
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
        
        gGradIntra_Navegacao_PainelDeConteudoAtual
            .addClass(CONST_CLASS_CARREGANDOCONTEUDO)
            .show();

        GradIntra_Navegacao_ExibirFormulario("Formularios/Dados/AlterarSenha.aspx");
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PARAMETROS_GLOBAIS) {
        $("#pnlBusca_Seguranca").hide();

        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

        gGradIntra_Navegacao_PainelDeConteudoAtual
            .addClass(CONST_CLASS_CARREGANDOCONTEUDO)
            .show();

        GradIntra_Navegacao_ExibirFormulario("Formularios/Dados/ParametrosGlobais.aspx");
    }
}

function GradIntra_Seguranca_ReverterTipoDeObjetoDeSegurancaAtual(pTipoDeObjeto, pRecarregarConteudo)
{
    ///<summary>Função geral centralizadora para tratar o objeto de segurança sendo editado como Usuario, Grupo ou Perfil.</summary>
    ///<param name="pTipoDeObjeto" type="String">Tipo de objeto: 'Usuario', 'Grupo' ou 'Perfil'.</param>
    ///<param name="pRecarregarConteudo" type="Boolean">Flag se deve marcar o formulário de dados básicos para recarregar o conteúdo.</param>
    ///<returns>void</returns>

    gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual = pTipoDeObjeto;

    if(pRecarregarConteudo)
        $("#pnlSeguranca_Formularios_Dados_DadosCompletos").removeClass(CONST_CLASS_CONTEUDOCARREGADO);    //mudou o tipo, tem que mudar o conteudo

    $("#lstItemMenu_Seguranca li.Tipo_U, #lstItemMenu_Seguranca li.Tipo_G, #lstItemMenu_Seguranca li.Tipo_P").hide();

    if (gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual != CONST_SEGURANCA_TIPODEOBJETO_PERMISSAOSEGURANCA) 
    {
        $("#lstItemMenu_Seguranca li.Tipo_" + gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual.charAt(0)).show();
    }
    else 
    {
        $("#lstItemMenu_Seguranca li.Tipo_PS").show();
    }
}

function GradIntra_Seguranca_SalvarDadosCompletos()
{
    var lAcao = gGradIntra_Cadastro_FlagIncluindoNovoItem ? "Cadastrar" : "Atualizar";
    var lDiv = $("#pnlNovoItem_Formulario");

    var lUrl = "Seguranca/Formularios/Dados/DadosCompletos_" + gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual + ".aspx";

    if (lAcao == "Atualizar") 
    {
        lDiv = $("#pnlSeguranca_Formularios_Dados_DadosCompletos");
    }

    if (ValidatePasswordFieldForm("#txtSeguranca_DadosCompletos_Usuario_Senha")) 
    {
        GradIntra_Cadastro_SalvarFormulario(lDiv, lUrl, lAcao);
    }

    return false;
}

function GradIntra_Seguranca_ItemIncluidoComSucesso(pItemIncluido)
{
    gGradIntra_Navegacao_SubSistemaAtual = CONST_SUBSISTEMA_BUSCA;

    gGradIntra_Navegacao_PainelDeBuscaAtual    = $("#pnlBusca_Seguranca_Busca");

    gGradIntra_Navegacao_PainelDeConteudoAtual = $("#pnlConteudo_Seguranca_Busca");

    var lNovaLI = GradIntra_Navegacao_AdicionarItemNaListaDeItensSelecionados(pItemIncluido, CONST_SISTEMA_SEGURANCA, true);
    
    //se já tinha algum selecionado, deseleciona:
    $("#lstItensSelecionados_Seguranca li").removeClass(CONST_CLASS_ITEM_EXPANDIDO);

    lNovaLI.addClass(CONST_CLASS_ITEM_EXPANDIDO);
    
    $("ul.Sistema_Seguranca li." + CONST_CLASS_ITEM_SELECIONADO).removeClass(CONST_CLASS_ITEM_SELECIONADO);

    $("ul.Sistema_Seguranca li a[rel='Busca']").parent().addClass(CONST_CLASS_ITEM_SELECIONADO);

    gGradIntra_Navegacao_PainelDeBuscaAtual
        .show();

    gGradIntra_Navegacao_PainelDeConteudoAtual
        .show()
        .find("ul.MenuDoObjeto")
            .show()
            .find("li.SubTitulo, li.Tipo_" + pItemIncluido.TipoDeObjeto.charAt(0))
                .show();

    GradIntra_Seguranca_ReverterTipoDeObjetoDeSegurancaAtual(pItemIncluido.TipoDeObjeto, false);

    $("#lnkItemMenu_Seguranca_DadosCompletos_Usuario").click();
}

function GradIntra_Seguranca_ExcluirItem() 
{
    if (confirm('Confirma a exclusão?'))
    {
        var lUrl = "Seguranca/Formularios/Dados/DadosCompletos_" + gGradIntra_Seguranca_TipoDeObjetoDeSegurancaAtual + ".aspx";

        var lDados = { Acao: 'Excluir', Id: gGradIntra_Cadastro_ItemSendoEditado.Id };

        GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, GradIntra_Seguranca_ExcluirItem_CallBack);
    }
}

function GradIntra_Seguranca_ExcluirItem_CallBack(pResposta) 
{
    if (pResposta.TemErro)
    {
        GradIntra_ExibirMensagem('E', pResposta.Mensagem);
    }
    else
    {
        GradIntra_ExibirMensagem('I', pResposta.Mensagem);

        GradIntra_ExcluirItemDaListaDeItensSelecionados($('#lstItensSelecionados_Seguranca li.Expandida'));

        GradIntra_Navegacao_ExpandirDadosDoPrimeiroItemSelecionado();
    }
}

function GradIntra_Seguranca_ExibirAssociacoes()
{
    GradIntra_CarregarHtmlVerificandoErro( "Seguranca/Formularios/Dados/AssociarPermissoes.aspx"
                                            , null
                                            , $("#pnlSeguranca_Formularios_Dados_AssociarPermissoes")
                                            , GradIntra_Seguranca_ExibirAssociacoes_CallBack);
}

function GradIntra_Seguranca_ExibirAssociacoes_CallBack(pResposta)
{
    $("#pnlConteudo_Seguranca div.pnlFormulario")
        .show()
        .find("input[type='checkbox'],input[type='radio']")
        .customInput();
}

function GradIntra_Seguranca_ListarInterfaces(pObjeto)
{
    var pDados = { Acao:'CarregarInterfaces', SubSistema:$(pObjeto).val() };

    GradIntra_CarregarJsonVerificandoErro( "Seguranca/Formularios/Dados/AssociarPermissoes.aspx"
                                            , pDados
                                            , GradIntra_Seguranca_ListarInterfaces_CallBack);
}

function GradIntra_Seguranca_ListarInterfaces_CallBack(pResposta)
{
    $("#cboSeguranca_Associacoes_Interfaces").children().remove();
    $("#cboSeguranca_Associacoes_Interfaces")
        .append( $("<option>").html("[ Selecione ]").val("") );

    lObjetoDeRetorno = pResposta.ObjetoDeRetorno;

    $(lObjetoDeRetorno).each(function()
    {
        lItem = this;   //só pra copiar o valor do item como string mesmo

        $("#cboSeguranca_Associacoes_Interfaces")
            .append( $("<option>").html(lItem.Nome).val(lItem.NomePermissao) );
    });
}

function GradIntra_Seguranca_AssossiarPermissoes(pObjeto)
{
    var pDadosAssociados = { EhUsuario  : $("#rdoSeguranca_Associacoes_Usuario").is(":checked")
                           , EhGrupo    : $("#rdoSeguranca_Associacoes_Grupo").is(":checked")
                           , Usuario    : $("#cboSeguranca_Associacoes_Usuario").val()
                           , Grupo      : $("#cboSeguranca_Associacoes_Grupo").val()
                           , Subsistema : $("#cboSeguranca_Associacoes_Subsistemas").val()
                           , Interface  : $("#cboSeguranca_Associacoes_Interfaces").val()
                           , Consultar  : $("#chkSeguranca_Associacoes_Permissoes_Consultar").is(":checked")
                           , Salvar     : $("#chkSeguranca_Associacoes_Permissoes_Salvar").is(":checked")
                           , Excluir    : $("#chkSeguranca_Associacoes_Permissoes_Excluir").is(":checked")
                           , Executar   : $("#chkSeguranca_Associacoes_Permissoes_Executar").is(":checked")};
    
       var pDados = {Acao:'Salvar', ObjetoJson:$.toJSON(pDadosAssociados)};

       GradIntra_CarregarJsonVerificandoErro( "Seguranca/Formularios/Dados/AssociarPermissoes.aspx"
                                            , pDados
                                            , GradIntra_Seguranca_AssossiarPermissoes_CallBack);
}

function GradIntra_Seguranca_AssossiarPermissoes_CallBack(pResposta)
{
        GradIntra_ExibirMensagem('I', pResposta.Mensagem);
}

function GradIntra_Seguranca_Associacoes_ReceberPermissoes(pObjeto)
{
     var pDadosAssociados = { EhUsuario:$("#rdoSeguranca_Associacoes_Usuario").is(":checked")
                            , EhGrupo:$("#rdoSeguranca_Associacoes_Grupo").is(":checked")
                            , Usuario:$("#cboSeguranca_Associacoes_Usuario").val()
                            , Grupo:$("#cboSeguranca_Associacoes_Grupo").val()
                            , Subsistema:$("#cboSeguranca_Associacoes_Subsistemas").val()
                            , Interface:$("#cboSeguranca_Associacoes_Interfaces").val()
                            , Consultar:$("#chkSeguranca_Associacoes_Permissoes_Consultar").is(":checked")
                            , Salvar:$("#chkSeguranca_Associacoes_Permissoes_Salvar").is(":checked")
                            , Excluir:$("#chkSeguranca_Associacoes_Permissoes_Excluir").is(":checked")
                            , Executar:$("#chkSeguranca_Associacoes_Permissoes_Executar").is(":checked")
                            };
                     
    var pDados = {Acao:'ReceberPermissoes', ObjetoJson:$.toJSON(pDadosAssociados)};

           GradIntra_CarregarJsonVerificandoErro( "Seguranca/Formularios/Dados/AssociarPermissoes.aspx"
                                                , pDados
                                                , GradIntra_Seguranca_Associacoes_ReceberPermissoes_CallBack);
}                                            

function GradIntra_Seguranca_Associacoes_ReceberPermissoes_CallBack(pResposta)
{
    if($("#chkSeguranca_Associacoes_Permissoes_Salvar").is(":checked"))
    {
        $("#chkSeguranca_Associacoes_Permissoes_Salvar").attr('checked', null);
        $("label[for='chkSeguranca_Associacoes_Permissoes_Salvar']").attr('class', null)
    }

    if($("#chkSeguranca_Associacoes_Permissoes_Excluir").is(":checked"))
    {
        $("#chkSeguranca_Associacoes_Permissoes_Excluir").attr('checked', null);
        $("label[for='chkSeguranca_Associacoes_Permissoes_Excluir']").attr('class', null)
    }

    if($("#chkSeguranca_Associacoes_Permissoes_Consultar").is(":checked"))
    {
        $("#chkSeguranca_Associacoes_Permissoes_Consultar").attr('checked', null);
        $("label[for='chkSeguranca_Associacoes_Permissoes_Consultar']").attr('class', null)
    }

    if($("#chkSeguranca_Associacoes_Permissoes_Executar").is(":checked"))
    {
        $("#chkSeguranca_Associacoes_Permissoes_Executar").attr('checked', null);
        $("label[for='chkSeguranca_Associacoes_Permissoes_Executar']").attr('class', null)
    }
    
    if(pResposta.ObjetoDeRetorno.Executar)
    {
        
        $("label[for='chkSeguranca_Associacoes_Permissoes_Executar']").attr('class', 'checked')
        $("#chkSeguranca_Associacoes_Permissoes_Executar").attr('checked', 'checked');
    }
    if(pResposta.ObjetoDeRetorno.Consultar)
    {
        $("label[for='chkSeguranca_Associacoes_Permissoes_Consultar']").attr('class', 'checked')
        $("#chkSeguranca_Associacoes_Permissoes_Consultar").attr('checked', 'checked');
    }
    if(pResposta.ObjetoDeRetorno.Salvar)
    {
        $("label[for='chkSeguranca_Associacoes_Permissoes_Salvar']").attr('class', 'checked')
        $("#chkSeguranca_Associacoes_Permissoes_Salvar").attr('checked', 'checked');
    }
    if(pResposta.ObjetoDeRetorno.Excluir)
    {
        $("label[for='chkSeguranca_Associacoes_Permissoes_Excluir']").attr('class', 'checked')
        $("#chkSeguranca_Associacoes_Permissoes_Excluir").attr('checked', 'checked');
    }
}

function GradIntra_Seguranca_ListarUsuarios(){}

function GradIntra_Seguranca_ListarUsuarios_CallBack(){}

function GradIntra_Seguranca_ListarGrupos_CallBack(pResposta){}

function GradIntra_Seguranca_AssociarPermissoes()
{
    var lArrPermissoes = eval($("#hidSeguranca_Permissoes_ListaJson").val());

    for (var i = 0; i < lArrPermissoes.length; i++)
    {
        $("#pnlSeguranca_Permissoes_Resultados")
            .find("input[ValorQuandoSelecionado='" + lArrPermissoes[i] + "']")
                .prop("checked", true)
                .parent()
                    .addClass("checked");
    }
}

function ValidatePasswordFieldForm(pSender) 
{
    var lCaller = $(pSender).val();

    if (lCaller == null) 
    {
        return true;
    }

    if (lCaller.length < 8 || lCaller.length > 15) {
    
        GradIntra_ExibirMensagem("A", "Campo deve ter de 8 a 15 caracteres");
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter de 8 a 15 caracteres";
        return false;
    }

    var goodChars = "ABCDEFGHIJKLMNOPQRSTUXYWVZ";

    var goodNumerics = "0123456789";

    var lContQuantidadeMinimoLetraMaiuscula = 1;

    var lContQuantidadeMinimoLetraMinuscula = 1;

    var lContQuantidadeMinimoNumero = 1;

    var lContLetraMaiuscula = 0;

    var lContLetraMinuscula = 0;

    var lContNumeros = 0;

    for (i = 0; i < lCaller.length; i++) {
        if (goodChars.indexOf(lCaller[i]) != -1) {
            lContLetraMaiuscula++;
        }

        if (goodChars.toLowerCase().indexOf(lCaller[i]) != -1) {
            lContLetraMinuscula++;
        }

        if (goodNumerics.indexOf(lCaller[i]) != -1) {
            lContNumeros++;
        }
    }

    if (lContLetraMaiuscula < lContQuantidadeMinimoLetraMaiuscula) {
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter pelo menos um caracter mai&uacutesculo";
        GradIntra_ExibirMensagem("A", "Campo deve ter pelo menos um caracter mai&uacutesculo");
        return false ;
    }

    if (lContLetraMinuscula < lContQuantidadeMinimoLetraMinuscula) {
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter pelo menos um caracter min&uacutesculo";
        GradIntra_ExibirMensagem("A", "Campo deve ter pelo menos um caracter min&uacutesculo");
        return false;
    }

    if (lContNumeros < lContQuantidadeMinimoNumero) {
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter pelo menos um caracter Num&eacuterico";
        GradIntra_ExibirMensagem("A", "Campo deve ter pelo menos um caracter Num&eacuterico");
        return false;
    }

    return true;
}

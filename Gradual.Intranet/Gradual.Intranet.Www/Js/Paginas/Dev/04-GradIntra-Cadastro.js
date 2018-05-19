/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

//var gGradIntra_Cadastro_ItemSendoIncluido = null;

var gGradIntra_Cadastro_FlagIncluindoNovoItem = false;

var gGradIntra_Cadastro_ItemSendoIncluido = null;

var gGradIntra_Cadastro_ItemSendoEditado  = null;

var gGradIntra_Cadastro_ItemFilhoSendoEditado = null;

var gGradIntra_Cadastro_DadosDoItemFilhoAtual = null;

/*

Formulários "Top Level"

*/

function GradIntra_Cadastro_NovoItem()
{
    //todo: verifica se está editando?
    
    //gGradIntra_Cadastro_ItemSendoIncluido = { Id: -1 };
    gGradIntra_Cadastro_ItemSendoEditado  = null;
    
}

function GradIntra_Cadastro_SalvarFormulario(pDivDeFormulario, pUrl, pAcao)
{
///<summary>Salva um formulário qualquer da página.</summary>
///<param name="pDivDeFormulario" type="Objeto_jQuery">Div cujos inputs serão enviados para salvar.</param>
///<param name="pUrl" type="String">URL para onde a chamada ajax será feita.</param>
///<param name="pAcao" type="String">Ação que será enviada na chamada ajax.</param>///<returns>void</returns>

    if (pDivDeFormulario.validationEngine({returnIsValid:true}))
    {
        var lDivId = pDivDeFormulario.attr("id");

        var lObjeto = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

        if (lObjeto.ParentId)
        {
            //é algum tipo de elemento filho
            lObjeto.ParentId = gGradIntra_Cadastro_ItemSendoEditado.Id;
        }

        //lObjeto.Id = gGradIntra_Cadastro_ItemSendoEditado.Id;

        var lDados = new Object();

        lDados.Acao = pAcao;

        lDados.ObjetoJson = $.toJSON(lObjeto);

        GradIntra_CarregarJsonVerificandoErro( pUrl
                                             , lDados
                                             , function(pResposta) { GradIntra_Cadastro_SalvarFormulario_CallBack(pResposta, lDivId); }
                                             , null
                                             , pDivDeFormulario);

        pDivDeFormulario.find("input, select").prop("disabled", true);
        
        gGradIntra_Cadastro_ItemSendoIncluido = lObjeto;
    }    
    else
    {
        //GradIntra_Cadastro_RemoverMultiplosRadiosDaTela();

        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar");
    }
}

function GradIntra_Cadastro_RemoverMultiplosRadiosDaTela()
{
    // Caro Programador, instrução abaixo para correção de bug de multiplicação dos radios na tela.
    if ($("div.custom-radio label").length > 0)
        $("div.custom-radio label").remove();
}

function GradIntra_Cadastro_SalvarFormulario_CallBack(pResposta, pIdDoDivDeFormulario)
{
///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Cadastro_SalvarFormulario: reabilita os controles e executa pCallBackAposEnviar.</summary>
///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
///<param name="pIdDoDivDeFormulario" type="String">Id do div que foi atualizado.</param>
///<returns>void</returns>

    $("#" + pIdDoDivDeFormulario).find("input, select, button").prop("disabled", false);

    if (pResposta.TemErro)
    {
        GradIntra_TratarRespostaComErro(pResposta);
    }
    else
    {
        GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);
        
        if (pResposta.ObjetoDeRetorno)
        {
            if (pResposta.ObjetoDeRetorno.IdCadastrado)
            {
                $("#" + pIdDoDivDeFormulario).find("input[Propriedade='Id']").val(pResposta.ObjetoDeRetorno.IdCadastrado);

                gGradIntra_Cadastro_ItemSendoIncluido.Id = pResposta.ObjetoDeRetorno.IdCadastrado;


                //gGradIntra_Cadastro_ItemSendoEditado.Id = pResposta.ObjetoDeRetorno.IdCadastrado;  

                //gGradIntra_Cadastro_ItemSendoEditado.DataCadastroString = GradAux_DataDeHoje();
            }
        }

        GradIntra_CadastroPF_LoadPage(); //--> Inabilitar campos necessários

        if ($("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").next().hasClass("checked"))
        {   //--> Apagando o valor do campo após a atualização [caso necessario]
            $("#txtClientes_DadosCompletos_NaoOperaPorContaPropria_Nome").val("");
            $("#txtClientes_DadosCompletos_NaoOperaPorContaPropria_CPF").val("");
        }
        

        if (gGradIntra_Cadastro_FlagIncluindoNovoItem)
        {
            GradIntra_Navegacao_OcultarFormularioDeNovoItem(null);

            //gGradIntra_Cadastro_ItemSendoEditado.Id = pResposta.ObjetoDeRetorno.IdCadastrado;  

            //gGradIntra_Cadastro_ItemSendoEditado.DataCadastroString = GradAux_DataDeHoje();

            //GradIntra_Navegacao_OcultarFormularioDeNovoItem(true);

            // "CallBacks" de cada sistema para quando um objeto é incluído:
            
            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_CLIENTES)
            {
                GradIntra_Clientes_ItemIncluidoComSucesso(gGradIntra_Cadastro_ItemSendoIncluido);
            }
            
            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_RISCO)
            {
                GradIntra_Risco_ItemIncluidoComSucesso(gGradIntra_Cadastro_ItemSendoIncluido);
            }
            
            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SEGURANCA)
            {
                GradIntra_Seguranca_ItemIncluidoComSucesso(gGradIntra_Cadastro_ItemSendoIncluido);
            }

            /*
                monitoramento não inclui nenhum objeto
            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_MONITORAMENTO)
            {
                GradIntra_Monitoramento_ItemIncluidoComSucesso(gGradIntra_Cadastro_ItemSendoIncluido);
            }
            */

            /*
                a parte do "Sistema" está diferente, não tem.
            if (gGradIntra_Navegacao_SistemaAtual == CONST_SISTEMA_SISTEMA)
            {
                GradIntra_Sistema_ItemIncluidoComSucesso(gGradIntra_Cadastro_ItemSendoIncluido);
            }
            */
        }

        if ("Seguranca" == gGradIntra_Navegacao_SistemaAtual
        && ("Busca" == gGradIntra_Navegacao_SubSistemaAtual)
        && ($("#cboSeguranca_DadosCompletos_Usuario_TipoAcesso").val() != 2))
        {
            $("#txtSeguranca_DadosCompletos_Usuario_CodAssessor").val("").prop("disabled", true);
            $("#txtSeguranca_DadosCompletos_Usuario_CodAssessorAssociado").val("").prop("disabled", true);
        }
    }
}


/*

    Formulários de elementos filhos

*/


function GradIntra_Cadastro_NovoItemFilho(pSender)
{
///<summary>Função para criar um novo elemento "filho", quando existe uma lista de elementos no formulário.</summary>
///<param name="pSender" type="Objeto_HTML">Objeto que originou a chamada.</param>
///<returns>void</returns>

    //verificar se não está editando...
    
    if ($(pSender).closest("table.Lista").find("tr.Editando").length > 0)
    {
        GradIntra_ExibirMensagem("A", "Favor cancelar ou confirmar a edição do item atual antes de criar um novo.", true);
        
        return false;
    }
    
    var lDivCampos = $(pSender).closest("table.Lista").next("div.pnlFormulario_Campos");

    lDivCampos.find("input, select, textarea").val("");

    lDivCampos.find("checkbox").prop("checked", false)
                                .parent().find("label")
                                         .removeClass("checked"); 

    $("#cboClientes_Enderecos_Pais").val("BRA");
    
    lDivCampos.show();
    
    if ($(pSender).closest("table").attr("id") == "tblRisco_AssociarPermissoesParametros")
    {
        $("#txt_Risco_AssociarPermissoesParametros_Valor").prop("disabled", true);
        $("#txt_Risco_AssociarPermissoesParametros_Data").prop("disabled", true);
        $(".Risco_Associacoes_Atualizacao").hide();

        var obj = $("#rdoRisco_Associacoes_Parametro");
            obj.click();
            obj.prop("checked", true)
                .parent().find("label[for='rdoRisco_Associacoes_Parametro']")
                    .addClass("checked");
       
        var obj2 = $("#rdoRisco_Associacoes_Permissao");
            obj2.prop("checked", false);
            obj2.parent()
                .find("label[for='rdoRisco_Associacoes_Permissao']")
                    .removeClass("checked");
    }

    GradIntra_Clientes_RepresentantesLegais_Load();

    GradIntra_Cadastro_SolicitacaoAlteracao_MontarTelaInclusao(lDivCampos);

     $("#txtAcoes_PendenciaCadastral_Resolucao").prop("disabled", false);

    return false;
}

function GradIntra_Cadastro_SolicitacaoAlteracao_MontarTelaInclusao( pDivCampos) 
{
    if ($(pDivCampos).attr("id") == "pnlFormulario_Campos_SolicitacaoAlteracaoCadastral") 
    {//Se for a tela de Solicitação
        
        $("#cboClientes_AlteracaoCadastral_Tipo").prop("disabled", false);
        $("#cboClientes_AlteracaoCadastral_Informacao").prop("disabled", false);
        $("#txtAcoes_AlteracaoCadastral_Descricao").prop("disabled", false);
        $("#txtAcoes_AlteracaoCadastral_Resolvido").prop("disabled", false);
        $("#txtAcoes_AlteracaoCadastral_DataResolucao").hide();
        $("#txtAcoes_AlteracaoCadastral_LoginResolucao").hide(); 
        $("#lblAcoes_AlteracaoCadastral_DataResolucao").hide();
        $("#lblAcoes_AlteracaoCadastral_LoginResolucao").hide();
        $(".BotoesSubmit").show();
        var lNomeUsuarioLogado = $("#lnkUsuario_logout").html();
        $("#txtAcoes_AlteracaoCadastral_LoginSolicitacao").val(lNomeUsuarioLogado);
        var today=new Date() ;
        var todayd=today.getDate(); 
        var todaym=today.getMonth()+1; 
        var todayy=today.getFullYear(); 
        var lHoje = todayd + "/" + todaym + "/" + todayy; 

        $("#txtAcoes_AlteracaoCadastral_DataSolicitacao").val(lHoje);
    }
}


function GradIntra_Cadastro_SalvarItemFilho(pTipoDeItem, pDivDeFormulario, pURL)
{
///<summary>Função para criar um novo elemento "filho", quando existe uma lista de elementos no formulário.</summary>
///<param name="pDivDeFormulario" type="Objeto_jQuery">Div de formulário.</param>
///<returns>void</returns>
    
    if (pDivDeFormulario.validationEngine({returnIsValid:true}))
    {        
        gGradIntra_Cadastro_ItemFilhoSendoEditado = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

        gGradIntra_Cadastro_ItemFilhoSendoEditado.ParentId = gGradIntra_Cadastro_ItemSendoEditado.Id;

        gGradIntra_Cadastro_ItemFilhoSendoEditado.IdDoFormulario = pDivDeFormulario.attr("id");
        
        gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem = pTipoDeItem;
        
        gGradIntra_Cadastro_ItemFilhoSendoEditado.BancoNome = $("#cboClientes_Conta_Banco :selected").html();

        if (pTipoDeItem == "Associacao") 
        {
            gGradIntra_Cadastro_ItemFilhoSendoEditado.CodBovespa = gGradIntra_Cadastro_ItemSendoEditado.CodBovespa;
            gGradIntra_Cadastro_ItemFilhoSendoEditado.CodBMF = gGradIntra_Cadastro_ItemSendoEditado.CodBMF;
        }
        
        if (pTipoDeItem == "ItemGrupo")
        {
            //Regras de perfil de risco; elas são especiais porque existe uma lista de inputs criados dinâmicamente
            // para os valores dos parâmetros com base na regra ecolhida; esses são mapeados pra uma 
            // List<TransportePerfilDeRiscoValorDeParametro> ValoresDosParametros, então vamos populá-la aqui:

            gGradIntra_Cadastro_ItemFilhoSendoEditado.ValoresDosParametros = new Array();

            var lInputsParametros = $("#pnlRisco_Formularios_Dados_Itens input[IdParametro]");

            lInputsParametros.each(function()
            {
                var lInput = $(this);

                gGradIntra_Cadastro_ItemFilhoSendoEditado.ValoresDosParametros.push({  IdParametro : lInput.attr("IdParametro")
                                                                                     , Valor       : lInput.val() });

            });
        }

        var lDados = { Acao            : "Salvar"
                     , ObjetoJson      : $.toJSON(gGradIntra_Cadastro_ItemFilhoSendoEditado)
                     , TipoDeObjetoPai : gGradIntra_Cadastro_ItemSendoEditado.TipoDeObjeto
                     , CdCliente       : gGradIntra_Cadastro_ItemSendoEditado.Id
                     , ClienteEmail    : gGradIntra_Cadastro_ItemSendoEditado.Email
                     , ClienteNome     : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente };

        pDivDeFormulario.find("input, select, button").prop("disabled", true);

        GradIntra_ExibirMensagem("I", "Enviando dados, por favor aguarde...");

        GradIntra_CarregarJsonVerificandoErro( pURL
                                             , lDados
                                             , GradIntra_Cadastro_SalvarItemFilho_CallBack
                                             );
    }
    else
    {
        GradIntra_ExibirMensagem("A", "Existem campos inválidos, favor verificar");
    }
    
    return false;
}


function GradIntra_Cadastro_SetaPrincipalEndereco()
{
    ///<sumary>Limpa a Propriedade FlagCorrespondencia para false se estiver setado com true</sumary>
    ///<sumary>void</sumary>
    var lTable = $("#tblCadastro_EnderecosDoCliente");

    var lInput =  lTable.find("tbody tr input[Propriedade='json']");

    if (gGradIntra_Cadastro_ItemFilhoSendoEditado.FlagCorrespondencia == true)
    {
        lInput.each(function()
        {
            var lValueInput =  $(this);

            lValueInput.val(lValueInput.val().replace('"FlagCorrespondencia":true','"FlagCorrespondencia":false','gi'));

        });
    }
}

function GradIntra_Cadastro_SetaPrincipalContaBancaria()
{
    ///<sumary>Limpa a Propriedade Principal para false se estiver setado com true</sumary>
    ///<sumary>void</sumary>
    var lTable = $("#tblCadastro_ContasDoCliente");

    var lInput =  lTable.find("tbody tr input[Propriedade='json']");

    if (gGradIntra_Cadastro_ItemFilhoSendoEditado.Principal == true)
    {
        lInput.each(function()
        {
            var lValueInput =  $(this);

            lValueInput.val(lValueInput.val().replace('"Principal":true','"Principal":false','gi'));

        });
    }
}

function GradIntra_Cadastro_SetaPrincipalTelefone()
{
    ///<sumary>Limpa a Propriedade Principal para false se estiver setado com true</sumary>
    ///<sumary>void</sumary>
    var lTable = $("#tblCadastro_TelefonesDoCliente");

    var lInput =  lTable.find("tbody tr input[Propriedade='json']");

    if (gGradIntra_Cadastro_ItemFilhoSendoEditado.Principal == true)
    {
        lInput.each(function()
        {
            var lValueInput =  $(this);

            lValueInput.val(lValueInput.val().replace('"Principal":true','"Principal":false','gi'));

        });
    }
}

function GradIntra_Cadastro_SetaPrincipalPessoasAutorizadas()
{
    ///<sumary>Limpa a Propriedade Principal para false se estiver setado com true</sumary>
    ///<sumary>void</sumary>
    var lTable = $("#tblCadastro_PessoasAutorizadas");

    var lInput =  lTable.find("tbody tr input[Propriedade='json']");

    if (gGradIntra_Cadastro_ItemFilhoSendoEditado.FlagPrincipal == true)
    {
        lInput.each(function()
        {
            var lValueInput =  $(this);
            
            if ( lValueInput.val() != "")
            {
                var lObjeto = $.evalJSON(lValueInput.val());

                if (gGradIntra_Cadastro_ItemFilhoSendoEditado.CodigoSistema == lObjeto.CodigoSistema )
                {
                    lValueInput.val(lValueInput.val().replace('"FlagPrincipal":true','"FlagPrincipal":false','gi'));
                }
            }

        });
    }
}

function GradIntra_Cadastro_SetaPrincipalItemFilho()
{
    ///<summary>Função para limpar a propriedade Principal do item filho de acordo com o formulário sendo editado</summary>
    ///<returns>void</returns>
    switch(gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem)
    {
        case "Endereco":

            GradIntra_Cadastro_SetaPrincipalEndereco();

        break;
        
        case "Conta":

            GradIntra_Cadastro_SetaPrincipalContaBancaria();

        break;

        case "Telefone":

            GradIntra_Cadastro_SetaPrincipalTelefone();

        break;

        case "Emitente":

            GradIntra_Cadastro_SetaPrincipalPessoasAutorizadas();

        break;
    }
}

function GradIntra_Cadastro_SalvarItemFilho_CallBack(pResposta)
{
///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Cadastro_SalvarItemFilho: exibe o novo item na lista e mostra mensagem.</summary>
///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
///<returns>void</returns>

    var lDivCampos = $("#" + gGradIntra_Cadastro_ItemFilhoSendoEditado.IdDoFormulario);

    if (pResposta.ObjetoDeRetorno && pResposta.ObjetoDeRetorno.IdCadastrado)
        gGradIntra_Cadastro_ItemFilhoSendoEditado.Id = pResposta.ObjetoDeRetorno.IdCadastrado;

    //Chama as funções para efetuar a configuração do item como principal
    //Exemplo: Telefone, Conta bancaria, Endereço principal
    GradIntra_Cadastro_SetaPrincipalItemFilho();

    if (null != pResposta.ObjetoDeRetorno && "DocumentacaoEntregue" == pResposta.ObjetoDeRetorno.TipoDeItem)
        GradIntra_ExibirObjetoNaLista(pResposta.ObjetoDeRetorno, lDivCampos);
    else
        GradIntra_ExibirObjetoNaLista(gGradIntra_Cadastro_ItemFilhoSendoEditado, lDivCampos);

    lDivCampos
        .find("input, select, button")
            .prop("disabled", false)
            .val("");

    if (gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem != "AlterarSenha" && gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem != "Associacao" && gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem != "Permissoes")
    {
        lDivCampos.hide();
    }
    else if (gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem == "Associacao")
    {
        var obj = $("#rdoRisco_Associacoes_Permissao");
        obj.click();
            obj.prop("checked", true)
                .parent().find("label[for='rdoRisco_Associacoes_Permissao']")
                    .addClass("checked");
       
       var obj2 = $("#rdoRisco_Associacoes_Parametro");
           obj2.prop("checked", false);
           obj2.parent()
                .find("label[for='rdoRisco_Associacoes_Parametro']")
                    .removeClass("checked");
    }

    gGradIntra_Cadastro_DadosDoItemFilhoAtual = null;

    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function GradIntra_Cadastro_SalvarItemFilhoDiretoDoGrid(pSender, pURL)
{
    var lTR = $(pSender).closest("TR");

    var lObjeto = $.evalJSON( lTR.find("input[Propriedade='json']").val() );

    var lInputs = lTR.find("td input");

    var lInput, lPropriedade, lValor;

    lInputs.each(function(){

        lInput = $(this);

        lPropriedade = lInput.parent().attr("Propriedade");
        lValor = lInput.val();

        //validações?

        if (lPropriedade && lPropriedade != "")
        {
            if (lInput.hasClass("ValorMonetario"))
            {
                if (lValor == "") lValor = "0";

                lValor = $.format.number(lValor);

                eval("lObjeto." + lPropriedade + " = " + lValor);
            }
            else
            {
                eval("lObjeto." + lPropriedade + " = '" + lValor + "'");
            }
        }
    });

    var lDados = { Acao: "Salvar", ObjetoJson: $.toJSON(lObjeto) };

    GradIntra_CarregarJsonVerificandoErro( pURL, lDados, GradIntra_Cadastro_SalvarItemFilhoDiretoDoGrid_CallBack );

    return false;
}

function GradIntra_Cadastro_SalvarItemFilhoDiretoDoGrid_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", "Dados salvados com sucesso.", true);
}

function GradIntra_Cadastro_ExcluirItemFilho(pUrl, pSender)
{
///<summary>Exclui o elemento filho atual da lista.</summary>
///<returns>void</returns>

    if (confirm("Tem certeza que deseja excluir este item?"))
    {
        var lTR = $(pSender).closest("tr");

        var lTBody = lTR.closest("tbody");

        if ((lTR.attr("rel") + "") != "")
        {
            //o elemento tinha sido incluido na lista mas não foi no ajax (cliente novo)

            var lDados = new Object();
            
            lDados.Id = lTR.attr("rel");
            lDados.Acao = "Excluir";
            lDados.CdCliente = gGradIntra_Cadastro_ItemSendoEditado.Id;

            GradIntra_CarregarJsonVerificandoErro( pUrl
                                                 , lDados
                                                 , function(pResposta) { GradIntra_Cadastro_ExcluirItemFilho_CallBack(pResposta, lTR); }
                                                 );
        }

        if (lTBody.children("tr").length == 2)
        {
            //esperando que haja 1 "Template" e 1 "Nenhuma"

            lTBody
                .find("tr.Nenhuma")
                .show()
            .parent()
                .find("thead")
                .hide();
        }
    }

    return false;
}

function GradIntra_Cadastro_ExcluirItemFilho_CallBack(pResposta, pTR)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    pTR.remove();
}

function GradIntra_Cadastro_EditarItemFilho(pSender)
{
///<summary>Edita o elemento filho.</summary>
///<param name="pSender" type="Objeto_HTML">Objeto que originou a chamada.</param>
///<returns>void</returns>

    var lSender = $(pSender);

    if (lSender.closest("tbody").find(".Editando").length > 0)
    {
        GradIntra_Cadastro_CancelarEdicaoDeItemFilho(lSender.closest("tbody").find(".CancelarEdicao:visible"), false)
    }

    var lTR = $(pSender).closest("tr");

    var lObjeto = $.evalJSON(lTR.find("td input[Propriedade='json']").val());

    var lDivCampos = lTR.closest("table").next("div.pnlFormulario_Campos");
    
    lObjeto.Estado = "Modificado";

    GradIntra_ExibirObjetoNoFormulario(lObjeto, lDivCampos);

    GradIntra_Cadastro_SolicitacaoAlteracao_MontarTelaAlteracao(lObjeto, lDivCampos);

    GradIntra_Cadastro_PendenciaCadastral_MontarTelaAlteracao(lObjeto, lDivCampos);

    GradIntra_Cadastro_Enderecos_InabilitarEstadoQuandoEstrangeiro($("#cboClientes_Enderecos_Pais"), $("#cboClientes_Enderecos_Estado"));

    lTR.addClass("Editando");
    if (lTR.closest("table").attr("id") == "tblRisco_AssociarPermissoesParametros")
    {
        var obj = $("#rdoRisco_Associacoes_Parametro");
        obj.click();
        obj.prop("checked", true)
                .parent().find("label[for='rdoRisco_Associacoes_Parametro']")
                    .addClass("checked");

        var obj2 = $("#rdoRisco_Associacoes_Permissao");
        obj2.prop("checked", false);
        obj2.parent()
                .find("label[for='rdoRisco_Associacoes_Permissao']")
                    .removeClass("checked");

        $("#txt_Risco_AssociarPermissoesParametros_Valor").prop("disabled", true);
        $("#txt_Risco_AssociarPermissoesParametros_Data").prop("disabled", true);
        $(".Editar").prop("disabled", true);
        $(".Menor1").hide();
        $(".Risco_Associacoes_Atualizacao").show();

        $("#cbo_Risco_AssociarPermissoesParametros_Parametro").hide();
        $("#cbo_Risco_AssociarPermissoesParametros_Grupo").hide();
        $("#lbl_Risco_AssociarPermissoesParametrosAssociarPermissoesParametros_Parametro").hide();
        $("#txt_Risco_AssociarPermissoesParametros_Parametro").html($("#cbo_Risco_AssociarPermissoesParametros_Parametro option:selected").text()).show();
        $("#txt_Risco_AssociarPermissoesParametros_Grupo").html(" - " + $("#cbo_Risco_AssociarPermissoesParametros_Grupo option:selected").text()).show();
    }

    return false;
}

function GradIntra_Cadastro_PendenciaCadastral_MontarTelaAlteracao(lObjeto, lDivCampos)
{ 
    if (lObjeto.TipoDeItem == "PendenciaCadastral") 
    {//Se for a tela de Pendencia
        $("#txtAcoes_PendenciaCadastral_DataResolucao").prop("disabled", true);
        $("#txtAcoes_PendenciaCadastral_LoginResolucao").prop("disabled", true);      
        
        if (lObjeto.FlagResolvido)
        {
            //Tratando os que acabaram de ser resolvidos
            if ($("#txtAcoes_PendenciaCadastral_LoginResolucao").val() == null || $("#txtAcoes_PendenciaCadastral_LoginResolucao").val() == "" || $("#txtAcoes_PendenciaCadastral_LoginResolucao").val() == "null" )
            {
                var lNomeUsuarioLogado = $("#lnkUsuario_logout").html();
                $("#txtAcoes_PendenciaCadastral_LoginResolucao").val(lNomeUsuarioLogado);
            }
            if ($("#txtAcoes_PendenciaCadastral_DataResolucao").val() == null || $("#txtAcoes_PendenciaCadastral_DataResolucao").val() == "" || $("#txtAcoes_PendenciaCadastral_DataResolucao").val() == "null" )
            {
                var today=new Date() ;
                var todayd=today.getDate(); 
                var todaym=today.getMonth()+1; 
                var todayy=today.getFullYear();
                if (todayd   < 10)  todayd = "0"+todayd;
                if (todaym   < 10)  todaym = "0"+todaym; 
                var lHoje = todayd + "/" + todaym + "/" + todayy; 
                $("#txtAcoes_PendenciaCadastral_DataResolucao").val(lHoje);
            }
            //desabilitando campos
            $("#txtAcoes_PendenciaCadastral_Resolvido").prop("disabled", true);
            $("#txtAcoes_PendenciaCadastral_Resolucao").prop("disabled", true);
        }
        else
        {
            $("#txtAcoes_PendenciaCadastral_Resolvido").prop("disabled", false);
            $("#txtAcoes_PendenciaCadastral_Resolucao").prop("disabled", false);
        }
    }
}

function GradIntra_Cadastro_SolicitacaoAlteracao_MontarTelaAlteracao(pObjeto, pDivCampos) 
{
    if ($(pDivCampos).attr("id") == "pnlFormulario_Campos_SolicitacaoAlteracaoCadastral") 
    {//Se for a tela de Solicitação
        //DesabilitarTudo...
        $("#cboClientes_AlteracaoCadastral_Tipo").prop("disabled", true);
        $("#cboClientes_AlteracaoCadastral_Informacao").prop("disabled", true);
        $("#txtAcoes_AlteracaoCadastral_Descricao").prop("disabled", true);
        $("#txtAcoes_AlteracaoCadastral_Resolvido").prop("disabled", true);
        $("#txtAcoes_AlteracaoCadastral_DataSolicitacao").prop("disabled", true);
        $("#txtAcoes_AlteracaoCadastral_LoginSolicitacao").prop("disabled", true);
        
        if (pObjeto.StResolvido != null && pObjeto.StResolvido)
         {  //Já resolvido
            $(".BotoesSubmit").hide();
            $("#txtAcoes_AlteracaoCadastral_DataResolucao").show();
            $("#txtAcoes_AlteracaoCadastral_LoginResolucao").show();
            $("#lblAcoes_AlteracaoCadastral_DataResolucao").show();
            $("#lblAcoes_AlteracaoCadastral_LoginResolucao").show();

            //Tratando os que acabaram de ser resolvidos
            $("#txtAcoes_AlteracaoCadastral_DataResolucao").prop("disabled", false);
            $("#txtAcoes_AlteracaoCadastral_LoginResolucao").prop("disabled", false);
            if ($("#txtAcoes_AlteracaoCadastral_LoginResolucao").val() == null || $("#txtAcoes_AlteracaoCadastral_LoginResolucao").val() == "" || $("#txtAcoes_AlteracaoCadastral_LoginResolucao").val() == "null" )
            {
                var lNomeUsuarioLogado = $("#lnkUsuario_logout").html();
                $("#txtAcoes_AlteracaoCadastral_LoginResolucao").val(lNomeUsuarioLogado);
            }
            if ($("#txtAcoes_AlteracaoCadastral_DataResolucao").val() == null || $("#txtAcoes_AlteracaoCadastral_DataResolucao").val() == "" || $("#txtAcoes_AlteracaoCadastral_DataResolucao").val() == "null" )
            {
                var today=new Date() ;
                var todayd=today.getDate(); 
                var todaym=today.getMonth()+1; 
                var todayy=today.getFullYear(); 
                if (todayd   < 10)  todayd = "0"+todayd;
                if (todaym   < 10)  todaym = "0"+todaym; 
                var lHoje = todayd + "/" + todaym + "/" + todayy; 
                $("#txtAcoes_AlteracaoCadastral_DataResolucao").val(lHoje);
            }
            $("#txtAcoes_AlteracaoCadastral_DataResolucao").prop("disabled", true);
            $("#txtAcoes_AlteracaoCadastral_LoginResolucao").prop("disabled", true);

        }
        else
        {   //Não resolvido ainda
            $("#txtAcoes_AlteracaoCadastral_DataResolucao").hide();
            $("#txtAcoes_AlteracaoCadastral_LoginResolucao").hide(); 
            $("#lblAcoes_AlteracaoCadastral_DataResolucao").hide();
            $("#lblAcoes_AlteracaoCadastral_LoginResolucao").hide();
            //habilitar ckbResolvido
            $("#txtAcoes_AlteracaoCadastral_Resolvido").prop("disabled", false);
            $(".BotoesSubmit").show();
        }
    }
}

function GradIntra_Cadastro_CancelarEdicaoDeItemFilho(pSender, pPerguntaConfirmaExclusao)
{
///<summary>Cancela edição do elemento filho atual.</summary>
///<param name="pSender" type="Objeto_HTML">Objeto que originou a chamada.</param>
///<returns>void</returns>
    
    if (pPerguntaConfirmaExclusao == null)
        pPerguntaConfirmaExclusao = true;

    if (!pPerguntaConfirmaExclusao || confirm("Descartar as alterações feitas neste item?"))
    {
        var lTR = $(pSender).closest("tr");

        var lDivCampos = lTR.closest("table").next("div.pnlFormulario_Campos");

        lTR.removeClass("Editando");

        lDivCampos
            .find("input, select, button")
                .val("");

        if (lTR.closest("table").attr("id") == "tblRisco_AssociarPermissoesParametros") 
        {
            $("#txt_Risco_AssociarPermissoesParametros_Valor").prop("disabled", false);
            $("#txt_Risco_AssociarPermissoesParametros_Data").prop("disabled", false);
            $(".Editar").prop("disabled", false);
            $(".Menor1").show();
            $(".Risco_Associacoes_Atualizacao").hide();
          
            var obj = $("#rdoRisco_Associacoes_Permissao");
                obj.click();
                obj.prop("checked", true)
                    .parent().find("label[for='rdoRisco_Associacoes_Permissao']")
                        .addClass("checked");

            var obj2 = $("#rdoRisco_Associacoes_Parametro");
                obj2.prop("checked", false);
                obj2.parent()
                    .find("label[for='rdoRisco_Associacoes_Parametro']")
                        .removeClass("checked");
        }
        else
        {
            lDivCampos.hide();
        }
    }

    return false;
}

function GradIntra_Cadastro_Enderecos_InabilitarEstadoQuandoEstrangeiro(pComboNacionalidade, pCampo)
{
    if (pComboNacionalidade.val() != 'BRA')
    {
        $(pCampo)
            .removeClass('validate[required]')
            .prop("disabled", true)
            .val('');
    }
    else
    {
        $(pCampo)
             .addClass('validate[required]')
             .prop("disabled", false);
    }

}

function GradIntra_Cadastro_InabilitarEstadoQuandoEstrangeiro(pValor, pCombo)
{
    if (pValor.val() == '2' || pValor.val() == '3' )
    {
         $(pCombo)
             .removeClass('validate[required]')
             .prop("disabled", true)
             .val('');
        
        var lComboBrasil = $("#cboClientes_DadosCompletos_PaisDeNascimento option[value='BRA']");

        lComboBrasil
            .hide();

        if (lComboBrasil.parent().val() == "BRA")
            lComboBrasil.parent().val("");
    }
    else
    {
         $(pCombo)
             .addClass('validate[required]')
             .prop("disabled", false);

        $("#cboClientes_DadosCompletos_PaisDeNascimento option[value='BRA']")
            .show()
            .parent()
            .val("BRA")
    }

    return false;
}

function GradIntra_Cadastro_InabilitarDadosComerciaisPF(pValorSelecionado, pLabelCargoAtual, pLabelEmpresa, pComboRamoAtividade)
{
    if (pValorSelecionado.find("option:selected").text() == "ESTUDANTE")
    {
        $(pLabelCargoAtual)
            .val('')
            .prop("disabled", true)
            .removeClass("validate[required,length[5,40]]");
        
        $(pLabelEmpresa)
            .val('')
            .prop("disabled", true)
            .removeClass("validate[required,length[5,60]]");
        
        $(pComboRamoAtividade)
             .removeClass('validate[required]')
             .prop("disabled", true)
             .val('');
    }
    else
    {
        $(pLabelCargoAtual)
            .prop("disabled", false)
            .addClass("validate[required,length[5,40]]");

        $(pLabelEmpresa)
            .prop("disabled", false)
            .addClass("validate[required,length[5,60]]");

        $(pComboRamoAtividade)
             .addClass('validate[required]')
             .prop("disabled", false);
    }
    
    return false;
}

function GradIntra_Cadastro_EstadoCivil(pValorSelecionado, pCampoAcao)
{
//    var lCampoAcao = $("#txtClientes_DadosCompletos_Conjuge");

    if (pValorSelecionado.val() == 1
    || (pValorSelecionado.val() == 2)
    || (pValorSelecionado.val() == 3)
    || (pValorSelecionado.val() == 4)
    || (pValorSelecionado.val() == 9))
    {
         $(pCampoAcao)
            .removeClass("validate[length[5,60]]")
            .prop("disabled", true)
            .val("");
    }
    else
    {
        $(pCampoAcao)
        .addClass("validate[length[5,60]]")
        .prop("disabled", false);
    }

    return false;
}

function GradIntra_Cadastro_AtivarInativarCliente(pDivDeFormulario ,pUrl)
{
    var lListaDeChecks = $("#pnlClientes_Formularios_Acoes_Inativar table.GridIntranet tbody input[type='checkbox']:checked");
    var lIdsChecados = "";
    
    lListaDeChecks.each(function()
    {
        lIdsChecados += $(this).attr("id") + ",";
    });

        var lDados = {   Acao: "Salvar"
                       , IdCliente:     $("#hdAcoes_Inativar_Id_Cliente").val()
                       , StAtivoLogin:  $("#chkAcoes_AtivarInativar_Login_Confirma").is(":checked")
                       , StAtivoCliGer: $("#chkAcoes_AtivarInativar_CliGer_Confirma").is(":checked")
                       //, StAtivoHb:     $("#chkAcoes_AtivarInativar_HB_Confirma").is(":checked")
                       , StAtivoHb: true
                       , DsCkbTrue:     lIdsChecados
                     };

        GradIntra_ExibirMensagem("I", "Enviando dados, por favor aguarde...");

        GradIntra_CarregarJsonVerificandoErro(  pUrl
                                              , lDados
                                              , GradIntra_Cadastro_AtivarInativarCliente_CallBack
                                             );
}

function GradIntra_Cadastro_AtivarInativarCliente_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Clientes_RepresentantesLegais_Load()
{
    $("#cboClientes_Representantes_OrgaoEmissor").val("SSP");
    $("#cboClientes_Representantes_TipoDocumento").val("RG");
}

function GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor_Click(pSender)
{
        var lDados = { Acao: "NotificarAssessor"
                     , CodAssessor: gGradIntra_Cadastro_ItemSendoEditado.CodAssessor
                     , CodCliente: gGradIntra_Cadastro_ItemSendoEditado.Id
                     , CodBovespa: gGradIntra_Cadastro_ItemSendoEditado.CodBovespa
                     , CodBMF: gGradIntra_Cadastro_ItemSendoEditado.CodBMF };

        GradIntra_ExibirMensagem("I", "Enviando e-mail para o assessor com as pendências do cliente " + gGradIntra_Cadastro_ItemSendoEditado.NomeCliente +", por favor aguarde...");

        GradIntra_CarregarJsonVerificandoErro( "Clientes/Formularios/Acoes/VerPendencias.aspx"
                                             , lDados
                                             , GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor_CallBack
                                             );
    return false;
}

function GradIntra_Cadastro_PendenciaCadastral_NotificarAsessor_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Cadastro_Assessores_Gerais_Load()
{
    var lIdAssessorLogado = $("#hddIdAssessorLogado").val();

    if (lIdAssessorLogado && "" != lIdAssessorLogado)
    {
        var lComboBusca_FiltroRelatorioRisco_Assessor = $("#cboClientes_DadosCompletos_Assessor");
        lComboBusca_FiltroRelatorioRisco_Assessor.prop("disabled", true);
        lComboBusca_FiltroRelatorioRisco_Assessor.val(lIdAssessorLogado);
    }
}

function GradIntra_Cadastro_DadosCompletos_OperaPorContaPropria_Click(pSender)
{
    var lSender = $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim");

    if ( lSender.is(":checked") )
    {
        $("#divNaoOperaPorContaPropria").hide();
        $("#txtClientes_DadosCompletos_NaoOperaPorContaPropria_Nome").removeClass("validate[required,length[3,60]]");
        //$("#txtClientes_DadosCompletos_NaoOperaPorContaPropria_CPF").removeClass("validate[funcCall[validatecpf]]");
    }
    else
    {
        $("#divNaoOperaPorContaPropria").show();
        $("#txtClientes_DadosCompletos_NaoOperaPorContaPropria_Nome").addClass("validate[required,length[3,60]]");
        //$("#txtClientes_DadosCompletos_NaoOperaPorContaPropria_CPF").addClass("validate[funcCall[validatecpf]]");
    }

    return false;
}


function Cadastro_PreencherTeste()
{
    var lEmail, lCPF;

    lCPF = prompt("CPF de teste (com ou sem pontuação, branco para randomizar):", "");

    if(lCPF = null || lCPF == "")
    {
        lCPF = cpf_gerar(false);
    }

    if(lCPF.indexOf(".") == -1)
    {
        lCPF = lCPF.substr(0,3) + "." + lCPF.substr(3, 3) + "." + lCPF.substr(6, 3) + "-" + lCPF.substr(9, 2);
    }

    lEmail = prompt("Email de teste", "teste____@teste.com");

    $("#cboClientes_DadosCompletos_Tipo").val("1");

    $("#txtClientes_DadosCompletos_NomeCliente").val("Teste Testerson");
    $("#txtClientes_DadosCompletos_CPF").val(lCPF);
    $("#txtClientes_DadosCompletos_Email").val(lEmail);
    $("#cboClientes_DadosCompletos_Assessor").val("0");
    $("#cboClientes_DadosCompletos_Nacionalidade").val("1");
    $("#txtClientes_DadosCompletos_Documento_DataNascimento").val("01/01/1971");
    $("#cboClientes_DadosCompletos_Sexo").val("M");
    $("#cboClientes_DadosCompletos_PaisDeNascimento").val("BRA");
    $("#cboClientes_DadosCompletos_EstadoDeNascimento").val("SP");
    $("#txtClientes_DadosCompletos_CidadeDeNascimento").val("São José dos Campos");
    $("#cboClientes_DadosCompletos_EstadoCivil").val("1");
    $("#cboClientes_DadosCompletos_Profissao").val("135");
    $("#txtClientes_DadosCompletos_CargoAtual").val("ARQUEÓLOGO");
    $("#txtClientes_DadosCompletos_Empresa").val("MUSEU DO CAIRO");
    $("#txtClientes_DadosCompletos_NomeDaMae").val("Testersona de Almeida");
    $("#txtClientes_DadosCompletos_NomeDoPai").val("Testerson Pai");
    $("#cboClientes_DadosCompletos_TipoDeDocumento").val("RG");
    $("#cboClientes_DadosCompletos_OrgaoEmissor").val("SSP");
    $("#txtClientes_DadosCompletos_Documento_Numero").val("34.655.665-2");
    $("#txtClientes_DadosCompletos_Documento_DataEmissao").val("01/01/1981");
    $("#cboClientes_DadosCompletos_Documento_EstadoEmissao").val("SP");
}

function Cadastro_PJ_PreencherTeste()
{
    $("#cboClientes_DadosCompletos_Tipo").val("4");
    $("#cboClientes_DadosCompletos_PaisDeNascimento").val("BRA");
    $("#cboClientes_DadosCompletos_ObjetivoSocial").val("402");
    $("#txtClientes_DadosCompletos_RazaoSocial").val("BANCO TESTECORP");
    $("#txtClientes_DadosCompletos_NomeFantasia").val("TESTECORP BANK");
    $("#cboClientes_DadosCompletos_Assessor").val("750");

    lEmail = prompt("Email de teste", "teste____@teste.com");

    $("#txtClientes_DadosCompletos_Email").val(lEmail);

    $("#txtClientes_DadosCompletos_CNPJ").val(cnpj_gerar());
    $("#txtClientes_DadosCompletos_DataDeConstituicao").val("01/01/2001");
    $("#txtClientes_DadosCompletos_NIRE").val("11111");
    $("#cboClientes_DadosCompletos_PrincipalAtividade").val("99")
    $("#txtClientes_DadosCompletos_FormaDeConstituicao").val("Teste");
    $("#txtClientes_DadosCompletos_PropositoGradual").val("Teste Proposito");

    return false;
}

function cpf_randomiza(n)
{
    var ranNum = Math.round(Math.random()*n);
    return ranNum;
}

function cpf_mod(dividendo,divisor)
{
    return Math.round(dividendo - (Math.floor(dividendo/divisor)*divisor));
}

function cnpj_gerar()
{
    var comPontos = true;
   
    var n = 9;
    var n1 = cpf_randomiza(n);
    var n2 = cpf_randomiza(n);
    var n3 = cpf_randomiza(n);
    var n4 = cpf_randomiza(n);
    var n5 = cpf_randomiza(n);
    var n6 = cpf_randomiza(n);
    var n7 = cpf_randomiza(n);
    var n8 = cpf_randomiza(n);
    var n9 = 0; //randomiza(n);
    var n10 = 0; //randomiza(n);
    var n11 = 0; //randomiza(n);
    var n12 = 1; //randomiza(n);
    var d1 = n12*2+n11*3+n10*4+n9*5+n8*6+n7*7+n6*8+n5*9+n4*2+n3*3+n2*4+n1*5;
    d1 = 11 - ( cpf_mod(d1,11) );
    
    if (d1>=10) d1 = 0;
    var d2 = d1*2+n12*3+n11*4+n10*5+n9*6+n8*7+n7*8+n6*9+n5*2+n4*3+n3*4+n2*5+n1*6;
    d2 = 11 - ( cpf_mod(d2,11) );
    if (d2>=10) d2 = 0;
    retorno = '';
    if (comPontos) cnpj = ''+n1+n2+'.'+n3+n4+n5+'.'+n6+n7+n8+'/'+n9+n10+n11+n12+'-'+d1+d2;
    else cnpj = ''+n1+n2+n3+n4+n5+n6+n7+n8+n9+n10+n11+n12+d1+d2;

   return cnpj;

}


function cpf_gerar(comPontos)
{
    var n = 9;
    var n1 = cpf_randomiza(n);
    var n2 = cpf_randomiza(n);
    var n3 = cpf_randomiza(n);
    var n4 = cpf_randomiza(n);
    var n5 = cpf_randomiza(n);
    var n6 = cpf_randomiza(n);
    var n7 = cpf_randomiza(n);
    var n8 = cpf_randomiza(n);
    var n9 = cpf_randomiza(n);
    var d1 = n9*2+n8*3+n7*4+n6*5+n5*6+n4*7+n3*8+n2*9+n1*10;

    d1 = 11 - ( cpf_mod(d1,11) );
    if (d1>=10) d1 = 0;

    var d2 = d1*2+n9*3+n8*4+n7*5+n6*6+n5*7+n4*8+n3*9+n2*10+n1*11;
    d2 = 11 - ( cpf_mod(d2,11) );

    if (d2>=10) d2 = 0;

    var retorno = '';

    if (comPontos) retorno = ''+n1+n2+n3+'.'+n4+n5+n6+'.'+n7+n8+n9+'-'+d1+d2;
    else retorno = ''+n1+n2+n3+n4+n5+n6+n7+n8+n9+d1+d2;

    return retorno;
}

function rdoCadastro_DadosCompletos_DesejaAplicar_PF_Change(pSender) 
{
    if (pSender.id == "rdoCadastro_DadosCompletos_DesejaAplicar_Cambio") 
    {
        if ($("#rdoCadastro_DadosCompletos_DesejaAplicar_Cambio").is(":checked") && !$("#rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa").is(":checked") && !$("#rdoCadastro_DadosCompletos_DesejaAplicar_Fundos").is(":checked") && !$("#rdoCadastro_DadosCompletos_DesejaAplicar_Ambos").is(":checked")) 
        {
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").attr('disabled', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").attr('disabled', true);

            $("#rdoCadastro_DadosCompletos_Emancipado_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_Emancipado_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_Emancipado_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_Emancipado_Nao").prop('disabled', true);

            $("#rdoCadastro_DadosCompletos_PPE_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_PPE_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_PPE_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_PPE_Nao").prop('disabled', true);

            $("#rdoCadastro_DadosCompletos_CVM387_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_CVM387_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_CVM387_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_CVM387_Nao").prop('disabled', true);

            $("#rdoCadastro_DadosCompletos_USPerson_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_USPerson_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_USPerson_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_USPerson_Nao").prop('disabled', true);

            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_SimG").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Nao").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_SimG").prop('disabled', true);

            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('disabled', true);

            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('disabled', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('disabled', true);

            $("rdoCadastro_DadosCompletos_Ciencia_Regulamento").prop('checked', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Prospecto").prop('checked', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Lamina").prop('checked', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Regulamento").prop('disabled', true);
            $("rdoCadastro_DadosCompletos_Ciencia_Prospecto").prop('disabled', true);
            $("rdoCadastro_DadosCompletos_Ciencia_Lamina").prop('disabled', true);
        }
        else 
        {
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").attr('disabled', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").attr('disabled', false);

            $("#rdoCadastro_DadosCompletos_Emancipado_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_Emancipado_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_Emancipado_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_Emancipado_Nao").prop('disabled', false);

            $("#rdoCadastro_DadosCompletos_PPE_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_PPE_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_PPE_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_PPE_Nao").prop('disabled', false);

            $("#rdoCadastro_DadosCompletos_CVM387_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_CVM387_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_CVM387_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_CVM387_Nao").prop('disabled', false);

            $("#rdoCadastro_DadosCompletos_USPerson_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_USPerson_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_USPerson_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_USPerson_Nao").prop('disabled', false);

            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Nao").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Sim").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_SimG").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Nao").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_PessoaVinculada_SimG").prop('disabled', false);

            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('disabled', false);

            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('checked', true);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('checked', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim").prop('disabled', false);
            $("#rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao").prop('disabled', false);

            $("rdoCadastro_DadosCompletos_Ciencia_Regulamento").prop('checked', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Prospecto").prop('checked', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Lamina").prop('checked', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Regulamento").prop('disabled', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Prospecto").prop('disabled', false);
            $("rdoCadastro_DadosCompletos_Ciencia_Lamina").prop('disabled', false);
        }
    }
}

function rdoCadastro_DadosCompletos_DesejaAplicar_PJ_Change(pSender)
{
    if ($('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked") && !$('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && !$('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && !$('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').is(":checked")) 
    {
        $('#pnlNovoItem [id*="chkCadastro_DadosCompletos_CienteRegulamento"]').prop('disabled', true);
        $('#pnlNovoItem [id*="chkCadastro_DadosCompletos_CienteProspecto"]').prop('disabled', true);
        $('#pnlNovoItem [id*="chkCadastro_DadosCompletos_CienteLamina"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PessoaVinculada_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PessoaVinculada_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PPE_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PPE_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_CVM387_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_CVM387_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_USPersonNacional_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_USPersonNacional_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonResidente_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonResidente_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonGreen_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonGreen_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonPresenca_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonPresenca_Nao"]').prop('disabled', true);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonNascido_Sim"]').prop('disabled', true);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonNascido_Nao"]').prop('disabled', true);
    }
    else 
    {
        $('#pnlNovoItem [id*="chkCadastro_DadosCompletos_CienteRegulamento"]').prop('disabled', false);
        $('#pnlNovoItem [id*="chkCadastro_DadosCompletos_CienteProspecto"]').prop('disabled', false);
        $('#pnlNovoItem [id*="chkCadastro_DadosCompletos_CienteLamina"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PessoaVinculada_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PessoaVinculada_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PPE_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_PPE_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_CVM387_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_CVM387_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_USPersonNacional_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_DadosCompletos_USPersonNacional_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonResidente_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonResidente_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonGreen_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonGreen_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonPresenca_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonPresenca_Nao"]').prop('disabled', false);

        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonNascido_Sim"]').prop('disabled', false);
        $('#pnlNovoItem [id*="rdoCadastro_PFPasso3_USPersonNascido_Nao"]').prop('disabled', false);
    }
}
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

//  Constantes de classes HTML que indicam estados dos objetos na página

var CONST_CLASS_CARREGANDOCONTEUDO   = "CarregandoConteudo";       // div que está com um request ajax em andamento para carregar seu 
var CONST_CLASS_CONTEUDOCARREGADO    = "ConteudoCarregado";        // div que já carregou seu conteúdo
var CONST_CLASS_CAMPOSPREENCHIDOS    = "CamposPreenchidos";        // indica que os campos já foram preenchidos dentro do próprio item selecionado //TODO: ainda não foi implementada a diferença dessa classe estar setada ou não (02-Navegaca.js função GradIntra_Navegacao_ExibirFormulario > linha 655)
var CONST_CLASS_ITEM_SELECIONADO     = "Selecionada";              // indica um item de interface que está selecionado
var CONST_CLASS_ITEM_EXPANDIDO       = "Expandida";                // indica um item de interface que está expandido
var CONST_CLASS_TEMPLATE             = "Template";                 // indica um objeto HTML que serve como "template" pra clonagem de "databind" javascript
var CONST_CLASS_ITEM_ITEMDINAMICO    = "ItemDinamico";             // indica um objeto HTML que foi clonado a partir de um template (usado pra poder selecionar os itens que são "clones" facilmente)
var CONST_CLASS_PICKERDEDATA         = "Picker_Data";              // indica um botão que serve como picker pro controle DatePicker javascript
var CONST_CLASS_BLOQUEADOPORINCLUSAO = "BloqueadoPorNovaInclusao"  // indica controles desabilitados porque está acontecendo uma inclusão de novo item

// Constantes dos nomes dos sistemas da intranet

var CONST_SISTEMA_CLIENTES      = "Clientes";
var CONST_SISTEMA_RISCO         = "Risco";
var CONST_SISTEMA_COMPLIANCE    = "Compliance";
var CONST_SISTEMA_PAPEIS        = "Papeis";
var CONST_SISTEMA_SEGURANCA     = "Seguranca";
var CONST_SISTEMA_MONITORAMENTO = "Monitoramento";
var CONST_SISTEMA_SISTEMA       = "Sistema";
var CONST_SISTEMA_RELATORIOS    = "Relatorios";
var CONST_SISTEMA_DBM           = "DBM";
var CONST_SISTEMA_HOMEBROKER    = "HomeBroker";
var CONST_SISTEMA_SOLICITACOES  = "Solicitacoes";

// Constantes dos nomes dos subsistemas da intranet; a princípio,
//   foi considerado que eles fossem "repetíveis" entre os sistemas,
//   ou seja, o subsistema "Novo" seria a mesma string para qualquer
//   novo objeto (usuário, cliente, grupo, etc). Porém, conforme a 
//   intranet foi crescendo, foi melhor separar alguns sub-sistemas.
//   Logo, hoje estamos utilizando uma metodologia híbrida; os
//   js suportam strings de sub-sistemas repetidos, porém caso
//   seja necessário, pode-se "separar" um subsistema e dar-lhe
//   um nome próprio. 

var CONST_SUBSISTEMA_BUSCA                         = "Busca";
var CONST_SUBSISTEMA_MIGRACAO                      = "Migracao";
var CONST_SUBSISTEMA_SINCRONIZACAO                 = "Sincronizacao";
var CONST_SUBSISTEMA_RELATORIOS                    = "Relatorios";
var CONST_SUBSISTEMA_RELATORIOS_DBM                = "RelatoriosDBM";
var CONST_SUBSISTEMA_USUARIOSLOGADOS               = "UsuariosLogados";
var CONST_SUBSISTEMA_ORDENS                        = "Ordens";
var CONST_SUBSISTEMA_ORDENS_NOVO_OMS               = "OrdensNovoOMS";
var CONST_SUBSISTEMA_ORDENS_STOP                   = "OrdensStop";
var CONST_SUBSISTEMA_ORDENS_RELATORIOS             = "Relatorios";
var CONST_SUBSISTEMA_OBJETOS                       = "ObjetosDoSistema";
var CONST_SUBSISTEMA_VARIAVEIS                     = "VariaveisDoSistema";
var CONST_SUBSISTEMA_PESSOASVINCULADAS             = "PessoasVinculadas";
var CONST_SUBSISTEMA_VALIDACAOROCKET               = "ValidacaoCadastralRocket";
var CONST_SUBSISTEMA_PESSOASEXPOSTASPOL            = "PessoasExpostasPoliticamente";
var CONST_SUBSISTEMA_ASSOCIAR_PERMISSOES           = "AssociarPermissoes";
var CONST_SUBSISTEMA_MONITORAMENTO_CLUBES_E_FUNDOS = "ClubesEFundos";
var CONST_SUBSISTEMA_MONITORAMENTO_TERMOS          = "Termos";
var CONST_SUBSISTEMA_IMPORTACAO                    = "Importacao";
var CONST_SUBSISTEMA_RESERVAIPO                    = "ReservaIPO";
var CONST_SUBSISTEMA_LINK_PROSPECT                 = "LinkProspect";
var CONST_SUBSISTEMA_ALTERAR_SENHA                 = "AlterarSenha";
var CONST_SUBSISTEMA_PARAMETROS_GLOBAIS            = "ParametrosGlobais";
var CONST_SUBSISTEMA_GERAL                         = "Geral";
var CONST_SUBSISTEMA_DESBLOQUEIOCUSTODIA           = "DesbloqueioCustodia";
var CONST_SUBSISTEMA_AVISOS                        = "Avisos";
var CONST_SUBSISTEMA_POUPE_DIRECT                  = "PoupeDirect";
var CONST_SUBSISTEMA_POUPE_OPERACAO                = "PoupeOperacoes";
var CONST_SUBSISTEMA_ESTATISTICA_DAYTRADE          = "EstatisticaDayTrade";
var CONST_SUBSISTEMA_VENDAS_DE_FERRAMENTAS         = "VendasDeFerramentas";
var CONST_SUBSISTEMA_ORDENS_ALTERADAS_DAYTRADE     = "OrdensAlteradasDayTrade";
var CONST_SUBSISTEMA_NEGOCIOS_DIRETOS              = "NegociosDiretos";
var CONST_SUBSISTEMA_AUTORIZACOES                  = "AutorizacoesDeCadastro";
var CONST_SUBSISTEMA_CAMBIO                        = "CadastroDeCambio";
var CONST_SUBSISTEMA_IPO                           = "GerenciamentoIPO";
var CONST_SUBSISTEMA_SOLICITACOES_RESGATE          = "SolicitacoesResgate"
var CONST_SUBSISTEMA_INTEGRCAO_ROCKET              = "IntegracaoRocket";

var CONST_SUBSISTEMA_NOVO_PF                       = "Novo_PF";
var CONST_SUBSISTEMA_NOVO_PJ                       = "Novo_PJ";
var CONST_SUBSISTEMA_NOVO_CB                       = "Novo_CB";
var CONST_SUBSISTEMA_NOVO_USUARIO                  = "Novo_Usuario";
var CONST_SUBSISTEMA_NOVO_GRUPO                    = "Novo_Grupo";
var CONST_SUBSISTEMA_NOVO_PERFIL                   = "Novo_Perfil";
var CONST_SUBSISTEMA_NOVO_PERMISSAOSEGURANCA       = "Novo_PermissaoSeguranca";
var CONST_SUBSISTEMA_NOVO_PAPEL                    = "Novo_Papel";
var CONST_SUBSISTEMA_NOVO_GRUPODERISCO             = "Novo_GrupoDeRisco";
var CONST_SUBSISTEMA_NOVO_PARAMETRODERISCO         = "Novo_ParametroDeRisco";
var CONST_SUBSISTEMA_NOVO_PERMISSAODERISCO         = "Novo_PermissaoDeRisco";


// Constantes dos tipos de objeto no sistema de segurança

var CONST_SEGURANCA_TIPODEOBJETO_USUARIO = "Usuario";
var CONST_SEGURANCA_TIPODEOBJETO_GRUPO   = "Grupo";
var CONST_SEGURANCA_TIPODEOBJETO_PERFIL = "Perfil";
var CONST_SEGURANCA_TIPODEOBJETO_PERMISSAOSEGURANCA = "PermissaoSeguranca";

// Constantes dos tipos de objeto no sistema de risco

var CONST_RISCO_TIPODEOBJETO_PERMISSAO = "Permissao";
var CONST_RISCO_TIPODEOBJETO_GRUPO   = "Grupo";
var CONST_RISCO_TIPODEOBJETO_PARAMETRO  = "Parametro";


// Constantes dos resultados de suitability

var CONST_SUITABILITY_RESULTADO_0  = "CadastroNaoFinalizado";
var CONST_SUITABILITY_RESULTADO_1  = "BaixoRisco";
var CONST_SUITABILITY_RESULTADO_2  = "MedioRiscoSemRendaVariavel";
var CONST_SUITABILITY_RESULTADO_3  = "MedioRiscoComRendaVariavel";
var CONST_SUITABILITY_RESULTADO_4  = "Arrojado";

//Constantes de alertas

var CONST_ALERT_CONFIRMA_CANCELAR_INCLUSAO = "Deseja cancelar o cadastro?";

// Variáveis globais da Intranet

var gGradIntra_Config_DelayDeRequest = 200;                      // Delay em ms para fazer um request pro servidor (geralmente os de "CarregarHtml") - usado para que o indicador de "aguarde" não pisque muito rápido, o usuário consiga ver uma indicação antes do request ir pro servidor.
var gGradIntra_Config_DuracaoDaAnimacaoDeMensagem = 500;         // Duração em ms da animação de "ExibirMensagem"

var gGradIntra_MensagemOriginal = null;                          // Mensagem inicial que aparece no div de mensagens quando a página é carregada, que deve exibir um "Olá, <usuário>". Assim o js sabe como "voltar" pra mensagem original depois de exibir um alerta por <gGradIntra_MensagemOriginalTimeout> segundos.
var gGradIntra_MensagemOriginalTimeout = 4000;                   // Tempo em ms de duração da mensagem quando usarem a flag para "voltar" pra mensagem original

var gGradIntra_RedirecionarParaLoginEm = 0;                      // Variável de controle de tempo quando o usuário será redirecionado para o login

var gGradIntra_MensagemRedirecionamentoLogin = "Sua sessão expirou. você será <a href='[URL]'>redirecionado para o login</a> em [SEG] segundos";

var gGradIntra_AplicacaoEmModoDeCompatibilidade = false;

/*

    Mensagens para o usuário

*/

function GradIntra_ExibirMensagem(pTipo_IAE, pMensagem, pRetornarAoEstadoNormalAposSegundos, pMensagemAdicional)
{
///<summary>Exibe uma mensagem no painel de alerta.</summary>
///<param name="pTipo_IAE"  type="String">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
///<param name="pRetornarAoEstadoNormalAposSegundos" type="Boolean">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
///<param name="pMensagemAdicional" type="String">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
///<returns>void</returns>

    // Guarda a mensagem "original" que estava no div se ela ainda não estiver na memória:

    if (gGradIntra_MensagemOriginal == null)
        gGradIntra_MensagemOriginal = $("#pnlMensagem span").html();

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

    var lAmarelo  = "#e9f68b";
    var lAzul     = "#78ccd2";
    var lVemelho  = "#d47d7d";
    var lOriginal = "#9BA2AC";

    var lClasse = "";

    var lCor = (pTipo_IAE == "A") ? lAmarelo : ((pTipo_IAE == "I") ? lAzul : ((pTipo_IAE == "E") ? lVemelho : lOriginal));

    var lLargura = Math.ceil( (pMensagem.length * lMultiplicador) );

    // Verifica valores mínimos do comprimento do painel, para que mensagens curtas não fiquem curtas demais
    // (especialmente quando tem erro adicional, que expande o painel maior; se a mensagem de cima for curta, o painel fica curto demais

    if (lCor == lOriginal)
    {
        if (lLargura < 142) lLargura = 142;  // mínimo 142 pra "original"
    }
    else
    {
        if (pTipo_IAE == "E")
        {
            if (lLargura < 400) lLargura = 400;  //minimo 400 pra erro
        }
        else
        {
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

    $("button.btnMenuScroller_Direito").animate(  { right: (lLargura + 1) }, gGradIntra_Config_DuracaoDaAnimacaoDeMensagem);

    $("#pnlMensagem")
        .addClass("BotaoEscondido")
        .animate(  { 
                       backgroundColor: lCor
                     , width:           lLargura 
                   }
                 , gGradIntra_Config_DuracaoDaAnimacaoDeMensagem
                 , function() 
                   { 
                        if (pMensagemAdicional && pMensagemAdicional != null && pMensagemAdicional != "")
                            GradIntra_ExibirMensagemAdicional(pMensagemAdicional); 

                        //$("button.btnMenuScroller_Direito").css( { right: $("#pnlMensagem").width() - 28 } )
                   }
                )
        .children("span")
            .html(pMensagem)
            .attr("class", lClasse);

    if (pRetornarAoEstadoNormalAposSegundos == true)
    {
       window.setTimeout(GradIntra_RetornarMensagemAoEstadoNormal, 4000);
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

function GradIntra_ExibirMensagemAdicional(pMensagem)
{ 
///<summary>Exibe uma mensagem na caixa de texto no painel de erro.</summary>
///<param name="pMensagem"  type="String">Mensagem para exibir.</param>
///<returns>void</returns>

    if (pMensagem && pMensagem != "")
    {
        var lPainel = $("#pnlMensagemAdicional");
        
        if (lPainel.is(":visible"))                                               // Se já estiver visível, só atualiza o conteúdo da textarea 
        {
            lPainel.children("textarea").html(pMensagem);
        }
        else
        {
            var lLargura = $("#pnlMensagem").width();                            // Se não estiver visível, pega a largura do painel de mensagem pra igualar no painel da mensagem adicional

            lPainel
                .css({
                          width: lLargura
                        //, heigth: 60
                     })
                .effect(  "blind"                                                // Esse é o efeito de animação pra "abrir" o painel
                        , { 
                               mode:   "show"                                    // Flag pra abrir
                             , easing: "easeOutBounce"                           // Easign que faz o "boing boing" em baixo
                          }
                        , (gGradIntra_Config_DuracaoDaAnimacaoDeMensagem * 2))   // Duração da animação
                .children("textarea").html(pMensagem)
        }
    }
}

function GradIntra_OcultarMensagemAdicional()
{
///<summary>Oculta o painel de mensagem adicional.</summary>
///<returns>void</returns>

    $("#pnlMensagemAdicional")
        .effect(  "blind"
                  , { mode: "hide", easing: "easeInOutExpo" }
                  , (gGradIntra_Config_DuracaoDaAnimacaoDeMensagem)
               );
    
    return false;
}

function GradIntra_RetornarMensagemAoEstadoNormal()
{
///<summary>Retorna o painel de alerta ao estado original (quando a página foi carregada).</summary>
///<returns>void</returns>

    if ($("#pnlMensagem span").html() == gGradIntra_MensagemOriginal)
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
    else
    {
        GradIntra_ExibirMensagem("O", gGradIntra_MensagemOriginal, true);
    }

    return false;
}

function GradIntra_InstanciarObjetoDosControles(pObjetoSeletor)
{
///<summary>Instancia um objeto a partir de um formulario (pObjetoSeletor) e seus inputs com o atributo 'Propriedade'.</summary>
///<param name="pObjetoSeletor"  type="Objeto_jQuery">Objeto jQuery cujos inputs serão analizados para instanciamento do objeto de retorno.</param>
///<returns>Objeto JSON instanciado com todas as propriedades relativas aos inputs de pOjetoSeletor</returns>

    var lRetorno = new Object();                                    // Instancia o objeto que irá retornar da função

    var lCampo, lPropriedade, lValor;                               // Variáveis para guardar os valores do elemento HTML
    
    var lTREditando = pObjetoSeletor                                // Acha a TR do item filho que está sendo editado, se houver
                        .parent()
                            .find("table.Lista tr.Editando");

    var lAspas = true;                                              // Flag que indica se o "eval" vai precisar de aspas pra setar o valor da variável

    pObjetoSeletor.find("[Propriedade]").each(function ()           // Roda todos os elementos dentro de <pObjetoSeletor> que tenham o atributo "Propriedade" com qualquer valor
    {
        lCampo = $(this);                                           // Insancia o elemento dentro de <lCampo> como objeto jQuery
        lAspas = true;

        if (lCampo.is(":visible") || gGradIntra_Navegacao_SistemaAtual != CONST_SISTEMA_RELATORIOS) {
            lPropriedade = lCampo.attr("Propriedade");              // Quarda a QUAL propriedade o elemento se refere (ex. "NomeDoCliente")
            lValor = lCampo.val() == "null" ? "" : lCampo.val();    // Pega o valor atual do campo >> Atenção!! Aqui estamos usando val(), então só vai funcionar pra elementos
            //  que suportam essa função: input, select, textarea. Se quisermos pegar algum valor de elementos tipo span,
            //  td ou outros, precisamos alterar a lógica aqui pra que ele verifique o campo e use html() ao invés de val()

            if (lCampo.is("span"))                                  // Trata spans
            {
                lValor = lCampo.html();
            }

            if (lCampo.is("[type='checkbox']"))                     // Trata checkboxes e radio buttons
            {
                //lCampo.attr("id") => "rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"
                if (lCampo.attr("name") == "rdoCadastro_DadosCompletos_DesejaAplicar") {
                    if (pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && !pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && !pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked"))
                    { lValor = "BOVESPA" }
                    if (!pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && !pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked"))
                    { lValor = "FUNDOS" }
                    if (!pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && !pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked"))
                    { lValor = "CAMBIO" }
                    if (pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && !pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked"))
                    { lValor = "BOV/FUN" }
                    if (pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && !pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked"))
                    { lValor = "BOV/CAM" }
                    if (!pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').is(":checked") && pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').is(":checked") && pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').is(":checked"))
                    { lValor = "FUN/CAM" }
                    if (pObjetoSeletor.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').is(":checked"))
                    { lValor = "TODOS" }

                    lAspas = true;

                }
                else 
                {
                    lValor = lCampo.is(":checked");
                    lAspas = false;
                }
            }

            if (lCampo.is("[type='radio']")) {
                if (lCampo.attr("RadioMultiplo") == "true") {
                    // patch para lidar com um campo radio que tem mais de duas opções; precisa indicar RadioMultiplo="true"

                    lValor = $("input[name='" + lCampo.attr("name") + "']:checked").val();
                    lAspas = true;
                }
                else {
                    // campos de duas opções que já existiam
                    lValor = lCampo.is(":checked");
                    //lValor = lCampo.next("label").hasClass("checked"); //trocando pra verificar ao invés do checked do controle, se o label tem a classe...
                    //lValor = (lCampo.prop("checked") == true);

                    //TODO: continuar daqui, ver o que a lib da radio bonitinha está fazendo

                    lAspas = false;
                }
            }

            if (lCampo.is("textarea")) {
                if (lValor == "")
                    lValor = lCampo.html();

                while (lValor.indexOf("\n") != -1) //--> Corrigindo um bug de 'unterminated string literal'
                    lValor = lValor.replace('\n', ' '); // por causa da string quebrada no bind().

                lAspas = true;
            }

            if (lCampo.attr("TipoPropriedade") == "Lista") {
                var lListaIniciada = eval(" (lRetorno." + lPropriedade + " != null) ");

                if (!lListaIniciada)
                    eval("lRetorno." + lPropriedade + " = new Array()");

                if (lCampo.is("[type='checkbox']")) {
                    if (lValor)
                        eval("lRetorno." + lPropriedade + ".push('" + lCampo.attr("ValorQuandoSelecionado") + "')");
                }
                else {
                    eval("lRetorno." + lPropriedade + ".push(" + lValor + ")");
                }
            }
            else {     // Gera o eval necessário pra guardar dentro de <lRetorno> a propriedade identificada e seu valor,
                //  passando quando necessário (somente boolean está sem aspas; números estão indo como strings mesmo,
                //   o JsonConvert deserializa eles corretamente no code-behind

                var lEval = "lRetorno." + lPropriedade + " = " + (lAspas ? "'" : "") + lValor + (lAspas ? "'" : "") + ";";

                try {
                    eval(lEval);
                }
                catch (Erro) {
                    //alert("Visualizar erro na barra de status");

                    window.status = lEval;
                }
            }
            if (lCampo.is("select"))
                eval("lRetorno." + lPropriedade + "Desc = '" + lCampo.children("option:selected").html().replace('\'', '') + "';");
        }
    });

    //TODO: Acho que isso está depreciado; o "estado" do objeto é indicado só pelo ID mesmo; verificar nos Cod-Behind se 
    //        tem algum elemento esperando essa propriedade, e remover daqui.

    if ((!lRetorno.Id || lRetorno.Id == "") && lTREditando.length == 0)
        lRetorno.Estado = "Novo"
    else
        lRetorno.Estado = "Modificado"
        
    return lRetorno;
}


function GradIntra_ExibirObjetoNaLista(pObjeto, pDivFormulario)
{
///<summary>Exibe um objeto JSON dentro de uma lista de objetos que seja uma tabela com a classe 'Lista'.</summary>
///<param name="pObjeto"  type="Objeto_JSON">Objeto que irá para a lista.</param>
///<param name="pDivFormulario" type="Objeto_jQuery">Formulário de onde pObjeto foi instanciado (cujo parent() contém a table.Lista).</param>
///<returns>void</returns>

    var lTBody = pDivFormulario.parent().find("table.Lista tbody");         // Acha o <TBody> da tabela de listagem

    // Verfica se o objeto tem a propriedade "TipoDeItem", e se tiver, configura
    //  a função Resumo() pra ele porque em muitos casos a lista mostra um resumo do objeto
    // em cada linha, e essa função é responsável por "condensar" as propriedades dele para exibição

    // Essa função <GradIntra_ExibirObjetoNaLista> é chamada tanto após o carregamento do formulário (a lista de objetos pra exibir na lista 
    //  está num campo hidden, e roda um loop desse array chamando essa função) quando na confirmação da edição de um item filho. 
    //  Por isso que tem esse "if" abaixo; no primeiro caso, o Objeto tendo a propriedade TipoDeItem já passa pra configuração.
    //  Se não, ele verifica se o item sendo editado tem a propriedade e se tiver, configura.

    if (pObjeto.TipoDeItem)
    {
        GradIntra_ConfigurarResumo(pObjeto, pObjeto.TipoDeItem);
    }
    else
    {
        if (gGradIntra_Cadastro_ItemFilhoSendoEditado != null)
            if (gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem && gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem != "")
                GradIntra_ConfigurarResumo(pObjeto, gGradIntra_Cadastro_ItemFilhoSendoEditado.TipoDeItem);
    }

    var lTR;                                                    // Objeto de <tr> que pode ser uma nova ou a que está sendo editada
    var lTD;                                                    // Objeto <td> dentro do loop
    
    var lItemNovo = false;                                      // Flag se for um item novo ou não
    
    lTR = lTBody.find("tr[rel='" + pObjeto.Id + "']");          // Acha a <tr> que está sendo editada, porque marcamos no atributo "rel" o ID do objeto filho
    
    if (lTR.length == 0)                                         // Se não encontrou a tr com <rel> = ao ID do <pObjeto>, então é um item novo
    {
        lTR = lTBody.find("tr.Template").clone();               // Clona a tr de template

        lTR.addClass("ItemDinamico");                           // Adiciona o marcador que é um item dinâmico

        lItemNovo = true;                                       // Marca a flag que é item novo
    }
    
    if (lTR.length > 0)
    {
        var lPropriedade, lValor; 

        lTR
         .attr("rel", pObjeto.Id)                                   // Marca no atributo <rel> da tr o id do objeto que ela representa (para edição)
         .removeClass(CONST_CLASS_TEMPLATE)                         // Tira a classe de template, porque é um item dinâmico
         .removeClass("Editando")                                   // Tira a classe "editando", caso ela esteja sendo editada (em item novo não faz diferença)
         .find("td input[Propriedade='json']")                      // Acha o input hidden que vai guardar o json completo do objeto
             .val($.toJSON(pObjeto));
        
        lTR.children("TD").each(function()
        {
            lTD = $(this);

            lPropriedade = lTD.attr("Propriedade"); 
        
            if (lPropriedade && lPropriedade != "")
            {
                lValor = eval("pObjeto." + lPropriedade);

                if (lTD.children("input").length > 0)
                {
                    lTD.children("input").val(lValor);
                }
                else if (lTD.children("span").length > 0)
                {
                    lTD.children("span").html(lValor);
                }
                else
                {
                    lTD.html(lValor);
                }
            }

            if (pObjeto.Exclusao != undefined)
            {
                if (!pObjeto.Exclusao)
                {
                    $("button.Excluir").remove();
                }
            }
        });
    
        if (lItemNovo)
        {
            if (pObjeto.TipoDeItem != "Associacao" || pObjeto.EhPermissao == false)
            {
                lTBody.children("tr:eq(0)").after(lTR);

                GradIntra_HabilitarInputsComMascaras(lTR);

                lTR.show();

                lTBody.find("tr.Nenhuma").hide();

                var lThead = lTBody.parent().find("thead");

                if (lThead.hasClass("ExibirSeTiverItem"))
                    lThead.show();

                if (pObjeto.Exclusao != undefined)
                {
                    if (!pObjeto.Exclusao)
                    {
                        $("button.Excluir").remove();
                    }
                }
            }
        }
        else
        {
            //pisca depois que atualizou?
        }
    }
}

function GradIntra_ExibirObjetoNoFormulario(pObjeto, pDivFormulario)
{
///<summary>Preenche um formulário com as propriedades de um objeto JSON.</summary>
///<param name="pObjeto"  type="Objeto_JSON">Objeto que irá para o formulário.</param>
///<param name="pDivFormulario" type="Objeto_jQuery">Formulário cujos inputs receberão as propriedades de pObjeto.</param>
///<returns>void</returns>

    var lCampo, lPropriedade, lValor;

    pDivFormulario.find("[Propriedade]").each(function () {
        lCampo = $(this);

        lPropriedade = lCampo.attr("Propriedade");
        lValor = eval("pObjeto." + lPropriedade + ";");

        if (lCampo.is("[type='checkbox']")) {
            if (pObjeto.TipoDeItem != "Associacao") {
                var lCheckId = lCampo.attr("id");

                if (lValor == true) {
                    lCampo.prop("checked", true);
                    lCampo.parent().find("label[for='" + lCheckId + "']").addClass("checked");
                }
                else 
                {
                    if (lValor == "BOVESPA") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').parent().find("label").removeClass("checked");
                    }
                    else if (lValor == "FUNDOS") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').parent().find("label").removeClass("checked");
                    }
                    else if (lValor == "CAMBIO") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').parent().find("label").removeClass("checked");
                    }
                    else if (lValor == "BOV/FUN") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').prop("checked", true).parent().find("label").addClass("checked");
                    }
                    else if (lValor == "BOV/CAM") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').prop("checked", true).parent().find("label").addClass("checked");
                    }
                    else if (lValor == "FUN/CAM") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').parent().find("label").removeClass("checked");
                    }
                    else if (lValor == "TODOS") {
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos"]').prop("checked", true).parent().find("label").addClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio"]').parent().find("label").removeClass("checked");
                        pDivFormulario.find('[id*="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa"]').parent().find("label").removeClass("checked");
                    } else {
                        lCampo.prop("checked", false);
                        lCampo.parent().find("label[for='" + lCheckId + "']").removeClass("checked");
                    }
                }



            }
        }
        else if (lCampo.is("input, select")) {
            if (!lCampo.is("[type='radio']")) {
                lCampo.val(lValor);
            }
            else {
                var lRadioName = lCampo.attr("name");
                var lRadioSim = lRadioName + "_Sim";
                var lRadioNao = lRadioName + "_Nao";

                var lPainelDoSim = $("#" + lRadioSim).attr("data-PainelDoSim");

                console.log("Name:[" + lRadioName + "] Sim:[" + lRadioSim + "] Nao:[" + lRadioNao + "] Valor:[" + lValor + "]");

                if (lValor == true) {
                    $("#" + lRadioSim).prop("checked", true)
                        .parent()
                        .find("label[for='" + lRadioSim + "']")
                                .addClass("checked");

                    $("#" + lRadioNao)
                        .prop("checked", null)
                        .parent()
                        .find("label[for='" + lRadioNao + "']")
                                .removeClass("checked");

                    if (lPainelDoSim !== undefined && lPainelDoSim != "")
                        $("#" + lPainelDoSim).show();
                }
                else {

                    if (lValor == 2) {
                        //exceção do campo "sim, à gradual" em pessoa vinculada

                        $("#" + lRadioSim).prop("checked", null)
                            .parent()
                            .find("label[for='" + lRadioSim + "']")
                                    .removeClass("checked");

                        $("#" + lRadioNao).prop("checked", null)
                            .parent()
                            .find("label[for='" + lRadioNao + "']")
                                    .removeClass("checked");

                        $("#rdoCadastro_DadosCompletos_PessoaVinculada_SimG")
                            .prop("checked", true)
                            .parent()
                            .find("label[for='rdoCadastro_DadosCompletos_PessoaVinculada_SimG']")
                                    .addClass("checked");
                    }
                    else {
                        $("#" + lRadioSim).prop("checked", null)
                            .parent()
                            .find("label[for='" + lRadioSim + "']")
                                    .removeClass("checked");

                        $("#" + lRadioNao)
                            .prop("checked", true)
                            .parent()
                            .find("label[for='" + lRadioNao + "']")
                                    .addClass("checked");
                    }

                    if (lPainelDoSim !== undefined && lPainelDoSim != "")
                        $("#" + lPainelDoSim).hide();
                }
            }
        }
        else if (lCampo.is("textarea")) {
            if (lValor != null && lValor != "")
                lValor = lValor.replace(/<br \/>/gi, "\r\n")

            //          lCampo.html(lValor); //--> Trecho comentado por causar inconsistencia na tela de 'Pendências Cadastrais'
            lCampo.val(lValor);
        }
        else {
            //          lCampo.html(lValor); //--> Trecho comentado por causar inconsistencia na tela de 'Pendências Cadastrais'
            lCampo.val(lValor);
        }
    });
    
    pDivFormulario.show();
}

function GradIntra_CarregarHtmlVerificandoErro(pUrl, pDadosDoRequest, pPainelParaAtualizar, pCallBack, pOpcoesPosCarregamento)
{
///<summary>Carrega o HTML de uma página em um elemento 'pPainelParaAtualizar'. Utilizar sempre essa função pois já lida com casos de erro.</summary>
///<param name="pUrl"  type="String">URL que será chamada.</param>
///<param name="pDadosDoRequest"  type="Objeto_JSON">Dados para a chamada ajax.</param>
///<param name="pPainelParaAtualizar"  type="String_ou_Objeto_jQuery">Seletor ou objeto jQuery que irá receber o HTML da resposta ajax.</param>
///<param name="pCallBack"  type="Função_Javascript">(opcional) Função para chamar após o carregamento de pPainelParaAtualizar.</param>
///<param name="pOpcoesPosCarregamento"  type="Objeto_JSON">(opcional) Opções de execução pós-carregamento. Propriedades suportadas (case-sensitive): {CustomInputs: string[], HabilitarMascaras: bool, HabilitarValidacoes: bool, AtivarToolTips: bool} .</param>
///<returns>void</returns>

    $.ajax({
            url:        pUrl
            , type:     "post"
            , cache:    false
            , data:     pDadosDoRequest
            , success:  function(pResposta) { GradIntra_CarregarVerificandoErro_CallBack(pResposta, pPainelParaAtualizar, pCallBack, pOpcoesPosCarregamento); }
            , error:    GradIntra_TratarRespostaComErro
           });
}

function GradIntra_CarregarJsonVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso, pOpcoesPosCarregamento, pDivDeFormulario)
{
///<summary>Carrega o JSON de uma chamada ajax.</summary>
///<param name="pUrl"  type="String">URL que será chamada.</param>
///<param name="pDadosDoRequest"  type="Objeto_JSON">Dados para a chamada ajax.</param>
///<param name="pCallBackDeSucesso"  type="Função_Javascript">(opcional) Função para chamar em caso de sucesso.</param>
///<returns>void</returns>

    $.ajax({
              url:      pUrl
            , type:     "post"
            , cache:    false
            , dataType: "json"
            , data:     pDadosDoRequest
            , success:  function(pResposta) { GradIntra_CarregarVerificandoErro_CallBack(pResposta, pDivDeFormulario, pCallBackDeSucesso, pOpcoesPosCarregamento); }
            , error:    GradIntra_TratarRespostaComErro
           });
}

var gTextBoxes;

function GradIntra_CarregarVerificandoErro_CallBack(pResposta, pPainelParaAtualizar, pCallBack, pOpcoesPosCarregamento)
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
                GradIntra_TratarRespostaComErro(pResposta, pPainelParaAtualizar);
            }
            else
            {
                //sucesso em chamada JSON
                if (pCallBack && pCallBack != null)
                    pCallBack(pResposta, pPainelParaAtualizar, pOpcoesPosCarregamento);  
            }   
        }
        else
        {   // resposta html
            if (pResposta.indexOf('"TemErro":true,') != -1)
            {   //erro, porém retorno json em chamada html (ocorre com timeout por exemplo)
            
                GradIntra_TratarRespostaComErro($.evalJSON(pResposta));
            }
            else
            {   //sucesso em resposta HTML
            
                if (pResposta.indexOf('<fieldset id="pnlLogin">') != -1)
                {
                    GradIntra_TratarRespostaComErro({ TemErro: true, Mensagem: "RESPOSTA_SESSAO_EXPIRADA" });
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
                                $(pOpcoesPosCarregamento.CustomInputs).each(function()
                                {
                                    $(this + "").customInput();
                                });
                            }

                            if (pOpcoesPosCarregamento.HabilitarMascaras)
                                GradIntra_HabilitarInputsComMascaras(pPainelParaAtualizar);

                            if (pOpcoesPosCarregamento.HabilitarValidacoes)
                                pPainelParaAtualizar.validationEngine({showTriangle: false});

                            if (pOpcoesPosCarregamento.AtivarToolTips)
                                GradIntra_AtivarTooltips(pPainelParaAtualizar);

                            if (pOpcoesPosCarregamento.AtivarAutoComplete)
                                pPainelParaAtualizar.find(".AtivarAutoComplete").ComboBoxAutocomplete();
                        }

                        pPainelParaAtualizar.find("." + CONST_CLASS_PICKERDEDATA).datepicker( { showOn: "button" } );

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

function GradIntra_FocoProximoInput(pEvento)
{
    if (pEvento.keyCode == 13)
    {
        var lIndiceTextbox = gTextBoxes.index(this);

        if (gTextBoxes[lIndiceTextbox + 1] != null)
        {
            var lTextbox = gTextBoxes[lIndiceTextbox + 1];
                lTextbox.focus();
                pEvento.preventDefault();

            return false;
        }
    }
}

function GradIntra_HabilitarInputsComMascaras(pContainerDosInputs)
{
    Validacao_HabilitarDataComFormatacao(pContainerDosInputs.find("input.Mascara_Data"));

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CEP"), "99999-999");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_TEL"), "9999-9999");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CPF"), "999.999.999-99");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_Hora"), "99:99");

    Validacao_HabilitarMascaraNumerica(pContainerDosInputs.find("input.Mascara_CNPJ"), "99.999.999/9999-99");

    Validacao_HabilitarSomenteNumeros(pContainerDosInputs.find("input.ProibirLetras"));

    Validacao_HabilitarSomenteNumerosComFormatacao(pContainerDosInputs.find("input.ValorMonetario"));
}

function GradIntra_ConfigurarResumo(pObjeto, pTipo)
{
///<summary>Configura a função Resumo() para um objeto JSON pObjeto, dependendo do seu tipo.</summary>
///<param name="pObjeto" type="Objeto_JSON">Objeto que irá receber a função.</param>
///<param name="pTipo"  type="String">Tipo do objeto.</param>
///<returns>void</returns>

    pObjeto.Resumo = function() { return "Resumo não configurado para tipo '" + pTipo + "'"; }

    if (pTipo == "Associacao")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";
        
//            if (this.TipoAssociacao == 1)
//            {   //  Prametro=1 
                lRetorno +=  this.CodigoParametroDesc + " - Valor: " + this.ValorParametro + " - Validade: " + this.DataValidadeParametro;
//            }
//            else
//            {   // Permissao=2
//                lRetorno +=  this.CodigoPermissaoDesc;
//            }
        
            return lRetorno;    
        }
    }

    if (pTipo == "Telefone")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            lRetorno += "(" + this.DDD + ") ";

            lRetorno += this.Numero;

            if (this.Ramal && this.Ramal != 0 && this.Ramal != null && this.Ramal != "")
                lRetorno += " r." + this.Ramal;

            if (this.Tipo == "R")
                lRetorno += " (res)";

            if (this.Tipo == "M")
                lRetorno += " (com)";

            if (this.Tipo == "C")
                lRetorno += " (cel)";

            return lRetorno;
        };
    }

    if (pTipo == "Procurador" || pTipo == "Diretor" || pTipo == "Representante")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            lRetorno += this.Nome + " - ";

            lRetorno += this.CPF ;

            return lRetorno;
        };
    }

    if (pTipo == "Emitente") 
    {
        pObjeto.Resumo = function () 
        {
            var lRetorno = "";

            lRetorno += this.Nome + ", ";

            lRetorno += this.CodigoSistema + " - ";

            lRetorno += this.CPFCNPJ ;

            return lRetorno;
        };
    }

    if (pTipo == "Endereco")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            lRetorno += this.Logradouro + ", ";

            lRetorno += this.Numero + " - ";

            lRetorno += this.Bairro + " - ";

            lRetorno += "CEP " + this.CEP;

            return lRetorno;
        };
    }
    
    if (pTipo == "PendenciaCadastral")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            if (this.DataPendencia == "" || this.DataPendencia == "null" || this.DataPendencia == "undefined" )
            {
                var today = new Date() ;
                var todayd = today.getDate(); 
                var todaym = today.getMonth()+1; 
                var todayy = today.getFullYear(); 
                var hrs = today.getHours();
                var min = today.getMinutes();
                var sec = today.getSeconds();

                if (min    < 10) min = "0"+min;
                if (hrs    < 10) hrs = "0"+hrs;
                if (sec    < 10) sec = "0"+sec;
                if (todayd < 10) todayd = "0"+todayd;
                if (todaym < 10) todaym = "0"+todaym;

                var lHoje = todayd + "/" + todaym + "/" + todayy + " " + hrs + ":" + min + ":" + sec  ; 

                lRetorno += lHoje + " - ";
            }
            else
            {
                lRetorno += this.DataPendencia + " - ";
            }
            
            lRetorno += this.TipoDesc + " - ";

            lRetorno += (this.FlagResolvido)? "Resolvido": "Pendente";

            return lRetorno;
        };
    }
    
    if (pTipo == "Conta")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            lRetorno += this.Banco + " - ";

            lRetorno += this.BancoNome + " - ";
                        
            lRetorno += "Ag. " + this.Agencia + "-" + this.AgenciaDigito + "  ";

            lRetorno += " / ";

            lRetorno += this.TipoConta + " ";

            lRetorno += this.ContaCorrente + "-" + this.ContaDigito;

            return lRetorno;
        }
    }
    
    if (pTipo == "PessoaFisica")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            lRetorno += this.Nome + ", ";
            
            lRetorno += "CPF " + this.CPF;

            return lRetorno;
        }
    }

    if (pTipo == "EmpresaColigada")
    {
        pObjeto.Resumo = function ()
        {
            var lRetorno = "";

            lRetorno += this.RazaoSocial + ", ";

            lRetorno += "CNPJ " + this.CNPJ;

            return lRetorno;
        }
    }

    if (pTipo == "RepresentanteParaNaoResidente")
    {
        pObjeto.Resumo = function()
        {
            var lRetorno = "";

            lRetorno += this.Nome + " - ";
            
            lRetorno += "[" + this.CodigoCVM + "]";

            return lRetorno;
        }
    }

    if (pTipo == "Grupos" || pTipo == "Usuarios" || pTipo == "Perfis")
    {
        pObjeto.Resumo = function ()
        {
            var lRetorno = "";

            lRetorno += this.ItemDesc + " - ";

            lRetorno += "[" + this.Item + "]";

            return lRetorno;
        }
    }

    if (pTipo == "Permissoes")
    {
        pObjeto.Resumo = function ()
        {
            var lRetorno = "";

            lRetorno += this.PermissaoDesc + " - ";

            lRetorno += "[" + this.RestricaoDesc + "]";

            return lRetorno;
        }
    }

    if (pTipo == "Regra")
    {
        pObjeto.Resumo = function ()
        {
            var lRetorno = "";
            
            lRetorno += this.Ativo + " - ";
            lRetorno += this.IdBolsaDesc + " - ";
            lRetorno += this.IdRegraDesc;
            
            return lRetorno;
        }
    }

    if (pTipo == "SolicitacaoAlteracaoCadastral")
    {
        pObjeto.Resumo = function ()
        {
            var lRetorno = "";

            lRetorno += this.DsInformacao + " - ";
            lRetorno += this.DtSolicitacao + " - ";
            lRetorno += this.StResolvido ? "Resolvido" : "Pendente";

            return lRetorno;
        }
    }

    if (pTipo == "DocumentacaoEntregue")
    {
        pObjeto.Resumo = function ()
        {
            var lRetorno = "";

            lRetorno += " Adicionado em - " + this.DtAdesaoDocumento;

            return lRetorno;
        }
    }
}

function GradIntra_TratarRespostaComErro(pResposta, pPainelParaAtualizar)
{
///<summary>Função que trata uma resposta que seja um objeto JSON com (.TemErro == true) ou um XmlHttpResponse com (.status != 200).</summary>
///<param name="pResposta" type="Objeto_XmlHttpResponse ou Objeto JSON">Objeto de resposta que retornou do code-behind. Pode ser Json ou HTML.</param>
///<returns>void</returns>

    //desmarca todos os divs que estiverem "carregando conteúdo"
    $("." + CONST_CLASS_CARREGANDOCONTEUDO).removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

    if (pPainelParaAtualizar != null)
    {
        $(pPainelParaAtualizar).find("[disabled]").prop("disabled", false);
    }

    if (pResposta.status && pResposta.status != 200)
    {   //resposta veio como HTML, provavelmente erro do servidor 
        
        var lTexto = pResposta.responseText;
        
        lTexto = lTexto.replace(/</gi, "&lt;");
        lTexto = lTexto.replace(/>/gi, "&gt;");

        GradIntra_ExibirMensagem("E", "Erro do servidor [" + pResposta.status + " - " + pResposta.statusText + "]                                       ", false, lTexto);
    }
    else
    {
        if (pResposta.TemErro)
        {
            if (pResposta.Mensagem == "RESPOSTA_SESSAO_EXPIRADA")
            {
                GradIntra_ExibirMensagem("E", GradIntra_MensagemDeRedirecionamento(5));

                gGradIntra_RedirecionarParaLoginEm = 4;

                window.setTimeout(GradIntra_TimeoutParaRedirecionarLogin, 1000);
            }
            else
            {
                GradIntra_ExibirMensagem("E", pResposta.Mensagem, false, pResposta.MensagemExtendida);
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

            GradIntra_ExibirMensagem("E", "Erro desconhecido                                       ", false, lMensagemExtendida);
        }
    }
}

function GradIntra_TimeoutParaRedirecionarLogin()
{
///<summary>Reduz em 1 segundo o timeout para redirecionar e se chama novamente caso ainda seja > 0.</summary>
///<returns>void</returns>

    gConfirmacaoDeSaidaDesnecessaria = true;

    window.onbeforeunload = null;

    if (gGradIntra_RedirecionarParaLoginEm >= 0)
    {
        $("#pnlMensagem span").html(GradIntra_MensagemDeRedirecionamento(gGradIntra_RedirecionarParaLoginEm));

        gGradIntra_RedirecionarParaLoginEm --;

        window.setTimeout(GradIntra_TimeoutParaRedirecionarLogin, 1000);
    }
    else
    {
        document.location = GradIntra_RaizDoSite() + "/Login.aspx";
    }
}

function GradIntra_RaizDoSite()
{
    var lRetorno = document.location.protocol + "//" + document.location.host;

    if (document.location.host.indexOf("localhost") != -1) 
        lRetorno += "/Gradual.Intranet";
    else
        lRetorno += "";

    return lRetorno;
}

function GradIntra_MensagemDeRedirecionamento(pSegundos)
{
///<summary>Retorna a string da mensagem de redirecionamento.</summary>
///<returns>void</returns>

    return gGradIntra_MensagemRedirecionamentoLogin.replace("[SEG]", pSegundos).replace("[URL]", GradIntra_RaizDoSite() + "/Login.aspx");
}

function GradIntra_AtivarTooltips(pSeletor)
{
///<summary>Ativa os Custom ToolTips nos objetos selecionados por pSeletor.</summary>
///<param name="pSeletor" type="String_ou_Objeto_jQuery">String ou objeto para selecionar os elementos que tiverem atributo 'title' para os tooltips.</param>
///<returns>void</returns>

    var lObjetosParaAtivar;

    if (!pSeletor || pSeletor == null || pSeletor == "")
    {
        pSeletor = "";
    }
    
    if (pSeletor.find)
    {
        //já veio como objeto json
        lObjetosParaAtivar = pSeletor.find("[title]");
    }
    else
    {
        if (pSeletor != "") pSeletor += " ";

        lObjetosParaAtivar = $(pSeletor + "[title]");
    }
    
    lObjetosParaAtivar.bt({ shrinkToFit:  true
                          , padding:      '4px'
                          , spikeGirth:   8
                          , spikeLength:  8
                          , positions:    ["right", "left", "most"]
                          , fill:         "rgb(242, 247, 204)"
                          , strokeWidth:  2
                          , strokeStyle:  "rgb(152, 156, 162)"
                          , centerPointX: 0.5
                          , centerPointY: 0.5
                          , cssClass:     "bt-tooltip-arrow"
                          , cornerRadius: 3
                          , cssStyles:    {color:"#506377", fontSize:"0.8em", padding: "2px 6px 2px 6px" }
                          });
}

function AbrirLinkPDF(sCaminhoVirtualPDF)
{
    window.open(sCaminhoVirtualPDF);

    return false;
}

function btnMensagemFechar_Click(pSender)
{
    GradIntra_RetornarMensagemAoEstadoNormal();

    return false;
}

function ConvertToDate(pParametro)
{
    var lArray = pParametro.split("/");

    return new Date(lArray[1] + "/" + lArray[0] + "/" + lArray[2]);
}

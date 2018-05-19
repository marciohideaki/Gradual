/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />

var gIntervaloDeAtualizacao = null;

var gCountUltimaAtualizacao = null;

function GradIntra_Monitoramento_AoSelecionarSistema()
{
    if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_ORDENS
    || (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_ORDENS_STOP)
    || (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_ORDENS_RELATORIOS)
    || (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_ORDENS_NOVO_OMS))
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca    = true;

        if (gGradIntra_Navegacao_PainelDeConteudoAtual.hasClass(CONST_CLASS_CONTEUDOCARREGADO))
            gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;
        else 
            gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = false;
            
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_MONITORAMENTO_TERMOS)
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;
        gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

        if (!$("#pnlMonitoramento_Termos_ListaDeResultados").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            $("#pnlConteudo_Monitoramento_Termos")
                .show()
                .find(".pnlFormularioExtendido")
                    .show()
                    .addClass(CONST_CLASS_CARREGANDOCONTEUDO);
                
            GradIntra_Navegacao_CarregarFormulario("Monitoramento/ResultadoTermos.aspx", "pnlMonitoramento_Termos_ListaDeResultados", null, null);
        }
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_USUARIOSLOGADOS)
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
        gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;
        
        if (!$("#pnlConteudo_Monitoramento_UsuariosLogados_ListaDeResultados").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            $("#pnlConteudo_Monitoramento_UsuariosLogados")
                .show()
                .find(".pnlFormularioExtendido")
                    .show()
                    .addClass(CONST_CLASS_CARREGANDOCONTEUDO);
                
            GradIntra_Navegacao_CarregarFormulario("Monitoramento/ResultadoUsuariosLogados.aspx", "pnlConteudo_Monitoramento_UsuariosLogados_ListaDeResultados", null, null);
        }
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_DESBLOQUEIOCUSTODIA)
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca    = false;
        gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

        $("#pnlConteudo_Monitoramento_DesbloqueioCustodia")
                .show()
                .find("#pnlFormularioDesbloqueio")
                    .show()
                    .addClass(CONST_CLASS_CARREGANDOCONTEUDO);

        GradIntra_Navegacao_CarregarFormulario("Monitoramento/DesbloqueioCustodia.aspx", "pnlConteudo_Monitoramento_DesbloqueioCustodia_ListaDeResultados", null, null);
    }

    if(gIntervaloDeAtualizacao == null)
    {
        gIntervaloDeAtualizacao = window.setInterval(GradIntra_Monitoramento_AtualizarListagem, 5000);
    }
}

function GradIntra_Monitoramento_ItemIncluidoComSucesso(pItemIncluido)
{
    
}

function GradIntra_Monitoramento_ExcluirOrdensStartStopSelecionadas(pListaDeOrdens, pListaPapeis)
{
    var lDados = {Acao:"ExcluirOrdens", Ids:pListaDeOrdens, Papeis:pListaPapeis};

    GradIntra_CarregarJsonVerificandoErro(  "Monitoramento/ResultadoOrdensStop.aspx"
                                          , lDados
                                          ,  function (pResposta) { GradIntra_Monitoramento_ExcluirOrdensStartStopSelecionadas_Callback(pResposta); }
                                         );

    return false;
}

function GradIntra_Monitoramento_ExcluirOrdensStartStopSelecionadas_Callback(pResposta)
{
    //var linhasSelecionadas = $("#chkMonitoramento_ResultadoOrdens_SelecionarTodos").closest("table").find("tbody tr input[type='checkbox']:checked").closest("tr");
    var linhasSelecionadas   = $("#tblBusca_OrdensStop_Resultados").find("tbody tr input[type='checkbox']:checked");

    //linhasSelecionadas.find("#tdStatus").html("Cancelado");                   //--> Aplicando a atualização do status.
    linhasSelecionadas.closest(".ABERTA, .PARCIALMENTEEXECUTADA").find("td").eq(13).html("Cancelada");
    linhasSelecionadas.closest("tr").find("td:gt(0)").effect("highlight", {}, 2700);        //--> Aplicando o efeito hightlight nas linhas selecionadas.

    linhasSelecionadas.closest("tr").removeClass("ABERTA");
    linhasSelecionadas.closest("tr").addClass("CANCELADA");

    linhasSelecionadas.find("input[type='checkbox']").prop("checked", false); //--> Retirando as marcações de check do checkbox.
    linhasSelecionadas.closest('td').find("label").removeClass("checked");                  //--> Retirando as marcações de label.

    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

//---\\

function GradIntra_Monitoramento_ExcluirOrdensNovoOMSSelecionadas(pListaDeOrdens, pListaDePortas) 
{
    var lDados = { Acao: "ExcluirOrdens", Ids: pListaDeOrdens, Portas: pListaDePortas };

    GradIntra_CarregarJsonVerificandoErro("Monitoramento/ResultadoOrdensNovoOMS.aspx"
                                          , lDados
                                          , function (pResposta) { GradIntra_Monitoramento_ExcluirOrdensNovoOMSSelecionadas_Callback(pResposta); }
                                         );

    return false;
}

function GradIntra_Monitoramento_ExcluirOrdensSelecionadasSpider(pListaDeOrdens, pListaDePortas, pAccounts, pPapeis) 
{
    var lDados = { Acao: "ExcluirOrdens", Ids: pListaDeOrdens, Portas: pListaDePortas, Accounts: pAccounts, Symbols: pPapeis };

    GradIntra_CarregarJsonVerificandoErro("Monitoramento/ResultadoOrdensSpider.aspx"
                                          , lDados
                                          , function (pResposta) { GradIntra_Monitoramento_ExcluirOrdensSpiderSelecionadas_Callback(pResposta); }
                                         );

    return false;
}

function GradIntra_Monitoramento_ExcluirOrdensSelecionadas(pListaDeOrdens, pListaDePortas) 
{
    var lDados = { Acao: "ExcluirOrdens", Ids: pListaDeOrdens, Portas: pListaDePortas };

    GradIntra_CarregarJsonVerificandoErro(  "Monitoramento/ResultadoOrdens.aspx"
                                          , lDados
                                          , function (pResposta) { GradIntra_Monitoramento_ExcluirOrdensSelecionadas_Callback(pResposta); }
                                         );

    return false;
}

function GradIntra_Monitoramento_ExcluirOrdensSpiderSelecionadas_Callback(pResposta) 
{
    //var linhasSelecionadas = $("#chkBusca_SelecionarTodosResultados").closest("table").find("tbody tr input[type='checkbox']:checked").closest("tr");
    var linhasSelecionadas = $("#tblBusca_OrdensSpider_Resultados").find("tbody tr input[type='checkbox']:checked");
    var i = 0;
    linhasSelecionadas.each(function () {

        //linhasSelecionadas.find("#tdStatus").html("Cancelada");                   //--> Aplicando a atualização do status.
        $(linhasSelecionadas[i]).closest(".ABERTA, .PARCIALMENTEEXECUTADA, .SUBSTITUÍDA").find("td").eq(5).html("Cancelada");
        $(linhasSelecionadas[i]).closest("tr").find("td:gt(0)").effect("highlight", {}, 2700);        //--> Aplicando o efeito hightlight nas linhas selecionadas.
        $(linhasSelecionadas[i]).closest("tr").removeClass("ABERTA");
        $(linhasSelecionadas[i]).closest("tr").removeClass("PARCIALMENTEEXECUTADA");
        $(linhasSelecionadas[i]).closest("tr").removeClass("SUBSTITUÍDA");
        $(linhasSelecionadas[i]).closest("tr").addClass("CANCELADA");
        $(linhasSelecionadas[i]).find("input[type='checkbox']").prop("checked", false); //--> Retirando as marcações de check do checkbox.
        $(linhasSelecionadas[i]).closest('td').find("label").removeClass("checked");
        i++;
        //linhasSelecionadas.find("label").removeClass("checked");                  //--> Retirando as marcações de label.
    });


    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Monitoramento_ExcluirOrdensNovoOMSSelecionadas_Callback(pResposta) 
{
    //var linhasSelecionadas = $("#chkBusca_SelecionarTodosResultados").closest("table").find("tbody tr input[type='checkbox']:checked").closest("tr");
    var linhasSelecionadas = $("#tblBusca_OrdensNovoOMS_Resultados").find("tbody tr input[type='checkbox']:checked");
    var i = 0;
    linhasSelecionadas.each(function () {

        //linhasSelecionadas.find("#tdStatus").html("Cancelada");                   //--> Aplicando a atualização do status.
        $(linhasSelecionadas[i]).closest(".ABERTA, .PARCIALMENTEEXECUTADA, .SUBSTITUÍDA").find("td").eq(5).html("Cancelada");
        $(linhasSelecionadas[i]).closest("tr").find("td:gt(0)").effect("highlight", {}, 2700);        //--> Aplicando o efeito hightlight nas linhas selecionadas.
        $(linhasSelecionadas[i]).closest("tr").removeClass("ABERTA");
        $(linhasSelecionadas[i]).closest("tr").removeClass("PARCIALMENTEEXECUTADA");
        $(linhasSelecionadas[i]).closest("tr").removeClass("SUBSTITUÍDA");
        $(linhasSelecionadas[i]).closest("tr").addClass("CANCELADA");
        $(linhasSelecionadas[i]).find("input[type='checkbox']").prop("checked", false); //--> Retirando as marcações de check do checkbox.
        $(linhasSelecionadas[i]).closest('td').find("label").removeClass("checked");
        i++;
        //linhasSelecionadas.find("label").removeClass("checked");                  //--> Retirando as marcações de label.
    });
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
//    //linhasSelecionadas.find("#tdStatus").html("Cancelada");                   //--> Aplicando a atualização do status.
//    linhasSelecionadas.closest(".ABERTA, .PARCIALMENTEEXECUTADA, .SUBSTITUÍDA").find("td").eq(5).html("Cancelada");
//    linhasSelecionadas.closest("tr").find("td:gt(0)").effect("highlight", {}, 2700);        //--> Aplicando o efeito hightlight nas linhas selecionadas.

//    linhasSelecionadas.closest("tr").removeClass("ABERTA");
//    linhasSelecionadas.closest("tr").removeClass("PARCIALMENTEEXECUTADA");
//    linhasSelecionadas.closest("tr").removeClass("SUBSTITUÍDA");
//    linhasSelecionadas.closest("tr").addClass("CANCELADA");
//    linhasSelecionadas.find("input[type='checkbox']").prop("checked", false); //--> Retirando as marcações de check do checkbox.
//    linhasSelecionadas.closest('td').find("label").removeClass("checked");
//    //linhasSelecionadas.find("label").removeClass("checked");                  //--> Retirando as marcações de label.


    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}
function GradIntra_Monitoramento_ExcluirOrdensSelecionadas_Callback(pResposta)
{
    //var linhasSelecionadas = $("#chkBusca_SelecionarTodosResultados").closest("table").find("tbody tr input[type='checkbox']:checked").closest("tr");
    var linhasSelecionadas = $("#tblBusca_Ordens_Resultados").find("tbody tr input[type='checkbox']:checked");

    //linhasSelecionadas.find("#tdStatus").html("Cancelada");                   //--> Aplicando a atualização do status.
    linhasSelecionadas.closest(".ABERTA, .PARCIALMENTEEXECUTADA, .SUBSTITUÍDA").find("td").eq(5).html("Cancelada");
    linhasSelecionadas.closest("tr").find("td:gt(0)").effect("highlight", {}, 2700);        //--> Aplicando o efeito hightlight nas linhas selecionadas.

    linhasSelecionadas.closest("tr").removeClass("ABERTA");
    linhasSelecionadas.closest("tr").removeClass("PARCIALMENTEEXECUTADA");
    linhasSelecionadas.closest("tr").removeClass("SUBSTITUÍDA");
    linhasSelecionadas.closest("tr").addClass("CANCELADA");
    linhasSelecionadas.find("input[type='checkbox']").prop("checked", false); //--> Retirando as marcações de check do checkbox.
    linhasSelecionadas.closest('td').find("label").removeClass("checked");
    //linhasSelecionadas.find("label").removeClass("checked");                  //--> Retirando as marcações de label.
    

    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}
function GradIntra_Monitoramento_DesbloqueiaCustodia()
{
    var lUrl = "Monitoramento/DesbloqueioCustodia.aspx";

    var CodBovespa = $("#txtRisco_DesbloqueioCustodia_CodBovespa").val();

    var Papel      = $("#txtRisco_DesbloqueioCustodia_Instrumento").val();

    var Quantidade = $("#txtRisco_DesbloqueioCustodia_Quantidade").val();

    if (CodBovespa == "" || Papel == "" || Quantidade == "")
    {
        alert("Preencha os campos necessários!");
            return false;
    }

    var lDados     = { Acao: 'DesbloqueioCustodia', CodBovespa: CodBovespa, Papel : Papel, Quantidade : Quantidade};

    GradIntra_CarregarJsonVerificandoErro(lUrl, lDados, GradIntra_Monitoramento_DesbloqueiaCustodia_CallBack);
}

function GradIntra_Monitoramento_DesbloqueiaCustodia_CallBack(pResposta)
{
    GradIntra_ExibirMensagem('I', pResposta.Mensagem);

    //GradIntra_ExcluirItemDaListaDeItensSelecionados($('#lstItensSelecionados_Risco li.Expandida'));

    //GradIntra_Navegacao_ExpandirDadosDoPrimeiroItemSelecionado();
}

function btnSalvar_Monitoramento_DebloqueioCustodia_Click()
{
    GradIntra_Monitoramento_DesbloqueiaCustodia();

    return false;
}

function GradIntra_Monitoramento_Relatorios_Load()
{
    var lIdAssessorLogado = $("#hddIdAssessorLogado").val();

    if (lIdAssessorLogado && "" != lIdAssessorLogado)
    {
        var lComboBusca_FiltroRelatorioRisco_Assessor = $("#cmbBusca_FiltroRelatorioRisco_Assessor");
        lComboBusca_FiltroRelatorioRisco_Assessor.prop("disabled", true);
        lComboBusca_FiltroRelatorioRisco_Assessor.val(lIdAssessorLogado);
    }
}

function EscondeBotaoCancelar(pSender)
{
    var lValor = $(pSender).val();
    var lBotaoCancel = $("#btnCancelarOrdens");

    if (lValor == 'Sinacor') lBotaoCancel.prop("disabled", true); else lBotaoCancel.prop("disabled", false);
}

function lnkTermo_Efetuar_Click(pSender)
{
    if (confirm("Confirma a efetuação?")) {
        var lTR = $(pSender).closest("tr");

        var lDados = { Acao: "EfetuarTermo"
                        , IdTermo: lTR.attr("data-IdTermo")
                        , IdCliente: lTR.attr("data-IdCliente")
                        , TipoSolicitacao: lTR.attr("data-TipoSolicitacao")
        };

        if (lDados.IdCliente.indexOf("-") != -1)
            lDados.IdCliente = lDados.IdCliente.substr(0, lDados.IdCliente.indexOf("-"));

        if(lDados.IdCliente.indexOf("-") != -1)
            lDados.IdCliente = lDados.IdCliente.substr(0, lDados.IdCliente.indexOf("-"));

        GradIntra_CarregarJsonVerificandoErro("Monitoramento/ResultadoTermos.aspx", lDados, lnkTermo_Efetuar_Click_CallBack);
    }

    return false;

}

function lnkTermo_Efetuar_Click_CallBack(pResposta)
{
    if(pResposta.Mensagem == "ok")
    {
        btnBusca_Click($("#pnlBusca_Monitoramento_Termos_Form p button.btnBusca"));

        $(".bt-wrapper").remove();
    }
    else
    {
        alert(pResposta.Mensagem);
    }
}


function GradIntra_Monitoramento_AtualizarListagem()
{
    if(   $("#pnlMonitoramento_Termos_ListaDeResultados").is(":visible") 
       && $("#pnlBusca_Monitoramento_Termos_Form").is(":visible")
       )
    {
        var lCountPreAtualizacao = $("#tblTermos tr").length;

        if(gCountUltimaAtualizacao != null)
        {
            if(lCountPreAtualizacao != gCountUltimaAtualizacao)
            {
                window.open("Monitoramento/ResultadoTermos.aspx?msg=termo", "mywindow","status=0,toolbar=0,location=0,toolbar=0,resizable=0,scrollbars=0,height=320,width=240");
            }
        }

        gCountUltimaAtualizacao = lCountPreAtualizacao;

        btnBusca_Click($("#pnlBusca_Monitoramento_Termos_Form p button.btnBusca"));
    }
}


function lnkTermo_Cancelar_Click(pSender)
{
    if(confirm("Confirma o cancelamento do Termo?"))
    {
        var lTR = $(pSender).closest("tr");

        var lDados = {    Acao:            "CancelarTermo"
                        , IdTermo:         lTR.attr("data-IdTermo")
                        , IdCliente:       lTR.attr("data-IdCliente")
                     };

        if(lDados.IdCliente.indexOf("-") != -1)
            lDados.IdCliente = lDados.IdCliente.substr(0, lDados.IdCliente.indexOf("-"));

        GradIntra_CarregarJsonVerificandoErro("Monitoramento/ResultadoTermos.aspx", lDados, lnkTermo_Efetuar_Click_CallBack);
    }

    return false;
}

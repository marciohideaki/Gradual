
$(document).ready(function () {

    if ($('#dvLinhaGrupo_1')[0] != undefined && $('#FluxoAltReg')[0] != undefined)
    {

        fnAlteracaoRegulamentoCarregarDadosIniciais();

        var idFundoCadastro = $.urlParam('IdFundoCadastro');

        $('#listaSelecionarFundo').val(idFundoCadastro);

        fnCarregarFluxoFundoSelecionadoAlteracaoRegulamento(idFundoCadastro);
    }
});

function fnCarregarFluxoFundoSelecionadoAlteracaoRegulamento(idFundoCadastro) {

    if (idFundoCadastro > 0) {
        $('#hidIdFundoCadastro').val(idFundoCadastro);

        fnLimparDadosAlteracaoRegulamento();

        fnCarregarFluxoAltReg();
    }
}

function fnLimparDadosAlteracaoRegulamento() {
    $('.selStatusEtapa').val(0);
    $('.txtDtInicio').val('');
    $('.txtDtFim').val('');
    $('.txtUsuarioResp').val('');

    $('.selStatusEtapa').removeClass("btn-success").removeClass("btn-info").removeClass("btn-warning");
}

function fnAlteracaoRegulamentoCarregarDadosIniciais() {

    fnCarregarListaFundosAltReg();

    fnCarregarEtapasAltReg();

    fnCarregarListasStatusAlteracaoRegulamento();

    $('.txtDtInicio').datepicker();
    $('.txtDtFim').datepicker();

    //configuração regional do datepicker
    $.datepicker.regional['pt-BR'] = {
        closeText: 'Fechar',
        prevText: '&#x3c;Anterior',
        nextText: 'Pr&oacute;ximo&#x3e;',
        currentText: 'Hoje',
        monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
        'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional["pt-BR"]);

    $(".txtDtInicio").blur(function () {
        fnValidarData(this);
    });
    $(".txtDtFim").blur(function () {
        fnValidarData(this);
    });    
}

//carrega as etapas de aprovação para preenchimento por parte do usuário
function fnCarregarEtapasAltReg() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAlteracaoRegulamento.aspx");

    var lDados =
        {
            Acao: 'CarregarEtapasFluxoAprovacao'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarEtapasAltReg_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarEtapasAltReg_CallBack(pResposta) {

    var lData = JSON.parse(pResposta);
    
    var html = '';

    var idFluxoAlteracaoRegulamentoGrupo = 0;
    var numEtapaPorGrupo = 1;

    for (var i = 0; i < lData.length; i++) {
        
        if (idFluxoAlteracaoRegulamentoGrupo !== lData[i].IdFluxoAlteracaoRegulamentoGrupo)
            numEtapaPorGrupo = 1;

        idFluxoAlteracaoRegulamentoGrupo = lData[i].IdFluxoAlteracaoRegulamentoGrupo;
                
        html +=
            '<div class="row" id="dvLinhaEtapa_' + lData[i].IdFluxoAlteracaoRegulamentoGrupoEtapa + '" style="padding-top: 5px">' +
                '<div class="row" style="height: 35px">' +
                    '<div class="col-md-4 dsEtapa" style="width: 300px">' +
                        numEtapaPorGrupo + '. ' + lData[i].DsFluxoAlteracaoRegulamentoGrupoEtapa + //descrição da etapa
                    '</div>' +
                    '<div class="col-md-2" style="width: 200px">' +
                        '<select class="form-control selStatusEtapa ctrEtapa" onchange="fnTrocarCorSelectAltReg(this)">' +
                        '<option value="0">Selecione</option>' +
                        '</select>' +
                    '</div>' +
                    '<div class="col-md-2" style="width: 130px">' +
                        '<input type="text" class="txtDtInicio ctrEtapa" placeholder="Data Início" />' +
                    '</div>' +
                    '<div class="col-md-2" style="width: 130px">' +
                        '<input type="text" class="txtDtFim ctrEtapa" placeholder="Data Fim" />' +
                    '</div>' +
                    '<div class="col-md-2" style="width: 180px">' +
                        '<input type="text" class="txtUsuarioResp"/>' +
                    '</div>' +
                '</div>\n' +
                '<div class="row rowBotoesGravar">' +
                    '<div style="width: 100%; padding-right: 75px; height: 60px">' +
                        '<div style="float: right; padding-top: 10px">' +
                            '<input class="btn btn-primary btn-sm btnGravarEtapa" type="button" value="Gravar" onclick="fnGravarEtapaComAnexoAltReg(' + lData[i].IdFluxoAlteracaoRegulamentoGrupoEtapa + ')">' +
                        '</div>' +
                        '<div style="float: right">' +
                            '<div class="fileUpload btn btn-info btn-sm">' +
                                '<span>Anexar...</span>' +
                                '<input type="file" class="upload btnAnexarArquivoEtapa" />' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>\n' +
            '</div>\n';
        
        $('#dvLinhaEtapasGrupo_' + idFluxoAlteracaoRegulamentoGrupo).append(html);
        
        numEtapaPorGrupo++;

        html = '';
    }

    fnDesabilitarTodosOsCamposParaEdicaoAltReg();
    
    $('.separator.s1').height(30);
}

function fnAnexarArquivo(id) {
    bootbox.alert('anexar arquivo à etapa ' + id);
}

function fnCarregarListasStatusAlteracaoRegulamento()
{
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAlteracaoRegulamento.aspx");

    var lDados =
        {
            Acao: 'CarregarListasStatus'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListasStatusAlteracaoRegulamento_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

//carrega os status parametrizados
function fnCarregarListasStatusAlteracaoRegulamento_CallBack(pResposta)
{

    //obtém todos os selects de status da tela
    var $select = $('.selStatusEtapa');
    
    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFluxoAlteracaoRegulamentoStatus).text(this.DsFluxoAlteracaoRegulamentoStatus);

        $select.append($option);
    });
}

//habilita campos e botões para que usuário possa gravar uma determinada etapa
function fnHabilitarCamposParaEdicaoAltReg(etapa, habilitarBotoes) {    

    var ultimaEtapaDoFluxo = false;

    while (habilitarBotoes && $('#dvLinhaEtapa_' + etapa + ' .ctrEtapa').length < 1) {
        etapa--;

        ultimaEtapaDoFluxo = true;
    }
    
    if (ultimaEtapaDoFluxo &&
        ($('#dvLinhaEtapa_' + etapa + ' .selStatusEtapa').val() == 1 || $('#dvLinhaEtapa_' + etapa + ' .selStatusEtapa').val() == 2))
    {
        $('#dvLinhaEtapa_' + etapa + ' .ctrEtapa').attr('disabled', true);
    }
    else
        $('#dvLinhaEtapa_' + etapa + ' .ctrEtapa').attr('disabled', false);
    
    var currentDate = new Date();

    if ($('#dvLinhaEtapa_' + etapa + ' .txtDtInicio').val() == '')
        $('#dvLinhaEtapa_' + etapa + ' .txtDtInicio').datepicker("setDate", currentDate);

    if (habilitarBotoes) {
        $('#dvLinhaEtapa_' + etapa + ' .btnAnexarArquivoEtapa').show();
        $('#dvLinhaEtapa_' + etapa + ' .btnGravarEtapa').show();
        $('#dvLinhaEtapa_' + etapa + ' .rowBotoesGravar').show();
    }
}

function fnDesabilitarTodosOsCamposParaEdicaoAltReg()
{
    $('.ctrEtapa').attr('disabled', true);
    $('.txtUsuarioResp').attr('disabled', true);
    $('.btnAnexarArquivoEtapa').hide();
    $('.btnGravarEtapa').hide();
    $('.rowBotoesGravar').hide();
}

function fnTrocarCorSelectAltReg(select) {

    fnRemoverCoresSelectAltReg(select);

    if ($(select).val() === "1") {
        $(select).addClass("btn-success");
    }
    else if ($(select).val() === "2") {
        $(select).addClass("btn-info");
    }
    else if ($(select).val() === "3" || $(select).val() === "4") {
        $(select).addClass("btn-warning");
    }
}

function fnRemoverCoresSelectAltReg(select)
{
    $(select).removeClass("btn-success").removeClass("btn-info").removeClass("btn-warning");
}

function fnGravarEtapaComAnexoAltReg(idFluxoGrupoEtapa) {

    var ultimaEtapa = idFluxoGrupoEtapa;
    
    for (idFluxoGrupoEtapa = 1; idFluxoGrupoEtapa <= ultimaEtapa; idFluxoGrupoEtapa++)
    {
        var habilitada = !($('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .selStatusEtapa').prop("disabled"));
        var idFluxoAlteracaoRegulamentoStatus = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .selStatusEtapa').val();

        if (habilitada //só grava as etapas habilitadas para edição
            && fnValidarDadosGravacao(idFluxoGrupoEtapa) //valida os dados da etapa
            && (idFluxoGrupoEtapa === ultimaEtapa || idFluxoAlteracaoRegulamentoStatus === "1" || idFluxoAlteracaoRegulamentoStatus === "2")
            ) { //verifica se os dados são válidos

            var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAlteracaoRegulamento.aspx");
            var dtInicio = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .txtDtInicio').val();
            var dtConclusao = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .txtDtFim').val();

            var idFundoCadastro = $('#hidIdFundoCadastro').val();

            var formData = new FormData();

            var $i = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .btnAnexarArquivoEtapa'),
            input = $i[0];

            if (input.files && input.files[0]) {

                var file = input.files[0];
                var fr = new FileReader();
                /* fr.onload = function () {
                    $('#file-content').append($('<div/>').html(fr.result))
                }; */
                fr.readAsDataURL(file);
                lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAlteracaoRegulamento.aspx");
                formData.append("NomeArquivoAnexo", file.name);
                formData.append("ArquivoAnexo", file);
            }

            formData.append('Acao', 'GravarEtapaFluxoAprovacaoFundo');
            formData.append('IdFundoCadastro', idFundoCadastro);
            formData.append('IdFundoFluxoGrupoEtapa', idFluxoGrupoEtapa);
            formData.append('IdFundoFluxoStatus', idFluxoAlteracaoRegulamentoStatus);
            formData.append('DtInicio', dtInicio);
            formData.append('DtConclusao', dtConclusao);

            $.ajax({
                url: lUrl,
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                async: false,
                success: fnGravarEtapa_CallBack,
                error: Aux_TratarRespostaComErro
            });
        }
    }
    fnCarregarFluxoAltReg();
}

function fnGravarEtapaAltReg_CallBack(pResposta) {
    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK") {
        
        resp.Mensagem = $('#hidDsEtapaAtual').val() +  ":\nEtapa atualizada com sucesso";
    }

    bootbox.alert(resp.Mensagem);
}

function fnCarregarFluxoAltReg() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAlteracaoRegulamento.aspx");

    var idFundoCadastro = $('#hidIdFundoCadastro').val();

    var lDados =
        {
            Acao: 'CarregarFluxoAprovacaoPorFundo'
          , IdFundoCadastro: idFundoCadastro
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarFluxoAprovacaoFundoAltReg_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarFluxoAprovacaoFundoAltReg_CallBack(pResposta)
{

    fnDesabilitarTodosOsCamposParaEdicaoAltReg();

    var lData = JSON.parse(pResposta);

    //caso não haja dados, deve habilitar apenas a primeira etapa para edição
    if (lData.length == 0)
        fnHabilitarCamposParaEdicao(1, true);
    else {
        $.each(lData, function () {

            $('#dvLinhaEtapa_' + this.IdFluxoAlteracaoRegulamentoGrupoEtapa + ' .selStatusEtapa').val(this.IdFluxoAlteracaoRegulamentoStatus);
            $('#dvLinhaEtapa_' + this.IdFluxoAlteracaoRegulamentoGrupoEtapa + ' .txtDtInicio').val(this.DtInicio);
            $('#dvLinhaEtapa_' + this.IdFluxoAlteracaoRegulamentoGrupoEtapa + ' .txtDtFim').val(this.DtConclusao);
            $('#dvLinhaEtapa_' + this.IdFluxoAlteracaoRegulamentoGrupoEtapa + ' .txtUsuarioResp').val(this.UsuarioResponsavel);

            fnTrocarCorSelectAltReg($('#dvLinhaEtapa_' + this.IdFluxoAlteracaoRegulamentoGrupoEtapa + ' .selStatusEtapa'));

            //habilitar etapa caso o status da mesma seja em análise ou a definir
            if (this.IdFluxoAlteracaoRegulamentoStatus === 3 || this.IdFluxoAlteracaoRegulamentoStatus === 4)
                fnHabilitarCamposParaEdicao(this.IdFluxoAlteracaoRegulamentoGrupoEtapa, false);
            else
                $('#dvLinhaEtapa_' + this.IdFluxoAlteracaoRegulamentoGrupoEtapa + ' .txtDtFim').removeAttr('placeholder');
        });
        
        //habilita a etapa seguinte para edição, ou seja, a próxima ainda não gravada
        fnHabilitarCamposParaEdicao(lData[lData.length - 1].IdFluxoAlteracaoRegulamentoGrupoEtapa + 1, true);
    }
}

//verifica o preenchimento dos dados
function fnValidarDadosGravacaoAltReg(idFluxoAlteracaoRegulamentoGrupoEtapa)
{

    //armazena descrição da etapa para exibir no alerta
    var etapa = $('#dvLinhaEtapa_' + idFluxoAlteracaoRegulamentoGrupoEtapa + ' .dsEtapa').text();

    //cria um hidden para armazenar a descricao da etapa
    if ($('#hidDsEtapaAtual').val() === undefined) $('form').append('<input type="hidden" id="hidDsEtapaAtual" />');

    $('#hidDsEtapaAtual').val(etapa);

    var idFundoFluxoStatus = $('#dvLinhaEtapa_' + idFluxoAlteracaoRegulamentoGrupoEtapa + ' .selStatusEtapa').val();
    var dtInicio = $('#dvLinhaEtapa_' + idFluxoAlteracaoRegulamentoGrupoEtapa + ' .txtDtInicio').val();
    var msg = '';

    if (idFundoFluxoStatus < 1)
        msg += 'Selecione um status \n';
    if (dtInicio === '')
        msg += 'Preencha a data de início \n';

    if (msg === '')
        return true;

    bootbox.alert("Etapa - " + etapa + ': ' + msg);
    return false;
}

function fnValidarDataAltReg(control) {

    if (($(control).val() !== '')) {
        var regExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])      [\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;

        if (!(($(control).val().match(regExPattern)))) {
            $(control).val('');
        }
    }
}

function fnCarregarListaFundosAltReg() {
    
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAlteracaoRegulamento.aspx");

    var lDados =
        {
            Acao: 'CarregarSelectFundos'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListaFundosAltReg_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarListaFundosAltReg_CallBack(pResposta) {

    //obtém todos os selects de status da tela
    var $select = $('#listaSelecionarFundo');

    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoCadastro).text(this.NomeFundo);

        $select.append($option);
    });
}

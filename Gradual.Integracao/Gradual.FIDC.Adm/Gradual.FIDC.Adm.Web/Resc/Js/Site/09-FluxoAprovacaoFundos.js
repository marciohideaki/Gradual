
$(document).ready(function () {

    if ($('#dvLinhaGrupo_1')[0] != undefined && $('#FluxoAprovacaoFundo')[0] != undefined)
    {

        fnAprovacaoFundosCarregarDadosIniciais();

        var idFundoCadastro = $.urlParam('IdFundoCadastro');

        $('#listaSelecionarFundo').val(idFundoCadastro);

        fnCarregarFluxoFundoSelecionado(idFundoCadastro);
    }
});

function fnCarregarFluxoFundoSelecionado(idFundoCadastro) {

    if (idFundoCadastro > 0) {
        $('#hidIdFundoCadastro').val(idFundoCadastro);

        fnLimparDados();

        fnCarregarFluxoAprovacaoFundo();
    }
}

function fnLimparDados() {
    $('.selStatusEtapa').val(0);
    $('.txtDtInicio').val('');
    $('.txtDtFim').val('');
    $('.txtUsuarioResp').val('');

    $('.selStatusEtapa').removeClass("btn-success").removeClass("btn-info").removeClass("btn-warning");
}

function fnAprovacaoFundosCarregarDadosIniciais() {

    fnCarregarListaFundos();

    fnCarregarEtapas();

    fnCarregarListasStatus();

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
function fnCarregarEtapas() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAprovacaoFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarEtapasFluxoAprovacao'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarEtapas_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarEtapas_CallBack(pResposta) {

    var lData = JSON.parse(pResposta);
    
    var html = '';

    var idFundoFluxoGrupo = 0;
    var numEtapaPorGrupo = 1;

    for (var i = 0; i < lData.length; i++) {
        
        if (idFundoFluxoGrupo !== lData[i].IdFundoFluxoGrupo)
            numEtapaPorGrupo = 1;

        idFundoFluxoGrupo = lData[i].IdFundoFluxoGrupo;
                
        html +=
            '<div class="row" id="dvLinhaEtapa_' + lData[i].IdFundoFluxoGrupoEtapa + '" style="padding-top: 5px">' +
                '<div class="row" style="height: 35px">' +
                    '<div class="col-md-4 dsEtapa" style="width: 300px">' +
                        numEtapaPorGrupo + '. ' + lData[i].DsFundoFluxoGrupoEtapa + //descrição da etapa
                    '</div>' +
                    '<div class="col-md-2" style="width: 200px">' +
                        '<select class="form-control selStatusEtapa ctrEtapa" onchange="fnTrocarCorSelect(this)">' +
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
                            '<input class="btn btn-primary btn-sm btnGravarEtapa" type="button" value="Gravar" onclick="fnGravarEtapaComAnexo(' + lData[i].IdFundoFluxoGrupoEtapa + ')">' +
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
        
        $('#dvLinhaEtapasGrupo_' + idFundoFluxoGrupo).append(html);
        
        numEtapaPorGrupo++;

        html = '';
    }

    fnDesabilitarTodosOsCamposParaEdicao();
    
    $('.separator.s1').height(30);
}

function fnAnexarArquivo(id) {
    bootbox.alert('anexar arquivo à etapa ' + id);
}

function fnCarregarListasStatus() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAprovacaoFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarListasStatus'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListasStatus_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });

    //Aux_CarregarHtmlVerificandoErro(lUrl, lDados, fnCarregarListasStatus_CallBack);
}

//carrega os status parametrizados
function fnCarregarListasStatus_CallBack(pResposta) {

    //obtém todos os selects de status da tela
    var $select = $('.selStatusEtapa');
    
    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoFluxoStatus).text(this.DsFundoFluxoStatus);

        $select.append($option);
    });
}

//habilita campos e botões para que usuário possa gravar uma determinada etapa
function fnHabilitarCamposParaEdicao(etapa, habilitarBotoes) {    

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

function fnDesabilitarTodosOsCamposParaEdicao() {
    $('.ctrEtapa').attr('disabled', true);
    $('.txtUsuarioResp').attr('disabled', true);
    $('.btnAnexarArquivoEtapa').hide();
    $('.btnGravarEtapa').hide();
    $('.rowBotoesGravar').hide();
}

function fnTrocarCorSelect(select) {

    fnRemoverCoresSelect(select);

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

function fnRemoverCoresSelect(select) {
    $(select).removeClass("btn-success").removeClass("btn-info").removeClass("btn-warning");
}

function fnGravarEtapaComAnexo(idFluxoGrupoEtapa) {

    var ultimaEtapa = idFluxoGrupoEtapa;
    
    for (idFluxoGrupoEtapa = 1; idFluxoGrupoEtapa <= ultimaEtapa; idFluxoGrupoEtapa++) {

        var habilitada = !($('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .selStatusEtapa').prop("disabled"));
        var idFundoFluxoStatus = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .selStatusEtapa').val();

        var valido = fnValidarDadosGravacao(idFluxoGrupoEtapa);

        if (habilitada //só grava as etapas habilitadas para edição
            && valido //valida os dados da etapa
            && (idFluxoGrupoEtapa === ultimaEtapa || idFundoFluxoStatus === "1" || idFundoFluxoStatus === "2")
            ) { //verifica se os dados são válidos

            var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAprovacaoFundos.aspx");
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
                lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAprovacaoFundos.aspx");
                formData.append("NomeArquivoAnexo", file.name);
                formData.append("ArquivoAnexo", file);
            }

            formData.append('Acao', 'GravarEtapaFluxoAprovacaoFundo');
            formData.append('IdFundoCadastro', idFundoCadastro);
            formData.append('IdFundoFluxoGrupoEtapa', idFluxoGrupoEtapa);
            formData.append('IdFundoFluxoStatus', idFundoFluxoStatus);
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
    fnCarregarFluxoAprovacaoFundo();
}

function fnGravarEtapa_CallBack(pResposta) {
    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK") {
        
        resp.Mensagem = $('#hidDsEtapaAtual').val() +  ":\nEtapa atualizada com sucesso";
    }

    bootbox.alert(resp.Mensagem);
}

function fnCarregarFluxoAprovacaoFundo() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAprovacaoFundos.aspx");

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
            , success: fnCarregarFluxoAprovacaoFundo_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarFluxoAprovacaoFundo_CallBack(pResposta) {

    fnDesabilitarTodosOsCamposParaEdicao();

    var lData = JSON.parse(pResposta);

    //caso não haja dados, deve habilitar apenas a primeira etapa para edição
    if (lData.length == 0)
        fnHabilitarCamposParaEdicao(1, true);
    else {
        $.each(lData, function () {

            $('#dvLinhaEtapa_' + this.IdFundoFluxoGrupoEtapa + ' .selStatusEtapa').val(this.IdFundoFluxoStatus);
            $('#dvLinhaEtapa_' + this.IdFundoFluxoGrupoEtapa + ' .txtDtInicio').val(this.DtInicio);
            $('#dvLinhaEtapa_' + this.IdFundoFluxoGrupoEtapa + ' .txtDtFim').val(this.DtConclusao);
            $('#dvLinhaEtapa_' + this.IdFundoFluxoGrupoEtapa + ' .txtUsuarioResp').val(this.UsuarioResponsavel);

            fnTrocarCorSelect($('#dvLinhaEtapa_' + this.IdFundoFluxoGrupoEtapa + ' .selStatusEtapa'));

            //habilitar etapa caso o status da mesma seja em análise ou a definir
            if (this.IdFundoFluxoStatus === 3 || this.IdFundoFluxoStatus === 4)
                fnHabilitarCamposParaEdicao(this.IdFundoFluxoGrupoEtapa, false);
            else
                $('#dvLinhaEtapa_' + this.IdFundoFluxoGrupoEtapa + ' .txtDtFim').removeAttr('placeholder');
        });
        
        //habilita a etapa seguinte para edição, ou seja, a próxima ainda não gravada
        fnHabilitarCamposParaEdicao(lData[lData.length - 1].IdFundoFluxoGrupoEtapa + 1, true);
    }
}

//verifica o preenchimento dos dados
function fnValidarDadosGravacao(idFluxoGrupoEtapa) {

    //armazena descrição da etapa para exibir no alerta
    var etapa = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .dsEtapa').text();

    //cria um hidden para armazenar a descricao da etapa
    if ($('#hidDsEtapaAtual').val() === undefined) $('form').append('<input type="hidden" id="hidDsEtapaAtual" />');

    $('#hidDsEtapaAtual').val(etapa);

    var idFundoFluxoStatus = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .selStatusEtapa').val();
    var dtInicio = $('#dvLinhaEtapa_' + idFluxoGrupoEtapa + ' .txtDtInicio').val();
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

function fnValidarData(control) {

    if (($(control).val() !== '')) {
        var regExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])      [\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;

        if (!(($(control).val().match(regExPattern)))) {
            $(control).val('');
        }
    }
}

function fnCarregarListaFundos() {
    
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/FluxoAprovacaoFundos.aspx");

    var lDados =
        {
            Acao: 'CarregarSelectFundos'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnCarregarListaFundos_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnCarregarListaFundos_CallBack(pResposta) {

    //obtém todos os selects de status da tela
    var $select = $('#listaSelecionarFundo');

    $.each(JSON.parse(pResposta), function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoCadastro).text(this.NomeFundo);

        $select.append($option);
    });
}

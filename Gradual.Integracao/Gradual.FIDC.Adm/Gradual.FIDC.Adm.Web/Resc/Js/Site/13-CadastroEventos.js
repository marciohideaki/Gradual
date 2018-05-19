var gEventos;
var gMock_EventosGrid = {};
var filterDate = "";
var filterDateEnd = "";
var filterIdFundoCadastro = 0;
var url_raiz = "";

$(document).ready(function () {
    if ($('#lblDataFiltro').val() != undefined) {
        url_raiz = "/CadastroFundos/CalendarioEventos.aspx";

        loadDatePickerCadEvento();
        loadDatePickerCadEventoHorario();
        loadDatePickerModal();

        filterDate = todaySimpleFormat();

        $('#lblDataFiltro').text('Eventos cadastrados para o dia ' + filterDate);

        fnGetFundos();
        getGridEventos();
    }
});

function getGridEventos() {
    this.getEventosGrid();
    this.filterCalendarioEventos();
}

function loadDatePickerCadEvento() {
    $('#datepickerCadEvento').datetimepicker({
        inline: true,
        format: "DD MM YYYY",
        locale: moment.locale("pt-br"),
    });

    $('#datepickerCadEvento').on("dp.change", function (e) {
        dataSelecionada = e.date.format("DD/MM/YYYY");
        $('#lblDataFiltro').text('Eventos cadastrados para o dia ' + dataSelecionada);
        filterDate = dataSelecionada;

        //Limpa filtro avancado
        filterDateEnd = "";
        filterIdFundoCadastro = 0;

        getGridEventos();
    });
};

function loadDatePickerCadEventoHorario() {
    $('#dtHorarioEvento').datetimepicker({
        format: "HH:mm",
        locale: moment.locale("pt-br"),
    });

    $('#txtHorarioEvento').datetimepicker({
        format: "HH:mm",
        locale: moment.locale("pt-br"),
    });
}

function loadDatePickerModal() {
    $('#dtFiltroAvancadoInicio').datetimepicker({
        format: "DD/MM/YYYY",
        locale: moment.locale("pt-br"),
    });

    $('#txtFiltroAvancadoInicio').datetimepicker({
        format: "DD/MM/YYYY",
        locale: moment.locale("pt-br"),
    });

    $('#dtFiltroAvancadoFim').datetimepicker({
        format: "DD/MM/YYYY",
        locale: moment.locale("pt-br"),
    });

    $('#txtFiltroAvancadoFim').datetimepicker({
        format: "DD/MM/YYYY",
        locale: moment.locale("pt-br"),
    });
}

function filterCalendarioEventos() {
    var lUrl = Aux_UrlComRaiz(url_raiz);

    var lDados =
        {
            Acao: "CarregarHtmlComDados"
            , DtEvento: filterDate
            , DtEventoEnd: filterDateEnd
            , IdFundoCadastro: filterIdFundoCadastro
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, filterCalendarioEventos_CallBack);

    return false;
}

function filterCalendarioEventos_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_EventosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdCalendarioEvento: lData.Itens[i].IdCalendarioEvento,
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        NomeFundo: lData.Itens[i].NomeFundo,
                        DtEvento: lData.Itens[i].DtEvento,
                        DescEvento: lData.Itens[i].DescEvento,
                        EmailEvento: lData.Itens[i].EmailEvento,
                        EnviarNotificacaoDia: lData.Itens[i].EnviarNotificacaoDia,
                        MostrarHome: lData.Itens[i].MostrarHome,
                    };

                gMock_EventosGrid.rows.push(lObjeto);
            }
        }

        $('#gridEventos')
            .jqGrid('clearGridData')  //Limpa
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_EventosGrid.rows }) //Configura e preenche
            .trigger("reloadGrid"); // Recarrega

        $('.ui-pg-input').width(20);
    }
}

function getEventosGrid() {

    gEventos =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_EventosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdCalendarioEvento", jsonmap: "IdCalendarioEvento", index: "IdCalendarioEvento", align: "left", sortable: true, hidden: true }
                    , { label: "Eventos Cadastrados", name: "NomeFundo", jsonmap: "NomeFundo", index: "NomeFundo", width: 150, align: "left", sortable: true }
                    , { label: "Data do Evento", name: "DtEvento", jsonmap: "DtEvento", index: "DtEvento", width:80, align: "left", sortable: true }
                    , { label: "Descrição", name: "DescEvento", jsonmap: "DescEvento", index: "DescEvento", width: 150, align: "left", sortable: true }
                    , { label: "E-mail", name: "EmailEvento", jsonmap: "EmailEvento", index: "EmailEvento", width: 110, align: "left", sortable: true }
                    , { label: "Notificação", name: "EnviarNotificacaoDia", jsonmap: "EnviarNotificacaoDia", index: "EnviarNotificacaoDia", width: 60, align: "center", sortable: false }
                    , { label: "Mostrar", name: "MostrarHome", jsonmap: "MostrarHome", index: "MostrarHome", width: 50, align: "center", sortable: false }
                    , { name: '', index: 'excluir', width: 30, formatter: excluirEventoFormatter, sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortname: "DtEvento"
      , sortorder: "asc"
      , viewrecords: true
      , gridview: false
      , pager: '#pgEventos'
      , subGrid: false
      , rowNum: 10
      , rowList: [20, 30, 40, 50, 60]
      , caption: '&nbsp;'
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
    };

    $('#gridEventos').jqGrid(gEventos);
}

function addEvento() {
    //verifica dados obrigatórios
    var erros = '';
    var bullet = '&#8226 ';

    if ($("#txtHorarioEvento").val() === '')
        erros += bullet + 'Informe o horário do evento.\n' + '</br>';

    if ($("#txtDescEvento").val() === '')
        erros += bullet + 'Informe a descrição do evento.\n' + '</br>';

    if ($("#lsRelacionarFundo").prop('selectedIndex') <= 0)
        erros += bullet + 'Selecione o fundo relacionado ao evento.\n' + '</br>';

    if (($("#txtEmailNotificacao").val() === '') || !isValidEmailAddress($("#txtEmailNotificacao").val()))
        erros += bullet + 'O e-mail de notificação precisa ser válido.\n' + '</br>';

    //caso todos os dados estejam preenchidos corretamente, prossegue com o cadastro do evento
    if (erros === "") {
        var lUrl = Aux_UrlComRaiz(url_raiz);

        var lDados =
            {
                Acao: "AddEvento"
              , IdFundoCadastro: $("#lsRelacionarFundo").val()
              , DtEvento: filterDate + " " + $("#txtHorarioEvento").val()
              , DescEvento: $("#txtDescEvento").val()
              , EmailEvento: $("#txtEmailNotificacao").val()
              , EnviarNotificacaoDia: $("#chkEnviarNotifDia").is(":checked")
              , MostrarHome: $("#chkMostrarHome").is(":checked")
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, addEvento_Callback);
    }
    else {
        bootbox.alert(erros);
    }

    return false;
}

function addEvento_Callback(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK")
        resp.Mensagem = "Evento adicionado com sucesso.";

    LimparDadosEvento();
    getGridEventos();

    bootbox.alert(resp.Mensagem);
}

function fnGetFundos() {

    var lUrl = Aux_UrlComRaiz(url_raiz);

    var lDados =
        {
            Acao: 'GetFundos'
        }

    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: fnGetFundos_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function fnGetFundos_CallBack(pResposta) {
    var fundos = JSON.parse(pResposta);

    var $select = $('#lsRelacionarFundo');
    $.each(fundos, function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoCadastro).text(this.NomeFundo);
        $select.append($option);
    });

    var $selectModal = $('#lsFiltroAvancadoFundoRelacionado');
    $.each(fundos, function () {
        var $option = $('<option />');
        $option.attr('value', this.IdFundoCadastro).text(this.NomeFundo);
        $selectModal.append($option);
    });
}

function excluirEventoFormatter(cellvalue, options) {
    return "<a href='#' style='height:25px;width:5px;' type='button' title='Excluir' onclick=\"excluirCalendarioEvento(" + options.rowId + ")\" >Excluir</a>";
}

function excluirCalendarioEvento(id) {
    bootbox.confirm("Você realmente deseja excluir este evento?", function (confirm) {
        if (confirm) {
            var lUrl = Aux_UrlComRaiz(url_raiz);
            var lDados =
                {
                    Acao: "RemoverEvento"
                    , IdCalendarioEvento: id
                };
            Aux_CarregarHtmlVerificandoErro(lUrl, lDados, excluirCalendarioEvento_Callback);
            jQuery('#gridEventos').jqGrid('setSelection', id);
        }
        else {
            return;
        }        
    });
}

function excluirCalendarioEvento_Callback(pResposta) {
    if (pResposta) {
        getGridEventos();
        bootbox.alert("Evento excluído com sucesso.");
    }
    else {
        bootbox.alert("Não foi possível excluir o evento.");
    }
}

function advFilter() {
    LimparDadosEvento();

    $('#mdlFiltroAvancado').modal('show');
}

function getEventosFiltroAvancado() {
    var erros = '';
    var bullet = '&#8226 ';
    var preenchidos = true;

    var filtroFundo = false;
    var filtroDatas = false;
    var filtroSomenteFuturos = $('#chkFiltroAvancadoSomenteEventosFuturos').is(":checked");

    if ($("#lsFiltroAvancadoFundoRelacionado").val() > 0)
        filtroFundo = true;

    if (!filtroSomenteFuturos) {
        if ($("#txtFiltroAvancadoInicio").val() === '')
            preenchidos = false;
        else
            filtroDatas = true;

        if ($("#txtFiltroAvancadoFim").val() === '')
            preenchidos = false;
        else
            filtroDatas = true;

        if (($("#txtFiltroAvancadoInicio").val() === '' && $("#txtFiltroAvancadoFim").val() != '')
         || ($("#txtFiltroAvancadoInicio").val() != '' && $("#txtFiltroAvancadoFim").val() === '')) {
            erros += bullet + 'Insira as duas datas.\n' + '</br>';
        }

        if (preenchidos) {
            var dtIniTxt = $("#txtFiltroAvancadoInicio").val();
            var dtFimTxt = $("#txtFiltroAvancadoFim").val();

            var dtIni = new Date(dtIniTxt.substring(6, 10), (dtIniTxt.substring(3, 5) - 1), dtIniTxt.substring(0, 2));
            var dtFim = new Date(dtFimTxt.substring(6, 10), (dtFimTxt.substring(3, 5) - 1), dtFimTxt.substring(0, 2), 23, 59, 59);

            if (dtIni > dtFim)
                erros += bullet + 'A data inicial não pode ser maior do que a data final.\n' + '</br>';
        }
    }

    if (!filtroFundo && !filtroDatas && !filtroSomenteFuturos)
        erros += bullet + 'É necessário utilizar ao menos um dos filtros.\n' + '</br>';

    if (erros === "") {
        if (filtroSomenteFuturos || filtroFundo) {
            filterDate = todaySimpleFormat();
            filterDateEnd = yearAdvance(); //Adiciona 1 ano para trazer futuros
        }
        else {
            filterDate = $("#txtFiltroAvancadoInicio").val();
            filterDateEnd = $("#txtFiltroAvancadoFim").val();
        }
        filterIdFundoCadastro = $("#lsFiltroAvancadoFundoRelacionado").val();

        getGridEventos();
        $('#mdlFiltroAvancado').modal('hide');
        $('#lblDataFiltro').text('Eventos cadastrados de ' + filterDate + ' a ' + filterDateEnd);
    }
    else {
        bootbox.alert(erros);
    }
}

function somenteFuturos() {
    if ($('#chkFiltroAvancadoSomenteEventosFuturos').is(":checked")) {
        $("#txtFiltroAvancadoInicio").val("");
        $("#txtFiltroAvancadoInicio").attr('disabled', true);
        $("#txtFiltroAvancadoFim").val("");
        $("#txtFiltroAvancadoFim").attr('disabled', true);
    }
    else {
        $("#txtFiltroAvancadoInicio").attr('disabled', false);
        $("#txtFiltroAvancadoFim").attr('disabled', false);
    }
}

//Auxiliares
function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function todaySimpleFormat() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var output = (day < 10 ? '0' : '') + day + '/' +
                (month < 10 ? '0' : '') + month + '/' +
                d.getFullYear();

    return output;
}

function yearAdvance() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var year = d.getFullYear() + 1;
    var output = (day < 10 ? '0' : '') + day + '/' +
                (month < 10 ? '0' : '') + month + '/' + year;

    return output;
}

function LimparDadosEvento() {
    $("#lsRelacionarFundo").selectpicker('refresh');
    $("#txtHorarioEvento").val("");
    $("#txtDescEvento").val("");
    $("#txtEmailNotificacao").val("");
    $("#chkEnviarNotifDia").removeAttr('checked');
    $("#chkMostrarHome").removeAttr('checked');
}

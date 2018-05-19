var ctrl = this;
ctrl.lsEventosBD = [];
ctrl.lsEventosCalendario = [];
ctrl.url_raiz = "";
ctrl.idDetail = 0;

$(document).ready(function () {
    if ($('#dvCalendarioEventos').val() != undefined) {
        ctrl.url_raiz = "/CadastroFundos/Calendario.aspx";
        ctrl.getEventos();

        $('#dvCalendarioEventos').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay,listWeek'
            },
            defaultDate: moment(new Date()).format('MM/DD/YYYY'),
            navLinks: true,
            editable: false,
            eventLimit: true,
            eventColor: "#2B78E4",
            eventClick: function (calEvent, jsEvent, view) {
                //Localiza evento na lista do banco
                var event = $.grep(ctrl.lsEventosBD, function (e) {
                    return e.IdCalendarioEvento == calEvent.id;
                })[0];

                ctrl.getDetail(event.DtEvento, event.NomeFundo, event.DescEvento, event.IdCalendarioEvento);
            },
            events: ctrl.lsEventosCalendario
        });
    };
});

function getEventos() {
    var lUrl = Aux_UrlComRaiz(ctrl.url_raiz);
    var lDados = { Acao: 'GetEventos' };
    $.ajax({
        url: lUrl
            , type: "post"
            , cache: false
            , data: lDados
            , success: ctrl.getEventos_CallBack
            , async: false
            , error: Aux_TratarRespostaComErro
    });
}

function getEventos_CallBack(pResposta) {
    ctrl.lsEventosBD = JSON.parse(pResposta);
    $.each(ctrl.lsEventosBD, function () {
        ctrl.lsEventosCalendario.push({
            id: this.IdCalendarioEvento,
            title: this.NomeFundo,
            start: this.DtEvento,
        });
    });
}

function getNavigationEvent(direction) {
    //Percorre lista de eventos localizando indice do evento em detalhe
    indexes = $.map(ctrl.lsEventosBD, function (obj, index) {
        if (obj.IdCalendarioEvento == ctrl.idDetail) {
            return index;
        }
    });

    if (direction == "next") {
        if (indexes[0] == (ctrl.lsEventosBD.length - 1))
            bootbox.alert("Não há um próximo evento.");
        else if (indexes[0] < ctrl.lsEventosBD.length) { //Caso não seja o último evento, carrega o próximo e atualiza modal
            var event = ctrl.lsEventosBD[indexes[0] + 1];
            ctrl.getDetail(event.DtEvento, event.NomeFundo, event.DescEvento, event.IdCalendarioEvento);
        }
    }
    else {
        if (indexes[0] == 0)
            bootbox.alert("Não há um evento anterior.");
        else if (indexes[0] < ctrl.lsEventosBD.length) { //Caso não seja o primeiro evento, carrega o anterior e atualiza modal
            var event = ctrl.lsEventosBD[indexes[0] - 1];
            ctrl.getDetail(event.DtEvento, event.NomeFundo, event.DescEvento, event.IdCalendarioEvento);
        }            
    }
}

function getDetail(data_hora, fundo, descricao, iddetail) {
    $('#lblDetalheEventoTitulo').text('Detalhes do Evento - ' + moment(data_hora).format('DD/MM/YYYY'));
    $('#lblDetalheEventoHora').text(moment(data_hora).format('HH:mm'));
    $('#lblDetalheEventoFundo').text(fundo);
    $('#lblDetalheEventoDesc').text(descricao);
    ctrl.idDetail = iddetail;
    $('#mdlFiltroAvancado').modal('show');
}

var idGlobal = "";
$('#editarR').click(function () {
    document.location.href = "/Reservacion/Details/" + idGlobal;
});
function buscarReserva(id) {
    var data = { id: id };
    var _url = '/Reservacion/buscarReserva';
    $.ajax({
        type: 'POST',
        url: _url,
        data: data,
        dataType: 'json',
        async: false,
        success: function (inmuebles) {
            $(inmuebles).each(function (index, value) {
                var start = new Date(parseInt(value.start.substr(6)));
                var end = new Date(parseInt(value.end.substr(6)));
                $("#infoNombre").text(value.title);
                $("#infoInmueble").text(value.inmueble);
                $("#infoTipo").text(value.tipo);
                $("#infoCantidad").text(value.cantidad);
                $("#infoTelefono").text(value.telefono);
                $("#infoFechaInicio").text(start.getDate() + '/' + (start.getMonth() + 1) + '/' + start.getFullYear());
                $("#infoFechaFin").text(end.getDate() + '/' + (end.getMonth() + 1) + '/' + end.getFullYear());
                $("#infoHoraInicio").text(value.horaI);
                $("#infoHoraFin").text(value.horaF);
                $("#infoEstado").text(value.estado);
            });
            $('#myModal').modal('show');
        },
        error: function () {
            alert("No se puede cargar la información de esta reserva!");
        }
    });
}
function cargarMes(_FechaMesBuscar) {// Se modificaron algunos parametros extras
    var _url = '/Reservacion/ReservasMes';
    var data = { _FechaMesBuscar: _FechaMesBuscar };
    $.ajax({
        type: 'POST',
        url: _url,
        data: data,
        dataType: 'json',
        async: false,
        success: function (inmuebles) {
            $(inmuebles).each(function (index, value) {

                var start = new Date(parseInt(value.start.substr(6)));
                var end = new Date(parseInt(value.end.substr(6)));
                start.setHours(parseInt(value.horaI));
                start.setMinutes(parseInt(value.minI));

                end.setHours(parseInt(value.horaF));
                end.setMinutes(parseInt(value.minF));

                var randomColor = 'rgb(13, 68, 112)'
                if (value.estado == "En Proceso") {
                    randomColor = 'rgb(154, 205, 155)'
                }

                var newEvent = new Object();
                newEvent.id = value.id;
                newEvent.title = value.title;
                newEvent.start = start;
                newEvent.end = end;
                newEvent.allDay = false;
                // newEvent.url = 'Details/' + value.id;
                newEvent.description = value.inmueble;
                newEvent.color = randomColor;
                newEvent.backgroundColor = randomColor;
                $('#calendar').fullCalendar('renderEvent', newEvent);
            });

        },
        error: function () {
            alert("Error al intentar al Servidor!");
        }

    });
}


$('body').on('click', '.fc-button-next', function () {
    $('#calendar').fullCalendar('removeEvents');
    var moment = $('#calendar').fullCalendar('getDate');
    cargarMes(moment.format());
    //alert("The current date of the calendar is " + moment.format());

});
$('body').on('click', '.fc-button-prev', function () {
    $('#calendar').fullCalendar('removeEvents');
    var moment = $('#calendar').fullCalendar('getDate');
    // alert("The current date of the calendar is " + moment.format());
    cargarMes(moment.format());
});

function modalF(id) {
    buscarReserva(id);
}

$('#calendar').fullCalendar({
    eventRender: function (event, element) {
        $(element).find(".fc-event-time").remove();
        $(element).popover({
            title: '<div class="text-info" style="text-align:center"><strong>' + event.title + '</strong></div>',
            placement: 'auto',
            html: true,
            trigger: 'hover',
            animation: 'true',
            content: '<div class="text-info">INMUEBLE: </div>' + '<div>' + event.description + '</div>' +
                      '<div class="text-info">INICIO: </div>' + event.start.format("DD-MM-YYYY h:mm A") + '<div> </div>' +
                      '<div class="text-info">FINALIZACIÓN: </div>' + event.end.format("DD-MM-YYYY h:mm A") + '<div> </div>',
            container: 'body'
        });
        $('.popover.in').remove();
    },
    eventClick: function (calEvent, jsEvent, view) {
        idGlobal = calEvent.id;
        modalF(idGlobal);
    },
    eventAfterRender: function (event, element, view) {
        $(element).css('text-align', 'center');
        $(element).css('font-weight', 'bold');
    },

    selectable: true,
    selectHelper: true,
    allDayDefault: false,
    allDaySlot: false,
    select: function (start, end, allDay) {
    },
    lang: 'es',
    header: {
        left: '',
        center: 'prev title next',
        right: ''
    },
    minTime: '07:00:00',
    maxTime: '21:00:00',
    defaultDate: new Date(),
    editable: true,
    droppable: true
});

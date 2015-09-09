$(document).ready(function () {
    $("#infoSemana").popover({
        html: true,
        placement: 'bottom',
        trigger: 'hover',
        title: "Importante",
        content: '<div style="font-size:12px;">' +
                   '<div> <strong style="color:rgb(16, 14, 48);">* 1): </strong> Si ninguno de los días es seleccionado, la reserva no se creará aunque esta tenga una fecha Inicio y Fin. </div>' +
                   '<div> <strong style="color:rgb(16, 14, 48);">* 2): </strong> Por defecto el sistema selecciona todos los días, no obstante si usted desea solo puede escojer los días que necesite el inmueble(es). </div>' + '</div>'
    });

    var listaDiasViejo = '@ListaDias'.split(',');
    var listaDias = [];
    var lista_de_inmuebles = '@ListaInmuebles'.split(',');
    var lista_de_dias = '@ListaDias'.split(',');
    for (i = 0; i <= lista_de_dias.length; i++) {
        if (lista_de_dias[i] == "0") {
            listaDias.push(0);
            $("#dom").bootstrapToggle('on');
        }
        else if (lista_de_dias[i] == "1") {
            listaDias.push(1);
            $("#lun").bootstrapToggle('on');
        }
        else if (lista_de_dias[i] == "2") {
            listaDias.push(2);
            $("#mar").bootstrapToggle('on');
        }
        else if (lista_de_dias[i] == "3") {
            listaDias.push(3);
            $("#mier").bootstrapToggle('on');
        }
        else if (lista_de_dias[i] == "4") {
            listaDias.push(4);
            $("#jue").bootstrapToggle('on');
        }
        else if (lista_de_dias[i] == "5") {
            listaDias.push(5);
            $("#vier").bootstrapToggle('on');
        }
        else if (lista_de_dias[i] == "6") {
            listaDias.push(6);
            $("#sab").bootstrapToggle('on');
        }

    }
    var estado = '@Model.TXT_ESTADO';
    var iduser = '@Session["NOM_USUARIO"]';
    document.getElementById("ID_TIPOACTIVIDAD").value = '@Model.ID_TIPOACTIVIDAD';
    if ((estado == ("Confirmada") || estado == ("Rechazada")) && iduser != "Administrador") {
        $('#idNombre').prop("disabled", true);
        $('#idTelefono').prop("disabled", true);
        $('#HoraInicio').prop("disabled", true);
        $('#HoraFin').prop("disabled", true);
        $('#idtipoactividad').prop("disabled", true);
        // $('#idNombreInmueble').prop("disabled", true);
        $('#idFechaFin').prop("disabled", true);
        $('#idFechaInicio').prop("disabled", true);
        $('#guardarReserva').prop("disabled", true);
        $('#idObservacion').prop("disabled", true);

        $('#idDescripcion').prop("disabled", true);
        $('#idOrganizador').prop("disabled", true);
        $('#idObjetivo').prop("disabled", true);
        $('#idCantidaddia').prop("disabled", true);
    } else {
        //  if ('@Model.FEC_INICIALRESERVACION' == '@Model.FEC_FINALRESERVACION') {
        // $('#idNombreInmueble').prop("disabled", true);
    }
    $('#guardarReserva').on("click", function () {
        $("#errorSpan").hide();
        if (listaDias.length <= 0) {
            $("#errorSpan").text("Debe seleccionar al menos un día de la semana para proceder con la reserva");
            $("#errorSpan").show();
            return;
        }

        $('#myModalcargando').modal();

        $('#guardarReserva').prop("disabled", true);
        var des = $('#idObservacion').val();
        if ($('#idObservacion').val() == "") {
            des = "N/A"
        }
        var URL = '/Reservacion/editarR';
        var idusuario_viejo = '@Model.ID_PERSONA';
        var id_viejo = '@Model.ID_RESERVACION';
        var nom_viejo = '@Html.Raw(HttpUtility.JavaScriptStringEncode(@Model.NOM_ACTIVIDAD))';
        var tel_viejo = '@Html.Raw(HttpUtility.JavaScriptStringEncode(@Model.TXT_TELEFONO))';
        var idInmueble_viejo = '@Model.ID_INMUEBLE';
        var fi_viejo = '@Model.FEC_INICIALRESERVACION';
        var ff_viejo = '@Model.FEC_FINALRESERVACION';
        var hi_viejo = '@Model.HOR_INICIO';
        var hf_viejo = '@Model.HOR_FINAL';
        var idTipoActividad_Viejo = '@Model.ID_TIPOACTIVIDAD';
        var idObjetivo_viejo = '@Html.Raw(HttpUtility.JavaScriptStringEncode(@Model.OBJETIVO))';
        var idDescripcion_viejo = '@Html.Raw(HttpUtility.JavaScriptStringEncode(@Model.DESCRIPCION))'
        var mobiliario = '@Model.DSRITMOBILIARIORESERVACION'


        var nombre = $('#idNombre').val();
        var telef = $('#idTelefono').val();
        var horai = $('#HoraInicio').val();
        var horaf = $('#HoraFin').val();
        var tipoactividad = $('select#ID_TIPOACTIVIDAD').val();
        var fechai = $('#idFechaInicio').val();
        var fechaf = $('#idFechaFin').val();

        var organizador = $('#idOrganizador').val();
        var objetivo = $('#idObjetivo').val();
        var cantAforo = $('#idcantidad').val();
        var cantAforodia = $('#idcantidaddia').val();

        var descripcion = $('#idDescripcion').val();


        var _ReservaNuevaVieja = {
            idusuario_viejo: idusuario_viejo,
            id_viejo: id_viejo,
            nom_viejo: nom_viejo,
            tel_viejo: tel_viejo,
            idInmueble_viejo: idInmueble_viejo,
            fi_viejo: fi_viejo,
            ff_viejo: ff_viejo,
            hi_viejo: hi_viejo,
            hf_viejo: hf_viejo,
            idTipoActividad_Viejo: idTipoActividad_Viejo,
            idObjetivo_viejo: idObjetivo_viejo,
            idDescripcion_viejo: idDescripcion_viejo,
            ListaDiasViejos: listaDiasViejo,
            // Datos de la reserva nueva
            nombre: nombre,
            telef: telef,
            horai: horai,
            horaf: horaf,
            tipoactividad: tipoactividad,
            des: des,
            fechai: fechai,
            fechaf: fechaf,
            organizador: organizador,
            objetivo: objetivo,
            cantAforo: cantAforo,
            cantAforodia: cantAforodia,
            descripcion: descripcion,
            listaDias: listaDias,
            listaInmuebles: lista_de_inmuebles
        };
        $.ajax({
            type: "POST",
            url: URL,
            contentType: "application/json;  charset=UTF-8",
            data: JSON.stringify(_ReservaNuevaVieja),
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });
        function successFunc() {
            $('#guardarReserva').prop("disabled", false);
            $("#myModal").modal();
            $('#myModalcargando').modal('hide');
        }
        function errorFunc() {
            $('#myModalError').modal();
            $('#myModalcargando').modal('hide');
            $('#guardarReserva').prop("disabled", false);
        }

    });

    $('#HoraInicio').timepicker({
        timeFormat: "hh:mm tt",
        hourMin: 7,
        hourMax: 20,
        stepMinute: 30,
    });
    $('#HoraFin').timepicker({
        timeFormat: "hh:mm tt",
        hourMin: 8,
        hourMax: 21,
        stepMinute: 30
    });

    $('#HoraInicio').change(function () {
        $('#HoraInicio').timepicker({
            timeFormat: "hh:mm tt",
            hourMin: 7,
            hourMax: 20,
            stepMinute: 30,
        });
    });
    $('#HoraFin').change(function () {
        $('#HoraFin').timepicker({
            timeFormat: "hh:mm tt",
            hourMin: 8,
            hourMax: 21,
            stepMinute: 30
        });
    }
   );
    var hoy = new Date.today();
    var value = '@Model.FEC_INICIALRESERVACION';
    var fecha = new Date.parseExact(value, "dd/MM/yyyy");


    $("#idFechaInicio").change(function () {


        var value = $("#idFechaInicio").val();
        var valueF = $("#idFechaFin").val();
        console.log(value);
        getHoras();
        var fecha = new Date.parseExact(value, "dd/MM/yyyy");
        var dia_semana = fecha.getDay();

        console.log(fecha);
        $('#idFechaFin').datepicker('option',
                            {
                                minDate: fecha,
                            });
    });

    $("#idFechaFin").change(function () {

        var value = $("#idFechaFin").val();
        console.log(value);
        getHoras();
        var fecha = new Date.parseExact(value, "dd/MM/yyyy");
        console.log(fecha);

        var dia_semanaFinal = fecha.getDay();
        //fecha inicio
        var valueInicio = $("#idFechaInicio").val();
        var fechaInicio = new Date.parseExact(valueInicio, "dd/MM/yyyy");
        var dia_semanaInicio = fechaInicio.getDay();


        $('#idFechaInicio').datepicker('option',
                            {
                                minDate: 0,
                            });


    });
    //$("#idFechaInicio").datepicker({
    //    minDate: 0,
    //    dateFormat: "dd/mm/yy",
    //});
    //$("#idFechaFin").datepicker({
    //    minDate: 0,
    //    dateFormat: 'dd/mm/yy'
    //});

    $('#HoraFin').change(function () {
        var _hora = $("#HoraFin").val();
        getHoras();
        var res = getHoraInicio(_hora);
    });
    $('#HoraInicio').change(function () {
        var _hora = $("#HoraInicio").val();
        getHoras();
        var res = getHoraInicio(_hora);
    });
    function getHoraInicio(start_time) {
        var _today = Date.today();
        var fecha = $("#idFechaInicio").val();
        var timeString = fecha + ' ' + start_time;
        var time = Date.parseExact(start_time, "hh:mm tt");
        var hora = time.getHours();
        var minutes = time.getMinutes();
        var seconds = time.getSeconds();
        var horaFijada = "";
        if (hora <= 9) {
            if (minutes <= 9) {
                horaFijada = '0' + hora + ':0' + minutes + ':0' + seconds;
            }
            else
                horaFijada = '0' + hora + ':' + minutes + ':0' + seconds;
        }
        else if (hora > 9) {
            if (minutes < 9) {
                horaFijada = hora + ':0' + minutes + ':0' + seconds;
            }
            else
                horaFijada = hora + ':0' + minutes + ':0' + seconds;
        }
        return horaFijada;
    }
    function getHoras() {
        $('#loadInmaux').hide();
        var strMessage = "ERROR DE USUARIO"
        //Hora de Inicio       
        var start_time = $("#HoraInicio").val();
        //Hora Final
        var end_time = $("#HoraFin").val();

        var stt = new Date("November 13, 2013 " + start_time);

        var hora = new Date().toString('hh:mm tt');
        var TiempoActual = new Date("November 13, 2013 " + hora);

        var _today = Date.today();

        var fecha = $("#idFechaInicio").val();
        var fechaTemp = Date.parseExact(fecha, "dd/MM/yyyy");
        var timeString = fecha + ' ' + start_time;
        console.log(timeString);
        var time = Date.parseExact(fecha, "dd/MM/yyyy");


        var fecha3 = $("#idFechaFin").val();
        var fechaTemp3 = Date.parseExact(fecha3, "dd/MM/yyyy");
        var timeString3 = fecha3 + ' ' + end_time;
        console.log(timeString3);


        var time3 = Date.parseExact(fecha3, "dd/MM/yyyy");


        /*-------------*/
        stt = stt.getTime();
        var endt = new Date("November 13, 2013 " + end_time);
        endt = endt.getTime();



        if (fecha == "") {
            document.getElementById("idFechaFin").style.borderColor = "#CCCCCC";
            document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
            document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
            document.getElementById("idFechaInicio").style.borderColor = "#FF0000";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#guardarReserva').hide();
            $('#errorSpan').show();
        }
        else if (fecha3 == "") {
            document.getElementById("idFechaFin").style.borderColor = "#FF0000";
            document.getElementById("idFechaInicio").style.borderColor = "#CCCCCC";
            document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
            document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#guardarReserva').hide();
        }
        else if (start_time == "") {
            document.getElementById("HoraInicio").style.borderColor = "#FF0000";
            document.getElementById("idFechaInicio").style.borderColor = "#CCCCCC";
            document.getElementById("idFechaFin").style.borderColor = "#CCCCCC";
            document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#guardarReserva').hide();
        }
        else if (end_time == "") {

            document.getElementById("HoraFin").style.borderColor = "#FF0000";
            document.getElementById("idFechaInicio").style.borderColor = "#CCCCCC";
            document.getElementById("idFechaFin").style.borderColor = "#CCCCCC";
            document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#guardarReserva').hide();
        }
        else {
            document.getElementById("idFechaInicio").style.borderColor = "#CCCCCC";
            document.getElementById("idFechaFin").style.borderColor = "#CCCCCC";
            document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
            document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
            console.log(_today);
            console.log(time);

            var fecha_hoy = _today.getDate();
            var fecha_inicio = time.getDate();
            var fecha_fin = time3.getDate();
            TiempoActual = TiempoActual.getTime();

            if ((fecha_hoy == fecha_inicio) && (fecha_hoy == fecha_fin) && _today.getMonth() == time.getMonth() && _today.getFullYear() == time.getFullYear()) {
                console.log(fecha_hoy);
                console.log(fecha_inicio);
                console.log(fecha_fin);
                if (TiempoActual > stt) {
                    document.getElementById("HoraInicio").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora ACTUAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#guardarReserva').hide();
                }
                else if (stt >= endt) {
                    document.getElementById("HoraInicio").style.borderColor = "#FF0000";
                    document.getElementById("HoraFin").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora FINAL.";
                    $("#errorSpan").text(strMessage);
                    $('#guardarReserva').hide();
                    $('#errorSpan').show();
                }
                else {
                    document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
                    document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
                    $('#errorSpan').hide();
                    $('#guardarReserva').show();
                    $('#loadInmaux').show();
                }
            }

                // La fecha de hoy con la fecha de inicio iguales y la fecha final direfentes
            else if ((fecha_hoy == fecha_inicio) && (fecha_hoy != fecha_fin) && _today.getMonth() == time3.getMonth() && time3.getFullYear() == _today.getFullYear()) {
                if (TiempoActual > stt) {
                    document.getElementById("HoraInicio").style.borderColor = "#FF0000";
                    strMessage = "Para reservar el día de hoy la hora INICIAL debe ser MAYOR a la hora ACTUAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#guardarReserva').hide();
                }

                else if (stt >= endt) {
                    document.getElementById("HoraInicio").style.borderColor = "#FF0000";
                    document.getElementById("HoraFin").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora FINAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#guardarReserva').hide();
                }
                else {
                    document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
                    document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
                    $('#errorSpan').hide();
                    $('#loadInmaux').show();
                    $('#guardarReserva').hide();
                }
            }
            else if (fecha_hoy != fecha_inicio) {
                if (stt >= endt) {
                    document.getElementById("HoraInicio").style.borderColor = "#FF0000";
                    document.getElementById("HoraFin").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora FINAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#guardarReserva').hide();
                }
                else {
                    document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
                    document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
                    $('#errorSpan').hide();
                    $('#loadInmaux').show();
                    $('#guardarReserva').show();
                }
            }
            else {
                document.getElementById("HoraInicio").style.borderColor = "#CCCCCC";
                document.getElementById("HoraFin").style.borderColor = "#CCCCCC";
                $('#errorSpan').hide();
                $('#loadInmauxaux').show();
                $('#guardarReserva').show();
            }

        }
    }
    //Para la parte del cambio de los dias que se hace a la hora de editar la reserva
    $('#lun').change(function () {
        if ($("#lun").is(':checked')) {
            listaDias.push(1);
        } else {
            var index = listaDias.indexOf(1);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
        //$("#resumen").text(resu);
    });
    $('#mar').change(function () {
        if ($("#mar").is(':checked')) {
            listaDias.push(2);
        } else {
            var index = listaDias.indexOf(2);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });
    $('#mier').change(function () {
        if ($("#mier").is(':checked')) {
            listaDias.push(3);
        }
        else {
            var index = listaDias.indexOf(3);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });
    $('#jue').change(function () {
        if ($("#jue").is(':checked')) {
            listaDias.push(4);
        } else {
            var index = listaDias.indexOf(4);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });
    $('#vier').change(function () {
        if ($("#vier").is(':checked')) {
            listaDias.push(5);
        } else {
            var index = listaDias.indexOf(5);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });
    $('#sab').change(function () {
        if ($("#sab").is(':checked')) {
            listaDias.push(6);
        } else {
            var index = listaDias.indexOf(6);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });

    $('#dom').change(function () {
        if ($("#dom").is(':checked')) {
            listaDias.push(0);
        } else {
            var index = listaDias.indexOf(0);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });


});
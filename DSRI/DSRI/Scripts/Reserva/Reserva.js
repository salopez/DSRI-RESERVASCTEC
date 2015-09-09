// Traducción al español
$(function ($) {
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '<Ant',
        nextText: 'Sig>',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);
});

$(document).ready(function () {
    $('#loadInmaux').hide();
    $('#loader').hide();
    $('#error').css('visibility', 'hidden');
    var min = 0;
    function getTime() {
        var _horat = new Date();
        var _horaMin = _horat.getHours();
        var FechaSeleccionada = $("#FEC_INICIALRESERVACION").val();
        var actual = Date.today();
        var seleccionada = Date.parse(FechaSeleccionada);
        var daysA = actual.getDate();
        var daysB = seleccionada.getDate();
        if (daysA == daysB) {
            min = _horaMin;
        }
        else {
            min = 7;
        }
        console.log(min);
    }
    $('#NOM_ACTIVIDAD').change(function () {
        var valor = $('#NOM_ACTIVIDAD').val();
        if (valor != "") {
            $('#next').prop('disabled', false);
        }
        else
            $('#next').prop('disabled', true);
    });
    $('#HOR_INICIO').timepicker({
        timeFormat: "hh:mm tt",
        hourMin: 7,
        hourMax: 20,
        stepMinute: 30,
    });
    $('#HOR_FINAL').timepicker({
        timeFormat: "hh:mm tt",
        hourMin: 8,
        hourMax: 21,
        stepMinute: 30
    });

    //$(".chosen-select").chosen({

    //    disable_search_threshold: 10

    //}).change(function (event) {

    //    if (event.target == this) {
    //        alert($(this).val());
    //    }

    //});

    $("#FEC_INICIALRESERVACION").change(function () {
        $('#errorNohay').hide();
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');

        var value = $("#FEC_INICIALRESERVACION").val();
        var valueF = $("#FEC_FINALRESERVACION").val();
        console.log(value);
        getHoras();
        var fecha = new Date.parseExact(value, "dd/MM/yyyy");
        var dia_semana = fecha.getDay();
        if ($("#FEC_FINALRESERVACION").val() != "") {
            getDiasSemanales(value, valueF);
        }
        else {
            getDiasSemanales(value, value);
        }
        _ReservaMob.FEC_INICIALRESERVACION = value;
        console.log(fecha);
        $('#FEC_FINALRESERVACION').datepicker('option',
                            {
                                minDate: fecha,
                            });
        $("#termina").datepicker('option',
                                {
                                    minDate: fecha,
                                });
        $('#empieza').val($("#FEC_INICIALRESERVACION").val());
    });

    function sumar(dia) {
        if (dia == 6) {
            return 0;
        }
        dia = dia + 1;
        return dia;
    }
    var listaDias = [];
    function getDiasSemanales(start_date, end_date) {
        $("#dom").bootstrapToggle('off');
        $("#lun").bootstrapToggle('off');
        $("#mar").bootstrapToggle('off');
        $("#mier").bootstrapToggle('off');
        $("#jue").bootstrapToggle('off');
        $("#vier").bootstrapToggle('off');
        $("#sab").bootstrapToggle('off');
        while (listaDias.length > 0) {
            listaDias.pop();
        }
        var FI = new Date.parseExact(start_date, "dd/MM/yyyy");
        var FF = new Date.parseExact(end_date, "dd/MM/yyyy");
        var ano = FI.getYear();
        var mes = FI.getMonth();
        var diames = FI.getDate();
        var dia = FI.getDay();
        while (FI <= FF) {
            if (dia == 0) {
                if ($("#dom").is(':checked')) {
                } else { $("#dom").bootstrapToggle('on'); }
            }
            if (dia == 1) {
                if ($("#lun").is(':checked')) {
                } else { $("#lun").bootstrapToggle('on'); }
            }
            if (dia == 2) {
                if ($("#mar").is(':checked')) {
                } else {
                    $("#mar").bootstrapToggle('on');
                }
            }
            if (dia == 3) {
                if ($("#mier").is(':checked')) {
                } else {
                    $("#mier").bootstrapToggle('on');
                }
            }
            if (dia == 4) {
                if ($("#jue").is(':checked')) {
                } else {
                    $("#jue").bootstrapToggle('on');
                }
            }
            if (dia == 5) {
                if ($("#vier").is(':checked')) {
                } else {
                    $("#vier").bootstrapToggle('on');
                }
            }
            if (dia == 6) {
                if ($("#sab").is(':checked')) {
                } else {
                    $("#sab").bootstrapToggle('on');
                }
            }
            FI.setDate(FI.getDate() + 1);
            dia = sumar(dia);
        }
    }

    $("#FEC_FINALRESERVACION").change(function () {
        $('#errorNohay').hide();
        //$('#ID_INMUEBLE').css('visibility', 'hidden');
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");

        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
        var value = $("#FEC_FINALRESERVACION").val();
        console.log(value);
        getHoras();
        var fecha = new Date.parseExact(value, "dd/MM/yyyy");
        _ReservaMob.FEC_FINALRESERVACION = value;
        console.log(fecha);

        var dia_semanaFinal = fecha.getDay();
        //fecha inicio
        var valueInicio = $("#FEC_INICIALRESERVACION").val();
        var fechaInicio = new Date.parseExact(valueInicio, "dd/MM/yyyy");
        var dia_semanaInicio = fechaInicio.getDay();
        getDiasSemanales(valueInicio, value);


        $('#FEC_INICIALRESERVACION').datepicker('option',
                            {
                                minDate: 0,
                            });

        $('#termina').val($("#FEC_FINALRESERVACION").val());

    });
    $("#FEC_INICIALRESERVACION").datepicker({
        minDate: 0,
        dateFormat: "dd/mm/yy",
    });
    $("#FEC_FINALRESERVACION").datepicker({
        minDate: 0,
        dateFormat: 'dd/mm/yy'
    });

    $('#HOR_FINAL').change(function () {
        $('#errorNohay').hide();
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");

        // $('#ID_INMUEBLE').css('visibility', 'hidden');
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
        var _hora = $("#HOR_FINAL").val();
        getHoras();
        var res = getHoraInicio(_hora);
        _ReservaMob.HOR_FINAL = res;
        console.log(_ReservaMob.HOR_FINAL);
    }
    );
    $('#HOR_INICIO').change(function () {
        $('#errorNohay').hide();
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");

        //$('#ID_INMUEBLE').css('visibility', 'hidden');
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
        var _hora = $("#HOR_INICIO").val();
        getHoras();
        var res = getHoraInicio(_hora);
        _ReservaMob.HOR_INICIO = res;
        console.log(_ReservaMob.HOR_INICIO);
    }
    );
    function getHoraInicio(start_time) {
        var _today = Date.today();
        var fecha = $("#FEC_INICIALRESERVACION").val();
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
        var start_time = $("#HOR_INICIO").val();
        //Hora Final
        var end_time = $("#HOR_FINAL").val();

        var stt = new Date("November 13, 2013 " + start_time);

        var hora = new Date().toString('hh:mm tt');
        var TiempoActual = new Date("November 13, 2013 " + hora);

        var _today = Date.today();

        var fecha = $("#FEC_INICIALRESERVACION").val();
        var fechaTemp = Date.parseExact(fecha, "dd/MM/yyyy");
        var timeString = fecha + ' ' + start_time;
        console.log(timeString);
        var time = Date.parseExact(fecha, "dd/MM/yyyy");


        var fecha3 = $("#FEC_FINALRESERVACION").val();
        var fechaTemp3 = Date.parseExact(fecha3, "dd/MM/yyyy");
        var timeString3 = fecha3 + ' ' + end_time;
        console.log(timeString3);


        var time3 = Date.parseExact(fecha3, "dd/MM/yyyy");


        /*-------------*/
        stt = stt.getTime();
        var endt = new Date("November 13, 2013 " + end_time);
        endt = endt.getTime();



        if (fecha == "") {
            document.getElementById("FEC_FINALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
            document.getElementById("FEC_INICIALRESERVACION").style.borderColor = "#FF0000";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#crearModal').prop('disabled', true);
        }
        else if (fecha3 == "") {
            document.getElementById("FEC_FINALRESERVACION").style.borderColor = "#FF0000";
            document.getElementById("FEC_INICIALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#crearModal').prop('disabled', true);
        }
        else if (start_time == "") {
            document.getElementById("HOR_INICIO").style.borderColor = "#FF0000";
            document.getElementById("FEC_INICIALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("FEC_FINALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#crearModal').prop('disabled', true);
        }
        else if (end_time == "") {

            document.getElementById("HOR_FINAL").style.borderColor = "#FF0000";
            document.getElementById("FEC_INICIALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("FEC_FINALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
            strMessage = "Campo Requerido *";
            $("#errorSpan").text(strMessage);
            $('#errorSpan').show();
            $('#crearModal').prop('disabled', true);
        }
        else {
            document.getElementById("FEC_INICIALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("FEC_FINALRESERVACION").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
            document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
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
                    document.getElementById("HOR_INICIO").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora ACTUAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#crearModal').prop('disabled', true);
                }
                else if (stt >= endt) {
                    document.getElementById("HOR_INICIO").style.borderColor = "#FF0000";
                    document.getElementById("HOR_FINAL").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora FINAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#crearModal').prop('disabled', true);
                }
                else {
                    document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
                    document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
                    $('#errorSpan').hide();
                    $('#loadInmaux').show();
                    $('#crearModal').prop('disabled', false);
                }
            }

                // La fecha de hoy con la fecha de inicio iguales y la fecha final direfentes
            else if ((fecha_hoy == fecha_inicio) && (fecha_hoy != fecha_fin) && _today.getMonth() == time3.getMonth() && time3.getFullYear() == _today.getFullYear()) {
                if (TiempoActual > stt) {
                    document.getElementById("HOR_INICIO").style.borderColor = "#FF0000";
                    strMessage = "Para reservar el día de hoy la hora INICIAL debe ser MAYOR a la hora ACTUAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#crearModal').prop('disabled', true);
                }

                else if (stt >= endt) {
                    document.getElementById("HOR_INICIO").style.borderColor = "#FF0000";
                    document.getElementById("HOR_FINAL").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora FINAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#crearModal').prop('disabled', true);
                }
                else {
                    document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
                    document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
                    $('#errorSpan').hide();
                    $('#loadInmaux').show();
                    $('#crearModal').prop('disabled', false);
                }
            }
            else if (fecha_hoy != fecha_inicio) {
                if (stt >= endt) {
                    document.getElementById("HOR_INICIO").style.borderColor = "#FF0000";
                    document.getElementById("HOR_FINAL").style.borderColor = "#FF0000";
                    strMessage = "La hora INICIAL debe ser MAYOR a la hora FINAL.";
                    $("#errorSpan").text(strMessage);
                    $('#errorSpan').show();
                    $('#crearModal').prop('disabled', true);
                }
                else {
                    document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
                    document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
                    $('#errorSpan').hide();
                    $('#loadInmaux').show();
                    $('#crearModal').prop('disabled', false);
                }
            }
            else {
                document.getElementById("HOR_INICIO").style.borderColor = "#CCCCCC";
                document.getElementById("HOR_FINAL").style.borderColor = "#CCCCCC";
                $('#errorSpan').hide();
                $('#loadInmauxaux').show();
                $('#crearModal').prop('disabled', false);

            }

        }
    }


    var inmuebles = [];
    //});
    /*Objeto que se va enviar al controlador*/
    var _ReservaMob = {
        "ID_RESERVACION": 0,
        "ID_INMUEBLE": 0,
        "NOM_ACTIVIDAD": "",
        "FEC_INICIALRESERVACION": "",
        "FEC_FINALRESERVACION": "",
        "HOR_INICIO": "",
        "HOR_FINAL": "",
        "ID_TIPOACTIVIDAD": 0,
        "ID_PERSONA": "",
        "TXT_ESTADO": "En Proceso",
        "CAN_AFORO": 1,
        "TXT_TELEFONO": "",
        "TXT_EXTRA": "N/A",
        "OBJETIVO": "",
        "CAN_AFORODIA": 1,
        "ORGANIZADOR": "",
        "DESCRIPCION": "",
        DSRITMOBILIARIORESERVACION: [] //Es un arreglo porque contiene la lista de mobiliario
    };
    function getIDComboActividad() {
        $('#tipoActividad').text("");
        var tipoActividad = $('#ID_TIPOACTIVIDAD').find(':selected').text();
        var idTipoActividad = $('#ID_TIPOACTIVIDAD').find(':selected').val();
        _ReservaMob.ID_TIPOACTIVIDAD = idTipoActividad;
        $('#tipoActividad').append("Tipo de Actividad: " + tipoActividad);
    }
    function getIDComboInmueble() {
        //$('#inmueble').text("");
        //var e = document.getElementById("ID_INMUEBLE");
        //var strUser = e.options[e.selectedIndex].text;
        //var inmueble = $('#ID_INMUEBLE').find(':selected').text();
        //var idInmueble = $('#ID_INMUEBLE').find(':selected').val();
        //_ReservaMob.ID_INMUEBLE = idInmueble;
        //$('#inmueble').append("Inmueble: " + inmueble);
    }
    //$(document).ready(function () {
    //Sección que define variables globales para utilizar a la hora de pasar, capturar los datos a la vista.
    $('#errmsgletra').hide();
    $('#errmsgrepetido').hide();
    $('#errmsg').hide();

    var nombre;
    var eliminado;
    var StockMobiliario;
    getIDComboActividad();
    getIDComboInmueble();

    $('#can_personas').append("Cantidad de personas: " + 1);
    $('#lstMobi').css('visibility', 'hidden');
    $('#finish').css('visibility', 'hidden');
    $('#ID_INMUEBLE').css('visibility', 'hidden');
    $('#lInmuebles').css('visibility', 'hidden');
    $('#crearModalaux').css('visibility', 'hidden');

    function addToList(ID_MOBILIARIORESERVACION, ID_RESERVACION, ID_MOBILIARIO, CAN_DISPONIBILIDAD) {
        _ReservaMob.DSRITMOBILIARIORESERVACION.push({
            "ID_MOBILIARIORESERVACION": ID_MOBILIARIORESERVACION,
            "ID_RESERVACION": ID_RESERVACION,
            "ID_MOBILIARIO": ID_MOBILIARIO,
            "CAN_DISPONIBILIDAD": CAN_DISPONIBILIDAD,
        });
    }
    $('#nomActividad').text("");
    var nomActividad = $('#NOM_ACTIVIDAD').val();
    _ReservaMob.NOM_ACTIVIDAD = nomActividad;
    $('#nomActividad').append("Nombre de Actividad: " + nomActividad);

    $('#NOM_ACTIVIDAD').change(function () {
        document.getElementById("NOM_ACTIVIDAD").style.borderColor = "rgb(204, 204, 204)";
        _ReservaMob.ID_PERSONA = $("#ID_PERSONA").val();
        $('#nomActividad').text("");
        var nomActividad = $('#NOM_ACTIVIDAD').val();
        _ReservaMob.NOM_ACTIVIDAD = nomActividad;
        $('#nomActividad').append("Nombre de Actividad: " + nomActividad);
    });
    $('#DESCRIPCION').change(function () {
        document.getElementById("DESCRIPCION").style.borderColor = "rgb(204, 204, 204)";
        var descripcion = $('#DESCRIPCION').val();
        _ReservaMob.DESCRIPCION = descripcion;
    });
    $('#CAN_AFORO').change(function () {
        _ReservaMob.ID_PERSONA = $('#ID_USUARIO').val();
        $('#can_personas').text("");
        var cantidad_aforo = $('#CAN_AFORO').val();
        console.log(cantidad_aforo);
        _ReservaMob.CAN_AFORO = cantidad_aforo;
        $('#can_personas').append("Cantidad de personas: " + cantidad_aforo);
    });
    $('#CAN_AFORODIA').change(function () {
        _ReservaMob.ID_PERSONA = $('#ID_USUARIO').val();
        var cantidad_aforodia = $('#CAN_AFORODIA').val();
        _ReservaMob.CAN_AFORODIA = cantidad_aforodia;
    });
    $('#TXT_TELEFONO').change(function () {
        document.getElementById("TXT_TELEFONO").style.borderColor = "rgb(204, 204, 204)";
        $('#telefono').text("");
        var num_telefono = $('#TXT_TELEFONO').val();
        _ReservaMob.TXT_TELEFONO = num_telefono;
        $('#telefono').append("Teléfono de contacto: " + num_telefono);
    });
    $('#ID_TIPOACTIVIDAD').change(function () {
        $('#tipoActividad').text("");
        var tipoActividad = $('#ID_TIPOACTIVIDAD').find(':selected').text();
        var idTipoActividad = $('#ID_TIPOACTIVIDAD').find(':selected').val();
        _ReservaMob.ID_TIPOACTIVIDAD = idTipoActividad;
        $('#tipoActividad').append("Tipo de Actividad: " + tipoActividad);
    });
    $('#TXT_EXTRA').change(function () {
        $('#nomActividad').text("");
        var extra = $('#TXT_EXTRA').val();
        _ReservaMob.TXT_EXTRA = extra;
        //$('#nomActividad').append("Nombre de Actividad: " + nomActividad);
    });
    $('#ORGANIZADOR').change(function () {
        document.getElementById("ORGANIZADOR").style.borderColor = "rgb(204, 204, 204)";
        var organiza = $('#ORGANIZADOR').val();
        _ReservaMob.ORGANIZADOR = organiza;
    });
    $('#OBJETIVO').change(function () {
        document.getElementById("OBJETIVO").style.borderColor = "rgb(204, 204, 204)";
        var objetivo = $('#OBJETIVO').val();
        _ReservaMob.OBJETIVO = objetivo;
    });
    $('#ID_INMUEBLE').change(function () {
        $('#inmueble').text("");
        var inmueble = $('#ID_INMUEBLE').find(':selected').text();
        var idInmueble = $('#ID_INMUEBLE').find(':selected').val();
        _ReservaMob.ID_INMUEBLE = idInmueble;
        $('#inmueble').append("Inmueble: " + inmueble);
    });

    function deleteElement(element) {
        var array = _ReservaMob.DSRITMOBILIARIORESERVACION;
        var removed;
        for (var i = 0; i < array.length; i++) {
            if (element == array[i].ID_MOBILIARIO) {
                removed = array.splice(i, 1);
            }
        }
        array = removed;
        $('#lstMob option[value= ' + element + ']').remove();
        $('#lstMobi option[value= ' + element + ']').remove();
    }

    function findElement(element) {
        var array = _ReservaMob.DSRITMOBILIARIORESERVACION;
        var res = false;
        for (var i = 0; i < array.length; i++) {
            if (element == array[i].ID_MOBILIARIO) {
                //$("errmsgrepetido").show();
                //alert("No se puede ingresar el mismo mobiliario dos veces, verifique y vuelva a intentar");
                res = true;
                break;
            }
            else
                res = false;
        }
        return res;
    }
    $('#delete').click(function () {
        deleteElement(eliminado);
    });
    $('#lstMob').click(function () {
        eliminado = $('#lstMob').find(':selected').val();
    });
    //called when key is pressed in textbox
    $("#cantidad").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $("#errmsgletra").show();
            //$("#errmsg").html("Sólo se pueden digitar números").show().fadeOut("slow");
            return false;
        }
    });
    $('#add').click(function (e) {
        $('#errmsgletra').hide();
        $('#errmsgrepetido').hide();
        $('#errmsg').hide();
        e.preventDefault();

        //nombre = $('#nombre').val();
        //_ReservaMob.nombre = nombre;
        var value = $('#ListaMobiliario').find(':selected').val();
        var text = $('#ListaMobiliario').find(':selected').text();
        var cantidad = $('#cantidad').val();
        //Verifica si el elemento no se ha ingresado
        if (!(findElement(value))) {
            StockMobiliario = GetCantidadMobiliario(value);
            var _Stock = StockMobiliario.cantidad;
            if (cantidad > _Stock) {
                $("#errmsg").show();
                //alert("No se puede agregar cantidades mayores al Stock");
            }
            else {
                addToList(0, 0, value, cantidad);
                $('#lstMob').append(new Option(cantidad + " , " + text, value));
                $('#lstMobi').append(new Option(cantidad + " , " + text, value));
                $('#lstMobi').prop("disabled", true);
                $('#lstMobi').css('visibility', 'visible');
                $('#cantidad').val('1');
                $("select#ListaMobiliario").prop('selectedIndex', 0);
            }
        }
        else {
            $("errmsgrepetido").show();
        }
    });

    $('#crearModal').click(function (e) {
        if (_ReservaMob.NOM_ACTIVIDAD == "") {
            document.getElementById("ORGANIZADOR").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("OBJETIVO").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("NOM_ACTIVIDAD").style.borderColor = "#FF0000";
            $('#errorFaltandatos').show();
        }
        else if (_ReservaMob.OBJETIVO == "") {
            document.getElementById("NOM_ACTIVIDAD").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("ORGANIZADOR").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("OBJETIVO").style.borderColor = "#FF0000";
            $('#errorFaltandatos').show();
        }
        else if (_ReservaMob.DESCRIPCION == "") {
            document.getElementById("NOM_ACTIVIDAD").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("ORGANIZADOR").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("DESCRIPCION").style.borderColor = "#FF0000";
            $('#errorFaltandatos').show();
        }

        else if (_ReservaMob.TXT_TELEFONO == "") {
            document.getElementById("NOM_ACTIVIDAD").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("TXT_TELEFONO").style.borderColor = "#FF0000";
            $('#errorFaltandatos').show();
        }
        else if (_ReservaMob.ORGANIZADOR == "") {
            document.getElementById("NOM_ACTIVIDAD").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("OBJETIVO").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("ORGANIZADOR").style.borderColor = "#FF0000";
            $('#errorFaltandatos').show();
        }
        else {
            document.getElementById("OBJETIVO").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("ORGANIZADOR").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("TXT_TELEFONO").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("NOM_ACTIVIDAD").style.borderColor = "#rgb(204, 204, 204)";
            document.getElementById("DESCRIPCION").style.borderColor = "#rgb(204, 204, 204)";
            $('#errorFaltandatos').hide();
            $("#infoNombre").text(_ReservaMob.NOM_ACTIVIDAD);
            $("#infoInmueble").text($(".chosen-select").find(':selected').text());
            $("#infoTipo").text($('#ID_TIPOACTIVIDAD').find(':selected').text());
            $("#infoCantidad").text(_ReservaMob.CAN_AFORO);
            $("#infoCantidaddia").text(_ReservaMob.CAN_AFORODIA);
            $("#infoTelefono").text(_ReservaMob.TXT_TELEFONO);
            $("#infoFechaInicio").text(_ReservaMob.FEC_INICIALRESERVACION);
            $("#infoFechaFin").text(_ReservaMob.FEC_FINALRESERVACION);
            $("#infoHoraInicio").text(_ReservaMob.HOR_INICIO);
            $("#infoHoraFin").text(_ReservaMob.HOR_FINAL);
            $("#infoExtra").text(_ReservaMob.TXT_EXTRA);
            $("#myModalInformacion").modal();
        }
    });


    $("#infoInmuebles").popover({
        html: true,
        placement: 'bottom',
        trigger: 'hover',
        title: '<strong style="color:rgb(60, 141, 188);">Reservar Bromelias</strong>',
        content: '<div style="font-size:12px;">' +
                   '<div> <strong style="color:rgb(16, 14, 48);">*</strong>Preste atención a la hora de seleccionar los Salones Bromelias. Si usted seleccionó Bromelia Completo el sistema no permitirá seleccionar Bromelia 1 ó Bromelia 2. Para hacerlo debe eliminar Bromelia Completo de su selección y luego proceder a seleccionar alguno de los 2 Bromelias individuales.</div>'
    });

    function deleteInmueble(values) {
        var listaInmuebles = $(".chosen-select").chosen().val();
        if (listaInmuebles != null) {
            if (listaInmuebles.indexOf('1011') > -1 && (listaInmuebles.indexOf('1006') > -1 || listaInmuebles.indexOf('1007') > -1)) {
                $('.chosen-select').find('option[value=' + 1011 + ']').removeAttr("selected");
                $('.chosen-select').chosen().trigger("chosen:updated");
            }
            else if (listaInmuebles.indexOf('1006') > -1 && listaInmuebles.indexOf('1007') > -1) {
                $('.chosen-select').find('option[value=' + 1006 + ']').removeAttr("selected");
                $('.chosen-select').find('option[value=' + 1007 + ']').removeAttr("selected");
                $('.chosen-select').find('option[value=' + 1011 + ']').attr("selected", "selected");
                $('.chosen-select').chosen().trigger("chosen:updated");
            }
            else if (listaInmuebles.indexOf('1011') > -1 && (listaInmuebles.indexOf('1006') > -1 || listaInmuebles.indexOf('1007') > -1)) {
                $('.chosen-select').find('option[value=' + 1011 + ']').removeAttr("selected");
                $('.chosen-select').chosen().trigger("chosen:updated");
            }
        }
    }

    $('.chosen-select').on('change', function (evt, params) {
        deleteInmueble($(".chosen-select").chosen().val()); // quitar bromelias para evitar choques
    });

    $('#crear').click(function (e) {
        var values = $(".chosen-select").chosen().val();
        var _fechaI = $('#FEC_INICIALRESERVACION').val();// Capturar la fecha de inicio 
        var _fechaF = $('#FEC_FINALRESERVACION').val();
        var info = {
            _Reserva: _ReservaMob, _Ldias: listaDias, _fechaI: _fechaI, _fechaF: _fechaF, _Listainmuebles: values
        };
        var _data = JSON.stringify(info);
        var _url = 'Create';
        var error = "";
        var succes = "";

        $.ajax({
            url: _url,
            async: true,
            type: "POST",
            data: _data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () { // success callback
                // Exito de toda la reserva
                $("#myModal").modal();
            },
            error: function () { // error callback
                //alert(ValidityState);
                $("#myModalError").modal();
            }
        });

    });

    //Obtiene los inmuebles disponibles dada una fecha y hora ingresadas
    //param _fecha: DateTime
    //param _hora:TimeSpan
    function GetInmuebles(_fechaI, _fechaF, _horaI, _horaF) {// Se modificaron algunos parametros extras
        //$('#loader').show();
        var _url = 'GetInmuebles';
        $('#ID_INMUEBLE').empty();
        var data = { _fechaI: _fechaI, _fechaF: _fechaF, _horaI: _horaI, _horaF: _horaF, _lDias: listaDias };
        console.log(data);
        $.ajax({
            type: 'POST',
            url: _url,
            data: data,
            dataType: 'json',
            async: false,
            success: function (inmuebles) {
                var list = $('select#ID_INMUEBLE');
                list.find('option').remove();
                $('.chosen-select').empty();
                $('.chosen-select').prop('disabled', false);
                //$('.chosen-container').show();
                // $('#ID_INMUEBLE').css('visibility', 'visible');
                $('#lInmuebles').css('visibility', 'visible');
                $(inmuebles).each(function (index, value) {
                    $('.chosen-select').append("<option value='" + value.id + "'>" + value.name + "</option>");
                    $('.chosen-select').chosen().trigger("chosen:updated");
                    list.append('<option value="' + value.id + '">' + value.name + '</option>');
                });
                // $('.chosen-container').show();
                if (inmuebles.length == 0) {
                    $('#errorNohay').show();
                    $('#crearModalaux').hide();
                    $('#todalalista').hide();
                }
                else {
                    $('#crearModalaux').css('visibility', 'visible');
                    $('#crearModalaux').show();
                    $('#todalalista').show();
                    $('#errorNohay').hide();
                }
            },
            error: function () {
                //console.log(_fecha);

                $('#myModalError').show();
                // alert("Error al intentar conectar con el Servidor!");
            }
        });
    }
    $('#loadInm').on("click", function () {
        $("#errorSpan").hide();
        if (listaDias.length <= 0) {
            $("#errorSpan").text("Debe seleccionar al menos un día de la semana para proceder con la reserva");
            $("#errorSpan").show();
            return;
        }
        $('#inform').hide();
        var _horaI = $('#HOR_INICIO').val();
        var _horaF = $('#HOR_FINAL').val();
        //var _fecha = $('#FEC_FINALRESERVACION').val();
        var _fechaI = $('#FEC_INICIALRESERVACION').val();// Capturar la fecha de inicio 
        var _fechaF = $('#FEC_FINALRESERVACION').val();
        GetInmuebles(_fechaI, _fechaF, _horaI, _horaF);

        var idInmueble = $('#ID_INMUEBLE').find(':selected').val();
        $('#inmueble').text("");
        var inmueble = $('#ID_INMUEBLE').find(':selected').text();
        var idInmueble = $('#ID_INMUEBLE').find(':selected').val();
        _ReservaMob.ID_INMUEBLE = idInmueble;
        $('#inmueble').append("Inmueble: " + inmueble);
        // $('#loader').hide();
    });

    //Variable que almacena las cantidades de los mobiliarios a insertar
    var res = {
        "id": 0,
        "cantidad": 0
    };

    //Obtiene los mobiliarios dado un id
    function GetCantidadMobiliario(id) {
        var data = { id: id };
        $variable = new Array();

        var _url = 'GetCantidadMobiliario';
        $.ajax({
            type: 'POST',
            url: _url, // url
            data: data,
            dataType: 'json',
            async: false,
            success: function (response) { // success callback
                res = response;
            },
            error: function () {
                alert('Error');
            }
        });
        return res;
    }

    $("#empieza").datepicker({
        minDate: 0,
        dateFormat: "dd/mm/yy",
    });
    $("#termina").datepicker({
        minDate: 0,
        dateFormat: 'dd/mm/yy'
    });
    var val = $("#FEC_INICIALRESERVACION").val();
    var minimo = new Date.parseExact(val, "dd/MM/yyyy");
    $("#termina").change(function () {
        $("#termina").datepicker('option',
                                {
                                    minDate: minimo,
                                });
    });

    $('#lun').change(function () {
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
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
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
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
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
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
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
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
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
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
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
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
        $(".chosen-select").val('').trigger("lista_chosen:updated");
        $('.chosen-select').prop('disabled', true);
        $('.chosen-select').chosen().trigger("chosen:updated");
        $('#crearModalaux').css('visibility', 'hidden');
        $('#lInmuebles').css('visibility', 'hidden');
        if ($("#dom").is(':checked')) {
            listaDias.push(0);
        } else {
            var index = listaDias.indexOf(0);
            if (index > -1) {
                listaDias.splice(index, 1);
            }
        }
    });

    $('#ID_INMUEBLE').change(function () {
        //alert($('#ID_INMUEBLE').val());
    });
});
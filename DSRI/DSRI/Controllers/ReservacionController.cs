using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datos;
using System.Net.Mail;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System.Web.Services;
using System.Json;
using System.Web.Script.Services;
using DSRI.wsSeguridad;
using System.Data.Entity.Validation;
namespace DSRI.Controllers
{
    public class ReservacionController : Controller
    {

        private DSRIEntities db = new DSRIEntities();

        // GET: /Reservacion/
        public ActionResult Index(string estado)
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Session["funcion"] = "Index";
                Session["estado"] = estado;
                var dsrifreservacion = from reservas in db.DSRIFRESERVACION.Include(d => d.DSRIFINMUEBLE).Include(d => d.DSRIFTIPOSACTIVIDAD)
                                       where (reservas.TXT_ESTADO == estado)
                                       select reservas;
                var _query1 = dsrifreservacion.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Year);
                var _query2 = _query1.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Month);
                var _query3 = _query2.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Date);
                return View(_query3.ToList());
            }
        }
        [WebMethod]
        public JsonResult buscarReserva(int id)
        {
            var _res = Json(new[] {
            new { title="", start="", end="",horaI="",minI="",horaF="",minF="",estado="",inmueble="",tipo="",cantidad="",telefono=""},
                }, JsonRequestBehavior.AllowGet);

            var dsrifreservacion = from reserva in db.DSRIFRESERVACION
                                   where reserva.ID_RESERVACION == id
                                   select new
                                   {
                                       title = reserva.NOM_ACTIVIDAD,
                                       start = reserva.FEC_INICIALRESERVACION,
                                       end = reserva.FEC_FINALRESERVACION,
                                       horaI = reserva.HOR_INICIO.ToString(),
                                       minI = reserva.HOR_INICIO.Minutes.ToString(),
                                       horaF = reserva.HOR_FINAL.ToString(),
                                       minF = reserva.HOR_FINAL.Minutes.ToString(),
                                       estado = reserva.TXT_ESTADO,
                                       inmueble = reserva.DSRIFINMUEBLE.NOM_INMUEBLE,
                                       tipo = reserva.DSRIFTIPOSACTIVIDAD.NOM_TIPOACTIVIDAD,
                                       cantidad = reserva.CAN_AFORO,
                                       telefono = reserva.TXT_TELEFONO
                                   };
            var lista_principal = dsrifreservacion.ToList();

            var resTemp = lista_principal;
            _res.Data = resTemp;
            return _res;
        }

        [WebMethod]
        public JsonResult ReservasMes(DateTime _FechaMesBuscar)
        {
            var _res = Json(new[] {
            new { id="", title="", start="",end="",horaI="",minI="",horaF="",minF="",estado="",inmueble=""},
                }, JsonRequestBehavior.AllowGet);

            var dsrifreservacion = from reserva in db.DSRIFRESERVACION
                                   where reserva.FEC_INICIALRESERVACION.Month == _FechaMesBuscar.Month &&
                                         reserva.FEC_INICIALRESERVACION.Year == _FechaMesBuscar.Year
                                   select new
                                   {
                                       id = reserva.ID_RESERVACION,
                                       title = reserva.NOM_ACTIVIDAD,
                                       start = reserva.FEC_INICIALRESERVACION,
                                       end = reserva.FEC_FINALRESERVACION,
                                       horaI = reserva.HOR_INICIO.Hours.ToString(),
                                       minI = reserva.HOR_INICIO.Minutes.ToString(),
                                       horaF = reserva.HOR_FINAL.Hours.ToString(),
                                       minF = reserva.HOR_FINAL.Minutes.ToString(),
                                       estado = reserva.TXT_ESTADO,
                                       inmueble = reserva.DSRIFINMUEBLE.NOM_INMUEBLE

                                   };

            var lista_principal = dsrifreservacion.ToList();



            foreach (var _reserva in dsrifreservacion.ToList())
            {
                var lista_aux = db.reservasUnicas();
                bool eliminar = true;
                foreach (var _reserva_aux in lista_aux)
                {
                    if (_reserva.id == _reserva_aux.ID_RESERVACION && _reserva.start.Month == _FechaMesBuscar.Month
                        && _reserva.start.Year == _FechaMesBuscar.Year)
                    {
                        eliminar = false;
                        break;
                    }

                }
                if (eliminar)
                {
                    for (int i = 0; i < lista_principal.Count; i++)
                    {
                        if (lista_principal.ElementAt(i).id == _reserva.id)
                        {
                            lista_principal.Remove(lista_principal.ElementAt(i));
                            eliminar = true;
                            break;
                        }
                    }
                }
            }


            var resTemp = lista_principal;
            _res.Data = resTemp;
            return _res;
        }

        public ActionResult Calendario()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Session["controller"] = "Reservacion";
                Session["funcion"] = "Calendario";
                var dsrifreservacion = from reservas in db.DSRIFRESERVACION
                                       where reservas.FEC_INICIALRESERVACION.Month == DateTime.Now.Month &&
                                         reservas.FEC_INICIALRESERVACION.Year == DateTime.Now.Year
                                       select reservas;
                var lista_principal = dsrifreservacion.ToList();

                foreach (var _reserva in dsrifreservacion.ToList())
                {
                    var lista_aux = db.reservasUnicas();
                    bool eliminar = true;
                    foreach (var _reserva_aux in lista_aux)
                    {
                        if (_reserva.ID_RESERVACION == _reserva_aux.ID_RESERVACION && _reserva.FEC_INICIALRESERVACION.Month == DateTime.Now.Month
                            && _reserva.FEC_INICIALRESERVACION.Year == DateTime.Now.Year)
                        {
                            eliminar = false;
                            break;
                        }

                    }
                    if (eliminar)
                    {
                        for (int i = 0; i < lista_principal.Count; i++)
                        {
                            if (lista_principal.ElementAt(i).ID_RESERVACION == _reserva.ID_RESERVACION)
                            {
                                lista_principal.Remove(lista_principal.ElementAt(i));
                                eliminar = true;
                                break;
                            }
                        }
                    }
                }
                //return RedirectToAction("Index", "Home");
                return View(lista_principal);
            }
        }


        public int fechaPrincipal(DateTime fi, DateTime ff, int id)
        {
            DSRIFRESERVACION temporal = db.DSRIFRESERVACION.Find(id);
            int resultado = temporal.ID_RESERVACION;
            var all_reservas = from reservas in db.DSRIFRESERVACION
                               where (reservas.FEC_FINALRESERVACION == ff &&
                                      reservas.ID_INMUEBLE == temporal.ID_INMUEBLE &&
                                      reservas.NOM_ACTIVIDAD == temporal.NOM_ACTIVIDAD)
                               select reservas;
            var listaReservas = all_reservas.ToList();
            foreach (var reser in listaReservas)
            {
                if (reser.FEC_INICIALRESERVACION <= fi)
                {
                    fi = reser.FEC_INICIALRESERVACION;
                    resultado = reser.ID_RESERVACION;
                }
            }
            return resultado;
        }

        public bool disponibilidadHora(TimeSpan hi_nueva, TimeSpan hf_nueva, DateTime fi_nueva, DateTime ff_nueva, DateTime fi, int id, List<int> _lDias, List<int> _lInmuebles)
        {
            using (var basededatos = new DSRIEntities())
            {
                using (var dbContextTransaction = basededatos.Database.BeginTransaction())
                {
                    try
                    { // Realizar toda la transaccion de borrar y agregar
                        DSRIFRESERVACION temp = basededatos.DSRIFRESERVACION.Find(id);

                        var all_reservas_relacionadas = from reservas in basededatos.DSRIFRESERVACION
                                                        where (reservas.NOM_ACTIVIDAD == temp.NOM_ACTIVIDAD && reservas.OBJETIVO == temp.OBJETIVO && reservas.ORGANIZADOR == temp.ORGANIZADOR &&
                                                               reservas.HOR_INICIO == temp.HOR_INICIO && reservas.HOR_FINAL == temp.HOR_FINAL && reservas.ID_PERSONA == temp.ID_PERSONA &&
                                                               reservas.ID_TIPOACTIVIDAD == temp.ID_TIPOACTIVIDAD && reservas.TXT_ESTADO == temp.TXT_ESTADO &&
                                                               reservas.TXT_TELEFONO == temp.TXT_TELEFONO && reservas.TXT_EXTRA == temp.TXT_EXTRA && reservas.TXT_ESTADO != "Rechazada")
                                                        select reservas;
                        var lista_reservas_aeliminar = all_reservas_relacionadas.ToList();
                        foreach (var r in lista_reservas_aeliminar)
                        {
                            basededatos.DSRIFRESERVACION.Remove(r);
                            basededatos.SaveChanges();
                        }
                        // VERIFICAR DISPONIBILIDAD DE TODOS LOS INMUEBLES
                        foreach (var i in _lInmuebles)
                        {
                            if (!disponibleInmueble(fi_nueva, ff_nueva, hi_nueva, hf_nueva, _lDias, i, basededatos))
                            {
                                dbContextTransaction.Rollback();
                                return false;
                            }
                        }
                        return true;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }

        }
        public bool pertenece(int dia, List<int> lista)
        {
            foreach (var d in lista)
            {
                if (d == dia)
                {
                    return true;
                }
            }
            return false;
        }

        public bool semanaigual(List<int> s1, List<int> s2)
        {
            s1 = s1.Distinct().ToList(); //dejas dias unicos
            s2 = s2.Distinct().ToList(); //dejar dias unicos
            s1.Sort();// ordenar listas vieja
            s2.Sort();// ordenar listas nueva
            if (s1.Count == s2.Count)
            {
                for (int i = 0; i < s1.Count; i++)
                {
                    if (s1[i] != s2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (s1.Count != s2.Count)
            {
                foreach (var d in s2)
                {
                    if (!pertenece(d, s1))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;

        }

        // Formato Json de los valores que vienen de la reserva nueva y vieja
        [Serializable]
        public class JsonReservaNueva_Vieja
        {
            //Datos de la vieja reserva
            public string idusuario_viejo { get; set; }
            public int id_viejo { get; set; }
            public string nom_viejo { get; set; }
            public string tel_viejo { get; set; }
            public int idInmueble_viejo { get; set; }
            public DateTime fi_viejo { get; set; }
            public DateTime ff_viejo { get; set; }
            public string hi_viejo { get; set; }
            public string hf_viejo { get; set; }
            public int idTipoActividad_Viejo { get; set; }
            public string idObjetivo_viejo { get; set; }
            public string idDescripcion_viejo { get; set; }
            public List<int> ListaDiasViejos { get; set; }
            // Datos de la nueva reserva
            public string nombre { get; set; }
            public string telef { get; set; }
            public string horai { get; set; }
            public string horaf { get; set; }
            public int tipoactividad { get; set; }
            public string des { get; set; }
            public DateTime fechai { get; set; }
            public DateTime fechaf { get; set; }
            public string organizador { get; set; }
            public string objetivo { get; set; }
            public int cantAforo { get; set; }
            public int cantAforodia { get; set; }
            public string descripcion { get; set; }
            public List<int> listaDias { get; set; }
            public List<int> listaInmuebles { get; set; }
        }


        // Función para editar todos los datos de la reserva 
        public ActionResult editarR(JsonReservaNueva_Vieja _ReservaNuevaVieja)
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                using (var basededatos = new DSRIEntities())
                {
                    using (var dbContextTransaction = basededatos.Database.BeginTransaction())
                    {
                        try
                        { // Realizar toda la transaccion de borrar y agregar
                            var _HoraInicioTemp = DateTime.Parse(_ReservaNuevaVieja.hi_viejo, CultureInfo.CreateSpecificCulture("es-ES"));
                            var _HoraFinalTemp = DateTime.Parse(_ReservaNuevaVieja.hf_viejo, CultureInfo.CreateSpecificCulture("es-ES"));
                            /*Convierte las fechas a formato "HH:MM:SS"*/
                            TimeSpan hi_viejo = _HoraInicioTemp.TimeOfDay;
                            TimeSpan hf_viejo = _HoraFinalTemp.TimeOfDay;
                            var _HoraInicioTemp_n = DateTime.Parse(_ReservaNuevaVieja.horai, CultureInfo.CreateSpecificCulture("es-ES"));
                            var _HoraFinalTemp_n = DateTime.Parse(_ReservaNuevaVieja.horaf, CultureInfo.CreateSpecificCulture("es-ES"));
                            /*Convierte las fechas a formato "HH:MM:SS"*/
                            TimeSpan horai = _HoraInicioTemp_n.TimeOfDay;
                            TimeSpan horaf = _HoraFinalTemp_n.TimeOfDay;
                            DSRIFRESERVACION reservacionTemporal = new DSRIFRESERVACION();
                            var _reservaciones = basededatos.DSRIFRESERVACION;
                            var _mobiliarios = basededatos.DSRITMOBILIARIORESERVACION;
                            // se sacan las reservas viejas a editar
                            var _getReservaciones = from reserva in _reservaciones
                                                    where reserva.NOM_ACTIVIDAD == _ReservaNuevaVieja.nom_viejo &&
                                                          reserva.ID_PERSONA == _ReservaNuevaVieja.idusuario_viejo &&
                                                          reserva.TXT_TELEFONO == _ReservaNuevaVieja.tel_viejo &&
                                                          reserva.TXT_ESTADO != "Rechazada" &&
                                                          reserva.OBJETIVO == _ReservaNuevaVieja.idObjetivo_viejo &&
                                                          reserva.DESCRIPCION == _ReservaNuevaVieja.idDescripcion_viejo &&
                                                          reserva.HOR_INICIO == hi_viejo &&
                                                          reserva.HOR_FINAL == hf_viejo
                                                    select reserva;

                            var _getMobiliarios = from mobiliario in _mobiliarios
                                                  where mobiliario.DSRIFRESERVACION.ID_RESERVACION == _ReservaNuevaVieja.id_viejo
                                                  select mobiliario;
                            List<DSRITMOBILIARIORESERVACION> _ListaMobiliario = new List<DSRITMOBILIARIORESERVACION>();
                            foreach (var m in _getMobiliarios)
                            {
                                _ListaMobiliario.Add(m);
                                basededatos.DSRITMOBILIARIORESERVACION.Remove(m);
                            }
                            var listaReservas = _getReservaciones.ToList();
                            //Sacar los id de las reservas anteriores a editar
                            List<int> idinmuebles = new List<int>();
                            foreach (var reser in listaReservas)
                            {
                                reservacionTemporal.ID_PERSONA = reser.ID_PERSONA;
                                idinmuebles.Add(reser.ID_INMUEBLE);
                            }
                            idinmuebles = idinmuebles.Distinct().ToList();
                            //  Tener en cuenta este pedaso de codigo para revisar el id a la hora de editar puesto que no deberia tener problema, pero al parecer no lo está haciendo bien 
                            // Para el viernes
                            int largo = idinmuebles.Count - 1;


                            if (!semanaigual(_ReservaNuevaVieja.ListaDiasViejos, _ReservaNuevaVieja.listaDias) ||
                                hi_viejo != horai || hf_viejo != horaf || _ReservaNuevaVieja.fi_viejo != _ReservaNuevaVieja.fechai ||
                                _ReservaNuevaVieja.ff_viejo != _ReservaNuevaVieja.fechaf)// si retorna false
                            {
                                foreach (var reser in listaReservas)
                                {
                                    if (disponibilidadHora(horai, horaf, _ReservaNuevaVieja.fechai, _ReservaNuevaVieja.fechaf, reser.FEC_FINALRESERVACION, reser.ID_RESERVACION, _ReservaNuevaVieja.listaDias, _ReservaNuevaVieja.listaInmuebles) == false)
                                    {
                                        return Content("Error al editar reserva");
                                    }
                                }
                            }
                            foreach (var reser in listaReservas)
                            {
                                // eliminar todas las reservas viejas sin hacer commit por aquello de que pase un error de conexion y todo quede como antes
                                DSRIFRESERVACION res_aux = basededatos.DSRIFRESERVACION.Find(reser.ID_RESERVACION);
                                reservacionTemporal = reser;
                                basededatos.DSRIFRESERVACION.Remove(res_aux);
                                basededatos.SaveChanges();
                            }
                            if (_ListaMobiliario.Count != 0)
                            {
                                reservacionTemporal.DSRITMOBILIARIORESERVACION = _ListaMobiliario;
                            }
                            reservacionTemporal.NOM_ACTIVIDAD = _ReservaNuevaVieja.nombre;
                            reservacionTemporal.TXT_TELEFONO = _ReservaNuevaVieja.telef;
                            reservacionTemporal.HOR_INICIO = horai;
                            reservacionTemporal.HOR_FINAL = horaf;
                            reservacionTemporal.ID_TIPOACTIVIDAD = _ReservaNuevaVieja.tipoactividad;
                            reservacionTemporal.FEC_INICIALRESERVACION = _ReservaNuevaVieja.fechai;
                            reservacionTemporal.FEC_FINALRESERVACION = _ReservaNuevaVieja.fechaf;
                            reservacionTemporal.CAN_AFORO = _ReservaNuevaVieja.cantAforo;
                            reservacionTemporal.CAN_AFORODIA = _ReservaNuevaVieja.cantAforodia;
                            reservacionTemporal.ORGANIZADOR = _ReservaNuevaVieja.organizador;
                            reservacionTemporal.OBJETIVO = _ReservaNuevaVieja.objetivo;
                            reservacionTemporal.DESCRIPCION = _ReservaNuevaVieja.descripcion;

                            if (_ReservaNuevaVieja.des == null || _ReservaNuevaVieja.des == "")
                            {
                                reservacionTemporal.TXT_EXTRA = "N/A";
                            }
                            else
                            {
                                reservacionTemporal.TXT_EXTRA = _ReservaNuevaVieja.des;
                            }
                            //Empieza la creacion de la nueva reserva modificada y nueva
                            DateTime _fechaI = _ReservaNuevaVieja.fechai;
                            DateTime _fechaF = _ReservaNuevaVieja.fechaf;
                            List<int> listadeDias = _ReservaNuevaVieja.listaDias;
                            listadeDias = listadeDias.Distinct().ToList();
                            listadeDias.Sort();
                            while (_fechaI <= _fechaF)
                            {
                                DateTime FECHAINICIALUNICA = _fechaI;
                                bool reservaEntrante = false;
                                bool guardarReserva = false;
                                DateTime UnicaFI = new DateTime();
                                UnicaFI = _fechaI;
                                reservacionTemporal.FEC_INICIALRESERVACION = UnicaFI; // cargar la nueva fecha a la que se va recorriendo las nuevas fechas
                                //recorrer la lista de dias y asignar la fecha final nueva a cada una de las nuevas reservas
                                for (int dia = 0; dia < listadeDias.Count && (_fechaI <= _fechaF); dia++)
                                {
                                    DateTime NuevoInicio = new DateTime();
                                    NuevoInicio = _fechaI;
                                    if (listadeDias[dia] == (int)_fechaI.DayOfWeek)
                                    {
                                        reservaEntrante = true;
                                    }
                                    if (reservaEntrante)
                                    {
                                        guardarReserva = true;
                                        if (listadeDias[dia] == 6)
                                        {
                                            if (listadeDias.Count == 1 && listadeDias[dia] != (int)_fechaI.DayOfWeek)
                                            {
                                                break;
                                            }
                                            else if (listadeDias[dia] == (int)_fechaI.DayOfWeek && (_fechaI <= _fechaF))
                                            {
                                                reservacionTemporal.FEC_FINALRESERVACION = NuevoInicio;
                                            }
                                            _fechaI = _fechaI.AddDays(1);
                                            dia = -1;
                                        }
                                        else if (listadeDias[dia] == (int)_fechaI.DayOfWeek && (_fechaI <= _fechaF))
                                        {
                                            reservacionTemporal.FEC_FINALRESERVACION = NuevoInicio;
                                            _fechaI = _fechaI.AddDays(1);
                                        }
                                        else { break; }
                                    }
                                }
                                if (guardarReserva)
                                {
                                    foreach (var idinmueble in _ReservaNuevaVieja.listaInmuebles)// Guardar reserva Por cada inmueble seleccionado
                                    {
                                        reservacionTemporal.ID_INMUEBLE = idinmueble;

                                        DateTime FECHAINICIAL = FECHAINICIALUNICA;

                                        while (FECHAINICIAL <= reservacionTemporal.FEC_FINALRESERVACION)
                                        {
                                            reservacionTemporal.FEC_INICIALRESERVACION = FECHAINICIAL;
                                            basededatos.DSRIFRESERVACION.Add(reservacionTemporal);
                                            ////Para guardar los inmuebles de esta reserva
                                            for (int i = 0; i < _ListaMobiliario.Count; i++)
                                            {
                                                DSRITMOBILIARIORESERVACION temp = _ListaMobiliario.ElementAt(i);
                                                temp.ID_RESERVACION = reservacionTemporal.ID_RESERVACION;
                                                basededatos.DSRITMOBILIARIORESERVACION.Add(temp);
                                            }

                                            basededatos.SaveChanges();
                                            FECHAINICIAL = FECHAINICIAL.AddDays(1);
                                        }
                                    }
                                }
                                if (!reservaEntrante)
                                {
                                    _fechaI = _fechaI.AddDays(1);
                                }
                            }
                            dbContextTransaction.Commit(); // para registrar todos los cambios hechos en la base de datos
                        }
                        catch (DbEntityValidationException) // En caso de error se regresa al estado anterior sin necesidad de eliminar la base de datos
                        {
                            dbContextTransaction.Rollback();
                            return Content("Error al editar reserva");
                        }
                    }
                }
                var resultz = new { Success = "True" };
                return Json(resultz, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCantidadMobiliario(int? id)
        {
            if (id == null)
            {
                return Json(null, "Error");
            }
            DSRIFMOBILIARIO _dsrifMobiliario = db.DSRIFMOBILIARIO.Find(id);
            if (_dsrifMobiliario == null)
            {
                return Json(null, "Error");
            }
            var _returnedData = new
            {
                id,
                cantidad = _dsrifMobiliario.CAN_MOBILIARIO
            };
            return Json(_returnedData, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Obtiene los inmuebles disponibles para una fecha y hora específica
        /// </summary>
        /// <param name="_fecha">Recibe la fecha de Inicio</param>
        /// <param name="_hora">Recibe este parámetro como la hora Final para comprobar que no existen choques</param>
        /// <returns></returns>
        //public delegate void Worker();

        [WebMethod]
        public JsonResult GetInmuebles(DateTime _fechaI, DateTime _fechaF, string _horaI, string _horaF, List<int> _lDias)
        {
            TimeSpan _horaInicio;
            TimeSpan _horaFinal;
            /*Obtiene la fecha para realizar las comparaciones*/
            var _HoraInicioTemp = DateTime.Parse(_horaI, CultureInfo.CreateSpecificCulture("es-ES"));
            var _HoraFinalTemp = DateTime.Parse(_horaF, CultureInfo.CreateSpecificCulture("es-ES"));
            /*Convierte las fechas a formato "HH:MM:SS"*/
            _horaInicio = _HoraInicioTemp.TimeOfDay;
            _horaFinal = _HoraFinalTemp.TimeOfDay;
            var _res = Json(new[] {
            new { id="", name=""},
                }, JsonRequestBehavior.AllowGet);
            JsonObject _resultado = new JsonObject();
            List<int> listaGeneralInmueblesDisponibles = new List<int>();
            List<int> listaGeneralInmueblesDisponibles_aux = new List<int>();

            //QUITAR LOS VALORES REPETIDOS
            _lDias = _lDias.Distinct().ToList();
            // TODOS LOS INMUBLES EN EL SISTEMA
            var queryTodosLosInmuebles = from Inm in db.DSRIFINMUEBLE
                                         select new
                                         {
                                             id = Inm.ID_INMUEBLE,
                                             name = Inm.NOM_INMUEBLE
                                         };
            bool listaVacia = true;
            //COMPARAR CADA UNO POR DIA Y ELIMINARLO DE TODOS LOS INMUEBLES
            DateTime inicioAux = _fechaI;
            //AL HACER LA CONSULTA POR DIA, SOLO SE OCUPA LA FECHA DE INICIO PARA VERIFICAR SI EN ESE DIA ESTA LIBRE EL INMUEBLE Y RECORRE DIA POR DIA
            while (inicioAux <= _fechaF)
            {
                foreach (var i in _lDias)
                {
                    if (i == (int)(inicioAux.DayOfWeek))
                    {
                        var queryInmueblesParaUnDia = from Inm in db.inmublesDisponibles(inicioAux, inicioAux, _horaInicio, _horaFinal)
                                                      select new
                                                      {
                                                          id = Inm.ID_INMUEBLE,
                                                          name = Inm.NOM_INMUEBLE
                                                      };
                        if (listaVacia)
                        {
                            foreach (var soloinmu in queryInmueblesParaUnDia)
                            {
                                listaGeneralInmueblesDisponibles.Add(soloinmu.id);
                            }
                            listaVacia = false;
                        }
                        else
                        {
                            foreach (var soloinmu in queryInmueblesParaUnDia)
                            {// por cada inmueble de este dia
                                foreach (var inmu in listaGeneralInmueblesDisponibles)
                                {//por cada inmueble disponible para el primer dia
                                    if (soloinmu.id == inmu)
                                    {

                                        //Agregar los inmuebles que estan disponibles para este dia en especifico a otra lista 
                                        listaGeneralInmueblesDisponibles_aux.Add(soloinmu.id);
                                    }
                                }
                            }
                            //restaurar a la lista original y vaciar la auxiliar
                            listaGeneralInmueblesDisponibles.RemoveRange(0, listaGeneralInmueblesDisponibles.Count);
                            foreach (var id in listaGeneralInmueblesDisponibles_aux)
                            {
                                listaGeneralInmueblesDisponibles.Add(id);
                            }
                            listaGeneralInmueblesDisponibles_aux.RemoveRange(0, listaGeneralInmueblesDisponibles_aux.Count);
                        }
                        break;
                    }
                }
                inicioAux = inicioAux.AddDays(1);
            }
            // jala todos los inmuebles 
            var queryB = from Inm in db.DSRIFINMUEBLE
                         select new
                         {
                             id = Inm.ID_INMUEBLE,
                             name = Inm.NOM_INMUEBLE
                         };

            var resTemp = queryB.ToList();
            foreach (var Inm in queryB)
            {
                bool del = true;
                foreach (var soloI in listaGeneralInmueblesDisponibles)
                {
                    if (soloI == Inm.id)
                    {
                        del = false;
                        break;
                    }
                    del = true;
                }
                if (del)
                {
                    for (int i = 0; i < resTemp.Count; i++)
                    {
                        if (resTemp.ElementAt(i).id == Inm.id)
                        {
                            resTemp.Remove(resTemp.ElementAt(i));
                            del = false;
                            break;
                        }
                    }

                }
            }


            int contador = 0;
            bool bromelia = false;
            foreach (var inmueble in resTemp)
            {
                if (inmueble.name == "Bromelia 1" || inmueble.name == "Bromelia 2")
                {
                    contador++;
                }
                if (inmueble.name == "Bromelia Completo")
                {
                    bromelia = true;
                }
            }
            if (bromelia == false)
            {
                for (int i = 0; i < resTemp.Count; i++)
                {
                    if (resTemp.ElementAt(i).name == "Bromelia 1")
                    {
                        resTemp.Remove(resTemp.ElementAt(i));
                    }
                }
                for (int i = 0; i < resTemp.Count; i++)
                {
                    if (resTemp.ElementAt(i).name == "Bromelia 2")
                    {
                        resTemp.Remove(resTemp.ElementAt(i));
                    }
                }
            }

            else if (contador < 2)
            {
                for (int i = 0; i < resTemp.Count; i++)
                {
                    if (resTemp.ElementAt(i).name == "Bromelia Completo")
                    {
                        resTemp.Remove(resTemp.ElementAt(i));
                    }
                }
            }

            _res.Data = resTemp;
            return _res;
        }

        public bool disponibleInmueble(DateTime _fechaI, DateTime _fechaF, TimeSpan _horaInicio, TimeSpan _horaFinal, List<int> _lDias, int iddisponible, DSRIEntities basededatos)
        {
            var _res = Json(new[] {
            new { id="", name=""},
                }, JsonRequestBehavior.AllowGet);
            JsonObject _resultado = new JsonObject();
            List<int> listaGeneralInmueblesDisponibles = new List<int>();
            List<int> listaGeneralInmueblesDisponibles_aux = new List<int>();

            //QUITAR LOS VALORES REPETIDOS
            _lDias = _lDias.Distinct().ToList();
            // TODOS LOS INMUBLES EN EL SISTEMA
            var queryTodosLosInmuebles = from Inm in basededatos.DSRIFINMUEBLE
                                         select new
                                         {
                                             id = Inm.ID_INMUEBLE,
                                             name = Inm.NOM_INMUEBLE
                                         };
            bool listaVacia = true;
            //COMPARAR CADA UNO POR DIA Y ELIMINARLO DE TODOS LOS INMUEBLES
            DateTime inicioAux = _fechaI;
            //AL HACER LA CONSULTA POR DIA, SOLO SE OCUPA LA FECHA DE INICIO PARA VERIFICAR SI EN ESE DIA ESTA LIBRE EL INMUEBLE Y RECORRE DIA POR DIA
            while (inicioAux <= _fechaF)
            {
                foreach (var i in _lDias)
                {
                    if (i == (int)(inicioAux.DayOfWeek))
                    {
                        var queryInmueblesParaUnDia = from Inm in basededatos.inmublesDisponibles(inicioAux, inicioAux, _horaInicio, _horaFinal)
                                                      select new
                                                      {
                                                          id = Inm.ID_INMUEBLE,
                                                          name = Inm.NOM_INMUEBLE
                                                      };
                        if (listaVacia)
                        {
                            foreach (var soloinmu in queryInmueblesParaUnDia)
                            {
                                listaGeneralInmueblesDisponibles.Add(soloinmu.id);
                            }
                            listaVacia = false;
                        }
                        else
                        {
                            foreach (var soloinmu in queryInmueblesParaUnDia)
                            {// por cada inmueble de este dia
                                foreach (var inmu in listaGeneralInmueblesDisponibles)
                                {//por cada inmueble disponible para el primer dia
                                    if (soloinmu.id == inmu)
                                    {

                                        //Agregar los inmuebles que estan disponibles para este dia en especifico a otra lista 
                                        listaGeneralInmueblesDisponibles_aux.Add(soloinmu.id);
                                    }
                                }
                            }
                            //restaurar a la lista original y vaciar la auxiliar
                            listaGeneralInmueblesDisponibles.RemoveRange(0, listaGeneralInmueblesDisponibles.Count);
                            foreach (var id in listaGeneralInmueblesDisponibles_aux)
                            {
                                listaGeneralInmueblesDisponibles.Add(id);
                            }
                            listaGeneralInmueblesDisponibles_aux.RemoveRange(0, listaGeneralInmueblesDisponibles_aux.Count);
                        }
                        break;
                    }
                }
                inicioAux = inicioAux.AddDays(1);
            }

            var queryB = from Inm in basededatos.DSRIFINMUEBLE
                         select new
                         {
                             id = Inm.ID_INMUEBLE,
                             name = Inm.NOM_INMUEBLE
                         };

            var resTemp = queryB.ToList();
            foreach (var Inm in queryB)
            {
                bool del = false;
                foreach (var soloI in listaGeneralInmueblesDisponibles)
                {
                    if (soloI == Inm.id)
                    {
                        del = false;
                        break;
                    }
                    del = true;
                }
                if (del)
                {
                    for (int i = 0; i < resTemp.Count; i++)
                    {
                        if (resTemp.ElementAt(i).id == Inm.id)
                        {
                            resTemp.Remove(resTemp.ElementAt(i));
                            del = false;
                            break;
                        }
                    }

                }
            }


            int contador = 0;
            bool bromelia = false;
            foreach (var inmueble in resTemp)
            {
                if (inmueble.name == "Bromelia 1" || inmueble.name == "Bromelia 2")
                {
                    contador++;
                }
                if (inmueble.name == "Bromelia Completo")
                {
                    bromelia = true;
                }
            }
            if (bromelia == false)
            {
                for (int i = 0; i < resTemp.Count; i++)
                {
                    if (resTemp.ElementAt(i).name == "Bromelia 1")
                    {
                        resTemp.Remove(resTemp.ElementAt(i));
                    }
                }
                for (int i = 0; i < resTemp.Count; i++)
                {
                    if (resTemp.ElementAt(i).name == "Bromelia 2")
                    {
                        resTemp.Remove(resTemp.ElementAt(i));
                    }
                }
            }

            else if (contador < 2)
            {
                for (int i = 0; i < resTemp.Count; i++)
                {
                    if (resTemp.ElementAt(i).name == "Bromelia Completo")
                    {
                        resTemp.Remove(resTemp.ElementAt(i));
                    }
                }
            }
            // Comprobar la disponibilidad del inmueble de la edicion con la nueva fecha
            foreach (var reserva in resTemp)
            {
                if (reserva.id == iddisponible)
                {
                    return true;
                }
            }
            return false;
        }



        // Ver detalles desde la pantalla de inicio donde se encuentran la mayoria de reservas para el usuario
        public ActionResult Details(int? id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Session["funcion"]==null)
                {
                    Session["funcion"] = "MisReservas";
                }
                //@Session["funcion"] = "Index";
                DSRIFRESERVACION reservacion_auxiliar = db.DSRIFRESERVACION.Find(id);

                var reservacionesEstado = from reserva in db.DSRIFRESERVACION
                                          where reserva.NOM_ACTIVIDAD == reservacion_auxiliar.NOM_ACTIVIDAD &&
                                                reserva.ID_PERSONA == reservacion_auxiliar.ID_PERSONA &&
                                                reserva.TXT_TELEFONO == reservacion_auxiliar.TXT_TELEFONO &&
                                                reserva.OBJETIVO == reservacion_auxiliar.OBJETIVO &&
                                                reserva.ORGANIZADOR == reservacion_auxiliar.ORGANIZADOR &&
                                                reserva.ID_TIPOACTIVIDAD == reservacion_auxiliar.ID_TIPOACTIVIDAD &&
                                                reserva.TXT_ESTADO == reservacion_auxiliar.TXT_ESTADO &&
                                                reserva.HOR_INICIO == reservacion_auxiliar.HOR_INICIO &&
                                                reserva.HOR_FINAL == reservacion_auxiliar.HOR_FINAL
                                          select reserva;

                int id_aux = fechaPrincipal(reservacion_auxiliar.FEC_INICIALRESERVACION, reservacion_auxiliar.FEC_FINALRESERVACION, reservacion_auxiliar.ID_RESERVACION);
                DSRIFRESERVACION dsrifreservacion = db.DSRIFRESERVACION.Find(id_aux);
                if (dsrifreservacion == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifreservacion);
            }
        }
        public ActionResult ReservaExitosa()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (Session["COD_USUARIO"].Equals("FUNCIONARIO"))
                {
                    wsSeguridad.SeguridadSoap wsSeg = new SeguridadSoapClient();
                    string _Cedula = "";
                    string _Usuario = Session["ID_USUARIO"].ToString();
                    _Cedula = wsSeg.ObtenerCedula(_Usuario);
                    DataSet ds = wsSeg.TDInformacionUsuario(_Cedula);
                    string _Correo = getCorreo(ds);
                    Session["Email"] = _Correo;
                }
                return View();
            }
        }
        // GET: /Reservacion/Create
        public ActionResult Create()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.ID_INMUEBLE = new SelectList(db.DSRIFINMUEBLE, "ID_INMUEBLE", "NOM_INMUEBLE");
                ViewBag.ID_TIPOACTIVIDAD = new SelectList(db.DSRIFTIPOSACTIVIDAD, "ID_TIPOACTIVIDAD", "NOM_TIPOACTIVIDAD");
                ViewData["ListaMobiliario"] = new SelectList(db.DSRIFMOBILIARIO, "ID_MOBILIARIO", "NOM_MOBILIARIO");
                return View();
            }
        }
        public static TimeSpan ToTimeSpan(string s)
        {
            TimeSpan t = TimeSpan.Parse(s.Remove(2, 1).Remove(5, 1));
            return t;
        }
        public static int Cantidad_Aforo = 0;
        public static DateTime globlal_Inicio;
        public static DateTime globlal_Fin;
        public static string Num_Telefono = "";
        public static bool BanderaReservaCreada = false;

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [HttpPost]
        public ActionResult Create(DSRIFRESERVACION _Reserva, List<int> _Ldias, DateTime _fechaI, DateTime _fechaF, List<int> _Listainmuebles)
        {
            List<int> listadeDias = _Ldias;
            listadeDias = listadeDias.Distinct().ToList();
            listadeDias.Sort();
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                using (var basededatos = new DSRIEntities())
                {
                    using (var dbContextTransaction = basededatos.Database.BeginTransaction())
                    {
                        try
                        {
                            while (_fechaI <= _fechaF)
                            {
                                DateTime FECHAINICIALUNICA = _fechaI;
                                _Reserva.ID_PERSONA = Session["ID_USUARIO"].ToString();
                                bool reservaEntrante = false;
                                bool guardarReserva = false;
                                DateTime UnicaFI = new DateTime();
                                UnicaFI = _fechaI;
                                _Reserva.FEC_INICIALRESERVACION = UnicaFI; // cargar la nueva fecha a la que se va recorriendo las nuevas fechas
                                //recorrer la lista de dias y asignar la fecha final nueva a cada una de las nuevas reservas
                                for (int dia = 0; dia < listadeDias.Count; dia++)
                                {
                                    DateTime NuevoInicio = new DateTime();
                                    NuevoInicio = _fechaI;
                                    if (listadeDias[dia] == (int)_fechaI.DayOfWeek)
                                    {
                                        reservaEntrante = true;
                                    }
                                    if (reservaEntrante)
                                    {
                                        guardarReserva = true;
                                        if (listadeDias[dia] == 6)
                                        {
                                            if (listadeDias.Count == 1 && listadeDias[dia] != (int)_fechaI.DayOfWeek)
                                            {
                                                break;
                                            }
                                            else if (listadeDias[dia] == (int)_fechaI.DayOfWeek && (_fechaI <= _fechaF))
                                            {
                                                _Reserva.FEC_FINALRESERVACION = NuevoInicio;
                                                _fechaI = _fechaI.AddDays(1);
                                            }
                                            dia = -1;
                                        }
                                        else if (listadeDias[dia] == (int)_fechaI.DayOfWeek && (_fechaI <= _fechaF))
                                        {
                                            _Reserva.FEC_FINALRESERVACION = NuevoInicio;
                                            _fechaI = _fechaI.AddDays(1);
                                        }
                                        else { break; }
                                    }
                                }
                                if (guardarReserva)
                                {
                                    foreach (var idReserva in _Listainmuebles)// Guardar reserva Por cada inmueble seleccionado
                                    {
                                        _Reserva.ID_INMUEBLE = idReserva;

                                        DateTime FECHAINICIAL = FECHAINICIALUNICA;

                                        while (FECHAINICIAL <= _Reserva.FEC_FINALRESERVACION)
                                        {
                                            _Reserva.FEC_INICIALRESERVACION = FECHAINICIAL;
                                            basededatos.DSRIFRESERVACION.Add(_Reserva);

                                            ICollection<DSRITMOBILIARIORESERVACION> _ListaMobiliario = _Reserva.DSRITMOBILIARIORESERVACION;
                                            //Para guardar los inmuebles de esta reserva
                                            for (int i = 0; i < _ListaMobiliario.Count; i++)
                                            {
                                                DSRITMOBILIARIORESERVACION temp = _ListaMobiliario.ElementAt(i);
                                                basededatos.DSRITMOBILIARIORESERVACION.Add(temp);
                                            }

                                            basededatos.SaveChanges();
                                            FECHAINICIAL = FECHAINICIAL.AddDays(1);
                                        }
                                    }
                                }
                                if (!reservaEntrante)
                                {
                                    _fechaI = _fechaI.AddDays(1);
                                }
                            }
                            dbContextTransaction.Commit();
                            var resultz = new { Success = "True" };
                            return Json(resultz, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                return HttpNotFound();
            }
        }

        public List<SelectListItem> GetInm(List<int> idinmuebles)
        {
            var queryB = from Inm in db.DSRIFINMUEBLE
                         select new
                         {
                             id = Inm.ID_INMUEBLE,
                             name = Inm.NOM_INMUEBLE
                         };



            var ListaInm = new List<SelectListItem>();
            foreach (var element in queryB)
            {
                foreach (var i in idinmuebles)
                {
                    if (element.id == i)
                    {
                        ListaInm.Add(
                        new SelectListItem
                        {
                            Value = element.id.ToString(),
                            Text = element.name
                        });
                    }

                }
            }
            return ListaInm;
        }
        // Esta opcion es para cargar los datos de la reserva que se va a editar pero el 
        // Estado por ejemplo de En Proceso a Confirmada o Rechazada

        public ActionResult Edit(int? id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DSRIFRESERVACION temp = db.DSRIFRESERVACION.Find(id);
                int id_aux = fechaPrincipal(temp.FEC_INICIALRESERVACION, temp.FEC_FINALRESERVACION, temp.ID_RESERVACION);

                DSRIFRESERVACION dsrifreservacion = db.DSRIFRESERVACION.Find(id_aux);

                DSRIEntities dbN = new DSRIEntities();
                var reservas_a_editar = from reser_edit in dbN.DSRIFRESERVACION
                                        where dsrifreservacion.NOM_ACTIVIDAD == reser_edit.NOM_ACTIVIDAD &&
                                              dsrifreservacion.OBJETIVO == reser_edit.OBJETIVO &&
                                              dsrifreservacion.ORGANIZADOR == reser_edit.ORGANIZADOR &&
                                              dsrifreservacion.ID_TIPOACTIVIDAD == reser_edit.ID_TIPOACTIVIDAD &&
                                              dsrifreservacion.TXT_TELEFONO == reser_edit.TXT_TELEFONO &&
                                              dsrifreservacion.ID_PERSONA == reser_edit.ID_PERSONA &&
                                              dsrifreservacion.HOR_INICIO == reser_edit.HOR_INICIO &&
                                              dsrifreservacion.HOR_FINAL == reser_edit.HOR_FINAL
                                        select reser_edit;

                List<int> Lista_de_dias = new List<int>();
                List<int> Lista_de_inmuebles = new List<int>();

                var lista_reservas_a_
                    = reservas_a_editar.ToList();

                foreach (var r in reservas_a_editar)
                {
                    while (r.FEC_INICIALRESERVACION <= r.FEC_FINALRESERVACION)
                    {
                        Lista_de_inmuebles.Add((int)r.ID_INMUEBLE);
                        Lista_de_dias.Add((int)r.FEC_INICIALRESERVACION.DayOfWeek);
                        r.FEC_INICIALRESERVACION = r.FEC_INICIALRESERVACION.AddDays(1);
                    }
                }
                Lista_de_inmuebles = Lista_de_inmuebles.Distinct().ToList();// Quitar los inmuebles repetidos de la lista
                Lista_de_dias = Lista_de_dias.Distinct().ToList(); //Quitar los dias que estan repetidos en la lista de dias de la parte de editar las reservas


                //PARA LOS INMUEBLES
                List<string> Lista_de_inmuebles_string = new List<string>();
                // Esta parte es para crear la lista de dias pero en string para poder verlos en la parte de html en viewbag
                foreach (var inmueblenumero in Lista_de_inmuebles)
                {
                    Lista_de_inmuebles_string.Add(inmueblenumero.ToString());
                }
                ViewBag.ListaInmuebles = String.Join(",", Lista_de_inmuebles_string.Cast<string>().ToArray());



                // PARA LOS DIAS 
                List<string> Lista_de_dias_string = new List<string>();
                // Esta parte es para crear la lista de dias pero en string para poder verlos en la parte de html en viewbag
                foreach (var dianumero in Lista_de_dias)
                {
                    Lista_de_dias_string.Add(dianumero.ToString());
                }

                ViewBag.DiasEditar = String.Join(",", Lista_de_dias_string.Cast<string>().ToArray());

                if (dsrifreservacion == null)
                {
                    return HttpNotFound();
                }

                var todo = from reserva in db.DSRIFRESERVACION
                           where reserva.NOM_ACTIVIDAD == dsrifreservacion.NOM_ACTIVIDAD &&
                                 reserva.TXT_TELEFONO == dsrifreservacion.TXT_TELEFONO &&
                                 reserva.TXT_ESTADO == dsrifreservacion.TXT_ESTADO &&
                                 reserva.OBJETIVO == dsrifreservacion.OBJETIVO &&
                                 reserva.DESCRIPCION == dsrifreservacion.DESCRIPCION &&
                                 reserva.HOR_INICIO == dsrifreservacion.HOR_INICIO &&
                                 reserva.HOR_FINAL == dsrifreservacion.HOR_FINAL &&
                                 reserva.ID_PERSONA == dsrifreservacion.ID_PERSONA
                           select reserva;


                var listaDeTodasLasReservas = todo.ToList();
                // Sacar la fecha mayor
                foreach (var i in listaDeTodasLasReservas)
                {
                    if (dsrifreservacion.FEC_FINALRESERVACION < i.FEC_FINALRESERVACION)
                    {
                        dsrifreservacion.FEC_FINALRESERVACION = i.FEC_FINALRESERVACION;
                    }
                }
                var fecha = dsrifreservacion.FEC_INICIALRESERVACION;
                var hora1 = dsrifreservacion.HOR_INICIO;
                var hora2 = dsrifreservacion.HOR_FINAL;
                var Inm = GetInm(Lista_de_inmuebles);
                //ViewBag.INMUEBLE = new SelectList(db.DSRIFINMUEBLE, "ID_INMUEBLE", "NOM_INMUEBLE", dsrifreservacion.ID_INMUEBLE);
                ViewBag.ID_INMUEBLE = new SelectList(Inm, "Value", "Text");
                //ViewBag.ID_PERSONA = Session["ID_USUARIO"].ToString();
                ViewBag.ID_TIPOACTIVIDAD = new SelectList(db.DSRIFTIPOSACTIVIDAD, "ID_TIPOACTIVIDAD", "NOM_TIPOACTIVIDAD");
                return View(dsrifreservacion);
            }
        }

        string getCorreo(DataSet _ds)
        {
            DataTable dt = _ds.Tables[0];
            string correo = "";
            foreach (DataRow dr in dt.Rows)
            {
                correo = dr[3].ToString();
            }
            return correo;
        }
        public string getCorreoUserNormal(string _idPersona)
        {
            var dsriusuarios = from user in db.DSRIFUSUARIOS
                               where (user.NOM_USUARIO == _idPersona)
                               select user;
            var _ListaUsuarios = dsriusuarios.ToList();
            var correo = "";
            foreach (var user in _ListaUsuarios)
            {
                correo = user.TXT_CORREO;
            }
            return correo;
        }
        void EnviarCorreo(string _Usuario, string _TipoRespuesta, string motivos, string _Inmueble,
            string _FechaI, string _FechaF, string _HoraI, string _HoraF, string nom_actividad)
        {
            //string _Usuario = Session["ID_USUARIO"].ToString();

            string _Correo = "";
            string _Remitente = "infoctec@itcr.ac.cr";
            string password = "Mailctec2011";
            wsSeguridad.SeguridadSoap wsSeg = new SeguridadSoapClient();
            string _Cedula = "";
            string _Destinatario = "";
            //_Cedula = wsSeg.ObtenerCedula(_Usuario);
            if (getCorreoUserNormal(_Usuario).Equals(""))
            {
                _Cedula = wsSeg.ObtenerCedula(_Usuario);
                DataSet ds = wsSeg.TDInformacionUsuario(_Cedula);
                _Correo = getCorreo(ds);
            }
            else
            {
                _Correo = getCorreoUserNormal(_Usuario);
            }
            _Destinatario = _Correo; 
            NetworkCredential loginInfo = new NetworkCredential(_Remitente, password);
            MailMessage msg = new MailMessage();

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            /* Este es el mensaje que llevará por defecto*/
            string _Mensaje = "";
            if (_TipoRespuesta.Equals("Rechazada"))
            {
                _Mensaje = " Estimado Usuario, le informamos que su solicitud  ha sido Rechazada por los siguientes motivos: <br />" + motivos + "<br /> Rogamos su comprensión." +
                            "<br /><br />" +
                             "<strong>Actividad: </strong>" + nom_actividad + "<br />" +
                             "<strong>Inmueble: </strong>" + _Inmueble + "<br />" +
                            "<strong>Fecha de Inicio: </strong>" + _FechaI + "<br />" +
                            "<strong>Fecha Final: </strong>" + _FechaF + "<br />" +
                            "<strong>Hora de Inicio: </strong>" + _HoraI + "<br />" +
                            "<strong>Hora Final: </strong>" + _HoraF + "<br />" +
                            "<br /> <br />" +
                                "Si desea más información puede llamar al teléfono: 2401-3002 o responder a este correo";
            }
            else if (_TipoRespuesta.Equals("Confirmada"))
            {
                _Mensaje = string.Format(@" Estimado Usuario, le informamos que su solicitud  ha sido Aprobada. Se adjunta Resumen de la solicitud:" +
                            "<br /><br /> " +
                             "<strong>Actividad: </strong>" + nom_actividad + "<br />" +
                             "<strong>Inmueble:</strong>" + _Inmueble + "<br />" +
                            "<strong>Fecha de Inicio: </strong>" + _FechaI + "<br />" +
                            "<strong>Fecha Final: </strong>" + _FechaF + "<br />" +
                            "<strong>Hora de Inicio: </strong>" + _HoraI + "<br />" +
                            "<strong>Hora Final: </strong>" + _HoraF + "<br />" +
                            "<br /> <br />" +
                            "Recuerde que tiene un límite máximo de 30 minutos para desocupar el Inmueble luego de la hora de Finalización");
            }
            try
            {

                msg.From = new MailAddress(_Remitente, "Sistema de Reservas de Inmuebles, TEC-SSC");
                msg.To.Add(new MailAddress(_Destinatario));
                msg.Subject = "Respuesta a Solicitud de Reserva";
                msg.Body = _Mensaje;
                msg.IsBodyHtml = true;
                smtpClient.Send(msg);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Devuelve el nombre un inmueble
        /// </summary>
        /// <param name="_id">Número que compará el ID del Inmueble</param>
        string getNombreInmueble(int _id)
        {
            var retorno = "";
            var res = from I in db.DSRIFINMUEBLE
                      where I.ID_INMUEBLE == _id
                      select I;
            foreach (var elemento in res)
            {
                retorno = elemento.NOM_INMUEBLE;
            }
            return retorno;
            //return res.ElementAt(0);
        }
        // POST: /Reservacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_RESERVACION,ID_INMUEBLE,TXT_ESTADO")] DSRIFRESERVACION dsrifreservacion, FormCollection form)// Editar del boton guardar cmbios para hacer una reserva
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DSRIFRESERVACION reservacion_auxiliar = db.DSRIFRESERVACION.Find(dsrifreservacion.ID_RESERVACION);
                try
                {
                    string motivo = form.Get("Motivo");
                    string Inmueble = getNombreInmueble(dsrifreservacion.ID_INMUEBLE);
                    string nom_actividad = "";
                    string FechaI = "";
                    string FechaF = "";
                    string HoraI = "";
                    string HoraF = "";
                    string Usuario = "";
                    string TipoRespuesta = dsrifreservacion.TXT_ESTADO;

                    //IQueryable<DSRIFRESERVACION> Reservaciones = db.DSRIFRESERVACION.Where(x => x.ID_RESERVACION == dsrifreservacion.ID_RESERVACION);
                    var reservacionesEstado = from reserva in db.DSRIFRESERVACION
                                              where reserva.NOM_ACTIVIDAD == reservacion_auxiliar.NOM_ACTIVIDAD &&
                                                    reserva.TXT_TELEFONO == reservacion_auxiliar.TXT_TELEFONO &&
                                                    reserva.TXT_ESTADO == reservacion_auxiliar.TXT_ESTADO &&
                                                    reserva.OBJETIVO == reservacion_auxiliar.OBJETIVO &&
                                                    reserva.ID_TIPOACTIVIDAD == reservacion_auxiliar.ID_TIPOACTIVIDAD &&
                                                    reserva.DESCRIPCION == reservacion_auxiliar.DESCRIPCION &&
                                                    reserva.HOR_INICIO == reservacion_auxiliar.HOR_INICIO &&
                                                    reserva.ID_PERSONA == reservacion_auxiliar.ID_PERSONA &&
                                                    reserva.HOR_FINAL == reservacion_auxiliar.HOR_FINAL
                                              select reserva;

                    DateTime fI = reservacion_auxiliar.FEC_INICIALRESERVACION;
                    var lista_principal = reservacionesEstado.ToList();
                    foreach (var registro in lista_principal)
                    {
                        nom_actividad = registro.NOM_ACTIVIDAD;
                        //registro.ID_INMUEBLE = dsrifreservacion.ID_INMUEBLE; Aqui se cargar el nuevo inmueble por lo que lo comente para que quede con el de siempre
                        Inmueble = getNombreInmueble(registro.ID_INMUEBLE);
                        registro.TXT_ESTADO = dsrifreservacion.TXT_ESTADO;
                        if (fI >= registro.FEC_INICIALRESERVACION)
                        {
                            fI = registro.FEC_INICIALRESERVACION;
                        }
                        FechaI = fI.ToShortDateString();
                        FechaF = registro.FEC_FINALRESERVACION.ToShortDateString();
                        HoraI = registro.HOR_INICIO.ToString();
                        HoraF = registro.HOR_FINAL.ToString();
                        Usuario = registro.ID_PERSONA; // AQUI HAY UN PROBLEMA
                        db.Entry(registro).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    if (Usuario.Equals("admin"))
                    {
                        //db.Entry(dSRIFUSUARIOS).State = EntityState.Modified;
                        //db.SaveChanges();
                    }
                    else
                    {
                        //db.Entry(dSRIFUSUARIOS).State = EntityState.Modified;
                        //db.SaveChanges();
                        EnviarCorreo(Usuario, TipoRespuesta, motivo, Inmueble, FechaI, FechaF, HoraI, HoraF, nom_actividad);
                    }
                }
                catch (DBConcurrencyException ex)
                {
                    Response.Write(ex.Message.ToString());
                }

                return RedirectToAction("Details/" + dsrifreservacion.ID_RESERVACION);
            }
        }
        // GET: /Reservacion/Edit/5
        public ActionResult EditarReserva(int? id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DSRIFRESERVACION temp = db.DSRIFRESERVACION.Find(id);
                int id_aux = fechaPrincipal(temp.FEC_INICIALRESERVACION, temp.FEC_FINALRESERVACION, temp.ID_RESERVACION);

                DSRIFRESERVACION dsrifreservacion = db.DSRIFRESERVACION.Find(id_aux);

                DSRIEntities dbN = new DSRIEntities();
                var reservas_a_editar = from reser_edit in dbN.DSRIFRESERVACION
                                        where dsrifreservacion.NOM_ACTIVIDAD == reser_edit.NOM_ACTIVIDAD &&
                                              dsrifreservacion.OBJETIVO == reser_edit.OBJETIVO &&
                                              dsrifreservacion.ORGANIZADOR == reser_edit.ORGANIZADOR &&
                                              dsrifreservacion.ID_TIPOACTIVIDAD == reser_edit.ID_TIPOACTIVIDAD &&
                                              dsrifreservacion.TXT_TELEFONO == reser_edit.TXT_TELEFONO &&
                                              dsrifreservacion.HOR_INICIO == reser_edit.HOR_INICIO &&
                                              dsrifreservacion.ID_PERSONA == reser_edit.ID_PERSONA &&
                                              dsrifreservacion.HOR_FINAL == reser_edit.HOR_FINAL
                                        select reser_edit;

                List<int> Lista_de_dias = new List<int>();
                List<int> Lista_de_inmuebles = new List<int>();

                var lista_reservas_a_
                    = reservas_a_editar.ToList();

                foreach (var r in reservas_a_editar)
                {
                    while (r.FEC_INICIALRESERVACION <= r.FEC_FINALRESERVACION)
                    {
                        Lista_de_inmuebles.Add((int)r.ID_INMUEBLE);
                        Lista_de_dias.Add((int)r.FEC_INICIALRESERVACION.DayOfWeek);
                        r.FEC_INICIALRESERVACION = r.FEC_INICIALRESERVACION.AddDays(1);
                    }
                }
                Lista_de_inmuebles = Lista_de_inmuebles.Distinct().ToList();// Quitar los inmuebles repetidos de la lista
                Lista_de_dias = Lista_de_dias.Distinct().ToList(); //Quitar los dias que estan repetidos en la lista de dias de la parte de editar las reservas


                //PARA LOS INMUEBLES
                List<string> Lista_de_inmuebles_string = new List<string>();
                // Esta parte es para crear la lista de dias pero en string para poder verlos en la parte de html en viewbag
                foreach (var inmueblenumero in Lista_de_inmuebles)
                {
                    Lista_de_inmuebles_string.Add(inmueblenumero.ToString());
                }
                ViewBag.ListaInmuebles = String.Join(",", Lista_de_inmuebles_string.Cast<string>().ToArray());



                // PARA LOS DIAS 
                List<string> Lista_de_dias_string = new List<string>();
                // Esta parte es para crear la lista de dias pero en string para poder verlos en la parte de html en viewbag
                foreach (var dianumero in Lista_de_dias)
                {
                    Lista_de_dias_string.Add(dianumero.ToString());
                }

                ViewBag.DiasEditar = String.Join(",", Lista_de_dias_string.Cast<string>().ToArray());

                if (dsrifreservacion == null)
                {
                    return HttpNotFound();
                }

                var todo = from reserva in db.DSRIFRESERVACION
                           where reserva.NOM_ACTIVIDAD == dsrifreservacion.NOM_ACTIVIDAD &&
                                 reserva.TXT_TELEFONO == dsrifreservacion.TXT_TELEFONO &&
                                 reserva.TXT_ESTADO == dsrifreservacion.TXT_ESTADO &&
                                 reserva.OBJETIVO == dsrifreservacion.OBJETIVO &&
                                 reserva.DESCRIPCION == dsrifreservacion.DESCRIPCION &&
                                 reserva.HOR_INICIO == dsrifreservacion.HOR_INICIO &&
                                 reserva.HOR_FINAL == dsrifreservacion.HOR_FINAL &&
                                 reserva.ID_PERSONA == dsrifreservacion.ID_PERSONA
                           select reserva;


                var listaDeTodasLasReservas = todo.ToList();
                // Sacar la fecha mayor
                foreach (var i in listaDeTodasLasReservas)
                {
                    if (dsrifreservacion.FEC_FINALRESERVACION < i.FEC_FINALRESERVACION)
                    {
                        dsrifreservacion.FEC_FINALRESERVACION = i.FEC_FINALRESERVACION;
                    }
                }
                var fecha = dsrifreservacion.FEC_INICIALRESERVACION;
                var hora1 = dsrifreservacion.HOR_INICIO;
                var hora2 = dsrifreservacion.HOR_FINAL;
                var Inm = GetInm(Lista_de_inmuebles);
                //ViewBag.INMUEBLE = new SelectList(db.DSRIFINMUEBLE, "ID_INMUEBLE", "NOM_INMUEBLE", dsrifreservacion.ID_INMUEBLE);
                ViewBag.ID_INMUEBLE = new SelectList(Inm, "Value", "Text");
                //ViewBag.ID_PERSONA = Session["ID_USUARIO"].ToString();
                ViewBag.ID_TIPOACTIVIDAD = new SelectList(db.DSRIFTIPOSACTIVIDAD, "ID_TIPOACTIVIDAD", "NOM_TIPOACTIVIDAD");
                return View(dsrifreservacion);
            }
        }
        //POST: /Reservacion/MisReservas/ycastillo
        public ActionResult MisReservas(string id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Session["estado"] = "";
                Session["funcion"] = "MisReservas";
                id = Session["ID_USUARIO"].ToString();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var reservacion = db.DSRIFRESERVACION;
                var reservaciones = from b in reservacion
                                    where (b.ID_PERSONA == id)
                                    select b;
                var _query1 = reservaciones.ToList().OrderBy(l => l.FEC_INICIALRESERVACION);
                var _query2 = _query1.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Month);
                var _query3 = _query2.ToList().OrderBy(l => l.FEC_INICIALRESERVACION.Date);
                return View(_query3);
            }
        }

        // GET: /Reservacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DSRIFRESERVACION dsrifreservacion = db.DSRIFRESERVACION.Find(id);
                if (dsrifreservacion == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifreservacion);
            }
        }

        // POST: /Reservacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DSRIFRESERVACION reservacion_auxiliar = db.DSRIFRESERVACION.Find(id);

                var reservacionesEstado = from reserva in db.DSRIFRESERVACION
                                          where reserva.NOM_ACTIVIDAD == reservacion_auxiliar.NOM_ACTIVIDAD &&
                                                reserva.TXT_TELEFONO == reservacion_auxiliar.TXT_TELEFONO &&
                                                reserva.ID_PERSONA == reservacion_auxiliar.ID_PERSONA &&
                                                reserva.OBJETIVO == reservacion_auxiliar.OBJETIVO &&
                                                reserva.ORGANIZADOR == reservacion_auxiliar.ORGANIZADOR &&
                                                reserva.ID_TIPOACTIVIDAD == reservacion_auxiliar.ID_TIPOACTIVIDAD &&
                                                reserva.TXT_ESTADO == reservacion_auxiliar.TXT_ESTADO &&
                                                reserva.HOR_INICIO == reservacion_auxiliar.HOR_INICIO &&
                                                reserva.HOR_FINAL == reservacion_auxiliar.HOR_FINAL
                                          select reserva;


                var lista_principal = reservacionesEstado.ToList();
                foreach (var registro in lista_principal)
                {
                    db.DSRIFRESERVACION.Remove(registro);
                    db.SaveChanges();
                }

                if (Session["COD_USUARIO"].ToString().Equals("FUNCIONARIO"))
                {
                    return RedirectToAction("MisReservas/" + Session["NOM_USUARIO"].ToString());
                }
                else if (Session["COD_USUARIO"].ToString().Equals("SISTEMA"))
                    return RedirectToAction("MisReservas/admin");
                else
                    return RedirectToAction("Index");
            }

        }
    }
}

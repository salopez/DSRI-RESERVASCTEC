using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DSRI.wsSeguridad;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Datos;
using System.ServiceModel;
using System.Net;
using System.Data;

namespace DSRI.Controllers
{
    public class HomeController : Controller
    {
        public static int usuario = 0;
        private DSRIEntities db = new DSRIEntities();
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null || Session["COD_USUARIO"].ToString() == "")
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var dsrifreservacion = from reservas in db.DSRIFRESERVACION
                                       where reservas.FEC_INICIALRESERVACION.Day == DateTime.Now.Day &&
                                             reservas.FEC_INICIALRESERVACION.Month == DateTime.Now.Month &&
                                             reservas.TXT_ESTADO == "Confirmada"
                                       select reservas;
                var lista_principal = dsrifreservacion.ToList();

                //foreach (var _reserva in dsrifreservacion.ToList())
                //{
                //    var lista_aux = db.reservasUnicas();
                //    bool eliminar = true;
                //    foreach (var _reserva_aux in lista_aux)
                //    {
                //        if (_reserva.ID_RESERVACION == _reserva_aux.ID_RESERVACION)
                //        {
                //            eliminar = false;
                //            break;
                //        }

                //    }
                //    if (eliminar)
                //    {
                //        for (int i = 0; i < lista_principal.Count; i++)
                //        {
                //            if (lista_principal.ElementAt(i).ID_RESERVACION == _reserva.ID_RESERVACION)
                //            {
                //                lista_principal.Remove(lista_principal.ElementAt(i));
                //                break;
                //            }
                //        }
                //    }
                //}
                //return RedirectToAction("Index", "Home");
                var _listaOrdenada = lista_principal.ToList().OrderBy(ll => ll.HOR_INICIO);
                return View(_listaOrdenada);
            }
        }

        public JsonResult Informacion(int? idRe)
        {
            //string idReser = Convert.ToString(idRe);
            //int idReservacion = Convert.ToInt32(idReser);
            DSRIFRESERVACION reserva = db.DSRIFRESERVACION.Find(idRe);
            var _res = new
            {
                idRe,
                reserva
            };
            //_res.Data = reserva;
            return Json(_res, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult Cotizar()
        {
            return View();
        }


        public ActionResult Inmuebles()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public int codTipoUsuario_funtion(string user, string pass)
        {
            int tipo = 1;
            var _Usuarios = db.DSRIFUSUARIOS;
            var _getUsuario = from us in _Usuarios
                              select us;
            var _ListaUsuarios = _getUsuario.ToList();

            foreach (var _Usuario in _ListaUsuarios)
            {
                if ((_Usuario.NOM_USUARIO.Equals(user)) && (_Usuario.TXT_CONTRASEÑA.Equals(pass)))
                {
                    if (_Usuario.TXT_ROL.Equals("Asociacion"))
                    {
                        tipo = 2;
                        break;
                    }
                    else if (_Usuario.TXT_ROL.Equals("Asistente"))
                    {
                        tipo = 3;
                        break;
                    }
                    else if (_Usuario.TXT_ROL.Equals("ODOO"))
                    {
                        tipo = 4;
                        break;
                    }
                    else if (_Usuario.TXT_ROL.Equals("Administrador"))
                    {
                        tipo = 5;
                        break;
                    }
                    else
                    {
                        tipo = 1;
                        break;
                    }

                }
            }
            return tipo;
        }
        public ActionResult Ingresar(List<String> Datos, string returnUrl)
        {
            string _Usuario = Datos[0];
            string _Contraseña = Datos[1];
            Session["COD_SEDE"] = "SC";
            wsSeguridad.SeguridadSoapClient wsseg = new wsSeguridad.SeguridadSoapClient();
            //int codTipoUsuario = Convert.ToInt32(Datos[2]);
            int codTipoUsuario = codTipoUsuario_funtion(_Usuario, _Contraseña);
            try
            {
                if ((_Usuario != "") && (_Contraseña != ""))
                {
                    switch (codTipoUsuario)
                    {
                        case 1://Funcionario
                            {
                                wsseg.ValidarFuncionario(_Usuario, _Contraseña);
                                Session.Add("ID_USUARIO", _Usuario);
                                Session.Add("COD_USUARIO", "FUNCIONARIO");
                                Session.Add("NOM_USUARIO", wsseg.ObtenerNombreUsuario(_Usuario)); //obtener nombre completo del usuario.
                                Session.Add("COD_SEDE", Session["COD_SEDE"].ToString());
                                Session.Add("NOM_DEPARTAMENTO", wsseg.ObtenerDepartamento(_Usuario));
                                User.Identity.Name.Equals(_Usuario);

                                wsSeguridad.SeguridadSoap wsSeg = new SeguridadSoapClient();
                                string _Cedula = "";
                                _Cedula = wsSeg.ObtenerCedula(_Usuario);
                                DataSet ds = wsSeg.TDInformacionUsuario(_Cedula);
                                string _Correo = getCorreo(ds);
                                Session["Email"] = _Correo;

                                //return RedirectToLocal(returnUrl);
                                var resultz = new { Success = "True" };
                                return Json(resultz, JsonRequestBehavior.AllowGet);

                            }
                        case 2://Usuario Asistente
                            {
                                bool res = false;
                                var _Usuarios = db.DSRIFUSUARIOS;
                                var _getUsuario = from us in _Usuarios
                                                  where us.TXT_ROL == "Asociacion"
                                                  select us;
                                var _ListaUsuarios = _getUsuario.ToList();
                                var _today = DateTime.Today;
                                foreach (var user in _ListaUsuarios)
                                {
                                    if ((_Usuario.Equals(user.NOM_USUARIO) && (_Contraseña.Equals(user.TXT_CONTRASEÑA))) && _today <= user.FEC_EXPIRACION)
                                    {
                                        Session.Add("ID_USUARIO", _Usuario);
                                        Session.Add("COD_USUARIO", "ASOCIACION");
                                        Session.Add("NOM_USUARIO", "Asociación de Estudiantes");
                                        Session.Add("NOM_DEPARTAMENTO", "Asociación de Estudiantes");
                                        Session.Add("Email", user.TXT_CORREO);
                                        res = true;
                                        usuario = 1;
                                        break;
                                    }
                                    else
                                        res = false;
                                }
                                if (res)
                                {
                                    var resultz = new { Success = "True" };
                                    return Json(resultz, JsonRequestBehavior.AllowGet);
                                }
                                else
                                    usuario = 0;
                                return Content("Datos incorrectos");
                            }
                        case 3://Usuario Sistema
                            {
                                bool res = false;
                                var _Usuarios = db.DSRIFUSUARIOS;
                                var _getUsuario = from us in _Usuarios
                                                  where us.TXT_ROL == "Asistente"
                                                  select us;
                                var _ListaUsuarios = _getUsuario.ToList();
                                var _today = DateTime.Today;
                                foreach (var user in _ListaUsuarios)
                                {
                                    if ((_Usuario.Equals(user.NOM_USUARIO) && (_Contraseña.Equals(user.TXT_CONTRASEÑA))) && _today <= user.FEC_EXPIRACION)
                                    {
                                        Session.Add("ID_USUARIO", _Usuario);
                                        Session.Add("COD_USUARIO", "ASISTENTE");
                                        Session.Add("NOM_USUARIO", "Asistente");
                                        Session.Add("NOM_DEPARTAMENTO", "Asistente");
                                        Session.Add("Email", user.TXT_CORREO);
                                        res = true;
                                        usuario = 1;
                                        break;
                                    }

                                    else
                                        res = false;
                                }
                                if (res)
                                {
                                    var resultz = new { Success = "True" };
                                    return Json(resultz, JsonRequestBehavior.AllowGet);
                                }
                                else
                                    usuario = 0;
                                return Content("Datos incorrectos");
                            }
                        case 4://Usuario Sistema
                            {
                                bool res = false;
                                var _Usuarios = db.DSRIFUSUARIOS;
                                var _getUsuario = from us in _Usuarios
                                                  where us.TXT_ROL == "ODOO"
                                                  select us;
                                var _ListaUsuarios = _getUsuario.ToList();
                                var _today = DateTime.Today;
                                foreach (var user in _ListaUsuarios)
                                {
                                    if ((_Usuario.Equals(user.NOM_USUARIO) && (_Contraseña.Equals(user.TXT_CONTRASEÑA))) && _today <= user.FEC_EXPIRACION)
                                    {
                                        Session.Add("ID_USUARIO", _Usuario);
                                        Session.Add("COD_USUARIO", "ODOO");
                                        Session.Add("NOM_USUARIO", "Odoo");
                                        Session.Add("NOM_DEPARTAMENTO", "Odoo");
                                        Session.Add("Email", user.TXT_CORREO);
                                        res = true;
                                        usuario = 1;
                                        break;
                                    }

                                    else
                                        res = false;
                                }
                                if (res)
                                {
                                    var resultz = new { Success = "True" };
                                    return Json(resultz, JsonRequestBehavior.AllowGet);
                                }
                                else
                                    usuario = 0;
                                return Content("Datos incorrectos");
                            }
                        case 5://Usuario Sistema
                            {
                                bool res = false;
                                var _Usuarios = db.DSRIFUSUARIOS;
                                var _getUsuario = from us in _Usuarios
                                                  where us.TXT_ROL == "Administrador"
                                                  select us;
                                var _ListaUsuarios = _getUsuario.ToList();
                                foreach (var user in _ListaUsuarios)
                                {
                                    if (_Usuario.Equals(user.NOM_USUARIO) && (_Contraseña.Equals(user.TXT_CONTRASEÑA)))
                                    {
                                        Session.Add("ID_USUARIO", _Usuario);
                                        Session.Add("COD_USUARIO", "SISTEMA");
                                        Session.Add("NOM_USUARIO", "Administrador");
                                        Session.Add("NOM_DEPARTAMENTO", "Sistema");
                                        Session.Add("Email", user.TXT_CORREO);
                                        res = true;
                                        usuario = 1;
                                        break;
                                    }

                                    else
                                        res = false;
                                }
                                if (res)
                                {
                                    var resultz = new { Success = "True" };
                                    return Json(resultz, JsonRequestBehavior.AllowGet);
                                }
                                else
                                    usuario = 0;
                                return Content("Datos incorrectos");
                            }
                    }
                }
            }
            catch (FaultException ex)
            {
                string error = ex.Message.ToString();
                Response.Write("<script>alert('" + error + "');</script>");
                return View("");
            }
            var result = new { Success = "False" };
            //return null;
            return Content("Datos incorrectos");
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Obtiene la vista de login
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session.Clear();
            Session.Abandon();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Valida si un usuario existe
        /// </summary>
        /// <param name="Credenciales">Instancia de LoginViewModel donde se contienen los datos capturados en el formulario, Nombre de Usuario, Contraseña, Tipo de Usuario</param>
        /// <returns></returns>

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
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

    }
}
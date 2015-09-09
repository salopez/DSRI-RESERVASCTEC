using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datos;
using DSRI.wsSeguridad;
using System.Net.Mail;

namespace DSRI.Controllers
{
    public class UsuariosController : Controller
    {
        private DSRIEntities db = new DSRIEntities();

        // GET: Usuarios activos
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var _Usuarios = db.DSRIFUSUARIOS;


                var _hoy = DateTime.Now.Date.ToString();
                DateTime FechaI = DateTime.Parse(_hoy);
                var _getUsuarios = from us in _Usuarios
                                   where us.FEC_EXPIRACION >= FechaI
                                   select us;
                var listUsuarios = _getUsuarios.ToList();
                return View(listUsuarios);
            }
        }

        // GET: Usuarios inactivos
        public ActionResult Inactivos()
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var _Usuarios = db.DSRIFUSUARIOS;


                var _hoy = DateTime.Now.Date.ToString();
                DateTime FechaI = DateTime.Parse(_hoy);
                var _getUsuarios = from us in _Usuarios
                                   where us.FEC_EXPIRACION < FechaI
                                   select us;
                var listUsuarios = _getUsuarios.ToList();
                return View(listUsuarios);
            }
        }

        // GET: Usuarios/Details/5
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
                DSRIFUSUARIOS dSRIFUSUARIOS = db.DSRIFUSUARIOS.Find(id);
                if (dSRIFUSUARIOS == null)
                {
                    return HttpNotFound();
                }
                return View(dSRIFUSUARIOS);
            }
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_USUARIO,NOM_USUARIO,TXT_CONTRASEÑA,TXT_CORREO,TXT_ROL,FEC_EXPIRACION")] DSRIFUSUARIOS dSRIFUSUARIOS)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.DSRIFUSUARIOS.Add(dSRIFUSUARIOS);
                    db.SaveChanges();
                    EnviarCorreo(dSRIFUSUARIOS.NOM_USUARIO, dSRIFUSUARIOS.TXT_CONTRASEÑA, dSRIFUSUARIOS.FEC_EXPIRACION.ToString(), dSRIFUSUARIOS.TXT_CORREO);
                    return RedirectToAction("Index");
                }

                return View(dSRIFUSUARIOS);
            }
        }


        void EnviarCorreo(string _Usuario, string pass, string fech_exp, string email)
        {
            //string _Usuario = Session["ID_USUARIO"].ToString();

            string _Correo = email;
            string _Remitente = "infoctec@itcr.ac.cr";
            string password = "Mailctec2011";
            wsSeguridad.SeguridadSoap wsSeg = new SeguridadSoapClient();

            string _Destinatario = "";
            _Destinatario = _Correo; //"cbermudez@itcr.ac.cr"; //Acá se debe obtener el correo de la persona 
            // Validar _UserNAME Mediante el username se obtiene la cédula y luego el correo por otro método

            // wsSeguridad.ObtenerCedulaCompletedEventArgs ws = new ObtenerCedulaCompletedEventArgs();// ("rojo");
            //DataSet InfoUsuario = wsSeg.TDInformacionUsuario(cedula);
            //bool Estudiante = wsSeg.EsEstudiante("201042590");

            NetworkCredential loginInfo = new NetworkCredential(_Remitente, password);
            MailMessage msg = new MailMessage();

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            /* Este es el mensaje que llevará por defecto*/
            string _Mensaje = "";
            _Mensaje = string.Format(@" Estimado Usuario, le informamos que se a creado un Usuario para que pueda realizar reservas en nuestro Sistema. Se adjunta Resumen de los datos del usuario creado:" +
                        "<br /><br /> " +
                         "<strong>Nombre de Usuario: </strong>" + _Usuario + "<br />" +
                        "<strong>Contraseña: </strong>" + pass + "<br />" +
                        "<strong>Fecha de Expiración: </strong>" + fech_exp + "<br />" +
                        "<strong>Email: </strong>" + email + "<br />" +
                        "<br /> <br />" +
                        "Cualquier duda o consulta llamar al teléfono: 2401-3002 o responder a este correo.");

            try
            {

                msg.From = new MailAddress(_Remitente, "Sistema de Reservas de Inmuebles, TEC-SSC");
                msg.To.Add(new MailAddress(_Destinatario));
                msg.Subject = "Creación de Usuario";
                msg.Body = _Mensaje;
                msg.IsBodyHtml = true;
                smtpClient.Send(msg);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }


        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
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
                DSRIFUSUARIOS dSRIFUSUARIOS = db.DSRIFUSUARIOS.Find(id);
                if (dSRIFUSUARIOS == null)
                {
                    return HttpNotFound();
                }
                return View(dSRIFUSUARIOS);
            }
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,NOM_USUARIO,TXT_CONTRASEÑA,TXT_CORREO,TXT_ROL,FEC_EXPIRACION")] DSRIFUSUARIOS dSRIFUSUARIOS, int id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dSRIFUSUARIOS.ID_USUARIO = id;
                    db.Entry(dSRIFUSUARIOS).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dSRIFUSUARIOS);
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
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
                DSRIFUSUARIOS dSRIFUSUARIOS = db.DSRIFUSUARIOS.Find(id);
                if (dSRIFUSUARIOS == null)
                {
                    return HttpNotFound();
                }
                return View(dSRIFUSUARIOS);
            }
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DSRIFUSUARIOS dSRIFUSUARIOS = db.DSRIFUSUARIOS.Find(id);
                db.DSRIFUSUARIOS.Remove(dSRIFUSUARIOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

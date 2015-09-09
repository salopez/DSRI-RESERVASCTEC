using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datos;

namespace DSRI.Controllers
{
    public class MobiliarioReservacionController : Controller
    {
        private DSRIEntities db = new DSRIEntities();

        // GET: /MobiliarioReservacion/
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var dsritmobiliarioreservacion = db.DSRITMOBILIARIORESERVACION.Include(d => d.DSRIFMOBILIARIO).Include(d => d.DSRIFRESERVACION);
                return View(dsritmobiliarioreservacion.ToList());
            }
        }

        // GET: /MobiliarioReservacion/Details/5
        public ActionResult Details(int? id)
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
                DSRITMOBILIARIORESERVACION dsritmobiliarioreservacion = db.DSRITMOBILIARIORESERVACION.Find(id);
                if (dsritmobiliarioreservacion == null)
                {
                    return HttpNotFound();
                }
                return View(dsritmobiliarioreservacion);
            }
        }

        // GET: /MobiliarioReservacion/Create
        public ActionResult Create()
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.ID_MOBILIARIO = new SelectList(db.DSRIFMOBILIARIO, "ID_MOBILIARIO", "COD_TIPOMOBILIARIO");
                ViewBag.ID_RESERVACION = new SelectList(db.DSRIFRESERVACION, "ID_RESERVACION", "NOM_ACTIVIDAD");
                return View();
            }
        }

        // POST: /MobiliarioReservacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_MOBILIARIORESERVACION,ID_MOBILIARIO,ID_RESERVACION,CAN_DISPONIBILIDAD")] DSRITMOBILIARIORESERVACION dsritmobiliarioreservacion)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.DSRITMOBILIARIORESERVACION.Add(dsritmobiliarioreservacion);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ID_MOBILIARIO = new SelectList(db.DSRIFMOBILIARIO, "ID_MOBILIARIO", "COD_TIPOMOBILIARIO", dsritmobiliarioreservacion.ID_MOBILIARIO);
                ViewBag.ID_RESERVACION = new SelectList(db.DSRIFRESERVACION, "ID_RESERVACION", "NOM_ACTIVIDAD", dsritmobiliarioreservacion.ID_RESERVACION);
                return View(dsritmobiliarioreservacion);
            }
        }
        // GET: /MobiliarioReservacion/Edit/5
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
                DSRITMOBILIARIORESERVACION dsritmobiliarioreservacion = db.DSRITMOBILIARIORESERVACION.Find(id);
                if (dsritmobiliarioreservacion == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ID_MOBILIARIO = new SelectList(db.DSRIFMOBILIARIO, "ID_MOBILIARIO", "COD_TIPOMOBILIARIO", dsritmobiliarioreservacion.ID_MOBILIARIO);
                ViewBag.ID_RESERVACION = new SelectList(db.DSRIFRESERVACION, "ID_RESERVACION", "NOM_ACTIVIDAD", dsritmobiliarioreservacion.ID_RESERVACION);
                return View(dsritmobiliarioreservacion);
            }
        }

        // POST: /MobiliarioReservacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID_MOBILIARIORESERVACION,ID_MOBILIARIO,ID_RESERVACION,CAN_DISPONIBILIDAD")] DSRITMOBILIARIORESERVACION dsritmobiliarioreservacion)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dsritmobiliarioreservacion).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ID_MOBILIARIO = new SelectList(db.DSRIFMOBILIARIO, "ID_MOBILIARIO", "COD_TIPOMOBILIARIO", dsritmobiliarioreservacion.ID_MOBILIARIO);
                ViewBag.ID_RESERVACION = new SelectList(db.DSRIFRESERVACION, "ID_RESERVACION", "NOM_ACTIVIDAD", dsritmobiliarioreservacion.ID_RESERVACION);
                return View(dsritmobiliarioreservacion);
            }
        }

        // GET: /MobiliarioReservacion/Delete/5
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
                DSRITMOBILIARIORESERVACION dsritmobiliarioreservacion = db.DSRITMOBILIARIORESERVACION.Find(id);
                if (dsritmobiliarioreservacion == null)
                {
                    return HttpNotFound();
                }
                return View(dsritmobiliarioreservacion);
            }
        }

        // POST: /MobiliarioReservacion/Delete/5
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
                DSRITMOBILIARIORESERVACION dsritmobiliarioreservacion = db.DSRITMOBILIARIORESERVACION.Find(id);
                db.DSRITMOBILIARIORESERVACION.Remove(dsritmobiliarioreservacion);
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

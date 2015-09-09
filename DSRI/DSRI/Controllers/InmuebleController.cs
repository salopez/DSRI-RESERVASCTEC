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
    public class InmuebleController : Controller
    {
        private DSRIEntities db = new DSRIEntities();

        // GET: /Inmueble/
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var dsrifinmueble = db.DSRIFINMUEBLE.Include(d => d.DSRIFCLASIFINMUEBLE);
                return View(dsrifinmueble.ToList());
            }
        }

        // GET: /Inmueble/Details/5
        public ActionResult Details(int id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DSRIFINMUEBLE dsrifinmueble = db.DSRIFINMUEBLE.Find(id);
                if (dsrifinmueble == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifinmueble);
            }
        }

        // GET: /Inmueble/Create
        public ActionResult Create()
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.COD_CLASIFICACION = new SelectList(db.DSRIFCLASIFINMUEBLE, "COD_CLASIFINMUEBLE", "NOM_CLASIFINMUEBLE");
                return View();
            }
        }

        // POST: /Inmueble/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_INMUEBLE,COD_INMUEBLE,COD_CLASIFICACION,NOM_INMUEBLE,CAN_CAPACIDAD,COS_INMUEBLE")] DSRIFINMUEBLE dsrifinmueble)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.DSRIFINMUEBLE.Add(dsrifinmueble);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.COD_CLASIFICACION = new SelectList(db.DSRIFCLASIFINMUEBLE, "COD_CLASIFINMUEBLE", "NOM_CLASIFINMUEBLE", dsrifinmueble.COD_CLASIFICACION);
                return View(dsrifinmueble);
            }
        }

        // GET: /Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DSRIFINMUEBLE dsrifinmueble = db.DSRIFINMUEBLE.Find(id);
                if (dsrifinmueble == null)
                {
                    return HttpNotFound();
                }
                ViewBag.COD_CLASIFICACION = new SelectList(db.DSRIFCLASIFINMUEBLE, "COD_CLASIFINMUEBLE", "NOM_CLASIFINMUEBLE", dsrifinmueble.COD_CLASIFICACION);
                return View(dsrifinmueble);
            }
        }

        // POST: /Inmueble/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_INMUEBLE,COD_INMUEBLE,COD_CLASIFICACION,NOM_INMUEBLE,CAN_CAPACIDAD,COS_INMUEBLE")] DSRIFINMUEBLE dsrifinmueble)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dsrifinmueble).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.COD_CLASIFICACION = new SelectList(db.DSRIFCLASIFINMUEBLE, "COD_CLASIFINMUEBLE", "NOM_CLASIFINMUEBLE", dsrifinmueble.COD_CLASIFICACION);
                return View(dsrifinmueble);
            }
        }

        // GET: /Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DSRIFINMUEBLE dsrifinmueble = db.DSRIFINMUEBLE.Find(id);
                if (dsrifinmueble == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifinmueble);
            }
        }

        // POST: /Inmueble/Delete/5
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
                DSRIFINMUEBLE dsrifinmueble = db.DSRIFINMUEBLE.Find(id);
                db.DSRIFINMUEBLE.Remove(dsrifinmueble);
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

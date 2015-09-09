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
    public class TipoMobiliarioController : Controller
    {
        private DSRIEntities db = new DSRIEntities();

        // GET: /TipoMobiliario/
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(db.DSRIFTIPOMOBILIARIO.ToList());
            }
        }
        // GET: /TipoMobiliario/Details/5
        public ActionResult Details(string id)
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
                DSRIFTIPOMOBILIARIO dsriftipomobiliario = db.DSRIFTIPOMOBILIARIO.Find(id);
                if (dsriftipomobiliario == null)
                {
                    return HttpNotFound();
                }
                return View(dsriftipomobiliario);
            }
        }

        // GET: /TipoMobiliario/Create
        public ActionResult Create()
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        // POST: /TipoMobiliario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COD_TIPOMOBILIARIO,NOM_TIPOMOBILIARIO")] DSRIFTIPOMOBILIARIO dsriftipomobiliario)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.DSRIFTIPOMOBILIARIO.Add(dsriftipomobiliario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(dsriftipomobiliario);
            }
        }

        // GET: /TipoMobiliario/Edit/5
        public ActionResult Edit(string id)
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
                DSRIFTIPOMOBILIARIO dsriftipomobiliario = db.DSRIFTIPOMOBILIARIO.Find(id);
                if (dsriftipomobiliario == null)
                {
                    return HttpNotFound();
                }
                return View(dsriftipomobiliario);
            }
        }

        // POST: /TipoMobiliario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "COD_TIPOMOBILIARIO,NOM_TIPOMOBILIARIO")] DSRIFTIPOMOBILIARIO dsriftipomobiliario)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dsriftipomobiliario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dsriftipomobiliario);
            }
        }
        // GET: /TipoMobiliario/Delete/5
        public ActionResult Delete(string id)
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
                DSRIFTIPOMOBILIARIO dsriftipomobiliario = db.DSRIFTIPOMOBILIARIO.Find(id);
                if (dsriftipomobiliario == null)
                {
                    return HttpNotFound();
                }
                return View(dsriftipomobiliario);
            }
        }

        // POST: /TipoMobiliario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DSRIFTIPOMOBILIARIO dsriftipomobiliario = db.DSRIFTIPOMOBILIARIO.Find(id);
                db.DSRIFTIPOMOBILIARIO.Remove(dsriftipomobiliario);
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

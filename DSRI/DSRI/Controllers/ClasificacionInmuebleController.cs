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
    public class ClasificacionInmuebleController : Controller
    {
        private DSRIEntities db = new DSRIEntities();

        // GET: /ClasificacionInmueble/
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(db.DSRIFCLASIFINMUEBLE.ToList());
            }
            }

        // GET: /ClasificacionInmueble/Details/5
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
                DSRIFCLASIFINMUEBLE dsrifclasifinmueble = db.DSRIFCLASIFINMUEBLE.Find(id);
                if (dsrifclasifinmueble == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifclasifinmueble);
            }
        }

        // GET: /ClasificacionInmueble/Create
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

        // POST: /ClasificacionInmueble/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COD_CLASIFINMUEBLE,NOM_CLASIFINMUEBLE")] DSRIFCLASIFINMUEBLE dsrifclasifinmueble)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.DSRIFCLASIFINMUEBLE.Add(dsrifclasifinmueble);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(dsrifclasifinmueble);
            }
        }

        // GET: /ClasificacionInmueble/Edit/5
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
                DSRIFCLASIFINMUEBLE dsrifclasifinmueble = db.DSRIFCLASIFINMUEBLE.Find(id);
                if (dsrifclasifinmueble == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifclasifinmueble);
            }
        }

        // POST: /ClasificacionInmueble/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "COD_CLASIFINMUEBLE,NOM_CLASIFINMUEBLE")] DSRIFCLASIFINMUEBLE dsrifclasifinmueble)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dsrifclasifinmueble).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dsrifclasifinmueble);
            }
        }

        // GET: /ClasificacionInmueble/Delete/5
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
                DSRIFCLASIFINMUEBLE dsrifclasifinmueble = db.DSRIFCLASIFINMUEBLE.Find(id);
                if (dsrifclasifinmueble == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifclasifinmueble);
            }
        }

        // POST: /ClasificacionInmueble/Delete/5
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
                DSRIFCLASIFINMUEBLE dsrifclasifinmueble = db.DSRIFCLASIFINMUEBLE.Find(id);
                db.DSRIFCLASIFINMUEBLE.Remove(dsrifclasifinmueble);
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

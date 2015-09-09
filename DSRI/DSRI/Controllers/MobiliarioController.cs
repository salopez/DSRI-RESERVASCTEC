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
    public class MobiliarioController : Controller
    {
        private DSRIEntities db = new DSRIEntities();

        // GET: /Mobiliario/
        public ActionResult Index()
        {
            if (Session["COD_USUARIO"] == null)
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var dsrifmobiliario = db.DSRIFMOBILIARIO.Include(d => d.DSRIFTIPOMOBILIARIO);
                return View(dsrifmobiliario.ToList());
            }
        }

        // GET: /Mobiliario/Details/5
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
                DSRIFMOBILIARIO dsrifmobiliario = db.DSRIFMOBILIARIO.Find(id);
                if (dsrifmobiliario == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifmobiliario);
            }
        }
        // GET: /Mobiliario/Create
        public ActionResult Create()
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.COD_TIPOMOBILIARIO = new SelectList(db.DSRIFTIPOMOBILIARIO, "COD_TIPOMOBILIARIO", "NOM_TIPOMOBILIARIO");
                return View();
            }
        }

        // POST: /Mobiliario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID_MOBILIARIO,COD_TIPOMOBILIARIO,NOM_MOBILIARIO,CAN_MOBILIARIO")] DSRIFMOBILIARIO dsrifmobiliario)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.DSRIFMOBILIARIO.Add(dsrifmobiliario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.COD_TIPOMOBILIARIO = new SelectList(db.DSRIFTIPOMOBILIARIO, "COD_TIPOMOBILIARIO", "NOM_TIPOMOBILIARIO", dsrifmobiliario.COD_TIPOMOBILIARIO);
                return View(dsrifmobiliario);
            }
        }

        // GET: /Mobiliario/Edit/5
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
                DSRIFMOBILIARIO dsrifmobiliario = db.DSRIFMOBILIARIO.Find(id);
                if (dsrifmobiliario == null)
                {
                    return HttpNotFound();
                }
                ViewBag.COD_TIPOMOBILIARIO = new SelectList(db.DSRIFTIPOMOBILIARIO, "COD_TIPOMOBILIARIO", "NOM_TIPOMOBILIARIO", dsrifmobiliario.COD_TIPOMOBILIARIO);
                return View(dsrifmobiliario);
            }
        }

        // POST: /Mobiliario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID_MOBILIARIO,COD_TIPOMOBILIARIO,NOM_MOBILIARIO,CAN_MOBILIARIO")] DSRIFMOBILIARIO dsrifmobiliario)
        {
            if (Session["COD_USUARIO"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dsrifmobiliario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.COD_TIPOMOBILIARIO = new SelectList(db.DSRIFTIPOMOBILIARIO, "COD_TIPOMOBILIARIO", "NOM_TIPOMOBILIARIO", dsrifmobiliario.COD_TIPOMOBILIARIO);
                return View(dsrifmobiliario);
            }
        }

        // GET: /Mobiliario/Delete/5
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
                DSRIFMOBILIARIO dsrifmobiliario = db.DSRIFMOBILIARIO.Find(id);
                if (dsrifmobiliario == null)
                {
                    return HttpNotFound();
                }
                return View(dsrifmobiliario);
            }
        }

        // POST: /Mobiliario/Delete/5
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
                DSRIFMOBILIARIO dsrifmobiliario = db.DSRIFMOBILIARIO.Find(id);
                db.DSRIFMOBILIARIO.Remove(dsrifmobiliario);
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

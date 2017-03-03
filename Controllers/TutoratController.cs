using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tutorat.Models;

namespace Tutorat.Controllers
{
    // Attribut Authorize = http://stackoverflow.com/questions/22591252/asp-net-mvc4-restrict-all-pages-for-a-non-logged-user
    // See Yellow in answer's post
    [Authorize]
    public class TutoratController : Controller
    {
        private Ephec db = new Ephec();

        // GET: Tutorat/Demande
        // /!\ page crée mais VIDE !!
        public ActionResult Demande(int id = 1)
        {
            // Test pour la dropdown :
            ViewBag.cours = new SelectList(db.cours, "cours_id", "libelle");
            ViewBag.student = new SelectList(db.etudiant, "etudiant_id", "nom");

            var list = from m in db.cours select m;
            ViewData["CompleteList"] = list;

            var annee1 = from c in db.cours where c.annee == 1 select c.libelle;
            ViewData["annee1"] = annee1;
            var annee2 = from c in db.cours where c.annee == 2 select c.libelle;
            ViewData["annee2"] = annee2; 
            var annee3 = from c in db.cours where c.annee == 3 select c.libelle;
            ViewData["annee3"] = annee3;
            return View();
        }

        // GET: Tutorat/Explanation
        // /!\ page crée mais VIDE !!
        public ActionResult Explanation()
        {
            return View();
        }

        // GET: Tutorat/Offre
        // /!\ page crée mais VIDE !!
        public ActionResult Offre()
        {
            return View();
        }

        // GET: Tutorat/Reglement
        public ActionResult Reglement()
        {
            return View();
        }


            // GET: Tutorat
        public ActionResult Index()
        {
            return View(db.tutorat.ToList());
        }

        /* ACTUELLEMENT : UNIQUEMENT AFFICHAGE DES ONGLETS ET PAGES ASSOCIÉS (DEMANDE, EXPLANATION, OFFRE, REGLEMENT (a finir)
         * A REVOIR PLUS TARD : 
         * Creation de tutorat,
         * **/

        // GET: Tutorat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tutorat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tutorat_id,demandeur_id,cours_id,commentaire,dateResign")] tutorat tutorat)
        {
            if (ModelState.IsValid)
            {
                db.tutorat.Add(tutorat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutorat);
        }

        // GET: Tutorat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tutorat tutorat = db.tutorat.Find(id);
            if (tutorat == null)
            {
                return HttpNotFound();
            }
            return View(tutorat);
        }

        // POST: Tutorat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tutorat_id,demandeur_id,cours_id,commentaire,dateResign")] tutorat tutorat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutorat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tutorat);
        }

        // GET: Tutorat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tutorat tutorat = db.tutorat.Find(id);
            if (tutorat == null)
            {
                return HttpNotFound();
            }
            return View(tutorat);
        }

        // POST: Tutorat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tutorat tutorat = db.tutorat.Find(id);
            db.tutorat.Remove(tutorat);
            db.SaveChanges();
            return RedirectToAction("Index");
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

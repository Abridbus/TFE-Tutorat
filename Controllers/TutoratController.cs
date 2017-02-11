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
        [HttpGet]
        public ActionResult Test()
        {
            List<string> ListAnnee = new List<string>();
            ListAnnee.Add("Select");
            ListAnnee.Add("All");
            ListAnnee.Add("1");
            ListAnnee.Add("2");
            ListAnnee.Add("3");
            ListAnnee.Add("Test");

            SelectList ListAnnees = new SelectList(ListAnnee);
            ViewData["Anneess"] = ListAnnees;


            ViewBag.Annee = new List<SelectListItem> {
                new SelectListItem { Text = "All", Value = "0"},
                new SelectListItem { Text = "1ère année", Value = "1"},
                new SelectListItem { Text = "2ème année", Value = "2"},
                new SelectListItem { Text = "3ème année", Value = "3"},
             };
            var model = new demandeurCours(); 
            // Viewbad les cours pour finalement ajouter un cours (dans l'offre) ==> demandeurCours
            return View(model);
        }

        [HttpPost]
        //Apres avoir fait la requete ajax, il donne 
        public ActionResult Test(demandeurCours model)
        {
            if (ModelState.IsValid)
            {
                // code to save record  and redirect to listing page
            }
            ViewBag.Annee = new List<SelectListItem> {
                 new SelectListItem { Text = "All", Value = "0"},
                 new SelectListItem { Text = "1ère année", Value = "1"},
                 new SelectListItem { Text = "2ème année", Value = "2"},
                 new SelectListItem { Text = "3ème année", Value = "3"},
             };
            return View();
        }

        public ActionResult FillDropdown(int a)
        {
            var courss = db.cours.Where(c => c.annee == a);
            return Json(courss, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test3()
        {
            List<string> ListAnnee = new List<string>();
            ListAnnee.Add("Select");
            ListAnnee.Add("All");
            ListAnnee.Add("1");
            ListAnnee.Add("2");
            ListAnnee.Add("3");
            ListAnnee.Add("Test");

            SelectList ListAnnees = new SelectList(ListAnnee);
            ViewData["Anneess"] = ListAnnees;

            //Get the value from database and then set it to ViewBag to pass it View
            IEnumerable<SelectListItem> items = db.cours
                .Where(item => item.annee == 2)
                .Select(c => new SelectListItem
                {
                    Value = c.libelle,
                    Text = c.libelle

                });
            ViewBag.JobTitle = items;

            return View();
        }
        public ActionResult Save()
        {
            return View();
        }


        public ActionResult Test2()
        {
            List<string> ListItems = new List<string>();
            ListItems.Add("Select");
            ListItems.Add("India");
            ListItems.Add("Australia");
            ListItems.Add("America");
            ListItems.Add("North Africa");

            SelectList Countries = new SelectList(ListItems);
            ViewData["Countries"] = Countries;

            return View();
        }
        public JsonResult States(string Country)
        {
            List<string> StatesList = new List<string>();
            switch (Country)
            {
                case "India":
                    StatesList.Add("New Delhi");
                    StatesList.Add("Mumbai");
                    StatesList.Add("Kolkata");
                    StatesList.Add("Chennai");
                    break;
                case "Australia":
                    StatesList.Add("Canberra");
                    StatesList.Add("Melbourne");
                    StatesList.Add("Perth");
                    StatesList.Add("Sydney");
                    break;
                case "America":
                    StatesList.Add("California");
                    StatesList.Add("Florida");
                    StatesList.Add("New York");
                    StatesList.Add("Washignton");
                    break;
                case "North Africa":
                    StatesList.Add("Tunisia");
                    StatesList.Add("Libya");
                    StatesList.Add("Morocco");
                    StatesList.Add("Sudan");
                    break;
            }
            return Json(StatesList,JsonRequestBehavior.AllowGet);
        }

        public JsonResult EnvoieLibelles(string a)
        {
            List<string> libelles = new List<string>();
            switch (a)
            {
                case "All":
                    IEnumerable<string> IEnumAll = from s in db.cours
                                                   select s.libelle;
                    libelles = IEnumAll.ToList();
                    break;
                case "1":
                    IEnumerable<string> IEnumOne = from s in db.cours
                                               where s.annee == 1
                                               select s.libelle;
                    libelles = IEnumOne.ToList();
                    break;
                case "2":
                    IEnumerable<string> IEnumTwo = from s in db.cours
                                                   where s.annee == 2
                                                   select s.libelle;
                    libelles = IEnumTwo.ToList();
                    break;
                case "3":
                    IEnumerable<string> IEnumThree = from s in db.cours
                                                   where s.annee == 3
                                                   select s.libelle;
                    libelles = IEnumThree.ToList();
                    break;
                /*case "Test":
                    IEnumerable<string>  list = from s in db.cours
                                where s.annee == 2
                                select s.libelle;
                    List<string> asList = list.ToList();
                    libelles = asList;
                    break;
                    */

            }
            return Json(libelles, JsonRequestBehavior.AllowGet);

        }
        /** SEE LATER FOR OTHER

        [HttpGet]
        public ActionResult Test()
        {
            ViewBag.CoursList = db.cours;
            // Viewbad les cours pour finalement ajouter un cours (dans l'offre) ==> demandeurCours
            return View();
        }
        [HttpPost]
        public ActionResult Test(demandeurCours co)
        {
            if (ModelState.IsValid)
            {
                // code to save record  and redirect to listing page
            }

            return View(co);
        }**/


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

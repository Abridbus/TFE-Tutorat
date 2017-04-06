using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tutorat.Models;

namespace Tutorat.Controllers
{
    // Attribut Authorize = http://stackoverflow.com/questions/22591252/asp-net-mvc4-restrict-all-pages-for-a-non-logged-user
    // See Yellow in answer's post
    [Authorize]
    public class TutoratController : DefautController // extend pour accéder aux fonction de DefautController
    {
        private Ephec db = new Ephec();
        private EphecTemp bddtemp = new EphecTemp();


        // GET: Tutorat/Demande
        // /!\ page crée mais VIDE !!
        public ActionResult Demande()
        {
            int id = getIdEtudiant();
            //Ajouter la vérification des tutorats :
            // Si le tutorat existe deja, ne pas l'afficher (Comment gérer la réouverture par Mme Alen??)
            //bool i = bddtemp.demandeurtmp.Where(d => d.demandeurCourstmp.);
            

            //Affiche les cours du Bloc 1 auxquels l'étudiant est inscrit cette année !!
            var coursEtu = db.etudiant
                            .Single(e => e.etudiant_id == id)
                            .cours.Where(c => c.annee <= 1).Select(c => c);
            foreach(var etu in coursEtu)
            {
                // Vérifier si existe
                // bddtemp.demandeurtmp.Where(d => d.demandeurCourstmp.);
            }
            //List<string> list = 
            List<cours> list = coursEtu.ToList();
            //Ajout des cours "Table de conversation", "remédiation Fr", "Extra académiques"    (pour l'exemple je n'en ai ajouté qu'un des 3)
            list.Add(new cours { libelle = "Table de Conversation", cours_id = 1002, annee=1, code="HELP01", section="TI" });
            ViewBag.listCoursDemande = list;
            return View();
        }

        [HttpPost]
        public ActionResult Demande(listeDemandeTmp model)
        {

            for (int i = 0; i < model.Items.Count(); i++)
            {
                if (model.Items[i].select == true)
                {
                    // OBLIGER DE CREER LES OBJETS ICI  !!! 
                    /* This problem happens because you are referencing the same object more than once. This is not a limitation of EF, but rather a safety feature to ensure you are not inserting the same object with two different IDs. 
                     * So to achieve what you are trying to do, is simply create a new object and add the newly created object to the database.
                     ** This issue often happens inside loops. If you are using a while or foreach loop, make sure to have the New Created Object INSIDE the loop body.
                     * */
                    demandeurtmp dtemp = new demandeurtmp();
                    demandeurCourstmp dctemp = new demandeurCourstmp();

                    int demandeurID = bddtemp.demandeurtmp.Max(u => u.demandeur_id);

                    model.Items[i].matricule = Session["matricule"].ToString();
                    Session["EnDemande"] = true;
                    dtemp.matricule = model.Items[i].matricule;
                    dtemp.dateInsc = DateTime.Now;
                    dtemp.demandeur_id = demandeurID + 1;

                    dctemp.demandeur_id = dtemp.demandeur_id;
                    dctemp.cours_id = model.Items[i].cours_id;
                    dctemp.commentaire = model.Items[i].commentaire;
                    dctemp.matriculeTuteurPref = model.Items[i].matriculeTuteurPref; // Vérifier si marchera si il est vide ! (normalement oui !!)
                    enregDemande(dtemp, dctemp);

                }

            } 
            
            return RedirectToAction("Index", "Home");
        }

        public void enregDemande(demandeurtmp d, demandeurCourstmp dc)
        {
            bddtemp.demandeurtmp.Add(d);
            bddtemp.demandeurCourstmp.Add(dc);
            //SaveChanges ici pour qu'il enregistre chaque item sélectionné
            //bddtemp.SaveChangesAsync();
            bddtemp.SaveChanges();
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
            int id = getIdEtudiant();
            //Affichage des cours auxquels l'étudiant est inscrit cette année et ou il a une cote supérieure à 10 !!
            var coursEtu =
                from e in db.etudiant
                from er in db.etuResult
                from c in db.cours
                where e.etudiant_id == er.etudiant_id
                && c.cours_id == er.cours_id
                && e.etudiant_id == id
                && er.cote >= 10
                select new resultCours
                {
                    libelle = c.libelle,
                    cours_id = c.cours_id,
                    annee = c.annee,
                    code = c.code,
                    cote = er.cote
                };
            //List<string> list = 
            List<resultCours> list = coursEtu.ToList();
            ViewBag.listCoursOffre = coursEtu; //list;
            return View();
        }

        [HttpPost]
        public ActionResult Offre(listeTuteurTmp model)
        {
            for (int i = 0; i < model.Items.Count(); i++)
            {
                if (model.Items[i].select == true)
                {
                    tuteurtmp ttemp = new tuteurtmp();
                    tuteurCourstmp tctemp = new tuteurCourstmp();

                    int tuteurID = bddtemp.tuteurtmp.Max(u => u.tuteur_id);

                    model.Items[i].matricule = Session["matricule"].ToString();
                    Session["EnOffre"] = true;
                    ttemp.matricule = model.Items[i].matricule;
                    ttemp.dateInsc = DateTime.Now;
                    ttemp.tuteur_id = tuteurID + 1;

                    tctemp.tuteur_id = ttemp.tuteur_id;
                    tctemp.cours_id = model.Items[i].cours_id;
                    tctemp.commentaire = model.Items[i].commentaire;
                    enregTuteur(ttemp, tctemp);
                }

            }

            return RedirectToAction("Index", "Home");
        }

        public void enregTuteur(tuteurtmp t, tuteurCourstmp tc)
        {
            bddtemp.tuteurtmp.Add(t);
            bddtemp.tuteurCourstmp.Add(tc);
            //SaveChanges ici pour qu'il enregistre chaque item sélectionné
            //bddtemp.SaveChangesAsync();
            bddtemp.SaveChanges();
        }

        // GET: Tutorat/Reglement
        public ActionResult Reglement()
        {
            return View();
        }

        public ActionResult MesTutorats()
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

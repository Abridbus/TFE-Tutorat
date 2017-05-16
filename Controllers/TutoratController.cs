using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
            var coursEtu =
                 from e in db.etudiant
                 from er in db.etuResult
                 from c in db.cours
                 where e.etudiant_id == er.etudiant_id
                 && c.cours_id == er.cours_id
                 && e.etudiant_id == id
                 && er.cote < 10
                 select new resultCours
                 {
                     libelle = c.libelle,
                     cours_id = c.cours_id,
                     annee = c.annee,
                     code = c.code,
                     cote = er.cote
                 };
            List<resultCours> list = coursEtu.ToList();
            //Ajout des cours "Table de conversation", "remédiation Fr", "Extra académiques"    (pour l'exemple je n'en ai ajouté qu'un des 3)
            list.Add(new resultCours { libelle = "Table de Conversation", cours_id = 1002, annee=1, code="HELP01"});
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

                    int demandeurID;
                    // Si table est vide : tuteurID = 0, sinon retourne le tuteur_id le plus grand
                    if (!bddtemp.demandeurtmp.Any())
                    {
                        demandeurID = 0;
                    }
                    else
                    {
                        demandeurID = bddtemp.demandeurtmp.Max(u => u.demandeur_id);
                    }

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
            bddtemp.SaveChanges();
        }

        // GET: Tutorat/Explanation
        // /!\ page crée mais VIDE !!
        public ActionResult Explication()
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

                    int tuteurID;
                    // Si table est vide : tuteurID = 0, sinon retourne le tuteur_id le plus grand
                    if (!bddtemp.tuteurtmp.Any())
                    {
                        tuteurID = 0;
                    } else
                    {
                        tuteurID = bddtemp.tuteurtmp.Max(u => u.tuteur_id);
                    }

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
            bddtemp.SaveChanges();
        }

        // GET: Tutorat/Reglement
        public ActionResult Reglement()
        {
            return View();
        }

        public List<tutoratInfos> ViewBagPrestations()
        {
            int[] idTutorat = getIdTutorat(getMatricule());
            List<tutoratInfos> tutInfo = new List<tutoratInfos>(idTutorat.Count());
            for (int i = 0; i < idTutorat.Count(); i++)
            {
                int j = idTutorat[i];
                int id = (from t in db.tutorat where t.tutorat_id == j select t.cours_id).First();
                string lib = (from c in db.cours from t in db.tutorat where t.tutorat_id == j && t.cours_id == c.cours_id select c.libelle).FirstOrDefault();
                int idTuteur = (from t in db.tutorat where t.tutorat_id == j select t.tuteur_id).First();
                int dureeRestante = (from t in db.tutorat where t.tutorat_id == j select t.tempsTotal).First();
                // En BDD est stocké le temps total RESTANT. On affiche le temps RESTANT !
                if (dureeRestante != 0)
                {
                    tutInfo.Add(new tutoratInfos { tutorat_id = j, tuteur_id = idTuteur, dureeRestante = dureeRestante, cours_id = id, cours_libelle = lib });
                }
            };
            return(tutInfo);
        }

        public ActionResult MesPrestations()
        {
            //Tableau de int car un étudiant peut avoir plusieurs tutorats

            ViewBag.infos = ViewBagPrestations();
            //Envoyer à la View une liste de model prestation
            return View();
        }

        [HttpPost]
        public ActionResult MesPrestations(listePrestationtmp prtmp)
        {
            // Problème 1 : On ne peut pas required les champs (sinon on force les deux encodages) => L'utilisateur peut donc ne pas rentrer de données... PROBLEME !!
            ViewBag.infos = ViewBagPrestations();

            //Continue avec problème non résolu :(
            foreach (var i in prtmp.Items)
            {
                //Retirer cette vérif si problème résolu :
                if(i.compteRendu != null &&  i.dureePrestation != 0 && i.tuteur_id != 0 && i.tutorat_id != 0)
                {
                    int sum = (bddtemp.prestationtmp.Where(x => x.tuteur_id == i.tuteur_id && x.tutorat_id == i.tutorat_id && x.datePrestation == i.datePrestation)
                        .Select(x => x.dureePrestation)
                        .DefaultIfEmpty(0)
                        .Sum()) + i.dureePrestation;
                    bddtemp.prestationtmp.Add(i);
                    if (sum < 121) {
                      bddtemp.SaveChanges();
                    }
                    //Releasing "unmanaged" resources
                    bddtemp.Dispose();
                }

            }
            return RedirectToAction("Index", "Home");
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
            // TO DELETE
            return View();
        }

        // POST: Tutorat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tutorat_id,demandeur_id,cours_id,commentaire,dateResign")] tutorat tutorat)
        {
            // TO DELETE : raison : la creation d'un tutorat ne suffit pas ! (il faut créer aussi les demandeurs et tuteurs)
            if (ModelState.IsValid)
            {
                db.tutorat.Add(tutorat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutorat);
        }

        // GET: Tutorat/Delete/5
        public ActionResult Delete(int? id)
        {
            // TO DELETE
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

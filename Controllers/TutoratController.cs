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

        /// <summary>
        /// Demande de tutorat :
        /// Affiche une liste de cours auxquels l'étudiant connecté peut s'inscrire
        /// </summary>
        /// <returns> Une vue </returns>
        // GET: Tutorat/Demande
        public ActionResult Demande()
        {
            int id = getIdEtudiant();

            //Tous les cours de l'étudiant
            var coursEtu =
                 db.etudiant
                    .Single(e => e.etudiant_id == id)
                    .cours.Select(c => new resultCours
                    {
                        libelle = c.libelle,
                        cours_id = c.cours_id,
                        annee = c.annee,
                        code = c.code

                    }).ToList();

            //Les cours ou l'étudiant à des résultats
            var coursEtuResult = (from e in db.etudiant
                                 from er in db.etuResult
                                 from c in db.cours
                                 where e.etudiant_id == er.etudiant_id
                                 && c.cours_id == er.cours_id
                                 && e.etudiant_id == id
                                 && er.cote > 9
                                  select (new resultCours
                                 {
                                     libelle = c.libelle,
                                     cours_id = c.cours_id,
                                     annee = c.annee,
                                     code = c.code

                                 })).ToList();

            //Tous les cours de l'étudiant - les cours ou l'étudiant à des résultats > 9
            var result = coursEtu.Where(p => !coursEtuResult.Any(x => x.cours_id == p.cours_id)).ToList();

            //Ajout des cours "Table de conversation", "remédiation Fr", "Extra académiques"    (pour l'exemple je n'en ai ajouté qu'un des 3)
            //result.Add(new resultCours { libelle = "Table de Conversation", cours_id = 1002, annee=1, code="HELP01"});
            ViewBag.listCoursDemande = result;
            return View();
        }

        /// <summary>
        /// Gestion des demandes de l'étudiant : envoie les cours sélectionnés en BDDtemp
        /// </summary>
        /// <param name="model"> La liste de cours </param>
        /// <returns> Redirige à l'index </returns>
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

                    model.Items[i].matricule = getMatricule();
                    Session["EnDemande"] = true;
                    dtemp.matricule = model.Items[i].matricule;
                    dtemp.dateInsc = DateTime.Now;
                    dtemp.demandeur_id = demandeurID + 1;

                    dctemp.demandeur_id = dtemp.demandeur_id;
                    dctemp.cours_id = model.Items[i].cours_id;
                    dctemp.commentaire = model.Items[i].commentaire;
                    dctemp.matriculeTuteurPref = model.Items[i].matriculeTuteurPref;
                    enregDemande(dtemp, dctemp);
                }

            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Ajout des demandeurstmp et demandeurCourstmp en BDDtmp + sauvegarde de la BDDtmp
        /// </summary>
        /// <param name="d"> demandeurtmp </param>
        /// <param name="dc"> demandeurCourstmp</param>
        public void enregDemande(demandeurtmp d, demandeurCourstmp dc)
        {
            bddtemp.demandeurtmp.Add(d);
            bddtemp.demandeurCourstmp.Add(dc);
            bddtemp.SaveChanges();

        }

        /// <summary>
        /// Affiche la page d'explication des tutorats
        /// </summary>
        /// <returns> Une vue </returns>
        // GET: Tutorat/Explanation
        public ActionResult Explications()
        {
            return View();
        }

        /// <summary>
        /// Offre de tutorat :
        /// Affiche une liste de cours auxquels l'étudiant connecté peut s'inscrire
        /// </summary>
        /// <returns> Une vue </returns>
        // GET: Tutorat/Offre
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
                && er.cote >= 14
                select new resultCours
                {
                    libelle = c.libelle,
                    cours_id = c.cours_id,
                    annee = c.annee,
                    code = c.code,
                    cote = er.cote
                };
            List<resultCours> list = coursEtu.ToList();

            string matricule = getMatricule();
            //TuteurIdTmp : Voir si l'étudiant est deja en Offre de tutorat dans la bdd temp :
            int[] tuteuridstmp = bddtemp.tuteurtmp.Where(t => t.matricule == matricule).Select(t => t.tuteur_id).ToArray();
            int[] tuteurids = bddtemp.tuteurtmp.Where(t => t.matricule == matricule).Select(t => t.tuteur_id).ToArray();
            
            //Vérif : si l'étudiant est déjà en Offre de tutorat (mais pas encore de tutorat trouvé) : 
            ViewBag.listCoursOffre = coursEtu; 

            if(tuteuridstmp.Length != 0 || tuteurids.Length != 0)
            { ViewBag.DejaTuteur = true; } else { ViewBag.DejaTuteur = false; }

            return View();
        }

        /// <summary>
        /// Gestion des demandes de l'étudiant : envoie les cours sélectionnés en BDDtemp
        /// </summary>
        /// <param name="model"> La liste de cours </param>
        /// <returns> Redirige à l'index </returns>
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

                    model.Items[i].matricule = getMatricule();
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



        /// <summary>
        /// Ajout des tuteurtmp et tuteurCourstmp en BDDtmp + sauvegarde de la BDDtmp
        /// </summary>
        /// <param name="t"> tuteurtmp </param>
        /// <param name="tc"> tuteurCourstmp</param>
        public void enregTuteur(tuteurtmp t, tuteurCourstmp tc)
        {
            bddtemp.tuteurtmp.Add(t);
            bddtemp.tuteurCourstmp.Add(tc);
            bddtemp.SaveChanges();
        }

        /// <summary>
        /// Affichage du guide pratique
        /// </summary>
        /// <returns> Une Vue </returns>
        // GET: Tutorat/GuidePratique
        public ActionResult GuidePratique()
        {
            return View();
        }

        /// <summary>
        /// Créée une liste pour l'affichage des prestations
        /// </summary>
        /// <returns> La liste pour le ViewBag</returns>
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
                int idDemandeur = (from t in db.tutorat where t.tutorat_id == j select t.demandeur_id).First();
                string matriculeDem = (from d in db.demandeur where d.demandeur_id == idDemandeur select d.matricule).First();
                string nomPrenomDem = (from e in db.etudiant where e.matricule == matriculeDem select e.nom + " " + e.prenom).First();
                // En BDD est stocké le temps total RESTANT. On affiche le temps RESTANT !
                if (dureeRestante != 0)
                {
                    tutInfo.Add(new tutoratInfos { tutorat_id = j, tuteur_id = idTuteur, matriculeDemandeur = matriculeDem, nomPrenomDemandeur = nomPrenomDem, dureeRestante = dureeRestante, cours_id = id, cours_libelle = lib });
                }
            };
            return(tutInfo);
        }

        /// <summary>
        /// Encodage des Prestations [GET]
        /// </summary>
        /// <returns>
        /// Affiche la vue
        /// </returns>
        public ActionResult MesPrestations()
        {
            //Tableau de int car un étudiant peut avoir plusieurs tutorats
            ViewBag.infos = ViewBagPrestations();

            return View();
        }

        /// <summary>
        /// Récupère les prestations complétés (1 par cours maximum)
        /// </summary>
        /// <param name="prtmp"> Liste de prestations </param>
        /// <returns> Redirige à l'index </returns>
        [HttpPost]
        public ActionResult MesPrestations(listePrestationtmp prtmp)
        {
            ViewBag.infos = ViewBagPrestations();

            foreach (var i in prtmp.Items)
            {
                if (i.compteRendu != null && i.dureePrestation != 0 && i.tuteur_id != 0 && i.tutorat_id != 0)
                {
                    int sum = (bddtemp.prestationtmp.Where(x => x.tuteur_id == i.tuteur_id && x.tutorat_id == i.tutorat_id && x.datePrestation == i.datePrestation)
                        .Select(x => x.dureePrestation)
                        .DefaultIfEmpty(0)
                        .Sum()) + i.dureePrestation;
                    bddtemp.prestationtmp.Add(i);
                    if (sum < 181)
                    {
                        bddtemp.SaveChanges();
                        ViewData["save"] = "Données enregistrées !";
                    }
                    else { ViewData["save"] = "Erreur : Durées maximales autorisées. "; }

                }
                else { ViewData["save"] = "Erreur : Données manquantes ! Veuillez renseigner tous les champs."; }

            }
            bddtemp.Dispose();
            return View();// RedirectToAction("Index", "Home");
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

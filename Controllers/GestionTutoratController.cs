using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tutorat.Models;

namespace Tutorat.Controllers
{
    [Authorize(Roles = "ConseilSocial")]
    public class GestionTutoratController : DefautController
    {
        private Ephec bdd = new Ephec();
        private EphecTemp bddtemp = new EphecTemp();

        private IEnumerable<gestionTutorat> Affichage()
        {
            IEnumerable<gestionTutorat> demandesTemp = (from dtemp in bddtemp.demandeurtmp
                                                        from dctemp in bddtemp.demandeurCourstmp
                                                        where dtemp.demandeur_id == dctemp.demandeur_id
                                                        select new gestionTutorat
                                                        {
                                                            cours_id = dctemp.cours_id,
                                                            matricule = dtemp.matricule,
                                                            commentaire = dctemp.commentaire,
                                                            dateInsc = dtemp.dateInsc,
                                                            matriculeTuteurPref = dctemp.matriculeTuteurPref
                                                        }).ToList();

            foreach (var result in demandesTemp)
            {
                result.nom = (from e in bdd.etudiant where e.matricule == result.matricule select e.nom).Single();
                result.prenom = (from e in bdd.etudiant where e.matricule == result.matricule select e.prenom).First();
                result.cours_libelle = (from c in bdd.cours where c.cours_id == result.cours_id select c.libelle).First();

            };
            return demandesTemp;
        }
        // GET: GestionTutorat
        public ActionResult DemandesTutorat()
        {
            /* Query should looks like this : Mais en changeant le fait que des data viennent d'une autre BDD !! (demandeur et demandeurCours)
             * 
             * from d in demandeur
                    select new {
	                    matricule = d.matricule,
	                    nom = (from e in etudiant where e.matricule == d.matricule select e.nom),
	                    prenom = (from e in etudiant where e.matricule == d.matricule select e.prenom),
	                    cours_id = (from dct in demandeurCours where dct.demandeur_id == d.demandeur_id select dct.cours_id),
	                    cours_libelle = (from c in cours from dct in demandeurCours where dct.cours_id == c.cours_id select c.libelle),
	                    commentaire = (from dct in demandeurCours where dct.demandeur_id == d.demandeur_id select dct.commentaire),
	                    matriculeTuteurPref = (from dct in demandeurCours where dct.demandeur_id == d.demandeur_id select dct.matriculeTuteurPref),
	                    dateInsc = d.dateInsc
                    }
             **/

            ViewBag.valueDropDown = GenereCours();

            List<SelectListItem> DDRecherche = new List<SelectListItem>();
                DDRecherche.Add(new SelectListItem
                {
                    Text = "Tous les résultats",
                    Value = "0",
                    Selected = true
                });
                DDRecherche.Add(new SelectListItem
                {
                    Text = "Libellé des cours",
                    Value = "1"
                });
                DDRecherche.Add(new SelectListItem
                {
                    Text = "Nom & prénom de l'étudiant demandeur",
                    Value = "2"
                });
                DDRecherche.Add(new SelectListItem
                {
                    Text = "Matricule de l'étudiant demandeur",
                    Value = "3"
                });
            ViewBag.DDRecherche = DDRecherche;
            return View(Affichage());
        }

        [HttpPost]
        public ActionResult DemandesTutorat(int valueDropDown)
        {
            /* Query should looks like this : Mais en changeant le fait que des data viennent d'une autre BDD !! (demandeur et demandeurCours)
             * 
             * from d in demandeur
                    select new {
	                    matricule = d.matricule,
	                    nom = (from e in etudiant where e.matricule == d.matricule select e.nom),
	                    prenom = (from e in etudiant where e.matricule == d.matricule select e.prenom),
	                    cours_id = (from dct in demandeurCours where dct.demandeur_id == d.demandeur_id select dct.cours_id),
	                    cours_libelle = (from c in cours from dct in demandeurCours where dct.cours_id == c.cours_id select c.libelle),
	                    commentaire = (from dct in demandeurCours where dct.demandeur_id == d.demandeur_id select dct.commentaire),
	                    matriculeTuteurPref = (from dct in demandeurCours where dct.demandeur_id == d.demandeur_id select dct.matriculeTuteurPref),
	                    dateInsc = d.dateInsc
                    }
             **/

            ViewBag.valueDropDown = GenereCours();

            List<SelectListItem> DDRecherche = new List<SelectListItem>();
            DDRecherche.Add(new SelectListItem
            {
                Text = "Tous les résultats",
                Value = "0",
                Selected = true
            });
            DDRecherche.Add(new SelectListItem
            {
                Text = "Libellé des cours",
                Value = "1"
            });
            DDRecherche.Add(new SelectListItem
            {
                Text = "Nom & prénom de l'étudiant demandeur",
                Value = "2"
            });
            DDRecherche.Add(new SelectListItem
            {
                Text = "Matricule de l'étudiant demandeur",
                Value = "3"
            });
            ViewBag.DDRecherche = DDRecherche;

            IEnumerable<gestionTutorat> demandesTemp = Affichage();

            if (valueDropDown != 0)
            {
                demandesTemp = demandesTemp.Where(s=> s.annee <= 1 && s.cours_id == valueDropDown);
            }

            return View(demandesTemp);
        }

        public ActionResult ListeDemande()
        {

            return View();
        }

        public ActionResult HistoriqueDemandes()
        {
            /* Création d'une page d'historique des demandes : 
             *      Une fois les demandes gérées (mis en contact d'un tutoré et d'un tutorant), 
             *      les demandeurs et tuteurs sont mis dans la BDD EPHEC.
             *      Madame Alen va se rendre sur cette page pour augmenter la durée d'un tutorat (par exemple) ou revisionner les tutorats acceptés
             *      */

            return View();
        }
    }
}
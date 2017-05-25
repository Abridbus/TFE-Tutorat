using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tutorat.Models;


namespace Tutorat.Controllers
{
    public class DefautController : Controller
    {
        private Ephec db = new Ephec();
        private EphecTemp bddtemp = new EphecTemp();
        public int getIdEtudiant()
        {
            int id;
            if (Session["id"] != null)
            {
                id = (int)Session["id"];
            }
            else
            {
                etudiant etuCo = db.etudiant.FirstOrDefault(u => u.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name));
                id = etuCo.etudiant_id;
            }
            return id;

        }

        public string getMatricule()
        {
            string matricule = (from t in db.etudiant where t.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name) select t.matricule).First();
            return matricule;
        }

        // Ne va sélectionner  les tuteur_id que pour l'étudiant connecté?
        public int[] getIdTuteur(int idEtudiant)
        {
            string matricule = (from e in db.etudiant where e.etudiant_id == idEtudiant select e.matricule).First().ToString();
            int[] tuteur_id = (from t in db.tuteur where t.matricule == matricule select t.tuteur_id).ToArray();

            return tuteur_id;

        }
        public int[] getIdTutorat(string matricule)
        {
            int[] tutorat_id = (from t in db.tuteur from tc in db.tuteurCours where t.tuteur_id == tc.tuteur_id && t.matricule == matricule select tc.tutorat_id).ToArray();
            return tutorat_id;
        }

        public void setSessionEtudiant(string mail)
        {
            etudiant etu = db.etudiant.FirstOrDefault(u => u.mail.Contains(mail));
            Session["id"] = etu.etudiant_id;
            Session["prenom"] = etu.prenom;
            Session["nom"] = etu.nom;
            Session["matricule"] = etu.matricule;

            //Si l'étudiant est inscrit à des cours, cette var aura le nombre de cours inscrit comme valeur
            Session["nbCoursInscrit"] = etu.cours.Count();

        }

        //Renvoie un objet IEnumerable pour faire un dropdown (menu déroulant) de cours
        //Valeur de l'<option> : l'id du cours, texte affiché de l'<option> : le libellé du cours
        // Utilisé dans EpersoController & TutoratController
        public IEnumerable<SelectListItem> GenereCours()
        {
            IEnumerable<SelectListItem> items;
            var bdd = new Ephec();

            items = bdd.cours
                .Where(c => c.annee <= 1)
                .Select(c => new SelectListItem
                {
                    Value = c.cours_id.ToString(), // La valeur dans le dropdown sera l'id du cours (typecast forcé)
                    Text = c.libelle // La valeur affichée dans le dropdown sera le libellé du cours
                });


            return items;
        }

        /// <summary>
        /// Pour la GestionTutoratController
        /// </summary>
        /// <returns></returns>
        ///        
        private IEnumerable<gestionTutorat> InfosDemandeurs(int coursID)
        {
            if (coursID != 0)
            {
                IEnumerable<gestionTutorat> demandesTemp = (
                    from dtemp in bddtemp.demandeurtmp
                    from dctemp in bddtemp.demandeurCourstmp
                    where dtemp.demandeur_id == dctemp.demandeur_id
                    && dctemp.cours_id == coursID
                    select new gestionTutorat
                    {
                        demandeur_id = dtemp.demandeur_id,
                        cours_id = dctemp.cours_id,
                        matricule = dtemp.matricule,
                        commentaire = dctemp.commentaire,
                        dateInsc = dtemp.dateInsc,
                        matriculeTuteurPref = dctemp.matriculeTuteurPref
                    }).ToList();

                foreach (var result in demandesTemp)
                {
                    result.etudiant_id = (from e in db.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                    result.nom = (from e in db.etudiant where e.matricule == result.matricule select e.nom).Single();
                    result.prenom = (from e in db.etudiant where e.matricule == result.matricule select e.prenom).First();
                    result.cours_libelle = (from c in db.cours where c.cours_id == result.cours_id select c.libelle).First();
                    result.annee = (from e in db.etudiant where e.matricule == result.matricule select e.annee).First();

                };
                return demandesTemp;
            }

            IEnumerable<gestionTutorat> demandesTemp2 = (
                from dtemp in bddtemp.demandeurtmp
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

            foreach (var result in demandesTemp2)
            {
                result.etudiant_id = (from e in db.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                result.nom = (from e in db.etudiant where e.matricule == result.matricule select e.nom).Single();
                result.prenom = (from e in db.etudiant where e.matricule == result.matricule select e.prenom).First();
                result.cours_libelle = (from c in db.cours where c.cours_id == result.cours_id select c.libelle).First();
                result.annee = (from e in db.etudiant where e.matricule == result.matricule select e.annee).First();
            };
            return demandesTemp2;
        }

        private IEnumerable<gestionTutorat> InfosTuteurs(int coursID)
        {
            // Utilisation de gestionTutorat, dans ce cas-ci, pas de matriculeTuteurPref (le laisser à null)
            // Mais uilisation de cote, qui représente la cote pour ce cours.

            if (coursID != 0)
            {
                IEnumerable<gestionTutorat> tuteurTemp =
                    (from ttemp in bddtemp.tuteurtmp
                     from tctemp in bddtemp.tuteurCourstmp
                     where ttemp.tuteur_id == tctemp.tuteur_id
                 && tctemp.cours_id == coursID
                     select new gestionTutorat
                     {
                         tuteur_id = ttemp.tuteur_id,
                         cours_id = tctemp.cours_id,
                         matricule = ttemp.matricule,
                         commentaire = tctemp.commentaire,
                         dateInsc = ttemp.dateInsc
                     }).ToList();
                foreach (var result in tuteurTemp)
                {
                    result.etudiant_id = (from e in db.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                    result.nom = (from e in db.etudiant where e.matricule == result.matricule select e.nom).Single();
                    result.prenom = (from e in db.etudiant where e.matricule == result.matricule select e.prenom).First();
                    result.cours_libelle = (from c in db.cours where c.cours_id == result.cours_id select c.libelle).First();
                    result.cote = (from er in db.etuResult where er.etudiant_id == result.etudiant_id select er.cote).First();
                };
                return tuteurTemp;
            }
            IEnumerable<gestionTutorat> tuteurTemp2 =
                (from ttemp in bddtemp.tuteurtmp
                 from tctemp in bddtemp.tuteurCourstmp
                 where ttemp.tuteur_id == tctemp.tuteur_id
                 select new gestionTutorat
                 {
                     cours_id = tctemp.cours_id,
                     matricule = ttemp.matricule,
                     commentaire = tctemp.commentaire,
                     dateInsc = ttemp.dateInsc
                 }).ToList();
            foreach (var result in tuteurTemp2)
            {
                result.etudiant_id = (from e in db.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                result.nom = (from e in db.etudiant where e.matricule == result.matricule select e.nom).Single();
                result.prenom = (from e in db.etudiant where e.matricule == result.matricule select e.prenom).First();
                result.cours_libelle = (from c in db.cours where c.cours_id == result.cours_id select c.libelle).First();
                result.cote = (from er in db.etuResult where er.etudiant_id == result.etudiant_id && er.cours_id == result.cours_id select er.cote).First();
            };
            return tuteurTemp2;
        }
    }
}
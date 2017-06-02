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

        /// <summary>
        ///  Récupère en BDD l'étudiant ID depuis le mail et l'ajoute à la variable Session
        /// </summary>
        /// <returns> intEtudiant ID </returns>
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

        /// <summary>
        /// Récupère en BDD le matricule depuis le mail de l'étudiant
        /// </summary>
        /// <returns> String matricule </returns>
        public string getMatricule()
        {
            string matricule = (from t in db.etudiant where t.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name) select t.matricule).First();
            return matricule;
        }

        /// <summary>
        /// Un étudiant ayant un seul matricule peut avoir plusieurs tuteurs_id
        /// </summary>
        /// <param name="matricule"> Matricule du tuteur</param>
        /// <returns> Table int de tutorat_id </returns>
        public int[] getIdTutorat(string matricule)
        {
            int[] tutorat_id = (from t in db.tuteur from tc in db.tuteurCours where t.tuteur_id == tc.tuteur_id && t.matricule == matricule select tc.tutorat_id).ToArray();
            return tutorat_id;
        }

        /// <summary>
        /// Créée le variable Session comprenant les infos de l'étudiant connecté
        /// </summary>
        /// <param name="mail"> Mail de l'étudiant connecté</param>
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

        /// <summary>
        ///  Utilisé dans EpersoController & TutoratController pour générer des listes de cours
        /// </summary>
        /// <returns> IEnumerable pour faire un dropdown (menu déroulant) de cours </returns>
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
    }
}
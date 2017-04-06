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
                .Select(c => new SelectListItem
                {
                    Value = c.cours_id.ToString(), // La valeur dans le dropdown sera l'id du cours (typecast forcé)
                    Text = c.libelle // La valeur affichée dans le dropdown sera le libellé du cours
                });

            return items;
        }
    }
}
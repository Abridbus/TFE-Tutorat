using Microsoft.AspNet.Identity;
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
    // Voir partie Jaune du post
    [Authorize]
    public class EpersoController : DefautController
    {
        private Ephec bdd = new Ephec();

        /// <summary>
        /// Contient les informations de l'étudiant récupérées en BDD depuis le mail
        /// </summary>
        /// <returns> Affiche la page info </returns>
        public ActionResult Info()
        {
            etudiant InfoEtudiant = bdd.etudiant.FirstOrDefault(u => u.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name));
            return View(InfoEtudiant);
        }

        /// <summary>
        ///  Affiche la page Cours si l'étudiant n'a pas de cours OU CoursInscrits si il en a
        /// </summary>
        /// <returns> Affiche la page en fonction de la variable session créée à la connexion</returns>
        public ActionResult Cours()
        {
           
            ViewBag.DDCours = GenereCours();

            if ((int)Session["nbCoursInscrit"] != 0)
            {
                return RedirectToAction("CoursInscrits");
            }
            //Sinon remplir sa liste de cours & résultats.
            return View();
        }

        /// <summary>
        /// Tableau des cours et résultats de l'étudiant
        /// </summary>
        /// <returns> Affiche la page avec la liste des cours de l'étudiant</returns>
        public ActionResult CoursInscrits()
        {
            int id = getIdEtudiant();

            // Following : http://stackoverflow.com/questions/16793767/net-mvc-passing-linq-data-from-controller-to-view
            // More Edits part !! Perfect <3

            var etuCours = from c in bdd.cours
                                   where c.etudiant.Any(e => e.etudiant_id == id)
                                   select new resultCours { cours_id = c.cours_id, libelle = c.libelle, code = c.code, annee = c.annee };
            ViewBag.etuR = etuCours;

            // Créé un objet resultCours pour l'affichage de la liste des cours et résultats de l'étudiant
            IEnumerable<resultCours> etuResult = from e in bdd.etudiant
                                                 from c in bdd.cours
                                                 from er in bdd.etuResult
                                                 where e.etudiant_id == id && e.etudiant_id == er.etudiant_id
                                                 where c.cours_id == er.cours_id
                                                 select new resultCours { libelle = c.libelle, annee = c.annee, code = c.code, cote = er.cote, cours_id = er.cours_id };

            listCoursResult list = new listCoursResult()
            {
                Items = etuResult.AsEnumerable(),

            };
            return View(list);

        }

        /// <summary>
        ///  Récupèré les listes de string contenant les ID des cours et les cotes entrées (si existent) pour les ajouter en BDD etuCours et etuResult (si existe)
        /// </summary>
        /// <param name="liste"> récupère les cours et résultats entrés par l'étudiant </param>
        /// <returns> Retourne à la page index si les données ont correctement été récupéré</returns>
        [HttpPost]
        public ActionResult Cours(listCoursResult liste)
        {
            int id = getIdEtudiant();
            ViewBag.DDCours = GenereCours();

            string[] DDCours = Request.Form.GetValues("DDCours");
            string[] coteCours = Request.Form.GetValues("coteCours");

            List<cours> listc = new List<cours> { };
            for (int i = 0; i < DDCours.Length; i++)
            {

                DDCours = DDCours.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                coteCours = coteCours.ToArray();

                int cotetmp;
                if(coteCours[i] == null || coteCours[i] == "")
                {
                    cotetmp = -1;
                } else {
                    cotetmp = int.Parse(coteCours[i]); 
                }

                if (cotetmp != -1)
                {
                    //Ajoute l'étudiant dans la bdd etuResult QUE si il a une cote pour le cours.
                    etuResult etuRes = new etuResult();
                    etuRes.etudiant_id = id ;
                    etuRes.cours_id = int.Parse(DDCours[i]);
                    etuRes.cote = cotetmp;
                    bdd.etuResult.Add(etuRes);
                }

                //Ajout des cours de l'étudiant
                int codeCours = int.Parse(DDCours[i]);
                cours c = bdd.cours.FirstOrDefault(u => u.cours_id == codeCours);
                listc.Add(c);
            }
 
            //Ajoute la liste de cours dans la table etuCours de l'étudiant
            etudiant etu = bdd.etudiant.FirstOrDefault(e => e.etudiant_id == id);
            etu.cours = listc;
            bdd.SaveChanges();
            if (listc.Any())
            {
                Session["nbCoursInscrit"] = etu.cours.Count();
            }
            // Ajouter le nombre de cours
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Explication()
        {
            return View();
        }
    }
}

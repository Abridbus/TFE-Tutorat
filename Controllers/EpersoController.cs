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

        // GET: Eperso
        // Page finie.
        public ActionResult Info()
        {
            etudiant InfoEtudiant = bdd.etudiant.FirstOrDefault(u => u.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name));
            return View(InfoEtudiant);

        }
        
        /*
         * Si l'étudiant n'a pas de cours pour le moment : afficher le dropdown de cours
         * Si l'étudiant a déjà enregistré ses cours : table des cours (avec résultats?)
         * */

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

        public ActionResult CoursInscrits()
        {
            int id = getIdEtudiant();
            /* Comment jusqu'à ce que je trouve comment afficher les cours de l'étudiantavec les résultats !!
             * var test = from e in bdd.etudiant
                       from c in bdd.cours
                       from er in bdd.etuResult
                       where e.etudiant_id == id && e.etudiant_id == er.etudiant_id
                       where c.cours_id == er.cours_id
                       select new resultCours { libelle = c.libelle, annee = c.annee, code = c.code, cote = er.cote, cours_id = er.cours_id };

            // Following : http://stackoverflow.com/questions/16793767/net-mvc-passing-linq-data-from-controller-to-view
            // More Edits part !! Perfect <3
            listCoursResult list = new listCoursResult()
            {
                Items = test.AsEnumerable(),
                
            };
            */
            var etuCours = from c in bdd.cours
                                   where c.etudiant.Any(e => e.etudiant_id == id)
                                   select new resultCours { cours_id = c.cours_id, libelle = c.libelle, code = c.code, annee = c.annee };
            ViewBag.etuR = etuCours;

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

            //List <resultCours> i = new List<resultCours>();

            /*foreach(var cours_id in etuCours)
            {
                if (etuCours.Contains(cours_id).Equals(etuResult.Contains(cours_id)))
                {
                    etuCours.Concat(etuCours.Where(cote)).Equals()
                }
            }
            */
            return View(list);
        }

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
                //LINQ to Entities does not recognize the method 'Int32 Parse(System.String)' method, and this method cannot be translated into a store expression.
                // ERROR

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
    }
}

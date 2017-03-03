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
    // See Yellow in answer's post
    [Authorize]
    public class EpersoController : Controller
    {
        private Ephec db = new Ephec();

        // GET: Eperso
        // Page finie.
        public ActionResult Info()
        {
            etudiant etuCo = db.etudiant.FirstOrDefault(u => u.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name));
            return View(etuCo);

        }

        /*
         * Si l'étudiant n'a pas de cours pour le moment : afficher une partialView avec le dropdown
         * Si l'étudiant a déjà enregistré ses cours : table des cours (avec résultats?)
         * */

        public ActionResult Cours()
        {
            int id;
            if (Session["id"] != null ) { 
                id = (int) Session["id"];
            } else
            {
                etudiant etuCo = db.etudiant.FirstOrDefault(u => u.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name));
                id = etuCo.etudiant_id;
            }
            // http://stackoverflow.com/questions/42138725/sql-to-linq-for-many-to-many-relation-c-asp-net-mvc
            /* Pas propre, TO DELETE
            var courses = db.etudiant
              .Include(e => e.cours)
              .Single(e => e.etudiant_id == id) // will throw exception if entity not found
              .cours.Select(c => c.libelle); // get all libelle's
            ViewBag.ce2 = string.Join("; ", courses);
            */

            IEnumerable<SelectListItem> items = db.cours
                    .Select(c => new SelectListItem
                    {
                        Value = c.cours_id.ToString(), // La valeur dans le dropdown sera l'id du cours (typecast forcé)
                        Text = c.libelle // La valeur affichée dans le dropdown sera le libellé du cours

                    });
            ViewBag.DDCours = items;

            //Working
            IEnumerable<cours> informations = db.etudiant
              .Include(e => e.cours)
              .Single(e => e.etudiant_id == id) // will throw exception if entity not found
              .cours.Select(c => c); // get all libelle's
            ViewBag.list = informations.ToList();

            if (informations.Any())
            {
                Session["HasCours"] = "true";
            }

            return View();
        }

        public void addetuCours(etudiant etu, cours c)
        {

        }

        [HttpPost]
        public ActionResult cours()
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


            string[] DDCours = Request.Form.GetValues("DDCours");
            string[] coteCours = Request.Form.GetValues("coteCours");

            List<cours> listc = new List<cours> { };
            for (int i = 0; i < DDCours.Length; i++)
            {

                DDCours = DDCours.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                coteCours = coteCours.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                etuResult etuRes = new etuResult();
                etuRes.etudiant_id = id;
                etuRes.cours_id = int.Parse(DDCours[i]);
                etuRes.cote = int.Parse(coteCours[i]);
                db.etuResult.Add(etuRes);

                cours c = db.cours.FirstOrDefault(u => u.cours_id == etuRes.cours_id);
                listc.Add(c);

                etudiant etu = db.etudiant.FirstOrDefault(e => e.etudiant_id == id);
                etu.cours = listc;
                //db.etudiant.Attach(etu);
                //var entry = db.Entry(etu);
                //entry.Property(e => e.cours).CurrentValue = listc;

            // Pour update : db.Entry(personalDetail).State = EntityState.Modified;
            // (voir ici : http://techfunda.com/howto/279/update-record-into-database )
            //db.etudiant.Add(etu);
            // Trouver un moyen de retirer les doublons et/ou modifier !
            // MArche MAIS !! Recréer des étudiants plutot que de mettre à jour les infos !!
        }


            ViewBag.taille = DDCours.Length;
            ViewBag.etudiant_id = 1;
            ViewBag.cours_id = DDCours;
            ViewBag.cote = coteCours;

            db.SaveChanges();

            return View("_PartialView");
        }
    }
}

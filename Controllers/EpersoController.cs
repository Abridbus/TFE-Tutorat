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
        public ActionResult Info(int ?id)
        {
            // Recupérer tous les infos de l'user et stocker dans l'objet etuCo. Envoyer le tout par ViewBag
            //etudiant etu = db.etudiant.Find(id);
            etudiant etuCo = db.etudiant.FirstOrDefault(u => u.mail.Contains(System.Web.HttpContext.Current.User.Identity.Name));
            ViewBag.sec = etuCo;
            return View();

        }

        public ActionResult Cours()
        {
            // http://stackoverflow.com/questions/42138725/sql-to-linq-for-many-to-many-relation-c-asp-net-mvc
            var courses = db.etudiant
              .Include(e => e.cours)
              .Single(e => e.etudiant_id == 1) // will throw exception if entity not found
              .cours.Select(c => c.libelle); // get all libelle's
            ViewBag.ce2 = string.Join(";", courses);

            IEnumerable<cours> informations = db.etudiant
              .Include(e => e.cours)
              .Single(e => e.etudiant_id == 1) // will throw exception if entity not found
              .cours.Select(c => c); // get all libelle's
            ViewBag.list = informations.ToList();

            // TEst not working
            etudiant etu1 = new etudiant();
            etu1.etudiant_id = 1;
            var libel = etu1.cours.Select(c => c.libelle);
            ViewBag.etu1 = string.Join(";", libel);

            return View();
        }

    }
}

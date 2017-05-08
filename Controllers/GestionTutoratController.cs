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
    [Authorize(Roles = "ConseilSocial")]
    public class GestionTutoratController : DefautController
    {
        private Ephec bdd = new Ephec();
        private EphecTemp bddtemp = new EphecTemp();

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
                    result.etudiant_id = (from e in bdd.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                    result.nom = (from e in bdd.etudiant where e.matricule == result.matricule select e.nom).Single();
                    result.prenom = (from e in bdd.etudiant where e.matricule == result.matricule select e.prenom).First();
                    result.cours_libelle = (from c in bdd.cours where c.cours_id == result.cours_id select c.libelle).First();
                    result.annee = (from e in bdd.etudiant where e.matricule == result.matricule select e.annee).First();

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
                result.etudiant_id = (from e in bdd.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                result.nom = (from e in bdd.etudiant where e.matricule == result.matricule select e.nom).Single();
                result.prenom = (from e in bdd.etudiant where e.matricule == result.matricule select e.prenom).First();
                result.cours_libelle = (from c in bdd.cours where c.cours_id == result.cours_id select c.libelle).First();
                result.annee = (from e in bdd.etudiant where e.matricule == result.matricule select e.annee).First();
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
                    result.etudiant_id = (from e in bdd.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                    result.nom = (from e in bdd.etudiant where e.matricule == result.matricule select e.nom).Single();
                    result.prenom = (from e in bdd.etudiant where e.matricule == result.matricule select e.prenom).First();
                    result.cours_libelle = (from c in bdd.cours where c.cours_id == result.cours_id select c.libelle).First();
                    result.cote = (from er in bdd.etuResult where er.etudiant_id == result.etudiant_id select er.cote).First();
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
                result.etudiant_id = (from e in bdd.etudiant where e.matricule == result.matricule select e.etudiant_id).First();
                result.nom = (from e in bdd.etudiant where e.matricule == result.matricule select e.nom).Single();
                result.prenom = (from e in bdd.etudiant where e.matricule == result.matricule select e.prenom).First();
                result.cours_libelle = (from c in bdd.cours where c.cours_id == result.cours_id select c.libelle).First();
                result.cote = (from er in bdd.etuResult where er.etudiant_id == result.etudiant_id && er.cours_id == result.cours_id select er.cote).First();
            };
            return tuteurTemp2;

        }


        // GET: GestionTutorat
        public ActionResult DemandesTutorat(int id)
        {
            ViewBag.infosDemandeurs = InfosDemandeurs(id);

            // Dropdown de tuteur, recherche dans  les 2 bdd :
            IEnumerable<gestionTutorat> tut = InfosTuteurs(id);

            List<SelectListItem> item = tut
                .Select(c => new SelectListItem
                {
                    Value = c.tuteur_id.ToString(),
                    Text = string.Format("{0} - {1} {2}", c.matricule, c.nom, c.prenom)
                }).ToList();


            ViewBag.tuteurs = item;

            return View();
        }

        [HttpPost]
        public ActionResult DemandesTutorat(listAccordTutorat model, int id)
        {
            ViewBag.infosDemandeurs = InfosDemandeurs(id);
            IEnumerable<gestionTutorat> tut = InfosTuteurs(id);
                    List<SelectListItem> item = tut
                        .Select(c => new SelectListItem
                        {
                            Value = c.tuteur_id.ToString(),
                            Text = string.Format("{0} - {1} {2}", c.matricule, c.nom, c.prenom)
                        }).ToList();
            ViewBag.tuteurs = item;

            copieBDD(model);
            return View();
        }

        public int copieBDD(listAccordTutorat mod)
        {
            foreach (var i in mod.item)
            {
                //CONVERT EN INT de tuteur_id car provient de la value d'un dropdown (=type string)!
                int tuteur_id = Convert.ToInt32(i.tuteur_id);

                //Création d'un tutorat depuis le modèle TUTORAT pour l'ajouter à la BDD "officiel" + Extraction l'id du tutorat.
                tutorat t = new tutorat()
                {
                    demandeur_id = 0,
                    cours_id = i.cours_id,
                    commentaire = "",
                    dateResign = null,
                    tempsTotal = 600,
                    tuteur_id = 0
                };

                //Ajouter & Enregistrer dans la bdd

                bdd.tutorat.Add(t);
                bdd.SaveChanges();
                // Va générer une erreur pour le moment puisque va renvoyer plusieurs résultats (ne devrait en renvoyer qu'un !!)
                int tutorat_id = t.tutorat_id;  //Convert.ToInt32(bdd.tutorat.Where(u => u.tuteur_id == tuteur_id && u.cours_id == i.cours_id).Select(u => u.tutorat_id));

                //Création depuis le modèle d'un DEMANDEUR pour l'ajouter dans la BDD officiel et le retirer de la BDD temp.
                demandeurtmp dtmp = bddtemp.demandeurtmp.FirstOrDefault(u => u.demandeur_id == i.demandeur_id);
                int dem_id = 0;
                if (dtmp != null)
                {
                    demandeur d = new demandeur()
                    {
                        matricule = dtmp.matricule,
                        dateInsc = dtmp.dateInsc
                    };
                    bdd.demandeur.Add(d);

                    bdd.SaveChanges();
                    dem_id = d.demandeur_id;
                }

                //Création depuis le modèle d'un DEMANDEURCOURS pour l'ajouter dans la BDD officiel et le retirer de la BDD temp.
                demandeurCourstmp dctmp = bddtemp.demandeurCourstmp.FirstOrDefault(u => u.demandeur_id == i.demandeur_id);
                if(dctmp != null && dem_id != 0)
                {
                    bddtemp.demandeurCourstmp.Remove(dctmp);
                    demandeurCours dc = new demandeurCours()
                    {
                        demandeur_id = dem_id,
                        cours_id = dctmp.cours_id,
                        tutorat_id = tutorat_id,
                        commentaire = dctmp.commentaire,
                        matriculeTuteurPref = dctmp.matriculeTuteurPref
                    };
                    bdd.demandeurCours.Add(dc);

                    // /!\ Ordre de remove : d'abord demandeurCours, ensuite demandeur. Mëme chose dans tuteur
                    if(dtmp != null) { bddtemp.demandeurtmp.Remove(dtmp); }

                    //Enregistrer l'ajout dans bdd et le retrait dans bddtemp
                    bddtemp.SaveChanges();
                    bdd.SaveChanges();
                }



                tuteurtmp ttmp = bddtemp.tuteurtmp.FirstOrDefault(u => u.tuteur_id == tuteur_id);
                int id_tuteur = 0;
                if (ttmp != null)
                {
                    tuteur d = new tuteur()
                    {
                        matricule = ttmp.matricule,
                        dateInsc = ttmp.dateInsc
                    };
                    bdd.tuteur.Add(d);

                    //Enregistrer l'ajout dans bdd et le retrait dans bddtemp
                    bdd.SaveChanges();
                    id_tuteur = d.tuteur_id;
                }

                tuteurCourstmp tctmp = bddtemp.tuteurCourstmp.FirstOrDefault(u => u.tuteur_id == tuteur_id);
                if (tctmp != null && id_tuteur != 0)
                {
                    bddtemp.tuteurCourstmp.Remove(tctmp);
                    tuteurCours d = new tuteurCours()
                    {
                        tuteur_id = id_tuteur,
                        cours_id = tctmp.cours_id,
                        tutorat_id = tutorat_id,
                        commentaire = tctmp.commentaire
                    };
                    bdd.tuteurCours.Add(d);

                    // /!\ Ordre de remove (2)
                    if (ttmp != null ) { bddtemp.tuteurtmp.Remove(ttmp); }

                    //Enregistrer l'ajout dans bdd et le retrait dans bddtemp
                    bddtemp.SaveChanges();
                    bdd.SaveChanges();

                    // Reupdate tutorat avec les demandeurs_id & tuteur_id
                    tutorat tupdate = bdd.tutorat.Where(t2 => t2.tutorat_id == tutorat_id).FirstOrDefault();
                    tupdate.tuteur_id = id_tuteur;
                    tupdate.demandeur_id = dem_id;

                    bdd.SaveChanges();
                }
           }

            return 0;
        }

        public ActionResult ListTuteurs()
        {
            return View(InfosTuteurs(0));
        }

        public ActionResult ListDemandeurs()
        {
            return View(InfosDemandeurs(0));
        }

        public ActionResult DemandesTutoratDeux()
        {
            //Dropdown de Cours :
            ViewBag.DDCours = GenereCours();

            return View();
        }

        [HttpPost]
        public ActionResult DemandesTutoratDeux(int DDCours)
        {
            // Si dd != 0, afficher partialView
            // Si dd == 0, afficher tous? ou ne rien faire?
            if (DDCours == 0)
            {
                return View();
            }
            return RedirectToAction("DemandesTutorat", "GestionTutorat", new { id = DDCours });

        }

        public List<affichageTutorat> ajoutInfoTutorat(List<tutorat> tut )
        {
            List<affichageTutorat> affichageTut = new List<affichageTutorat>();
             int i = 0;
            foreach (var t in tut)
            {
                affichageTut.Add(new affichageTutorat());
                affichageTut[i].demandeur_id = t.demandeur_id;
                affichageTut[i].matricule_demandeur = (from bt in bdd.tutorat from d in bdd.demandeur where bt.demandeur_id == t.demandeur_id select d.matricule).First();
                affichageTut[i].nom_demandeur = (from e in bdd.etudiant from d in bdd.demandeur where d.demandeur_id == t.demandeur_id && d.matricule == e.matricule select e.nom).First();
                affichageTut[i].prenom_demandeur = (from e in bdd.etudiant from d in bdd.demandeur where d.demandeur_id == t.demandeur_id && d.matricule == e.matricule select e.prenom).First();

                affichageTut[i].tuteur_id = t.tuteur_id;
                affichageTut[i].matricule_tuteur = (from bt in bdd.tutorat from d in bdd.tuteur where bt.tuteur_id == t.tuteur_id select d.matricule).First();
                affichageTut[i].nom_tuteur = (from e in bdd.etudiant from tu in bdd.tuteur where tu.tuteur_id == t.tuteur_id && tu.matricule == e.matricule select e.nom).First();// error ??
                affichageTut[i].prenom_tuteur = (from e in bdd.etudiant from d in bdd.tuteur where d.tuteur_id == t.tuteur_id && d.matricule == e.matricule select e.prenom).First();

                affichageTut[i].cours_libelle = (from c in bdd.cours where c.cours_id == t.cours_id select c.libelle).First();
                i++;
            };

            return affichageTut;
        }
        

        public ActionResult TutoratEnCours()
        {
            /* Une fois les tutorats confirmés (& ajoutés dans la BDD Ephec) */
            List<tutorat> tutEnCours;
            tutEnCours = bdd.tutorat.Where(c => c.tempsTotal > 0).ToList();
            //Viewbag at et garder le type list dans le model de la view
            //Viewbag at car on affiche plus d'informations, jouer avec la boucle du model pour afficher
            ViewBag.InfoSupp = ajoutInfoTutorat(tutEnCours);

            return View(tutEnCours);
        }

        public ActionResult TutoratFini()
        {
            List<tutorat> tut;
            tut = bdd.tutorat.Where(t => t.tempsTotal == 0).ToList();

            //Viewbag at et garder le type list dans le model de la view
            //Viewbag at car on affiche plus d'informations, jouer avec la boucle du model pour afficher
            ViewBag.InfoSupp = ajoutInfoTutorat(tut);


            return View(tut);
        }

        // GET: Tutorat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tutorat tutorat = bdd.tutorat.Find(id);
            if (tutorat == null)
            {
                return HttpNotFound();
            }
            return View(tutorat);
        }

        // POST: Tutorat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tutorat_id, demandeur_id, tuteur_id, cours_id, commentaire,dateResign, tempsTotal")] tutorat tutorat)
        {
            if (ModelState.IsValid)
            {
                bdd.Entry(tutorat).State = EntityState.Modified;
                bdd.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(tutorat);
        }

        public ActionResult Explication()
        {

            return View();
        }

    }
}
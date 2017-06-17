namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class affichageTutorat
    {
        //Infos du Tuteur 
        public int tuteur_id { get; set; }

        [StringLength(8)]
        public string matricule_tuteur { get; set; }

        [StringLength(50)]
        public string nom_tuteur { get; set; }

        [StringLength(50)]
        public string prenom_tuteur { get; set; }

        //Infos du Demandeur
        public int demandeur_id { get; set; }

        [StringLength(8)]
        public string matricule_demandeur{ get; set; }

        [StringLength(50)]
        public string nom_demandeur { get; set; }

        [StringLength(50)]
        public string prenom_demandeur { get; set; }

        //Infos du cours
        [StringLength(200)]
        public string cours_libelle { get; set; }

        public int cours_id { get; set; }

        //Infos du tutorat
        [StringLength(255)]
        public string commentaire { get; set; }

        [Display(Name = "Date résiliation")]
        public DateTime? dateResign { get; set; }

        [DefaultValue(600)]
        public int tempsTotal { get; set; }

        // ?? Utile  ??
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<demandeurCours> demandeurCours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestation> prestation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCours> tuteurCours { get; set; }
    }

    public class listAccordTutorat
    {
        public List<accordTutorat> item { get; set; }
    }
    
    public class accordTutorat
    {
        public int demandeur_id { get; set; }

        //Ok this is shit
        public string tuteur_id { get; set; }

        public int cours_id { get; set; }

        public Boolean select { get; set; }

    }

    public class gestionTutorat
    {
        [Key]
        public int etudiant_id { get; set; }

        public int demandeur_id { get; set; }

        public int tuteur_id { get; set; }

        [StringLength(50)]
        public string nom { get; set; }

        [StringLength(50)]
        public string prenom { get; set; }

        [StringLength(8)]
        public string matricule { get; set; }

        [StringLength(200)]
        public string cours_libelle { get; set; }

        public int cours_id { get; set; }
         
        public int? cote { get; set; }

        public int annee { get; set; }

        [StringLength(512)]
        public string commentaire { get; set; }

        public DateTime dateInsc { get; set; }

        [StringLength(8)]
        public string matriculeTuteurPref { get; set; }
    }

    [Table("tutorat")]
    public partial class tutorat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tutorat()
        {
            demandeurCours = new HashSet<demandeurCours>();
            tuteurCours = new HashSet<tuteurCours>();
            prestation = new HashSet<prestation>();
        }

        [Key]
        public int tutorat_id { get; set; }

        public int demandeur_id { get; set; }

        public int tuteur_id { get; set; }

        public int cours_id { get; set; }

        [DisplayName("Commentaire")]
        [StringLength(255)]
        public string commentaire { get; set; }

        [DisplayName("Date résiliation")]
        public DateTime? dateResign { get; set; }

        [DefaultValue(600)]
        [DisplayName("Temps rest.")]
        public int tempsTotal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<demandeurCours> demandeurCours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestation> prestation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCours> tuteurCours { get; set; }
    }
}

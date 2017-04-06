namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ListGestionTutorat
    {
        public List<gestionTutorat> liste {get; set;}
    }
    public class gestionTutorat
    {
        [StringLength(50)]
        public string nom { get; set; }

        [StringLength(50)]
        public string prenom { get; set; }

        [StringLength(8)]
        public string matricule { get; set; }

        [StringLength(200)]
        public string cours_libelle { get; set; }

        public int cours_id { get; set; }

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

        public int cours_id { get; set; }

        [StringLength(255)]
        public string commentaire { get; set; }

        public DateTime? dateResign { get; set; }

        [DefaultValue(600)]
        public int tempsTotal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<demandeurCours> demandeurCours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestation> prestation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCours> tuteurCours { get; set; }
    }
}

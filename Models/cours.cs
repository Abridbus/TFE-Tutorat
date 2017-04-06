namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    /// <summary>
    /// Modèles de EtuCours pour l'affichage : ensemble de modèles d'étudiants et de cours
    /// </summary>
    /// 
    public class listCoursResult
    {
        public IEnumerable<resultCours> Items { get; set; }
    }

    public class resultCours{
        public string libelle { get; set; }
        public string code { get; set; }
        public int annee { get; set; }
        public int? cote { get; set; }
        public int cours_id { get; set; }
    }

    [Table("cours")]
    public partial class cours
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cours()
        {
            demandeurCours = new HashSet<demandeurCours>();
            etuResult = new HashSet<etuResult>();
            tuteurCours = new HashSet<tuteurCours>();
            etudiant = new HashSet<etudiant>();
        }

        [Key]
        public int cours_id { get; set; }

        [Required]
        [StringLength(16)]
        public string code { get; set; }

        [Required]
        [StringLength(200)]
        public string libelle { get; set; }

        [Required]
        [StringLength(4)]
        public string section { get; set; }

        public int annee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<demandeurCours> demandeurCours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<etuResult> etuResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCours> tuteurCours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<etudiant> etudiant { get; set; }
    }
}

namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etudiant")]
    public partial class etudiant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public etudiant()
        {
            etuResult = new HashSet<etuResult>();
            cours = new HashSet<cours>();
        }

        [Key]
        public int etudiant_id { get; set; }

        [Required]
        [StringLength(8)]
        public string matricule { get; set; }

        [StringLength(50)]
        public string prenom { get; set; }

        [StringLength(50)]
        public string nom { get; set; }

        [Required]
        [StringLength(4)]
        public string section { get; set; }

        public int annee { get; set; }

        [StringLength(4)]
        public string groupe { get; set; }

        [StringLength(80)]
        public string mail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<etuResult> etuResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cours> cours { get; set; }
    }
}

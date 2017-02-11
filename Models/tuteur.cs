namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tuteur")]
    public partial class tuteur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tuteur()
        {
            prestation = new HashSet<prestation>();
            tuteurCours = new HashSet<tuteurCours>();
        }

        [Key]
        public int tuteur_id { get; set; }

        [Required]
        [StringLength(8)]
        public string matricule { get; set; }

        public DateTime dateInsc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestation> prestation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCours> tuteurCours { get; set; }
    }
}

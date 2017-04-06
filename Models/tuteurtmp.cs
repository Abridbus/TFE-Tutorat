namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tuteurtmp")]
    public partial class tuteurtmp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tuteurtmp()
        {
            tuteurCourstmp = new HashSet<tuteurCourstmp>();
        }

        [Key]
        public int tuteur_id { get; set; }

        [Required]
        [StringLength(8)]
        public string matricule { get; set; }

        public DateTime dateInsc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCourstmp> tuteurCourstmp { get; set; }
    }
}

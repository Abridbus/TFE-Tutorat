namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tutorat")]
    public partial class tutorat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tutorat()
        {
            demandeurCours = new HashSet<demandeurCours>();
            prestationTutorat = new HashSet<prestationTutorat>();
            tuteurCours = new HashSet<tuteurCours>();
        }

        [Key]
        public int tutorat_id { get; set; }

        public int demandeur_id { get; set; }

        public int cours_id { get; set; }

        [StringLength(255)]
        public string commentaire { get; set; }

        public DateTime? dateResign { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<demandeurCours> demandeurCours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prestationTutorat> prestationTutorat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tuteurCours> tuteurCours { get; set; }
    }
}

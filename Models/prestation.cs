namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //Modèle pour la gestion des Prestations (Conseil Social)
    public class libPrestation{
        public List<string> cours_libelles { get; set; }

        public List<int> cours_ids { get; set; }

        public int[] tuteur_ids { get; set; }

        public List<int> dureeRestantes { get; set; }
    }


    [Table("prestation")]
    public partial class prestation
    {
        public prestation()
        {
            tutorat = new HashSet<tutorat>();
        }
        [Key]
        public int prestation_id { get; set; }

        public int tuteur_id { get; set; }

        public DateTime datePrestation { get; set; }

        public int dureePrestation { get; set; }

        [Required]
        [StringLength(512)]
        public string compteRendu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tutorat> tutorat { get; set; }

        public virtual tuteur tuteur { get; set; }
    }
}

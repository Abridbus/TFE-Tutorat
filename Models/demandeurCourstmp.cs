namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public class listeDemandeTmp
    {
        public List<demandeTmp> Items { get; set; }
    }


    public class demandeTmp
    {
        public int cours_id { get; set; }

        [StringLength(512)]
        public string commentaire { get; set; }

        [StringLength(8)]
        public string matricule { get; set; }

        public DateTime dateInsc { get; set; }
        
        [Required]
        public Boolean select { get; set; }

        [StringLength(8)]
        public string matriculeTuteurPref { get; set; }
    }

    [Table("demandeurCourstmp")]
    public partial class demandeurCourstmp
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int demandeur_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cours_id { get; set; }

        [StringLength(512)]
        public string commentaire { get; set; }

        [StringLength(8)]
        public string matriculeTuteurPref { get; set; }

        public virtual demandeurtmp demandeurtmp { get; set; }
    }
}

namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prestationtmp")]
    public partial class prestationtmp
    {
        [Key]
        public int prestation_id { get; set; }

        public int tutorat_id { get; set; }

        public int tuteur_id { get; set; }

        public DateTime datePrestation { get; set; }

        public int dureePrestation { get; set; }

        [Required]
        [StringLength(512)]
        public string compteRendu { get; set; }
    }
}

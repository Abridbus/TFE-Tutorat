namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prestation")]
    public partial class prestation
    {
        [Key]
        public int prestation_id { get; set; }

        public int tuteur_id { get; set; }

        public DateTime dateFeuille { get; set; }

        public virtual tuteur tuteur { get; set; }

        public virtual prestationTutorat prestationTutorat { get; set; }
    }
}

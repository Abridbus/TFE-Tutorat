namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tuteurCours
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tuteur_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cours_id { get; set; }

        public int tutorat_id { get; set; }

        [StringLength(512)]
        public string commentaire { get; set; }

        public virtual cours cours { get; set; }

        public virtual tuteur tuteur { get; set; }

        public virtual tutorat tutorat { get; set; }
    }
}

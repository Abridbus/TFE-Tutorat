namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("demandeurCours")]
    public partial class demandeurCours
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int demandeur_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cours_id { get; set; }

        public int tutorat_id { get; set; }

        [StringLength(512)]
        public string commentaire { get; set; }

        [StringLength(8)]
        public string matriculeTuteurPref { get; set; }

        public virtual cours cours { get; set; }

        public virtual demandeur demandeur { get; set; }

        public virtual tutorat tutorat { get; set; }
    }
}

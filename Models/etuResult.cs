namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etuResult")]
    public partial class etuResult
    {
        [Key]
        public int etuResult_id { get; set; }

        public int etudiant_id { get; set; }

        public int cours_id { get; set; }

        public int? cote { get; set; }

        public virtual cours cours { get; set; }

        public virtual etudiant etudiant { get; set; }
    }
}

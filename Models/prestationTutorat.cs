namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prestationTutorat")]
    public partial class prestationTutorat
    {
        [Key]
        public int presTutorat_id { get; set; }

        public int dureePrest { get; set; }

        public int prestation_id { get; set; }

        public int tutorat_id { get; set; }

        public virtual prestation prestation { get; set; }

        public virtual tutorat tutorat { get; set; }
    }
}

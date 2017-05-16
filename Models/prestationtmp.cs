namespace Tutorat.Models
{
    using Controllers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public class listePrestationtmp
    {
        public List<prestationtmp> Items { get; set; }

        [DisplayName("Sélection")]
        public List<Boolean> select { get; set; }

    }

    public class tutoratInfos
    {
        public int tutorat_id { get; set; }

        public int tuteur_id { get; set; }

        public int dureeRestante { get; set; }

        public int cours_id { get; set; }

        public string cours_libelle { get; set; }
    }

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

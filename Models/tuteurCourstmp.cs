namespace Tutorat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    /*
     * Réutiliser le code demandeTmp car identique pour Demande (Etudiant demandeur) et Offre (Tuteur)
     * Attention, cette utilisation ne sera utile que pour la view, il faudra modifier le controller pour qu'il ajoute dans les bonnes tables TuteurTmp !!
     * */
    public class listeTuteurTmp
    {
        public List<demandeTmp> Items { get; set; }
    }

    [Table("tuteurCourstmp")]
    public partial class tuteurCourstmp
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

        public virtual tuteurtmp tuteurtmp { get; set; }
    }
}

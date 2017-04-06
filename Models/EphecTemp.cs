namespace Tutorat.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EphecTemp : DbContext
    {
        public EphecTemp()
            : base("name=EphecTemp")
        {
        }

        public virtual DbSet<demandeurCourstmp> demandeurCourstmp { get; set; }
        public virtual DbSet<demandeurtmp> demandeurtmp { get; set; }
        public virtual DbSet<prestationtmp> prestationtmp { get; set; }
        public virtual DbSet<tuteurCourstmp> tuteurCourstmp { get; set; }
        public virtual DbSet<tuteurtmp> tuteurtmp { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<demandeurCourstmp>()
                .Property(e => e.commentaire)
                .IsUnicode(false);

            modelBuilder.Entity<demandeurtmp>()
                .Property(e => e.matricule)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<demandeurtmp>()
                .HasMany(e => e.demandeurCourstmp)
                .WithRequired(e => e.demandeurtmp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<prestationtmp>()
                .Property(e => e.compteRendu)
                .IsUnicode(false);

            modelBuilder.Entity<tuteurCourstmp>()
                .Property(e => e.commentaire)
                .IsUnicode(false);

            modelBuilder.Entity<tuteurtmp>()
                .Property(e => e.matricule)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tuteurtmp>()
                .HasMany(e => e.tuteurCourstmp)
                .WithRequired(e => e.tuteurtmp)
                .WillCascadeOnDelete(false);
        }
    }
}

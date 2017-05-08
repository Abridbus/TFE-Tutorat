namespace Tutorat.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Ephec : DbContext
    {
        public Ephec()
            : base("name=EPHECDatabase")
        {
        }

        public virtual DbSet<cours> cours { get; set; }
        public virtual DbSet<demandeur> demandeur { get; set; }
        public virtual DbSet<demandeurCours> demandeurCours { get; set; }
        public virtual DbSet<etudiant> etudiant { get; set; }
        public virtual DbSet<etuResult> etuResult { get; set; }
        public virtual DbSet<prestation> prestation { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tuteur> tuteur { get; set; }
        public virtual DbSet<tuteurCours> tuteurCours { get; set; }
        public virtual DbSet<tutorat> tutorat { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cours>()
                .Property(e => e.code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<cours>()
                .Property(e => e.libelle)
                .IsUnicode(false);

            modelBuilder.Entity<cours>()
                .Property(e => e.section)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<cours>()
                .HasMany(e => e.demandeurCours)
                .WithRequired(e => e.cours)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<cours>()
                .HasMany(e => e.etuResult)
                .WithRequired(e => e.cours)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<cours>()
                .HasMany(e => e.tuteurCours)
                .WithRequired(e => e.cours)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<cours>()
                .HasMany(e => e.etudiant)
                .WithMany(e => e.cours)
                .Map(m => m.ToTable("etuCours").MapLeftKey("cours_id").MapRightKey("etudiant_id"));

            modelBuilder.Entity<demandeur>()
                .Property(e => e.matricule)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<demandeur>()
                .HasMany(e => e.demandeurCours)
                .WithRequired(e => e.demandeur)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<demandeurCours>()
                .Property(e => e.commentaire)
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .Property(e => e.matricule)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .Property(e => e.prenom)
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .Property(e => e.section)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .Property(e => e.groupe)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .Property(e => e.mail)
                .IsUnicode(false);

            modelBuilder.Entity<etudiant>()
                .HasMany(e => e.etuResult)
                .WithRequired(e => e.etudiant)
                .WillCascadeOnDelete(false);

            //Many to Many avec middle table prestationTutorat, n'existant pas sous Entity Framework
            modelBuilder.Entity<prestation>()
                .HasMany(e => e.tutorat)
                .WithMany(e => e.prestation)
                .Map(m => m.ToTable("prestationTutorat").MapLeftKey("prestation_id").MapRightKey("tutorat_id"));

            modelBuilder.Entity<tuteur>()
                .Property(e => e.matricule)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tuteur>()
                .HasMany(e => e.prestation)
                .WithRequired(e => e.tuteur)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tuteur>()
                .HasMany(e => e.tuteurCours)
                .WithRequired(e => e.tuteur)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tuteurCours>()
                .Property(e => e.commentaire)
                .IsUnicode(false);

            modelBuilder.Entity<tutorat>()
                .Property(e => e.commentaire)
                .IsUnicode(false);

            modelBuilder.Entity<tutorat>()
                .HasMany(e => e.demandeurCours)
                .WithRequired(e => e.tutorat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tutorat>()
                .HasMany(e => e.prestation)
                .WithMany(e => e.tutorat)
                .Map(m => m.ToTable("prestationTutorat").MapLeftKey("tutorat_id").MapRightKey("prestation_id"));

            modelBuilder.Entity<tutorat>()
                .HasMany(e => e.tuteurCours)
                .WithRequired(e => e.tutorat)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<Tutorat.Models.gestionTutorat> gestionTutorats { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AgencijaZaNekretnine.Models
{
    public partial class AgencijaNekretninaContext : DbContext
    {
        public AgencijaNekretninaContext()
        {
        }

        public AgencijaNekretninaContext(DbContextOptions<AgencijaNekretninaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Nekretnina> Nekretninas { get; set; } = null!;
        public virtual DbSet<Placanje> Placanjes { get; set; } = null!;
        public virtual DbSet<Preduzimac> Preduzimacs { get; set; } = null!;
        public virtual DbSet<Rezervacija> Rezervacijas { get; set; } = null!;
        public virtual DbSet<SlikeNekretnine> SlikeNekretnines { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Tip> Tips { get; set; } = null!;
        public virtual DbSet<Ugovor> Ugovors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-OL7ML3V;Database=AgencijaNekretnina;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.IsApproved).HasColumnName("isApproved");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Nekretnina>(entity =>
            {
                entity.ToTable("Nekretnina");

                entity.Property(e => e.Adresa).HasMaxLength(150);

                entity.Property(e => e.Cena).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdUser).HasMaxLength(450);

                entity.Property(e => e.Slika).HasMaxLength(250);

                entity.HasOne(d => d.IdPreduzimacNavigation)
                    .WithMany(p => p.Nekretninas)
                    .HasForeignKey(d => d.IdPreduzimac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nekretnina_Preduzimac");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Nekretninas)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nekretnina_Status");

                entity.HasOne(d => d.IdTipNavigation)
                    .WithMany(p => p.Nekretninas)
                    .HasForeignKey(d => d.IdTip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nekretnina_Tip");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Nekretninas)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Nekretnina_AspNetUsers");
            });

            modelBuilder.Entity<Placanje>(entity =>
            {
                entity.ToTable("Placanje");

                entity.Property(e => e.NacinPlacanja).HasMaxLength(150);

                entity.Property(e => e.Opis).HasMaxLength(600);
            });

            modelBuilder.Entity<Preduzimac>(entity =>
            {
                entity.ToTable("Preduzimac");

                entity.Property(e => e.Naziv).HasMaxLength(150);
            });

            modelBuilder.Entity<Rezervacija>(entity =>
            {
                entity.ToTable("Rezervacija");

                entity.Property(e => e.DatumDo).HasColumnType("datetime");

                entity.Property(e => e.DatumOd).HasColumnType("datetime");

                entity.Property(e => e.IdAgenta).HasMaxLength(450);

                entity.HasOne(d => d.IdAgentaNavigation)
                    .WithMany(p => p.Rezervacijas)
                    .HasForeignKey(d => d.IdAgenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rezervacija_AspNetUsers");
            });

            modelBuilder.Entity<SlikeNekretnine>(entity =>
            {
                entity.ToTable("SlikeNekretnine");

                entity.Property(e => e.UrlSlike).HasMaxLength(250);

                entity.HasOne(d => d.IdNekretnineNavigation)
                    .WithMany(p => p.SlikeNekretnines)
                    .HasForeignKey(d => d.IdNekretnine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SlikeNekretnine_Nekretnina");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NazivStatusa).HasMaxLength(150);
            });

            modelBuilder.Entity<Tip>(entity =>
            {
                entity.ToTable("Tip");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NazivTipa).HasMaxLength(150);
            });

            modelBuilder.Entity<Ugovor>(entity =>
            {
                entity.ToTable("Ugovor");

                entity.Property(e => e.BrojTelefona).HasMaxLength(50);

                entity.Property(e => e.Datum).HasColumnType("date");

                entity.Property(e => e.Iznos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PdfUrl).HasMaxLength(350);

                entity.Property(e => e.PunoIme).HasMaxLength(250);

                entity.HasOne(d => d.IdNekretnineNavigation)
                    .WithMany(p => p.Ugovors)
                    .HasForeignKey(d => d.IdNekretnine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ugovor_Nekretnina");

                entity.HasOne(d => d.IdPlacanjaNavigation)
                    .WithMany(p => p.Ugovors)
                    .HasForeignKey(d => d.IdPlacanja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ugovor_Placanje");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

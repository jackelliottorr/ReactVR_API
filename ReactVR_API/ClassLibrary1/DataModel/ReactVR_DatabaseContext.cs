using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClassLibrary1.DataModel
{
    public partial class ReactVR_DatabaseContext : DbContext
    {
        public ReactVR_DatabaseContext()
        {
        }

        public ReactVR_DatabaseContext(DbContextOptions<ReactVR_DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Level> Level { get; set; }
        public virtual DbSet<LevelConfiguration> LevelConfiguration { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<OrganisationInvite> OrganisationInvite { get; set; }
        public virtual DbSet<OrganisationMembership> OrganisationMembership { get; set; }
        public virtual DbSet<PasswordReset> PasswordReset { get; set; }
        public virtual DbSet<Scoreboard> Scoreboard { get; set; }
        public virtual DbSet<Target> Target { get; set; }
        public virtual DbSet<TargetAppearance> TargetAppearance { get; set; }
        public virtual DbSet<TargetZone> TargetZone { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-EFE5Q9P;Initial Catalog=ReactVR_Database;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Level>(entity =>
            {
                entity.Property(e => e.LevelId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LevelConfiguration>(entity =>
            {
                entity.Property(e => e.LevelConfigurationId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.TargetLifespan).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TargetSpawnDelay).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.LevelConfiguration)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LevelConfiguration_To_UserAccount");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.LevelConfiguration)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LevelConfiguration_To_Level");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.LevelConfiguration)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LevelConfiguration_To_Organisation");

                entity.HasOne(d => d.TargetZone)
                    .WithMany(p => p.LevelConfiguration)
                    .HasForeignKey(d => d.TargetZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LevelConfiguration_To_TargetZone");
            });

            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.Property(e => e.OrganisationId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.OrganisationName).HasMaxLength(128);
            });

            modelBuilder.Entity<OrganisationInvite>(entity =>
            {
                entity.Property(e => e.OrganisationInviteId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.HasOne(d => d.InviteUserTypeNavigation)
                    .WithMany(p => p.OrganisationInvite)
                    .HasForeignKey(d => d.InviteUserType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganisationInvite_To_UserType");

                entity.HasOne(d => d.InvitedBy)
                    .WithMany(p => p.OrganisationInvite)
                    .HasForeignKey(d => d.InvitedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganisationInvite_To_UserAccount");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.OrganisationInvite)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganisationInvite_To_Organisation");
            });

            modelBuilder.Entity<OrganisationMembership>(entity =>
            {
                entity.Property(e => e.OrganisationMembershipId).ValueGeneratedNever();

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.OrganisationMembership)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganisationMembership_To_Organisation");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.OrganisationMembership)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganisationMembership_To_UserAccount");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.OrganisationMembership)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganisationMembership_To_UserType");
            });

            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.Property(e => e.PasswordResetId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.PasswordReset)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PasswordReset_To_UserAccount");
            });

            modelBuilder.Entity<Scoreboard>(entity =>
            {
                entity.Property(e => e.ScoreboardId).ValueGeneratedNever();

                entity.HasOne(d => d.LevelConfiguration)
                    .WithMany(p => p.Scoreboard)
                    .HasForeignKey(d => d.LevelConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Scoreboard_To_LevelConfiguration");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Scoreboard)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Scoreboard_To_UserAccount");
            });

            modelBuilder.Entity<Target>(entity =>
            {
                entity.Property(e => e.TargetId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OffsetX).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.OffsetY).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.OffsetZ).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TargetShape)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TargetX).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TargetY).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TargetZ).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.LevelConfiguration)
                    .WithMany(p => p.Target)
                    .HasForeignKey(d => d.LevelConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Target_To_LevelConfiguration");
            });

            modelBuilder.Entity<TargetAppearance>(entity =>
            {
                entity.Property(e => e.TargetAppearanceId).ValueGeneratedNever();

                entity.Property(e => e.TargetUptime).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Scoreboard)
                    .WithMany(p => p.TargetAppearance)
                    .HasForeignKey(d => d.ScoreboardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TargetAppearance_To_Scoreboard");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.TargetAppearance)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TargetAppearance_To_Target");
            });

            modelBuilder.Entity<TargetZone>(entity =>
            {
                entity.Property(e => e.TargetZoneId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TargetZoneShape)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TargetZoneX).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TargetZoneY).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TargetZoneZ).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.Property(e => e.UserAccountId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Password).HasMaxLength(256);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

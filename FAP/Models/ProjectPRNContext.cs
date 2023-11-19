using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FAP.Models
{
    public partial class ProjectPRNContext : DbContext
    {
        public ProjectPRNContext()
        {
        }

        public ProjectPRNContext(DbContextOptions<ProjectPRNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<ClassSchedule> ClassSchedules { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			var builder = new ConfigurationBuilder()
	          .SetBasePath(Directory.GetCurrentDirectory())
	          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			        IConfigurationRoot configuration = builder.Build();

			        optionsBuilder.UseLazyLoadingProxies(true);
			        optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Account__A25C5AA645196EDF");

                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "UQ__Account__A9D10534A3990799")
                    .IsUnique();

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Student')");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.SubjectCode).HasMaxLength(8);

                entity.Property(e => e.TeacherCode).HasMaxLength(20);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__SemesterI__31EC6D26");

                entity.HasOne(d => d.SubjectCodeNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SubjectCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__SubjectCo__300424B4");

                entity.HasOne(d => d.TeacherCodeNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.TeacherCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__TeacherCo__33D4B598");
            });

            modelBuilder.Entity<ClassSchedule>(entity =>
            {
                entity.ToTable("ClassSchedule");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Room).HasMaxLength(7);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassSchedules)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClassSche__Class__34C8D9D1");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.ClassSchedules)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClassSche__SlotI__35BCFE0A");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.SendBy).HasMaxLength(8);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Request__ClassId__4CA06362");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.End).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Start).HasColumnType("date");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Subject__A25C5AA640B2EC64");

                entity.ToTable("Subject");

                entity.Property(e => e.Code).HasMaxLength(8);

                entity.Property(e => e.ManagerCode).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ManagerCodeNavigation)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.ManagerCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subject__Manager__29572725");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

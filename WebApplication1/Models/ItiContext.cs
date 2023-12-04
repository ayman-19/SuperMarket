using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class ItiContext : DbContext
{
    public ItiContext()
    {
    }

    public ItiContext(DbContextOptions<ItiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Debendent> Debendents { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Empolyee> Empolyees { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<WorkOn> WorkOns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ITI;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Debendent>(entity =>
        {
            entity.HasKey(e => new { e.Essn, e.Name }).HasName("PK__Debenden__4439C7A3472ED811");

            entity.ToTable("Debendent");

            entity.Property(e => e.Essn)
                .HasMaxLength(50)
                .HasColumnName("ESSN");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Sex)
                .HasMaxLength(50)
                .HasColumnName("SEX");

            entity.HasOne(d => d.EssnNavigation).WithMany(p => p.Debendents)
                .HasForeignKey(d => d.Essn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Debendent_Empolyee");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Dnum);

            entity.ToTable("Department");

            entity.Property(e => e.Dnum)
                .HasMaxLength(50)
                .HasColumnName("DNum");
            entity.Property(e => e.Dname)
                .HasMaxLength(50)
                .HasColumnName("DName");
            entity.Property(e => e.Mgrssn)
                .HasMaxLength(50)
                .HasColumnName("MGRSSN");
            entity.Property(e => e.MgrstartDate)
                .HasColumnType("date")
                .HasColumnName("MGRStart Date");

            entity.HasOne(d => d.MgrssnNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.Mgrssn)
                .HasConstraintName("FK_Department_Empolyee");
        });

        modelBuilder.Entity<Empolyee>(entity =>
        {
            entity.HasKey(e => e.Ssn);

            entity.ToTable("Empolyee");

            entity.Property(e => e.Ssn)
                .HasMaxLength(50)
                .HasColumnName("SSN");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Dno)
                .HasMaxLength(50)
                .HasColumnName("DNo");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .HasColumnName("FName");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .HasColumnName("LName");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Sex).HasMaxLength(50);
            entity.Property(e => e.Superson).HasMaxLength(50);

            entity.HasOne(d => d.DnoNavigation).WithMany(p => p.Empolyees)
                .HasForeignKey(d => d.Dno)
                .HasConstraintName("FK_Empolyee_Department");

            entity.HasOne(d => d.SupersonNavigation).WithMany(p => p.InverseSupersonNavigation)
                .HasForeignKey(d => d.Superson)
                .HasConstraintName("FK_Empolyee_Empolyee1");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Pnumber);

            entity.ToTable("Project");

            entity.Property(e => e.Pnumber)
                .HasMaxLength(50)
                .HasColumnName("PNumber");
            entity.Property(e => e.Dnum)
                .HasMaxLength(50)
                .HasColumnName("DNum");
            entity.Property(e => e.Plocation)
                .HasMaxLength(50)
                .HasColumnName("PLocation");
            entity.Property(e => e.Pname)
                .HasMaxLength(50)
                .HasColumnName("PName");

            entity.HasOne(d => d.DnumNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Dnum)
                .HasConstraintName("FK_Project_Department");
        });

        modelBuilder.Entity<WorkOn>(entity =>
        {
            entity.HasKey(e => new { e.Essn, e.Pno }).HasName("PK__Work_On__CF59EC3037291D77");

            entity.ToTable("Work_On");

            entity.Property(e => e.Essn)
                .HasMaxLength(50)
                .HasColumnName("ESSN");
            entity.Property(e => e.Pno)
                .HasMaxLength(50)
                .HasColumnName("PNO");
            entity.Property(e => e.Hour).HasMaxLength(50);

            entity.HasOne(d => d.EssnNavigation).WithMany(p => p.WorkOns)
                .HasForeignKey(d => d.Essn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Work_On_Empolyee");

            entity.HasOne(d => d.PnoNavigation).WithMany(p => p.WorkOns)
                .HasForeignKey(d => d.Pno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Work_Project");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

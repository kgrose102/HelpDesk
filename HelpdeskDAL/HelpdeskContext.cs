using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL;

public partial class HelpdeskContext : DbContext
{
    public HelpdeskContext()
    {
    }

    public HelpdeskContext(DbContextOptions<HelpdeskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HelpdeskDb;Trusted_Connection=True;");
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Call>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Call");

            entity.Property(e => e.DateClosed).HasColumnType("smalldatetime");
            entity.Property(e => e.DateOpened).HasColumnType("smalldatetime");
            entity.Property(e => e.Notes)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Timer)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Employee).WithMany(p => p.CallEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CallHasEmployee");

            entity.HasOne(d => d.Problem).WithMany(p => p.Calls)
                .HasForeignKey(d => d.ProblemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CallHasProblem");

            entity.HasOne(d => d.Tech).WithMany(p => p.CallTeches)
                .HasForeignKey(d => d.TechId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CallHasTech");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Department");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Timer)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Timer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Title)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeInDept");
        });

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Problem");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Timer)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

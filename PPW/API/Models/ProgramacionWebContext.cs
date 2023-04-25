using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class ProgramacionWebContext : DbContext
{
    public ProgramacionWebContext()
    {
    }

    public ProgramacionWebContext(DbContextOptions<ProgramacionWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Modelos.Chat> Chats { get; set; }

    public virtual DbSet<Modelos.Contact> Contacts { get; set; }

    public virtual DbSet<Modelos.Status> Statuses { get; set; }

    public virtual DbSet<Modelos.User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("MySQLConnection");
            optionsBuilder.UseMySQL(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Modelos.Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("chat");

            entity.HasIndex(e => e.ContactId, "FK_ContactID");

            entity.HasIndex(e => e.UserId, "FK_UserID");

            entity.Property(e => e.Id).HasColumnName("ID");
            /*entity.Property(e => e.Archivos)
                .HasMaxLength(8000)
                .HasColumnName("archivos");*/
            entity.Property(e => e.ContactId).HasColumnName("Contact_ID");
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(8000)
                .HasColumnName("mensaje");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            /*entity.HasOne(d => d.Contact).WithMany(p => p.Chats)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContactID");

            entity.HasOne(d => d.User).WithMany(p => p.Chats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserID");*/
        });

        modelBuilder.Entity<Modelos.Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contact");

            entity.HasIndex(e => e.PuserId, "FK_PPersonID");

            entity.HasIndex(e => e.SuserId, "FK_SPersonID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.PuserId).HasColumnName("PUser_Id");
            entity.Property(e => e.SuserId).HasColumnName("SUser_Id");

            /*entity.HasOne(d => d.Puser).WithMany(p => p.ContactPusers)
                .HasForeignKey(d => d.PuserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PPersonID");

            entity.HasOne(d => d.Suser).WithMany(p => p.ContactSusers)
                .HasForeignKey(d => d.SuserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SPersonID");*/
        });

        modelBuilder.Entity<Modelos.Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("status");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Vigente).HasColumnName("vigente");
        });

        modelBuilder.Entity<Modelos.User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.StatusId, "FK_StatusID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");
            entity.Property(e => e.Username).HasMaxLength(30);

            /*entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusID");*/
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

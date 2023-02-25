using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P_Progra_Web.Models;

public partial class ProgramacionWebContext : DbContext
{
    public ProgramacionWebContext()
    {
    }

    public ProgramacionWebContext(DbContextOptions<ProgramacionWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ContactList> ContactLists { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;userid=jose;password=Zerafina5H;database=programacion_web;TreatTinyAsBoolean=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("chat");

            entity.HasIndex(e => e.ListaId, "FK_ListaID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Archivos).HasColumnName("archivos");
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.ListaId).HasColumnName("Lista_ID");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(16350)
                .HasColumnName("mensaje");

            entity.HasOne(d => d.Lista).WithMany(p => p.Chats)
                .HasForeignKey(d => d.ListaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListaID");
        });

        modelBuilder.Entity<ContactList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contact_list");

            entity.HasIndex(e => e.PrimerUserId, "FK_PPersonID");

            entity.HasIndex(e => e.SegundoUserId, "FK_SPersonID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.PrimerUserId).HasColumnName("primerUserId");
            entity.Property(e => e.SegundoUserId).HasColumnName("segundoUserId");
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");

            entity.HasOne(d => d.PrimerUser).WithMany(p => p.ContactListPrimerUsers)
                .HasForeignKey(d => d.PrimerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PPersonID");

            entity.HasOne(d => d.SegundoUser).WithMany(p => p.ContactListSegundoUsers)
                .HasForeignKey(d => d.SegundoUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SPersonID");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("person");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Status>(entity =>
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.IdPersona, "FK_PersonID");

            entity.HasIndex(e => e.StatusId, "FK_StatusID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FecTransac)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fec_transac");
            entity.Property(e => e.IdPersona).HasColumnName("ID_Persona");
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");
            entity.Property(e => e.Username).HasMaxLength(30);

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonID");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

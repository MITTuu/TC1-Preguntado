using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Preguntado.Models;

public partial class WebPreguntadosContext : DbContext
{
    public WebPreguntadosContext()
    {
    }

    public WebPreguntadosContext(DbContextOptions<WebPreguntadosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HistorialPartidum> HistorialPartida { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=DYLAN; database=WebPreguntados; integrated security=true; TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistorialPartidum>(entity =>
        {
            entity.HasKey(e => e.IdPartida).HasName("PK__Historia__552192F61774906D");

            entity.Property(e => e.IdPartida).HasColumnName("idPartida");
            entity.Property(e => e.Aciertos).HasColumnName("aciertos");
            entity.Property(e => e.FechaPartida)
                .HasColumnType("datetime")
                .HasColumnName("fecha_partida");
            entity.Property(e => e.IdJugador).HasColumnName("idJugador");
            entity.Property(e => e.Resultado).HasColumnName("resultado");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.HistorialPartida)
                .HasForeignKey(d => d.IdJugador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__idJug__398D8EEE");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.IdJugador).HasName("PK__Jugador__73F34C02291A1001");

            entity.ToTable("Jugador");

            entity.Property(e => e.IdJugador).HasColumnName("idJugador");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

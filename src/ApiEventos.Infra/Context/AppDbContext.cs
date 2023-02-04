using ApiEventos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("eventos_pkey");

                entity.ToTable("eventos");

                entity.Property(e => e.Id).HasColumnName("codigo_evento");
                entity.Property(e => e.CargaHoraria).HasColumnName("carga_horaria");
                entity.Property(e => e.DataRealizado).HasColumnName("data_realizado");
                entity.Property(e => e.Descricao).HasColumnName("descricao");
                entity.Property(e => e.Empresa).HasColumnName("empresa");
                entity.Property(e => e.Instrutor).HasColumnName("instrutor");
                entity.Property(e => e.ParticipantesEsperados).HasColumnName("participantes_esperados");
                entity.Property(e => e.TipoEvento).HasColumnName("tipo_evento");
                entity.Property(e => e.UrlConteudo).HasColumnName("url_documentos");
            });
        }
    }
}

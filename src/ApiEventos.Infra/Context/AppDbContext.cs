using ApiEventos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<StorageFile> StorageFiles { get; set; }
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
                entity.Property(e => e.ParticipacoesConfirmadas).HasColumnName("participacoes_confirmadas");
                entity.Property(e => e.Inativo).HasColumnName("inativo");
                entity.Property(e => e.TipoEvento).HasColumnName("tipo_evento");
            });

            modelBuilder.Entity<StorageFile>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("conteudo_eventos_pkey");

                entity.ToTable("conteudo_eventos");

                entity.Property(e => e.CodigoEvento).HasColumnName("codigo_evento");
                entity.Property(e => e.Url).HasColumnName("url_conteudo");
                entity.Property(e => e.Name).HasColumnName("nome_conteudo");
            });
        }
    }
}

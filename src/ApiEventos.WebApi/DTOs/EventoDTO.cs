using System.ComponentModel.DataAnnotations;

namespace ApiEventos.WebApi.DTOs
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public int? TipoEvento { get; set; }
        public string? Descricao { get; set; }
        public string Empresa { get; set; }
        public string? Instrutor { get; set; }
        public DateTime? DataRealizado { get; set; }
        public float? CargaHoraria { get; set; }
        public int? ParticipantesEsperados { get; set; }
        public int? ParticipacoesConfirmadas { get; set; }
        public bool? Inativo { get; set; }
        public IEnumerable<string>? ArquivosBase64 { get; set; }
    }
}

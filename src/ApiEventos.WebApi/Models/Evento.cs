using System;
using System.Collections.Generic;

namespace ApiEventos.WebApi.Models;

public partial class Evento
{
    public long CodigoEvento { get; set; }

    public DateTime? DataRealizado { get; set; }

    public string? Empresa { get; set; }

    public string? Instrutor { get; set; }

    public float? CargaHoraria { get; set; }

    public string? Descricao { get; set; }

    public long? TipoEvento { get; set; }

    public long ParticipantesEsperados { get; set; }

    public long ParticipacoesConfirmadas { get; set; }

    public bool? Inativo { get; set; }

    public virtual ICollection<ConteudoEvento> ConteudoEventos { get; } = new List<ConteudoEvento>();

    public virtual ICollection<Participante> Participantes { get; } = new List<Participante>();

    public virtual EventosTipo? TipoEventoNavigation { get; set; }
}

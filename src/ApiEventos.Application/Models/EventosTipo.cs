using System;
using System.Collections.Generic;

namespace ApiEventos.Application.Models;

public partial class EventosTipo
{
    public long TipoCodigo { get; set; }

    public string TipoDescricao { get; set; } = null!;

    public virtual ICollection<Evento> Eventos { get; } = new List<Evento>();
}

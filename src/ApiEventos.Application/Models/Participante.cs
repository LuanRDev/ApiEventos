using System;
using System.Collections.Generic;

namespace ApiEventos.Application.Models;

public partial class Participante
{
    public long CodigoParticipante { get; set; }

    public DateTime? DataParticipacao { get; set; }

    public string Nome { get; set; } = null!;

    public string? Cpf { get; set; }

    public long CodigoEvento { get; set; }

    public virtual Evento CodigoEventoNavigation { get; set; } = null!;
}

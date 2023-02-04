using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEventos.Application.Models;

public partial class Evento
{
    public int CodigoEvento { get; set; }

    public DateTime? DataRealizado { get; set; }

    public string? Empresa { get; set; }

    public string? Instrutor { get; set; }

    public float? CargaHoraria { get; set; }

    public string? Descricao { get; set; }

    public int? TipoEvento { get; set; }

    public int ParticipantesEsperados { get; set; }

    public string? UrlDocumentos { get; set; }
}

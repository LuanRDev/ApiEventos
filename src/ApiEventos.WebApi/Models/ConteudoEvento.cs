using System;
using System.Collections.Generic;

namespace ApiEventos.WebApi.Models;

public partial class ConteudoEvento
{
    public long CodigoConteudo { get; set; }

    public long? CodigoEvento { get; set; }

    public string UrlConteudo { get; set; } = null!;

    public string NomeConteudo { get; set; } = null!;

    public virtual Evento? CodigoEventoNavigation { get; set; }
}

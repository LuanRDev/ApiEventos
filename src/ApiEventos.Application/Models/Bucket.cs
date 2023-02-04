using System;
using System.Collections.Generic;

namespace ApiEventos.Application.Models;

public partial class Bucket
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid? Owner { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? Public { get; set; }

    public virtual ICollection<Object> Objects { get; } = new List<Object>();

    public virtual User? OwnerNavigation { get; set; }
}

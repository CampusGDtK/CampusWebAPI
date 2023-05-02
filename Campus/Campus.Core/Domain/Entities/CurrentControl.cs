using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class CurrentControl
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid DisciplineId { get; set; }

    public string Detail { get; set; } = null!;

    public string Mark { get; set; } = null!;

    public long TotalMark { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}

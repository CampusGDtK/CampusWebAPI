using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Cathedra
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Head { get; set; } = null!;

    public Guid FacultyId { get; set; }

    public virtual ICollection<Academic> Academics { get; set; } = new List<Academic>();

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual Faculty Faculty { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Discipline
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CathedralId { get; set; }

    public virtual ICollection<AcademicDisciplineGroup> AcademicDisciplineGroups { get; set; } = new List<AcademicDisciplineGroup>();

    public virtual Cathedra Cathedral { get; set; } = null!;

    public virtual ICollection<CurrentControl> CurrentControls { get; set; } = new List<CurrentControl>();
}

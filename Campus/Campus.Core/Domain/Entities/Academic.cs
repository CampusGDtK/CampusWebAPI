using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Academic
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CathedralId { get; set; }

    public virtual ICollection<AcademicDisciplineGroup> AcademicDisciplineGroups { get; set; } = new List<AcademicDisciplineGroup>();

    public virtual Cathedra Cathedral { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}

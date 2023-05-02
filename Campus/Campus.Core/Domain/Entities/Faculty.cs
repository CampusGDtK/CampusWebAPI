using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Faculty
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Dean { get; set; } = null!;

    public virtual ICollection<Cathedra> Cathedras { get; set; } = new List<Cathedra>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<SpecialityFaculty> SpecialityFaculties { get; set; } = new List<SpecialityFaculty>();
}

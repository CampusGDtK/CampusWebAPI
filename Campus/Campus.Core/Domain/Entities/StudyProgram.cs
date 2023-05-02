using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class StudyProgram
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid SpecialityId { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Speciality Speciality { get; set; } = null!;
}

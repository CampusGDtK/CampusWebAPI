using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class AcademicDisciplineGroup
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public Guid DisciplineId { get; set; }

    public Guid AcademicId { get; set; }

    public virtual Academic Academic { get; set; } = null!;

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}

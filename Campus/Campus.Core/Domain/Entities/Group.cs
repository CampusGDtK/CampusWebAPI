using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CuratorId { get; set; }

    public Guid StudyProgramId { get; set; }

    public Guid FacultyId { get; set; }

    public virtual ICollection<AcademicDisciplineGroup> AcademicDisciplineGroups { get; set; } = new List<AcademicDisciplineGroup>();

    public virtual Academic Curator { get; set; } = null!;

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual StudyProgram StudyProgram { get; set; } = null!;
}

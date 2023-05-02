using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Speciality
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SpecialityFaculty> SpecialityFaculties { get; set; } = new List<SpecialityFaculty>();

    public virtual ICollection<StudyProgram> StudyPrograms { get; set; } = new List<StudyProgram>();
}

using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class SpecialityFaculty
{
    public Guid Id { get; set; }

    public Guid SpecialityId { get; set; }

    public Guid FacultyId { get; set; }

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual Speciality Speciality { get; set; } = null!;
}

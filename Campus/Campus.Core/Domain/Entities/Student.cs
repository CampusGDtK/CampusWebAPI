using System;
using System.Collections.Generic;

namespace Campus.Core.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid GroupId { get; set; }

    public virtual ICollection<CurrentControl> CurrentControls { get; set; } = new List<CurrentControl>();

    public virtual Group Group { get; set; } = null!;
}

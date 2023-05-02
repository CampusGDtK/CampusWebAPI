﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class Group
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CuratorId { get; set; }

    public Guid StudyProgramId { get; set; }

    public Guid FacultyId { get; set; }

    [ForeignKey("CuratorId")]
    public Academic Curator { get; set; } = null!;

    [ForeignKey("StudyProgramId")]
    public Faculty Faculty { get; set; } = null!;

    [ForeignKey("FacultyId")]
    public StudyProgram StudyProgram { get; set; } = null!;
}

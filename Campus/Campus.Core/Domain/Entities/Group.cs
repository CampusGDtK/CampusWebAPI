using System.ComponentModel.DataAnnotations;
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
    public Academic? Curator { get; set; }

    [ForeignKey("FacultyId")]
    public Faculty? Faculty { get; set; }

    [ForeignKey("StudyProgramId")]
    public StudyProgram? StudyProgram { get; set; }
}

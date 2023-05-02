using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class AcademicDisciplineGroup
{
    [Key]
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public Guid DisciplineId { get; set; }

    public Guid AcademicId { get; set; }

    [ForeignKey("GroupId")]
    public virtual Academic Academic { get; set; } = null!;

    [ForeignKey("DisciplineId")]
    public virtual Discipline Discipline { get; set; } = null!;

    [ForeignKey("AcademicId")]
    public virtual Group Group { get; set; } = null!;
}

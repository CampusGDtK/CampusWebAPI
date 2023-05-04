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
    public Academic? Academic { get; set; }

    [ForeignKey("DisciplineId")]
    public Discipline? Discipline { get; set; }

    [ForeignKey("AcademicId")]
    public Group? Group { get; set; }
}

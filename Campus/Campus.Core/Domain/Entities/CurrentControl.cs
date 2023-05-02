using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class CurrentControl
{
    [Key]
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid DisciplineId { get; set; }

    [Column(TypeName = "jsonb")]
    public string Detail { get; set; } = null!;

    [Column(TypeName = "jsonb")]
    public string Mark { get; set; } = null!;

    public long TotalMark { get; set; }

    [ForeignKey("DisciplineId")]
    public Discipline Discipline { get; set; } = null!;

    [ForeignKey("StudentId")]
    public Student Student { get; set; } = null!;
}

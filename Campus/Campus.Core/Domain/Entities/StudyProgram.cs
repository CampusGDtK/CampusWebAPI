using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class StudyProgram
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid SpecialityId { get; set; }

    [ForeignKey("SpecialityId")]
    public virtual Speciality Speciality { get; set; } = null!;
}

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
    public Speciality? Speciality { get; set; }

    public Guid CathedraId { get; set; }

    [ForeignKey("CathedraId")]
    public Cathedra? Cathedra { get; set; }
}

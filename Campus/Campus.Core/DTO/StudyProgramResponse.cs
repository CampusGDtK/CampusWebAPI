using System.ComponentModel.DataAnnotations;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class StudyProgramResponse
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public Guid SpecialityId { get; set; }

    public Speciality Speciality { get; set; }
}
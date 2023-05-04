using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public sealed class StudyProgramAddRequest
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public Guid SpecialityId { get; set; }
    
    public StudyProgram ToStudyProgram()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            SpecialityId = SpecialityId
        };
    } 
}
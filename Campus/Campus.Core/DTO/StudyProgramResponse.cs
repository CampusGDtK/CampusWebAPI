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

    public string SpecialityName { get; set; }
}

public static partial class EntitiesExtensions
{
    public static StudyProgramResponse ToStudyProgramResponse(this StudyProgram studyProgram)
    {
        return new StudyProgramResponse
        {
            Id = studyProgram.Id,
            Name = studyProgram.Name,
            SpecialityId = studyProgram.SpecialityId,
            SpecialityName = studyProgram.Speciality.Name
        };
    }
}
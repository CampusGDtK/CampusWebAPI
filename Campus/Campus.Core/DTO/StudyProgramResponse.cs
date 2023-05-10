using System.ComponentModel.DataAnnotations;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class StudyProgramResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid SpecialityId { get; set; }

    public string? SpecialityName { get; set; }

    public Guid CathedraId { get; set; }

    public string? CathedraName { get; set; }
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
            SpecialityName = studyProgram.Speciality?.Name,
            CathedraId = studyProgram.CathedraId,
            CathedraName = studyProgram.Cathedra?.Name
        };
    }
}
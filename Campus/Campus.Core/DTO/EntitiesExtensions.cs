using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public static partial class EntitiesExtensions
{
    public static StudentResponse ToResponse(this Student student)
    {
        return new StudentResponse
        {
            Id = student.Id,
            FullName = student.FullName,
            DateOfBirth = student.DateOfBirth,
            PhoneNumber = student.PhoneNumber,
            Email = student.Email,
            GroupId = student.GroupId,
            GroupName = student.Group.Name
        };
    }

    public static DisciplineResponse ToResponse(this Discipline discipline)
    {
        return new DisciplineResponse
        {
            Id = discipline.Id,
            Name = discipline.Name,
            CathedraId = discipline.CathedralId,
            CathedraName = discipline.Cathedral.Name
        };
    }

    public static StudyProgramResponse ToResponse(this StudyProgram studyProgram)
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
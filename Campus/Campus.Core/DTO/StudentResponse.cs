using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class StudentResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = null!;
}

public static partial class EntitiesExtensions
{
    public static StudentResponse ToStudentResponse(this Student student)
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
}
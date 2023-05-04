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
using System.ComponentModel.DataAnnotations;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class StudentUpdateRequest
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(4)]
    public string FullName { get; set; } = null!;
    [Required]
    public DateOnly DateOfBirth { get; set; }
    [Required]
    [StringLength(8)]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public Guid GroupId { get; set; }

    public Student ToStudent()
    {
        return new()
        {
            Id = Id,
            FullName = FullName,
            DateOfBirth = DateOfBirth,
            PhoneNumber = PhoneNumber,
            Email = Email,
            GroupId = GroupId
        };
    }
}
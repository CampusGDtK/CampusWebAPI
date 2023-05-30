using System.ComponentModel.DataAnnotations;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class StudentAddRequest
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = null!;
    [Required]
    public DateOnly DateOfBirth { get; set; }
    [Required]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public Guid GroupId { get; set; }

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    public Student ToStudent()
    {
        return new()
        {
            FullName = FullName,
            DateOfBirth = DateOfBirth,
            PhoneNumber = PhoneNumber,
            Email = Email,
            GroupId = GroupId
        };
    }
}
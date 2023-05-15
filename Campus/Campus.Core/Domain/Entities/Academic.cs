using Campus.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class Academic
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Position { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string Gender { get; set; } = null!;

    public Guid CathedraId { get; set; }

    [ForeignKey("CathedralId")]
    public Cathedra? Cathedra { get; set; }
}

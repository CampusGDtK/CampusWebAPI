using System.ComponentModel.DataAnnotations;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class DisciplineUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(4)]
    public string Name { get; set; } = null!;
    [Required]
    public Guid CathedraId { get; set; }
    
    public Discipline ToDiscipline()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            CathedraId = CathedraId
        };
    }
}
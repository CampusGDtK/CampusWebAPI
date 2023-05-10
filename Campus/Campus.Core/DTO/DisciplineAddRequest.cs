using System.ComponentModel.DataAnnotations;
using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class DisciplineAddRequest
{
    [Required]
    [MaxLength(100)]
    [MinLength(4)]
    public string Name { get; set; }
    [Required]
    public Guid CathedraId { get; set; }
    
    public Discipline ToDiscipline()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            CathedraId = CathedraId
        };
    }
}
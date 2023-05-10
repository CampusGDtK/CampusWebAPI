using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class Discipline
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CathedraId { get; set; }

    [ForeignKey("CathedraId")]
    public Cathedra? Cathedra { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class Discipline
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CathedralId { get; set; }

    [ForeignKey("CathedralId")]
    public Cathedra Cathedral { get; set; } = null!;
}

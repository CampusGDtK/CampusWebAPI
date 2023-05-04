using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Domain.Entities;

public class Cathedra
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Head { get; set; } = null!;

    public Guid FacultyId { get; set; }

    [ForeignKey("FacultyId")]
    public Faculty? Faculty { get; set; }
}

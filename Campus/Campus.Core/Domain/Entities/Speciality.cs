using System.ComponentModel.DataAnnotations;

namespace Campus.Core.Domain.Entities;

public class Speciality
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
}

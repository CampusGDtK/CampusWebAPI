using System.ComponentModel.DataAnnotations;

namespace Campus.Core.Domain.Entities;

public class Faculty
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Dean { get; set; } = null!;
}

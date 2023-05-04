using Campus.Core.Domain.Entities;

namespace Campus.Core.DTO;

public class DisciplineResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid CathedraId { get; set; }
    public string CathedraName { get; set; }
}
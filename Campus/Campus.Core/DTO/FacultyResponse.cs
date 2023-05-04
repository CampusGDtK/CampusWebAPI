using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class FacultyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Dean { get; set; } = null!;
        public Guid? SpecialitiesId { get; set; }
        public string? SpecialityName { get; set; }
    }

    public static partial class EntitiesExtensions
    {
        public static FacultyResponse ToFacultyResponse(this Faculty faculty)
        {
            return new FacultyResponse
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Dean = faculty.Dean
            };
        }
    }
}

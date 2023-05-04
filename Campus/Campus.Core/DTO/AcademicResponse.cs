using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class AcademicResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid CathedraId { get; set; }
        public string? CathedraName { get; set; }
    }

    public static partial class EntitiesExtensions
    {
        public static AcademicResponse ToAcademicResponse(this Academic academic)
        {
            AcademicResponse academicResponse = new AcademicResponse()
            {
                Id = academic.Id,
                Name = academic.Name,
                CathedraId = academic.CathedralId,
                CathedraName = academic.Cathedral?.Name
            };

            return academicResponse;
        }
    }
}

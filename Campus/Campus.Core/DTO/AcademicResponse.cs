using Campus.Core.Domain.Entities;
using Campus.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class AcademicResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
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
                Position = academic.Position,
                Email = academic.Email,
                PhoneNumber = academic.PhoneNumber,
                Gender = academic.Gender,
                CathedraId = academic.CathedraId,
                CathedraName = academic.Cathedra?.Name
            };

            return academicResponse;
        }
    }
}

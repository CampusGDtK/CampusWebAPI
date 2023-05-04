using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class SpecialityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public static partial class EntitiesExtensions
    {
        public static SpecialityResponse ToSpecialityResponse(this Speciality specialty)
        {
            SpecialityResponse specialityResponse = new SpecialityResponse()
            {
                Id = specialty.Id,
                Name = specialty.Name
            };

            return specialityResponse;
        }
    }
}

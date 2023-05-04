using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class FacultuResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Dean { get; set; } = null!;
        public Guid SpecialitiesId { get; set; }
        public string SpecialityName { get; set; } = null!;
    }
}

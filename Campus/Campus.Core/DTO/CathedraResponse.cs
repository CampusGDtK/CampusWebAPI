using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class CathedraResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Head { get; set; } = null!;

        public Guid FacultyId { get; set; }

        public Faculty Faculty { get; set; } = null!;
    }
}

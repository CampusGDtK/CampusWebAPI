using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class CathedraUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Head { get; set; } = null!;

        [Required]
        public Guid FacultyId { get; set; }

        public Cathedra ToCathedra()
        {
            return new Cathedra
            {
                Id = Id,
                FacultyId = FacultyId,
                Name = Name,
                Head = Head
            };
        }
    }
}

using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class FacultyAddRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Dean { get; set; } = null!;
        
        [Required]
        [Range(1,int.MaxValue)]
        public IEnumerable<Guid> Specialities { get; set; } = null!;

        public Faculty ToFaculty()
        {
            return new Faculty
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Dean = Dean
            };
        }
    }
}

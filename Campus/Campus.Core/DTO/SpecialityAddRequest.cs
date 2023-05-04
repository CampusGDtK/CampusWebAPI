using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class SpecialityAddRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public Speciality ToSpeciality()
        {
            Speciality speciality = new Speciality()
            {
                Id = Guid.NewGuid(),
                Name = this.Name
            };

            return speciality;
        }
    }
}

using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class AcademicAddRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        public Guid CathedralId { get; set; }

        public Academic ToAcademic()
        {
            Academic academic = new Academic()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                CathedralId = this.CathedralId
            };

            return academic;
        }
    }
}

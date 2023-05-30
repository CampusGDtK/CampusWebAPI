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
    public class AcademicAddRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Position { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = null!;

        [Required]
        public Guid CathedraId { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        public Academic ToAcademic()
        {
            Academic academic = new Academic()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Position = this.Position,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Gender = this.Gender,
                CathedraId = this.CathedraId
            };

            return academic;
        }
    }
}

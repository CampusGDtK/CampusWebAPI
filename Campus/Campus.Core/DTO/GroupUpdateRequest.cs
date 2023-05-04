using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class GroupUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public Guid CuratorId { get; set; }

        [Required]
        public Guid StudyProgramId { get; set; }

        [Required]
        public Guid FacultyId { get; set; }

        public Group ToGroup()
        {
            return new Group
            {
                Id = Id,
                Name = Name,
                CuratorId = CuratorId,
                StudyProgramId = StudyProgramId,
                FacultyId = FacultyId
            };
        }
    }
}

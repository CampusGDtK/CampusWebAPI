using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class GroupAddRequest
    {
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
                Id = Guid.NewGuid(),
                Name = Name,
                CuratorId = CuratorId,
                StudyProgramId = StudyProgramId,
                FacultyId = FacultyId
            };
        }
    }
}

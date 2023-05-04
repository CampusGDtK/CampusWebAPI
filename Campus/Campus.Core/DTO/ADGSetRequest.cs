using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class ADGSetRequest
    {
        [Required]
        public Guid AcademicId { get; set; }
        [Required]
        [DisplayName("Discipline groups relation")]
        public IDictionary<Guid, IEnumerable<Guid>> DisciplineGroupsRelation { get; set; } = null!;
    }
}

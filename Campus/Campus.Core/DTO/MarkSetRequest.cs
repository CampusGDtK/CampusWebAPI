using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class MarkSetRequest
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public Guid DisciplineId { get; set; }
        [Required]
        public IEnumerable<int> Marks { get; set; } = null!;
    }
}

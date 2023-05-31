using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class MarkResponse
    {
        public Guid StudentId { get; set; }
        public Guid DisciplineId { get; set; }
        public string DisciplineName { get; set; } = string.Empty;
        public IEnumerable<string> Details { get; set; } = null!;
        public IEnumerable<int> Marks { get; set; } = null!;
        public int TotalMark { get; set; }
    }
}

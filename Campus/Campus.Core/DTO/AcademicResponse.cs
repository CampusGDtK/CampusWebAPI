using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class AcademicResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid CathedraId { get; set; }
        public string CathedraName { get; set; } = null!;
    }
}

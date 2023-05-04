using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class GroupResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Guid CuratorId { get; set; }

        public string CuratorName { get; set; } = null!;

        public Guid StudyProgramId { get; set; }

        public string StudyProgramName { get; set; } = null!;

        public Guid FacultyId { get; set; }

        public string FacultyName { get;set; } = null!;
    }

    public static partial class EntitiesExtensions
    {
        public static GroupResponse ToGroupResponse(this Group group)
        {
            return new GroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                CuratorId = group.CuratorId,
                CuratorName = group.Curator?.Name ?? "",
                StudyProgramId = group.StudyProgramId,
                StudyProgramName = group.StudyProgram?.Name ?? "",
                FacultyId = group.FacultyId,
                FacultyName = group.Faculty?.Name ?? ""
            };
        }
    }
}

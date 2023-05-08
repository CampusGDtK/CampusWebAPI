﻿using Campus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.DTO
{
    public class AcademicUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        public Guid CathedralId { get; set; }

        public Academic ToAcademic()
        {
            Academic academic = new Academic()
            {
                Id = this.Id,
                Name = this.Name,
                CathedralId = this.CathedralId
            };

            return academic;
        }
    }

    public static partial class ResponsesExtensions
    {
        public static AcademicUpdateRequest ToAcademicUpdateRequest(this AcademicResponse academicResponse)
        {
            AcademicUpdateRequest academicUpdateRequest = new AcademicUpdateRequest()
            {
                Id = academicResponse.Id,
                Name = academicResponse.Name,
                CathedralId = academicResponse.CathedraId
            };

            return academicUpdateRequest;
        }
    }
}

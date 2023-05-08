using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IAcademicService
    {
        Task<AcademicResponse> GetById(Guid academicId);
        Task<IEnumerable<AcademicResponse>> GetAll();
        Task<IEnumerable<AcademicResponse>> GetByCathedraId(Guid cathedraId);
        Task<AcademicResponse> Add(AcademicAddRequest academicAddRequest);
        Task<AcademicResponse> Update(AcademicUpdateRequest academicUpdateRequest);
        Task Delete(Guid academicId);
    }
}

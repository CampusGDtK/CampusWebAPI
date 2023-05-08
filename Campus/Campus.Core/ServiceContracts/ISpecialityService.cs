using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface ISpecialityService
    {
        Task<SpecialityResponse?> GetSpecialityById(Guid specialityId);
        Task<IEnumerable<SpecialityResponse>> GetAll();
        Task<IEnumerable<SpecialityResponse>> GetByFacultyId(Guid facultyId);
        Task<SpecialityResponse> Add(SpecialityAddRequest specialityAddRequest);
        Task<SpecialityResponse> Update(SpecialityUpdateRequest specialityUpdateRequest);
        Task Remove(Guid specialityId);
    }
}

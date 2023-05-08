using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IFacultyService
    {
        Task<IEnumerable<FacultyResponse>> GetAll();
        Task<FacultyResponse> GetById(Guid facultyId);
        Task<FacultyResponse> Add(FacultyAddRequest? facultyAddRequest);
        Task Remove(Guid facultyId);
        Task<FacultyResponse> Update(FacultyUpdateRequest? facultyUpdateRequest);
    }
}

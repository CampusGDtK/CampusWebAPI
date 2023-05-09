using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponse>> GetAll();
        Task<StudentResponse> GetById(Guid id);
        Task<IEnumerable<StudentResponse>> GetByGroupId(Guid groupId);
        Task<StudentResponse> Create(StudentAddRequest request);
        Task<StudentResponse> Update(StudentUpdateRequest request);
        Task Delete(Guid id);
    }
}

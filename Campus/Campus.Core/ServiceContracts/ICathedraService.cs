using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface ICathedraService
    {
        Task<CathedraResponse> GetById(Guid cathedraId);
        Task<IEnumerable<CathedraResponse>> GetAll();
        Task<CathedraResponse> GetByFacultyId(Guid facultyId);
        Task<IEnumerable<CathedraResponse>> Add(CathedraAddRequest cathedraAddRequest);
        Task<CathedraResponse> Update(CathedraUpdateRequest cathedraUpdateRequest);
        Task Delete(Guid cathedraId);
    }
}

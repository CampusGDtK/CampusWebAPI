using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IDisciplineService
    {
        Task<DisciplineResponse?> GetDisciplineById(Guid disciplineId);
        Task<IEnumerable<DisciplineResponse>> GetAll();
        Task<IEnumerable<DisciplineResponse>> GetByCathedraId(Guid cathedraId);
        Task<DisciplineResponse> Add(DisciplineAddRequest disciplineAddRequest);
        Task<DisciplineResponse> Update(DisciplineUpdateRequest disciplineUpdateRequest);
        Task Remove(Guid disciplineId);
    }
}

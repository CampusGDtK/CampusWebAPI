using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IGroupService
    {
        Task<GroupResponse> GetById(Guid groupId);
        Task<IEnumerable<GroupResponse>> GetAll();
        Task<IEnumerable<GroupResponse>> GetByFacultyId(Guid facultyId);
        Task<GroupResponse> Add(GroupAddRequest groupAddRequest);
        Task<GroupResponse> Update(GroupUpdateRequest groupUpdateRequest);
        Task Delete(Guid groupId);
    }
}

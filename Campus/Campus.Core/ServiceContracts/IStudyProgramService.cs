using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IStudyProgramService
    {
        Task<StudyProgramResponse> GetById(Guid studyProgramId);
        Task<IEnumerable<StudyProgramResponse>> GetAll();
        Task<IEnumerable<StudyProgramResponse>> GetByCathedraId(Guid cathedraId);
        Task<IEnumerable<StudyProgramResponse>> GetBySpecialityId(Guid specialityId);
        Task<IEnumerable<StudyProgramResponse>> GetBySpecialityAndCathedraId(Guid cathedraId,Guid specialityId);
        Task<StudyProgramResponse> Add(StudyProgramAddRequest studyProgramAddRequest);
        Task<StudyProgramResponse> Update(StudyProgramUpdateRequest studyProgramUpdateRequest);
        Task Delete(Guid studyProgramId);
    }
}

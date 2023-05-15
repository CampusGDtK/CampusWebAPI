using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.Services
{
    public class AcademicService : IAcademicService
    {
        private readonly IRepository<Academic> _academicRepository;
        private readonly IRepository<Cathedra> _cathedraRepository;

        public AcademicService(IRepository<Academic> academicRepository, IRepository<Cathedra> cathedraRepository)
        {
            _academicRepository = academicRepository;
            _cathedraRepository = cathedraRepository;
        }

        public async Task<AcademicResponse> Add(AcademicAddRequest academicAddRequest)
        {
            if(academicAddRequest == null)
                throw new ArgumentNullException(nameof(academicAddRequest));

            ValidationHelper.ModelValidation(academicAddRequest);

            if (await _cathedraRepository.GetValueById(academicAddRequest.CathedraId) is null)
                throw new KeyNotFoundException(nameof(academicAddRequest.CathedraId));

            Academic academic = academicAddRequest.ToAcademic();
            academic.Id = Guid.NewGuid();

            await _academicRepository.Create(academic);

            return academic.ToAcademicResponse();
        }

        public async Task Remove(Guid academicId)
        {
            bool result = await _academicRepository.Delete(academicId);

            if(!result)
                throw new KeyNotFoundException(nameof(academicId));
        }

        public async Task<IEnumerable<AcademicResponse>> GetAll()
        {
            IEnumerable<Academic> academics = await _academicRepository.GetAll();

            return academics.Select(academic => academic.ToAcademicResponse());
        }

        public async Task<IEnumerable<AcademicResponse>> GetByCathedraId(Guid cathedraId)
        {
            if(await _cathedraRepository.GetValueById(cathedraId) is null)
                throw new KeyNotFoundException(nameof(cathedraId));

            List<Academic> academics = (await _academicRepository.GetAll())
                .Where(academic => academic.CathedraId == cathedraId)
                .ToList();

            return academics.Select(academic => academic.ToAcademicResponse());
        }

        public async Task<AcademicResponse> GetAcademicById(Guid academicId)
        {
            Academic? academic = await _academicRepository.GetValueById(academicId);

            if (academic == null)
                throw new KeyNotFoundException(nameof(academicId));

            return academic.ToAcademicResponse();
        }

        public async Task<AcademicResponse> Update(AcademicUpdateRequest academicUpdateRequest)
        {
            if(academicUpdateRequest == null)
                throw new ArgumentNullException(nameof(academicUpdateRequest));

            ValidationHelper.ModelValidation(academicUpdateRequest);

            if (await _cathedraRepository.GetValueById(academicUpdateRequest.CathedraId) is null)
                throw new KeyNotFoundException(nameof(academicUpdateRequest.CathedraId));

            Academic academic = academicUpdateRequest.ToAcademic();
            Academic? academicUpdated = await _academicRepository.Update(academic);

            if (academicUpdated == null)
                throw new KeyNotFoundException(nameof(academicUpdateRequest.Id));

            return academicUpdated.ToAcademicResponse();
        }
    }
}

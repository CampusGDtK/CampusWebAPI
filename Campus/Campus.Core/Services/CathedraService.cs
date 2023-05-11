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
    public class CathedraService : ICathedraService
    {
        private readonly IRepository<Cathedra> _cathedraRepository;
        private readonly IRepository<Faculty> _facultyRepository;

        public CathedraService(IRepository<Cathedra> cathedraRepository, IRepository<Faculty> facultyRepository)
        {
            _cathedraRepository = cathedraRepository;
            _facultyRepository = facultyRepository;
        }

        public async Task<CathedraResponse> Add(CathedraAddRequest cathedraAddRequest)
        {
            if (cathedraAddRequest == null)
                throw new ArgumentNullException(nameof(cathedraAddRequest));

            ValidationHelper.ModelValidation(cathedraAddRequest);

            if(await _facultyRepository.GetValueById(cathedraAddRequest.FacultyId) == null)
                throw new KeyNotFoundException(nameof(cathedraAddRequest.FacultyId));

            Cathedra cathedra = cathedraAddRequest.ToCathedra();

            cathedra.Id = Guid.NewGuid();

            await _cathedraRepository.Create(cathedra);

            return cathedra.ToCathedraResponse();
        }   

        public async Task Delete(Guid cathedraId)
        {
            var result = await _cathedraRepository.Delete(cathedraId);

            if (!result)
                throw new KeyNotFoundException(nameof(cathedraId));
        }

        public async Task<IEnumerable<CathedraResponse>> GetAll()
        {
            var result = await _cathedraRepository.GetAll();
            return result.Select(x => x.ToCathedraResponse());
        }

        public async Task<IEnumerable<CathedraResponse>> GetByFacultyId(Guid facultyId)
        {
            if (await _facultyRepository.GetValueById(facultyId) == null)
                throw new KeyNotFoundException(nameof(facultyId));

            var cathedras = await _cathedraRepository.GetAll();

            cathedras = cathedras.Where(x => x.FacultyId == facultyId);

            return cathedras.Select(x => x.ToCathedraResponse());
        }

        public async Task<CathedraResponse> GetById(Guid cathedraId)
        {
            var cathedra = await _cathedraRepository.GetValueById(cathedraId);

            if (cathedra == null)
                throw new KeyNotFoundException(nameof(cathedraId));

            return cathedra.ToCathedraResponse();
        }

        public async Task<CathedraResponse> Update(CathedraUpdateRequest cathedraUpdateRequest)
        {
            if (cathedraUpdateRequest == null)
                throw new ArgumentNullException(nameof(cathedraUpdateRequest));

            ValidationHelper.ModelValidation(cathedraUpdateRequest);

            if (await _facultyRepository.GetValueById(cathedraUpdateRequest.FacultyId) == null)
                throw new KeyNotFoundException(nameof(cathedraUpdateRequest.FacultyId));

            var cathedra = cathedraUpdateRequest.ToCathedra();

            var result = await _cathedraRepository.Update(cathedra);

            return result.ToCathedraResponse();
        }
    }
}

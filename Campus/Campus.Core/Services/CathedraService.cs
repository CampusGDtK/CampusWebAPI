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
                throw new ArgumentNullException("CathedraAddRequest is null");

            ValidationHelper.ModelValidation(cathedraAddRequest);

            if(await _facultyRepository.GetValueById(cathedraAddRequest.FacultyId) == null)
                throw new KeyNotFoundException("Id of faculty not found");

            Cathedra cathedra = cathedraAddRequest.ToCathedra();

            cathedra.Id = Guid.NewGuid();

            await _cathedraRepository.Create(cathedra);

            return cathedra.ToCathedraResponse();
        }   

        public async Task Delete(Guid cathedraId)
        {
            var result = await _cathedraRepository.Delete(cathedraId);

            if (!result)
                throw new KeyNotFoundException("Id of cathedra not found");
        }

        public async Task<IEnumerable<CathedraResponse>> GetAll()
        {
            var result = await _cathedraRepository.GetAll();
            return result.Select(x => x.ToCathedraResponse());
        }

        public async Task<IEnumerable<CathedraResponse>> GetByFacultyId(Guid facultyId)
        {
            if (await _facultyRepository.GetValueById(facultyId) == null)
                throw new KeyNotFoundException("Id of faculty not found");

            var cathedras = await _cathedraRepository.GetAll();

            cathedras = cathedras.Where(x => x.FacultyId == facultyId);

            return cathedras.Select(x => x.ToCathedraResponse());
        }

        public async Task<CathedraResponse> GetById(Guid cathedraId)
        {
            var cathedra = await _cathedraRepository.GetValueById(cathedraId);

            if (cathedra == null)
                throw new KeyNotFoundException("Id of cathedra not found");

            return cathedra.ToCathedraResponse();
        }

        public async Task<CathedraResponse> Update(CathedraUpdateRequest cathedraUpdateRequest)
        {
            if (cathedraUpdateRequest == null)
                throw new ArgumentNullException("CathderaUpdateRequest is null");

            ValidationHelper.ModelValidation(cathedraUpdateRequest);

            var faculty = await _facultyRepository.GetValueById(cathedraUpdateRequest.FacultyId);

            if (faculty == null)
                throw new KeyNotFoundException("Id of faculty not found");

            var cathedra = cathedraUpdateRequest.ToCathedra();

            var result = await _cathedraRepository.Update(cathedra);

            if (result is null)
                throw new KeyNotFoundException("Id of cathedra not found");

            result.Faculty = faculty;

            return result.ToCathedraResponse();
        }
    }
}

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
    public class FacultyService : IFacultyService
    {
        private readonly IRepository<Faculty> _facultyRepository;

        public FacultyService(IRepository<Faculty> facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public async Task<FacultyResponse> Add(FacultyAddRequest? facultyAddRequest)
        {
            if (facultyAddRequest == null) 
                throw new ArgumentNullException();

            ValidationHelper.ModelValidation(facultyAddRequest);

            var faculty = facultyAddRequest.ToFaculty();

            faculty.Id = Guid.NewGuid();

            await _facultyRepository.Create(faculty);

            return faculty.ToFacultyResponse();

        }

        public async Task<IEnumerable<FacultyResponse>> GetAll()
        {
            var faculties = await _facultyRepository.GetAll();

            return faculties.Select(x => x.ToFacultyResponse());
        }

        public async Task<FacultyResponse> GetById(Guid facultyId)
        {
            var result = await _facultyRepository.GetValueById(facultyId);

            if (result is null)
                throw new KeyNotFoundException(nameof(result));

            return result.ToFacultyResponse();
        }

        public async Task<bool> Remove(Guid facultyId)
        {
            var result = await _facultyRepository.Delete(facultyId);

            if(!result)
                throw new KeyNotFoundException(nameof(result));

            return result;
        }

        public async Task<FacultyResponse> Update(FacultyUpdateRequest facultyUpdateRequest)
        {
            ValidationHelper.ModelValidation(facultyUpdateRequest);

            var updatedFaculty = await _facultyRepository.Update(facultyUpdateRequest.ToFaculty());

            if (updatedFaculty == null)
                throw new KeyNotFoundException();

            return updatedFaculty.ToFacultyResponse();
        }
    }
}

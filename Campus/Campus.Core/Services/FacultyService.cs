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
        private readonly IRepository<SpecialityFaculty> _specialityFacultyRepository;
        private readonly ISpecialityService _specialityService;

        public FacultyService(IRepository<Faculty> facultyRepository, IRepository<SpecialityFaculty> specialityFacultyRepository, ISpecialityService specialityService)
        {
            _facultyRepository = facultyRepository;
            _specialityFacultyRepository = specialityFacultyRepository;
            _specialityService = specialityService;
        }

        public async Task<FacultyResponse> Add(FacultyAddRequest? facultyAddRequest)
        {
            if (facultyAddRequest == null)
                throw new ArgumentNullException("FacultyAddRequest is null");

            ValidationHelper.ModelValidation(facultyAddRequest);

            var faculty = facultyAddRequest.ToFaculty();

            faculty.Id = Guid.NewGuid();

            await _facultyRepository.Create(faculty);

            foreach (Guid specialityId in facultyAddRequest.Specialities)
            {
                SpecialityFaculty specialityFaculty = new SpecialityFaculty()
                {
                    Id = Guid.NewGuid(),
                    FacultyId = faculty.Id,
                    SpecialityId = specialityId
                };

                await _specialityFacultyRepository.Create(specialityFaculty);
            }

            return await GetFacultyResponse(faculty);
        }

        public async Task<IEnumerable<FacultyResponse>> GetAll()
        {
            var faculties = await _facultyRepository.GetAll();

            List<FacultyResponse> facultiesResponse = new List<FacultyResponse>();

            foreach (var faculty in faculties)
            {                
                facultiesResponse.Add(await GetFacultyResponse(faculty));
            }           

            return facultiesResponse;
        }

        public async Task<FacultyResponse> GetById(Guid facultyId)
        {
            var result = await _facultyRepository.GetValueById(facultyId);

            if (result is null)
                throw new KeyNotFoundException("Id of faclulty not found");

            return await GetFacultyResponse(result);
        }

        public async Task Remove(Guid facultyId)
        {
            var result = await _facultyRepository.Delete(facultyId);

            if (!result)
                throw new KeyNotFoundException("Id of faclulty not found");
        }

        public async Task<FacultyResponse> Update(FacultyUpdateRequest facultyUpdateRequest)
        {
            if (facultyUpdateRequest == null)
                throw new ArgumentNullException("FacultyUpdateRequest is null");

            ValidationHelper.ModelValidation(facultyUpdateRequest);

            var updatedFaculty = await _facultyRepository.Update(facultyUpdateRequest.ToFaculty());

            if (updatedFaculty == null)
                throw new KeyNotFoundException("Id of faclulty not found");

            var specialitiesFacultiesId = (await _specialityFacultyRepository.GetAll()).Where(x => x.FacultyId == updatedFaculty.Id).Select(x => x.Id);

            foreach (var id in specialitiesFacultiesId)
            {
                await _specialityFacultyRepository.Delete(id);
            }

            foreach (Guid specialityId in facultyUpdateRequest.Specialities)
            {
                SpecialityFaculty specialityFaculty = new SpecialityFaculty()
                {
                    Id = Guid.NewGuid(),
                    FacultyId = updatedFaculty.Id,
                    SpecialityId = specialityId
                };

                await _specialityFacultyRepository.Create(specialityFaculty);
            }

            return await GetFacultyResponse(updatedFaculty);
        }

        private async Task<FacultyResponse> GetFacultyResponse(Faculty faculty)
        {
            var facultyResponse = faculty.ToFacultyResponse();

            var specialitiesByFaculty = await _specialityService.GetByFacultyId(faculty.Id);

            facultyResponse.SpecialitiesId = specialitiesByFaculty?.Select(x => x.Id);

            facultyResponse.SpecialitiesName = specialitiesByFaculty?.Select(x => x.Name);

            return facultyResponse;
        }
    }
}

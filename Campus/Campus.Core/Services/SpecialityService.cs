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
    public class SpecialityService : ISpecialityService
    {
        private readonly IRepository<Speciality> _specialityRepository;
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IRepository<SpecialityFaculty> _specialityFacultyRepository;

        public SpecialityService(IRepository<Speciality> specialityRepository, IRepository<Faculty> facultyRepository, IRepository<SpecialityFaculty> specialityFacultyRepository)
        {
            _specialityRepository = specialityRepository;
            _facultyRepository = facultyRepository;
            _specialityFacultyRepository = specialityFacultyRepository;
        }

        public async Task<SpecialityResponse> Add(SpecialityAddRequest specialityAddRequest)
        {
            if(specialityAddRequest== null) 
                throw new ArgumentNullException("SpecialityAddRequest is null");

            ValidationHelper.ModelValidation(specialityAddRequest);

            Speciality speciality = specialityAddRequest.ToSpeciality();
            speciality.Id = Guid.NewGuid();

            await _specialityRepository.Create(speciality);

            return speciality.ToSpecialityResponse();
        }

        public async Task<IEnumerable<SpecialityResponse>> GetAll()
        {
            return (await _specialityRepository.GetAll())
                .Select(speciality => speciality.ToSpecialityResponse());
        }

        public async Task<IEnumerable<SpecialityResponse>> GetByFacultyId(Guid facultyId)
        {
            if(await _facultyRepository.GetValueById(facultyId) is null)
                throw new KeyNotFoundException("Id of faculty not found");

            List<SpecialityFaculty> specialtyFaculties = (await _specialityFacultyRepository.GetAll())
                .Where(specialtyFaculty => specialtyFaculty.FacultyId == facultyId)
                .Where(specialtyFaculty => specialtyFaculty.Speciality != null)
                .ToList();

            return specialtyFaculties.Select(specialtyFaculty => specialtyFaculty.Speciality.ToSpecialityResponse());
        }

        public async Task<SpecialityResponse> GetSpecialityById(Guid specialityId)
        {
            Speciality? speciality = await _specialityRepository.GetValueById(specialityId);

            if (speciality == null)
                throw new KeyNotFoundException("Id of speciality not found");

            return speciality.ToSpecialityResponse();
        }

        public async Task Remove(Guid specialityId)
        {
            bool result = await _specialityRepository.Delete(specialityId);

            if (!result)
                throw new KeyNotFoundException("Id of speciality not found");
        }

        public async Task<SpecialityResponse> Update(SpecialityUpdateRequest specialityUpdateRequest)
        {
            if(specialityUpdateRequest == null)
                throw new ArgumentNullException("SpecialityUpdateRequest is required");

            ValidationHelper.ModelValidation(specialityUpdateRequest);

            Speciality specialityToUpdate = specialityUpdateRequest.ToSpeciality();

            Speciality? specialityUpdated = await _specialityRepository.Update(specialityToUpdate);

            if (specialityUpdated == null)
                throw new KeyNotFoundException("Id of speciality not found");

            return specialityUpdated.ToSpecialityResponse();
        }
    }
}

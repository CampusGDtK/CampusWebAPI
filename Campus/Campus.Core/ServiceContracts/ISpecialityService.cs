using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing the specialities.
    /// </summary>
    public interface ISpecialityService
    {
        /// <summary>
        /// Method to read speciality from data source by it`s Id.
        /// </summary>
        /// <param name="specialityId">Guid of speciality to read.</param>
        /// <returns>SpecialityResponse object, null - if speciality with passed Id does not exsist.</returns>
        Task<SpecialityResponse> GetSpecialityById(Guid specialityId);

        /// <summary>
        /// Method to read all specialities from data source.
        /// </summary>
        /// <returns>Collection IEnumerable of SpecialityResponse.</returns>
        Task<IEnumerable<SpecialityResponse>> GetAll();

        /// <summary>
        /// Method to filter specialities by faculty Id.
        /// </summary>
        /// <param name="facultyId">Guid of faculty to filter specialities.</param>
        /// <returns>Collection IEnumerable of SpecialityResponse.</returns>
        Task<IEnumerable<SpecialityResponse>> GetByFacultyId(Guid facultyId);

        /// <summary>
        /// Method to add new speciality to the data source.
        /// </summary>
        /// <param name="specialityAddRequest">Object of SpecialityAddRequest to add to the data source.</param>
        /// <returns>Object of SpecialityResponse added to the data source.</returns>
        Task<SpecialityResponse> Add(SpecialityAddRequest specialityAddRequest);

        /// <summary>
        /// Method to update existing specialty in the data source.
        /// </summary>
        /// <param name="specialityUpdateRequest">Object of SpecialityUpdateRequest to update.</param>
        /// <returns>Object of SpecialityResponse updated in the data source.</returns>
        Task<SpecialityResponse> Update(SpecialityUpdateRequest specialityUpdateRequest);

        /// <summary>
        /// Method to remove existing speciality from data source by it`s Id.
        /// </summary>
        /// <param name="specialityId">Guid of speciality to remove.</param>
        Task Remove(Guid specialityId);
    }
}

using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing academics.
    /// </summary>
    public interface IAcademicService
    {
        /// <summary>
        /// Method to read academic from data source by it`s Id.
        /// </summary>
        /// <param name="academicId">Guid of academic to read.</param>
        /// <returns>Academic response with entered Id.</returns>
        Task<AcademicResponse> GetAcademicById(Guid academicId);

        /// <summary>
        /// Method to read all academics from data source.
        /// </summary>
        /// <returns>Collection IEnumerable of AcademicResponse.</returns>
        Task<IEnumerable<AcademicResponse>> GetAll();

        /// <summary>
        /// Method to filter academics by cathedra.
        /// </summary>
        /// <param name="cathedraId">Guid of cathedra to filter academics.</param>
        /// <returns>Collection IEnumerable of AcademicResponse objects filtered by cathedra.</returns>
        Task<IEnumerable<AcademicResponse>> GetByCathedraId(Guid cathedraId);

        /// <summary>
        /// Method to add new academic to the data source.
        /// </summary>
        /// <param name="academicAddRequest">AcademicAddRequest object with details of academic to add.</param>
        /// <returns>AcademicResponse object added to the data source.</returns>
        Task<AcademicResponse> Add(AcademicAddRequest academicAddRequest);

        /// <summary>
        /// Method to update existing academic in the data source.
        /// </summary>
        /// <param name="academicUpdateRequest">AcademicUpdateRequest with details of academic to update.</param>
        /// <returns>AcademicReponse updated in the data source.</returns>
        Task<AcademicResponse> Update(AcademicUpdateRequest academicUpdateRequest);

        /// <summary>
        /// Method to remoe academic from data source by it`s Id.
        /// </summary>
        /// <param name="academicId">Guid of academic to remove.</param>
        Task Remove(Guid academicId);
    }
}

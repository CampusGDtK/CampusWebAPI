using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    /// <summary>
    /// Service for syllabus managment.
    /// </summary>
    public interface ISyllabusService
    {
        /// <summary>
        /// Method for reading syllabus by academic and discipline Id.
        /// </summary>
        /// <param name="academicId">Guid of academic.</param>
        /// <param name="disciplineId">Guid of discipline.</param>
        /// <returns>Collection IEnumerable of details of syllabus.</returns>
        Task<IEnumerable<string>> GetSyllabus(Guid academicId, Guid disciplineId);
        
        /// <summary>
        /// Method for set new syllabus, or add new details to existing one.
        /// </summary>
        /// <param name="academicId">Guid of academic.</param>
        /// <param name="disciplineId">Guid of discipline.</param>
        /// <param name="syllabus">Details of syllabus.</param>
        Task SetSyllabus(Guid academicId, Guid disciplineId, IEnumerable<string> syllabus);

        /// <summary>
        /// Method for deleting syllabus of academic for descipline.
        /// </summary>
        /// <param name="academicId">Guid of academic.</param>
        /// <param name="disciplineId">Guid of discipline.</param>
        Task DeleteSyllabus(Guid academicId, Guid disciplineId);
    }
}

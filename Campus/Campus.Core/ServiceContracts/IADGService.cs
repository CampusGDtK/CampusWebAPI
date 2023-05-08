using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    /// <summary>
    /// Service to manage relation between academics, disciplines and groups .
    /// </summary>
    public interface IADGService
    {
        /// <summary>
        /// Method for reading disciplines of academic with passed guid.
        /// </summary>
        /// <param name="academicId">Id of academic to read his disciplines.</param>
        /// <returns>Collection IEnumerable of disciplines of academic.</returns>
        Task<IEnumerable<DisciplineResponse>> GetDisciplinesByAcademicId(Guid academicId);

        /// <summary>
        /// Method for reading groups which is teached by academic with passed id discipline with passed id.
        /// </summary>
        /// <param name="academicId">Id of academic.</param>
        /// <param name="disciplineId">Id of discipline to read all groups.</param>
        /// <returns>Collection IEnumerable of groups which are teached discipline by academic.</returns>
        Task<IEnumerable<GroupResponse>> GetGroupsByDisciplineAndAcademicId(Guid academicId, Guid disciplineId);
        
        /// <summary>
        /// Method to set realation between academic, disciplines and groups.
        /// </summary>
        /// <param name="setRequest">Object of ASGSetRequest with data for setting relation.</param>
        Task SetRelation(ADGSetRequest setRequest);

        /// <summary>
        /// Method to reset realation between academic, disciplines and groups.
        /// </summary>
        /// <param name="academicId">Guid of academic to reset his realtion with disciplines and groups.</param>
        Task ResetRelation(Guid academicId);
    }
}

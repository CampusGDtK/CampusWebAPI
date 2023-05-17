using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    /// <summary>
    /// Service for marking students.
    /// </summary>
    public interface IMarkingService
    {
        /// <summary>
        /// Method to read marks of student for all disciplines. 
        /// </summary>
        /// <param name="studentId">Guid of student to read marks.</param>
        /// <returns>Collection IEnumerable of MarkResponse objects of students for all disciplines.</returns>
        Task<IEnumerable<MarkResponse>> GetByStudentId(Guid studentId);

        /// <summary>
        /// Method to read mark of student for discipline.
        /// </summary>
        /// <param name="studentId">Guid of student.</param>
        /// <param name="disciplineId">Guid of discipline</param>
        /// <returns>Mark response with all marks of student for discipline.</returns>
        Task<MarkResponse> GetByStudentAndDisciplineId(Guid studentId, Guid disciplineId);

        /// <summary>
        /// Method to set mark to student for discipline.
        /// </summary>
        /// <param name="markSetRequest">Object of MarkSetRequest that contains student and discipline Id and marks to set.</param>
        /// <returns>Marks that has been set.</returns>
        Task<MarkResponse> SetMark(MarkSetRequest markSetRequest);

        /// <summary>
        ///  Method to read mark of all students in group for discipline.
        /// </summary>
        /// <param name="groupId">Guid of group</param>
        /// <param name="disciplineId">Guid of discipline</param>
        /// <returns> Enumerable of mark response with all marks of all students in specified group for discipline</returns>
        Task<IEnumerable<MarkResponse>> GetByGroupAndDisciplineId(Guid groupId, Guid disciplineId);
    }
}

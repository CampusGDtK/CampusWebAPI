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
        /// Method to delete mark o student for discipline.
        /// </summary>
        /// <param name="studentId">Guid of student.</param>
        /// <param name="disciplineId">Guid of discipline.</param>
        Task DeleteMark(Guid studentId, Guid disciplineId);
    }
}

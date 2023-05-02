using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Repository for data base.
    /// </summary>
    /// <typeparam name="T">Entity in data base.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Method for reading entity from data base by it`s guid.
        /// </summary>
        /// <param name="id">Guid of entity to read.</param>
        /// <returns>Object of entity with passed guid, null - if entity with passed guid does not exist.</returns>
        Task<T?> GetValueById(Guid id);

        /// <summary>
        /// Method for reading all entities from data base.
        /// </summary>
        /// <returns>Collection IEnumerable of all entities from data base.</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Method for creating new entity in data base.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity</returns>
        Task Create(T entity);

        /// <summary>
        /// Method for updating entity in data base.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity, null - if entity with passed guid does not exist.</returns>
        Task<T?> Update(T entity);

        /// <summary>
        /// Mehtod for deleting entity from data base.
        /// </summary>
        /// <param name="id">Guid of entity to delete.</param>
        /// <returns>True - if deleting is successful, otherwise - false.</returns>
        Task<bool> Delete(Guid id);
    }
}

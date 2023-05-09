using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            //Model validations
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }

        internal async static Task ValidateADGSetRequest(ADGSetRequest setRequest, IRepository<Academic> academicRepository,
        IRepository<Discipline> disciplineRepository, IRepository<Group> groupsRepository)
        {
            Academic? academic = await academicRepository.GetValueById(setRequest.AcademicId);

            if (academic == null)
                throw new KeyNotFoundException(nameof(setRequest.AcademicId));

            foreach (KeyValuePair<Guid, IEnumerable<Guid>> pair in setRequest.DisciplineGroupsRelation)
            {
                Discipline? discipline = await disciplineRepository.GetValueById(pair.Key);

                if (discipline == null)
                    throw new KeyNotFoundException("dicsiplie");

                foreach (Guid groupId in pair.Value)
                {
                    Group? group = await groupsRepository.GetValueById(groupId);

                    if (group == null)
                        throw new KeyNotFoundException("group");
                }
            }
        }
    }
}

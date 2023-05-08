using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.Services
{
    public class ADGService : IADGService
    {
        private readonly IRepository<AcademicDisciplineGroup> _adgRepository;
        private readonly IRepository<Academic> _academicRepository;
        private readonly IRepository<Discipline> _disciplineRepository;
        private readonly IRepository<Group> _groupsRepository;

        public ADGService(IRepository<AcademicDisciplineGroup> adgRepository, IRepository<Academic> academicRepository,
        IRepository<Discipline> disciplineRepository, IRepository<Group> groupsRepository)
        {
            _adgRepository = adgRepository;
            _academicRepository = academicRepository;
            _disciplineRepository = disciplineRepository;
            _groupsRepository = groupsRepository;
        }
        
        public async Task<IEnumerable<DisciplineResponse>> GetDisciplinesByAcademicId(Guid academicId)
        {
            Academic? academic = await _academicRepository.GetValueById(academicId);

            if (academic == null)
            {
                throw new KeyNotFoundException(nameof(academicId));
            }

            List<Discipline?> disciplines = (await _adgRepository.GetAll())
                .Where(adg => adg.AcademicId == academicId)
                .Where(adg => adg.Discipline != null)
                .Select(adg => adg.Discipline)
                .ToList();

            return disciplines.Select(discipline => discipline.ToDisciplineResponseResponse());
        }

        public async Task<IEnumerable<GroupResponse>> GetGroupsByDisciplineAndAcademicId(Guid academicId, Guid disciplineId)
        {
            Academic? academic = await _academicRepository.GetValueById(academicId);

            if(academic == null)
            {
                throw new KeyNotFoundException(nameof(academicId));
            }

            Discipline? discipline = await _disciplineRepository.GetValueById(disciplineId);

            if(discipline == null)
            {
                throw new KeyNotFoundException(nameof(discipline));
            }

            List<AcademicDisciplineGroup> relation = (await _adgRepository.GetAll())
                .Where(adg => adg.AcademicId == academicId && adg.DisciplineId == disciplineId)
                .ToList();

            if(relation.Count == 0)
            {
                throw new ArgumentException("Academic and discipline have no relation.");
            }

            List<Group> groups = relation
                .Where(adg => adg.Group != null)
                .Select(adg => adg.Group)
                .ToList();

            return groups.Select(group => group.ToGroupResponse());
        }

        public async Task SetRelation(ADGSetRequest setRequest)
        {
            if(setRequest == null)
                throw new ArgumentNullException(nameof(setRequest));

            //Validating keys
            Academic? academic = await _academicRepository.GetValueById(setRequest.AcademicId);

            if (academic == null)
                throw new KeyNotFoundException(nameof(setRequest.AcademicId));

            foreach(KeyValuePair<Guid, IEnumerable<Guid>> pair in setRequest.DisciplineGroupsRelation)
            {
                Discipline? discipline = await _disciplineRepository.GetValueById(pair.Key);

                if(discipline == null)
                    throw new KeyNotFoundException("dicsiplie");

                foreach(Guid groupId in pair.Value)
                {
                    Group? group = await _groupsRepository.GetValueById(groupId);

                    if (group == null)
                        throw new KeyNotFoundException("group");
                }
            }

            //Proceeding
            foreach(KeyValuePair<Guid, IEnumerable<Guid>> pair in setRequest.DisciplineGroupsRelation)
            {
                foreach(Guid groupId in pair.Value)
                {
                    AcademicDisciplineGroup adg = new AcademicDisciplineGroup()
                    {
                        Id = Guid.NewGuid(),
                        AcademicId = setRequest.AcademicId,
                        DisciplineId = pair.Key,
                        GroupId = groupId,
                    };

                    await _adgRepository.Create(adg);
                }
            }
        }

        public async Task ResetRelation(Guid academicId)
        {
            Academic? academic = await _academicRepository.GetValueById(academicId);

            if (academic == null)
                throw new KeyNotFoundException(nameof(academicId));

            List<AcademicDisciplineGroup> relation = (await _adgRepository.GetAll())
                .Where(adg => adg.AcademicId == academicId)
                .ToList();

            if(relation.Count == 0)
                throw new ArgumentException("Academic has no relations.");

            foreach(AcademicDisciplineGroup adg in relation)
            {
                await _adgRepository.Delete(adg.Id);
            }
        }
    }
}

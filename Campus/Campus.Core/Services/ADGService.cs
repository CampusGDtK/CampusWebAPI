﻿using Campus.Core.Domain.Entities;
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
    public class ADGService : IADGService
    {
        private readonly IRepository<AcademicDisciplineGroup> _adgRepository;
        private readonly IRepository<Academic> _academicRepository;
        private readonly IRepository<Discipline> _disciplineRepository;
        private readonly IRepository<Group> _groupsRepository;
        private readonly IRepository<CurrentControl> _currentControlRepository;

        private readonly IRepository<Student> _studentRepository;

        public ADGService(IRepository<AcademicDisciplineGroup> adgRepository, IRepository<Academic> academicRepository,
        IRepository<Discipline> disciplineRepository, IRepository<Group> groupsRepository, 
        IRepository<CurrentControl> currentControlRepository, IRepository<Student> studentRepository)
        {
            _adgRepository = adgRepository;
            _academicRepository = academicRepository;
            _disciplineRepository = disciplineRepository;
            _groupsRepository = groupsRepository;
            _currentControlRepository = currentControlRepository;
            _studentRepository = studentRepository;
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

            return disciplines.Select(discipline => discipline.ToDisciplineResponse());
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
            await ValidationHelper.ValidateADGSetRequest(setRequest, _academicRepository, _disciplineRepository, _groupsRepository);

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

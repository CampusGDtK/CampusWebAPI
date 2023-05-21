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
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IRepository<StudyProgram> _studyProgramRepository;
        private readonly IRepository<Academic> _academicRepository;

        public GroupService(IRepository<Group> groupRepository, IRepository<Faculty> facultyRepository,
            IRepository<StudyProgram> studyProgramRepository, IRepository<Academic> academicRepository)
        {
            _groupRepository = groupRepository;
            _facultyRepository = facultyRepository;
            _studyProgramRepository = studyProgramRepository;
            _academicRepository = academicRepository;
        }

        public async Task<GroupResponse> Add(GroupAddRequest groupAddRequest)
        {
            if(groupAddRequest is null)
                throw new ArgumentNullException("GroupAddRequest is null");

            if (await _facultyRepository.GetValueById(groupAddRequest.FacultyId) == null)
            {
                throw new KeyNotFoundException("Id of faculty not found");
            }

            if (await _studyProgramRepository.GetValueById(groupAddRequest.StudyProgramId) == null)
            {
                throw new KeyNotFoundException("Id of study program not found");
            }

            if (await _academicRepository.GetValueById(groupAddRequest.CuratorId) == null)
            {
                throw new KeyNotFoundException("Id of curator not found");
            }

            ValidationHelper.ModelValidation(groupAddRequest);

            Group group = groupAddRequest.ToGroup();

            group.Id = Guid.NewGuid();

            await _groupRepository.Create(group);

            return group.ToGroupResponse();
        }

        public async Task Delete(Guid groupId)
        {
            var result = await _groupRepository.Delete(groupId);

            if (!result)
                throw new KeyNotFoundException("Id of group not found");
        }

        public async Task<IEnumerable<GroupResponse>> GetAll()
        {
            var result = await _groupRepository.GetAll();

            return result.Select(x => x.ToGroupResponse());
        }

        public async Task<IEnumerable<GroupResponse>> GetByFacultyId(Guid facultyId)
        {
            if (await _facultyRepository.GetValueById(facultyId) == null)
                throw new KeyNotFoundException("Id of faculty not found");

            var allGroups = await _groupRepository.GetAll();

            allGroups = allGroups.AsQueryable().Where(x => x.FacultyId == facultyId);

            return allGroups.AsEnumerable().Select(x => x.ToGroupResponse());
        }

        public async Task<GroupResponse> GetById(Guid groupId)
        {
            var result = await _groupRepository.GetValueById(groupId);

            if(result == null) 
                throw new KeyNotFoundException("Id of group not found");

            return result.ToGroupResponse();
        }

        public async Task<GroupResponse> Update(GroupUpdateRequest groupUpdateRequest)
        {
            if (groupUpdateRequest == null)
                throw new ArgumentNullException("GroupUpdateRequest is null");

            var faculty = await _facultyRepository.GetValueById(groupUpdateRequest.FacultyId);

            if (faculty == null)
                throw new KeyNotFoundException("Id of faculty not found");

            var studyProgram = await _studyProgramRepository.GetValueById(groupUpdateRequest.StudyProgramId);

            if (studyProgram == null)
            {
                throw new KeyNotFoundException("Id of study program not found");
            }

            var academic = await _academicRepository.GetValueById(groupUpdateRequest.CuratorId);

            if (academic == null)
            {
                throw new KeyNotFoundException("Id of curator not found");
            }

            ValidationHelper.ModelValidation(groupUpdateRequest);

            var group = groupUpdateRequest.ToGroup();
            group.Faculty = faculty;
            group.StudyProgram = studyProgram;
            group.Curator = academic;

            var result = await _groupRepository.Update(group);

            if(result == null)
                throw new KeyNotFoundException();

            return result.ToGroupResponse();
        }
    }
}

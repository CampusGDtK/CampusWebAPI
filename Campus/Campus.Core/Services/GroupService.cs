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

        public GroupService(IRepository<Group> groupRepository, IRepository<Faculty> facultyRepository)
        {
            _groupRepository = groupRepository;
            _facultyRepository = facultyRepository;
        }

        public async Task<GroupResponse> Add(GroupAddRequest groupAddRequest)
        {
            if(groupAddRequest is null)
                throw new ArgumentNullException(nameof(groupAddRequest));

            if (await _facultyRepository.GetValueById(groupAddRequest.FacultyId) == null)
            {
                throw new KeyNotFoundException(nameof(groupAddRequest.FacultyId));
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
                throw new KeyNotFoundException();
        }

        public async Task<IEnumerable<GroupResponse>> GetAll()
        {
            var result = await _groupRepository.GetAll();

            return result.Select(x => x.ToGroupResponse());
        }

        public async Task<IEnumerable<GroupResponse>> GetByFacultyId(Guid facultyId)
        {
            if (await _facultyRepository.GetValueById(facultyId) == null)
                throw new KeyNotFoundException(nameof(facultyId));

            var allGroups = await _groupRepository.GetAll();

            allGroups = allGroups.AsQueryable().Where(x => x.FacultyId == facultyId);

            return allGroups.AsEnumerable().Select(x => x.ToGroupResponse());
        }

        public async Task<GroupResponse> GetById(Guid groupId)
        {
            var result = await _groupRepository.GetValueById(groupId);

            if(result == null) 
                throw new KeyNotFoundException();

            return result.ToGroupResponse();
        }

        public async Task<GroupResponse> Update(GroupUpdateRequest groupUpdateRequest)
        {
            if (groupUpdateRequest == null)
                throw new ArgumentNullException(nameof(groupUpdateRequest));

            if (await _facultyRepository.GetValueById(groupUpdateRequest.FacultyId) == null)
                throw new KeyNotFoundException(nameof(groupUpdateRequest.FacultyId));

            ValidationHelper.ModelValidation(groupUpdateRequest);

            var group = groupUpdateRequest.ToGroup();

            var result = await _groupRepository.Update(group);

            if(result == null)
                throw new KeyNotFoundException();

            return result.ToGroupResponse();
        }
    }
}

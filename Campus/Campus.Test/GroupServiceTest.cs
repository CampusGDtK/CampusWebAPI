using AutoFixture;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Campus.Core.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Test
{
    public class GroupServiceTest
    {
        private readonly IGroupService _groupService;

        private readonly IRepository<Group> _groupRepository;
        private readonly Mock<IRepository<Group>> _groupRepositoryMock;

        private readonly IFixture _fixture;

        public GroupServiceTest()
        {
            _fixture = new Fixture();

            _groupRepositoryMock = new Mock<IRepository<Group>>();
            _groupRepository = _groupRepositoryMock.Object;

            _groupService = new GroupService(_groupRepository);
        }

        #region AddGroup
        [Fact]
        public async Task Add_NullGroup_ArgumentNullException()
        {
            //Arrange
            GroupAddRequest groupToUpdate = null;

            //Act
            var action = async () =>
            {
                await _groupService.Add(groupToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Add_InvalidName_ArgumentException()
        {
            //Arrange
            GroupAddRequest groupToAdd = _fixture.Build<GroupAddRequest>()
                .With(x => x.Name, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _groupService.Add(groupToAdd);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Add_ValidGroup_ValidResponse()
        {
            //Arrange
            GroupAddRequest groupToAdd = _fixture.Build<GroupAddRequest>()
                .Create();

            var group = groupToAdd.ToGroup();

            var exeptedResponse = group.ToGroupResponse();

            //Act
            var result = await _groupService.Add(groupToAdd);

            exeptedResponse.Id = result.Id;

            //Assert
            result.Should().BeEquivalentTo(exeptedResponse);
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetById_InvalidId_KeyNotFoundException()
        {
            //Arrange
            var guid = Guid.NewGuid();

            //Act
            var action = async () =>
            {
                await _groupService.GetById(guid);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetById_ProperKey_ValidResponse()
        {
            //Arrange
            var group = _fixture.Build<Group>().Create();

            var exeptedResponse = group.ToGroupResponse();

            _groupRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(group);

            //Act
            var actualResponse = await _groupService.GetById(group.FacultyId);

            //Assert
            actualResponse.Should().BeEquivalentTo(exeptedResponse);
        }
        #endregion

        #region GetByFacultyId
        [Fact]
        public async Task GetByFacultyId_FewFaculties_ValidGroups()
        {
            //Arrange
            var group1 = _fixture.Build<Group>().Create();
            var group2 = _fixture.Build<Group>().Create();
            var group3 = _fixture.Build<Group>().Create();
            var group4 = _fixture.Build<Group>().Create();

            group3.FacultyId = group1.FacultyId;

            var groups = new List<Group>
            {
                group1, group2, group3, group4
            };

            var exeptedGroupsResponse = new List<GroupResponse>()
            {
                group3.ToGroupResponse(), group1.ToGroupResponse()
            };

            _groupRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(groups);

            //Act
            var actualGroupsResponse = await _groupService.GetByFacultyId(group1.FacultyId);

            //Assert
            actualGroupsResponse.Should().BeEquivalentTo(exeptedGroupsResponse);
        }

        [Fact]
        public async Task GetByFacultyId_UnknownFacultyId_EmptyGroupList()
        {
            //Arrange
            var group1 = _fixture.Build<Group>().Create();
            var group2 = _fixture.Build<Group>().Create();
            var group3 = _fixture.Build<Group>().Create();
            var group4 = _fixture.Build<Group>().Create();

            group3.FacultyId = group1.FacultyId;

            var groups = new List<Group>
            {
                group1, group2, group3, group4
            };

            _groupRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(groups);

            //Act
            var actualGroupsResponse = await _groupService.GetByFacultyId(Guid.NewGuid());

            //Assert
            actualGroupsResponse.Count().Should().Be(0);
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GetAll_EmtyRepo_EmtyList()
        {
            //Arrange
            _groupRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Group>());

            //Act
            var resultedList = await _groupService.GetAll();

            //Assert
            resultedList.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAll_FewGroups_AllGroups()
        {
            //Arrange
            var groups = _fixture.Build<Group>().CreateMany();

            var expectedResponse = groups.Select(x => x.ToGroupResponse());

            _groupRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(groups);

            //Act
            var resultedResponse = await _groupService.GetAll();

            //Assert
            resultedResponse.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_NullGroup_ArgumentNullException()
        {
            //Arrange
            GroupUpdateRequest groupToUpdate = null;

            //Act
            var action = async () =>
            {
                await _groupService.Update(groupToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Update_InvalidName_ArgumentException()
        {
            //Arrange
            var groupToUpdate = _fixture.Build<GroupUpdateRequest>()
                .With(x => x.Name, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _groupService.Update(groupToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Update_InvalidId_KeyNotFoundException()
        {
            //Arrange
            var groupToUpdate = _fixture.Build<GroupUpdateRequest>()
                 .Create();

            _groupRepositoryMock.Setup(x => x.Update(It.IsAny<Group>())).ReturnsAsync(null as Group);

            //Act
            var action = async () =>
            {
                await _groupService.Update(groupToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Update_ProperDetails_ValidResponse()
        {
            //Arrange
            var group = _fixture.Create<Group>();

            var expectedResponse = group.ToGroupResponse();

            var groupToUpdate = expectedResponse.ToGroupUpdateRequest();

            _groupRepositoryMock.Setup(x => x.Update(It.IsAny<Group>())).ReturnsAsync(group);

            //Act
            var actualResult = await _groupService.Update(groupToUpdate);

            //Assert
            actualResult.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Remove
        [Fact]
        public async Task Remove_InvalidId_KeyNotFoundException()
        {
            //Arrange
            var groupId = Guid.NewGuid();

            _groupRepositoryMock.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            var action = async () =>
            {
                await _groupService.Delete(groupId);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        #endregion
    }
}

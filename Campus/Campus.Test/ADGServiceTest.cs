using AutoFixture;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Campus.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Test
{
    public class ADGServiceTest
    {
        private readonly IADGService _adgService;

        private readonly IRepository<AcademicDisciplineGroup> _adgRepository;
        private readonly IRepository<Academic> _academicRepository;
        private readonly IRepository<Discipline> _disciplineRepository;
        private readonly IRepository<Group> _groupsRepository;

        private readonly Mock<IRepository<AcademicDisciplineGroup>> _adgRepositoryMock;
        private readonly Mock<IRepository<Academic>> _academicRepositoryMock;
        private readonly Mock<IRepository<Discipline>> _disciplineRepositoryMock;
        private readonly Mock<IRepository<Group>> _groupsRepositoryMock;

        private readonly Fixture _autoFixture;

        public ADGServiceTest()
        {
            _adgRepositoryMock = new Mock<IRepository<AcademicDisciplineGroup>>();
            _academicRepositoryMock = new Mock<IRepository<Academic>>();
            _disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
            _groupsRepositoryMock = new Mock<IRepository<Group>>();

            _adgRepository = _adgRepositoryMock.Object;
            _academicRepository = _academicRepositoryMock.Object;
            _disciplineRepository = _disciplineRepositoryMock.Object;
            _groupsRepository = _groupsRepositoryMock.Object;

            _adgService = new ADGService(_adgRepository, _academicRepository, _disciplineRepository, _groupsRepository);

            _autoFixture = new Fixture();
        }

        #region GetDisciplinesByAcademicId
        //If id of not existing academic is passed to method, it should throw KeyNotFoundException
        [Fact]
        public async Task GetDisciplinesByAcademicId_InvalidGuid()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            _academicRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Academic?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _adgService.GetDisciplinesByAcademicId(academicId);
            });
        }
        #endregion

        #region GetGroupsByDisciplineAndAcademicId
        //If Id of not existing academic is passed to the method, it throws KeyNotFoundException
        [Fact]
        public async Task GetGroupsByDisciplineAndAcademicId_InvalidAcademicId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Academic?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _adgService.GetGroupsByDisciplineAndAcademicId(academicId, disciplineId);
            });
        }

        //If Id of not existing discipline is passed to the method, it throws KeyNotFoundException
        [Fact]
        public async Task GetGroupsByDisciplineAndAcademicId_InvalidDisciplineId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _disciplineRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Discipline?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _adgService.GetGroupsByDisciplineAndAcademicId(academicId, disciplineId);
            });
        }

        //If Id of academic and discipline that do not have any realaition are passed, it throw ArgumentException
        [Fact]
        public async Task GetGroupsByDisciplineAndAcademicId_AbsenceOfRelation()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            Academic academic = _autoFixture.Build<Academic>()
                .With(academic => academic.Id, academicId)
                .Create();
            Discipline discipline = _autoFixture.Build<Discipline>()
                .With(discipline => discipline.Id, disciplineId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                    .ReturnsAsync(academic);
            _disciplineRepositoryMock.Setup(p => p.GetValueById(disciplineId))
                    .ReturnsAsync(discipline);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _adgService.GetGroupsByDisciplineAndAcademicId(academicId, disciplineId);
            });
        }
        #endregion

        #region SetRelation
        //If null object is passed to the method, it should return ArgumentNullException
        [Fact]
        public async Task SetRelation_NullObject()
        {
            //Arrange
            ADGSetRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _adgService.SetRelation(request);
            });
        }

        //If ADGSetRequest with Id of not existing academic is passed to the method, it
        //should throw KeyNotFoundException
        [Fact]
        public async Task SetRelation_InvalidAcademicId()
        {
            Guid disciplineId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            //Arrange
            ADGSetRequest request = new ADGSetRequest()
            {
                AcademicId = Guid.NewGuid(),
                DisciplineGroupsRelation = new Dictionary<Guid, IEnumerable<Guid>>()
                {
                    { disciplineId, new List<Guid>() { groupId } }
                }
            };

            Discipline discipline = _autoFixture.Build<Discipline>()
                .With(discipline => discipline.Id, disciplineId)
                .Create();
            Group group = _autoFixture.Build<Group>()
                .With(group => group.Id, groupId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                    .ReturnsAsync((Academic?)null);
            _disciplineRepositoryMock.Setup(p => p.GetValueById(disciplineId))
                    .ReturnsAsync(discipline);
            _groupsRepositoryMock.Setup(p => p.GetValueById(groupId))
                    .ReturnsAsync(group);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _adgService.SetRelation(request);
            });
        }

        //If ADGSetRequest with Id of not existing discipline is passed to the method, it
        //should throw KeyNotFoundException
        [Fact]
        public async Task SetRelation_InvalidDisciplineId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();

            ADGSetRequest request = new ADGSetRequest()
            {
                AcademicId = academicId,
                DisciplineGroupsRelation = new Dictionary<Guid, IEnumerable<Guid>>()
                {
                    { Guid.NewGuid(), new List<Guid>() { groupId } }
                }
            };

            Academic academic = _autoFixture.Build<Academic>()
                .With(academic => academic.Id, academicId)
                .Create();
            Group group = _autoFixture.Build<Group>()
                .With(group => group.Id, groupId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                    .ReturnsAsync(academic);
            _disciplineRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                    .ReturnsAsync((Discipline?)null);
            _groupsRepositoryMock.Setup(p => p.GetValueById(groupId))
                    .ReturnsAsync(group);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _adgService.SetRelation(request);
            });
        }

        //If ADGSetRequest with Id of not existing group is passed to the method, it
        //should throw KeyNotFoundException
        [Fact]
        public async Task SetRelation_InvalidGroupId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            ADGSetRequest request = new ADGSetRequest()
            {
                AcademicId = academicId,
                DisciplineGroupsRelation = new Dictionary<Guid, IEnumerable<Guid>>()
                {
                    { disciplineId, new List<Guid>() { Guid.NewGuid() } }
                }
            };

            Academic academic = _autoFixture.Build<Academic>()
                .With(academic => academic.Id, academicId)
                .Create();
            Discipline discipline = _autoFixture.Build<Discipline>()
                .With(discipline => discipline.Id, disciplineId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                    .ReturnsAsync(academic);
            _disciplineRepositoryMock.Setup(p => p.GetValueById(disciplineId))
                    .ReturnsAsync(discipline);
            _groupsRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                    .ReturnsAsync((Group?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _adgService.SetRelation(request);
            });
        }
        #endregion

        #region ResetRelation
        //When id of not existing academic is passed to the method, it should throw KeyNotFoundException
        [Fact]
        public async Task ResetRelation_IdNotExist()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            _academicRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                    .ReturnsAsync((Academic?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _adgService.ResetRelation(academicId);
            });
        }

        //When id of academic, which is has not any realtion, is passed to the method, it should throw ArgumentException
        [Fact]
        public async Task ResetRelation_IdHasNotRelation()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            Academic academic = _autoFixture.Build<Academic>()
                .With(academic => academic.Id, academicId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                    .ReturnsAsync(academic);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _adgService.ResetRelation(academicId);
            });
        }
        #endregion
    }
}

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
using System.Text;
using System.Threading.Tasks;

namespace Campus.Test
{
    public class SpecialityServiceTest
    {
        private readonly ISpecialityService _specialityService;
        private readonly IRepository<Speciality> _specialityRepository;
        private readonly Mock<IRepository<Speciality>> _specialityRepositoryMock;
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly Mock<IRepository<Faculty>> _facultyRepositoryMock;
        private readonly IRepository<SpecialityFaculty> _specialityFacultyRepository;
        private readonly Mock<IRepository<SpecialityFaculty>> _specialityFacultyRepositoryMock;
        private readonly IFixture _autoFixture;

        public SpecialityServiceTest()
        {
            _specialityRepositoryMock = new Mock<IRepository<Speciality>>();
            _specialityRepository = _specialityRepositoryMock.Object;

            _facultyRepositoryMock = new Mock<IRepository<Faculty>>();
            _facultyRepository = _facultyRepositoryMock.Object;

            _specialityFacultyRepositoryMock = new Mock<IRepository<SpecialityFaculty>>();
            _specialityFacultyRepository = _specialityFacultyRepositoryMock.Object;

            _specialityService = new SpecialityService(_specialityRepository, _facultyRepository, _specialityFacultyRepository);

            _autoFixture = new Fixture();
        }

        #region GetSpecialityById
        //When guid of not existing speciality is passed to the method it should throw KeyNotFoundException
        [Fact]
        public async Task GetSpecialityById_InvalidId()
        {
            //Arrange
            Guid specialityId = Guid.NewGuid();

            _specialityRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Speciality?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _specialityService.GetSpecialityById(specialityId);
            });
        }

        //When valid guid is passed to the method, it should return valid SpecialityResponse
        [Fact]
        public async Task GetSpecialityById_ValidId()
        {
            //Arrange
            Guid specialityId = Guid.NewGuid();

            Speciality speciality = _autoFixture.Build<Speciality>()
                .With(p => p.Id, specialityId)
                .Create();

            _specialityRepositoryMock.Setup(p => p.GetValueById(specialityId))
                .ReturnsAsync(speciality);

            //Act
            SpecialityResponse? specialityResponse = await _specialityService.GetSpecialityById(specialityId);


            //Assert
            Assert.NotNull(specialityResponse);
            Assert.Equal(specialityId, specialityResponse.Id);
            Assert.Equal(speciality.Name, specialityResponse.Name);
        }
        #endregion

        #region GetAll
        //Method should return collection of existing specialities
        [Fact]
        public async Task GetAll_ReturnsCollection()
        {
            //Arrange
            List<Speciality> specialitiesExpected = _autoFixture.CreateMany<Speciality>(5).ToList();

            _specialityRepositoryMock.Setup(p => p.GetAll())
                .ReturnsAsync(specialitiesExpected);

            //Act
            IEnumerable<SpecialityResponse>? specialitiesActual = await _specialityService.GetAll();

            //Assert
            Assert.NotNull(specialitiesActual);
            foreach (Speciality specialityExpected in specialitiesExpected)
            {
                Assert.Contains(specialitiesActual, specialityActual => specialityActual.Id == specialityExpected.Id);
            }
        }
        #endregion

        #region GetByFacultyId
        //When guid of not existing faculty is passed to the method, it should return KeyNotFoundException
        [Fact]
        public async Task GetByFacultyId_InvalidId()
        {
            //Arrange
            Guid facultyId = Guid.NewGuid();

            _facultyRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Faculty?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _specialityService.GetByFacultyId(facultyId);
            });
        }

        //When valid guid of faculty is passed to the method, it should return specialties filtered by faculty
        [Fact]
        public async Task GetByFacultyId_ValidId()
        {
            //Arrange
            Guid facultyId = Guid.NewGuid();

            Guid specialityId1 = Guid.NewGuid();
            Guid specialityId2 = Guid.NewGuid();

            Faculty faculty = _autoFixture.Build<Faculty>()
                .With(p => p.Id, facultyId)
                .Create();

            Speciality speciality1 = _autoFixture.Build<Speciality>()
                .With(p => p.Id, specialityId1)
                .Create();
            Speciality speciality2 = _autoFixture.Build<Speciality>()
                .With(p => p.Id, specialityId2)
                .Create();

            List<SpecialityFaculty> specialityFaculties = new List<SpecialityFaculty>()
            {
                new SpecialityFaculty() { Id = Guid.NewGuid(), FacultyId = facultyId, SpecialityId = specialityId1, Speciality = speciality1},
                new SpecialityFaculty() { Id = Guid.NewGuid(), FacultyId = facultyId, SpecialityId = specialityId2, Speciality = speciality2},
            };

            _facultyRepositoryMock.Setup(p => p.GetValueById(facultyId))
                .ReturnsAsync(faculty);

            _specialityFacultyRepositoryMock.Setup(p => p.GetAll())
                .ReturnsAsync(specialityFaculties);

            //Act
            IEnumerable<SpecialityResponse>? specialitiesAcutal = await _specialityService.GetByFacultyId(facultyId);


            //Assert
            Assert.NotNull(specialitiesAcutal);
            Assert.Contains(specialitiesAcutal, speciality => speciality.Id == specialityId1);
            Assert.Contains(specialitiesAcutal, speciality => speciality.Id == specialityId2);
        }
        #endregion

        #region Add
        //When null object is passed to the method, it should throw ArgumentNullException
        [Fact]
        public async Task Add_NullObject()
        {
            //Arrange
            SpecialityAddRequest? specialityAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _specialityService.Add(specialityAddRequest);
            });
        }

        //When invalid name of speciality is passed to the method, it should throw ArgumentException
        [Fact]
        public async Task Add_InvalidName()
        {
            //Arrange
            SpecialityAddRequest? specialityAddRequest = new SpecialityAddRequest()
            {
                Name = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _specialityService.Add(specialityAddRequest);
            });
        }

        //When valid object is passed to the method, it should return added SpecialityResponse object
        [Fact]
        public async Task Add_ValidObject()
        {
            //Arrange
            SpecialityAddRequest specialityAddRequest = _autoFixture.Create<SpecialityAddRequest>();

            _specialityRepositoryMock.Setup(p => p.Create(It.IsAny<Speciality>()))
                .Returns(Task.CompletedTask);

            //Act
            SpecialityResponse? specialityResponse = await _specialityService.Add(specialityAddRequest);

            //Assert
            Assert.NotNull(specialityResponse);
            Assert.Equal(specialityAddRequest.Name, specialityResponse.Name);
        }
        #endregion

        #region Update
        //When null object is passed to the method, it should throw ArgumentNullException
        [Fact]
        public async Task Update_NullObject()
        {
            //Arrange
            SpecialityUpdateRequest? specialityUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _specialityService.Update(specialityUpdateRequest);
            });
        }

        //When object with not existing guid is passed to the method, it should return KeyNotFoundException
        [Fact]
        public async Task Update_InvalidId()
        {
            //Arrange
            Guid specialityId = Guid.NewGuid();

            //Speciality speciality = _autoFixture.Build<Speciality>()
            //    .With(p => p.Id, specialityId)
            //    .Create();

            //_specialityRepositoryMock.Setup(p => p.GetValueById(specialityId))
            //    .ReturnsAsync(speciality);

            SpecialityUpdateRequest specialityUpdateRequest = _autoFixture.Create<SpecialityUpdateRequest>();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _specialityService.Update(specialityUpdateRequest);
            });
        }

        //When invalid name of speciality is passed to the method, it should throw ArgumentException
        [Fact]
        public async Task Update_InvalidName()
        {
            //Arrange
            Guid specialityId = Guid.NewGuid();

            Speciality speciality = _autoFixture.Build<Speciality>()
                .With(p => p.Id, specialityId)
                .Create();

            SpecialityUpdateRequest specialityUpdateRequest = new SpecialityUpdateRequest()
            {
                Id = specialityId,
                Name = null
            };

            _specialityRepositoryMock.Setup(p => p.GetValueById(specialityId))
                .ReturnsAsync(speciality);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _specialityService.Update(specialityUpdateRequest);
            });
        }

        //When valid object is passed to the method, it should return updated SpecialityResponse object
        [Fact]
        public async Task Update_ValidObject()
        {
            //Arrange
            Guid specialityId = Guid.NewGuid();

            Speciality speciality = _autoFixture.Build<Speciality>()
                .With(p => p.Id, specialityId)
                .Create();

            SpecialityUpdateRequest specialityUpdateRequest = _autoFixture.Build<SpecialityUpdateRequest>()
                .With(p => p.Id, specialityId)
                .Create();

            Speciality specialityToUpdate = specialityUpdateRequest.ToSpeciality();

            _specialityRepositoryMock.Setup(p => p.GetValueById(specialityId))
                .ReturnsAsync(speciality);
            _specialityRepositoryMock.Setup(p => p.Update(It.IsAny<Speciality>()))
                .ReturnsAsync(specialityToUpdate);

            //Act
            SpecialityResponse? specialityResponse = await _specialityService.Update(specialityUpdateRequest);

            //Assert
            Assert.NotNull(specialityResponse);
            Assert.Equal(specialityToUpdate.Id, specialityResponse.Id);
            Assert.Equal(specialityToUpdate.Name, specialityResponse.Name);
        }
        #endregion

        #region Remove
        //When id of not existing speciality is passed to the method, it should throw KeyNotFoundException
        [Fact]
        public async Task Remove_InvalidId()
        {
            //Arrange
            Guid specialityId = Guid.NewGuid();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _specialityService.Remove(specialityId);
            });
        }
        #endregion
    }
}

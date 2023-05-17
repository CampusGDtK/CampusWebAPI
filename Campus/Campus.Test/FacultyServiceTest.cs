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
using FluentAssertions;

namespace Campus.Test
{
    public class FacultyServiceTest
    {
        private readonly IFacultyService _facultyService;
        private readonly ISpecialityService _specialityService;

        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IRepository<SpecialityFaculty> _specialityFacultyRepository;

        private readonly Mock<IRepository<Faculty>> _facultyRepositoryMock;
        private readonly Mock<IRepository<SpecialityFaculty>> _specialityFacultyRepositoryMock;
        private readonly Mock<ISpecialityService> _specialityServiceMock;

        private readonly IFixture _fixture;

        public FacultyServiceTest()
        {
            _fixture = new Fixture();

            _facultyRepositoryMock = new Mock<IRepository<Faculty>>();
            _specialityServiceMock = new Mock<ISpecialityService>();
            _specialityFacultyRepositoryMock = new Mock<IRepository<SpecialityFaculty>>();

            _facultyRepository = _facultyRepositoryMock.Object;
            _specialityFacultyRepository = _specialityFacultyRepositoryMock.Object;
            _specialityService = _specialityServiceMock.Object;

            _facultyService = new FacultyService(_facultyRepository, _specialityFacultyRepository, _specialityService);
        }

        #region AddFaculty
        [Fact]
        public async Task AddFaculty_NullFaculty()
        {
            //Arrange
            FacultyAddRequest? facultyAddRequest = null;

            //Act
            var action = async () =>
            {
                await _facultyService.Add(facultyAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddFaculty_NameNull()
        {
            //Arrange

            FacultyAddRequest facultyAddRequest = _fixture.Build<FacultyAddRequest>()
                .With(x => x.Name, null as string)
                .With(x => x.Specialities, new List<Guid> { Guid.NewGuid() })
                .Create();

            //Act
            var action = async () =>
            {
                await _facultyService.Add(facultyAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddFaculty_DeanIsNull()
        {
            //Arrange
            var facultyAddRequest = _fixture.Build<FacultyAddRequest>()
                .With(x => x.Dean, null as string)
                .With(x => x.Specialities, new List<Guid> { Guid.NewGuid() })
                .Create();

            //Act
            var action = async () =>
            {
                await _facultyService.Add(facultyAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddFaculty_ValidFaculty()
        {
            var specialitiesList = _fixture.CreateMany<Speciality>();

            var specialitiesResponses = specialitiesList.Select(x => x.ToSpecialityResponse());

            var specialitiesIds = specialitiesList.Select(x => x.Id);

            //Arrange
            var facultyAddRequest = _fixture.Build<FacultyAddRequest>()
                .With(x => x.Specialities, specialitiesIds)
                .Create(); ;

            var faculty = facultyAddRequest.ToFaculty();

            var facultyResponseExpected = faculty.ToFacultyResponse();

            facultyResponseExpected.SpecialitiesId = specialitiesIds;
            facultyResponseExpected.SpecialitiesName = specialitiesList.Select(x => x.Name);

            _specialityServiceMock.Setup(x => x.GetByFacultyId(It.IsAny<Guid>())).ReturnsAsync(specialitiesResponses);

            //Act
            var resultedFaculty = await _facultyService.Add(facultyAddRequest);

            facultyResponseExpected.Id = resultedFaculty.Id;

            //Assert
            resultedFaculty.Id.Should().NotBeEmpty();
            resultedFaculty.Should().BeEquivalentTo(facultyResponseExpected);
        }
        #endregion

        #region GetFacultyById
        [Fact]
        public async Task GetFacultyByID_InvalidId()
        {
            //Arrange
            var guid = Guid.NewGuid();

            //Act
            var action = async () =>
            {
                await _facultyService.GetById(guid);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetFacultyByID_ValidId()
        {
            //Assert
            var faculty = _fixture.Create<Faculty>();

            var exeptedFacultyResponse = faculty.ToFacultyResponse();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(faculty);

            //Act
            var actualFacultyResponse = await _facultyService.GetById(faculty.Id);

            //Assert
            actualFacultyResponse.Should().BeEquivalentTo(exeptedFacultyResponse);
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GetAll_FewFaculties()
        {
            //Assert
            var faculties = new List<Faculty>
            {
                _fixture.Create<Faculty>(),
                _fixture.Create<Faculty>(),
                _fixture.Create<Faculty>(),
                _fixture.Create<Faculty>(),
            };

            var exeptedfaculties = faculties.Select(x => x.ToFacultyResponse());

            _facultyRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(faculties);

            //Act
            var resultedList = await _facultyService.GetAll();

            //Assert
            resultedList.Should().BeEquivalentTo(exeptedfaculties);
        }

        [Fact]
        public async Task GetAll_EmtyList()
        {
            //Assert
            _facultyRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Faculty>());

            //Act
            List<FacultyResponse> resultedFaculties = (await _facultyService.GetAll()).ToList();

            //Assert
            resultedFaculties.Should().BeEmpty();
        }
        #endregion

        #region UpdateFaculty
        [Fact]
        public async Task Update_InvalidId()
        {
            //Arrange
            var faculty = _fixture.Create<FacultyUpdateRequest?>();

            _facultyRepositoryMock.Setup(x => x.Update(It.IsAny<Faculty>())).ReturnsAsync(null as Faculty);

            //Act
            var action = async () => 
            {
                await _facultyService.Update(faculty);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Update_InvalidName()
        {
            //Arrange
            var faculty = _fixture.Build<FacultyUpdateRequest?>()
                .With(x => x.Name, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _facultyService.Update(faculty);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Update_InvalidDean()
        {
            //Arrange
            var faculty = _fixture.Build<FacultyUpdateRequest?>()
                .With(x => x.Dean, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _facultyService.Update(faculty);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Update_NullFaculty()
        {
            //Arrange
            FacultyUpdateRequest? faculty = null;

            //Act
            var action = async () =>
            {
                await _facultyService.Update(faculty);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Update_AllPropertiesUpdated()
        {
            //Arrange
            var facultyBefore = _fixture.Create<Faculty>();

            var faculty = _fixture.Create<Faculty>();

            var facultyResponse = faculty.ToFacultyResponse();

            var facultyUpdateRequest = facultyResponse.ToFacultyUpdateRequest();

            _facultyRepositoryMock.Setup(x => x.Update(It.IsAny<Faculty>())).ReturnsAsync(faculty);
            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(facultyBefore);

            //Act
            var resultedResponse = await _facultyService.Update(facultyUpdateRequest);

            //Assert
            resultedResponse.Should().BeEquivalentTo(facultyResponse);
        }
        #endregion

        //Ask about Specialities list with null Id!!!

        #region Remove
        [Fact]
        public async Task Remove_InvalidId()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var action = async () =>
            {
                await _facultyService.Remove(id);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        #endregion
    }
}

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
    public class CathedraServiceTest
    {
        private readonly IFixture _fixture;

        private readonly ICathedraService _cathedraService;

        private readonly IRepository<Cathedra> _cathedraRepository;
        private readonly IRepository<Faculty> _facultyRepository;

        private readonly Mock<IRepository<Cathedra>> _cathedraRepositoryMock;
        private readonly Mock<IRepository<Faculty>> _facultyRepositoryMock;

        public CathedraServiceTest()
        {
            _fixture = new Fixture();

            _cathedraRepositoryMock = new Mock<IRepository<Cathedra>>();
            _facultyRepositoryMock = new Mock<IRepository<Faculty>>();

            _cathedraRepository = _cathedraRepositoryMock.Object;
            _facultyRepository = _facultyRepositoryMock.Object;

            _cathedraService = new CathedraService(_cathedraRepository, _facultyRepository);
        }

        #region AddCathedra
        [Fact]
        public async Task Add_InvalidFacultyId_KeyNotFoundException()
        {
            //Arrange
            var cathedraToAdd = _fixture.Create<CathedraAddRequest>();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(null as Faculty);

            //Act
            var action = async () =>
            {
                await _cathedraService.Add(cathedraToAdd);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Add_NullCathedra_ArgumentNullException()
        {
            //Arrange
            CathedraAddRequest cathedraToUpdate = null;

            //Act
            var action = async () =>
            {
                await _cathedraService.Add(cathedraToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Add_InvalidName_ArgumentException()
        {
            //Arrange
            CathedraAddRequest groupToAdd = _fixture.Build<CathedraAddRequest>()
                .With(x => x.Name, null as string)
                .Create();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(_fixture.Build<Faculty>().With(x => x.Id, groupToAdd.FacultyId).Create());

            //Act
            var action = async () =>
            {
                await _cathedraService.Add(groupToAdd);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Add_InvalidHead_ArgumentException()
        {
            //Arrange
            CathedraAddRequest cathedraToAdd = _fixture.Build<CathedraAddRequest>()
                .With(x => x.Head, null as string)
                .Create();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(_fixture.Build<Faculty>().With(x => x.Id, cathedraToAdd.FacultyId).Create());

            //Act
            var action = async () =>
            {
                await _cathedraService.Add(cathedraToAdd);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Add_ValidCathdedra_ValidResponse()
        {
            //Arrange
            CathedraAddRequest cathedraToAdd = _fixture.Build<CathedraAddRequest>()
                .Create();

            var cathedra = cathedraToAdd.ToCathedra();

            var exeptedResponse = cathedra.ToCathedraResponse();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(_fixture.Build<Faculty>().With(x => x.Id, cathedraToAdd.FacultyId).Create());

            //Act
            var result = await _cathedraService.Add(cathedraToAdd);

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
                await _cathedraService.GetById(guid);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetById_ProperKey_ValidResponse()
        {
            //Arrange
            var cathedra = _fixture.Build<Cathedra>().Create();

            var exeptedResponse = cathedra.ToCathedraResponse();

            _cathedraRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(cathedra);

            //Act
            var actualResponse = await _cathedraService.GetById(cathedra.FacultyId);

            //Assert
            actualResponse.Should().BeEquivalentTo(exeptedResponse);
        }
        #endregion

        #region GetByFacultyId
        [Fact]
        public async Task GetByFacultyId_FewFaculties_ValidCathedras()
        {
            //Arrange
            var cathedra1 = _fixture.Build<Cathedra>().Create();
            var cathedra2 = _fixture.Build<Cathedra>().Create();
            var cathedra3 = _fixture.Build<Cathedra>().Create();
            var cathedra4 = _fixture.Build<Cathedra>().Create();

            cathedra3.FacultyId = cathedra1.FacultyId;

            var faculty = _fixture.Build<Faculty>()
                .With(x => x.Id, cathedra1.FacultyId)
                .Create();

            var groups = new List<Cathedra>
            {
                cathedra1, cathedra2, cathedra3, cathedra4
            };

            var exeptedGroupsResponse = new List<CathedraResponse>()
            {
                cathedra3.ToCathedraResponse(), cathedra1.ToCathedraResponse()
            };

            _cathedraRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(groups);
            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(faculty);

            //Act
            var actualGroupsResponse = await _cathedraService.GetByFacultyId(cathedra1.FacultyId);

            //Assert
            actualGroupsResponse.Should().BeEquivalentTo(exeptedGroupsResponse);
        }

        [Fact]
        public async Task GetByFacultyId_UnknownFacultyId_KeyNotFoundException()
        {
            //Arrange
            var cathedra1 = _fixture.Build<Cathedra>().Create();
            var cathedra2 = _fixture.Build<Cathedra>().Create();
            var cathedra3 = _fixture.Build<Cathedra>().Create();
            var cathedra4 = _fixture.Build<Cathedra>().Create();

            cathedra3.FacultyId = cathedra1.FacultyId;

            var groups = new List<Cathedra>
            {
                cathedra1, cathedra2, cathedra3, cathedra4
            };

            _cathedraRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(groups);

            //Act
            var action = async () =>
            {
                await _cathedraService.GetByFacultyId(Guid.NewGuid());
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GetAll_EmtyRepo_EmtyList()
        {
            //Arrange
            _cathedraRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Cathedra>());

            //Act
            var resultedList = await _cathedraService.GetAll();

            //Assert
            resultedList.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAll_FewCathedras_AllCathedras()
        {
            //Arrange
            var cathedras = _fixture.Build<Cathedra>().CreateMany();

            var expectedResponse = cathedras.Select(x => x.ToCathedraResponse());

            _cathedraRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(cathedras);

            //Act
            var resultedResponse = await _cathedraService.GetAll();

            //Assert
            resultedResponse.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_InvalidFacultyId_KeyNotFoundException()
        {
            //Arrange
            var cathedraToAdd = _fixture.Create<CathedraUpdateRequest>();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(null as Faculty);

            //Act
            var action = async () =>
            {
                await _cathedraService.Update(cathedraToAdd);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Update_NullCathedra_ArgumentNullException()
        {
            //Arrange
            CathedraUpdateRequest groupToUpdate = null;

            //Act
            var action = async () =>
            {
                await _cathedraService.Update(groupToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Update_InvalidName_ArgumentException()
        {
            //Arrange
            var cathedraToUpdate = _fixture.Build<CathedraUpdateRequest>()
                .With(x => x.Name, null as string)
                .Create();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(_fixture.Build<Faculty>().With(x => x.Id, cathedraToUpdate.FacultyId).Create());

            //Act
            var action = async () =>
            {
                await _cathedraService.Update(cathedraToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Update_InvalidHead_ArgumentException()
        {
            //Arrange
            var cathedraToUpdate = _fixture.Build<CathedraUpdateRequest>()
                .With(x => x.Head, null as string)
                .Create();

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(_fixture.Build<Faculty>().With(x => x.Id, cathedraToUpdate.FacultyId).Create());

            //Act
            var action = async () =>
            {
                await _cathedraService.Update(cathedraToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Update_InvalidId_KeyNotFoundException()
        {
            //Arrange
            var cathedraToUpdate = _fixture.Build<CathedraUpdateRequest>()
                 .Create();

            _cathedraRepositoryMock.Setup(x => x.Update(It.IsAny<Cathedra>())).ReturnsAsync(null as Cathedra);

            //Act
            var action = async () =>
            {
                await _cathedraService.Update(cathedraToUpdate);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Update_ProperDetails_ValidResponse()
        {
            //Arrange
            var cathedra = _fixture.Create<Cathedra>();

            var expectedResponse = cathedra.ToCathedraResponse();

            var cathedraToUpdate = expectedResponse.ToCathedraUpdateRequest();

            _cathedraRepositoryMock.Setup(x => x.Update(It.IsAny<Cathedra>())).ReturnsAsync(cathedra);

            _facultyRepositoryMock.Setup(x => x.GetValueById(It.IsAny<Guid>())).ReturnsAsync(_fixture.Build<Faculty>().With(x => x.Id, cathedra.FacultyId).Create());

            //Act
            var actualResult = await _cathedraService.Update(cathedraToUpdate);

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

            _cathedraRepositoryMock.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            var action = async () =>
            {
                await _cathedraService.Delete(groupId);
            };

            //Assert
            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        #endregion
    }
}

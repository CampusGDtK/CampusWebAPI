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
    public class AcademicServiceTest
    {
        private readonly IAcademicService _academicService;
        private readonly IRepository<Academic> _academicRepository;
        private readonly Mock<IRepository<Academic>> _academicRepositoryMock;
        private readonly IRepository<Cathedra> _cathedraRepository;
        private readonly Mock<IRepository<Cathedra>> _cathedraRepositoryMock;
        private readonly Fixture _autoFixture;

        public AcademicServiceTest()
        {
            _academicRepositoryMock = new Mock<IRepository<Academic>>();
            _academicRepository = _academicRepositoryMock.Object;
            _cathedraRepositoryMock = new Mock<IRepository<Cathedra>>();
            _cathedraRepository = _cathedraRepositoryMock.Object;

            _academicService = new AcademicService(_academicRepository, _cathedraRepository);

            _autoFixture = new Fixture();
        }

        #region GetAcademicById
        //When guid of not existing academic is passed to the method it should throw KeyNotFoundException
        [Fact]
        public async Task GetacademicById_InvalidId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            _academicRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Academic?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _academicService.GetAcademicById(academicId);
            });
        }

        //When valid guid is passed to the method, it should return valid AcademicResponse
        [Fact]
        public async Task GetacademicById_ValidId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            Academic academic = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                .ReturnsAsync(academic);

            //Act
            AcademicResponse? academicResponse = await _academicService.GetAcademicById(academicId);

            //Assert
            Assert.NotNull(academicResponse);
            Assert.Equal(academicId, academicResponse.Id);
            Assert.Equal(academic.Name, academicResponse.Name);
        }
        #endregion

        #region GetAll
        //Method should return collection of existing academics
        [Fact]
        public async Task GetAll_ReturnsCollection()
        {
            //Arrange
            List<Academic> academicsExpected = _autoFixture.CreateMany<Academic>(5).ToList();

            _academicRepositoryMock.Setup(p => p.GetAll())
                .ReturnsAsync(academicsExpected);

            //Act
            IEnumerable<AcademicResponse>? academicsActual = await _academicService.GetAll();

            //Assert
            Assert.NotNull(academicsActual);
            foreach (Academic academicExpected in academicsExpected)
            {
                Assert.Contains(academicsActual, academicActual => academicActual.Id == academicExpected.Id);
            }
        }
        #endregion

        #region GetByCathedraId
        //When guid of not existing cathedra is passed to the method, it should return KeyNotFoundException
        [Fact]
        public async Task GetByCathedraId_InvalidId()
        {
            //Arrange
            Guid cathedraId = Guid.NewGuid();

            _cathedraRepositoryMock.Setup(p => p.GetValueById(It.IsAny<Guid>()))
                .ReturnsAsync((Cathedra?)null);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _academicService.GetByCathedraId(cathedraId);
            });
        }

        //When valid guid of cathedra is passed to the method, it should return academics filtered by cathedra
        [Fact]
        public async Task GetByCathedraId_ValidId()
        {
            //Arrange
            Guid cathedraId = Guid.NewGuid();

            Guid academicId1 = Guid.NewGuid();
            Guid academicId2 = Guid.NewGuid();

            Cathedra cathedra = _autoFixture.Build<Cathedra>()
                .With(p => p.Id, cathedraId)
                .Create();

            Academic academic1 = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId1)
                .With(p => p.CathedraId, cathedraId)
                .Create();
            Academic academic2 = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId2)
                .With(p => p.CathedraId, cathedraId)
                .Create();

            List<Academic> academics = new List<Academic>()
            {
                academic1, academic2 
            };

            _cathedraRepositoryMock.Setup(p => p.GetValueById(cathedraId))
                .ReturnsAsync(cathedra);
            _academicRepositoryMock.Setup(p => p.GetAll())
                .ReturnsAsync(academics);

            //Act
            IEnumerable<AcademicResponse>? academicsAcutal = await _academicService.GetByCathedraId(cathedraId);


            //Assert
            Assert.NotNull(academicsAcutal);
            Assert.Contains(academicsAcutal, academic => academic.Id == academicId1);
            Assert.Contains(academicsAcutal, academic => academic.Id == academicId2);
        }
        #endregion

        #region Add
        //When null object is passed to the method, it should throw ArgumentNullException
        [Fact]
        public async Task Add_NullObject()
        {
            //Arrange
            AcademicAddRequest? academicAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _academicService.Add(academicAddRequest);
            });
        }

        //When invalid name of academic is passed to the method, it should throw ArgumentException
        [Fact]
        public async Task Add_InvalidName()
        {
            //Arrange
            AcademicAddRequest? academicAddRequest = new AcademicAddRequest()
            {
                Name = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _academicService.Add(academicAddRequest);
            });
        }

        //When Id of not existing cathedra is passed to the method, it should throw KeyNotFoundException
        [Fact]
        public async Task Add_InvalidCathedraId()
        {
            //Arrange
            AcademicAddRequest academicAddRequest = _autoFixture.Create<AcademicAddRequest>();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _academicService.Add(academicAddRequest);
            });
        }

        //When valid object is passed to the method, it should return added AcademicResponse object
        [Fact]
        public async Task Add_ValidObject()
        {
            //Arrange
            Guid cathedraId = Guid.NewGuid();

            Guid academicId = Guid.NewGuid();

            Cathedra cathedra = _autoFixture.Build<Cathedra>()
                .With(p => p.Id, cathedraId)
                .Create();

            Academic academic = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId)
                .Create();

            AcademicAddRequest academicAddRequest = _autoFixture.Build<AcademicAddRequest>()
                .With(p => p.CathedraId, cathedraId)
                .Create();

            _cathedraRepositoryMock.Setup(p => p.GetValueById(cathedraId))
                .ReturnsAsync(cathedra);
            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                .ReturnsAsync(academic);
            _academicRepositoryMock.Setup(p => p.Create(academic))
                .Returns(Task.CompletedTask);

            //Act
            AcademicResponse? academicResponse = await _academicService.Add(academicAddRequest);

            //Assert
            Assert.NotNull(academicResponse);
            Assert.Equal(academicAddRequest.Name, academicResponse.Name);
        }
        #endregion

        #region Update
        //When null object is passed to the method, it should throw ArgumentNullException
        [Fact]
        public async Task Update_NullObject()
        {
            //Arrange
            AcademicUpdateRequest? academicUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _academicService.Update(academicUpdateRequest);
            });
        }

        //When object with not existing guid is passed to the method, it should return KeyNotFoundException
        [Fact]
        public async Task Update_InvalidId()
        {
            //Arrange
            Guid cathedraId = Guid.NewGuid();

            Cathedra cathedra = _autoFixture.Build<Cathedra>()
                .With(p => p.Id, cathedraId)
                .Create();

            AcademicUpdateRequest academicUpdateRequest = _autoFixture.Build<AcademicUpdateRequest>()
                .With(p => p.CathedraId, cathedraId)
                .Create();

            _cathedraRepositoryMock.Setup(p => p.GetValueById(cathedraId))
                .ReturnsAsync(cathedra);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _academicService.Update(academicUpdateRequest);
            });
        }

        //When invalid name of academic is passed to the method, it should throw ArgumentException
        [Fact]
        public async Task Update_InvalidName()
        {
            //Arrange
            Guid cathedraId = Guid.NewGuid();

            Guid academicId = Guid.NewGuid();

            Cathedra cathedra = _autoFixture.Build<Cathedra>()
                .With(p => p.Id, cathedraId)
                .Create();

            Academic academic = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId)
                .Create();

            AcademicUpdateRequest academicUpdateRequest = new AcademicUpdateRequest()
            {
                Id = academicId,
                Name = null,
                CathedraId = cathedraId,
            };

            _cathedraRepositoryMock.Setup(p => p.GetValueById(cathedraId))
                .ReturnsAsync(cathedra);
            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                .ReturnsAsync(academic);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _academicService.Update(academicUpdateRequest);
            });
        }

        //When Id of not existing cathedra is passed to the method, it should throw KeyNotFoundException
        [Fact]
        public async Task Update_InvalidCathedraId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            Academic academic = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId)
                .Create();

            AcademicUpdateRequest academicUpdateRequest = _autoFixture.Build<AcademicUpdateRequest>()
                .With(p => p.Id, academicId)
                .Create();

            _academicRepositoryMock.Setup(p => p.GetValueById(academicId))
                .ReturnsAsync(academic);

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _academicService.Update(academicUpdateRequest);
            });
        }

        //When valid object is passed to the method, it should return updated AcademicResponse object
        [Fact]
        public async Task Update_ValidObject()
        {
            //Arrange
            Guid cathedraId = Guid.NewGuid();

            Guid academicId = Guid.NewGuid();

            Cathedra cathedra = _autoFixture.Build<Cathedra>()
                .With(p => p.Id, cathedraId)
                .Create();

            Academic academic = _autoFixture.Build<Academic>()
                .With(p => p.Id, academicId)
                .Create();

            AcademicUpdateRequest academicUpdateRequest = _autoFixture.Build<AcademicUpdateRequest>()
                .With(p => p.Id, academicId)
                .With(p => p.CathedraId, cathedraId)
                .Create();

            _cathedraRepositoryMock.Setup(p => p.GetValueById(cathedraId))
                .ReturnsAsync(cathedra);
            _academicRepositoryMock.Setup(p => p.Update(It.IsAny<Academic>()))
                .ReturnsAsync(academicUpdateRequest.ToAcademic());

            //Act
            AcademicResponse? academicResponse = await _academicService.Update(academicUpdateRequest);

            //Assert
            Assert.NotNull(academicResponse);
            Assert.Equal(academicUpdateRequest.Id, academicResponse.Id);
            Assert.Equal(academicUpdateRequest.Name, academicResponse.Name);
        }
        #endregion

        #region Remove
        //When id of not existing academic is passed to the method, it should throw KeyNotFoundException
        [Fact]
        public async Task Remove_InvalidId()
        {
            //Arrange
            Guid academicId = Guid.NewGuid();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _academicService.Remove(academicId);
            });
        }
        #endregion
    }
}

using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.Services;
using Moq;

namespace Campus.Test;

public class DisciplineServiceTest
{
    private readonly Mock<IRepository<Discipline>> _disciplineRepositoryMock;
    private readonly Mock<IRepository<Cathedra>> _cathedraRepositoryMock;
    private readonly DisciplineService _disciplineService;

    public DisciplineServiceTest()
    {
        _disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
        _cathedraRepositoryMock = new Mock<IRepository<Cathedra>>();
        _disciplineService = new DisciplineService(_disciplineRepositoryMock.Object, _cathedraRepositoryMock.Object);
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnDisciplineResponse()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline { Id = disciplineId, Name = "Math" };
        _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(discipline);

        // Act
        var result = await _disciplineService.GetDisciplineById(disciplineId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(disciplineId, result.Id);
        Assert.Equal(discipline.Name, result.Name);
    }

    [Fact]
    public async Task GetById_WithNonexistentId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync((Discipline?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _disciplineService.GetDisciplineById(disciplineId));
        _disciplineRepositoryMock.Verify(r => r.GetValueById(disciplineId), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfDisciplineResponses()
    {
        // Arrange
        var disciplines = new List<Discipline>
        {
            new Discipline { Id = Guid.NewGuid(), Name = "Math" },
            new Discipline { Id = Guid.NewGuid(), Name = "Physics" }
        };
        _disciplineRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(disciplines);

        // Act
        var result = await _disciplineService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(disciplines.Count, result.Count());
        Assert.All(result, disciplineResponse =>
        {
            Assert.NotNull(disciplineResponse);
            Assert.Contains(disciplines, d => d.Id == disciplineResponse.Id && d.Name == disciplineResponse.Name);
        });
    }
    
    [Fact]
    public async Task GetByCathedraId_WithNonexistentCathedraId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var cathedraId = Guid.NewGuid();
        _cathedraRepositoryMock.Setup(r => r.GetValueById(cathedraId)).ReturnsAsync((Cathedra?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _disciplineService.GetByCathedraId(cathedraId));
    }
    
    [Fact]
    public async Task GetByCathedraId_WithExistingCathedraId_ShouldReturnListOfDisciplineResponses()
    {
        // Arrange
        var cathedraId = Guid.NewGuid();
        var cathedra = new Cathedra { Id = cathedraId, Name = "Computer Science" };
        var disciplines = new List<Discipline>
        {
            new Discipline { Id = Guid.NewGuid(), Name = "Math", CathedraId = cathedraId },
            new Discipline { Id = Guid.NewGuid(), Name = "Physics", CathedraId = cathedraId }
        };
        _cathedraRepositoryMock.Setup(r => r.GetValueById(cathedraId)).ReturnsAsync(cathedra);
        _disciplineRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(disciplines);

        // Act
        var result = await _disciplineService.GetByCathedraId(cathedraId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(disciplines.Count, result.Count());
        Assert.All(result, disciplineResponse =>
        {
            Assert.NotNull(disciplineResponse);
            Assert.Contains(disciplines, d => d.Id == disciplineResponse.Id 
                                              && d.Name == disciplineResponse.Name 
                                              && d.CathedraId == disciplineResponse.CathedraId);
        });
    }
    [Fact]
    public async Task Create_WithValidDisciplineRequest_ShouldReturnCreatedDisciplineResponse()
    {
        // Arrange
        var disciplineRequest = new DisciplineAddRequest { Name = "Chemistry" };
        var discipline = disciplineRequest.ToDiscipline();
        _disciplineRepositoryMock.Setup(r => r.Create(discipline)).Returns(Task.CompletedTask);

        // Act
        var result = await _disciplineService.Add(disciplineRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(discipline.Name, result.Name);
    }

    [Fact]
    public async Task Create_WithNullDisciplineRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _disciplineService.Add(null));
    }

    [Fact]
    public async Task Update_WithValidDisciplineRequest_ReturnsUpdatedDisciplineResponse()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var existingDiscipline = new Discipline { Id = disciplineId, Name = "Math" };
        var disciplineRequest = new DisciplineUpdateRequest { Id = disciplineId, Name = "Biology" };
        var updatedDiscipline = disciplineRequest.ToDiscipline();
        _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(existingDiscipline);
        _disciplineRepositoryMock.Setup(r => r.Update(It.IsAny<Discipline>())).ReturnsAsync(updatedDiscipline);

        // Act
        var result = await _disciplineService.Update(disciplineRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedDiscipline.Id, result.Id);
        Assert.Equal(updatedDiscipline.Name, result.Name);
    }

    [Fact]
    public async Task Update_WithNullDisciplineRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _disciplineService.Update(null));
    }

    [Fact]
    public async Task Update_WithNonexistentDiscipline_ThrowsKeyNotFoundException()
    {
        // Arrange
        var disciplineRequest = new DisciplineUpdateRequest { Id = Guid.NewGuid(), Name = "Biology" };
        _disciplineRepositoryMock.Setup(r => r.Update(It.IsAny<Discipline>())).ReturnsAsync((Discipline?)null);
        

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _disciplineService.Update(disciplineRequest));
    }

    [Fact]
    public async Task Delete_WithNonexistentId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync((Discipline?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _disciplineService.Remove(disciplineId));
    }

    [Fact]
    public async Task Create_WithNonExistentCathedraId_ShouldThrowKeyNotFoundException()
    {
        
        // Arrange
        var disciplineRequest = new DisciplineAddRequest { Name = "Chemistry", CathedraId = Guid.NewGuid() };
        var discipline = disciplineRequest.ToDiscipline();
        _cathedraRepositoryMock.Setup(r => r.GetValueById(disciplineRequest.CathedraId)).ReturnsAsync((Cathedra?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _disciplineService.Add(disciplineRequest));
        
    }
    
    [Fact]
    public async Task Update_WithNonExistentCathedraId_ShouldThrowKeyNotFoundException()
    {
        
        // Arrange
        var disciplineRequest = new DisciplineUpdateRequest { Name = "Chemistry", CathedraId = Guid.NewGuid() };
        var discipline = disciplineRequest.ToDiscipline();
        _cathedraRepositoryMock.Setup(r => r.GetValueById(disciplineRequest.CathedraId)).ReturnsAsync((Cathedra?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _disciplineService.Update(disciplineRequest));
        
    }
    
}
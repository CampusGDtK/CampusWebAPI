using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.Services;
using Moq;

namespace Campus.Test;

public class StudyProgramServiceTest
{
    private readonly Mock<IRepository<StudyProgram>> _studyProgramRepositoryMock;
    private readonly Mock<IRepository<Speciality>> _specialityRepositoryMock;
    private readonly Mock<IRepository<Cathedra>> _cathedraRepositoryMock;
    private readonly StudyProgramService _studyProgramService;

    public StudyProgramServiceTest()
    {
        _studyProgramRepositoryMock = new Mock<IRepository<StudyProgram>>();
        _specialityRepositoryMock = new Mock<IRepository<Speciality>>();
        _cathedraRepositoryMock = new Mock<IRepository<Cathedra>>();
        _studyProgramService = new StudyProgramService(
            _studyProgramRepositoryMock.Object,
            _specialityRepositoryMock.Object,
            _cathedraRepositoryMock.Object
        );
    }

    [Fact]
    public async Task GetById_ExistingStudyProgram_ShouldReturnStudyProgramResponse()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        var studyProgram = new StudyProgram { Id = studyProgramId };
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramId))
            .ReturnsAsync(studyProgram);

        // Act
        var result = await _studyProgramService.GetById(studyProgramId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(studyProgramId, result.Id);
        // Add more assertions as needed
    }

    [Fact]
    public async Task GetById_NonExistingStudyProgram_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramId))
            .ReturnsAsync((StudyProgram?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.GetById(studyProgramId));
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllStudyProgramResponses()
    {
        // Arrange
        var studyPrograms = new List<StudyProgram>
        {
            new StudyProgram { Id = Guid.NewGuid(), Name = "Test1"},
            new StudyProgram { Id = Guid.NewGuid(), Name = "Test2" }
        };
        _studyProgramRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(studyPrograms);

        // Act
        var result = await _studyProgramService.GetAll();

        // Assert
        Assert.Equal(studyPrograms.Count, result.Count());
        Assert.All(result, studyProgramResponse =>
        {
            Assert.NotNull(studyProgramResponse);
            Assert.Contains(studyPrograms, s => s.Id == studyProgramResponse.Id && s.Name == studyProgramResponse.Name);
        });
    }
    [Fact]
    public async Task GetByCathedraId_ExistingCathedra_ShouldReturnMatchingStudyProgramResponses()
    {
        // Arrange
        var cathedraId = Guid.NewGuid();
        var studyPrograms = new List<StudyProgram>
        {
            new StudyProgram { Id = Guid.NewGuid(),CathedraId = cathedraId },
            new StudyProgram { Id = Guid.NewGuid(),CathedraId = cathedraId }
        };
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync(new Cathedra { Id = cathedraId });
        _studyProgramRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(studyPrograms);

        // Act
        var result = await _studyProgramService.GetByCathedraId(cathedraId);

        // Assert
        Assert.Equal(studyPrograms.Count(), result.Count());
        Assert.All(result, studyProgramResponse =>
        {
            Assert.NotNull(studyProgramResponse);
            Assert.Contains(studyPrograms, s => s.Id == studyProgramResponse.Id && s.CathedraId == studyProgramResponse.CathedraId);
        });
    }

    [Fact]
    public async Task GetByCathedraId_NonExistingCathedra_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var cathedraId = Guid.NewGuid();
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync((Cathedra?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.GetByCathedraId(cathedraId));
    }
    
    [Fact]
    public async Task GetBySpecialityId_ExistingSpeciality_ShouldReturnMatchingStudyProgramResponses()
    {
        // Arrange
        var specialityId = Guid.NewGuid();
        var studyPrograms = new List<StudyProgram>
        {
            new StudyProgram { Id = Guid.NewGuid(),SpecialityId = specialityId },
            new StudyProgram { Id = Guid.NewGuid(),SpecialityId = specialityId }
        };
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync(new Speciality { Id = specialityId });
        _studyProgramRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(studyPrograms);

        // Act
        var result = await _studyProgramService.GetBySpecialityId(specialityId);

        // Assert
        Assert.Equal(studyPrograms.Count(), result.Count());
        Assert.All(result, studyProgramResponse =>
        {
            Assert.NotNull(studyProgramResponse);
            Assert.Contains(studyPrograms, s => s.Id == studyProgramResponse.Id && s.SpecialityId == studyProgramResponse.SpecialityId);
        });
    }
    
    [Fact]
    public async Task GetBySpecialityId_NonExistingSpeciality_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var specialityId = Guid.NewGuid();
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync((Speciality?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.GetBySpecialityId(specialityId));
    }
    
    [Fact]
    public async Task GetBySpecialityAndCathedra_ExistentCathedraAndSpeciality_ShouldReturnMatchingStudyProgramResponses()
    {
        // Arrange
        var specialityId = Guid.NewGuid();
        var cathedraId = Guid.NewGuid();
        var studyPrograms = new List<StudyProgram>
        {
            new StudyProgram { Id = Guid.NewGuid(),SpecialityId = specialityId,CathedraId = cathedraId },
            new StudyProgram { Id = Guid.NewGuid(),SpecialityId = specialityId,CathedraId = cathedraId }
        };
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync(new Speciality { Id = specialityId });
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync(new Cathedra { Id = cathedraId });
        _studyProgramRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(studyPrograms);

        // Act
        var result = await _studyProgramService.GetBySpecialityAndCathedraId(cathedraId,specialityId);

        // Assert
        Assert.Equal(studyPrograms.Count(), result.Count());
        Assert.All(result, studyProgramResponse =>
        {
            Assert.NotNull(studyProgramResponse);
            Assert.Contains(studyPrograms, s => s.Id == studyProgramResponse.Id && s.SpecialityId == studyProgramResponse.SpecialityId && s.CathedraId == studyProgramResponse.CathedraId);
        });
    }
    
    [Fact]
    public async Task GetBySpecialityAndCathedra_NonExistentCathedra_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var specialityId = Guid.NewGuid();
        var cathedraId = Guid.NewGuid();
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync(new Speciality { Id = specialityId });
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync((Cathedra?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.GetBySpecialityAndCathedraId(specialityId,cathedraId));
    }
    
    [Fact]
    public async Task GetBySpecialityAndCathedra_NonExistentSpeciality_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var specialityId = Guid.NewGuid();
        var cathedraId = Guid.NewGuid();
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync((Speciality?)null);
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync(new Cathedra { Id = cathedraId });

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.GetBySpecialityAndCathedraId(specialityId,cathedraId));
    }

    [Fact]
    public async Task Add_ValidStudyProgramAddRequest_shouldCreateStudyProgramAndReturnsStudyProgramResponse()
    {
        // Arrange
        var cathedraId = Guid.NewGuid();
        var specialityId = Guid.NewGuid();
        var studyProgramAddRequest = new StudyProgramAddRequest { Name = "Test",CathedraId = cathedraId, SpecialityId = specialityId };
        var createdStudyProgram = studyProgramAddRequest.ToStudyProgram();
        _studyProgramRepositoryMock.Setup(repo => repo.Create(It.IsAny<StudyProgram>()))
            .Callback<StudyProgram>(studyProgram => studyProgram.Id = createdStudyProgram.Id)
            .Returns(Task.CompletedTask);
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync(new Cathedra { Id = cathedraId });
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync(new Speciality { Id = specialityId });
        

        // Act
        var result = await _studyProgramService.Add(studyProgramAddRequest);

        // Assert
        Assert.Equal(createdStudyProgram.Id, result.Id);
    }
    
    [Fact]
    public async Task Add_ValidStudyProgramWithNonExistentCathedra_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studyProgramAddRequest = new StudyProgramAddRequest { Name = "Test", CathedraId = Guid.NewGuid()};
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(studyProgramAddRequest.CathedraId))
            .ReturnsAsync((Cathedra?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.Add(studyProgramAddRequest));
    }
    
    [Fact]
    public async Task Add_ValidStudyProgramWithNonExistentSpeciality_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studyProgramAddRequest = new StudyProgramAddRequest { Name = "Test", SpecialityId = Guid.NewGuid()};
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(studyProgramAddRequest.SpecialityId))
            .ReturnsAsync((Speciality?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.Add(studyProgramAddRequest));
    }

    [Fact]
    public async Task Update_ExistingStudyProgramUpdateRequest_ShouldReturnStudyProgramResponse()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        var specialityId = Guid.NewGuid();
        var cathedraId = Guid.NewGuid();
        var studyProgramUpdateRequest = new StudyProgramUpdateRequest { Id = studyProgramId,Name = "Test", CathedraId = cathedraId,SpecialityId = specialityId};
        var existingStudyProgram = new StudyProgram { Id = studyProgramId };
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramUpdateRequest.Id))
            .ReturnsAsync(existingStudyProgram);
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync(new Cathedra { Id = cathedraId });
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync(new Speciality { Id = specialityId });
        _studyProgramRepositoryMock.Setup(repo => repo.Update(It.IsAny<StudyProgram>()))
            .ReturnsAsync(studyProgramUpdateRequest.ToStudyProgram());

        // Act
        var result = await _studyProgramService.Update(studyProgramUpdateRequest);

        // Assert
        Assert.Equal(studyProgramUpdateRequest.Id, result.Id);
        Assert.Equal(studyProgramUpdateRequest.CathedraId, result.CathedraId);
        Assert.Equal(studyProgramUpdateRequest.SpecialityId, result.SpecialityId);
    }
    
    [Fact]
    public async Task Update_NonExistingStudyProgramUpdateRequest_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        var studyProgramUpdateRequest = new StudyProgramUpdateRequest { Id = studyProgramId, Name = "Test" };
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramUpdateRequest.Id))
            .ReturnsAsync((StudyProgram?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.Update(studyProgramUpdateRequest));
    }
    
    [Fact]
    public async Task Update_StudyProgramUpdateRequestWithNonExistingCathedra_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        var cathedraId = Guid.NewGuid();
        var studyProgramUpdateRequest = new StudyProgramUpdateRequest { Id = studyProgramId, Name = "Test", CathedraId = cathedraId };
        var existingStudyProgram = new StudyProgram { Id = studyProgramId };
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramUpdateRequest.Id))
            .ReturnsAsync(existingStudyProgram);
        _cathedraRepositoryMock.Setup(repo => repo.GetValueById(cathedraId))
            .ReturnsAsync((Cathedra?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.Update(studyProgramUpdateRequest));
    }
    
    [Fact]
    public async Task Update_StudyProgramUpdateRequestWithNonExistingSpeciality_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        var specialityId = Guid.NewGuid();
        var studyProgramUpdateRequest = new StudyProgramUpdateRequest { Id = studyProgramId, Name = "Test", SpecialityId = specialityId };
        var existingStudyProgram = new StudyProgram { Id = studyProgramId };
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramUpdateRequest.Id))
            .ReturnsAsync(existingStudyProgram);
        _specialityRepositoryMock.Setup(repo => repo.GetValueById(specialityId))
            .ReturnsAsync((Speciality?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.Update(studyProgramUpdateRequest));
    }

    [Fact]
    public async Task Delete_NonExistingStudyProgram_ThrowsKeyNotFoundException()
    {
        // Arrange
        var studyProgramId = Guid.NewGuid();
        _studyProgramRepositoryMock.Setup(repo => repo.GetValueById(studyProgramId))
            .ReturnsAsync((StudyProgram?)null);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studyProgramService.Delete(studyProgramId));
    }
}
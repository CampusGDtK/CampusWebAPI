using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.Services;
using Moq;

namespace Campus.Test;

public class StudentServiceTests
{
    private readonly Mock<IRepository<Student>> _studentRepositoryMock;
    private readonly Mock<IRepository<Group>> _groupRepositoryMock;
    private readonly StudentService _studentService;

    public StudentServiceTests()
    {
        _studentRepositoryMock = new Mock<IRepository<Student>>();
        _groupRepositoryMock = new Mock<IRepository<Group>>();
        _studentService = new StudentService(_studentRepositoryMock.Object, _groupRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = Guid.NewGuid(), FullName = "John Doe", GroupId = Guid.NewGuid() },
            new Student { Id = Guid.NewGuid(), FullName = "Jane Smith", GroupId = Guid.NewGuid() }
        };
        _studentRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(students);

        // Act
        var result = await _studentService.GetAll();

        // Assert
        Assert.Equal(students.Count, result.Count());
        Assert.All(result, studentResponse =>
        {
            Assert.NotNull(studentResponse);
            Assert.Contains(students, s => s.Id == studentResponse.Id && s.FullName == studentResponse.FullName);
        });
    }

    [Fact]
    public async Task GetById_ExistingId_ShouldReturnStudent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var student = new Student { Id = studentId, FullName = "John Doe", GroupId = Guid.NewGuid() };
        _studentRepositoryMock.Setup(repo => repo.GetValueById(studentId)).ReturnsAsync(student);

        // Act
        var result = await _studentService.GetById(studentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(student.Id, result.Id);
        Assert.Equal(student.FullName, result.FullName);
    }

    [Fact]
    public async Task GetById_NonExistingId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _studentRepositoryMock.Setup(repo => repo.GetValueById(studentId)).ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studentService.GetById(studentId));
    }
    
    [Fact]
    public async Task GetByGroupId_ExistingId_ShouldReturnStudents()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var students = new List<Student>
        {
            new Student { Id = Guid.NewGuid(), FullName = "John Doe", GroupId = groupId },
            new Student { Id = Guid.NewGuid(), FullName = "Jane Smith", GroupId = groupId }
        };
        _groupRepositoryMock.Setup(repo => repo.GetValueById(groupId)).ReturnsAsync(new Group { Id = groupId });
        _studentRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(students);

        // Act
        var result = await _studentService.GetByGroupId(groupId);

        // Assert
        Assert.Equal(students.Count, result.Count());
        Assert.All(result, studentResponse =>
        {
            Assert.NotNull(studentResponse);
            Assert.Contains(students, s => s.Id == studentResponse.Id && s.FullName == studentResponse.FullName && s.GroupId == groupId);
        });
    }
    
    [Fact]
    public async Task GetByGroupId_NonExistingId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        _groupRepositoryMock.Setup(repo => repo.GetValueById(groupId)).ReturnsAsync((Group?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studentService.GetByGroupId(groupId));
    }

    [Fact]
    public async Task Update_ExistingStudent_ShouldReturnUpdatedStudent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var request = new StudentUpdateRequest
        {
            Id = studentId,
            FullName = "Updated Name",
            GroupId = Guid.NewGuid()
        };
        var existingStudent = new Student
        {
            Id = studentId,
            FullName = "John Doe",
            GroupId = Guid.NewGuid()
        };
        var updatedStudent = new Student
        {
            Id = studentId,
            FullName = request.FullName,
            GroupId = request.GroupId
        };

        _studentRepositoryMock.Setup(repo => repo.Update(It.IsAny<Student>()))
                             .ReturnsAsync(updatedStudent);
        _studentRepositoryMock.Setup(repo => repo.GetValueById(studentId))
                             .ReturnsAsync(existingStudent);

        // Act
        var result = await _studentService.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedStudent.Id, result.Id);
        Assert.Equal(updatedStudent.FullName, result.FullName);
        Assert.Equal(updatedStudent.GroupId, result.GroupId);
    }

    [Fact]
    public async Task Update_NonExistingStudent_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var request = new StudentUpdateRequest
        {
            Id = studentId,
            FullName = "Updated Name",
            GroupId = Guid.NewGuid()
        };

        _studentRepositoryMock.Setup(repo => repo.Update(It.IsAny<Student>()))
                             .ReturnsAsync((Student?)null);
        _studentRepositoryMock.Setup(repo => repo.GetValueById(studentId))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studentService.Update(request));
    }
    
    [Fact]
    public async Task Update_ExistingStudentWithNonExistingGroupId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var request = new StudentUpdateRequest
        {
            Id = studentId,
            FullName = "Updated Name",
            GroupId = Guid.NewGuid()
        };
        var existingStudent = new Student
        {
            Id = studentId,
            FullName = "John Doe",
            GroupId = Guid.NewGuid()
        };

        _studentRepositoryMock.Setup(repo => repo.Update(It.IsAny<Student>()))
                             .ReturnsAsync((Student?)null);
        _studentRepositoryMock.Setup(repo => repo.GetValueById(studentId))
                             .ReturnsAsync(existingStudent);
        _groupRepositoryMock.Setup(repo => repo.GetValueById(request.GroupId))
                            .ReturnsAsync((Group?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studentService.Update(request));
    }

    [Fact]
    public async Task Delete_NonExistingStudent_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _studentRepositoryMock.Setup(repo => repo.Delete(studentId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _studentService.Delete(studentId));
    }
}
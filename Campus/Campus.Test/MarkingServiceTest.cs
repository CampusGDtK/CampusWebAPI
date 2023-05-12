using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Test
{
    public class MarkingServiceTests
    {
        private readonly Mock<IRepository<CurrentControl>> _currentControlRepositoryMock;
        private readonly Mock<IRepository<Student>> _studentRepositoryMock;
        private readonly Mock<IRepository<Discipline>> _disciplineRepositoryMock;
        private readonly MarkingService _markingService;

        public MarkingServiceTests()
        {
            _currentControlRepositoryMock = new Mock<IRepository<CurrentControl>>();
            _studentRepositoryMock = new Mock<IRepository<Student>>();
            _disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
            _markingService = new MarkingService(
                _currentControlRepositoryMock.Object,
                _studentRepositoryMock.Object,
                _disciplineRepositoryMock.Object
            );
        }

        #region GetByStudentAndDisciplineId

        [Fact]
        public async Task GetByStudentAndDisciplineId_Should_Return_MarkResponse()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            var currentControl = new CurrentControl
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Detail = JsonConvert.SerializeObject(new List<string> { "Topic 1", "Topic 2" }),
                Mark = JsonConvert.SerializeObject(new List<int> { 80, 90 }),
                TotalMark = 170
            };
            _studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(new Student());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(new Discipline());
            _currentControlRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CurrentControl> { currentControl });

            // Act
            var result = await _markingService.GetByStudentAndDisciplineId(studentId, disciplineId);

            // Assert
            Assert.Equal(studentId, result.StudentId);
            Assert.Equal(disciplineId, result.DisciplineId);
            Assert.Equal(new List<string> { "Topic 1", "Topic 2" }, result.Details);
            Assert.Equal(new List<int> { 80, 90 }, result.Marks);
            Assert.Equal(170, result.TotalMark);
        }

        [Fact]
        public async Task GetByStudentAndDisciplineId_Should_Throw_KeyNotFoundException_When_Student_Does_Not_Exist()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            _studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync((Student)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _markingService.GetByStudentAndDisciplineId(studentId, disciplineId));
        }

        [Fact]
        public async Task GetByStudentAndDisciplineId_Should_Throw_KeyNotFoundException_When_Discipline_Does_Not_Exist()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            _studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(new Student());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync((Discipline)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _markingService.GetByStudentAndDisciplineId(studentId, disciplineId));
        }

        [Fact]
        public async Task GetByStudentAndDisciplineId_Should_Throw_ArgumentException_When_Student_Has_Not_Mark_For_Discipline()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            var student = new Student() { Id = studentId };

            var discipline = new Discipline() { Id = disciplineId };

            var currentControls = new List<CurrentControl>() 
            {
                new CurrentControl() { StudentId = Guid.NewGuid(), DisciplineId = Guid.NewGuid() },
                new CurrentControl() { StudentId = Guid.NewGuid(), DisciplineId = Guid.NewGuid() }
            };

            var currentControlRepositoryMock = new Mock<IRepository<CurrentControl>>();
            currentControlRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(currentControls);

            var studentRepositoryMock = new Mock<IRepository<Student>>();
            studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(student);

            var disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
            disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(discipline);

            var markingService = new MarkingService(currentControlRepositoryMock.Object, studentRepositoryMock.Object, disciplineRepositoryMock.Object);
            
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => markingService.GetByStudentAndDisciplineId(studentId, disciplineId));
        }

        [Fact]
        public async Task GetByStudentAndDisciplineId_Should_Return_MarkResponse_When_Student_Has_Mark_For_Discipline()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            var student = new Student() { Id = studentId };
            var discipline = new Discipline() { Id = disciplineId };
            var currentControl = new CurrentControl()
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                DisciplineId = disciplineId,
                Detail = JsonConvert.SerializeObject(new List<string> { "Topic 1", "Topic 2", "Topic 3" }),
                Mark = JsonConvert.SerializeObject(new List<int> { 80, 90, 70 }),
                TotalMark = 240
            };
            var currentControls = new List<CurrentControl>() { currentControl };
            var currentControlRepositoryMock = new Mock<IRepository<CurrentControl>>();
            currentControlRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(currentControls);
            var studentRepositoryMock = new Mock<IRepository<Student>>();
            studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(student);
            var disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
            disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(discipline);
            var markingService = new MarkingService(currentControlRepositoryMock.Object, studentRepositoryMock.Object, disciplineRepositoryMock.Object);

            // Act
            var result = await markingService.GetByStudentAndDisciplineId(studentId, disciplineId);

            // Assert
            Assert.Equal(studentId, result.StudentId);
            Assert.Equal(disciplineId, result.DisciplineId);
            Assert.Equal(new List<string> { "Topic 1", "Topic 2", "Topic 3" }, result.Details);
            Assert.Equal(new List<int> { 80, 90, 70 }, result.Marks);
            Assert.Equal(240, result.TotalMark);
        }
        #endregion

        #region SetMark
        [Fact]
        public async Task SetMark_Should_Throw_KeyNotFoundException_When_StudentId_Is_Not_Found()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            var markSetRequest = new MarkSetRequest()
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Marks = new List<int> { 90, 85, 95 }
            };
            _studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync((Student?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _markingService.SetMark(markSetRequest));
        }

        [Fact]
        public async Task SetMark_Should_Throw_KeyNotFoundException_When_DisciplineId_Is_Not_Found()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            var markSetRequest = new MarkSetRequest()
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Marks = new List<int> { 90, 85, 95 }
            };
            _studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(new Student());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync((Discipline?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _markingService.SetMark(markSetRequest));
        }

        [Fact]
        public async Task SetMark_Should_Throw_ArgumentException_When_Marks_Count_Does_Not_Match_Syllabus_Count()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            var markSetRequest = new MarkSetRequest()
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Marks = new List<int> { 90, 85, 95 }
            };
            var currentControl = new CurrentControl()
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Detail = JsonConvert.SerializeObject(new List<string> { "Topic 1", "Topic 2" }),
                Mark = JsonConvert.SerializeObject(new List<int> { 80, 75 }),
                TotalMark = 155
            };
            _studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(new Student());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(new Discipline());
            _currentControlRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CurrentControl> { currentControl });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _markingService.SetMark(markSetRequest));
        }

        [Fact]
        public async Task SetMark_Should_Update_CurrentControl_And_Return_MarkResponse()
        {
            // Arrange
            Guid studentId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            List<string> details = new List<string> { "Topic 1", "Topic 2", "Topic 3" };
            List<int> marks = new List<int> { 85, 90, 95 };
            int totalMark = marks.Sum();

            var student = new Student { Id = studentId };
            var discipline = new Discipline { Id = disciplineId };
            var currentControl = new CurrentControl { Id = Guid.NewGuid(), StudentId = studentId, DisciplineId = disciplineId, Detail = JsonConvert.SerializeObject(details), Mark = JsonConvert.SerializeObject(marks), TotalMark = totalMark };

            var studentRepositoryMock = new Mock<IRepository<Student>>();
            var disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
            var currentControlRepositoryMock = new Mock<IRepository<CurrentControl>>();

            studentRepositoryMock.Setup(r => r.GetValueById(studentId)).ReturnsAsync(student);
            disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(discipline);
            currentControlRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CurrentControl> { currentControl });

            var markingService = new MarkingService(currentControlRepositoryMock.Object, studentRepositoryMock.Object, disciplineRepositoryMock.Object);

            // Act
            var result = await markingService.GetByStudentAndDisciplineId(studentId, disciplineId);

            // Assert
            Assert.Equal(studentId, result.StudentId);
            Assert.Equal(disciplineId, result.DisciplineId);
            Assert.Equal(details, result.Details);
            Assert.Equal(marks, result.Marks);
            Assert.Equal(totalMark, result.TotalMark);
        }
        #endregion
    }
}

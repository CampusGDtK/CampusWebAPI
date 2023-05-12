using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.Services;
using FluentAssertions.Common;
using Moq;
using Newtonsoft.Json;

namespace Campus.Test
{
    public class SyllabusServiceTests
    {
        private readonly Mock<IRepository<AcademicDisciplineGroup>> _adgRepositoryMock;
        private readonly Mock<IRepository<Academic>> _academicRepositoryMock;
        private readonly Mock<IRepository<Discipline>> _disciplineRepositoryMock;
        private readonly Mock<IRepository<Student>> _studentRepositoryMock;
        private readonly Mock<IRepository<CurrentControl>> _currentControlRepositoryMock;
        private readonly SyllabusService _syllabusService;

        public SyllabusServiceTests()
        {
            _adgRepositoryMock = new Mock<IRepository<AcademicDisciplineGroup>>();
            _academicRepositoryMock = new Mock<IRepository<Academic>>();
            _disciplineRepositoryMock = new Mock<IRepository<Discipline>>();
            _studentRepositoryMock = new Mock<IRepository<Student>>();
            _currentControlRepositoryMock = new Mock<IRepository<CurrentControl>>();

            _syllabusService = new SyllabusService(
                _adgRepositoryMock.Object,
                _academicRepositoryMock.Object,
                _disciplineRepositoryMock.Object,
                _studentRepositoryMock.Object,
                _currentControlRepositoryMock.Object);
        }

        #region Delete
        [Fact]
        public async Task DeleteSyllabus_ShouldThrowKeyNotFoundException_WhenAcademicIdIsNotFound()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(x => x.GetValueById(academicId)).ReturnsAsync(null as Academic);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _syllabusService.DeleteSyllabus(academicId, disciplineId));
        }

        [Fact]
        public async Task DeleteSyllabus_ShouldThrowKeyNotFoundException_WhenDisciplineIdIsNotFound()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(x => x.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(x => x.GetValueById(disciplineId)).ReturnsAsync(null as Discipline);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _syllabusService.DeleteSyllabus(academicId, disciplineId));
        }

        [Fact]
        public async Task DeleteSyllabus_ShouldThrowArgumentException_WhenNoRelationBetweenAcademicAndDiscipline()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(x => x.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(x => x.GetValueById(disciplineId)).ReturnsAsync(new Discipline());
            _adgRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<AcademicDisciplineGroup>());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _syllabusService.DeleteSyllabus(academicId, disciplineId));
        }
        #endregion

        #region GetSyllabus
        [Fact]
        public async Task GetSyllabus_Should_Return_Syllabus_For_Existing_Academic_And_Discipline()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            AcademicDisciplineGroup adg = new AcademicDisciplineGroup
            {
                AcademicId = academicId,
                DisciplineId = disciplineId,
                GroupId = Guid.NewGuid()
            };
            _adgRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new[] { adg });

            Academic academic = new Academic { Id = academicId };
            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(academic);

            Discipline discipline = new Discipline { Id = disciplineId };
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(discipline);

            Guid studentId = Guid.NewGuid();
            Student student = new Student { Id = studentId, GroupId = adg.GroupId };
            _studentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new[] { student });

            CurrentControl currentControl = new CurrentControl
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Detail = JsonConvert.SerializeObject(new[] { "Syllabus item 1", "Syllabus item 2" })
            };
            _currentControlRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new[] { currentControl });

            // Act
            IEnumerable<string> syllabus = await _syllabusService.GetSyllabus(academicId, disciplineId);

            // Assert
            Assert.NotNull(syllabus);
            Assert.Equal(2, syllabus.Count());
            Assert.Equal("Syllabus item 1", syllabus.ElementAt(0));
            Assert.Equal("Syllabus item 2", syllabus.ElementAt(1));
        }

        [Fact]
        public async Task GetSyllabus_Should_Throw_KeyNotFoundException_For_Nonexistent_Academic()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(null as Academic);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _syllabusService.GetSyllabus(academicId, disciplineId));
        }

        [Fact]
        public async Task GetSyllabus_Should_Throw_KeyNotFoundException_For_Nonexistent_Discipline()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(null as Discipline);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _syllabusService.GetSyllabus(academicId, disciplineId));
        }

        [Fact]
        public async Task GetSyllabus_Should_Throw_ArgumentException_When_No_Relations_Exist()
        {
            // Arrange
            var academicId = Guid.NewGuid();
            var disciplineId = Guid.NewGuid();
            List<AcademicDisciplineGroup> adgList = new List<AcademicDisciplineGroup>();
            List<Student> studentList = new List<Student>();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(new Discipline());
            _adgRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(adgList);
            _studentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(studentList);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _syllabusService.GetSyllabus(academicId, disciplineId));
        }
        #endregion

        #region SetSyllabus
        [Fact]
        public async Task SetSyllabus_Should_Throw_KeyNotFoundException_When_Academic_Not_Found()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            IEnumerable<string> syllabus = new List<string>();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync((Academic)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _syllabusService.SetSyllabus(academicId, disciplineId, syllabus));
        }

        [Fact]
        public async Task SetSyllabus_Should_Throw_KeyNotFoundException_When_Discipline_Not_Found()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            IEnumerable<string> syllabus = new List<string>();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync((Discipline)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _syllabusService.SetSyllabus(academicId, disciplineId, syllabus));
        }

        [Fact]
        public async Task SetSyllabus_Should_Throw_ArgumentException_When_No_Relations_Between_Academic_And_Disciplines()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();
            IEnumerable<string> syllabus = new List<string>();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(new Discipline());
            _adgRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<AcademicDisciplineGroup>());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _syllabusService.SetSyllabus(academicId, disciplineId, syllabus));
        }

        [Fact]
        public async Task SetSyllabus_Should_Throw_Exception_When_Syllabus_Is_Null()
        {
            // Arrange
            Guid academicId = Guid.NewGuid();
            Guid disciplineId = Guid.NewGuid();

            _academicRepositoryMock.Setup(r => r.GetValueById(academicId)).ReturnsAsync(new Academic());
            _disciplineRepositoryMock.Setup(r => r.GetValueById(disciplineId)).ReturnsAsync(new Discipline());

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _syllabusService.SetSyllabus(academicId, disciplineId, null));
        }
        #endregion
    }
}

using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Campus.Core.Services
{
    public class MarkingService : IMarkingService
    {
        private readonly IRepository<CurrentControl> _currentControlRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Discipline> _disciplineRepository;

        public MarkingService(IRepository<CurrentControl> currentControlRepository, IRepository<Student> studentRepository, IRepository<Discipline> disciplineRepository)
        {
            _currentControlRepository = currentControlRepository;
            _studentRepository = studentRepository;
            _disciplineRepository = disciplineRepository;
        }

        public async Task<MarkResponse> GetByStudentAndDisciplineId(Guid studentId, Guid disciplineId)
        {
            if (await _studentRepository.GetValueById(studentId) is null)
                throw new KeyNotFoundException(nameof(studentId));

            if (await _disciplineRepository.GetValueById(disciplineId) is null)
                throw new KeyNotFoundException(nameof(disciplineId));

            CurrentControl? currentControl = (await _currentControlRepository.GetAll())
                .FirstOrDefault(control => control.StudentId == studentId && control.DisciplineId == disciplineId);

            if (currentControl == null)
                throw new ArgumentException("Student has not mark for discipline.");

            IEnumerable<string>? details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail);

            IEnumerable<int>? marks = JsonConvert.DeserializeObject<IEnumerable<int>>(currentControl.Mark);

            MarkResponse markResponse = new MarkResponse()
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                Details = details,
                Marks = marks,
                TotalMark = currentControl.TotalMark,
            };

            return markResponse;
        }

        public async Task<MarkResponse> SetMark(MarkSetRequest markSetRequest)
        {
            if (await _studentRepository.GetValueById(markSetRequest.StudentId) is null)
                throw new KeyNotFoundException(nameof(markSetRequest.StudentId));

            if (await _disciplineRepository.GetValueById(markSetRequest.DisciplineId) is null)
                throw new KeyNotFoundException(nameof(markSetRequest.DisciplineId));

            CurrentControl? currentControl = (await _currentControlRepository.GetAll())
                .FirstOrDefault(control => control.StudentId == markSetRequest.StudentId 
                && control.DisciplineId == markSetRequest.DisciplineId);

            if (currentControl == null)
                throw new ArgumentException("Student has not mark for discipline.");

            IEnumerable<string>? details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail);

            if (details.Count() != markSetRequest.Marks.Count())
                throw new ArgumentException("Size of marks collection does not match to size of syllabus.");

            currentControl.Mark = JsonConvert.SerializeObject(markSetRequest.Marks);
            currentControl.TotalMark = markSetRequest.Marks.Sum();
            CurrentControl? currentControlUpdated = await _currentControlRepository.Update(currentControl);

            IEnumerable<int>? marks = JsonConvert.DeserializeObject<IEnumerable<int>>(currentControlUpdated.Mark);

            MarkResponse markResponse = new MarkResponse()
            {
                StudentId = currentControlUpdated.StudentId,
                DisciplineId = currentControlUpdated.DisciplineId,
                Details = details,
                Marks = marks,
                TotalMark = currentControlUpdated.TotalMark
            };

            return markResponse;
        }
    }
}

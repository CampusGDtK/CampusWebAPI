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
        private readonly IRepository<Group> _groupRepository;
        private readonly IDisciplineService _disciplineService;

        public MarkingService(IRepository<CurrentControl> currentControlRepository, IRepository<Student> studentRepository, 
            IRepository<Discipline> disciplineRepository, IRepository<Group> groupRepository, 
            IDisciplineService disciplineService)
        {
            _currentControlRepository = currentControlRepository;
            _studentRepository = studentRepository;
            _disciplineRepository = disciplineRepository;
            _groupRepository = groupRepository;
            _disciplineService = disciplineService;
        }

        public async Task<IEnumerable<MarkResponse>> GetByGroupAndDisciplineId(Guid groupId, Guid disciplineId)
        {
            if (await _groupRepository.GetValueById(groupId) is null)
                throw new KeyNotFoundException("Id of group not found");

            Discipline? discipline = await _disciplineRepository.GetValueById(disciplineId);

            if (discipline is null)
                throw new KeyNotFoundException("Id of discipline not found");

            var sudentsIds = (await _studentRepository.GetAll()).Where(x => x.GroupId == groupId).Select(x => x.Id);

            List<MarkResponse> responses = new List<MarkResponse>();

            var currentControls = (await _currentControlRepository.GetAll())
                .Where(x => x.DisciplineId == disciplineId && sudentsIds.Contains(x.StudentId));

            foreach (var currentControl in currentControls)
            {
                IEnumerable<string>? details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail);

                IEnumerable<int>? marks = JsonConvert.DeserializeObject<IEnumerable<int>>(currentControl.Mark);

                responses.Add(
                new MarkResponse()
                {
                    StudentId = currentControl.StudentId,
                    DisciplineId = disciplineId,
                    DisciplineName = discipline.Name,
                    Details = details,
                    Marks = marks,
                    TotalMark = currentControl.TotalMark,
                });
            }

            return responses;
        }

        public async Task<MarkResponse> GetByStudentAndDisciplineId(Guid studentId, Guid disciplineId)
        {
            if (await _studentRepository.GetValueById(studentId) is null)
                throw new KeyNotFoundException("Id of student not found");

            Discipline? discipline = await _disciplineRepository.GetValueById(disciplineId);

            if (discipline is null)
                throw new KeyNotFoundException("Id of discipline not found");

            CurrentControl? currentControl = (await _currentControlRepository.GetAll())
                .FirstOrDefault(control => control.StudentId == studentId && control.DisciplineId == disciplineId);

            if (currentControl == null)
                throw new ArgumentException("Student has not mark for discipline");

            IEnumerable<string>? details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail);

            IEnumerable<int>? marks = JsonConvert.DeserializeObject<IEnumerable<int>>(currentControl.Mark);

            MarkResponse markResponse = new MarkResponse()
            {
                StudentId = studentId,
                DisciplineId = disciplineId,
                DisciplineName = discipline.Name,
                Details = details,
                Marks = marks,
                TotalMark = currentControl.TotalMark,
            };

            return markResponse;
        }

        public async Task<IEnumerable<MarkResponse>> GetByStudentId(Guid studentId)
        {
            if (await _studentRepository.GetValueById(studentId) is null)
                throw new KeyNotFoundException("Id of student not found");

            List<MarkResponse> marks = (await _currentControlRepository.GetAll())
                .Where(currentControl => currentControl.StudentId == studentId)
                .Select(currentControl => new MarkResponse()
                {
                    StudentId = studentId,
                    DisciplineId = currentControl.DisciplineId,
                    Marks = JsonConvert.DeserializeObject<IEnumerable<int>>(currentControl.Mark),
                    Details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail),
                })
                .ToList();

            IEnumerable<DisciplineResponse> disciplines = await _disciplineService.GetByStudentId(studentId);

            foreach(var mark in marks)
            {
                var discipline = disciplines.FirstOrDefault(x => x.Id == mark.DisciplineId);

                if(discipline != null)
                {
                    mark.DisciplineName = discipline.Name;
                }
            }

            return marks;
        }

        public async Task<MarkResponse> SetMark(MarkSetRequest markSetRequest)
        {
            if (markSetRequest == null)
                throw new ArgumentNullException("MarkSetRequest is null");

            if (await _studentRepository.GetValueById(markSetRequest.StudentId) is null)
                throw new KeyNotFoundException("Id of student not found");

            Discipline? discipline = await _disciplineRepository.GetValueById(markSetRequest.DisciplineId);

            if (discipline is null)
                throw new KeyNotFoundException("Id of discipline not found");

            CurrentControl? currentControl = (await _currentControlRepository.GetAll())
                .FirstOrDefault(control => control.StudentId == markSetRequest.StudentId
                && control.DisciplineId == markSetRequest.DisciplineId);

            if (currentControl == null)
                throw new ArgumentException("Student has not mark for discipline");

            IEnumerable<string>? details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail);

            if (details.Count() != markSetRequest.Marks.Count())
                throw new ArgumentException("Size of marks collection does not match to size of syllabus");

            currentControl.Mark = JsonConvert.SerializeObject(markSetRequest.Marks);
            currentControl.TotalMark = markSetRequest.Marks.Sum();
            CurrentControl? currentControlUpdated = await _currentControlRepository.Update(currentControl);

            IEnumerable<int>? marks = JsonConvert.DeserializeObject<IEnumerable<int>>(currentControlUpdated.Mark);

            MarkResponse markResponse = new MarkResponse()
            {
                StudentId = currentControlUpdated.StudentId,
                DisciplineId = currentControlUpdated.DisciplineId,
                DisciplineName = discipline.Name,
                Details = details,
                Marks = marks,
                TotalMark = currentControlUpdated.TotalMark
            };

            return markResponse;
        }
    }
}

using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.ServiceContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.Services
{
    public class SyllabusService : ISyllabusService
    {
        private readonly IRepository<AcademicDisciplineGroup> _adgRepository;
        private readonly IRepository<Academic> _academicRepository;
        private readonly IRepository<Discipline> _disciplineRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<CurrentControl> _currentControlRepository;

        public SyllabusService(IRepository<AcademicDisciplineGroup> adgRepository,
            IRepository<Academic> academicRepository, IRepository<Discipline> disciplineRepository,
            IRepository<Student> studentRepository, IRepository<CurrentControl> currentControlRepository)
        {
            _adgRepository = adgRepository;
            _academicRepository = academicRepository;
            _disciplineRepository = disciplineRepository;
            _studentRepository = studentRepository;
            _currentControlRepository = currentControlRepository;
        }

        public async Task DeleteSyllabus(Guid academicId, Guid disciplineId)
        {
            if (await _academicRepository.GetValueById(academicId) is null)
                throw new KeyNotFoundException("Id of academic not found");

            if (await _disciplineRepository.GetValueById(disciplineId) is null)
                throw new KeyNotFoundException("Id of discipline not found");

            List<Guid> groupsId = (await _adgRepository.GetAll())
                .Where(adg => adg.AcademicId == academicId && adg.DisciplineId == disciplineId)
                .Select(adg => adg.GroupId)
                .ToList();

            if (groupsId.Count == 0)
                throw new ArgumentException("There are not realations between academic and disciplines");

            List<Guid> studentsId = (await _studentRepository.GetAll())
                .Where(student => groupsId.Contains(student.GroupId))
                .Select(student => student.Id)
                .ToList();

            foreach (Guid studentId in studentsId)
            {
                CurrentControl currentControl = (await _currentControlRepository.GetAll())
                    .First(control => control.StudentId == studentId && control.DisciplineId == disciplineId);

                await _currentControlRepository.Delete(currentControl.Id);
            }
        }

        public async Task<IEnumerable<string>> GetSyllabus(Guid academicId, Guid disciplineId)
        {
            if (await _academicRepository.GetValueById(academicId) is null)
                throw new KeyNotFoundException("Id of academic not found");

            if (await _disciplineRepository.GetValueById(disciplineId) is null)
                throw new KeyNotFoundException("Id of discipline not found");

            Guid? groupId = (await _adgRepository.GetAll())
                .FirstOrDefault(adg => adg.AcademicId == academicId && adg.DisciplineId == disciplineId)?
                .GroupId;

            if (groupId == null)
                throw new ArgumentException("There are not realations between academic and disciplines");

            Guid studentId = (await _studentRepository.GetAll())
                .First(student => student.GroupId == groupId)
                .Id;

            CurrentControl currentControl = (await _currentControlRepository.GetAll())
                .First(control => control.StudentId == studentId && control.DisciplineId == disciplineId);

            IEnumerable<string> details = JsonConvert.DeserializeObject<IEnumerable<string>>(currentControl.Detail);

            return details;
        }

        public async Task SetSyllabus(Guid academicId, Guid disciplineId, IEnumerable<string> syllabus)
        {
            if (await _academicRepository.GetValueById(academicId) is null)
                throw new KeyNotFoundException("Id of academic not found");

            if (await _disciplineRepository.GetValueById(disciplineId) is null)
                throw new KeyNotFoundException("Id of discipline not found");

            if(syllabus is null)
                throw new ArgumentNullException("Syllabus is null");

            List<Guid> groupsId = (await _adgRepository.GetAll())
                .Where(adg => adg.AcademicId == academicId && adg.DisciplineId == disciplineId)
                .Select(adg => adg.GroupId)
                .ToList();

            if (groupsId.Count == 0)
                throw new ArgumentException("There are not realations between academic and disciplines");

            List<Guid> studentsId = (await _studentRepository.GetAll())
                .Where(student => groupsId.Contains(student.GroupId))
                .Select(student => student.Id)
                .ToList();

            foreach (Guid studentId in studentsId)
            {
                CurrentControl currentControl = (await _currentControlRepository.GetAll())
                    .First(control => control.StudentId == studentId && control.DisciplineId == disciplineId);

                List<int> marks = JsonConvert.DeserializeObject<List<int>>(currentControl.Mark);

                int countNewMarks = syllabus.Count() - marks.Count();

                for (int i = 0; i < countNewMarks; i++)
                {
                    marks.Add(0);
                }

                currentControl.Detail = JsonConvert.SerializeObject(syllabus);
                currentControl.Mark = JsonConvert.SerializeObject(marks);

                await _currentControlRepository.Update(currentControl);
            }
        }
    }
}

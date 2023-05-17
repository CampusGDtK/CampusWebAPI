using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;

namespace Campus.Core.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Group> _groupRepository;

    public StudentService(IRepository<Student> _studentRepository, IRepository<Group> _groupRepository)
    {
        this._studentRepository = _studentRepository;
        this._groupRepository = _groupRepository;
    }
    
    public async Task<IEnumerable<StudentResponse>> GetAll()
    {
        var students = await _studentRepository.GetAll();
        return students.Select(s => s.ToStudentResponse());
    }
    
    public async Task<StudentResponse> GetById(Guid id)
    {
        var student = await _studentRepository.GetValueById(id);
        if (student is null)
        {
            throw new KeyNotFoundException("Id of student not found");
        }
        
        return student.ToStudentResponse();
    }
    
    public async Task<IEnumerable<StudentResponse>> GetByGroupId(Guid groupId)
    {
        if (await _groupRepository.GetValueById(groupId) is null)
        {
            throw new KeyNotFoundException("Id of group not found");
        }
        
        var students = await _studentRepository.GetAll();
        return students.Where(s => s.GroupId == groupId).Select(s => s.ToStudentResponse());
    }

    public async Task<StudentResponse> Create(StudentAddRequest? request)
    {
        if (request is null)
        {
            throw new ArgumentNullException("StudentAddRequest is null");
        }
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            GroupId = request.GroupId
        };
        await _studentRepository.Create(student);

        return student.ToStudentResponse();
    }
    
    public async Task<StudentResponse> Update(StudentUpdateRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException("StudentUpdateRequest is null");
        }
        var student = new Student
        {
            Id = request.Id,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            GroupId = request.GroupId
        };
        var result = await _studentRepository.Update(student);
        if (result is null)
        {
            throw new KeyNotFoundException("Id of student not found");
        }
        return result.ToStudentResponse();
    }

    public async Task Delete(Guid id)
    {
        var result = await _studentRepository.Delete(id);
        if (!result)
        {
            throw new KeyNotFoundException("Student not found");
        }
    }
    
}
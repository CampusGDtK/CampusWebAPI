using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IMarkingService _markingService;

    public StudentsController(IStudentService studentService, IMarkingService markingService)
    {
        _studentService = studentService;
        _markingService = markingService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]Guid? groupId)
    {
        if(groupId is null)
        {
            return Ok(await _studentService.GetAll());
        }
        return Ok(await _studentService.GetByGroupId(groupId.Value));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _studentService.GetById(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]StudentAddRequest? request)
    {
        return Ok(await _studentService.Create(request!));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]StudentUpdateRequest? request)
    {
        return Ok(await _studentService.Update(request!));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studentService.Delete(id);
        return Ok();
    }
    
    [HttpGet("{studentId}/marks")]
    public async Task<IActionResult> GetMarks(Guid studentId)
    {
        return Ok(await _markingService.GetByStudentId(studentId));
    }
    
    [HttpGet("{studentId}/marks/{disciplineId}")]
    public async Task<IActionResult> GetMarkInDiscipline(Guid studentId, Guid disciplineId)
    {
        return Ok(await _markingService.GetByStudentAndDisciplineId(studentId, disciplineId));
    }
    
    [HttpPost("{studentId}/marks/{disciplineId}")]
    public async Task<IActionResult> AddMark(Guid studentId, Guid disciplineId, [FromBody]IEnumerable<int> marks)
    {
        var markSetRequest = new MarkSetRequest
        {
            StudentId = studentId,
            DisciplineId = disciplineId,
            Marks = marks
        };
        return Ok(await _markingService.SetMark(markSetRequest));
    }
    
}
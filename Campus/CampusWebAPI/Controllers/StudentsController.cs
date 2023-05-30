using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CampusWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IMarkingService _markingService;
    private readonly IDisciplineService _disciplineService;

    public StudentsController(IStudentService studentService, IMarkingService markingService, 
        IDisciplineService disciplineService)
    {
        _studentService = studentService;
        _markingService = markingService;
        _disciplineService = disciplineService;
    }

    [Authorize(Roles = "Admin,Academic")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]Guid? groupId)
    {
        if(groupId is null)
        {
            return Ok(await _studentService.GetAll());
        }
        return Ok(await _studentService.GetByGroupId(groupId.Value));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _studentService.GetById(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]StudentUpdateRequest? request)
    {
        return Ok(await _studentService.Update(request!));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studentService.Delete(id);
        return Ok("Successfully deleted");
    }

    [Authorize]
    [HttpGet("{studentId}/disciplines")]
    public async Task<IActionResult> GetDisciplines(Guid studentId)
    {
        return Ok(await _disciplineService.GetByStudentId(studentId));
    }

    [Authorize]
    [HttpGet("{studentId}/marks")]
    public async Task<IActionResult> GetMarks(Guid studentId)
    {
        return Ok(await _markingService.GetByStudentId(studentId));
    }

    [Authorize]
    [HttpGet("{studentId}/marks/{disciplineId}")]
    public async Task<IActionResult> GetMarkInDiscipline(Guid studentId, Guid disciplineId)
    {
        return Ok(await _markingService.GetByStudentAndDisciplineId(studentId, disciplineId));
    }

    [Authorize(Roles = "Academic")]
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
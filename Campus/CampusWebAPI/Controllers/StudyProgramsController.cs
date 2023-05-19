using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudyProgramsController : ControllerBase
{
    private readonly IStudyProgramService _studyProgramService;

    public StudyProgramsController(IStudyProgramService studyProgramService)
    {
        _studyProgramService = studyProgramService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]Guid? specialityId, [FromQuery]Guid? cathedraId)
    {
        if(specialityId is null && cathedraId is null)
        {
            return Ok(await _studyProgramService.GetAll());
        }
        if(specialityId is null)
        {
            return Ok(await _studyProgramService.GetByCathedraId(cathedraId.Value));
        }
        
        return Ok(await _studyProgramService.GetBySpecialityId(specialityId.Value));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _studyProgramService.GetById(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]StudyProgramAddRequest? request)
    {
        return Ok(await _studyProgramService.Add(request));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]StudyProgramUpdateRequest? request)
    {
        return Ok(await _studyProgramService.Update(request));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studyProgramService.Delete(id);
        return Ok("Successfully deleted");
    }
}
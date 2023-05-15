using Campus.Core.DTO;
using Campus.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers;

public class DisciplineController : Controller
{
    private readonly DisciplineService _disciplineService;

    public DisciplineController(DisciplineService disciplineService)
    {
        _disciplineService = disciplineService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetDisciplines([FromQuery]Guid? cathedraId)
    {
        if (cathedraId is null)
        {
            return Ok(await _disciplineService.GetAll());
        }
        return Ok(await _disciplineService.GetByCathedraId(cathedraId.Value));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDisciplineById(Guid id)
    {
        return Ok(await _disciplineService.GetDisciplineById(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDiscipline([FromBody] DisciplineAddRequest? disciplineRequest)
    {
        return Ok(await _disciplineService.Add(disciplineRequest));
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateDiscipline([FromBody] DisciplineUpdateRequest? disciplineRequest)
    {
        return Ok(await _disciplineService.Update(disciplineRequest));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscipline(Guid id)
    {
        await _disciplineService.Remove(id);
        return Ok();
    }
}
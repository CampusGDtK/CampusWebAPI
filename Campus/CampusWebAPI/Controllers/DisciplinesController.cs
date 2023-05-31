using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CampusWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DisciplinesController : ControllerBase
{
    private readonly IDisciplineService _disciplineService;

    public DisciplinesController(IDisciplineService disciplineService)
    {
        _disciplineService = disciplineService;
    }

    [Authorize(Roles = "Admin,Academic")]
    [HttpGet]
    public async Task<IActionResult> GetDisciplines([FromQuery]Guid? cathedraId)
    {
        if (cathedraId is null)
        {
            return Ok(await _disciplineService.GetAll());
        }
        return Ok(await _disciplineService.GetByCathedraId(cathedraId.Value));
    }

    [Authorize(Roles = "Admin,Academic")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDisciplineById(Guid id)
    {
        return Ok(await _disciplineService.GetDisciplineById(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddDiscipline([FromBody] DisciplineAddRequest? disciplineRequest)
    {
        return Ok(await _disciplineService.Add(disciplineRequest!));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateDiscipline([FromBody] DisciplineUpdateRequest? disciplineRequest)
    {
        return Ok(await _disciplineService.Update(disciplineRequest!));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscipline(Guid id)
    {
        await _disciplineService.Remove(id);
        return Ok("Successfully deleted");
    }
}
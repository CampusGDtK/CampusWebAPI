﻿using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CampusWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class DisciplinesController : ControllerBase
{
    private readonly IDisciplineService _disciplineService;

    public DisciplinesController(IDisciplineService disciplineService)
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
        return Ok(await _disciplineService.Add(disciplineRequest!));
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateDiscipline([FromBody] DisciplineUpdateRequest? disciplineRequest)
    {
        return Ok(await _disciplineService.Update(disciplineRequest!));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscipline(Guid id)
    {
        await _disciplineService.Remove(id);
        return Ok("Successfully deleted");
    }
}
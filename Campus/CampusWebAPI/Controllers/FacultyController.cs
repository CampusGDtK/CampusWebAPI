using AutoMapper.Configuration;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;

namespace CampusWebAPI.Controllers
{
    [Route("/faculties")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFaculties()
        {
            return Ok(await _facultyService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> CreateFaculty(FacultyAddRequest facultyAddRequest)
        {
            try
            {
                await _facultyService.Add(facultyAddRequest);
            }
            catch (Exception)
            {
                return (StatusCode(405));
            }

            return StatusCode(200);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFaculty(FacultyUpdateRequest facultyUpdateRequest)
        {
            FacultyResponse facultyResponse;

            try
            {
                facultyResponse =  await _facultyService.Update(facultyUpdateRequest);
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404);
            }
            catch (ArgumentException)
            {
                return StatusCode(405);
            }

            return Ok(facultyResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByFacultyId(Guid id)
        {
            FacultyResponse response;

            try
            {
                response = await _facultyService.GetById(id);
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404);             
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaculty(Guid id)
        {
            try
            {
                await _facultyService.Remove(id);
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404);
            }

            return Ok();
        }
    }
}

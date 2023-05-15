using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
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
            await _facultyService.Add(facultyAddRequest);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFaculty(FacultyUpdateRequest facultyUpdateRequest)
        {
            return Ok(await _facultyService.Update(facultyUpdateRequest));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByFacultyId(Guid id)
        {
            return Ok(await _facultyService.GetById(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaculty(Guid id)
        {
            await _facultyService.Remove(id);
            return Ok();
        }
    }
}

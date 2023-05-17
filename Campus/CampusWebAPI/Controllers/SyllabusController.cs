using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly ISyllabusService _syllabusService;

        public SyllabusController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSyllabus([FromQuery]Guid academicId, [FromQuery] Guid disciplineId)
        {
            return Ok(await _syllabusService.GetSyllabus(academicId, disciplineId));
        }

        [HttpPost]
        public async Task<IActionResult> SetSyllabus([FromQuery] Guid academicId, [FromQuery] Guid disciplineId, [FromBody]IEnumerable<string> syllabus)
        {
            await _syllabusService.SetSyllabus(academicId, disciplineId, syllabus);
            return Ok("Successfully set");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSyllabus([FromQuery] Guid academicId, [FromQuery] Guid disciplineId)
        {
            await _syllabusService.DeleteSyllabus(academicId, disciplineId);
            return Ok("Successfully deleted");
        }
    }
}

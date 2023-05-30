using Campus.Core.DTO;
using Campus.Core.Enums;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicsController : ControllerBase
    {
        private readonly IAcademicService _academicService;
        private readonly IADGService _adgService;

        public AcademicsController(IAcademicService academicService, IADGService adgService)
        {
            _academicService = academicService;
            _adgService = adgService;
        }
        [Authorize(Roles = "Admin,Academic")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcademicResponse>>> GetAll([FromQuery]Guid? cathedraId)
        {
            if(cathedraId is null)
            {
                return Ok(await _academicService.GetAll());
            }

            return Ok(await _academicService.GetByCathedraId(cathedraId.Value));
        }

        [Authorize(Roles = "Admin,Academic")]
        [HttpGet("{academicId}")]
        public async Task<ActionResult<AcademicResponse>> GetById(Guid academicId)
        {
            return Ok(await _academicService.GetAcademicById(academicId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<AcademicResponse>> Update([FromBody]AcademicUpdateRequest? academicUpdateRequest)
        {
            return Ok(await _academicService.Update(academicUpdateRequest));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{academicId}")]
        public async Task<ActionResult> Delete(Guid academicId)
        {
            await _academicService.Remove(academicId);
            return Ok("Successfully deleted");
        }

        [Authorize(Roles = "Admin,Academic")]
        [HttpGet("{academicId}/disciplines")]
        public async Task<ActionResult<IEnumerable<DisciplineResponse>>> GetDisciplinesByAcademicId(Guid academicId)
        {
            return Ok(await _adgService.GetDisciplinesByAcademicId(academicId));
        }

        [Authorize(Roles = "Admin,Academic")]
        [HttpGet("{academicId}/disciplines/{disciplineId}/groups")]
        public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroupsByDisciplineAndAcademicId(Guid academicId, Guid disciplineId)
        {
            return Ok(await _adgService.GetGroupsByDisciplineAndAcademicId(academicId, disciplineId));
        }
    }
}

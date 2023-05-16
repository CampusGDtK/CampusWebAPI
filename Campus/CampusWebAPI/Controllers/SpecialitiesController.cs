using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly ISpecialityService _specialityService;

        public SpecialitiesController(ISpecialityService specialityService)
        {
            _specialityService = specialityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialityResponse>>> GetAll([FromQuery]Guid? facultyId)
        {
            if(facultyId is null)
            {
                return Ok(await _specialityService.GetAll());
            }

            return Ok(await _specialityService.GetByFacultyId(facultyId.Value));
        }

        [HttpGet("{specialityId}")]
        public async Task<ActionResult<SpecialityResponse>> GetById(Guid specialityId)
        {
            return Ok(await _specialityService.GetSpecialityById(specialityId));
        }

        [HttpPost]
        public async Task<ActionResult<SpecialityResponse>> Create([FromBody]SpecialityAddRequest? specialityAddRequest)
        {
            return Ok(await _specialityService.Add(specialityAddRequest));
        }

        [HttpPut]
        public async Task<ActionResult<SpecialityResponse>> Update([FromBody]SpecialityUpdateRequest? specialityUpdateRequest)
        {
            return Ok(await _specialityService.Update(specialityUpdateRequest));
        }

        [HttpDelete("{specialityId}")]
        public async Task<ActionResult> Delete(Guid specialityId)
        {
            await _specialityService.Remove(specialityId);
            return Ok("Successfully deleted");
        }
    }
}

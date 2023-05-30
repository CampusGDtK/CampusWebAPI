using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/adg-relation")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ADGController : ControllerBase
    {
        private readonly IADGService _adgService;

        public ADGController(IADGService adgService)
        {
            _adgService = adgService;
        }

        [HttpPost]
        public async Task<IActionResult> SetRelation([FromBody, ModelBinder(typeof(ADGSetRequestModelBinder))] ADGSetRequest adgSetRequest)
        {
            await _adgService.SetRelation(adgSetRequest);
            return Ok("Successfully set ADG relation");
        }

        [HttpDelete("{academicId}")]
        public async Task<IActionResult> ResetRelation(Guid academicId)
        {
            await _adgService.ResetRelation(academicId);
            return Ok("Successfully reset");
        }
    }
}

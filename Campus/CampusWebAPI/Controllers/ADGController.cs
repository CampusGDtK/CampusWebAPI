using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/adg-relation")]
    [ApiController]
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
            try
            {
                await _adgService.SetRelation(adgSetRequest);
            }
            catch(ArgumentNullException)
            {
                return BadRequest("Null object");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Id not found");
            }

            return Ok("Successfully set ADG relation");
        }

        [HttpDelete("{academicId}")]
        public async Task<IActionResult> ResetRelation(Guid academicId)
        {
            try
            {
                await _adgService.ResetRelation(academicId);
            }
            catch(ArgumentNullException)
            {
                return BadRequest("Null object");
            }
            catch(ArgumentException)
            {
                return BadRequest("Academic has no relation");
            }
            catch(KeyNotFoundException)
            {
                return NotFound("Id not found");
            }

            return Ok("Successfully reset");
        }
    }
}

using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CathedrasController : ControllerBase
    {
        private readonly ICathedraService _cathedraService;

        public CathedrasController(ICathedraService cathedraService)
        {
            _cathedraService = cathedraService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]Guid? facultyId)
        {
            IEnumerable<CathedraResponse> result;

            if (facultyId is null)
                result = await _cathedraService.GetAll();
            else
                result = await _cathedraService.GetByFacultyId(facultyId.Value);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CathedraAddRequest cathedraAddRequest)
        {
            var result = await _cathedraService.Add(cathedraAddRequest);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CathedraUpdateRequest cathedraUpdateRequest)
        {
            var result = await _cathedraService.Update(cathedraUpdateRequest);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var result = await _cathedraService.GetById(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _cathedraService.Delete(id);
            return Ok();
        }

    }
}

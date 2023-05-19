using Campus.Core.Domain.Entities;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMarkingService _markingService;

        public GroupsController(IGroupService groupService, IMarkingService markingService)
        {
            _groupService = groupService;
            _markingService = markingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups([FromQuery] Guid? facultyId)
        {
            IEnumerable<GroupResponse> result;
            if (facultyId is null)
                result = await _groupService.GetAll();
            else
                result = await _groupService.GetByFacultyId(facultyId.Value);           

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody]GroupAddRequest group)
        {
            var result = await _groupService.Add(group);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupUpdateRequest group)
        {
            var result = await _groupService.Update(group);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById([FromRoute]Guid id)
        {
            var result = await _groupService.GetById(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup([FromRoute]Guid id)
        {
            await _groupService.Delete(id);
            return Ok("Successfully deleted");
        }

        [HttpGet("{groupId}/marks/{disciplineId}")]
        public async Task<IActionResult> GetMarksOfStudentsFromGroupOfDiscipline([FromRoute] Guid groupId, [FromRoute] Guid disciplineId)
        {
            var result = await _markingService.GetByGroupAndDisciplineId(groupId, disciplineId);
            return Ok(result);
        }

    }
}

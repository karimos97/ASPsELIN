using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Groups.Commnand.CreateGroup;
using GroupPoster.ApplicationLayer.Groups.Commnand.DeleteGroup;
using GroupPoster.ApplicationLayer.Groups.Commnand.UpdateGroup;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using Microsoft.AspNetCore.Mvc;

namespace GroupPoster.WebUI.Controllers
{
    [ApiController]
    [Route("/api/groups")]
    public class GroupController : ControllerBase
    {
        private readonly ISystemPersistence persistence;

        public GroupController(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await new GetGroupsQueryHandler(persistence).Handle(new ()));


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateGroupCommand command) =>
            Ok(await new CreateGroupCommandHandler(persistence).Handle(command));

        [HttpDelete]
        public async Task Delete([FromBody] DeleteGroupCommand command) =>
            await new DeleteGroupCommandHandler(persistence).Handle(command);


        [HttpPut]
        public async Task Update([FromBody] UpdateGroupCommand command) =>
            await new UpdatedGroupCommandHandler(persistence).Handle(command);
    }
}

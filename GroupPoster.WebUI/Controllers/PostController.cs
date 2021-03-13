using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Posts.Command.CreatePost;
using GroupPoster.ApplicationLayer.Posts.Command.DeletePost;
using GroupPoster.ApplicationLayer.Posts.Command.UpdatePost;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;
using Microsoft.AspNetCore.Mvc;

namespace GroupPoster.WebUI.Controllers
{
    [ApiController]
    [Route("/api/posts")]
    public class PostController : ControllerBase
    {
        private readonly ISystemPersistence persistence;

        public PostController(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await new GetPostsQueryHandler(persistence).Handle(new GetPostsQuery()));


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePostCommand command) =>
            Ok(await new CreatePostCommandHandler(persistence).Handle(command));

        [HttpDelete]
        public async Task Delete([FromBody] DeletePostCommand command) =>
            await new DeletePostCommandHandler(persistence).Handle(command);


        [HttpPut]
        public async Task Update([FromBody] UpdatePostCommand command) =>
            await new UpdatePostCommandHandler(persistence).Handle(command);
    }
}

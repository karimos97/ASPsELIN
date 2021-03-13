using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Actions.Posting;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GroupPoster.WebUI.Controllers
{
    [ApiController]
    [Route("/api/actions")]
    public class ActionController : ControllerBase
    {
        private readonly IFBAccess fBAccess;

        public ActionController(IFBAccess fBAccess)
        {
            this.fBAccess = fBAccess;
        }

        [HttpPost]
        public async Task PostToGroups([FromBody] PostingCommand command)
        {
            using (fBAccess)
            {
                await new PostingCommandHandler(fBAccess).Handle(command);
            }
        }
    }
}

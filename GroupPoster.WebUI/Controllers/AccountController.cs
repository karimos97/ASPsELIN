using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Accounts.Commnand.CreateAccount;
using GroupPoster.ApplicationLayer.Accounts.Commnand.DeleteAccount;
using GroupPoster.ApplicationLayer.Accounts.Commnand.UpdateAccount;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GroupPoster.WebUI.Controllers
{
    [ApiController]
    [Route("/api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly ISystemPersistence persistence;

        public AccountController(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await new GetAccountsQueryHandler(persistence).Handle(new GetAccountsQuery()));


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAccountCommand command) =>
            Ok(await new CreateAccountCommandHandler(persistence).Handle(command));

        [HttpDelete]
        public async Task Delete([FromBody] DeleteAccountCommand command) =>
            await new DeleteAccountCommandHandler(persistence).Handle(command);


        [HttpPut]
        public async Task Update([FromBody] UpdateAccountCommand command) =>
            await new UpdateAccountCommandHandler(persistence).Handle(command);
    }
}

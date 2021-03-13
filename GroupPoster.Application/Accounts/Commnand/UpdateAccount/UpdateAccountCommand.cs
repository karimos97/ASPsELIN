using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;

namespace GroupPoster.ApplicationLayer.Accounts.Commnand.UpdateAccount
{
    public class UpdateAccountCommand
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }

    public class UpdateAccountCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public UpdateAccountCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task Handle(UpdateAccountCommand request)
        {
            Account account = new()
            {
                Id = request.Id,
                Email = request.Email,
                Password = request.Password,
                //Status = Enum.Parse<AccountStatus>(request.Status)
            };

            return persistence.UpdateAccount(account);
        }
    }
}

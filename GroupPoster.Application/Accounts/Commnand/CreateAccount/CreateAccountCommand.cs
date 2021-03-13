using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Enums;
using GroupPoster.Domain.Entities;
using System.Net.Mail;

namespace GroupPoster.ApplicationLayer.Accounts.Commnand.CreateAccount
{
    public class CreateAccountCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public string Status { get; set; }
    }

    public class CreateAccountCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public CreateAccountCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task<int> Handle(CreateAccountCommand request)
        {
            Account account = new Account
            {
                Email = request.Email,
                Password = request.Password,
                //Status = Enum.Parse<AccountStatus>(request.Status)
            };

            return persistence.AddAccount(account);
        }
    }

}

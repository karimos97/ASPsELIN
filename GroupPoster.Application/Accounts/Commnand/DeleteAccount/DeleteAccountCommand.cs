using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;

namespace GroupPoster.ApplicationLayer.Accounts.Commnand.DeleteAccount
{
    public class DeleteAccountCommand
    {
        public int Id { get; set; }
    }

    public class DeleteAccountCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public DeleteAccountCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task Handle(DeleteAccountCommand request)
        {
            return persistence.DeleteAccount(request.Id);
        }
    }
}

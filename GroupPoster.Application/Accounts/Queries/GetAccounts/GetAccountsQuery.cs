using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts
{
    public class GetAccountsQuery
    {

    }

    public class GetAccountsQueryHandler
    {
        private readonly ISystemPersistence persistence;

        public GetAccountsQueryHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public async Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request)
        {
            IEnumerable<Account> accounts = await persistence.GetAllAccounts();
            return accounts.Select(x => AccountDto.From(x));
        }
    }
}

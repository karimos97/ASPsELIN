using System;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }

        public static AccountDto From(Account x) => new()
        {
            Id = x.Id,
            Email = x.Email,
            Password = x.Password,
            //Status = x.Status.ToString()
        };
    }
}
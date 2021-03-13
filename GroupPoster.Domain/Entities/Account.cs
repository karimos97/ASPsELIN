using GroupPoster.Domain.Enums;
using System;
using System.Net.Mail;

namespace GroupPoster.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public AccountStatus Status { get; set; }
    }
}

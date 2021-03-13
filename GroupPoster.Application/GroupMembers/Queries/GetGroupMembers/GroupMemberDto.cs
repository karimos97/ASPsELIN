using System;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;

namespace GroupPoster.ApplicationLayer.GroupMembers.Queries.GetGroupMembers
{
    public class GroupMemberDto
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GroupMemberStatus { get; set; }

        public static GroupMemberDto From(GroupMemberStatus status, Account account) => new GroupMemberDto()
        {
            AccountId = account.Id,
            Email = account.Email,
            Password = account.Password,
            GroupMemberStatus = status.ToString()
        };
    }
}
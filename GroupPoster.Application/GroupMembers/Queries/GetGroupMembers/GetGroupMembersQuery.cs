using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.GroupMembers.Queries.GetGroupMembers
{
    public class GetGroupMembersQuery
    {
        public int GroupId { get; set; }
        public string Status { get; set; }
    }

    public class GetGroupMembersQueryHandler
    {
        private readonly ISystemPersistence persistence;

        public GetGroupMembersQueryHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public async Task<IEnumerable<GroupMemberDto>> Handle(GetGroupMembersQuery request)
        {
            var groupMembers = await persistence
                .GetGroupMembers(request.GroupId)
                .ConfigureAwait(false);
            
            var accounts = await persistence
                .GetAllAccounts()
                .ConfigureAwait(false);

            List<GroupMemberDto> groupMemberDtos = new ();

            foreach (var groupMember in groupMembers)
            {
                Account account = accounts.FirstOrDefault(x => x.Id == groupMember.AccountId);

                if (account is null)
                    continue;

                GroupMemberDto dto = GroupMemberDto.From(groupMember.Status, account);
                groupMemberDtos.Add(dto);
            }

            return groupMemberDtos;
        }
    }
}

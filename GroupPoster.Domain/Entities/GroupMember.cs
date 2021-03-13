using GroupPoster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupPoster.Domain.Entities
{
    public class GroupMember
    {
        public int GroupId { get; set; }
        public int AccountId { get; set; }
        public GroupMemberStatus Status { get; set; }
    }
}

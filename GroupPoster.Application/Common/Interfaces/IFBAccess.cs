using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;

namespace GroupPoster.ApplicationLayer.Common.Interfaces
{
    public interface IFBAccess : IDisposable
    {
        //Task<AccountStatus> CheckAccountStatus(Account account);
        GroupMemberStatus CheckGroupMemberStatus();
        //GroupStatus CheckGroupStatus();
        Task Post(string email, string passowrd, string group, string content);
        void Reset();
    }
}

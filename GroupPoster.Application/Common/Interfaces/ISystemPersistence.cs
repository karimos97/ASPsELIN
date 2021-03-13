using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using GroupPoster.ApplicationLayer.Posts.Command.CreatePost;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Common.Interfaces
{
    public interface ISystemPersistence
    {
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<int> AddAccount(Account account);
        Task UpdateAccount(Account updatedaccount);
        Task DeleteAccount(int id);
        
        Task<IEnumerable<Group>> GetAllGroups();
        Task<int> AddGroup(Group group);
        Task UpdateGroup(Group updatedgroup);
        Task DeleteGroup(int id);
     
        Task<IEnumerable<Post>> GetAllPosts();
        Task<IEnumerable<GroupMember>> GetGroupMembers(int groupId);
        Task<int> AddPost(Post post);
        Task UpdatePost(Post updatedpost);
        Task DeletePost(int id);
    }
}

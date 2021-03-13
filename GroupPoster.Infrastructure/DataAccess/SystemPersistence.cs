using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Exceptions;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;
using GroupPoster.Infrastructure.DataAccess.DataInteractor;

namespace GroupPoster.Infrastructure.DataAccess
{
    public class SystemPersistence : ISystemPersistence
    {
        private readonly FileInteractor context;

        private const string GroupMembers = "GroupMembers.json";
        private const string Accounts = "Account.json";
        private const string Groups = "Group.json";
        private const string Posts = "Posts.json";

        public SystemPersistence(FileInteractor context)
        {
            this.context = context;
        }

        #region Common

        private async Task<IEnumerable<T>> GetAllData<T>(string FileName)
        {
            (bool readSuccess, string data) = await context.Read(FileName).ConfigureAwait(false);

            if (readSuccess)
            {
                return JsonSerializer.Deserialize<IEnumerable<T>>(data);
            }

            return Enumerable.Empty<T>();
        }

        private Task SaveAllData<T>(string fileName, IEnumerable<T> data) =>
            context.Write(fileName, JsonSerializer.Serialize(data));

        #endregion

        #region Accounts

        public Task<IEnumerable<Account>> GetAllAccounts() => GetAllData<Account>(Accounts);

        public async Task<int> AddAccount(Account account)
        {
            List<Account> accounts = (await GetAllAccounts().ConfigureAwait(false)).ToList();

            account.Id = accounts.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            accounts.Add(account);

            await SaveAllAccounts(accounts).ConfigureAwait(false);

            return account.Id;
        }

        public async Task UpdateAccount(Account updatedAccount)
        {
            IEnumerable<Account> accounts = await GetAllAccounts();

            Account accountToModify =
                accounts.FirstOrDefault(x => x.Id == updatedAccount.Id)
                ?? throw new NotFoundException("The Wanted Account Was Not Found!");

            accountToModify.Email = updatedAccount.Email;
            accountToModify.Password = updatedAccount.Password;
            //accountToModify.Status = updatedAccount.Status;

            await SaveAllAccounts(accounts).ConfigureAwait(false);
        }

        public async Task DeleteAccount(int id)
        {
            IEnumerable<Account> accounts = await GetAllAccounts().ConfigureAwait(false);

            accounts = accounts.Where(x => x.Id != id);

            await SaveAllAccounts(accounts).ConfigureAwait(false);
        }

        private Task SaveAllAccounts(IEnumerable<Account> accounts) => SaveAllData(Accounts, accounts);

        #endregion

        #region Groups

        public Task<IEnumerable<Group>> GetAllGroups() => GetAllData<Group>(Groups);

        public async Task<int> AddGroup(Group group)
        {
            List<Group> groups = (await GetAllGroups().ConfigureAwait(false)).ToList();

            group.Id = groups.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            groups.Add(group);

            await SaveAllGroups(groups).ConfigureAwait(false);

            return group.Id;
        }

        public async Task UpdateGroup(Group updatedGroup)
        {
            IEnumerable<Group> groups = await GetAllGroups().ConfigureAwait(false);

            Group groupToModify =
                groups.FirstOrDefault(x => x.Id == updatedGroup.Id)
                ?? throw new NotFoundException("The Wanted Group Was Not Found!");

            groupToModify.Name = updatedGroup.Name;
            groupToModify.Link = updatedGroup.Link;
            //groupToModify.Status = updatedGroup.Status;

            await SaveAllGroups(groups).ConfigureAwait(false);
        }

        public async Task DeleteGroup(int id)
        {
            IEnumerable<Group> groups = await GetAllGroups().ConfigureAwait(false);

            groups = groups.Where(x => x.Id != id);

            await SaveAllGroups(groups).ConfigureAwait(false);
        }

        private Task SaveAllGroups(IEnumerable<Group> groups) => SaveAllData(Groups, groups);

        #endregion

        #region Posts

        public Task<IEnumerable<Post>> GetAllPosts() => GetAllData<Post>(Posts);

        public async Task<int> AddPost(Post post)
        {
            List<Post> posts = (await GetAllPosts().ConfigureAwait(false)).ToList();

            post.Id = posts.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            posts.Add(post);

            await SaveAllPosts(posts).ConfigureAwait(false);

            return post.Id;
        }

        public async Task UpdatePost(Post updatedpost)
        {
            IEnumerable<Post> posts = await GetAllPosts().ConfigureAwait(false);

            Post postToModify = posts.FirstOrDefault(x => x.Id == updatedpost.Id)
                ?? throw new NotFoundException("The Wanted Account Was Not Found!");

            postToModify.Title = updatedpost.Title;
            postToModify.Content = updatedpost.Content;

            await SaveAllPosts(posts).ConfigureAwait(false);
        }

        public async Task DeletePost(int id)
        {
            IEnumerable<Post> posts = await GetAllPosts().ConfigureAwait(false);
            posts = posts.Where(x => x.Id != id);
            await SaveAllPosts(posts).ConfigureAwait(false);
        }

        private Task SaveAllPosts(IEnumerable<Post> posts) => SaveAllData(Posts, posts);

        #endregion

        #region GroupMembers

        public Task<IEnumerable<GroupMember>> GetAllGroupMembers() => GetAllData<GroupMember>(GroupMembers);

        public Task<IEnumerable<GroupMember>> GetGroupMembers(int groupId) => GetAllData<GroupMember>(GroupMembers);

        public async Task AddGroupMembers(GroupMember groupMember)
        {
            List<GroupMember> groupMembers =
                (await GetAllGroupMembers()
                .ConfigureAwait(false))
                .ToList();

            groupMembers.Add(groupMember);

            await SaveAllGroupMembers(groupMembers)
                .ConfigureAwait(false);
        }

        public async Task UpdateGroupMembers(GroupMember updatedGroupMember)
        {
            IEnumerable<GroupMember> groupMembers =
                await GetAllGroupMembers()
                .ConfigureAwait(false);

            var groupMemberToModify = groupMembers.FirstOrDefault(x =>
            x.AccountId == updatedGroupMember.AccountId &&
            x.GroupId == updatedGroupMember.GroupId)
            ?? throw new NotFoundException("Group Members Not Fonund!!");

            groupMemberToModify.Status = updatedGroupMember.Status;

            await SaveAllGroupMembers(groupMembers)
                .ConfigureAwait(false);
        }

        public async Task DeleteGroupMembers(int groupId, int accountId)
        {
            IEnumerable<GroupMember> groupMembers =
                await GetAllGroupMembers()
                .ConfigureAwait(false);

            groupMembers = groupMembers.Where(x => x.AccountId != groupId || x.GroupId != accountId);

            await SaveAllGroupMembers(groupMembers)
            .ConfigureAwait(false);
        }

        private Task SaveAllGroupMembers(IEnumerable<GroupMember> groupMembers) => SaveAllData(GroupMembers, groupMembers);

        #endregion
    }
}

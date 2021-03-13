using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using GroupPoster.ApplicationLayer.Actions.Posting;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;
using GroupPoster.Infrastructure.BrowserAccess;
using GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors;
using ReactiveUI;

namespace UniversalPoster.ViewModels.UserControlsViewModels.OperationViewModels
{
    public class OperationViewModel : ViewModelBase
    {
        private ISystemPersistence persistence;
        private string error = string.Empty;
        private string success = string.Empty;
        private bool autoSelection;
        private bool posting;
        private GroupComboBoxModel group = null;
        private PostComboBoxModel post = null;
        private readonly SemaphoreSlim gate;

        public OperationViewModel(ISystemPersistence persistence, SemaphoreSlim semaphoreSlim)
        {
            this.persistence = persistence;
            this.gate = semaphoreSlim;
            Load();
            AutoSelection = false;
        }

        public List<AccountTableModel> Accounts { get; set; } = new();
        public List<GroupComboBoxModel> Groups { get; set; } = new();
        public List<PostComboBoxModel> Posts { get; set; } = new();

        public GroupComboBoxModel Group { get => group; set { this.RaiseAndSetIfChanged(ref group, value); } }
        public PostComboBoxModel Post { get => post; set { this.RaiseAndSetIfChanged(ref post, value); } }
        public bool AutoSelection { get => autoSelection; set { this.RaiseAndSetIfChanged(ref autoSelection, value); } }
        public bool Posting { get => posting; set { this.RaiseAndSetIfChanged(ref posting, value); } }

        public string Error { get => error; set { this.RaiseAndSetIfChanged(ref error, value); } }
        public string Success { get => success; set { this.RaiseAndSetIfChanged(ref success, value); } }

        private async void Load()
        {
            var accounts = (await new GetAccountsQueryHandler(persistence).Handle(new()))
                .Select(x => new AccountTableModel(x));

            var groups = (await new GetGroupsQueryHandler(persistence).Handle(new()))
                .Select(x => new GroupComboBoxModel(x));

            var posts = (await new GetPostsQueryHandler(persistence).Handle(new()))
                .Select(x => new PostComboBoxModel(x));

            Accounts.AddRange(accounts);
            Groups.AddRange(groups);
            Posts.AddRange(posts);
        }

        public async void StartPosting()
        {
            Posting = true;

            if (VerifyData() && VerifySelection())
            {
                PostingCommand posting = GetData();

                if (autoSelection)
                {
                    AccountTableModel account = GetRandomAccount();
                    posting.Emails.Add(account.Email);
                    posting.Passwords.Add(account.Password);
                }
                else
                {
                    (List<string> emails, List<string> passwords) = GetAccounts();
                    posting.Emails = emails;
                    posting.Passwords = passwords;
                }

                using (IFBAccess fbAccess = new FBAccess(new BrowserInteractor()))
                {
                    await new PostingCommandHandler(fbAccess).Handle(posting).ConfigureAwait(false);
                }

                Success = "Completed!";
                Error = string.Empty;
            }
            else
            {
                Success = string.Empty;
                Error = "Make Sure To Fill All Fields And Select Accounts!";
            }

            Posting = false;
        }

        private bool VerifyData()
        {
            return Post is not null || Group is not null;
        }

        private bool VerifySelection()
        {
            return autoSelection || Accounts.Any(x => x.Selected);
        }

        private (List<string> emails, List<string> passwords) GetAccounts()
        {
            List<string> emails = new();
            List<string> passwords = new();

            for (int i = 0; i < Accounts.Count; i++)
            {
                emails.Add(Accounts[i].Email);
                passwords.Add(Accounts[i].Password);
            }

            return (emails, passwords);
        }

        private AccountTableModel GetRandomAccount()
        {
            return Accounts[new Random().Next(0, Accounts.Count)];
        }

        private PostingCommand GetData()
        {
            GroupComboBoxModel group = Group;
            PostComboBoxModel post = Post;

            return new()
            {
                Content = post.Content,
                Link = group.Link
            };
        }
    }
}

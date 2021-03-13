using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors;
using GroupPoster.Infrastructure.BrowserAccess;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using GroupPoster.ApplicationLayer.Actions.Posting;
using System.Collections.ObjectModel;

namespace GroupPoster.WPFUI.Pages.ActionsManager
{
    /// <summary>
    /// Interaction logic for ActionsManagerPage.xaml
    /// </summary>
    public partial class ActionsManagerPage : Page
    {
        private readonly ISystemPersistence persistence;
        private IEnumerable<GroupComboBoxModel> GroupCollection;
        private IEnumerable<PostComboBoxModel> PostCollection;
        private ObservableCollection<AccountTableModel> AccountCollection;

        public ActionsManagerPage(ISystemPersistence persistence)
        {
            this.persistence = persistence;
            AccountCollection = new ObservableCollection<AccountTableModel>();
            InitializeComponent();
        }

        private void ButtonStates(bool status)
        {
            Post_Button.IsEnabled = status;
            //CheckGroupStatus_Button.IsEnabled = status;
        }

        private void CheckGroupStatus_Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonStates(false);



            ButtonStates(true);
        }

        private async void Post_Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonStates(false);

            if (VerifyData() && VerifySelection())
            {
                PostingCommand posting = GetData();

                if (Auto_CheckBox.IsChecked == true)
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

                using (IFBAccess fBAccess = new FBAccess(new BrowserInteractor()))
                {
                    await new PostingCommandHandler(fBAccess).Handle(posting);
                }

                MessageBox.Show("Posting Completed!", "Success", MessageBoxButton.OK);

            }
            else
            {
                MessageBox.Show("Provided Data is Invalid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            ButtonStates(true);
        }

        private (List<string> emails, List<string> password) GetAccounts()
        {
            List<string> emails = new List<string>();
            List<string> passwords = new List<string>();

            foreach (var item in AccountCollection)
            {
                if (item.Selected)
                {
                    emails.Add(item.Email);
                    passwords.Add(item.Password);
                }
            }

            return (emails, passwords);
        }

        private AccountTableModel GetRandomAccount()
        {
            return AccountCollection[new Random().Next(0, AccountCollection.Count)];
        }

        private PostingCommand GetData()
        {
            return new PostingCommand
            {
                Content = ((PostComboBoxModel)PostList_ComboBox.SelectedItem).Content,
                Link = ((GroupComboBoxModel)GroupList_ComboBox.SelectedItem).Link
            };
        }

        private bool VerifyData()
        {
            return PostList_ComboBox.SelectedIndex != -1
                && GroupList_ComboBox.SelectedIndex != -1
                && VerifySelection();
        }

        private bool VerifySelection()
        {
            return (bool)Auto_CheckBox.IsChecked? true : AccountCollection.Any(x => x.Selected);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var groupDtos = await new GetGroupsQueryHandler(persistence).Handle(new GetGroupsQuery());
            var postDtos = await new GetPostsQueryHandler(persistence).Handle(new GetPostsQuery());
            var accountsDtos = await new GetAccountsQueryHandler(persistence).Handle(new GetAccountsQuery());

            GroupCollection = groupDtos.Select(x => new GroupComboBoxModel(x));
            PostCollection = postDtos.Select(x => new PostComboBoxModel(x));

            foreach (var account in accountsDtos)
            {
                AccountCollection.Add(new AccountTableModel(account));
            }

            GroupList_ComboBox.ItemsSource = GroupCollection;
            PostList_ComboBox.ItemsSource = PostCollection;
            AccountTable.ItemsSource = AccountCollection;
        }
    }
}

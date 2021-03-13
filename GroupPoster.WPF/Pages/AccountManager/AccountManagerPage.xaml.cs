using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
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
using GroupPoster.ApplicationLayer.Accounts.Commnand.CreateAccount;
using GroupPoster.ApplicationLayer.Accounts.Commnand.DeleteAccount;
using GroupPoster.ApplicationLayer.Accounts.Commnand.UpdateAccount;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Enums;

namespace GroupPoster.WPFUI.Pages.AccountManager
{
    /// <summary>
    /// Interaction logic for AccountManagerPage.xaml
    /// </summary>
    public partial class AccountManagerPage : Page
    {
        private readonly ISystemPersistence persistence;
        private readonly SemaphoreSlim gate;

        public ObservableCollection<AccountTableModel> TableCollection { get; }

        public AccountManagerPage(ISystemPersistence persistence)
        {
            this.persistence = persistence;
            this.gate = new SemaphoreSlim(1, 1);
            this.TableCollection = new ObservableCollection<AccountTableModel>();
            this.TableCollection.CollectionChanged += TableCollection_CollectionChanged;

            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<AccountDto> accounts = await new GetAccountsQueryHandler(persistence).Handle(new GetAccountsQuery());

            foreach (AccountDto account in accounts)
            {
                AddToAccountTable(new AccountTableModel(account));
            }

            this.AccountTable.ItemsSource = TableCollection;
        }

        private async void AccountTableModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is AccountTableModel tableModel)
            {
                UpdateAccountCommand updateCommand = new()
                {
                    Id = tableModel.Id,
                    Email = tableModel.Email,
                    Password = tableModel.Password,
                    //Status = tableModel.Status.ToString()
                };

                await new UpdateAccountCommandHandler(persistence).Handle(updateCommand);
            }
        }

        private async void TableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                await gate.WaitAsync();
                await DeleteAccount(e.OldItems[0]);
                gate.Release();
            }
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                CreateAccountCommand request = GetData();
                CreateAccountCommandHandler handler = new(persistence);
                int accountId = await handler.Handle(request);
                AddToAccountTable(new AccountTableModel(accountId, request));
                ClearInput();
                return;
            }

            MessageBox.Show("Provided Data is Invalid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private bool ValidateData()
        {
            string email = Email_TextBox.Text;
            return !string.IsNullOrWhiteSpace(email) && MailAddress.TryCreate(email, out _) && !string.IsNullOrWhiteSpace(Password_TextBox.Text) /*&& Status_ComboBox.SelectedIndex != -1*/;
        }

        private CreateAccountCommand GetData() => new()
        {
            Email = Email_TextBox.Text,
            Password = Password_TextBox.Text,
            //Status = ((AccountStatus)Status_ComboBox.SelectedItem).ToString()
        };

        private void AddToAccountTable(AccountTableModel model)
        {
            model.PropertyChanged += AccountTableModel_PropertyChanged;
            TableCollection.Add(model);
        }

        private void ClearInput()
        {
            Email_TextBox.Text = string.Empty;
            Password_TextBox.Text = string.Empty;
        }
        
        private async Task DeleteAccount(object model)
        {
            if (model is AccountTableModel accountModel)
            {
                DeleteAccountCommand request = new()
                {
                    Id = accountModel.Id
                };

                await new DeleteAccountCommandHandler(persistence).Handle(request);
            }
        }
    }
}

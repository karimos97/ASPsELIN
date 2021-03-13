using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Data;
using ReactiveUI;
using GroupPoster.ApplicationLayer.Accounts.Commnand.CreateAccount;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using System.ComponentModel;
using GroupPoster.ApplicationLayer.Accounts.Commnand.UpdateAccount;
using GroupPoster.ApplicationLayer.Accounts.Commnand.DeleteAccount;
using System.Threading;
using System.Xml.Linq;

namespace UniversalPoster.ViewModels.UserControlsViewModels.AccountViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private readonly ISystemPersistence persistence;
        private readonly SemaphoreSlim gate;

        private string error = string.Empty;
        private string email = string.Empty;
        private string password = string.Empty;

        public AccountViewModel(ISystemPersistence persistence, SemaphoreSlim semaphoreSlim)
        {
            this.persistence = persistence;
            this.gate = semaphoreSlim;
            LoadData();
        }

        public ObservableCollection<AccountTableModel> Accounts { get; set; } = new();
 
        public string Email
        {
            get => email;
            set
            {
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        public string Error
        {
            get => error;
            set
            {
                this.RaiseAndSetIfChanged(ref error, value);
            }
        }

        public string Password
        {
            get => password;
            set
            {
                this.RaiseAndSetIfChanged(ref password, value);
            }
        }

        private async void LoadData()
        {
            var accounts = await new GetAccountsQueryHandler(persistence).Handle(new GetAccountsQuery());

            foreach (var account in accounts)
            {
                AddAccountToCollection(new AccountTableModel(account));
            }
        }

        private async void AccountTableModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is AccountTableModel model)
            {
                var request = new UpdateAccountCommand
                {
                    Id = model.Id,
                    Email = model.Email,
                    Password = model.Password
                };

                await new UpdateAccountCommandHandler(persistence).Handle(request);
            }
        }

        private bool ValidateData()
        {
            return !string.IsNullOrWhiteSpace(Email) && MailAddress.TryCreate(Email, out _) && !string.IsNullOrWhiteSpace(Password);
        }

        private CreateAccountCommand GetData()
        {
            return new CreateAccountCommand
            {
                Email = email,
                Password = password
            };
        }
        
        private void AddAccountToCollection(AccountTableModel account)
        {
            account.PropertyChanged += AccountTableModel_PropertyChanged;
            Accounts.Add(account);
        }

        private void ClearData()
        {
            Email = string.Empty;
            Password = string.Empty;
            Error = string.Empty;
        }

        public async void AddAccount()
        {
            if (ValidateData())
            {
                CreateAccountCommand request = GetData();

                var Id = await new CreateAccountCommandHandler(persistence).Handle(request);
                AddAccountToCollection(new AccountTableModel(Id, request));
                ClearData();
            }
            else
            {
                Error = "Invalid data!";
            }
        }

        public async void DeleteRow(int id)
        {
            var request = new DeleteAccountCommand { Id = id };

            await gate.WaitAsync();
            await new DeleteAccountCommandHandler(persistence).Handle(request);
            gate.Release();

            Accounts.Remove(Accounts.First(x => x.Id == id));
        }

    }
}

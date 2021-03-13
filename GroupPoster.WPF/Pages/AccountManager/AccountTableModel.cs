using System;
using System.ComponentModel;
using GroupPoster.ApplicationLayer.Accounts.Commnand.CreateAccount;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;

namespace GroupPoster.WPFUI.Pages.AccountManager
{


    public class AccountTableModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly int id;
        private string email;
        private string password;
        //private AccountStatus status;

        public AccountTableModel() { }

        public AccountTableModel(AccountDto account)
        {
            this.id = account.Id;
            this.email = account.Email;
            this.password = account.Password;
            //this.status = Enum.Parse<AccountStatus>(account.Status);
        }

        public AccountTableModel(int id, CreateAccountCommand command)
        {
            this.id = id;
            this.email = command.Email;
            this.password = command.Password;
            //this.status = Enum.Parse<AccountStatus>(command.Status);
        }

        public int Id => this.id;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }
        public string Password
        {
            get => password;

            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }
        //public AccountStatus Status
        //{
        //    get => status;
        //    set
        //    {
        //        status = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        //    }
        //}
    }
}


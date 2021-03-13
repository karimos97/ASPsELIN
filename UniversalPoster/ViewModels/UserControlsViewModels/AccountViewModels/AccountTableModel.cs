using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Accounts.Commnand.CreateAccount;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;

namespace UniversalPoster.ViewModels.UserControlsViewModels.AccountViewModels
{
    public class AccountTableModel : INotifyPropertyChanged
    {
        private string? email;
        private string? password;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AccountTableModel(AccountDto account)
        {
            Id = account.Id;
            this.email = account.Email;
            this.password = account.Password;
        }

        public AccountTableModel(int id, CreateAccountCommand request)
        {
            Id = id;
            this.email = request.Email;
            this.password = request.Password;
        }

        public int Id { get; set; }
        public string? Email
        {
            get => email;
            set
            {
                email = value;
                InvokeChange();
            }
        }

        public string? Password
        {
            get => password;
            set
            {
                password = value;
                InvokeChange();
            }
        }

        private void InvokeChange([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}

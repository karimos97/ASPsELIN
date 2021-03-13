using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;

namespace UniversalPoster.ViewModels.UserControlsViewModels.OperationViewModels
{
    public class AccountTableModel
    {
        public AccountTableModel(AccountDto x)
        {
            Email = x.Email;
            Password = x.Password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool Selected { get; set; }
    }
}
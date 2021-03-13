using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;

namespace GroupPoster.WPFUI.Pages.ActionsManager
{
    public class AccountTableModel
    {
        public AccountTableModel(AccountDto x)
        {
            this.Id = x.Id;
            this.Email = x.Email;
            this.Password = x.Password;
        }

        public int Id { get; }
        public string Email { get; }
        public string Password { get; }
        public bool Selected { get; set; }
    }
}

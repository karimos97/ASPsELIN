using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UniversalPoster.Views.UserControlsViews.AccountViews
{
    public class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

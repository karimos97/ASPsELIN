using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UniversalPoster.Views.UserControlsViews.GroupViews
{
    public class GroupView : UserControl
    {
        public GroupView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

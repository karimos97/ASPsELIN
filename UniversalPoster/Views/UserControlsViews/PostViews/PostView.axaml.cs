using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UniversalPoster.Views.UserControlsViews.PostViews
{
    public class PostView : UserControl
    {
        public PostView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

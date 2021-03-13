using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UniversalPoster.Views.UserControlsViews.OperationViews
{
    public class OperationView : UserControl
    {
        public OperationView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

using System.Windows;
using System.Windows.Input;

namespace Gizmo.WPF
{
    public partial class DialogWindowStyle : ResourceDictionary
    {
        public DialogWindowStyle()
        {
            this.InitializeComponent();
        }

        void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                sender.ForWindowFromTemplate(w => SystemCommands.CloseWindow(w));
        }

        void IconMouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            var point = element.PointToScreen(new Point(element.ActualWidth / 2, element.ActualHeight));
            sender.ForWindowFromTemplate(w => SystemCommands.ShowSystemMenu(w, point));
        }

        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => SystemCommands.CloseWindow(w));
        }
    }
}

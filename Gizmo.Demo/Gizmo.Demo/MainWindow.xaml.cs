using Gizmo.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AppViewModel appvm;
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, UIThemeEnum.BlueDark);
            appvm = new AppViewModel();
            DataContext = appvm;
        }

        private void UIEnumSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                if ((sender as UIEnumSwitch).SelectedItem != null)
                    ThemeManager.ApplyThemeToWindow(this, (UIThemeEnum)(sender as UIEnumSwitch).SelectedItem);
            }
        }
    }
}

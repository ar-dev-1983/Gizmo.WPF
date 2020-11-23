using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gizmo.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UIEnumSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                if ((sender as UIEnumSwitch).SelectedItem != null)
                    ThemeManager.ApplyTheme((UIThemeEnum)(sender as UIEnumSwitch).SelectedItem);
            }
        }
    }
}

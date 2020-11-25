using Gizmo.WPF;
using System.Windows.Controls;

namespace Gizmo.Demo
{
    /// <summary>
    /// Логика взаимодействия для GizmoControls.xaml
    /// </summary>
    public partial class GizmoControls : UserControl
    {
        public GizmoControls()
        {
            InitializeComponent();
        }

        private void UISearchBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                if ((sender as UISearchBox).SelectedItem != null)
                {
                    SearchValues.SelectedItem=(sender as UISearchBox).SelectedItem as SearchValue;
                }
            }
        }
    }
}

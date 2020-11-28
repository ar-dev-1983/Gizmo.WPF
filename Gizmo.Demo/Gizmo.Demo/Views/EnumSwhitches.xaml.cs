using System.Windows.Controls;

namespace Gizmo.Demo
{
    /// <summary>
    /// Логика взаимодействия для EnumSwhitches.xaml
    /// </summary>
    public partial class EnumSwhitches : UserControl
    {
        public EnumSwhitches()
        {
            InitializeComponent();
        }

        private void UIButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TestEnumEwitch3.SeparateItems = !TestEnumEwitch3.SeparateItems;
        }
    }
}

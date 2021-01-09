using System.Windows.Controls;

namespace Gizmo.Demo
{
    /// <summary>
    /// Логика взаимодействия для TabPanels.xaml
    /// </summary>
    public partial class TabPanels : UserControl
    {
        int counter = 0;
        public TabPanels()
        {
            InitializeComponent();
        }

        private void TabPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            counter++;
            tbCounter.Text = counter.ToString();
        }
    }
}

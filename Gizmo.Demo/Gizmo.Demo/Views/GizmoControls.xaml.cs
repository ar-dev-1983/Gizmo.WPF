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
    public enum TestEnum
    {
        If=0,
        You=1,
        Happy=2,
        And=3,
        you=4,
        Know=5,
        It=6,
        Clapp=7,
        Your=8,
        Hands=9
    }
    /// <summary>
    /// Логика взаимодействия для GizmoControls.xaml
    /// </summary>
    public partial class GizmoControls : Page
    {
        public GizmoControls()
        {
            InitializeComponent();
        }
    }
}

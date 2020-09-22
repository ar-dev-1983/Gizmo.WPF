using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UITreeNodeExpanderControl : Control
    {
        static UITreeNodeExpanderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UITreeNodeExpanderControl), new FrameworkPropertyMetadata(typeof(UITreeNodeExpanderControl)));
        }
    }
}

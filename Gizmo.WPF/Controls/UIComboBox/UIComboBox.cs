using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UIComboBox : ComboBox, ICorneredControl
    {
        public UIComboBox()
            : base()
        {
            DefaultStyleKey = typeof(UIComboBox);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIComboBox), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIComboBox), new FrameworkPropertyMetadata(true));
    }
}

using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UIComboBoxItem : ComboBoxItem, ICorneredControl
    {
        public UIComboBoxItem()
            : base()
        {
            DefaultStyleKey = typeof(UIComboBoxItem);
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
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIComboBoxItem), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIComboBoxItem), new FrameworkPropertyMetadata(true));
    }
}

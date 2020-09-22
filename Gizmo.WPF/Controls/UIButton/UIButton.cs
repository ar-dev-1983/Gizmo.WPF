using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UIButton : Button, ICorneredControl
    {
        static UIButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIButton), new FrameworkPropertyMetadata(typeof(UIButton)));
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
        public object Icon
        {
            get => (object)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIButton), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIButton), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UIButton), new FrameworkPropertyMetadata(null));
    }
}

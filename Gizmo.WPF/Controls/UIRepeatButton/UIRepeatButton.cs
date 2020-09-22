using System.Windows;
using System.Windows.Controls.Primitives;

namespace Gizmo.WPF
{
    public class UIRepeatButton : RepeatButton, ICorneredControl
    {
        static UIRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIRepeatButton), new FrameworkPropertyMetadata(typeof(UIRepeatButton)));
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
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIRepeatButton), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIRepeatButton), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UIRepeatButton), new FrameworkPropertyMetadata(null));
    }
}

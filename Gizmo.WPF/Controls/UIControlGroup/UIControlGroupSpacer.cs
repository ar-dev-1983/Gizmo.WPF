using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UIControlGroupSpacer : ContentControl, ICorneredControl
    {
        static UIControlGroupSpacer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIControlGroupSpacer), new FrameworkPropertyMetadata(typeof(UIControlGroupSpacer)));
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIControlGroupSpacer), new UIPropertyMetadata(new CornerRadius(3)));

    }
}

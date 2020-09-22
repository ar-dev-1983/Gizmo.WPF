using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gizmo.WPF
{
    public class UISwitch : ContentControl
    {
        public static RoutedEvent CheckedEvent;
        public static RoutedEvent UncheckedEvent;
        public event RoutedEventHandler CheckedClick
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }
        public event RoutedEventHandler UncheckedClick
        {
            add { AddHandler(UncheckedEvent, value); }
            remove { RemoveHandler(UncheckedEvent, value); }
        }

        public UISwitch()
: base()
        {
            DefaultStyleKey = typeof(UISwitch);
        }
        static UISwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISwitch), new FrameworkPropertyMetadata(typeof(UISwitch)));
            CheckedEvent = EventManager.RegisterRoutedEvent("CheckedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISwitch));
            UncheckedEvent = EventManager.RegisterRoutedEvent("UncheckedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISwitch));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public UISwitchHeaderPlacement HeaderPlacement
        {
            get => (UISwitchHeaderPlacement)GetValue(HeaderPlacementProperty);
            set => SetValue(HeaderPlacementProperty, value);
        }
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UISwitch), new UIPropertyMetadata(new CornerRadius(10)));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(UISwitch), new UIPropertyMetadata(null));
        public static readonly DependencyProperty HeaderPlacementProperty = DependencyProperty.Register("HeaderPlacement", typeof(UISwitchHeaderPlacement), typeof(UISwitch), new UIPropertyMetadata(UISwitchHeaderPlacement.Left));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(UISwitch), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(UISwitch), new UIPropertyMetadata(false, new PropertyChangedCallback(IsCheckedChanged)));

        private static void IsCheckedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UISwitch sw = (UISwitch)o;
                sw.ProcessIsChecked();
            }
        }

        internal void ProcessIsChecked()
        {
            if (IsChecked)
            {
                RoutedEventArgs args = new RoutedEventArgs();
                args.RoutedEvent = CheckedEvent;
                RaiseEvent(args);
            }
            else
            {
                RoutedEventArgs args = new RoutedEventArgs();
                args.RoutedEvent = UncheckedEvent;
                RaiseEvent(args);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (!IsReadOnly)
                IsChecked = !IsChecked;
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gizmo.WPF
{
    public class UIEnumSwitchItem : Control, ICorneredControl
    {
        public static RoutedEvent ItemSelectedEvent;
        public static RoutedEvent ItemUnSelectedEvent;
        public event RoutedEventHandler ItemSelectedClick
        {
            add { AddHandler(ItemSelectedEvent, value); }
            remove { RemoveHandler(ItemSelectedEvent, value); }
        }
        public event RoutedEventHandler ItemUnSelectedClick
        {
            add { AddHandler(ItemUnSelectedEvent, value); }
            remove { RemoveHandler(ItemUnSelectedEvent, value); }
        }
        public UIEnumSwitchItem()
        : base()
        {
            DefaultStyleKey = typeof(UIEnumSwitchItem);
        }
        static UIEnumSwitchItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIEnumSwitchItem), new FrameworkPropertyMetadata(typeof(UIEnumSwitchItem)));
            ItemSelectedEvent = EventManager.RegisterRoutedEvent("ItemSelectedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIEnumSwitchItem));
            ItemUnSelectedEvent = EventManager.RegisterRoutedEvent("ItemUnSelectedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIEnumSwitchItem));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        public object Value
        {
            get => (object)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public object Icon
        {
            get => (object)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(UIEnumSwitchItem), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(UIEnumSwitchItem), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UIEnumSwitchItem), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(UIEnumSwitchItem), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIEnumSwitchItem), new UIPropertyMetadata(new CornerRadius(10)));

        private static void IsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UIEnumSwitchItem item = (UIEnumSwitchItem)o;
            item.CheckIsSelected();
        }

        internal void CheckIsSelected()
        {
            if (IsSelected)
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = ItemSelectedEvent
                };
                RaiseEvent(args);
            }
            else
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = ItemUnSelectedEvent
                };
                RaiseEvent(args);
            }
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            IsSelected = true;
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.WPF
{
    public class UITabPanelItem : ContentControl
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
        public UITabPanelItem()
        : base()
        {
            DefaultStyleKey = typeof(UITabPanelItem);
        }
        static UITabPanelItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UITabPanelItem), new FrameworkPropertyMetadata(typeof(UITabPanelItem)));
            ItemSelectedEvent = EventManager.RegisterRoutedEvent("ItemSelectedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanelItem));
            ItemUnSelectedEvent = EventManager.RegisterRoutedEvent("ItemUnSelectedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanelItem));
        }
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            return parentObject == null ? (T)null : parentObject is T parent ? parent : FindParent<T>(parentObject);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (FindParent<UITabPanel>(this) is UITabPanel tabPanel)
            {
                var Binding = new Binding() { Path = new PropertyPath("Orientation"), Source = tabPanel, Mode = BindingMode.TwoWay };
                SetBinding(UITabPanelItem.OrientationProperty, Binding);
            }
        }
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        public UITabPanelItemOrientation Orientation
        {
            get => (UITabPanelItemOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(UITabPanelItem), new UIPropertyMetadata(null));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(UITabPanelItem), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(UITabPanelItemOrientation), typeof(UITabPanelItem), new FrameworkPropertyMetadata(UITabPanelItemOrientation.Left));

        private static void IsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanelItem tabPanelItem = (UITabPanelItem)o;
            tabPanelItem.CheckIsSelected();
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

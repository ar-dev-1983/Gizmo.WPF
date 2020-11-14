using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gizmo.WPF
{
    public class UISearchBoxItem : Control, ICorneredControl
    {
        public static RoutedEvent ItemSelectedEvent;
        public event RoutedEventHandler ItemSelectedClick
        {
            add { AddHandler(ItemSelectedEvent, value); }
            remove { RemoveHandler(ItemSelectedEvent, value); }
        }
        public UISearchBoxItem()
: base()
        {
            DefaultStyleKey = typeof(UISearchBoxItem);
        }
        static UISearchBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISearchBoxItem), new FrameworkPropertyMetadata(typeof(UISearchBoxItem)));
            ItemSelectedEvent = EventManager.RegisterRoutedEvent("ItemSelectedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBoxItem));
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
        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(UISearchBoxItem), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(UISearchBoxItem), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UISearchBoxItem), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(UISearchBoxItem), new UIPropertyMetadata(null));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(object), typeof(UISearchBoxItem), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UISearchBoxItem), new UIPropertyMetadata(new CornerRadius(0)));

        private static void IsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UISearchBoxItem item = (UISearchBoxItem)o;
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
                IsSelected = false;
            }
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            IsSelected = true;
        }
    }
}

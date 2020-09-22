using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UITabPanel : ContentControl, ICorneredControl
    {
        public static RoutedEvent ExpandedEvent;
        public event RoutedEventHandler Expanded
        {
            add { AddHandler(ExpandedEvent, value); }
            remove { RemoveHandler(ExpandedEvent, value); }
        }
        public static RoutedEvent SelectionChangedEvent;
        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        public static RoutedEvent PressedEvent;
        public static RoutedEvent UnpressedEvent;
        public event RoutedEventHandler PressedClick
        {
            add { AddHandler(PressedEvent, value); }
            remove { RemoveHandler(PressedEvent, value); }
        }
        public event RoutedEventHandler UnpressedClick
        {
            add { AddHandler(UnpressedEvent, value); }
            remove { RemoveHandler(UnpressedEvent, value); }
        }

        public UITabPanel()
        : base()
        {
            Items = new ObservableCollection<UITabPanelItem>();
            DefaultStyleKey = typeof(UITabPanel);
        }
        static UITabPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UITabPanel), new FrameworkPropertyMetadata(typeof(UITabPanel)));
            ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanel));
            SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanel));
            PressedEvent = EventManager.RegisterRoutedEvent("PressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanel));
            UnpressedEvent = EventManager.RegisterRoutedEvent("UnpressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanel));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            BindEventsItems();
            if (SelectedIndex != -1)
                CheckSelectedIndex(-1, SelectedIndex);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }
        public UITabPanelItemOrientation Orientation
        {
            get => (UITabPanelItemOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public ObservableCollection<UITabPanelItem> Items
        {
            get => (ObservableCollection<UITabPanelItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UITabPanel), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(UITabPanel), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsExpandedPropertyChanged)));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(UITabPanelItemOrientation), typeof(UITabPanel), new FrameworkPropertyMetadata(UITabPanelItemOrientation.Left));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<UITabPanelItem>), typeof(UITabPanel), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemsCollectionChanged)));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(UITabPanel), new UIPropertyMetadata(null));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(UITabPanel), new UIPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));
        private static void IsExpandedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanel tabPanel = (UITabPanel)o;
            tabPanel.RiseExpandedEvent();
        }
        public static void OnSelectedIndexPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanel tabPanel = (UITabPanel)o;
            tabPanel.CheckSelectedIndex((int)e.OldValue, (int)e.NewValue);
        }

        internal void RiseExpandedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = ExpandedEvent;
            RaiseEvent(args);
        }

        private static void OnItemsCollectionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanel tabPanel = (UITabPanel)o;
            tabPanel.BindEventsItems();
        }

        internal void BindEventsItems()
        {
            if (Items != null)
            {
                foreach (var node in Items)
                {

                    node.ItemSelectedClick -= Node_ItemSelected;
                    node.ItemSelectedClick += Node_ItemSelected;
                    node.ItemUnSelectedClick -= Node_ItemUnSelectedClick;
                    node.ItemUnSelectedClick += Node_ItemUnSelectedClick;
                }
            }
        }

        internal void CheckSelectedIndex(int oldValue, int newValue)
        {
            if (Items != null)
            {
                if (Items.Count != 0)
                {
                    if (oldValue != newValue)
                    {
                        if (newValue != -1)
                        {
                            SelectedItem = Items[newValue];
                            Items[newValue].IsSelected = true;

                            RoutedEventArgs args = new RoutedEventArgs
                            {
                                RoutedEvent = SelectionChangedEvent
                            };
                            RaiseEvent(args);
                        }
                    }
                    else
                    {
                        if (!Items[newValue].IsSelected)
                        {
                            Items[newValue].IsSelected = true;

                            foreach (var node in Items)
                            {
                                if (Items.IndexOf(node) != newValue)
                                    node.IsSelected = false;
                            }
                        }
                    }
                }
                else
                {
                    SelectedItem = null;
                }
            }
        }

        private void Node_ItemUnSelectedClick(object sender, RoutedEventArgs e)
        {
            var selectedIndex = SelectedIndex;
            if (selectedIndex == Items.IndexOf((sender as UITabPanelItem)))
            {
                SelectedIndex = -1;
                IsExpanded = false;
            }
        }

        private void Node_ItemSelected(object sender, RoutedEventArgs e)
        {
            SelectedIndex = Items.IndexOf((sender as UITabPanelItem));
            foreach (var node in Items)
            {
                if (Items.IndexOf(node) != SelectedIndex)
                    node.IsSelected = false;
            }
        }
    }
}

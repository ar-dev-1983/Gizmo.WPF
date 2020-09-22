using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UIEnumSwitch : Control, ICorneredControl
    {
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

        public UIEnumSwitch()
        : base()
        {
            Items = new ObservableCollection<UIEnumSwitchItem>();
            DefaultStyleKey = typeof(UIEnumSwitch);
        }
        static UIEnumSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIEnumSwitch), new FrameworkPropertyMetadata(typeof(UIEnumSwitch)));
            SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIEnumSwitch));
            PressedEvent = EventManager.RegisterRoutedEvent("PressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIEnumSwitch));
            UnpressedEvent = EventManager.RegisterRoutedEvent("UnpressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIEnumSwitch));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            BindEventsItems();
            if (SelectedValue != null)
                CheckSelectedValue(null, SelectedValue);

            if (SelectedIndex != -1)
                CheckSelectedIndex(-1, SelectedIndex);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public ObservableCollection<UIEnumSwitchItem> Items
        {
            get => (ObservableCollection<UIEnumSwitchItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public UIEnumSwitchItem SelectedItem
        {
            get => (UIEnumSwitchItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        public object SelectedValue
        {
            get => (object)GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIEnumSwitch), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<UIEnumSwitchItem>), typeof(UIEnumSwitch), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemsCollectionChanged)));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(UIEnumSwitchItem), typeof(UIEnumSwitch), new UIPropertyMetadata(null));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(UIEnumSwitch), new UIPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(UIEnumSwitch), new UIPropertyMetadata(null, new PropertyChangedCallback(OnSelectedValuePropertyChanged)));


        private static void OnItemsCollectionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UIEnumSwitch cs = (UIEnumSwitch)o;
            cs.BindEventsItems();
        }

        public static void OnSelectedIndexPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UIEnumSwitch cs = (UIEnumSwitch)o;
            cs.CheckSelectedIndex((int)e.OldValue, (int)e.NewValue);
        }

        public static void OnSelectedValuePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UIEnumSwitch cs = (UIEnumSwitch)o;
            cs.CheckSelectedValue(e.OldValue, e.NewValue);
        }

        internal void BindEventsItems()
        {
            if (Items != null)
            {
                CornerRadius r = CornerRadius;
                foreach (var node in Items)
                {
                    node.ItemSelectedClick -= Node_ItemSelected;
                    node.ItemSelectedClick += Node_ItemSelected;
                    node.ItemUnSelectedClick -= Node_ItemUnSelectedClick;
                    node.ItemUnSelectedClick += Node_ItemUnSelectedClick;
                    if (node is ICorneredControl)
                    {
                        if (Items.Count == 1)
                        {
                            (node as ICorneredControl).CornerRadius = CornerRadius;
                            (node as ICorneredControl).BorderThickness = new Thickness(1);
                        }
                        else if (Items.IndexOf(node) == 0)
                        {
                            (node as ICorneredControl).CornerRadius = new CornerRadius(r.TopLeft, 0d, 0d, r.BottomLeft);
                            (node as ICorneredControl).BorderThickness = new Thickness(1, 1, 0, 1);
                        }
                        else if (Items.IndexOf(node) == Items.Count - 1)
                        {
                            (node as ICorneredControl).CornerRadius = new CornerRadius(0d, r.TopRight, r.BottomRight, 0d);
                            (node as ICorneredControl).BorderThickness = new Thickness(0, 1, 1, 1);
                        }
                        else
                        {
                            (node as ICorneredControl).CornerRadius = new CornerRadius(0);
                            (node as ICorneredControl).BorderThickness = new Thickness(0, 1, 0, 1);
                        }
                    }
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
                        SelectedItem = Items[newValue];
                        SelectedValue = SelectedItem.Value;
                        Items[SelectedIndex].IsSelected = true;

                        RoutedEventArgs args = new RoutedEventArgs
                        {
                            RoutedEvent = SelectionChangedEvent
                        };
                        RaiseEvent(args);
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

        internal void CheckSelectedValue(object oldValue, object newValue)
        {
            if (Items != null)
            {
                if (Items.Count != 0)
                {
                    if (oldValue != newValue && SelectedIndex == -1)
                    {
                        var selectedItem = Items.Where(x => object.Equals(x.Value, newValue)).First();
                        SelectedIndex = Items.IndexOf(selectedItem);
                        Items[SelectedIndex].IsSelected = true;

                        foreach (var node in Items)
                        {
                            if (Items.IndexOf(node) != SelectedIndex)
                                node.IsSelected = false;
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
            if (selectedIndex == Items.IndexOf((sender as UIEnumSwitchItem)))
            {
                SelectedIndex = -1;
            }
        }
        private void Node_ItemSelected(object sender, RoutedEventArgs e)
        {
            SelectedIndex = Items.IndexOf((sender as UIEnumSwitchItem));
            foreach (var node in Items)
            {
                if (Items.IndexOf(node) != SelectedIndex)
                    node.IsSelected = false;
            }
        }
    }
}

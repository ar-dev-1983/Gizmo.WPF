using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    [TemplatePart(Name = partSearchInput)]
    [TemplatePart(Name = partPopup)]
    public class UISearchBox : ItemsControl, ICorneredControl
    {
        #region Internal Properties
        const string partSearchInput = "PART_SearchInput";
        protected UITextBox searchTextBox;
        internal UITextBox SearchTextBox => searchTextBox;
        private Popup ItemsPopup;
        const string partPopup = "PART_Popup";
        #endregion

        #region Routed Events
        public static RoutedEvent SearchTextChangedEvent;
        public event RoutedEventHandler SearchTextChanged
        {
            add { AddHandler(SearchTextChangedEvent, value); }
            remove { RemoveHandler(SearchTextChangedEvent, value); }
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

        public static RoutedEvent SelectionChangedEvent;
        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        } 
        #endregion

        public UISearchBox()
: base()
        {
            Items = new ObservableCollection<UISearchBoxItem>();
            DefaultStyleKey = typeof(UISearchBox);
        }
        static UISearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISearchBox), new FrameworkPropertyMetadata(typeof(UISearchBox)));
            SearchTextChangedEvent = EventManager.RegisterRoutedEvent("SearchTextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            PressedEvent = EventManager.RegisterRoutedEvent("PressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            UnpressedEvent = EventManager.RegisterRoutedEvent("UnpressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (searchTextBox != null)
            {
                searchTextBox.TextChanged += SearchTextBox_TextChanged;
            }
            searchTextBox = GetTemplateChild(partSearchInput) as UITextBox;
            if (searchTextBox != null)
            {
                searchTextBox.TextChanged += SearchTextBox_TextChanged;
            }

            ItemsPopup = GetTemplateChild(partPopup) as Popup;
            BindEventsItems();

            if (SelectedValue != null)
                CheckSelectedValue(null, SelectedValue);

            if (SelectedIndex != -1)
                CheckSelectedIndex(-1, SelectedIndex);
        }

        #region Event Handlers
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                if (IsPressed)
                {
                    SearchText = (sender as UITextBox).Text;
                    RoutedEventArgs args = new RoutedEventArgs
                    {
                        RoutedEvent = SearchTextChangedEvent
                    };
                    RaiseEvent(args);
                }
            }
        }
        #endregion

        #region Public Properties
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public new ObservableCollection<UISearchBoxItem> Items
        {
            get => (ObservableCollection<UISearchBoxItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public UISearchBoxItem SelectedItem
        {
            get => (UISearchBoxItem)GetValue(SelectedItemProperty);
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

        public bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedProperty, value);
        }
        public bool HoldSearchTextAfterHide
        {
            get => (bool)GetValue(HoldSearchTextAfterHideProperty);
            set => SetValue(HoldSearchTextAfterHideProperty, value);
        }
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }
        public SearchPlacementEnum Orientation
        {
            get => (SearchPlacementEnum)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UISearchBox), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<UISearchBoxItem>), typeof(UISearchBox), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemsCollectionChanged)));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(UISearchBoxItem), typeof(UISearchBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(UISearchBox), new UIPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(UISearchBox), new UIPropertyMetadata(null, new PropertyChangedCallback(OnSelectedValuePropertyChanged)));
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(false, new PropertyChangedCallback(IsPressedChanged)));
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(UISearchBox), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty HoldSearchTextAfterHideProperty = DependencyProperty.Register("HoldSearchTextAfterHide", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(true));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(SearchPlacementEnum), typeof(UISearchBox), new UIPropertyMetadata(SearchPlacementEnum.Left));
        #endregion

        private static void IsPressedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UISearchBox cs = (UISearchBox)o;
                cs.ProcessIsPressed();
            }
        }

        internal void ProcessIsPressed()
        {
            if (IsPressed)
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = PressedEvent
                };
                RaiseEvent(args);
            }
            else
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = UnpressedEvent
                };
                RaiseEvent(args);
                if (!HoldSearchTextAfterHide)
                {
                    SearchTextBox.Text = string.Empty;
                    SearchText = string.Empty;
                }
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            IsPressed = !IsPressed;
        }

        private static void OnItemsCollectionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UISearchBox cs = (UISearchBox)o;
            cs.BindEventsItems();
        }

        public static void OnSelectedIndexPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UISearchBox cs = (UISearchBox)o;
            cs.CheckSelectedIndex((int)e.OldValue, (int)e.NewValue);
        }

        public static void OnSelectedValuePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UISearchBox cs = (UISearchBox)o;
            cs.CheckSelectedValue(e.OldValue, e.NewValue);
        }

        internal void BindEventsItems()
        {
            if (Items != null)
            {
                foreach (var node in Items)
                {
                    node.ItemSelectedClick -= Node_ItemSelected;
                    node.ItemSelectedClick += Node_ItemSelected;
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
            if (selectedIndex == Items.IndexOf((sender as UISearchBoxItem)))
            {
                SelectedIndex = -1;
            }
        }
        private void Node_ItemSelected(object sender, RoutedEventArgs e)
        {
            SelectedIndex = Items.IndexOf((sender as UISearchBoxItem));
            foreach (var node in Items)
            {
                if (Items.IndexOf(node) != SelectedIndex)
                    node.IsSelected = false;
            }
        }

    }
}
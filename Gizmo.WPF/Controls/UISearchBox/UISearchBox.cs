using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Gizmo.WPF
{
    [TemplatePart(Name = partSearchInput)]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UISearchBoxItem))]
    public class UISearchBox : Selector, ICorneredControl
    {
        #region Internal Properties
        const string partSearchInput = "PART_SearchInput";

        protected UITextBox searchTextBox;

        internal UITextBox SearchTextBox => searchTextBox;

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
        #endregion

        #region Constructors
        public UISearchBox()
: base()
        {
            DefaultStyleKey = typeof(UISearchBox);
        }

        static UISearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISearchBox), new FrameworkPropertyMetadata(typeof(UISearchBox)));
            SearchTextChangedEvent = EventManager.RegisterRoutedEvent("SearchTextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            PressedEvent = EventManager.RegisterRoutedEvent("PressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            UnpressedEvent = EventManager.RegisterRoutedEvent("UnpressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
            template.Seal();
            ItemsPanelProperty.OverrideMetadata(typeof(UISearchBox), new FrameworkPropertyMetadata(template));
        }
        #endregion

        #region Override Methods
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UISearchBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UISearchBoxItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (searchTextBox != null)
            {
                searchTextBox.TextChanged -= SearchTextBox_TextChanged;
                searchTextBox.GotFocus -= SearchTextBox_GotFocus;
                searchTextBox.LostFocus -= SearchTextBox_LostFocus;
            }
            searchTextBox = GetTemplateChild(partSearchInput) as UITextBox;
            if (searchTextBox != null)
            {
                searchTextBox.TextChanged += SearchTextBox_TextChanged;
                searchTextBox.GotFocus += SearchTextBox_GotFocus;
                searchTextBox.LostFocus += SearchTextBox_LostFocus;
            }
        }
        private void AttachItems()
        {
            foreach (object item in Items)
            {
                UISearchBoxItem container = item as UISearchBoxItem;
                if (container == null)
                {
                    UpdateLayout();
                    container = ItemContainerGenerator.ContainerFromItem(item) as UISearchBoxItem;
                }
                if (container != null)
                {
                    UISearchBoxItem searchBoxItem = new UISearchBoxItem();
                    Binding b = new Binding("IsSelected")
                    {
                        Mode = BindingMode.TwoWay,
                        Source = container
                    };
                    searchBoxItem.SetBinding(UISearchBoxItem.IsSelectedProperty, b);
                    PrepareContainerForItemOverride(searchBoxItem, item);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                if (Items != null)
                {
                    if (!Collapsible && Items.Count != 0 && IsPressed)
                    {
                        IsPressed = false;
                    }
                }
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!Collapsible)
            {
                if (!IsPressed)
                    IsPressed = true;
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                if (Collapsible)
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
                else
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

        public bool ShowOptions
        {
            get => (bool)GetValue(ShowOptionsProperty);
            set => SetValue(ShowOptionsProperty, value);
        }

        public object OptionsContent
        {
            get => (bool)GetValue(OptionsContentProperty);
            set => SetValue(OptionsContentProperty, value);
        }
        public bool Collapsible
        {
            get => (bool)GetValue(CollapsibleProperty);
            set => SetValue(CollapsibleProperty, value);
        }
        public PlacementMode PopupPlacement
        {
            get => (PlacementMode)GetValue(PopupPlacementProperty);
            set => SetValue(PopupPlacementProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UISearchBox), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(false, new PropertyChangedCallback(IsPressedChanged)));
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(UISearchBox), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty HoldSearchTextAfterHideProperty = DependencyProperty.Register("HoldSearchTextAfterHide", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(true));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(SearchPlacementEnum), typeof(UISearchBox), new UIPropertyMetadata(SearchPlacementEnum.Left));
        public static readonly DependencyProperty ShowOptionsProperty = DependencyProperty.Register("ShowOptions", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty OptionsContentProperty = DependencyProperty.Register("OptionsContent", typeof(object), typeof(UISearchBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CollapsibleProperty = DependencyProperty.Register("Collapsible", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(true));
        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(UISearchBox), new UIPropertyMetadata(PlacementMode.Bottom));
        #endregion

        #region Property Callbacks
        private static void IsPressedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UISearchBox searchBox = o as UISearchBox;
                searchBox.ProcessIsPressed();
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
                if (!Collapsible)
                {
                    //then the Collapsible is false we must get rid of the logical and keyboard focus set in searchTextBox
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(searchTextBox), null);
                    Keyboard.ClearFocus();
                }
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
        #endregion    
    }
}

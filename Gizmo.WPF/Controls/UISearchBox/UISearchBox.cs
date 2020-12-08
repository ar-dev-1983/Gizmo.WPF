using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    /// <summary>
    /// UISearchBox - это элемент управления для организации стандартизированного интерфейса для пользователя в части поиска по спискам в приложении.
    /// UISearchBox сам по себе не выполняет поиск, а только передает строку для поиска в логике приложения, и выводит результаты поиска, передаваемые ему извне, 
    /// для отображения пользователю и выбора определенного результата.
    /// </summary>
    /// <remarks>
    /// UISearchBox is a control for organizing a standardized user interface for searching through lists in an application.
    /// UISearchBox itself does not perform a search, it only passes the search string in the application logic,
    /// and outputs the search results passed to it externally for display to the user and selecting a specific result.
    /// </remarks>
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
        /// <summary>
        /// Событие представляющее изменение строки поиска в UISearchBox
        /// </summary>
        /// <remarks>
        /// Routed Event representing search string is changed in UISearchBox
        /// </remarks>
        public static RoutedEvent SearchTextChangedEvent;

        /// <summary>
        /// Обработчик события представляющего изменение строки поиска в UISearchBox
        /// </summary>
        /// <remarks>
        /// Routed event handler represents Unselection Event for UISearchBox
        /// </remarks>
        public event RoutedEventHandler SearchTextChanged
        {
            add { AddHandler(SearchTextChangedEvent, value); }
            remove { RemoveHandler(SearchTextChangedEvent, value); }
        }

        public static RoutedEvent PressedEvent;

        public event RoutedEventHandler PressedClick
        {
            add { AddHandler(PressedEvent, value); }
            remove { RemoveHandler(PressedEvent, value); }
        }

        public static RoutedEvent UnpressedEvent;

        public event RoutedEventHandler UnpressedClick
        {
            add { AddHandler(UnpressedEvent, value); }
            remove { RemoveHandler(UnpressedEvent, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UISearchBox()
: base()
        {
            DefaultStyleKey = typeof(UISearchBox);
        }

        static UISearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISearchBox), new FrameworkPropertyMetadata(typeof(UISearchBox)));
            SearchTextChangedEvent = EventManager.RegisterRoutedEvent("SearchTextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UISearchBox));
            PressedEvent = EventManager.RegisterRoutedEvent("PressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof (UISearchBox));
            UnpressedEvent = EventManager.RegisterRoutedEvent("UnpressedClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof (UISearchBox));
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
            template.Seal();
            ItemsPanelProperty.OverrideMetadata(typeof(UISearchBox), new FrameworkPropertyMetadata(template));
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Возвращает true если элемент является своим собственным контейнером
        /// </summary>
        /// <remarks>
        /// Return true if the item is its own ItemContainer
        /// </remarks>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UISearchBoxItem;
        }
        /// <summary>
        /// Создает контейнер для элемента
        /// </summary>
        /// <remarks>
        /// Create or identify the element used to display the given item.
        /// </remarks>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UISearchBoxItem();
        }

        /// <summary>
        /// Задает свойства контейнера элемента для отображения
        /// </summary>
        /// <remarks>
        /// Prepare the element to display the item
        /// </remarks>
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
        #endregion

        #region Event Handlers
        /// <summary>
        /// Функция отслеживает фокус у SearchTextBox. Если фокус потерян - закрываем Popup с результатами поиска.
        /// </summary>
        /// <remarks>
        /// The function tracks of focus in SearchTextBox. If focus is lost - we hide Popup with search results.
        /// </remarks>
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
        /// <summary>
        /// Функция отслеживает фокус у SearchTextBox. Если фокус получен - показываем Popup с результатами поиска.
        /// </summary>
        /// <remarks>
        /// The function tracks focus in SearchTextBox. If focus is got - we show Popup with search results.
        /// </remarks>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!Collapsible)
            {
                if (!IsPressed)
                    IsPressed = true;
            }
        }

        /// <summary>
        /// Функция генерирует событие SearchTextChangedEvent при изменении текста в SearchTextBox.
        /// </summary>
        /// <remarks>
        /// The function generates the SearchTextChangedEvent when the text in the SearchTextBox changesю.
        /// </remarks>
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
        public object Watermark
        {
            get => (object)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        public DataTemplate WatermarkDataTemplate
        {
            get => (DataTemplate)GetValue(WatermarkDataTemplateProperty);
            set => SetValue(WatermarkDataTemplateProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UISearchBox), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof (UISearchBox), new UIPropertyMetadata(false, new PropertyChangedCallback(IsPressedChanged)));
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof (UISearchBox), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty HoldSearchTextAfterHideProperty = DependencyProperty.Register("HoldSearchTextAfterHide", typeof(bool), typeof(UISearchBox), new UIPropertyMetadata(true));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof (SearchPlacementEnum), typeof(UISearchBox), new UIPropertyMetadata(SearchPlacementEnum.Left));
        public static readonly DependencyProperty ShowOptionsProperty = DependencyProperty.Register("ShowOptions", typeof(bool), typeof (UISearchBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty OptionsContentProperty = DependencyProperty.Register("OptionsContent", typeof(object), typeof(UISearchBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CollapsibleProperty = DependencyProperty.Register("Collapsible", typeof(bool), typeof (UISearchBox), new UIPropertyMetadata(true));
        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register("PopupPlacement", typeof (PlacementMode), typeof(UISearchBox), new UIPropertyMetadata(PlacementMode.Bottom));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof (UISearchBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WatermarkDataTemplateProperty = DependencyProperty.Register("WatermarkDataTemplate", typeof(DataTemplate), typeof(UISearchBox), new UIPropertyMetadata(null));
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

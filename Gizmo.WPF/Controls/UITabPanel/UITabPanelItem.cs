using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    /// <summary>
    /// UITabPanelItem - это элемент управления для представления выбираемых элементов для UITabPanel
    /// </summary>
    /// <remarks>
    /// UITabPanelItem is a control that implements a selectable item for UITabPanel
    /// </remarks>
    public class UITabPanelItem : ContentControl
    {
        #region Routed Events
        /// <summary>
        /// Событие представляющее выделение элемента UITabPanelItem
        /// </summary>
        /// <remarks>
        /// Routed event represents Selection Event for UITabPanelItem
        /// </remarks>
        public static readonly RoutedEvent SelectedEvent;

        /// <summary>
        /// Событие представляющее снятие выделения с элемента UITabPanelItem
        /// </summary>
        /// <remarks>
        /// Routed event represents Unselection Event for UITabPanelItem
        /// </remarks>
        public static readonly RoutedEvent UnselectedEvent;

        /// <summary>
        /// Обработчик события представляющего снятие выделения с элемента UITabPanelItem
        /// </summary>
        /// <remarks>
        /// Routed event handler represents Unselection Event for UITabPanelItem
        /// </remarks>
        public event RoutedEventHandler Selected
        {
            add
            {
                AddHandler(SelectedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedEvent, value);
            }
        }

        /// <summary>
        /// Обработчик события представляющего снятие выделения с элемента UITabPanelItem
        /// </summary>
        /// <remarks>
        /// Routed event handler represents Unselection Event for UITabPanelItem
        /// </remarks>
        public event RoutedEventHandler Unselected
        {
            add
            {
                AddHandler(UnselectedEvent, value);
            }
            remove
            {
                RemoveHandler(UnselectedEvent, value);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UITabPanelItem()
        : base()
        {
            DefaultStyleKey = typeof(UITabPanelItem);
        }

        static UITabPanelItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UITabPanelItem), new FrameworkPropertyMetadata(typeof(UITabPanelItem)));
            SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanelItem));
            UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UITabPanelItem));
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Виртуальная функция, которая реализует обработку свойства IsSelected.
        /// </summary>
        /// <remarks>
        /// Virtual function that implements how to handle IsSelected.
        /// </remarks>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            //нажатие по контенту вкладки трактуется как нажатие по вкладке, соответственно нужно фильтровать источник события OnMouseLeftButtonUp
            //clicking on the content of the tab is interpreted as clicking on the tab, respectively, you need to filter the source of the OnMouseLeftButtonUp event
            if (Equals(e.Source, this))
            {
                if (ParentUITabPanel != null)
                {
                    if (ParentUITabPanel.CanUnselect)
                        Selection(!IsSelected);
                    else if (!IsSelected)
                        Selection(true);
                }
                e.Handled = true;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Вспомогательная функция для обработки действий при выборе элемента.
        /// </summary>
        /// <remarks>
        /// Helper function for handling actions when selecting an item.
        /// </remarks>
        internal void Selection(bool value)
        {
            if (ParentUITabPanel != null)
            {
                if (value)
                {
                    ParentUITabPanel.Select(this, true);
                }
                else
                {
                    ParentUITabPanel.Select(this, false);
                }
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Parent ItemsControl for UITabPanel Items
        /// </summary>
        /// <remarks>
        /// Parent ItemsControl for UITabPanel Items
        /// </remarks>
        private UITabPanel ParentUITabPanel
        {
            get
            {
                UITabPanel tabPanel = ParentSelector as UITabPanel;

                if (tabPanel == null)//in some cases we cant find ParentSelector, so we try to find it in another way
                {
                    tabPanel = VisualHelper.FindVisulaParent<UITabPanel>(this);
                }
                return tabPanel;
            }
        }

        /// <summary>
        /// Parent Selector for UITabPanel Items
        /// </summary>
        /// <remarks>
        /// Parent Selector for UITabPanel Items
        /// </remarks>
        private Selector ParentSelector
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Контент для отображени  в заголовке UITabPanelItem
        /// </summary>
        /// <remarks>
        /// Header content for UITabPanelItem
        /// </remarks>
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        /// <summary>
        /// Указывает был ли выделен UITabPanelItem.
        /// </summary>
        /// <remarks>
        /// Indicates whether this UITabPanelItem is selected.
        /// </remarks>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        /// <summary>
        /// Указывает ориентацию UITabPanelItem.
        /// </summary>
        /// <remarks>
        /// Specifies orientation of UITabPanelItem.
        /// </remarks>
        public UITabPanelItemOrientation Orientation
        {
            get => (UITabPanelItemOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        /// <summary>
        /// Указывает стиль отображения UITabPanelItem.
        /// </summary>
        /// <remarks>
        /// Specifies the display style of the UITabPanelItem.
        /// </remarks>
        public UITabPanelStyleEnum TabStyle
        {
            get => (UITabPanelStyleEnum)GetValue(TabStyleProperty);
            set => SetValue(TabStyleProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(UITabPanelItem), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(UITabPanelItem), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(UITabPanelItemOrientation), typeof(UITabPanelItem), new FrameworkPropertyMetadata(UITabPanelItemOrientation.Left));
        public static readonly DependencyProperty TabStyleProperty = DependencyProperty.Register("TabStyle", typeof(UITabPanelStyleEnum), typeof(UITabPanelItem), new FrameworkPropertyMetadata(UITabPanelStyleEnum.Tabs));
        #endregion

        #region Property Callbacks
        private static void IsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanelItem tabPanelItem = (UITabPanelItem)o;
            tabPanelItem.RaiseEvents();
        }
        /// <summary>
        /// Вызываем соответствующее событие, когда значение IsSelected было изменено.
        /// </summary>
        /// <remarks>
        /// Raise the appropriate event when the IsSelected value has changed.
        /// </remarks>
        internal void RaiseEvents()
        {
            if (IsSelected)
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = SelectedEvent
                };
                RaiseEvent(args);
            }
            else
            {
                RoutedEventArgs args = new RoutedEventArgs
                {
                    RoutedEvent = UnselectedEvent
                };
                RaiseEvent(args);
            }
        }
        #endregion
    }
}

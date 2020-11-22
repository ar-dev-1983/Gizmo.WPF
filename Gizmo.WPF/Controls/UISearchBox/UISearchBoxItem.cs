using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    /// <summary>
    /// UISearchBoxItem - это элемент управления для представления выбираемых элементов для UISearchBox
    /// </summary>
    /// <remarks>
    /// UISearchBoxItem is a control that implements a selectable item for UISearchBox
    /// </remarks>
    public class UISearchBoxItem : ContentControl, ICorneredControl
    {
        #region Routed Events
        /// <summary>
        /// Событие представляющее выбор элемента UISearchBoxItem
        /// </summary>
        /// <remarks>
        /// Routed event represents Selection Event for UISearchBoxItem
        /// </remarks>
        public static readonly RoutedEvent SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(UISearchBoxItem));

        /// <summary>
        /// Обработчик события представляющего выбор элемента UISearchBoxItem
        /// </summary>
        /// <remarks>
        /// Routed event handler represents Selection Event for UISearchBoxItem
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
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UISearchBoxItem() : base()
        {
            DefaultStyleKey = typeof(UISearchBoxItem);
        }
        static UISearchBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISearchBoxItem), new FrameworkPropertyMetadata(typeof(UISearchBoxItem)));
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
        /// Virtual function that implements how to handle IsSelected property.
        /// </remarks>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            Select();
            e.Handled = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Вспомогательная функция для обработки действий при выборе элемента в режиме одиночного выделения элементов.
        /// </summary>
        /// <remarks>
        /// Helper function for handling actions when selecting an item in single item selection mode.
        /// </remarks>
        private void Select()
        {
            if (ParentUISearchBox != null)
            {
                IsSelected = true;
                ParentUISearchBox.IsPressed = false;
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Parent ItemsControl for UISearchBoxItems
        /// </summary>
        /// <remarks>
        /// Parent ItemsControl for UISearchBoxItems
        /// </remarks>
        private UISearchBox ParentUISearchBox
        {
            get
            {
                UISearchBox searchBox = ParentSelector as UISearchBox;

                if (searchBox == null)
                {
                    searchBox = VisualHelper.FindVisulaParent<UISearchBox>(this);
                }
                return searchBox;
            }
        }

        /// <summary>
        /// Parent Selector for UISearchBoxItems
        /// </summary>
        /// <remarks>
        /// Parent Selector for UISearchBoxItems
        /// </remarks>
        internal Selector ParentSelector
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Указывает был ли выделен UISearchBoxItems.
        /// </summary>
        /// <remarks>
        /// Indicates whether this UISearchBoxItems is selected.
        /// </remarks>
        [Bindable(true), Category("Appearance")]
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        /// <summary>
        /// Задает радиус углов для UISearchBoxItems.
        /// </summary>
        /// <remarks>
        /// Sets the corner radius for the UISearchBoxItems.
        /// </remarks>
        [Bindable(true), Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(UISearchBoxItem), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UISearchBoxItem), new UIPropertyMetadata(new CornerRadius(0)));
        #endregion

        #region Property Callbacks
        private static void IsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UISearchBoxItem item = o as UISearchBoxItem;
            bool isSelected = (bool)e.NewValue;

            if (isSelected)
            {
                item.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, item));
            }
        }

        protected virtual void OnSelected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(true, e);
        }

        private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion
    }
}

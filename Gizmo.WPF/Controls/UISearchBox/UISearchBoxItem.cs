using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    public class UISearchBoxItem : ContentControl, ICorneredControl
    {
        #region Routed Events
        public static readonly RoutedEvent SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(ListBoxItem));
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
        public UISearchBoxItem()
: base()
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
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            Select();
            e.Handled = true;
        }
        #endregion

        #region Private Methods
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
        private UISearchBox ParentUISearchBox
        {
            get
            {
                UISearchBox searchBox = ParentSelector as UISearchBox;

                if (searchBox == null)
                {
                    searchBox = CustomVisualTreeHelper.FindVisulaParent<UISearchBox>(this);
                }
                return searchBox;
            }
        }
        internal Selector ParentSelector
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
            }
        }
        #endregion

        #region Public Properties
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
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

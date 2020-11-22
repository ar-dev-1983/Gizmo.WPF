using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    /// <summary>
    /// UIEnumSwitchItem - это элемент управления для представления выбираемых элементов для UIEnumSwitch
    /// </summary>
    /// <remarks>
    /// UIEnumSwitchItem is a control that implements a selectable item for UIEnumSwitch
    /// </remarks>
    public class UIEnumSwitchItem : ContentControl, ICorneredControl
    {
        #region Routed Events
        /// <summary>
        /// Событие представляющее выделение элемента UIEnumSwitchItem
        /// </summary>
        /// <remarks>
        /// Routed event represents Selection Event for UIEnumSwitchItem
        /// </remarks>
        public static readonly RoutedEvent SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(UIEnumSwitchItem));

        /// <summary>
        /// Событие представляющее снятие выделения с элемента UIEnumSwitchItem
        /// </summary>
        /// <remarks>
        /// Routed event represents Unselection Event for UIEnumSwitchItem
        /// </remarks>
        public static readonly RoutedEvent UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(UIEnumSwitchItem));

        /// <summary>
        /// Обработчик события представляющего снятие выделения с элемента UIEnumSwitchItem
        /// </summary>
        /// <remarks>
        /// Routed event handler represents Unselection Event for UIEnumSwitchItem
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
        /// Обработчик события представляющего снятие выделения с элемента UIEnumSwitchItem
        /// </summary>
        /// <remarks>
        /// Routed event handler represents Unselection Event for UIEnumSwitchItem
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
        public UIEnumSwitchItem() : base()
        {
            DefaultStyleKey = typeof(UIEnumSwitchItem);
        }
        static UIEnumSwitchItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIEnumSwitchItem), new FrameworkPropertyMetadata(typeof(UIEnumSwitchItem)));
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Виртуальная функция, которая реализует обработку свойства IsSelected в соответствии с режимом выбора элементов родительского UIEnumSwitch.
        /// </summary>
        /// <remarks>
        /// Virtual function that implements how to handle IsSelected property acording to parent UIEnumSwitch selection mode
        /// </remarks>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (ParentUIEnumSwitch.SelectionMode == SelectionModeEnum.Single)
            {
                if (!IsSelected && FirstTimeSelected)
                {
                    SingleSelection(true, SelectionAction.FirstSelection);
                }
                else if (!IsSelected && !FirstTimeSelected)
                {
                    SingleSelection(true, SelectionAction.Selection);
                }
            }
            else if (ParentUIEnumSwitch.SelectionMode == SelectionModeEnum.MultipleWhithDefault)
            {
                if (FirstTimeSelected)
                {
                    MultipleSelectionWithDefault(!IsSelected, SelectionAction.FirstSelection);
                }
                else
                {
                    if (ParentUIEnumSwitch.IsItemIsDefault(this))
                    {
                        if (!IsSelected)
                            MultipleSelectionWithDefault(!IsSelected, SelectionAction.Selection);
                    }
                    else
                    {
                        MultipleSelectionWithDefault(!IsSelected, SelectionAction.Selection);
                    }
                }
            }
            else if (ParentUIEnumSwitch.SelectionMode == SelectionModeEnum.Multiple)
            {
                if (FirstTimeSelected)
                {
                    MultipleSelection(!IsSelected, SelectionAction.FirstSelection);
                }
                else
                {
                    MultipleSelection(!IsSelected, SelectionAction.Selection);
                }
            }
            e.Handled = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Вспомогательная функция для установки признака сброса состояния при выборе элемента по умолчанию в родительском UIEnumSwitch.
        /// </summary>
        /// <remarks>
        /// Helper function for setting the state reset flag when the default item is selected in the parent UIEnumSwitch.
        /// </remarks>
        internal void SetReseted()
        {
            ResetedByDefaultItem = true;
        }
        /// <summary>
        /// Вспомогательная функция для снятия признака сброса состояния.
        /// </summary>
        /// <remarks>
        /// Helper function for setting state reset flag to its default value.
        /// </remarks>
        internal void RevertReseted()
        {
            ResetedByDefaultItem = false;
        }

        /// <summary>
        /// Вспомогательная функция для обработки действий при выборе элемента в режиме одиночного выделения элементов.
        /// </summary>
        /// <remarks>
        /// Helper function for handling actions when selecting an item in single item selection mode.
        /// </remarks>
        internal void SingleSelection(bool value, SelectionAction state)
        {
            if (ParentUIEnumSwitch != null)
            {
                IsSelected = value;

                switch (state)
                {
                    case SelectionAction.FirstSelection:
                        if (IsSelected)
                        {
                            if (FirstTimeSelected)
                                FirstTimeSelected = false;
                            OnSelected(new RoutedEventArgs(Selector.SelectedEvent, this));
                        }
                        ParentUIEnumSwitch.HandleSingleSelection(DataContext, false);
                        break;
                    case SelectionAction.Selection:
                        if (IsSelected)
                        {
                            OnSelected(new RoutedEventArgs(Selector.SelectedEvent, this));
                        }
                        ParentUIEnumSwitch.HandleSingleSelection(DataContext, true);
                        break;
                }
            }
        }

        /// <summary>
        /// Вспомогательная функция для обработки действий при выборе элемента в режиме множественного выделения элементов с выбором элемента по умолчанию, если ничего не выбрано.
        /// </summary>
        /// <remarks>
        /// Helper function for handling actions when selecting an item in multiple selection mode with a default item selection if nothing is selected.
        /// </remarks>
        internal void MultipleSelectionWithDefault(bool value, SelectionAction state)
        {
            if (ParentUIEnumSwitch != null)
            {
                IsSelected = value;

                switch (state)
                {
                    case SelectionAction.FirstSelection:
                        if (IsSelected)
                        {
                            if (FirstTimeSelected)
                                FirstTimeSelected = false;

                            OnSelected(new RoutedEventArgs(Selector.SelectedEvent, this));
                        }
                        else
                        {
                            if (FirstTimeSelected)
                                FirstTimeSelected = false;

                            OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, this));
                        }
                        ParentUIEnumSwitch.HandleMultipleSelectionWithDefault(DataContext);
                        break;
                    case SelectionAction.Selection:
                        if (IsSelected)
                        {
                            OnSelected(new RoutedEventArgs(Selector.SelectedEvent, this));
                        }
                        else
                        {
                            OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, this));
                        }
                        ParentUIEnumSwitch.HandleMultipleSelectionWithDefault(DataContext);
                        break;
                }
            }
        }

        /// <summary>
        /// Вспомогательная функция для обработки действий при выборе элемента в режиме множественного выделения элементов.
        /// </summary>
        /// <remarks>
        /// Helper function for handling actions when selecting an item in multiple selection mode.
        /// </remarks>
        internal void MultipleSelection(bool value, SelectionAction state)
        {
            if (ParentUIEnumSwitch != null)
            {
                IsSelected = value;

                switch (state)
                {
                    case SelectionAction.FirstSelection:
                        if (IsSelected)
                        {
                            if (FirstTimeSelected)
                                FirstTimeSelected = false;

                            OnSelected(new RoutedEventArgs(Selector.SelectedEvent, this));
                        }
                        else
                        {
                            if (FirstTimeSelected)
                                FirstTimeSelected = false;

                            OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, this));
                        }
                        ParentUIEnumSwitch.HandleMultipleSelection(DataContext, false);
                        break;
                    case SelectionAction.Selection:
                        if (IsSelected)
                        {
                            OnSelected(new RoutedEventArgs(Selector.SelectedEvent, this));
                        }
                        else
                        {
                            OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, this));
                        }
                        ParentUIEnumSwitch.HandleMultipleSelection(DataContext, false);
                        break;
                }
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Указывает на то, вызывались ли события OnSelected и OnUnselected в первый раз. Для реализации внутренней логики выбора элементов в родительском UIEnumSwitch.
        /// </summary>        
        /// <remarks>
        /// Indicates that in OnSelected and OnUselected rises first time. To implement the internal logic for selecting items in the parent UIEnumSwitch.
        /// </remarks>
        internal bool FirstTimeSelected { private set; get; } = true;

        /// <summary>
        /// Указывает на то, были ли элемент сброшен при выборе элемента по умолчанию в родительском UIEnumSwitch. Для реализации внутренней логики выбора элементов в родительском UIEnumSwitch.
        /// </summary>        
        /// <remarks>
        /// Indicates whether the item was reseted when the default item was selected in the parent UIEnumSwitch. To implement the internal logic for selecting items in the parent UIEnumSwitch.
        /// </remarks>
        internal bool ResetedByDefaultItem { private set; get; } = false;

        /// <summary>
        /// Parent ItemsControl for UIEnumSwitchItems
        /// </summary>
        /// <remarks>
        /// Parent ItemsControl for UIEnumSwitchItems
        /// </remarks>
        private UIEnumSwitch ParentUIEnumSwitch
        {
            get
            {
                UIEnumSwitch searchBox = ParentSelector as UIEnumSwitch;

                if (searchBox == null)//in some cases we cant find ParentSelector, so we try to find it in another way
                {
                    searchBox = VisualHelper.FindVisulaParent<UIEnumSwitch>(this);
                }
                return searchBox;
            }
        }

        /// <summary>
        /// Parent Selector for UIEnumSwitchItems
        /// </summary>
        /// <remarks>
        /// Parent Selector for UIEnumSwitchItems
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
        /// Указывает был ли выделен UIEnumSwitchItem.
        /// </summary>
        /// <remarks>
        /// Indicates whether this UIEnumSwitchItem is selected.
        /// </remarks>
        [Bindable(true), Category("Appearance")]
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        /// <summary>
        /// Задает радиус углов для UIEnumSwitchItem. Не влияет напрямую на внешний вид, значение устанавливается логикой внутри родительского UIEnumSwitch.
        /// </summary>
        /// <remarks>
        /// Sets the corner radius for the UIEnumSwitchItem. Does not directly affect the appearance, the value is set by logic inside the parent UIEnumSwitch.
        /// </remarks>
        [Bindable(true), Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(UIEnumSwitchItem), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIEnumSwitchItem), new UIPropertyMetadata(new CornerRadius(0)));
        #endregion

        #region Property Callbacks

        protected virtual void OnSelected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(true, e);
        }

        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            HandleIsUnselectedChanged(true, e);
        }

        private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        private void HandleIsUnselectedChanged(bool newValue, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion
    }
}

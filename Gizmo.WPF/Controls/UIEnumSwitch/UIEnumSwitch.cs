using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Gizmo.WPF
{
    /// <summary>
    /// UIEnumSwitch - это элемент управления для представления всех значений enum определенного типа, когда установлено свойство SourceEnum,
    /// или списка значений для разных типов, производных от Enum, когда установлено свойство ItemsSource
    /// </summary>
    /// <remarks>
    /// UIEnumSwitch is a control for presenting all enum values of a particular type when the SourceEnum property is set,
    /// or a list of values for different types derived from Enum when the ItemsSource property is set
    /// </remarks>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIEnumSwitchItem))]
    public class UIEnumSwitch : Selector, ICorneredControl
    {
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UIEnumSwitch()
        : base()
        {
            SelectedItems = new ObservableCollection<object>();
            UnselectedItems = new ObservableCollection<object>();
            DefaultStyleKey = typeof(UIEnumSwitch);
        }

        static UIEnumSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIEnumSwitch), new FrameworkPropertyMetadata(typeof(UIEnumSwitch)));
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
            template.Seal();
            ItemsPanelProperty.OverrideMetadata(typeof(UIEnumSwitch), new FrameworkPropertyMetadata(template));
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
            return item is UIEnumSwitchItem;
        }

        /// <summary>
        /// Создает контейнер для элемента
        /// </summary>
        /// <remarks>
        /// Create or identify the element used to display the given item.
        /// </remarks>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIEnumSwitchItem();
        }

        /// <summary>
        /// Возвращает индекс в коллекции элементов ItemsContainer
        /// </summary>
        /// <remarks>
        /// Returns index of item in ItemsContainer collection
        /// </remarks>
        private int ElementIndex(UIEnumSwitchItem item)
        {
            return ItemContainerGenerator.IndexFromContainer(item);
        }

        /// <summary>
        /// Возвращает true если элемент является элементом по умолчанию (первый в коллекции элементов)
        /// </summary>
        /// <remarks>
        /// Returns true if item is first in ItemsContainer collection
        /// </remarks>
        public bool IsItemIsDefault(UIEnumSwitchItem item) => ElementIndex(item) == 0;

        /// <summary>
        /// Задает свойства контейнера элемента для отображения,
        /// а так же устанавливает значения IsSelected если задано свойство SelectedIndex или SelectedItem
        /// </summary>
        /// <remarks>
        /// Prepare the element to display the item,
        /// depending on SelectionMode of elements and specified SelectedIndex or SelectedItem
        /// </remarks>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (SelectedIndex == -1 && SelectedItem == null)
            {
                if (SelectionMode == SelectionModeEnum.MultipleWhithDefault)
                {
                    if (Items.IndexOf(item) == 0)
                    {
                        (element as UIEnumSwitchItem).MultipleSelectionWithDefault(true, SelectionAction.FirstSelection);
                    }
                }
            }
            else if (SelectedIndex != -1)
            {
                if (SelectedIndex == Items.IndexOf(item))
                {
                    if (SelectionMode == SelectionModeEnum.Single)
                    {
                        (element as UIEnumSwitchItem).SingleSelection(true, SelectionAction.FirstSelection);
                    }
                    else if (SelectionMode == SelectionModeEnum.MultipleWhithDefault)
                    {
                        (element as UIEnumSwitchItem).MultipleSelectionWithDefault(true, SelectionAction.FirstSelection);
                    }
                    else if (SelectionMode == SelectionModeEnum.Multiple)
                    {
                        (element as UIEnumSwitchItem).MultipleSelection(true, SelectionAction.FirstSelection);
                    }
                }
            }
            else if (SelectedItem != null)
            {
                if (Equals(item, SelectedItem))
                {
                    if (SelectionMode == SelectionModeEnum.Single)
                    {
                        (element as UIEnumSwitchItem).SingleSelection(true, SelectionAction.FirstSelection);
                    }
                    else if (SelectionMode == SelectionModeEnum.MultipleWhithDefault)
                    {
                        (element as UIEnumSwitchItem).MultipleSelectionWithDefault(true, SelectionAction.FirstSelection);
                    }
                    else if (SelectionMode == SelectionModeEnum.Multiple)
                    {
                        (element as UIEnumSwitchItem).MultipleSelection(true, SelectionAction.FirstSelection);
                    }
                }
            }

            if (Items.Count == 1)
            {
                (element as UIEnumSwitchItem).CornerRadius = CornerRadius;
                (element as UIEnumSwitchItem).BorderThickness = new Thickness(1);
            }
            else if (Items.IndexOf(item) == 0)
            {
                (element as UIEnumSwitchItem).CornerRadius = new CornerRadius(CornerRadius.TopLeft, 0d, 0d, CornerRadius.BottomLeft);
                (element as UIEnumSwitchItem).BorderThickness = new Thickness(1, 1, 0, 1);
            }
            else if (Items.IndexOf(item) == Items.Count - 1)
            {
                (element as UIEnumSwitchItem).CornerRadius = new CornerRadius(0d, CornerRadius.TopRight, CornerRadius.BottomRight, 0d);
                (element as UIEnumSwitchItem).BorderThickness = new Thickness(0, 1, 1, 1);
            }
            else
            {
                (element as UIEnumSwitchItem).CornerRadius = new CornerRadius(0);
                (element as UIEnumSwitchItem).BorderThickness = new Thickness(0, 1, 0, 1);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Виртуальная функция, которая вызывается при изменении коллекции элементов Items
        /// </summary>
        /// <remarks>
        /// A virtual function that is called when the Items is changed.
        /// </remarks>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            AttachItems();
        }

        /// <summary>
        /// Присоединяет элементы коллекции если они еще не присоеденены к контейнеру элементов
        /// </summary>
        /// <remarks>
        /// Attach items if they not attached already
        /// </remarks>
        private void AttachItems()
        {
            bool appropriateTypeOfItemIsNotSet = false;
            foreach (object item in Items)
            {
                if (item.GetType().IsEnum && !appropriateTypeOfItemIsNotSet)
                {
                    appropriateTypeOfItemIsNotSet = false;
                    var itemContainer = GetItemContainer(item);
                    if (itemContainer != null)
                    {
                        UIEnumSwitchItem enumSwitchItem = new UIEnumSwitchItem();

                        Binding IsSelectedBinding = new Binding("IsSelected")
                        {
                            Mode = BindingMode.TwoWay,
                            Source = itemContainer
                        };

                        enumSwitchItem.SetBinding(UIEnumSwitchItem.IsSelectedProperty, IsSelectedBinding);
                        PrepareContainerForItemOverride(enumSwitchItem, item);
                    }
                }
                else
                {
                    appropriateTypeOfItemIsNotSet = true;
                    continue;
                }
            }
            if (appropriateTypeOfItemIsNotSet)
                throw new InvalidOperationException("One or more elements in Items collection is not Enun derived type!");
        }

        /// <summary>
        /// Возвращает контейнер для заданного элемента.
        /// </summary>
        /// <remarks>
        /// Returns the element used to display the given item.
        /// </remarks>
        private UIEnumSwitchItem GetItemContainer(object item)
        {
            UIEnumSwitchItem itemContainer = item as UIEnumSwitchItem;
            if (itemContainer == null)
            {
                UpdateLayout();
                itemContainer = ItemContainerGenerator.ContainerFromItem(item) as UIEnumSwitchItem;
            }
            return itemContainer;
        }

        /// <summary>
        /// Вспомогательная функция для режима выбора одного элемента, задает состояние IsSelected если оно указано и заполняет список коллекции UnselectedItems 
        /// </summary>
        /// <remarks>
        /// Helper function that sets items state if state value is specified, and filled UnselectedItems collection
        /// This function is used in Single item Selection Mode
        /// </remarks>
        private void FillUnselected(object excludedValue, bool setItemsToState = false)
        {
            foreach (var item in Items)
            {
                if (!Equals(item, excludedValue))
                {
                    var itemContainer = GetItemContainer(item);
                    if (itemContainer != null)
                    {
                        if (!itemContainer.IsSelected && !UnselectedItems.Contains(item))
                        {
                            UnselectedItems.Add(item);
                        }
                        else if (itemContainer.IsSelected && !UnselectedItems.Contains(item))
                        {
                            if (setItemsToState)
                            {
                                itemContainer.IsSelected = false;

                                if (!itemContainer.IsSelected)
                                    UnselectedItems.Add(item);
                            }
                        }
                        else if (itemContainer.IsSelected && UnselectedItems.Contains(item))
                        {
                            if (setItemsToState)
                            {
                                itemContainer.IsSelected = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Вспомогательная функция для режима выбора множества элементов, задает состояние IsSelected, и ResetedByDefaultItem, а так же заполняет список коллекции UnselectedItems
        /// </summary>
        /// <remarks>
        /// Helper function that sets private property ResetedByDefaultItem of items to true and deselected they, then items added to UnselectedItems collection
        /// This function is used in Multiple items Selection Mode with fallback to default
        /// </remarks>
        private void ResetItems()
        {
            foreach (var item in Items)
            {
                if (Items.IndexOf(item) != 0)
                {
                    UnselectedItems.Add(item);
                    var itemContainer = GetItemContainer(item);
                    if (itemContainer != null)
                    {
                        if (itemContainer.IsSelected)
                        {
                            itemContainer.IsSelected = false;
                            itemContainer.SetReseted();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Вспомогательная функция задает состояние элементов и вызывает событие SelectionChanged если задано raiseEvent в значении true.
        /// Эта функция обеспечивает выполнение режима выбора одиночного элемента в коллекции одновременно.
        /// </summary>
        /// <remarks>
        /// Helper function that sets state of the item and raised SelectionChanged Event if raiseEvent is set to true.
        /// This function is used to maintain Single item Selection Mode.
        /// </remarks>
        internal void HandleSingleSelection(object value, bool riseEvent)
        {
            if (value != null)
            {
                SelectedItems.Clear();
                UnselectedItems.Clear();

                SelectedItems.Add(value);
                FillUnselected(value, true);

                LastSelectedItem = SelectedItems != null ? SelectedItems.Count != 0 ? SelectedItems[SelectedItems.Count - 1] : null : null;

                if (riseEvent)
                    RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, UnselectedItems, SelectedItems));

            }
        }

        /// <summary>
        /// Вспомогательная функция задает состояние элементов и вызывает событие SelectionChanged.
        /// Эта функция обеспечивает выполнение режима выбора множества элементов в коллекции одновременно со сбросом на элемент по умолчанию, если ни один из элементов не выбран.
        /// </summary>
        /// <remarks>
        /// Helper function that sets state of the item and raised SelectionChanged Event.
        /// This function is used to maintain Multiple items Selection Mode with fallback to default.
        /// </remarks>
        internal void HandleMultipleSelectionWithDefault(object value)
        {
            if (value != null)
            {
                if (SelectedItems.Count == 0)
                {
                    //ни один элемент не был выбран ни разу, выбирает элемент по умолчанию
                    //no item was selected before, so we select default item
                    SelectedItems.Clear();
                    UnselectedItems.Clear();

                    SelectedItems.Add(value);

                    foreach (var node in Items)
                    {
                        if (!Equals(node, value))
                        {
                            UnselectedItems.Add(node);
                        }
                    }
                }
                else
                {
                    if (Equals(value, Items[0]))
                    {
                        //был выбран элемент по умолчанию, снимаем выделения со всех выбранных ранее элементов
                        //default item was selected, so we unselect previously selected items
                        SelectedItems.Clear();
                        UnselectedItems.Clear();

                        SelectedItems.Add(value);
                        ResetItems();

                        RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, UnselectedItems, SelectedItems));
                    }
                    else
                    {
                        if (SelectedItems.Contains(Items[0]))
                        {
                            //был выбран элемент и в выбранных ранее элементах присутсвует элемент по умолчанию, убираем элемент по умолчанию из списка выбранных и добавляем в список выбранных элементов текущий
                            //item was selected after default item selection, so we remove default items from selection and select current item
                            SelectedItems.Remove(Items[0]);
                            UnselectedItems.Add(Items[0]);

                            var defaultItem = GetItemContainer(Items[0]);
                            if (defaultItem != null)
                            {
                                if (defaultItem.IsSelected)
                                    defaultItem.IsSelected = false;
                            }

                            SelectedItems.Add(value);
                            UnselectedItems.Remove(value);

                            if (CheckIfItemWasReseted(value))
                            {
                                RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, UnselectedItems, SelectedItems));
                                var selectedItem = GetItemContainer(value);
                                if (selectedItem != null)
                                {
                                    selectedItem.RevertReseted();
                                }
                            }
                        }
                        else
                        {
                            if (!SelectedItems.Contains(value))
                            {
                                //в ранее выбранных элементах отсутсвует текущий элемент, добавляем его в список выбранных элементов
                                //in the previously selected items the current element is missing, add it to the list of selected items
                                SelectedItems.Add(value);
                                UnselectedItems.Remove(value);

                                if (CheckIfItemWasReseted(value))
                                {
                                    RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, UnselectedItems, SelectedItems));
                                    var selectedItem = GetItemContainer(value);
                                    if (selectedItem != null)
                                    {
                                        selectedItem.RevertReseted();
                                    }
                                }
                            }
                            else
                            {
                                //в ранее выбранных элементах находится текущий, убираем его из списка выбранных элементов
                                //the current item is in the previously selected items, remove it from the list of selected items
                                SelectedItems.Remove(value);
                                UnselectedItems.Add(value);

                                if (SelectedItems.Count == 0)
                                {
                                    //если в списке выбранных элементов не осталось ни одного - добавляем в него элемент по умолчанию и выделяем его
                                    //if there are no items left in the list of selected items, add the default item to it and select it
                                    SelectedItems.Add(Items[0]);
                                    UnselectedItems.Remove(Items[0]);

                                    var defaultItem = GetItemContainer(Items[0]);
                                    if (defaultItem != null)
                                    {
                                        defaultItem.IsSelected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                LastSelectedItem = SelectedItems != null ? SelectedItems.Count != 0 ? SelectedItems[SelectedItems.Count - 1] : null : null;
            }
        }

        /// <summary>
        /// Вспомогательная функция, которая проверяет был ли заданный элемент сброшен элементом по умолчанию
        /// Эта функция используется в режиме выбора множества элементов в коллекции одновременно со сбросом на элемент по умолчанию, если ни один из элементов не выбран.
        /// </summary>
        /// <remarks>
        /// Helper function that checks if item is reseted by default item
        /// This function is used in Multiple items Selection Mode with fallback to default
        /// </remarks>
        private bool CheckIfItemWasReseted(object item)
        {
            bool result = false;

            var itemContainer = GetItemContainer(item);
            if (itemContainer != null)
            {
                if (itemContainer.ResetedByDefaultItem)
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// Вспомогательная функция обеспечивает выполнение режима выбора множества элементов в коллекции одновременно.
        /// </summary>
        /// <remarks>
        /// Helper function is used to maintain Multiple items Selection Mode.
        /// </remarks>
        internal void HandleMultipleSelection(object value, bool riseEvent)
        {
            if (value != null)
            {
                if (SelectedItems.Count == 0)
                {
                    //ни один элемент не был выбран
                    //no item was selected
                    SelectedItems.Clear();
                    UnselectedItems.Clear();

                    SelectedItems.Add(value);

                    foreach (var node in Items)
                    {
                        if (!Equals(node, value))
                        {
                            UnselectedItems.Add(node);
                        }
                    }
                }
                else
                {
                    if (!SelectedItems.Contains(value))
                    {
                        //в ранее выбранных элементах отсутсвует текущий элемент, добавляем его в список выбранных элементов
                        //in the previously selected items the current element is missing, add it to the list of selected items
                        SelectedItems.Add(value);
                        UnselectedItems.Remove(value);
                    }
                    else
                    {
                        //в ранее выбранных элементах находится текущий, убираем его из списка выбранных элементов
                        //the current item is in the previously selected items, remove it from the list of selected items
                        SelectedItems.Remove(value);
                        UnselectedItems.Add(value);
                    }
                }
                LastSelectedItem = SelectedItems != null ? SelectedItems.Count != 0 ? SelectedItems[SelectedItems.Count - 1] : null : null;
            }
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Задает радиус углов для UIEnumSwitch. Corner radius of UIEnumSwitch
        /// </summary>
        /// <remarks>
        /// Так же данное своейство влияет напрямую на радиус углов первого и последнего элемента в коллекции. Affects also first and last items in Items Collection
        /// </remarks>
        [Bindable(true), Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Исходное значение Enum из значений типа которого заполняются элементы в коллекции Items. Source Enum for Items Collection to fill.
        /// </summary>
        /// <remarks>
        /// Использовать только если не задано свойство ItemsSource. Set SourceEnum only if ItemsSource is not set
        /// </remarks>
        [Bindable(true)]
        public Type SourceEnum
        {
            get => (Type)GetValue(SourceEnumProperty);
            set => SetValue(SourceEnumProperty, value);
        }

        /// <summary>
        /// Режим поведения UIEnumSwitch при выборе элементов. Selection mode of UIEnumSwitch
        /// </summary>
        [Bindable(true)]
        public SelectionModeEnum SelectionMode
        {
            get { return (SelectionModeEnum)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }
        /// <summary>
        /// Коллекция выбранных элементов. Collection of Selected Items.
        /// </summary>
        /// <remarks>
        /// Used as storage for Selected Items
        /// </remarks>
        [Bindable(true)]
        public ObservableCollection<object> SelectedItems
        {
            get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
            private set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        /// Коллекция не выбранных элементов. Collection of Unselected Items.
        /// </summary>
        /// <remarks>
        /// Used as storage for Unselected Items
        /// </remarks>
        [Bindable(true)]
        public ObservableCollection<object> UnselectedItems
        {
            get { return (ObservableCollection<object>)GetValue(UnselectedItemsProperty); }
            private set { SetValue(UnselectedItemsProperty, value); }
        }
        /// <summary>
        /// Значение последнего выбранного элемента
        /// </summary>
        /// <remarks>
        /// Value of last item in selection
        /// </remarks>
        [Bindable(true)]
        public object LastSelectedItem
        {
            get { return (object)GetValue(LastSelectedItemProperty); }
            private set { SetValue(LastSelectedItemProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIEnumSwitch), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty SourceEnumProperty = DependencyProperty.Register("SourceEnum", typeof(Type), typeof(UIEnumSwitch), new UIPropertyMetadata(null, new PropertyChangedCallback(SourceEnumChanged)));
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionModeEnum), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(SelectionModeEnum.Single));
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty UnselectedItemsProperty = DependencyProperty.Register("UnselectedItems", typeof(ObservableCollection<object>), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LastSelectedItemProperty = DependencyProperty.Register("LastSelectedItem", typeof(object), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(null));
        #endregion

        #region Property Callbacks
        /// <summary>
        /// Заполняет элементами коллекцию Items значениями из SourceEnum. Fills Items collection with enum values specified in SoueceEnum
        /// </summary>
        private static void SourceEnumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UIEnumSwitch enumSwitch = o as UIEnumSwitch;
                enumSwitch.ProcessSourceEnum();
            }
        }

        internal void ProcessSourceEnum()
        {
            if (SourceEnum != null)
            {
                if (ItemsSource is null)
                {
                    if (SourceEnum.IsEnum)
                    {
                        Items.Clear();
                        foreach (var node in SourceEnum.GetEnumValues())
                        {
                            Items.Add(node);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("SourceEnum property is not the Enun derived type!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Cannot set Items from SourceEnum because ItemsSource is already set!");
                }
            }
        }
        #endregion
    }
}

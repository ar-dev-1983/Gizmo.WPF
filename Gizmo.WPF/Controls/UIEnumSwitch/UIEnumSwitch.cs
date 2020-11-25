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
        #region Internal
        /// <summary>
        /// Перечисление для отслеживания текущего состояния UIEnumSwitch
        /// </summary>
        /// <remarks>
        /// Enum for determine UIEnumSwitch state
        /// </remarks>
        internal enum ControlState
        {
            /// <summary>
            /// Не готов, элементы коллекции Items не установлены
            /// </summary>
            /// <remarks>
            /// Not ready, no items in Items Collection
            /// </remarks>
            NotReady,
            /// <summary>
            /// Элементы коллекции Items созданы
            /// </summary>
            /// <remarks>
            /// Items added to collection
            /// </remarks>
            ItemsCreated,
            /// <summary>
            /// Готов для работы
            /// </summary>
            /// <remarks>
            /// Ready for work
            /// </remarks>
            Ready,
            /// <summary>
            /// Этот статус требуется для прерывания обработки OnSelectedIndexChanged и OnSelectedItemChanged при задании значений SelectedIndex или SelectedItem в логике выбора элементов внутри UIEnumSwitch.
            /// </summary>
            /// <remarks>
            /// This status is required to interrupt the processing of OnSelectedIndexChanged and OnSelectedItemChanged when the SelectedIndex or SelectedItem values are set in the item selection logic inside the UIEnumSwitch.
            /// </remarks>
            IgnoreChanges
        }
        #endregion

        #region Private Properties
        private ControlState state = ControlState.NotReady;
        #endregion

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
                if (SelectionMode == SelectionModeEnum.MultipleWithDefault)
                {
                    if (Items.IndexOf(item) == 0)
                    {
                        (element as UIEnumSwitchItem).MultipleSelectionWithDefault(true, SelectionAction.FirstSelection);
                    }
                    else
                    {
                        if (!UnselectedItems.Contains(item))
                            UnselectedItems.Add(item);
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
                    else if (SelectionMode == SelectionModeEnum.MultipleWithDefault)
                    {
                        (element as UIEnumSwitchItem).MultipleSelectionWithDefault(true, SelectionAction.FirstSelection);
                    }
                    else if (SelectionMode == SelectionModeEnum.Multiple)
                    {
                        (element as UIEnumSwitchItem).MultipleSelection(true, SelectionAction.FirstSelection);
                    }
                }
                else
                {
                    if (!UnselectedItems.Contains(item))
                        UnselectedItems.Add(item);
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
                    else if (SelectionMode == SelectionModeEnum.MultipleWithDefault)
                    {
                        (element as UIEnumSwitchItem).MultipleSelectionWithDefault(true, SelectionAction.FirstSelection);
                    }
                    else if (SelectionMode == SelectionModeEnum.Multiple)
                    {
                        (element as UIEnumSwitchItem).MultipleSelection(true, SelectionAction.FirstSelection);
                    }
                }
                else
                {
                    if (!UnselectedItems.Contains(item))
                        UnselectedItems.Add(item);
                }
            }
            if (SeparateItems)
            {
                this.BorderThickness = new Thickness(0);

                (element as UIEnumSwitchItem).CornerRadius = CornerRadius;
                (element as UIEnumSwitchItem).BorderThickness = new Thickness(1);

                if (Items.Count == 1)
                {
                    (element as UIEnumSwitchItem).Margin = new Thickness(0);
                }
                else if (Items.IndexOf(item) == 0)
                {
                    (element as UIEnumSwitchItem).Margin = new Thickness(0, 0, 2, 0);
                }
                else if (Items.IndexOf(item) == Items.Count - 1)
                {
                    (element as UIEnumSwitchItem).Margin = new Thickness(2, 0, 0, 0);
                }
                else
                {
                    (element as UIEnumSwitchItem).Margin = new Thickness(2, 0, 2, 0);
                }
            }
            else
            {
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
            state = ControlState.Ready;
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
        #endregion

        #region Selection Logic
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

        private void Select(object value, bool raiseEvent = false, bool selectItem = false)
        {
            if (value != null)
            {
                if (SelectionMode == SelectionModeEnum.Single)
                {
                    HandleSingleSelection(value, raiseEvent, selectItem);
                }
                else if (SelectionMode == SelectionModeEnum.MultipleWithDefault)
                {
                    HandleMultipleSelectionWithDefault(value, selectItem);
                }
                else if (SelectionMode == SelectionModeEnum.Multiple)
                {
                    HandleMultipleSelection(value, selectItem);
                }
            }
        }

        private object GetItemAtIndex(int index)
        {
            if (Items != null)
            {
                if (Items.Count != 0)
                {
                    return index == -1 ? Items[0] : index >= Items.Count ? Items[Items.Count - 1] : Items[index];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #region Single Selection Mode
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
        /// Вспомогательная функция задает состояние элементов и вызывает событие SelectionChanged если задано raiseEvent в значении true.
        /// Эта функция обеспечивает выполнение режима выбора одиночного элемента в коллекции одновременно.
        /// </summary>
        /// <remarks>
        /// Helper function that sets state of the item and raised SelectionChanged Event if raiseEvent is set to true.
        /// This function is used to maintain Single item Selection Mode.
        /// </remarks>
        internal void HandleSingleSelection(object value, bool raiseEvent, bool selectItem = false)
        {
            if (value != null)
            {
                SelectedItems.Clear();
                UnselectedItems.Clear();

                SelectedItems.Add(value);
                FillUnselected(value, true);

                if (selectItem)
                {
                    var itemToSelect = GetItemContainer(value);
                    if (itemToSelect != null)
                    {
                        itemToSelect.IsSelected = true;
                    }
                }

                state = ControlState.IgnoreChanges;
                if (SelectedItems.Count != 0)
                {
                    SelectedItem = SelectedItems[SelectedItems.Count - 1];
                    SelectedIndex = Items.IndexOf(SelectedItem);
                }
                state = ControlState.Ready;

                if (raiseEvent)
                    RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, UnselectedItems, SelectedItems));

            }
        }
        #endregion

        #region Multiple Selection Mode
        /// <summary>
        /// Вспомогательная функция обеспечивает выполнение режима выбора множества элементов в коллекции одновременно.
        /// </summary>
        /// <remarks>
        /// Helper function is used to maintain Multiple items Selection Mode.
        /// </remarks>
        internal void HandleMultipleSelection(object value, bool selectItem = false)
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
                    if (selectItem)
                    {
                        var itemToSelect = GetItemContainer(value);
                        if (itemToSelect != null)
                        {
                            itemToSelect.IsSelected = true;
                        }
                    }

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

                state = ControlState.IgnoreChanges;
                if (SelectedItems.Count != 0)
                {
                    SelectedItem = SelectedItems[SelectedItems.Count - 1];
                    SelectedIndex = Items.IndexOf(SelectedItem);
                }
                state = ControlState.Ready;
            }
        }

        #endregion

        #region MultipleWithDefault Selection Mode
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
        /// Вспомогательная функция задает состояние элементов и вызывает событие SelectionChanged.
        /// Эта функция обеспечивает выполнение режима выбора множества элементов в коллекции одновременно со сбросом на элемент по умолчанию, если ни один из элементов не выбран.
        /// </summary>
        /// <remarks>
        /// Helper function that sets state of the item and raised SelectionChanged Event.
        /// This function is used to maintain Multiple items Selection Mode with fallback to default.
        /// </remarks>
        internal void HandleMultipleSelectionWithDefault(object value, bool selectItem = false)
        {
            if (value != null)
            {
                if (selectItem)
                {
                    var itemToSelect = GetItemContainer(value);
                    if (itemToSelect != null)
                    {
                        itemToSelect.IsSelected = true;
                    }
                }

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

                state = ControlState.IgnoreChanges;
                if (SelectedItems.Count != 0)
                {
                    SelectedItem = SelectedItems[SelectedItems.Count - 1];
                    SelectedIndex = Items.IndexOf(SelectedItem);
                }
                state = ControlState.Ready;
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
        #endregion

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
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Bindable(true), Category("Appearance")]
        public bool SeparateItems
        {
            get => (bool)GetValue(SeparateItemsProperty);
            set => SetValue(SeparateItemsProperty, value);
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
            get => (SelectionModeEnum)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }
        /// <summary>
        /// Коллекция выбранных элементов.
        /// </summary>
        /// <remarks>
        ///  Collection of Selected Items.
        /// </remarks>
        [Bindable(true)]
        public ObservableCollection<object> SelectedItems
        {
            get => (ObservableCollection<object>)GetValue(SelectedItemsProperty);
            private set => SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Коллекция выбранных элементов.
        /// </summary>
        /// <remarks>
        ///  Collection of Selected Items.
        /// </remarks>
        [Bindable(true)]
        public ObservableCollection<object> UnselectedItems
        {
            get => (ObservableCollection<object>)GetValue(UnselectedItemsProperty);
            private set => SetValue(UnselectedItemsProperty, value);
        }

        /// <summary>
        /// Значение выбранного элемента
        /// </summary>
        /// <remarks>
        /// Value of selected item
        /// </remarks>
        [Bindable(true)]
        public new object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        /// <summary>
        /// Index выбранного элемента
        /// </summary>
        /// <remarks>
        /// Index of selected item
        /// </remarks>
        [Bindable(true)]
        public new int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIEnumSwitch), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty SeparateItemsProperty = DependencyProperty.Register("SeparateItems", typeof(bool), typeof(UIEnumSwitch), new UIPropertyMetadata(false, new PropertyChangedCallback(OnSeparateItemsChanged)));
        public static readonly DependencyProperty SourceEnumProperty = DependencyProperty.Register("SourceEnum", typeof(Type), typeof(UIEnumSwitch), new UIPropertyMetadata(null, new PropertyChangedCallback(OnSourceEnumChanged)));
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionModeEnum), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(SelectionModeEnum.Single, new PropertyChangedCallback(OnSelectionModeChanged)));
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty UnselectedItemsProperty = DependencyProperty.Register("UnselectedItems", typeof(ObservableCollection<object>), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(null));
        public static new readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemChanged)));
        public static new readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(UIEnumSwitch), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexChanged)));
        #endregion

        #region Property Callbacks
        /// <summary>
        /// Вызов функции обработки изменения режима выбора элементов.
        /// </summary>
        /// <remarks>
        /// Calling the function that handles SelectionMode changes.
        /// </remarks>
        private static void OnSelectionModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UIEnumSwitch enumSwitch = o as UIEnumSwitch;
            }
        }
        /// <summary>
        /// Вызов функции обработки изменения режима выбора элементов.
        /// </summary>
        /// <remarks>
        /// Calling the function that handles SelectionMode changes.
        /// </remarks>
        private static void OnSeparateItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UIEnumSwitch enumSwitch = o as UIEnumSwitch;
            }
        }
        /// <summary>
        /// Вызов функции заполнения элементами коллекции Items значениями из SourceEnum.
        /// </summary>
        /// <remarks>
        /// Calling the function filling the Items collection with the values from SourceEnum.
        /// </remarks>
        private static void OnSourceEnumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UIEnumSwitch enumSwitch = o as UIEnumSwitch;
                enumSwitch.CreateItemsFromSourceEnum();
            }
        }

        /// <summary>
        /// Функция заполнения элементами коллекции Items значениями из SourceEnum.
        /// </summary>
        /// <remarks>
        /// A function to fill the items in the Items collection with values from SourceEnum.
        /// </remarks>
        internal void CreateItemsFromSourceEnum()
        {
            if (SourceEnum != null)
            {
                if (ItemsSource is null)
                {
                    if (SourceEnum.IsEnum)
                    {
                        Items.Clear();
                        foreach (var node in Enum.GetValues(SourceEnum))
                        {
                            Items.Add(node);
                        }
                        state = ControlState.ItemsCreated;
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

        /// <summary>
        /// Выполнение выбора элемента в коллекции при установке значения SelectedItem из внешнего кода или из привязок.
        /// </summary>
        /// <remarks>
        /// Selects an item in the collection when the SelectedItem is set from external code or from bindings.
        /// </remarks>
        private static void OnSelectedItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UIEnumSwitch enumSwitch = o as UIEnumSwitch;
                if (enumSwitch.state == ControlState.IgnoreChanges)
                    return;
                else if (enumSwitch.state == ControlState.Ready)
                {
                    enumSwitch.Select(e.NewValue, true, true);
                }
            }
        }

        /// <summary>
        /// Выполнение выбора элемента в коллекции при установке значения SelectedIndex из внешнего кода или из привязок.
        /// </summary>
        /// <remarks>
        /// Selects an item in the collection when the SelectedIndex is set from external code or from bindings.
        /// </remarks>
        private static void OnSelectedIndexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UIEnumSwitch enumSwitch = o as UIEnumSwitch;
                if (enumSwitch.state == ControlState.IgnoreChanges)
                    return;
                else if (enumSwitch.state == ControlState.Ready)
                {
                    enumSwitch.Select(enumSwitch.GetItemAtIndex((int)e.NewValue), true, true);
                }
            }

        }
        #endregion
    }
}

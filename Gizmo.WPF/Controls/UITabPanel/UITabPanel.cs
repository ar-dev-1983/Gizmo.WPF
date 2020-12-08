using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Gizmo.WPF
{
    /// <summary>
    /// UITabPanel - это элемент управления для представления закладок.
    /// </summary>
    /// <remarks>
    /// UITabPanel is a control for presenting tabs.
    /// </remarks>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UITabPanelItem))]
    public class UITabPanel : Selector
    {
        #region Internal
        /// <summary>
        /// Перечисление для отслеживания текущего состояния UITabPanel
        /// </summary>
        /// <remarks>
        /// Enum for determine UITabPanel state
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
            /// Этот статус требуется для прерывания обработки OnSelectedIndexChanged и OnSelectedItemChanged при задании значений SelectedIndex или SelectedItem в логике выбора элементов внутри UITabPanel.
            /// </summary>
            /// <remarks>
            /// This status is required to interrupt the processing of OnSelectedIndexChanged and OnSelectedItemChanged when the SelectedIndex or SelectedItem values are set in the item selection logic inside the UITabPanel.
            /// </remarks>
            IgnoreChanges
        }
        #endregion

        #region Private Properties
        private ControlState state = ControlState.NotReady;

        /// <summary>
        /// Коллекция выбранных элементов.
        /// </summary>
        /// <remarks>
        ///  Collection of Selected Items.
        /// </remarks>
        private ObservableCollection<object> SelectedItems;

        /// <summary>
        /// Коллекция не выбранных элементов.
        /// </summary>
        /// <remarks>
        ///  Collection of Unselected Items.
        /// </remarks>
        private ObservableCollection<object> UnselectedItems;
        #endregion

        #region Commands
        /// <summary>
        /// Команда для изменения размера UITabPanel.
        /// </summary>
        ///<remarks>
        /// Command for UITabPanel resize action.
        ///</remarks>
        public static RoutedUICommand ResizeCommand
        {
            get { return resizeCommand; }
        }
        private static RoutedUICommand resizeCommand = new RoutedUICommand("Resize", "ResizeCommand", typeof(UITabPanel));
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UITabPanel()
        : base()
        {
            SelectedItems = new ObservableCollection<object>();
            UnselectedItems = new ObservableCollection<object>();
            DefaultStyleKey = typeof(UITabPanel);
            CommandBindings.Add(new CommandBinding(ResizeCommand, ResizeCommandExecuted));
        }
        static UITabPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UITabPanel), new FrameworkPropertyMetadata(typeof(UITabPanel)));
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Возвращает true если элемент является своим собственным контейнером
        /// </summary>
        /// <remarks>
        /// Return true if the item is its own ItemContainer
        /// </remarks>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UITabPanelItem;
        }

        /// <summary>
        /// Создает контейнер для элемента
        /// </summary>
        /// <remarks>
        /// Create or identify the element used to display the given item.
        /// </remarks>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UITabPanelItem();
        }

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
            if (!CanUnselect && SelectedIndex == -1 && SelectedItem == null)
            {
                if (Items.IndexOf(element) == 0)
                {
                    (element as UITabPanelItem).Selection(true);
                }
            }

            if (SelectedIndex != -1)
            {
                if (SelectedIndex == Items.IndexOf(element))
                {
                    (element as UITabPanelItem).Selection(true);
                }
            }
            else if (SelectedItem != null)
            {
                if (Equals(element, SelectedItem))
                {
                    (element as UITabPanelItem).Selection(true);
                }
            }
            SetItemAppearance(element as UITabPanelItem);
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
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                SetItemsAppearance(e.NewItems);
            state = ControlState.Ready;
        }
        #endregion

        #region Private Methods

        #region Resize

        /// <summary>
        /// Инициирует процесс изменения размера при нажатии и движении мыши.
        /// </summary>
        ///<remarks>
        /// Initiates the resizing when the mouse is pressed and move.
        ///</remarks>
        private void ResizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Control source = e.OriginalSource as Control;
            if (source != null)
            {
                source.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragMouseLeftButtonUp);
            }
            PreviewMouseMove += new MouseEventHandler(PreviewMouseMoveResize);
        }

        /// <summary>
        /// Если левая клавиша мыши нажата и указатель мыши перемещается - меняем размер.
        /// </summary>
        ///<remarks>
        /// If the left mouse button is pressed and the pointer moves, we change the size (in this case, the width).
        ///</remarks>
        private void PreviewMouseMoveResize(object sender, MouseEventArgs e)
        {
            Control source = e.OriginalSource as Control;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Resize(e);
            }
            else 
                PreviewMouseMove -= PreviewMouseMoveResize;
        }

        /// <summary>
        /// В зависимости от ориентации вкладок вычисляем новую ширину и присваиваем ее UITabPanel.
        /// </summary>
        ///<remarks>
        /// Depending on the orientation of the tabs we calculate the new width and assign it UITabPanel.
        ///</remarks>
        private void Resize(MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            double w = 0.0d;

            if (Orientation == UITabPanelItemOrientation.Left)
            {
                w = ActualWidth - (pos.X + 33.0d);
            }
            else if (Orientation == UITabPanelItemOrientation.Right)
            {
                w = pos.X + 33.0d;
            }

            if (w < MinResizeWidth)
            {
                w = MinResizeWidth;
            }
            if (MaxResizeWidth != double.NaN && w > MaxResizeWidth)
            {
                w = MaxResizeWidth;
            }
            Width = w;
        }

        private void DragMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Control source)
            {
                source.PreviewMouseLeftButtonUp -= DragMouseLeftButtonUp;
            }
            PreviewMouseMove -= PreviewMouseMoveResize;
        }
        #endregion

        /// <summary>
        /// Функция устанавливает внешний вид UITabPanelItem из заданного списка элементов.
        /// </summary>
        ///<remarks>
        /// The function sets the appearance of the UITabPanelItem from the given list of items.
        ///</remarks>
        private void SetItemsAppearance(IList items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    SetItemAppearance(item as UITabPanelItem);
                }
            }
        }

        /// <summary>
        /// Функция устанавливает внешний вид заданному UITabPanelItem.
        /// </summary>
        ///<remarks>
        /// The function sets the appearance of the given UITabPanelItem.
        ///</remarks>
        private void SetItemAppearance(UITabPanelItem item)
        {
            item.TabStyle = TabsStyle;

            if (TabsStyle == UITabPanelStyleEnum.CompactTabs)
            {
                BringItemToUp(item);
            }

            if (Orientation == UITabPanelItemOrientation.Left)
            {
                item.HorizontalAlignment = HorizontalAlignment.Right;
                item.VerticalAlignment = VerticalAlignment.Top;
                if (TabsStyle == UITabPanelStyleEnum.CompactTabs)
                    item.Margin = new Thickness(0, -22, 0, 0);
                else
                    item.Margin = new Thickness(0);
            }
            else if (Orientation == UITabPanelItemOrientation.Right)
            {
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.VerticalAlignment = VerticalAlignment.Top;
                if (TabsStyle == UITabPanelStyleEnum.CompactTabs)
                    item.Margin = new Thickness(0, -22, 0, 0);
                else
                    item.Margin = new Thickness(0);
            }
            else if (Orientation == UITabPanelItemOrientation.Top)
            {
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.VerticalAlignment = VerticalAlignment.Bottom;
                if (TabsStyle == UITabPanelStyleEnum.CompactTabs)
                    item.Margin = new Thickness(-22, 0, 0, 0);
                else
                    item.Margin = new Thickness(0);
            }
            else if (Orientation == UITabPanelItemOrientation.Bottom)
            {
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.VerticalAlignment = VerticalAlignment.Top;
                if (TabsStyle == UITabPanelStyleEnum.CompactTabs)
                    item.Margin = new Thickness(-22, 0, 0, 0);
                else
                    item.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// Вспомогательная функция, устанавливает ZIndex  таким образом, чтобы казалось будто вкладки расположены друг под другом,
        /// при этом первая вкладка всегда самая верхняя, а последняя самая нижняя. Если вкладка выделена, то ZIndex  устанавливается таким, чтобы визуально быть наверху всех вкладок.
        /// </summary>
        ///<remarks>
        /// A helper function that sets the ZIndex so that it appears as if the tabs are located one above the other, with the first tab always the top one and the last one the bottom one.
        /// If a tab is selected, then the ZIndex is set to visually be at the top of all tabs.
        ///</remarks>
        private void BringItemToUp(UITabPanelItem item)
        {
            if (item.IsSelected)
            {
                Panel.SetZIndex(item, 100);
            }
            else
            {
                var itemIndex = Items.IndexOf(item);
                Panel.SetZIndex(item, Items.Count - itemIndex + 1);
            }
        }

        /// <summary>
        /// Возвращает UITabPanelItem из коллекции по заданному индексу.
        /// </summary>
        ///<remarks>
        /// Returns the UITabPanelItem from the collection at the specified index.
        ///</remarks>
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
        #endregion

        #region Selection Logic
        /// <summary>
        /// Функция реализующая логику выбора вкладок.
        /// </summary>
        ///<remarks>
        /// A function that implements the selecting tabs logic.
        ///</remarks>
        internal void Select(UITabPanelItem item, bool value)
        {
            if (item != null)
            {
                SelectedItems.Clear();
                UnselectedItems.Clear();

                foreach (UITabPanelItem node in Items)
                {
                    if (Equals(node, item))
                    {
                        if (!item.IsSelected && value)
                        {
                            node.IsSelected = true;
                            SelectedItems.Add(item);
                        }
                        else if (item.IsSelected && !value)
                        {
                            node.IsSelected = false;
                            UnselectedItems.Add(node);
                        }
                    }
                    else
                    {
                        node.IsSelected = false;
                        UnselectedItems.Add(node);
                    }

                    if (TabsStyle == UITabPanelStyleEnum.CompactTabs)
                        BringItemToUp(node);
                }

                state = ControlState.IgnoreChanges;
                if (SelectedItems.Count != 0)
                {
                    SelectedItem = SelectedItems[SelectedItems.Count - 1];
                    SelectedIndex = Items.IndexOf(SelectedItem);
                    if (IsExpandable)
                    {
                        IsExpanded = true;
                    }
                }
                else
                {
                    SelectedItem = null;
                    SelectedIndex = -1;
                    if (IsExpandable)
                    {
                        IsExpanded = false;
                    }
                }
                state = ControlState.Ready;

                RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, UnselectedItems, SelectedItems));
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Значение выбранного элемента
        /// </summary>
        /// <remarks>
        /// Value of selected item
        /// </remarks>
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
        public new int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        /// <summary>
        /// Признак того, что область отображения контента вкладки была развернута.
        /// </summary>
        /// <remarks>
        /// Indicates that the tab content display area has been expanded.
        /// </remarks>
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        /// <summary>
        /// Признак того, что область отображения контента вкладки должна быть показана при выборе вкладки.
        /// </summary>
        /// <remarks>
        /// Indicates that the display area of the tab content should be shown when the tab is selected.
        /// </remarks>
        public bool IsExpandable
        {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        /// <summary>
        /// Указывает ориентацию вкладок в UITabPanel.
        /// </summary>
        /// <remarks>
        /// Specifies the orientation of the tabs in the UITabPanel.
        /// </remarks>
        public UITabPanelItemOrientation Orientation
        {
            get => (UITabPanelItemOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Указывает стиль отображения вкладок в UITabPanel.
        /// </summary>
        /// <remarks>
        /// Specifies the display style of tabs in the UITabPanel.
        /// </remarks>
        public UITabPanelStyleEnum TabsStyle
        {
            get => (UITabPanelStyleEnum)GetValue(TabsStyleProperty);
            set => SetValue(TabsStyleProperty, value);
        }

        /// <summary>
        /// Указывает возможность снять выделение с вкладки.
        /// </summary>
        /// <remarks>
        /// Indicates the ability to deselect a tab.
        /// </remarks>
        public bool CanUnselect
        {
            get => (bool)GetValue(CanUnselectProperty);
            set => SetValue(CanUnselectProperty, value);
        }

        /// <summary>
        /// Указывает возможность менять размер области отображения контента вкладки (только при Orientation равном Left или Right).
        /// </summary>
        /// <remarks>
        /// Indicates the ability to resize the display area of the tab content (only when Orientation is Left or Right).
        /// </remarks>
        public bool ResizeAllowed
        {
            get => (bool)GetValue(ResizeAllowedProperty);
            set => SetValue(ResizeAllowedProperty, value);
        }

        /// <summary>
        /// Минимальное значение ширины UITabPanel при ResizeAllowed равном true.
        /// </summary>
        /// <remarks>
        /// The minimum value for the UITabPanel width when ResizeAllowed is true.
        /// </remarks>
        public double MinResizeWidth
        {
            get => (double)GetValue(MinResizeWidthProperty);
            set => SetValue(MinResizeWidthProperty, value);
        }

        /// <summary>
        /// Максимальное значение ширины UITabPanel при ResizeAllowed равном true.
        /// Примечание: да, есть свойства MinWidth и MaxWidth, но для этих целей изменения размера они не совсем подошли, хотелось чтобы эти значения были выделены в отдельные свойства,
        /// не зависящие от MinWidth и MaxWidth.
        /// По крайней мере мне показалось это решение логичным, но возможно я в будущем уберу отдельные свойства и заменю их на значения из MinWidth и MaxWidth.
        /// </summary>
        /// <remarks>
        /// The maximum value for the UITabPanel width when ResizeAllowed is true.
        /// Note: yes, there are properties like MinWidth and MaxWidth, but for purposes of resizing they did not quite fit,
        /// I would like these values to be separated from MinWidth and MaxWidth properties.
        /// At least this solution seemed logical to me, but maybe in the future I will remove individual properties and replace them with values from MinWidth and MaxWidth.
        /// </remarks>
        public double MaxResizeWidth
        {
            get => (double)GetValue(MaxResizeWidthProperty);
            set => SetValue(MaxResizeWidthProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(UITabPanel), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsExpandableProperty = DependencyProperty.Register("IsExpandable", typeof(bool), typeof(UITabPanel), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(UITabPanelItemOrientation), typeof(UITabPanel), new FrameworkPropertyMetadata(UITabPanelItemOrientation.Left, new PropertyChangedCallback(StylePropertyChanged)));
        public static new readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(UITabPanel), new FrameworkPropertyMetadata(null));
        public static new readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(UITabPanel), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexChanged)));
        public static readonly DependencyProperty TabsStyleProperty = DependencyProperty.Register("TabsStyle", typeof(UITabPanelStyleEnum), typeof(UITabPanel), new FrameworkPropertyMetadata(UITabPanelStyleEnum.Tabs, new PropertyChangedCallback(StylePropertyChanged)));
        public static readonly DependencyProperty CanUnselectProperty = DependencyProperty.Register("CanUnselect", typeof(bool), typeof(UITabPanel), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty ResizeAllowedProperty = DependencyProperty.Register("ResizeAllowed", typeof(bool), typeof(UITabPanel), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty MinResizeWidthProperty = DependencyProperty.Register("MinResizeWidth", typeof(double), typeof(UITabPanel), new FrameworkPropertyMetadata(200.0d));
        public static readonly DependencyProperty MaxResizeWidthProperty = DependencyProperty.Register("MaxResizeWidth", typeof(double), typeof(UITabPanel), new FrameworkPropertyMetadata(400.0d));
        #endregion

        #region Property Callbacks
        /// <summary>
        /// Выполнение выбора элемента в коллекции при установке значения SelectedItem из внешнего кода или из привязок.
        /// </summary>
        /// <remarks>
        /// Selects an item in the collection when the SelectedItem is set from external code or from bindings.
        /// </remarks>
        private static void StylePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                UITabPanel tabPanel = o as UITabPanel;
                tabPanel.UpdateItems();
            }
        }

        private void UpdateItems()
        {
            SetItemsAppearance(Items);
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
                UITabPanel tabPanel = o as UITabPanel;
                if (tabPanel.state == ControlState.IgnoreChanges)
                    return;
                else if (tabPanel.state == ControlState.Ready)
                {
                    tabPanel.Select(tabPanel.GetItemAtIndex((int)e.NewValue) as UITabPanelItem, true);
                }
            }
        }
        #endregion
    }
}

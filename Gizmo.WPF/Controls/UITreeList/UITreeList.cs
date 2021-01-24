using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    /// <summary>
    /// UITreeList - это элемент управления для представления стандартного TreeView со столбцами.
    /// </summary>
    /// <remarks>
    /// UITreeList is a control for presenting standard TreeView with Columns.
    /// </remarks>
    public class UITreeList : TreeView
    {
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UITreeList()
  : base()
        {
            Columns = new GridViewColumnCollection();
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Создает контейнер для элемента
        /// </summary>
        /// <remarks>
        /// Create or identify the element used to display the given item.
        /// </remarks>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UITreeListItem();
        }

        /// <summary>
        /// Возвращает true если элемент является своим собственным контейнером
        /// </summary>
        /// <remarks>
        /// Return true if the item is its own ItemContainer
        /// </remarks>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UITreeListItem;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Коллекция столбцов
        /// </summary>
        /// <remarks>
        /// Columns for grid view
        /// </remarks>
        public GridViewColumnCollection Columns
        {
            get => (GridViewColumnCollection)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(UITreeList), new FrameworkPropertyMetadata(new GridViewColumnCollection()));
        #endregion
    }
}

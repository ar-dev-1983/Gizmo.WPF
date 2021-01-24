using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    /// <summary>
    /// UITreeListItem - это элемент управления для представления стандартного TreeViewItem для отображения в UITreeList.
    /// </summary>
    /// <remarks>
    /// UITreeListItem is a control for presenting a standard TreeViewItem for display in a UITreeList.
    /// </remarks>
    public class UITreeListItem : TreeViewItem
    {
        #region Private Properties
        private int _level = -1;
        #endregion

        #region Public Properties

        /// <summary>
        /// Уровень вложенности элемента
        /// </summary>
        /// <remarks>
        /// Item nesting level
        /// </remarks>
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    _level = (ItemsControlFromItemContainer(this) is UITreeListItem parent) ? parent.Level + 1 : 0;
                }
                return _level;
            }
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
    }
}

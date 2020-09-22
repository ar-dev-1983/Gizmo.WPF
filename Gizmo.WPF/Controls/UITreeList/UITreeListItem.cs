using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UITreeListItem : TreeViewItem
    {
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

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UITreeListItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UITreeListItem;
        }

        private int _level = -1;
    }

}

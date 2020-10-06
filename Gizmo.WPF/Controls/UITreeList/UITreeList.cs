using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{

    public class UITreeList : TreeView
    {
        public UITreeList()
  : base()
        {
            Columns = new GridViewColumnCollection();
            DefaultStyleKey = typeof(UITreeList);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UITreeListItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UITreeListItem;
        }

        public GridViewColumnCollection Columns
        {
            get => (GridViewColumnCollection)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(UITreeList), new FrameworkPropertyMetadata(new GridViewColumnCollection()));
    }

}

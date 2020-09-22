using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UIControlGroup : ItemsControl
    {
        static UIControlGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIControlGroup), new FrameworkPropertyMetadata(typeof(UIControlGroup)));
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            CornerRadius r = CornerRadius;
            foreach (var node in Items)
            {
                if (node is ICorneredControl)
                {
                    if (Items.Count == 1)
                    {
                        (node as ICorneredControl).CornerRadius = CornerRadius;
                        (node as ICorneredControl).BorderThickness = new Thickness(1);
                    }
                    else if (Items.IndexOf(node) == 0)
                    {
                        (node as ICorneredControl).CornerRadius = new CornerRadius(r.TopLeft, 0d, 0d, r.BottomLeft);
                        (node as ICorneredControl).BorderThickness = new Thickness(1, 1, 0, 1);
                    }
                    else if (Items.IndexOf(node) == Items.Count - 1)
                    {
                        (node as ICorneredControl).CornerRadius = new CornerRadius(0d, r.TopRight, r.BottomRight, 0d);
                        (node as ICorneredControl).BorderThickness = new Thickness(0, 1, 1, 1);
                    }
                    else
                    {
                        (node as ICorneredControl).CornerRadius = new CornerRadius(0);
                        (node as ICorneredControl).BorderThickness = new Thickness(0, 1, 0, 1);
                    }
                }
            }
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIControlGroup), new UIPropertyMetadata(new CornerRadius(3)));
    }
}

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gizmo.WPF
{
    /// <summary>
    /// Static class provides methods to search for the visual tree of elements 
    /// </summary>
    public static class VisualHelper
    {
        /// <summary>
        /// Static method is used to search for an child in the visual tree of Parent UIElement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                var result = (child as T) ?? FindChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        /// <summary>
        /// Static method is used to search for an Parent UIElement in the visual tree of child.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            return parentObject == null ? (T)null : parentObject is T parent ? parent : FindParent<T>(parentObject);
        }


        /// <summary>
        /// Static method is used to search for an Parent UIElement in the visual tree of child. Another variant commonly used then child is placed in Popup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindVisulaParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            return parentObject switch
            {
                //is parentObject is null, then we have reached the root element of the popup, which means we need to start a new search cycle already using the Parent property of Pupup;
                //if the Parent property is null, then alas - we return null.
                null => child is FrameworkElement ? (child as FrameworkElement).Parent != null ? FindVisulaParent<T>((child as FrameworkElement).Parent) : null : null,
                //we found an Parent of the desired type - we return it.
                T _ => parentObject as T,
                //we have not yet reached the Parent of the desired type, so we continue to search.
                _ => FindVisulaParent<T>(parentObject)
            };
        }
        
        /// <summary>
        /// Static method used for determine is one UIElement is ancestor of another UIElement
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsLogicalAncestorOf(this UIElement ancestor, UIElement child)
        {
            if (child != null)
            {
                FrameworkElement obj = child as FrameworkElement;
                while (obj != null)
                {
                    FrameworkElement parent = VisualTreeHelper.GetParent(obj) as FrameworkElement;
                    obj = parent == null ? obj.Parent as FrameworkElement : parent as FrameworkElement;
                    if (obj == ancestor) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Static method used for take picture of given UIElement
        /// </summary>
        /// <param name="visualSource">Source UIElement: window, tab, stackpanel or grid, etc...</param>
        /// <returns>PngBitmapEncoder to save actual picture</returns>
        public static PngBitmapEncoder SnapShotPNG(this UIElement visualSource)
        {
            var clip = visualSource.ClipToBounds;
            visualSource.ClipToBounds = true;
            visualSource.Measure(new Size((int)visualSource.RenderSize.Width, (int)visualSource.RenderSize.Height));
            visualSource.Arrange(new Rect(new Size((int)visualSource.RenderSize.Width, (int)visualSource.RenderSize.Height))); 

            double actualHeight = visualSource.RenderSize.Height;
            double actualWidth = visualSource.RenderSize.Width;
            //TODO: replace with system values or add input values of dpi
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)actualWidth, (int)actualHeight, 96, 96, PixelFormats.Pbgra32);
            VisualBrush sourceBrush = new VisualBrush(visualSource);
        
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }
            renderTarget.Render(drawingVisual);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));
            visualSource.ClipToBounds = clip;

            return encoder;
        }
    }
}

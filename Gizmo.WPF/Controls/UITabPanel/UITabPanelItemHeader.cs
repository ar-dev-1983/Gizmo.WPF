using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Gizmo.WPF
{
    public class UITabPanelItemHeader : Shape
    {
        #region Constructors
        static UITabPanelItemHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UITabPanelItemHeader), new FrameworkPropertyMetadata(typeof(UITabPanelItemHeader)));
        }
        public UITabPanelItemOrientation Orientation
        {
            get => (UITabPanelItemOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion

        #region Override Methods
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), DefiningGeometry);
        }
        #endregion

        #region Override Properties
        protected override Geometry DefiningGeometry
        {
            get
            {
                Geometry geometry = null;
                if (Orientation == UITabPanelItemOrientation.Top)
                {
                    geometry = Geometry.Parse("m0 19.5h5c3 0 4.15-2.0 5-4.75l3-9.75c0.75-2.75 2-4.5 5-4.5h" + (ContentSize + 15).ToString() + "c3 0 4.15 1.7766 5 4.5l3 9.75c0.75 2.75 2 4.75 5 4.75h5");
                }
                else if (Orientation == UITabPanelItemOrientation.Bottom)
                {
                    geometry = Geometry.Parse("m0 0.5h5c3 0 4.15 2 5 4.75l3 9.75c0.75 2.75 2 4.5 5 4.5h" + (ContentSize + 15).ToString() + "c3 0 4.15-1.75 5-4.5l3-9.75c0.75-2.75 2-4.75 5-4.75h5");
                }
                else if (Orientation == UITabPanelItemOrientation.Left)
                {
                    geometry = Geometry.Parse("m19.5 0v5c0 3-2 4.15-4.75 5l-9.75 3c-2.75 0.75-4.5 2-4.5 5v" + (ContentSize + 15).ToString() + "c0 3 1.75 4.15 4.5 5l9.75 3c2.75 0.75 4.75 2 4.75 5v5");
                }
                else
                {
                    geometry = Geometry.Parse("m0.5 0v5c0 3 2 4.1618 4.75 5l9.75 3c2.75 0.75 4.5 2 4.5 5v" + (ContentSize + 15).ToString() + "c0 3-1.75 4.1618-4.5 5l-9.75 3c-2.75 0.75-4.75 2-4.75 5v5");
                }
                return geometry;
            }
        } 
        #endregion

        #region Public Methods
        private void UpdateRender()
        {
            InvalidateArrange();
            InvalidateMeasure();
            InvalidateVisual();
        } 
        #endregion

        #region Private Methods
        private void ProcessHeader()
        {
            Effect = new DropShadowEffect()
            {

                Color = ShadowColor,
                BlurRadius = 5,
                Opacity = 0.5,
                ShadowDepth = 2,
                Direction = Orientation == UITabPanelItemOrientation.Left ? 180 : Orientation == UITabPanelItemOrientation.Right ? 0 : Orientation == UITabPanelItemOrientation.Top ? 90 : Orientation == UITabPanelItemOrientation.Bottom ? 270 : 0
            };
        } 
        #endregion

        #region Public Properties
        public double ContentSize
        {
            get => (double)GetValue(ContentSizeProperty);
            set => SetValue(ContentSizeProperty, value);
        }
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(UITabPanelItemOrientation), typeof(UITabPanelItemHeader), new FrameworkPropertyMetadata(UITabPanelItemOrientation.Left, new PropertyChangedCallback(OrientationChanged)));
        public static readonly DependencyProperty ContentSizeProperty = DependencyProperty.Register("ContentSize", typeof(double), typeof(UITabPanelItemHeader), new FrameworkPropertyMetadata(0.0d, new PropertyChangedCallback(ContentSizeChanged)));
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register("ShadowColor", typeof(Color), typeof(UITabPanelItemHeader), new FrameworkPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(ShadowColorChanged)));
        #endregion

        #region Property Callbacks
        private static void OrientationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanelItemHeader header = (UITabPanelItemHeader)o;
            header.ProcessHeader();
            header.UpdateRender();
        }
        private static void ContentSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanelItemHeader header = (UITabPanelItemHeader)o;
            header.UpdateRender();
        }
        private static void ShadowColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UITabPanelItemHeader header = (UITabPanelItemHeader)o;
            header.ProcessHeader();
            header.UpdateRender();
        }
        #endregion
    }
}

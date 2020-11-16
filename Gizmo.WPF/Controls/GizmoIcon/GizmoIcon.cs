using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.WPF
{
    ///
    /// this part of the code contains the code from the project FontAwesome5
    /// https://github.com/MartinTopfstedt/FontAwesome5
    ///

    //using in xaml:
    //1. include xmlns namespace with enum alongside with Gizmo.WPF namespace, like this
    //
    //      xmlns:ui="clr-namespace:Gizmo.WPF;assembly=Gizmo.WPF"
    //      xmlns:enums="clr-namespace:Gizmo.HardwareAudit.Enums"
    //
    //2. then place GizmoIcon, and set Icon Property like this
    //
    //      <ui:GizmoIcon FontSize="16" Icon="{x:Static enums:ExampleEnum.Icon}" />
    //
    //3. if you need to use another IconFont, like FontAwesome, you can specify IconFontFamily to GizmoIcon, and use apropriate Enum to chose icon
    //
    //      <ui:GizmoIcon FontSize="16" Icon="{x:Static fa:FontAwesomeIcon.Star}" IconFontFamily="{StaticResource FontAwesome}"/>

    public class GizmoIcon : TextBlock
    {
        private static readonly FontFamily GizmoIconFontFamily = new FontFamily(new Uri("pack://application:,,,/Gizmo.WPF;component/Resources/Fonts/Gizmo.IconFont.ttf"), "./#Gizmo.IconFont");

        public Enum Icon
        {
            get { return (Enum)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Enum), typeof(GizmoIcon), new PropertyMetadata(GizmoIconEnum.None, OnIconPropertyChanged));
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(GizmoIcon), new PropertyMetadata(GizmoIconFontFamily, OnIconFontFamilyPropertyChanged));

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(FontFamilyProperty, !(d is GizmoIcon) ? GizmoIconFontFamily : (d as GizmoIcon).IconFontFamily);
            d.SetValue(TextAlignmentProperty, TextAlignment.Center);
            if (e.NewValue != null)
                d.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }
        private static void OnIconFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(FontFamilyProperty, !(d is GizmoIcon) ? GizmoIconFontFamily : (d as GizmoIcon).IconFontFamily);
        }
    }

    public class GizmoIconImage : Image
    {
        private static readonly FontFamily GizmoIconFontFamily = new FontFamily(new Uri("pack://application:,,,/Gizmo.WPF;component/Resources/Fonts/Gizmo.IconFont.ttf"), "./#Gizmo.IconFont");

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public Enum Icon
        {
            get { return (Enum)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(GizmoIconImage), new PropertyMetadata(Brushes.Black, OnIconPropertyChanged));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Enum), typeof(GizmoIconImage), new PropertyMetadata(GizmoIconEnum.None, OnIconPropertyChanged));
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(GizmoIconImage), new PropertyMetadata(GizmoIconFontFamily, OnIconPropertyChanged));

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is GizmoIconImage gizmoIconImage)) return;
            gizmoIconImage.SetValue(SourceProperty, UpdateImageSource(gizmoIconImage.Icon, gizmoIconImage.Foreground, gizmoIconImage.FontFamily));
        }

        public static ImageSource UpdateImageSource(object icon, Brush foregroundBrush, FontFamily fontFamily, double emSize = 100)
        {
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var typeFace = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal);
#if NET45
                drawingContext.DrawText(new FormattedText(char.ConvertFromUtf32((int)icon), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace, emSize, foregroundBrush) { TextAlignment = TextAlignment.Center }, new Point(0, 0));
#else
                drawingContext.DrawText(new FormattedText(char.ConvertFromUtf32((int)icon), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace, emSize, foregroundBrush, 1) { TextAlignment = TextAlignment.Center }, new Point(0, 0));
#endif
            }
            return new DrawingImage(visual.Drawing);
        }
    }
}

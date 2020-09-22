using System;
using System.Windows;
using System.Windows.Data;


namespace Gizmo.WPF
{
    public class RoundedCornerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CornerRadius cr = (CornerRadius)value;
            string s = parameter as string;
            return s switch
            {
                "left" => new CornerRadius(cr.TopLeft, 0d, 0d, cr.BottomLeft),
                "left_top" => new CornerRadius(cr.TopLeft, 0d, 0d, 0d),
                "left_bottom" => new CornerRadius(0d, 0d, 0d, cr.BottomLeft),
                "right_top" => new CornerRadius(0d, cr.TopRight, 0d, 0d),
                "right_bottom" => new CornerRadius(0d, 0d, cr.BottomRight, 0d),
                "right" => new CornerRadius(0d, cr.TopRight, cr.BottomRight, 0d),
                "top" => new CornerRadius(cr.TopLeft, cr.TopRight, 0d, 0d),
                "bottom" => new CornerRadius(0d, 0d, cr.BottomRight, cr.BottomLeft),
                _ => null,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

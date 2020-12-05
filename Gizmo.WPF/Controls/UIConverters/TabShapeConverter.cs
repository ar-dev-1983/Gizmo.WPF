using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Gizmo.WPF
{
    public class TabShapeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                var contentSize = 20d;
                if (value != null)
                {
                    contentSize += (double)value;
                }
                return parameter switch
                {
                    "Top" => Geometry.Parse("m0 20s1 0 3-2c3-3 1-18 7-20h" + contentSize.ToString("0.0").Replace(",", ".") + "c6 0 4 15 7 20 2 2 3 2 3 2"),
                    "Bottom" => Geometry.Parse("m0 -0.6s1 0 3 2c3 3 1 18 7 20h" + contentSize.ToString("0.0").Replace(",", ".") + "c6 0 4-15 7-20 2-2 3-2 3-2"),
                    "Left" => Geometry.Parse("m20 0s0 1-2 3c-3 3-18 1-20 7l0 " + contentSize.ToString("0.0").Replace(",", ".") + "c0 6 15 4 20 7 2 2 2 3 2 3"),
                    "Right" => Geometry.Parse("m-0.6 0s0 1 2 3c3 3 18 1 20 7v" + contentSize.ToString("0.0").Replace(",", ".") + "c0 6-15 4-20 7-2 2-2 3-2 3"),
                    _ => Geometry.Parse("m0,0")
                };
            }
            else
            {
                return Geometry.Parse("m0,0");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

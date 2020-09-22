using System;
using System.Globalization;
using System.Windows.Data;

namespace Gizmo.WPF
{
    public class HeightDivideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Height = (double)value;

            return Height / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

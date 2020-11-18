using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.WPF
{
    internal class BoolToFontWeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? FontWeights.Black : FontWeights.Normal;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

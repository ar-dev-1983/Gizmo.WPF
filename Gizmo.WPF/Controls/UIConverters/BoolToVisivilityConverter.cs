using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.WPF
{
    internal class BoolToVisivilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            value != null ? (bool)value == true ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.WPF
{
    internal class IsExpandableConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture) => (bool)o ? Visibility.Visible : (object)Visibility.Hidden;
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}

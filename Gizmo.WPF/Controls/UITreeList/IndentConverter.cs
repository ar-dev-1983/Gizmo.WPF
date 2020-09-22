using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.WPF
{
    public class IndentConverter : IValueConverter
    {
        private const double IndentValue = 19.0;

        public object Convert(object o, Type type, object parameter, CultureInfo culture) => new Thickness((int)o * IndentValue, 0, 0, 0);

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}

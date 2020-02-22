using System;
using System.Globalization;
using System.Windows.Data;

namespace BookOrganizer.UI.WPFCore.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToMaxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((bool)value == true) ? double.MaxValue : 0d;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookOrganizer.UI.WPFCore.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToMaxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((bool)value) ? new GridLength(1, GridUnitType.Star) : new GridLength(0, GridUnitType.Star);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}

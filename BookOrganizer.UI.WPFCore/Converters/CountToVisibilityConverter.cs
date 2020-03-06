using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookOrganizer.UI.WPFCore.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value <= 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
